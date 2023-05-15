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
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using EasyLog;
using TypeConvertHelper;
using Sunny.UI;
using DAL;
using Entity;

namespace MesOpcServer
{
    public partial class FormSygoleRFID : UIForm
    {
        Log _log;
        public static FormSygoleRFID formSygoleRFID;
        //实例化到RFID网关的连接
        EnService MyEnService = new EnService("192.168.1.10", "502");

        OpcUaReadWrite OpcReadWrite;

        public static string[] g_UID = new string[4];
        public static bool[] g_RFIDStatus = new bool[4];
        public static bool[] g_RFIDTagOnline = new bool[4];

        public FormSygoleRFID()
        {

            InitializeComponent();

            _log = Log.getInstance();

            WriteLog("RFID模块加载......");
            //开启检查和更新处理定时器
            CheckAndUpdateTimer.Enabled = false;

           // OpcReadWrite = OpcUaReadWrite.getInstance();
        }


        private void btnConnect_Click(object sender, EventArgs e)
        {

            Connect();

        }

        private void btnDisConnect_Click(object sender, EventArgs e)
        {
            DisConnect();

        }


        public void DisposeRFID()
        {
            DisConnect();
            Environment.Exit(0);
        }

        private void CheckAndUpdateTimer_Tick(object sender, EventArgs e)
        {



        }

        private void FormSygoleRFID_Load(object sender, EventArgs e)
        {
            //连接RFID
            WriteLog("RFID连接中...");
            Connect();

        }

        public void Connect()
        {
            Task.Run(() =>
                {

                    if (!MyEnService.IsConnect())
                    {

                        MyEnService.Connect();

                        if (MyEnService.IsConnect())
                        {
                            bool ret0 = MyEnService.EnableReader(Reader.RF0, true);
                            bool ret1 = MyEnService.EnableReader(Reader.RF1, true);

                            LED_State.Value = true;

                            WriteLog("RFID网关连线成功！");

                            if (ret0)
                            {
                                WriteLog("上料读头使能成功！");
                            }
                            else
                            {
                                WriteLog("上料读头使能失败！");
                            }

                            if (ret1)
                            {
                                WriteLog("下料读头使能成功！");
                            }
                            else
                            {
                                WriteLog("下料读头使能失败！");
                            }
                        }
                        else
                        {
                            LED_State.Value = false;
                            WriteLog("RFID网关连线失败！");
                        }
                    }
                    else
                    {
                        LED_State.Value = true;
                        WriteLog("RFID网关当前为连线状态！无需重连！");
                    }


                });

        }

        public void DisConnect()
        {
            try
            {
                MyEnService.DisConnect();
                if (!MyEnService.IsConnect())
                {
                    LED_State.Value = false;
                }
                else
                {
                    LED_State.Value = true;
                }
            }
            catch (Exception ex)
            {
                WriteLog("RFID断开连线函数执行错误：" + ex.Message);
            }

        }

        private void FormSygoleRFID_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

        }

        // 重写OnClosing使点击关闭按键时窗体能够缩进托盘

        protected override void OnClosing(CancelEventArgs e)
        {

            this.Hide();

            e.Cancel = true;

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            LogBox.Clear();
        }


