using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceManeger
{
    public class SystemSetting : AbstractMessage
    {
        public SystemSetting()
        {
            AgvComSetting = new AgvComMessage();
        }
        public AgvComMessage AgvComSetting { get; set; }
        /// <summary>
        /// 左右侧
        /// </summary>
        public ExcutionModule LeftOrRight { get; set; }
        /// <summary>
        /// Plc类型
        /// </summary>
        public PlcType PLCType { get; set; }
        /// <summary>
        /// AGV通讯方式
        /// </summary>
        public AgvCommunicatType AgvType { get; set; }
    }
    public class AgvComMessage
    {
        public string portName { get; set; }
        public int baudRate { get; set; }
        public int dataBits { get; set; }

    }
    public enum ExcutionModule
    {
        LeftModule = 0,
        RightModule = 1,
    }
}
