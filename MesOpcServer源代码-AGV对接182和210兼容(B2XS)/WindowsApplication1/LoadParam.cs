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

namespace WindowsApplication1
{
    public delegate void ChangeLPFormData(bool topGetState4);
   
    public partial class LoadParam : Form
    {
        public event ChangeLPFormData ChangeData;     
        
         public LoadParam()//FormHMI parent)     
         {
            InitializeComponent();
            StartPos.Value = FormHMI.g_SysParam[3];
            TotalWafer.Value = FormHMI.g_SysParam[28];
            BoxInv.Value = FormHMI.g_SysParam[5];
            BigBoxInv.Value = FormHMI.g_SysParam[29];
            InBox.Value = FormHMI.g_SysParam[20];
            OutBox.Value = FormHMI.g_SysParam[21];
            ToughDis.Value = FormHMI.g_SysParam[35];
            FirstWaferPos.Value = FormHMI.g_SysParam[38];
            BoxKeyInDelay.Value = Convert.ToUInt32(FormHMI.g_SysParam[67]);
            AlignPos.Value = FormHMI.g_SysParam[76];    //花篮打齐位置
            if (FormHMI.g_MainStep < 30)
                SetParam.Visible = false;
            else
                SetParam.Visible = true;
            switch (FormPassword.g_Authority)
            {
                case 0:
                case 1:
                    SetParam.Enabled = false;
                    break;
                case 2:
                    SetParam.Enabled = true;
                    break;
                case 3:
                    SetParam.Enabled = true;
                    break;
            }

         }
        
         /*********************************************************************************************************
         ** 函数名称 ：SetParam_Click
         ** 函数功能 ：上料接驳台参数设置
         ** 修改时间 ：20170921
         ** 修改内容 ：
         *********************************************************************************************************/
         private void SetParam_Click(object sender, EventArgs e)
         {
             DialogResult ResultTemp;
             int i;
             string sysp;
            
             ResultTemp = MessageBox.Show(this, "不是必要情况下，请勿修改参数，请确认是否需要进行参数修改？", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
             if (ResultTemp == DialogResult.OK)   //表示点击确认        
             {                
                 FormHMI.g_SysParam[3] = StartPos.Value;
                 FormHMI.g_SysParam[28] = TotalWafer.Value;
                 FormHMI.g_SysParam[35] = ToughDis.Value;
                 FormHMI.g_SysParam[5] = BoxInv.Value;
                 FormHMI.g_SysParam[29] = BigBoxInv.Value;
                 FormHMI.g_SysParam[20] = InBox.Value;
                 FormHMI.g_SysParam[21] = OutBox.Value;
                 FormHMI.g_SysParam[38] = FirstWaferPos.Value;
                 FormHMI.g_SysParam[67] = BoxKeyInDelay.Value;
                 FormHMI.g_SysParam[76] = AlignPos.Value;       //花篮打齐位置
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

         private void TotalWafer_AfterChangeValue(object sender, NationalInstruments.UI.AfterChangeNumericValueEventArgs e)
         {

         }

    }
}
