using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Advantech.Motion;
using System.IO;
using TPM;
///////////////////////////////////////////////////////////////////////////////////////////////////

namespace WindowsApplication1
{
    public partial class FormLoad : Form
    {
        public FormLoad()
        {
            InitializeComponent();
            IN51.Checked = Convert.ToBoolean(FormHMI.g_IN51);
            IN52.Checked = Convert.ToBoolean(FormHMI.g_IN52);
            IN53.Checked = Convert.ToBoolean(FormHMI.g_IN53);
            IN54.Checked = Convert.ToBoolean(FormHMI.g_IN54);
            IN55.Checked = Convert.ToBoolean(FormHMI.g_IN55);

        }
        /*********************************************************************************************************
       ** 函数名称 ：timer1_Tick
       ** 函数功能 ：主定时器  
       ** 修改时间 ：20170915
       ** 修改内容 ：
       *********************************************************************************************************/
        private void timer1_Tick(object sender, EventArgs e)
        {

            uint Status = 0;
            //int Loadm_VelLow = (int)FormHMI.pci1245l.ElevatorRatio * 50, Loadm_VelHigh = (int)FormHMI.pci1245l.ElevatorRatio * 300;


            ///获取升降模组位置
            if ((FormHMI.g_LoadElevationP / FormHMI.pci1245l.ElevatorRatio < -20) || (FormHMI.g_LoadElevationP / FormHMI.pci1245l.ElevatorRatio > 1000))
                LoadElevatorP.Value = 0;
            else
                LoadElevatorP.Value = FormHMI.g_LoadElevationP / FormHMI.pci1245l.ElevatorRatio;

            if (FormHMI.g_LoadHome == true)
                groupBox5.Visible = true;

            MNet.M1A._mnet_m1a_get_io_status(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, ref Status);

            LED_PLimit.Value = Convert.ToBoolean((Status >> 2) & 0x01);
            LED_NLimit.Value = Convert.ToBoolean((Status >> 3) & 0x01);
            LED_Home.Value = Convert.ToBoolean((Status >> 4) & 0x01);//升降模组原点
            LED_XPos.Value = Convert.ToBoolean((Status >> 13) & 0x01);//升降模组到位信号
            LED_ALM.Value = Convert.ToBoolean((Status >> 1) & 0x01);//升降模组报警

            groupBox11.Visible = Convert.ToBoolean(FormHMI.g_MotorAlign); //打齐电机显示
            KEY_UpDownOnOff.Visible = !(Convert.ToBoolean(FormHMI.g_MotorAlign));//打齐气缸显示
            label65.Visible = !(Convert.ToBoolean(FormHMI.g_MotorAlign));//打齐气缸显示

            MNet.M1A._mnet_m1a_get_io_status(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX1IP, ref Status);
            LED_DPos.Value = Convert.ToBoolean((Status >> 13) & 0x01);//舌头到位信号
            LED_YHome.Value = Convert.ToBoolean((Status >> 4) & 0x1);//压力检测
            LED_DALM.Value = Convert.ToBoolean((Status >> 1) & 0x01);//舌头模组报警
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 0, 0, ref m_LoadIN00);//进花篮数量检测
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 0, 1, ref m_LoadIN01);//进花篮位置检测1
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 0, 2, ref m_LoadIN02);//进花篮位置检测2
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 0, 3, ref m_LoadIN03);//进花篮位置检测3
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 0, 4, ref m_LoadIN04);//进花篮位置检测4
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 0, 5, ref m_LoadIN05);//进花篮位置检测5
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 0, 6, ref m_LoadIN06);//进花篮位置检测6
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 0, 7, ref m_LoadIN07);//进料阻挡判断

            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 1, 0, ref m_LoadIN10);//进花阻挡气缸1伸出到位
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 1, 1, ref m_LoadIN11);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 1, 2, ref m_LoadIN12);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 1, 3, ref m_LoadIN13);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 1, 4, ref m_LoadIN14);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 1, 5, ref m_LoadIN15);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 1, 6, ref m_LoadIN16);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 1, 7, ref m_LoadIN17);//

            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 2, 0, ref m_LoadIN20);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 2, 1, ref m_LoadIN21);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 2, 2, ref m_LoadIN22);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 2, 3, ref m_LoadIN23);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 2, 4, ref m_LoadIN24);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 2, 5, ref m_LoadIN25);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 2, 6, ref m_LoadIN26);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 2, 7, ref m_LoadIN27);//

            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 3, 0, ref m_LoadIN30);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 3, 1, ref m_LoadIN31);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 3, 2, ref m_LoadIN32);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 3, 3, ref m_LoadIN33);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 3, 4, ref m_LoadIN34);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 3, 5, ref m_LoadIN35);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 3, 6, ref m_LoadIN36);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD140IP, 3, 7, ref m_LoadIN37);//

            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 0, 0, ref m_LoadIN40);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 0, 1, ref m_LoadIN41);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 0, 2, ref m_LoadIN42);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 0, 3, ref m_LoadIN43);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 0, 4, ref m_LoadIN44);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 0, 5, ref m_LoadIN45);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 0, 6, ref m_LoadIN46);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 0, 7, ref m_LoadIN47);//

            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 1, 0, ref m_LoadIN50);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 1, 1, ref m_LoadIN51);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 1, 2, ref m_LoadIN52);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 1, 3, ref m_LoadIN53);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 1, 4, ref m_LoadIN54);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 1, 5, ref m_LoadIN55);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 1, 6, ref m_LoadIN56);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 1, 7, ref m_LoadIN57);//

            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 0, 0, ref m_LoadIN60);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 0, 1, ref m_LoadIN61);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 0, 2, ref m_LoadIN62);//

            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 0, 4, ref m_LoadIN64);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 0, 5, ref m_LoadIN65);//
            LED_IN64.Value = Convert.ToBoolean(m_LoadIN64);//m_IN64
            LED_IN65.Value = Convert.ToBoolean(m_LoadIN65);//m_IN65
            //上料打齐电机
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 0, 6, ref m_LoadIN66);//
            MNet.Basic._mnet_bit_io_input(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 0, 7, ref m_LoadIN67);//
            led_BHomeDone.Value = Convert.ToBoolean(m_LoadIN67);//m_IN67
            led_BAlarm.Value = Convert.ToBoolean(m_LoadIN66);//m_IN66

            LED_IN00.Value = Convert.ToBoolean(m_LoadIN00);
            LED_IN01.Value = Convert.ToBoolean(m_LoadIN01);
            LED_IN02.Value = Convert.ToBoolean(m_LoadIN02);
            LED_IN03.Value = Convert.ToBoolean(m_LoadIN03);
            LED_IN04.Value = Convert.ToBoolean(m_LoadIN04);
            LED_IN05.Value = Convert.ToBoolean(m_LoadIN05);
            LED_IN06.Value = Convert.ToBoolean(m_LoadIN06);
            LED_IN07.Value = Convert.ToBoolean(m_LoadIN07);

            LED_IN10.Value = Convert.ToBoolean(m_LoadIN10);
            LED_IN11.Value = Convert.ToBoolean(m_LoadIN11);

            LED_IN12.Value = Convert.ToBoolean(m_LoadIN12);
            LED_IN13.Value = Convert.ToBoolean(m_LoadIN13);
            LED_IN14.Value = Convert.ToBoolean(m_LoadIN14);
            LED_IN15.Value = Convert.ToBoolean(m_LoadIN15);
            LED_IN16.Value = Convert.ToBoolean(m_LoadIN16);
            LED_IN17.Value = Convert.ToBoolean(m_LoadIN17);

            LED_IN20.Value = Convert.ToBoolean(m_LoadIN20);
            LED_IN21.Value = Convert.ToBoolean(m_LoadIN21);
            LED_IN22.Value = Convert.ToBoolean(m_LoadIN22);
            LED_IN23.Value = Convert.ToBoolean(m_LoadIN23);
            LED_IN24.Value = Convert.ToBoolean(m_LoadIN24);
            LED_IN25.Value = Convert.ToBoolean(m_LoadIN25);
            LED_IN26.Value = Convert.ToBoolean(m_LoadIN26);
            LED_IN27.Value = Convert.ToBoolean(m_LoadIN27);

            LED_IN30.Value = Convert.ToBoolean(m_LoadIN30);
            LED_IN31.Value = Convert.ToBoolean(m_LoadIN31);
            LED_IN32.Value = Convert.ToBoolean(m_LoadIN32);
            LED_IN33.Value = Convert.ToBoolean(m_LoadIN33);
            LED_IN34.Value = Convert.ToBoolean(m_LoadIN34);
            LED_IN37.Value = Convert.ToBoolean(m_LoadIN37);
            LED_IN35.Value = Convert.ToBoolean(m_LoadIN35);
            LED_IN36.Value = Convert.ToBoolean(m_LoadIN36);

            LED_IN40.Value = Convert.ToBoolean(m_LoadIN40);
            LED_IN41.Value = Convert.ToBoolean(m_LoadIN41);
            LED_IN42.Value = Convert.ToBoolean(m_LoadIN42);
            LED_IN43.Value = Convert.ToBoolean(m_LoadIN43);
            LED_IN44.Value = Convert.ToBoolean(m_LoadIN44);
            LED_IN45.Value = Convert.ToBoolean(m_LoadIN45);
            LED_IN46.Value = Convert.ToBoolean(m_LoadIN46);
            LED_IN47.Value = Convert.ToBoolean(m_LoadIN47);

            LED_IN50.Value = Convert.ToBoolean(m_LoadIN50);
            LED_IN51.Value = Convert.ToBoolean(m_LoadIN51);
            LED_IN52.Value = Convert.ToBoolean(m_LoadIN52);
            LED_IN53.Value = Convert.ToBoolean(m_LoadIN53);
            LED_IN54.Value = Convert.ToBoolean(m_LoadIN54);
            LED_IN55.Value = Convert.ToBoolean(m_LoadIN55);
            LED_IN56.Value = Convert.ToBoolean(m_LoadIN56);
            LED_IN57.Value = Convert.ToBoolean(m_LoadIN57);

            LED_IN60.Value = Convert.ToBoolean(m_LoadIN60);
            LED_IN61.Value = Convert.ToBoolean(m_LoadIN61);
            LED_IN62.Value = Convert.ToBoolean(m_LoadIN62);
            //////////////////////运动停止
            if ((m_MotorCCWRun > 0) && (m_MotorCCWRun < 100))//托盘电机
                m_MotorCCWRun++;
            else if (m_MotorCCWRun == 100)
            {
                m_MotorCCWRun = 0;
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 2, 0);//托盘电机正转停止
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
            }
            ///////////进料电机1
            if ((m_MotorIn1Run > 0) && (m_MotorIn1Run < 100))//进料电机1
                m_MotorIn1Run++;
            else if (m_MotorIn1Run == 100)
            {
                m_MotorIn1Run = 0;
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 0, 0);////进花篮电机1反转停止
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 0, 0);////进花篮电机1运动 停止               
            }
            ///////////进料电机2
            if ((m_MotorIn2Run > 0) && (m_MotorIn2Run < 100))//进料电机1
                m_MotorIn2Run++;
            else if (m_MotorIn2Run == 100)
            {
                m_MotorIn2Run = 0;
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 1, 0);//进花篮电机2反转停止
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 1, 0);//进花篮电机2运动停止                
            }
            //////////////////出料电机1
            if ((m_MotorOut1Run > 0) && (m_MotorOut1Run < 100))//出料电机1
                m_MotorOut1Run++;
            else if (m_MotorOut1Run == 100)
            {
                m_MotorOut1Run = 0;
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 2, 0);////出花篮电机反转停止
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 4, 0);////出花篮电机运动停止                
            }
            //////////////////出料电机2
            if ((m_MotorOut2Run > 0) && (m_MotorOut2Run < 100))//出料电机1
                m_MotorOut2Run++;
            else if (m_MotorOut2Run == 100)
            {
                m_MotorOut2Run = 0;
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 3, 0);////出花篮电机2反转停止
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 5, 0);////出花篮电机2运动停止
            }
            ///上料KK模组归零完成
            ushort GetState4 = 20;
            switch (LoadHomeFlag)
            {
                case 1:
                    MNet.M1A._mnet_m1a_motion_done(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, ref GetState4);//获取轴状态   
                    if (GetState4 == 0)//如果轴就绪
                    {
                        if (m_LoadHomeDelay < 10)//延时200ms
                            m_LoadHomeDelay++;
                        else
                        {
                            m_LoadHomeDelay = 0;
                            MNet.M1A._mnet_m1a_set_home_config(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, 0, 0, 0, 0, 0);
                            MNet.M1A._mnet_m1a_start_home_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, 0);
                            LoadHomeFlag = 2;

                        }
                    }
                    break;
                case 2:
                    MNet.M1A._mnet_m1a_motion_done(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, ref GetState4);//获取轴状态 
                    if (GetState4 == 0)
                    {
                        if (m_LoadHomeDelay < 10)//延时200ms
                            m_LoadHomeDelay++;
                        else
                        {
                            m_LoadHomeDelay = 0;
                            MNet.M1A._mnet_m1a_set_tmove_speed(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, FormHMI.pci1245l.ElevatorRatio * 5, FormHMI.pci1245l.ElevatorRatio * FormHMI.pci1245l.ElevatorMaxSpeed,
                                     0.1f, 0.1f);
                            MNet.M1A._mnet_m1a_start_a_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, (int)(FormHMI.g_SysParam[20] * FormHMI.pci1245l.ElevatorRatio));
                            LoadHomeFlag = 3;
                        }
                    }
                    break;
                case 3:
                    MNet.M1A._mnet_m1a_motion_done(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, ref GetState4);//获取轴状态 
                    if (GetState4 == 0)
                    {
                        LoadHomeFlag = 0; FormHMI.g_LoadHome = true;
                        Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 1);
                        System.Threading.Thread.Sleep(200);//延时200ms
                        Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 0);
                        if (FormHMI.m_HomeFinished == 1)
                            FormHMI.m_LoadHomeFlag = 5;
                        else
                            FormHMI.m_LoadHomeFlag = 4;
                        MessageBox.Show(this, "接驳台上料升降模组已完成归零运动！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    break;
            }

            /////////////自动化运行时，全部按钮变灰
            if ((FormPassword.g_Authority == 3) || ((FormPassword.g_Authority == 2) && (FormHMI.g_Modify == true)))
                fDim(true);
            else
                fDim(false);
        }
        /*********************************************************************************************************
        ** 函数名称 ：fDim
        ** 函数功能 ：按钮使能控制 
        ** 修改时间 ：20170925
        ** 修改内容 ：
        *********************************************************************************************************/
        void fDim(bool data)
        {

            KEY_SendUpDownOnOff.Enabled = data;
            KEY_SendStopOnOff.Enabled = data;
            KEY_FixtureOnOff.Enabled = data;
            //KEY_WaferSendOnOff.Enabled = data;
            KEY_WaferIn.Enabled = data;
            KEY_UpDown1OnOff.Enabled = data;
            Key_OUT25.Enabled = data;
            Key_OUT26.Enabled = data;
            KEY_LoadInvRun.Enabled = data;
            KEY_LoadElevatorOut.Enabled = data;
            KEY_LoadElevatorStart.Enabled = data;
            KEY_LoadElevatorStep.Enabled = data;
            KEY_LoadElevatorIn.Enabled = data;
            KEY_LoadBoxIn1.Enabled = data;
            KEY_LoadOUT41.Enabled = data;
            KEY_OUT30.Enabled = data;
            KEY_OUT40.Enabled = data;
            KEY_LoadBoxOut2.Enabled = data;
            KEY_LoadBoxIn2.Enabled = data;
            SetLoadCasstte.Visible = data;
            // KEY_ElevatorInHome.Enabled = data;

        }
        /*********************************************************************************************************
        ** 函数名称 ：SetLoadCasstte_Click
        ** 函数功能 ：设置花篮判断sensor 
        ** 修改时间 ：20170925
        ** 修改内容 ：
        *********************************************************************************************************/
        private void SetLoadCasstte_Click(object sender, EventArgs e)
        {
            DialogResult ResultTemp;
            int i;
            uint LoadCasstteStatus = 0;
            string sysp;

            ResultTemp = MessageBox.Show(this, "不是必要情况下，请勿修改参数，请确认是否需要进行参数修改？", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (ResultTemp == DialogResult.OK)   //表示点击确认        
            {

                FormHMI.g_IN51 = Convert.ToUInt32(IN51.Checked);
                FormHMI.g_IN52 = Convert.ToUInt32(IN52.Checked);
                FormHMI.g_IN53 = Convert.ToUInt32(IN53.Checked);
                FormHMI.g_IN54 = Convert.ToUInt32(IN54.Checked);
                FormHMI.g_IN55 = Convert.ToUInt32(IN55.Checked);
                LoadCasstteStatus = (FormHMI.g_IN55 << 4) + (FormHMI.g_IN54 << 3) + (FormHMI.g_IN53 << 2) + (FormHMI.g_IN52 << 1) + FormHMI.g_IN51;
                FormHMI.g_SysParam[43] = LoadCasstteStatus;
                StreamWriter write = new StreamWriter("D:\\DRLaser\\SysPara.ini");
                for (i = 0; i < 80; i++)
                {
                    sysp = Convert.ToString(FormHMI.g_SysParam[i]);
                    write.WriteLine(sysp);
                }
                write.Close();
            }
        }
        #region   ///气缸动作区
        /*********************************************************************************************************
        ** 函数名称 ：SendUpDownOnOff_StateChanged
        ** 函数功能 ：进料阻挡气缸1 
        ** 修改时间 ：20180710
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_UpDown1OnOff_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {

            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 2, 0, Convert.ToByte(KEY_UpDown1OnOff.Value));
            // MNet.Basic._mnet_bit_io_output(0, 2, 2, 4, 1);

        }
        /*********************************************************************************************************
        ** 函数名称 ：SendUpDownOnOff_StateChanged
        ** 函数功能 ：进料阻挡气缸2 
        ** 修改时间 ：20180210
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_SendUpDownOnOff_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 2, 1, Convert.ToByte(KEY_SendUpDownOnOff.Value));
            // MNet.Basic._mnet_bit_io_output(0, 2, 2, 4, 1);

        }

        /*********************************************************************************************************
        ** 函数名称 ：KEY_SendStopOnOff_StateChanged
        ** 函数功能 ：进料阻挡气缸3 
        ** 修改时间 ：20180210
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_SendStopOnOff_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 2, 2, Convert.ToByte(KEY_SendStopOnOff.Value));
        }
        /*********************************************************************************************************
        ** 函数名称 ：FixtureOnOff_StateChanged
        ** 函数功能 ：下压气缸动作
        ** 修改时间 ：20180210
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_FixtureOnOff_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {

            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 2, 3, Convert.ToByte(KEY_FixtureOnOff.Value));
        }
        /*********************************************************************************************************
       ** 函数名称 ：KEY_WaferSendOnOff_StateChanged
       ** 函数功能 ：舌头伸缩
       ** 修改时间 ：20180210
       ** 修改内容 ：
       *********************************************************************************************************/
        private void KEY_WaferSendOnOff_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {
            byte OnOff = 0;
            OnOff = Convert.ToByte(KEY_WaferSendOnOff.Value);
            if ((FormHMI.g_LoadElevationP <= FormHMI.g_SysParam[3] * FormHMI.pci1245l.ElevatorRatio) && (FormHMI.g_LoadElevationP >= (FormHMI.g_SysParam[3] - FormHMI.g_SysParam[5] * FormHMI.g_SysParam[28] - FormHMI.g_SysParam[29]) * FormHMI.pci1245l.ElevatorRatio) && (FormHMI.m_LoadHandOut == false))
            {

                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 2, 4, Convert.ToByte(KEY_WaferSendOnOff.Value));
            }
            else
            {
                if (OnOff == 1)
                {
                    Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 1);
                    System.Threading.Thread.Sleep(200);//延时200ms
                    Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 0);
                    KEY_WaferSendOnOff.Value = false;
                    MessageBox.Show(this, "升降模组处于舌头不能伸出位置，舌头不能伸出！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }
        /*********************************************************************************************************
      ** 函数名称 ：Key_OUT25_StateChanged
      ** 函数功能 ：出花篮阻挡气缸1
      ** 修改时间 ：20180710
      ** 修改内容 ：
      *********************************************************************************************************/
        private void Key_OUT25_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 2, 5, Convert.ToByte(Key_OUT25.Value));
        }


        #endregion
        #region ///电机运动区
        /*********************************************************************************************************
        ** 函数名称 ：LoadBoxIn1_Click
        ** 函数功能 ：进料传送模组2运动    
        ** 修改时间 ：20180210
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_LoadBoxIn1_Click(object sender, EventArgs e)
        {
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 1, 0);//进花篮电机2反转停止
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 1, 1);//进花篮电机2运动 
            m_MotorIn2Run = 1;
        }

        /*********************************************************************************************************
        ** 函数名称 ：LoadBoxIn2_Click
        ** 函数功能 ：托盘模组传送运动    
        ** 修改时间 ：20180210
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_LoadBoxIn2_Click(object sender, EventArgs e)
        {
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 2, 1);//托盘电机正转
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
            m_MotorCCWRun = 1;
        }
        /*********************************************************************************************************
       ** 函数名称 ：KEY_LoadBoxOut2_Click
       ** 函数功能 ：托盘模组传送运动    
       ** 修改时间 ：20180704
       ** 修改内容 ：
       *********************************************************************************************************/
        private void KEY_LoadBoxOut2_Click(object sender, EventArgs e)
        {
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 2, 0);//托盘电机正转停止
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 3, 1);//托盘电机反转
            m_MotorCCWRun = 1;
        }
        /*********************************************************************************************************
        ** 函数名称 ：LoadBoxOut_Click
        ** 函数功能 ：出料传送模组运动  
        ** 修改时间 ：20180210
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_LoadBoxOut_Click(object sender, EventArgs e)
        {
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 2, 0);////出花篮电机反转停止
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 4, 1);////出花篮电机运动
            m_MotorOut1Run = 1;
        }

        /*********************************************************************************************************
        ** 函数名称 ：WaferIn_Click
        ** 函数功能 ：舌头电机运动         
        ** 修改时间 ：20180210
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_WaferIn_Click(object sender, EventArgs e)
        {
            //Motion.mAcm_AxMoveRel(FormHMI.ax3Handle[FormHMI.pci1245l.Axis_FOUR], 300 * FormHMI.pci1245l.WaferRatio);             
            MNet.M1A._mnet_m1a_start_r_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX1IP, (int)(FormHMI.pci1245l.WaferRatio * 300));//舌头运动
        }
        #endregion
        #region ///KK模组运动区
        /*********************************************************************************************************
        ** 函数名称 ：KEY_ElevatorInHome_Click
        ** 函数功能 ：进料KK模组回零 
        ** 修改时间 ：20180210
        ** 修改内容 ：  
        *********************************************************************************************************/
        private void KEY_ElevatorInHome_Click(object sender, EventArgs e)
        {
            ushort Get1State = 100;
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 2, 0);
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 3, 0);
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 1, 0);
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 0, 0);

            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 2, 0);//托盘电机正转停止          
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 1, 0);//进花篮电机运动停止
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 4, 0);////出花篮电机运动停止
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 2, 1, 0);//阻挡汽缸2伸出
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 2, 2, 0);//阻挡汽缸3伸出
            MNet.M1A._mnet_m1a_motion_done(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, ref Get1State);//升降模组停止运动 
            FormHMI.m_LoadKeyIn = 0; FormHMI.m_LoadHomeFlag = 0;
            if (LED_IN64.Value || ((FormHMI.g_CyAlign == 1) && (LED_IN65.Value = false)))   //打齐伸出||如果启用了打齐&&打齐没缩回
            {
                MessageBox.Show(this, "请先缩回花篮打齐气缸，再执行到达进花篮位置指令！");
                return;
            }
            if ((LED_IN47.Value != true) || (LED_IN42.Value == true) || (LED_IN43.Value == true))
            {
                MessageBox.Show(this, "舌头没有缩回，或者上下对射式传感器被遮挡，回零动作不能执行，请检查！", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                return;
            }
            else if (((Get1State == 0) || (Get1State == 3)) && LED_XPos.Value)
            {
                DialogResult ResultTemp;
                Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 1);
                System.Threading.Thread.Sleep(200);//延时200ms
                Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 0);
                ResultTemp = MessageBox.Show(this, "请确认是否进行上料KK模组电机归零运动！", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (ResultTemp == DialogResult.OK)   //表示点击确认        
                {
                    MNet.M1A._mnet_m1a_set_tmove_speed(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, FormHMI.pci1245l.ElevatorRatio * 5, FormHMI.pci1245l.ElevatorRatio * 30,
                                  0.1f, 0.1f);
                    MNet.M1A._mnet_m1a_start_r_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, (int)(50 * FormHMI.pci1245l.ElevatorRatio));
                    LoadHomeFlag = 1;

                }

            }
            else
            {
                MessageBox.Show(this, "上料接驳台有运动轴没有停止运动，归零运动不能执行！", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                return;
            }
        }
        /*********************************************************************************************************
        ** 函数名称 ：LoadInvRun_Click
        ** 函数功能 ：升降模组格点运动  
        ** 修改时间 ：20180210
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_LoadInvRun_Click(object sender, EventArgs e)
        {
            double InvPosData = 0;
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 2, 0);//托盘电机正转停止           
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
            FormHMI.m_LoadKeyIn = 0; FormHMI.m_LoadHomeFlag = 0;
            if (LED_IN64.Value || ((FormHMI.g_CyAlign == 1) && (LED_IN65.Value = false)))   //打齐伸出||如果启用了打齐&&打齐没缩回
            {
                MessageBox.Show(this, "请先缩回花篮打齐气缸，再执行到达进花篮位置指令！");
                return;
            }
            if ((FormHMI.g_SysParam[38] - FormHMI.g_SysParam[3]) > 4.0)		//两个位置偏差太大
                MessageBox.Show(this, "请注意：第一片位置与起始位置偏差太大，请重新设置！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                InvPosData = InvPos.Value;
                if (InvPosData == 1)
                    MNet.M1A._mnet_m1a_start_a_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, (int)(FormHMI.g_SysParam[38] * FormHMI.pci1245l.ElevatorRatio));//

                else if (InvPosData <= FormHMI.g_SysParam[28])
                {
                    MNet.M1A._mnet_m1a_set_tmove_speed(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, FormHMI.pci1245l.ElevatorRatio * 5, FormHMI.pci1245l.ElevatorRatio * 50,
                                   0.1f, 0.1f);
                    if (InvPosData > (FormHMI.g_SysParam[28] / 2))
                        MNet.M1A._mnet_m1a_start_a_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, (int)((FormHMI.g_SysParam[38] - (InvPosData - 1) * FormHMI.g_SysParam[5] - FormHMI.g_SysParam[29]) * FormHMI.pci1245l.ElevatorRatio));

                    else
                        MNet.M1A._mnet_m1a_start_a_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, (int)((FormHMI.g_SysParam[38] - (InvPosData - 1) * FormHMI.g_SysParam[5]) * FormHMI.pci1245l.ElevatorRatio));
                }
                else
                {
                    MessageBox.Show(this, "格点数大于花篮格数最大值，格点运动不能执行！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
        }

        /*********************************************************************************************************
        ** 函数名称 ：LoadElevatorStart_Click
        ** 函数功能 ：升降模组起点位置 
        ** 修改时间 ：20170925
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_LoadElevatorStart_Click(object sender, EventArgs e)
        {
            //int IOStatus;
            //IOStatus = (FormHMI.g_Ax3Hand_SpecialIN[2] >> 1) & 0x01;
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 2, 0);//托盘电机正转停止           
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
            FormHMI.m_LoadKeyIn = 0; FormHMI.m_LoadHomeFlag = 0;
            if (LED_IN64.Value || ((FormHMI.g_CyAlign == 1) && (LED_IN65.Value = false)))   //打齐伸出||如果启用了打齐&&打齐没缩回
            {
                MessageBox.Show(this, "请先缩回花篮打齐气缸，再执行到达进花篮位置指令！");
                return;
            }
            if ((LED_IN47.Value == true))//舌头缩回到位     
            {
                MNet.M1A._mnet_m1a_set_tmove_speed(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, FormHMI.pci1245l.ElevatorRatio * 5, FormHMI.pci1245l.ElevatorRatio * FormHMI.pci1245l.ElevatorMaxSpeed,
                                   0.1f, 0.1f);
                MNet.M1A._mnet_m1a_start_a_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, (int)(FormHMI.g_SysParam[3] * FormHMI.pci1245l.ElevatorRatio));
            }
            else
                MessageBox.Show(this, "舌头处于伸出状态，请先缩回舌头，再执行到达进花篮位置指令！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        /*********************************************************************************************************
        ** 函数名称 ：LoadElevatorStep_Click
        ** 函数功能 ：升降模组向下单步运动 
        ** 修改时间 ：20170925
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_LoadElevatorStep_Click(object sender, EventArgs e)
        {
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 2, 0);//托盘电机正转停止           
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
            if (LED_IN64.Value || ((FormHMI.g_CyAlign == 1) && (LED_IN65.Value = false)))   //打齐伸出||如果启用了打齐&&打齐没缩回
            {
                MessageBox.Show(this, "请先缩回花篮打齐气缸，再执行到达进花篮位置指令！");
                return;
            }
            FormHMI.m_LoadKeyIn = 0; FormHMI.m_LoadHomeFlag = 0;
            if ((FormHMI.g_LoadElevationP <= FormHMI.g_SysParam[3] * FormHMI.pci1245l.ElevatorRatio) && (FormHMI.g_LoadElevationP >= (FormHMI.g_SysParam[3] - FormHMI.g_SysParam[28] * FormHMI.g_SysParam[5]) * FormHMI.pci1245l.ElevatorRatio))
            {
                MNet.M1A._mnet_m1a_set_tmove_speed(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, FormHMI.pci1245l.ElevatorRatio * 5, FormHMI.pci1245l.ElevatorRatio * FormHMI.pci1245l.ElevatorMaxSpeed,
                                   0.1f, 0.1f);
                MNet.M1A._mnet_m1a_start_r_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, (int)(-FormHMI.g_SysParam[5] * FormHMI.pci1245l.ElevatorRatio));
            }
            else
            {
                MessageBox.Show(this, "升降模组处于非正常出硅片位置，单步运动不能执行！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        /*********************************************************************************************************
        ** 函数名称 ：LoadElevatorUpStep_Click
        ** 函数功能 ：升降模组向上单步运动 
        ** 修改时间 ：20190226
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_LoadElevatorUpStep_Click(object sender, EventArgs e)
        {
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 2, 0);//托盘电机正转停止           
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
            if (LED_IN64.Value || ((FormHMI.g_CyAlign == 1) && (LED_IN65.Value = false)))   //打齐伸出||如果启用了打齐&&打齐没缩回
            {
                MessageBox.Show(this, "请先缩回花篮打齐气缸，再执行到达进花篮位置指令！");
                return;
            }
            FormHMI.m_LoadKeyIn = 0; FormHMI.m_LoadHomeFlag = 0;
            if ((FormHMI.g_LoadElevationP <= FormHMI.g_SysParam[3] * FormHMI.pci1245l.ElevatorRatio) && (FormHMI.g_LoadElevationP >= (FormHMI.g_SysParam[3] - FormHMI.g_SysParam[28] * FormHMI.g_SysParam[5]) * FormHMI.pci1245l.ElevatorRatio))
            {
                MNet.M1A._mnet_m1a_set_tmove_speed(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, FormHMI.pci1245l.ElevatorRatio * 5, FormHMI.pci1245l.ElevatorRatio * FormHMI.pci1245l.ElevatorMaxSpeed,
                                   0.1f, 0.1f);
                MNet.M1A._mnet_m1a_start_r_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, (int)(FormHMI.g_SysParam[5] * FormHMI.pci1245l.ElevatorRatio));
            }
            else
            {
                MessageBox.Show(this, "升降模组处于非正常出硅片位置，单步运动不能执行！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }



        /*********************************************************************************************************
        ** 函数名称 ：LoadElevatorIn_Click
        ** 函数功能 ：升降模组进花篮位置
        ** 修改时间 ：20170925
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_LoadElevatorIn_Click(object sender, EventArgs e)
        {
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 2, 0);//托盘电机正转停止           
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
            FormHMI.m_LoadKeyIn = 0; FormHMI.m_LoadHomeFlag = 0;
            //int IOStatus;
            //IOStatus = (FormHMI.g_Ax3Hand_SpecialIN[2] >> 1) & 0x01;
            if (LED_IN64.Value || ((FormHMI.g_CyAlign == 1) && (LED_IN65.Value = false)))   //打齐伸出||如果启用了打齐&&打齐没缩回
            {
                MessageBox.Show(this, "请先缩回花篮打齐气缸，再执行到达进花篮位置指令！");
                return;
            }
            if ((LED_IN47.Value == true))//舌头缩回到位  
            {

                MNet.M1A._mnet_m1a_set_tmove_speed(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, FormHMI.pci1245l.ElevatorRatio * 5, FormHMI.pci1245l.ElevatorRatio * FormHMI.pci1245l.ElevatorMaxSpeed,
                                   0.1f, 0.1f);
                MNet.M1A._mnet_m1a_start_a_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, (int)(FormHMI.g_SysParam[20] * FormHMI.pci1245l.ElevatorRatio));
            }
            else
                MessageBox.Show(this, "舌头处于伸出状态，请先缩回舌头，再执行到达进花篮位置指令！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void KEY_InAlignPos_Click(object sender, EventArgs e)
        {
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 2, 0);//托盘电机正转停止           
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止

            if (LED_IN64.Value || ((FormHMI.g_CyAlign == 1) && (LED_IN65.Value = false)))   //打齐伸出||如果启用了打齐&&打齐没缩回
            {
                MessageBox.Show(this, "请先缩回花篮打齐气缸，再执行到达进花篮位置指令！");
                return;
            }

            if ((LED_IN47.Value == true))//舌头缩回到位     
            {
                MNet.M1A._mnet_m1a_set_tmove_speed(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, FormHMI.pci1245l.ElevatorRatio * 5, FormHMI.pci1245l.ElevatorRatio * FormHMI.pci1245l.ElevatorMaxSpeed, 0.1f, 0.1f);
                MNet.M1A._mnet_m1a_start_a_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, (int)(FormHMI.m_AlignPos * FormHMI.pci1245l.ElevatorRatio));
            }
            else
                MessageBox.Show(this, "舌头处于伸出状态，请先缩回舌头，再执行到达进花篮位置指令！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        ///*********************************************************************************************************
        //** 函数名称 ：KEY_LoadEquPosition_Click
        //** 函数功能 ：进料KK模组 齐平位置
        //** 修改时间 ：20180116
        //** 修改内容 ： 
        //*********************************************************************************************************/
        //private void KEY_LoadEquPosition_Click(object sender, EventArgs e)
        //{
        //    int IOStatus;
        //    IOStatus = (FormHMI.g_Ax3Hand_SpecialIN[2] >> 1) & 0x01;
        //    if ((IOStatus == 1) && (LED_CIN1.Value == true))//舌头缩回到位          
        //    {
        //        Motion.mAcm_SetF64Property(FormHMI.ax3Handle[FormHMI.pci1245l.Axis_FIVE], (uint)PropertyID.PAR_AxVelLow, FormHMI.pci1245l.ElevatorRatio * 5);
        //        Motion.mAcm_SetF64Property(FormHMI.ax3Handle[FormHMI.pci1245l.Axis_FIVE], (uint)PropertyID.PAR_AxVelHigh, FormHMI.pci1245l.ElevatorRatio * FormHMI.pci1245l.ElevatorMaxSpeed);
        //        Motion.mAcm_AxMoveAbs(FormHMI.ax3Handle[FormHMI.pci1245l.Axis_FIVE], FormHMI.g_SysParam[35] * FormHMI.pci1245l.ElevatorRatio);
        //    }
        //    else
        //        MessageBox.Show(this, "舌头处于伸出状态，请先缩回舌头，再执行到达进花篮位置指令！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //}
        /*********************************************************************************************************
        ** 函数名称 ：LoadElevatorOut_Click
        ** 函数功能 ：升降模组出花篮运动 
        ** 修改时间 ：20170925
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_LoadElevatorOut_Click(object sender, EventArgs e)
        {
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 2, 0);//托盘电机正转停止           
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 3, 0);//托盘电机反转停止
            FormHMI.m_LoadKeyIn = 0; FormHMI.m_LoadHomeFlag = 0;
            if (LED_IN64.Value || ((FormHMI.g_CyAlign == 1) && (LED_IN65.Value = false)))   //打齐伸出||如果启用了打齐&&打齐没缩回
            {
                MessageBox.Show(this, "请先缩回花篮打齐气缸，再执行到达进花篮位置指令！");
                return;
            }
            if ((LED_IN47.Value == true))//舌头缩回到位    
            {
                MNet.M1A._mnet_m1a_set_tmove_speed(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, FormHMI.pci1245l.ElevatorRatio * 5, FormHMI.pci1245l.ElevatorRatio * FormHMI.pci1245l.ElevatorMaxSpeed,
                                   0.1f, 0.1f);
                MNet.M1A._mnet_m1a_start_a_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX0IP, (int)(FormHMI.g_SysParam[21] * FormHMI.pci1245l.ElevatorRatio));
            }
            else
                MessageBox.Show(this, "舌头处于伸出状态，请先缩回舌头，再执行到达出花篮位置指令！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        #endregion
        /*********************************************************************************************************
        ** 函数名称 ：KEY_OUT35_Click
        ** 函数功能 ：出料2电机         
        ** 修改时间 ：20180818
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_OUT35_Click(object sender, EventArgs e)
        {
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 3, 0);////出花篮电机2反转停止
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 5, 1);////出花篮电机2运动
            m_MotorOut2Run = 1;
        }
        /*********************************************************************************************************
        ** 函数名称 ：KEY_OUT30_Click
        ** 函数功能 ：进料1电机         
        ** 修改时间 ：20180818
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_OUT30_Click(object sender, EventArgs e)
        {
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 0, 0);////进花篮电机1反转停止
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 0, 1);////进花篮电机1运动
            m_MotorIn1Run = 1;
        }
        /*********************************************************************************************************
        ** 函数名称 ：Key_OUT26_StateChanged
        ** 函数功能 ：AGV阻挡         
        ** 修改时间 ：20180818
        ** 修改内容 ：
        *********************************************************************************************************/
        private void Key_OUT26_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 2, 6, Convert.ToByte(Key_OUT26.Value));
        }
        /*********************************************************************************************************
        ** 函数名称 ：KEY_LoadBoxOutN_Click
        ** 函数功能 ：出料传送模组反转  
        ** 修改时间 ：20180822
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_LoadBoxOutN_Click(object sender, EventArgs e)
        {
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 4, 0);////出花篮电机运动停止
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 2, 1);////出花篮电机反转
            m_MotorOut1Run = 1;
        }
        /*********************************************************************************************************
        ** 函数名称 ：KEY_OUT43_Click
        ** 函数功能 ：出料2电机反转         
        ** 修改时间 ：20180818
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_OUT43_Click(object sender, EventArgs e)
        {
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 5, 0);////出花篮电机运动停止
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 3, 1);////出花篮电机反转 
            m_MotorOut2Run = 1;
        }
        /*********************************************************************************************************
        ** 函数名称 ：KEY_LoadOUT41_Click
        ** 函数功能 ：进料传送模组反转  
        ** 修改时间 ：20180822
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_LoadOUT41_Click(object sender, EventArgs e)
        {
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 1, 0);//进花篮电机停止
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 1, 1);//进花篮电机反转
            m_MotorIn2Run = 1;
        }
        /*********************************************************************************************************
        ** 函数名称 ：KEY_OUT40_Click
        ** 函数功能 ：进料1电机反转       
        ** 修改时间 ：20180822
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_OUT40_Click(object sender, EventArgs e)
        {
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 3, 0, 0);////出花篮电机运动停止
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 0, 1);////出花篮电机反转
            m_MotorIn1Run = 1;
        }
        /*********************************************************************************************************
        ** 函数名称 ：KEY_UpDownOnOff_StateChanged
        ** 函数功能 ：打齐      
        ** 修改时间 ：20180914
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_UpDownOnOff_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122IP, 2, 7, Convert.ToByte(KEY_UpDownOnOff.Value));
        }

        private void KEY_OUT46_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {
            if (Convert.ToByte(KEY_OUT46.Value) == 1)
            {
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 6, 1); //进料打齐
            }
            System.Threading.Thread.Sleep(20);
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 6, 0); //进料打齐
        }

        private void KEY_Align_Click(object sender, EventArgs e)
        {
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 5, 1); //进料打齐复位
            System.Threading.Thread.Sleep(50);
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 5, 0); //进料打齐复位
            System.Threading.Thread.Sleep(2);
            MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 6, 0); //进料打齐
        }

        private void KEY_AlignDy_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {
            if ((Math.Abs(FormHMI.g_LoadElevationP - (int)(FormHMI.m_AlignPos * FormHMI.pci1245l.ElevatorRatio)) < 0.01))
            {
                MNet.Basic._mnet_bit_io_output(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadD122_2IP, 2, 4, Convert.ToByte(KEY_AlignDy.Value));
            }
            else
            {
                MessageBox.Show(this, "升降模组不在打齐位置，打齐气缸不能伸出！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

    }
}
