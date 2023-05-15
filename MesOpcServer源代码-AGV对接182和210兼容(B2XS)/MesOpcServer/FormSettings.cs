using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sunny.UI;

namespace MesOpcServer
{
    public partial class FormSettings : UIForm
    {
        private static FormSettings formSettings = new FormSettings();
        

        private FormSettings()
        {
            InitializeComponent();

          

            EqpNum.Text = MESServer._cfg.ReadConfig("MES", "EqpNum", "01").PadLeft(2, '0');
            ProcessNum.Text = MESServer._cfg.ReadConfig("MES", "ProcessNum", "60");

        }

        public static FormSettings getInstance()
        {
            return formSettings;
        }

        private void DownstreamIP_TextChanged(object sender, EventArgs e)
        {
           
           
        }

        private void DownstreamPort_TextChanged(object sender, EventArgs e)
        {
           
        }

     

        private void EqpNum_TextChanged(object sender, EventArgs e)
        {
            MESServer._cfg.WriteConfig("MES", "EqpNum", EqpNum.Text.PadLeft(2, '0'));
        }

        private void ProcessNum_TextChanged(object sender, EventArgs e)
        {
            MESServer._cfg.WriteConfig("MES", "ProcessNum", ProcessNum.Text);
        }


        private void FormSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        // 重写OnClosing使点击关闭按键时窗体能够缩进托盘

        protected override void OnClosing(CancelEventArgs e)
        {

            this.Hide();

            e.Cancel = true;

        }

        private void btnReConnect_Click(object sender, EventArgs e)
        {
            

        }
        int i;
        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void checkBoxDisableConnection_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {

        }

        private void radioBtn12In12Out_MouseClick(object sender, MouseEventArgs e)
        {
            MESServer._cfg.WriteConfig("MES", "CassetteNumberConstant", 12);
            FormAGV.CassetteNumberConstant = 12;
            FormAGV.LoadTrackIn1 = 6;
            FormAGV.LoadTrackIn2 = 6;
            FormAGV.LoadTrackInTotal = 12;

            FormAGV.LoadTrackOut1 = 6;
            FormAGV.LoadTrackOut2 = 6;
            FormAGV.LoadTrackOutTotal = 12;
        }

        private void radioBtn10In10Out_MouseClick(object sender, MouseEventArgs e)
        {
            MESServer._cfg.WriteConfig("MES", "CassetteNumberConstant", 10);
            FormAGV.CassetteNumberConstant = 10;

            FormAGV.LoadTrackIn1 = 5;
            FormAGV.LoadTrackIn2 = 5;
            FormAGV.LoadTrackInTotal = 10;

            FormAGV.LoadTrackOut1 = 5;
            FormAGV.LoadTrackOut2 = 5;
            FormAGV.LoadTrackOutTotal = 10;
        }

        private void FormSettings_Shown(object sender, EventArgs e)
        {
            int CassetteNumberConstant = MESServer._cfg.ReadConfig("MES", "CassetteNumberConstant", 10);

            if (CassetteNumberConstant == 12)
            {
                radioBtn12In12Out.Checked = true;
            }
            else if (CassetteNumberConstant == 10)
            {
                radioBtn10In10Out.Checked = true;
            }

        }

        private void radioBtn12In12Out_CheckedChanged(object sender, EventArgs e)
        {

        }

       

         


    }
}
