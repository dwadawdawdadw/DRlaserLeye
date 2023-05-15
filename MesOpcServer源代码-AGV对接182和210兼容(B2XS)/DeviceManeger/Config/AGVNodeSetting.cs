using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceManeger.Config
{
    public class AGVNodeSetting
    {
        public byte[] SendHandShakeUnload50;
        public byte[] SendHandShakeUnload05;
        public byte[] SendHandShakeUnload55;
        public byte[] SendLoadingUnload50;
        public byte[] SendLoadingUnload05;
        public byte[] SendLoadingUnload55;
        public byte[] SendReceivedUnload50;
        public byte[] SendReceivedUnload05;
        public byte[] SendReceivedUnload55;
        public byte[] SendLeave;
        public byte[] SendCRCError;
        public byte[] ErrorTrackState;
        public byte[] ErrorQueryState;
        public byte[] SendIDError;
    }
}
