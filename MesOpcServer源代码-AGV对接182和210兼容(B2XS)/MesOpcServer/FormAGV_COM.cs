using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using IniHelper;
using EasyLog;
using Sunny.UI;

namespace MesOpcServer
{
    public partial class FormAGV_COM : UIForm
    {
        public static bool StartWork = false;


        bool agvWorkFlagIn = false;
        bool agvWorkFlagOut = false;
        bool agvAlarmFlag = false;

        bool IsComDataReceived;

        byte AGVComunicateID = 0x00;

        OpcUaReadWrite OpcReadWrite;


        //     FormHMI formHmi;

        #region   AGVDATA    十进十出   下进上出
        public Log m_Log;

        byte[] SendHandShakeUnload50 = { 0xB5, 0xFF, 0xFF, 0x03, 0x01, 0x10, 0xA0, 0xA3, 0xEC, 0x5B };  //握手并报告状态
        byte[] SendHandShakeUnload05 = { 0xB5, 0xFF, 0xFF, 0x03, 0x01, 0x10, 0x0F, 0xE3, 0x90, 0x5B };  //握手并报告状态
        byte[] SendHandShakeUnload55 = { 0xB5, 0xFF, 0xFF, 0x03, 0x01, 0x10, 0xAF, 0xE3, 0xE8, 0x5B };  //握手并报告状态

        byte[] SendLoadingUnload50 = { 0xB5, 0xFF, 0xFF, 0x03, 0x02, 0x10, 0xF0, 0x53, 0xD0, 0x5B };  //机器人接受起转
        byte[] SendLoadingUnload05 = { 0xB5, 0xFF, 0xFF, 0x03, 0x02, 0x11, 0x00, 0x52, 0x04, 0x5B };  //机器人接受起转
        byte[] SendLoadingUnload55 = { 0xB5, 0xFF, 0xFF, 0x03, 0x02, 0x10, 0xF0, 0x53, 0xD0, 0x5B };  //机器人接受起转

        byte[] SendReceivedUnload50 = { 0xB5, 0xFF, 0xFF, 0x03, 0x04, 0x10, 0xF0, 0xB3, 0xD1, 0x5B };
        byte[] SendReceivedUnload05 = { 0xB5, 0xFF, 0xFF, 0x03, 0x04, 0x11, 0x00, 0xB2, 0x05, 0x5B };
        byte[] SendReceivedUnload55 = { 0xB5, 0xFF, 0xFF, 0x03, 0x04, 0x10, 0xF0, 0xB3, 0xD1, 0x5B };

        byte[] SendLeave = { 0xB5, 0xFF, 0xFF, 0x03, 0x05, 0x10, 0x10, 0xE3, 0x99, 0x5B };  //
        byte[] SendCRCError = { 0xB5, 0xFF, 0xFF, 0x03, 0xEE, 0x11, 0x01, 0x52, 0x31, 0x5B };  //校验失败
        byte[] ErrorTrackState = { 0xB5, 0xFF, 0xFF, 0x03, 0xEE, 0x10, 0x02, 0x13, 0xA0, 0x5B };  //错误轨道状态

        byte[] ErrorQueryState = { 0xB5, 0xFF, 0xFF, 0x03, 0xEE, 0x10, 0x08, 0x93, 0xA7, 0x5B };
        byte[] SendIDError = { 0xB5, 0xFF, 0xFF, 0x03, 0xEE, 0x10, 0x11, 0x52, 0x6d, 0x5B };  //校验失败

        #endregion


        int m_ucLoadAGVStep = 20;	 // 上料RS232发送指令与响应的对应step 
        int m_AGVHandShakeState = 0;      //0空闲状态       1:进5个花篮      2出5个花篮      3进5个花篮并出5个花篮

        int SideBStepState = 0;
        SerialPort ComPortAGV = new SerialPort();//定义串口对象

        string cfgpath = "D:\\DRLaser\\";
        //    Configuration.Configure _cfg = new Configuration.Configure();// ;//= new Configure(cfgpath);

        Configure cfg_MES_Agv = new Configure("D:\\DRLaser\\MES\\MES.ini");


        public FormAGV_COM()
        {
            InitializeComponent();
            m_Log = Log.getInstance();

          //  OpcReadWrite = OpcUaReadWrite.getInstance();

        }



        private void FormAGV_COM_Load(object sender, EventArgs e)
        {
            ConnectToPLC();


            fInitialCOM();

            AddListMsg("AGV窗口初始化，启用COM21串口接收数据,");
            AddListMsg("界面显示！");


            timerAGV.Enabled = true;
        }

        void fInitialCOM()
        {
            Task.Run(() =>
               {
                   try
                   {
                       ComPortAGV.BaudRate = 19200;

                       ComPortAGV.PortName = "COM21";
                       ComPortAGV.DataBits = 8;
                       ComPortAGV.Parity = (Parity)0;
                       ComPortAGV.StopBits = StopBits.One;
                       ComPortAGV.WriteTimeout = 3000;
                       ComPortAGV.ReadTimeout = 3000;
                       ComPortAGV.Open();

                       if (ComPortAGV.IsOpen)
                       {
                           ComPortAGV.ReceivedBytesThreshold = 5;
                           ComPortAGV.DataReceived += new SerialDataReceivedEventHandler(com_DataReceivedLoad);
                           AddListMsg("打开串口成功!");
                       }
                   }
                   catch (Exception ex)
                   {
                       MessageBox.Show("AGV串口" + ComPortAGV.PortName + "打开失败！错误：" + ex.Message);
                   }
               });
        }

