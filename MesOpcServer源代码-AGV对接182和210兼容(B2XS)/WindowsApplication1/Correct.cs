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

namespace WindowsApplication1
{
    public delegate void ChangeFMFormData3(bool topGetState4);
    public delegate void ChangeFMFormData4(bool topGetState4);
    public delegate void ChangeFMFormData5(bool topGetState4);
    public partial class Correct : Form
    {
        public event ChangeFMFormData3 ChangeData;//校正
        public event ChangeFMFormData4 ChangeData1;//清除
        public event ChangeFMFormData5 ChangeData2;//参数设定
        public Correct()
        {
            InitializeComponent();
            Theory_X1.Value = FormHMI.g_TheoryPix[0];
            Theory_Y1.Value = FormHMI.g_TheoryPix[1];
            Theory_X2.Value = FormHMI.g_TheoryPix[2];
            Theory_Y2.Value = FormHMI.g_TheoryPix[3];
            Theory_X3.Value = FormHMI.g_TheoryPix[4];
            Theory_Y3.Value = FormHMI.g_TheoryPix[5];
            Theory_X4.Value = FormHMI.g_TheoryPix[6];
            Theory_Y4.Value = FormHMI.g_TheoryPix[7];
        }
        /*********************************************************************************************************
       ** 函数名称 ：Currect_Click
       ** 函数功能 ：振镜校正
       ** 修改时间 ：20190115
       ** 修改内容 ：
       *********************************************************************************************************/
        private void Currect_Click(object sender, EventArgs e)
        {
            DialogResult ResultTemp;
            ResultTemp = MessageBox.Show(this, "请确认是否进行振镜校正！", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (ResultTemp == DialogResult.OK)   //表示点击确认        
            {
                FormHMI.g_TheoryPix[0] = Theory_X1.Value;
                FormHMI.g_TheoryPix[1] = Theory_Y1.Value;
                FormHMI.g_TheoryPix[2] = Theory_X2.Value;
                FormHMI.g_TheoryPix[3] = Theory_Y2.Value;
                FormHMI.g_TheoryPix[4] = Theory_X3.Value;
                FormHMI.g_TheoryPix[5] = Theory_Y3.Value;
                FormHMI.g_TheoryPix[6] = Theory_X4.Value;
                FormHMI.g_TheoryPix[7] = Theory_Y4.Value;
                FormHMI.g_RealPix[0] = Preci_X1.Value;
                FormHMI.g_RealPix[1] = Preci_Y1.Value;
                FormHMI.g_RealPix[2] = Preci_X2.Value;
                FormHMI.g_RealPix[3] = Preci_Y2.Value;
                FormHMI.g_RealPix[4] = Preci_X3.Value;
                FormHMI.g_RealPix[5] = Preci_Y3.Value;
                FormHMI.g_RealPix[6] = Preci_X4.Value;
                FormHMI.g_RealPix[7] = Preci_Y4.Value;
                ChangeData(true);
            }
        }
        /*********************************************************************************************************
         ** 函数名称 ：ClearCB
         ** 函数功能 ：振镜校正清除
         ** 修改时间 ：20190115
         ** 修改内容 ：
         *********************************************************************************************************/
        private void Clear_Click(object sender, EventArgs e)
        {
            DialogResult ResultTemp;
            ResultTemp = MessageBox.Show(this, "请确认是否清除振镜校正数值！", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (ResultTemp == DialogResult.OK)   //表示点击确认        
            {
                ChangeData1(true);
            }
        }
        /*********************************************************************************************************
         ** 函数名称 ：TheorySet_Click
         ** 函数功能 ：参数套用
         ** 修改时间 ：20190115
         ** 修改内容 ：
         *********************************************************************************************************/
        private void TheorySet_Click(object sender, EventArgs e)
        {
            DialogResult ResultTemp;
            int i;
            string sysp;
            ResultTemp = MessageBox.Show(this, "非必要情况下，请勿修改该参数，请确认是否修改校正理论坐标值？", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (ResultTemp == DialogResult.OK)   //表示点击确认        
            {
                FormHMI.g_SysParam[59] = Theory_X1.Value;
                FormHMI.g_SysParam[60] = Theory_Y1.Value;
                FormHMI.g_SysParam[61] = Theory_X2.Value;
                FormHMI.g_SysParam[62] = Theory_Y2.Value;
                FormHMI.g_SysParam[63] = Theory_X3.Value;
                FormHMI.g_SysParam[64] = Theory_Y3.Value;
                FormHMI.g_SysParam[65] = Theory_X4.Value;
                FormHMI.g_SysParam[66] = Theory_Y4.Value;
                StreamWriter write = new StreamWriter("D:\\DRLaser\\SysPara.ini");
                for (i = 0; i < 80; i++)
                {
                    sysp = Convert.ToString(FormHMI.g_SysParam[i]);
                    write.WriteLine(sysp);
                }
                write.Close();
                ChangeData2(true);///参数设置到各变量
            }
        }
    }
}
