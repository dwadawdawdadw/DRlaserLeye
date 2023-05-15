using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace DeviceManeger.AGVCommunication
{
    public class AgvCom : AgvLinker
    {
        #region 成员
        private SerialPort _serialPort;
        public bool AgvComIsConnect = false;
        private string portName;
        private int baudRate;
        private int dataBits;
        private readonly object _disposingLock = new object();
        private bool _isDisposed = false;
        #endregion
        #region 构造函数

        #endregion
        #region 方法

        #endregion
        public ResultCode Connect(SystemSetting absMsg)
        {
            this.portName = absMsg.AgvComSetting.portName;
            this.baudRate = absMsg.AgvComSetting.baudRate;
            this.dataBits = absMsg.AgvComSetting.dataBits;
            Connect(portName, baudRate, dataBits);
            return ResultCode.Success;
        }
        public ResultCode Connect(string portName, int baudRate, int dataBits)
        {
            try
            {
                if (_serialPort == null)
                {
                    _serialPort = new SerialPort(portName);
                    _serialPort.BaudRate = baudRate;
                    _serialPort.DataBits = dataBits;
                    _serialPort.Parity = System.IO.Ports.Parity.None;
                    _serialPort.StopBits = System.IO.Ports.StopBits.One;
                }
                _serialPort.Open();
                AgvComIsConnect = true;
                return ResultCode.Success;
            }
            catch (Exception ex)
            {
                return ResultCode.SerialComOpenFiled;
            }
        }
        public ResultCode DisConnect()
        {

            AgvComIsConnect = false;
            return ResultCode.Success;
        }
        public ResultCode Read()
        {
            return ResultCode.Success;
        }
        public ResultCode Write(AbstractMessage absMes)
        {
            return ResultCode.Success;
        }
        /// <summary>
        /// PLC连接属于非托管资源，需手动释放
        /// </summary>
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

                if (disposing)
                {

                }
            }
        }
    }
}
