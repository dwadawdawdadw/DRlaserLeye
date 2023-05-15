using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DeviceManeger
{
    public enum ResultCode
    {
        [Description("成功")]
        Success = 0,
        [Description("PLC连接失败")]
        PlcConnectFailed = -1,
        [Description("读取成功")]
        ReadPlcSucess = 1,
        [Description("读取失败")]
        ReadPlcFailed = -2,
        [Description("读取超时")]
        ReadPlcTimeOut = 2,
        [Description("读取异常")]
        ReadPlcError = 3,
        [Description("配置文件异常")]
        ConfigFileError = 4,
        [Description("操作失败")]
        OperateFiled = 5,
        [Description("操作失败")]
        SerialComOpenFiled = 6,
    }
}
