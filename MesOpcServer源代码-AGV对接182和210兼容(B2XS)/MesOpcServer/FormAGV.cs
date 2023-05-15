using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IniHelper;
using Sunny.UI;
using DeviceManeger;

namespace MesOpcServer
{
    public partial class FormAGV : UIForm
    {
        FormAGV_COM formAGV_A;
        FormAGV_COM2 formAGV_B;
        Configure _cfg = new Configure("D:\\DRLaser\\MES\\MES.ini");
        public static FormAGV formAGV;

        public static int CassetteNumberConstant = 10;

        public static int LoadTrackIn1 = 5;
        public static int LoadTrackIn2 = 5;
        public static int LoadTrackInTotal = 10;

        public static int LoadTrackOut1 = 5;
        public static int LoadTrackOut2 = 5;
        public static int LoadTrackOutTotal = 10;

        public FormAGV()
        {
            InitializeComponent();

            string agvCOM = _cfg.ReadConfig("MES", "COM", "");

            CassetteNumberConstant = _cfg.ReadConfig("MES", "CassetteNumberConstant", 10);

            if (CassetteNumberConstant == 12)
            {
                LoadTrackIn1 = 6;
                LoadTrackIn2 = 6;
                LoadTrackInTotal = 12;

                LoadTrackOut1 = 6;
                LoadTrackOut2 = 6;
                LoadTrackOutTotal = 12;
            }
            else if (CassetteNumberConstant == 10)
            {
                LoadTrackIn1 = 5;
                LoadTrackIn2 = 5;
                LoadTrackInTotal = 10;

                LoadTrackOut1 = 5;
                LoadTrackOut2 = 5;
                LoadTrackOutTotal = 10;
            }

            if (agvCOM == "21")
            {

                formAGV_A = new FormAGV_COM();

                formAGV_A.Location = panelAGV.Location;
                formAGV_A.TopLevel = false;
                formAGV_A.Size = panelAGV.Size;
                formAGV_A.Parent = panelAGV;
                formAGV_A.Dock = DockStyle.Fill;
                formAGV_A.Show();



            }
            else if (agvCOM == "22")
            {
                formAGV_B = new FormAGV_COM2();
                formAGV_B.Show();
                formAGV_B.Hide();

                formAGV_B.Location = panelAGV.Location;
                formAGV_B.TopLevel = false;
                formAGV_B.Size = panelAGV.Size;
                formAGV_B.Parent = panelAGV;
                formAGV_B.Dock = DockStyle.Fill;
                formAGV_B.Show();


            }

        }



        private void FormAGV_FormClosing(object sender, FormClosingEventArgs e)
        {

            e.Cancel = true;
        }

        // 重写OnClosing使点击关闭按键时窗体能够缩进托盘

        protected override void OnClosing(CancelEventArgs e)
        {

            this.Hide();

            e.Cancel = true;

        }

        private void agvFormCom1_Load(object sender, EventArgs e)
        {
            
        }

        private void FormAGV_Load(object sender, EventArgs e)
        {
            Device.GetDevice().SettingInit();
            if (Device.GetDevice().SystemSettings.LeftOrRight == ExcutionModule.LeftModule)
            {
                agvFormCom1.FormInit(ExcutionModule.LeftModule);
            }
            else
            {
                agvFormCom1.FormInit(ExcutionModule.RightModule);
            }
        }
    }
}