        void ComDataHandler()
        {
            if (IsComDataReceived)
            {
                IsComDataReceived = false;

                try
                {
                    //接收数据

                    int count = ComPortAGV.BytesToRead;
                    if ((count <= 0) || (count > 100))
                    {
                        ComPortAGV.DiscardInBuffer();
                        return;
                    }

                    byte[] read_data = new byte[count];
                    ComPortAGV.Read(read_data, 0, count);
                    int i = 0;
                    string mTempStr = "";
                    for (i = 0; i < count; i++)
                    {
                        mTempStr = mTempStr + " " + read_data[i].ToString("X2");
                    }

                    AddListMsg("串口接收到数据:" + mTempStr);

                    if (count > 20 || count < 9)
                    {
                        AddListMsg("接收数据长度不对！");
                        return;
                    }

                    if (read_data[0] != 0xB5)
                    {
                        AddListMsg("数据头不正确");
                        return;
                    }



                    byte CRCHi = 0x00;
                    byte CRCLo = 0x00;
                    Crc16(read_data, 7, ref  CRCHi, ref CRCLo);

                    if ((read_data[7] != CRCLo) || (read_data[8] != CRCHi))
                    {
                        AddListMsg("SendCRCError : CRC校验出错");
                        ComWrt(ComPortAGV, SendCRCError, 10);
                        m_ucLoadAGVStep = 20;
                        return;
                    }

                    if (read_data[0] == 0xB5)
                    {
                        if ((read_data[4] == 0x01) && (agvWorkFlagIn == false) && (agvWorkFlagOut == false) && (m_ucLoadAGVStep == 20))
                        {
                            AddListMsg("AGV机器人请求握手,开始启动AGV模式");
                            if ((read_data[2] == 0xA0) && ((read_data[6] == 0x5A) || (read_data[6] == 0xAA)))
                            {
                                AddListMsg("轨道任务A0,只接收无传送");
                                m_ucLoadAGVStep = 0;
                                m_AGVHandShakeState = 1;
                            }
                            else if (read_data[2] == 0x0F)
                            {
                                AddListMsg("轨道任务0F,只传送无接收");
                                m_ucLoadAGVStep = 0;
                                m_AGVHandShakeState = 2;
                            }
                            else if (((read_data[2] == 0xAF) || ((read_data[2] == 0xFF))) && (read_data[6] == 0xA5))
                            {
                                AddListMsg("轨道任务AF,即接收又发送");
                                m_ucLoadAGVStep = 0;
                                m_AGVHandShakeState = 3;
                            }
                            else
                            {
                                AddListMsg("握手字节不匹配！");
                                m_AGVHandShakeState = 0;
                            }
                        }
                        else if ((read_data[4] == 0x01) && ((m_ucLoadAGVStep == 20) || (m_ucLoadAGVStep == 0) || (m_ucLoadAGVStep == 2)))
                        {
                            AddListMsg("AGV机器人请求握手,AGV模式已启动");

                            if ((read_data[2] == 0xA0) && ((read_data[6] == 0x5A) || (read_data[6] == 0xAA)) && (agvWorkFlagIn == true))
                            {
                                AddListMsg("#轨道任务A0,只接收无传送");

                                m_ucLoadAGVStep = 1;
                                m_AGVHandShakeState = 1;
                            }
                            //   else if ((read_data[2] == 0xF0) && ((read_data[6] == 0x05) || (read_data[6] == 0x55)))
                            else if ((read_data[2] == 0xF0) && (agvWorkFlagOut == true))
                            {
                                AddListMsg("#轨道任务0F,只传送无接收");

                                m_ucLoadAGVStep = 1;
                                m_AGVHandShakeState = 2;
                            }
                            else if ((((read_data[2] == 0xAF) || ((read_data[2] == 0xFF))) && (read_data[6] == 0xA5)) && (agvWorkFlagIn == true) && (agvWorkFlagOut == true))
                            {
                                AddListMsg("#轨道任务AF,即接收又发送");
                                m_ucLoadAGVStep = 1;
                                m_AGVHandShakeState = 3;
                            }
                            else
                            {
                                AddListMsg("握手字节不匹配！或PLC未允许");
                                m_AGVHandShakeState = 0;
                            }
                        }
                        else if (read_data[4] == 0x02)
                        {
                            if ((read_data[6] == 0x0F) || (read_data[6] == 0x00))
                            {
                                m_ucLoadAGVStep = 3;
                                AddListMsg("AGV机器人发送轨道状态确认！");
                            }
                        }
                        else if (read_data[4] == 0x04)
                        {
                            if ((read_data[6] == 0x0F) || (read_data[6] == 0x00))
                            {
                                m_ucLoadAGVStep = 5;
                                AddListMsg("AGV机器人发送轨道接收完成确认");
                            }
                        }
                        else if ((read_data[4] == 0x05) && (read_data[6] == 0x01))
                        {
                            m_ucLoadAGVStep = 7;
                            AddListMsg("AGV机器人请求离开！");
                        }
                        else if (read_data[4] == 0xEE)
                        {
                            autoRunCount = 0;
                            //如果AGV那边报警了 我这边进料没有问题，花篮和传感器都正常，不响应他的报警。
                            if ((m_AGVHandShakeState == 3) && (FormAGV.LoadTrackInTotal == FormAGV.CassetteNumberConstant) && (FormAGV.LoadTrackOutTotal == 0) && (LED_SafeIn1.Value == false) && (LED_SafeIn2.Value == false) && (LED_SafeOut1.Value == false) && (LED_SafeOut2.Value == false))
                            {       //直接切换到正常状态 不响应0xee
                                //axActUtlType1.SetDevice2("M1216", 0);
                                //axActUtlType1.SetDevice2("M1217", 0);
                                Thread.Sleep(10);
                                axActUtlType1.SetDevice2("M0", 0);
                                axActUtlType1.SetDevice2("M50", 0);
                                axActUtlType1.SetDevice2("M850", 0);
                                axActUtlType1.SetDevice2("M900", 0);

                                axActUtlType1.SetDevice2("M1754", 0);
                                axActUtlType1.SetDevice2("M1756", 0);

                                m_ucLoadAGVStep = 20;
                                m_AGVHandShakeState = 0;
                                AddListMsg("收到AGV异常停止指令0xEE,设备已经完成了对接，不再响应AGV的报警,开始自动化运行。");
                            }
                            else
                            {

                                m_AGVHandShakeState = 0;
                                //夹紧气缸伸出
                                AddListMsg("AGV报警,进料出料停止,请检查之后点击强制对接完成！");
                                //axActUtlType1.SetDevice2("M1216", 0);
                                //axActUtlType1.SetDevice2("M1217", 0);
                                Thread.Sleep(10);
                                axActUtlType1.SetDevice2("M0", 0);
                                axActUtlType1.SetDevice2("M50", 0);
                                axActUtlType1.SetDevice2("M850", 0);
                                axActUtlType1.SetDevice2("M900", 0);

                            }
                        }
                        else if (read_data[4] == 0xAA)
                        {
                            autoRunCount = 0;
                            //如果AGV那边报警了 我这边进料没有问题，花篮和传感器都正常，不响应他的报警。
                            if ((m_AGVHandShakeState == 3) && (FormAGV.LoadTrackInTotal == FormAGV.CassetteNumberConstant) && (FormAGV.LoadTrackOutTotal == 0) && (LED_SafeIn1.Value == false) && (LED_SafeIn2.Value == false) && (LED_SafeOut1.Value == false) && (LED_SafeOut2.Value == false))
                            {   //直接切换到正常状态 不响应0xee
                                //axActUtlType1.SetDevice2("M1216", 0);
                                //axActUtlType1.SetDevice2("M1217", 0);
                                Thread.Sleep(10);
                                axActUtlType1.SetDevice2("M0", 0);
                                axActUtlType1.SetDevice2("M50", 0);
                                axActUtlType1.SetDevice2("M850", 0);
                                axActUtlType1.SetDevice2("M900", 0);

                                axActUtlType1.SetDevice2("M1754", 0);
                                axActUtlType1.SetDevice2("M1756", 0);

                                m_ucLoadAGVStep = 20;
                                m_AGVHandShakeState = 0;
                                AddListMsg("收到AGV异常停止指令0xAA,设备已经完成了对接,不再响应小车异常,设备开始自动化运行。");
                            }
                            else
                            {
                                Thread.Sleep(10);
                                axActUtlType1.SetDevice2("M0", 0);
                                axActUtlType1.SetDevice2("M50", 0);
                                axActUtlType1.SetDevice2("M850", 0);
                                axActUtlType1.SetDevice2("M900", 0);

                                m_AGVHandShakeState = 0;
                                AddListMsg("AGV报警,进料出料停止,请检查之后手动点击强制对接完成！");

                            }
                        }
                    }

                }
                catch (Exception ex)
                {

                    AddListMsg("AGV串口通讯异常:" + ex.Message);

                }
            }
        }

        private void com_DataReceivedLoad(object sender, SerialDataReceivedEventArgs e)
        {

            IsComDataReceived = true;

        }

        Queue<string> LogQueue = new Queue<string>();

        public void AddListMsg(string strMsg)
        {
            if (!string.IsNullOrEmpty(strMsg))
            {
                LogQueue.Enqueue(MESServer.SoftVersion + strMsg);
            }
        }

