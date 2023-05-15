using NationalInstruments.Analysis;
using NationalInstruments.Analysis.Conversion;
using NationalInstruments.Analysis.Dsp;
using NationalInstruments.Analysis.Dsp.Filters;
using NationalInstruments.Analysis.Math;
using NationalInstruments.Analysis.Monitoring;
using NationalInstruments.Analysis.SignalGeneration;
using NationalInstruments.Analysis.SpectralMeasurements;
using NationalInstruments;
using NationalInstruments.UI;
using NationalInstruments.NetworkVariable;
using NationalInstruments.NetworkVariable.WindowsForms;
using NationalInstruments.Tdms;
using NationalInstruments.UI.WindowsForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using IniHelper;
using EasyLog;
using TwinCAT.Ads;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TypeConvertHelper;
using AutoStartHelper;
using MicroLibrary;
using Sunny.UI;
using DeviceManeger;

namespace MesOpcServer
{
    public partial class MESServer : UIForm
    {

        string _MESCorePath = @"D:\DRLaser\MES\MESCore\MESCore.exe";
        string _MESCoreWorkingDirectory = @"D:\DRLaser\MES\MESCore";
        Log _log;
        public static Configure _cfg = new Configure("D:\\DRLaser\\MES\\MES.ini");
        public static TcAdsClient tcClient = new TcAdsClient();
        int FormHMIReadhvar3 = new int();
        bool m_FirstStartupFlag = true, m_FirstTimeRun = true;
        FormProductionStatistics formProductionStatistics;
        FormSettings FormSet;
        OpcUaReadWrite OpcReadWrite;
        AutoStart autoStart = new AutoStart();
        int m_CurrentDay;
        public static MESServer mesServer;
        public static string SoftVersion = "[V2.6.2]";

        public MESServer()
        {
            _log = Log.getInstance();
            if (checkSelfExeExist("MesOpcServer"))
            {
                WriteLog("尝试打开MesOpcServer程序......");
                MessageBox.Show("MES OPC Server已经开启，请不要重复打开！", "温馨提示");
                WriteLog("MES OPC Server程序打开失败！");
                Environment.Exit(0);

            }
            //打开OPC UA Server
            ExeOpen();


            InitializeComponent();

        //    OpcReadWrite = OpcUaReadWrite.getInstance();

            formProductionStatistics = FormProductionStatistics.getInstance();

            FormSygoleRFID.formSygoleRFID = new FormSygoleRFID();
            FormSygoleRFID.formSygoleRFID.Show();
            FormSygoleRFID.formSygoleRFID.Hide();

            FormAGV.formAGV = new FormAGV();
            FormAGV.formAGV.Show();
            FormAGV.formAGV.Hide();

            //设置程序开机自启动，并创建桌面快捷方式
            autoStart.SetMeAutoStart(true);

            m_CurrentDay = DateTime.Now.Day;

            DeleteExpiredFiles("D:\\MESLog\\MES\\", "*.log", 14);
            DeleteExpiredFiles("D:\\MESLog\\AGV\\", "*.log", 14);
            DeleteExpiredFiles("D:\\MESLog\\RFID\\", "*.log", 14);
            DeleteExpiredFiles("D:\\DRLaser\\ProductionData\\", "*.ini", 14);

            FormSet = FormSettings.getInstance();
            FormSet.Show();
            FormSet.Hide();
            mesServer = this;
        }

        private void MESServer_Load(object sender, EventArgs e)
        {
            //连接PLC
            InitADS();

            WriteLog("第一次打开程序！");
        }

        private void InitADS()
        {
            Task.Run(() =>
               {
                   try
                   {
                       //初始化ADS连接
                       tcClient.Connect("172.255.255.255.1.1", 801);
                       tcClient.Timeout = 300;

                       //通信检测
                       FormHMIReadhvar3 = tcClient.CreateVariableHandle(".Cyl_Ctrl_HMI[1]");
                       WriteLog("Init ADS successfully!");
                   }
                   catch
                   {
                       WriteLog("Init ADS failed!");
                   }
               });
        }

