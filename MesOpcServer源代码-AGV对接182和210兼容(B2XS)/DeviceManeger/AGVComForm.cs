using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeviceManeger
{
    public partial class AGVComForm : UserControl
    {
        public AGVComForm()
        {
            InitializeComponent();
        }

        private void axActUtlType1_OnDeviceStatus(object sender, AxActUtlTypeLib._IActUtlTypeEvents_OnDeviceStatusEvent e)
        {

        }

        private void AGVFormCom_Load(object sender, EventArgs e)
        {
        }
        public void FormInit(ExcutionModule module)
        {
            var device = Device.GetDevice().EquipmentInit(this.axActUtlType1);

            if (module == ExcutionModule.LeftModule)
            {
                LeftOrRightCom.Text = "左侧 COM21";
            }
            else if (module == ExcutionModule.RightModule)
            {
                LeftOrRightCom.Text = "右侧 COM22";
            }
        }

        private void buttonManualFinished_Click(object sender, EventArgs e)
        {

        }

        private void timerConnect_Tick(object sender, EventArgs e)
        {

        }
    }
}
