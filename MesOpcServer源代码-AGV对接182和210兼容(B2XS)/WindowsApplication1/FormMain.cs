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
using TPM;
namespace WindowsApplication1
{
    public delegate void ChangeFMFormData(bool topGetState4);
    public delegate void ChangeFMFormData2(bool topGetState42);
    public partial class FormMain : Form
    {
        public event ChangeFMFormData ChangeData;
        public event ChangeFMFormData ChangeData1;//DD正转45度
        public event ChangeFMFormData ChangeData2;//DD反转45度
        public event ChangeFMFormData2 OutChangeData;
        public event ChangeFMFormData2 OutChangeData1;//出料旋转臂正转45度
        public event ChangeFMFormData2 OutChangeData2;//出料旋转臂反转45度
        public FormMain()
        {
            InitializeComponent();
        }


        /*********************************************************************************************************
       ** 函数名称 ：timer1_Tick
       ** 函数功能 ：主定时器  
       ** 修改时间 ：20170915
       ** 修改内容 ：
       *********************************************************************************************************/
        private void timer1_Tick(object sender, EventArgs e)
        {
            ushort GetState1 = 0, GetState2 = 0, GetState3 = 0;
            int Rotatem_VelLow = (int)FormHMI.pci1245l.RotateRatio * 100, Rotatem_VelHigh = (int)FormHMI.pci1245l.RotateRatio * FormHMI.pci1245l.RotateSpeed;
            double Bufferm_VelLow = FormHMI.pci1245l.BufferRatio * 10, Bufferm_VelHigh = FormHMI.pci1245l.BufferRatio * FormHMI.pci1245l.BufferSpeed;
            if (FormHMI.g_InRotateHome == true)
                KEY_InRotate.Visible = true;
            //////进料旋转臂位置
            switch (FormHMI.g_InRotateMotorPosition)
            {
                case 0:
                    groupBox5.Left = 43; groupBox5.Top = 10;
                    groupBox9.Left = 43; groupBox9.Top = 160;
                    break;
                case 1:
                    groupBox5.Left = 43; groupBox5.Top = 160;
                    groupBox9.Left = 43; groupBox9.Top = 10;
                    break;

            }
            //////出料旋转臂位置
            switch (FormHMI.g_OutRotateMotorPosition)
            {
                case 0:
                    groupBox10.Left = 7; groupBox10.Top = 67;
                    groupBox13.Left = 92; groupBox13.Top = 67;
                    break;
                case 1:
                    groupBox10.Left = 92; groupBox10.Top = 67;
                    groupBox13.Left = 7; groupBox13.Top = 67;
                    break;

            }
            //////DD旋转位置
            switch (FormHMI.g_CWPositionFlag)
            {
                case 0:
                    groupBox1.Left = 99; groupBox1.Top = 9;
                    groupBox2.Left = 193; groupBox2.Top = 88;
                    groupBox3.Left = 99; groupBox3.Top = 159;
                    groupBox4.Left = 5; groupBox4.Top = 88;
                    break;
                case 1:
                    groupBox2.Left = 99; groupBox2.Top = 9;
                    groupBox3.Left = 193; groupBox1.Top = 88;
                    groupBox4.Left = 99; groupBox4.Top = 159;
                    groupBox1.Left = 5; groupBox3.Top = 88;
                    break;
                case 2:
                    groupBox3.Left = 99; groupBox3.Top = 9;
                    groupBox4.Left = 193; groupBox4.Top = 88;
                    groupBox1.Left = 99; groupBox1.Top = 159;
                    groupBox2.Left = 5; groupBox2.Top = 88;
                    break;
                case 3:
                    groupBox4.Left = 99; groupBox4.Top = 9;
                    groupBox1.Left = 193; groupBox3.Top = 88;
                    groupBox2.Left = 99; groupBox2.Top = 159;
                    groupBox3.Left = 5; groupBox1.Top = 88;
                    break;

            }

            /////////Buffer数值
            LoadBufferQ.Value = FormHMI.g_LoadBuffer;
            //  UnLoadBufferQ.Value = FormHMI.g_UnLoadBuffer;
            /////////////自动化运行时，全部按钮变灰
            if (FormHMI.g_MainStep < 30)
                fDim(false);
            else
                fDim(true);
            /////////IO/////////////////////

            LED_AIN0.Value = Convert.ToBoolean((g_Ax1Hand_IO >> 0) & 0x1);//AIN0             
            LED_AIN1.Value = Convert.ToBoolean((g_Ax1Hand_IO >> 1) & 0x1);//AIN1

            LED_AIN2.Value = Convert.ToBoolean((g_Ax1Hand_IO >> 2) & 0x1);//AIN2

            LED_BIN0.Value = Convert.ToBoolean((g_Ax1Hand_IO >> 4) & 0x1);//BIN0
            LED_BIN1.Value = Convert.ToBoolean((g_Ax1Hand_IO >> 5) & 0x1);//BIN1 
            LED_BIN3.Value = Convert.ToBoolean((g_Ax1Hand_IO >> 7) & 0x1);//BIN3 

            LED_CIN0.Value = Convert.ToBoolean((g_Ax1Hand_IO >> 8) & 0x1);//CIN0
            LED_CIN1.Value = Convert.ToBoolean((g_Ax1Hand_IO >> 9) & 0x1);//CIN1
            LED_CIN2.Value = Convert.ToBoolean((g_Ax1Hand_IO >> 10) & 0x1);//CIN2
            LED_CIN3.Value = Convert.ToBoolean((g_Ax1Hand_IO >> 11) & 0x1);//CIN3


            LED_DIN1.Value = Convert.ToBoolean((g_Ax1Hand_IO >> 13) & 0x1);//DIN1

            LED_DIN2.Value = Convert.ToBoolean((g_Ax1Hand_IO >> 14) & 0x1);//DIN2
            LED_DIN3.Value = Convert.ToBoolean((g_Ax1Hand_IO >> 15) & 0x1);//DIN3

            LED_XIN0.Value = Convert.ToBoolean((g_Ax2Hand_IO >> 0) & 0x1);//XIN0
            //  LED_XIN2.Value = Convert.ToBoolean((g_Ax2Hand_IO >> 2) & 0x1);//XIN2
            //  LED_XIN3.Value = Convert.ToBoolean((g_Ax2Hand_IO >> 3) & 0x1);//XIN3

            // LED_YIN0.Value = Convert.ToBoolean((g_Ax2Hand_IO >> 4) & 0x1);//YIN0
            // LED_YIN1.Value = Convert.ToBoolean((g_Ax2Hand_IO >> 5) & 0x1);//YIN1

            LED_ZIN0.Value = Convert.ToBoolean((g_Ax2Hand_IO >> 8) & 0x1);//ZIN0
            //  LED_ZIN1.Value = Convert.ToBoolean((g_Ax2Hand_IO >> 9) & 0x1);//ZIN1
            LED_ZIN2.Value = Convert.ToBoolean((g_Ax2Hand_IO >> 10) & 0x1);//ZIN0
            LED_ZIN3.Value = Convert.ToBoolean((g_Ax2Hand_IO >> 11) & 0x1);//ZIN1

            LED_UIN0.Value = Convert.ToBoolean((g_Ax2Hand_IO >> 12) & 0x1);//UIN0
            LED_UIN1.Value = Convert.ToBoolean((g_Ax2Hand_IO >> 13) & 0x1);//UIN1
            LED_UIN2.Value = Convert.ToBoolean((g_Ax2Hand_IO >> 14) & 0x1);//UIN0
            LED_UIN3.Value = Convert.ToBoolean((g_Ax2Hand_IO >> 15) & 0x1);//UIN1


            ////////特殊IO////////   
            //////进料电机1
            LED_APOS.Value = Convert.ToBoolean((g_Ax1Hand_ONE_MIO >> 13) & 0x1);//POS
            MainMotor1Alarm.Value = !Convert.ToBoolean((g_Ax1Hand_ONE_MIO >> 1) & 0x1);//ALM
            LED_BPOS.Value = Convert.ToBoolean((g_Ax1Hand_TWO_MIO >> 13) & 0x1);//POS
            MainMotor2Alarm.Value = !Convert.ToBoolean((g_Ax1Hand_TWO_MIO >> 1) & 0x1);//ALM
            LED_CPOS.Value = Convert.ToBoolean((g_Ax1Hand_THREE_MIO >> 13) & 0x1);//POS
            MainMotor3Alarm.Value = !Convert.ToBoolean((g_Ax1Hand_THREE_MIO >> 1) & 0x1);//ALM
            //////进料buffer
            INBufferAlarm.Value = !Convert.ToBoolean((g_Ax2Hand_ONE_MIO >> 1) & 0x1);//ALM
            LED_PLimit1.Value = Convert.ToBoolean((g_Ax2Hand_ONE_MIO >> 2) & 0x1);//P+
            LED_NLimit1.Value = Convert.ToBoolean((g_Ax2Hand_ONE_MIO >> 3) & 0x1);//P-
            LED_Home1.Value = Convert.ToBoolean((g_Ax2Hand_ONE_MIO >> 4) & 0x1);//HOME
            LED_XPOS.Value = Convert.ToBoolean((g_Ax2Hand_ONE_MIO >> 13) & 0x1);//POS


            /////进料旋转电机            
            LED_YALM.Value = Convert.ToBoolean((g_Ax2Hand_TWO_MIO >> 1) & 0x1);//ALM
            LED_YHome.Value = Convert.ToBoolean((g_Ax2Hand_TWO_MIO >> 4) & 0x1);//HOME
            LED_YPOS.Value = Convert.ToBoolean((g_Ax2Hand_TWO_MIO >> 13) & 0x1);//POS
            /////出料旋转电机
            LED_ZALM.Value = Convert.ToBoolean((g_Ax2Hand_THREE_MIO >> 1) & 0x1);//ALM
            LED_ZHome.Value = Convert.ToBoolean((g_Ax2Hand_THREE_MIO >> 4) & 0x1);//HOME
            LED_ZPOS.Value = Convert.ToBoolean((g_Ax2Hand_THREE_MIO >> 13) & 0x1);//POS
            /////DD马达

            LED_UHome.Value = Convert.ToBoolean((g_Ax2Hand_FOUR_MIO >> 4) & 0x1);//HOME

            /* if ((XHomeFlag == 0) && (YHomeFlag == 0) && (ZHomeFlag == 0) && (UHomeFlag==0))
                  this.ControlBox = true;*/
            ////////////归零运动////////////////////////////////
            ///进料Buffer归零完成           
            switch (XHomeFlag)
            {
                case 1:
                    Motion.mAcm_AxGetState(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_ONE], ref GetState1);  //等待Buffer运动完成 
                    if (GetState1 == 3)
                    {
                        Motion.mAcm_AxResetError(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_ONE]);
                        System.Threading.Thread.Sleep(200);

                    }
                    else if (GetState1 == 1)
                    {
                        Motion.mAcm_AxHome(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_ONE], 0, 1);
                        XHomeFlag = 2;
                    }
                    break;
                case 2:
                    Motion.mAcm_AxGetState(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_ONE], ref GetState1);  //等待Buffer运动完成 
                    if (GetState1 == 1)
                    {
                        if (m_InBufferHomeCount < 5)
                            m_InBufferHomeCount++;
                        else
                        {
                            m_InBufferHomeCount = 0;
                            if ((LED_AIN0.Value == true) || (FormHMI.g_LoadBuffer == 0))//有硅片正处于buffer之下，归零运动完成
                                XHomeFlag = 3;
                            else
                            {
                                FormHMI.g_LoadBuffer--;
                                LoadBufferQ.Value = FormHMI.g_LoadBuffer;
                                Motion.mAcm_AxMoveRel(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_ONE], 5 * FormHMI.pci1245l.BufferRatio);
                            }
                        }
                    }
                    else
                        m_InBufferHomeCount = 0;
                    break;
                case 3:
                    Motion.mAcm_AxGetState(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_ONE], ref GetState1);  //等待Buffer运动完成 
                    if (GetState1 == 1)
                    {
                        Motion.mAcm_SetF64Property(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_ONE], (uint)PropertyID.PAR_AxVelLow, Bufferm_VelLow);
                        Motion.mAcm_SetF64Property(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_ONE], (uint)PropertyID.PAR_AxVelHigh, Bufferm_VelHigh);
                        XHomeFlag = 0;
                        Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 1);
                        System.Threading.Thread.Sleep(200);//延时200ms
                        Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 0);
                        MessageBox.Show(this, "主机“进料”Buffer模组已完成归零运动！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    break;
            }
            ///进料旋转臂归零完成
            switch (YHomeFlag)
            {
                case 1:
                    Motion.mAcm_AxGetState(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_TWO], ref GetState2);  //等待旋转运动完成 
                    if (GetState2 == 3)
                    {
                        Motion.mAcm_AxResetError(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_TWO]);
                        System.Threading.Thread.Sleep(200);

                    }
                    else if (GetState2 == 1)
                    {
                        Motion.mAcm_AxHome(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_TWO], 0, 1);
                        YHomeFlag = 2;
                    }
                    break;
                case 2:
                    Motion.mAcm_AxGetState(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_TWO], ref GetState2);  //等待Buffer运动完成 
                    if (GetState2 == 1)
                    {
                        Motion.mAcm_SetF64Property(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_TWO], (uint)PropertyID.PAR_AxVelLow, Rotatem_VelLow);
                        Motion.mAcm_SetF64Property(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_TWO], (uint)PropertyID.PAR_AxVelHigh, Rotatem_VelHigh);
                        YHomeFlag = 0;
                        Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 1);
                        System.Threading.Thread.Sleep(200);//延时200ms
                        Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 0);
                        MessageBox.Show(this, "进料旋转已完成归零运动！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FormHMI.g_InRotateMotorPosition = 0; FormHMI.g_InRotateHome = true;
                    }
                    break;
            }
            ///出料旋转臂归零完成
            switch (ZHomeFlag)
            {
                case 1:
                    Motion.mAcm_AxGetState(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_THREE], ref GetState3);  //等待Buffer运动完成 
                    if (GetState3 == 3)
                    {
                        Motion.mAcm_AxResetError(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_THREE]);
                        System.Threading.Thread.Sleep(200);

                    }
                    else if (GetState3 == 1)
                    {
                        Motion.mAcm_AxHome(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_THREE], 0, 1);
                        ZHomeFlag = 2;
                    }
                    break;
                case 2:
                    Motion.mAcm_AxGetState(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_THREE], ref GetState3);  //等待Buffer运动完成 
                    if (GetState3 == 1)
                    {
                        Motion.mAcm_SetF64Property(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_THREE], (uint)PropertyID.PAR_AxVelLow, Rotatem_VelLow);
                        Motion.mAcm_SetF64Property(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_THREE], (uint)PropertyID.PAR_AxVelHigh, Rotatem_VelHigh);
                        ZHomeFlag = 0;
                        Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 1);
                        System.Threading.Thread.Sleep(200);//延时200ms
                        Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 0);
                        MessageBox.Show(this, "出料旋转臂已完成归零运动！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FormHMI.g_OutRotateMotorPosition = 0;
                    }
                    break;
            }
            ///DD马达归零完成
            switch (m_DDHomeFlag)
            {
                case 1:
                    if (LED_UIN1.Value == false)//DD马达开始运动
                        m_DDHomeFlag = 2;
                    break;
                case 2:
                    if (LED_UIN1.Value == true)//DD马达运动完成
                    {
                        m_DDHomeFlag = 0;
                        Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 1);
                        System.Threading.Thread.Sleep(200);//延时200ms
                        Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 0);
                        MessageBox.Show(this, "主机DD马达已完成归零运动！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    break;
            }


        }
        #region ///气动及其他区域
        /*********************************************************************************************************
          ** 函数名称 ：fDim
          ** 函数功能 ：按钮使能控制 
          ** 修改时间 ：20170925
          ** 修改内容 ：
          *********************************************************************************************************/
        void fDim(bool data)
        {
            // KEY_BufferOutUp.Enabled = data;
            // KEY_BufferOutHome.Enabled = data;
            // KEY_BufferOutDown.Enabled = data;
            //DD台面
            //if (FormHMI.g_MainStep < 30)
            //{
            //    KEY_TableC.Visible = false;
            //    KEY_TableA.Visible = false;
            //    KEY_TableD.Visible = false;
            //    KEY_TableB.Visible = false;
            //}
            //else
            //{
            //switch (FormHMI.g_CWPositionFlag)
            //{
            //case 0:
            //    KEY_TableC.Visible = true;
            //    KEY_TableA.Visible = false;
            //    KEY_TableD.Visible = false;
            //    KEY_TableB.Visible = false;
            //    break;
            //case 1:
            //    KEY_TableC.Visible = false;
            //    KEY_TableA.Visible = false;
            //    KEY_TableD.Visible = false;
            //    KEY_TableB.Visible = true;
            //    break;
            //case 2:
            //    KEY_TableC.Visible = false;
            //    KEY_TableA.Visible = true;
            //    KEY_TableD.Visible = false;
            //    KEY_TableB.Visible = false;
            //    break;
            //case 3:
            //    KEY_TableC.Visible = false;
            //    KEY_TableA.Visible = false;
            //    KEY_TableD.Visible = true;
            //    KEY_TableB.Visible = false;
            //    break;
            //}
            // }

            //KEY_TableA.Enabled = data;
            //KEY_TableD.Enabled = data;
            //KEY_TableB.Enabled = data;
            //KEY_TableC.Enabled = data;
            // KEY_URunP.Enabled = data;
            // KEY_URunN.Enabled = data;

            //进料旋转臂气抓
            KEY_WaferUpBCapOut.Enabled = data;
            KEY_AOUT5.Enabled = data;
            KEY_WaferUpACapOut.Enabled = data;
            KEY_AOUT4.Enabled = data;
            KEY_InRotate.Enabled = data;
            KEY_InRotateHome.Enabled = data;

            //出料旋转臂气抓
            KEY_WaferDOWNACapOut.Enabled = data;
            KEY_BOUT4.Enabled = data;
            KEY_WaferDOWNBCapOut.Enabled = data;
            KEY_DOUT6.Enabled = data;
            KEY_OutRotateHome.Enabled = data;
            KEY_OutRotate.Enabled = data;
            // KEY_OutRotateP.Enabled = data;
            //KEY_OutRotateN.Enabled = data;

            //旋转气缸
            KEY_UnLoadRotateOnOff.Enabled = data;

            KEY_UHome.Enabled = data;
            KEY_URun.Enabled = data;
            KEY_BufferInUp.Enabled = data;
            KEY_BufferInHome.Enabled = data;
            KEY_BufferInDown.Enabled = data;

            KEY_MainMotor1.Enabled = data;
            KEY_MainMotor2.Enabled = data;
            KEY_MainMotor3.Enabled = data;

            KEY_MainMotorSyn1.Enabled = data;
            KEY_MainMotorSyn2.Enabled = data;

            KEY_TotalAirOut.Enabled = data;
            KEY_AirKnife.Enabled = data;
            if (FormPassword.g_Authority == 3)
            {
                KEY_OutRotateP.Enabled = true;
                KEY_OutRotateN.Enabled = true;
                KEY_URunP.Enabled = true;
                KEY_URunN.Enabled = true;
            }
            else
            {
                KEY_OutRotateP.Enabled = false;
                KEY_OutRotateN.Enabled = false;
                KEY_URunP.Enabled = false;
                KEY_URunN.Enabled = false;
            }
            //   KEY_BlowerControl.Enabled = data;
        }

        /*********************************************************************************************************
        ** 函数名称 ：KEY_WaferUpACapOut_StateChanged
        ** 函数功能 ：上料A吸附  
        ** 修改时间 ：20180209
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_WaferUpACapOut_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {
            Motion.mAcm_AxDoSetBit(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_TWO], 4, Convert.ToByte(KEY_WaferUpACapOut.Value));
        }
        /*********************************************************************************************************
         ** 函数名称 ：KEY_WaferUpBCapOut_StateChanged
         ** 函数功能 ：上料B吸附  
         ** 修改时间 ：20180209
         ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_WaferUpBCapOut_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {
            Motion.mAcm_AxDoSetBit(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_TWO], 5, Convert.ToByte(KEY_WaferUpBCapOut.Value));
        }

        /*********************************************************************************************************
        ** 函数名称 ：KEY_WaferDOWNACapOut_StateChanged
        ** 函数功能 ：下料A吸附  
        ** 修改时间 ：20180209
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_WaferDOWNACapOut_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {
            Motion.mAcm_AxDoSetBit(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_THREE], 4, Convert.ToByte(KEY_WaferDOWNACapOut.Value));
        }
        /*********************************************************************************************************
         ** 函数名称 ：KEY_WaferDOWNBCapOut_StateChanged
         ** 函数功能 ：下料B吸附  
         ** 修改时间 ：20180209
         ** 修改内容 ：
         *********************************************************************************************************/
        private void KEY_WaferDOWNBCapOut_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {
            Motion.mAcm_AxDoSetBit(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_THREE], 5, Convert.ToByte(KEY_WaferDOWNBCapOut.Value));
        }
        /*********************************************************************************************************
              ** 函数名称 ：TotalAirOut_StateChanged
              ** 函数功能 ：总气控制
              ** 修改时间 ：20170915
              ** 修改内容 ：
              *********************************************************************************************************/
        private void TotalAirOut_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {
            Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_ONE], 6, Convert.ToByte(KEY_TotalAirOut.Value));
        }
        /*********************************************************************************************************
        ** 函数名称 ：KEY_AirKnife_StateChanged
        ** 函数功能 ：风刀  
        ** 修改时间 ：20180209
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_AirKnife_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {
            Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_ONE], 7, Convert.ToByte(KEY_AirKnife.Value));
        }
        #endregion
        #region ///皮带电机运行区
        /*********************************************************************************************************
       ** 函数名称 ：MainMotor1_Click
       ** 函数功能 ：进料1单轴运动
       ** 修改时间 ：20180209
       ** 修改内容 ：
       *********************************************************************************************************/
        private void MainMotor1_Click(object sender, EventArgs e)
        {
            Motion.mAcm_AxMoveRel(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_ONE], 300 * FormHMI.pci1245l.WaferRatio);
        }
        /*********************************************************************************************************
       ** 函数名称 ：MainMotor2_Click
       ** 函数功能 ：进料2单轴运动
       ** 修改时间 ：20180209
       ** 修改内容 ：
       *********************************************************************************************************/
        private void MainMotor2_Click(object sender, EventArgs e)
        {
            Motion.mAcm_AxMoveRel(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_TWO], 300 * FormHMI.pci1245l.WaferRatio);
        }
        /*********************************************************************************************************
       ** 函数名称 ：MainMotor3_Click
       ** 函数功能 ：出料1单轴运动
       ** 修改时间 ：20170915
       ** 修改内容 ：
       *********************************************************************************************************/
        private void MainMotor3_Click(object sender, EventArgs e)
        {

            Motion.mAcm_AxMoveRel(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.g_SysParam[27] * FormHMI.pci1245l.WaferRatio);
        }
        // /*********************************************************************************************************
        //** 函数名称 ：MainMotor4_Click
        //** 函数功能 ：出料2单轴运动
        //** 修改时间 ：20170915
        //** 修改内容 ：
        //*********************************************************************************************************/
        // private void MainMotor4_Click(object sender, EventArgs e)
        // {
        //     Motion.mAcm_AxMoveRel(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_FOUR], 1500 * FormHMI.pci1245l.WaferRatio);
        // }
        /*********************************************************************************************************
      ** 函数名称 ：MainMotorSyn1_Click
      ** 函数功能 ：同步1运动
      ** 修改时间 ：20170915
      ** 修改内容 ：
      *********************************************************************************************************/
        private void MainMotorSyn1_Click(object sender, EventArgs e)
        {
            MNet.M1A._mnet_m1a_start_r_move(FormHMI.m_RingNoA, FormHMI.ClineAddr.LoadAX1IP, (int)(FormHMI.pci1245l.WaferRatio * FormHMI.g_SysParam[35]));//硅片传送电机跑300mm
            Motion.mAcm_AxMoveRel(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_ONE], 1500 * FormHMI.pci1245l.WaferRatio);

        }
        /*********************************************************************************************************
      ** 函数名称 ：MainMotorSyn2_Click
      ** 函数功能 ：同步2运动
      ** 修改时间 ：20170915
      ** 修改内容 ：
      *********************************************************************************************************/
        private void MainMotorSyn2_Click(object sender, EventArgs e)
        {
            Motion.mAcm_AxMoveRel(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_ONE], 400 * FormHMI.pci1245l.WaferRatio);
            Motion.mAcm_AxMoveRel(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_TWO], 1500 * FormHMI.pci1245l.WaferRatio);
        }

        #endregion
        #region ///进料buff区域
        /*********************************************************************************************************
        ** 函数名称 ：BufferInUp_Click
        ** 函数功能 ：进料buffer向下运动
        ** 修改时间 ：20180209
        ** 修改内容 ：
        *********************************************************************************************************/
        private void BufferInUp_Click(object sender, EventArgs e)
        {
            ushort GetState = 0;
            Motion.mAcm_AxGetState(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_ONE], ref GetState);  //等待Buffer运动完成 
            if (GetState == 1)
            {
                if (FormHMI.g_LoadBuffer > 0)
                {
                    FormHMI.g_LoadBuffer--;
                    Motion.mAcm_AxMoveRel(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_ONE], 5 * FormHMI.pci1245l.BufferRatio);

                }
            }

        }
        /*********************************************************************************************************
        ** 函数名称 ：BufferInDown_Click
        ** 函数功能 ：进料buffer向上运动
        ** 修改时间 ：20180209
        ** 修改内容 ：
      *********************************************************************************************************/
        private void BufferInDown_Click(object sender, EventArgs e)
        {
            ushort GetState = 0;
            Motion.mAcm_AxGetState(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_ONE], ref GetState);  //等待Buffer运动完成 
            if (GetState == 1)
            {
                if (FormHMI.g_LoadBuffer < FormHMI.g_InBufferTotal)
                {
                    FormHMI.g_LoadBuffer++;
                    Motion.mAcm_AxMoveRel(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_ONE], -5 * FormHMI.pci1245l.BufferRatio);

                }
            }
        }


        /*********************************************************************************************************
         ** 函数名称 ：BufferInHome_Click
         ** 函数功能 ：进料buffer归零
         ** 修改时间 ：20180209
         ** 修改内容 ：
         *********************************************************************************************************/
        private void BufferInHome_Click(object sender, EventArgs e)
        {
            DialogResult ResultTemp;
            double m_VelLow = FormHMI.pci1245l.BufferRatio * 5, m_VelHigh = FormHMI.pci1245l.BufferRatio * 20;
            Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 1);
            System.Threading.Thread.Sleep(200);//延时200ms
            Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 0);
            ResultTemp = MessageBox.Show(this, "请确认是否进行进料缓存电机归零运动！", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (ResultTemp == DialogResult.OK)   //表示点击确认        
            {
                Motion.mAcm_SetF64Property(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_ONE], (uint)PropertyID.PAR_AxVelLow, m_VelLow);
                Motion.mAcm_SetF64Property(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_ONE], (uint)PropertyID.PAR_AxVelHigh, m_VelHigh);
                Motion.mAcm_AxResetError(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_ONE]);
                System.Threading.Thread.Sleep(100);//延时100ms
                if (LED_Home1.Value)//如果已经在回零位置，先运动5mm，脱离回零点
                    Motion.mAcm_AxMoveRel(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_ONE], 5 * FormHMI.pci1245l.BufferRatio);
                XHomeFlag = 1; //this.ControlBox = false;
                FormHMI.g_LoadBuffer = FormHMI.g_InBufferTotal; LoadBufferQ.Value = FormHMI.g_InBufferTotal;
            }
        }
        #endregion

        #region ///旋转电机区
        /*********************************************************************************************************
        ** 函数名称 ：RotateInHome_Click
        ** 函数功能 ：进料旋转臂归零
        ** 修改时间 ：20180211
        ** 修改内容 ：
        *********************************************************************************************************/
        private void RotateInHome_Click(object sender, EventArgs e)
        {
            DialogResult ResultTemp;
            double m_VelLow = FormHMI.pci1245l.RotateRatio * 5, m_VelHigh = FormHMI.pci1245l.RotateRatio * 10;
            if (LED_YPOS.Value == false)  //旋转臂没有送电
            {
                Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 1);
                System.Threading.Thread.Sleep(200);//延时200ms
                Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 0);
                MessageBox.Show(this, "进料旋转臂没有送电或者处于异常状态，请检查！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (LED_BIN3.Value == false)
            {
                MessageBox.Show(this, "主机安全门未关闭到位，请关闭到位后，再操作！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 1);
                System.Threading.Thread.Sleep(200);//延时200ms
                Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 0);
                ResultTemp = MessageBox.Show(this, "请确认是否进行进料旋转臂电机归零运动！", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (ResultTemp == DialogResult.OK)   //表示点击确认        
                {
                    Motion.mAcm_SetF64Property(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_TWO], (uint)PropertyID.PAR_AxVelLow, m_VelLow);
                    Motion.mAcm_SetF64Property(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_TWO], (uint)PropertyID.PAR_AxVelHigh, m_VelHigh);
                    //  if (LED_YHome.Value)
                    // {
                    Motion.mAcm_AxMoveRel(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_TWO], 20 * FormHMI.pci1245l.RotateRatio);
                    YHomeFlag = 1;// this.ControlBox = false;
                    //  }
                    //  else
                    //  {
                    //    Motion.mAcm_AxHome(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_TWO], 0, 1);
                    //     YHomeFlag = 2; //this.ControlBox = false;
                    // }
                }
            }
        }
        /*********************************************************************************************************
        ** 函数名称 ：KEY_InRotate_Click
        ** 函数功能 ：进料旋转臂运动
        ** 修改时间 ：20180212
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_InRotate_Click(object sender, EventArgs e)
        {
            ushort GetState = 0;
            Motion.mAcm_AxGetState(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_TWO], ref GetState);

            DialogResult ResultTemp;
            if ((LED_YPOS.Value == false) || (GetState != 1))  //旋转臂没有送电
            {
                // m_ErrorFlag = 2;
                MessageBox.Show(this, "旋转臂没有送电或者处于异常状态或者没运动完，请检查！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            else if (LED_BIN3.Value == false)
            {
                MessageBox.Show(this, "主机安全门未关闭到位，请关闭到位后，再操作！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ResultTemp = MessageBox.Show(this, "请确认是否进行旋转臂旋转运动？", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (ResultTemp == DialogResult.OK)   //表示点击确认
                {
                    if (FormHMI.g_InRotateMotorPosition == 0)    //0度位置
                    {
                        Motion.mAcm_AxMoveAbs(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_TWO], 180 * FormHMI.pci1245l.RotateRatio);
                        FormHMI.g_InRotateMotorPosition = 1;
                        return;
                    }
                    if (FormHMI.g_InRotateMotorPosition == 1)
                    {
                        Motion.mAcm_AxMoveAbs(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_TWO], 0);
                        FormHMI.g_InRotateMotorPosition = 0;
                        return;
                    }

                }
            }
        }
        /*********************************************************************************************************
        ** 函数名称 ：RotateOutHome_Click
        ** 函数功能 ：出料旋转臂归零
        ** 修改时间 ：20180211
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_OutRotateHome_Click(object sender, EventArgs e)
        {
            DialogResult ResultTemp;
            double m_VelLow = FormHMI.pci1245l.RotateRatio * 5, m_VelHigh = FormHMI.pci1245l.RotateRatio * 10;
            if (LED_ZPOS.Value == false)  //旋转臂没有送电
            {
                Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 1);
                System.Threading.Thread.Sleep(200);//延时200ms
                Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 0);
                MessageBox.Show(this, "旋转臂没有送电或者处于异常状态，请检查！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (LED_BIN3.Value == false)
            {
                MessageBox.Show(this, "主机安全门未关闭到位，请关闭到位后，再操作！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 1);
                System.Threading.Thread.Sleep(200);//延时200ms
                Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 0);
                ResultTemp = MessageBox.Show(this, "请确认是否进行旋转臂电机归零运动！", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (ResultTemp == DialogResult.OK)   //表示点击确认        
                {
                    Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_TWO], 6, 0);//  下料旋转气缸回原位
                    Motion.mAcm_SetF64Property(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_THREE], (uint)PropertyID.PAR_AxVelLow, m_VelLow);
                    Motion.mAcm_SetF64Property(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_THREE], (uint)PropertyID.PAR_AxVelHigh, m_VelHigh);
                    if (LED_ZHome.Value)
                    {
                        Motion.mAcm_AxMoveRel(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_THREE], 10 * FormHMI.pci1245l.RotateRatio);
                        ZHomeFlag = 1;// this.ControlBox = false;
                    }
                    else
                    {
                        Motion.mAcm_AxHome(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_THREE], 0, 1);
                        ZHomeFlag = 2; //this.ControlBox = false;
                    }
                }
            }
        }
        /*********************************************************************************************************
        ** 函数名称 ：KEY_OutRotate_Click
        ** 函数功能 ：出料旋转臂运动
        ** 修改时间 ：20180212
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_OutRotate_Click(object sender, EventArgs e)
        {
            ushort GetState = 0;
            Motion.mAcm_AxGetState(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_THREE], ref GetState);
            DialogResult ResultTemp;
            if ((LED_ZPOS.Value == false) || (GetState != 1))  //旋转臂没有送电
            {
                // m_ErrorFlag = 2;
                MessageBox.Show(this, "旋转臂没有送电或者处于异常状态或者没运动完，请检查！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            else if (LED_BIN3.Value == false)
            {
                MessageBox.Show(this, "主机安全门未关闭到位，请关闭到位后，再操作！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ResultTemp = MessageBox.Show(this, "请确认是否进行旋转臂旋转运动？", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (ResultTemp == DialogResult.OK)   //表示点击确认
                {
                    OutChangeData(true);

                }
            }
        }
        #endregion
        #region ///DD马达区域
        /*********************************************************************************************************
         ** 函数名称 ：DDHome_Click
         ** 函数功能 ：DD归零
         ** 修改时间 ：20170915
         ** 修改内容 ：
         *********************************************************************************************************/
        private void KEY_UHome_Click(object sender, EventArgs e)
        {
            DialogResult ResultTemp;

            //if (LED_UIN1.Value == false)  //DD马达没有送电
            //{
            //    Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 1);
            //    System.Threading.Thread.Sleep(200);//延时200ms
            //    Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 0);
            //    MessageBox.Show(this, "DD马达送电或者处于异常状态，请检查！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //else
            //{
            if (LED_BIN3.Value == false)
            {
                MessageBox.Show(this, "主机安全门未关闭到位，请关闭到位后，再操作！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 1);
            System.Threading.Thread.Sleep(200);//延时200ms
            Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], FormHMI.pci1245l.BuzzerOut, 0);
            ResultTemp = MessageBox.Show(this, "请确认是否进行DD马达归零运动！", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (ResultTemp == DialogResult.OK)   //表示点击确认        
            {
                Motion.mAcm_AxDoSetBit(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_FOUR], 7, 0);    //启动关闭
                Motion.mAcm_AxDoSetBit(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_FOUR], 4, 0);    //M0
                Motion.mAcm_AxDoSetBit(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_FOUR], 5, 0);    //M1
                Motion.mAcm_AxDoSetBit(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_THREE], 6, 0);   //M2

                Motion.mAcm_AxDoSetBit(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_FOUR], 4, 1);    //M0
                Motion.mAcm_AxDoSetBit(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_FOUR], 5, 0);    //M1
                Motion.mAcm_AxDoSetBit(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_THREE], 6, 0);   //M2
                System.Threading.Thread.Sleep(20);//延时2ms
                Motion.mAcm_AxDoSetBit(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_FOUR], 7, 1);//启动打开
                m_DDHomeFlag = 1; FormHMI.g_CWPositionFlag = 0;// this.ControlBox = false;

            }
            //  }
        }
        /*********************************************************************************************************
        ** 函数名称 ：KEY_TableA_StateChanged
        ** 函数功能 ：B台面电磁阀
        ** 修改时间 ：20180209
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_TableA_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {

            Motion.mAcm_AxDoSetBit(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_ONE], 4, Convert.ToByte(KEY_TableA.Value));

        }
        /*********************************************************************************************************
       ** 函数名称 ：KEY_TableB_StateChanged
       ** 函数功能 ：A台面电磁阀
       ** 修改时间 ：20180209
       ** 修改内容 ：
       *********************************************************************************************************/
        private void KEY_TableB_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {

            Motion.mAcm_AxDoSetBit(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_ONE], 5, Convert.ToByte(KEY_TableB.Value));
            System.Threading.Thread.Sleep(200);//延时200ms
        }
        /*********************************************************************************************************
       ** 函数名称 ：KEY_TableC_StateChanged
       ** 函数功能 ：D台面电磁阀
       ** 修改时间 ：20180209
       ** 修改内容 ：
       *********************************************************************************************************/
        private void KEY_TableC_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {
            Motion.mAcm_AxDoSetBit(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_ONE], 6, Convert.ToByte(KEY_TableC.Value));
        }
        /*********************************************************************************************************
       ** 函数名称 ：KEY_TableD_StateChanged
       ** 函数功能 ：C台面电磁阀
       ** 修改时间 ：20180209
       ** 修改内容 ：
       *********************************************************************************************************/
        private void KEY_TableD_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {

            Motion.mAcm_AxDoSetBit(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_ONE], 7, Convert.ToByte(KEY_TableD.Value));
        }
        /*********************************************************************************************************
            ** 函数名称 ：KEY_URun_Click
            ** 函数功能 ：DD运动
            ** 修改时间 ：20171222
            ** 修改内容 ：
            *********************************************************************************************************/
        private void KEY_URun_Click(object sender, EventArgs e)
        {
            DialogResult ResultTemp;
            if (LED_UIN1.Value == false)  //DD马达没有送电
            {
                MessageBox.Show(this, "DD马达没有送电或者处于异常状态，请检查！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else if (LED_BIN3.Value == false)
            {
                MessageBox.Show(this, "主机安全门未关闭到位，请关闭到位后，再操作！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ResultTemp = MessageBox.Show(this, "请确认是否进行DD马达“旋转”运动！", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (ResultTemp == DialogResult.OK)   //表示点击确认   
                {
                    Motion.mAcm_AxDoSetBit(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_FOUR], 7, 0);   //启动关闭
                    System.Threading.Thread.Sleep(100);//延时100ms
                    /* ++FormHMI.g_CWPositionFlag;
                     if (FormHMI. g_CWPositionFlag> 1)
                         FormHMI.g_CWPositionFlag = 0;*/
                    // this.parent.fDDPositionStatus(FormHMI.g_CWPositionFlag);
                    ChangeData(true);

                }
            }
        }
        #endregion
        #region ///气缸动作区
        /*********************************************************************************************************
        ** 函数名称 ：KEY_UnLoadRotateOnOff_StateChanged
        ** 函数功能 ：下料旋转气缸
        ** 修改时间 ：20180209
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_UnLoadRotateOnOff_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {

            if (LED_BIN3.Value == false)
            {
                MessageBox.Show(this, "主机安全门未关闭到位，请关闭到位后，再操作！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_TWO], 6, Convert.ToByte(KEY_UnLoadRotateOnOff.Value));
        }

        #endregion
        /*********************************************************************************************************
        ** 函数名称 ：KEY_AOUT4_StateChanged
        ** 函数功能 ：上料旋转臂B吸盘反吹
        ** 修改时间 ：20180309
        ** 修改内容 ：
        *********************************************************************************************************/
        private void KEY_AOUT4_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {
            Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_ONE], 4, Convert.ToByte(KEY_AOUT4.Value));
        }
        /*********************************************************************************************************
       ** 函数名称 ：KEY_AOUT5_StateChanged
       ** 函数功能 ：上料旋转臂A吸盘反吹
       ** 修改时间 ：20180309
       ** 修改内容 ：
       *********************************************************************************************************/
        private void KEY_AOUT5_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {
            Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_ONE], 5, Convert.ToByte(KEY_AOUT5.Value));
        }
        /*********************************************************************************************************
      ** 函数名称 ：KEY_BOUT4_StateChanged
      ** 函数功能 ：下料旋转臂A吸盘反吹
      ** 修改时间 ：20180309
      ** 修改内容 ：
      *********************************************************************************************************/
        private void KEY_BOUT4_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {
            Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_TWO], 4, Convert.ToByte(KEY_BOUT4.Value));
        }
        /*********************************************************************************************************
      ** 函数名称 ：KEY_DOUT6_StateChanged
      ** 函数功能 ：下料旋转臂B吸盘反吹
      ** 修改时间 ：20180309
      ** 修改内容 ：
      *********************************************************************************************************/
        private void KEY_DOUT6_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {
            Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_FOUR], 6, Convert.ToByte(KEY_DOUT6.Value));
        }

        private void textBoxA_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)//出料旋转臂正转45度
        {
            ushort GetState = 0;
            Motion.mAcm_AxGetState(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_THREE], ref GetState);
            DialogResult ResultTemp;
            if ((LED_ZPOS.Value == false) || (GetState != 1))  //旋转臂没有送电
            {
                // m_ErrorFlag = 2;
                MessageBox.Show(this, "旋转臂没有送电或者处于异常状态或者没运动完，请检查！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            else if (LED_BIN3.Value == false)
            {
                MessageBox.Show(this, "主机安全门未关闭到位，请关闭到位后，再操作！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ResultTemp = MessageBox.Show(this, "请确认是否进行旋转臂正转45度？", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (ResultTemp == DialogResult.OK)   //表示点击确认
                {
                    OutChangeData1(true);

                }
            }
        }

        private void KEY_OutRotateN_Click(object sender, EventArgs e)
        {
            ushort GetState = 0;
            Motion.mAcm_AxGetState(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_THREE], ref GetState);
            DialogResult ResultTemp;
            if ((LED_ZPOS.Value == false) || (GetState != 1))  //旋转臂没有送电
            {
                // m_ErrorFlag = 2;
                MessageBox.Show(this, "旋转臂没有送电或者处于异常状态或者没运动完，请检查！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            else if (LED_BIN3.Value == false)
            {
                MessageBox.Show(this, "主机安全门未关闭到位，请关闭到位后，再操作！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ResultTemp = MessageBox.Show(this, "请确认是否进行旋转臂反转45度？", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (ResultTemp == DialogResult.OK)   //表示点击确认
                {
                    OutChangeData2(true);

                }
            }
        }

        private void label26_Click(object sender, EventArgs e)
        {

        }

        private void KEY_URunP_Click(object sender, EventArgs e)
        {
            DialogResult ResultTemp;
            if (LED_UIN1.Value == false)  //DD马达没有送电
            {
                MessageBox.Show(this, "DD马达没有送电或者处于异常状态，请检查！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else if (LED_BIN3.Value == false)
            {
                MessageBox.Show(this, "主机安全门未关闭到位，请关闭到位后，再操作！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ResultTemp = MessageBox.Show(this, "请确认是否进行DD马达“正转30度”运动！", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (ResultTemp == DialogResult.OK)   //表示点击确认   
                {
                    Motion.mAcm_AxDoSetBit(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_FOUR], 7, 0);   //启动关闭
                    System.Threading.Thread.Sleep(100);//延时100ms                   
                    ChangeData1(true);

                }
            }
        }

        private void KEY_URunN_Click(object sender, EventArgs e)
        {
            DialogResult ResultTemp;
            if (LED_UIN1.Value == false)  //DD马达没有送电
            {
                MessageBox.Show(this, "DD马达没有送电或者处于异常状态，请检查！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else if (LED_BIN3.Value == false)
            {
                MessageBox.Show(this, "主机安全门未关闭到位，请关闭到位后，再操作！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ResultTemp = MessageBox.Show(this, "请确认是否进行DD马达“正转60度”运动！", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (ResultTemp == DialogResult.OK)   //表示点击确认   
                {
                    Motion.mAcm_AxDoSetBit(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_FOUR], 7, 0);   //启动关闭
                    System.Threading.Thread.Sleep(100);//延时100ms                   
                    ChangeData2(true);
                }
            }
        }

        private void KEY_Dust_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {
            Motion.mAcm_AxDoSetBit(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_TWO], 5, Convert.ToByte(KEY_Dust.Value));
        }


    }
}
