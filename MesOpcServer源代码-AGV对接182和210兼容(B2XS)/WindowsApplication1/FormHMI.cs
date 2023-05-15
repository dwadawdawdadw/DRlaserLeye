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
using NationalInstruments.Controls;
using NationalInstruments.Controls.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Advantech.Motion;
using System.IO.Ports;
using System.IO;
using System.Runtime.InteropServices;
using MicroLibrary;
using System.Threading;
using TPM;
using SGModbus;
using DRMotion;
///////////////////////////////////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////版本说明////////////////////////
////////////////V2.0.1:
//1.增加上料接驳台连续3pcs没有检测到硅片报警功能。
//2.增加上料接驳台延时2*16ms等待硅片判断后，在下压接驳台模组
//3.增加接驳台升降模组alarm报警提示
namespace WindowsApplication1
{
    public partial class FormHMI : Form
    {

        string file_nameIni = "D:\\DRLaser\\Laser.ini";
        public FormHMI()
        {
            StreamReader read = new StreamReader(file_nameIni, Encoding.Default);
            string strReadline = null;
            strReadline = read.ReadLine();
            g_IniLaser = Convert.ToInt16(strReadline);
            read.Close();
            if (g_IniLaser == 0)
            {
                StreamWriter write = new StreamWriter(file_nameIni);
                write.WriteLine(1);
                write.Close();
                InitializeComponent();

                /////Markingmate初始化///////////////
                axMMMark1.Initial();
                axMMEdit1.Initial();
                axMMIO1.Initial();
                axMMStatus1.Initial();
                axMMIO1.SetOutput(15, 0); //打开Gate 
                // axMMEdit1.ThermalDrift_Enable(1, 1);
                //axMMMark1.LaserOff();
                //axMMMark1.LaserOff();
                //////////CCD初始化//////////////
                axCKVisionCtrl1.LoadConfigure(file_name1_Acti);
                axCKVisionCtrl1.Redraw();

                /*  m_FirstProc = axCkvsRunCtrl1.GetFirstProc();
                  ReturnTool = axCkvsRunCtrl1.GetFirstTool(m_FirstProc);
                  while (ReturnTool != 0)
                  {
                      stoolname = axCkvsRunCtrl1.GetToolName(ReturnTool);
                      comboBoxCCD.Items.Insert(i, stoolname);
                      i++;
                      ReturnTool = axCkvsRunCtrl1.GetNextTool(ReturnTool);
                  }   */
                //初始化设备
                fInitFile(); //txt参数文档读出
                fSysParamAssignment();//参数赋值
                fInitMainBoard();
                fInitLoadBoard();
                fInitialCOM();
                // fInitModbus();
                fInitRFID();
                setTimers();//设置多媒体定时器
                mcTimer.Start();//启动多媒体定时器
                m_ErrorFlag = 2;
                m_HomeFinished = 0;
                timerBuzzer.Enabled = true;
                KEY_StartMeasure.Enabled = true;
                ////////////////////
                // fOpenAlarmFile();
                //fInitFile(); //txt参数文档读出
                //fSysParamAssignment();//参数赋值
                fOffsetIni();//补偿设置  
                Thread_T1 = new Thread(new ThreadStart(fAutoCCDRead));
                Thread_T1.Start();
                // Thread_T2 = new Thread(new ThreadStart(fLoadWaferCheck));
                // Thread_T2.Start();

                Thread_T2 = new Thread(new ThreadStart(fCheckIO));
                Thread_T2.Start();

                m_DoorProtect = false; //屏蔽安全门勾选
            }
            else
            {
                MessageBox.Show(this, "自动化控制软件已经被打开，不能重复打开软件!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Environment.Exit(0);//系统完全退出
            }
        }
        // FileStream ParamFile = new FileStream("D:\\DRLaser\\SysPara.ini",FileMode.Open);

        SerialPort ComPower = new SerialPort();//定义串口对象
        /*********************************************************************************************************
       ** 函数名称 ：fInitFile
       ** 函数功能 ：文件读取
       ** 入口参数 ：
       ** 出口参数 ： 
       *********************************************************************************************************/
        void fInitFile()
        {
            int i = 0;
            StreamReader read = new StreamReader("D:\\DRLaser\\SysPara.ini", Encoding.Default);
            string strReadline = null;
            // while ((strReadline = read.ReadLine()) != null)
            //  {
            //  g_SysParam[i] = strReadline;
            // }
            for (i = 0; i < 80; i++)
            {
                strReadline = read.ReadLine();
                g_SysParam[i] = Convert.ToDouble(strReadline);

            }
            read.Close();
        }
        /*********************************************************************************************************
         ** 函数名称 ：setTimers
         ** 函数功能 ：多媒体定时器设置 
         ** 修改时间 ：20180523
         ** 修改内容 ：
         *********************************************************************************************************/
        void setTimers()
        {
            mcTimer = new MicroTimer(5000);
            mcTimer.MicroTimerElapsed += new MicroTimer.MicroTimerElapsedEventHandler(OnTimedEvent);
        }
        /*********************************************************************************************************
        ** 函数名称 ：OnTimedEvent
        ** 函数功能 ：多媒体定时器中断事件 
        ** 修改时间 ：20180523
        ** 修改内容 ：
        *********************************************************************************************************/
        private void OnTimedEvent(object sender, MicroTimerEventArgs timerEventArgs)
        {
            //fCheckIOStatus();//IO扫描 
            fCTShow();
        }
        /*********************************************************************************************************
        ** 函数名称 ：fCTShow
        ** 函数功能 ：CT计算及显示
        ** 修改时间 ：20180523
        ** 修改内容 ：
        *********************************************************************************************************/
        void fCTShow()
        { //CT显示
            if (m_CTFlag)  //CT显示
            {
                m_CT++;
            }
            else
            {
                if ((m_CT < 500) && (m_CT > 0))
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        CT.Value = (double)m_CT / 200;
                        Capacity.Value = 3600 / CT.Value;
                    }));

                }
                m_CT = 0;
            }

            if (!m_CTFlag)  //CT显示
            {
                m_CT1++;
            }
            else
            {
                if ((m_CT1 < 500) && (m_CT1 > 0))
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        CT.Value = (double)m_CT1 / 200;
                        Capacity.Value = 3600 / CT.Value;
                    }));

                }
                m_CT1 = 0;
            }
        }
        /*********************************************************************************************************
      ** 函数名称 ：fSysParamAssignment
      ** 函数功能 ：参数赋值
      ** 入口参数 ：
      ** 出口参数 ： 
      *********************************************************************************************************/
        public void fSysParamAssignment()
        {
            g_LoadElevationP = (int)g_SysParam[0];
            //g_UnLoadElevationP = (int)g_SysParam[1]; //g_CWPositionFlag = Convert.ToUInt32(g_SysParam[2]);
            m_Load1220XElevatorStart = g_SysParam[3]; 	//上料升降模组开始位置
            //m_UnLoad1220XElevatorStart = g_SysParam[4];   //下料升降模组开始位置
            m_LoadStepInv = g_SysParam[5]; 	 //上料花篮齿间距
            // = g_SysParam[6];  // 下料花篮齿间距
            m_CCDBaseX = g_SysParam[7]; 		//CCD基准点X
            m_CCDBaseY = g_SysParam[8];  		//CCD基准点Y
            //m_UnLoadConstDistance =Convert.ToInt16( g_SysParam[9]);
            m_OffsetDistanceTwoAxis = g_SysParam[10];	  	//主机进料2归零补偿  
            //m_UnLoadMotorOffset = g_SysParam[12];  				//下料接驳台舌头传送位置
            m_CCDNGArea = g_SysParam[13];  				//CCD图像NG阀值 
            m_BufferInOffset = g_SysParam[14];	  		//进料Buffer归零补偿
            m_RotateInOffset = g_SysParam[15];	  		//旋转臂归零补偿  
            m_RotateOutOffset = g_SysParam[16];	  			//旋转臂归零补偿  
            m_PowerInv = Convert.ToUInt32(g_SysParam[17]);	  		//功率测量间隔
            m_PowerMin = g_SysParam[18];	  		//下限 
            m_PowerMax = g_SysParam[19];	  		//上限
            m_Load1220XElevatorIn = g_SysParam[20];
            m_Load1220XElevatorOut = g_SysParam[21];
            //m_UnLoad1220XElevatorIn = g_SysParam[22];
            // = g_SysParam[23];
            m_InMotorOffset = Convert.ToUInt32(g_SysParam[26]);
            m_OutMotorOffset = Convert.ToUInt32(g_SysParam[27]);
            m_LoadWaferConst = Convert.ToUInt32(g_SysParam[28]);
            m_LoadWaferBigInv = g_SysParam[29];
            // m_UnLoadWaferConst = Convert.ToUInt32(g_SysParam[30]);
            //m_UnLoadWaferBigInv = g_SysParam[31];
            m_BufferOutOffset = g_SysParam[32];
            m_PowerRatio = g_SysParam[33];
            m_CCDBaseAng = g_SysParam[34];
            m_LoadMotorOffset = g_SysParam[35];//上料接驳台舌头定长
            m_CCDWaferStand = g_SysParam[36];
            m_CCDShallStand = g_SysParam[37];
            m_FirstWaferPos = g_SysParam[38];
            //m_UnLoadEqu=g_SysParam[39];  //下料接驳台齐平位置 
            m_TotalQulity = Convert.ToUInt32(g_SysParam[40]);
            m_CCDStandX = g_SysParam[41];		//标准值
            m_CCDStandY = g_SysParam[42];
            m_CCDdelay = Convert.ToUInt32(g_SysParam[46]);//CCD延时
            m_OutReleaseDelay = Convert.ToUInt32(g_SysParam[48]);//下料反吹时间
            m_InReleaseDelay = Convert.ToUInt32(g_SysParam[49]);//上料反吹时间
            m_CCDAlarmValue = g_SysParam[47];//CCD报警值
            m_CCDAlarmAngleValue = g_SysParam[51];//CCD角度报警值
            m_LoadCasstteStatus = Convert.ToUInt32(g_SysParam[43]); //Casstte Status			  
            m_Broken = Convert.ToUInt32(g_SysParam[52]);
            m_TooSkew = Convert.ToUInt32(g_SysParam[53]);
            m_Broken = Convert.ToUInt32(g_SysParam[57]);
            m_TooSkew = Convert.ToUInt32(g_SysParam[58]);
            m_BoxKeyInDelay = Convert.ToUInt32(FormHMI.g_SysParam[67]);
            g_InBufferTotal = Convert.ToInt32(FormHMI.g_SysParam[68]);
            m_MainIn2 = Convert.ToUInt32(g_SysParam[69]);//进料模组2是否有料
            m_DisableUnLoadRotateCylinder = Convert.ToUInt32(g_SysParam[70]);
            m_MAccDecc = g_SysParam[71];
            if (m_MAccDecc == 0)
            {
                m_MAccDecc = 8;
            }

            m_LongOutDelay = Convert.ToUInt32(FormHMI.g_SysParam[72]);
            if (m_LongOutDelay == 0)
            {
                m_LongOutDelay = 850;
            }
            m_DustCrack = Convert.ToUInt32(g_SysParam[73]);
            g_MotorAlign = Convert.ToUInt32(g_SysParam[74]);
            g_CyAlign = Convert.ToUInt32(g_SysParam[75]);
            m_AlignPos = FormHMI.g_SysParam[76];    //花篮打齐位置
            m_RotateCylindDelay = FormHMI.g_SysParam[77];  //旋转气缸延时

            g_TheoryPix[0] = FormHMI.g_SysParam[59];//理论值
            g_TheoryPix[1] = FormHMI.g_SysParam[60];
            g_TheoryPix[2] = FormHMI.g_SysParam[61];
            g_TheoryPix[3] = FormHMI.g_SysParam[62];
            g_TheoryPix[4] = FormHMI.g_SysParam[63];
            g_TheoryPix[5] = FormHMI.g_SysParam[64];
            g_TheoryPix[6] = FormHMI.g_SysParam[65];
            g_TheoryPix[7] = FormHMI.g_SysParam[66];

            // m_AIN2 = Convert.ToUInt32(g_SysParam[45]);//进料模组2是否有料
            g_IN55 = (m_LoadCasstteStatus >> 4) & 0x01;  //上料KK模组有无花篮检测
            g_IN54 = (m_LoadCasstteStatus >> 3) & 0x01; //上料花篮正反检测
            g_IN53 = (m_LoadCasstteStatus >> 2) & 0x01;
            g_IN52 = (m_LoadCasstteStatus >> 1) & 0x01;
            g_IN51 = m_LoadCasstteStatus & 0x01;


            TotalQulity.Value = m_TotalQulity;
            Broken.Value = m_Broken;
            TooSkew.Value = m_TooSkew;
            //g_OUT07 = (m_UnLoadCasstteStatus >> 3) & 0x01; 
            //g_OUT06 = (m_UnLoadCasstteStatus >> 2) & 0x01; 
            //g_OUT04 = (m_UnLoadCasstteStatus >> 1) & 0x01; 
            //g_OUT03 = m_UnLoadCasstteStatus & 0x01;

        }

        /*********************************************************************************************************
       ** 函数名称 ：fOffsetIni
       ** 函数功能 ：补偿初始化
       ** 入口参数 ：
       ** 出口参数 ： 
       *********************************************************************************************************/
        void fOffsetIni()
        {
            double TempDistanceOneAxis, TempDistanceTwoAxis, TempDistanceThreeAxis;
            double TempBufferInOffset, TempInRotateOffset, TempOutRotateOffset, TempDistanceUAxis;
            TempDistanceOneAxis = m_InMotorOffset * pci1245l.WaferRatio;
            TempDistanceTwoAxis = m_OffsetDistanceTwoAxis * pci1245l.WaferRatio;
            TempDistanceThreeAxis = m_OutMotorOffset * pci1245l.WaferRatio;
            TempBufferInOffset = m_BufferInOffset * pci1245l.BufferRatio;
            TempInRotateOffset = m_RotateInOffset * pci1245l.RotateRatio;
            TempOutRotateOffset = m_RotateOutOffset * pci1245l.RotateRatio;
            TempDistanceUAxis = m_BufferOutOffset * pci1245l.DDRatio;

            // Motion.mAcm_SetU32Property(ax1Handle[pci1245l.Axis_ONE], (uint)PropertyID.CFG_AxElLogic, 1); 
            Motion.mAcm_SetU32Property(ax1Handle[pci1245l.Axis_ONE], (uint)PropertyID.CFG_AxIN2StopEdge, 0);
            Motion.mAcm_SetF64Property(ax1Handle[pci1245l.Axis_ONE], (uint)PropertyID.CFG_AxIN2StopReact, 1);
            Motion.mAcm_SetF64Property(ax1Handle[pci1245l.Axis_ONE], (uint)PropertyID.CFG_AxIN2OffsetValue, TempDistanceOneAxis);
            Motion.mAcm_SetU32Property(ax1Handle[pci1245l.Axis_TWO], (uint)PropertyID.CFG_AxIN2StopEdge, 0);
            Motion.mAcm_SetF64Property(ax1Handle[pci1245l.Axis_TWO], (uint)PropertyID.CFG_AxIN2StopReact, 1);
            Motion.mAcm_SetF64Property(ax1Handle[pci1245l.Axis_TWO], (uint)PropertyID.CFG_AxIN2OffsetValue, TempDistanceTwoAxis);

            //Motion.mAcm_SetU32Property(ax1Handle[pci1245l.Axis_THREE], (uint)PropertyID.CFG_AxIN1StopEdge, 0);
            //Motion.mAcm_SetF64Property(ax1Handle[pci1245l.Axis_THREE], (uint)PropertyID.CFG_AxIN1StopReact, 1);
            //Motion.mAcm_SetF64Property(ax1Handle[pci1245l.Axis_THREE], (uint)PropertyID.CFG_AxIN1OffsetValue, TempDistanceThreeAxis);

            //Motion.mAcm_SetU32Property(ax1Handle[pci1245l.Axis_THREE], (uint)PropertyID.CFG_AxIN2StopEdge, 0);//
            //Motion.mAcm_SetF64Property(ax1Handle[pci1245l.Axis_THREE], (uint)PropertyID.CFG_AxIN2StopReact, 1);
            //Motion.mAcm_SetF64Property(ax1Handle[pci1245l.Axis_THREE], (uint)PropertyID.CFG_AxIN2OffsetValue, TempDistanceThreeAxis);

            Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_ONE], (uint)PropertyID.CFG_AxHomeOffsetDistance, TempBufferInOffset);
            Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_TWO], (uint)PropertyID.CFG_AxHomeOffsetDistance, TempInRotateOffset);
            Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_THREE], (uint)PropertyID.CFG_AxHomeOffsetDistance, TempOutRotateOffset);
            Motion.mAcm_SetF64Property(ax1Handle[pci1245l.Axis_FOUR], (uint)PropertyID.CFG_AxHomeOffsetDistance, TempDistanceUAxis);
            /////下料接驳台补偿
            ////DD初始化
            DDInit = true;
            fDDPositionStatus(0);
            DDInit = false;
        }
        /*********************************************************************************************************
         ** 函数名称 ：fInitMainBoard
         ** 函数功能 ：主机板卡初始化
         ** 入口参数 ：
         ** 出口参数 ： 
         *********************************************************************************************************/
        int fInitMainBoard()
        {
            int errcde;
            ushort i;
            uint devCnt = 0, devNum = 0, Rerrcde;
            string file_name_Main1 = "d:\\DRLaser\\AMotionParam\\Main(M0).cfg";
            string file_name_Main2 = "d:\\DRLaser\\AMotionParam\\Main(M1).cfg";


            double Wafer_VelLow = pci1245l.WaferRatio * 10, Wafer_VelHigh = pci1245l.WaferRatio * pci1245l.WaferSpeed;
            double Bufferm_VelLow = pci1245l.BufferRatio * 10, Bufferm_VelHigh = pci1245l.BufferRatio * pci1245l.BufferSpeed;
            double Rotatem_VelLow = pci1245l.RotateRatio * 10, Rotatem_VelHigh = pci1245l.RotateRatio * pci1245l.RotateSpeed;

            //double WaferAxisAcc = pci1245l.WaferRatio * pci1245l.WaferSpeed * 8;
            //double WaferAxisDec = pci1245l.WaferRatio * pci1245l.WaferSpeed * 7;
            double WaferAxisAcc = pci1245l.WaferRatio * pci1245l.WaferSpeed * FormHMI.m_MAccDecc;
            double WaferAxisDec = pci1245l.WaferRatio * pci1245l.WaferSpeed * FormHMI.m_MAccDecc;

            double BufferAxisAcc = pci1245l.BufferRatio * pci1245l.BufferSpeed * 10;
            double RotateAxisAcc = pci1245l.RotateRatio * pci1245l.RotateSpeed * 10;


            /////////////PCI-1245L board
            //Step1.  Get  available  devices  by  calling  API "Acm_GetAvailableDevs"  
            errcde = Motion.mAcm_GetAvailableDevs(devList, 100, ref devCnt);
            if (errcde != 0)
                MessageBox.Show(this, "Can not find available device1245L! \n", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //else
            //	MessagePopup("温馨提示","Get available devices1245L successfully! \n");  
            //Step2. Open device.
            ////第一张1245L卡   	
            devNum = devList[0].DeviceNum;
            Rerrcde = Motion.mAcm_DevOpen(devNum, ref dev1Handle);
            if (Rerrcde != 0)
                MessageBox.Show(this, "Open device 1245L-1 is failed! \n", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //	else
            //		MessagePopup("温馨提示","Open device successfully! \n ");  
            //Step3. Open the axes.
            for (i = 0; i < 4; i++)
            {
                Rerrcde = Motion.mAcm_AxOpen(dev1Handle, i, ref ax1Handle[i]);
            }

            Motion.mAcm_DevLoadConfig(dev1Handle, file_name_Main1);

            ////重新设置硅片传输速度

            Motion.mAcm_SetF64Property(ax1Handle[pci1245l.Axis_ONE], (uint)PropertyID.PAR_AxVelLow, Wafer_VelLow);
            Motion.mAcm_SetF64Property(ax1Handle[pci1245l.Axis_ONE], (uint)PropertyID.PAR_AxVelHigh, Wafer_VelHigh);
            Motion.mAcm_SetF64Property(ax1Handle[pci1245l.Axis_TWO], (uint)PropertyID.PAR_AxVelLow, Wafer_VelLow);
            Motion.mAcm_SetF64Property(ax1Handle[pci1245l.Axis_TWO], (uint)PropertyID.PAR_AxVelHigh, Wafer_VelHigh);
            Motion.mAcm_SetF64Property(ax1Handle[pci1245l.Axis_THREE], (uint)PropertyID.PAR_AxVelLow, Wafer_VelLow);
            Motion.mAcm_SetF64Property(ax1Handle[pci1245l.Axis_THREE], (uint)PropertyID.PAR_AxVelHigh, Wafer_VelHigh);


            ///定义加减速	
            Motion.mAcm_SetF64Property(ax1Handle[pci1245l.Axis_ONE], (uint)PropertyID.PAR_AxAcc, WaferAxisAcc);
            Motion.mAcm_SetF64Property(ax1Handle[pci1245l.Axis_ONE], (uint)PropertyID.PAR_AxDec, WaferAxisDec);
            Motion.mAcm_SetF64Property(ax1Handle[pci1245l.Axis_TWO], (uint)PropertyID.PAR_AxAcc, WaferAxisAcc);
            Motion.mAcm_SetF64Property(ax1Handle[pci1245l.Axis_TWO], (uint)PropertyID.PAR_AxDec, WaferAxisDec);
            Motion.mAcm_SetF64Property(ax1Handle[pci1245l.Axis_THREE], (uint)PropertyID.PAR_AxAcc, WaferAxisAcc);
            Motion.mAcm_SetF64Property(ax1Handle[pci1245l.Axis_THREE], (uint)PropertyID.PAR_AxDec, WaferAxisDec);


            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 7, 0);  //关闭CCD光源
            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 5, 0);  //关闭光闸

            ////第二张1245L卡
            devNum = devList[1].DeviceNum;
            Rerrcde = Motion.mAcm_DevOpen(devNum, ref dev2Handle);
            if (Rerrcde != 0)
                MessageBox.Show(this, "Open device 1245L-2 is failed! \n", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //	else
            //		MessagePopup("温馨提示","Open device successfully! \n ");  
            //Step3. Open the axes.
            for (i = 0; i < 4; i++)
            {
                Rerrcde = Motion.mAcm_AxOpen(dev2Handle, i, ref ax2Handle[i]);
            }

            Motion.mAcm_DevLoadConfig(dev2Handle, file_name_Main2);
            /////////////速度设置/////////////////////
            /////////设置Buffer速度
            Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_ONE], (uint)PropertyID.PAR_AxVelLow, Bufferm_VelLow);
            Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_ONE], (uint)PropertyID.PAR_AxVelHigh, Bufferm_VelHigh);
            Motion.mAcm_SetF64Property(ax1Handle[pci1245l.Axis_FOUR], (uint)PropertyID.PAR_AxVelLow, Bufferm_VelLow);
            Motion.mAcm_SetF64Property(ax1Handle[pci1245l.Axis_FOUR], (uint)PropertyID.PAR_AxVelHigh, Bufferm_VelHigh);
            ////////设置旋转轴速度
            Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_TWO], (uint)PropertyID.PAR_AxVelLow, Rotatem_VelLow);
            Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_TWO], (uint)PropertyID.PAR_AxVelHigh, Rotatem_VelHigh);
            Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_THREE], (uint)PropertyID.PAR_AxVelLow, Rotatem_VelLow);
            Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_THREE], (uint)PropertyID.PAR_AxVelHigh, Rotatem_VelHigh);
            /////设置DD速度
            // Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_FOUR], (uint)PropertyID.PAR_AxVelLow,  DDm_VelLow);
            // Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_FOUR], (uint)PropertyID.PAR_AxVelHigh,  DDm_VelHigh);

            ////////////加减速设置////////////////////
            ///////设置Buffer加减速
            Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_ONE], (uint)PropertyID.PAR_AxAcc, BufferAxisAcc);
            Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_ONE], (uint)PropertyID.PAR_AxDec, BufferAxisAcc);
            Motion.mAcm_SetF64Property(ax1Handle[pci1245l.Axis_FOUR], (uint)PropertyID.PAR_AxAcc, BufferAxisAcc);
            Motion.mAcm_SetF64Property(ax1Handle[pci1245l.Axis_FOUR], (uint)PropertyID.PAR_AxDec, BufferAxisAcc);
            ////////设置旋转臂加减速
            Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_TWO], (uint)PropertyID.PAR_AxAcc, RotateAxisAcc);
            Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_TWO], (uint)PropertyID.PAR_AxDec, RotateAxisAcc);
            ////////设置旋转臂加减速
            Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_THREE], (uint)PropertyID.PAR_AxAcc, RotateAxisAcc);
            Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_THREE], (uint)PropertyID.PAR_AxDec, RotateAxisAcc);

            ///旋转电机使能   	
            Motion.mAcm_AxSetSvOn(ax2Handle[pci1245l.Axis_TWO], 1);
            Motion.mAcm_AxSetSvOn(ax2Handle[pci1245l.Axis_THREE], 1);
            ///DD马达使能 	
            Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 6, 1);//高
            //  Motion.mAcm_AxSetSvOn(ax2Handle[pci1245l.Axis_FOUR], 1);


            return 0;
        }
        /*********************************************************************************************************
       ** 函数名称 ：fInitLoadBoard
       ** 函数功能 ：上料接驳台初始化
       ** 入口参数 ：
       ** 出口参数 ： 
       *********************************************************************************************************/
        int fInitLoadBoard()
        {
            short ret = 0;
            ushort m_CardNo = 0;
            ushort m_Baudrate0 = 2;
            uint[] lDevTable = new uint[2];
            uint UpLoad_Wafer_VelLow = Convert.ToUInt16(pci1245l.WaferRatio);
            uint UpLoad_Wafer_VelHigh = Convert.ToUInt32(pci1245l.WaferRatio * pci1245l.WaferSpeed);
            //float UpLoad_WaferAccTime = 0.12f;
            //float UpLoad_WaferDecTime = 0.14f;
            float UpLoad_WaferAccTime = 0.10f;
            float UpLoad_WaferDecTime = 0.12f;
            uint Elevator_VelLow = 10 * pci1245l.ElevatorRatio, Elevator_VelHigh = pci1245l.ElevatorRatio * pci1245l.ElevatorMaxSpeed;
            ///////////////_l122_dsf_open////////////////////
            ret += Master.PCI_L122_DSF._l122_dsf_open(ref m_Existcards);
            if (m_Existcards == 0)
            {
                MessageBox.Show(this, "No any PCI_L122_DSF!!!", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                ret = Master.PCI_L122_DSF._l122_dsf_get_switch_card_num(0, ref m_CardNo);
            ///////////////m_RingNoA////////////////////
            if (Master.PCI_L122_DSF._l122_dsf_set_ring_config(m_CardNo, m_RingNoA, m_Baudrate0) != 0)
            {
                MessageBox.Show(this, "Set ring config fail !!", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            ///////////////_mnet_reset_ring////////////////////
            if (MNet.Basic._mnet_reset_ring(m_RingNoA) != 0)
            {
                MessageBox.Show(this, "Reset Ring fail!!", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            ///////////////_mnet_start_ring////////////////////
            ret = MNet.Basic._mnet_start_ring(m_RingNoA);
            ///////////////_mnet_get_ring_active_table////////////////////
            ret = MNet.Basic._mnet_get_ring_active_table(m_RingNoA, lDevTable);

            if (ret == -74)
            {
                MessageBox.Show(this, "No Device!", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            ///////////////////////////////////////////////////
            MNet.SlaveType slavetype = 0;
            ret += MNet.Basic._mnet_get_slave_type(m_RingNoA, ClineAddr.LoadAX0IP, ref slavetype);
            if (slavetype == MNet.SlaveType.AXIS_M1x1a)//axis module
            { //初始化轴1
                ret = MNet.M1A._mnet_m1a_initial(m_RingNoA, ClineAddr.LoadAX0IP);
                MNet.M1A._mnet_m1a_set_alm(m_RingNoA, ClineAddr.LoadAX0IP, 1, 0);///////////         
            }
            ret += MNet.Basic._mnet_get_slave_type(m_RingNoA, ClineAddr.LoadAX1IP, ref slavetype);
            if (slavetype == MNet.SlaveType.AXIS_M1x1a)//axis module
            {
                //初始化轴2
                ret = MNet.M1A._mnet_m1a_initial(m_RingNoA, ClineAddr.LoadAX1IP);
                MNet.M1A._mnet_m1a_set_alm(m_RingNoA, ClineAddr.LoadAX1IP, 1, 0);///////////          
            }
            /////////////////////////////////////     
            //MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNoA, ClineAddr.LoadAX0IP, Elevator_VelLow, Elevator_VelHigh, 0.1f, 0.1f);//升降模组
            MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNoA, ClineAddr.LoadAX0IP, Elevator_VelLow, Elevator_VelHigh, 0.08f, 0.08f);//升降模组
            MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNoA, ClineAddr.LoadAX1IP, UpLoad_Wafer_VelLow, UpLoad_Wafer_VelHigh, UpLoad_WaferAccTime, UpLoad_WaferDecTime);//舌头

            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 3, 0);//下压气缸缩回
            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 4, 0);//舌头缩回
            MNet.M1A._mnet_m1a_reset_position(m_RingNoA, ClineAddr.LoadAX0IP);//舌头模组位置清零
            MNet.M1A._mnet_m1a_set_svon(m_RingNoA, ClineAddr.LoadAX0IP, 1);
            MNet.M1A._mnet_m1a_set_svon(m_RingNoA, ClineAddr.LoadAX1IP, 1);
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 4, 0);//20220715

            return 0;
        }
        /*********************************************************************************************************
        ** 函数名称 ：fInitRFID
        ** 函数功能 ：RFID初始化
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/

        int fInitRFID()
        {
            RFIDInit = new ReaderInit(ReaderInit.RFNum_enum.RF0);
            RFIDReader = new Modbus(true);
            if (RFIDReader.Connect("192.168.1.10", 502))
            {
                RFIDReader.EnableRF(RFIDInit);
            }
            else
            {
                MessageBox.Show("Could not open RFID connection!");
            }
            return 0;
        }
        /*********************************************************************************************************
         ** 函数名称 ：fInitModbus
         ** 函数功能 ：Modbus初始化
         ** 入口参数 ：
         ** 出口参数 ： 
         *********************************************************************************************************/
        //int fInitModbus()
        //{
        //    // TCP/IP Connection
        //    //axMbaxp1.Connection = MBAXPLib.enumConnection.TCP_IP;
        //    //axMbaxp1.ConnectTimeout = 3000;
        //    //axMbaxp1.IPAddr1 = 127;
        //    //axMbaxp1.IPAddr2 = 0;
        //    //axMbaxp1.IPAddr3 = 0;
        //    //axMbaxp1.IPAddr4 = 1;
        //    //axMbaxp1.TCPIPPort = 502;
        //    //axMbaxp1.Timeout = 1000;
        //    //axMbaxp1.OpenConnection();
        //    //if (axMbaxp1.GetLastError() != 0)
        //    //    MessageBox.Show("Could not open modbus connection!");
        //    //else
        //    //{
        //    //    // Read 16 Holding registers from address 0 (40001) every 100ms
        //    //    axMbaxp1.ReadHoldingRegisters(1, 1, 0, 3, 500);
        //    //    axMbaxp1.UpdateEnable(1);

        //    //}

        //    //return 0;
        //}
        /*********************************************************************************************************
        ** 函数名称 ：fInitialCOM
        ** 函数功能 ：串口初始化
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        void fInitialCOM()
        {
            ComPower.BaudRate = 9600;
            ComPower.PortName = "COM4";
            ComPower.DataBits = 8;
            ComPower.Parity = (Parity)0;
            ComPower.StopBits = StopBits.One;
            ComPower.WriteTimeout = 3000;
            ComPower.ReadTimeout = 3000;
            ComPower.Open();
            if (ComPower.IsOpen)
            {
                ComPower.ReceivedBytesThreshold = 1;
                ComPower.DataReceived += new SerialDataReceivedEventHandler(com_DataReceived);
            }
            else
                MessageBox.Show("串口打开失败！");

        }
        /*********************************************************************************************************
        ** 函数名称 ：com_DataReceived
        ** 函数功能 ：串口中断函数
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        private void com_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string strData = null;
            string[] DataBuffer;
            try
            {
                //接收数据
                do
                {
                    int count = ComPower.BytesToRead;
                    if (count <= 0)
                        break;
                    byte[] g_RS232DataBuff = new byte[count];
                    ComPower.Read(g_RS232DataBuff, 0, count);
                    strData = Encoding.UTF8.GetString(g_RS232DataBuff);//获取串口字符串
                    DataBuffer = strData.Split(';');//返回包含';'字符的子字符串=DataBuffer
                    double.TryParse(DataBuffer[0], out m_PowerData);

                    /* if (m_LaserPower)
                     { m_LaserPower = false; LaserPower.BackColor = Color.Green; }
                     else
                     { m_LaserPower = true; LaserPower.BackColor = Color.Brown; }
                     */
                    this.Invoke((EventHandler)(delegate
                    {
                        if (m_PowerData > 0.0)
                        {
                            m_PowerData = m_PowerData * m_PowerRatio;
                            NUM_PowerData.Value = m_PowerData;
                        }

                        /*  if ((m_PowerCounter > 16) && (m_PowerCounter < 18))  //自动加工，并且稳定测量
                          {
                              if ((m_PowerData < g_SysParam[18]) || (m_PowerData > g_SysParam[19])) //超出功率范围
                              {
                                  m_PowerCounter = 0;
                                  timerPower.Enabled = false;   //关闭定時器      
                                  m_ErrorFlag = 1; fALAM("警告", AlarmMess[11]);//激光器出光报警	   

                                  fEMGStop();
                              }

                          }*/
                    }));


                } while (ComPower.BytesToRead > 0);

                //处理接收后的命令

            }
            catch (Exception ex)
            {
                LogClass.Text = "error:接收返回消息异常！具体原因：" + ex.Message;
            }
        }
        /*********************************************************************************************************
       ** 函数名称 ：fALAM
       ** 函数功能 ：主机报警
       ** 入口参数 ：
       ** 出口参数 ： 
       *********************************************************************************************************/
        void fALAM(string title, string message)
        {
            string SystemTime, SystemTime1;
            m_AlamBuzzer = false; m_GreenYLED = 0;
            m_BuzzerCount = 0; m_AlarmFlag = 1;
            m_RotateOut2Count = 0;
            /////////关闭灯塔 
            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_THREE], pci1245l.BuzzerOut, 0);
            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_THREE], pci1245l.GreenOut, 0);
            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_THREE], pci1245l.YellowOut, 0);
            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_THREE], pci1245l.RedOut, 0);
            timerBuzzer.Enabled = true;  //打开定時器	
            timerRead.Enabled = false;
            timerPower.Enabled = false;
            MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            timerRead.Enabled = true;
            if (m_PowerCounter > 0)
                timerPower.Enabled = true;
            timerBuzzer.Enabled = false;   //关闭定時器     
            /////////关闭灯塔 
            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_THREE], pci1245l.BuzzerOut, 0);
            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_THREE], pci1245l.GreenOut, 0);
            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_THREE], pci1245l.YellowOut, 0);
            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_THREE], pci1245l.RedOut, 0);
            m_AlarmFlag = 0; m_LaserRelease = 0; m_LoadToughWafer = false;

            Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_ONE], 0, ref m_AIN0);   //进料buff硅片检测
            if (m_AIN0 == 1)
                m_In1AhomeFlag = 1;
            else
                m_In1AhomeFlag = 0;

            Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_ONE], 2, ref m_AIN2);   //进料模组1硅片检测
            if (m_AIN2 == 0)
            {
                m_MainIn2 = 0;
            }
            else
            {
                m_MainIn2 = 1;
            }

            //SystemTime = DateTime.Now.ToLongDateString();           
            //DateTime1 = DateTime.Now.ToString("hh:mm:ss");
            /////////错误记录文件           
            //SystemTime = "d:\\DRLaser\\ALARM\\" + SystemTime + ".txt";
            //if (!File.Exists(SystemTime))         
            //{
            //    //新建文件
            //    FileStream Errorfile = new FileStream(SystemTime, FileMode.Create);
            //    Errorfile.Close();
            //}
            //FileStream AlarmFile = new FileStream(SystemTime, FileMode.Append);//最后一行追加
            //StreamWriter write = new StreamWriter(AlarmFile);
            //message = DateTime1+" "+ message;
            //write.WriteLine(message);
            //if (m_CCDAlarm == 3)
            //{
            //    message = APositionX.ToString() + "   " + APositionY.ToString() + "   " + SampleAngle.ToString();
            //    write.WriteLine(message);
            //}              
            //write.Close(); AlarmFile.Close();
            SystemTime = DateTime.Now.ToLongDateString();
            SystemTime1 = DateTime.Now.ToString("HH:mm:ss");
            ///////错误记录文件  
            int TimeNow;
            string TimeDate;
            TimeNow = DateTime.Now.Hour * 100 + DateTime.Now.Minute;
            if ((TimeNow > 830) && (TimeNow < 2031))
            {
                SystemTime = "d:\\DRLaser\\ALARM\\" + SystemTime + "_白班.txt";
            }
            else if ((TimeNow <= 830) && (TimeNow >= 0))
            {
                TimeDate = DateTime.Now.AddDays(-1).ToLongDateString();
                SystemTime = "d:\\DRLaser\\ALARM\\" + TimeDate + "_晚班.txt";
            }
            else if ((TimeNow >= 2031) && (TimeNow <= 2359))
            {
                SystemTime = "d:\\DRLaser\\ALARM\\" + SystemTime + "_晚班.txt";
            }
            //SystemTime = "d:\\DRLaser\\ALARM\\" + SystemTime + ".txt";
            if (File.Exists(SystemTime))
                ;
            else
            {
                //新建文件
                using (FileStream Errorfile = new FileStream(SystemTime, FileMode.Create))
                {

                }
            }
            using (FileStream AlarmFile = new FileStream(SystemTime, FileMode.Append))//最后一行追加
            {
                using (StreamWriter write = new StreamWriter(AlarmFile))
                {
                    if (m_CCDAlarm == 3)
                    {
                        message = message + APositionX.ToString() + "   " + APositionY.ToString() + "   " + SampleAngle.ToString();

                    }
                    write.WriteLine(SystemTime1 + "  " + message);
                }
            }
            if ((m_CCDAlarm == 3) || (m_CCDAlarm == 1) || (m_CCDAlarm == 2))
                fBrokenWaferFile();
        }

        /*********************************************************************************************************
        ** 函数名称 ：fEMGStop
        ** 函数功能 ：紧急停止
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        void fEMGStop()
        {
            int IsMarking;
            g_MainStep = 30; m_Out1WaferAlarm2Flag = 0;
            LogClass.Text = "紧急停止,请取走DD马达台面硅片";
            m_Pause = false; m_InBufferRelease = 0;
            m_AutoMoveCounter = 0; m_WaferCheckStop = false;
            m_PowerCounter = 0; m_InMotor2Continue = 0; m_InMotor1Continue = 0;
            m_AlamDownContinue = 0; m_InStepConter = 0;
            m_CCDFinishFlag = 0;// m_CCDWaferHave = false;
            m_RotateInFlag = 0; m_RotateOutFlag = 0; m_DDRunDelay = 0; m_DDRunFlag = false;
            m_RotateInFinish = 0;
            m_LaserPowerFlag = 0; m_CCDBreakWaferAlarm = 0;
            m_LoadSuck = 0; m_UnLoadSuck = false; m_LoadToughWafer = false;
            m_WaferSuckDelay = 0;
            m_LoadHandOut = false; m_LaserCheckCount = 0;
            m_TotalGaspressureDelay = 0;
            m_LaserRelease = 0; m_LeftWaferDeal = 0;
            m_StepContinue = 0; m_MainStepContinue = 0;
            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 7, 0);  //关闭CCD光源  

            Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 7, 0);   //启动关闭
            Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 6, 0);  //低
            Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 4, 0);  //低
            Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 5, 0);//低

            /////////判断是否正在雕刻
            do
            {
                IsMarking = axMMMark1.IsMarking();
            }
            while (IsMarking == 1);
            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 5, 0); //关闭光闸 

            timerTotal.Enabled = false;
            AutoRun.Enabled = true;
            Stop.Enabled = false;
            KEY_StartMeasure.Enabled = true;
            AutoPause.Value = false;//暂停按钮复位
            AutoPause.Enabled = false;

            KEY_OPTSwitch.Value = false;

            ////////////下料位置硅片记忆处理
            switch (g_OutRotateMotorPosition)
            {
                //下料旋转原始位置
                case 0:
                    if (m_ZIN3 == 1)//吸附到位
                        m_waferoutflag = 1;
                    break;
                case 1:
                    if (m_ZIN2 == 1)//吸附到位
                        m_waferoutflag = 1;
                    break;
            }
            if (m_WaferReleaseFlag == true)
                m_waferoutflag = 1;
            System.Threading.Thread.Sleep(300);//延时300ms
            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_ONE], 7, 0);  //加工吹气关闭  
            Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_TWO], 4, 0);
            Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_TWO], 5, 0);
            Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 4, 0);
            Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 5, 0);
            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_ONE], 4, 0);
            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_ONE], 5, 0);
            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_TWO], 4, 0);
            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 6, 0);
            m_NoCheck = false;
            CheckBox_NoCheck.Checked = false;
            Text_NoCheck.Visible = false;


            TextShow.Text = "自动化运行停止！";
            fDim(true);
            axMMIO1.SetOutput(15, 0); //打开Gate 
            //axMMMark1.LaserOn(); //打开Gate 

        }
        /*********************************************************************************************************
        ** 函数名称 ：fCCDRead
        ** 函数功能 ：CCD拍照
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        uint fCCDRead(double[] Data)
        {
            int ToolAddr;
            double CCDBugEmpty = 0, CCDBug = 0;
            double value = 0.0;

            object TempData1 = new VariantWrapper(value);
            object TempData2 = new VariantWrapper(value);
            object TempData3 = new VariantWrapper(value);
            object CCDStatus = new VariantWrapper(value);
            object CCDCheck = new VariantWrapper(value);

            object CCDFinishData = new VariantWrapper(value);
            object CCDFinishData1 = new VariantWrapper(value);

            CCDFinishData1 = 0;

            double[] m_CCDDataTemp = new double[4];
            m_CCDDataTemp[0] = Data[0]; m_CCDDataTemp[1] = Data[1]; m_CCDDataTemp[2] = Data[2];

            ToolAddr = axCKVisionCtrl1.GetTool("用户变量");
            axCKVisionCtrl1.SetValue(ToolAddr, 107, 0, CCDFinishData1);

            //  System.Threading.Thread.Sleep(20);//延时20ms

            axCKVisionCtrl1.Execute(0);
            axCKVisionCtrl1.Redraw();

            axCKVisionCtrl1.GetValue(ToolAddr, 100, 0, ref CCDStatus);          //CCD状态读取
            m_CCDStatus = Convert.ToInt16(CCDStatus);
            axCKVisionCtrl1.GetValue(ToolAddr, 107, 0, ref CCDFinishData);
            m_CCDFinishData = Convert.ToInt16(CCDFinishData);

            if (m_CCDStatus != 1)
            {
                return 1;
            }
            if (m_CCDFinishData != 1)
            {
                m_CCDAlarm5Count++;
                return 5;

            }
            if ((m_CCDStatus == 1) && (m_CCDFinishData == 1))
            {
                m_CCDAlarm5Count = 0;
                axCKVisionCtrl1.GetValue(ToolAddr, 101, 0, ref TempData1);
                Data[0] = Convert.ToDouble(TempData1.ToString());

                axCKVisionCtrl1.GetValue(ToolAddr, 102, 0, ref TempData2);
                Data[1] = Convert.ToDouble(TempData2.ToString());

                axCKVisionCtrl1.GetValue(ToolAddr, 103, 0, ref TempData3);
                Data[2] = Convert.ToDouble(TempData3.ToString());

                axCKVisionCtrl1.GetValue(ToolAddr, 104, 0, ref CCDCheck);
                Data[3] = Convert.ToInt16(CCDCheck);

                if (((Math.Abs(Data[0] - 0.0f) < 0.0001f) && (Math.Abs(Data[1] - 0.0f) < 0.0001f) && (Math.Abs(Data[2] - 0.0f) < 0.0001f)))
                {
                    //  m_CCDAlarm = 4;
                    return 4;
                }

                Data[0] = Data[0] - m_CCDBaseX;
                Data[1] = Data[1] - m_CCDBaseY;
                Data[2] = Data[2] - m_CCDBaseAng;

                //  m_CCDShallDiff = Math.Abs(m_CCDShallStand - CCDBugEmpty);
                //  m_CCDWaferDiff = Math.Abs(m_CCDWaferStand - CCDBug);
                this.Invoke((EventHandler)(delegate
                {
                    //   ShallDiff.Value = m_CCDShallDiff;
                    //  WaferDiff.Value = m_CCDWaferDiff;
                    APositionX.Value = Data[0];
                    APositionY.Value = Data[1];
                    SampleAngle.Value = Data[2];
                    m_RelativeX = Data[0] - m_CCDStandX;
                    m_RelativeY = Data[1] - m_CCDStandY;
                    RelativeX.Value = Data[0] - m_CCDStandX;
                    RelativeY.Value = Data[1] - m_CCDStandY;
                }));
            }
            //      Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 7, 0);//高
            return 0;
        }
        /***************************** fScanRun ********************************/
        /**  振镜开始加工
        ***********************************************************************/
        void fScanRun()
        {
            int WaferOn = 0;

            switch (g_CWPositionFlag)
            {
                case 0:
                    if (m_XIN0 == 1) //C台面吸附到位  
                        WaferOn = 1;
                    break;
                case 1:
                    if (m_UIN3 == 1) //D台面吸附到位 
                        WaferOn = 1;
                    break;
                case 2:
                    if (m_DIN1 == 1) //A台面吸附到位  
                        WaferOn = 1;
                    break;
                case 3:
                    if (m_UIN2 == 1) //B台面吸附到位 
                        WaferOn = 1;
                    break;

            }
            ////////打标开始
            if (m_CCDNoWafer == true)//CCD拍照时没有发现硅片，所以并没有拍照，不能打标
            {
                if (WaferOn == 1)
                {
                    m_ErrorFlag = 1; fALAM("警告", AlarmMess[39]);
                    m_CCDNoWafer = false; fEMGStop();
                }
                else
                {
                    m_MarkFinish = 11; m_CCDNoWafer = false;
                }
            }
            else//CCD拍照时有硅片
            {
                if (WaferOn == 0)
                {
                    m_LaserNoWafer++;
                    LaserNoWafer.Value = m_LaserNoWafer;
                }
                m_MarkFinish = 1;
                if (m_YIN2 == 0)//如果激光已经被释放，或者光闸没有打开	   
                {
                    // axMMMark1.LaserOff();//关闭Gate  
                    axMMIO1.SetOutput(15, 1); //关闭Gate
                    Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 5, 1); //打开光闸 
                    KEY_OPTSwitch.Value = true;
                    System.Threading.Thread.Sleep(400);//延时100ms

                }
                //光闸打开 
                Motion.mAcm_AxDiGetBit(ax2Handle[pci1245l.Axis_TWO], 2, ref m_YIN2);
                if (m_YIN2 == 1) //光闸打开到位
                {
                    axMMMark1.StartMarkingExt(4);
                    m_TotalQulity++;
                    this.Invoke((EventHandler)(delegate
                    {
                        TotalQulity.Value = m_TotalQulity;
                    }));

                }
                else
                {
                    m_ErrorFlag = 1; fALAM("警告", AlarmMess[41]);
                    fEMGStop();
                }
            }
            //if (WaferOn == 1)//有硅片
            //{
            //    if (m_CCDWaferHave == true)//CCD拍照时没有发现硅片，所以并没有拍照，不能打标
            //    {
            //        m_ErrorFlag = 1; fALAM("警告", AlarmMess[75]);
            //        m_CCDWaferHave = false; fEMGStop();
            //    }
            //    else
            //    {
            //        m_MarkFinish = 1;
            //        if (m_YIN2 == 0)//如果激光已经被释放，或者光闸没有打开	   
            //        {
            //            axMMIO1.SetOutput(15, 1); //关闭Gate 
            //            //axMMMark1.LaserOff();//关闭Gate 
            //            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 5, 1); //打开光闸 
            //            KEY_OPTSwitch.Value = true;
            //            System.Threading.Thread.Sleep(400);
            //        }
            //        //光闸打开 
            //        Motion.mAcm_AxDiGetBit(ax2Handle[pci1245l.Axis_TWO], 2, ref m_YIN2);
            //        if (m_YIN2 == 1) //光闸打开到位
            //        {
            //            axMMMark1.StartMarkingExt(4);
            //            m_TotalQulity++;
            //            this.Invoke((EventHandler)(delegate
            //            {
            //                TotalQulity.Value = m_TotalQulity;
            //            }));

            //        }
            //        else
            //        {
            //            m_ErrorFlag = 1; fALAM("警告", AlarmMess[41]);
            //            fEMGStop();
            //        }
            //    }
            //}
            //else
            //{ m_MarkFinish = 11; m_CCDWaferHave = false; }

        }
        /***************************** fScanRun ********************************/
        /**  振镜开始加工
        ***********************************************************************/
        void fScanRunSample(double FinishAngle, double dx, double dy)
        {
            //MMMARKLib__DMMMarkSetMatrix (MarkActiveXHandle, NULL, 10, 10, 5, &Return_Value);  //平移和旋转
            //	MMMARKLib__DMMMarkSetMatrix (MarkActiveXHandle, NULL, -dx, -dy, -FinishAngle, &Return_Value);  //平移和旋转                

            axMMMark1.StartMarkingExt(4);
            // Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_ONE], 7, 1);   //加工吹气打开 

        }
        /*********************************************************************************************************
          ** 函数名称 ：Quit_Click
          ** 函数功能 ：系统退出  
          ** 修改时间 ：20170915
          ** 修改内容 ：
          *********************************************************************************************************/
        private void Quit_Click(object sender, EventArgs e)
        {
            DialogResult ResultTemp;
            ResultTemp = MessageBox.Show(this, "将退出“自动化软件”，请确认是否需要退出自动化控制软件？", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (ResultTemp == DialogResult.OK)   //表示点击确认        
            {
                fSystemQuit();
            }
        }
        /*********************************************************************************************************
       ** 函数名称 ：GetObject_Click
       ** 函数功能 ：获取MM对象名称  
       ** 修改时间 ：20170915
       ** 修改内容 ：
       *********************************************************************************************************/
        private void GetObject_Click(object sender, EventArgs e)
        {
            int Count;
            string ChildName = null;
            Count = axMMEdit1.GetCount();
            axMMEdit1.GetLayerName(1, ref ChildName);
            axMMEdit1.GetChildObjectName("L1", 1, ref ChildName);
            axMMEdit1.GetChildObjectName("L1", 2, ref ChildName);

        }
        /*********************************************************************************************************
          ** 函数名称 ：MenuLoadOpen_Click
          ** 函数功能 ：菜单打开上料接驳台  
          ** 修改时间 ：20170915
          ** 修改内容 ：
          *********************************************************************************************************/
        private void MenuLoadOpen_Click(object sender, EventArgs e)
        {
            FormLoad Load = new FormLoad();
            Load.StartPosition = FormStartPosition.CenterScreen;
            Load.ShowDialog();

        }

        /*********************************************************************************************************
       ** 函数名称 ：MeunMainOpen_Click
       ** 函数功能 ：菜单打开主机  
       ** 修改时间 ：20170915
       ** 修改内容 ：
       *********************************************************************************************************/
        private void MeunMainOpen_Click(object sender, EventArgs e)
        {
            FormMain oMainForm = new FormMain();

            oMainForm.ChangeData += new ChangeFMFormData(f_ChangeFMData);
            oMainForm.ChangeData1 += new ChangeFMFormData(f_ChangeFMData1);//DD正转45度
            oMainForm.ChangeData2 += new ChangeFMFormData(f_ChangeFMData2);//DD反转45度
            oMainForm.OutChangeData += new ChangeFMFormData2(f_OutChangeFMData);
            oMainForm.OutChangeData1 += new ChangeFMFormData2(f_OutChangeFMData1);//出料旋转臂正转45度
            oMainForm.OutChangeData2 += new ChangeFMFormData2(f_OutChangeFMData2);//出料旋转臂反转45度
            oMainForm.ShowDialog();
        }
        void f_ChangeFMData(bool topGetState4)//DD马达
        {
            ++g_CWPositionFlag;
            if (g_CWPositionFlag > 3)
                g_CWPositionFlag = 0;
            fDDPositionStatus(0);
            //Test1.Value = m_InMotorOffset;
        }
        void f_ChangeFMData1(bool topGetState41)//DD正转45度
        {
            ++g_CWPositionFlag;
            if (g_CWPositionFlag > 3)
                g_CWPositionFlag = 0;
            if (DDInit == false)
            {
                Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 7, 0);   //启动关闭
                Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 6, 0);  //低               
                Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 4, 0);  //高
                Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 5, 1);//高
                System.Threading.Thread.Sleep(20);//延时2ms
                Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 7, 1);//启动打开
            }
        }
        void f_ChangeFMData2(bool topGetState42)//DD反转45度
        {
            ++g_CWPositionFlag;
            if (g_CWPositionFlag > 3)
                g_CWPositionFlag = 0;
            if (DDInit == false)
            {
                Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 7, 0);   //启动关闭
                Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 6, 1);  //低               
                Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 4, 0);  //高
                Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 5, 0);//高
                System.Threading.Thread.Sleep(20);//延时2ms
                Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 7, 1);//启动打开
            }
        }

        void f_OutChangeFMData(bool topGetState42)//出料旋转臂
        {
            ++g_OutRotateMotorPosition;
            if (g_OutRotateMotorPosition > 1)
                g_OutRotateMotorPosition = 0;
            fOutWaferPositionStatus(g_OutRotateMotorPosition);
            //Test1.Value = m_InMotorOffset;
        }
        void f_OutChangeFMData1(bool topGetState43)//出料旋转臂正转45度
        {
            ++g_OutRotateMotorPosition;
            if (g_OutRotateMotorPosition > 1)
                g_OutRotateMotorPosition = 0;
            if (DDInit == false)
                Motion.mAcm_AxMoveRel(ax2Handle[pci1245l.Axis_THREE], FormHMI.g_SysParam[45] * FormHMI.pci1245l.RotateRatio);
        }
        void f_OutChangeFMData2(bool topGetState44)//出料旋转臂反转45度
        {
            ++g_OutRotateMotorPosition;
            if (g_OutRotateMotorPosition > 1)
                g_OutRotateMotorPosition = 0;
            if (DDInit == false)
                Motion.mAcm_AxMoveRel(ax2Handle[pci1245l.Axis_THREE], -FormHMI.g_SysParam[45] * FormHMI.pci1245l.RotateRatio);
        }
        /*********************************************************************************************************
       ** 函数名称 ：MenuOpenGraph_Click
       ** 函数功能 ：打开图形 
       ** 修改时间 ：20170915
       ** 修改内容 ：
       *********************************************************************************************************/
        private void MenuOpenGraph_Click(object sender, EventArgs e)
        {
            this.openFile.FileName = "*.ezm";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                axMMMark1.MarkData_UnLock();
                //AutoRun.Enabled = true;
                axMMMark1.LoadFile(openFile.FileName);
                axMMMark1.Redraw();
                m_CenterX = axMMEdit1.GetCenterX("root");
                m_CenterY = axMMEdit1.GetCenterY("root");
                GotoMM.Enabled = true;
                Marking.Enabled = true;
                MMDirection.Text = openFile.FileName;
                axMMMark1.MarkData_Lock();
            }
        }
        /*********************************************************************************************************
       ** 函数名称 ：MenuMainPara_Click
       ** 函数功能 ：菜单打开主机参数设置  
       ** 修改时间 ：20170915
       ** 修改内容 ：
       *********************************************************************************************************/
        private void MenuMainPara_Click(object sender, EventArgs e)
        {
            /* MainParam oMainParam = new MainParam();
             oMainParam.StartPosition = FormStartPosition.CenterScreen;
             oMainParam.Show();
             oMainParam.TopGetState4 = true;*/
            MainParam f = new MainParam();
            f.ChangeData += new ChangeMPFormData(f_ChangeMPData);
            f.ShowDialog();

        }
        void f_ChangeMPData(bool topGetState4)
        {
            //  this.BackColor = Color.LightBlue;
            //  this.Text = "改变成功！";
            fSysParamAssignment();
            //Test1.Value = m_InMotorOffset;
        }

        /*********************************************************************************************************
       ** 函数名称 ：MenuLoadParam_Click
       ** 函数功能 ：菜单打开上料接驳台参数设置  
       ** 修改时间 ：20170915
       ** 修改内容 ：
       *********************************************************************************************************/
        private void MenuLoadParam_Click(object sender, EventArgs e)
        {
            LoadParam oLoadParam = new LoadParam();
            oLoadParam.StartPosition = FormStartPosition.CenterScreen;
            oLoadParam.ChangeData += new ChangeLPFormData(f_ChangeLPData);
            oLoadParam.ShowDialog();

        }
        void f_ChangeLPData(bool topGetState4)
        {
            /*  this.BackColor = Color.LightBlue;
              this.Text = "改变成功！";*/
            fSysParamAssignment();
            //Test1.Value = m_Load1220XElevatorStart;
        }

        //////////////////////////////////////CCD ActiveX操作/////////////////////////////////////////////////////////////////////////////
        /*********************************************************************************************************
       ** 函数名称 ：TakeOnePhoto_Click
       ** 函数功能 ：单次拍照  
       ** 修改时间 ：20170915
       ** 修改内容 ：
       *********************************************************************************************************/
        private void TakeOnePhoto_Click(object sender, EventArgs e)
        {
            // int ToolAdd;
            // object Revalue = null;
            fCCDRead(g_CCDSource);  //读取CCD数值   
        }

        /*********************************************************************************************************
       ** 函数名称 ：OpenCCDTool_Click
       ** 函数功能 ：打开CCD工具  
       ** 修改时间 ：20170915
       ** 修改内容 ：
       *********************************************************************************************************/
        /*      private void OpenCCDTool_Click(object sender, EventArgs e)
              {

              }*/

        /*********************************************************************************************************
      ** 函数名称 ：SaveCCDTool_Click
      ** 函数功能 ：保存CCD工具  
      ** 修改时间 ：20170915
      ** 修改内容 ：
      *********************************************************************************************************/
        private void SaveCCDTool_Click(object sender, EventArgs e)
        {
            // string strFilePathName = "D:\DRLaser\CCDLib\TestNew-Three.vscf";
            axCKVisionCtrl1.SaveConfigure(file_name1_Acti);
        }

        /*********************************************************************************************************
      ** 函数名称 ：ZOOM_IN_Click
      ** 函数功能 ：放大  
      ** 修改时间 ：20180521
      ** 修改内容 ：
      *********************************************************************************************************/
        private void ZOOM_IN_Click(object sender, EventArgs e)
        {
            axCKVisionCtrl1.ZoomView(0);
            axCKVisionCtrl1.Redraw();
        }

        /*********************************************************************************************************
      ** 函数名称 ：ZOOM_OUT_Click
      ** 函数功能 ：缩小 
      ** 修改时间 ：20180521
      ** 修改内容 ：
      *********************************************************************************************************/
        private void ZOOM_OUT_Click(object sender, EventArgs e)
        {
            axCKVisionCtrl1.ZoomView(1);
            axCKVisionCtrl1.Redraw();
        }

        /*********************************************************************************************************
      ** 函数名称 ：ZOOM_FIT_Click
      ** 函数功能 ：适应 
      ** 修改时间 ：20180521
      ** 修改内容 ：
      *********************************************************************************************************/
        private void ZOOM_FIT_Click(object sender, EventArgs e)
        {
            axCKVisionCtrl1.ZoomView(2);
            axCKVisionCtrl1.Redraw();
        }

        /*********************************************************************************************************
       ** 函数名称 ：EDIT_PROC_Click
       ** 函数功能 ：编辑
       ** 修改时间 ：20180521
       ** 修改内容 ：
       *********************************************************************************************************/
        private void OpenCCDTool_Click(object sender, EventArgs e)
        {
            try
            {
                axCKVisionCtrl1.ShowEditor("编辑流程", 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not edit proc. Original error: " + ex.Message);
            }
        }

        /*********************************************************************************************************
       ** 函数名称 ：OpenGraph_Click
       ** 函数功能 ：打开MM选择对话框 
       ** 修改时间 ：20170915
       ** 修改内容 ：
       *********************************************************************************************************/
        private void OpenGraph_Click(object sender, EventArgs e)
        {
            this.openFile.FileName = "*.ezm";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                axMMMark1.MarkData_UnLock();
                //AutoRun.Enabled = true;
                axMMMark1.LoadFile(openFile.FileName);
                axMMMark1.Redraw();
                m_CenterX = axMMEdit1.GetCenterX("root");
                m_CenterY = axMMEdit1.GetCenterY("root");
                GotoMM.Enabled = true;
                Marking.Enabled = true;
                MMDirection.Text = openFile.FileName;
                axMMMark1.MarkData_Lock();
            }
        }
        /*********************************************************************************************************
       ** 函数名称 ：GotoMM_Click
       ** 函数功能 ：切换到MM  
       ** 修改时间 ：20170915
       ** 修改内容 ：
       *********************************************************************************************************/
        private void GotoMM_Click(object sender, EventArgs e)
        {
            axMMMark1.MarkData_UnLock();
            axMMMark1.RunMarkingMate(openFile.FileName, 0);
            axMMMark1.MarkData_Lock();
        }

        /*********************************************************************************************************
       ** 函数名称 ：Help_Click
       ** 函数功能 ：帮助文件 
       ** 修改时间 ：20170915
       ** 修改内容 ：
       *********************************************************************************************************/
        private void Help_Click(object sender, EventArgs e)
        {
            AboutHelp Help = new AboutHelp();
            Help.ShowDialog();
        }
        /**********************************************************************************************************
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// TimerRead主定时器
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ***********************************************************************/
        private void timerRead_Tick(object sender, EventArgs e)
        {

            ////////////////////////////////////////////// 
            fSystemHome();//系统归零
            if (m_HomeFinished == 1)
            {
                KEY_LoadHandOut.Enabled = true;
                if (Marking.Enabled)
                {
                    AutoRun.Enabled = true;
                }
                // AutoPause.Enabled = true;
            }
            CurrentTable.Text = g_CWPositionFlag.ToString();//新增台面显示功能          
            fAuthorityStatus();//权限控制

            // fAutoLoadRun();//上料接驳台自动化 
            fAgvAlarm();
            fAutoAlign();//打齐自动化

            fAutoLoadAlarm();//接驳台报警主机轴报警

            if (m_NoCheck)
                Text_NoCheck.Visible = true;
            else
                Text_NoCheck.Visible = false;

            if ((m_Pause == false) && (m_SysResetFlag == true))//没有处于回零状态
            {
                if (g_MainStep < 30)
                {
                    // FormPassword.g_Authority = 0;
                    fAutoRotateIn();
                    fAutoRotateOut();
                    fAutoMainRun();//主机自动化 
                    fAutoAlarm();//报警自动化 
                    fAutoWaferTransfore();//传送模组自动化 

                    fAutoLoadRun();//上料接驳台自动化
                }
                else if (m_LoadHandOut == true)
                {
                    fAutoLoadRun();//上料接驳台自动化
                }
            }


            fLEDShark();//指示灯闪烁

            //抽尘气缸动作
            if (m_DustActionCount < 800/*5000*/)
                m_DustActionCount++;
            else
            {
                m_DustActionCount = 0;
                g_DustActionFlag = true;
                m_DustActionStep = 1;
            }

            fAutoDustAction(); //抽尘气缸动作
        }

        /*********************************************************************************************************
         ** 函数名称 ：fAutoDustAction()
         ** 函数功能 ：抽尘气缸动作
         ** 入口参数 ：
         ** 出口参数 ： 
         *********************************************************************************************************/
        void fAutoDustAction()
        {
            if (g_DustActionFlag == true)
            {
                switch (m_DustActionStep)
                {
                    case 1:
                        if (m_DustCrack == 1)
                            Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_TWO], 5, 1);
                        //motion.fSetBit(m_RingNoA, ClineAddr.MainXNT, 3, 3, 1);//抽尘气缸伸出  
                        m_DustActionCount1 = 0;
                        m_DustActionStep = 2;
                        break;
                    case 2:
                        if (m_DustActionCount1 < 50)
                            m_DustActionCount1++;
                        else
                        {
                            m_DustActionCount1 = 0;
                            m_DustActionStep = 3;
                        }
                        break;
                    case 3:
                        Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_TWO], 5, 0);
                        //motion.fSetBit(m_RingNoA, ClineAddr.MainXNT, 3, 3, 0);//抽尘气缸缩回  
                        g_DustActionFlag = false;
                        m_DustActionStep = 0;
                        break;

                }
            }
        }
        /*********************************************************************************************************
         ** 函数名称 ：fLEDShark
         ** 函数功能 ：指示灯闪烁
         ** 入口参数 ：
         ** 出口参数 ： 
         *********************************************************************************************************/
        void fLEDShark()
        {
            if (g_MainStep < 30)//自动流程 ，绿灯闪烁
            {
                if (m_GreenYLED < 40)
                    m_GreenYLED++;
                else
                {
                    Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_THREE], pci1245l.YellowOut, 0);
                    m_GreenYLED = 0;
                    if (m_Buzzer)
                    {
                        m_Buzzer = false;
                        Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_THREE], pci1245l.GreenOut, 0);
                    }
                    else
                    {
                        m_Buzzer = true;
                        Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_THREE], pci1245l.GreenOut, 1);
                    }
                }

            }
            else//空闲状态	，黄灯闪烁		  
            {
                if (m_GreenYLED < 40)
                    m_GreenYLED++;
                else
                {
                    m_GreenYLED = 0;
                    Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_THREE], pci1245l.GreenOut, 0);
                    if (m_Buzzer)
                    {
                        m_Buzzer = false;
                        Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_THREE], pci1245l.YellowOut, 0);
                    }
                    else
                    {
                        m_Buzzer = true;
                        Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_THREE], pci1245l.YellowOut, 1);
                    }
                }
            }

        }

        /***************************** fBoxStop ********************************/
        /**  报警时，紧急停止花篮上料
        ***********************************************************************/
        void fBoxStop()
        {

        }

        void fAuthortyMode(bool data)
        {
            //DoorProtect.Visible = data;
            Modify.Visible = data;
            LaserCheck.Visible = data;
            OpenCCDTool.Visible = data;
            SaveCCDTool.Visible = data;

            //  OpenGraph.Visible = data;
            //  GotoMM.Visible = data;
            //  MenuOpenGraph.Visible = data;
            //Quit.Visible = data;

        }
        /*********************************************************************************************************
     ** 函数名称 ：fAuthorityStatus
     ** 函数功能 ：权限状况
     ** 入口参数 ：
     ** 出口参数 ： 
     *********************************************************************************************************/
        void fAuthorityStatus()
        {
            switch (FormPassword.g_Authority)
            {
                case 0://作业员
                    fAuthortyMode(false);
                    OpenGraph.Visible = false;
                    MenuOpenGraph.Visible = false;
                    GotoMM.Visible = false;
                    MenuMainCorrect.Visible = false;
                    TextAuthority.Text = "权限：作业员";
                    break;
                case 1://管理员
                    fAuthortyMode(true);
                    OpenGraph.Visible = true;
                    MenuOpenGraph.Visible = true;
                    MenuMainCorrect.Visible = true;
                    GotoMM.Visible = true;
                    TextAuthority.Text = "权限：管理员";
                    break;
                case 2:///工程师
                    fAuthortyMode(true);
                    OpenGraph.Visible = true;
                    MenuOpenGraph.Visible = true;
                    GotoMM.Visible = true;
                    MenuMainCorrect.Visible = true;
                    TextAuthority.Text = "权限：工程师";
                    break;
                case 3://厂商
                    fAuthortyMode(true);
                    OpenGraph.Visible = false;
                    MenuOpenGraph.Visible = false;
                    GotoMM.Visible = false;
                    MenuMainCorrect.Visible = true;
                    TextAuthority.Text = "权限：厂商";
                    break;
            }
            if ((FormPassword.g_Authority > 0) && (FormPassword.g_Authority < 4) && (g_Modify))
            {
                Clean.Enabled = true;
            }
            else
            {
                Clean.Enabled = false;
            }
        }

        void fCheckIO()
        {
            while (true)
            {
                Thread.Sleep(2);
                fCheckIOStatus(); //IO扫描 
            }
        }

        /*********************************************************************************************************
     ** 函数名称 ：fCheckIOStatus
     ** 函数功能 ：轮询主机，接驳台IO状况
     ** 入口参数 ：
     ** 出口参数 ： 
     *********************************************************************************************************/
        public void fCheckIOStatus()
        {

            ////*********************旋转模组，升降模组实际位置获取************************************///////
            Motion.mAcm_AxGetCmdPosition(ax2Handle[pci1245l.Axis_TWO], ref g_Posnt[1]);//上料旋转电机位置
            Motion.mAcm_AxGetCmdPosition(ax2Handle[pci1245l.Axis_THREE], ref g_Posnt[2]);//下料旋转电机位置              
            //Motion.mAcm_AxGetCmdPosition(ax3Handle[pci1245l.Axis_FIVE], ref g_LoadElevationP);//获取上料升降模组实际位置
            MNet.M1A._mnet_m1a_get_command(m_RingNoA, ClineAddr.LoadAX0IP, ref g_LoadElevationP);  //获取上料升降模组实际位置   

            /////////////////////////////////////轮询主机，接驳台IO状况//////////////////////////////////////////////////////////////
            /////////获取主机IO   
            ///////扩展板A
            ////////第一轴IO判断
            Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_ONE], 0, ref m_AIN0);   //进料buff硅片检测	 
            Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_ONE], 1, ref m_AIN1);   //进料模组1定位 	           
            Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_ONE], 2, ref m_AIN2);   //进料模组1硅片检测
            Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_ONE], 3, ref m_AIN3);   //下游要片信号

            ////////第二轴IO判断     	
            Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_TWO], 0, ref m_BIN0);   //进料模组2硅片检测
            Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_TWO], 1, ref m_BIN1);   //进料模组2硅片定位  
            Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_TWO], 3, ref m_BIN3);   //安全门保护  

            ////////第三轴IO判断  
            Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_THREE], 0, ref m_CIN0);   //定位sensor  	
            Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_THREE], 1, ref m_CIN1);  //出料模组1硅片检测
            Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_THREE], 2, ref m_CIN2);   //丢料吸嘴提升气缸上升到位  	
            Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_THREE], 3, ref m_CIN3);   //丢料吸嘴提升气缸下升到位 
            if (m_CIN0 == 0) m_Out1WaferAlarm = 0;

            ////////第四轴IO判断    
            Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_FOUR], 0, ref m_DIN0);    //电源是否打开 
            Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_FOUR], 1, ref m_DIN1);    //加工台面B真空检测
            Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_FOUR], 2, ref m_DIN2);    //上料吸盘A真空检测
            Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_FOUR], 3, ref m_DIN3);    //上料吸盘B真空检测
            if (m_DIN0 == 0)
            {
                PowerOnOff = 1;
                Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_TWO], 7, 0);//切断DD Start信号
                Motion.mAcm_AxStopEmg(ax2Handle[pci1245l.Axis_ONE]);
                Motion.mAcm_AxStopEmg(ax2Handle[pci1245l.Axis_TWO]);
                Motion.mAcm_AxStopEmg(ax2Handle[pci1245l.Axis_THREE]);
                Motion.mAcm_AxStopEmg(ax2Handle[pci1245l.Axis_FOUR]);

                //fEMGStop();  
            }
            else
            {
                if (PowerOnOff == 1)//表示电源已经被断掉过，需要DD重新归零
                {

                    /*  m_ErrorFlag=2;//出现异常
                      fALAM("警告","由于电源刚刚被重启，请点击“主机复位”按钮！");  */
                    PowerOnOff = 0;
                }

            }
            //Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_FOUR], 1, ref Temp_DIN1);    //出料2模组定位 
            //if (Temp_DIN1 == 1)
            //    m_DIN1 = 0;
            //else
            //    m_DIN1 = 1;


            FormMain.g_Ax1Hand_IO = (uint)m_AIN0 + ((uint)m_AIN1 << 1) + ((uint)m_AIN2 << 2)
                                  + ((uint)m_BIN0 << 4) + ((uint)m_BIN1 << 5) + ((uint)m_BIN3 << 7)
                                  + ((uint)m_CIN0 << 8) + ((uint)m_CIN1 << 9) + ((uint)m_CIN2 << 10) + ((uint)m_CIN3 << 11)
                                  + ((uint)m_DIN0 << 12) + ((uint)m_DIN1 << 13) + ((uint)m_DIN2 << 14) + ((uint)m_DIN3 << 15);

            ///////扩展板B
            ////////第一轴IO判断
            Motion.mAcm_AxDiGetBit(ax2Handle[pci1245l.Axis_ONE], 0, ref m_XIN0);      //加工台面D真空检测       
            Motion.mAcm_AxDiGetBit(ax2Handle[pci1245l.Axis_ONE], 2, ref m_XIN2);  	  //激光状态检测 
            LED_XIN2.Value = Convert.ToBoolean(m_XIN2);
            Motion.mAcm_AxDiGetBit(ax2Handle[pci1245l.Axis_ONE], 3, ref m_XIN3);  	  //抽尘状态检测预留
            LED_XIN3.Value = Convert.ToBoolean(m_XIN3);

            ////////第二轴IO判断
            Motion.mAcm_AxDiGetBit(ax2Handle[pci1245l.Axis_TWO], 0, ref m_YIN0); 	  //水冷报警 
            Motion.mAcm_AxDiGetBit(ax2Handle[pci1245l.Axis_TWO], 1, ref m_YIN1);	  //丢料摆台丟料位 
            Motion.mAcm_AxDiGetBit(ax2Handle[pci1245l.Axis_TWO], 2, ref m_YIN2); 	  //光闸打开 
            LED_YIN2.Value = Convert.ToBoolean(m_YIN2);
            Motion.mAcm_AxDiGetBit(ax2Handle[pci1245l.Axis_TWO], 3, ref m_YIN3);  //光闸关闭 
            LED_YIN3.Value = Convert.ToBoolean(m_YIN3);

            ////////第三轴IO判断  
            Motion.mAcm_AxDiGetBit(ax2Handle[pci1245l.Axis_THREE], 0, ref m_ZIN0);//旋转气缸旋转检测
            Motion.mAcm_AxDiGetBit(ax2Handle[pci1245l.Axis_THREE], 1, ref m_ZIN1);//丢料吸盘真空检测
            Motion.mAcm_AxDiGetBit(ax2Handle[pci1245l.Axis_THREE], 2, ref m_ZIN2);//下料吸盘A检测
            Motion.mAcm_AxDiGetBit(ax2Handle[pci1245l.Axis_THREE], 3, ref m_ZIN3);//下料吸盘B检测 

            ////////第四轴IO判断  
            Motion.mAcm_AxDiGetBit(ax2Handle[pci1245l.Axis_FOUR], 0, ref m_UIN0);//DD马达报警
            Motion.mAcm_AxDiGetBit(ax2Handle[pci1245l.Axis_FOUR], 1, ref m_UIN1);//DD马达到位
            Motion.mAcm_AxDiGetBit(ax2Handle[pci1245l.Axis_FOUR], 2, ref m_UIN2);//加工台面A负压
            Motion.mAcm_AxDiGetBit(ax2Handle[pci1245l.Axis_FOUR], 3, ref m_UIN3);//加工台面C负压 
            LED_UIN3.Value = Convert.ToBoolean(m_UIN3);


            FormMain.g_Ax2Hand_IO = (uint)m_XIN0 + ((uint)m_XIN2 << 2) + ((uint)m_XIN3 << 3)
                                   + ((uint)m_YIN0 << 4) + ((uint)m_YIN1 << 5) + ((uint)m_YIN2 << 6) + ((uint)m_YIN3 << 7)
                                   + ((uint)m_ZIN0 << 8) + ((uint)m_ZIN1 << 9) + ((uint)m_ZIN2 << 10) + ((uint)m_ZIN3 << 11)
                                   + ((uint)m_UIN0 << 12) + ((uint)m_UIN1 << 13) + ((uint)m_UIN2 << 14) + ((uint)m_UIN3 << 15);

            //////PCI-1245L 特殊IO 判断 
            ////////A板 
            Motion.mAcm_AxGetMotionIO(ax1Handle[pci1245l.Axis_ONE], ref FormMain.g_Ax1Hand_ONE_MIO); // 
            m_APOS = (FormMain.g_Ax1Hand_ONE_MIO >> 13) & 0x01; //进料传送1到位信号 
            m_AAlarm = (FormMain.g_Ax1Hand_ONE_MIO >> 1) & 0x01; //进料传送1轴报警
            Motion.mAcm_AxGetMotionIO(ax1Handle[pci1245l.Axis_TWO], ref FormMain.g_Ax1Hand_TWO_MIO); //  
            m_BPOS = (FormMain.g_Ax1Hand_TWO_MIO >> 13) & 0x01; //进料传送2到位信号 
            m_BAlarm = (FormMain.g_Ax1Hand_TWO_MIO >> 1) & 0x01; //进料传送2轴报警
            Motion.mAcm_AxGetMotionIO(ax1Handle[pci1245l.Axis_THREE], ref FormMain.g_Ax1Hand_THREE_MIO); // 
            m_CPOS = (FormMain.g_Ax1Hand_THREE_MIO >> 13) & 0x01; //出料传送1到位信号
            m_CAlarm = (FormMain.g_Ax1Hand_THREE_MIO >> 1) & 0x01; //出料传送1轴报警
            Motion.mAcm_AxGetMotionIO(ax1Handle[pci1245l.Axis_FOUR], ref FormMain.g_Ax1Hand_FOUR_MIO); // 
            m_DPOS = (FormMain.g_Ax1Hand_FOUR_MIO >> 13) & 0x01; //出料Buffer到位信号  



            ////////B板  
            Motion.mAcm_AxGetMotionIO(ax2Handle[pci1245l.Axis_ONE], ref FormMain.g_Ax2Hand_ONE_MIO); //进料Buffer电机正限位
            m_XAlarm = (FormMain.g_Ax2Hand_ONE_MIO >> 1) & 0x01; //进料Buffer轴报警
            m_XLimP = (FormMain.g_Ax2Hand_ONE_MIO >> 2) & 0x01;
            m_XLimN = (FormMain.g_Ax2Hand_ONE_MIO >> 3) & 0x01;
            m_XHome = (FormMain.g_Ax2Hand_ONE_MIO >> 4) & 0x01;
            m_XPOS = (FormMain.g_Ax2Hand_ONE_MIO >> 13) & 0x01; //进料Buffer到位信号 
            Motion.mAcm_AxGetMotionIO(ax2Handle[pci1245l.Axis_TWO], ref FormMain.g_Ax2Hand_TWO_MIO); //进料旋转臂
            m_YAlarm = (FormMain.g_Ax2Hand_TWO_MIO >> 1) & 0x01; //进料旋转臂轴报警
            m_YHome = (FormMain.g_Ax2Hand_TWO_MIO >> 4) & 0x01; //
            m_YPOS = (FormMain.g_Ax2Hand_TWO_MIO >> 13) & 0x01; //进料旋转臂轴报警到位信号 

            Motion.mAcm_AxGetMotionIO(ax2Handle[pci1245l.Axis_THREE], ref FormMain.g_Ax2Hand_THREE_MIO);
            m_ZAlarm = (FormMain.g_Ax2Hand_THREE_MIO >> 1) & 0x01;  //出料旋转臂轴报警
            m_RotateHome = (FormMain.g_Ax2Hand_THREE_MIO >> 4) & 0x01; //出料旋转臂归零点 
            m_ZPOS = (FormMain.g_Ax2Hand_THREE_MIO >> 13) & 0x01;  //出料旋转臂到位信号 

            Motion.mAcm_AxGetMotionIO(ax2Handle[pci1245l.Axis_FOUR], ref FormMain.g_Ax2Hand_FOUR_MIO);
            m_DDHome = (FormMain.g_Ax2Hand_FOUR_MIO >> 4) & 0x01;  //DD马达归零点 
            m_DDPOS = (FormMain.g_Ax2Hand_FOUR_MIO >> 13) & 0x01; 	//DD到位信号 


            ////////////////////////////////// 获取上料接驳台DI ////////////////////////////////////////////////////	          

            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 0, 0, ref m_LoadIN00);//进花篮数量检测
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 0, 1, ref m_LoadIN01);//进花篮位置检测1
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 0, 2, ref m_LoadIN02);//进花篮位置检测2
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 0, 3, ref m_LoadIN03);//进花篮位置检测3
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 0, 4, ref m_LoadIN04);//进花篮位置检测4
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 0, 5, ref m_LoadIN05);//进花篮位置检测5
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 0, 6, ref m_LoadIN06);//进花篮位置检测6
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 0, 7, ref m_LoadIN07);//进料阻挡判断

            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 1, 0, ref m_LoadIN10);//进花阻挡气缸1伸出到位
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 1, 1, ref m_LoadIN11);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 1, 2, ref m_LoadIN12);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 1, 3, ref m_LoadIN13);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 1, 4, ref m_LoadIN14);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 1, 5, ref m_LoadIN15);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 1, 6, ref m_LoadIN16);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 1, 7, ref m_LoadIN17);//

            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 2, 0, ref m_LoadIN20);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 2, 1, ref m_LoadIN21);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 2, 2, ref m_LoadIN22);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 2, 3, ref m_LoadIN23);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 2, 4, ref m_LoadIN24);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 2, 5, ref m_LoadIN25);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 2, 6, ref m_LoadIN26);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 2, 7, ref m_LoadIN27);//

            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 3, 0, ref m_LoadIN30);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 3, 1, ref m_LoadIN31);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 3, 2, ref m_LoadIN32);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 3, 3, ref m_LoadIN33);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 3, 4, ref m_LoadIN34);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 3, 5, ref m_LoadIN35);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 3, 6, ref m_LoadIN36);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD140IP, 3, 7, ref m_LoadIN37);//

            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD122IP, 0, 0, ref m_LoadIN40);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD122IP, 0, 1, ref m_LoadIN41);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD122IP, 0, 2, ref m_LoadIN42);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD122IP, 0, 3, ref m_LoadIN43);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD122IP, 0, 4, ref m_LoadIN44);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD122IP, 0, 5, ref m_LoadIN45);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD122IP, 0, 6, ref m_LoadIN46);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD122IP, 0, 7, ref m_LoadIN47);//

            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD122IP, 1, 0, ref m_LoadIN50);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD122IP, 1, 1, ref m_LoadIN51);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD122IP, 1, 2, ref m_LoadIN52);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD122IP, 1, 3, ref m_LoadIN53);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD122IP, 1, 4, ref m_LoadIN54);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD122IP, 1, 5, ref m_LoadIN55);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD122IP, 1, 6, ref m_LoadIN56);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD122IP, 1, 7, ref m_LoadIN57);//

            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD122_2IP, 0, 0, ref m_LoadIN60);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD122_2IP, 0, 1, ref m_LoadIN61);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD122_2IP, 0, 2, ref m_LoadIN62);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD122_2IP, 0, 4, ref m_LoadIN64);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD122_2IP, 0, 5, ref m_LoadIN65);//
            MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD122_2IP, 0, 6, ref m_LoadIN66);//
            //if (m_LoadIN66 == 1)
            //    LED_IN66.Value = true;
            //else
            //    LED_IN66.Value = false;

            if (m_LoadIN43 == 1) m_LoadToughWafer = false;
            //   if (m_LoadIN40 == 0)
            //      m_CasstteWaferOut = false;

            uint IO_status = 0;
            MNet.M1A._mnet_m1a_get_io_status(m_RingNoA, ClineAddr.LoadAX0IP, ref IO_status);     //获取升降模组状态 
            m_Load_XPos = (IO_status >> 13) & 0x01;  //升降模组到位信号
            m_Load_XAlarm = (IO_status >> 1) & 0x1;//升降模组报警
            MNet.M1A._mnet_m1a_get_io_status(m_RingNoA, ClineAddr.LoadAX1IP, ref IO_status);
            m_Load_YHome = Convert.ToByte((IO_status >> 4) & 0x01);  //总压检测
            m_Load_DPos = (IO_status >> 13) & 0x01;  //舌头模组到位信号
            m_Load_DAlarm = (IO_status >> 1) & 0x1;//舌头模组模组报警
        }
        /*********************************************************************************************************
     ** 函数名称 ：fDDPositionStatus
     ** 函数功能 ：DD位置状态记录函数
     ** 入口参数 ：
     ** 出口参数 ：              
     *********************************************************************************************************/
        public void fDDPositionStatus(uint Status)
        {
            if (DDInit == false)
            {
                switch (Status)//
                {

                    case 0://旋转90°      011
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 7, 0);    //启动关闭
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 4, 1);    //M0
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 5, 1);    //M1
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 6, 0);   //M2  
                        System.Threading.Thread.Sleep(20);//延时2ms
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 7, 1);//启动打开
                        break;
                    case 1://旋转22.5°    010
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 7, 0);    //启动关闭            
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 4, 0);    //M0
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 5, 1);    //M1
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 6, 0);   //M2
                        System.Threading.Thread.Sleep(20);//延时2ms
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 7, 1);//启动打开
                        break;
                    case 2://旋转67.5°    100    
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 7, 0);    //启动关闭          
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 4, 0);    //M0
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 5, 0);    //M1
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 6, 1);   //M2     
                        System.Threading.Thread.Sleep(20);//延时2ms
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 7, 1);//启动打开 
                        break;
                    case 5: //【新增】测功率位     101  
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 7, 0);    //启动关闭          
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 4, 1);    //M0
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 5, 0);    //M1
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 6, 1);   //M2     
                        System.Threading.Thread.Sleep(20);//延时2ms
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 7, 1);//启动打开 
                        break;
                    case 6://【新增】测功率回     110
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 7, 0);    //启动关闭          
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 4, 0);    //M0
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 5, 1);    //M1
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 6, 1);   //M2     
                        System.Threading.Thread.Sleep(20);//延时2ms
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 7, 1);//启动打开 
                        break;
                }
            }
        }

        /*********************************************************************************************************
        ** 函数名称 ：fOutWaferPositionStatus
        ** 函数功能 ：出料旋转臂位置状态记录函数
        ** 入口参数 ：
        ** 出口参数 ：              
        *********************************************************************************************************/
        public void fOutWaferPositionStatus(uint Status)
        {

            if (DDInit == false)
            {

                Motion.mAcm_AxMoveRel(ax2Handle[pci1245l.Axis_THREE], 180 * FormHMI.pci1245l.RotateRatio);
                //    Motion.mAcm_AxMoveAbs(ax2Handle[pci1245l.Axis_THREE], 0);
                //else
                //    Motion.mAcm_AxMoveRel(ax2Handle[pci1245l.Axis_THREE], 180 * FormHMI.pci1245l.RotateRatio);
            }
        }

        /*********************************************************************************************************
       ** 函数名称 ：fSystemHome
       ** 函数功能 ：系统归零
       ** 入口参数 ：
       ** 出口参数 ： 
       *********************************************************************************************************/
        void fSystemHome()
        {
            ushort GetState1 = 0, GetState3 = 0, GetState4 = 0;
            uint IO_status = 0;
            int Rotatem_VelLow = (int)pci1245l.RotateRatio * 100, Rotatem_VelHigh = (int)pci1245l.RotateRatio * pci1245l.RotateSpeed;
            double Bufferm_VelLow = pci1245l.BufferRatio * 10, Bufferm_VelHigh = pci1245l.BufferRatio * pci1245l.BufferSpeed;
            //double DDm_VelLow = 5 * pci1245l.DDRatio, DDm_VelHigh = pci1245l.DDSpeed * pci1245l.DDRatio;
            /////////////////////系统（主机+接驳台）归零运动///////////////////
            if ((m_SysReset == 1) && (m_XHomeFlag == 4)/* && (m_DHomeFlag == 4) */&& (m_YHomeFlag == 3) && (m_ZHomeFlag == 3) && (m_LoadHomeFlag == 4))
            {
                m_ErrorFlag = 2; m_MainBreak = false;
                fALAM("温馨提示", "系统（主机和接驳台）复位已经完成！");
                m_HomeFinished = 1;
                g_LoadHome = true; g_InRotateHome = true;
                LogClass.Text = "系统复位完成！"; this.Enabled = true; m_SysReset = 0; m_SysResetFlag = true;
                m_XHomeFlag = 0; m_DHomeFlag = 0; m_YHomeFlag = 0; m_ZHomeFlag = 0; m_UHomeFlag = 0; m_LoadHomeFlag = 5;
                g_InRotateMotorPosition = 0;
                TextShow.Text = "";

                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 5, 1); //进料打齐复位
                System.Threading.Thread.Sleep(50);
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 5, 0); //进料打齐复位
                System.Threading.Thread.Sleep(2);
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 6, 0); //进料打齐
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 4, 0);
            }
            ////////////主机归零运动////////////////////////////////
            if ((m_XHomeFlag == 4)/* && (m_DHomeFlag == 4) */&& (m_YHomeFlag == 3) && (m_ZHomeFlag == 3) && (m_UHomeFlag == 3) && (m_SysReset == 0))
            {
                m_ErrorFlag = 2; m_MainBreak = false;
                fALAM("温馨提示", "主机复位已经完成！");
                m_XHomeFlag = 0; m_DHomeFlag = 0; m_YHomeFlag = 0; m_ZHomeFlag = 0; m_UHomeFlag = 0; g_InRotateMotorPosition = 0;
            }
            ///进料Buffer归零完成
            switch (m_XHomeFlag)
            {
                case 1:
                    Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_ONE], ref GetState1);  //等待Buffer运动完成 
                    if (GetState1 == 3)
                    {
                        Motion.mAcm_AxResetError(ax2Handle[pci1245l.Axis_ONE]);
                        System.Threading.Thread.Sleep(200);

                    }
                    else if (GetState1 == 1)
                    {
                        Motion.mAcm_AxHome(ax2Handle[pci1245l.Axis_ONE], 0, 1);
                        m_XHomeFlag = 2;
                    }
                    break;
                case 2:
                    Motion.mAcm_AxGetState(FormHMI.ax2Handle[pci1245l.Axis_ONE], ref GetState1);  //等待Buffer运动完成 
                    if (GetState1 == 1)
                    {
                        if (m_InBufferHomeCount < 5)
                            m_InBufferHomeCount++;
                        else
                        {
                            m_InBufferHomeCount = 0;
                            if ((m_AIN0 == 1) || (g_LoadBuffer == 0))//有硅片正处于buffer之下，归零运动完成
                                m_XHomeFlag = 3;
                            else
                            {
                                g_LoadBuffer--;
                                Motion.mAcm_AxMoveRel(ax2Handle[pci1245l.Axis_ONE], 5 * pci1245l.BufferRatio);
                            }
                        }
                    }
                    else
                        m_InBufferHomeCount = 0;
                    break;
                case 3:
                    Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_ONE], ref GetState1);  //等待Buffer运动完成 
                    if (GetState1 == 1)
                    {
                        Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_ONE], (uint)PropertyID.PAR_AxVelLow, Bufferm_VelLow);
                        Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_ONE], (uint)PropertyID.PAR_AxVelHigh, Bufferm_VelHigh);
                        m_XHomeFlag = 4;

                    }
                    break;
            }
            ///进料旋转臂归零完成
            switch (m_YHomeFlag)
            {

                case 1:
                    Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_TWO], ref GetState3);  //等待旋转臂运动完成 
                    if (GetState3 == 3)
                    {
                        Motion.mAcm_AxResetError(ax2Handle[pci1245l.Axis_TWO]);
                        System.Threading.Thread.Sleep(200);

                    }
                    else if (GetState3 == 1)
                    {
                        Motion.mAcm_AxHome(ax2Handle[pci1245l.Axis_TWO], 0, 1);
                        m_YHomeFlag = 2;
                    }
                    break;
                case 2:
                    Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_TWO], ref GetState3);  //等待旋转臂运动完成 
                    if (GetState3 == 1)
                    {
                        Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_TWO], (uint)PropertyID.PAR_AxVelLow, Rotatem_VelLow);
                        Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_TWO], (uint)PropertyID.PAR_AxVelHigh, Rotatem_VelHigh);
                        m_YHomeFlag = 3;
                        g_InRotateMotorPosition = 0;
                    }
                    break;
            }
            ///出料旋转臂归零完成
            switch (m_ZHomeFlag)
            {

                case 1:
                    Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_THREE], ref GetState3);  //等待旋转臂运动完成 
                    if (GetState3 == 3)
                    {
                        Motion.mAcm_AxResetError(ax2Handle[pci1245l.Axis_THREE]);
                        System.Threading.Thread.Sleep(200);

                    }
                    else if (GetState3 == 1)
                    {
                        Motion.mAcm_AxHome(ax2Handle[pci1245l.Axis_THREE], 0, 1);
                        m_ZHomeFlag = 2;
                    }
                    break;
                case 2:
                    Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_THREE], ref GetState3);  //等待旋转臂运动完成 
                    if (GetState3 == 1)
                    {
                        Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_THREE], (uint)PropertyID.PAR_AxVelLow, Rotatem_VelLow);
                        Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_THREE], (uint)PropertyID.PAR_AxVelHigh, Rotatem_VelHigh);
                        m_ZHomeFlag = 3;
                        g_OutRotateMotorPosition = 0;
                    }
                    break;
            }
            ///DD马达归零完成
            switch (m_UHomeFlag)
            {
                case 1:
                    if (m_UIN1 == 0)//DD马达开始运动
                        m_UHomeFlag = 2;
                    break;
                case 2:
                    if (m_UIN1 == 1)//DD马达运动完成
                    {
                        m_UHomeFlag = 3;

                    }
                    break;
            }

            //////////上料接驳台/////////////
            switch (m_LoadHomeFlag)
            {
                case 1:
                    MNet.M1A._mnet_m1a_motion_done(m_RingNoA, ClineAddr.LoadAX0IP, ref GetState4);//获取轴状态  
                    if (GetState4 == 0)//如果轴就绪
                    {
                        if (m_LoadHomeDelay < 10)//延时200ms
                            m_LoadHomeDelay++;
                        else
                        {
                            m_LoadHomeDelay = 0;
                            MNet.M1A._mnet_m1a_set_home_config(m_RingNoA, ClineAddr.LoadAX0IP, 0, 0, 0, 0, 0);
                            MNet.M1A._mnet_m1a_start_home_move(m_RingNoA, ClineAddr.LoadAX0IP, 0);

                            //Motion.mAcm_AxHome(FormHMI.ax3Handle[FormHMI.pci1245l.Axis_FIVE], 0, 1);
                            m_LoadHomeFlag = 2;
                        }
                    }
                    break;
                case 2:
                    MNet.M1A._mnet_m1a_motion_done(m_RingNoA, ClineAddr.LoadAX0IP, ref GetState4);//获取轴状态 
                    if (GetState4 == 0)
                    {
                        if (m_LoadHomeDelay < 10)//延时200ms
                            m_LoadHomeDelay++;
                        else
                        {
                            m_LoadHomeDelay = 0;
                            MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNoA, ClineAddr.LoadAX0IP, pci1245l.ElevatorRatio * 10, pci1245l.ElevatorRatio * pci1245l.ElevatorMaxSpeed,
                                       0.1f, 0.1f);//升降模组
                            MNet.M1A._mnet_m1a_start_a_move(m_RingNoA, ClineAddr.LoadAX0IP, (int)(FormHMI.g_SysParam[20] * FormHMI.pci1245l.ElevatorRatio));//到起始位置
                            m_LoadHomeFlag = 3;
                        }
                    }
                    break;
                case 3:
                    MNet.M1A._mnet_m1a_motion_done(m_RingNoA, ClineAddr.LoadAX0IP, ref GetState4);//获取轴状态 
                    if (GetState4 == 0)
                    {
                        m_LoadKeyIn = 0;
                        if (m_SysReset == 0)
                        {
                            m_LoadHomeFlag = 5;
                            m_ErrorFlag = 2;
                            fALAM("温馨提示", " 1.“上料接驳台”升降模组已完成归零运动！\n 2.如果升降模组中有花篮，请先点击“上料出花篮”按钮！");
                        }
                        else
                            m_LoadHomeFlag = 4;
                    }
                    break;
            }
        }

        /*********************************************************************************************************
       ** 函数名称 ：ContextMenu_MainOpen_Click
       ** 函数功能 ：右键打开主机
       ** 入口参数 ：
       ** 出口参数 ： 
       *********************************************************************************************************/
        private void ContextMenu_MainOpen_Click(object sender, EventArgs e)
        {
            FormMain oMainForm = new FormMain();
            oMainForm.StartPosition = FormStartPosition.CenterScreen;
            oMainForm.ShowDialog();

        }
        /*********************************************************************************************************
       ** 函数名称 ：ContextMenu_LoadOpen_Click
       ** 函数功能 ：右键打开上料接驳台
       ** 入口参数 ：
       ** 出口参数 ： 
       *********************************************************************************************************/
        private void ContextMenu_LoadOpen_Click(object sender, EventArgs e)
        {
            FormLoad Load = new FormLoad();
            Load.StartPosition = FormStartPosition.CenterScreen;
            Load.ShowDialog();

        }

        /*********************************************************************************************************
      ** 函数名称 ：MenuMainReset_Click
      ** 函数功能 ：主机复位
      ** 入口参数 ：
      ** 出口参数 ： 
      *********************************************************************************************************/
        private void MenuMainReset_Click(object sender, EventArgs e)
        {
            DialogResult ResultTemp;
            double Rotatem_VelLow = pci1245l.RotateRatio * 1, Rotatem_VelHigh = pci1245l.RotateRatio * 10;

            double Bufferm_VelLow = pci1245l.BufferRatio * 1, Bufferm_VelHigh = pci1245l.BufferRatio * 20;
            double DDm_VelLow = 2 * pci1245l.DDRatio, DDm_VelHigh = 20 * pci1245l.DDRatio;
            if (m_ZPOS == 0)  //DD马达或者旋转臂没有送电
            {
                m_ErrorFlag = 2;
                fALAM("温馨提示", "DD马达或者旋转臂没有送电或者处于异常状态，请检查！");
            }
            else
            {

                Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_THREE], pci1245l.BuzzerOut, 1);
                System.Threading.Thread.Sleep(200);//延时200ms
                Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_THREE], pci1245l.BuzzerOut, 0);
                ResultTemp = MessageBox.Show(this, "请确认是否主机各轴复位，如果台面上有硅片，请先清理硅片，再复位！", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (ResultTemp == DialogResult.OK)   //表示点击确认        
                {
                    m_LoadHandOut = false;
                    Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_TWO], 6, 0);//  下料旋转气缸回原位
                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_TWO], 5, 0);  //上料A吸附关闭
                    Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_ONE], 5, 0);//上料A吸盘反吹关闭
                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_TWO], 4, 0);  //上料B吸附关闭
                    Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_ONE], 4, 0);//上料B吸盘反吹关闭  

                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 4, 0);  //下料吸附A关闭
                    Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_TWO], 4, 0);  //下料反吹A关闭
                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 5, 0);  //下料吸附B关闭
                    Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 6, 0);  //下料反吹B关闭
                    /////////////速度设置/////////////////////
                    /////////设置Buffer速度
                    Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_ONE], (uint)PropertyID.PAR_AxVelLow, Bufferm_VelLow);
                    Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_ONE], (uint)PropertyID.PAR_AxVelHigh, Bufferm_VelHigh);
                    ////////设置进料旋转轴速度
                    Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_TWO], (uint)PropertyID.PAR_AxVelLow, Rotatem_VelLow);
                    Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_TWO], (uint)PropertyID.PAR_AxVelHigh, Rotatem_VelHigh);
                    ////////设置出料旋转轴速度
                    Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_THREE], (uint)PropertyID.PAR_AxVelLow, Rotatem_VelLow);
                    Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_THREE], (uint)PropertyID.PAR_AxVelHigh, Rotatem_VelHigh);
                    /////设置DD速度
                    Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_FOUR], (uint)PropertyID.PAR_AxVelLow, DDm_VelLow);
                    Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_FOUR], (uint)PropertyID.PAR_AxVelHigh, DDm_VelHigh);

                    if (m_XHome == 1)
                    {
                        Motion.mAcm_AxMoveRel(ax2Handle[pci1245l.Axis_ONE], 5 * pci1245l.BufferRatio);
                        m_XHomeFlag = 1;
                    }
                    //else
                    //{
                    //    Motion.mAcm_AxHome(ax2Handle[pci1245l.Axis_ONE], 0, 1);
                    //    m_XHomeFlag = 2;
                    //}
                    System.Threading.Thread.Sleep(100);//延时100ms
                    if (m_YHome == 1)
                    {
                        Motion.mAcm_AxMoveRel(ax2Handle[pci1245l.Axis_TWO], 20 * pci1245l.RotateRatio);
                        m_YHomeFlag = 1;
                    }
                    //else 
                    //{
                    //    Motion.mAcm_AxHome(ax2Handle[pci1245l.Axis_TWO], 0, 1);
                    //    m_YHomeFlag = 2;
                    //}

                    System.Threading.Thread.Sleep(100);//延时100ms
                    if (m_RotateHome == 1)//已经在原点位置
                    {
                        Motion.mAcm_AxMoveRel(ax2Handle[pci1245l.Axis_THREE], 20 * pci1245l.RotateRatio);
                        m_ZHomeFlag = 1;
                    }
                    else
                    {
                        Motion.mAcm_AxHome(ax2Handle[pci1245l.Axis_THREE], 0, 1);
                        m_ZHomeFlag = 2;
                    }
                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 7, 0);   //启动关闭
                    System.Threading.Thread.Sleep(100);//延时100ms
                    /////////DD归零
                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 6, 0);  //低                         
                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 4, 1);  //低
                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 5, 0);//高
                    // System.Threading.Thread.Sleep(2);//延时2ms
                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 7, 1);//启动打开
                    g_CWPositionFlag = 0;
                    m_XHomeFlag = 1; m_YHomeFlag = 1; m_UHomeFlag = 1;
                    g_LoadBuffer = g_InBufferTotal;

                }
            }
        }

        /*********************************************************************************************************
     ** 函数名称 ：timerBuzzer_Tick
     ** 函数功能 ：报警定时器
     ** 入口参数 ：
     ** 出口参数 ： 
     *********************************************************************************************************/
        private void timerBuzzer_Tick(object sender, EventArgs e)
        {
            switch (m_ErrorFlag)
            {
                ////////异常状况
                case 1:
                    if (m_BuzzerCount < 55)
                        m_BuzzerCount++;
                    else
                    {
                        m_ErrorFlag = 0; m_BuzzerCount = 0;
                        timerBuzzer.Enabled = false;//关闭报警定時器                        
                    }
                    break;
                ///////正常提示
                case 2:
                    if (m_BuzzerCount < 1)
                        m_BuzzerCount++;
                    else
                    {
                        m_ErrorFlag = 0; m_BuzzerCount = 0;
                        timerBuzzer.Enabled = false;//关闭报警定時器  
                        if (m_IniBuzzer == 0)
                        {
                            m_IniBuzzer = 1;
                            timerRead.Enabled = true;//打开主定时 
                        }
                    }
                    break;
            }

            if (m_AlamBuzzer)
            {
                m_AlamBuzzer = false;
                Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_THREE], pci1245l.BuzzerOut, 0);
                Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_THREE], pci1245l.RedOut, 0);

            }
            else
            {
                m_AlamBuzzer = true;
                //	if(BuzzerMax<2)
                //	{
                //		BuzzerMax++;
                Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_THREE], pci1245l.BuzzerOut, 1);
                //	}
                Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_THREE], pci1245l.RedOut, 1);

            }
            if (m_IniBuzzer == 1)
            {
                m_IniBuzzer = 2;
                MessageBox.Show(this, "如果设备初次上电，或者断过电源，请先将主机“复位”，再进行正常操作！", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                fOffsetIni();//补偿设置     
                timerCheck.Enabled = true;
            }
        }
        /*********************************************************************************************************
        ** 函数名称 ：OPTSwitch_StateChanged
        ** 函数功能 ：关闸开关
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        private void OPTSwitch_StateChanged(object sender, MouseEventArgs e)
        {
            if (KEY_OPTSwitch.Value)
                LogClass.Text = "光闸打开";
            else
                LogClass.Text = "光闸关闭";
            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 5, Convert.ToByte(KEY_OPTSwitch.Value));

        }
        /*********************************************************************************************************
        ** 函数名称 ：LoadHandOut_Click
        ** 函数功能 ：手动上料出料
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        private void LoadHandOut_Click(object sender, EventArgs e)
        {
            DialogResult ResultTemp;
            if (m_LoadHomeFlag == 5)
            {
                //	下压气缸下压到位 
                if (((m_LoadIN54 == g_IN54) || (m_LoadIN51 == 0)) && (m_LoadIN43 == 0) && (m_LoadIN42 == 0))//花篮已经处于升降模组正确位置   
                {
                    ResultTemp = MessageBox.Show(this, "请确认是否需要“上料”退出花篮？", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (ResultTemp == DialogResult.OK)   //表示点击确认        
                    {
                        m_LoadKeyIn = 11; m_LoadHandOut = true;
                    }
                }
                else
                {
                    m_ErrorFlag = 2;
                    fALAM("温馨提示", "“上料”接驳台，升降模组中无花篮，或者上下对射式传感器被遮挡，故不执行出花篮动作！");
                }
            }
            else
            {
                m_ErrorFlag = 2;
                fALAM("温馨提示", "“上料”接驳台，升降模组正在复位中，故不执行出花篮动作！");
            }

        }

        /*********************************************************************************************************
            ** 函数名称 ：timerPower_Tick
            ** 函数功能 ：功率测量定时器
            ** 入口参数 ：
            ** 出口参数 ： 
            *********************************************************************************************************/
        private void timerPower_Tick(object sender, EventArgs e)
        {
            string SendDataPower = "*OUTPM :";
            if ((m_MeasureCount < 30) && (m_PowerStep == 3))
                ComPower.Write(SendDataPower);
            /*		 //发送指令  
               /////////////////功率测量开始////
           ///////////////////手动测功率///////////////////////////////////////////////		
            if((m_DDPOS==1)&&(m_PowerOut==1))  //DD马达运动完成
            {
                m_PowerOut = 0;
               LogClass.Text="功率测试，打开Gate";  
               Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 5, 1);   //光闸打开
               KEY_OPTSwitch.Value=true;
               axMMIO1.SetOutput(15, 0); //打开Gate 
               //axMMMark1.LaserOn();        
               m_LaserPowerFlag=1;   
            }
            //功率开始测量后，最大延时20s，防止功率计一直被高功率照射   
            if((m_LaserPowerFlag>0)&&(m_LaserPowerFlag<40))   
                m_LaserPowerFlag++;
            else if(m_LaserPowerFlag==40)
            {
               Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 5, 0);   //关闭光闸
               KEY_OPTSwitch.Value=false;
               m_PowerOut=3;		 m_LaserPowerFlag=0;
               timerPower.Enabled = false;//关闭定時器
            }
            if((m_YIN3==1)&&(m_PowerOut==3))  //光闸关闭到位 
           {   
               m_PowerOut=0;
               axMMIO1.SetOutput(15, 1); //关闭Gate 
               //axMMMark1.LaserOff();   		
                timerPower.Enabled=false;//关闭定時器
            }	 
		 
           ///////////////////自动测功率///////////////////////////////////////////////	 
           if(g_MainStep<30)//自动状态
            {
            if((m_PowerCounter>0)&&(m_PowerCounter<20))//进入自动测量功率状态 
            {
                if(m_PowerCounter<3)   //延时1.5s
                    m_PowerCounter++; 
                else if(m_PowerCounter==3)
                {
                    Motion.mAcm_AxMoveRel(ax2Handle[pci1245l.Axis_FOUR], 45 * pci1245l.DDRatio);//顺时针45° 
                   m_PowerCounter=4;
                }
		 
                if(m_PowerCounter==4)   //判断汽缸是否伸出到位
                {
                    if(m_DDPOS==1)	 //DD旋转完成
                    {
                       LogClass.Text="打开光闸";  
                    Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 5, 1);   //光闸打开  
                       KEY_OPTSwitch.Value=true;
                       m_PowerCounter=5;	   m_LaserPowerFlag=0;
                    }
                    else
                        m_LaserPowerFlag++;
			
			 
                }
               if(m_PowerCounter==5)  //光闸打开到位 
               {  
			
                   if(m_YIN2==1)
                   {			
                   m_PowerCounter=6;   m_LaserPowerFlag=0;
                   }
                    else
                        m_LaserPowerFlag++;
                    if(m_LaserPowerFlag>4)  //2s
                    {		fBoxStop() ; 
                           m_ErrorFlag=2;  
                           fALAM("温馨提示","没有正确接受到光闸打开信号，请检测！"); 
                           m_LaserPowerFlag=0;
                    }
                }
		
               if(m_PowerCounter>5) //进入正常测量功率状态
                    m_PowerCounter++;
            }
            else	 //自动测量完成,关闭激光，缩回汽缸
            { 	
                   if(m_PowerCounter==20)	  //关闭Gate，并缩回汽缸
                 {		
                   Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 5, 1); ;
                   Motion.mAcm_AxMoveRel(ax2Handle[pci1245l.Axis_FOUR], -45 * pci1245l.DDRatio);//逆时针45° 
                   m_PowerCounter=21;   
                 }
                ///////////判断汽缸是否缩回
                if(m_PowerCounter==21)//
                {
                    if (m_DDPOS == 1)
                {
                    m_Pause = false; m_PowerCounter = 0; m_LaserPowerFlag = 0;
                    m_AutoMoveCounter = 0; m_TouchOnCount = 0;
                   fPauseProcess(m_Pause);
                   timerPower.Enabled = false; //关闭定時器  
                }
                else
                    m_LaserPowerFlag++;
        
                }
            }
          }    */

            switch (m_PowerStep)
            {
                case 1:

                    axMMMark1.JumpTo(0, 0);
                    fDDPositionStatus(5);   //20220809测量功率开始
                    m_PowerStep = 2;
                    break;

                case 2:
                    if (m_UIN1 == 1)
                    {
                        KEY_EndMeasure.Enabled = true;
                        LogClass.Text = "打开光闸";
                        Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 5, 1);   //光闸打开  
                        KEY_OPTSwitch.Value = true;
                    }
                    m_PowerStep = 3;
                    break;

                case 3:
                    if (m_MeasureCount < 30)
                        m_MeasureCount++;
                    else
                    {
                        m_MeasureCount = 0;
                        LogClass.Text = "关闭光闸";
                        Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 5, 0);   //光闸关闭  
                        //axMMMark1.LaserOff();
                        axMMIO1.SetOutput(15, 1); //关闭Gate
                        KEY_OPTSwitch.Value = false;
                        m_PowerStep = 4;
                    }
                    break;

                case 4:
                    KEY_EndMeasure.Enabled = false;
                    AutoRun.Visible = true;
                    AutoPause.Visible = true;
                    fDDPositionStatus(6);       //20220809测量功率结束    
                    ++g_CWPositionFlag;
                    if (g_CWPositionFlag > 3)
                        g_CWPositionFlag = 0;
                    m_PowerStep = 0;
                    break;

            }
        }
        /*********************************************************************************************************
        ** 函数名称 ：fPauseProcess
        ** 函数功能 ：暂停处理
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        void fPauseProcess(bool PauseStatus)
        {
            ushort GetStateC = 0;
            int IsMarking;
            if (PauseStatus)
            {
                if (m_PowerCounter > 0)
                    LogClass.Text = "自动检测激光器输出功率";
                else
                    LogClass.Text = "暂停";
                /////////判断是否正在雕刻
                do
                {
                    IsMarking = axMMMark1.IsMarking();
                }
                while (IsMarking == 1);


                Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 5, 0); //关闭光闸  
                KEY_OPTSwitch.Value = false;
                KEY_LoadHandOut.Enabled = true;
                KEY_StartMeasure.Enabled = true;
                //MMMARKLib__DMMMarkLaserOn (MarkActiveXHandle, NULL,&Return_Value ); //打开Gate
                axMMIO1.SetOutput(15, 0);
                //axMMMark1.LaserOn(); //打开Gate
                System.Threading.Thread.Sleep(300);//延时300ms
                Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_ONE], 3, ref m_AIN3); //下游设备要片信号
                if ((m_AIN3 == 1) && (m_ReadyNow == 1))
                {
                    Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_ONE], ref GetStateC);  //出料电机3运动判断
                    if (GetStateC == 1)
                        Motion.mAcm_AxMoveRel(ax1Handle[pci1245l.Axis_THREE], pci1245l.OUT1Distance * pci1245l.WaferRatio);
                }
                m_YOUT7 = 0;
                Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_TWO], 7, m_YOUT7);//本机清除Ready信号
                m_ReadyNow = 0;


            }
            else
            {
                LogClass.Text = "继续自动运行";
                m_LaserRelease = 0;
                Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 4, 0);		 //关闭照明灯
                KEY_StartMeasure.Enabled = false;
                KEY_LoadHandOut.Enabled = false;
            }

        }
        //AGV报警
        void fAgvAlarm()
        {
            if (g_NotAgvAlarm)
                return;

            //Agv报警
            if (g_AgvAlarm > 0)
            {
                timerAGV.Enabled = false;
                m_ErrorFlag = 1;

                if ((g_AgvAlarm == 2) || (g_AgvAlarm == 1))
                {
                    // MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 6, 0);//出料伸出
                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 3, 0);////出花篮电机2反转停止
                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 5, 0);////出花篮电机2运动停止
                    m_LoadOut1run = 0;
                    dataSocketSource1.Bindings[6].Data.Value = 8;
                    fALAM("警告", AlarmMess[32]);
                    dataSocketSource1.Bindings[6].Data.Value = 16;
                    m_OpcData_UnLoad = 16;
                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 6, 0);//出料伸出
                    g_UnLoadAGVCommStep = 10;
                }
                if ((g_AgvAlarm == 3) || (g_AgvAlarm == 4))
                {

                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 0, 0);////进花篮电机1反转停止
                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 0, 0);//进花篮电机1停止
                    m_LoadIn1run = 0;
                    dataSocketSource1.Bindings[4].Data.Value = 8;
                    fALAM("警告", AlarmMess[33]);
                    dataSocketSource1.Bindings[4].Data.Value = 16;
                    m_OpcData_Load = 16;
                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 0, 0); //进料阻挡汽缸1缩回
                    g_LoadAGVCommStep = 10;
                }
                g_AgvAlarm = 0;
                timerAGV.Enabled = true;

            }
        }
        //////////////**************************************************************************************************///////////////////
        ///////////////*************************上料接驳台自动化流程********************////////////////////
        //////////////**************************************************************************************************/////////////////// 
        /*********************************************************************************************************
        ** 函数名称 ：fAutoLoadRun
        ** 函数功能 ：上料接驳台
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        void fAutoLoadRun()
        {
            //uint GetState1 = 0;
            //uint IO_status = 0;
            ushort GetState1 = 20, GetState2 = 20;
            short m_UnloadEmptyNum = (short)(m_LoadIN31 + m_LoadIN32 + m_LoadIN33 + m_LoadIN34 + m_LoadIN30 + m_LoadIN35 + m_LoadIN22 + m_LoadIN23 + m_LoadIN27 + m_LoadIN61);
            Edit_LoadStep.Value = m_LoadKeyIn;
            if ((m_LoadIN56 == 1) || (m_LoadIN62InFlag == 1) || ((m_LoadIN14 == 1) && (g_MainStep < 30)) && (m_LoadHomeFlag == 5)) //进料按钮被按下，进料阻挡判断没被遮挡
            {
                //   m_LoadIN62InFlag = 0;
                if (m_LoadKeyIn == 16)  //停止后，重新开始
                    m_LoadKeyIn = 0;
            }

            if ((m_LoadKeyIn == 0) && (g_Modify == false) && ((g_LoadAGVCommStep < 2) || (g_LoadAGVCommStep == 20)))	   //   
            {
                if (((m_LoadIN56 == 1) || (m_LoadIN62InFlag == 1)) && (m_SysIniReset == 0) && (m_LoadHomeFlag == 5))//||(m_Load_DI01&&(g_MainStep<30))) //进料按钮被按下,软件按钮按下，阻挡sensor被遮挡
                {
                    //m_LoadIN62InFlag = 0;
                    if (Math.Abs(m_Load1220XElevatorStart - m_FirstWaferPos) > 4.0)		//两个位置偏差太大
                    { fBoxStop(); m_ErrorFlag = 1; fALAM("警告", AlarmMess[44]); }
                    else
                    {
                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 6, 1);   //指示灯点亮
                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 2, 0);//阻挡汽缸3伸出,避免花篮重大事故
                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 0, 0);//进料阻挡汽缸1缩回      
                        // MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 5, 0);//出料阻挡汽缸1缩回    
                        m_LoadKeyIn = 1;
                    }
                }
                else if (((m_LoadIN56 == 1) || (m_LoadIN62InFlag == 1)) && (m_LoadHomeFlag != 5))
                {
                    m_ErrorFlag = 1; //m_LoadIN62InFlag = 0;
                    fALAM("警告", AlarmMess[65]);
                    m_LoadKeyIn = 0;
                }
            }
            if (((m_LoadIN56 == 1) || (m_LoadIN62InFlag == 1)) && (m_LoadHomeFlag == 5))
            {
                m_NextWait = 0; m_NextWaitFlag = 0; //m_LoadIN62InFlag = 0;
            }
            // if ((InONOFF == 0) && (Modify == 0)) //上料正常
            if (g_Modify == false) //处于非检修模式
            {
                switch (m_LoadKeyIn)
                {
                    #region case 1://花篮传送电机1开始运动
                    case 1:   //花篮传送电机1开始运动
                        //Motion.mAcm_AxGetState(FormHMI.ax3Handle[FormHMI.pci1245l.Axis_FIVE], ref GetState1);//升降模组停止判断  
                        MNet.M1A._mnet_m1a_motion_done(m_RingNoA, ClineAddr.LoadAX0IP, ref GetState1);

                        if (m_LoadIN42 == 0)//上下对射式没有被遮挡
                        {
                            if ((m_LoadIN11 == 1) && (m_LoadIN51 == 1) && (m_LoadIN52 == 1) && (m_LoadIN53 == 0) && (m_LoadIN54 == 1) && (m_LoadIN55 == 0) && (m_LoadIN47 == 1) && (GetState1 == 0) && (m_Load_XPos == 1))//升降模组中没有花篮 ,且舌头缩回，将升降模组移动到进花篮位置
                            {
                                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 4, 0);//打齐气缸缩回
                                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 2, 0);//托盘电机正转停止                        
                                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
                                m_LoadCCWrun = 0;
                                MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNoA, ClineAddr.LoadAX0IP, pci1245l.ElevatorRatio * 5, FormHMI.pci1245l.ElevatorRatio * FormHMI.pci1245l.ElevatorMaxSpeed,
                                           0.1f, 0.1f);//升降模组
                                MNet.M1A._mnet_m1a_start_a_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, (int)(m_Load1220XElevatorIn * FormHMI.pci1245l.ElevatorRatio));//升降模组运动到进花篮位置
                                m_LoadKeyIn = 2; m_StepContinue3 = 0; m_StepContinue2 = 0; m_PositionContinue = 0;
                            }
                            else if ((m_LoadIN51 == g_IN51) && (m_LoadIN52 == g_IN52) && (m_LoadIN53 == g_IN53) && (m_LoadIN54 == g_IN54) && (m_LoadIN55 == g_IN55) && (m_LoadHomeFlag == 5))//花篮已经处于升降模组正确位置，且交接处没有遮挡   
                            {
                                m_LoadKeyIn = 4;
                                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 6, 0);     //指示灯熄灭  
                            }
                        }
                        else
                        {
                            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 2, 3, 0);//下压气缸缩回
                            if ((m_LoadIN11 == 1) && (GetState1 == 0) && (m_Load_XPos == 1) && (Math.Abs(g_LoadElevationP - (int)(m_Load1220XElevatorIn * pci1245l.ElevatorRatio)) < 0.01) && (m_LoadIN45 == 1))
                            //花篮处于交接位置，且升降模组处于进花篮位置,下压气缸缩回到位
                            {
                                m_StepContinue2 = 0;
                                if ((g_LoadAGVCommStep < 2) || (g_LoadAGVCommStep == 20))
                                {
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 2, 1);//托盘电机正转
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
                                    m_LoadCCWrun = 1;
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 1, 0);//进花篮电机2反转停止
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 1, 1);//进花篮电机2运动 
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 0, 0);////进花篮电机1反转停止
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 0, 1);//进花篮电机1运动
                                    m_LoadIn1run = 1;

                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 1, 0);//阻挡汽缸2伸出
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 2, 1);//阻挡汽缸3缩回
                                    m_LoadKeyIn = 4;
                                }
                            }
                            else
                            {
                                if (m_StepContinue2 < 100)
                                    m_StepContinue2++;
                                else if (m_LoadIN45 == 0)
                                {
                                    m_ErrorFlag = 1;
                                    fALAM("警告", AlarmMess[20]);
                                    m_StepContinue2 = 0;
                                    m_LoadKeyIn = 0;
                                }
                                else if (m_LoadIN11 == 0)
                                {
                                    m_ErrorFlag = 1;
                                    fALAM("警告", AlarmMess[77]);
                                    m_StepContinue2 = 0;
                                    m_LoadKeyIn = 0;
                                }
                            }
                        }
                        m_NextWait = 0; m_NextWaitFlag = 0;

                        break;
                    #endregion
                    #region  case 2://判断是否到达阻挡位置,开始进花篮电机2运动，使得花篮进入到升降模组之中
                    case 2:	   //判断是否到达阻挡位置,开始进花篮电机2运动，使得花篮进入到升降模组之中
                        MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 2, 3, 0);//下压气缸缩回
                        MNet.M1A._mnet_m1a_motion_done(m_RingNoA, ClineAddr.LoadAX0IP, ref GetState1);
                        if ((m_Load_XPos == 1) && (GetState1 == 0) && (Math.Abs(g_LoadElevationP - (int)(m_Load1220XElevatorIn * pci1245l.ElevatorRatio)) < 0.01) && (m_LoadIN42 == 0) && (m_LoadIN45 == 1))//花篮已到阻挡位置,且升降模组到了进花篮位置
                        {
                            m_PositionContinue = 0;
                            if (m_LoadIN14 == 0) //花篮进sensor被遮挡，表示有花篮等待
                            {
                                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 2, 1, 0);//阻挡汽缸2伸出
                                m_StepContinue4 = 0;
                                if (m_LoadIN12 == 1)//阻挡气缸2伸出到位
                                {
                                    m_StepContinue2 = 0;
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 2, 1);//阻挡汽缸3缩回
                                    if (m_LoadIN16 == 1)//第3个阻挡气缸缩回到位
                                    {
                                        m_StepContinue3 = 0;
                                        if (((g_LoadAGVCommStep < 2) || (g_LoadAGVCommStep == 20)))
                                        {
                                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 1, 0);//进花篮电机2反转停止
                                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 1, 1);//进花篮电机2运动
                                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 0, 0);////进花篮电机1反转停止
                                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 0, 1);//进花篮电机1运动
                                            m_LoadIn1run = 1;
                                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 2, 1);//托盘电机正转
                                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
                                            m_LoadCCWrun = 1;
                                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 6, 0);//指示灯熄灭
                                            m_LoadKeyIn = 4; m_StepContinue = 0;
                                        }
                                    }
                                    else
                                    {
                                        if (m_StepContinue3 < 25)
                                            m_StepContinue3++;
                                        else
                                        {
                                            m_StepContinue3 = 0;
                                            m_ErrorFlag = 1;
                                            fALAM("警告", AlarmMess[64]);
                                        }
                                    }
                                }
                                else
                                {
                                    if (m_StepContinue2 < 25)
                                    {
                                        m_StepContinue2++;
                                    }
                                    else
                                    {
                                        m_StepContinue2 = 0;
                                        m_ErrorFlag = 1;
                                        fALAM("警告", AlarmMess[18]);
                                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 1, 0);//阻挡汽缸2伸出
                                        m_LoadKeyIn = 0;
                                    }
                                }
                            }
                            else
                            {
                                if (m_LoadIN15 == 1)//第3个阻挡气缸必须伸出
                                {
                                    m_StepContinue4 = 0;
                                    if ((g_LoadAGVCommStep < 2) || (g_LoadAGVCommStep == 20))
                                    {
                                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 6, 0);   //指示灯熄灭  

                                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 1, 1);//阻挡汽缸2缩回
                                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 2, 0);//阻挡汽缸3伸出
                                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 1, 0);//进花篮电机2反转停止
                                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 1, 1);//进花篮电机2运动
                                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 0, 0);////进花篮电机1反转停止
                                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 0, 1);//进花篮电机1运动
                                        m_LoadIn1run = 1;
                                        m_LoadKeyIn = 3; m_StepContinue = 0;
                                    }
                                }
                                else
                                {
                                    if (m_StepContinue4 < 25)
                                        m_StepContinue4++;
                                    else
                                    {
                                        m_StepContinue4 = 0;
                                        m_ErrorFlag = 1;
                                        fALAM("警告", AlarmMess[39]);
                                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 2, 0);//阻挡汽缸3伸出
                                        m_LoadKeyIn = 0;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (m_PositionContinue < 400)  //延时8s
                                m_PositionContinue++;
                            else if (m_LoadIN45 == 0)
                            {
                                fEMGStop();
                                m_ErrorFlag = 1;
                                fALAM("警告", AlarmMess[20]);
                                m_PositionContinue = 0;
                            }
                            else
                            {
                                fEMGStop();
                                m_ErrorFlag = 1;
                                fALAM("警告", AlarmMess[23]);
                                m_LoadKeyIn = 0; m_LoadHomeFlag = 0; m_PositionContinue = 0;
                            }
                        }
                        break;
                    #endregion
                    #region case 3://判断花篮是否到进料阻挡位
                    case 3:	   //判断花篮是否到进料阻挡位
                        if ((g_LoadAGVCommStep < 2) || (g_LoadAGVCommStep == 20))
                        {
                            if (m_LoadIN14 == 0)//花篮进sensor被遮挡
                            {
                                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 1, 0);//阻挡汽缸2伸出
                                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 1, 0);//进花篮电机2反转停止
                                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 1, 0);//进花篮电机2停止
                                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 0, 0);////进花篮电机1反转停止
                                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 0, 0);//进花篮电机1停止
                                m_LoadIn1run = 0;
                                if ((m_LoadIN12 == 1) && (m_LoadIN45 == 1))//阻挡气缸2伸出到位,下压气缸缩回
                                {
                                    if (m_StepContinue < 10)
                                        m_StepContinue++;
                                    else
                                    {
                                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 2, 1);//阻挡汽缸3缩回
                                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 1, 0);//进花篮电机2反转停止
                                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 1, 1);//进花篮电机2运动
                                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 0, 0);////进花篮电机1反转停止
                                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 0, 1);//进花篮电机1运动
                                        m_LoadIn1run = 1;
                                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 2, 1);//托盘电机正转
                                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
                                        m_LoadCCWrun = 1;

                                        m_LoadKeyIn = 4; m_StepContinue = 0;
                                    }
                                }
                            }
                            else
                            {
                                if (m_StepContinue < 850)  //延时8s
                                    m_StepContinue++;
                                else
                                {
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 2, 0);//阻挡汽缸3伸出
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 1, 0);//阻挡汽缸2伸出，防止故障
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 1, 0);//进花篮电机2反转停止
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 1, 0);//进花篮电机2停止运动
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 0, 0);////进花篮电机1反转停止
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 0, 0);//进花篮电机1停止                               
                                    m_LoadIn1run = 0;
                                    m_StepContinue = 0;
                                    m_LoadKeyIn = 0;
                                }
                            }
                        }

                        break;
                    #endregion
                    #region case 4://判断花篮是否完全处于升降模组正确位置
                    case 4:	   //判断花篮是否完全处于升降模组正确位置，
                        if ((g_LoadAGVCommStep < 2) || (g_LoadAGVCommStep == 20))
                        {
                            if ((m_LoadIN42 == 0) && (m_LoadIN51 == g_IN51) && (m_LoadIN52 == g_IN52) && (m_LoadIN53 == g_IN53) && (m_LoadIN54 == g_IN54) && (m_LoadIN55 == g_IN55))//花篮已经处于升降模组正确位置              
                            {
                                if (m_StepContinue < 15)
                                    m_StepContinue++;
                                else
                                {
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 1, 0);//进花篮电机2停止运动
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 1, 0);//进花篮电机2反转停止
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 0, 0);//进花篮电机1停止
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 0, 0);////进花篮电机1反转停止
                                    m_LoadIn1run = 0;
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 2, 0);//托盘电机正转停止
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
                                    m_LoadCCWrun = 0;

                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 3, 1);//下压气缸伸出
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 2, 0);//阻挡升降汽缸3伸出

                                    if (g_CyAlign == 0)
                                    {
                                        //不启用花篮打齐
                                        m_LoadKeyIn = 5;
                                        m_StepContinue = 0;
                                    }
                                    else
                                    {
                                        //[新增]启用花篮打齐
                                        m_LoadKeyIn = 25;
                                        m_StepContinue = 0;
                                    }
                                }
                            }
                            else
                            {
                                m_StepContinue = 0;
                                if (m_LoadInErrorDelay < 350)
                                {
                                    m_LoadInErrorDelay++;
                                }
                                else
                                {
                                    m_LoadInErrorDelay = 0;
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 1, 0);//进花篮电机2停止运动
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 1, 0);//进花篮电机2反转停止
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 0, 0);//进花篮电机1停止
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 0, 0);////进花篮电机1反转停止
                                    m_LoadIn1run = 0;
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 2, 0);//托盘电机正转停止
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
                                    m_LoadCCWrun = 0;
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 2, 0);//阻挡汽缸3伸出
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 1, 0);//阻挡汽缸2伸出，防止故障
                                    m_ErrorFlag = 1;
                                    fALAM("警告", AlarmMess[19]);
                                    m_LoadKeyIn = 11;

                                }
                            }
                        }
                        break;
                    #endregion

                    #region case 25://[新增]开始上升花篮打齐位置
                    case 25:	   //[新增]开始上升花篮打齐位置
                        //下压和固紧到位 ,花篮托盘sensor没有被遮挡 ，且舌头缩回到位，硅片离开sensor没有被遮挡,阻挡气缸顶起m_Load_DI18
                        m_LoadInErrorDelay = 0;
                        if ((m_LoadIN43 == 0) && (m_LoadIN42 == 0) && (m_LoadIN15 == 1) && (m_LoadIN47 == 1) && (m_LoadIN44 == 1))
                        {
                            if ((g_CyAlign == 0) || (m_LoadIN65 == 1))  //没有启用花篮打齐 或者 花篮打齐缩回
                            {
                                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 2, 0);//托盘电机正转停止
                                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
                                m_LoadCCWrun = 0;
                                MNet.M1A._mnet_m1a_start_a_move(m_RingNoA, ClineAddr.LoadAX0IP, (int)(m_AlignPos * pci1245l.ElevatorRatio));//升降模组运动到打齐位置
                                m_LoadKeyIn = 26;
                                m_StepContinue = 0;
                            }
                            else
                            {
                                if (m_StepContinue < 100)  //延时1S
                                    m_StepContinue++;
                                else
                                {
                                    m_StepContinue = 0;
                                    fBoxStop();
                                    m_ErrorFlag = 1;
                                    fALAM("警告", AlarmMess[81]);    //20221109YP
                                }
                            }
                        }
                        else
                        {
                            if (m_StepContinue < 300)  //延时4s
                                m_StepContinue++;
                            else
                            {
                                m_StepContinue = 0;
                                fBoxStop();
                                m_ErrorFlag = 1;
                                fALAM("警告", AlarmMess[27]);
                                m_LoadKeyIn = 11;
                            }
                        }
                        break;
                    #endregion
                    #region case 26://[新增]判断花篮是否完全处于升降模组正确位置，打齐气缸伸出
                    case 26://判断花篮是否完全处于升降模组正确位置，打齐气缸伸出
                        if ((Math.Abs(g_LoadElevationP - (int)(m_AlignPos * pci1245l.ElevatorRatio)) < 0.01) && (m_Load_XPos == 1) && (m_LoadIN15 == 1))
                        {
                            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 4, 1);//20220715   打齐气缸伸出
                            if (m_StepContinue < 50)  //延时1S
                                m_StepContinue++;
                            else
                            {
                                m_LoadKeyIn = 27;
                                m_StepContinue = 0;
                            }
                        }
                        break;
                    #endregion
                    #region case 27://[新增]判断花篮是否完全处于升降模组正确位置，打齐气缸缩回
                    case 27://判断花篮是否完全处于升降模组正确位置，打齐气缸缩回
                        if ((Math.Abs(g_LoadElevationP - (int)(m_AlignPos * pci1245l.ElevatorRatio)) < 0.01) && (m_Load_XPos == 1) && (m_LoadIN15 == 1))
                        {
                            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 4, 0);//打齐气缸缩回
                            if (m_LoadIN65 == 1)//气缸缩回成功
                            {
                                m_LoadKeyIn = 5;
                                m_StepContinue = 0;
                            }
                            else
                            {
                                if (m_StepContinue < 100)  //延时1S
                                    m_StepContinue++;
                                else
                                {
                                    m_StepContinue = 0;
                                    fBoxStop();
                                    m_ErrorFlag = 1;
                                    fALAM("警告", AlarmMess[81]);        //20221109YP
                                }
                            }
                        }
                        break;
                    #endregion

                    #region case 5://开始上升到出硅片位置
                    case 5:	   //开始上升到出硅片位置  
                        //下压和固紧到位 ,花篮托盘sensor没有被遮挡 ，且舌头缩回到位，硅片离开sensor没有被遮挡,阻挡气缸顶起m_Load_DI18
                        m_LoadInErrorDelay = 0;
                        if ((m_LoadIN43 == 0) && (m_LoadIN42 == 0) && (m_LoadHomeFlag == 5) && (m_LoadIN15 == 1) && (m_LoadIN47 == 1) && (m_LoadIN44 == 1))
                        {
                            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 4, 0);//打齐气缸缩回
                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 2, 0);//托盘电机正转停止
                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
                            m_LoadCCWrun = 0;

                            MNet.M1A._mnet_m1a_start_a_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, (int)(FormHMI.g_SysParam[3] * FormHMI.pci1245l.ElevatorRatio));//升降模组运动到出硅片位置
                            m_LoadKeyIn = 6; g_LoadWaferQ = 0; m_FirstWaferFlag = 1; m_LoadWaferQTemp = 0; m_InBufferUp = false;
                            m_NoCheck = false;
                            CheckBox_NoCheck.Checked = false;
                            Text_NoCheck.Visible = false;
                        }
                        else
                        {
                            if (m_StepContinue < 300)  //延时4s
                                m_StepContinue++;
                            else if (m_LoadIN15 == 0)
                            {
                                m_ErrorFlag = 1;
                                fALAM("警告", AlarmMess[39]);
                                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 2, 0);//阻挡汽缸3伸出
                                m_LoadKeyIn = 0;
                            }
                            else if (m_LoadIN47 == 0)
                            {
                                m_ErrorFlag = 1;
                                fALAM("警告", AlarmMess[63]);
                                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 4, 0);//舌头缩回
                                m_LoadKeyIn = 0;
                            }
                            else
                            {
                                m_StepContinue = 0;
                                fBoxStop();
                                m_ErrorFlag = 1;
                                fALAM("警告", AlarmMess[27]);
                                m_LoadKeyIn = 11;
                            }
                        }
                        break;
                    #endregion
                    #region case 6://判断花篮是否完全处于升降模组正确位置，开始上升到出硅片位置
                    case 6:	   //判断花篮是否完全处于升降模组正确位置，开始上升到出硅片位置
                        if ((Math.Abs(g_LoadElevationP - (int)(m_Load1220XElevatorStart * pci1245l.ElevatorRatio)) < 0.01) && (m_Load_XPos == 1) && (m_LoadIN15 == 1))//升降模组处于出硅片位置
                        {

                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 2, 0);//托盘电机正转停止
                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
                            m_LoadCCWrun = 0;
                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 4, 1);//伸出舌头
                            MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNoA, ClineAddr.LoadAX0IP, pci1245l.ElevatorRatio * 10, pci1245l.ElevatorRatio * 300, 0.1f, 0.1f);//升降模组120
                            m_LoadKeyIn = 7; m_FirstWaferFlag = 1; m_FirstPostFlag = true; m_InBufferRelease = 1; m_StepContinue = 0; m_KKHaveWafer = false;
                        }
                        break;
                    #endregion
                    #region case 7://达到初始化位置后，需要微调一小格，
                    case 7:	   //达到初始化位置后，需要微调一小格，     
                        MNet.M1A._mnet_m1a_motion_done(m_RingNoA, ClineAddr.LoadAX0IP, ref GetState1);//升降模组运动完成
                        MNet.M1A._mnet_m1a_motion_done(m_RingNoA, ClineAddr.LoadAX1IP, ref GetState2);//硅片传送模组运动完成
                        if (/*((m_LoadIN65 == 1) || (g_CyAlign == 0)) &&*/ (m_LoadIN46 == 1) && (GetState1 == 0) && (GetState2 == 0) && (m_Load_XPos == 1) && (m_Load_DPos == 1) && (m_LoadIN15 == 1) && (!m_LoadOff))//舌头伸出到位  //20221109YP
                        {

                            if (m_LoadIN43 == 1)		//硅片没有离开出料位置
                            {
                                fBoxStop();
                                m_ErrorFlag = 1; fALAM("警告", AlarmMess[17]);
                                m_LoadOff = true; m_KKHaveWafer = false;
                                CheckBox_LoadOff.Checked = m_LoadOff;
                            }
                            else if ((m_KKHaveWafer == true) && (m_LoadIN40 == 1))
                            {
                                fBoxStop();
                                m_ErrorFlag = 1; fALAM("警告", AlarmMess[76]);
                                m_KKHaveWafer = false;
                                m_LoadOff = true;
                                CheckBox_LoadOff.Checked = m_LoadOff;
                            }
                            else
                            {
                                m_KKHaveWafer = false;
                                if (g_LoadWaferQ < m_LoadWaferConst) //传送硅片数量计算
                                {
                                    if (m_FirstPostFlag)
                                    {
                                        //升降模组运动到第一片位置                                 
                                        MNet.M1A._mnet_m1a_start_a_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, (int)(m_FirstWaferPos * pci1245l.ElevatorRatio));
                                        m_FirstPostFlag = false;
                                        m_LoadKeyIn = 8; g_LoadWaferQ++;
                                    }
                                    else   //正常走格子
                                    {
                                        MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD122IP, 0, 0, ref m_LoadIN40);
                                        if (m_LoadIN40 == 1)//舌头中间无料
                                            m_IN11Flag++;
                                        else
                                            m_IN11Flag = 0;
                                        if (m_NoCheck == true) m_IN11Flag = 0;
                                        MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD122IP, 1, 0, ref m_LoadIN50);
                                        if ((m_LoadIN50 == 0) || (m_IN11Flag > 3))
                                        {
                                            if (m_StepContinue < 10)  //延时40*16ms
                                                m_StepContinue++;
                                            else
                                            {
                                                m_StepContinue = 0;
                                                m_ErrorFlag = 1;
                                                if (m_LoadIN50 == 0)
                                                    fALAM("警告", AlarmMess[16]);
                                                else
                                                    fALAM("警告", AlarmMess[30]);
                                                m_IN11Flag = 0;
                                                m_LoadOff = true;
                                                CheckBox_LoadOff.Checked = m_LoadOff;
                                            }
                                        }
                                        else
                                        {
                                            m_WaferCheckStop = false;
                                            if (g_LoadWaferQ == (m_LoadWaferConst / 2))
                                                MNet.M1A._mnet_m1a_start_r_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, (int)(-(m_LoadStepInv + m_LoadWaferBigInv) * pci1245l.ElevatorRatio));
                                            else
                                                MNet.M1A._mnet_m1a_start_r_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, (int)(-m_LoadStepInv * pci1245l.ElevatorRatio));
                                            m_LoadKeyIn = 8; g_LoadWaferQ++; m_StepContinue = 0;

                                        }
                                    }
                                }
                                else   //达到花篮硅片最大值
                                { m_LoadKeyIn = 11; g_LoadWaferQ = 0; }
                            }
                        }
                        break;
                    #endregion
                    #region case 8://升降模组停止运动，如果硅片位置没有料，可以启动归零传输
                    case 8:	   //升降模组停止运动，如果硅片位置没有料，可以启动归零传输  
                        MNet.M1A._mnet_m1a_motion_done(m_RingNoA, ClineAddr.LoadAX0IP, ref GetState1);
                        MNet.M1A._mnet_m1a_motion_done(m_RingNoA, ClineAddr.LoadAX1IP, ref GetState2);
                        if ((m_LoadIN46 == 1) && (GetState1 == 0) && (GetState2 == 0) && (m_Load_DPos == 1) && (m_Load_XPos == 1))	//运动完成,且舌头伸出到位，
                        {
                            if (m_LoadIN40 == 0)//舌头上硅片位置检测
                            {
                                m_FirstWaferFlag = 0;
                            }
                            else  //硅片位置没有硅片
                            {
                                if (m_StepContinue < 15)  //延时20*16ms
                                    m_StepContinue++;
                                else if (m_flagCheck1 == false)
                                {
                                    m_StepContinue = 0;
                                    MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD122IP, 1, 0, ref m_LoadIN50);//
                                    if (m_LoadIN50 == 0) //舌头顶端有硅片
                                    {
                                        m_KKHaveWafer = true;
                                    }
                                    else
                                        m_KKHaveWafer = false;
                                    MNet.M1A._mnet_m1a_start_r_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX1IP, (int)(pci1245l.WaferRatio * m_LoadMotorOffset));//舌头运动
                                    //m_LoadWaferOut = 1;
                                    m_flagCheck1 = true; m_LoadKeyIn = 7; //继续运行	 

                                }

                            }
                        }
                        break;
                    #endregion
                    #region case 11://硅片传输完成，缩回舌头
                    case 11:	   //硅片传输完成，缩回舌头
                        MNet.M1A._mnet_m1a_motion_done(m_RingNoA, ClineAddr.LoadAX0IP, ref GetState1);
                        if ((GetState1 == 0) && (m_Load_XPos == 1))	//运动完成
                        {
                            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 4, 0);//打齐气缸缩回
                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 4, 0);//舌头缩回
                            m_LoadKeyIn = 12;
                        }
                        break;
                    #endregion
                    #region case 12://判断花篮是否完全处于升降模组正确位置，开始上升到出出花篮位置
                    case 12:	   //判断花篮是否完全处于升降模组正确位置，开始上升到出出花篮位置                
                        if ((m_LoadIN47 == 1) && (m_LoadIN42 == 0) && (m_LoadIN43 == 0))//花篮托盘sensor没有被遮挡 ，且舌头缩回到位，硅片离开sensor没有被遮挡 
                        {
                            if (m_StepContinue < 20)
                                m_StepContinue++;
                            else
                            {
                                m_StepContinue2 = 0;
                                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 2, 0);//托盘电机正转停止
                                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
                                m_LoadCCWrun = 0;

                                MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNoA, ClineAddr.LoadAX0IP, pci1245l.ElevatorRatio * 5, pci1245l.ElevatorRatio * pci1245l.ElevatorMaxSpeed,
                                                                   0.1f, 0.1f);//升降模组
                                MNet.M1A._mnet_m1a_start_a_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, (int)(m_Load1220XElevatorOut * pci1245l.ElevatorRatio));
                                m_LoadKeyIn = 13;
                                m_StepContinue = 0;
                            }
                        }
                        else
                        {
                            if (m_StepContinue2 < 250)
                                m_StepContinue2++;
                            else
                            {
                                m_ErrorFlag = 1;
                                fALAM("警告", AlarmMess[63]);
                                m_StepContinue2 = 0;
                            }
                        }
                        break;
                    #endregion
                    #region case 13://到达出花篮位置， 缩回固紧和定位汽缸
                    case 13:	   //到达出花篮位置， 缩回固紧和定位汽缸
                        MNet.M1A._mnet_m1a_motion_done(m_RingNoA, ClineAddr.LoadAX0IP, ref GetState1);
                        if ((Math.Abs(g_LoadElevationP - (int)(m_Load1220XElevatorOut * pci1245l.ElevatorRatio)) < 0.01) && (m_Load_XPos == 1) && (GetState1 == 0))//升降模组处于出花篮位置
                        {
                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 3, 0);//下压气缸缩回
                            //空花篮太多报警
                            //if ((m_LoadIN22 == 1) && (g_UnloadEmptyNum > 2))
                            //{
                            //    m_ErrorFlag = 1; fALAM("警告", AlarmMess[22]);
                            //}
                            //else
                            m_LoadKeyIn = 14;
                        }
                        break;
                    #endregion
                    #region case 14://固紧和定位汽缸回到原位后
                    case 14:	   //固紧和定位汽缸回到原位后 
                        MNet.M1A._mnet_m1a_motion_done(m_RingNoA, ClineAddr.LoadAX1IP, ref GetState1);
                        if ((GetState1 == 0) && ((g_UnLoadAGVCommStep < 2) || (g_UnLoadAGVCommStep == 20)) && (m_LoadOut1run == 0))
                        {
                            if (m_LoadIN45 == 1)  //固紧和定位汽缸回到原位 ,出料阻挡气缸1缩回到位
                            {
                                if (m_LoadIN22 == 0)  //出花篮缓存位置传感器无花篮
                                {
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 3, 1);//托盘电机反转
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 2, 0);//托盘电机正转停止
                                    m_LoadCCWrun = 1;
                                    if (m_UnloadEmptyNum > 5)
                                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 5, 1);//出料阻挡伸出
                                    else
                                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 5, 0);//出料阻挡缩回
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 2, 0);////出花篮电机1反转停止
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 4, 1);////出花篮电机1运动
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 3, 0);////出花篮电机2反转停止
                                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 5, 1);//出花篮电机2运动
                                    m_LoadOut1run = 1;
                                    m_LoadHandOut = false;
                                    m_LoadKeyIn = 15;
                                }
                                else  //如果花篮满盒位置被遮挡
                                {
                                    //花篮放置上，花篮被移开
                                    // MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 5, 1);//出料阻挡缩回
                                    if (m_UnloadEmptyNum < 7)
                                    {

                                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 5, 0);//出料阻挡缩回s
                                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 2, 0);////出花篮电机1反转停止
                                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 4, 1);////出花篮电机1运动
                                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 3, 0);////出花篮电机2反转停止
                                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 5, 1);//出花篮电机2运动
                                        m_LoadOut1run = 1;
                                    }
                                    else
                                    {
                                        if (m_StepContinue5 < 200)  //延时4s
                                            m_StepContinue5++;
                                        else
                                        {
                                            m_StepContinue5 = 0;
                                            if (m_Pause == false)
                                            {
                                                m_ErrorFlag = 1; fALAM("警告", AlarmMess[22]);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (m_StepContinue < 200)  //延时4s
                                    m_StepContinue++;
                                else
                                {
                                    m_StepContinue = 0;
                                    fBoxStop();
                                    m_ErrorFlag = 1;
                                    fALAM("警告", AlarmMess[20]);
                                }
                            }
                        }
                        break;
                    #endregion
                    ////转移到另外一个定时器，避免报警导致错过sensor检测
                    //case 15:	   //传送花篮电机1停止运动，立即进入下一个循环
                    //    if ((m_LoadIN22 == 1) && (m_LoadIN51 == 1) && (m_LoadIN52 == 1) && (m_LoadIN53 == 0) && (m_LoadIN54 == 1) && (m_LoadIN55 == 0) && (m_LoadIN42 == 0) && (m_LoadIN43 == 0))
                    //    {
                    //        if (m_StepContinue < 50)  //延时
                    //            m_StepContinue++;
                    //        else
                    //        {
                    //            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
                    //            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 2, 1);//托盘电机正转停止
                    //            m_LoadCCWrun = false; m_LoadCCWrun = false;
                    //            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 5, 1);//出料阻挡缩回
                    //            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 4, 0);////出花篮电机1运动停止
                    //            m_LoadOut1run = false;
                    //            m_LoadOut2run = false;
                    //            m_StepContinue = 0;
                    //            m_LoadKeyIn = 1;
                    //        }

                    //    }
                    //    else
                    //    {
                    //        if (m_LoadInErrorDelay < 200)
                    //        {
                    //            m_LoadInErrorDelay++;
                    //        }
                    //        else
                    //        {
                    //            m_LoadInErrorDelay = 0;
                    //            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
                    //            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 2, 1);//托盘电机正转停止
                    //            m_LoadCCWrun = false; m_LoadCCWrun = false;
                    //            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 5, 1);//出料阻挡缩回
                    //            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 4, 0);////出花篮电机运动停止
                    //            m_LoadOut1run = false;
                    //            m_LoadOut2run = false;
                    //            m_ErrorFlag = 1;
                    //            fALAM("警告", AlarmMess[57]);
                    //            m_LoadKeyIn = 1;
                    //        }

                    //    }
                    //    break;
                }
            }
            ////////当花篮进入到可进出硅片位置后，启动下一个花篮等待
            if ((m_LoadKeyIn > 7) && (m_LoadKeyIn < 16) && (m_NextWaitFlag == 0) && (m_LoadIN42 == 0) && ((g_LoadAGVCommStep < 2) || (g_LoadAGVCommStep == 20)))
            {
                switch (m_NextWait)
                {
                    case 0:
                        if ((m_LoadIN14 == 1) && (m_LoadIN15 == 1)) //花篮进sensor没有被遮挡，进料阻挡气缸3伸出
                        {
                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 2, 0);//阻挡汽缸3伸出
                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 1, 1);//阻挡汽缸2缩回
                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 0, 0);////进花篮电机1反转停止
                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 0, 1);//进花篮电机1运动
                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 1, 0);//进花篮电机2反转停止
                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 1, 1);//进花篮电机2运动
                            m_LoadIn1run = 1;
                            m_NextWait = 1; m_WaitContinue = 0;
                        }
                        else
                            m_NextWaitFlag = 1;
                        break;
                    case 1:
                        if (m_LoadIN14 == 0)//花篮进sensor被遮挡
                        {
                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 2, 0);//阻挡汽缸1伸出 
                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 1, 0);//阻挡夹汽缸伸出
                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 0, 0);//进花篮电机1
                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 1, 0);//进花篮电机2停止运动
                            m_LoadIn1run = 0;
                            m_NextWaitFlag = 1;
                        }
                        else
                        {
                            if (m_WaitContinue < 200)  //延时4s
                                m_WaitContinue++;
                            else
                            {
                                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 2, 0);//阻挡汽缸1伸出
                                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 1, 0);//阻挡夹汽缸伸出，防止故障
                                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 0, 0);//进花篮电机1停止
                                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 0, 0);////进花篮电机1反转停止
                                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 1, 0);//进花篮电机2运动停止
                                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 1, 0);//进花篮电机2反转停止
                                m_LoadIn1run = 0;
                                m_WaitContinue = 0; m_NextWaitFlag = 1;
                            }
                        }

                        break;
                }
            }
        }

        //////////////**************************************************************************************************///////////////////
        ///////////////*************************主机自动化流程********************////////////////////
        //////////////**************************************************************************************************/////////////////// 	  
        /*********************************************************************************************************
        ** 函数名称 ：fAutoMainRun
        ** 函数功能 ：主机自动化
        ** 入口参数 ：
         ******DD马达的位置命名如下：
        **        A
         *        |
        ** D   ---+----   B
         *        |
         **       C
         **************旋转方向均为逆时针***********************
        *********************************************************************************************************/
        void fAutoMainRun()
        {
            if (m_DDRunFlag == true)
            {
                if (m_DDRunDelay < 5)
                    m_DDRunDelay++;
                else
                {
                    m_DDRunDelay = 0;
                    m_DDRunFlag = false;
                }
            }
            // Edit_MainStep.Value = g_MainStep;
            switch (g_MainStep)
            {
                case 2:  //延时 
                    m_RotateInFinish = 0;
                    // m_RotateInOn = 0;
                    if ((m_ZPOS == 1) && (m_RotateOutFlag == 0))
                    {
                        m_RotateOutFlag = 1;
                    }

                    if (m_MainStepContinue < 3)	  //延时3*16ms
                        m_MainStepContinue++;
                    else
                    {
                        m_MainStepContinue = 0;
                        g_MainStep = 3;

                    }
                    break;
                case 3:  //DD马达到位信号，判断台面是否有负压，如果有负压表示有wafer
                    if ((m_ZPOS == 1) && (m_UIN1 == 1))
                    {
                        Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_ONE], 7, 0);//台面清理关闭                      
                        Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 7, 1);   //打开CCD光源 
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 7, 0);   //启动关闭
                        if (m_RotateOutFlag == 0)
                            m_RotateOutFlag = 1;
                        m_MainStepContinue = 0;
                        g_MainStep = 4;
                        fScanRun();
                        m_CTFlag = !m_CTFlag;
                        // fAutoCCDRead();  //读取CCD数值,	

                    }
                    break;
                case 4:  // 
                    if (m_MainStepContinue < m_CCDdelay)	  //延时40ms
                        m_MainStepContinue++;
                    else
                    {
                        m_MainStepContinue = 0;
                        g_MainStep = 5;
                    }
                    break;
                case 5:  //CCD拍照	
                    switch (g_CWPositionFlag)
                    {
                        case 0:
                            if (m_UIN3 == 1)//D台面吸附到位
                            {
                                //  fAutoCCDRead();  //读取CCD数值,	
                                m_nState = 100; g_MainStep = 6;
                                m_MainStepContinue = 0; m_CCDNoWafer = false;
                            }
                            else
                            {
                                if (m_MainStepContinue < pci1245l.OutWaferDelay)
                                    m_MainStepContinue++;
                                else
                                {
                                    m_MainStepContinue = 0;
                                    m_CCDFinishFlag = 1;
                                    g_MainStep = 6; m_CCDNoWafer = true;
                                }
                            }
                            break;

                        case 1:
                            if (m_DIN1 == 1)//A台面吸附到位
                            {
                                //  fAutoCCDRead();  //读取CCD数值,	
                                m_nState = 100; g_MainStep = 6;
                                m_MainStepContinue = 0; m_CCDNoWafer = false;
                            }
                            else
                            {
                                if (m_MainStepContinue < pci1245l.OutWaferDelay)
                                    m_MainStepContinue++;
                                else
                                {
                                    m_MainStepContinue = 0;
                                    m_CCDFinishFlag = 1;
                                    g_MainStep = 6; m_CCDNoWafer = true;
                                }
                            }
                            break;

                        case 2:
                            if (m_UIN2 == 1)//B台面吸附到位
                            {
                                //  fAutoCCDRead();  //读取CCD数值,	
                                m_nState = 100; g_MainStep = 6;
                                m_MainStepContinue = 0; m_CCDNoWafer = false;
                            }
                            else
                            {
                                if (m_MainStepContinue < pci1245l.OutWaferDelay)
                                    m_MainStepContinue++;
                                else
                                {
                                    m_MainStepContinue = 0;
                                    m_CCDFinishFlag = 1;
                                    g_MainStep = 6; m_CCDNoWafer = true;
                                }
                            }
                            break;

                        case 3:
                            if (m_XIN0 == 1)//C台面吸附到位
                            {
                                //  fAutoCCDRead();  //读取CCD数值,	
                                m_nState = 100; g_MainStep = 6;
                                m_MainStepContinue = 0; m_CCDNoWafer = false;
                            }
                            else
                            {
                                if (m_MainStepContinue < pci1245l.OutWaferDelay)
                                    m_MainStepContinue++;
                                else
                                {
                                    m_MainStepContinue = 0;
                                    m_CCDFinishFlag = 1;
                                    g_MainStep = 6; m_CCDNoWafer = true;
                                }
                            }
                            break;
                    }
                    break;
                case 6:  //DD旋转  

                    Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_TWO], 3, ref m_BIN3);   //主机安全门保护  
                    if (m_BIN3 == 1 && (m_DoorProtect == false))   //门保护
                    {
                        if ((m_MarkFinish == 11) && (m_CCDFinishFlag == 1) && (m_RotateOutFlag == 0) && (m_CCDAlarm == 0))
                        {

                            if (m_WaferBreak > 0)//拍照位置硅片异常
                            {
                                if (m_DDWaferInFlag == true)//如果有台面吸附指令，必须吸附到位后才能DD运动
                                {
                                    if (((g_CWPositionFlag == 0) && (m_DIN1 == 1)) || ((g_CWPositionFlag == 1) && (m_UIN2 == 1)) || ((g_CWPositionFlag == 2) && (m_XIN0 == 1)) || ((g_CWPositionFlag == 3) && (m_UIN3 == 1)))
                                    {
                                        m_DDWaferInFlag = false;
                                    }
                                }
                                else
                                {
                                    m_CCDNoWafer = true;
                                    switch (g_CWPositionFlag)
                                    {
                                        case 0:
                                            if (m_UIN3 == 0) //D台面吸附没有到位 
                                            { m_WaferBreak = 0; }
                                            else
                                            {
                                                m_RotateOutFlag = 1;
                                                m_DDRunFlag = true;
                                                ++g_CWPositionFlag;//顺时针旋转90°
                                                if (g_CWPositionFlag > 3)
                                                    g_CWPositionFlag = 0;
                                                fDDPositionStatus(0);
                                                g_MainStep = 11;
                                            }
                                            break;
                                        case 1:
                                            if (m_DIN1 == 0) //A台面吸附没有到位 
                                            { m_WaferBreak = 0; }
                                            else
                                            {
                                                m_RotateOutFlag = 1;
                                                m_DDRunFlag = true;
                                                ++g_CWPositionFlag;//顺时针旋转90°
                                                if (g_CWPositionFlag > 3)
                                                    g_CWPositionFlag = 0;
                                                fDDPositionStatus(0);
                                                g_MainStep = 11;
                                            }
                                            break;
                                        case 2:
                                            if (m_UIN2 == 0) //B台面吸附没有到位 
                                            { m_WaferBreak = 0; }
                                            else
                                            {
                                                m_RotateOutFlag = 1;
                                                m_DDRunFlag = true;
                                                ++g_CWPositionFlag;//顺时针旋转90°
                                                if (g_CWPositionFlag > 3)
                                                    g_CWPositionFlag = 0;
                                                fDDPositionStatus(0);
                                                g_MainStep = 11;
                                            }
                                            break;
                                        case 3:
                                            if (m_XIN0 == 0) //C台面吸附没有到位 
                                            { m_WaferBreak = 0; }
                                            else
                                            {
                                                m_RotateOutFlag = 1;
                                                m_DDRunFlag = true;
                                                ++g_CWPositionFlag;//顺时针旋转90°
                                                if (g_CWPositionFlag > 3)
                                                    g_CWPositionFlag = 0;
                                                fDDPositionStatus(0);
                                                g_MainStep = 11;
                                            }
                                            break;
                                    }
                                }
                            }
                            else  //拍照位置硅片Ok 
                            {
                                if (m_DDWaferInFlag == true)//如果有台面吸附指令，必须吸附到位后才能DD运动
                                {
                                    if (((g_CWPositionFlag == 0) && (m_DIN1 == 1)) || ((g_CWPositionFlag == 1) && (m_UIN2 == 1)) || ((g_CWPositionFlag == 2) && (m_XIN0 == 1)) || ((g_CWPositionFlag == 3) && (m_UIN3 == 1)))
                                    {
                                        m_DDWaferInFlag = false;
                                    }
                                }
                                else
                                {
                                    if ((m_DIN1 == 1) || (m_UIN2 == 1) || (m_UIN3 == 1) || (m_XIN0 == 1))
                                    {
                                        switch (g_CWPositionFlag)
                                        {
                                            case 0:
                                                if (((m_DIN1 == 0) && (m_RotateInFinish == 1)) || (m_DIN1 == 1))
                                                {
                                                    m_MarkFinish = 0; m_CCDFinishFlag = 0;
                                                    m_DDRunFlag = true;
                                                    ++g_CWPositionFlag;//顺时针旋转90°
                                                    if (g_CWPositionFlag > 3)
                                                        g_CWPositionFlag = 0;
                                                    fDDPositionStatus(0);
                                                    axMMMark1.JumpToStartPos();
                                                    axMMMark1.SetMatrix(m_CCDFinish[0], m_CCDFinish[1], m_CCDFinish[2]);
                                                    m_MainStepContinue = 0;
                                                    g_MainStep = 2; m_UnLoadSuck = false;//加工吹气打开 
                                                    Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_ONE], 7, 1);//台面清理
                                                }
                                                break;
                                            case 1:
                                                if (((m_UIN2 == 0) && (m_RotateInFinish == 1)) || (m_UIN2 == 1))     //B台面吸附           
                                                {
                                                    m_MarkFinish = 0; m_CCDFinishFlag = 0;
                                                    m_DDRunFlag = true;
                                                    ++g_CWPositionFlag;//顺时针旋转90°
                                                    if (g_CWPositionFlag > 3)
                                                        g_CWPositionFlag = 0;
                                                    fDDPositionStatus(0);
                                                    axMMMark1.JumpToStartPos();
                                                    axMMMark1.SetMatrix(m_CCDFinish[0], m_CCDFinish[1], m_CCDFinish[2]);
                                                    m_MainStepContinue = 0;
                                                    g_MainStep = 2; m_UnLoadSuck = false;
                                                    Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_ONE], 7, 1);//台面清理
                                                }
                                                break;
                                            case 2:
                                                if (((m_XIN0 == 0) && (m_RotateInFinish == 1)) || (m_XIN0 == 1))  //C台面吸附没有到位
                                                {

                                                    m_MarkFinish = 0; m_CCDFinishFlag = 0;
                                                    m_DDRunFlag = true;
                                                    ++g_CWPositionFlag;//顺时针旋转90°
                                                    if (g_CWPositionFlag > 3)
                                                        g_CWPositionFlag = 0;
                                                    fDDPositionStatus(0);
                                                    axMMMark1.JumpToStartPos();
                                                    axMMMark1.SetMatrix(m_CCDFinish[0], m_CCDFinish[1], m_CCDFinish[2]);
                                                    m_MainStepContinue = 0;
                                                    g_MainStep = 2; m_UnLoadSuck = false;
                                                    Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_ONE], 7, 1);//台面清理
                                                }
                                                break;
                                            case 3:
                                                if (((m_UIN3 == 0) && (m_RotateInFinish == 1)) || (m_UIN3 == 1))//D台面吸附
                                                {
                                                    m_MarkFinish = 0; m_CCDFinishFlag = 0;
                                                    m_DDRunFlag = true;
                                                    ++g_CWPositionFlag;//顺时针旋转90°
                                                    if (g_CWPositionFlag > 3)
                                                        g_CWPositionFlag = 0;
                                                    fDDPositionStatus(0);
                                                    axMMMark1.JumpToStartPos();
                                                    axMMMark1.SetMatrix(m_CCDFinish[0], m_CCDFinish[1], m_CCDFinish[2]);
                                                    m_MainStepContinue = 0;
                                                    g_MainStep = 2; m_UnLoadSuck = false;
                                                    Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_ONE], 7, 1);//台面清理
                                                }
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    break;
                case 11:  //DD马达与旋转臂到位，之后再旋转90°，使得硅片到达下料位置
                    Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_TWO], 3, ref m_BIN3);   //主机安全门保护  
                    if (m_BIN3 == 1 && (m_DoorProtect == false))   //门保护
                    {
                        if ((m_ZPOS == 1) && (m_UIN1 == 1) && (m_DDRunFlag == false) && (m_RotateOutFlag == 0))
                        {
                            Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 7, 0);   //启动关闭
                            if (m_MainStepContinue < pci1245l.OutWaferDelay)
                                m_MainStepContinue++;
                            else
                            {
                                m_MainStepContinue = 0;
                                m_MainBreak = true;
                                m_DDRunFlag = true; m_WaferBreak = 2;
                                ++g_CWPositionFlag;//顺时针旋转90°
                                if (g_CWPositionFlag > 3)
                                    g_CWPositionFlag = 0;
                                fDDPositionStatus(0);
                                m_RotateOutFlag = 1;
                                g_MainStep = 12;
                            }
                        }
                    }

                    break;
                case 12:  //硅片到达下料位置,并已经执行丢硅片动作     
                    Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_TWO], 3, ref m_BIN3);   //主机安全门保护  
                    if (m_BIN3 == 1 && (m_DoorProtect == false))   //门保护
                    {
                        if ((m_ZPOS == 1) && (m_UIN1 == 1) && (m_DDRunFlag == false) && (m_WaferBreak == 5) && (m_RotateOutFlag == 0))
                        {
                            Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 7, 0);   //启动关闭
                            if (m_MainStepContinue < pci1245l.OutWaferDelay)
                                m_MainStepContinue++;
                            else
                            {
                                m_MainStepContinue = 0;
                                m_MarkFinish = 0; m_CCDFinishFlag = 0;
                                m_DDRunFlag = true; m_WaferBreak = 0;
                                ++g_CWPositionFlag;//顺时针旋转90°
                                if (g_CWPositionFlag > 3)
                                    g_CWPositionFlag = 0;
                                fDDPositionStatus(0);
                                m_MainStepContinue = 0;
                                g_MainStep = 2; m_MainBreak = false;
                            }
                        }
                    }

                    break;
            }

        }
        /*********************************************************************************************************
        ** 函数名称 ：fAutoRotateIn
        ** 函数功能 ：上料旋转臂逻辑
        ** 修改时间 ：20131011
        ** 修改内容 ：
        *********************************************************************************************************/
        void fAutoRotateIn()
        {
            ushort GetState1 = 0, GetState2 = 0;
            switch (m_RotateInFlag)
            {
                case 0://上料有料等待，旋转臂开始吸附           
                    Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_TWO], ref GetState1);
                    Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_TWO], ref GetState2);
                    if ((GetState1 == 1) && (GetState2 == 1) && (m_BPOS == 1) && (m_YPOS == 1))
                    {
                        if (m_MainIn2 == 1)//上料有料
                        {
                            //if (m_LeftWaferDeal1 < 4)
                            //    m_LeftWaferDeal1++;
                            //else
                            //{
                            m_LeftWaferDeal1 = 0;
                            if (g_Posnt[1] == 0)//上料旋转臂原始位置
                            {
                                Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_TWO], 5, 1);  //上料A吸附
                                m_LoadSuck = 1;
                                m_RotateInFlag = 1;
                            }
                            else
                            {
                                Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_TWO], 4, 1);  //上料B吸附
                                m_LoadSuck = 1;
                                m_RotateInFlag = 1;
                            }
                            //  }
                        }
                        else//上料无料
                        {
                            if (m_LeftWaferDeal < 100)  //延时1.6s,确定一定没料
                                m_LeftWaferDeal++;
                            else
                            {
                                m_LeftWaferDeal = 0;
                                m_RotateInFlag = 0;
                                m_RotateInFinish = 1;
                            }
                        }
                    }
                    break;

                case 1://旋转臂吸附到位        
                    if (((g_Posnt[1] == 0) && (m_DIN3 == 1) && (m_YPOS == 1)) || ((g_Posnt[1] == 180 * pci1245l.RotateRatio) && (m_DIN2 == 1) && (m_YPOS == 1)))
                    {
                        m_WaferSuckDelay = 0;
                        //if (m_LeftWaferDeal1 < 2)
                        //    m_LeftWaferDeal1++;
                        //else
                        //{
                        m_MainIn2 = 0;  //允许上料传送模组2进料
                        m_LeftWaferDeal1 = 0;
                        m_RotateInCount = 0;
                        m_RotateInFlag = 2;
                        // }
                    }
                    break;

                case 2://吸附到位后，旋转    
                    Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_TWO], 3, ref m_BIN3);   //主机安全门保护  
                    if (m_BIN3 == 1 && (m_DoorProtect == false))   //门保护
                    {
                        //m_LeftWaferDeal = 0;
                        if (m_WaferUpFlag == false)
                        {
                            if (g_Posnt[1] == 0)//上料旋转臂原始位置
                            {
                                if (m_DIN3 == 1)   //A吸附到位
                                {
                                    Motion.mAcm_AxMoveAbs(ax2Handle[pci1245l.Axis_TWO], 180 * pci1245l.RotateRatio);
                                    m_RotateInFlag = 3;
                                }
                            }
                            else
                            {
                                if (m_DIN2 == 1)  //B吸附到位
                                {
                                    Motion.mAcm_AxMoveAbs(ax2Handle[pci1245l.Axis_TWO], 0);
                                    m_RotateInFlag = 3;
                                }
                            }
                        }
                    }

                    break;

                case 3://根据台面是否有负压，决定是否台面吸附  
                    // m_RotateInOn = 1;
                    Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_TWO], ref GetState1);
                    if ((GetState1 == 1) && (m_UIN1 == 1) && (m_DDRunFlag == false) && (m_DDSuckfail == false) && (m_WaferBreak == 0) && (m_UnLoadSuck == false))  //DD马达到位 上料旋转电机到位
                    {
                        // if (m_LeftWaferDeal1 < 1)
                        //     m_LeftWaferDeal1++;
                        //  else
                        //  {
                        //     m_LeftWaferDeal1 = 0;
                        switch (g_CWPositionFlag)
                        {
                            case 0:
                                if (m_DIN1 == 0) //A台面无料
                                {
                                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_ONE], 5, 1);//A台面吸附
                                    m_RotateInFlag = 4; m_DDWaferInFlag = true;
                                }
                                //else
                                //{
                                //    m_RotateInFinish = 1;
                                //}

                                break;
                            case 1:
                                if (m_UIN2 == 0)  //B台面无料
                                {
                                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_ONE], 4, 1);//B台面吸附
                                    m_RotateInFlag = 4; m_DDWaferInFlag = true;
                                }
                                //else
                                //{
                                //    m_RotateInFinish = 1;
                                //}

                                break;
                            case 2:
                                if (m_XIN0 == 0) //C台面无料
                                {
                                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_ONE], 7, 1);//C台面吸附
                                    m_RotateInFlag = 4; m_DDWaferInFlag = true;
                                }
                                //else
                                //{
                                //    m_RotateInFinish = 1;
                                //}

                                break;
                            case 3:
                                if (m_UIN3 == 0) //D台面无料
                                {
                                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_ONE], 6, 1);//D台面吸附
                                    m_RotateInFlag = 4; m_DDWaferInFlag = true;
                                }
                                //else
                                //{
                                //    m_RotateInFinish = 1;
                                //}
                                break;
                            // }
                        }
                    }
                    else m_LeftWaferDeal1 = 0;
                    break;

                case 4://旋转到位，释放硅片到台面
                    Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_TWO], ref GetState1);
                    if ((GetState1 == 1) && (m_UIN1 == 1) && (m_DDRunFlag == false) && (m_DDSuckfail == false) && (m_UnLoadSuck == false))  //DD马达到位 上料旋转电机到位
                    {
                        if (g_Posnt[1] == 0)//上料旋转臂原始位置
                        {
                            Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_TWO], 4, 0);//上料B放料
                            m_LoadSuck = 0; m_UnLoadSuck = true;
                            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_ONE], 4, 1);//上料B吸盘反吹                         
                        }
                        else
                        {
                            Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_TWO], 5, 0);//上料A放料
                            m_LoadSuck = 0; m_UnLoadSuck = true;
                            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_ONE], 5, 1);//上料A吸盘反吹

                        }
                        // if(((g_CWPositionFlag == 0) && (m_DIN1 == 0)) || ((g_CWPositionFlag == 1) && (m_UIN2 == 0)) || ((g_CWPositionFlag == 2) && (m_XIN0 == 0)) || ((g_CWPositionFlag == 3) && (m_UIN3 == 0)))
                        m_RotateInFlag = 0; m_WaferUpFlag = true; m_RotateInCount = 0;

                    }
                    break;
            }
            //吸附完成，延时一段时间
            if (m_WaferUpFlag == true)
            {
                if (m_RotateInCount < m_InReleaseDelay)//延时10*16ms
                    m_RotateInCount++;
                else
                {
                    Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_ONE], 4, 0);//上料B吸盘反吹关闭  
                    Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_ONE], 5, 0);//上料A吸盘反吹关闭
                    m_RotateInCount = 0;
                    m_WaferUpFlag = false; m_RotateInFinish = 1;
                }
            }
        }

        /*********************************************************************************************************
        ** 函数名称 ：fAutoRotateOut
        ** 函数功能 ：下料旋转臂逻辑
        ** 修改时间 ：20131011
        ** 修改内容 ：
        *********************************************************************************************************/
        void fAutoRotateOut()
        {
            ushort GetState1 = 0, GetState2 = 0;

            switch (m_RotateOutFlag)
            {
                case 1://根据台面是否有负压，决定是否需要下料动作
                    switch (g_CWPositionFlag)
                    {
                        case 0:
                            if (m_UIN2 == 1)  //B台面吸附到位
                            {
                                if (g_OutRotateMotorPosition == 0)//下料旋转原始位置
                                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 4, 1);  //下料吸附A
                                else
                                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 5, 1);  //下料吸附B
                                m_RotateOutFlag = 2; m_RotateOutCount = 0;
                            }
                            else//no wafer
                            { m_RotateOutFlag = 0; }
                            break;
                        case 1:
                            if (m_XIN0 == 1) //C台面吸附到位
                            {
                                if (g_OutRotateMotorPosition == 0)//下料旋转原始位置
                                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 4, 1);  //下料吸附A
                                else
                                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 5, 1);  //下料吸附B
                                m_RotateOutFlag = 2; m_RotateOutCount = 0;
                            }
                            else//no wafer
                            { m_RotateOutFlag = 0; }
                            break;
                        case 2:
                            if (m_UIN3 == 1) //D台面吸附到位
                            {
                                if (g_OutRotateMotorPosition == 0)//下料旋转原始位置
                                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 4, 1);  //下料吸附A
                                else
                                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 5, 1);  //下料吸附B						    
                                m_RotateOutFlag = 2; m_RotateOutCount = 0;
                            }
                            else//no wafer
                            { m_RotateOutFlag = 0; }
                            break;
                        case 3:
                            if (m_DIN1 == 1) //A台面吸附到位
                            {
                                if (g_OutRotateMotorPosition == 0)//下料旋转原始位置
                                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 4, 1);  //下料吸附A
                                else
                                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 5, 1);  //下料吸附B
                                m_RotateOutFlag = 2; m_RotateOutCount = 0;
                            }
                            else//no wafer
                            { m_RotateOutFlag = 0; }
                            break;
                    }
                    break;

                case 2://对应的DD台面释放负压
                    if (m_RotateOutCount < 2)//延时5*16ms
                        m_RotateOutCount++;
                    else
                    {
                        m_RotateOutCount = 0;
                        switch (g_CWPositionFlag)
                        {
                            case 0:
                                // if (m_UIN2 == 1)  //B台面吸附到位
                                // {
                                Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_ONE], 4, 0);//B台面释放   
                                m_RotateOutFlag = 3;
                                //  }
                                break;

                            case 1:
                                //   if (m_XIN0 == 1) //C台面吸附到位
                                //  {
                                Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_ONE], 7, 0);//C台面释放    
                                m_RotateOutFlag = 3;
                                //  }

                                break;

                            case 2:
                                // if (m_UIN3 == 1) //D台面吸附到位
                                //  {
                                Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_ONE], 6, 0);//D台面释放  
                                m_RotateOutFlag = 3;
                                //   }
                                break;

                            case 3:
                                // if (m_DIN1 == 1) //A台面吸附到位
                                //  {
                                Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_ONE], 5, 0);//A台面释放 
                                m_RotateOutFlag = 3;
                                //  }
                                break;
                        }
                    }
                    break;
                case 3://旋转臂吸附到位，准备旋转,下料气缸旋转
                    Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_THREE], ref GetState1);
                    Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_TWO], 3, ref m_BIN3);   //主机安全门保护  
                    if (m_BIN3 == 1 && (m_DoorProtect == false))   //门保护
                    {
                        if ((GetState1 == 1) && (m_WaferReleaseFlag == false))
                        {
                            switch (g_OutRotateMotorPosition)
                            {
                                //下料旋转原始位置
                                case 0:
                                    if ((m_ZIN2 == 1) || ((m_WaferBreak == 2) && (m_WaferBreakOutError == 1)))//吸附到位
                                    {
                                        if (m_WaferBreak == 2)//拍照异常片达到下料位置
                                        {
                                            if ((m_UIN1 == 1) && (m_DDRunFlag == false))
                                            {
                                                m_WaferBreak = 3;
                                                Motion.mAcm_AxMoveRel(ax2Handle[pci1245l.Axis_THREE], FormHMI.g_SysParam[45] * FormHMI.pci1245l.RotateRatio);
                                                /////旋转22.5°
                                                m_DDRunFlag = true; m_WaferBreakOutError = 0;
                                                fDDPositionStatus(1);   //DD旋转                          
                                                m_RotateOutFlag = 5;
                                            }
                                        }
                                        else
                                        {
                                            g_OutRotateMotorPosition = 1; m_WaferBreakOutError = 0;
                                            fOutWaferPositionStatus(g_OutRotateMotorPosition);
                                            //if (m_DisableUnLoadRotateCylinder == 0)
                                            //{
                                            //    Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_TWO], 6, 1);//  下料气缸旋转位  XP
                                            //}
                                            m_RotateOutFlag = 4;
                                        }

                                    }
                                    break;
                                //下料旋转臂180位置
                                case 1:
                                    if ((m_ZIN3 == 1) || ((m_WaferBreak == 2) && (m_WaferBreakOutError == 1)))//吸附到位
                                    {
                                        if (m_WaferBreak == 2)//拍照异常片达到下料位置
                                        {
                                            if ((m_UIN1 == 1) && (m_DDRunFlag == false))
                                            {
                                                m_WaferBreak = 3;
                                                Motion.mAcm_AxMoveRel(ax2Handle[pci1245l.Axis_THREE], FormHMI.g_SysParam[45] * FormHMI.pci1245l.RotateRatio);
                                                /////旋转22.5°
                                                m_DDRunFlag = true; m_WaferBreakOutError = 0;
                                                fDDPositionStatus(1); //DD旋转
                                                m_RotateOutFlag = 5;
                                            }
                                        }
                                        else
                                        {
                                            g_OutRotateMotorPosition = 0; m_WaferBreakOutError = 0;
                                            fOutWaferPositionStatus(g_OutRotateMotorPosition);
                                            //if (m_DisableUnLoadRotateCylinder == 0)
                                            //{
                                            //    Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_TWO], 6, 0);//  下料气缸旋原位  XP
                                            //}
                                            m_RotateOutFlag = 4;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    break;

                case 4://延时 旋转下料气缸动作
                    if (m_RotateOutCount < m_RotateCylindDelay) //旋转气缸延时
                        m_RotateOutCount++;
                    else
                    {
                        m_RotateOutCount = 0;
                        if (m_DisableUnLoadRotateCylinder == 0)
                        {
                            if (g_OutRotateMotorPosition == 1) //下料旋转原始位置
                            {
                                Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_TWO], 6, 1);//  下料气缸旋转位  XP
                            }
                            else
                            {
                                Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_TWO], 6, 0);//  下料气缸旋原位  XP 
                            }       
                        }
                        m_RotateOutFlag = 5;
                    }
                    break;

                case 5://下料气缸和下料旋转臂旋转完成后，准备释放硅片
                    Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_THREE], ref GetState1);
                    Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_THREE], ref GetState2);
                    if ((m_ZPOS == 1) && (GetState1 == 1) && (GetState2 == 1))
                    {
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 7, 0);   //启动关闭
                        if (g_OutRotateMotorPosition == 0)//下料旋转原始位置
                        {
                            if (m_WaferBreak == 3)//拍照异常片达到下料位置
                            {
                                Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_THREE], ref GetState1);
                                if ((m_UIN1 == 1) && (m_DDRunFlag == false) && (GetState1 == 1))
                                {
                                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 4, 0);  //下料吸附A释放
                                    Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_TWO], 4, 1);  //下料反吹A打开
                                    m_RotateOut2Count = 0;
                                    m_BreakOutCount = 0; m_WaferReleaseFlag = true;
                                    m_WaferBreak = 4; m_RotateOut2Count = 0;
                                    m_RotateOutFlag = 0;
                                }

                            }
                            else
                            {
                                if (/*(m_ZIN3 == 1) &&*/ (m_waferoutflag == 0))//吸附到位,且下料允许，气缸到位
                                {
                                    Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_THREE], ref GetState1);
                                    if ((m_ZIN0 == 1) && (GetState1 == 1) && (m_CPOS == 1))
                                    {
                                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 5, 0);  //下料吸附B释放
                                        Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 6, 1);  //下料反吹B打开
                                        m_BreakOutCount = 0; m_WaferReleaseFlag = true;
                                        m_RotateOutFlag = 0; m_RotateOut2Count = 0;
                                        //   m_waferoutflag = 1;//出料模组1运动标志
                                    }
                                    else
                                    {
                                        if (m_RotateOut2Count < 100)//延时4*16ms
                                            m_RotateOut2Count++;
                                        else
                                        {
                                            m_RotateOut2Count = 0;
                                            fBoxStop();
                                            m_ErrorFlag = 1;
                                            fALAM("警告", AlarmMess[54]);
                                            fEMGStop();
                                            // g_MainStep = 12;
                                        }
                                    }
                                }
                            }
                        }
                        else//下料旋转臂180位置
                        {
                            if (m_WaferBreak == 3)//拍照异常片达到下料位置
                            {
                                Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_THREE], ref GetState1);
                                if ((m_UIN1 == 1) && (m_DDRunFlag == false) && (GetState1 == 1))
                                {
                                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 5, 0);  //下料吸附B释放
                                    Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 6, 1);  //下料反吹B打开 
                                    m_BreakOutCount = 0; m_WaferReleaseFlag = true; m_RotateOut2Count = 0;
                                    m_WaferBreak = 4; m_RotateOut2Count = 0;
                                    m_RotateOutFlag = 0;
                                }

                            }
                            else
                            {
                                if (/*(m_ZIN2 == 1) && */(m_waferoutflag == 0))//吸附到位,且下料允许，气缸到位
                                {
                                    Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_THREE], ref GetState1);
                                    if ((m_ZIN0 == 1) && (GetState1 == 1) && (m_CPOS == 1))
                                    {
                                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 4, 0);  //下料吸附A释放
                                        Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_TWO], 4, 1);  //下料反吹A打开
                                        m_BreakOutCount = 0; m_WaferReleaseFlag = true;
                                        m_RotateOutFlag = 0; m_RotateOut2Count = 0;
                                        //  m_waferoutflag = 1;//出料模组1运动标志
                                    }
                                    else
                                    {
                                        if (m_RotateOut2Count < 100)//延时4*16ms
                                            m_RotateOut2Count++;
                                        else
                                        {
                                            m_RotateOut2Count = 0;
                                            fBoxStop();
                                            m_ErrorFlag = 1;
                                            fALAM("警告", AlarmMess[54]);
                                            fEMGStop();
                                            // g_MainStep = 12;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
            }
            //放片完成，延时一段时间
            if (m_WaferReleaseFlag == true)
            {
                if (m_RotateOut2Count < m_OutReleaseDelay)//延时10*16ms,m_OutReleaseDelay
                    m_RotateOut2Count++;
                else
                {
                    Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_TWO], 4, 0);  //下料反吹A关闭
                    Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 6, 0);  //下料反吹B关闭
                    if (m_WaferBreak == 4)//拍照异常片达到下料位置
                    {
                        if ((m_UIN1 == 1) && (m_DDRunFlag == false))
                        {
                            Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 7, 0);   //启动关闭
                            if (m_BreakOutCount < 40)//延时10*16ms

                                m_BreakOutCount++;
                            else
                            {
                                Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_TWO], 3, ref m_BIN3);   //主机安全门保护  
                                if (m_BIN3 == 1 && (m_DoorProtect == false))   //门保护
                                {
                                    m_BreakOutCount = 0;
                                    Motion.mAcm_AxMoveRel(ax2Handle[pci1245l.Axis_THREE], -FormHMI.g_SysParam[45] * FormHMI.pci1245l.RotateRatio);//上料旋转臂转回45度
                                    //DD正转45度
                                    m_WaferBreak = 5;
                                    m_MarkFinish = 0; m_CCDFinishFlag = 0; m_DDRunFlag = true;
                                    //旋转67.5°
                                    fDDPositionStatus(2);
                                    ++g_CWPositionFlag;
                                    if (g_CWPositionFlag > 3)
                                        g_CWPositionFlag = 0;
                                    m_WaferReleaseFlag = false; m_RotateOut2Count = 0;
                                }

                            }
                        }
                    }
                    else
                    {
                        m_RotateOut2Count = 0; m_WaferReleaseFlag = false;// m_RotateOutFlag = 0; 
                        m_waferoutflag = 1;//出料模组1运动标志
                    }
                }
            }



        }
        /*********************************************************************************************************
       ** 函数名称 ：fLoadWaferCheck
       ** 函数功能 ：上料硅片检测
       ** 修改时间 ：20181011
       ** 修改内容 ：
       *********************************************************************************************************/
        void fLoadWaferCheck()
        {
            while (true)
            {
                switch (m_WaferCheck)
                {
                    case 100:
                    case 101:
                    case 102:
                    case 103:
                    case 104:
                    case 105:
                    case 106:
                        m_WaferCheck++;
                        MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD122IP, 1, 0, ref m_LoadIN50);
                        if (m_LoadIN50 == 0) //舌头顶端有硅片
                            m_WaferCheckFlag = true;
                        break;
                }
            }
        }
        /*********************************************************************************************************
        ** 函数名称 ：fAutoCCDRead
        ** 函数功能 ：CCD自动化拍照
        ** 修改时间 ：20131011
        ** 修改内容 ：
        *********************************************************************************************************/
        void fAutoCCDRead()
        {
            double[] CCDSourceTemp = new double[4];
            int i, j;
            uint CCDReadReturn;
            double[] CCDSourceBefore = new double[4];
            while (true)
            {
                System.Threading.Thread.Sleep(20);//延时20ms
                switch (m_nState)
                {
                    case 100:
                        double[] m_CCDSourceData0 = new double[3];
                        double[] m_CCDSourceData1 = new double[3];
                        double[] m_CCDSourceData2 = new double[3];
                        int[] m_CCDSourceData3 = new int[3];
                        CCDSourceBefore[0] = g_CCDSource[0]; CCDSourceBefore[1] = g_CCDSource[1]; CCDSourceBefore[2] = g_CCDSource[2];
                        for (j = 0; j < 2; )
                        {
                            CCDReadReturn = fCCDRead(g_CCDSource);
                            if (CCDReadReturn == 0)
                            {
                                m_CCDSourceData0[j] = g_CCDSource[0];
                                m_CCDSourceData1[j] = g_CCDSource[1];
                                m_CCDSourceData2[j] = g_CCDSource[2];
                                m_CCDSourceData3[j] = Convert.ToInt32(g_CCDSource[3]);
                                j++;
                            }
                            else if (CCDReadReturn == 1)
                            {
                                m_CCDAlarm = 1; m_nState = 101;
                                break;
                            }
                            else if (CCDReadReturn == 4)
                            {
                                m_CCDAlarm = 4; m_nState = 101;
                                break;
                            }
                            else if (CCDReadReturn == 5)
                            {
                                m_CCDAlarm = 5; m_nState = 101;
                                break;
                            }
                        }
                        if ((j == 2) && (m_CCDAlarm == 0))
                        {
                            if ((Math.Abs(m_CCDSourceData0[0] - m_CCDSourceData0[1]) > 0.1f)
                                || (Math.Abs(m_CCDSourceData1[0] - m_CCDSourceData1[1]) > 0.1f)
                                || (Math.Abs(m_CCDSourceData2[0] - m_CCDSourceData2[1]) > 0.1f))
                            {
                                m_CCDAlarm = 4;
                                m_nState = 101;
                                break;
                            }

                            else if ((Math.Abs(CCDSourceBefore[0] - g_CCDSource[0]) < 0.0001f)
                                && (Math.Abs(CCDSourceBefore[1] - g_CCDSource[1]) < 0.0001f)
                                && (Math.Abs(CCDSourceBefore[2] - g_CCDSource[2]) < 0.0001f))
                            {
                                m_CCDAlarm = 4;
                                m_nState = 101;
                                break;
                            }
                            else if ((m_CCDSourceData3[0] == 0) && (m_CCDSourceData3[1] == 0))
                            {
                                m_CCDAlarm = 2;
                                m_nState = 101;
                                break;
                            }
                            else
                            {
                                CCDSourceTemp[0] = g_CCDSource[0] - m_CCDStandX; CCDSourceTemp[1] = g_CCDSource[1] - m_CCDStandY;
                                CCDSourceTemp[2] = g_CCDSource[2];
                                if ((CCDSourceTemp[0] < -m_CCDAlarmValue) || (CCDSourceTemp[0] > m_CCDAlarmValue)
                                    || (CCDSourceTemp[1] > m_CCDAlarmValue) || (CCDSourceTemp[1] < -m_CCDAlarmValue)
                                    || (CCDSourceTemp[2] < -m_CCDAlarmAngleValue) || (CCDSourceTemp[2] > m_CCDAlarmAngleValue))
                                {
                                    m_CCDAlarm = 3; m_WaferBreak = 1; m_nState = 101;
                                    break;
                                }
                                else
                                {
                                    m_CCDFinish[0] = (m_CCDSourceData0[0] + m_CCDSourceData0[1]) / 2;
                                    m_CCDFinish[1] = (m_CCDSourceData1[0] + m_CCDSourceData1[1]) / 2;
                                    m_CCDFinish[2] = (m_CCDSourceData2[0] + m_CCDSourceData2[1]) / 2;
                                    m_CCDFinish[3] = 1;
                                    m_nState = 101; m_CCDFinishFlag = 1;
                                    break;
                                }
                            }
                        }
                        break;
                }
            }
        }
        //////////////**************************************************************************************************///////////////////
        ///////////////*************************故障报警处理/********************////////////////////
        //////////////**************************************************************************************************///////////////////

        /*********************************************************************************************************
        ** 函数名称 ：fAutoLoadAlarm
        ** 函数功能 ：上下料接驳台报警处理
        ** 修改时间 ：20181019
        ** 修改内容 ：
        *********************************************************************************************************/
        void fAutoLoadAlarm()
        {
            ushort GetState1 = 0;

            //上料KK模组报警
            if (m_Load_XAlarm == 1)
            {
                if (m_KKAlamLoadContinue < 20)
                    m_KKAlamLoadContinue++;
                else
                {
                    MNet.M1A._mnet_m1a_imd_stop(m_RingNoA, ClineAddr.LoadAX0IP); //升降模组停止运动
                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 4, 0);//舌头缩回
                    m_KKAlamLoadContinue = 0;
                    m_ErrorFlag = 1; fALAM("警告", AlarmMess[48]);
                    //fSystemQuit();
                    fEMGStop();
                    m_SysIniReset = 1;
                    TextShow.Text = "请先进行系统复位！！";
                }
            }
            else
                m_KKAlamLoadContinue = 0;
            //上料舌头传送模组报警
            if (m_Load_DAlarm == 1)
            {
                if (m_DAlamLoadContinue < 20)
                    m_DAlamLoadContinue++;
                else
                {
                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 4, 0);//舌头缩回                    
                    m_DAlamLoadContinue = 0;
                    m_ErrorFlag = 1;
                    fALAM("警告", AlarmMess[66]);
                }
            }
            else
                m_DAlamLoadContinue = 0;

            //主机轴报警
            //进料模组1轴报警

            if (m_AAlarm == 0)
            {
                if (m_MainAAlarmContinue < 20)
                {
                    m_MainAAlarmContinue++;
                }
                else
                {
                    m_ErrorFlag = 1;
                    fALAM("警告", AlarmMess[67]);
                    m_MainAAlarmContinue = 0;
                }
            }
            else
                m_MainAAlarmContinue = 0;
            //进料模组2轴报警
            if (m_BAlarm == 0)
            {
                if (m_MainBAlarmContinue < 20)
                {
                    m_MainBAlarmContinue++;
                }
                else
                {
                    m_ErrorFlag = 1;
                    fALAM("警告", AlarmMess[68]);
                    m_MainBAlarmContinue = 0;
                }
            }
            else
                m_MainBAlarmContinue = 0;
            //出料模组1轴报警
            if (m_CAlarm == 0)
            {
                if (m_MainCAlarmContinue < 20)
                {
                    m_MainCAlarmContinue++;
                }
                else
                {
                    m_ErrorFlag = 1;
                    fALAM("警告", AlarmMess[69]);
                    m_MainCAlarmContinue = 0;
                }
            }
            else
                m_MainCAlarmContinue = 0;
            //进料buffer轴报警
            if (m_XAlarm == 0)
            {
                if (m_MainXAlarmContinue < 20)
                {
                    m_MainXAlarmContinue++;
                }
                else
                {
                    m_ErrorFlag = 1;
                    fALAM("警告", AlarmMess[70]);
                    m_MainXAlarmContinue = 0;
                }
            }
            else
                m_MainXAlarmContinue = 0;
            //进料旋转臂轴报警
            if (m_YAlarm == 1)
            {
                if (m_MainYAlarmContinue < 20)
                {
                    m_MainYAlarmContinue++;
                }
                else
                {
                    m_ErrorFlag = 1;
                    fALAM("警告", AlarmMess[71]);
                    m_MainYAlarmContinue = 0;
                }
            }
            else
                m_MainYAlarmContinue = 0;
            //出料旋转臂轴报警
            if (m_ZAlarm == 1)
            {
                if (m_MainZAlarmContinue < 20)
                {
                    m_MainZAlarmContinue++;
                }
                else
                {
                    m_ErrorFlag = 1;
                    fALAM("警告", AlarmMess[72]);
                    m_MainZAlarmContinue = 0;
                }
            }
            else
                m_MainZAlarmContinue = 0;
            //DD马达轴报警
            if (m_UIN0 == 1)
            {
                if (m_MainDDAlarmContinue < 20)
                {
                    m_MainDDAlarmContinue++;
                }
                else
                {
                    m_ErrorFlag = 1;
                    fALAM("警告", AlarmMess[73]);
                    m_MainDDAlarmContinue = 0;
                }
            }
            else
                m_MainDDAlarmContinue = 0;

            //通讯异常报警
            m_ComStatusA = MNet.Basic._mnet_get_com_status(m_RingNoA);
            if (m_ComStatusA != 1)
            {
                if (m_ComStatusErrorDelay < 20)
                {
                    m_ComStatusErrorDelay++;
                }
                else
                {
                    m_ComStatusErrorDelay = 0;
                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 4, 0);//舌头缩回                   

                    m_ErrorFlag = 1; fALAM("警告", AlarmMess[61]);
                    fSystemQuit();
                }
            }
            else
            {
                m_ComStatusErrorDelay = 0;
            }


            if (m_LoadAlarm42 == true)
            {
                if (m_LoadKeyIn == 4)
                {
                    m_ErrorFlag = 1; fALAM("警告", AlarmMess[21]);
                    m_LoadAlarm42 = false;
                }
                else if (m_LoadKeyIn == 15)
                {
                    m_ErrorFlag = 1; fALAM("警告", AlarmMess[31]);
                    m_LoadAlarm42 = false;
                }
            }

            if (m_OutBoxAlarmFlag == true)
            {
                m_ErrorFlag = 1;
                fALAM("警告", AlarmMess[59]);
                m_OutBoxAlarmFlag = false;
                // fSystemQuit();
            }


            /////////////////////上料接驳台，花篮进sensor被遮挡，不允许运动    
            MNet.M1A._mnet_m1a_motion_done(m_RingNoA, ClineAddr.LoadAX0IP, ref GetState1);//升降模组停止运动 
            if ((m_LoadIN42 == 1) && ((GetState1 != 0) || ((m_LoadKeyIn < 9) && (m_LoadKeyIn > 6))))
            {
                if (m_AlamLoadContinue < 5)
                    m_AlamLoadContinue++;
                else
                {
                    m_AlamLoadContinue = 0;
                    MNet.M1A._mnet_m1a_emg_stop(m_RingNoA, ClineAddr.LoadAX0IP);//升降模组停止运动 
                    m_ErrorFlag = 1; m_LoadHomeFlag = 0;
                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 4, 0);//舌头缩回
                    fALAM("警告", AlarmMess[42]);
                }
            }
            else
                m_AlamLoadContinue = 0;
            /////硅片遮挡报警
            MNet.M1A._mnet_m1a_motion_done(m_RingNoA, ClineAddr.LoadAX0IP, ref GetState1);//升降模组停止运动 
            if ((m_LoadIN43 == 1) && (m_LoadKeyIn > 0) && (m_LoadKeyIn < 16))
            {
                if (m_AlamLoad1Continue < 30)
                    m_AlamLoad1Continue++;
                else
                {
                    m_AlamLoad1Continue = 0;
                    m_ErrorFlag = 1;
                    fALAM("警告", AlarmMess[50]);
                    m_LoadOff = true;
                    CheckBox_LoadOff.Checked = m_LoadOff;
                }
            }
            else m_AlamLoad1Continue = 0;

        }

        /*********************************************************************************************************
        ** 函数名称 ：AutoAlarm
        ** 函数功能 ：自动化报警处理
        ** 修改时间 ：20170921
        ** 修改内容 ：
        *********************************************************************************************************/
        void fAutoAlarm()
        {
            ushort GetState1 = 0;
            ////////////CCD报警////////////////////
            #region
            switch (m_CCDAlarm)
            {
                case 1:
                    //fEMGStop();		//紧急停止
                    m_ErrorFlag = 1;//出现异常 
                    m_TooSkew++;
                    fALAM("警告", "相机没有打开，拍照不能完成，请先打开相机，再自动运行！");
                    // Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 7, 1);   //关闭CCD光源   
                    CCDSTATUS.Value = m_CCDStatus;
                    AutoPause.Value = true;
                    m_WaferBreak = 1; m_CCDAlarm = 0;
                    m_CCDFinishFlag = 1; m_LaserRelease = 0;
                    break;
                case 2:
                    fBoxStop();
                    m_ErrorFlag = 1;//出现异常  
                    m_Broken++;
                    fALAM("警告", "拍照位置，硅片异常(破片)，系统将自动丢弃该硅片，请确认！");
                    m_WaferBreak = 1; m_CCDAlarm = 0;
                    m_CCDFinishFlag = 1; m_LaserRelease = 0;
                    // Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 7, 1);   //关闭CCD光源   
                    break;
                case 3:
                    fBoxStop();
                    m_TooSkew++;
                    fALAM("警告", "拍照位置，硅片放置太偏，系统将自动丢弃该硅片，请确认！");
                    m_WaferBreak = 1; m_CCDAlarm = 0;
                    m_CCDFinishFlag = 1; m_LaserRelease = 0;
                    //  Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 7, 1);   //关闭CCD光源   
                    break;
                case 4:
                    fBoxStop();
                    m_ErrorFlag = 1;//出现异常    
                    fALAM("警告", "CCD数据刷新失败，点击确认再拍一次！！");
                    AutoPause.Value = true;
                    m_nState = 100;
                    m_CCDAlarm = 0;
                    // Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 7, 1);   //关闭CCD光源   
                    break;
                case 5:
                    fBoxStop();
                    fALAM("警告", "拍照失败，点击确认再拍一次！");
                    m_nState = 100;
                    if (m_CCDAlarm5Count > 2)
                    {
                        fEMGStop();		//紧急停止
                        m_ErrorFlag = 1;//出现异常 
                        fALAM("警告", "相机异常，设备将自动停机，请检修相机！");
                        m_CCDAlarm5Count = 0;
                    }
                    break;
            }
            #endregion

            //抽尘机气缸报警
            byte m_DustStatus = 0;
            Motion.mAcm_AxDoGetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_TWO], 5, ref m_DustStatus);
            //motion.fGetBit(m_RingNoA, ClineAddr.MainXNT, 3, 3, ref m_DustStatus);//上料下压气缸  

            if ((!(m_CIN2 == 1) && (m_DustStatus == 1)) || (!(m_CIN3 == 1) && (m_DustStatus == 0)))
            {
                if (m_DustErrorCheck < 50)
                    m_DustErrorCheck++;
                else
                {
                    m_DustErrorCheck = 0;
                    m_ErrorFlag = 1;
                    if (m_DustCrack == 1)
                        fALAM("警告", AlarmMess[80]);

                    g_DustActionFlag = false;
                    m_DustActionStep = 0;
                    Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_TWO], 5, 0);
                    //motion.fSetBit(m_RingNoA, ClineAddr.MainXNT, 3, 3, 0);//抽尘气缸缩回
                }
            }
            else
            {
                m_DustErrorCheck = 0;
            }

            ///////////////主机报警/////////////////////////////
            ////进料模组 1   
            if (m_AIN1 == 1)	   //////进料电机1堵料 
            {
                if (m_InMotor1Continue < 15)  //延时15*16ms
                    m_InMotor1Continue++;
                else
                {
                    fBoxStop();
                    m_ErrorFlag = 1; fALAM("警告", AlarmMess[1]);
                    Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_ONE], 0, ref m_AIN0);   //进料buff硅片检测	 
                    if (m_AIN0 == 1)
                        m_In1AhomeFlag = 1;
                    else
                        m_In1AhomeFlag = 0;

                    m_InMotor1Continue = 0;

                    AutoPause.Value = true;
                }
            }
            else
                m_InMotor1Continue = 0;

            /*  if (m_AIN2 == 1)	   //////进料电机1堵料 
              {
                  if (m_InMotor2Continue < 50)  //延时1s
                      m_InMotor2Continue++;
                  else
                  {
                      fBoxStop();
                      m_ErrorFlag = 1; fALAM("警告", AlarmMess[3]);
                       m_InMotor2Continue = 0;

                  }
              }
              else
                  m_InMotor2Continue = 0;*/

            ////进料模组 2  
            if (m_BIN1 == 1)	   //////进料电机2堵料 
            {
                if (m_InMotor2Continue < 15)  //延时1s
                    m_InMotor2Continue++;
                else
                {
                    fBoxStop();
                    m_ErrorFlag = 1; fALAM("警告", AlarmMess[6]);
                    Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_ONE], 2, ref m_AIN2);   //进料模组1硅片检测
                    if (m_AIN2 == 0)
                    {
                        m_MainIn2 = 0;
                    }
                    else
                    {
                        m_MainIn2 = 1;
                    }
                    Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_ONE], 0, ref m_AIN0);   //进料buff硅片检测
                    if (m_AIN0 == 1)
                        m_In1AhomeFlag = 1;
                    else
                        m_In1AhomeFlag = 0;

                    m_InMotor2Continue = 0;
                    if (m_RotateInFlag < 2)
                    {
                        if (g_Posnt[1] == 0)
                        {
                            Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_TWO], 5, 0);  //上料A松开吸附
                            m_RotateInFlag = 0;
                        }
                        else if (g_Posnt[1] == 180 * pci1245l.RotateRatio)
                        {
                            Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_TWO], 4, 0);  //上料B松开吸附
                            m_RotateInFlag = 0;
                        }
                    }
                    AutoPause.Value = true;
                }
            }
            else
                m_InMotor2Continue = 0;

            ////遛片检测  
            Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_TWO], 1, ref m_BIN1);   //进料模组2硅片定位
            if ((m_BIN1 == 1) && (m_BIN1Flag == 1))   //确保  下游每一个要料信号 只执行一次对接
            {
                if (m_AlarmBIN0BIN1Delay == 0)
                    m_AlarmBIN0BIN1Delay = 1;
                m_AlarmBIN0BIN1Flag++;
                m_BIN1Flag = 0;
            }
            if (m_BIN1 == 0)
            {
                m_BIN1Flag = 1;
            }
            if ((m_AlarmBIN0BIN1Delay > 0) && (m_AlarmBIN0BIN1Delay < m_AlarmBIN1Time))
                m_AlarmBIN0BIN1Delay++;
            else
            {
                m_AlarmBIN0BIN1Flag = 0;
                m_AlarmBIN0BIN1Delay = 0;
            }

            if (m_AlarmBIN0BIN1Flag == 2)
            {
                Motion.mAcm_AxStopEmg(ax1Handle[pci1245l.Axis_TWO]);
                fBoxStop();
                m_ErrorFlag = 1; fALAM("警告", AlarmMess[78]);
                m_AlarmBIN0BIN1Flag = 0;
            }


            /////A,B,C,D台面吸附报警
            if ((m_DDWaferInFlag == true) && (g_MainStep == 6))
            {
                if (m_AlamDownContinue < 50)  //延时50*16ms，确保确保吸附时间足够 
                    m_AlamDownContinue++;
                else
                {
                    m_AlamDownContinue = 0;
                    switch (g_CWPositionFlag)
                    {
                        case 0:
                            if (m_DIN1 == 0) //A台面吸附没有到位
                            {
                                fBoxStop();
                                Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_ONE], 5, 0);	//DD台面A释放  
                                m_DDWaferInFlag = false;
                                m_DDSuckfail = true;
                                m_ErrorFlag = 1; fALAM("警告", AlarmMess[9]);
                                m_DDSuckfail = false;

                                if ((m_UIN2 == 0) && (m_XIN0 == 0) && (m_UIN3 == 0))//四个台面都无硅片
                                {
                                    g_MainStep = 2; m_MainStepContinue = 0; m_UnLoadSuck = false;
                                }
                                AutoPause.Value = true;
                            }
                            break;
                        case 1:
                            if (m_UIN2 == 0) //B台面吸附没有到位
                            {
                                fBoxStop();
                                Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_ONE], 4, 0);	//DD台面B释放   
                                m_DDWaferInFlag = false;
                                m_DDSuckfail = true;
                                m_ErrorFlag = 1; fALAM("警告", AlarmMess[10]);
                                m_DDSuckfail = false;

                                if ((m_DIN1 == 0) && (m_XIN0 == 0) && (m_UIN3 == 0))//四个台面都无硅片
                                {
                                    g_MainStep = 2; m_MainStepContinue = 0; m_UnLoadSuck = false;
                                }
                                AutoPause.Value = true;
                            }
                            break;
                        case 2:
                            if (m_XIN0 == 0) //C台面吸附没有到位
                            {
                                fBoxStop();
                                Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_ONE], 7, 0);	//DD台面C释放  
                                m_DDWaferInFlag = false;
                                m_ErrorFlag = 1; fALAM("警告", AlarmMess[11]);


                                if ((m_DIN1 == 0) && (m_UIN2 == 0) && (m_UIN3 == 0))//四个台面都无硅片
                                {
                                    g_MainStep = 2; m_MainStepContinue = 0; m_UnLoadSuck = false;
                                }
                                AutoPause.Value = true;
                            }
                            break;
                        case 3:
                            if (m_UIN3 == 0) //D台面吸附没有到位
                            {
                                fBoxStop();
                                Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_ONE], 6, 0);	//DD台面D释放   
                                m_DDWaferInFlag = false;
                                m_ErrorFlag = 1; fALAM("警告", AlarmMess[12]);

                                if ((m_DIN1 == 0) && (m_UIN2 == 0) && (m_XIN0 == 0))//四个台面都无硅片
                                {
                                    g_MainStep = 2; m_MainStepContinue = 0; m_UnLoadSuck = false;
                                }
                                AutoPause.Value = true;
                            }
                            break;
                    }
                }
            }
            else
                m_AlamDownContinue = 0;

            //////////上料A吸附异常报警
            if ((g_Posnt[1] == 0) && (m_RotateInFlag == 1) && (m_DIN3 == 0) && (m_LoadSuck == 1) && (m_WaferUpFlag == false))       //进料旋转电机A吸附异常
            {
                if (m_WaferSuckDelay < 100)  //延时1秒钟吸附
                    m_WaferSuckDelay++;
                else
                {
                    m_WaferSuckDelay = 0;
                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_TWO], 5, 0);  //上料A松开吸附
                    m_LoadSuck = 0;
                    m_ErrorFlag = 1;
                    fALAM("警告", AlarmMess[7]);
                    Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_ONE], 2, ref m_AIN2);   //进料模组1硅片检测
                    if (m_AIN2 == 0)
                    {
                        m_MainIn2 = 0;
                    }
                    else
                    {
                        m_MainIn2 = 1;
                    }
                    m_RotateInFlag = 0;
                }
            }
            else if ((g_Posnt[1] == 0) && (m_DIN3 == 1))
                m_WaferSuckDelay = 0;


            //////////上料B吸附异常报警
            if ((g_Posnt[1] == 180 * pci1245l.RotateRatio) && (m_RotateInFlag == 1) && (m_LoadSuck == 1) && (m_DIN2 == 0) && (m_WaferUpFlag == false))   //进料旋转电机，B吸附异常
            {
                if (m_WaferSuckDelay < 100)  //延时1秒钟吸附
                    m_WaferSuckDelay++;
                else
                {
                    m_WaferSuckDelay = 0;
                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_TWO], 4, 0);  //上料B松开吸附
                    m_LoadSuck = 0;
                    m_ErrorFlag = 1;
                    fALAM("警告", AlarmMess[7]);
                    if (m_AIN2 == 0)
                    {
                        m_MainIn2 = 0;
                    }
                    else
                    {
                        m_MainIn2 = 1;
                    }
                    m_RotateInFlag = 0;
                }
            }
            else if ((g_Posnt[1] == 180 * pci1245l.RotateRatio) && (m_DIN2 == 1))
                m_WaferSuckDelay = 0;


            //////////下料A吸附异常报警
            if ((g_OutRotateMotorPosition == 0) && (m_ZIN2 == 0) && (m_RotateOutFlag == 3))
            {
                if (m_WaferOutSuckDelay < 100)  //延时1秒钟吸附
                    m_WaferOutSuckDelay++;
                else
                {
                    m_WaferOutSuckDelay = 0;
                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 4, 0);  //下料吸附A松开
                    m_ErrorFlag = 1;
                    fALAM("警告", AlarmMess[8]);
                    if (m_WaferBreak == 2)
                    {
                        m_WaferBreakOutError = 1;
                    }
                    else
                        m_RotateOutFlag = 0;

                }
            }
            else if ((g_OutRotateMotorPosition == 0) && (m_ZIN2 == 1))
                m_WaferOutSuckDelay = 0;


            //////////下料B吸附异常报警
            if ((g_OutRotateMotorPosition != 0) && (m_ZIN3 == 0) && (m_RotateOutFlag == 3))
            {
                if (m_WaferOutSuckDelay < 100)  //延时1秒钟吸附
                    m_WaferOutSuckDelay++;
                else
                {
                    m_WaferOutSuckDelay = 0;
                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 5, 0);  //下料吸附B松开
                    m_ErrorFlag = 1;
                    fALAM("警告", AlarmMess[8]);
                    if (m_WaferBreak == 2)
                    {
                        m_WaferBreakOutError = 1;
                    }
                    else
                        m_RotateOutFlag = 0;

                }
            }
            else if ((g_OutRotateMotorPosition != 0) && (m_ZIN2 == 1))
                m_WaferOutSuckDelay = 0;

            ////////////////////////出料模组1报警//////////////////
            //if (m_Out1WaferAlarm > 2)
            //{
            //    m_ErrorFlag = 1; fALAM("警告", AlarmMess[56]);
            //    m_Out1WaferAlarm = 0;
            //}
            /////////////硅片被Buffer卡住，没有正常进入Buffer
            //if (m_CIN0 == 1)
            //{
            //    if (m_Out11WaferDelay < 30)
            //        m_Out11WaferDelay++;
            //    else
            //    {
            //        m_ErrorFlag = 1; fALAM("警告", AlarmMess[56]);
            //        m_Out11WaferDelay = 0;
            //    }
            //}
            //else
            //    m_Out11WaferDelay = 0;

            ////////////上料接驳台舌头
            //MNet.Basic._mnet_bit_io_input(m_RingNoA, ClineAddr.LoadD122IP, 0, 3, ref m_LoadIN43);//
            //if (m_LoadIN43 == 1) m_LoadToughWafer = false;
            if (m_LoadToughWafer == true)
            {
                if (m_LoadToughWaferCount < 20)
                    m_LoadToughWaferCount++;
                else
                {
                    fBoxStop(); m_LoadToughWaferCount = 0;
                    m_ErrorFlag = 1; fALAM("警告", AlarmMess[16]);
                    m_LoadToughWafer = false; m_IN11Flag = 0;
                    m_LoadOff = true;
                    CheckBox_LoadOff.Checked = m_LoadOff;
                }
            }
            else
                m_LoadToughWaferCount = 0;
            LoadToughWafer.Value = Convert.ToInt32(m_LoadToughWafer);

            //////////安全门报警
            // Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_TWO], 3, ref m_BIN3);    //门保护    
            if (((m_BIN3 == 0) || (m_LoadIN17 == 0)) && (m_DoorProtect == false))
            {
                fBoxStop();
                m_ErrorFlag = 1; fALAM("警告", AlarmMess[15]);
                //m_Pause = true;
                AutoPause.Value = true;
            }

            //水冷报警
            if (m_YIN0 == 1)
            {
                fBoxStop();
                m_ErrorFlag = 1; fALAM("警告", AlarmMess[74]);
                fEMGStop();
            }
            ///////////////////////////////////////激光状态检测//////////////////////////////////////////
            if ((m_YIN2 == 1) && (m_LaserCheck == false))   //如果光闸已经打开，没有检测到激光出光,
            {
                if ((m_MarkFinish > 0) && (m_MarkFinish < 11))
                {
                    if (m_LaserRelease == 0)
                        m_LaserRelease = 1;
                    Motion.mAcm_AxDiGetBit(ax2Handle[pci1245l.Axis_ONE], 2, ref m_XIN2);
                    if ((m_XIN2 == 1) && (m_LaserRelease > 0))
                        m_LaserRelease++;

                }
                else
                {
                    if (m_LaserRelease == 1)
                    {
                        m_ErrorFlag = 1; fALAM("警告", AlarmMess[29]);
                        fEMGStop();
                    }
                    m_LaserRelease = 0;
                }
            }

            //////////总气检测//////////   
            if (m_Load_YHome == 0)
            {
                if (m_TotalGaspressureDelay < 50)
                    m_TotalGaspressureDelay++;
                else
                {
                    m_TotalGaspressureDelay = 0;
                    m_ErrorFlag = 1; fALAM("警告", AlarmMess[45]);
                    fEMGStop();
                }
            }
            else
                m_TotalGaspressureDelay = 0;

            if (m_XIN3 == 1)
            {
                m_ErrorFlag = 1; fALAM("警告", AlarmMess[25]);
                fEMGStop();
            }


        }
        /*********************************************************************************************************
        ** 函数名称 ：fStartRun
        ** 函数功能 ：自动化运行
        ** 入口参数 ：
        ** 出口参数 ： 
         *********************************************************************************************************/
        void fDim(bool data)
        {

            KEY_OPTSwitch.Enabled = data;
            //  KEY_LoadHandOut.Enabled = data;
            ToolKeyPress.Enabled = data;
            TakeOnePhoto.Enabled = data;
            //comboBoxCCD.Enabled = data;
            OpenCCDTool.Enabled = data;
            SaveCCDTool.Enabled = data;
            GotoMM.Enabled = data;
            Marking.Enabled = data;
            MenuMainReset.Enabled = data;
            MenuOpenGraph.Enabled = data;

            OpenGraph.Enabled = data;
            ToolSysReset.Enabled = data;
            CCDONOFF.Enabled = data;
            LightONOFF.Enabled = data;

            //DoorProtect.Enabled = data;
            LaserCheck.Enabled = data;
            Modify.Enabled = data;

            KEY_LaserOnOff.Enabled = data;
            KEY_DD45AngleRun.Enabled = data;

        }
        /*********************************************************************************************************
        ** 函数名称 ：fStartRun
        ** 函数功能 ：自动化运行
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        void fStartRun()
        {
            m_LoadOff = false;
            CheckBox_LoadOff.Checked = m_LoadOff;
            uint AutoAlarm = 0; m_flagCheck2 = true;
            DialogResult ResultTemp;

            if ((m_YPOS == 0) || (m_ZPOS == 0) || (m_UIN1 == 0))	  //旋转臂或者DD马达没有电
                AutoAlarm = 1;
            if (m_CCDWaferStand < 20000)	  //CCD标准值错误
                AutoAlarm = 2;
            if ((m_CCDBaseX == 0) || (m_Load1220XElevatorStart == 0) || (m_PowerInv == 0))	  //系统参数丢失
                AutoAlarm = 3;

            if ((m_DIN1 == 1) || (m_UIN2 == 1) || (m_UIN3 == 1) || (m_XIN0 == 1))  //台面有硅片,或者吸附没有弹起
                AutoAlarm = 5;
            if (((m_BIN3 == 0) || (m_LoadIN17 == 0)) && (m_DoorProtect == false))   //门保护
                AutoAlarm = 6;
            if (m_Load_YHome == 0)//总气负压不够
                AutoAlarm = 7;
            if (m_SysIniReset == 1)//系统没有复位         
                AutoAlarm = 8;
            else
                m_IniBuzzer = 3;
            if (m_MainBreak == true)
                AutoAlarm = 9;
            if ((m_XHomeFlag > 0) || (m_YHomeFlag > 0) || (m_LoadHomeFlag < 5))
                AutoAlarm = 12;
            if (((Math.Abs(m_CenterX) > 0.001) || (Math.Abs(m_CenterY) > 0.001)) && (NoCentreAlarm.Checked == true)) //图形中心点不是（0，0）
                AutoAlarm = 14;
            if (g_Clean)  //皮带清理
                AutoAlarm = 15;
            switch (AutoAlarm)
            {
                case 0:   //正常运行

                    ResultTemp = MessageBox.Show(this, "请确认DD台面上是否有硅片，清除后再执行自动化运行！", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (ResultTemp == DialogResult.OK)   //表示点击确认
                    {
                        m_flagStatus1 = true;
                        LogClass.Text = "运行";
                        //激光器状态初始化     
                        axMMMark1.MarkData_UnLock();
                        axMMIO1.SetOutput(15, 1); //关闭Gate 
                        //axMMMark1.LaserOff();
                        TextShow.Text = "请注意：正在自动化运行！";
                        m_WaferBreak = 0; m_LoadIN62InFlag = 0; m_LoadIN62MoveFlag = 0;
                        Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 5, 1); //打开光闸                
                        KEY_OPTSwitch.Value = true;
                        Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 7, 1);//打开背光源
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_ONE], 4, 0);   //关闭B台面吸附
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_ONE], 6, 0);   //关闭D台面吸附
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_ONE], 5, 0);   //关闭A台面吸附 
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_ONE], 7, 0);   //关闭C台面吸附
                        axMMMark1.MarkData_Lock();
                        System.Threading.Thread.Sleep(500);//延时500ms
                        g_MainStep = 2;
                        m_CCDFinishFlag = 0; m_MarkFinish = 0; m_CCDAlarm = 0;
                        //if (m_LoadKeyIn == 16)   //停止后，重新开始
                        //    m_LoadKeyIn = m_TempLoadKeyIn;
                        AutoRun.Enabled = false;
                        AutoPause.Enabled = true;
                        Stop.Enabled = true;
                        Modify.Visible = false;
                        Clean.Enabled = false;
                        //关闭照明灯   
                        Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 4, 0);
                        LightONOFF.Checked = false;
                        timerTotal.Enabled = true;//打开计数定時器  
                        KEY_StartMeasure.Enabled = false;
                        Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_ONE], 6, 1);    //总气阀打开       
                        /////// /Buffer下面硅片
                        if (m_AIN0 == 1)
                            m_In1AhomeFlag = 1;
                        if (m_AIN2 == 1)
                            m_MainIn2 = 1;
                        fDim(false);
                        m_CCDNoWafer = true;
                    }

                    break;
                case 1://相机连续拍照报警
                    m_ErrorFlag = 2;
                    fALAM("温馨提示", "旋转臂或者DD马达没有送电或者异常，请检查！");
                    break;
                case 2://CCD标准值错误
                    m_ErrorFlag = 2;
                    fALAM("温馨提示", "CCD标准值错误，请重新设置！");
                    break;
                case 3://系统参数丢失
                    m_ErrorFlag = 2;
                    fALAM("温馨提示", "系统参数丢失，自动化不能运行，请检查！");
                    break;
                case 4://下料接驳台有按键按下
                    m_ErrorFlag = 2;
                    fALAM("温馨提示", "功率测量汽缸没有缩回，自动化不能运行，请先手动缩回汽缸，再运行！");
                    break;
                case 5://
                    m_ErrorFlag = 2;
                    fALAM("温馨提示", "DD台面有硅片没有被取下，请先清理硅片，或者吸附按钮被按下，请恢复按钮，再运行！");
                    break;
                case 6://安全门被打开
                    m_ErrorFlag = 2;
                    fALAM("温馨提示", "安全门没有关闭，请先关闭，再运行自动程序！");
                    break;
                case 7://总气负压不够
                    m_ErrorFlag = 2;
                    fALAM("温馨提示", "总气负压不够，请检查气路是否正常，再运行自动程序！");
                    break;
                case 8://系统没有复位
                    m_ErrorFlag = 2;
                    fALAM("温馨提示", "系统没有复位，请先复位，再运行自动程序！");

                    m_IniBuzzer = 3;
                    break;
                case 9://功率汽缸伸出，不能自动运行
                    m_ErrorFlag = 2;
                    fALAM("温馨提示", "刚刚正在处理拍照异常时，系统运行停止，需要重新复位，再自动运行！");

                    break;
                case 10://上料接驳台被打开，需要关闭  
                    m_ErrorFlag = 2;
                    fALAM("温馨提示", "上料接驳台面板被打开，需要先关闭 ，再自动运行！");

                    break;
                case 11://下料接驳台被打开，需要关闭
                    m_ErrorFlag = 2;
                    fALAM("温馨提示", "下料接驳台面板被打开，需要先关闭，再自动运行！");

                    break;
                case 12://系统正在复位中，需要关闭
                    m_ErrorFlag = 2;
                    fALAM("温馨提示", "系统正在复位过程中，请等待复位完成，再自动运行！");

                    break;
                case 13://功率汽缸处于伸出状态   
                    m_ErrorFlag = 2;
                    fALAM("温馨提示", "功率测量汽缸没有缩回，请先手动缩回功率汽缸，再自动运行！");

                    break;
                case 14://打标图形不在中心点
                    m_ErrorFlag = 2;
                    fALAM("温馨提示", "打标图形中心点没有居中，请重新调整图形，并再次打开！");

                    break;
                case 15://
                    m_ErrorFlag = 2;
                    fALAM("温馨提示", "请先关闭皮带清理后，再运行！");
                    break;
            }
        }
        /*********************************************************************************************************
        ** 函数名称 ：AutoRun_Click
        ** 函数功能 ：自动化运行
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        private void AutoRun_Click(object sender, EventArgs e)
        {
            fStartRun();

        }
        /*********************************************************************************************************
        ** 函数名称 ：AutoPause_StateChanged
        ** 函数功能 ：暂停
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        private void AutoPause_StateChanged(object sender, ActionEventArgs e)
        {
            m_Pause = AutoPause.Value;

            fPauseProcess(m_Pause);
        }
        /*********************************************************************************************************
        ** 函数名称 ：Stop_Click
        ** 函数功能 ：停止
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        private void Stop_Click(object sender, EventArgs e)
        {
            fEMGStop();
        }
        /*********************************************************************************************************
        ** 函数名称 ：Stop_Click
        ** 函数功能 ：停止
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        private void DoorProtect_CheckedChanged(object sender, EventArgs e)
        {
            m_DoorProtect = DoorProtect.Checked;

        }
        /*********************************************************************************************************
        ** 函数名称 ：Marking_Click
        ** 函数功能 ：手动打标
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        private void Marking_Click(object sender, EventArgs e)
        {
            if ((g_CCDSource[0] < -15.0) || (g_CCDSource[0] > 15.0) || (g_CCDSource[1] > 15.0) || (g_CCDSource[1] < -15.0))
            { m_ErrorFlag = 2; fALAM("警告", AlarmMess[40]); }
            else
            {
                axMMMark1.MarkData_UnLock();
                //axMMMark1.LaserOff();
                axMMIO1.SetOutput(15, 1); //关闭Gate 

                Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 5, 1);		//打开光闸
                KEY_OPTSwitch.Value = true;
                axMMMark1.JumpToStartPos();
                axMMMark1.SetMatrix(g_CCDSource[0], g_CCDSource[1], g_CCDSource[2]);
                axMMMark1.MarkData_Lock();
                System.Threading.Thread.Sleep(300);
                fScanRunSample(g_CCDSource[2], g_CCDSource[0], g_CCDSource[1]);
            }
        }
        /*********************************************************************************************************
        ** 函数名称 ：axMMStatus1_MarkEnd
        ** 函数功能 ：打标结束回调函数
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        private void axMMStatus1_MarkEnd(object sender, AxMMSTATUSLib._DMMStatusEvents_MarkEndEvent e)
        {
            int TimeData;
            ///////////////////////////手动雕刻中，图形雕刻时间获取///////////////////		
            if (g_MainStep == 30)
            {

                TimeData = axMMMark1.GetMarkTime();
                this.Invoke((EventHandler)(delegate
               {
                   MarkTime.Value = TimeData / 1000.0;
                   if (m_MarkTimeFlag)
                   {
                       m_MarkTimeFlag = false;
                       this.Invoke((EventHandler)(delegate
                       {
                           MarkTime.BackColor = Color.Green;
                       }));
                   }
                   else
                   {
                       m_MarkTimeFlag = true;
                       this.Invoke((EventHandler)(delegate
                       {
                           MarkTime.BackColor = Color.LightBlue;
                       }));
                   }

                   // Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_ONE], 7, 0);//加工吹气关闭 
                   Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 5, 0);   //关闭光闸

                   KEY_OPTSwitch.Value = false;
                   System.Threading.Thread.Sleep(500);
                   //axMMMark1.LaserOn();//打开Gate 
                   axMMIO1.SetOutput(15, 0); //打开Gate 
               }));
            }
            else//自动化状态
            {
                TimeData = axMMMark1.GetMarkTime();
                this.Invoke((EventHandler)(delegate
                {
                    MarkTime.Value = TimeData / 1000.0;
                    if (m_MarkTimeFlag)
                    {
                        m_MarkTimeFlag = false;
                        this.Invoke((EventHandler)(delegate
                        {
                            MarkTime.BackColor = Color.Green;
                        }));
                    }
                    else
                    {
                        m_MarkTimeFlag = true;
                        this.Invoke((EventHandler)(delegate
                        {
                            MarkTime.BackColor = Color.LightBlue;
                        }));
                    }

                    m_MarkFinish = 11;
                }));

            }
        }
        /*********************************************************************************************************
        ** 函数名称 ：KeyPress_Click
        ** 函数功能 ：调用登录对话框
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        private void KeyPress_Click(object sender, EventArgs e)
        {
            FormPassword PassForm = new FormPassword();
            PassForm.StartPosition = FormStartPosition.CenterScreen;
            PassForm.ChangeData += new ChangePWFormData(f_ChangePWData);
            PassForm.ShowDialog();

        }

        void f_ChangePWData(bool topGetState4)
        {
            /* if (topGetState4)
                 this.BackColor = Color.Green;
             else
                 this.BackColor = Color.Gray;*/
        }
        /*********************************************************************************************************
        ** 函数名称 ：Modify_CheckedChanged
        ** 函数功能 ：切换检修状态
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        private void Modify_CheckedChanged(object sender, EventArgs e)
        {
            ushort GetState1 = 20, GetState2 = 20;
            g_Modify = Modify.Checked;

            if (g_InRotateHome == false)
            {
                MessageBox.Show(this, "上料旋转臂没有复位，不能执行检修功能！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Modify.Checked = false;
            }
            else
            {
                if (g_Modify == true)
                {
                    fEMGStop();
                    Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_TWO], ref GetState1);
                    Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_THREE], ref GetState2);
                    if (GetState1 == 1 && GetState2 == 1)
                    {
                        Motion.mAcm_AxMoveRel(ax2Handle[pci1245l.Axis_THREE], 90 * FormHMI.pci1245l.RotateRatio);//下料转90度
                        Motion.mAcm_AxMoveAbs(ax2Handle[pci1245l.Axis_TWO], 90 * FormHMI.pci1245l.RotateRatio);

                        Modify.Enabled = false;
                    }
                    System.Threading.Thread.Sleep(400);//延时400ms
                    Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_TWO], ref GetState1);
                    Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_THREE], ref GetState2);
                    if (GetState1 == 1 && GetState2 == 1)
                    {
                        LogClass.Text = "当前为检修状态";
                        AutoRun.Visible = false;
                        Modify.Enabled = true;
                        if (m_LoadKeyIn == 16)   //停止后，重新开始
                            m_LoadKeyIn = m_TempLoadKeyIn;
                    }
                    else
                    {
                        m_ErrorFlag = 1; fALAM("警告", AlarmMess[58]);
                    }
                }

                else
                {
                    Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_TWO], ref GetState1);
                    Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_THREE], ref GetState2);
                    if (GetState1 == 1 && GetState2 == 1)
                    {
                        Motion.mAcm_AxMoveRel(ax2Handle[pci1245l.Axis_THREE], -90 * FormHMI.pci1245l.RotateRatio);//下料转-90度
                        if (FormHMI.g_InRotateMotorPosition == 0)    //0度位置
                        {
                            Motion.mAcm_AxMoveAbs(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_TWO], 0);
                        }
                        if (FormHMI.g_InRotateMotorPosition == 1)
                        {
                            Motion.mAcm_AxMoveAbs(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_TWO], 180 * FormHMI.pci1245l.RotateRatio);
                        }

                        Modify.Enabled = false;
                    }
                    System.Threading.Thread.Sleep(400);//延时400ms
                    Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_TWO], ref GetState1);
                    Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_THREE], ref GetState2);
                    if (GetState1 == 1 && GetState2 == 1)
                    {
                        LogClass.Text = "取消检修状态";
                        Modify.Enabled = true;
                        AutoRun.Visible = true;
                    }
                }
            }
        }
        /*********************************************************************************************************
        ** 函数名称 ：timerCheck_Tick
        ** 函数功能 ：检查通信状态
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        private void timerCheck_Tick(object sender, EventArgs e)
        {
            ushort GetState = 0;
            uint ReStatus = 0;
            // ReStatus=Motion.mAcm_AxGetState(ax3Handle[pci1245l.Axis_FIVE], ref GetState);//硅片传送模组运动完成
        }
        /*********************************************************************************************************
        ** 函数名称 ：fSystemQuit
        ** 函数功能 ：系统退出
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        void fSystemQuit()
        {
            int i;
            string sysp;
            StreamWriter write = new StreamWriter(file_nameIni);
            m_LoadHomeFlag = 0;
            mcTimer.Abort();
            Thread_T1.Abort();
            Thread_T2.Abort();
            write.WriteLine(0);
            write.Close();

            //關閉定時器 
            timerRead.Enabled = false;
            timerBuzzer.Enabled = false;
            timerCheck.Enabled = false;

            /////////关闭灯塔 
            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_THREE], pci1245l.BuzzerOut, 0);
            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_THREE], pci1245l.GreenOut, 0);
            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_THREE], pci1245l.YellowOut, 0);
            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_THREE], pci1245l.RedOut, 0);
            Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_TWO], 7, 0);//本机准备好清零
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 2, 1, 0);//阻挡2伸出
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 2, 2, 0);//阻挡3伸出
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 2, 6, 0);//出料气缸伸出
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 2, 3, 0);//下压缩回
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 2, 4, 0);//舌头缩回

            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 5, 0); //关闭光闸  
            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 1, 0);//进花篮电机2反转停止
            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 1, 0);//进花篮电机2运动停止 
            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 2, 0);//托盘电机停止
            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 4, 0);//出花篮电机1运动停止
            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 2, 0);////出花篮电机1反转停止
            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 0, 0);//进花篮电机1正转停止
            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 0, 0);////进花篮电机1反转停止
            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 5, 0);// 出花篮电机2正转停止
            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 3, 0);////出花篮电机2反转停止
            axMMIO1.SetOutput(15, 0);
            //axMMMark1.LaserOn(); //打开Gate
            /* 数组保存 */
            g_SysParam[0] = Convert.ToDouble(g_LoadElevationP); g_SysParam[2] = g_CWPositionFlag;
            g_SysParam[40] = m_TotalQulity; g_SysParam[69] = Convert.ToDouble(m_MainIn2);//进料模组2是否有料
            g_SysParam[52] = m_Broken;
            g_SysParam[53] = m_TooSkew;
            StreamWriter writeSys = new StreamWriter("D:\\DRLaser\\SysPara.ini");
            for (i = 0; i < 80; i++)
            {
                sysp = Convert.ToString(FormHMI.g_SysParam[i]);
                writeSys.WriteLine(sysp);
            }
            writeSys.Close();
            System.Threading.Thread.Sleep(200);
            axMMMark1.Finish();
            axCKVisionCtrl1.Dispose();
            Motion.mAcm_DevClose(ref dev1Handle); Motion.mAcm_DevClose(ref dev2Handle);
            MNet.Basic._mnet_stop_ring(m_RingNoA);
            Master.PCI_L122_DSF._l122_dsf_close(m_Existcards);

            ComPower.Close();
            Close();

        }
        /*********************************************************************************************************
        ** 函数名称 ：fAutoWaferTransfore
        ** 函数功能 ：主机自动进出料
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        void fAutoWaferTransfore()
        {
            ushort GetState = 0, GetStateA = 0, GetStateB = 0, GetStateC = 0, GetStateD = 0, RunStatus = 0;
            ushort GetState0 = 0, GetState1 = 0;
            uint WaferStatus, MoudleIn0 = 0, MoudleIn1 = 0, MoudleIn2 = 0, MoudleIn3 = 0, ToughWafer = 0;
            Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_ONE], ref GetStateA);  //进料电机1运动判断 
            Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_TWO], ref GetStateB);  //进料电机2运动判断  

            #region [进料逻辑]
            switch (m_In_WaferFlag)
            {
                case 0:   //如果舌头上有料，舌头与进料电机1运动	   
                    GetState = 10;
                    Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_ONE], ref GetStateC);  //等待Buffer运动完成             
                    MNet.M1A._mnet_m1a_motion_done(m_RingNoA, ClineAddr.LoadAX0IP, ref GetState0);//升降模组停止运动 
                    if (GetState0 == 0) GetState0 = 1;
                    else GetState0 = 0;
                    MNet.M1A._mnet_m1a_motion_done(m_RingNoA, ClineAddr.LoadAX1IP, ref GetState1);//硅片传送模组运动完成
                    if (GetState1 == 0) GetState1 = 1;
                    else GetState1 = 0;
                    Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_ONE], ref GetStateA);
                    Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_TWO], ref GetStateB);
                    Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_ONE], 2, ref m_AIN2);   //进料模组1硅片检测
                    if ((m_AIN2 == 1) && (((g_Posnt[1] == 0) && (m_DIN3 == 0)) || ((g_Posnt[1] == 180 * pci1245l.RotateRatio) && (m_DIN2 == 0))))
                    {
                        m_MainIn2 = 1;
                    }
                    if (m_LoadOff == false)
                    {
                        if (m_LoadIN40 == 0)
                            ToughWafer = 0;
                        else
                            ToughWafer = 1;
                    }
                    else
                        ToughWafer = 1;
                    if (ToughWafer == 0)//舌头有料  
                    {
                        if (m_MainIn2 == 1)//进料模组2不允许硅片进入  
                        {
                            if (m_In1AhomeFlag == 0)//进料模组1buffer下没有料,且舌头上有料 
                            {
                                if ((m_LoadKeyIn != 7) && (m_LoadKeyIn != 8))	 //上料花篮不在自动加工位置, 
                                {
                                    //if ((GetState1 == 1) &&( m_flagCheck1 == false) &&(GetState0 == 1) && (GetStateA == 1) && (GetStateC == 1) && (m_AIN1 == 0) && (m_Load_XPos == 1))   //进料电机1，2没有运动	  
                                    if ((GetState1 == 1) && (m_flagCheck1 == false) && (GetState0 == 1) && (GetStateA == 1) && (m_AIN1 == 0) && (m_Load_XPos == 1))   //进料电机1，2没有运动
                                    {
                                        m_In1AhomeFlag = 1; m_In_WaferFlag = 5;
                                        MNet.M1A._mnet_m1a_start_r_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX1IP, (int)(pci1245l.WaferRatio * m_LoadMotorOffset));//硅片传送电机跑300mm
                                        Motion.mAcm_AxMoveRel(ax1Handle[pci1245l.Axis_ONE], pci1245l.In2MaxDistance * pci1245l.WaferRatio);
                                        m_flagCheck1 = true;
                                    }

                                }
                                else  //上料花篮处于自动加工位置,  //舌头有料，进料1buffer下面无料，且进料2不允许运动     
                                {
                                    if ((m_InONOFF == 0) && (m_InBufferRelease == 0))
                                    {
                                        if ((GetState1 == 1) && (m_flagCheck1 == false) && (GetState0 == 1) && (GetStateA == 1) && (m_AIN1 == 0) && (m_LoadKeyIn != 7))
                                        {
                                            m_In1AhomeFlag = 1; m_In_WaferFlag = 5;
                                            if (m_LoadIN50 == 0) m_LoadToughWafer = true;
                                            Motion.mAcm_AxMoveRel(ax1Handle[pci1245l.Axis_ONE], pci1245l.In2MaxDistance * pci1245l.WaferRatio); 	   //进料1运动
                                            MNet.M1A._mnet_m1a_start_r_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX1IP, (int)(pci1245l.WaferRatio * m_LoadMotorOffset));//硅片传送电机跑300mm
                                            m_flagCheck1 = true; m_LoadKeyIn = 7; //继续运行

                                        }
                                    }
                                }
                            }
                        }
                        else//进料模组2允许硅片进入  
                        {
                            if ((m_LoadKeyIn != 7) && (m_LoadKeyIn != 8))		 //上料花篮不在自动加工位置, 
                            {
                                if (m_In1AhomeFlag == 1)   //如果进料模组1buffe下有料
                                {
                                    //if ((GetStateA == 1) && (m_flagCheck1 == false) && (GetStateB == 1) && (GetState1 == 1) && (GetState0 == 1) && (GetStateC == 1) && (m_AIN1 == 0))   //进料电机1，2没有运动	  
                                    if ((GetStateA == 1) && (m_flagCheck1 == false) && (GetStateB == 1) && (GetState1 == 1) && (GetState0 == 1) && (m_AIN1 == 0))   //进料电机1，2没有运动
                                    {
                                        MNet.M1A._mnet_m1a_start_r_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX1IP, (int)(pci1245l.WaferRatio * m_LoadMotorOffset));//硅片传送电机跑300mm
                                        Motion.mAcm_AxMoveRel(ax1Handle[pci1245l.Axis_ONE], pci1245l.In2MaxDistance * pci1245l.WaferRatio);
                                        Motion.mAcm_AxMoveRel(ax1Handle[pci1245l.Axis_TWO], pci1245l.In2MaxDistance * pci1245l.WaferRatio);   //跑长距离，等待IN5信号停止  
                                        m_MainIn2 = 1;
                                        m_In_WaferFlag = 3; m_flagCheck1 = true;
                                    }
                                }
                                else   //如果进料模组1buffe下无料  
                                {
                                    if ((GetState1 == 1) && (m_flagCheck1 == false) && (GetState0 == 1) && (GetStateA == 1) && (m_AIN1 == 0))   //进料电机1没有运动
                                    {
                                        MNet.M1A._mnet_m1a_start_r_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX1IP, (int)(pci1245l.WaferRatio * m_LoadMotorOffset));//硅片传送电机跑300mm
                                        Motion.mAcm_AxMoveRel(ax1Handle[pci1245l.Axis_ONE], pci1245l.In2MaxDistance * pci1245l.WaferRatio);
                                        m_In1AhomeFlag = 1; m_In_WaferFlag = 5; m_flagCheck1 = true;

                                    }
                                }
                            }
                            else  //上料花篮处于自动加工位置, 
                            {
                                if ((m_InONOFF == 1) || (m_InBufferRelease == 1))
                                {
                                    if ((g_LoadBuffer > 0) && (m_In1AhomeFlag == 0))
                                    {
                                        Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_ONE], ref GetStateA);
                                        Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_TWO], ref GetStateB);
                                        Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_ONE], ref GetStateC);
                                        if ((GetStateA == 1) && (GetStateC == 1))   //进料电机1没有运动	  
                                        {
                                            m_In_WaferFlag = 1;//缓存电机下降时，不允许进料电机以及舌头电机运动   
                                            Motion.mAcm_AxMoveRel(ax2Handle[pci1245l.Axis_ONE], 5 * pci1245l.BufferRatio); //缓存电机下降一格
                                            g_LoadBuffer--;
                                        }
                                    }
                                    else if (m_In1AhomeFlag == 1)  //进料1buffer下有料,且进料2无料,且没有运动
                                    {
                                        Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_ONE], ref GetStateA);
                                        Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_TWO], ref GetStateB);
                                        //Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_ONE], ref GetStateC);
                                        //if ((GetStateA == 1) && (GetStateB == 1) && (GetStateC == 1) && (m_AIN1 == 0))   //进料电机1，2没有运动
                                        if ((GetStateA == 1) && (GetStateB == 1) && (m_AIN1 == 0))   //进料电机1，2没有运动	  
                                        {
                                            m_In_WaferFlag = 4;
                                            m_MainIn2 = 1;
                                            m_In1AhomeFlag = 0;
                                            Motion.mAcm_AxMoveRel(ax1Handle[pci1245l.Axis_ONE], pci1245l.ConstDistance * pci1245l.WaferRatio);
                                            Motion.mAcm_AxMoveRel(ax1Handle[pci1245l.Axis_TWO], pci1245l.In2MaxDistance * pci1245l.WaferRatio);   //跑长距离，等待IN5信号停止
                                        }
                                    }
                                    if (g_LoadBuffer == 0)
                                        m_InBufferRelease = 0;
                                }
                                else	 //舌头有料，且进料2允许运动  
                                {
                                    if (m_In1AhomeFlag == 1)  //进料模组1有料 ，*******重点修改************
                                    {
                                        Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_ONE], ref GetStateA);
                                        Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_TWO], ref GetStateB);
                                        //Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_ONE], ref GetStateC);
                                        if ((GetStateA == 1) && (GetStateB == 1) && (m_AIN1 == 0))   //进料1，2没有运动	  
                                        {
                                            if (m_LoadKeyIn != 7)
                                            {
                                                if ((GetState0 == 1) && (m_flagCheck1 == false) && (GetState1 == 1) && (m_AIN1 == 0))
                                                {
                                                    m_In_WaferFlag = 4;
                                                    m_MainIn2 = 1;
                                                    m_In1AhomeFlag = 1;
                                                    if (m_LoadIN50 == 0) m_LoadToughWafer = true;
                                                    Motion.mAcm_AxMoveRel(ax1Handle[pci1245l.Axis_ONE], pci1245l.In2MaxDistance * pci1245l.WaferRatio);    //进料1运动
                                                    Motion.mAcm_AxMoveRel(ax1Handle[pci1245l.Axis_TWO], pci1245l.In2MaxDistance * pci1245l.WaferRatio);   //进料2运动  
                                                    MNet.M1A._mnet_m1a_start_r_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX1IP, (int)(pci1245l.WaferRatio * m_LoadMotorOffset));//硅片传送电机跑300mm
                                                    m_flagCheck1 = true; m_LoadKeyIn = 7; //继续运行

                                                }
                                            }
                                            else//升降模组或者舌头有运动
                                            {
                                                if (m_In1Delay < 20)//&&(m_InBufferUp==false ))
                                                    m_In1Delay++;
                                                else
                                                {
                                                    m_In1Delay = 0;// m_InBufferUp = false ;
                                                    m_In_WaferFlag = 4;
                                                    m_MainIn2 = 1;
                                                    m_In1AhomeFlag = 0;
                                                    Motion.mAcm_AxMoveRel(ax1Handle[pci1245l.Axis_ONE], pci1245l.ConstDistance * pci1245l.WaferRatio);    //进料1运动
                                                    Motion.mAcm_AxMoveRel(ax1Handle[pci1245l.Axis_TWO], pci1245l.In2MaxDistance * pci1245l.WaferRatio);   //进料2运动  
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        // if ((GetStateA == 1) && (RunStatus == 1) && (GetState == 1) && (m_AIN1 == 0) && (m_Load_XPos == 1) && (m_LoadKeyIn != 7))
                                        Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_ONE], ref GetStateA);
                                        Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_TWO], ref GetStateB);
                                        //Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_ONE], ref GetStateC);
                                        if ((GetStateA == 1) && (m_flagCheck1 == false) && (GetState0 == 1) && (GetState1 == 1) && (m_LoadKeyIn != 7) && (m_AIN1 == 0))
                                        {
                                            m_In1AhomeFlag = 1; m_In_WaferFlag = 5;
                                            if (m_LoadIN50 == 0) m_LoadToughWafer = true;
                                            Motion.mAcm_AxMoveRel(ax1Handle[pci1245l.Axis_ONE], pci1245l.In2MaxDistance * pci1245l.WaferRatio); 						//进料1运动
                                            MNet.M1A._mnet_m1a_start_r_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX1IP, (int)(pci1245l.WaferRatio * m_LoadMotorOffset));//硅片传送电机跑300mm
                                            m_flagCheck1 = true; m_LoadKeyIn = 7; //继续运行

                                        }
                                    }
                                }

                            }
                        }
                    }
                    else //舌头无料
                    {
                        if (((m_LoadKeyIn != 7) || (m_FirstWaferFlag == 1)) && (m_LoadKeyIn != 8))	 //上料花篮不再自动加工位置，或者花篮进料被取消,且舌头无料   
                        {
                            if ((g_LoadBuffer > 0) && (m_In1AhomeFlag == 0) && (m_AIN1 == 0) && (m_AIN0 == 0))
                            {
                                Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_ONE], ref GetStateA);
                                Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_TWO], ref GetStateB);
                                Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_ONE], ref GetStateC);
                                if ((GetStateA == 1) && (GetStateC == 1))   //进料电机1没有运动	  
                                {
                                    m_In_WaferFlag = 1;//缓存电机下降时，不允许进料电机以及舌头电机运动	
                                    Motion.mAcm_AxMoveRel(ax2Handle[pci1245l.Axis_ONE], 5 * pci1245l.BufferRatio); //缓存电机下降一格
                                    g_LoadBuffer--;
                                }
                            }
                            else if ((m_In1AhomeFlag == 1) && (m_MainIn2 == 0))  //进料1buffer下有料,且进料2无料,且没有运动
                            {
                                Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_ONE], ref GetStateA);
                                Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_TWO], ref GetStateB);
                                //Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_ONE], ref GetStateC);
                                //if ((GetStateA == 1) && (GetStateB == 1) && (GetStateC == 1) && (GetState1 == 1) && (m_AIN1 == 0) && (m_BIN1 == 0))   //进料电机1，2没有运动	  
                                if ((GetStateA == 1) && (GetStateB == 1) && (GetState1 == 1) && (m_AIN1 == 0) && (m_BIN1 == 0))   //进料电机1，2没有运动
                                {
                                    m_In_WaferFlag = 4;
                                    m_MainIn2 = 1;
                                    m_In1AhomeFlag = 0;
                                    Motion.mAcm_AxMoveRel(ax1Handle[pci1245l.Axis_ONE], pci1245l.ConstDistance * pci1245l.WaferRatio);
                                    Motion.mAcm_AxMoveRel(ax1Handle[pci1245l.Axis_TWO], pci1245l.In2MaxDistance * pci1245l.WaferRatio);   //跑长距离，等待IN5信号停止 
                                }
                            }
                        }
                    }

                    break;
                case 1: //等待进料Buffer下降运动完成   
                    Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_ONE], ref GetStateC);  //等待Buffer运动完成 
                    if (GetStateC == 1)
                    {
                        m_In1AhomeFlag = 1;
                        m_In_WaferFlag = 0;
                        m_In1Delay = 0;
                    }
                    break;
                case 2: //等待进料Buffer上升运动完成   
                    if (m_InStepConter < 1)//延时3*16ms
                        m_InStepConter++;
                    else
                    {
                        m_InStepConter = 0;
                        m_In_WaferFlag = 0;
                        m_In1Delay = 0;
                    }
                    break;
                case 3:  //料已经传送到进料2位置 
                    Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_ONE], ref GetStateA);
                    Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_TWO], ref GetStateB);
                    if (m_InStepConter < 2)//延时40ms
                        m_InStepConter++;
                    else
                    {
                        if ((GetStateA == 1) && (GetStateB == 1)) //进料电机1,2没有运动
                        {
                            m_InStepConter = 0;
                            m_In_WaferFlag = 0;
                            m_In1Delay = 0;
                        }
                    }
                    break;
                case 4:  //料已经传送到进料1,2位置
                    Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_ONE], ref GetStateA);
                    if (m_InStepConter < 2)//延时40ms
                        m_InStepConter++;
                    else
                    {
                        if (GetStateA == 1) //进料电机1没有运动
                        {
                            m_InStepConter = 0;
                            m_In1Delay = 0;
                            m_In_WaferFlag = 0;
                        }
                    }
                    break;
                case 5:  //料已经传送到进料1位置 
                    Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_ONE], ref GetStateA);
                    if (m_InStepConter < 2)//延时40ms
                        m_InStepConter++;
                    else
                    {
                        if (GetStateA == 1)   //进料电机1没有运动 
                        {
                            m_InStepConter = 0;
                            m_In_WaferFlag = 0;
                            m_In1Delay = 0;
                        }
                    }
                    break;
            }
            ////////////////////////进料Buffer电机上升//////////////////////////////////////	
            Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_ONE], ref GetStateA);  //进料电机1运动判断  
            Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_ONE], ref GetState);  //等待Buffer运动完成 
            Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_ONE], 1, ref m_AIN1);   //进料模组1定位
            //	if((m_In1AhomeFlag==1)&&(m_AIN2==1)&&(InONOFF==0)&&(GetState==1)&&(GetStateA==1)&&((m_LoadKeyIn==7)||(m_LoadKeyIn==8)))//如果进料1Buffer下面有料，且进料模组2不允许硅片进入
            if ((m_In1AhomeFlag == 1) && (m_MainIn2 == 1) && (m_InONOFF == 0) && (GetStateA == 1) && (GetState == 1) && ((m_LoadKeyIn == 7) || (m_LoadKeyIn == 8)) && (m_FirstWaferFlag == 0) && (m_InBufferRelease == 0))
            {
                //if ((g_LoadBuffer < g_InBufferTotal) && (m_InBufferUp == false) && (m_In_WaferFlag != 2) && (m_In_WaferFlag != 1) && (m_XPOS == 1) && (m_APOS == 1) && (m_AIN1 == 0))  //且定位1没有被遮挡
                if ((g_LoadBuffer < g_InBufferTotal) && (m_InBufferUp == false) && (m_In_WaferFlag != 2) && (m_In_WaferFlag != 1) && (m_XPOS == 1) && (m_AIN1 == 0))  //且定位1没有被遮挡
                {

                    m_In1AhomeFlag = 0;
                    Motion.mAcm_AxMoveRel(ax2Handle[pci1245l.Axis_ONE], -5 * pci1245l.BufferRatio);      //缓存电机上升一格 
                    g_LoadBuffer++; m_InBufferUp = true; m_LoadWaferQTemp = g_LoadWaferQ;
                    m_In_WaferFlag = 2;
                }

            }
            if (g_LoadWaferQ > (m_LoadWaferQTemp + 5))//4
            { m_InBufferUp = false; }
            #endregion
            #region [出料逻辑]
            ////////////////////出料模组2,3运动，科隆威逻辑：我主动，科隆威被动////////////////////////       
            Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_THREE], ref GetStateC);  //出料电机1运动判断 
            Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_THREE], 0, ref m_CIN0);//出料1.1硅片检测
            Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_THREE], 1, ref m_CIN1); //出料1.2遮挡    
            Motion.mAcm_AxDiGetBit(ax1Handle[pci1245l.Axis_ONE], 3, ref m_AIN3); //下游设备要片信号

            /////////////////////////出料旋转臂甩片报警//////////////////
            if ((GetStateC == 1) && (m_Out1WaferAlarm2Flag == 1))
            {
                if (m_CIN1 == 0)
                {
                    m_ErrorFlag = 1; fALAM("警告", AlarmMess[62]);
                    m_Out1WaferAlarm2Flag = 0;
                    //if (m_waferoutflag == 2)
                    //    m_waferoutflag = 0;
                }
                else
                {
                    m_Out1WaferAlarm2Flag = 0;
                }
            }
            /////////////////////////出料堵片报警//////////////////
            if (GetStateC == 1)
            {
                if (m_CIN0 == 1)
                {
                    m_ErrorFlag = 1;
                    fALAM("警告", AlarmMess[56]);
                }
            }

            if (m_SignalNeed)
            {
                MoudleIn0 = (uint)(m_ReadyNow == 1 ? 1 : 0);
            }
            else
            {
                MoudleIn0 = m_AIN3;
            }

            //  MoudleIn2 = m_CIN0;
            MoudleIn2 = m_waferoutflag;//出料模组1起始位置有硅片       
            MoudleIn1 = m_CIN1;
            WaferStatus = MoudleIn0 * 100 + MoudleIn1 * 10 + MoudleIn2;//AIN3+CIN1+m_waferoutflag
            Edit_WaferStatus.Value = WaferStatus;
            switch (m_CommunicateStatus)
            {
                case 0:
                    switch (WaferStatus)
                    {
                        case 010://模组1.2有料，但模组1.1没有料，需要延时判断后，再发送ready信号
                            Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_THREE], ref GetStateC);
                            if (GetStateC == 1)
                            {
                                if (m_Out3DelayCount < 100)   //延时2秒
                                    m_Out3DelayCount++;
                                else
                                {
                                    m_Out3DelayCount = 0;
                                    m_YOUT7 = 1;
                                    Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_TWO], 7, m_YOUT7);//本机准备好
                                    m_ReadyNow = 1;
                                    LED_YOUT7.Value = true;

                                    m_CommunicateStatus = 1;
                                }
                            }
                            break;

                        case 110:    //模组1.2有料    
                            Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_THREE], ref GetStateC);
                            if (GetStateC == 1)
                            {
                                m_Out3DelayCount = 0; m_Out1WaferAlarm2Flag = 0;
                                Motion.mAcm_AxMoveRel(ax1Handle[pci1245l.Axis_THREE], m_OutMotorOffset * pci1245l.WaferRatio);
                                m_YOUT7 = 0;
                                Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_TWO], 7, m_YOUT7);//本机清除Ready信号
                                m_ReadyNow = 0; m_Out1WaferAlarm++; m_waferoutflag = 0;
                                LED_YOUT7.Value = false;
                                m_CommunicateStatus = 1;
                            }
                            break;

                        case 011://出料模组1.2有料，科隆威准备好
                            m_Out3DelayCount = 0;
                            Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_THREE], ref GetStateC);
                            if (GetStateC == 1)
                            {
                                m_YOUT7 = 1;
                                Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_TWO], 7, m_YOUT7);//本机传送ready信号给科隆威
                                m_ReadyNow = 1;
                                LED_YOUT7.Value = true;
                                m_Out3DelayCount = 0;
                            }
                            break;

                        case 111://科隆威有响应，进行要片
                            m_Out3DelayCount = 0;
                            Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_THREE], ref GetStateC);
                            if (GetStateC == 1)
                            {
                                m_YOUT7 = 0;
                                Motion.mAcm_AxMoveRel(ax1Handle[pci1245l.Axis_THREE], m_OutMotorOffset * pci1245l.WaferRatio);
                                Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_TWO], 7, m_YOUT7);//本机清除Ready信号
                                m_ReadyNow = 0; m_Out1WaferAlarm++; m_waferoutflag = 0;
                                m_Out1WaferAlarm2Flag = 1;//甩片报警标志
                                LED_YOUT7.Value = false;
                                m_Out3DelayCount = 0;
                                m_CommunicateStatus = 1;
                            }
                            break;

                        case 101:
                        case 001://
                            Motion.mAcm_AxGetState(ax1Handle[pci1245l.Axis_THREE], ref GetStateC);
                            if (GetStateC == 1)
                            {
                                m_Out3DelayCount = 0;
                                m_CommunicateStatus = 1; m_Out1WaferAlarm++;
                                Motion.mAcm_AxMoveRel(ax1Handle[pci1245l.Axis_THREE], m_OutMotorOffset * pci1245l.WaferRatio);
                                m_waferoutflag = 0;
                                m_Out1WaferAlarm2Flag = 1;//甩片报警标志
                            }
                            break;
                    }
                    break;

                case 1:///延时等待
                    if (m_Out3DelayCount < 5)//延时判断
                        m_Out3DelayCount++;
                    else
                    {
                        m_Out3DelayCount = 0; m_CommunicateStatus = 0;
                    }
                    break;
            }
            #endregion

            /////////////////////////////////////周期性测量激光功率//////////////////////////////////////////
            //if ((m_TotalQulity > 0) && ((m_TotalQulity % m_PowerInv) == 0) && (m_Pause == false) && (m_InvAdd == false) && (m_MarkFinish == 11))   //周期性功率测量
            //{
            //    m_Pause = true; m_InvAdd = true;

            //    fPauseProcess(m_Pause);
            //    m_PowerCounter = 1;
            //    timerPower.Enabled = true;  //启动定時器  
            //        }


        }
        /*********************************************************************************************************
        ** 函数名称 ：timerOPT_Tick
        ** 函数功能 ：光闸自动控制
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        private void timerOPT_Tick(object sender, EventArgs e)
        {
            int IsMarking = 0;
            if ((g_MainStep < 30) && (m_Pause == false))//自动运行状态,且没有暂停
            {
                if (m_AutoMoveCounter == m_MarkFinish)		//且没有打标 
                {
                    if (m_TimerOPTFlag < 80)//延时8s
                        m_TimerOPTFlag++;
                }
                else
                    m_TimerOPTFlag = 0;
                m_AutoMoveCounter = m_MarkFinish;
                if (m_TimerOPTFlag == 80)//延时8s
                {
                    IsMarking = axMMMark1.IsMarking();
                    if (IsMarking == 1)   //正在加工 
                        m_TimerOPTFlag = 0;
                    else
                    {
                        if (m_YIN2 == 1)//光闸打开sensor 
                        {
                            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 5, 0); //关闭光闸      
                            KEY_OPTSwitch.Value = false;
                            m_TimerOPTFlag = 81;
                        }
                    }
                }
                else if ((m_TimerOPTFlag > 80) && (m_TimerOPTFlag < 85))  //
                    m_TimerOPTFlag++;
                else if (m_TimerOPTFlag == 85)  //延时0.5s释放Gate
                {
                    axMMIO1.SetOutput(15, 0);
                    //axMMMark1.LaserOn();
                    m_TimerOPTFlag = 0;
                }

            }
            else
                m_TimerOPTFlag = 0;
        }
        /*********************************************************************************************************
        ** 函数名称 ：Clean_Click
        ** 函数功能 ：计时清零
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        private void Clean_Click(object sender, EventArgs e)
        {
            int i;
            string sysp;
            TotalQulity.Value = 0;
            TotalTime.Value = 0;
            m_TotalTime = 0;
            m_TotalQulity = 0;

            StreamWriter writeSys = new StreamWriter("D:\\DRLaser\\SysPara.ini");
            for (i = 0; i < 80; i++)
            {
                sysp = Convert.ToString(FormHMI.g_SysParam[i]);
                writeSys.WriteLine(sysp);
            }
            writeSys.Close();
        }
        /*********************************************************************************************************
        ** 函数名称 ：timerTotal_Tick
        ** 函数功能 ：系统定时器
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        private void timerTotal_Tick(object sender, EventArgs e)
        {
            string SystemTime = null;
            if ((m_AlarmFlag == 0) && (m_Pause == false))
            {
                m_TotalTime++;
                TotalTime.Value = m_TotalTime;
            }
            ////////////获得当前年月日，如果发现日期有变化，创建新文件//////////////////////////////////////////////////////////   

            //SystemTime = DateTime.Now.ToLongDateString();
            //if (m_CurrentDay != DateTime.Now.DayOfYear.ToString())//判断是否是同一天
            //{
            //    ///////错误记录文件              
            //    SystemTime = "d:\\DRLaser\\ALARM\\" + SystemTime + ".txt";
            //    //新建文件
            //    FileStream Errorfile = new FileStream(SystemTime, FileMode.Create);
            //    Errorfile.Close();
            //    // ErrorfileHandle = OpenFile(file_name_Error, VAL_READ_WRITE, VAL_APPEND, VAL_ASCII);  //再次打开	
            //    m_CurrentDay = DateTime.Now.DayOfYear.ToString();
            //}
        }

        private void CCDONOFF_CheckedChanged(object sender, EventArgs e)
        {

            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 7, Convert.ToByte(CCDONOFF.Checked));
        }

        private void LightONOFF_CheckedChanged(object sender, EventArgs e)
        {
            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 4, Convert.ToByte(LightONOFF.Checked));
        }

        /*********************************************************************************************************
        ** 函数名称 ：FormHMI_KeyDown
        ** 函数功能 ：主界面快捷键
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        private void FormHMI_KeyDown(object sender, KeyEventArgs e)
        {
            if (m_SysResetFlag == true)
            {
                if ((e.KeyData == Keys.Escape) && (Stop.Enabled == true))
                    fEMGStop();
                if ((e.KeyData == Keys.F1) && (AutoRun.Enabled == true) && (Modify.Checked == false))
                    fStartRun();
                if ((e.KeyData == Keys.F9) && (AutoPause.Enabled == true))
                {
                    if (AutoPause.Value == false)
                        AutoPause.Value = true;
                    else
                        AutoPause.Value = false;
                    m_Pause = AutoPause.Value;

                    fPauseProcess(m_Pause);
                }
                /////权限对话框
                if ((e.KeyData == Keys.F10) && (Stop.Enabled == false))
                {
                    FormPassword PassForm = new FormPassword();
                    PassForm.StartPosition = FormStartPosition.CenterScreen;
                    PassForm.ChangeData += new ChangePWFormData(f_ChangePWData);
                    PassForm.ShowDialog();

                }
                /////图形打开
                if ((e.KeyData == Keys.F5) && (Stop.Enabled == false) && (FormPassword.g_Authority > 0))
                {
                    this.openFile.FileName = "*.ezm";
                    if (openFile.ShowDialog() == DialogResult.OK)
                    {
                        //AutoRun.Enabled = true;
                        axMMMark1.LoadFile(openFile.FileName);
                        axMMMark1.Redraw();
                        m_CenterX = axMMEdit1.GetCenterX("root");
                        m_CenterY = axMMEdit1.GetCenterY("root");
                        GotoMM.Enabled = true;
                        Marking.Enabled = true;
                        MMDirection.Text = openFile.FileName;
                    }

                }
                /////CCD拍照
                if ((e.KeyData == Keys.F12) && (Stop.Enabled == false))
                {
                    fCCDRead(g_CCDSource);  //读取CCD数值  
                }
            }
        }
        /*********************************************************************************************************
        ** 函数名称 ：ToolSysReset_Click
        ** 函数功能 ：系统一键复位
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        private void ToolSysReset_Click(object sender, EventArgs e)
        {

            DialogResult ResultTemp;
            double Bufferm_VelLow = pci1245l.BufferRatio * 1, Bufferm_VelHigh = pci1245l.BufferRatio * 20;

            ushort Get1State = 0;
            uint LoadHome = 0;
            //////上料接驳台复位
            if ((m_LoadHomeFlag == 0) || (m_LoadHomeFlag == 5) || (m_LoadHomeFlag == 4))
            {
                m_LoadKeyIn = 0;
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 2, 0);
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 3, 0);
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 1, 0);
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 0, 0);
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 0, 0);
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 1, 0);
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 2, 0);
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 3, 0);
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 4, 0);
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 5, 0);
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 3, 0);//下压气缸缩回
                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 0, 0);//进料阻挡汽缸1缩回      
                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 5, 0);//出料阻挡汽缸1缩回  
                MNet.M1A._mnet_m1a_motion_done(m_RingNoA, ClineAddr.LoadAX0IP, ref Get1State);//升降模组停止运动        
                if ((Get1State == 0) || (Get1State == 3))
                {
                    //  if ((m_LoadCIN1 == 1) && (m_LoadAIN2 == 0) && (m_ID6 == 0))//舌头缩回到位,硅片离开sensor，花篮托盘sensor没有被遮挡 
                    if ((m_LoadIN47 == 1) && (m_LoadIN43 == 0) && (m_LoadIN42 == 0))
                    {
                        LoadHome = 1;

                    }
                    else
                    {
                        m_ErrorFlag = 2;//出现异常
                        fALAM("警告", "请检查“上料接驳台”：\n  1.“舌头”模组是否缩回到位？\n  2.上下对射式传感器没有被遮挡？");

                    }
                }
                else
                {
                    m_ErrorFlag = 2;//出现异常
                    fALAM("警告", "上料接驳台有运动轴没有停止运动，归零运动不能执行！");
                }
            }
            else if (m_LoadHomeFlag < 4)
            {
                m_ErrorFlag = 2;//出现异常
                fALAM("警告", "上料接驳台正在复位中，不能再执行复位操作！");
            }

            if (LoadHome == 1)//接驳台准备好，才能进行下面动作
            {
                /////////////主机复位
                if ((m_XPOS == 0) || (m_YPOS == 0) || (m_ZPOS == 0))  //DD马达或者旋转臂没有送电,或者接驳台存在问题
                {
                    m_ErrorFlag = 2;
                    fALAM("温馨提示", "DD马达或者旋转臂没有送电或者处于异常状态，或者接驳台没有准备好，请检查！");
                }
                else
                {

                    Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_THREE], pci1245l.BuzzerOut, 1);
                    System.Threading.Thread.Sleep(200);//延时200ms
                    Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_THREE], pci1245l.BuzzerOut, 0);

                    ResultTemp = MessageBox.Show(this, "请确认是否要进行系统复位！", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (ResultTemp == DialogResult.OK)   //表示点击确认  
                    {

                        LogClass.Text = "系统正在复位中....，请勿进行其它操作！！！";
                        this.Enabled = false;
                        m_SysReset = 1;
                        Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_TWO], 6, 0);//  下料旋转气缸回原位
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_TWO], 5, 0);  //上料A吸附关闭
                        Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_ONE], 5, 0);//上料A吸盘反吹关闭
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_TWO], 4, 0);  //上料B吸附关闭
                        Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_ONE], 4, 0);//上料B吸盘反吹关闭  

                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 4, 0);  //下料吸附A关闭
                        Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_TWO], 4, 0);  //下料反吹A关闭
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 5, 0);  //下料吸附B关闭
                        Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 6, 0);  //下料反吹B关闭
                        m_LoadHandOut = false;
                        // m_AIN2 = 0;
                        //先上升50mm                
                        MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNoA, ClineAddr.LoadAX0IP, pci1245l.ElevatorRatio * 5, pci1245l.ElevatorRatio * 50, 0.1f, 0.1f);//升降模组
                        MNet.M1A._mnet_m1a_start_r_move(m_RingNoA, ClineAddr.LoadAX0IP, (int)(80 * pci1245l.ElevatorRatio));
                        m_LoadHomeFlag = 1;

                        /////////////速度设置/////////////////////
                        /////////设置Buffer速度
                        Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_ONE], (uint)PropertyID.PAR_AxVelLow, Bufferm_VelLow);//进料Buffer
                        Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_ONE], (uint)PropertyID.PAR_AxVelHigh, Bufferm_VelHigh);
                        //  Motion.mAcm_SetF64Property(ax1Handle[pci1245l.Axis_FOUR], (uint)PropertyID.PAR_AxVelLow, Bufferm_VelLow);//出料Buffer
                        // Motion.mAcm_SetF64Property(ax1Handle[pci1245l.Axis_FOUR], (uint)PropertyID.PAR_AxVelHigh, Bufferm_VelHigh);
                        Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_TWO], (uint)PropertyID.PAR_AxVelLow, pci1245l.RotateRatio * 5);//上料旋转臂
                        Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_TWO], (uint)PropertyID.PAR_AxVelHigh, pci1245l.RotateRatio * 10);
                        ////////设置旋转轴速度
                        Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_THREE], (uint)PropertyID.PAR_AxVelLow, pci1245l.RotateRatio * 5);
                        Motion.mAcm_SetF64Property(ax2Handle[pci1245l.Axis_THREE], (uint)PropertyID.PAR_AxVelHigh, pci1245l.RotateRatio * 10);

                        Motion.mAcm_AxMoveRel(ax2Handle[pci1245l.Axis_TWO], 10 * pci1245l.RotateRatio);
                        Motion.mAcm_AxMoveRel(ax2Handle[pci1245l.Axis_THREE], 10 * pci1245l.RotateRatio);
                        System.Threading.Thread.Sleep(100);//延时100ms
                        /////////DD归零
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 7, 0);   //启动关闭
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_THREE], 6, 0);  //低

                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 4, 1);  //低
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 5, 0);//高
                        System.Threading.Thread.Sleep(20);//延时2ms
                        Motion.mAcm_AxDoSetBit(ax2Handle[pci1245l.Axis_FOUR], 7, 1);//启动打开

                        m_UHomeFlag = 1; FormHMI.g_CWPositionFlag = 0;// this.ControlBox = false;

                        m_XHomeFlag = 1; m_YHomeFlag = 1; m_ZHomeFlag = 1; m_DHomeFlag = 1;
                        g_LoadBuffer = g_InBufferTotal;
                        m_SysIniReset = 0;
                    }
                }
            }
        }
        /*********************************************************************************************************
            ** 函数名称 ：LaserCheck_CheckedChanged
            ** 函数功能 ：激光检测选择
            ** 修改时间 ：20171222
            ** 修改内容 ：
            *********************************************************************************************************/
        private void LaserCheck_CheckedChanged(object sender, EventArgs e)
        {
            m_LaserCheck = LaserCheck.Checked;
        }

        /*********************************************************************************************************
        ** 函数名称 ：KEY_StartMeasure_Click
        ** 函数功能 ：功率测量开始
        ** 修改时间 ：20180511
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_StartMeasure_Click(object sender, EventArgs e)
        {
            DialogResult ResultTemp;
            Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 1);
            System.Threading.Thread.Sleep(200);//延时200ms
            Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 0);
            ResultTemp = MessageBox.Show(this, "请确认是否进行激光功率测量！", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (ResultTemp == DialogResult.OK)   //表示点击确认        
            {
                //axMMMark1.LaserOn();
                axMMIO1.SetOutput(15, 0); //打开Gate
                AutoRun.Visible = false;
                AutoPause.Visible = false;
                m_PowerStep = 1;
                timerPower.Enabled = true;
            }
        }


        /*********************************************************************************************************
        ** 函数名称 ：KEY_EndMeasure_Click
        ** 函数功能 ：功率测量结束
        ** 修改时间 ：20180511
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_EndMeasure_Click(object sender, EventArgs e)
        {
            timerPower.Enabled = false;
            m_MeasureCount = 0;
            LogClass.Text = "关闭光闸";
            Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 5, 0);   //光闸关闭  
            System.Threading.Thread.Sleep(300);//延时300ms
            KEY_OPTSwitch.Value = false;
            //axMMMark1.LaserOff();
            axMMIO1.SetOutput(15, 1); //关闭Gate
            KEY_EndMeasure.Enabled = false;
            m_PowerStep = 0;
            if (DDInit == false)
            {
                AutoRun.Visible = true;
                AutoPause.Visible = true;
                fDDPositionStatus(6);   //20220809测量功率结束    
                ++g_CWPositionFlag;
                if (g_CWPositionFlag > 3)
                    g_CWPositionFlag = 0;
            }
        }

        /*********************************************************************************************************
            ** 函数名称 ：KEY_DD45AngleRun_StateChanged
            ** 函数功能 ：DD旋转45°
            ** 修改时间 ：20171222
            ** 修改内容 ：
            *********************************************************************************************************/
        private void KEY_DD45AngleRun_StateChanged(object sender, ActionEventArgs e)
        {
            /*   if (KEY_DD45AngleRun.Value)
               {
                   Motion.mAcm_AxMoveRel(ax2Handle[pci1245l.Axis_FOUR], 45 * pci1245l.DDRatio);//顺时针45°
                   timerPower.Enabled = true; m_PowerOut = 1;
               }
               else
               {
                   Motion.mAcm_AxDoSetBit(ax1Handle[pci1245l.Axis_FOUR], 5, 0); //关闭光闸
                   //axMMMark1.LaserOff();
                   axMMIO1.SetOutput(15, 1); //关闭Gate 
                   m_PowerOut = 3;
                   System.Threading.Thread.Sleep(200);//延时200ms
                   Motion.mAcm_AxMoveRel(ax2Handle[pci1245l.Axis_FOUR], -45 * pci1245l.DDRatio);//逆时针45°
                   KEY_OPTSwitch.Value = false;
               }*/
        }
        /*********************************************************************************************************
            ** 函数名称 ：KEY_LaserOnOff_StateChanged
            ** 函数功能 ：Laser开关
            ** 修改时间 ：20171222
            ** 修改内容 ：
            *********************************************************************************************************/
        private void KEY_LaserOnOff_StateChanged(object sender, MouseEventArgs e)
        {
            if (KEY_LaserOnOff.Value)
                axMMIO1.SetOutput(15, 0); //打开Gate 
            //axMMMark1.LaserOn();
            else
                axMMIO1.SetOutput(15, 1); //关闭Gate 
            // axMMMark1.LaserOff();
        }
        /*********************************************************************************************************
            ** 函数名称 ：timerPassBy_Tick
            ** 函数功能 ：辅助定时器
            ** 修改时间 ：20180123
            ** 修改内容 ：
            *********************************************************************************************************/
        private void timerPassBy_Tick(object sender, EventArgs e)
        {
            //////////////////////感应电机控制逻辑////////////////////////////////////
            ///////////////上料接驳台////////////////////////////
            if (m_LoadKeyIn == 15)
            {
                if (m_LoadIN42 == 1)
                    m_LoadIN42Count++;
                if ((m_LoadIN22 == 1) && (m_LoadIN42Count > 0) && (m_LoadIN51 == 1) && (m_LoadIN52 == 1) && (m_LoadIN54 == 1)) //并且花篮进料被遮挡
                {
                    m_PassStepContinue1 = 0;
                    if (m_PassStepContinue < 30)  //延时20*16ms
                        m_PassStepContinue++;
                    else
                    {
                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 2, 0);//托盘电机正转停止
                        m_LoadCCWrun = 0;
                        m_LoadIN42Count = 0;
                        m_PassStepContinue = 0;
                        m_LoadKeyIn = 1;
                    }
                }

                else if (m_LoadIN42 == 0)
                {
                    if (m_PassStepContinue1 < 400)//延时50*16后
                    {
                        m_PassStepContinue1++;
                    }
                    else
                    {
                        m_PassStepContinue1 = 0;
                        m_OutBoxAlarmFlag = true;
                    }

                }
            }
            //进花篮电机1即按即走
            byte[] m_LoadOutPut = new byte[4];
            MNet.Basic._mnet_io_input_all_ex(m_RingNoA, ClineAddr.LoadD122IP, m_LoadOutPut);//获取122卡的输出
            m_LoadOut30 = Convert.ToByte(m_LoadOutPut[3] & 0x01);//进花篮电机1  
            m_LoadOut34 = Convert.ToByte((m_LoadOutPut[3] >> 4) & 0x01);//出花篮电机1 
            m_LoadOut35 = Convert.ToByte((m_LoadOutPut[3] >> 5) & 0x01);//出花篮电机2 
            if (m_LoadIN62 == 1)
            {
                m_LoadIN62Count++;
            }
            else
            {
                if (m_LoadIN62Count >= 75)
                {
                    m_LoadIN62MoveFlag = 0;
                    m_LoadIN62InFlag = 1;//IN
                    m_LoadIN62Count = 0;
                }
                else if (m_LoadIN62Count > 1)
                {
                    m_LoadIN62MoveFlag = 1;//花篮移动短距离
                    m_LoadIN62InFlag = 0;
                    m_LoadIN62Count = 0;
                }
                else
                {
                    //  m_LoadIN62MoveFlag = 0;
                    // m_LoadIN62InFlag = 0;
                    m_LoadIN62Count = 0;
                }
            }

            switch (m_LoadIN62MoveFlag)
            {
                case 1:
                    if ((m_LoadIn1run == 0) && (FormLoad.m_MotorIn1Run == 0) && (FormLoad.m_MotorIn2Run == 0) && (m_LoadOut30 == 0) && (m_LoadIN15 == 1))
                    {
                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 0, 0);////进花篮电机1反转停止
                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 0, 1);//进花篮电机1运行
                        MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 1, 0);//进花篮电机2反转停止
                        MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 1, 1);//进花篮电机2运动
                        m_LoadIN62MoveFlag = 2; m_BoxKeyInDelayCount = 0;
                    }
                    else
                    {
                        m_LoadIN62MoveFlag = 0; m_BoxKeyInDelayCount = 0;
                    }

                    break;
                case 2:
                    if (m_BoxKeyInDelayCount < m_BoxKeyInDelay)
                        m_BoxKeyInDelayCount++;
                    else
                    {
                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 0, 0);//进花篮电机1运行停止
                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 0, 0);////进花篮电机1反转停止
                        MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 1, 0);//进花篮电机2反转停止
                        MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 1, 0);//进花篮电机2运动
                        m_BoxKeyInDelayCount = 0; m_LoadIN62MoveFlag = 0;
                    }
                    break;
            }

            if (m_LoadIN62InFlag == 1)
            {
                if (m_LoadIN62InFlagCount < 10)
                    m_LoadIN62InFlagCount++;
                else
                {
                    m_LoadIN62InFlagCount = 0;
                    m_LoadIN62InFlag = 0;
                }
            }

            //OUT键
            if ((m_LoadIN57 == 1) && (m_LoadIN36 == 1) && (m_LoadOut1run == 0) && (m_LoadOut34 == 0) && (m_LoadOut35 == 0))//出料阻挡气缸伸出
            {
                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 2, 0);////出花篮电机反转停止
                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 4, 1);////出花篮电机运动
                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 3, 0);////出花篮电机2反转停止
                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 5, 1);////出花篮电机2运动
                m_LoadOut1run = 1;
            }


            //////////////感应电机逐个停止控制////////////////////////
            if ((m_LoadCCWrun > 0) && (m_LoadCCWrun < pci1245l.MotorDelay))
                m_LoadCCWrun++;
            else if (m_LoadCCWrun == pci1245l.MotorDelay)
            {
                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 2, 0);//托盘电机正转停止
                m_LoadCCWrun = 0;
            }
            ///////进料1，2
            if ((m_LoadIn1run > 0) && (m_LoadIn1run < pci1245l.LongMotorDelay) && ((g_LoadAGVCommStep < 2) || (g_LoadAGVCommStep == 20)))
                m_LoadIn1run++;
            else if (m_LoadIn1run == pci1245l.LongMotorDelay)
            {
                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 0, 0);//进花篮电机1停止
                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 0, 0);////进花篮电机1反转停止
                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 1, 0);//进花篮电机2反转停止
                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 1, 0);//进花篮电机2正转停止
                m_LoadIn1run = 0;
            }
            ///////出料1,2
            //if ((m_LoadOut1run > 0) && (m_LoadOut1run < pci1245l.LongMotorDelay) && ((g_UnLoadAGVCommStep < 2) || (g_UnLoadAGVCommStep == 20)))
            if ((m_LoadOut1run > 0) && (m_LoadOut1run < m_LongOutDelay) && ((g_UnLoadAGVCommStep < 2) || (g_UnLoadAGVCommStep == 20)))
                m_LoadOut1run++;
            //else if (m_LoadOut1run == pci1245l.LongMotorDelay)
            else if (m_LoadOut1run == m_LongOutDelay)
            {
                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 4, 0);//出花篮电机1停止
                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 2, 0);////出花篮电机1反转停止
                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 5, 0);//出花篮电机2正转停止
                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 3, 0);////出花篮电机2反转停止
                m_LoadOut1run = 0;
            }

            //手动OUT

            //if (m_LoadIN57 == 1)
            //{
            //    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 4, 1);////出花篮电机运动
            //    m_LoadOut1run = 1;
            //}

            ///////防止传送模组交接处，由于某个电机没有运动，导致倒花篮
            if (m_LoadIN42 == 1)
            {
                if (m_LoadKeyIn == 4)//花篮进料
                {
                    if (m_AlamLoad3Continue < 150)
                        m_AlamLoad3Continue++;
                    else
                    {
                        m_AlamLoad3Continue = 0;
                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 1, 0);//进花篮电机2运动停止
                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 1, 0);//进花篮电机2反转停止
                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 0, 0);//进花篮电机1停止
                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 0, 0);////进花篮电机1反转停止
                        m_LoadIn1run = 0;
                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 2, 0);//托盘电机正转停止
                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
                        m_LoadCCWrun = 0;
                        m_LoadAlarm42 = true;
                    }
                }
                else if (m_LoadKeyIn == 15)//花篮出料
                {
                    if (m_AlamLoad3Continue < 150)
                        m_AlamLoad3Continue++;
                    else
                    {
                        m_AlamLoad3Continue = 0;
                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 4, 0);//出花篮电机1运动停止
                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 2, 0);////出花篮电机1反转停止
                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 5, 0);//出花篮电机2正转停止
                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 3, 0);////出花篮电机2反转停止
                        m_LoadOut1run = 0;
                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 2, 0);//托盘电机正转停止
                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
                        m_LoadCCWrun = 0;
                        m_LoadAlarm42 = true;
                    }
                }
            }
            else
                m_AlamLoad3Continue = 0;

        }

        /*********************************************************************************************************
            ** 函数名称 ：CheckBox_NoCheck_CheckedChanged
            ** 函数功能 ：上料接驳台空片检测
            ** 修改时间 ：20180811
            ** 修改内容 ：
            *********************************************************************************************************/
        private void CheckBox_NoCheck_CheckedChanged(object sender, EventArgs e)
        {
            m_NoCheck = CheckBox_NoCheck.Checked;
            if (m_NoCheck)//|| (m_LoadZIN1 == 1))
                Text_NoCheck.Visible = true;
            else
                Text_NoCheck.Visible = false;

        }
        /*********************************************************************************************************
            ** 函数名称 ：CheckBox_LoadOff_CheckedChanged
            ** 函数功能 ：上料OnOFF
            ** 修改时间 ：20180811
            ** 修改内容 ：
            *********************************************************************************************************/
        private void CheckBox_LoadOff_CheckedChanged(object sender, EventArgs e)
        {
            m_LoadOff = CheckBox_LoadOff.Checked;
        }

        private void KEY_OPTSwitch_StateChanged(object sender, ActionEventArgs e)
        {

        }
        /*********************************************************************************************************
         ** 函数名称 ：Modify_CheckedChanged
         ** 函数功能 ：切换检修状态
         ** 入口参数 ：
         ** 出口参数 ： 
         *********************************************************************************************************/

        private void Modify_MouseClick(object sender, MouseEventArgs e)
        {

            ushort GetState1 = 20, GetState2 = 20;
            if (g_InRotateHome == false)
            {
                MessageBox.Show(this, "上料旋转臂没有复位，不能执行检修功能！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Modify.Checked = false;
            }
            else
            {
                DialogResult ResultTemp;
                ResultTemp = MessageBox.Show(this, "为方便检修，上料旋转臂及下料旋转臂将旋转90度，请注意安全！", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (ResultTemp == DialogResult.OK)
                {
                    g_Modify = Modify.Checked;
                    if (g_Modify == true)
                    {
                        fEMGStop(); AutoRun.Visible = false;
                        Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_TWO], ref GetState1);
                        Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_THREE], ref GetState2);
                        if (GetState1 == 1 && GetState2 == 1)
                        {
                            Motion.mAcm_AxMoveRel(ax2Handle[pci1245l.Axis_THREE], 90 * FormHMI.pci1245l.RotateRatio);//下料转90度
                            Motion.mAcm_AxMoveAbs(ax2Handle[pci1245l.Axis_TWO], 90 * FormHMI.pci1245l.RotateRatio);

                            Modify.Enabled = false;
                        }
                        System.Threading.Thread.Sleep(400);//延时400ms
                        Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_TWO], ref GetState1);
                        Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_THREE], ref GetState2);
                        if (GetState1 == 1 && GetState2 == 1)
                        {
                            LogClass.Text = "当前为检修状态";
                            AutoRun.Visible = false;
                            Modify.Enabled = true;
                            if (m_LoadKeyIn == 16)   //停止后，重新开始
                                m_LoadKeyIn = m_TempLoadKeyIn;
                        }
                        else
                        {
                            m_ErrorFlag = 1; fALAM("警告", AlarmMess[58]);
                        }
                    }
                    else
                    {
                        Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_TWO], ref GetState1);
                        Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_THREE], ref GetState2);
                        if (GetState1 == 1 && GetState2 == 1)
                        {
                            Motion.mAcm_AxMoveRel(ax2Handle[pci1245l.Axis_THREE], -90 * FormHMI.pci1245l.RotateRatio);//下料转-90度
                            if (FormHMI.g_InRotateMotorPosition == 0)    //0度位置
                            {
                                Motion.mAcm_AxMoveAbs(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_TWO], 0);
                            }
                            if (FormHMI.g_InRotateMotorPosition == 1)
                            {
                                Motion.mAcm_AxMoveAbs(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_TWO], 180 * FormHMI.pci1245l.RotateRatio);
                            }

                            Modify.Enabled = false;
                        }
                        System.Threading.Thread.Sleep(400);//延时400ms
                        Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_TWO], ref GetState1);
                        Motion.mAcm_AxGetState(ax2Handle[pci1245l.Axis_THREE], ref GetState2);
                        if (GetState1 == 1 && GetState2 == 1)
                        {
                            LogClass.Text = "取消检修状态";
                            Modify.Enabled = true;
                            AutoRun.Visible = true;
                        }
                        else
                        {
                            m_ErrorFlag = 1; fALAM("警告", AlarmMess[58]);
                        }
                        if (g_Clean)
                        {
                            //停止皮带清理
                            //上料接驳台
                            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 2, 0);//托盘电机正转停止        
                            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
                            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 0, 0);////进花篮电机1运动停止
                            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 1, 0);//进花篮电机2运动停止
                            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 4, 0);////出花篮电机1运动停止                  
                            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 5, 0);////出花篮电机2运动
                            MNet.M1A._mnet_m1a_sd_stop(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX1IP);//舌头运动舌头运动减速停止
                            //主机
                            Motion.mAcm_AxStopDec(ax1Handle[FormHMI.pci1245l.Axis_THREE]);
                            Motion.mAcm_AxStopDec(ax1Handle[FormHMI.pci1245l.Axis_TWO]);
                            Motion.mAcm_AxStopDec(ax1Handle[FormHMI.pci1245l.Axis_ONE]);
                            g_Clean = false;
                            Clean.Checked = g_Clean;
                        }
                    }
                }
                else
                {
                    Modify.Checked = g_Modify;
                }
            }

        }
        /*********************************************************************************************************
         ** 函数名称 ：fAutoAlign
         ** 函数功能 ：自动打齐
         ** 入口参数 ：
         ** 出口参数 ： 
         *********************************************************************************************************/
        void fAutoAlign()
        {
            //MNet.M1A._mnet_m1a_start_r_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX1IP, (int)(FormHMI.pci1245l.WaferRatio * 300));//舌头运动

            ushort GetState1 = 0;
            MNet.M1A._mnet_m1a_motion_done(m_RingNoA, ClineAddr.LoadAX1IP, ref GetState1);//硅片传送模组运动完成
            #region[上料打齐]
            if (g_MotorAlign == 1)//选择打齐电机
            {
                if ((m_flagCheck1 == true) && (m_LoadAlignCount == 0) && (GetState1 == 0) && (m_Load_DPos == 1))
                {
                    if (m_flagStatus1 == true)
                    {
                        MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 6, 1); //进料打齐

                        m_flagStatus1 = false;
                    }

                    m_LoadAlignCount = 1;
                }

                if ((m_LoadAlignCount > 0) && (m_LoadAlignCount < 8))
                    m_LoadAlignCount++;
                else if (m_LoadAlignCount >= 8)
                {
                    MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 6, 0); //进料打齐
                    m_flagStatus1 = true;
                    m_flagCheck1 = false; m_LoadAlignCount = 0;
                }
            }
            else
            {
                // if ((m_flagCheck1 == true) && (m_LoadAlignCount == 0) && (GetState1 == 0) && (m_Load_DPos == 1))
                if ((m_flagCheck1 == true) && (m_LoadAlignCount == 0) && (GetState1 == 0))
                {

                    if (m_flagStatus1 == true)
                    {
                        MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 2, 7, 1);
                        m_flagStatus1 = false;
                    }
                    else
                    {
                        MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 2, 7, 0);
                        m_flagStatus1 = true;
                    }
                    m_LoadAlignCount = 1;
                }
                if ((m_LoadAlignCount > 0) && (m_LoadAlignCount < 4))
                    m_LoadAlignCount++;
                else if (m_LoadAlignCount == 4)
                {
                    m_flagCheck1 = false; m_LoadAlignCount = 0;
                }
            }
            #endregion

        }
        /*********************************************************************************************************
        ** 函数名称 ：SignalNeed_CheckedChanged
        ** 函数功能 ：信号模拟
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        private void SignalNeed_CheckedChanged(object sender, EventArgs e)
        {
            m_SignalNeed = SignalNeed.Checked;
        }

        /*********************************************************************************************************
       ** 函数名称 ：fBrokenWaferFile
       ** 函数功能 ：记录破片
       ** 入口参数 ：
       ** 出口参数 ： 
       *********************************************************************************************************/
        void fBrokenWaferFile()
        {
            string SystemTime, SystemTime1;
            string message = "";
            Broken.Value = m_Broken;
            TooSkew.Value = m_TooSkew;
            SystemTime = DateTime.Now.ToLongDateString();
            SystemTime1 = DateTime.Now.ToString("HH:mm:ss");
            ///////错误记录文件   
            int TimeNow;
            string TimeDate;
            TimeNow = DateTime.Now.Hour * 100 + DateTime.Now.Minute;
            if ((TimeNow > 830) && (TimeNow < 2031))
            {
                SystemTime = "d:\\DRLaser\\Broken\\" + SystemTime + "_白班.txt";
            }
            else if ((TimeNow <= 830) && (TimeNow >= 0))
            {
                TimeDate = DateTime.Now.AddDays(-1).ToLongDateString();
                SystemTime = "d:\\DRLaser\\Broken\\" + TimeDate + "_晚班.txt";
            }
            else if ((TimeNow >= 2031) && (TimeNow <= 2359))
                SystemTime = "d:\\DRLaser\\Broken\\" + SystemTime + "_晚班.txt";
            //SystemTime = "d:\\DRLaser\\Broken\\" + SystemTime + ".txt";

            string Broken_path = "d:\\DRLaser\\Broken\\";
            if (false == System.IO.Directory.Exists(Broken_path))
            {
                System.IO.Directory.CreateDirectory(Broken_path);
            }

            if (File.Exists(SystemTime))
                ;
            else
            {
                //新建文件
                using (FileStream BrokenWaferFile = new FileStream(SystemTime, FileMode.Create))
                {

                }
            }
            using (FileStream BrokenFile = new FileStream(SystemTime, FileMode.Append))//最后一行追加
            {
                using (StreamWriter write = new StreamWriter(BrokenFile))
                {
                    if ((m_CCDAlarm == 3) || (m_CCDAlarm == 1) || (m_CCDAlarm == 2))
                    {
                        message = message + "破片：" + m_Broken.ToString() + ";放置太偏:" + m_TooSkew.ToString();

                    }
                    write.WriteLine(SystemTime1 + "  " + message);
                    write.Close();
                }
                BrokenFile.Close();
            }
        }

        /*********************************************************************************************************
        ** 函数名称 ：BrokenClear_Click
        ** 函数功能 ：破片清零
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        private void BrokenClear_Click(object sender, EventArgs e)
        {
            int i;
            string sysp;

            string SystemTime, SystemTime1;
            string message = "";
            SystemTime = DateTime.Now.ToLongDateString();
            SystemTime1 = DateTime.Now.ToString("HH:mm:ss");

            m_Broken = 0; m_TooSkew = 0;
            Broken.Value = m_Broken;
            TooSkew.Value = m_TooSkew;
            ///////错误破片  

            int TimeNow;
            string TimeDate;
            TimeNow = DateTime.Now.Hour * 100 + DateTime.Now.Minute;
            if ((TimeNow > 830) && (TimeNow < 2031))
            {
                SystemTime = "d:\\DRLaser\\Broken\\" + SystemTime + "_白班.txt";
            }
            else if ((TimeNow <= 830) && (TimeNow >= 0))
            {
                TimeDate = DateTime.Now.AddDays(-1).ToLongDateString();
                SystemTime = "d:\\DRLaser\\Broken\\" + TimeDate + "_晚班.txt";
            }
            else if ((TimeNow >= 2031) && (TimeNow <= 2359))
                SystemTime = "d:\\DRLaser\\Broken\\" + SystemTime + "_晚班.txt";

            // SystemTime = "d:\\DRLaser\\Broken\\" + SystemTime + ".txt";
            string Broken_path = "d:\\DRLaser\\Broken\\";
            if (false == System.IO.Directory.Exists(Broken_path))
            {
                System.IO.Directory.CreateDirectory(Broken_path);
            }
            if (File.Exists(SystemTime))
                ;
            else
            {
                //新建文件
                using (FileStream BrokenWaferFile = new FileStream(SystemTime, FileMode.Create))
                {

                }
            }
            using (FileStream BrokenFile = new FileStream(SystemTime, FileMode.Append))//最后一行追加
            {
                using (StreamWriter writebroken = new StreamWriter(BrokenFile))
                {
                    message = message + "破片清零：" + m_Broken.ToString() + ";放置太偏清零:" + m_TooSkew.ToString();
                    writebroken.WriteLine(SystemTime1 + "  " + message);
                    writebroken.Close();
                }
                BrokenFile.Close();
            }


            g_SysParam[57] = m_Broken;
            g_SysParam[58] = m_TooSkew;
            StreamWriter writeSys = new StreamWriter("D:\\DRLaser\\SysPara.ini");
            for (i = 0; i < 80; i++)
            {
                sysp = Convert.ToString(FormHMI.g_SysParam[i]);
                writeSys.WriteLine(sysp);
            }
            writeSys.Close();
        }

        /*********************************************************************************************************
         ** 函数名称 ：MenuMainCorrect_Click
         ** 函数功能 ：振镜校正
         ** 修改时间 ：20190115
         ** 修改内容 ：
         *********************************************************************************************************/
        private void MenuMainCorrect_Click(object sender, EventArgs e)
        {
            Correct MainCorrect = new Correct();
            MainCorrect.StartPosition = FormStartPosition.CenterScreen;
            MainCorrect.ChangeData += new ChangeFMFormData3(f_ChangeFMCorrect);
            MainCorrect.ChangeData1 += new ChangeFMFormData4(f_ChangeFMClear);
            MainCorrect.ChangeData2 += new ChangeFMFormData5(f_ChangeFMSet);
            MainCorrect.ShowDialog();
        }
        void f_ChangeFMCorrect(bool topGetState4)//四点校正
        {
            //axMMEdit1.ThermalDrift_Enable(1, 1);//
            axMMEdit1.ThermalDrift_SetStretchData(1,
                g_TheoryPix[0], g_TheoryPix[1], g_TheoryPix[2], g_TheoryPix[3], g_TheoryPix[4], g_TheoryPix[5], g_TheoryPix[6], g_TheoryPix[7],
            g_RealPix[0], g_RealPix[1], g_RealPix[2], g_RealPix[3], g_RealPix[4], g_RealPix[5], g_RealPix[6], g_RealPix[7]);
        }
        void f_ChangeFMClear(bool topGetState4)//清除
        {
            axMMEdit1.ThermalDrift_Clear(1);
        }
        void f_ChangeFMSet(bool topGetState4)//参数设定
        {
            fSysParamAssignment();
        }

        /*********************************************************************************************************
         ** 函数名称 ：ReturnWafer_CheckedChanged
         ** 函数功能 ：皮带清理
         ** 修改时间 ：20181228
         ** 修改内容 ：
         *********************************************************************************************************/
        private void Clean_MouseClick(object sender, MouseEventArgs e)
        {
            if (Clean.Checked)
            {
                DialogResult ResultTemp;
                ResultTemp = MessageBox.Show(this, "即将进行皮带清理，请务必确保设备中已无硅片及花篮！", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (ResultTemp == DialogResult.OK)
                {
                    g_Clean = Clean.Checked;
                    m_LoadKeyIn = 0; m_LoadHomeFlag = 0;
                    MNet.M1A._mnet_m1a_v_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX1IP, 1);//舌头运动

                    //主机
                    m_HomeFinished = 0; g_MainStep = 30;
                    Motion.mAcm_AxMoveRel(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_ONE], 1000000 * FormHMI.pci1245l.WaferRatio);
                    Motion.mAcm_AxMoveRel(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_TWO], 1000000 * FormHMI.pci1245l.WaferRatio);
                    Motion.mAcm_AxMoveRel(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], 1000000 * FormHMI.pci1245l.WaferRatio);
                }
                else
                {
                    Clean.Checked = false;
                    g_Clean = Clean.Checked;
                }
            }
            else
            {
                Clean.Checked = false;
                g_Clean = Clean.Checked;
                //上料接驳台
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 2, 0);//托盘电机正转停止        
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 1, 0);//进花篮电机运动停止
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 4, 0);////出花篮电机运动停止
                MNet.M1A._mnet_m1a_sd_stop(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX1IP);//舌头运动舌头运动减速停止
                //主机
                Motion.mAcm_AxStopDec(ax1Handle[FormHMI.pci1245l.Axis_THREE]);
                Motion.mAcm_AxStopDec(ax1Handle[FormHMI.pci1245l.Axis_TWO]);
                Motion.mAcm_AxStopDec(ax1Handle[FormHMI.pci1245l.Axis_ONE]);
            }
        }

        //private void axMbaxp1_ResultOk(object sender, AxMBAXPLib._DMbaxpEvents_ResultOkEvent e)
        //{
        //    if (e.handle == 1)
        //    {
        //        int i;
        //        m_WcsHeart = axMbaxp1.get_Register(1, 0);
        //        m_AgvUpStateTotal = axMbaxp1.get_Register(1, 1);
        //        m_AgvDownStateTotal = axMbaxp1.get_Register(1, 2);
        //        for (i = 0; i < 17; i++)
        //        {
        //            if (((m_AgvUpStateTotal >> i) & 0x01) == 1)
        //                m_AgvUpState[i] = true;
        //            else
        //                m_AgvUpState[i] = false;
        //        }

        //        for (i = 0; i < 17; i++)
        //        {
        //            if (((m_AgvDownStateTotal >> i) & 0x01) == 1)
        //                m_AgvDownState[i] = true;
        //            else
        //                m_AgvDownState[i] = false;
        //        }

        //    }
        //}

        private void aGVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAgv LoadAgv = new FormAgv();
            LoadAgv.StartPosition = FormStartPosition.CenterScreen;
            LoadAgv.ShowDialog();
        }

        private void timerAGV_Tick(object sender, EventArgs e)
        {
            //机台心跳信号，脉宽1秒方波
            if (m_MesCounter < 60)
                m_MesCounter++;
            else
            {
                m_MesCounter = 0;

                //写入心跳信号
                dataSocketSource1.Bindings[2].Data.Value = g_EQPHeartBeatValue;

                if (g_EQPHeartBeatValue == 255)
                    g_EQPHeartBeatValue = 1;
                else
                    g_EQPHeartBeatValue++;

                if (!LED_IN66.Value || g_NoCheckAgvSensor)
                {
                    dataSocketSource1.Bindings[4].Data.Value = m_OpcData_Load;
                    dataSocketSource1.Bindings[6].Data.Value = m_OpcData_UnLoad;
                }
                else
                {
                    dataSocketSource1.Bindings[4].Data.Value = m_OpcData_Load + 512;
                    dataSocketSource1.Bindings[6].Data.Value = m_OpcData_UnLoad + 512;
                }

            }
            //不对接勾选
            if (g_AgvModel && (g_LoadAGVCommStep < 2) && (g_UnLoadAGVCommStep < 2))
            {
                g_LoadAGVCommStep = 0;
                g_UnLoadAGVCommStep = 0;
                if (m_MesCounter == 0)
                {
                    dataSocketSource1.Bindings[4].Data.Value = 0;
                    dataSocketSource1.Bindings[6].Data.Value = 0;
                }
                return;
            }

            //上轨道强制对接完成
            if (g_ForceUpFinish)
            {
                g_ForceUpFinish = false;
                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 6, 0);//出料伸出
                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 3, 0);////出花篮电机2反转停止
                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 5, 0);////出花篮电机2运动停止
                m_LoadOut1run = 0;
                dataSocketSource1.Bindings[6].Data.Value = 24;
                m_OpcData_UnLoad = 24;
                g_UnLoadAGVCommStep = 10;

            }

            if (m_AgvUpState[6] && (g_UnLoadAGVCommStep < 5) && (g_UnLoadAGVCommStep > 1))
                g_UnLoadAGVCommStep = 5;

            if (!m_AgvUpState[0] && (g_UnLoadAGVCommStep < 5) && (g_UnLoadAGVCommStep > 1))
            {
                g_AgvAlarm = 1;
                g_UnLoadAGVCommStep = 10;
            }
            if (!LED_IN66.Value && !g_NoCheckAgvSensor && (g_UnLoadAGVCommStep < 5) && (g_UnLoadAGVCommStep > 1))
            {
                g_AgvAlarm = 1;
                g_UnLoadAGVCommStep = 10;
            }
            //下轨道强制对接完成
            if (g_ForceDownFinish)
            {
                g_ForceDownFinish = false;
                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 0, 0);////进花篮电机1反转停止
                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 0, 0);//进花篮电机1停止    
                MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 0, 0); //进料阻挡汽缸1缩回
                m_LoadIn1run = 0;
                m_OpcData_Load = 24;//  dataSocketSource1.Bindings[4].Data.Value = 24;
                g_LoadAGVCommStep = 10;
            }
            if (m_AgvDownState[3] && (g_LoadAGVCommStep < 4) && (g_LoadAGVCommStep > 1))
                g_LoadAGVCommStep = 4;
            if (!m_AgvDownState[0] && (g_LoadAGVCommStep < 4) && (g_LoadAGVCommStep > 1))
            {
                g_AgvAlarm = 4;
                g_LoadAGVCommStep = 10;
            }
            if (!LED_IN66.Value && !g_NoCheckAgvSensor && (g_LoadAGVCommStep < 4) && (g_LoadAGVCommStep > 1))
            {
                g_AgvAlarm = 4;
                g_LoadAGVCommStep = 10;
            }


            if ((g_LoadAGVCommStep == 20) || (g_LoadAGVCommStep == 10))
            {
                //延时10秒允许下次对接，防止上次对接完成后，AGV小车信号未及时清空
                if (m_LoadRestDelayCounter < 625)
                    m_LoadRestDelayCounter++;
                else
                {
                    m_LoadRestDelayCounter = 0;
                    if (g_LoadAGVCommStep == 20)
                        g_LoadAGVCommStep = 0;
                    else
                        g_LoadAGVCommStep = 6;
                }


            }

            if ((g_UnLoadAGVCommStep == 20) || (g_UnLoadAGVCommStep == 10))
            {
                //延时10秒允许下次对接，防止上次对接完成后，AGV小车信号未及时清空
                if (m_UnLoadRestDelayCounter < 625)
                    m_UnLoadRestDelayCounter++;
                else
                {
                    m_UnLoadRestDelayCounter = 0;
                    if (g_UnLoadAGVCommStep == 20)
                        g_UnLoadAGVCommStep = 0;
                    else
                        g_UnLoadAGVCommStep = 6;

                }
            }

            // 写入花篮数量
            if (m_CassetteNumCounter < 6)
                m_CassetteNumCounter++;
            else
            {
                m_CassetteNumCounter = 0;

                g_LoadFullNum = (short)(m_LoadIN06 + m_LoadIN02 + m_LoadIN03 + m_LoadIN04 + m_LoadIN05 + m_LoadIN01);
                g_UnloadEmptyNum = (short)(m_LoadIN31 + m_LoadIN32 + m_LoadIN33 + m_LoadIN34 + m_LoadIN35 + m_LoadIN30);


                dataSocketSource1.Bindings[3].Data.Value = g_LoadFullNum;
                //写入上轨道空花篮数量
                //axMbaxp1.PresetSingleRegister(12, 1, 6, 100);
                //axMbaxp1.set_Register(12, 0, (short)g_UnloadEmptyNum);
                //axMbaxp1.UpdateOnce(12);
                dataSocketSource1.Bindings[5].Data.Value = g_UnloadEmptyNum;

            }






            switch (g_LoadAGVCommStep)
            {
                case 0:

                    //if (m_AgvDownState[0]) //AGV准备好
                    //{

                    //机台准备好
                    if (m_SysResetFlag == true)
                    {

                        m_OpcData_Load = 3;// dataSocketSource1.Bindings[4].Data.Value = 3;
                        g_LoadAGVCommStep = 1;


                    }

                    // }



                    break;

                case 1:
                    if (m_AgvDownState[1] && m_AgvDownState[0] && (LED_IN66.Value || g_NoCheckAgvSensor)) //AGV请求卸货
                    {
                        if ((m_SysIniReset == 0) && (g_LoadFullNum == 0) && (m_LoadIn1run == 0) && (m_LoadIN07 == 1))
                        {



                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 0, 1);//进料阻挡汽缸1打开
                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 0, 0);////进花篮电机1反转停止
                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 0, 1);//进花篮电机1运动
                            m_LoadIn1run = 1;

                            m_OpcData_Load = 7;// dataSocketSource1.Bindings[4].Data.Value = 7;
                            g_LoadAGVCommStep = 2;



                        }

                    }
                    else
                    {
                        if (m_MesCounter == 0)
                        {

                            m_OpcData_Load = 3;//   dataSocketSource1.Bindings[4].Data.Value = 3;

                        }
                    }
                    break;
                case 2:
                    if ((g_LoadFullNum > 0) && (g_LoadFullNum >= g_PlcInNum))
                    {
                        if (m_StopDelayCounter < 180)
                            m_StopDelayCounter++;
                        else
                        {
                            m_StopDelayCounter = 0;

                            m_OpcData_Load = 39;// dataSocketSource1.Bindings[4].Data.Value = 39;
                            g_LoadAGVCommStep = 3;
                        }

                    }
                    break;
                case 3:
                    if (m_AgvDownState[3]) //AGV卸货完成
                    {
                        g_LoadAGVCommStep = 4;

                    }
                    break;
                case 4:

                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 0, 0);////进花篮电机1反转停止
                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 0, 0);//进花篮电机1停止                  
                    m_LoadIn1run = 0;
                    g_LoadAGVCommStep = 5;


                    break;
                case 5:
                    if (m_StopCounter < 60)
                        m_StopCounter++;
                    else
                    {
                        m_StopCounter = 0;
                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 0, 0); //进料阻挡汽缸1缩回

                        g_LoadAGVCommStep = 6;
                    }

                    break;

                case 6:
                    if (m_LoadIN11 == 1) //进料阻挡汽缸1缩回到位
                    {
                        m_OpcData_Load = 1;//    dataSocketSource1.Bindings[4].Data.Value = 1;

                        g_LoadAGVCommStep = 20;


                        if ((m_LoadIN51 == 1) && (m_LoadIN52 == 1) && (m_LoadIN53 == 0) && (m_LoadIN54 == 1) && (m_LoadIN55 == 0))
                        {
                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 6, 1);   //指示灯点亮
                            MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 2, 0);//阻挡汽缸3伸出,避免花篮重大事故  
                            m_LoadKeyIn = 1;
                        }


                    }


                    break;


                case 20:


                    break;
                case 10:


                    break;



            }

            switch (g_UnLoadAGVCommStep)
            {
                case 0:

                    //   if (m_AgvUpState[0]) //AGV准备好
                    //   {

                    //机台准备好
                    if (m_SysResetFlag == true)
                    {


                        m_OpcData_UnLoad = 3;// dataSocketSource1.Bindings[6].Data.Value = 3;
                        g_UnLoadAGVCommStep = 1;



                    }

                    //   }



                    break;

                case 1:
                    if (m_AgvUpState[4] && m_AgvUpState[0] && (LED_IN66.Value || g_NoCheckAgvSensor)) //AGV请求上货
                    {
                        if ((m_SysIniReset == 0) && (m_LoadOut1run == 0) && (m_LoadIN27 == 0))
                        {


                            if ((g_UnloadEmptyNum == g_PlcOutNum) || (g_UnloadEmptyNum == 6))
                            {
                                m_OpcData_UnLoad = 67;//dataSocketSource1.Bindings[6].Data.Value = 67;
                                g_UnLoadAGVCommStep = 2;
                            }
                            else
                            {
                                if (g_PlcOutNum < 6)
                                {
                                    g_UnLoadAGVCommStep = 10;
                                    timerAGV.Enabled = false;
                                    dataSocketSource1.Bindings[6].Data.Value = 8;
                                    fALAM("警告", AlarmMess[79]);
                                    m_OpcData_UnLoad = 16;//dataSocketSource1.Bindings[6].Data.Value = 16;
                                    timerAGV.Enabled = true;
                                }

                            }


                        }

                    }
                    else
                    {
                        if (m_MesCounter == 0)
                        {

                            m_OpcData_UnLoad = 3;//  dataSocketSource1.Bindings[6].Data.Value = 3;

                        }
                    }
                    break;
                case 2:
                    if (m_AgvUpState[5] && (LED_IN66.Value || g_NoCheckAgvSensor)) //AGV上货准备好
                    {



                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 6, 1);//出料缩回
                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 3, 0);////出花篮电机2反转停止
                        MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 5, 1);////出花篮电机2运动
                        m_LoadOut1run = 1;
                        m_OpcData_UnLoad = 195;// dataSocketSource1.Bindings[6].Data.Value = 195;
                        g_UnLoadAGVCommStep = 3;




                    }
                    break;
                case 3:
                    if (g_UnloadEmptyNum == 0)
                    {
                        m_OpcData_UnLoad = 451;// dataSocketSource1.Bindings[6].Data.Value = 451;
                        g_UnLoadAGVCommStep = 4;

                    }
                    break;
                case 4:
                    if (m_AgvUpState[6])
                    {
                        g_UnLoadAGVCommStep = 5;
                    }
                    break;
                case 5:
                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 2, 6, 0);//出料伸出
                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122_2IP, 2, 3, 0);////出花篮电机2反转停止
                    MNet.Basic._mnet_bit_io_output(m_RingNoA, ClineAddr.LoadD122IP, 3, 5, 0);////出花篮电机2运动停止
                    m_LoadOut1run = 0;
                    g_UnLoadAGVCommStep = 6;
                    break;
                case 6:
                    if (m_LoadIN36 == 1) //出阻挡汽缸1伸出到位
                    {
                        m_OpcData_UnLoad = 1;//  dataSocketSource1.Bindings[6].Data.Value = 1;
                        g_UnLoadAGVCommStep = 20;


                    }
                    break;

                case 20:

                    break;
                case 10:


                    break;

            }

            //AGV报警
            //AGV报警

            if ((m_WcsHeartTemp == m_WcsHeart) && (!g_NotAgvAlarm) && !g_NoCheckAgvSensor && (g_UnLoadAGVCommStep > 1) && (g_UnLoadAGVCommStep < 7))
            {

                if (m_WcsHeartCount < 300)
                    m_WcsHeartCount++;
                else
                {
                    g_AgvAlarm = 1;
                    m_WcsHeartCount = 0;
                }
            }
            else
            {

                m_WcsHeartTemp = m_WcsHeart;
                m_WcsHeartCount = 0;
            }


            if ((m_WcsHeartTemp2 == m_WcsHeart) && (!g_NotAgvAlarm) && !g_NoCheckAgvSensor && (g_LoadAGVCommStep > 1) && (g_LoadAGVCommStep < 7))
            {

                if (m_WcsHeartCount2 < 300)
                    m_WcsHeartCount2++;
                else
                {
                    g_AgvAlarm = 4;
                    m_WcsHeartCount2 = 0;
                }
            }
            else
            {

                m_WcsHeartTemp2 = m_WcsHeart;
                m_WcsHeartCount2 = 0;
            }



            if ((g_UnLoadAGVCommStep > 1) && (g_UnLoadAGVCommStep < 7))
            {
                if (m_AgvAlarmCount < 1500)
                    m_AgvAlarmCount++;
                else
                {

                    g_AgvAlarm = 2;
                    m_AgvAlarmCount = 0;
                }
            }
            else
                m_AgvAlarmCount = 0;


            if ((g_LoadAGVCommStep > 1) && (g_LoadAGVCommStep < 7))
            {
                if (m_AgvAlarmCount1 < 1500)
                    m_AgvAlarmCount1++;
                else
                {

                    g_AgvAlarm = 3;
                    m_AgvAlarmCount1 = 0;
                }
            }
            else
                m_AgvAlarmCount1 = 0;





        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[,] bArray = new byte[4, 255];

            RFIDInit = new ReaderInit(ReaderInit.RFNum_enum.RF0, 0, 60);

            if (RFIDReader.ReadTagByte(ref RFIDInit) == Modbus.Status_enum.succeed)
            {
                bArray = RFIDInit.TagData;

                byte[] BArray = new byte[60];

                for (int i = 0; i < 60; i++)
                    BArray[i] = bArray[0, i];


                button1.Text = Encoding.ASCII.GetString(BArray);


            }
            else
            {

                button1.Text = "读取失败！";
            }
        }

        /*********************************************************************************************************
        ** 函数名称 ：KEY_NoWaferClean_Click
        ** 函数功能 ：激光位无硅片清零
        ** 入口参数 ：
        ** 出口参数 ： 
        *********************************************************************************************************/
        private void KEY_NoWaferClean_Click(object sender, EventArgs e)
        {
            m_LaserNoWafer = 0;
            LaserNoWafer.Value = m_LaserNoWafer;
        }

        private void dataSocketSource1_BindingDataUpdated(object sender, NationalInstruments.Net.BindingDataUpdatedEventArgs e)
        {
            if (e.Binding.Name == dataSocketSource1.Bindings[1].Name)
            {
                int i;
                //  m_WcsHeart = axMbaxp1.get_Register(1, 0);
                m_AgvUpStateTotal = ushort.Parse(dataSocketSource1.Bindings[1].Data.Value.ToString());

                for (i = 0; i < 17; i++)
                {
                    if (((m_AgvUpStateTotal >> i) & 0x01) == 1)
                        m_AgvUpState[i] = true;
                    else
                        m_AgvUpState[i] = false;
                }

            }

            if (e.Binding.Name == dataSocketSource1.Bindings[0].Name)
            {

                int i;
                //  m_WcsHeart = axMbaxp1.get_Register(1, 0);
                m_AgvDownStateTotal = ushort.Parse(dataSocketSource1.Bindings[0].Data.Value.ToString());
                for (i = 0; i < 17; i++)
                {
                    if (((m_AgvDownStateTotal >> i) & 0x01) == 1)
                        m_AgvDownState[i] = true;
                    else
                        m_AgvDownState[i] = false;
                }

            }

        }

        private void dataSocketSource1_BindingDataUpdated_1(object sender, NationalInstruments.Net.BindingDataUpdatedEventArgs e)
        {
            if (e.Binding.Name == dataSocketSource1.Bindings[13].Name)
            {


                m_WcsHeart = short.Parse(dataSocketSource1.Bindings[13].Data.Value.ToString());

            }
            if (e.Binding.Name == dataSocketSource1.Bindings[26].Name)
            {


                g_PlcInNum = short.Parse(dataSocketSource1.Bindings[26].Data.Value.ToString());

            }
            if (e.Binding.Name == dataSocketSource1.Bindings[27].Name)
            {


                g_PlcOutNum = short.Parse(dataSocketSource1.Bindings[27].Data.Value.ToString());

            }

            if (e.Binding.Name == dataSocketSource1.Bindings[1].Name)
            {
                int i;
                //  m_WcsHeart = axMbaxp1.get_Register(1, 0);
                m_AgvUpStateTotal = ushort.Parse(dataSocketSource1.Bindings[1].Data.Value.ToString());

                for (i = 0; i < 17; i++)
                {
                    if (((m_AgvUpStateTotal >> i) & 0x01) == 1)
                        m_AgvUpState[i] = true;
                    else
                        m_AgvUpState[i] = false;
                }

            }

            if (e.Binding.Name == dataSocketSource1.Bindings[0].Name)
            {

                int i;
                //  m_WcsHeart = axMbaxp1.get_Register(1, 0);
                m_AgvDownStateTotal = ushort.Parse(dataSocketSource1.Bindings[0].Data.Value.ToString());
                for (i = 0; i < 17; i++)
                {
                    if (((m_AgvDownStateTotal >> i) & 0x01) == 1)
                        m_AgvDownState[i] = true;
                    else
                        m_AgvDownState[i] = false;
                }

            }

        }

    }

}