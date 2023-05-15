using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyModbus;

namespace DeviceManeger
{
    public interface AgvLinker : CommunicateInterFace
    {
        #region 成员
        //private readonly object _syncRoot = new object();
        //public bool AGVIsOpen { get; private set; }
        //public string Name { get; set; }
        #endregion
        #region 属性
        //public AgvLinker()
        //{
        //    AGVIsOpen = false;
        //}
        #endregion
        #region 方法
        /// <summary>
        /// 连接AGV
        /// </summary>
        /// <param name="mes"></param>
        /// <returns></returns>
        ResultCode Connect(SystemSetting mes);
        /// <summary>
        /// 断开AGV
        /// </summary>
        /// <returns></returns>
        ResultCode DisConnect();
        /// <summary>
        /// 读Agv
        /// </summary>
        /// <returns></returns>
        ResultCode Read();
        /// <summary>
        /// 写agv
        /// </summary>
        /// <param name="absMsg"></param>
        /// <returns></returns>
        ResultCode Write(AbstractMessage absMsg);
        void Dispose();

        #endregion
    }
    public enum AgvCommunicatType
    {
        AgvOPCUA,
        TCP,
        AgvModbus,
        AgvCom
    }
    public interface AgvModel
    {

    }
}