        /*********************************************************************************************************
** 函数名称 ：fCheckConnect()
** 函数功能 ：通信检测
** 入口参数 ：
** 出口参数 ： 
*********************************************************************************************************/
        int m_CheckCommunicationCounter;
        void fCheckConnect()
        {
            Task.Run(() =>
              {
                  if (m_CheckCommunicationCounter < 20)
                  {
                      m_CheckCommunicationCounter++;
                  }
                  else
                  {
                      m_CheckCommunicationCounter = 0;
                      using (AdsStream datastream = new AdsStream(2))
                      {
                          using (BinaryReader binread = new BinaryReader(datastream))
                          {
                              datastream.Position = 0;
                              try
                              {
                                  tcClient.Read(FormHMIReadhvar3, datastream);
                              }
                              catch
                              {

                                  LED_PLC.Value = false;

                                  InitADS();

                                  WriteLog("请连接PLC！");

                                  return;
                              }
                          }
                      }
                      if (tcClient.ReadState().AdsState != AdsState.Run)
                      {
                          LED_PLC.Value = false;
                          InitADS();

                          WriteLog("PLC连接不成功，请连接PLC！");
                          return;
                      }

                      LED_PLC.Value = true;



                  }

              });
        }

        private void MESServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        // 重写OnClosing使点击关闭按键时窗体能够缩进托盘

        protected override void OnClosing(CancelEventArgs e)
        {

            this.ShowInTaskbar = false;

            this.WindowState = FormWindowState.Minimized;

            e.Cancel = true;

        }

        private void MESServer_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.notifyIcon1.Visible = true;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                FormSygoleRFID.formSygoleRFID.DisConnect();
                tcClient.Dispose();

            }
            catch
            {
            }

