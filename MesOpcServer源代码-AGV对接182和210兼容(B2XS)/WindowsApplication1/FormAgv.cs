using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsApplication1;

namespace DRMotion
{
    public partial class FormAgv : Form
    {
        public FormAgv()
        {
            InitializeComponent();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormHMI.g_ForceDownFinish = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            FormHMI.g_AgvModel = checkBox1.Checked;

        }

        private void timerMonitor_Tick(object sender, EventArgs e)
        {
            textUpAgv.Text = FormHMI.g_PlcOutNum.ToString();
            textDownAgv.Text = FormHMI.g_PlcInNum.ToString();
            textBox1.Text = FormHMI.m_WcsHeart.ToString();

            checkBox1.Checked = FormHMI.g_AgvModel;
            checkBox2.Checked = FormHMI.g_NotAgvAlarm;
            checkBox5.Checked = FormHMI.g_NoCheckAgvSensor;
            textDownStep.Text = FormHMI.g_LoadAGVCommStep.ToString();
            textUpStep.Text = FormHMI.g_UnLoadAGVCommStep.ToString();
            textDownTrack.Text = FormHMI.g_LoadFullNum.ToString();
            textUpTrack.Text = FormHMI.g_UnloadEmptyNum.ToString();
            textUpWcs.Text = FormHMI.m_AgvUpStateTotal.ToString();
            textDownWcs.Text = FormHMI.m_AgvDownStateTotal.ToString();
            led1.Value = FormHMI.m_AgvUpState[0];
            led2.Value = FormHMI.m_AgvUpState[4];
            led3.Value = FormHMI.m_AgvUpState[5];
            led4.Value = FormHMI.m_AgvUpState[6];
            led5.Value = FormHMI.m_AgvDownState[0];
            led6.Value = FormHMI.m_AgvDownState[1];
            led7.Value = FormHMI.m_AgvDownState[2];
            led8.Value = FormHMI.m_AgvDownState[3];

        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormHMI.g_ForceUpFinish = true;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            FormHMI.g_NotAgvAlarm = checkBox2.Checked;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            FormHMI.g_NoCheckAgvSensor = checkBox5.Checked;
        }
    }
}
