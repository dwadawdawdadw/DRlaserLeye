using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Advantech.Motion;
namespace WindowsApplication1
{
    public delegate void ChangeMPFormData(bool topGetState4);

    public partial class MainParam : Form
    {
        public event ChangeMPFormData ChangeData;
        // private FormHMI parent=new FormHMI();
        // private FormHMI parent=new FormHMI();
        public MainParam()//FormHMI parent)       
        {
            InitializeComponent();
            InMotor1Offset.Value = FormHMI.g_SysParam[26];
            InMotor2Offset.Value = FormHMI.g_SysParam[10];
            OutMotor1Offset.Value = FormHMI.g_SysParam[27];

            InBufferOffset.Value = FormHMI.g_SysParam[14];
            InRotateOffset.Value = FormHMI.g_SysParam[15];
            OutRotateOffset.Value = FormHMI.g_SysParam[16];
            OutBufferOffset.Value = FormHMI.g_SysParam[32];
            OutRotateAngle.Value = FormHMI.g_SysParam[45];
            InReleaseDelay.Value = FormHMI.g_SysParam[49];
            OutReleaseDelay.Value = FormHMI.g_SysParam[48];//下料反吹时间
            ////CCD补偿
            WaferStand.Value = FormHMI.g_SysParam[36];
            ShadowStand.Value = FormHMI.g_SysParam[37];
            CCDNGArea.Value = FormHMI.g_SysParam[13];

            CCDBaseX.Value = FormHMI.g_SysParam[7];
            CCDBaseY.Value = FormHMI.g_SysParam[8];
            CCDBaseAng.Value = FormHMI.g_SysParam[34];
            CCDdelay.Value = FormHMI.g_SysParam[46];
            CCDAlarmValue.Value = FormHMI.g_SysParam[47];
            CCDAlarmAngleValue.Value = FormHMI.g_SysParam[51];//CCD角度报警值
            //   FormHMI.g_SysParam[41] = FormHMI.g_CCDSource[0];
            //   FormHMI.g_SysParam[42] = FormHMI.g_CCDSource[1];
            PowerInv.Value = FormHMI.g_SysParam[17];
            PowerMin.Value = FormHMI.g_SysParam[18];
            PowerMax.Value = FormHMI.g_SysParam[19];
            PowerRatio.Value = FormHMI.g_SysParam[33];
            BUFFERNum.Value = FormHMI.g_SysParam[68];
            MAccDecc.Value = FormHMI.m_MAccDecc;
            LongOutDelay.Value = FormHMI.g_SysParam[72];
            RotateCylindDelay.Value = FormHMI.g_SysParam[77];  //旋转气缸延时
            if (FormHMI.m_DisableUnLoadRotateCylinder == 1)
            {
                DisableUnLoadRotateCylinder.Checked = true;
            }
            else
            {
                DisableUnLoadRotateCylinder.Checked = false;
            }

            if (FormHMI.m_DustCrack == 1)
            {
                DustCrack.Checked = true;
            }
            else
            {
                DustCrack.Checked = false;
            }
            if (FormHMI.g_MotorAlign == 0)
                MotorAlign.Checked = false;
            else
                MotorAlign.Checked = true;

            if (FormHMI.g_CyAlign == 0)
                CyAlign.Checked = false;
            else
                CyAlign.Checked = true;

            if (FormHMI.g_MainStep < 30)
            {
                SetOffsetDistance.Visible = false;
                SetCCDArea.Visible = false;
                SetCCDBase.Visible = false;
                PowerSet.Visible = false;

            }
            else
            {
                SetOffsetDistance.Visible = true;
                SetCCDArea.Visible = true;
                SetCCDBase.Visible = true;
                PowerSet.Visible = true;
            }
            switch (FormPassword.g_Authority)
            {
                case 0:
                case 1:
                    SetOffsetDistance.Enabled = false;
                    SetCCDArea.Enabled = false;
                    SetCCDBase.Enabled = false;
                    PowerSet.Enabled = false;
                    groupBox4.Visible = false;
                    break;
                case 2:
                    SetOffsetDistance.Enabled = true;
                    SetCCDArea.Enabled = true;
                    SetCCDBase.Enabled = true;
                    PowerSet.Enabled = true;
                    break;
                case 3:
                    SetOffsetDistance.Enabled = true;
                    SetCCDArea.Enabled = true;
                    SetCCDBase.Enabled = true;
                    PowerSet.Enabled = true;
                    groupBox4.Visible = true;
                    break;
            }

        }
        /*********************************************************************************************************
        ** 函数名称 ：SetOffsetDistance_Click
        ** 函数功能 ：运动轴补偿参数设置
        ** 修改时间 ：20170921
        ** 修改内容 ：
        *********************************************************************************************************/
        private void SetOffsetDistance_Click(object sender, EventArgs e)
        {
            double TempDistanceOneAxis, TempDistanceTwoAxis, TempDistanceThreeAxis;
            double TempBufferInOffset, TempOutRotateOffset, TempInRotateOffset, TempDistanceUAxis;

            DialogResult ResultTemp;
            int i;
            string sysp;

            ResultTemp = MessageBox.Show(this, "不是必要情况下，请勿修改参数，请确认是否需要进行参数修改？", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (ResultTemp == DialogResult.OK)   //表示点击确认        
            {
                FormHMI.g_SysParam[26] = InMotor1Offset.Value;
                FormHMI.g_SysParam[10] = InMotor2Offset.Value;
                FormHMI.g_SysParam[27] = OutMotor1Offset.Value;
                FormHMI.g_SysParam[14] = InBufferOffset.Value;
                FormHMI.g_SysParam[15] = InRotateOffset.Value;
                FormHMI.g_SysParam[16] = OutRotateOffset.Value;
                FormHMI.g_SysParam[32] = OutBufferOffset.Value;
                FormHMI.g_SysParam[45] = OutRotateAngle.Value;
                FormHMI.g_SysParam[68] = BUFFERNum.Value;
                FormHMI.g_SysParam[70] = Convert.ToDouble(FormHMI.m_DisableUnLoadRotateCylinder);
                FormHMI.g_SysParam[71] = MAccDecc.Value;
                FormHMI.g_SysParam[72] = LongOutDelay.Value;
                FormHMI.g_SysParam[77] = RotateCylindDelay.Value;   //旋转气缸延时

                if (DustCrack.Checked == true)
                {
                    FormHMI.g_SysParam[73] = 1;
                }
                else
                {
                    FormHMI.g_SysParam[73] = 0;
                }
                if (MotorAlign.Checked == false)    //夹持电机
                    FormHMI.g_SysParam[74] = 0;
                else
                    FormHMI.g_SysParam[74] = 1;
                if (CyAlign.Checked == true)
                {
                    FormHMI.g_SysParam[75] = 1;
                }
                else
                {
                    FormHMI.g_SysParam[75] = 0;
                }

                StreamWriter write = new StreamWriter("D:\\DRLaser\\SysPara.ini");
                for (i = 0; i < 80; i++)
                {
                    sysp = Convert.ToString(FormHMI.g_SysParam[i]);
                    write.WriteLine(sysp);
                }
                write.Close();
                ChangeData(true);///参数设置到各变量
                // this.parent.fSysParamAssignment();///参数设置到各变量
                TempDistanceOneAxis = InMotor1Offset.Value * FormHMI.pci1245l.WaferRatio;
                TempDistanceTwoAxis = InMotor2Offset.Value * FormHMI.pci1245l.WaferRatio;
                TempDistanceThreeAxis = OutMotor1Offset.Value * FormHMI.pci1245l.WaferRatio;
                TempBufferInOffset = InBufferOffset.Value * FormHMI.pci1245l.BufferRatio;
                TempInRotateOffset = InRotateOffset.Value * FormHMI.pci1245l.RotateRatio;
                TempOutRotateOffset = OutRotateOffset.Value * FormHMI.pci1245l.RotateRatio;
                TempDistanceUAxis = OutBufferOffset.Value * FormHMI.pci1245l.DDRatio;

                Motion.mAcm_SetU32Property(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_ONE], (uint)PropertyID.CFG_AxIN2StopEdge, 0);
                Motion.mAcm_SetF64Property(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_ONE], (uint)PropertyID.CFG_AxIN2StopReact, 1);
                Motion.mAcm_SetF64Property(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_ONE], (uint)PropertyID.CFG_AxIN2OffsetValue, TempDistanceOneAxis);

