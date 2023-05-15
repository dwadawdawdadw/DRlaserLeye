using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxActUtlTypeLib;

namespace DeviceManeger
{
    public class PlcLinker : IEquipments
    {
        #region 成员
        public PlcLinker plcLinker { get; set; }
        private AxActUtlTypeLib.AxActUtlType axActUtlType1;
        private readonly object _disposingLock = new object();
        private bool _isDisposed = false;

        #endregion
       
        #region 初始化
        public ResultCode Init()
        {
            return ResultCode.Success;
        }
        #endregion
        #region 方法
      
        /// <summary>
        /// plc初始化
        /// </summary>
        /// <returns></returns>
        public ResultCode Init(PlcType type, object t)
        {
            //switch (type)
            //{
            //    case PlcType.Mitsubishi:
            //        //初始化PLC连接
            //        try
            //        {
            //            axActUtlType1 = (AxActUtlTypeLib.AxActUtlType)t;
            //            axActUtlType1.ActLogicalStationNumber = 2;
            //            int ret = axActUtlType1.Open();
            //            if (ret == 0)
            //            {
            //                //  LED_PLC.Value = true;
            //            }
            //            else
            //            {
            //                // LED_PLC.Value = false;
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine(ex);
            //            //LED_PLC.Value = false;
            //            //AddListMsg("自动重连接驳台PLC失败：" + ex.Message);
            //        }
            //        break;
            //    case PlcType.ModbusTCP:

            //        break;
            //    case PlcType.SerialCOM:

            //        break;
            //    case PlcType.TwinCAT:

            //        break;
            //}
            return ResultCode.Success;
        }

        /// <summary>
        /// 开始工作
        /// </summary>
        /// <returns></returns>
        public ResultCode Start()
        {
            return ResultCode.Success;

        }
        /// <summary>
        /// 停止工作
        /// </summary>
        /// <returns></returns>
        public ResultCode Stop()
        {
            return ResultCode.Success;

        }
        public AbstractMessage GetMessage()
        {
            return null;
        }
        public ResultCode SetMessage(AbstractMessage message)
        {

            return ResultCode.Success;
        }
        /// <summary>
        /// PLC心跳检测
        /// </summary>
        public void HeartBeatDetection()
        {

        }
        #endregion
    }
    public enum PlcType
    {
        TwinCAT,
        SerialCOM,
        ModbusTCP,
        Mitsubishi
    }
}