        void LogHandler()
        {
            try
            {
                if (LogQueue.Count > 500)
                    LogQueue.Clear();

                if (LogQueue.Count > 0)
                {
                    if (LogBox.IsHandleCreated)
                    {
                        string msg = LogQueue.Dequeue();

                        m_Log.Write("左侧进:" + FormAGV.LoadTrackInTotal.ToString() + "出:" + FormAGV.LoadTrackOutTotal.ToString() + "  " + msg, MsgType.AGV);

                        string str_msg = DateTime.Now.ToString("HH:mm:ss.fff  ") + msg + "\n";


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


        static int timercount = 1;
        int DelayCount2 = 0;
        int autoRunCount = 0;
        static bool CanStopflagIn1 = true;
        static bool CanStopflagIn2 = true;

        uint StopCountIn1 = 0;  //进料1延时计数
        uint StopCountIn2 = 0;  //进料2延时计数
        bool AllowLeave = false;

        int TrackInTenNumDelay = 0;
        bool TrackInTenNumOK = false;
        int m_AutoExitLoadStepCounter;

        //  int alarmCount = 0;
        private void timerAGV_Tick(object sender, EventArgs e)
        {
            try
            {
                timercount++;


                textBoxAGVstep.Text = m_ucLoadAGVStep.ToString();


                //允许信号状态灯
                led_AGVAllowIn.Value = AGVAllowIn;
                led_AGVAllowOut.Value = AGVAllowOut;

                LED_AGVWorkIn.Value = agvWorkFlagIn;
                LED_AGVWorkOut.Value = agvWorkFlagOut;

                ushort m_EmergencyStop = OpcReadWrite.Read("ns=2;s=DRLaser/AGVEmergencyStop", (ushort)0);

                if (m_EmergencyStop == 128)
                {
                    AddListMsg("收到MES下发的急停指令：128！");

                    OpcReadWrite.Write("ns=2;s=DRLaser/AGVEmergencyStop", (ushort)0);

                    if (m_AGVHandShakeState == 3)
                    {
                        axActUtlType1.SetDevice2("M1216", 0);
                        axActUtlType1.SetDevice2("M1217", 0);
                        Thread.Sleep(10);
                        axActUtlType1.SetDevice2("M0", 0);
                        axActUtlType1.SetDevice2("M50", 0);
                        axActUtlType1.SetDevice2("M850", 0);
                        axActUtlType1.SetDevice2("M900", 0);

                        axActUtlType1.SetDevice2("M1754", 0);
                        axActUtlType1.SetDevice2("M1756", 0);
                        m_ucLoadAGVStep = 20;
                        m_AGVHandShakeState = 0;

                        AddListMsg("收到MES下发的急停指令，进出料四个轨道全部停止！");
                    }
                    else if (m_AGVHandShakeState == 1)
                    {
                        //axActUtlType1.SetDevice2("M1216", 0);
                        //axActUtlType1.SetDevice2("M1217", 0);
                        //Thread.Sleep(100);
                        axActUtlType1.SetDevice2("M0", 0);
                        axActUtlType1.SetDevice2("M50", 0);
                        //axActUtlType1.SetDevice2("M850", 0);
                        //axActUtlType1.SetDevice2("M900", 0);

                        axActUtlType1.SetDevice2("M1754", 0);
                        //axActUtlType1.SetDevice2("M1756", 0);
                        m_ucLoadAGVStep = 20;
                        m_AGVHandShakeState = 0;

                        AddListMsg("收到MES下发的急停指令，进料两个轨道全部停止！");
                    }
                    else if (m_AGVHandShakeState == 2)
                    {
                        axActUtlType1.SetDevice2("M1216", 0);
                        axActUtlType1.SetDevice2("M1217", 0);
                        Thread.Sleep(10);
                        //axActUtlType1.SetDevice2("M0", 0);
                        //axActUtlType1.SetDevice2("M50", 0);
                        axActUtlType1.SetDevice2("M850", 0);
                        axActUtlType1.SetDevice2("M900", 0);

                        //axActUtlType1.SetDevice2("M1754", 0);
                        axActUtlType1.SetDevice2("M1756", 0);
                        m_ucLoadAGVStep = 20;
                        m_AGVHandShakeState = 0;

                        AddListMsg("收到MES下发的急停指令，出料两个轨道全部停止！");
                    }
                }

                if ((FormAGV.LoadTrackInTotal >= FormAGV.CassetteNumberConstant) && ((m_ucLoadAGVStep >= 4) && (m_ucLoadAGVStep <= 8)))
                {
                    TrackInTenNumDelay++;
                    if (TrackInTenNumDelay > 6)    //满足10个花篮3.5秒钟
                    {
                        TrackInTenNumDelay = 0;
                        TrackInTenNumOK = true;
                    }
                }
                else if ((m_ucLoadAGVStep < 4) && (m_ucLoadAGVStep > 8))
                {
                    TrackInTenNumDelay = 0;
                    TrackInTenNumOK = false;
                }



                if (agvAlarmFlag)
                {
                    AddListMsg("左侧agv对接状态超时,皮带停止转动,请检查" + "当前对接步骤:" + m_ucLoadAGVStep.ToString());
                }


                //非空闲状态下   握手状态不对报警
                if ((m_ucLoadAGVStep != 20) && (m_AGVHandShakeState != 1) && (m_AGVHandShakeState != 2) && (m_AGVHandShakeState != 3))
                {
                    AddListMsg("当前握手状态不对！");
                    ComWrt(ComPortAGV, ErrorTrackState, ErrorTrackState.Length);

                    m_ucLoadAGVStep = 20;
                    m_AGVHandShakeState = 0;
                    return;
                }



                if ((m_ucLoadAGVStep != 20) && (!LongLoadIn1Safe) && (!LongLoadIn2Safe) && (!LongLoadOut1Safe) && (!LongLoadOut2Safe) && (!agvWorkFlagIn) && (!agvWorkFlagOut))
                {
                    if (m_AutoExitLoadStepCounter < 8)
                        m_AutoExitLoadStepCounter++;
                    else
                    {
                        m_AutoExitLoadStepCounter = 0;

                        //axActUtlType1.SetDevice2("M1216", 0);
                        //axActUtlType1.SetDevice2("M1217", 0);
                        Thread.Sleep(10);
                        axActUtlType1.SetDevice2("M0", 0);
                        axActUtlType1.SetDevice2("M50", 0);
                        axActUtlType1.SetDevice2("M850", 0);
                        axActUtlType1.SetDevice2("M900", 0);

                        axActUtlType1.SetDevice2("M1754", 0);
                        axActUtlType1.SetDevice2("M1756", 0);
                        m_ucLoadAGVStep = 20;
                        m_AGVHandShakeState = 0;
                        AddListMsg("检测到接驳台复位，左侧自动强制对接完成！");
                    }
                }
                else
                {
                    m_AutoExitLoadStepCounter = 0;
                }


                #region  握手指令为3

                if (m_AGVHandShakeState == 3)
                {
                    switch (m_ucLoadAGVStep)
                    {
                        //已经处于AGVwork状态则不启动.

                        case 0:
                            if ((agvWorkFlagIn) || (agvWorkFlagOut))
                                break;
                            if ((FormAGV.LoadTrackInTotal == 0) && (FormAGV.LoadTrackOutTotal >= FormAGV.CassetteNumberConstant) && (m_AGVHandShakeState == 3) /*&& (LongLoadIn1OutStretch) && (LongLoadIn2OutStretch)*/)
                            {
                                if ((LED_SafeIn1.Value == false) && (LED_SafeIn2.Value == false) && (LED_SafeOut1.Value == false) && (LED_SafeOut2.Value == false))  //安全光电状态状态正确
                                {
                                    if ((AGVAllowIn) && (AGVAllowOut))
                                    {
                                        AddListMsg("开始启用AGV模式,等待AGV模式启动");
                                        axActUtlType1.SetDevice2("M1754", 1);
                                        axActUtlType1.SetDevice2("M1756", 1);
                                    }
                                    else
                                    {
                                        AddListMsg("PLC出料或者进料未允许,AGV机器人握手失败！");
                                        ComWrt(ComPortAGV, ErrorTrackState, ErrorTrackState.Length);
                                        m_ucLoadAGVStep = 20;
                                        m_AGVHandShakeState = 0;
                                    }
                                }
                                else
                                {
                                    //进料皮带状态  出料皮带状态  进料气缸状态  出料气缸状态
                                    AddListMsg("轨道安全光电状态不对,AGV机器人握手失败！");
                                    ComWrt(ComPortAGV, ErrorTrackState, ErrorTrackState.Length);
                                    m_ucLoadAGVStep = 20;
                                    m_AGVHandShakeState = 0;
                                }
                            }
                            else
                            {
                                AddListMsg("ErrorTrackState + 花篮数量不对,AGV机器人握手失败！");
                                ComWrt(ComPortAGV, ErrorTrackState, ErrorTrackState.Length);
                                m_ucLoadAGVStep = 20;
                                m_AGVHandShakeState = 0;
                                /*
                                if (((LongLoadIn1OutStretch) && (LongLoadIn2OutStretch)) == false)
                                {
                                    AddListMsg("进料长模组气缸状态不对,AGV机器人握手失败！");
                                }
                                 */
                            }
                            break;


                        case 1:
                            //进料阻挡气缸1和进料阻挡气缸2都是伸出状态。
                            AddListMsg("step1");
                            AllowLeave = false;
                            if ((FormAGV.LoadTrackInTotal == 0) && (FormAGV.LoadTrackOutTotal >= FormAGV.CassetteNumberConstant) && (m_AGVHandShakeState == 3) /*&& (LongLoadIn1OutStretch) && (LongLoadIn2OutStretch)*/)
                            {
                                if ((LED_SafeIn1.Value == false) && (LED_SafeIn2.Value == false) && (LED_SafeOut1.Value == false) && (LED_SafeOut2.Value == false))  //安全光电状态状态正确
                                {
                                    if ((AGVAllowIn) && (AGVAllowOut))
                                    {
                                        AddListMsg("回复握手SendHandShake55");
                                        ComWrt(ComPortAGV, SendHandShakeUnload55, 10);
                                        m_ucLoadAGVStep = 2;
                                    }
                                    else
                                    {
                                        AddListMsg("PLC出料或者进料未允许,AGV机器人握手失败！");
                                        ComWrt(ComPortAGV, ErrorTrackState, ErrorTrackState.Length);
                                        m_ucLoadAGVStep = 20;
                                        m_AGVHandShakeState = 0;
                                    }
                                }
                                else
                                {
                                    AddListMsg("轨道安全光电状态不对,AGV机器人握手失败！");
                                    ComWrt(ComPortAGV, ErrorTrackState, ErrorTrackState.Length);
                                    m_ucLoadAGVStep = 20;
                                    m_AGVHandShakeState = 0;
                                }
                            }
                            else
                            {
                                //if (LongLoadIn1OutStretch == false || LongLoadIn2OutStretch == false)
                                //{
                                //    AddListMsg("进料阻挡气缸状态不对,AGV机器人握手失败！");
                                //}
                                //else
                                //{
                                AddListMsg("ErrorTrackState + 花篮数量不对,AGV机器人握手失败！");
                                //}
                                ComWrt(ComPortAGV, ErrorTrackState, ErrorTrackState.Length);
                                m_ucLoadAGVStep = 20;
                                m_AGVHandShakeState = 0;
                            }

                            break;

                        case 2:
                            //WaitState = true;
                            AddListMsg("step2");
                            break;

                        case 3:
                            CanStopflagIn1 = true;
                            CanStopflagIn2 = true;
                            StopCountIn1 = 0;
                            //WaitState = false;
                            AddListMsg("step3");
                            if ((FormAGV.LoadTrackInTotal == 0) && (m_AGVHandShakeState == 3)/* && (LongLoadIn1OutStretch) && (LongLoadIn2OutStretch)*/)
                            {
                                if ((LED_SafeIn1.Value == false) && (LED_SafeIn2.Value == false) && (LED_SafeOut1.Value == false) && (LED_SafeOut2.Value == false))  //安全光电状态状态正确
                                {
                                    Thread.Sleep(10);
                                    AddListMsg("回复轨道状态确认:SendLoading55");
                                    ComWrt(ComPortAGV, SendLoadingUnload55, 10);

                                    //axActUtlType1.SetDevice2("M1216", 1);
                                    //axActUtlType1.SetDevice2("M1217", 1);
                                    Thread.Sleep(10);
                                    axActUtlType1.SetDevice2("M0", 1);
                                    axActUtlType1.SetDevice2("M50", 1);
                                    axActUtlType1.SetDevice2("M850", 1);
                                    axActUtlType1.SetDevice2("M900", 1);

                                    m_ucLoadAGVStep = 4;
                                }
                                else
                                {
                                    AddListMsg("轨道状态不对，未起转");
                                    m_ucLoadAGVStep = 2;
                                }
                            }
                            else
                            {
                                AddListMsg("进料阻挡气缸状态不对，未起转");
                            }
                            break;

                        case 4:
                            //防止传感器误感应.
                            AddListMsg("step4");

                            break;
                        case 5:
                            AllowLeave = false;
                            AddListMsg("step5");

                            if ((FormAGV.LoadTrackInTotal >= FormAGV.CassetteNumberConstant) && (FormAGV.LoadTrackOutTotal == 0) && (LED_SafeIn1.Value == false) && (LED_SafeIn2.Value == false) && (LED_SafeOut1.Value == false) && (LED_SafeOut2.Value == false))
                            {
                                //axActUtlType1.SetDevice2("M1216", 0); //A出1阻挡
                                //axActUtlType1.SetDevice2("M1217", 0); //A出2阻挡
                                //Thread.Sleep(10);
                                axActUtlType1.SetDevice2("M0", 0);
                                axActUtlType1.SetDevice2("M50", 0);
                                axActUtlType1.SetDevice2("M850", 0);
                                axActUtlType1.SetDevice2("M900", 0);
                                CanStopflagIn1 = false;
                                CanStopflagIn2 = false;
                                AddListMsg("进料皮带出料皮带全部停止，阻挡夹气缸伸出！");
                                AddListMsg("回复接收状态:SendReceived55");
                                ComWrt(ComPortAGV, SendReceivedUnload55, 10);

                                m_ucLoadAGVStep = 6;
                            }
                            else
                            {
                                AddListMsg("接收花篮数不正确SendReceived55  failed!" + FormAGV.LoadTrackInTotal.ToString() + ":" + FormAGV.LoadTrackOutTotal.ToString());
                                m_ucLoadAGVStep = 4;
                            }

                            break;

                        case 6:
                            AddListMsg("step6");
                            break;
                        case 7:
                            AddListMsg("step7");
                            if ((m_AGVHandShakeState == 3) && (FormAGV.LoadTrackInTotal >= FormAGV.CassetteNumberConstant) && (FormAGV.LoadTrackOutTotal == 0))
                            {
                                if ((LED_SafeIn1.Value == true) || (LED_SafeIn2.Value == true) || (LED_SafeOut1.Value == true) || (LED_SafeOut2.Value == true))
                                {
                                    AddListMsg("轨道状态已到位但是出料口或进料口卡花篮,请检查之后点击强制对接完成！");
                                    ComWrt(ComPortAGV, ErrorQueryState, ErrorTrackState.Length);
                                    m_ucLoadAGVStep = 6;
                                }
                                else
                                {


                                    axActUtlType1.SetDevice2("M1754", 0);
                                    axActUtlType1.SetDevice2("M1756", 0);
                                    ComWrt(ComPortAGV, SendLeave, 10);
                                    m_ucLoadAGVStep = 8;
                                    AllowLeave = true;
                                    AddListMsg("轨道状态已到位,允许AGV小车离开");


                                }
                            }
                            else if ((m_AGVHandShakeState == 3) && (FormAGV.LoadTrackOutTotal == 0) && (AllowLeave || TrackInTenNumOK))
                            {
                                AddListMsg("小车再次请求离开,允许AGV小车离开！");
                                //增加离开判断条件
                                if ((LED_SafeIn1.Value == true) || (LED_SafeIn2.Value == true) || (LED_SafeOut1.Value == true) || (LED_SafeOut2.Value == true))
                                {
                                    AddListMsg("轨道状态已到位但是出料口或进料口卡花篮,请检查之后点击强制对接完成！");
                                    ComWrt(ComPortAGV, ErrorQueryState, ErrorTrackState.Length);
                                    m_ucLoadAGVStep = 6;
                                }
                                else
                                {


                                    axActUtlType1.SetDevice2("M1754", 0);
                                    axActUtlType1.SetDevice2("M1756", 0);
                                    ComWrt(ComPortAGV, SendLeave, 10);
                                    m_ucLoadAGVStep = 8;
                                    AddListMsg("轨道状态已到位,允许AGV小车离开");


                                }
                            }

                            else
                            {
                                AddListMsg("当前轨道状态未到位");
                                m_ucLoadAGVStep = 6;
                            }

                            break;
                        case 8:
                            AllowLeave = true;
                            AddListMsg("step8");
                            DelayCount2++;
                            if (DelayCount2 < 2)
                            {
                                break;
                            }
                            axActUtlType1.SetDevice2("M1754", 0);
                            axActUtlType1.SetDevice2("M1756", 0);
                            AddListMsg("关闭AGV模式,当前AGV流程结束");
                            m_ucLoadAGVStep = 20;
                            break;

                        case 20:                        //等待状态
                            DelayCount2 = 0;
                            break;
                    }
                }
                #endregion
                #region  握手指令为1

                if (m_AGVHandShakeState == 1)
                {
                    switch (m_ucLoadAGVStep)
                    {
                        //已经处于AGVwork状态则不启动.

                        case 0:
                            if (agvWorkFlagIn)
                                break;
                            if (FormAGV.LoadTrackInTotal == 0)
                            {
                                if (AGVAllowIn)
                                {
                                    AddListMsg("开始启用AGV模式,等待AGV模式启动");
                                    axActUtlType1.SetDevice2("M1754", 1);
                                }
                                else
                                {
                                    AddListMsg("PLC出料或者进料未允许,AGV机器人握手失败！");
                                    ComWrt(ComPortAGV, ErrorTrackState, ErrorTrackState.Length);
                                    m_ucLoadAGVStep = 20;
                                    m_AGVHandShakeState = 0;
                                }
                                //  FormHMI.S7.WriteVBitToPLC(14, 0, true);
                                //写入AGVwork
                            }
                            else
                            {

                                AddListMsg("ErrorTrackState + 花篮数量不对,AGV机器人握手失败！");

                                ComWrt(ComPortAGV, ErrorTrackState, ErrorTrackState.Length);
                                m_ucLoadAGVStep = 20;
                                m_AGVHandShakeState = 0;
                            }
                            break;


                        case 1:
                            //进料阻挡气缸1和进料阻挡气缸2都是伸出状态。
                            AddListMsg("step1");
                            if ((FormAGV.LoadTrackInTotal == 0) && LongLoadIn1OutStretch && LongLoadIn2OutStretch)
                            {
                                //气缸状态和皮带状态到位
                                // if ((formHmi.m_Load_DI00 == 0) && (formHmi.m_Load_DI10 == 0) )
                                if ((LED_SafeIn1.Value == false) && (LED_SafeIn2.Value == false))  //安全光电状态状态正确
                                {
                                    if (AGVAllowIn)
                                    {
                                        AddListMsg("回复握手SendHandShake50");
                                        ComWrt(ComPortAGV, SendHandShakeUnload50, 10);

                                        m_ucLoadAGVStep = 2;
                                    }
                                    else
                                    {
                                        AddListMsg("PLC出料或者进料未允许,AGV机器人握手失败！");
                                        ComWrt(ComPortAGV, ErrorTrackState, ErrorTrackState.Length);
                                        m_ucLoadAGVStep = 20;
                                        m_AGVHandShakeState = 0;
                                    }
                                }
                                else
                                {
                                    //进料皮带状态  出料皮带状态  进料气缸状态  出料气缸状态
                                    AddListMsg("轨道安全光电状态不对,AGV机器人握手失败！");
                                    ComWrt(ComPortAGV, ErrorTrackState, ErrorTrackState.Length);
                                    m_ucLoadAGVStep = 20;
                                    m_AGVHandShakeState = 0;
                                }
                            }
                            else
                            {
                                AddListMsg("ErrorTrackState + 花篮数量不对,或进料阻挡气缸状态不对,AGV机器人握手失败！");
                                ComWrt(ComPortAGV, ErrorTrackState, ErrorTrackState.Length);
                                m_ucLoadAGVStep = 20;
                                m_AGVHandShakeState = 0;
                            }

                            break;

                        case 2:
                            //WaitState = true;
                            AddListMsg("step2");
                            break;

                        case 3:
                            CanStopflagIn1 = true;
                            CanStopflagIn2 = true;
                            StopCountIn1 = 0;
                            //WaitState = false;
                            AddListMsg("step3");
                            if ((FormAGV.LoadTrackInTotal == 0) && LongLoadIn1OutStretch && LongLoadIn2OutStretch)
                            {      //出料进料允许     5进5出
                                AddListMsg("当前任务10进0出,开始起转！");
                                if ((LED_SafeIn1.Value == false) && (LED_SafeIn2.Value == false))  //安全光电状态状态正确
                                {
                                    AddListMsg("回复轨道状态确认:SendLoading50");
                                    ComWrt(ComPortAGV, SendLoadingUnload50, 10);
                                    axActUtlType1.SetDevice2("M0", 1);
                                    axActUtlType1.SetDevice2("M50", 1);
                                    m_ucLoadAGVStep = 4;
                                }
                                else
                                {
                                    AddListMsg("轨道状态不对，未起转");
                                    m_ucLoadAGVStep = 2;
                                }
                            }
                            else
                            {
                                AddListMsg("进料阻挡气缸状态不对，未起转");
                            }
                            break;

                        case 4:
                            //防止传感器误感应.
                            AddListMsg("step4");

                            break;
                        case 5:
                            AddListMsg("step5");

                            if ((FormAGV.LoadTrackInTotal >= FormAGV.CassetteNumberConstant) && (LED_SafeIn1.Value == false) && (LED_SafeIn2.Value == false))
                            {
                                axActUtlType1.SetDevice2("M0", 0);
                                axActUtlType1.SetDevice2("M50", 0);


                                CanStopflagIn1 = false;
                                CanStopflagIn2 = false;
                                AddListMsg("进料皮带出料皮带全部停止，阻挡夹气缸伸出！");
                                AddListMsg("回复接收状态:SendReceived50");
                                ComWrt(ComPortAGV, SendReceivedUnload50, 10);

                                m_ucLoadAGVStep = 6;
                            }
                            else
                            {
                                AddListMsg("接收花篮数不正确SendReceived55  failed!" + FormAGV.LoadTrackInTotal.ToString() + ":" + FormAGV.LoadTrackOutTotal.ToString());
                                m_ucLoadAGVStep = 4;
                            }

                            break;

                        case 6:
                            AddListMsg("step6");
                            break;
                        case 7:

                            AddListMsg("step7");

                            if (FormAGV.LoadTrackInTotal == FormAGV.CassetteNumberConstant)
                            {
                                if ((LED_SafeIn1.Value == true) || (LED_SafeIn2.Value == true) || (LED_SafeOut1.Value == true) || (LED_SafeOut2.Value == true))
                                {
                                    AddListMsg("轨道状态已到位但是出料口或进料口卡花篮,请检查之后点击强制对接完成！");
                                    ComWrt(ComPortAGV, ErrorQueryState, ErrorTrackState.Length);
                                    m_ucLoadAGVStep = 6;
                                }
                                else
                                {
                                    axActUtlType1.SetDevice2("M1754", 0);
                                    ComWrt(ComPortAGV, SendLeave, 10);
                                    m_ucLoadAGVStep = 8;
                                    AddListMsg("轨道状态已到位,允许AGV小车离开");
                                }
                            }
                            else
                            {
                                AddListMsg("当前轨道状态未到位");
                                m_ucLoadAGVStep = 6;
                            }

                            break;
                        case 8:
                            AddListMsg("step8");
                            DelayCount2++;
                            if (DelayCount2 < 2)
                            {
                                break;
                            }
                            axActUtlType1.SetDevice2("M1754", 0);
                            AddListMsg("关闭AGV模式,当前AGV流程结束");
                            m_ucLoadAGVStep = 20;
                            break;

                        case 20:                        //等待状态
                            DelayCount2 = 0;
                            break;
                    }
                }
                #endregion
                #region  握手指令为2

                if (m_AGVHandShakeState == 2)
                {
                    switch (m_ucLoadAGVStep)
                    {
                        case 0:
                            if (agvWorkFlagOut)
                                break;
                            if ((FormAGV.LoadTrackOutTotal == FormAGV.CassetteNumberConstant) && (LED_SafeOut1.Value == false) && (LED_SafeOut2.Value == false))
                            {
                                if (AGVAllowOut)
                                {
                                    AddListMsg("开始启用AGV模式,等待AGV模式启动");
                                    axActUtlType1.SetDevice2("M1756", 1);
                                }
                                else
                                {
                                    AddListMsg("PLC出料或者进料未允许,AGV机器人握手失败！");
                                    ComWrt(ComPortAGV, ErrorTrackState, ErrorTrackState.Length);
                                    m_ucLoadAGVStep = 20;
                                    m_AGVHandShakeState = 0;
                                }
                            }
                            else
                            {
                                AddListMsg("ErrorTrackState + 花篮数量不对,或者安全光电被遮挡,AGV机器人握手失败！");

                                ComWrt(ComPortAGV, ErrorTrackState, ErrorTrackState.Length);
                                m_ucLoadAGVStep = 20;
                                m_AGVHandShakeState = 0;
                            }
                            break;


                        case 1:
                            AddListMsg("step1");
                            if (FormAGV.LoadTrackOutTotal == FormAGV.CassetteNumberConstant)
                            {
                                if ((LED_SafeOut1.Value == false) && (LED_SafeOut2.Value == false))  //安全光电状态状态正确
                                {
                                    if (AGVAllowOut)
                                    {
                                        AddListMsg("回复握手SendHandShake05");
                                        ComWrt(ComPortAGV, SendHandShakeUnload05, 10);

                                        m_ucLoadAGVStep = 2;
                                    }
                                    else
                                    {
                                        AddListMsg("PLC出料或者进料未允许,AGV机器人握手失败！");
                                        ComWrt(ComPortAGV, ErrorTrackState, ErrorTrackState.Length);
                                        m_ucLoadAGVStep = 20;
                                        m_AGVHandShakeState = 0;
                                    }
                                }
                                else
                                {
                                    //进料皮带状态  出料皮带状态  进料气缸状态  出料气缸状态
                                    AddListMsg("轨道安全光电状态不对,AGV机器人握手失败！");
                                    ComWrt(ComPortAGV, ErrorTrackState, ErrorTrackState.Length);
                                    m_ucLoadAGVStep = 20;
                                    m_AGVHandShakeState = 0;
                                }
                            }
                            else
                            {
                                AddListMsg("ErrorTrackState + 花篮数量不对,或进料阻挡气缸状态不对,AGV机器人握手失败！");
                                ComWrt(ComPortAGV, ErrorTrackState, ErrorTrackState.Length);
                                m_ucLoadAGVStep = 20;
                                m_AGVHandShakeState = 0;
                            }

                            break;

                        case 2:
                            //WaitState = true;
                            AddListMsg("step2");
                            break;

                        case 3:

                            AddListMsg("step3");
                            //出料进料允许     5进5出
                            AddListMsg("当前任务0进10出,开始起转！");
                            if ((LED_SafeOut1.Value == false) && (LED_SafeOut2.Value == false))  //安全光电状态状态正确
                            {



                                axActUtlType1.SetDevice2("M1216", 1);
                                axActUtlType1.SetDevice2("M1217", 1);

                                Thread.Sleep(10);

                                axActUtlType1.SetDevice2("M850", 1);
                                axActUtlType1.SetDevice2("M900", 1);

                                AddListMsg("回复轨道状态确认:SendLoading05");
                                ComWrt(ComPortAGV, SendLoadingUnload05, 10);



                                m_ucLoadAGVStep = 4;
                            }
                            else
                            {
                                AddListMsg("轨道状态不对，未起转");
                                m_ucLoadAGVStep = 2;
                            }

                            break;

                        case 4:
                            //防止传感器误感应.
                            AddListMsg("step4");
                            break;
                        case 5:
                            AddListMsg("step5");


                            if ((FormAGV.LoadTrackOutTotal == 0) && (LED_SafeOut1.Value == false) && (LED_SafeOut2.Value == false))
                            {
                                axActUtlType1.SetDevice2("M1216", 0);
                                axActUtlType1.SetDevice2("M1217", 0);
                                Thread.Sleep(10);
                                axActUtlType1.SetDevice2("M850", 0);
                                axActUtlType1.SetDevice2("M900", 0);

                                AddListMsg("出料皮带全部停止，阻挡夹气缸伸出！");
                                AddListMsg("回复接收状态:SendReceived05");
                                ComWrt(ComPortAGV, SendReceivedUnload05, 10);

                                m_ucLoadAGVStep = 6;
                            }
                            else
                            {
                                AddListMsg("接收花篮数不正确SendReceived05  failed!" + FormAGV.LoadTrackInTotal.ToString() + ":" + FormAGV.LoadTrackOutTotal.ToString());
                                m_ucLoadAGVStep = 4;
                            }

                            break;

                        case 6:
                            AddListMsg("step6");
                            break;
                        case 7:
                            AddListMsg("step7");
                            if (FormAGV.LoadTrackOutTotal == 0)
                            {
                                //出料加紧气缸是伸出状态

                                if ((LED_SafeIn1.Value == true) || (LED_SafeIn2.Value == true) || (LED_SafeOut1.Value == true) || (LED_SafeOut2.Value == true))
                                {
                                    AddListMsg("轨道状态已到位但是出料口或进料口卡花篮,请检查之后点击强制对接完成！");
                                    ComWrt(ComPortAGV, ErrorQueryState, ErrorTrackState.Length);
                                    m_ucLoadAGVStep = 6;
                                }
                                else
                                {
                                    if ((LongLoadOut1OutStretch) && (LongLoadOut2OutStretch))
                                    {
                                        axActUtlType1.SetDevice2("M1756", 0);
                                        ComWrt(ComPortAGV, SendLeave, 10);
                                        m_ucLoadAGVStep = 8;
                                        AddListMsg("轨道状态已到位,允许AGV小车离开");
                                    }
                                    else
                                    {
                                        AddListMsg("出料夹紧气缸未伸出到位!");
                                    }
                                }
                            }
                            else
                            {
                                AddListMsg("当前轨道状态未到位");
                                m_ucLoadAGVStep = 6;
                            }

                            break;
                        case 8:
                            AddListMsg("step8");
                            DelayCount2++;
                            if (DelayCount2 < 2)
                            {
                                break;
                            }
                            axActUtlType1.SetDevice2("M1756", 0);
                            AddListMsg("关闭AGV模式,当前AGV流程结束");
                            m_ucLoadAGVStep = 20;
                            break;

                        case 20:                        //等待状态
                            DelayCount2 = 0;
                            break;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                AddListMsg("左侧：AGV Timer对接流程出错：" + ex.Message);
            }


        }


        void ComWrt(SerialPort com, byte[] data, int bytelength)
        {
            com.Write(data, 0, bytelength);

            int i = 0;
            string mTempStr = "";
            for (i = 0; i < bytelength; i++)
            {
                mTempStr = mTempStr + " " + data[i].ToString("X2");
            }

            AddListMsg("串口发送数据:" + mTempStr);
        }

        //CRC校验 FROM Google 
        #region
        private static readonly byte[] aucCRCHi = {
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 
             0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40
         };
        private static readonly byte[] aucCRCLo = {
             0x00, 0xC0, 0xC1, 0x01, 0xC3, 0x03, 0x02, 0xC2, 0xC6, 0x06, 0x07, 0xC7,
             0x05, 0xC5, 0xC4, 0x04, 0xCC, 0x0C, 0x0D, 0xCD, 0x0F, 0xCF, 0xCE, 0x0E,
             0x0A, 0xCA, 0xCB, 0x0B, 0xC9, 0x09, 0x08, 0xC8, 0xD8, 0x18, 0x19, 0xD9,
             0x1B, 0xDB, 0xDA, 0x1A, 0x1E, 0xDE, 0xDF, 0x1F, 0xDD, 0x1D, 0x1C, 0xDC,
             0x14, 0xD4, 0xD5, 0x15, 0xD7, 0x17, 0x16, 0xD6, 0xD2, 0x12, 0x13, 0xD3,
             0x11, 0xD1, 0xD0, 0x10, 0xF0, 0x30, 0x31, 0xF1, 0x33, 0xF3, 0xF2, 0x32,
             0x36, 0xF6, 0xF7, 0x37, 0xF5, 0x35, 0x34, 0xF4, 0x3C, 0xFC, 0xFD, 0x3D,
             0xFF, 0x3F, 0x3E, 0xFE, 0xFA, 0x3A, 0x3B, 0xFB, 0x39, 0xF9, 0xF8, 0x38, 
             0x28, 0xE8, 0xE9, 0x29, 0xEB, 0x2B, 0x2A, 0xEA, 0xEE, 0x2E, 0x2F, 0xEF,
             0x2D, 0xED, 0xEC, 0x2C, 0xE4, 0x24, 0x25, 0xE5, 0x27, 0xE7, 0xE6, 0x26,
             0x22, 0xE2, 0xE3, 0x23, 0xE1, 0x21, 0x20, 0xE0, 0xA0, 0x60, 0x61, 0xA1,
             0x63, 0xA3, 0xA2, 0x62, 0x66, 0xA6, 0xA7, 0x67, 0xA5, 0x65, 0x64, 0xA4,
             0x6C, 0xAC, 0xAD, 0x6D, 0xAF, 0x6F, 0x6E, 0xAE, 0xAA, 0x6A, 0x6B, 0xAB, 
             0x69, 0xA9, 0xA8, 0x68, 0x78, 0xB8, 0xB9, 0x79, 0xBB, 0x7B, 0x7A, 0xBA,
             0xBE, 0x7E, 0x7F, 0xBF, 0x7D, 0xBD, 0xBC, 0x7C, 0xB4, 0x74, 0x75, 0xB5,
             0x77, 0xB7, 0xB6, 0x76, 0x72, 0xB2, 0xB3, 0x73, 0xB1, 0x71, 0x70, 0xB0,
             0x50, 0x90, 0x91, 0x51, 0x93, 0x53, 0x52, 0x92, 0x96, 0x56, 0x57, 0x97,
             0x55, 0x95, 0x94, 0x54, 0x9C, 0x5C, 0x5D, 0x9D, 0x5F, 0x9F, 0x9E, 0x5E,
             0x5A, 0x9A, 0x9B, 0x5B, 0x99, 0x59, 0x58, 0x98, 0x88, 0x48, 0x49, 0x89,
             0x4B, 0x8B, 0x8A, 0x4A, 0x4E, 0x8E, 0x8F, 0x4F, 0x8D, 0x4D, 0x4C, 0x8C,
             0x44, 0x84, 0x85, 0x45, 0x87, 0x47, 0x46, 0x86, 0x82, 0x42, 0x43, 0x83,
             0x41, 0x81, 0x80, 0x40
         };

        private void Crc16(byte[] pucFrame, int usLen, ref byte ucCRCHi, ref byte ucCRCLo)
        {
            int i = 0;
            ucCRCHi = 0xFF;
            ucCRCLo = 0xFF;
            UInt16 iIndex = 0x0000;

            while (usLen-- > 0)
            {
                iIndex = (UInt16)(ucCRCLo ^ pucFrame[i++]);
                ucCRCLo = (byte)(ucCRCHi ^ aucCRCHi[iIndex]);
                ucCRCHi = aucCRCLo[iIndex];
            }
        }
        #endregion

        private void button1_Click(object sender, System.EventArgs e)
        {

        }

        private void buttonHide_Click(object sender, System.EventArgs e)
        {
            AddListMsg("界面隐藏！");
            this.Hide();
        }

        private void buttonClearList_Click(object sender, System.EventArgs e)
        {
            AddListMsg("消息清理！");
            LogBox.Clear();
        }

        public void ReInitTCPClient_AGV_Click(object sender, EventArgs e)
        {
            //if (tcpRFID.Connected == false)
            //    fInitRFID();
        }


        private void AGVWork_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void FormAGV_COM_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                axActUtlType1.Close();

            }
            catch (Exception err)
            {
                MessageBox.Show("断开三菱PLC失败！");
            }
            //  AddListMsg("AGV释放退出");


            //          tcpclientB.Close();
        }

        private void buttonOutStart_Click(object sender, EventArgs e)
        {

        }

        private void buttonManualFinished_Click(object sender, EventArgs e)
        {
            axActUtlType1.SetDevice2("M1216", 0);
            axActUtlType1.SetDevice2("M1217", 0);
            Thread.Sleep(10);
            axActUtlType1.SetDevice2("M0", 0);
            axActUtlType1.SetDevice2("M50", 0);
            axActUtlType1.SetDevice2("M850", 0);
            axActUtlType1.SetDevice2("M900", 0);

            axActUtlType1.SetDevice2("M1754", 0);
            axActUtlType1.SetDevice2("M1756", 0);
            m_ucLoadAGVStep = 20;
            m_AGVHandShakeState = 0;
            AddListMsg("手动强制对接完成");
        }




        bool LongLoadIn1Safe = false;
        public bool LongLoadIn1OutStretch = false;



        bool LongLoadIn2Safe = false;
        public bool LongLoadIn2OutStretch = false;



        bool LongLoadOut1Safe = false;
        public bool LongLoadOut1OutStretch = false;



        bool LongLoadOut2Safe = false;
        public bool LongLoadOut2OutStretch = false;





        bool AGVAllowIn = false;   //对接允许信号
        bool AGVAllowOut = false;   //对接允许信号



        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (LED_PLC.Value)
                {

                    cfg_MES_Agv.WriteConfig("MES", "vs_LoadCasNUm", FormAGV.LoadTrackInTotal);
                    cfg_MES_Agv.WriteConfig("MES", "vs_UnLoadCasNUm", FormAGV.LoadTrackOutTotal);

                    if (checkBoxNoUpper.Checked)
                    {
                        cfg_MES_Agv.WriteConfig("MES", "vs_LoadAGVDisable", true);
                    }
                    else
                    {
                        cfg_MES_Agv.WriteConfig("MES", "vs_LoadAGVDisable", false);
                    }

                    if (checkBoxNoLower.Checked)
                    {
                        cfg_MES_Agv.WriteConfig("MES", "vs_UnLoadAGVDisable", true);
                    }
                    else
                    {
                        cfg_MES_Agv.WriteConfig("MES", "vs_UnLoadAGVDisable", false);
                    }



                    Int16 AGVAlarmRet = 0;
                    axActUtlType1.ReadDeviceRandom2("M1590\n", 1, out AGVAlarmRet);//出料右对接允许
                    if (AGVAlarmRet == 1) agvAlarmFlag = true; else agvAlarmFlag = false;



                    ledSyl_In1.Value = LongLoadIn1OutStretch;
                    ledSyl_In2.Value = LongLoadIn2OutStretch;

                    OutLedUp_3.Value = LongLoadOut1OutStretch;
                    OutLedUp_4.Value = LongLoadOut2OutStretch;


                    int[] ReadValue1 = new int[100];
                    int returncode = 10;
                    try
                    {
                        int iReturn = axActUtlType1.ReadDeviceBlock("D1000", 50, out ReadValue1[0]);
                    }
                    catch (Exception err)
                    {
                        returncode = 10;
                    }

                    int RetD1033 = ReadValue1[33];
                    if ((RetD1033 & 0x0001) == 0x0001) LongLoadIn1OutStretch = true; else LongLoadIn1OutStretch = false;
                    if ((RetD1033 & 0x0002) == 0x0002) LongLoadIn2OutStretch = true; else LongLoadIn2OutStretch = false;

                    //if ((RetD1033 & 0x0004) == 0x0004) LongLoadIn1OutStretch = true; else LongLoadIn1OutStretch = false;
                    //if ((RetD1033 & 0x0008) == 0x0008) LongLoadIn2OutStretch = true; else LongLoadIn2OutStretch = false;

                    if ((RetD1033 & 0x0010) == 0x0010) LongLoadOut1OutStretch = true; else LongLoadOut1OutStretch = false;
                    if ((RetD1033 & 0x0020) == 0x0020) LongLoadOut2OutStretch = true; else LongLoadOut2OutStretch = false;
                    //if ((RetD1033 & 0x0040) == 0x0040) LongLoadOut1OutStretch = true; else LongLoadOut1OutStretch = false;
                    //if ((RetD1033 & 0x0080) == 0x0080) LongLoadOut2OutStretch = true; else LongLoadOut2OutStretch = false;


                    if ((RetD1033 & 0x0100) == 0x0100) LongLoadIn1Safe = true; else LongLoadIn1Safe = false;
                    if ((RetD1033 & 0x0200) == 0x0200) LongLoadIn2Safe = true; else LongLoadIn2Safe = false;

                    //if ((RetD1033 & 0x0400) == 0x0400) LongLoadIn1Safe = true; else LongLoadIn1Safe = false;
                    //if ((RetD1033 & 0x0800) == 0x0800) LongLoadIn2Safe = true; else LongLoadIn2Safe = false;

                    if ((RetD1033 & 0x1000) == 0x1000) LongLoadOut1Safe = true; else LongLoadOut1Safe = false;
                    if ((RetD1033 & 0x2000) == 0x2000) LongLoadOut2Safe = true; else LongLoadOut2Safe = false;

                    //if ((RetD1033 & 0x4000) == 0x4000) LongLoadOut1Safe = true; else LongLoadOut1Safe = false;
                    //if ((RetD1033 & 0x8000) == 0x8000) LongLoadOut2Safe = true; else LongLoadOut2Safe = false;


                    int RetD1034 = ReadValue1[34];
                    if ((RetD1034 & 0x1000) == 0x1000) AGVAllowIn = true; else AGVAllowIn = false; //A进
                    // if ((RetD1034 & 0x2000) == 0x2000) AGVAllowIn = true; else AGVAllowIn = false; //B进
                    if ((RetD1034 & 0x4000) == 0x4000) AGVAllowOut = true; else AGVAllowOut = false;//A出
                    //  if ((RetD1034 & 0x8000) == 0x8000) AGVAllowOut = true; else AGVAllowOut = false;//B出


                    int RetD1035 = ReadValue1[36];
                    if ((RetD1035 & 0x1000) == 0x1000) agvWorkFlagIn = true; else agvWorkFlagIn = false;
                    // if ((RetD1035 & 0x2000) == 0x2000) agvWorkFlagIn = true; else agvWorkFlagIn = false;
                    if ((RetD1035 & 0x4000) == 0x4000) agvWorkFlagOut = true; else agvWorkFlagOut = false;
                    // if ((RetD1035 & 0x8000) == 0x8000) agvWorkFlagOut = true; else agvWorkFlagOut = false;



                    LED_SafeIn1.Value = LongLoadIn1Safe; //Slave_EL1889[5,4]	;	(*	I4.4	进料1#AGV过渡检测  *)
                    LED_SafeIn2.Value = LongLoadIn2Safe; //Slave_EL1889[5,5]	;	(*	I4.5	进料2#AGV过渡检测  *)
                    LED_SafeOut1.Value = LongLoadOut1Safe; //Slave_EL1889[5,6]	;	(*	I4.6	出料2#AGV过渡检测  *)
                    LED_SafeOut2.Value = LongLoadOut2Safe; //Slave_EL1889[5,7]	;	(*	I4.7	出料3#AGV过渡检测   *)


                    FormAGV.LoadTrackIn1 = ReadValue1[25];//左下
                    FormAGV.LoadTrackIn2 = ReadValue1[26];//右下
                    FormAGV.LoadTrackOut1 = ReadValue1[27];//左shang
                    FormAGV.LoadTrackOut2 = ReadValue1[28];//右上

                    //LoadTrackIn1 = ReadValue1[29];//左下
                    //LoadTrackIn2 = ReadValue1[30];//右下
                    //LoadTrackOut1 = ReadValue1[31];//左shang
                    //LoadTrackOut2 = ReadValue1[32];//右上



                    FormAGV.LoadTrackInTotal = FormAGV.LoadTrackIn1 + FormAGV.LoadTrackIn2;
                    FormAGV.LoadTrackOutTotal = FormAGV.LoadTrackOut1 + FormAGV.LoadTrackOut2;

                    textBoxIN_num1.Text = FormAGV.LoadTrackIn1.ToString();
                    textBoxIN_num2.Text = FormAGV.LoadTrackIn2.ToString();

                    textBoxBout_num1.Text = FormAGV.LoadTrackOut1.ToString();
                    textBoxBout_num2.Text = FormAGV.LoadTrackOut2.ToString();

                    cfg_MES_Agv.WriteConfig("MES", "eap_cascount_1_1", FormAGV.LoadTrackIn1);
                    cfg_MES_Agv.WriteConfig("MES", "eap_cascount_1_2", FormAGV.LoadTrackIn2);
                    cfg_MES_Agv.WriteConfig("MES", "eap_cascount_1_3", FormAGV.LoadTrackOut1);
                    cfg_MES_Agv.WriteConfig("MES", "eap_cascount_1_4", FormAGV.LoadTrackOut2);
                }
            }
            catch (Exception ex)
            {
                AddListMsg("左侧AGV IO扫描异常：" + ex.Message);
            }


        }

        private void buttonOut1_Click(object sender, EventArgs e)
        {
            //formHmi.fWriteToPlc("stOutput.Out1MotorNeg", true);
            //    formHmi.WriteToPlc_Bool(".Feed_Ctrl_HMI[10]", true);

        }

        private void buttonOut2_Click(object sender, EventArgs e)
        {

        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            AddListMsg("重新初始化串口COM19!");
            fInitialCOM();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (ComPortAGV.IsOpen)
            {
                AddListMsg("关闭串口COM519!");
                ComPortAGV.Close();
            }
        }

        private void buttonStartAll_Click(object sender, EventArgs e)
        {


        }

        private void checkBoxNoUpper_CheckedChanged(object sender, EventArgs e)
        {
            //定时器里面去判断勾选
            //if (checkBoxNoUpper.Checked)
            //{
            //    cfg_MES_Agv.WriteConfig("MES", "vs_LoadAGVDisable", 1);
            //}
            //else
            //{
            //    cfg_MES_Agv.WriteConfig("MES", "vs_LoadAGVDisable", 0);
            //}
        }

        private void checkBoxNoLower_CheckedChanged(object sender, EventArgs e)
        {
            //if (checkBoxNoLower.Checked)
            //{
            //    cfg_MES_Agv.WriteConfig("MES", "vs_UnLoadAGVDisable", 1);
            //}
            //else
            //{
            //    cfg_MES_Agv.WriteConfig("MES", "vs_UnLoadAGVDisable", 0);
            //}
        }





        private void KEY_OutSyc1_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {
            //if (KEY_OutSyc1.Value == true)
            //{
            //    axActUtlType1.SetDevice2("M2016", 1); ;//进料气缸左伸出
            //}
            //else if (KEY_OutSyc1.Value == false)
            //{
            //    axActUtlType1.SetDevice2("M2016",0); ;//进料气缸左伸出
            //}
        }

        private void KEY_OutSyc2_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {
            //if (KEY_OutSyc2.Value == true)
            //{
            //    axActUtlType1.SetDevice2("M2017", 1); ;//进料气缸左伸出
            //}
            //else if (KEY_OutSyc2.Value == false)
            //{
            //    axActUtlType1.SetDevice2("M2017", 0); ;//进料气缸左伸出
            //}
        }

        void ConnectToPLC()
        {
            Task.Run(() =>
             {
                 //初始化PLC连接
                 try
                 {
                     axActUtlType1.ActLogicalStationNumber = 2;
                     int ret = axActUtlType1.Open();
                     if (ret == 0)
                     {
                         LED_PLC.Value = true;
                     }
                     else
                     {
                         LED_PLC.Value = false;
                     }
                 }
                 catch (Exception ex)
                 {
                     LED_PLC.Value = false;
                     AddListMsg("自动重连接驳台PLC失败：" + ex.Message);
                 }
             });
        }

        private void timerConnect_Tick(object sender, EventArgs e)
        {
            if (!LED_PLC.Value)
            {
                ConnectToPLC();

            }


        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {

            if (!LED_PLC.Value)
            {
                ConnectToPLC();

            }
        }

        private void checkBoxNOagv_CheckedChanged(object sender, EventArgs e)
        {
            //if (checkBoxNOagv.Checked)
            //{
            //    cfg_MES_Agv.WriteConfig("MES", "eap_portstatus_1", 0);
            //}
            //else
            //{
            //    cfg_MES_Agv.WriteConfig("MES", "eap_portstatus_1", 1);
            //}
        }





        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void FormAGV_COM_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                axActUtlType1.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show("断开三菱PLC失败！");
            }
        }

        private void ComHandlerTimer_Tick(object sender, EventArgs e)
        {
            ComDataHandler();
        }

        private void LogTimer_Tick(object sender, EventArgs e)
        {
            LogHandler();
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void LED_PLC_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {

        }

        private void axActUtlType1_OnDeviceStatus(object sender, AxActUtlTypeLib._IActUtlTypeEvents_OnDeviceStatusEvent e)
        {

        }
    }
}
