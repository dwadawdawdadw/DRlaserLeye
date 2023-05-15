using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opc.Ua;
using OpcUaHelper;

namespace DeviceManeger.AGVCommunication
{
    public class AgvOPCUA : AgvLinker, IDisposable
    {
        #region
        private readonly object _disposingLock = new object();
        private bool _isDisposed = false;
        private OpcUaClient opcUaClient = new OpcUaHelper.OpcUaClient();

        #endregion
        /// <summary>
        /// 建立OPC连接
        /// </summary>
        /// <param name="mes"></param>
        /// <returns></returns>
        public ResultCode Connect(SystemSetting mes)
        {
            //用户及密码验证
            opcUaClient.UserIdentity = new UserIdentity("admin", "d2008r");
            opcUaClient.ReconnectPeriod = 5;
            try
            {
                opcUaClient.ConnectServer("opc.tcp://0.0.0.0:9347");
                if (opcUaClient.Connected)
                {
                    //MonitorSet();
                    //    AddMsg("连接OPCUA服务器" + IP + "成功", MessageType.Info);
                }
                else
                {
                    //     AddMsg("连接OPCUA服务器" + IP + "失败", MessageType.Alarm);
                }
            }
            catch (Exception ex)
            {
                //  AddMsg("连接OPCUA服务器" + IP + "失败", MessageType.Alarm);
            }
            return ResultCode.Success;
        }
        public ResultCode DisConnect()
        {
            return ResultCode.Success;
        }
        public ResultCode Read()
        {


            return ResultCode.Success; 
        }
        public ResultCode Write(AbstractMessage absMsg)
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
