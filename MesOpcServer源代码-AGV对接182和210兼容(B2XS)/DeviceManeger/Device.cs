using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceManager;
using DeviceManeger.Config;
using DeviceManeger.AGVCommunication;
using System.Reflection;

namespace DeviceManeger
{
    public class Device : IDisposable
    {
        #region 成员
        private static Device _device;
        private readonly object _disposingLock = new object();
        private static readonly object _syncRoot = new object();
        private bool _isDisposed = false;
        private readonly string _configDirPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Config\";
        public PlcLinker plcLinker { get; set; }
        public SystemSetting SystemSettings { get; set; }
        public AGVNodeSetting AgvSetting { get; set; }
        public AgvLinker agvLinker { get; set; }
        public RFIDLinker rfidLinker { get; set; }
        public AxActUtlTypeLib.AxActUtlType curMX { get; set; }
        #endregion

        #region 构造函数和析构函数
        public Device()
        {
            AGVComForm agvForm = new AGVComForm();
            plcLinker = new PlcLinker();

            //AgvLinker = new Modbus();
            rfidLinker = new RFIDLinker();
        }


        public static Device GetDevice()
        {
            lock (_syncRoot)
            {
                if (_device == null)
                    _device = new Device();

                return _device;
            }
        }
        ~Device()
        {
            Dispose(false);
        }

        #endregion
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            lock (_disposingLock)
            {
                if (_isDisposed)
                    return;

                //// ResetAlarmOrWarning();

                // if (IO != null)
                // {

                // }

                // SPReaders?.ForEach(t => t?.Dispose());
                // MotionCardMgr?.Dispose();
                // Cameras?.ForEach(t => t?.Dispose());
                // Measuring?.Dispose();

                if (disposing)
                {

                }
            }
        }
        /// <summary>
        /// 所有设备初始化
        /// </summary>
        /// <returns></returns>
        public ResultCode EquipmentInit(AxActUtlTypeLib.AxActUtlType formMX)
        {
            #region PLC初始化
            this.curMX = formMX;
            //  plcLinker.Init();
            List<byte[]> lsit = new List<byte[]>();
            //如果PLC为三菱PLC则传入控件进行初始化
            if (SystemSettings.PLCType == PlcType.Mitsubishi)
            {
                plcLinker.Init(SystemSettings.PLCType, curMX);
            }
            else
            {
                plcLinker.Init(SystemSettings.PLCType, null);
            }
            #endregion

            #region AGV初始化
            //AGV根据配置文件映射初始化
            Assembly assembly = Assembly.Load("DeviceManeger"); //程序集名称
            //从程序集中获取指定对象类型;
            Type type = assembly.GetType("DeviceManeger.AGVCommunication." + SystemSettings.AgvType.ToString());
            object instance = Activator.CreateInstance(type);
            // Object obj = type.Assembly.CreateInstance(type.ToString());
            agvLinker = (AgvLinker)instance;
            agvLinker.Connect(SystemSettings);
            #endregion

            #region RFID初始化
            rfidLinker.Init();
            #endregion

            return ResultCode.Success;
        }
        /// <summary>
        /// 配置文件初始化
        /// </summary>
        /// <returns></returns>
        public ResultCode SettingInit()
        {
            SystemSettings = ConfigUtility.Load<SystemSetting>(_configDirPath + "SystemSettings.json");
            AgvSetting = ConfigUtility.Load<AGVNodeSetting>(_configDirPath + "AGVSettings.json");
            return ResultCode.Success;
        }
    }
}
