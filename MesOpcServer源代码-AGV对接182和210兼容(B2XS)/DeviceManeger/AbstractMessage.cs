using System;
using System.Collections.Generic;

namespace DeviceManeger
{
    /// <summary>
    /// 抽象消息类
    /// </summary>
    public abstract class AbstractMessage
    {

    }

    /// <summary>
    /// 用户指令类
    /// </summary>
    public class MessageUTF8 : AbstractMessage, ICloneable
    {
        public char cmd { set; get; }
        public string context { set; get; }
        public string sender { set; get; }
        public string receiver { set; get; }
        public int parkingRecordsID { set; get; }
        public int garageID { get; set; }

        public MessageUTF8()
        {

        }
        public MessageUTF8(char cmd, string context, string sender, string receiver, int garageID, int parkingRecordsID)
        {
            this.cmd = cmd;
            this.context = context;
            this.sender = sender;
            this.receiver = receiver;
            this.garageID = garageID;
            this.parkingRecordsID = parkingRecordsID;
        }

        public object Clone()
        {
            if (this == null)
                return null;
            MessageUTF8 webMsg = new MessageUTF8(this.cmd, this.context, this.sender, this.receiver, this.garageID, this.parkingRecordsID);
            return webMsg;
        }
    }

    public class AGVNode : AbstractMessage
    {
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        public byte[] ShakeHandNode { get; set; }

    }
    /// <summary>
    /// Agv消息类
    /// </summary>
    public class AgvcComNode : AbstractMessage
    {

        public string PortName { get; set; }
        public string BaudRate { get; set; }
        public string DataBits { get; set; }
        public string Parity { get; set; }
        public string StopBits { get; set; }
    }
    /// <summary>
    /// RFID消息类
    /// </summary>
    public class RFIDnode : AbstractMessage
    {
        public string Cmd { get; set; }
    }
    //********************************************* plc ************************************************
    /// <summary>
    /// plc节点类
    /// </summary>
    public class PLCNode : AbstractMessage
    {
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        public PLCNode()
        {
            Address = "";
            Value = "";
        }
        public PLCNode(string addr, string v)
        {
            Address = addr;
            Value = v;
        }

        public override bool Equals(System.Object obj)
        {
            if (this == obj)
                return true;
            if (obj == null || obj.GetType() != this.GetType())
                return false;
            PLCNode p = obj as PLCNode;
            return (Address == p.Address) && (Value == p.Value);
        }

        public override int GetHashCode()
        {
            int hash = 7;
            return 31 * hash + Address.GetHashCode() + Value.GetHashCode();
        }

    }

    /// <summary>
    /// plc消息类
    /// </summary>
    public class PLCMessage : AbstractMessage, ICloneable
    {
        public List<PLCNode> extendedPlcList { get; set; }
        public List<PLCNode> originalPlcList { get; set; }
        public List<LaserMessage> laserMsgList { get; set; }

        public PLCMessage()
        {
            extendedPlcList = new List<PLCNode>();
            originalPlcList = new List<PLCNode>();
            laserMsgList = new List<LaserMessage>();
        }

        public object Clone()
        {
            PLCMessage plcClone = new PLCMessage();
            foreach (PLCNode pn in extendedPlcList)
            {
                plcClone.extendedPlcList.Add(pn);
            }
            foreach (PLCNode pn in originalPlcList)
            {
                plcClone.originalPlcList.Add(pn);
            }
            foreach (LaserMessage lm in laserMsgList)
            {
                plcClone.laserMsgList.Add(lm);
            }
            return plcClone;
        }
    }

    //******************************************* NumMachine *********************************************
    /// <summary>
    /// 号牌机状态枚举
    /// </summary>
    public enum EnumNumberMachineStatus
    {
        Offline, Normal
    }

    /// <summary>
    /// 号牌机节点类
    /// </summary>
    public class NumberMachineNode : ICloneable
    {
        public string ip { get; set; }
        public string LicenseNum { get; set; }
        public string TimeRecord { get; set; }
        public int id;
        public EnumNumberMachineStatus status;
        public NumberMachineNode() { }
        public NumberMachineNode(string ipAddr, int id, string license, string time, int stat)
        {
            SetLic(ipAddr, id, license, time, stat);
        }
        public void SetLic(string ipAddr, int id, string license, string time, int stat)
        {
            ip = ipAddr;
            this.id = id;
            LicenseNum = license;
            TimeRecord = time;
            if (stat == 1)
                status = EnumNumberMachineStatus.Normal;
            else
                status = EnumNumberMachineStatus.Offline;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
                return false;
            if (this == obj)
                return true;
            NumberMachineNode l = (NumberMachineNode)obj;
            return (l.LicenseNum == LicenseNum);
        }
        public override int GetHashCode()
        {
            return ip.GetHashCode() + LicenseNum.GetHashCode();
        }

        public object Clone()
        {
            if (status == EnumNumberMachineStatus.Normal)
                return new NumberMachineNode(ip, id, LicenseNum, TimeRecord, 1);
            else
                return new NumberMachineNode(ip, id, LicenseNum, TimeRecord, 0);
        }
    }