                Motion.mAcm_SetU32Property(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_TWO], (uint)PropertyID.CFG_AxIN2StopEdge, 0);
                Motion.mAcm_SetF64Property(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_TWO], (uint)PropertyID.CFG_AxIN2StopReact, 1);
                Motion.mAcm_SetF64Property(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_TWO], (uint)PropertyID.CFG_AxIN2OffsetValue, TempDistanceTwoAxis);

                //Motion.mAcm_SetU32Property(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], (uint)PropertyID.CFG_AxIN1StopEdge, 0);
                //Motion.mAcm_SetF64Property(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], (uint)PropertyID.CFG_AxIN1StopReact, 1);
                //Motion.mAcm_SetF64Property(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], (uint)PropertyID.CFG_AxIN1OffsetValue, TempDistanceThreeAxis);

                //uint B1, A1, C1;
                //B1 = Motion.mAcm_SetU32Property(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], (uint)PropertyID.CFG_AxIN2StopEdge, 0);
                //A1 = Motion.mAcm_SetF64Property(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], (uint)PropertyID.CFG_AxIN2StopReact, 1);
                //C1 = Motion.mAcm_SetF64Property(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], (uint)PropertyID.CFG_AxIN2OffsetValue, TempDistanceThreeAxis);

                Motion.mAcm_SetF64Property(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_ONE], (uint)PropertyID.CFG_AxHomeOffsetDistance, TempBufferInOffset);
                Motion.mAcm_SetF64Property(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_TWO], (uint)PropertyID.CFG_AxHomeOffsetDistance, TempInRotateOffset);
                Motion.mAcm_SetF64Property(FormHMI.ax2Handle[FormHMI.pci1245l.Axis_THREE], (uint)PropertyID.CFG_AxHomeOffsetDistance, TempOutRotateOffset);
                Motion.mAcm_SetF64Property(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_FOUR], (uint)PropertyID.CFG_AxHomeOffsetDistance, TempDistanceUAxis);

                double WaferAxisAcc = FormHMI.pci1245l.WaferRatio * FormHMI.pci1245l.WaferSpeed * FormHMI.m_MAccDecc;
                double WaferAxisDec = FormHMI.pci1245l.WaferRatio * FormHMI.pci1245l.WaferSpeed * FormHMI.m_MAccDecc;

                ///定义加减速	
                Motion.mAcm_SetF64Property(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_ONE], (uint)PropertyID.PAR_AxAcc, WaferAxisAcc);
                Motion.mAcm_SetF64Property(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_ONE], (uint)PropertyID.PAR_AxDec, WaferAxisDec);
                Motion.mAcm_SetF64Property(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_TWO], (uint)PropertyID.PAR_AxAcc, WaferAxisAcc);
                Motion.mAcm_SetF64Property(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_TWO], (uint)PropertyID.PAR_AxDec, WaferAxisDec);
                Motion.mAcm_SetF64Property(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], (uint)PropertyID.PAR_AxAcc, WaferAxisAcc);
                Motion.mAcm_SetF64Property(FormHMI.ax1Handle[FormHMI.pci1245l.Axis_THREE], (uint)PropertyID.PAR_AxDec, WaferAxisDec);
            }
        }
        /*********************************************************************************************************
        ** 函数名称 ：SetCCDArea_Click
        ** 函数功能 ：CCD遮挡补偿
        ** 修改时间 ：20170921
        ** 修改内容 ：
        *********************************************************************************************************/
        private void SetCCDArea_Click(object sender, EventArgs e)
        {

            DialogResult ResultTemp;
            int i;
            string sysp;

            ResultTemp = MessageBox.Show(this, "不是必要情况下，请勿修改参数，请确认是否需要进行参数修改？", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (ResultTemp == DialogResult.OK)   //表示点击确认        
            {
                FormHMI.g_SysParam[36] = WaferStand.Value;
                FormHMI.g_SysParam[37] = ShadowStand.Value;
                FormHMI.g_SysParam[13] = CCDNGArea.Value;
                StreamWriter write = new StreamWriter("D:\\DRLaser\\SysPara.ini");
                for (i = 0; i < 80; i++)
                {
                    sysp = Convert.ToString(FormHMI.g_SysParam[i]);
                    write.WriteLine(sysp);
                }
                write.Close();
                ChangeData(true);///参数设置到各变量

            }
        }
        /*********************************************************************************************************
               ** 函数名称 ：SetCCDBase_Click
               ** 函数功能 ：CCD基准值
               ** 修改时间 ：20170921
               ** 修改内容 ：
               *********************************************************************************************************/
        private void SetCCDBase_Click(object sender, EventArgs e)
        {

            DialogResult ResultTemp;
            int i;
            string sysp;

            ResultTemp = MessageBox.Show(this, "不是必要情况下，请勿修改参数，请确认是否需要进行参数修改？", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (ResultTemp == DialogResult.OK)   //表示点击确认        
            {
                FormHMI.g_SysParam[7] = CCDBaseX.Value;
                FormHMI.g_SysParam[8] = CCDBaseY.Value;
                FormHMI.g_SysParam[34] = CCDBaseAng.Value;
                FormHMI.g_SysParam[41] = FormHMI.g_CCDSource[0];
                FormHMI.g_SysParam[42] = FormHMI.g_CCDSource[1];

                //FormHMI.g_SysParam[47] = CCDAlarmValue.Value;
                StreamWriter write = new StreamWriter("D:\\DRLaser\\SysPara.ini");
                for (i = 0; i < 80; i++)
                {
                    sysp = Convert.ToString(FormHMI.g_SysParam[i]);
                    write.WriteLine(sysp);
                }
                write.Close();
                ChangeData(true);///参数设置到各变量

            }
        }
        /*********************************************************************************************************
               ** 函数名称 ：PowerSet_Click
               ** 函数功能 ：自动化功率测试参数
               ** 修改时间 ：20170921
               ** 修改内容 ：
               *********************************************************************************************************/
        private void PowerSet_Click(object sender, EventArgs e)
        {
            DialogResult ResultTemp;
            int i;
            string sysp;

            ResultTemp = MessageBox.Show(this, "不是必要情况下，请勿修改参数，请确认是否需要进行参数修改？", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (ResultTemp == DialogResult.OK)   //表示点击确认        
            {
                FormHMI.g_SysParam[17] = PowerInv.Value;
                FormHMI.g_SysParam[18] = PowerMin.Value;
                FormHMI.g_SysParam[19] = PowerMax.Value;
                StreamWriter write = new StreamWriter("D:\\DRLaser\\SysPara.ini");
                for (i = 0; i < 80; i++)
                {
                    sysp = Convert.ToString(FormHMI.g_SysParam[i]);
                    write.WriteLine(sysp);
                }
                write.Close();
                ChangeData(true);///参数设置到各变量
            }
        }
        /*********************************************************************************************************
               ** 函数名称 ：SetPowerRatio_Click
               ** 函数功能 ：设置功率系数
               ** 修改时间 ：20171013
               ** 修改内容 ：
               *********************************************************************************************************/
        private void SetPowerRatio_Click(object sender, EventArgs e)
        {
            DialogResult ResultTemp;
            int i;
            string sysp;

            ResultTemp = MessageBox.Show(this, "不是必要情况下，请勿修改参数，请确认是否需要进行参数修改？", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (ResultTemp == DialogResult.OK)   //表示点击确认        
            {
                FormHMI.g_SysParam[33] = PowerRatio.Value;
                FormHMI.g_SysParam[49] = InReleaseDelay.Value;
                FormHMI.g_SysParam[48] = OutReleaseDelay.Value;
                FormHMI.g_SysParam[46] = CCDdelay.Value;
                FormHMI.g_SysParam[47] = CCDAlarmValue.Value;
                FormHMI.g_SysParam[51] = CCDAlarmAngleValue.Value;
                StreamWriter write = new StreamWriter("D:\\DRLaser\\SysPara.ini");
                for (i = 0; i < 80; i++)
                {
                    sysp = Convert.ToString(FormHMI.g_SysParam[i]);
                    write.WriteLine(sysp);
                }
                write.Close();
                ChangeData(true);///参数设置到各变量
            }
        }

        private void DDMotoOffset_AfterChangeValue(object sender, NationalInstruments.UI.AfterChangeNumericValueEventArgs e)
        {

        }

        private void DisableUnLoadRotateCylinder_CheckedChanged(object sender, EventArgs e)
        {
            if (DisableUnLoadRotateCylinder.Checked == true)
            {
                FormHMI.m_DisableUnLoadRotateCylinder = 1;
            }
            else
            {
                FormHMI.m_DisableUnLoadRotateCylinder = 0;
            }
        }

    }
}