        public void WriteLog(string msg)
        {
            try
            {
                if (LogBox.IsHandleCreated)
                {
                    string str_msg = DateTime.Now.ToString("HH:mm:ss.fff  ") + MESServer.SoftVersion + msg + "\n";

                    _log.Write(MESServer.SoftVersion + msg, MsgType.RFID);

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
            catch
            {
            }
        }

        private void btnReadRFID_Click(object sender, EventArgs e)
        {
            ReadLoadRFID();
        }

        private void btnReadUnLoadRFID_Click(object sender, EventArgs e)
        {
            ReadUnLoadRFID();
        }

        public string ReadLoadRFID()
        {
            try
            {
                if (!MyEnService.IsConnect())
                {
                    WriteLog("读取前检测到RFID网关离线！尝试自动重连！");
                    Connect();
                }
                //RFID读数据转成字符串
                if (MyEnService.IsConnect())
                {
                    ReadResult MyReadResult = MyEnService.ReadTagUsers(Reader.RF0, 0, 20);
                    WriteLog("上料ReadTagUsers返回：0x" + MyReadResult.ErrCode.ToString("X2") + ", 命令状态：" + MyReadResult.sataus.DisplayText());
                    string strLoadRFID = TypeConvert.ByteArrayToString(MyReadResult.TagUser.ByteArray, 0, MyReadResult.TagUser.ByteArray.Length).TrimEnd(new char[] { '\0', '\r', '\n' });
                    if (String.IsNullOrWhiteSpace(strLoadRFID))
                    {
                        strLoadRFID = "NULL";
                    }

                    LoadRFID.Text = strLoadRFID;
                    MESServer._cfg.WriteConfig("MES", "vs_LoadRFID", strLoadRFID);
                    OpcReadWrite.Write("ns=2;s=DRLaser/vs_LoadRFID", strLoadRFID);
                    return strLoadRFID;
                }
                else
                {
                    WriteLog("RFID网关离线！上料RFID读取失败！");
                    LoadRFID.Text = "FAIL";
                    MESServer._cfg.WriteConfig("MES", "vs_LoadRFID", "FAIL");
                    OpcReadWrite.Write("ns=2;s=DRLaser/vs_LoadRFID", "FAIL");
                    return "FAIL";
                }
            }
            catch
            {
                LoadRFID.Text = "EXCEPTION";
                MESServer._cfg.WriteConfig("MES", "vs_LoadRFID", "EXCEPTION");
                OpcReadWrite.Write("ns=2;s=DRLaser/vs_LoadRFID", "EXCEPTION");
                return "EXCEPTION";
            }
        }

        public string ReadUnLoadRFID()
        {
            try
            {
                if (!MyEnService.IsConnect())
                {
                    WriteLog("读取前检测到RFID网关离线！尝试自动重连！");
                    Connect();
                }

                if (MyEnService.IsConnect())
                {
                    ReadResult MyReadResult = MyEnService.ReadTagUsers(Reader.RF1, 0, 20);
                    WriteLog("下料ReadTagUsers返回：0x" + MyReadResult.ErrCode.ToString("X2") + ", 命令状态：" + MyReadResult.sataus.DisplayText());
                    string strUnLoadRFID = TypeConvert.ByteArrayToString(MyReadResult.TagUser.ByteArray, 0, MyReadResult.TagUser.ByteArray.Length).TrimEnd(new char[] { '\0', '\r', '\n' });

                    if (String.IsNullOrWhiteSpace(strUnLoadRFID))
                    {
                        strUnLoadRFID = "NULL";
                    }

                    UnLoadRFID.Text = strUnLoadRFID;
                    MESServer._cfg.WriteConfig("MES", "vs_UnLoadRFID", strUnLoadRFID);
                    OpcReadWrite.Write("ns=2;s=DRLaser/vs_UnLoadRFID", strUnLoadRFID);
                    return strUnLoadRFID;
                }
                else
                {
                    WriteLog("RFID网关离线！下料RFID读取失败！");
                    UnLoadRFID.Text = "FAIL";
                    MESServer._cfg.WriteConfig("MES", "vs_UnLoadRFID", "FAIL");
                    OpcReadWrite.Write("ns=2;s=DRLaser/vs_UnLoadRFID", "FAIL");
                    return "FAIL";
                }
            }
            catch
            {
                UnLoadRFID.Text = "EXCEPTION";
                MESServer._cfg.WriteConfig("MES", "vs_UnLoadRFID", "EXCEPTION");
                OpcReadWrite.Write("ns=2;s=DRLaser/vs_UnLoadRFID", "EXCEPTION");
                return "EXCEPTION";
            }
        }

        private void LoadRFIDDisable_CheckedChanged(object sender, EventArgs e)
        {
            MESServer._cfg.WriteConfig("MES", "vs_LoadRFIDDisable", LoadRFIDDisable.Checked);
            OpcReadWrite.Write("ns=2;s=DRLaser/vs_LoadRFIDDisable", LoadRFIDDisable.Checked);
        }

        private void 清除内容ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogBox.Clear();
        }
    }
}