    /// <summary>
    /// 号牌机消息类
    /// </summary>
    public class NumberMachineMessage : AbstractMessage
    {
        public List<NumberMachineNode> data { get; set; }
        public NumberMachineNode aNode;

        public NumberMachineMessage()
        {
            data = new List<NumberMachineNode>();
            aNode = new NumberMachineNode();
        }

        public override bool Equals(System.Object obj)
        {
            if (this == obj)
                return true;
            if (obj == null || obj.GetType() != this.GetType())
                return false;
            NumberMachineMessage n = obj as NumberMachineMessage;
            return (data.Equals(n.data));
        }
        public override int GetHashCode()
        {
            int hash = 7;
            return 31 * hash + data.GetHashCode();
        }
    }

    //********************************************** laser ************************************************
    /// <summary>
    /// 激光消息类
    /// </summary>
    public class LaserMessage : AbstractMessage, ICloneable
    {
        public int id { get; set; }
        public int status { get; set; }
        public bool recorded { get; set; }
        public bool abort_rescan { get; set; }
        public bool occupied { get; set; }
        public string licenseNum { get; set; }
        public Data data;
        public LaserMessage()
        {
            data = new Data();
            licenseNum = "";
        }
        public LaserMessage(int id, int status)
        {
            this.id = id;
            this.status = status;
            abort_rescan = false;
            data = new Data();
        }

        public object Clone()
        {
            LaserMessage lm = new LaserMessage();
            lm.id = id;
            lm.status = status;
            lm.data = (Data)data.Clone();
            lm.recorded = recorded;
            lm.abort_rescan = abort_rescan;
            lm.licenseNum = licenseNum;
            return lm;
        }
    }

    /// <summary>
    /// 激光数据类
    /// </summary>
    public class Data : ICloneable
    {
        public int centerX { get; set; }
        public int centerY { get; set; }
        public int angleA { get; set; }
        public int length { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public Data() : this(0, 0, 0, 0, 0, 0) { }
        public Data(int cx, int cy, int aa, int l, int w, int h)
        {
            centerX = cx;
            centerY = cy;
            angleA = aa;
            length = l;
            width = w;
            height = h;
        }

        public object Clone()
        {
            Data d = new Data(centerX, centerY, angleA, length, width, height);
            return d;
        }
    }

    //********************************************** command **********************************************
    /// <summary>
    /// 命令类，由队列线程处理号牌与指令后产生
    /// </summary>
    public class Command : AbstractMessage, ICloneable
    {
        public char commandType { get; set; }
        public string LicenseNum { get; set; }
        public string userID { get; set; }
        public int garageID { get; set; }
        public int parkingRecordsID { get; set; }
        public string TimeRecord { get; set; }
        public string ip { get; set; }//新添加，用于定位号牌机
        public int returnedCount { get; set; }//标记被返回的命令
        public int id { get; set; }
        public bool manual { get; set; }//判断是否手动停取

        public Command()
        {
            LicenseNum = "";
            TimeRecord = "";
            userID = "";
            garageID = 0;
            parkingRecordsID = 0;
            TimeRecord = "";
            parkingRecordsID = 0;
            ip = "";
            returnedCount = 0;
            id = 0;
            manual = false;
        }

        public object Clone()
        {
            Command cmdClone = new Command();
            cmdClone.commandType = commandType;
            cmdClone.LicenseNum = LicenseNum;
            cmdClone.userID = userID;
            cmdClone.garageID = garageID;
            cmdClone.parkingRecordsID = parkingRecordsID;
            cmdClone.TimeRecord = TimeRecord;
            cmdClone.parkingRecordsID = parkingRecordsID;
            cmdClone.ip = ip;
            cmdClone.id = id;
            cmdClone.returnedCount = returnedCount;
            cmdClone.manual = manual;
            return cmdClone;
        }
    }

    /// <summary>
    /// 控制信息类，核心在不同阶段发至plc
    /// 1: 停车startLaser--park_command_address
    /// 2: 停车激光的6个数据,startRobot,车位信息4个
    /// 3：停车完成，归零--park_completed_address
    /// 4：取车startRobot,车位信息4个
    /// 5: 取车完成，归零-fetch_completed_address
    /// </summary>
    public class ControlMessage : AbstractMessage
    {
        public int status { get; set; }
        public string LicenseNum { get; set; }
        public int laserID { get; set; }//激光地址
        public string parkingSpaceID { get; set; }
        public string parkingSpaceX { get; set; }
        public string parkingSpaceY { get; set; }
        public string parkingSpaceZ { get; set; }
        public string centerX { get; set; }
        public string centerY { get; set; }
        public string angleA { get; set; }
        public string length { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public int fetchPosition { get; set; }//取车放置位置（临时缓冲位）
        public int RobotID { get; set; }//机械手编号
        public int frontWheelbase { get; set; }
        public int rearWheelbase { get; set; }
    }
}
