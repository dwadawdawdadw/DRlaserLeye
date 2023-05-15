using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opc.Ua;
using Opc.Ua.Client;
using OpcUaHelper;

namespace MesOpcServer
{
    class OpcUaReadWrite
    {
        #region 私有变量

        OpcUaClient opcUaClient = new OpcUaClient();
        //心跳信号每隔500毫秒跳变一次
        System.Timers.Timer TaskTimer = new System.Timers.Timer(500);
        bool HeartBeatValue;

        #endregion 私有变量

        #region 单例模式
        private static OpcUaReadWrite opcUaReadWrite = new OpcUaReadWrite();

        private OpcUaReadWrite()
        {
            Connect();


            TaskTimer.Elapsed += TaskTimer_Elapsed;
            TaskTimer.AutoReset = true;
            TaskTimer.Enabled = true;
        }

        public static OpcUaReadWrite getInstance()
        {
            return opcUaReadWrite;
        }

        #endregion 单例模式

        #region 私有方法

        void TaskTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //以下代码有两个作用，一方面断线可自动重连，一方面提供了设备心跳，地址需要依据实际做改变
            ConnectionHandler("ns=2;s=DRLaser/OpcHeartBeat");
        }


        void Connect()
        {
            try
            {
                opcUaClient.UserIdentity = new UserIdentity("admin", "");
                opcUaClient.ConnectServer("opc.tcp://127.0.0.1:9347");
            }
            catch
            {

            }
        }

        void ConnectionHandler(string NodeID)
        {
            HeartBeatValue = !HeartBeatValue;

            try
            {
                opcUaClient.WriteNode(NodeID, HeartBeatValue);
            }
            catch
            {
                Connect();
            }
        }

        #endregion 私有方法

        #region 写Node的方法
        public void Write(string NodeID, int Value)
        {
            try
            {
                opcUaClient.WriteNode(NodeID, Value);
            }
            catch
            {
            }

        }

        public void Write(string NodeID, ushort Value)
        {
            try
            {
                opcUaClient.WriteNode(NodeID, Value);
            }
            catch
            {
            }

        }

        public void Write(string NodeID, uint Value)
        {
            try
            {
                opcUaClient.WriteNode(NodeID, Value);
            }
            catch
            {
            }
        }

        public void Write(string NodeID, double Value)
        {
            try
            {
                opcUaClient.WriteNode(NodeID, Value);
            }
            catch
            {
            }

        }

        public void Write(string NodeID, string Value)
        {
            try
            {
                opcUaClient.WriteNode(NodeID, Value);
            }
            catch
            {
            }

        }

        public void Write(string NodeID, bool Value)
        {
            try
            {
                opcUaClient.WriteNode(NodeID, Value);
            }
            catch
            {
            }

        }

        public void Write(string NodeID, List<string> Value)
        {
            try
            {
                opcUaClient.WriteNode(NodeID, Value);
            }
            catch
            {
            }

        }

        public void Write(string NodeID, List<double> Value)
        {
            try
            {
                opcUaClient.WriteNode(NodeID, Value);
            }
            catch
            {
            }

        }

        public void Write(string NodeID, string[] Value)
        {
            try
            {
                opcUaClient.WriteNode(NodeID, Value);
            }
            catch
            {
            }

        }

        public void Write(string NodeID, int[] Value)
        {
            try
            {
                opcUaClient.WriteNode(NodeID, Value);
            }
            catch
            {
            }

        }

        public void Write(string NodeID, List<int> Value)
        {
            try
            {
                opcUaClient.WriteNode(NodeID, Value);
            }
            catch
            {
            }

        }

        #endregion 写Node的方法

        #region 读Node的方法
        public int Read(string NodeID, int FailValue)
        {
            int Value;

            try
            {
                Value = opcUaClient.ReadNode<int>(NodeID);
                return Value;
            }
            catch
            {
                return FailValue;
            }


        }

        public ushort Read(string NodeID, ushort FailValue)
        {
            ushort Value;

            try
            {
                Value = opcUaClient.ReadNode<ushort>(NodeID);
                return Value;
            }
            catch
            {
                return FailValue;
            }


        }

        public uint Read(string NodeID, uint FailValue)
        {
            uint Value;

            try
            {
                Value = opcUaClient.ReadNode<uint>(NodeID);
                return Value;
            }
            catch
            {
                return FailValue;
            }


        }

        public double Read(string NodeID, double FailValue)
        {
            double Value;

            try
            {
                Value = opcUaClient.ReadNode<double>(NodeID);
                return Value;
            }
            catch
            {
                return FailValue;
            }


        }

        public string Read(string NodeID, string FailValue)
        {
            string Value;

            try
            {
                Value = opcUaClient.ReadNode<string>(NodeID);
                return Value;
            }
            catch
            {
                return FailValue;
            }


        }


        public bool Read(string NodeID, bool FailValue)
        {
            bool Value;

            try
            {
                Value = opcUaClient.ReadNode<bool>(NodeID);
                return Value;
            }
            catch
            {
                return FailValue;
            }

        }

        public string[] Read(string NodeID, string[] FailValue)
        {
            string[] Value;

            try
            {
                Value = opcUaClient.ReadNode<string[]>(NodeID);
                return Value;
            }
            catch
            {
                return FailValue;
            }

        }

        public int[] Read(string NodeID, int[] FailValue)
        {
            int[] Value;

            try
            {
                Value = opcUaClient.ReadNode<int[]>(NodeID);
                return Value;
            }
            catch
            {
                return FailValue;
            }

        }

        #endregion 读Node的方法

    }
}
