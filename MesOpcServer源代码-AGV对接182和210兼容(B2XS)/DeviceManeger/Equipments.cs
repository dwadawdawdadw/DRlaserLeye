using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeviceManeger
{
    public interface IEquipments
    {
        AbstractMessage GetMessage();
        ResultCode SetMessage(AbstractMessage message);
        ResultCode Start();
        ResultCode Stop();
        ResultCode Init();
    }

    public abstract class Equipments : IEquipments
    {
        //AGV、RFID、PLC设备有没有共有的属性?
        public abstract AbstractMessage GetMessage();
        public abstract ResultCode SetMessage(AbstractMessage message);
        public abstract ResultCode Start();
        public abstract ResultCode Stop();
        public abstract ResultCode Init();
    }
}
