using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceManeger
{
    public class RFIDLinker:IEquipments 
    {
        public AbstractMessage GetMessage()
        {

            return null;
        }
        public ResultCode SetMessage(AbstractMessage message)
        {



            return ResultCode.Success;
        }
        public ResultCode Start()
        {




            return ResultCode.Success;
        }
        public ResultCode Stop()
        {



            return ResultCode.Success;
        }
        public ResultCode Init()
        {




            return ResultCode.Success;
        }
    }
}
