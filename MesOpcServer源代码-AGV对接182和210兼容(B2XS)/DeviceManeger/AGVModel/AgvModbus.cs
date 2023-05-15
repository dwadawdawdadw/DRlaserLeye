using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyModbus;

namespace DeviceManeger.AGVCommunication
{
    public class AgvModbus : AgvLinker
    {
        #region 成员
        public ModbusServer ModbusServer;
        public string Name;
        /// <summary>
        /// 是否将RFID数据写入modbus
        /// </summary>
        public bool IsWriteModbus = false;
        /// <summary>
        /// 
        /// </summary>
        bool _IsAGVOnLine = false;
        private readonly object _disposingLock = new object();
        private bool _isDisposed = false;
        #endregion


        #region 属性
        public AgvModbus()
        {
            ModbusServer = new ModbusServer();
        }
        /// <summary>
        /// AGV连接状态
        /// </summary>
        public bool AGVHeart_Modbus
        {
            get
            {
                return _IsAGVOnLine;
            }
        }
        #endregion
        #region 方法
        public ResultCode Connect(SystemSetting absMsg)
        {
            ModbusServer.Listen();
            return ResultCode.Success;
        }
        public ResultCode DisConnect()
        {
            ModbusServer.Listen();
            return ResultCode.Success;
        }
        /// <summary>
        /// 读取寄存器值
        /// </summary>
        public ResultCode GetAGVData()
        {
            return ResultCode.Success;
        }
        /// <summary>
        /// Modbus读服务器
        /// </summary>
        /// <param name="writeValue"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public ResultCode Read()
        {

            return ResultCode.Success;
        }
        public ResultCode Write(AbstractMessage absMsg)
        {
            return ResultCode.Success;
        }
        /// <summary>
        /// Modbus写服务器
        /// </summary>
        /// <param name="writeValue"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public ResultCode Write(int address, short writeValue)
        {
            ModbusServer.holdingRegisters.localArray[address] = writeValue;
            return ResultCode.Success;
        }
        /// <summary>
        /// 解析寄存器值信号
        /// </summary>
        public ResultCode GetAGVPoint()
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
        /// <summary>
        /// 写入寄存器值
        /// </summary>
        /// <param name="add">地址</param>
        /// <param name="val">值</param>
        #endregion
    }
}
