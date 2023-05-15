using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using IniHelper;
using Sunny.UI;

namespace MesOpcServer
{
    public partial class FormProductionStatistics : UIForm
    {
        private static FormProductionStatistics formStatistics = new FormProductionStatistics();
        public static Configure _cfg;
        int m_CurrentDay;
        int m_CurrentHour;

        int[] WaferInArray = new int[24];
        int[] WaferOutArray = new int[24];
        int[] CassetteInArray = new int[24];
        int[] CassetteOutArray = new int[24];



        private FormProductionStatistics()
        {
            InitializeComponent();

            CreateDirectories();

            string CurrentFileName = string.Format("D:\\DRLaser\\ProductionData\\{0:D4}-{1:D2}-{2:D2}.ini", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            _cfg = new Configure(CurrentFileName);

            m_CurrentDay = DateTime.Now.Day;

            dataGridView1.Rows.Add(24);
            dataGridView1.RowHeadersVisible = false;
            for (int i = 0; i < 24; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = i.ToString("D2");
            }


        }

        void CreateDirectories()
        {
            if (!System.IO.Directory.Exists("D:\\DRLaser\\ProductionData"))
            {
                Directory.CreateDirectory("D:\\DRLaser\\ProductionData");
            }
        }

        public static FormProductionStatistics getInstance()
        {
            return formStatistics;
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            RefreshWaferInData();
            RefreshWaferOutData();
            RefreshCassetteInData();
            RefreshCassetteOutData();


        }

        #region 计数方法
        public void AddWaferInData()
        {
            int number = _cfg.ReadConfig("WaferInData", m_CurrentHour.ToString("D2"), 0);
            _cfg.WriteConfig("WaferInData", m_CurrentHour.ToString("D2"), ++number);

        }

        public void AddWaferOutData()
        {
            int number = _cfg.ReadConfig("WaferOutData", m_CurrentHour.ToString("D2"), 0);
            _cfg.WriteConfig("WaferOutData", m_CurrentHour.ToString("D2"), ++number);
        }

        public void AddCassetteInData()
        {
            int number = _cfg.ReadConfig("CassetteInData", m_CurrentHour.ToString("D2"), 0);
            _cfg.WriteConfig("CassetteInData", m_CurrentHour.ToString("D2"), ++number);

        }

        public void AddCassetteOutData()
        {
            int number = _cfg.ReadConfig("CassetteOutData", m_CurrentHour.ToString("D2"), 0);
            _cfg.WriteConfig("CassetteOutData", m_CurrentHour.ToString("D2"), ++number);

        }

        #endregion 计数方法

        #region 刷新当日数据方法
        public void RefreshWaferInData()
        {

            for (int i = 0; i < 24; i++)
            {
                WaferInArray[i] = _cfg.ReadConfig("WaferInData", i.ToString("D2"), 0);
                dataGridView1.Rows[i].Cells[1].Value = WaferInArray[i].ToString();
            }
        }

        public void RefreshWaferOutData()
        {

            for (int i = 0; i < 24; i++)
            {
                WaferOutArray[i] = _cfg.ReadConfig("WaferOutData", i.ToString("D2"), 0);
                dataGridView1.Rows[i].Cells[2].Value = WaferOutArray[i].ToString();
            }
        }

        public void RefreshCassetteInData()
        {

            for (int i = 0; i < 24; i++)
            {
                CassetteInArray[i] = _cfg.ReadConfig("CassetteInData", i.ToString("D2"), 0);
                dataGridView1.Rows[i].Cells[3].Value = CassetteInArray[i].ToString();
            }
        }

        public void RefreshCassetteOutData()
        {

            for (int i = 0; i < 24; i++)
            {
                CassetteOutArray[i] = _cfg.ReadConfig("CassetteOutData", i.ToString("D2"), 0);
                dataGridView1.Rows[i].Cells[4].Value = CassetteOutArray[i].ToString();
            }
        }
        #endregion 刷新当日数据方法

        #region 刷新历史数据方法
        public void RefreshWaferInData_History()
        {
            try
            {
                string CurrentFileName = "D:\\DRLaser\\ProductionData\\" + comboBoxDateChoose.Text + ".ini";

                Configure _cfg_History = new Configure(CurrentFileName);

                for (int i = 0; i < 24; i++)
                {
                    WaferInArray[i] = _cfg_History.ReadConfig("WaferInData", i.ToString("D2"), 0);
                    dataGridView1.Rows[i].Cells[1].Value = WaferInArray[i].ToString();
                }
            }
            catch
            {
            }
        }

        public void RefreshWaferOutData_History()
        {
            try
            {
                string CurrentFileName = "D:\\DRLaser\\ProductionData\\" + comboBoxDateChoose.Text + ".ini";

                Configure _cfg_History = new Configure(CurrentFileName);

                for (int i = 0; i < 24; i++)
                {
                    WaferOutArray[i] = _cfg_History.ReadConfig("WaferOutData", i.ToString("D2"), 0);
                    dataGridView1.Rows[i].Cells[2].Value = WaferOutArray[i].ToString();
                }
            }
            catch
            {
            }
        }

        public void RefreshCassetteInData_History()
        {
            try
            {
                string CurrentFileName = "D:\\DRLaser\\ProductionData\\" + comboBoxDateChoose.Text + ".ini";
                Configure _cfg_History = new Configure(CurrentFileName);

                for (int i = 0; i < 24; i++)
                {
                    CassetteInArray[i] = _cfg_History.ReadConfig("CassetteInData", i.ToString("D2"), 0);
                    dataGridView1.Rows[i].Cells[3].Value = CassetteInArray[i].ToString();
                }
            }
            catch
            {
            }
        }

        public void RefreshCassetteOutData_History()
        {
            try
            {
                string CurrentFileName = "D:\\DRLaser\\ProductionData\\" + comboBoxDateChoose.Text + ".ini";
                Configure _cfg_History = new Configure(CurrentFileName);

                for (int i = 0; i < 24; i++)
                {
                    CassetteOutArray[i] = _cfg_History.ReadConfig("CassetteOutData", i.ToString("D2"), 0);
                    dataGridView1.Rows[i].Cells[4].Value = CassetteOutArray[i].ToString();
                }
            }
            catch
            {
            }

        }

        #endregion 刷新历史数据方法

        int m_ProductionCounter;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (m_CurrentDay != DateTime.Now.Day)
            {
                m_CurrentDay = DateTime.Now.Day;

                string CurrentFileName = string.Format("D:\\DRLaser\\ProductionData\\{0:D4}-{1:D2}-{2:D2}.ini", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                _cfg = new Configure(CurrentFileName);
            }

            m_CurrentHour = DateTime.Now.Hour;


            if (m_ProductionCounter < 10)
                m_ProductionCounter++;
            else
            {
                m_ProductionCounter = 0;

                CurrentShiftProduction.Text = GetCurrentShiftProduction().ToString();
                LastShiftProduction.Text = GetLastShiftProduction().ToString();

                CurrentShiftInput.Text = GetCurrentShiftInput().ToString();
                LastShiftInput.Text = GetLastShiftInput().ToString();
            }


        }

        int GetCurrentShiftProduction()
        {
            int total = 0;
            DateTime CurrentDateTime = DateTime.Now;
            DateTime LastDateTime = CurrentDateTime.Subtract(TimeSpan.FromDays(1.0));

            if (CurrentDateTime.Hour < 8)
            {

                string CurrentFileName1 = string.Format("D:\\DRLaser\\ProductionData\\{0:D4}-{1:D2}-{2:D2}.ini", LastDateTime.Year, LastDateTime.Month, LastDateTime.Day);
                Configure _TempConfig1 = new Configure(CurrentFileName1);

                int number20 = _TempConfig1.ReadConfig("WaferOutData", "20", 0);
                int number21 = _TempConfig1.ReadConfig("WaferOutData", "21", 0);
                int number22 = _TempConfig1.ReadConfig("WaferOutData", "22", 0);
                int number23 = _TempConfig1.ReadConfig("WaferOutData", "23", 0);

                string CurrentFileName2 = string.Format("D:\\DRLaser\\ProductionData\\{0:D4}-{1:D2}-{2:D2}.ini", CurrentDateTime.Year, CurrentDateTime.Month, CurrentDateTime.Day);
                Configure _TempConfig2 = new Configure(CurrentFileName1);

                int number00 = _TempConfig2.ReadConfig("WaferOutData", "00", 0);
                int number01 = _TempConfig2.ReadConfig("WaferOutData", "01", 0);
                int number02 = _TempConfig2.ReadConfig("WaferOutData", "02", 0);
                int number03 = _TempConfig2.ReadConfig("WaferOutData", "03", 0);
                int number04 = _TempConfig2.ReadConfig("WaferOutData", "04", 0);
                int number05 = _TempConfig2.ReadConfig("WaferOutData", "05", 0);
                int number06 = _TempConfig2.ReadConfig("WaferOutData", "06", 0);
                int number07 = _TempConfig2.ReadConfig("WaferOutData", "07", 0);

                total = number20 + number21 + number22 + number23 + number00 + number01 + number02 + number03 + number04 + number05 + number06 + number07;

                MESServer._cfg.WriteConfig("MES", "CurrentShiftProduction", total);

                return total;
            }
            else if (CurrentDateTime.Hour < 20)
            {
                string CurrentFileName = string.Format("D:\\DRLaser\\ProductionData\\{0:D4}-{1:D2}-{2:D2}.ini", CurrentDateTime.Year, CurrentDateTime.Month, CurrentDateTime.Day);
                Configure _TempConfig = new Configure(CurrentFileName);

                int number08 = _TempConfig.ReadConfig("WaferOutData", "08", 0);
                int number09 = _TempConfig.ReadConfig("WaferOutData", "09", 0);
                int number10 = _TempConfig.ReadConfig("WaferOutData", "10", 0);
                int number11 = _TempConfig.ReadConfig("WaferOutData", "11", 0);
                int number12 = _TempConfig.ReadConfig("WaferOutData", "12", 0);
                int number13 = _TempConfig.ReadConfig("WaferOutData", "13", 0);

                int number14 = _TempConfig.ReadConfig("WaferOutData", "14", 0);
                int number15 = _TempConfig.ReadConfig("WaferOutData", "15", 0);
                int number16 = _TempConfig.ReadConfig("WaferOutData", "16", 0);
                int number17 = _TempConfig.ReadConfig("WaferOutData", "17", 0);
                int number18 = _TempConfig.ReadConfig("WaferOutData", "18", 0);
                int number19 = _TempConfig.ReadConfig("WaferOutData", "19", 0);

                total = number08 + number09 + number10 + number11 + number12 + number13 + number14 + number15 + number16 + +number17 + number18 + number19;

                MESServer._cfg.WriteConfig("MES", "CurrentShiftProduction", total);

                return total;
            }
            else
            {
                string CurrentFileName = string.Format("D:\\DRLaser\\ProductionData\\{0:D4}-{1:D2}-{2:D2}.ini", CurrentDateTime.Year, CurrentDateTime.Month, CurrentDateTime.Day);
                Configure _TempConfig = new Configure(CurrentFileName);

                int number20 = _TempConfig.ReadConfig("WaferOutData", "20", 0);
                int number21 = _TempConfig.ReadConfig("WaferOutData", "21", 0);
                int number22 = _TempConfig.ReadConfig("WaferOutData", "22", 0);
                int number23 = _TempConfig.ReadConfig("WaferOutData", "23", 0);

                total = number20 + number21 + number22 + number23;

                MESServer._cfg.WriteConfig("MES","CurrentShiftProduction",total);

                return total;
            }
        }

        int GetLastShiftProduction()
        {
            int total = 0;
            DateTime CurrentDateTime = DateTime.Now;
            DateTime LastDateTime = CurrentDateTime.Subtract(TimeSpan.FromDays(1.0));

            if (CurrentDateTime.Hour < 8)
            {
                string CurrentFileName1 = string.Format("D:\\DRLaser\\ProductionData\\{0:D4}-{1:D2}-{2:D2}.ini", LastDateTime.Year, LastDateTime.Month, LastDateTime.Day);
                Configure _TempConfig1 = new Configure(CurrentFileName1);

                int number08 = _TempConfig1.ReadConfig("WaferOutData", "08", 0);
                int number09 = _TempConfig1.ReadConfig("WaferOutData", "09", 0);
                int number10 = _TempConfig1.ReadConfig("WaferOutData", "10", 0);
                int number11 = _TempConfig1.ReadConfig("WaferOutData", "11", 0);
                int number12 = _TempConfig1.ReadConfig("WaferOutData", "12", 0);
                int number13 = _TempConfig1.ReadConfig("WaferOutData", "13", 0);
                int number14 = _TempConfig1.ReadConfig("WaferOutData", "14", 0);
                int number15 = _TempConfig1.ReadConfig("WaferOutData", "15", 0);
                int number16 = _TempConfig1.ReadConfig("WaferOutData", "16", 0);
                int number17 = _TempConfig1.ReadConfig("WaferOutData", "17", 0);
                int number18 = _TempConfig1.ReadConfig("WaferOutData", "18", 0);
                int number19 = _TempConfig1.ReadConfig("WaferOutData", "19", 0);


                total = number08 + number09 + number10 + number11 + number12 + number13 + number14 + number15 + number16 + number17 + number18 + number19;

                MESServer._cfg.WriteConfig("MES", "LastShiftProduction", total);

                return total;
            }
            else if (CurrentDateTime.Hour < 20)
            {
                string CurrentFileName2 = string.Format("D:\\DRLaser\\ProductionData\\{0:D4}-{1:D2}-{2:D2}.ini", LastDateTime.Year, LastDateTime.Month, LastDateTime.Day);
                Configure _TempConfig2 = new Configure(CurrentFileName2);

                int number20 = _TempConfig2.ReadConfig("WaferOutData", "20", 0);
                int number21 = _TempConfig2.ReadConfig("WaferOutData", "21", 0);
                int number22 = _TempConfig2.ReadConfig("WaferOutData", "22", 0);
                int number23 = _TempConfig2.ReadConfig("WaferOutData", "23", 0);

                string CurrentFileName3 = string.Format("D:\\DRLaser\\ProductionData\\{0:D4}-{1:D2}-{2:D2}.ini", CurrentDateTime.Year, CurrentDateTime.Month, CurrentDateTime.Day);
                Configure _TempConfig3 = new Configure(CurrentFileName3);

                int number00 = _TempConfig3.ReadConfig("WaferOutData", "00", 0);
                int number01 = _TempConfig3.ReadConfig("WaferOutData", "01", 0);
                int number02 = _TempConfig3.ReadConfig("WaferOutData", "02", 0);
                int number03 = _TempConfig3.ReadConfig("WaferOutData", "03", 0);
                int number04 = _TempConfig3.ReadConfig("WaferOutData", "04", 0);
                int number05 = _TempConfig3.ReadConfig("WaferOutData", "05", 0);
                int number06 = _TempConfig3.ReadConfig("WaferOutData", "06", 0);
                int number07 = _TempConfig3.ReadConfig("WaferOutData", "07", 0);

                total = number20 + number21 + number22 + number23 + number00 + number01 + number02 + number03 + number04 + number05 + number06 + number07;

                MESServer._cfg.WriteConfig("MES", "LastShiftProduction", total);

                return total;
            }
            else
            {
                string CurrentFileName4 = string.Format("D:\\DRLaser\\ProductionData\\{0:D4}-{1:D2}-{2:D2}.ini", CurrentDateTime.Year, CurrentDateTime.Month, CurrentDateTime.Day);
                Configure _TempConfig4 = new Configure(CurrentFileName4);

                int number08 = _TempConfig4.ReadConfig("WaferOutData", "08", 0);
                int number09 = _TempConfig4.ReadConfig("WaferOutData", "09", 0);
                int number10 = _TempConfig4.ReadConfig("WaferOutData", "10", 0);
                int number11 = _TempConfig4.ReadConfig("WaferOutData", "11", 0);
                int number12 = _TempConfig4.ReadConfig("WaferOutData", "12", 0);
                int number13 = _TempConfig4.ReadConfig("WaferOutData", "13", 0);
                int number14 = _TempConfig4.ReadConfig("WaferOutData", "14", 0);
                int number15 = _TempConfig4.ReadConfig("WaferOutData", "15", 0);
                int number16 = _TempConfig4.ReadConfig("WaferOutData", "16", 0);
                int number17 = _TempConfig4.ReadConfig("WaferOutData", "17", 0);
                int number18 = _TempConfig4.ReadConfig("WaferOutData", "18", 0);
                int number19 = _TempConfig4.ReadConfig("WaferOutData", "19", 0);

                total = number08 + number09 + number10 + number11 + number12 + number13 + number14 + number15 + number16 + number17 + number18 + number19;

                MESServer._cfg.WriteConfig("MES", "LastShiftProduction", total);

                return total;
            }


        }

        int GetCurrentShiftInput()
        {
            int total = 0;
            DateTime CurrentDateTime = DateTime.Now;
            DateTime LastDateTime = CurrentDateTime.Subtract(TimeSpan.FromDays(1.0));

            if (CurrentDateTime.Hour < 8)
            {

                string CurrentFileName1 = string.Format("D:\\DRLaser\\ProductionData\\{0:D4}-{1:D2}-{2:D2}.ini", LastDateTime.Year, LastDateTime.Month, LastDateTime.Day);
                Configure _TempConfig1 = new Configure(CurrentFileName1);

                int number20 = _TempConfig1.ReadConfig("WaferInData", "20", 0);
                int number21 = _TempConfig1.ReadConfig("WaferInData", "21", 0);
                int number22 = _TempConfig1.ReadConfig("WaferInData", "22", 0);
                int number23 = _TempConfig1.ReadConfig("WaferInData", "23", 0);

                string CurrentFileName2 = string.Format("D:\\DRLaser\\ProductionData\\{0:D4}-{1:D2}-{2:D2}.ini", CurrentDateTime.Year, CurrentDateTime.Month, CurrentDateTime.Day);
                Configure _TempConfig2 = new Configure(CurrentFileName1);

                int number00 = _TempConfig2.ReadConfig("WaferInData", "00", 0);
                int number01 = _TempConfig2.ReadConfig("WaferInData", "01", 0);
                int number02 = _TempConfig2.ReadConfig("WaferInData", "02", 0);
                int number03 = _TempConfig2.ReadConfig("WaferInData", "03", 0);
                int number04 = _TempConfig2.ReadConfig("WaferInData", "04", 0);
                int number05 = _TempConfig2.ReadConfig("WaferInData", "05", 0);
                int number06 = _TempConfig2.ReadConfig("WaferInData", "06", 0);
                int number07 = _TempConfig2.ReadConfig("WaferInData", "07", 0);

                total = number20 + number21 + number22 + number23 + number00 + number01 + number02 + number03 + number04 + number05 + number06 + number07;

                MESServer._cfg.WriteConfig("MES", "CurrentShiftInput", total);

                return total;
            }
            else if (CurrentDateTime.Hour < 20)
            {
                string CurrentFileName = string.Format("D:\\DRLaser\\ProductionData\\{0:D4}-{1:D2}-{2:D2}.ini", CurrentDateTime.Year, CurrentDateTime.Month, CurrentDateTime.Day);
                Configure _TempConfig = new Configure(CurrentFileName);

                int number08 = _TempConfig.ReadConfig("WaferInData", "08", 0);
                int number09 = _TempConfig.ReadConfig("WaferInData", "09", 0);
                int number10 = _TempConfig.ReadConfig("WaferInData", "10", 0);
                int number11 = _TempConfig.ReadConfig("WaferInData", "11", 0);
                int number12 = _TempConfig.ReadConfig("WaferInData", "12", 0);
                int number13 = _TempConfig.ReadConfig("WaferInData", "13", 0);

                int number14 = _TempConfig.ReadConfig("WaferInData", "14", 0);
                int number15 = _TempConfig.ReadConfig("WaferInData", "15", 0);
                int number16 = _TempConfig.ReadConfig("WaferInData", "16", 0);
                int number17 = _TempConfig.ReadConfig("WaferInData", "17", 0);
                int number18 = _TempConfig.ReadConfig("WaferInData", "18", 0);
                int number19 = _TempConfig.ReadConfig("WaferInData", "19", 0);

                total = number08 + number09 + number10 + number11 + number12 + number13 + number14 + number15 + number16 + +number17 + number18 + number19;

                MESServer._cfg.WriteConfig("MES", "CurrentShiftInput", total);

                return total;
            }
            else
            {
                string CurrentFileName = string.Format("D:\\DRLaser\\ProductionData\\{0:D4}-{1:D2}-{2:D2}.ini", CurrentDateTime.Year, CurrentDateTime.Month, CurrentDateTime.Day);
                Configure _TempConfig = new Configure(CurrentFileName);

                int number20 = _TempConfig.ReadConfig("WaferInData", "20", 0);
                int number21 = _TempConfig.ReadConfig("WaferInData", "21", 0);
                int number22 = _TempConfig.ReadConfig("WaferInData", "22", 0);
                int number23 = _TempConfig.ReadConfig("WaferInData", "23", 0);

                total = number20 + number21 + number22 + number23;

                MESServer._cfg.WriteConfig("MES", "CurrentShiftInput", total);

                return total;
            }
        }

        int GetLastShiftInput()
        {
            int total = 0;
            DateTime CurrentDateTime = DateTime.Now;
            DateTime LastDateTime = CurrentDateTime.Subtract(TimeSpan.FromDays(1.0));

            if (CurrentDateTime.Hour < 8)
            {
                string CurrentFileName1 = string.Format("D:\\DRLaser\\ProductionData\\{0:D4}-{1:D2}-{2:D2}.ini", LastDateTime.Year, LastDateTime.Month, LastDateTime.Day);
                Configure _TempConfig1 = new Configure(CurrentFileName1);

                int number08 = _TempConfig1.ReadConfig("WaferInData", "08", 0);
                int number09 = _TempConfig1.ReadConfig("WaferInData", "09", 0);
                int number10 = _TempConfig1.ReadConfig("WaferInData", "10", 0);
                int number11 = _TempConfig1.ReadConfig("WaferInData", "11", 0);
                int number12 = _TempConfig1.ReadConfig("WaferInData", "12", 0);
                int number13 = _TempConfig1.ReadConfig("WaferInData", "13", 0);
                int number14 = _TempConfig1.ReadConfig("WaferInData", "14", 0);
                int number15 = _TempConfig1.ReadConfig("WaferInData", "15", 0);
                int number16 = _TempConfig1.ReadConfig("WaferInData", "16", 0);
                int number17 = _TempConfig1.ReadConfig("WaferInData", "17", 0);
                int number18 = _TempConfig1.ReadConfig("WaferInData", "18", 0);
                int number19 = _TempConfig1.ReadConfig("WaferInData", "19", 0);


                total = number08 + number09 + number10 + number11 + number12 + number13 + number14 + number15 + number16 + number17 + number18 + number19;

                MESServer._cfg.WriteConfig("MES", "LastShiftInput", total);

                return total;
            }
            else if (CurrentDateTime.Hour < 20)
            {
                string CurrentFileName2 = string.Format("D:\\DRLaser\\ProductionData\\{0:D4}-{1:D2}-{2:D2}.ini", LastDateTime.Year, LastDateTime.Month, LastDateTime.Day);
                Configure _TempConfig2 = new Configure(CurrentFileName2);

                int number20 = _TempConfig2.ReadConfig("WaferInData", "20", 0);
                int number21 = _TempConfig2.ReadConfig("WaferInData", "21", 0);
                int number22 = _TempConfig2.ReadConfig("WaferInData", "22", 0);
                int number23 = _TempConfig2.ReadConfig("WaferInData", "23", 0);

                string CurrentFileName3 = string.Format("D:\\DRLaser\\ProductionData\\{0:D4}-{1:D2}-{2:D2}.ini", CurrentDateTime.Year, CurrentDateTime.Month, CurrentDateTime.Day);
                Configure _TempConfig3 = new Configure(CurrentFileName3);

                int number00 = _TempConfig3.ReadConfig("WaferInData", "00", 0);
                int number01 = _TempConfig3.ReadConfig("WaferInData", "01", 0);
                int number02 = _TempConfig3.ReadConfig("WaferInData", "02", 0);
                int number03 = _TempConfig3.ReadConfig("WaferInData", "03", 0);
                int number04 = _TempConfig3.ReadConfig("WaferInData", "04", 0);
                int number05 = _TempConfig3.ReadConfig("WaferInData", "05", 0);
                int number06 = _TempConfig3.ReadConfig("WaferInData", "06", 0);
                int number07 = _TempConfig3.ReadConfig("WaferInData", "07", 0);

                total = number20 + number21 + number22 + number23 + number00 + number01 + number02 + number03 + number04 + number05 + number06 + number07;

                MESServer._cfg.WriteConfig("MES", "LastShiftInput", total);

                return total;
            }
            else
            {
                string CurrentFileName4 = string.Format("D:\\DRLaser\\ProductionData\\{0:D4}-{1:D2}-{2:D2}.ini", CurrentDateTime.Year, CurrentDateTime.Month, CurrentDateTime.Day);
                Configure _TempConfig4 = new Configure(CurrentFileName4);

                int number08 = _TempConfig4.ReadConfig("WaferInData", "08", 0);
                int number09 = _TempConfig4.ReadConfig("WaferInData", "09", 0);
                int number10 = _TempConfig4.ReadConfig("WaferInData", "10", 0);
                int number11 = _TempConfig4.ReadConfig("WaferInData", "11", 0);
                int number12 = _TempConfig4.ReadConfig("WaferInData", "12", 0);
                int number13 = _TempConfig4.ReadConfig("WaferInData", "13", 0);
                int number14 = _TempConfig4.ReadConfig("WaferInData", "14", 0);
                int number15 = _TempConfig4.ReadConfig("WaferInData", "15", 0);
                int number16 = _TempConfig4.ReadConfig("WaferInData", "16", 0);
                int number17 = _TempConfig4.ReadConfig("WaferInData", "17", 0);
                int number18 = _TempConfig4.ReadConfig("WaferInData", "18", 0);
                int number19 = _TempConfig4.ReadConfig("WaferInData", "19", 0);

                total = number08 + number09 + number10 + number11 + number12 + number13 + number14 + number15 + number16 + number17 + number18 + number19;

                MESServer._cfg.WriteConfig("MES", "LastShiftInput", total);

                return total;
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            RefreshWaferInData();
            RefreshWaferOutData();
            RefreshCassetteInData();
            RefreshCassetteOutData();
        }

        public void ShowPanel()
        {
            try
            {
                comboBoxDateChoose.Items.Clear();
                comboBoxDateChoose.Enabled = false;
                checkBoxHistoryData.Checked = false;
                comboBoxDateChoose.Text = "";
                button1.Visible = true;


            }
            catch
            {
            }

            RefreshWaferInData();
            RefreshWaferOutData();
            RefreshCassetteInData();
            RefreshCassetteOutData();

            this.Show();

        }

        private void FormProductionStatistics_FormClosing(object sender, FormClosingEventArgs e)
        {

            e.Cancel = true;


        }

        // 重写OnClosing使点击关闭按键时窗体隐藏

        protected override void OnClosing(CancelEventArgs e)
        {



            this.Hide();

            e.Cancel = true;

        }

        private void checkBoxHistoryData_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBoxHistoryData.Checked)
                {
                    comboBoxDateChoose.Enabled = true;
                    button1.Visible = false;


                    string strFolderPath = "D:\\DRLaser\\ProductionData\\";

                    DirectoryInfo dyInfo = new DirectoryInfo(strFolderPath);
                    //获取文件夹下所有的文件
                    foreach (FileInfo feInfo in dyInfo.GetFiles("*.ini"))
                    {
                        string[] dateStr = feInfo.Name.Split(new char[] { '.' });
                        comboBoxDateChoose.Items.Add(dateStr[0]);

                    }

                    comboBoxDateChoose.SelectedIndex = 0;

                    RefreshWaferInData_History();
                    RefreshWaferOutData_History();
                    RefreshCassetteInData_History();
                    RefreshCassetteOutData_History();

                }
                else
                {
                    comboBoxDateChoose.Enabled = false;
                    button1.Visible = true;
                    comboBoxDateChoose.Items.Clear();
                    comboBoxDateChoose.Text = "";

                    RefreshWaferInData();
                    RefreshWaferOutData();
                    RefreshCassetteInData();
                    RefreshCassetteOutData();

                }
            }
            catch
            {
            }

        }

        private void comboBoxDateChoose_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshWaferInData_History();
            RefreshWaferOutData_History();
            RefreshCassetteInData_History();
            RefreshCassetteOutData_History();

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void CurrentShiftProduction_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }






    }
}