            Environment.Exit(0);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.notifyIcon1.Visible = false;
        }

        //检测自身程序是否存在
        public bool checkSelfExeExist(string strExe)
        {
            uint count = 0;
            Process[] vProcesses = Process.GetProcesses();

            foreach (Process vProcess in vProcesses)
            {
                if (vProcess.ProcessName == strExe)
                {
                    count++;

                }
            }

            if (count >= 2)
            {
                return true;
            }
            return false;
        }

        //检测其他程序是否存在
        public bool checkOtherExeExist(string strExe)
        {
            uint count = 0;
            Process[] vProcesses = Process.GetProcesses();

            foreach (Process vProcess in vProcesses)
            {
                if (vProcess.ProcessName == strExe)
                {
                    count++;

                }
            }

            if (count >= 1)
            {
                return true;
            }
            return false;
        }


        //函数：打开OPC UA Server
        void ExeOpen()
        {

            if (!checkOtherExeExist("MESCore"))
            {
                WriteLog("程序后台打开MESCore!");
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    Process process = new Process();
                    startInfo.FileName = _MESCorePath;
                    startInfo.UseShellExecute = false;
                    startInfo.CreateNoWindow = true;
                    process.StartInfo = startInfo;
                    process.Start();
                    WriteLog("程序后台打开MESCore成功!");
                }
                catch
                {
                    WriteLog("程序后台打开MESCore失败!");
                }
            }

            //if (!checkOtherExeExist("Startup"))
            //{
            //    WriteLog("程序后台打开Startup!");
            //    try
            //    {

            //        ProcessStartInfo startInfo = new ProcessStartInfo();
            //        Process process = new Process();
            //        startInfo.FileName = "D:\\DRLaser\\MES\\Startup.exe";
            //        startInfo.UseShellExecute = false;
            //        startInfo.CreateNoWindow = true;
            //        process.StartInfo = startInfo;
            //        process.Start();
            //        WriteLog("程序后台打开Startup成功!");
            //    }
            //    catch
            //    {
            //        WriteLog("程序后台打开Startup失败!");
            //    }

            //}

        }


        uint m_timerCount = 0;
        private void timerServerOpen_Tick(object sender, EventArgs e)
        {
            if (m_timerCount < 500)
                m_timerCount++;
            else
            {
                m_timerCount = 0;
                //如果未启动则，则重新打开OPC UA Server
                ExeOpen();

            }

            LogHandler();



        }

        private void menuProduction_Click(object sender, EventArgs e)
        {
            formProductionStatistics.Show();
        }

        Queue<string> LogQueue = new Queue<string>();

        public void WriteLog(string msg)
        {
            if (msg.Length > 0)
            {
                LogQueue.Enqueue(MESServer.SoftVersion + msg);
            }


        }

        void LogHandler()
        {

            try
            {
                if (LogQueue.Count > 0)
                {
                    if (LogBox.IsHandleCreated)
                    {
                        string msg = LogQueue.Dequeue();

                        string str_msg = DateTime.Now.ToString("HH:mm:ss  ") + msg + "\n";

                        _log.Write(msg, MsgType.MES);

                        LogBox.Focus();
                        LogBox.Select(LogBox.TextLength, 0);
                        LogBox.ScrollToCaret();

                        LogBox.AppendText(str_msg);

                        if (LogBox.Lines.Length > 5000)
                        {
                            LogBox.Clear();
                        }
                    }
                }


            }
            catch
            {
            }

        }

        private void btnReConnect_Click(object sender, EventArgs e)
        {
            Device.GetDevice().agvLinker.Write(Device.GetDevice().SystemSettings);
            InitADS();
        }


        public static ushort g_InPieceNum, g_OutKKInPiece, iState_ENTERRFIDCHECK_RFID, iState_ExitRFIDCHECK_HMI, g_UnLoadCarrierArriveAtRFIDPortExit;
        public static ushort g_InPieceNum_Old, g_OutKKInPiece_Old, iState_ENTERRFIDCHECK_RFID_Old, iState_ExitRFIDCHECK_HMI_Old;
        public static bool g_WaferIDTransfer;

        void fMesEventSignalInit()
        {
            if (LED_PLC.Value)
            {
                if (m_FirstTimeRun)
                {
                    g_InPieceNum = ReadFromPlc(".g_InPieceNum", 0);
                    g_OutKKInPiece = ReadFromPlc(".g_OutKKInPiece", 0);
                    iState_ENTERRFIDCHECK_RFID = ReadFromPlc(".iState_ENTERRFIDCHECK_RFID", 0);
                    iState_ExitRFIDCHECK_HMI = ReadFromPlc(".iState_ExitRFIDCHECK_HMI", 0);


                    g_InPieceNum_Old = g_InPieceNum;
                    g_OutKKInPiece_Old = g_OutKKInPiece;
                    iState_ENTERRFIDCHECK_RFID_Old = iState_ENTERRFIDCHECK_RFID;
                    iState_ExitRFIDCHECK_HMI_Old = iState_ExitRFIDCHECK_HMI;

                    m_FirstTimeRun = false;
                }
            }

        }

        void fMesEventSignalRefresh()
        {
            if (LED_PLC.Value)
            {
                g_InPieceNum = ReadFromPlc(".g_InPieceNum", 0);
                g_OutKKInPiece = ReadFromPlc(".g_OutKKInPiece", 0);
                iState_ENTERRFIDCHECK_RFID = ReadFromPlc(".iState_ENTERRFIDCHECK_RFID", 0);
                iState_ExitRFIDCHECK_HMI = ReadFromPlc(".iState_ExitRFIDCHECK_HMI", 0);

                g_UnLoadCarrierArriveAtRFIDPortExit = ReadFromPlc(".g_UnLoadCarrierArriveAtRFIDPortExit", 0);

            }

        }



        /*********************************************************************************************************
         ** 函数名称 ：fWriteToPlc
         ** 函数功能 ：读PLC的整型变量
         ** 入口参数 ：
         ** 出口参数 ： 
         *********************************************************************************************************/
        public static AdsStream datastream_UINT = new AdsStream(2); //每个元素占用2字节
        public static AdsBinaryReader binRead_UINT = new AdsBinaryReader(datastream_UINT);
        int variableHandle_UINT;

        public UInt16 ReadFromPlc(string Plc_Variable, UInt16 FailDefault)
        {
            UInt16 reVarValue;

            try
            {
                variableHandle_UINT = tcClient.CreateVariableHandle(Plc_Variable);
                datastream_UINT.Position = 0;
                tcClient.Read(variableHandle_UINT, datastream_UINT);
                reVarValue = binRead_UINT.ReadUInt16();

            }
            catch
            {
                reVarValue = FailDefault;
            }
            return reVarValue;
        }


        public static AdsStream datastream_bool = new AdsStream(1); //每个元素占用1字节
        public static AdsBinaryReader binRead_bool = new AdsBinaryReader(datastream_bool);
        int variableHandle_bool;

        public bool ReadFromPlc(string Plc_Variable, bool FailDefault)
        {
            bool reVarValue;

            try
            {
                variableHandle_bool = tcClient.CreateVariableHandle(Plc_Variable);
                datastream_bool.Position = 0;
                tcClient.Read(variableHandle_bool, datastream_bool);
                reVarValue = binRead_bool.ReadBoolean();

            }
            catch
            {
                reVarValue = FailDefault;
            }
            return reVarValue;
        }





        public bool fWriteToPlc_String(string Plc_Variable, string Value)
        {
            try
            {
                tcClient.WriteSymbol(Plc_Variable, Value, false);
                return true;
            }
            catch (Exception ex)
            {

                WriteLog(Plc_Variable + "   " + ex.Message);
                return false;
            }
        }

        public string fReadFromPlc_String(string Plc_Variable, string FailDefault)
        {
            try
            {
                string WaferID;
                int FormHMIWritehvar = tcClient.CreateVariableHandle(Plc_Variable);
                AdsStream datastream = new AdsStream(15);
                BinaryReader binRead = new BinaryReader(datastream);
                datastream.Position = 0;
                tcClient.Read(FormHMIWritehvar, datastream);


                string temvalue = Encoding.Default.GetString(binRead.ReadBytes(15));
                WaferID = temvalue.Substring(0, temvalue.IndexOf('\0'));

                return WaferID;
            }
            catch (Exception ex)
            {

                WriteLog(Plc_Variable + "   " + ex.Message);
                return FailDefault;
            }
        }


        public static string[] g_ExitWaferID = new string[8];
        public static string[] g_ExitWaferID_Old = new string[8];

        void GetExitBeltWaferIDFromPLC()
        {
            try
            {
                string temvalue;
                int FormHMIWritehvar = tcClient.CreateVariableHandle(".g_ExitWaferID");
                AdsStream datastream = new AdsStream(15 * 8); //每个元素占用2位
                BinaryReader binRead = new BinaryReader(datastream);
                datastream.Position = 0;
                tcClient.Read(FormHMIWritehvar, datastream);

                for (int i = 0; i < 8; i++)
                {
                    temvalue = Encoding.Default.GetString(binRead.ReadBytes(15));
                    g_ExitWaferID[i] = temvalue.Substring(0, temvalue.IndexOf('\0'));


                }


                if (m_FirstStartupFlag)
                {
                    m_FirstStartupFlag = false;

                    g_ExitWaferID_Old[7] = g_ExitWaferID[7];
                }

            }
            catch (Exception ex)
            {
                //WriteLog("下料皮带获取WaferID失败！"+ex.Message);
            }
        }

        /*********************************************************************************************************
         ** 函数名称 ：fWriteToPlc
         ** 函数功能 ：写PLC的Bool型变量
         ** 入口参数 ：
         ** 出口参数 ： 
         *********************************************************************************************************/
        public void fWriteToPlc(string Plc_Variable, bool OnOff)
        {

            int FormHMIWritehvar;
            try
            {

                FormHMIWritehvar = tcClient.CreateVariableHandle(Plc_Variable);
                AdsStream datastream = new AdsStream(1); //每个元素占用2位
                BinaryWriter binwrite = new BinaryWriter(datastream);
                datastream.Position = 0;
                binwrite.Write(OnOff);
                tcClient.Write(FormHMIWritehvar, datastream);
                tcClient.DeleteVariableHandle(FormHMIWritehvar);

                binwrite.Dispose();
                datastream.Dispose();

            }
            catch
            {
                // MessageBox.Show("get hvar error");
            }
        }

        public void fWriteToPlc(string Plc_Variable, ushort OnOff)
        {

            int FormHMIWritehvar;
            try
            {

                FormHMIWritehvar = tcClient.CreateVariableHandle(Plc_Variable);
                AdsStream datastream = new AdsStream(2); //每个元素占用2位
                BinaryWriter binwrite = new BinaryWriter(datastream);
                datastream.Position = 0;
                binwrite.Write(OnOff);
                tcClient.Write(FormHMIWritehvar, datastream);
                tcClient.DeleteVariableHandle(FormHMIWritehvar);

                binwrite.Dispose();
                datastream.Dispose();

            }
            catch
            {
                // MessageBox.Show("get hvar error");
            }
        }

        void fMesHandler()
        {
            //while (true)
            //{
            //    try
            //    {
            //        Thread.Sleep(100);
            //        fMesEventSignalRefresh();
            //        fEventHandler();
            //        fCheckConnect();
            //    }
            //    catch
            //    {
            //    }

            //}
        }

        public static string[] g_ExitKKWaferID = new string[105];

        void GetWaferIdFromPLC()
        {
            try
            {
                DateTime aa = DateTime.Now;
                string temvalue;
                int FormHMIWritehvar = tcClient.CreateVariableHandle(".g_ExitKKWaferID");
                AdsStream datastream = new AdsStream(15 * 101); //每个元素占用2位
                BinaryReader binRead = new BinaryReader(datastream);
                datastream.Position = 0;
                tcClient.Read(FormHMIWritehvar, datastream);

                for (int i = 0; i < 101; i++)
                {
                    temvalue = Encoding.Default.GetString(binRead.ReadBytes(15));
                    g_ExitKKWaferID[i] = temvalue.Substring(0, temvalue.IndexOf('\0'));
                    //    m_WaferIDLog.Write("出料花篮第" + i.ToString() + "片waferid:" + g_ExitKKWaferID[i]);
                    //    m_WaferIDCheckLogTest.Write("出料花篮第" + i.ToString() + "片waferid:" + g_ExitKKWaferID[i]);
                }
            }
            catch
            {
                WriteLog("下料出花篮获取waferid失败！");
            }
        }

        bool m_IsRFIDRead = false, IssueWaferIDCom;
        int m_WaitMesSentWaferIDCounter;
        List<string> UnLoadWaferIDList = new List<string>();
        int m_LoadRFIDReadCounter = 0;

        void fEventHandler()
        {
            if (!m_FirstTimeRun)
            {

                //上料花篮每出一片硅片
                if (g_InPieceNum_Old != g_InPieceNum)
                {
                    g_InPieceNum_Old = g_InPieceNum;

                    int CurrentCellIn = OpcReadWrite.Read("ns=2;s=DRLaser/vs_CurrentCellIn", 0);
                    OpcReadWrite.Write("ns=2;s=DRLaser/vs_CurrentCellIn", ++CurrentCellIn);

                    formProductionStatistics.AddWaferInData();
                    WriteLog("g_InPieceNum信号触发-上料Wafer从花篮流出！");
                }

                //下料花篮每进一片硅片
                if (g_OutKKInPiece_Old != g_OutKKInPiece)
                {
                    g_OutKKInPiece_Old = g_OutKKInPiece;

                    int CurrentCellOut = OpcReadWrite.Read("ns=2;s=DRLaser/vs_CurrentCellOut", 0);
                    OpcReadWrite.Write("ns=2;s=DRLaser/vs_CurrentCellOut", ++CurrentCellOut);

                    formProductionStatistics.AddWaferOutData();
                    WriteLog("g_OutKKInPiece信号触发-下料Wafer进入花篮！");

                    //string UnLoadWaferID = fReadFromPlc_String(".g_ExitOutWaferID", "5373999999");
                    //UnLoadWaferIDList.Add(UnLoadWaferID);

                    //OpcReadWrite.Write("ns=2;s=DRLaser/vs_UnLoadWaferIDs", UnLoadWaferIDList);

                }


                if (g_UnLoadCarrierArriveAtRFIDPortExit == 1)
                {
                    g_UnLoadCarrierArriveAtRFIDPortExit = 0;
                    fWriteToPlc(".g_UnLoadCarrierArriveAtRFIDPortExit", 0);

                    UnLoadWaferIDList.Clear();

                    GetWaferIdFromPLC();

                    Thread.Sleep(10);

                    for (int i = 1; i < 101; i++)
                    {
                        if (g_ExitKKWaferID[i].Length > 3)
                        {
                            UnLoadWaferIDList.Add(g_ExitKKWaferID[i]);
                        }
                        else
                        {
                            UnLoadWaferIDList.Add("0");
                        }


                        WriteLog("g_ExitKKWaferID[" + i + "] = " + g_ExitKKWaferID[i]);
                    }

                    OpcReadWrite.Write("ns=2;s=DRLaser/vs_UnLoadWaferIDs", UnLoadWaferIDList);

                    WriteLog("g_UnLoadCarrierArriveAtRFIDPortExit信号触发 - 下料花篮开始出花篮！设备将vs_UnLoadReadRFIDCom信号写True！");

                    OpcReadWrite.Write("ns=2;s=DRLaser/vs_UnLoadReadRFIDCom", true);


                }


                if (iState_ExitRFIDCHECK_HMI == 1)
                {
                    iState_ExitRFIDCHECK_HMI = 0;

                    WriteLog("iState_ExitRFIDCHECK_HMI信号触发！- 下料花篮进入KK，开始读RFID！");

                    string UnLoadRFID = FormSygoleRFID.formSygoleRFID.ReadUnLoadRFID();

                    if (!(UnLoadRFID.Contains("SJ") || UnLoadRFID.Contains("ZR")))
                    {
                        FormSygoleRFID.formSygoleRFID.WriteLog("下料RFID第1次读取失败，开始第2次读取RFID.......");
                        Thread.Sleep(50);
                        UnLoadRFID = FormSygoleRFID.formSygoleRFID.ReadUnLoadRFID();  //下料RFID读取失败后，第1次尝试
                        if (!(UnLoadRFID.Contains("SJ") || UnLoadRFID.Contains("ZR")))
                        {
                            FormSygoleRFID.formSygoleRFID.WriteLog("下料RFID第2次读取失败，开始第3次读取RFID.......");
                            Thread.Sleep(50);
                            UnLoadRFID = FormSygoleRFID.formSygoleRFID.ReadUnLoadRFID();  //下料RFID读取失败后，第2次尝试
                        }
                    }

                    FormSygoleRFID.formSygoleRFID.WriteLog("下料RFID读取结果：" + UnLoadRFID);

                    fWriteToPlc(".iState_ExitRFIDCHECK_HMI", 0);
                    Thread.Sleep(10);
                    fWriteToPlc(".iState_ExitRFIDCHECK_HMI", 0);

                }




                //上料花篮到达KK,等待RFID读取
                if (iState_ENTERRFIDCHECK_RFID == 1)
                {

                    if (m_LoadRFIDReadCounter < 10)
                        m_LoadRFIDReadCounter++;
                    else
                    {
                        WriteLog("iState_ENTERRFIDCHECK_RFID信号触发-上料花篮进入RFID位置，开始读RFID！");
                        m_LoadRFIDReadCounter = 0;

                        iState_ENTERRFIDCHECK_RFID = 0;

                        string LoadRFID = FormSygoleRFID.formSygoleRFID.ReadLoadRFID();

                        if (!(LoadRFID.Contains("SJ") || LoadRFID.Contains("ZR")))
                        {
                            FormSygoleRFID.formSygoleRFID.WriteLog("上料RFID第1次读取失败，开始第2次读取RFID.......");
                            Thread.Sleep(50);
                            LoadRFID = FormSygoleRFID.formSygoleRFID.ReadLoadRFID();  //上料RFID读取失败后，第1次尝试

                            if (!(LoadRFID.Contains("SJ") || LoadRFID.Contains("ZR")))
                            {
                                Thread.Sleep(50);
                                FormSygoleRFID.formSygoleRFID.WriteLog("上料RFID第2次读取失败，开始第3次读取RFID.......");
                                LoadRFID = FormSygoleRFID.formSygoleRFID.ReadLoadRFID();  //上料RFID读取失败后，第2次尝试
                            }
                        }

                        FormSygoleRFID.formSygoleRFID.WriteLog("上料RFID读取结果：" + LoadRFID);

                        if (LoadRFID.Contains("SJ") || LoadRFID.Contains("ZR"))
                        {
                            OpcReadWrite.Write("ns=2;s=DRLaser/vs_LoadRFID", LoadRFID);
                            OpcReadWrite.Write("ns=2;s=DRLaser/vs_LoadReadRFIDCom", true);
                            //  m_IsRFIDRead = true;
                            m_WaitMesSentWaferIDCounter = 1;
                            fWriteToPlc(".iState_ENTERRFIDCHECK_RFID", 0);
                        }
                        else
                        {
                            fWriteToPlc(".iState_ENTERRFIDCHECK_RFID", 0);
                            // m_IsRFIDRead = false;

                            //报警同时，自动产生100个WaferID
                            for (int i = 1; i <= 100; i++)
                            {
                                fWriteToPlc_String(".g_EnterKKWaferID[" + i + "]", "\0".PadRight(14, '\0'));
                            }


                            WriteLog("上料读取RFID异常！开始自动产生下发100个WaferID:");
                            for (int i = 1; i <= 100; i++)
                            {
                                g_SelfGenerateWaferIDArray[100 - i] = WaferIDBuilder.GenerateWaferID();
                                WriteLog(string.Format("WaferID[{0}] : {1}", 100 - i, g_SelfGenerateWaferIDArray[100 - i]));
                                fWriteToPlc_String(".g_EnterKKWaferID[" + (101 - i).ToString() + "]", g_SelfGenerateWaferIDArray[100 - i].PadRight(14, '\0'));
                            }
                        }
                    }





                }

                //从MES端接收100个WaferID并分配给PLC
                if (IssueWaferIDCom)
                {

                    m_WaitMesSentWaferIDCounter = 0;

                    WriteLog("vs_IssueWaferIDCom信号触发！");

                    g_LoadWaferIDArray = OpcReadWrite.Read("ns=2;s=DRLaser/vs_LoadWaferIDs", WaferIDInit);

                    for (int i = 1; i <= 100; i++)
                    {
                        fWriteToPlc_String(".g_EnterKKWaferID[" + i + "]", "\0".PadRight(14, '\0'));
                    }

                    WriteLog(string.Format("MES下发{0}个WaferID:", g_LoadWaferIDArray.Length));
                    for (int i = 1; i <= g_LoadWaferIDArray.Length; i++)
                    {

                        WriteLog(string.Format("WaferID[{0}] : {1}", i - 1, g_LoadWaferIDArray[i - 1]));
                        fWriteToPlc_String(".g_EnterKKWaferID[" + (g_LoadWaferIDArray.Length + 1 - i).ToString() + "]", g_LoadWaferIDArray[i - 1].PadRight(14, '\0'));
                    }

                    OpcReadWrite.Write("ns=2;s=DRLaser/vs_IssueWaferIDCom", false);
                    WriteLog("机台复位vs_IssueWaferIDCom信号！");
                }

                //从MES端接收WaferID异常，自动产生100个WaferID
                if (m_WaitMesSentWaferIDCounter > 0)
                {
                    if (m_WaitMesSentWaferIDCounter < 70)  //延时7秒未回复，花篮直接进入
                        m_WaitMesSentWaferIDCounter++;
                    else
                    {
                        m_WaitMesSentWaferIDCounter = 0;
                        WriteLog("MES未在7秒内下发WaferID，花篮自动进入！");

                        for (int i = 1; i <= 100; i++)
                        {
                            fWriteToPlc_String(".g_EnterKKWaferID[" + i + "]", "\0".PadRight(14, '\0'));
                        }


                        WriteLog("开始自动产生下发100个WaferID:");
                        for (int i = 1; i <= 100; i++)
                        {
                            g_SelfGenerateWaferIDArray[i - 1] = WaferIDBuilder.GenerateWaferID();
                            WriteLog(string.Format("WaferID[{0}] : {1}", i - 1, g_SelfGenerateWaferIDArray[i - 1]));
                            fWriteToPlc_String(".g_EnterKKWaferID[" + (101 - i).ToString() + "]", g_SelfGenerateWaferIDArray[i - 1].PadRight(14, '\0'));
                        }

                        fWriteToPlc(".iState_ENTERRFIDCHECK_RFID", 0);
                    }

                }






            }
        }

        void DeleteExpiredFiles(string FilePath, string FileType, int Days)
        {
            /* File Path: "C:\\LogFiles\\"        KeepDays:7     FileType:"*.txt"       */
            DirectoryInfo dyInfo;
            //文件夹路径
            try
            {
                dyInfo = new DirectoryInfo(FilePath);
                //获取文件夹下所有的文件
                foreach (FileInfo feInfo in dyInfo.GetFiles(FileType))
                {
                    //判断文件最后写入日期是否Days天前，是则删除
                    if (feInfo.LastWriteTime.AddDays(Days) < DateTime.Today)
                        feInfo.Delete();
                }
            }
            catch
            {
            }
        }


        void fCheckIOStatus()
        {
            int MachineStatus = _cfg.ReadConfig("MES", "MachineStatus", 0);
            OpcReadWrite.Write("ns=2;s=DRLaser/vs_MachineStatus", MachineStatus);

            int AlarmID = _cfg.ReadConfig("MES", "AlarmID", 0);
            int AlarmCode = _cfg.ReadConfig("MES", "AlarmCode", 0);

            if (AlarmCode >= 200)
            {
                OpcReadWrite.Write("ns=2;s=DRLaser/vs_AlarmCode", AlarmCode);
            }
            else
            {
                OpcReadWrite.Write("ns=2;s=DRLaser/vs_AlarmCode", AlarmID);
            }

            int LoadCasNum = _cfg.ReadConfig("MES", "vs_LoadCasNum", 0);
            OpcReadWrite.Write("ns=2;s=DRLaser/vs_LoadCasNum", LoadCasNum);

            int UnLoadCasNum = _cfg.ReadConfig("MES", "vs_UnLoadCasNum", 0);
            OpcReadWrite.Write("ns=2;s=DRLaser/vs_UnLoadCasNum", UnLoadCasNum);

            bool LoadAGVDisable = _cfg.ReadConfig("MES", "vs_LoadAGVDisable", false);
            OpcReadWrite.Write("ns=2;s=DRLaser/vs_LoadAGVDisable", LoadAGVDisable);

            bool UnLoadAGVDisable = _cfg.ReadConfig("MES", "vs_UnLoadAGVDisable", false);
            OpcReadWrite.Write("ns=2;s=DRLaser/vs_UnLoadAGVDisable", UnLoadAGVDisable);

            IssueWaferIDCom = OpcReadWrite.Read("ns=2;s=DRLaser/vs_IssueWaferIDCom", false);


            bool ProClear = OpcReadWrite.Read("ns=2;s=DRLaser/vs_ProClear", false);
            if (ProClear)
            {

                int CurrentCellIn = OpcReadWrite.Read("ns=2;s=DRLaser/vs_CurrentCellIn", 0);
                OpcReadWrite.Write("ns=2;s=DRLaser/vs_LastCellIn", CurrentCellIn);
                OpcReadWrite.Write("ns=2;s=DRLaser/vs_CurrentCellIn", 0);

                int CurrentCellOut = OpcReadWrite.Read("ns=2;s=DRLaser/vs_CurrentCellOut", 0);
                OpcReadWrite.Write("ns=2;s=DRLaser/vs_LastCellOut", CurrentCellOut);
                OpcReadWrite.Write("ns=2;s=DRLaser/vs_CurrentCellOut", 0);



                Thread.Sleep(300);
                OpcReadWrite.Write("ns=2;s=DRLaser/vs_ProClearFinish", 1);
                OpcReadWrite.Write("ns=2;s=DRLaser/vs_ProClear", false);
            }





        }

        int usingResource;
        private void timerMES_Tick(object sender, EventArgs e)
        {
            //if (0 == Interlocked.Exchange(ref usingResource, 1))
            //{
            //try
            //{
            //    if (m_CurrentDay != DateTime.Now.Day)
            //    {
            //        m_CurrentDay = DateTime.Now.Day;

            //        DeleteExpiredFiles("D:\\MESLog\\MES\\", "*.log", 14);
            //        DeleteExpiredFiles("D:\\MESLog\\RFID\\", "*.log", 14);
            //        DeleteExpiredFiles("D:\\MESLog\\AGV\\", "*.log", 14);
            //        DeleteExpiredFiles("D:\\DRLaser\\ProductionData\\", "*.ini", 14);
            //    }

            //    fCheckIOStatus();
            //    fMesEventSignalInit();
            //    fMesEventSignalRefresh();
            //    fEventHandler();
            //    fCheckConnect();

            //    fRefreshHMI();
            //}
            //catch (Exception ex)
            //{
            //    WriteLog("timerMES error: " + ex.Message);
            //}


            //    //释放资源
            //    Interlocked.Exchange(ref usingResource, 0);
            //}






        }

        string[] WaferIDInit = new string[1] { "0" }, g_LoadWaferIDArray, g_SelfGenerateWaferIDArray = new string[100];
        Queue<string> LoadWaferIDQueue = new Queue<string>();

        private void fRefreshHMI()
        {
            CurrentCellIn.Text = OpcReadWrite.Read("ns=2;s=DRLaser/vs_CurrentCellIn", 0).ToString();
            LastCellIn.Text = OpcReadWrite.Read("ns=2;s=DRLaser/vs_LastCellIn", 0).ToString();
            ProClear.Text = OpcReadWrite.Read("ns=2;s=DRLaser/vs_ProClear", false).ToString();
            ProClearFinish.Text = OpcReadWrite.Read("ns=2;s=DRLaser/vs_ProClearFinish", 0).ToString();
            MachineStatus.Text = OpcReadWrite.Read("ns=2;s=DRLaser/vs_MachineStatus", 0).ToString();
            AlarmCode.Text = OpcReadWrite.Read("ns=2;s=DRLaser/vs_AlarmCode", 0).ToString();
            LoadRFID.Text = OpcReadWrite.Read("ns=2;s=DRLaser/vs_LoadRFID", "");
            //LoadWaferids.Text = OpcReadWrite.Read("ns=2;s=DRLaser/vs_LoadWaferIDs", WaferIDInit).ToString();
            IssueWaferidCom.Text = OpcReadWrite.Read("ns=2;s=DRLaser/vs_IssueWaferIDCom", false).ToString();
            LoadReadRFIDCom.Text = OpcReadWrite.Read("ns=2;s=DRLaser/vs_LoadReadRFIDCom", false).ToString();
            LoadCasCheck.Text = OpcReadWrite.Read("ns=2;s=DRLaser/vs_LoadCasCheck", 0).ToString();
            LoadMesMode.Text = OpcReadWrite.Read("ns=2;s=DRLaser/vs_LoadMesMode", false).ToString();
            LoadRFIDDisable.Text = OpcReadWrite.Read("ns=2;s=DRLaser/vs_LoadRFIDDisable", false).ToString();
            CasLeftCellNum.Text = OpcReadWrite.Read("ns=2;s=DRLaser/vs_CasLeftCellNum", 0).ToString();



        }

        private void RFIDMenuItem_Click(object sender, EventArgs e)
        {
            FormSygoleRFID.formSygoleRFID.Show();
        }

        private void AGVMenuItem_Click(object sender, EventArgs e)
        {
            FormAGV.formAGV.Show();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void 清空内容ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogBox.Clear();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void 通信设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSet.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }











    }
}
