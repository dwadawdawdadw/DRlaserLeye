using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceManeger
{
    public interface CommunicateInterFace
    {
        ResultCode Connect(SystemSetting connectMes);
        ResultCode DisConnect();
        ResultCode Read();
        ResultCode Write(AbstractMessage absMes);
    }
}
