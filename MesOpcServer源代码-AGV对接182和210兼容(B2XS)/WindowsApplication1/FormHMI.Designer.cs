using Advantech.Motion;
using System;
using MicroLibrary;
using System.Threading;
using SGModbus;
namespace WindowsApplication1
{
    partial class FormHMI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);

        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHMI));
            NationalInstruments.Net.DataSocketBinding dataSocketBinding1 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding2 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding3 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding4 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding5 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding6 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding7 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding8 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding9 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding10 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding11 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding12 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding13 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding14 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding15 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding16 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding17 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding18 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding19 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding20 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding21 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding22 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding23 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding24 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding25 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding26 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding27 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding28 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding29 = new NationalInstruments.Net.DataSocketBinding();
            NationalInstruments.Net.DataSocketBinding dataSocketBinding30 = new NationalInstruments.Net.DataSocketBinding();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.Quit = new System.Windows.Forms.Button();
            this.GetObject = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ContextMenu_MainOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenu_LoadOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenu_UnLoadOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.主机ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MeunMainOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuOpenGraph = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuMainPara = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuMainReset = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuMainCorrect = new System.Windows.Forms.ToolStripMenuItem();
            this.上料接驳台ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuLoadOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuLoadParam = new System.Windows.Forms.ToolStripMenuItem();
            this.aGVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.TakeOnePhoto = new System.Windows.Forms.ToolStripButton();
            this.ToolSysReset = new System.Windows.Forms.ToolStripButton();
            this.OpenCCDTool = new System.Windows.Forms.ToolStripButton();
            this.SaveCCDTool = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolKeyPress = new System.Windows.Forms.ToolStripButton();
            this.OpenGraph = new System.Windows.Forms.ToolStripButton();
            this.GotoMM = new System.Windows.Forms.ToolStripButton();
            this.Marking = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.Help = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RelativeY = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.RelativeX = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.APositionY = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.SampleAngle = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.APositionX = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.KEY_LoadHandOut = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.KEY_EndMeasure = new System.Windows.Forms.Button();
            this.KEY_StartMeasure = new System.Windows.Forms.Button();
            this.NUM_PowerData = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.KEY_LaserOnOff = new NationalInstruments.UI.WindowsForms.Switch();
            this.KEY_DD45AngleRun = new NationalInstruments.UI.WindowsForms.Switch();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.CheckBox_LoadOff = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.TotalTime = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.TotalQulity = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.label30 = new System.Windows.Forms.Label();
            this.Capacity = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.KEY_Clean = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.MarkTime = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.Clean = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.NoCentreAlarm = new System.Windows.Forms.CheckBox();
            this.CheckBox_NoCheck = new System.Windows.Forms.CheckBox();
            this.label21 = new System.Windows.Forms.Label();
            this.AutoPause = new NationalInstruments.UI.WindowsForms.Switch();
            this.Stop = new System.Windows.Forms.Button();
            this.AutoRun = new System.Windows.Forms.Button();
            this.Modify = new System.Windows.Forms.CheckBox();
            this.LaserCheck = new System.Windows.Forms.CheckBox();
            this.LightONOFF = new System.Windows.Forms.CheckBox();
            this.CCDONOFF = new System.Windows.Forms.CheckBox();
            this.DoorProtect = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.LED_XIN3 = new NationalInstruments.UI.WindowsForms.Led();
            this.label26 = new System.Windows.Forms.Label();
            this.timerRead = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.LED_XIN2 = new NationalInstruments.UI.WindowsForms.Led();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label29 = new System.Windows.Forms.Label();
            this.CT = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.label19 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.KEY_OPTSwitch = new NationalInstruments.UI.WindowsForms.Switch();
            this.LED_YIN3 = new NationalInstruments.UI.WindowsForms.Led();
            this.LED_YIN2 = new NationalInstruments.UI.WindowsForms.Led();
            this.timerBuzzer = new System.Windows.Forms.Timer(this.components);
            this.timerPower = new System.Windows.Forms.Timer(this.components);
            this.LogClass = new System.Windows.Forms.Label();
            this.timerTotal = new System.Windows.Forms.Timer(this.components);
            this.label9 = new System.Windows.Forms.Label();
            this.TextAuthority = new System.Windows.Forms.Label();
            this.timerCheck = new System.Windows.Forms.Timer(this.components);
            this.timerOPT = new System.Windows.Forms.Timer(this.components);
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.MMDirection = new System.Windows.Forms.Label();
            this.axMMMark1 = new AxMMMARKLib.AxMMMark();
            this.axMMStatus1 = new AxMMSTATUSLib.AxMMStatus();
            this.axMMIO1 = new AxMMIOLib.AxMMIO();
            this.axMMEdit1 = new AxMMEDITLib.AxMMEdit();
            this.timerPassBy = new System.Windows.Forms.Timer(this.components);
            this.TextShow = new System.Windows.Forms.Label();
            this.UIN3 = new System.Windows.Forms.Label();
            this.LED_UIN3 = new NationalInstruments.UI.WindowsForms.Led();
            this.axCKVisionCtrl1 = new AxCKVisionCtrlLib.AxCKVisionCtrl();
            this.ZOOM_IN = new System.Windows.Forms.Button();
            this.ZOOM_OUT = new System.Windows.Forms.Button();
            this.ZOOM_FIT = new System.Windows.Forms.Button();
            this.CurrentTable = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.Edit_WaferStatus = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.Text_NoCheck = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.Edit_LoadStep = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.label24 = new System.Windows.Forms.Label();
            this.SignalNeed = new System.Windows.Forms.CheckBox();
            this.label25 = new System.Windows.Forms.Label();
            this.LED_AIN3 = new NationalInstruments.UI.WindowsForms.Led();
            this.label27 = new System.Windows.Forms.Label();
            this.LED_YOUT7 = new NationalInstruments.UI.WindowsForms.Led();
            this.BrokenClear = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.TooSkew = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.label28 = new System.Windows.Forms.Label();
            this.Broken = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.label31 = new System.Windows.Forms.Label();
            this.LoadToughWafer = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.timerAGV = new System.Windows.Forms.Timer(this.components);
            this.CCDSTATUS = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.label32 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.KEY_NoWaferClean = new System.Windows.Forms.Button();
            this.LaserNoWafer = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.dataSocketSource1 = new NationalInstruments.Net.DataSocketSource(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.LED_IN66 = new NationalInstruments.UI.WindowsForms.Led();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RelativeY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RelativeX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.APositionY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SampleAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.APositionX)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_PowerData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.KEY_LaserOnOff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.KEY_DD45AngleRun)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TotalTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TotalQulity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Capacity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MarkTime)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AutoPause)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_XIN3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_XIN2)).BeginInit();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.KEY_OPTSwitch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_YIN3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_YIN2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMMMark1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMMStatus1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMMIO1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMMEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_UIN3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axCKVisionCtrl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Edit_WaferStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Edit_LoadStep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_AIN3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_YOUT7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TooSkew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Broken)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoadToughWafer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CCDSTATUS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LaserNoWafer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSocketSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_IN66)).BeginInit();
            this.SuspendLayout();
            // 
            // openFile
            // 
            this.openFile.FileName = "openFile";
            // 
            // Quit
            // 
            this.Quit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Quit.Location = new System.Drawing.Point(1128, 929);
            this.Quit.Name = "Quit";
            this.Quit.Size = new System.Drawing.Size(97, 34);
            this.Quit.TabIndex = 5;
            this.Quit.Text = "退出";
            this.Quit.UseVisualStyleBackColor = false;
            this.Quit.Click += new System.EventHandler(this.Quit_Click);
            // 
            // GetObject
            // 
            this.GetObject.Location = new System.Drawing.Point(968, 929);
            this.GetObject.Name = "GetObject";
            this.GetObject.Size = new System.Drawing.Size(86, 31);
            this.GetObject.TabIndex = 6;
            this.GetObject.Text = "获取物件";
            this.GetObject.UseVisualStyleBackColor = true;
            this.GetObject.Visible = false;
            this.GetObject.Click += new System.EventHandler(this.GetObject_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContextMenu_MainOpen,
            this.ContextMenu_LoadOpen,
            this.ContextMenu_UnLoadOpen});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(170, 70);
            // 
            // ContextMenu_MainOpen
            // 
            this.ContextMenu_MainOpen.Name = "ContextMenu_MainOpen";
            this.ContextMenu_MainOpen.Size = new System.Drawing.Size(169, 22);
            this.ContextMenu_MainOpen.Text = "主机打开...";
            this.ContextMenu_MainOpen.Click += new System.EventHandler(this.ContextMenu_MainOpen_Click);
            // 
            // ContextMenu_LoadOpen
            // 
            this.ContextMenu_LoadOpen.Name = "ContextMenu_LoadOpen";
            this.ContextMenu_LoadOpen.Size = new System.Drawing.Size(169, 22);
            this.ContextMenu_LoadOpen.Text = "上料接驳台打开...";
            this.ContextMenu_LoadOpen.Click += new System.EventHandler(this.ContextMenu_LoadOpen_Click);
            // 
            // ContextMenu_UnLoadOpen
            // 
            this.ContextMenu_UnLoadOpen.Name = "ContextMenu_UnLoadOpen";
            this.ContextMenu_UnLoadOpen.Size = new System.Drawing.Size(169, 22);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.主机ToolStripMenuItem,
            this.上料接驳台ToolStripMenuItem,
            this.aGVToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1276, 25);
            this.menuStrip1.TabIndex = 18;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 主机ToolStripMenuItem
            // 
            this.主机ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MeunMainOpen,
            this.toolStripSeparator1,
            this.MenuOpenGraph,
            this.MenuMainPara,
            this.MenuMainReset,
            this.MenuMainCorrect});
            this.主机ToolStripMenuItem.Name = "主机ToolStripMenuItem";
            this.主机ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.主机ToolStripMenuItem.Text = "主机";
            // 
            // MeunMainOpen
            // 
            this.MeunMainOpen.Name = "MeunMainOpen";
            this.MeunMainOpen.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.MeunMainOpen.Size = new System.Drawing.Size(154, 22);
            this.MeunMainOpen.Text = "打开界面...";
            this.MeunMainOpen.Click += new System.EventHandler(this.MeunMainOpen_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(151, 6);
            // 
            // MenuOpenGraph
            // 
            this.MenuOpenGraph.Name = "MenuOpenGraph";
            this.MenuOpenGraph.ShowShortcutKeys = false;
            this.MenuOpenGraph.Size = new System.Drawing.Size(154, 22);
            this.MenuOpenGraph.Text = "图形打开...    F5";
            this.MenuOpenGraph.Click += new System.EventHandler(this.MenuOpenGraph_Click);
            // 
            // MenuMainPara
            // 
            this.MenuMainPara.Name = "MenuMainPara";
            this.MenuMainPara.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.MenuMainPara.Size = new System.Drawing.Size(154, 22);
            this.MenuMainPara.Text = "参数设置...";
            this.MenuMainPara.Click += new System.EventHandler(this.MenuMainPara_Click);
            // 
            // MenuMainReset
            // 
            this.MenuMainReset.Name = "MenuMainReset";
            this.MenuMainReset.Size = new System.Drawing.Size(154, 22);
            this.MenuMainReset.Text = "复位";
            this.MenuMainReset.Click += new System.EventHandler(this.MenuMainReset_Click);
            // 
            // MenuMainCorrect
            // 
            this.MenuMainCorrect.Name = "MenuMainCorrect";
            this.MenuMainCorrect.Size = new System.Drawing.Size(154, 22);
            this.MenuMainCorrect.Text = "振镜校正";
            this.MenuMainCorrect.Visible = false;
            this.MenuMainCorrect.Click += new System.EventHandler(this.MenuMainCorrect_Click);
            // 
            // 上料接驳台ToolStripMenuItem
            // 
            this.上料接驳台ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuLoadOpen,
            this.MenuLoadParam});
            this.上料接驳台ToolStripMenuItem.Name = "上料接驳台ToolStripMenuItem";
            this.上料接驳台ToolStripMenuItem.Size = new System.Drawing.Size(80, 21);
            this.上料接驳台ToolStripMenuItem.Text = "上料接驳台";
            // 
            // MenuLoadOpen
            // 
            this.MenuLoadOpen.Name = "MenuLoadOpen";
            this.MenuLoadOpen.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.MenuLoadOpen.Size = new System.Drawing.Size(154, 22);
            this.MenuLoadOpen.Text = "打开界面...";
            this.MenuLoadOpen.Click += new System.EventHandler(this.MenuLoadOpen_Click);
            // 
            // MenuLoadParam
            // 
            this.MenuLoadParam.Name = "MenuLoadParam";
            this.MenuLoadParam.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.MenuLoadParam.Size = new System.Drawing.Size(154, 22);
            this.MenuLoadParam.Text = "参数设置...";
            this.MenuLoadParam.Click += new System.EventHandler(this.MenuLoadParam_Click);
            // 
            // aGVToolStripMenuItem
            // 
            this.aGVToolStripMenuItem.Name = "aGVToolStripMenuItem";
            this.aGVToolStripMenuItem.Size = new System.Drawing.Size(45, 21);
            this.aGVToolStripMenuItem.Text = "AGV";
            this.aGVToolStripMenuItem.Click += new System.EventHandler(this.aGVToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TakeOnePhoto,
            this.ToolSysReset,
            this.OpenCCDTool,
            this.SaveCCDTool,
            this.toolStripSeparator2,
            this.ToolKeyPress,
            this.OpenGraph,
            this.GotoMM,
            this.Marking,
            this.toolStripSeparator3,
            this.toolStripSeparator4,
            this.Help});
            this.toolStrip1.Location = new System.Drawing.Point(0, 25);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1276, 38);
            this.toolStrip1.TabIndex = 19;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // TakeOnePhoto
            // 
            this.TakeOnePhoto.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TakeOnePhoto.Image = ((System.Drawing.Image)(resources.GetObject("TakeOnePhoto.Image")));
            this.TakeOnePhoto.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.TakeOnePhoto.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TakeOnePhoto.Name = "TakeOnePhoto";
            this.TakeOnePhoto.Size = new System.Drawing.Size(29, 35);
            this.TakeOnePhoto.Text = "单次拍照";
            this.TakeOnePhoto.Click += new System.EventHandler(this.TakeOnePhoto_Click);
            // 
            // ToolSysReset
            // 
            this.ToolSysReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolSysReset.Image = ((System.Drawing.Image)(resources.GetObject("ToolSysReset.Image")));
            this.ToolSysReset.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ToolSysReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolSysReset.Name = "ToolSysReset";
            this.ToolSysReset.Size = new System.Drawing.Size(36, 35);
            this.ToolSysReset.Text = "系统复位";
            this.ToolSysReset.Click += new System.EventHandler(this.ToolSysReset_Click);
            // 
            // OpenCCDTool
            // 
            this.OpenCCDTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.OpenCCDTool.Image = ((System.Drawing.Image)(resources.GetObject("OpenCCDTool.Image")));
            this.OpenCCDTool.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.OpenCCDTool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OpenCCDTool.Name = "OpenCCDTool";
            this.OpenCCDTool.Size = new System.Drawing.Size(32, 36);
            this.OpenCCDTool.Text = "相机编辑工具";
            this.OpenCCDTool.Visible = false;
            this.OpenCCDTool.Click += new System.EventHandler(this.OpenCCDTool_Click);
            // 
            // SaveCCDTool
            // 
            this.SaveCCDTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveCCDTool.Image = ((System.Drawing.Image)(resources.GetObject("SaveCCDTool.Image")));
            this.SaveCCDTool.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.SaveCCDTool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveCCDTool.Name = "SaveCCDTool";
            this.SaveCCDTool.Size = new System.Drawing.Size(36, 36);
            this.SaveCCDTool.Text = "保存相机设置";
            this.SaveCCDTool.Visible = false;
            this.SaveCCDTool.Click += new System.EventHandler(this.SaveCCDTool_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 38);
            // 
            // ToolKeyPress
            // 
            this.ToolKeyPress.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolKeyPress.Image = ((System.Drawing.Image)(resources.GetObject("ToolKeyPress.Image")));
            this.ToolKeyPress.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ToolKeyPress.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolKeyPress.Name = "ToolKeyPress";
            this.ToolKeyPress.Size = new System.Drawing.Size(33, 35);
            this.ToolKeyPress.Text = "权限登录";
            this.ToolKeyPress.Click += new System.EventHandler(this.KeyPress_Click);
            // 
            // OpenGraph
            // 
            this.OpenGraph.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.OpenGraph.Image = ((System.Drawing.Image)(resources.GetObject("OpenGraph.Image")));
            this.OpenGraph.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OpenGraph.Name = "OpenGraph";
            this.OpenGraph.Size = new System.Drawing.Size(23, 35);
            this.OpenGraph.Text = "打开雕刻图形";
            this.OpenGraph.Visible = false;
            this.OpenGraph.Click += new System.EventHandler(this.OpenGraph_Click);
            // 
            // GotoMM
            // 
            this.GotoMM.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.GotoMM.Enabled = false;
            this.GotoMM.Image = ((System.Drawing.Image)(resources.GetObject("GotoMM.Image")));
            this.GotoMM.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.GotoMM.Name = "GotoMM";
            this.GotoMM.Size = new System.Drawing.Size(23, 35);
            this.GotoMM.Text = "进入图形编辑环境";
            this.GotoMM.Visible = false;
            this.GotoMM.Click += new System.EventHandler(this.GotoMM_Click);
            // 
            // Marking
            // 
            this.Marking.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Marking.Enabled = false;
            this.Marking.Image = ((System.Drawing.Image)(resources.GetObject("Marking.Image")));
            this.Marking.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Marking.Name = "Marking";
            this.Marking.Size = new System.Drawing.Size(23, 35);
            this.Marking.Text = "激光雕刻";
            this.Marking.Click += new System.EventHandler(this.Marking_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 38);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 38);
            // 
            // Help
            // 
            this.Help.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Help.Image = ((System.Drawing.Image)(resources.GetObject("Help.Image")));
            this.Help.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Help.Name = "Help";
            this.Help.Size = new System.Drawing.Size(23, 35);
            this.Help.Text = "帮助说明";
            this.Help.Click += new System.EventHandler(this.Help_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.groupBox1.Controls.Add(this.RelativeY);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.RelativeX);
            this.groupBox1.Controls.Add(this.APositionY);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.SampleAngle);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.APositionX);
            this.groupBox1.Location = new System.Drawing.Point(17, 773);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(192, 141);
            this.groupBox1.TabIndex = 38;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "拍照";
            // 
            // RelativeY
            // 
            this.RelativeY.BackColor = System.Drawing.SystemColors.Control;
            this.RelativeY.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(3);
            this.RelativeY.InteractionMode = NationalInstruments.UI.NumericEditInteractionModes.Indicator;
            this.RelativeY.Location = new System.Drawing.Point(108, 75);
            this.RelativeY.Name = "RelativeY";
            this.RelativeY.Size = new System.Drawing.Size(64, 21);
            this.RelativeY.TabIndex = 58;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 12);
            this.label1.TabIndex = 55;
            this.label1.Text = "Y";
            // 
            // RelativeX
            // 
            this.RelativeX.BackColor = System.Drawing.SystemColors.Control;
            this.RelativeX.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(3);
            this.RelativeX.InteractionMode = NationalInstruments.UI.NumericEditInteractionModes.Indicator;
            this.RelativeX.Location = new System.Drawing.Point(108, 34);
            this.RelativeX.Name = "RelativeX";
            this.RelativeX.Size = new System.Drawing.Size(64, 21);
            this.RelativeX.TabIndex = 56;
            // 
            // APositionY
            // 
            this.APositionY.BackColor = System.Drawing.SystemColors.Control;
            this.APositionY.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(3);
            this.APositionY.InteractionMode = NationalInstruments.UI.NumericEditInteractionModes.Indicator;
            this.APositionY.Location = new System.Drawing.Point(20, 75);
            this.APositionY.Name = "APositionY";
            this.APositionY.Size = new System.Drawing.Size(64, 21);
            this.APositionY.TabIndex = 54;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(67, 102);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 55;
            this.label6.Text = "偏移角度";
            // 
            // SampleAngle
            // 
            this.SampleAngle.BackColor = System.Drawing.SystemColors.Control;
            this.SampleAngle.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(3);
            this.SampleAngle.InteractionMode = NationalInstruments.UI.NumericEditInteractionModes.Indicator;
            this.SampleAngle.Location = new System.Drawing.Point(62, 115);
            this.SampleAngle.Name = "SampleAngle";
            this.SampleAngle.Size = new System.Drawing.Size(64, 21);
            this.SampleAngle.TabIndex = 54;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(25, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(11, 12);
            this.label7.TabIndex = 53;
            this.label7.Text = "X";
            // 
            // APositionX
            // 
            this.APositionX.BackColor = System.Drawing.SystemColors.Control;
            this.APositionX.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(3);
            this.APositionX.InteractionMode = NationalInstruments.UI.NumericEditInteractionModes.Indicator;
            this.APositionX.Location = new System.Drawing.Point(20, 34);
            this.APositionX.Name = "APositionX";
            this.APositionX.Size = new System.Drawing.Size(64, 21);
            this.APositionX.TabIndex = 20;
            // 
            // KEY_LoadHandOut
            // 
            this.KEY_LoadHandOut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.KEY_LoadHandOut.Cursor = System.Windows.Forms.Cursors.Hand;
            this.KEY_LoadHandOut.Enabled = false;
            this.KEY_LoadHandOut.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.KEY_LoadHandOut.Location = new System.Drawing.Point(24, 29);
            this.KEY_LoadHandOut.Name = "KEY_LoadHandOut";
            this.KEY_LoadHandOut.Size = new System.Drawing.Size(91, 31);
            this.KEY_LoadHandOut.TabIndex = 39;
            this.KEY_LoadHandOut.Text = "上料出花篮";
            this.KEY_LoadHandOut.UseVisualStyleBackColor = false;
            this.KEY_LoadHandOut.Click += new System.EventHandler(this.LoadHandOut_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.groupBox2.Controls.Add(this.KEY_EndMeasure);
            this.groupBox2.Controls.Add(this.KEY_StartMeasure);
            this.groupBox2.Controls.Add(this.NUM_PowerData);
            this.groupBox2.Location = new System.Drawing.Point(207, 773);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(173, 141);
            this.groupBox2.TabIndex = 43;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "功率测量";
            // 
            // KEY_EndMeasure
            // 
            this.KEY_EndMeasure.BackColor = System.Drawing.Color.Green;
            this.KEY_EndMeasure.Cursor = System.Windows.Forms.Cursors.Hand;
            this.KEY_EndMeasure.Enabled = false;
            this.KEY_EndMeasure.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.KEY_EndMeasure.Location = new System.Drawing.Point(94, 35);
            this.KEY_EndMeasure.Name = "KEY_EndMeasure";
            this.KEY_EndMeasure.Size = new System.Drawing.Size(68, 31);
            this.KEY_EndMeasure.TabIndex = 90;
            this.KEY_EndMeasure.Text = "测量结束";
            this.KEY_EndMeasure.UseVisualStyleBackColor = false;
            this.KEY_EndMeasure.Click += new System.EventHandler(this.KEY_EndMeasure_Click);
            // 
            // KEY_StartMeasure
            // 
            this.KEY_StartMeasure.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.KEY_StartMeasure.Cursor = System.Windows.Forms.Cursors.Hand;
            this.KEY_StartMeasure.Enabled = false;
            this.KEY_StartMeasure.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.KEY_StartMeasure.Location = new System.Drawing.Point(11, 35);
            this.KEY_StartMeasure.Name = "KEY_StartMeasure";
            this.KEY_StartMeasure.Size = new System.Drawing.Size(68, 31);
            this.KEY_StartMeasure.TabIndex = 89;
            this.KEY_StartMeasure.Text = "测量开始";
            this.KEY_StartMeasure.UseVisualStyleBackColor = false;
            this.KEY_StartMeasure.Click += new System.EventHandler(this.KEY_StartMeasure_Click);
            // 
            // NUM_PowerData
            // 
            this.NUM_PowerData.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.NUM_PowerData.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.NUM_PowerData.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.NUM_PowerData.ForeColor = System.Drawing.SystemColors.Info;
            this.NUM_PowerData.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(3);
            this.NUM_PowerData.InteractionMode = NationalInstruments.UI.NumericEditInteractionModes.Indicator;
            this.NUM_PowerData.Location = new System.Drawing.Point(51, 93);
            this.NUM_PowerData.Name = "NUM_PowerData";
            this.NUM_PowerData.Size = new System.Drawing.Size(66, 29);
            this.NUM_PowerData.TabIndex = 88;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(149, 82);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(35, 12);
            this.label16.TabIndex = 87;
            this.label16.Text = "LASER";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(191, 106);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(23, 12);
            this.label15.TabIndex = 86;
            this.label15.Text = "OFF";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(127, 106);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(17, 12);
            this.label14.TabIndex = 85;
            this.label14.Text = "ON";
            // 
            // KEY_LaserOnOff
            // 
            this.KEY_LaserOnOff.Location = new System.Drawing.Point(142, 95);
            this.KEY_LaserOnOff.Name = "KEY_LaserOnOff";
            this.KEY_LaserOnOff.OnColor = System.Drawing.Color.Yellow;
            this.KEY_LaserOnOff.Size = new System.Drawing.Size(49, 33);
            this.KEY_LaserOnOff.SwitchStyle = NationalInstruments.UI.SwitchStyle.HorizontalSlide;
            this.KEY_LaserOnOff.TabIndex = 84;
            this.KEY_LaserOnOff.MouseClick += new System.Windows.Forms.MouseEventHandler(this.KEY_LaserOnOff_StateChanged);
            // 
            // KEY_DD45AngleRun
            // 
            this.KEY_DD45AngleRun.Location = new System.Drawing.Point(341, 929);
            this.KEY_DD45AngleRun.Name = "KEY_DD45AngleRun";
            this.KEY_DD45AngleRun.OnColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.KEY_DD45AngleRun.Size = new System.Drawing.Size(97, 54);
            this.KEY_DD45AngleRun.SwitchStyle = NationalInstruments.UI.SwitchStyle.HorizontalSlide3D;
            this.KEY_DD45AngleRun.TabIndex = 57;
            this.KEY_DD45AngleRun.Visible = false;
            this.KEY_DD45AngleRun.StateChanged += new NationalInstruments.UI.ActionEventHandler(this.KEY_DD45AngleRun_StateChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.groupBox3.Controls.Add(this.KEY_LoadHandOut);
            this.groupBox3.Controls.Add(this.CheckBox_LoadOff);
            this.groupBox3.Location = new System.Drawing.Point(610, 773);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(132, 141);
            this.groupBox3.TabIndex = 44;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "接驳台控制";
            // 
            // CheckBox_LoadOff
            // 
            this.CheckBox_LoadOff.AutoSize = true;
            this.CheckBox_LoadOff.Location = new System.Drawing.Point(35, 95);
            this.CheckBox_LoadOff.Name = "CheckBox_LoadOff";
            this.CheckBox_LoadOff.Size = new System.Drawing.Size(66, 16);
            this.CheckBox_LoadOff.TabIndex = 117;
            this.CheckBox_LoadOff.Text = "上料OFF";
            this.CheckBox_LoadOff.UseVisualStyleBackColor = true;
            this.CheckBox_LoadOff.CheckedChanged += new System.EventHandler(this.CheckBox_LoadOff_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.groupBox4.Controls.Add(this.TotalTime);
            this.groupBox4.Controls.Add(this.TotalQulity);
            this.groupBox4.Controls.Add(this.label30);
            this.groupBox4.Controls.Add(this.Capacity);
            this.groupBox4.Controls.Add(this.label23);
            this.groupBox4.Controls.Add(this.label22);
            this.groupBox4.Controls.Add(this.KEY_Clean);
            this.groupBox4.Location = new System.Drawing.Point(737, 773);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(173, 141);
            this.groupBox4.TabIndex = 45;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "产能统计";
            // 
            // TotalTime
            // 
            this.TotalTime.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TotalTime.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.TotalTime.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TotalTime.ForeColor = System.Drawing.SystemColors.Info;
            this.TotalTime.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(0);
            this.TotalTime.InteractionMode = NationalInstruments.UI.NumericEditInteractionModes.Indicator;
            this.TotalTime.Location = new System.Drawing.Point(33, 56);
            this.TotalTime.Name = "TotalTime";
            this.TotalTime.Size = new System.Drawing.Size(79, 29);
            this.TotalTime.TabIndex = 127;
            // 
            // TotalQulity
            // 
            this.TotalQulity.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TotalQulity.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.TotalQulity.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TotalQulity.ForeColor = System.Drawing.SystemColors.Info;
            this.TotalQulity.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(0);
            this.TotalQulity.InteractionMode = NationalInstruments.UI.NumericEditInteractionModes.Indicator;
            this.TotalQulity.Location = new System.Drawing.Point(33, 21);
            this.TotalQulity.Name = "TotalQulity";
            this.TotalQulity.Size = new System.Drawing.Size(79, 29);
            this.TotalQulity.TabIndex = 126;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(10, 106);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(53, 12);
            this.label30.TabIndex = 131;
            this.label30.Text = "实时产能";
            // 
            // Capacity
            // 
            this.Capacity.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Capacity.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.Capacity.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Capacity.ForeColor = System.Drawing.SystemColors.Desktop;
            this.Capacity.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(0);
            this.Capacity.InteractionMode = NationalInstruments.UI.NumericEditInteractionModes.Indicator;
            this.Capacity.Location = new System.Drawing.Point(81, 101);
            this.Capacity.Name = "Capacity";
            this.Capacity.Size = new System.Drawing.Size(60, 29);
            this.Capacity.TabIndex = 130;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(4, 67);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(41, 12);
            this.label23.TabIndex = 129;
            this.label23.Text = "计时：";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(2, 28);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(41, 12);
            this.label22.TabIndex = 128;
            this.label22.Text = "产量：";
            // 
            // KEY_Clean
            // 
            this.KEY_Clean.Cursor = System.Windows.Forms.Cursors.Hand;
            this.KEY_Clean.Location = new System.Drawing.Point(112, 25);
            this.KEY_Clean.Name = "KEY_Clean";
            this.KEY_Clean.Size = new System.Drawing.Size(54, 31);
            this.KEY_Clean.TabIndex = 58;
            this.KEY_Clean.Text = "清 零";
            this.KEY_Clean.UseVisualStyleBackColor = true;
            this.KEY_Clean.Click += new System.EventHandler(this.Clean_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(109, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 60;
            this.label5.Text = "打标时间";
            // 
            // MarkTime
            // 
            this.MarkTime.BackColor = System.Drawing.SystemColors.Control;
            this.MarkTime.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(2);
            this.MarkTime.InteractionMode = NationalInstruments.UI.NumericEditInteractionModes.Indicator;
            this.MarkTime.Location = new System.Drawing.Point(112, 34);
            this.MarkTime.Name = "MarkTime";
            this.MarkTime.Size = new System.Drawing.Size(46, 21);
            this.MarkTime.TabIndex = 59;
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.groupBox5.Controls.Add(this.Clean);
            this.groupBox5.Controls.Add(this.button1);
            this.groupBox5.Controls.Add(this.NoCentreAlarm);
            this.groupBox5.Controls.Add(this.CheckBox_NoCheck);
            this.groupBox5.Controls.Add(this.label21);
            this.groupBox5.Controls.Add(this.AutoPause);
            this.groupBox5.Controls.Add(this.Stop);
            this.groupBox5.Controls.Add(this.AutoRun);
            this.groupBox5.Controls.Add(this.Modify);
            this.groupBox5.Controls.Add(this.LaserCheck);
            this.groupBox5.Controls.Add(this.LightONOFF);
            this.groupBox5.Controls.Add(this.CCDONOFF);
            this.groupBox5.Controls.Add(this.DoorProtect);
            this.groupBox5.Location = new System.Drawing.Point(906, 773);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(346, 141);
            this.groupBox5.TabIndex = 46;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "控制";
            // 
            // Clean
            // 
            this.Clean.AutoSize = true;
            this.Clean.Enabled = false;
            this.Clean.Location = new System.Drawing.Point(194, 117);
            this.Clean.Name = "Clean";
            this.Clean.Size = new System.Drawing.Size(72, 16);
            this.Clean.TabIndex = 130;
            this.Clean.Text = "皮带清理";
            this.Clean.UseVisualStyleBackColor = true;
            this.Clean.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Clean_MouseClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(115, 62);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(97, 34);
            this.button1.TabIndex = 129;
            this.button1.Text = "RFID获取";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // NoCentreAlarm
            // 
            this.NoCentreAlarm.AutoSize = true;
            this.NoCentreAlarm.Checked = true;
            this.NoCentreAlarm.CheckState = System.Windows.Forms.CheckState.Checked;
            this.NoCentreAlarm.Location = new System.Drawing.Point(94, 95);
            this.NoCentreAlarm.Name = "NoCentreAlarm";
            this.NoCentreAlarm.Size = new System.Drawing.Size(84, 16);
            this.NoCentreAlarm.TabIndex = 128;
            this.NoCentreAlarm.Text = "不居中报警";
            this.NoCentreAlarm.UseVisualStyleBackColor = true;
            // 
            // CheckBox_NoCheck
            // 
            this.CheckBox_NoCheck.AutoSize = true;
            this.CheckBox_NoCheck.Location = new System.Drawing.Point(94, 117);
            this.CheckBox_NoCheck.Name = "CheckBox_NoCheck";
            this.CheckBox_NoCheck.Size = new System.Drawing.Size(84, 16);
            this.CheckBox_NoCheck.TabIndex = 117;
            this.CheckBox_NoCheck.Text = "空片不检测";
            this.CheckBox_NoCheck.UseVisualStyleBackColor = true;
            this.CheckBox_NoCheck.CheckedChanged += new System.EventHandler(this.CheckBox_NoCheck_CheckedChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label21.Location = new System.Drawing.Point(241, 41);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(40, 16);
            this.label21.TabIndex = 116;
            this.label21.Text = "暂停";
            // 
            // AutoPause
            // 
            this.AutoPause.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AutoPause.Enabled = false;
            this.AutoPause.Location = new System.Drawing.Point(235, 20);
            this.AutoPause.Name = "AutoPause";
            this.AutoPause.OnColor = System.Drawing.Color.Yellow;
            this.AutoPause.Size = new System.Drawing.Size(97, 54);
            this.AutoPause.SwitchStyle = NationalInstruments.UI.SwitchStyle.HorizontalSlide;
            this.AutoPause.TabIndex = 9;
            this.AutoPause.StateChanged += new NationalInstruments.UI.ActionEventHandler(this.AutoPause_StateChanged);
            // 
            // Stop
            // 
            this.Stop.BackColor = System.Drawing.Color.Red;
            this.Stop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Stop.Enabled = false;
            this.Stop.Font = new System.Drawing.Font("楷体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Stop.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Stop.Location = new System.Drawing.Point(235, 80);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(97, 34);
            this.Stop.TabIndex = 8;
            this.Stop.Text = "停 止(&Esc)";
            this.Stop.UseVisualStyleBackColor = false;
            this.Stop.Click += new System.EventHandler(this.Stop_Click);
            // 
            // AutoRun
            // 
            this.AutoRun.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.AutoRun.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AutoRun.Enabled = false;
            this.AutoRun.Font = new System.Drawing.Font("楷体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AutoRun.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.AutoRun.Location = new System.Drawing.Point(115, 26);
            this.AutoRun.Name = "AutoRun";
            this.AutoRun.Size = new System.Drawing.Size(97, 34);
            this.AutoRun.TabIndex = 6;
            this.AutoRun.Text = "运 行(F1)";
            this.AutoRun.UseVisualStyleBackColor = false;
            this.AutoRun.Click += new System.EventHandler(this.AutoRun_Click);
            // 
            // Modify
            // 
            this.Modify.AutoSize = true;
            this.Modify.Location = new System.Drawing.Point(13, 117);
            this.Modify.Name = "Modify";
            this.Modify.Size = new System.Drawing.Size(48, 16);
            this.Modify.TabIndex = 4;
            this.Modify.Text = "检修";
            this.Modify.UseVisualStyleBackColor = true;
            this.Modify.Visible = false;
            this.Modify.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Modify_MouseClick);
            // 
            // LaserCheck
            // 
            this.LaserCheck.AutoSize = true;
            this.LaserCheck.Location = new System.Drawing.Point(13, 95);
            this.LaserCheck.Name = "LaserCheck";
            this.LaserCheck.Size = new System.Drawing.Size(84, 16);
            this.LaserCheck.TabIndex = 3;
            this.LaserCheck.Text = "无激光检测";
            this.LaserCheck.UseVisualStyleBackColor = true;
            this.LaserCheck.Visible = false;
            this.LaserCheck.CheckedChanged += new System.EventHandler(this.LaserCheck_CheckedChanged);
            // 
            // LightONOFF
            // 
            this.LightONOFF.AutoSize = true;
            this.LightONOFF.Location = new System.Drawing.Point(13, 73);
            this.LightONOFF.Name = "LightONOFF";
            this.LightONOFF.Size = new System.Drawing.Size(60, 16);
            this.LightONOFF.TabIndex = 2;
            this.LightONOFF.Text = "照明灯";
            this.LightONOFF.UseVisualStyleBackColor = true;
            this.LightONOFF.CheckedChanged += new System.EventHandler(this.LightONOFF_CheckedChanged);
            // 
            // CCDONOFF
            // 
            this.CCDONOFF.AutoSize = true;
            this.CCDONOFF.Location = new System.Drawing.Point(13, 52);
            this.CCDONOFF.Name = "CCDONOFF";
            this.CCDONOFF.Size = new System.Drawing.Size(66, 16);
            this.CCDONOFF.TabIndex = 1;
            this.CCDONOFF.Text = "CCD光源";
            this.CCDONOFF.UseVisualStyleBackColor = true;
            this.CCDONOFF.CheckedChanged += new System.EventHandler(this.CCDONOFF_CheckedChanged);
            // 
            // DoorProtect
            // 
            this.DoorProtect.AutoSize = true;
            this.DoorProtect.Location = new System.Drawing.Point(13, 27);
            this.DoorProtect.Name = "DoorProtect";
            this.DoorProtect.Size = new System.Drawing.Size(72, 16);
            this.DoorProtect.TabIndex = 0;
            this.DoorProtect.Text = "无门保护";
            this.DoorProtect.UseVisualStyleBackColor = true;
            this.DoorProtect.Visible = false;
            this.DoorProtect.CheckedChanged += new System.EventHandler(this.DoorProtect_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label2.Location = new System.Drawing.Point(453, 936);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(373, 29);
            this.label2.TabIndex = 48;
            this.label2.Text = "武汉帝尔科技股份有限公司";
            // 
            // LED_XIN3
            // 
            this.LED_XIN3.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.LED_XIN3.Location = new System.Drawing.Point(255, 723);
            this.LED_XIN3.Name = "LED_XIN3";
            this.LED_XIN3.OffColor = System.Drawing.Color.Black;
            this.LED_XIN3.Size = new System.Drawing.Size(50, 50);
            this.LED_XIN3.TabIndex = 50;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(255, 713);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(53, 12);
            this.label26.TabIndex = 78;
            this.label26.Text = "除尘遮挡";
            // 
            // timerRead
            // 
            this.timerRead.Interval = 16;
            this.timerRead.Tick += new System.EventHandler(this.timerRead_Tick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(319, 713);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 80;
            this.label3.Text = "激光检测";
            // 
            // LED_XIN2
            // 
            this.LED_XIN2.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.LED_XIN2.Location = new System.Drawing.Point(318, 723);
            this.LED_XIN2.Name = "LED_XIN2";
            this.LED_XIN2.OffColor = System.Drawing.Color.Black;
            this.LED_XIN2.Size = new System.Drawing.Size(50, 50);
            this.LED_XIN2.TabIndex = 79;
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.groupBox6.Controls.Add(this.label29);
            this.groupBox6.Controls.Add(this.CT);
            this.groupBox6.Controls.Add(this.label19);
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.label16);
            this.groupBox6.Controls.Add(this.MarkTime);
            this.groupBox6.Controls.Add(this.label13);
            this.groupBox6.Controls.Add(this.label12);
            this.groupBox6.Controls.Add(this.KEY_OPTSwitch);
            this.groupBox6.Controls.Add(this.label15);
            this.groupBox6.Controls.Add(this.LED_YIN3);
            this.groupBox6.Controls.Add(this.LED_YIN2);
            this.groupBox6.Controls.Add(this.label14);
            this.groupBox6.Controls.Add(this.KEY_LaserOnOff);
            this.groupBox6.Location = new System.Drawing.Point(378, 773);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(234, 141);
            this.groupBox6.TabIndex = 81;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "激光控制";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(191, 11);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(23, 12);
            this.label29.TabIndex = 125;
            this.label29.Text = "CT ";
            // 
            // CT
            // 
            this.CT.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CT.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.CT.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CT.ForeColor = System.Drawing.SystemColors.Desktop;
            this.CT.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(3);
            this.CT.InteractionMode = NationalInstruments.UI.NumericEditInteractionModes.Indicator;
            this.CT.Location = new System.Drawing.Point(169, 29);
            this.CT.Name = "CT";
            this.CT.Size = new System.Drawing.Size(57, 29);
            this.CT.TabIndex = 124;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(52, 124);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(29, 12);
            this.label19.TabIndex = 88;
            this.label19.Text = "光闸";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(25, 58);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 12);
            this.label13.TabIndex = 82;
            this.label13.Text = "关闭";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(75, 57);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(29, 12);
            this.label12.TabIndex = 81;
            this.label12.Text = "打开";
            // 
            // KEY_OPTSwitch
            // 
            this.KEY_OPTSwitch.Location = new System.Drawing.Point(20, 73);
            this.KEY_OPTSwitch.Name = "KEY_OPTSwitch";
            this.KEY_OPTSwitch.OnColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.KEY_OPTSwitch.Size = new System.Drawing.Size(91, 54);
            this.KEY_OPTSwitch.SwitchStyle = NationalInstruments.UI.SwitchStyle.HorizontalSlide3D;
            this.KEY_OPTSwitch.TabIndex = 56;
            this.KEY_OPTSwitch.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OPTSwitch_StateChanged);
            // 
            // LED_YIN3
            // 
            this.LED_YIN3.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.LED_YIN3.Location = new System.Drawing.Point(22, 22);
            this.LED_YIN3.Name = "LED_YIN3";
            this.LED_YIN3.OffColor = System.Drawing.Color.Black;
            this.LED_YIN3.Size = new System.Drawing.Size(35, 35);
            this.LED_YIN3.TabIndex = 44;
            // 
            // LED_YIN2
            // 
            this.LED_YIN2.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.LED_YIN2.Location = new System.Drawing.Point(71, 21);
            this.LED_YIN2.Name = "LED_YIN2";
            this.LED_YIN2.OffColor = System.Drawing.Color.Black;
            this.LED_YIN2.Size = new System.Drawing.Size(35, 35);
            this.LED_YIN2.TabIndex = 43;
            // 
            // timerBuzzer
            // 
            this.timerBuzzer.Interval = 1000;
            this.timerBuzzer.Tick += new System.EventHandler(this.timerBuzzer_Tick);
            // 
            // timerPower
            // 
            this.timerPower.Interval = 500;
            this.timerPower.Tick += new System.EventHandler(this.timerPower_Tick);
            // 
            // LogClass
            // 
            this.LogClass.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.LogClass.Font = new System.Drawing.Font("楷体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LogClass.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LogClass.Location = new System.Drawing.Point(606, 722);
            this.LogClass.Name = "LogClass";
            this.LogClass.Size = new System.Drawing.Size(662, 31);
            this.LogClass.TabIndex = 82;
            this.LogClass.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timerTotal
            // 
            this.timerTotal.Interval = 1000;
            this.timerTotal.Tick += new System.EventHandler(this.timerTotal_Tick);
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label9.Location = new System.Drawing.Point(5, 710);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(1260, 2);
            this.label9.TabIndex = 90;
            // 
            // TextAuthority
            // 
            this.TextAuthority.AutoSize = true;
            this.TextAuthority.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TextAuthority.ForeColor = System.Drawing.Color.Black;
            this.TextAuthority.Location = new System.Drawing.Point(1071, 25);
            this.TextAuthority.Name = "TextAuthority";
            this.TextAuthority.Size = new System.Drawing.Size(0, 21);
            this.TextAuthority.TabIndex = 91;
            // 
            // timerCheck
            // 
            this.timerCheck.Interval = 2000;
            this.timerCheck.Tick += new System.EventHandler(this.timerCheck_Tick);
            // 
            // timerOPT
            // 
            this.timerOPT.Enabled = true;
            this.timerOPT.Tick += new System.EventHandler(this.timerOPT_Tick);
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label10.Location = new System.Drawing.Point(12, 919);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(1260, 2);
            this.label10.TabIndex = 93;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(640, 483);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(0, 21);
            this.label11.TabIndex = 94;
            // 
            // MMDirection
            // 
            this.MMDirection.AutoSize = true;
            this.MMDirection.ForeColor = System.Drawing.Color.Red;
            this.MMDirection.Location = new System.Drawing.Point(674, 47);
            this.MMDirection.Name = "MMDirection";
            this.MMDirection.Size = new System.Drawing.Size(11, 12);
            this.MMDirection.TabIndex = 96;
            this.MMDirection.Text = ":";
            // 
            // axMMMark1
            // 
            this.axMMMark1.Enabled = true;
            this.axMMMark1.Location = new System.Drawing.Point(648, 134);
            this.axMMMark1.Name = "axMMMark1";
            this.axMMMark1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMMMark1.OcxState")));
            this.axMMMark1.Size = new System.Drawing.Size(624, 515);
            this.axMMMark1.TabIndex = 97;
            // 
            // axMMStatus1
            // 
            this.axMMStatus1.Enabled = true;
            this.axMMStatus1.Location = new System.Drawing.Point(625, 33);
            this.axMMStatus1.Name = "axMMStatus1";
            this.axMMStatus1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMMStatus1.OcxState")));
            this.axMMStatus1.Size = new System.Drawing.Size(17, 13);
            this.axMMStatus1.TabIndex = 89;
            this.axMMStatus1.MarkEnd += new AxMMSTATUSLib._DMMStatusEvents_MarkEndEventHandler(this.axMMStatus1_MarkEnd);
            // 
            // axMMIO1
            // 
            this.axMMIO1.Enabled = true;
            this.axMMIO1.Location = new System.Drawing.Point(606, 28);
            this.axMMIO1.Name = "axMMIO1";
            this.axMMIO1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMMIO1.OcxState")));
            this.axMMIO1.Size = new System.Drawing.Size(13, 10);
            this.axMMIO1.TabIndex = 83;
            // 
            // axMMEdit1
            // 
            this.axMMEdit1.Enabled = true;
            this.axMMEdit1.Location = new System.Drawing.Point(477, 12);
            this.axMMEdit1.Name = "axMMEdit1";
            this.axMMEdit1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMMEdit1.OcxState")));
            this.axMMEdit1.Size = new System.Drawing.Size(34, 30);
            this.axMMEdit1.TabIndex = 3;
            // 
            // timerPassBy
            // 
            this.timerPassBy.Enabled = true;
            this.timerPassBy.Interval = 20;
            this.timerPassBy.Tick += new System.EventHandler(this.timerPassBy_Tick);
            // 
            // TextShow
            // 
            this.TextShow.BackColor = System.Drawing.SystemColors.Highlight;
            this.TextShow.Font = new System.Drawing.Font("楷体", 21.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TextShow.ForeColor = System.Drawing.Color.Red;
            this.TextShow.Location = new System.Drawing.Point(786, 652);
            this.TextShow.Name = "TextShow";
            this.TextShow.Size = new System.Drawing.Size(482, 48);
            this.TextShow.TabIndex = 101;
            this.TextShow.Text = "请先进行系统复位！！";
            this.TextShow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UIN3
            // 
            this.UIN3.AutoSize = true;
            this.UIN3.Location = new System.Drawing.Point(194, 712);
            this.UIN3.Name = "UIN3";
            this.UIN3.Size = new System.Drawing.Size(29, 12);
            this.UIN3.TabIndex = 109;
            this.UIN3.Text = "UIN3";
            // 
            // LED_UIN3
            // 
            this.LED_UIN3.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.LED_UIN3.Location = new System.Drawing.Point(185, 722);
            this.LED_UIN3.Name = "LED_UIN3";
            this.LED_UIN3.OffColor = System.Drawing.Color.Black;
            this.LED_UIN3.Size = new System.Drawing.Size(50, 50);
            this.LED_UIN3.TabIndex = 108;
            // 
            // axCKVisionCtrl1
            // 
            this.axCKVisionCtrl1.Enabled = true;
            this.axCKVisionCtrl1.Location = new System.Drawing.Point(5, 72);
            this.axCKVisionCtrl1.Name = "axCKVisionCtrl1";
            this.axCKVisionCtrl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axCKVisionCtrl1.OcxState")));
            this.axCKVisionCtrl1.Size = new System.Drawing.Size(637, 524);
            this.axCKVisionCtrl1.TabIndex = 110;
            // 
            // ZOOM_IN
            // 
            this.ZOOM_IN.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ZOOM_IN.Location = new System.Drawing.Point(12, 607);
            this.ZOOM_IN.Name = "ZOOM_IN";
            this.ZOOM_IN.Size = new System.Drawing.Size(76, 36);
            this.ZOOM_IN.TabIndex = 111;
            this.ZOOM_IN.Text = "放大";
            this.ZOOM_IN.UseVisualStyleBackColor = true;
            this.ZOOM_IN.Click += new System.EventHandler(this.ZOOM_IN_Click);
            // 
            // ZOOM_OUT
            // 
            this.ZOOM_OUT.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ZOOM_OUT.Location = new System.Drawing.Point(271, 610);
            this.ZOOM_OUT.Name = "ZOOM_OUT";
            this.ZOOM_OUT.Size = new System.Drawing.Size(76, 36);
            this.ZOOM_OUT.TabIndex = 112;
            this.ZOOM_OUT.Text = "缩小";
            this.ZOOM_OUT.UseVisualStyleBackColor = true;
            this.ZOOM_OUT.Click += new System.EventHandler(this.ZOOM_OUT_Click);
            // 
            // ZOOM_FIT
            // 
            this.ZOOM_FIT.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ZOOM_FIT.Location = new System.Drawing.Point(482, 610);
            this.ZOOM_FIT.Name = "ZOOM_FIT";
            this.ZOOM_FIT.Size = new System.Drawing.Size(76, 36);
            this.ZOOM_FIT.TabIndex = 113;
            this.ZOOM_FIT.Text = "适应";
            this.ZOOM_FIT.UseVisualStyleBackColor = true;
            this.ZOOM_FIT.Click += new System.EventHandler(this.ZOOM_FIT_Click);
            // 
            // CurrentTable
            // 
            this.CurrentTable.Location = new System.Drawing.Point(79, 736);
            this.CurrentTable.Name = "CurrentTable";
            this.CurrentTable.Size = new System.Drawing.Size(46, 21);
            this.CurrentTable.TabIndex = 114;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(13, 740);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(65, 12);
            this.label20.TabIndex = 115;
            this.label20.Text = "当前台面：";
            // 
            // Edit_WaferStatus
            // 
            this.Edit_WaferStatus.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Edit_WaferStatus.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Edit_WaferStatus.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Edit_WaferStatus.ForeColor = System.Drawing.SystemColors.Info;
            this.Edit_WaferStatus.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(0);
            this.Edit_WaferStatus.InteractionMode = NationalInstruments.UI.NumericEditInteractionModes.Indicator;
            this.Edit_WaferStatus.Location = new System.Drawing.Point(295, 671);
            this.Edit_WaferStatus.Name = "Edit_WaferStatus";
            this.Edit_WaferStatus.Size = new System.Drawing.Size(86, 29);
            this.Edit_WaferStatus.TabIndex = 116;
            // 
            // Text_NoCheck
            // 
            this.Text_NoCheck.BackColor = System.Drawing.Color.Red;
            this.Text_NoCheck.Font = new System.Drawing.Font("楷体", 21.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Text_NoCheck.ForeColor = System.Drawing.Color.Yellow;
            this.Text_NoCheck.Location = new System.Drawing.Point(720, 72);
            this.Text_NoCheck.Name = "Text_NoCheck";
            this.Text_NoCheck.Size = new System.Drawing.Size(482, 48);
            this.Text_NoCheck.TabIndex = 117;
            this.Text_NoCheck.Text = "上料空片不检测，请及时取消!!";
            this.Text_NoCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Text_NoCheck.Visible = false;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label17.Location = new System.Drawing.Point(405, 651);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(72, 16);
            this.label17.TabIndex = 119;
            this.label17.Text = "LoadStep";
            // 
            // Edit_LoadStep
            // 
            this.Edit_LoadStep.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Edit_LoadStep.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Edit_LoadStep.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Edit_LoadStep.ForeColor = System.Drawing.SystemColors.Info;
            this.Edit_LoadStep.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(0);
            this.Edit_LoadStep.InteractionMode = NationalInstruments.UI.NumericEditInteractionModes.Indicator;
            this.Edit_LoadStep.Location = new System.Drawing.Point(397, 672);
            this.Edit_LoadStep.Name = "Edit_LoadStep";
            this.Edit_LoadStep.Size = new System.Drawing.Size(86, 29);
            this.Edit_LoadStep.TabIndex = 118;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label24.Location = new System.Drawing.Point(292, 652);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(96, 16);
            this.label24.TabIndex = 122;
            this.label24.Text = "WaferStatus";
            // 
            // SignalNeed
            // 
            this.SignalNeed.AutoSize = true;
            this.SignalNeed.Location = new System.Drawing.Point(535, 736);
            this.SignalNeed.Name = "SignalNeed";
            this.SignalNeed.Size = new System.Drawing.Size(72, 16);
            this.SignalNeed.TabIndex = 127;
            this.SignalNeed.Text = "信号模拟";
            this.SignalNeed.UseVisualStyleBackColor = true;
            this.SignalNeed.CheckedChanged += new System.EventHandler(this.SignalNeed_CheckedChanged);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(478, 704);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(53, 24);
            this.label25.TabIndex = 126;
            this.label25.Text = "下游允许\r\n(AIN3)";
            // 
            // LED_AIN3
            // 
            this.LED_AIN3.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.LED_AIN3.Location = new System.Drawing.Point(474, 722);
            this.LED_AIN3.Name = "LED_AIN3";
            this.LED_AIN3.OffColor = System.Drawing.Color.Black;
            this.LED_AIN3.Size = new System.Drawing.Size(50, 50);
            this.LED_AIN3.TabIndex = 125;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(403, 704);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(59, 24);
            this.label27.TabIndex = 124;
            this.label27.Text = "本机Ready\r\n (YOUT7)";
            // 
            // LED_YOUT7
            // 
            this.LED_YOUT7.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.LED_YOUT7.Location = new System.Drawing.Point(405, 722);
            this.LED_YOUT7.Name = "LED_YOUT7";
            this.LED_YOUT7.OffColor = System.Drawing.Color.Black;
            this.LED_YOUT7.Size = new System.Drawing.Size(50, 50);
            this.LED_YOUT7.TabIndex = 123;
            // 
            // BrokenClear
            // 
            this.BrokenClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BrokenClear.Location = new System.Drawing.Point(726, 668);
            this.BrokenClear.Name = "BrokenClear";
            this.BrokenClear.Size = new System.Drawing.Size(54, 31);
            this.BrokenClear.TabIndex = 128;
            this.BrokenClear.Text = "清 零";
            this.BrokenClear.UseVisualStyleBackColor = true;
            this.BrokenClear.Click += new System.EventHandler(this.BrokenClear_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(641, 655);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(53, 12);
            this.label18.TabIndex = 132;
            this.label18.Text = "硅片太偏";
            // 
            // TooSkew
            // 
            this.TooSkew.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TooSkew.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.TooSkew.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TooSkew.ForeColor = System.Drawing.SystemColors.Info;
            this.TooSkew.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(0);
            this.TooSkew.InteractionMode = NationalInstruments.UI.NumericEditInteractionModes.Indicator;
            this.TooSkew.Location = new System.Drawing.Point(624, 670);
            this.TooSkew.Name = "TooSkew";
            this.TooSkew.Size = new System.Drawing.Size(86, 29);
            this.TooSkew.TabIndex = 131;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(542, 655);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(53, 12);
            this.label28.TabIndex = 130;
            this.label28.Text = "破片计数";
            // 
            // Broken
            // 
            this.Broken.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Broken.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Broken.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Broken.ForeColor = System.Drawing.SystemColors.Info;
            this.Broken.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(0);
            this.Broken.InteractionMode = NationalInstruments.UI.NumericEditInteractionModes.Indicator;
            this.Broken.Location = new System.Drawing.Point(526, 670);
            this.Broken.Name = "Broken";
            this.Broken.Size = new System.Drawing.Size(86, 29);
            this.Broken.TabIndex = 129;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(367, 602);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(101, 12);
            this.label31.TabIndex = 134;
            this.label31.Text = "m_LoadToughWafer";
            // 
            // LoadToughWafer
            // 
            this.LoadToughWafer.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LoadToughWafer.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.LoadToughWafer.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LoadToughWafer.ForeColor = System.Drawing.SystemColors.Info;
            this.LoadToughWafer.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(0);
            this.LoadToughWafer.InteractionMode = NationalInstruments.UI.NumericEditInteractionModes.Indicator;
            this.LoadToughWafer.Location = new System.Drawing.Point(369, 617);
            this.LoadToughWafer.Name = "LoadToughWafer";
            this.LoadToughWafer.Size = new System.Drawing.Size(86, 29);
            this.LoadToughWafer.TabIndex = 133;
            // 
            // timerAGV
            // 
            this.timerAGV.Enabled = true;
            this.timerAGV.Interval = 16;
            this.timerAGV.Tick += new System.EventHandler(this.timerAGV_Tick);
            // 
            // CCDSTATUS
            // 
            this.CCDSTATUS.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CCDSTATUS.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.CCDSTATUS.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CCDSTATUS.ForeColor = System.Drawing.SystemColors.Info;
            this.CCDSTATUS.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(3);
            this.CCDSTATUS.InteractionMode = NationalInstruments.UI.NumericEditInteractionModes.Indicator;
            this.CCDSTATUS.Location = new System.Drawing.Point(185, 610);
            this.CCDSTATUS.Name = "CCDSTATUS";
            this.CCDSTATUS.Size = new System.Drawing.Size(66, 29);
            this.CCDSTATUS.TabIndex = 136;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label32.Location = new System.Drawing.Point(99, 617);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(80, 16);
            this.label32.TabIndex = 137;
            this.label32.Text = "CCDSTATUS";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(19, 654);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 16);
            this.label4.TabIndex = 140;
            this.label4.Text = "LaserNoWafer";
            // 
            // KEY_NoWaferClean
            // 
            this.KEY_NoWaferClean.Cursor = System.Windows.Forms.Cursors.Hand;
            this.KEY_NoWaferClean.Location = new System.Drawing.Point(135, 671);
            this.KEY_NoWaferClean.Name = "KEY_NoWaferClean";
            this.KEY_NoWaferClean.Size = new System.Drawing.Size(54, 31);
            this.KEY_NoWaferClean.TabIndex = 138;
            this.KEY_NoWaferClean.Text = "清 零";
            this.KEY_NoWaferClean.UseVisualStyleBackColor = true;
            this.KEY_NoWaferClean.Click += new System.EventHandler(this.KEY_NoWaferClean_Click);
            // 
            // LaserNoWafer
            // 
            this.LaserNoWafer.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LaserNoWafer.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.LaserNoWafer.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LaserNoWafer.ForeColor = System.Drawing.SystemColors.Info;
            this.LaserNoWafer.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(0);
            this.LaserNoWafer.InteractionMode = NationalInstruments.UI.NumericEditInteractionModes.Indicator;
            this.LaserNoWafer.Location = new System.Drawing.Point(32, 673);
            this.LaserNoWafer.Name = "LaserNoWafer";
            this.LaserNoWafer.Size = new System.Drawing.Size(79, 29);
            this.LaserNoWafer.TabIndex = 139;
            // 
            // dataSocketSource1
            // 
            dataSocketBinding1.AccessMode = NationalInstruments.Net.AccessMode.ReadWriteAutoUpdate;
            dataSocketBinding1.Name = "AgvDownState";
            dataSocketBinding1.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.AgvDownState";
            dataSocketBinding2.AccessMode = NationalInstruments.Net.AccessMode.ReadWriteAutoUpdate;
            dataSocketBinding2.Name = "AgvUpState";
            dataSocketBinding2.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.AgvUpState";
            dataSocketBinding3.AccessMode = NationalInstruments.Net.AccessMode.ReadWriteAutoUpdate;
            dataSocketBinding3.Name = "HEART";
            dataSocketBinding3.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.HEART";
            dataSocketBinding4.AccessMode = NationalInstruments.Net.AccessMode.ReadWriteAutoUpdate;
            dataSocketBinding4.Name = "PlcDownBasketNum";
            dataSocketBinding4.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.PlcDownBasketNum";
            dataSocketBinding5.AccessMode = NationalInstruments.Net.AccessMode.ReadWriteAutoUpdate;
            dataSocketBinding5.Name = "PlcDownState";
            dataSocketBinding5.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.PlcDownState";
            dataSocketBinding6.AccessMode = NationalInstruments.Net.AccessMode.ReadWriteAutoUpdate;
            dataSocketBinding6.Name = "PlcUpBasketNum";
            dataSocketBinding6.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.PlcUpBasketNum";
            dataSocketBinding7.AccessMode = NationalInstruments.Net.AccessMode.ReadWriteAutoUpdate;
            dataSocketBinding7.Name = "PlcUpState";
            dataSocketBinding7.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.PlcUpState";
            dataSocketBinding8.AccessMode = NationalInstruments.Net.AccessMode.ReadWriteAutoUpdate;
            dataSocketBinding8.Name = "UnloadAgvDownState";
            dataSocketBinding8.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.UnloadAgvDownState";
            dataSocketBinding9.AccessMode = NationalInstruments.Net.AccessMode.ReadWriteAutoUpdate;
            dataSocketBinding9.Name = "UnloadAgvUpState";
            dataSocketBinding9.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.UnloadAgvUpState";
            dataSocketBinding10.AccessMode = NationalInstruments.Net.AccessMode.ReadWriteAutoUpdate;
            dataSocketBinding10.Name = "UnloadPlcDownNum";
            dataSocketBinding10.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.UnloadPlcDownNum";
            dataSocketBinding11.AccessMode = NationalInstruments.Net.AccessMode.ReadWriteAutoUpdate;
            dataSocketBinding11.Name = "UnloadPlcDownState";
            dataSocketBinding11.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.UnloadPlcDownState";
            dataSocketBinding12.AccessMode = NationalInstruments.Net.AccessMode.ReadWriteAutoUpdate;
            dataSocketBinding12.Name = "UnloadPlcUpNum";
            dataSocketBinding12.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.UnloadPlcUpNum";
            dataSocketBinding13.AccessMode = NationalInstruments.Net.AccessMode.ReadWriteAutoUpdate;
            dataSocketBinding13.Name = "UnloadPlcUpState";
            dataSocketBinding13.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.UnloadPlcUpState";
            dataSocketBinding14.AccessMode = NationalInstruments.Net.AccessMode.ReadWriteAutoUpdate;
            dataSocketBinding14.Name = "WHEART";
            dataSocketBinding14.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.WHEART";
            dataSocketBinding15.AccessMode = NationalInstruments.Net.AccessMode.ReadWriteAutoUpdate;
            dataSocketBinding15.Name = "EQPReadRFIDComplete";
            dataSocketBinding15.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.LoadBasketIn";
            dataSocketBinding16.AccessMode = NationalInstruments.Net.AccessMode.ReadWriteAutoUpdate;
            dataSocketBinding16.Name = "MESReadRFIDComplete";
            dataSocketBinding16.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.LoadMesAgree";
            dataSocketBinding17.AccessMode = NationalInstruments.Net.AccessMode.ReadWriteAutoUpdate;
            dataSocketBinding17.Name = "MESReadRFIDError";
            dataSocketBinding17.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.LoadMesReject";
            dataSocketBinding18.AccessMode = NationalInstruments.Net.AccessMode.ReadWriteAutoUpdate;
            dataSocketBinding18.Name = "MESReadRFIDErrorCode";
            dataSocketBinding18.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.LoadErrorCode";
            dataSocketBinding19.AccessMode = NationalInstruments.Net.AccessMode.ReadWriteAutoUpdate;
            dataSocketBinding19.Name = "LoadRFID";
            dataSocketBinding19.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.LoadRfid";
            dataSocketBinding20.AccessMode = NationalInstruments.Net.AccessMode.ReadWriteAutoUpdate;
            dataSocketBinding20.Name = "EQPReadRFIDComplete1";
            dataSocketBinding20.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.UnLoadBasketIn";
            dataSocketBinding21.AccessMode = NationalInstruments.Net.AccessMode.ReadWriteAutoUpdate;
            dataSocketBinding21.Name = "MESReadRFIDComplete1";
            dataSocketBinding21.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.UnLoadMesAgree";
            dataSocketBinding22.AccessMode = NationalInstruments.Net.AccessMode.ReadWriteAutoUpdate;
            dataSocketBinding22.Name = "MESReadRFIDError1";
            dataSocketBinding22.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.UnLoadMesReject";
            dataSocketBinding23.AccessMode = NationalInstruments.Net.AccessMode.ReadWriteAutoUpdate;
            dataSocketBinding23.Name = "MESReadRFIDErrorCode1";
            dataSocketBinding23.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.UnLoadErrorCode";
            dataSocketBinding24.AccessMode = NationalInstruments.Net.AccessMode.ReadWriteAutoUpdate;
            dataSocketBinding24.Name = "UnLoadRFID";
            dataSocketBinding24.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.UnLoadRfid";
            dataSocketBinding25.AccessMode = NationalInstruments.Net.AccessMode.ReadWriteAutoUpdate;
            dataSocketBinding25.Name = "MESEnabled";
            dataSocketBinding25.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.MesSignal";
            dataSocketBinding26.AccessMode = NationalInstruments.Net.AccessMode.ReadWriteAutoUpdate;
            dataSocketBinding26.Name = "EQPState";
            dataSocketBinding26.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.MachineStatus";
            dataSocketBinding27.Name = "PlcInNumber";
            dataSocketBinding27.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.PlcInNumber";
            dataSocketBinding28.Name = "PlcOutNumber";
            dataSocketBinding28.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.PlcOutNumber";
            dataSocketBinding29.Name = "UnloadPlcInmber";
            dataSocketBinding29.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.UnloadPlcInNumber";
            dataSocketBinding30.Name = "UnloadPlcOutNumber";
            dataSocketBinding30.Url = "opc://localhost/BroadWin.BwOpcServerDA/WebAccess.UnloadPlcOutNumber";
            this.dataSocketSource1.Bindings.AddRange(new NationalInstruments.Net.DataSocketBinding[] {
            dataSocketBinding1,
            dataSocketBinding2,
            dataSocketBinding3,
            dataSocketBinding4,
            dataSocketBinding5,
            dataSocketBinding6,
            dataSocketBinding7,
            dataSocketBinding8,
            dataSocketBinding9,
            dataSocketBinding10,
            dataSocketBinding11,
            dataSocketBinding12,
            dataSocketBinding13,
            dataSocketBinding14,
            dataSocketBinding15,
            dataSocketBinding16,
            dataSocketBinding17,
            dataSocketBinding18,
            dataSocketBinding19,
            dataSocketBinding20,
            dataSocketBinding21,
            dataSocketBinding22,
            dataSocketBinding23,
            dataSocketBinding24,
            dataSocketBinding25,
            dataSocketBinding26,
            dataSocketBinding27,
            dataSocketBinding28,
            dataSocketBinding29,
            dataSocketBinding30});
            this.dataSocketSource1.BindingDataUpdated += new NationalInstruments.Net.BindingDataUpdatedEventHandler(this.dataSocketSource1_BindingDataUpdated_1);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(138, 713);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 142;
            this.label8.Text = "上料AGV";
            // 
            // LED_IN66
            // 
            this.LED_IN66.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.LED_IN66.Location = new System.Drawing.Point(135, 722);
            this.LED_IN66.Name = "LED_IN66";
            this.LED_IN66.OffColor = System.Drawing.Color.Black;
            this.LED_IN66.Size = new System.Drawing.Size(50, 50);
            this.LED_IN66.TabIndex = 141;
            // 
            // FormHMI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1276, 986);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.ControlBox = false;
            this.Controls.Add(this.label8);
            this.Controls.Add(this.LED_IN66);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.KEY_NoWaferClean);
            this.Controls.Add(this.LaserNoWafer);
            this.Controls.Add(this.label32);
            this.Controls.Add(this.CCDSTATUS);
            this.Controls.Add(this.label31);
            this.Controls.Add(this.LoadToughWafer);
            this.Controls.Add(this.BrokenClear);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.TooSkew);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.Broken);
            this.Controls.Add(this.SignalNeed);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.LED_AIN3);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.LED_YOUT7);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.Edit_LoadStep);
            this.Controls.Add(this.Text_NoCheck);
            this.Controls.Add(this.Edit_WaferStatus);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.CurrentTable);
            this.Controls.Add(this.ZOOM_FIT);
            this.Controls.Add(this.ZOOM_OUT);
            this.Controls.Add(this.ZOOM_IN);
            this.Controls.Add(this.axCKVisionCtrl1);
            this.Controls.Add(this.UIN3);
            this.Controls.Add(this.LED_UIN3);
            this.Controls.Add(this.KEY_DD45AngleRun);
            this.Controls.Add(this.TextShow);
            this.Controls.Add(this.axMMMark1);
            this.Controls.Add(this.MMDirection);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.TextAuthority);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.axMMStatus1);
            this.Controls.Add(this.axMMIO1);
            this.Controls.Add(this.LogClass);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LED_XIN2);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.LED_XIN3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.GetObject);
            this.Controls.Add(this.Quit);
            this.Controls.Add(this.axMMEdit1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormHMI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mini系列 M2（TPM旋转丢料/6+3+1下满上空）";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormHMI_KeyDown);
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RelativeY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RelativeX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.APositionY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SampleAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.APositionX)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NUM_PowerData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.KEY_LaserOnOff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.KEY_DD45AngleRun)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TotalTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TotalQulity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Capacity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MarkTime)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AutoPause)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_XIN3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_XIN2)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.KEY_OPTSwitch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_YIN3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_YIN2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMMMark1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMMStatus1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMMIO1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMMEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_UIN3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axCKVisionCtrl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Edit_WaferStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Edit_LoadStep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_AIN3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_YOUT7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TooSkew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Broken)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoadToughWafer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CCDSTATUS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LaserNoWafer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSocketSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_IN66)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        // public static double g_Value;        
        string file_name1_Acti = "d:\\DRLaser\\CCDLib\\TestNew-Three.vscf";
        int PowerOnOff = 0;
        bool g_DustActionFlag;
        uint m_DustActionStep;
        uint m_DustActionCount;
        uint m_DustActionCount1;
        uint m_DustErrorCheck;
        public static int g_IniLaser = 0, m_HomeFinished;
        public static uint m_DisableUnLoadRotateCylinder, m_DustCrack, g_MotorAlign, g_CyAlign;
        uint m_SysReset, m_RotateHome, m_DDHome, m_ZPOS, m_DDPOS,  m_ErrorFlag, m_BuzzerCount, m_IniBuzzer, m_UnLoadHomeFlag;
        uint m_XHomeFlag, m_YHomeFlag, m_DHomeFlag,m_ZHomeFlag, m_UHomeFlag, m_GreenYLED, m_InBufferHomeCount, m_OutBufferHomeCount;
        uint m_PowerCounter, m_PowerOut, m_LaserPowerFlag, m_AutoMoveCounter, m_SysIniReset, m_PowerStep;
        uint m_LoadAlignCount, m_UnLoadKeyIn, m_ReadyNow, m_MeasureCount, m_FirstWaferFlag;

        uint m_CCDFinishFlag, m_CCDAlarm5Count, m_MarkFinish, m_RotateInFinish, m_CCDAlarm, m_Broken, m_TooSkew;
        uint m_TempLoadKeyIn, m_MainStepContinue, m_MainStepContinue1, m_LeftWaferDeal, m_LeftWaferDeal1, m_LaserRelease;
        uint m_WaferBreak, m_InMotor1Continue, m_InMotor2Continue, m_In1AhomeFlag,m_AlamDownContinue;
        uint m_AlamLoadContinue, m_AlamLoad1Continue, m_AlamLoad3Continue, m_ComStatusErrorDelay, m_DAlamLoadContinue,
            m_KKAlamLoadContinue,m_MainAAlarmContinue, m_MainBAlarmContinue, m_MainCAlarmContinue, m_MainDAlarmContinue, m_MainXAlarmContinue, m_MainYAlarmContinue, m_MainZAlarmContinue, m_MainDDAlarmContinue;
        uint m_TotalQulity, m_OutMotorOffset, m_InMotorOffset, m_LoadCasstteStatus, m_MainIn2;
        uint m_In_WaferFlag, m_InONOFF, m_AlarmFlag, m_TotalTime, m_In1Delay, m_LaserCheckCount;
        uint m_PowerInv, m_TimerOPTFlag, m_LoadWaferQTemp, m_CCDdelay, m_InReleaseDelay, m_OutReleaseDelay, m_PosErrorAlarm;
        int m_LoadHomeDelay,  m_WaferSuckDelay,m_TotalGaspressureDelay;
        short m_InStepConter, m_ComStatusA;
        uint m_NextWait, m_NextWaitFlag, m_LoadIN42Count, m_StepContinue, m_StepContinue1, m_PassStepContinue, m_PassStepContinue1, m_PositionContinue, m_StepContinue2, m_StepContinue3, m_StepContinue4, m_StepContinue5,
            m_LoadWaferConst, m_WaitContinue;
        bool m_CCDNoWafer, m_DDSuckfail, m_SysResetFlag, m_DoorProtect, m_SignalNeed, m_MarkTimeFlag,
            m_WaferCheckFlag, m_WaferCheckStop,m_FirstPostFlag, m_DDRunFlag;
        bool m_InBufferUp, m_MainBreak, m_NoCheck, m_LaserCheck, m_KKHaveWafer, m_LoadOff, m_UnLoadSuck, m_OutBoxAlarmFlag;
        double m_OffsetDistanceTwoAxis, m_CCDNGArea, m_BufferInOffset, m_BufferOutOffset,m_RotateInOffset, m_RotateOutOffset, m_PowerMin, m_PowerMax;
        double m_PowerRatio,m_CenterX, m_CenterY, m_LoadMotorOffset;
        double m_Load1220XElevatorIn, m_PowerData,m_Load1220XElevatorStart, m_FirstWaferPos, m_LoadStepInv, m_LoadWaferBigInv,m_Load1220XElevatorOut;

        public static uint m_LongOutDelay;
        double m_CCDStatus,m_CCDFinishData,
            m_CCDBaseX, m_CCDBaseY, m_CCDBaseAng, m_CCDShallStand,
            m_CCDShallDiff, m_CCDWaferStand, m_CCDWaferDiff,
            m_CCDStandX, m_CCDStandY, m_RelativeX, m_RelativeY, m_CCDAlarmValue, m_CCDAlarmAngleValue;
      
        public static double[] g_CCDSource = new double[4], m_CCDFinish = new double[4];
        public static bool m_LoadHandOut = false;
            bool DDInit = false, m_AlamBuzzer = false, m_Pause, m_Buzzer, m_DDWaferInFlag, m_WaferUpFlag, m_flagCheck1, m_flagStatus1 = true, m_flagCheck2, m_flagStatus2 = false,
            m_Modify, m_CTFlag, m_WaferReleaseFlag, m_LoadAlarm42, m_LoadToughWafer;
        DEV_LIST[] devList = new DEV_LIST[Motion.MAX_DEVICES];

        public static double m_AlignPos;    //花篮打齐位置
        public static double m_RotateCylindDelay;   //旋转气缸延时
        public static IntPtr[] ax1Handle = new IntPtr[32];
        public static IntPtr[] ax2Handle = new IntPtr[32];
        public static double[] g_Posnt = new double[4], g_SysParam = new double[80], g_TheoryPix = new double[8], g_RealPix = new double[8];
        public static int g_LoadElevationP;
        public static double m_MAccDecc;
        public static int m_UnLoadConstDistance,g_LoadBuffer, g_UnLoadBuffer;
        public static uint m_LoadHomeFlag = 0, m_LoadKeyIn, g_InRotateMotorPosition, g_OutRotateMotorPosition, g_breakstat, g_breakstat1, g_breakstat2, g_breakstat3, g_breakstat4, g_CWPositionFlag, g_LoadWaferQ, g_UnLoadWaferQ, g_MainStep = 30;
        public static IntPtr dev1Handle = IntPtr.Zero, dev2Handle = IntPtr.Zero, g_dev3Handle = IntPtr.Zero, dev4Handle = IntPtr.Zero;
        public static ushort m_RingNoA = 0;
        byte m_AIN0, m_AIN1,m_AIN3, m_AIN2, m_BIN0, m_BIN1 ,m_BIN1Flag , m_BIN3, m_CIN0, m_CIN1, m_CIN2, m_CIN3, m_DIN0, m_DIN1, m_DIN2, m_DIN3;
        byte m_XIN0, m_XIN2, m_XIN3, m_YIN0, m_YIN1, m_YIN2, m_YIN3, m_ZIN0, m_ZIN1, m_ZIN2, m_ZIN3, m_UIN0, m_UIN1, m_UIN2, m_UIN3, m_YOUT7;
        public byte m_LoadIN00, m_LoadIN01, m_LoadIN02, m_LoadIN03, m_LoadIN04, m_LoadIN05, m_LoadIN06, m_LoadIN07,
            m_LoadIN10, m_LoadIN11, m_LoadIN12, m_LoadIN13, m_LoadIN14, m_LoadIN15, m_LoadIN16, m_LoadIN17,
            m_LoadIN20, m_LoadIN21, m_LoadIN22, m_LoadIN23, m_LoadIN24, m_LoadIN25, m_LoadIN26, m_LoadIN27,
            m_LoadIN30, m_LoadIN31, m_LoadIN32, m_LoadIN33, m_LoadIN34, m_LoadIN35, m_LoadIN36, m_LoadIN37,
            m_LoadIN40, m_LoadIN41, m_LoadIN42, m_LoadIN43, m_LoadIN44, m_LoadIN45, m_LoadIN46, m_LoadIN47,
            m_LoadIN50, m_LoadIN51, m_LoadIN52, m_LoadIN53, m_LoadIN54, m_LoadIN55, m_LoadIN56, m_LoadIN57,
            m_LoadIN60, m_LoadIN61, m_LoadIN62,m_LoadIN64,m_LoadIN65, m_LoadIN66,
            m_LoadOut30, m_LoadOut34, m_LoadOut35;
        public static uint g_IN51, g_IN52, g_IN53, g_IN54, g_IN55;       
        uint m_APOS, m_BPOS, m_CPOS, m_DPOS,m_XLimP, m_XLimN, m_XHome, m_YHome, m_XPOS, m_YPOS, m_InBufferRelease,
            m_AAlarm, m_BAlarm, m_CAlarm, m_DAlarm, m_XAlarm, m_YAlarm, m_ZAlarm,m_CT,m_CT1,
            m_Load_DPos, m_Load_DAlarm, m_Load_XPos, m_Load_XAlarm, m_Out1WaferAlarm2Flag = 0,
            m_IN11Flag, m_Step7Continue, m_RotateInFlag, m_waferoutflag, m_RotateOutFlag,  m_OutBufferflag,m_WaferBreakOutError,
            m_RotateOutCount, m_BreakOutCount, m_RotateOut2Count, m_RotateInCount, m_CommunicateStatus, m_Out3DelayCount;

        uint m_DDRunDelay, m_AlarmBIN0BIN1Delay, m_AlarmBIN1Time, m_AlarmBIN0BIN1Flag, m_LoadSuck, m_nState, m_WaferCheck, m_Out11WaferDelay, m_Out1WaferAlarm, m_AlamLoad2Continue, m_CCDBreakWaferAlarm,
            m_WaferOutSuckDelay, m_LoadInErrorDelay, m_UnLoadInErrorDelay, m_LaserNoWafer;
        byte m_Load_YHome;
        public static bool g_Clean, g_Modify, g_InRotateHome, g_LoadHome, g_UnLoadHome, g_ForceUpFinish = false, g_ForceDownFinish = false, g_NotAgvAlarm = false, g_AgvModel = false, g_NoCheckAgvSensor=false;
        uint m_LoadIn1run, m_LoadCCWrun, m_LoadOut1run, m_LoadToughWaferCount, m_BoxKeyInStep, m_LoadIN62InFlagCount,m_BoxKeyInDelayCount, m_LoadIN62Count,
            m_LoadIN62MoveFlag=0, m_LoadIN62InFlag=0, m_BoxKeyInDelay;
        ushort m_Existcards = 0;
        MicroTimer mcTimer;
        Thread Thread_T1, Thread_T2;

        //AGV相关定义
        ReaderInit RFIDInit = new ReaderInit(ReaderInit.RFNum_enum.RF0);
        Modbus RFIDReader = new Modbus(true);
        short g_AgvAlarm = 0;
        short m_AgvAlarmCount = 0;
        short m_AgvAlarmCount1 = 0;
        short m_WcsHeartCount = 0;
        short m_WcsHeartCount2 = 0;
        short m_WcsHeartTemp = 0;
        short m_WcsHeartTemp2 = 0;
        public static short m_WcsHeart = 10;
        public static ushort m_AgvUpStateTotal = 0;
        public static ushort m_AgvDownStateTotal = 0;
        public static bool[] m_AgvUpState = new bool[17];
        public static bool[] m_AgvDownState = new bool[17];


        public static short g_PlcInNum = 6;
        public static short g_PlcOutNum = 6;

        short m_StopDelayCounter = 0;
        short m_MesCounter = 0;
        short g_EQPHeartBeatValue = 1;
        int m_OpcData_UnLoad = 0;
        int m_OpcData_Load = 0;

        public static int g_LoadAGVCommStep = 20;
        int m_LoadRestDelayCounter = 0;
        public static int g_UnLoadAGVCommStep = 20, g_InBufferTotal;
        int m_UnLoadRestDelayCounter = 0;

        short m_CassetteNumCounter = 0;
        public static short g_LoadFullNum = 0;
        public static short g_UnloadEmptyNum = 0;

        short m_StopCounter = 0;
        ////////////PCI-1245 IO定义
        public static class pci1245l
        {            
            public const int Axis_ONE =	0;
            public const int Axis_TWO= 	1;
            public const int Axis_THREE= 	2; 
            public const int Axis_FOUR =	3;
            public const int Axis_FIVE = 4;
            public const int Axis_SIX= 5;
            public const int Axis_SEVEN = 6;
            public const int Axis_EIGHT = 7;


            public const int  RedOut=4;			 //灯塔：红色
            public const  int GreenOut=				5;			 //灯塔：绿色
            public const  int YellowOut	=			6;			 //灯塔：黄色
            public const int BuzzerOut = 7;			 //灯塔：蜂鸣器  
            //////////电子齿轮
            public const  double WaferRatio	=			21.74*5	;	//主机传送硅片电机 
            public const double LoadWaferRatio	=		109.6	;	//接驳台传送硅片电机  
            public const double BoxRatio	=			10000/62.47;		//花篮传送电机 
            public const double LoadBoxRatio=			50000/76;		//花篮传送电机 
            public const uint ElevatorRatio = 500;			 	//升降模组   //XP20180115 500->1000
            public const double	 BufferRatio=			1000;
            public const double RotateRatio	=		100	;		   //旋转电机比例
            public const double DDRatio	=			1000;		 //DD电机比例 
            public const double LineRatio = 500;
            //速度设置
            public const int WaferSpeed	=			800 ;  //650,增强型
            public const int RotateSpeed = 800;//700   //800,增强型
            public const int DDSpeed = 900;//650	   //750 ,增强型
           // public const int ElevatorMaxSpeed = 500;
            public const int ElevatorMaxSpeed = 400;  
            public const int BoxTranSpeed = 120;
            public const int BufferSpeed = 400;//120

            //默认距离
            public const double OUT1Distance=		280;
            public const double ConstDistance=		  300 ;          
            public const double In2MaxDistance	=		1500 ;   
            public const double LoadTouchPos=			320;

            //Buffer最大缓存片数
           // public const  int		g_InBufferTotal=	   	15 ;
            public const int         OutBufferTotal = 15;
            ////延时定义
            public const uint UnLoadSynDelay = 8;
            public const uint MotorDelay = 350;
            public const uint LongMotorDelay = 850;
            public const uint OutWaferDelay = 30;
        }
        //地址设置
        public static class ClineAddr
        {
            public const ushort LoadAX0IP = 0;
            public const ushort LoadAX1IP = 1;
            public const ushort LoadD122IP = 2;
            public const ushort LoadD140IP = 3;
            public const ushort LoadD122_2IP = 4;

        public const ushort UnLoad1240IP=		  48;
        public const ushort UnLoad1220IP	=	  16;
        public const ushort UnLoad2752IP = 32;
        }
        /// <summary>
        /// ////////报警数组
        /// </summary>
        string[] AlarmMess={
						"请注意：检测到激光光闸没有打开，自动化加工将自动退出，请检查！",//00 
						"请注意：主机“进料”第一个模组堵料，请检查！",	   	//01
						"请注意：主机“进料”缓存，释放异常，请检查！",   	//02   
						"请注意：主机“进料”第1，2模组间堵料，请检查！",	  	//03   
						"请注意：主机“出料”旋转臂可能存在甩料，导致硅片没有正常进入出料模组，请检查！",	  	//04   
						"请注意：主机“出料”缓存，释放异常，请检查！",   	//05   
						"请注意：主机“进料”模组2，传感器被遮挡，请移除硅片，然后点击确认",  //06   
						"请注意：主机“上料”抓料异常，请取走上料气抓下的硅片！",			 //07   
						"请注意：主机“下料”抓料异常，请取走下料气抓下的硅片！", 			  //08   
						"请注意：台面A负压异常，请检查！",				 //09   
						"请注意：台面B负压异常，请检查！", 				 //10   
						"请注意：台面C负压异常，请检查！",	 //11   
						"请注意：台面D负压异常，请检查！", 		 		//12   
						"请注意：硅片卡在“主机出料”第二个模组，与“下料接驳台”进料口之间，请移除该硅片！", //13   
						"请注意：下料接驳台出料异常，出料运动立即停止，请检查出料传送模组是否卡死！",			//14  
						"温馨提示：系统检测到安全门没有关闭，将自动切换到暂停状态，请关闭安全门再自动运行！ ",	 //15  
						"请注意：上料接驳台，检测到硅片被卡在花篮之中，请务必移除花篮中被卡住的硅片，然后点击确定！",	 //16 
						"温馨提示：上料接驳台，硅片遮挡“离开位置”，请检查！", //17
						"温馨提示：上料接驳台，进料阻挡气缸2伸出不到位",		  //18
						"温馨提示：上料接驳台，进料花篮放置异常，或者花篮上下颠倒，请检查！",   //19 
						"温馨提示：上料接驳台，下压气缸异常，没有检测到上升到位信号，请检查！",   //20 
						"温馨提示：上料接驳台，进花篮处于交接位置，存在倒花篮风险，请将其推入到托盘之中，再点击确认！",   //21 
						"温馨提示：上料接驳台，出花篮过多，请取走空花篮！",		  //22 
						"请注意：上料接驳台升降模组没有到达进料位置，系统异常，请重新复位升降模组！", //23  
						"请注意：下料接驳台升降模组没有到达进/出料位置，系统异常，请重新复位升降模组！",	 //24   
						"请注意：有硅片被卡在抽尘罩出口位置，请立即清除，自动化运行将停止！",		 //25  
						"请注意：主机“出料”第一个模组堵料，请检查！",								 //26  
						"请注意：上料接驳台升降模组，未检测到下压汽缸到位信号，点击“确定”后，执行上料出花篮动作！",	//27  
						"请注意：出料模组可能有硅片踩脚，请检查，并清理硅片！",												 //28 
						"请注意：系统未检测到激光器出光，请检查激光器是否正常！\n---点击“OK”后，系统将退出自动运行状态！",	 //29 
						"温馨提示：上料接驳台，已经连续三次未检测到硅片，花篮内堵片或存在压碎片风险，请检查或清理堵片！",		 //30  
						"温馨提示：上料接驳台,出花篮处于交接位置，存在倒花篮风险，请将其推出到出料传送模组之上，再点击确认！",	 //31 
						"温馨提示：AGV对接时间过长可能卡花篮，或者出料阻挡气缸2没有伸出到位!", 	//32
						"温馨提示：AGV对接时间过长可能卡花篮，或者进料阻挡气缸1没缩回到位!！", //33
						"温馨提示：下料接驳台，进花篮处于交接位置，存在倒花篮风险，请将其推入到托盘之中，再点击确认！",		//34    
						"温馨提示：下料接驳台，出料花篮放置异常，或者花篮上下颠倒，请检查！",		//35    
						"温馨提示：下料接驳台，下压气缸异常，没有检测到上升到位信号，请检查！",		//36    
						"温馨提示：下料接驳台，出花篮处于交接位置，存在倒花篮风险，请将其推出到出料传送模组之上，再点击确认！",		//37    
						"温馨提示：下料接驳台，出花篮过多，请取走空花篮！",		//38  
						"温馨提示：上料接驳台，进料阻挡气缸3伸出不到位",	//39   
						"请注意：拍照数据偏差太大，不能进行手动打标！",												 //40   
						"请注意：光闸没有打开，或者功率汽缸没有缩回，自动运行停止，请检查！",	 //41  
						"请注意：上料接驳台升降模组运动时发现上下对射式“花篮进”Sensor被遮挡，升降运动立即停止！",	 //42  
						"请注意：下料接驳台升降模组运动时发现上下对射式“花篮进”Sensor被遮挡，升降运动立即停止！",	 //43  
						"请注意：第一片位置与起始位置偏差太大，请重新设置！",								 //44  
						"请注意：总气气压低于正常值，自动化运行将立即停止，请检查！",							 //45  
						"温馨提示：接驳台通信失败。请确认接驳台是否上电，控制软件将自动退出！",		 //46
						"温馨提示：“下料”接驳台通信失败。请确认接驳台是否上电，控制软件将自动退出！",
						"温馨提示：上料接驳台升降模组报警，请立即关闭电源，重新启动并复位升降模组！",		 //48
						"温馨提示：下料接驳台升降模组报警，请立即关闭电源，重新启动并复位升降模组！",   //49
						"温馨提示：上料接驳台，硅片没有正常出花篮，遮挡上下对射式传感器，请移除该硅片！",		 //50
						"温馨提示：下料接驳台，硅片没有正常进入花篮，遮挡上下对射式传感器，请移除该硅片！",      //51 
                        "请注意：台面C负压异常，请检查！",				 //52  
						"请注意：台面D负压异常，请检查！", 				 //53
                        "请注意：出料旋转气缸，动作异常，请检查！", 				 //54
                        "请注意：排费片工位，吸附动作异常，请检查！", 				 //55
                        "请注意：“主机”出料传送模组硅片堵料，请移除硅片！！", 				 //56
                        "请注意：上料接驳台舌头伸出异常，确认以后，请点击上料出花篮！",//57
                        "请注意：下料旋转臂吸附异常，请清理台面及出料模组上的碎片！",//58
                        "请注意：上料接驳台出花篮失败，请手动取走空花篮后，点击确定！",//59
                        "请注意：下料接驳台出花篮失败，托盘电机可能出现异常，请手动取走满盒花篮后，点击‘确定’退出软件，并检查！",//60
                        "请注意：接驳台通讯异常，请重启软件并重新复位，点击‘确定’将自动退出软件！",//61
                        "请注意：“主机”出料传送模组出硅片异常，有追尾或甩片风险，请检查",//62
                        "请注意：上料接驳台舌头缩回不到位，请检查！",//63
                        "温馨提示：上料接驳台，进料阻挡气缸3缩回不到位",//64
                        "请注意：上料接驳台没有完成复位，请重新复位！",  //65
                        "请注意：上料接驳台硅片传送模组伺服驱动器报警,请将该驱动器断电重启",  //66
                        "请注意：主机进料模组1伺服驱动器报警,请将该驱动器断电重启",  //67
                        "请注意：主机进料模组2伺服驱动器报警,请将该驱动器断电重启",  //68
                        "请注意：主机出料模组1伺服驱动器报警,请将该驱动器断电重启",  //69                       
                        "请注意：主机进料buffer伺服驱动器报警,请将该驱动器断电重启",  //70
                        "请注意：主机进料旋转臂伺服驱动器报警,请将该驱动器断电重启",  //71
                        "请注意：主机出料旋转臂伺服驱动器报警,请将该驱动器断电重启",  //72
                        "请注意：DD马达驱动器报警,请将该驱动器断电重启",  //73
                        "请注意：激光冷水机异常，请检查，点击‘确定’将自动停止！",  //74
                        "请注意：DD马达激光位硅片无拍照数据，点击确定,系统自动停止！",	//75 
                        "请注意：上料接驳台，传送模组传感器未检测到有硅片，请移除该硅片，再点击确认！",      //76
                        "温馨提示：上料接驳台，进料阻挡气缸1缩回不到位",		  //77
                        "温馨提示：BIN0和BIN1同时触发，进料模组有遛片风险，请检查！",		  //78
                        "请注意：上料接驳台AGV需要花篮数量与机台不一致，对接结束！",		  //79
                        "请注意：抽尘罩气缸动作异常，请检查！",//80
                        "请注意：花篮硅片打齐气缸状态异常，请检查",//81
                           };

      
        private AxMMEDITLib.AxMMEdit axMMEdit1;
        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.Button Quit;
        private System.Windows.Forms.Button GetObject;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 主机ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MeunMainOpen;
        private System.Windows.Forms.ToolStripMenuItem MenuMainReset;
        private System.Windows.Forms.ToolStripMenuItem 上料接驳台ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuLoadOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton TakeOnePhoto;
        private System.Windows.Forms.ToolStripButton OpenCCDTool;
        private System.Windows.Forms.ToolStripMenuItem MenuMainPara;
        private System.Windows.Forms.ToolStripMenuItem MenuLoadParam;
        private System.Windows.Forms.GroupBox groupBox1;
        private NationalInstruments.UI.WindowsForms.NumericEdit RelativeY;
        private System.Windows.Forms.Label label1;
        private NationalInstruments.UI.WindowsForms.NumericEdit RelativeX;
        private NationalInstruments.UI.WindowsForms.NumericEdit APositionY;
        private System.Windows.Forms.Label label6;
        private NationalInstruments.UI.WindowsForms.NumericEdit SampleAngle;
        private System.Windows.Forms.Label label7;
        private NationalInstruments.UI.WindowsForms.NumericEdit APositionX;
        private System.Windows.Forms.ToolStripButton OpenGraph;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton GotoMM;
        private System.Windows.Forms.ToolStripButton Marking;
        private System.Windows.Forms.Button KEY_LoadHandOut;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button KEY_Clean;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox Modify;
        private System.Windows.Forms.CheckBox LaserCheck;
        private System.Windows.Forms.CheckBox LightONOFF;
        private System.Windows.Forms.CheckBox CCDONOFF;
        private System.Windows.Forms.CheckBox DoorProtect;
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.Button AutoRun;
        private NationalInstruments.UI.WindowsForms.Switch AutoPause;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ContextMenu_MainOpen;
        private System.Windows.Forms.ToolStripMenuItem ContextMenu_LoadOpen;
        private System.Windows.Forms.ToolStripMenuItem ContextMenu_UnLoadOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private NationalInstruments.UI.WindowsForms.Led LED_XIN3;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label5;
        private NationalInstruments.UI.WindowsForms.NumericEdit MarkTime;
        private System.Windows.Forms.ToolStripButton Help;
        private System.Windows.Forms.Timer timerRead;
        private System.Windows.Forms.Label label3;
        private NationalInstruments.UI.WindowsForms.Led LED_XIN2;
        private System.Windows.Forms.GroupBox groupBox6;
        private NationalInstruments.UI.WindowsForms.Switch KEY_OPTSwitch;
        private NationalInstruments.UI.WindowsForms.Led LED_YIN3;
        private NationalInstruments.UI.WindowsForms.Led LED_YIN2;
        private System.Windows.Forms.Timer timerBuzzer;
        private System.Windows.Forms.Timer timerPower;
        private System.Windows.Forms.Label LogClass;
        private AxMMIOLib.AxMMIO axMMIO1;
        private System.Windows.Forms.Timer timerTotal;
        private AxMMSTATUSLib.AxMMStatus axMMStatus1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ToolStripMenuItem MenuOpenGraph;
        private System.Windows.Forms.ToolStripButton ToolKeyPress;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.Label TextAuthority;
        private System.Windows.Forms.Timer timerCheck;
        private System.Windows.Forms.Timer timerOPT;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label MMDirection;
        private AxMMMARKLib.AxMMMark axMMMark1;
        private System.Windows.Forms.ToolStripButton ToolSysReset;
        private System.Windows.Forms.ToolStripButton SaveCCDTool;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private NationalInstruments.UI.WindowsForms.Switch KEY_DD45AngleRun;
        private NationalInstruments.UI.WindowsForms.NumericEdit NUM_PowerData;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private NationalInstruments.UI.WindowsForms.Switch KEY_LaserOnOff;
        private System.Windows.Forms.Timer timerPassBy;
        private System.Windows.Forms.Label TextShow;
        private System.Windows.Forms.Button KEY_EndMeasure;
        private System.Windows.Forms.Button KEY_StartMeasure;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label UIN3;
        private NationalInstruments.UI.WindowsForms.Led LED_UIN3;
        private AxCKVisionCtrlLib.AxCKVisionCtrl axCKVisionCtrl1;
        private System.Windows.Forms.Button ZOOM_IN;
        private System.Windows.Forms.Button ZOOM_OUT;
        private System.Windows.Forms.Button ZOOM_FIT;
        private System.Windows.Forms.TextBox CurrentTable;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.CheckBox CheckBox_LoadOff;
        private NationalInstruments.UI.WindowsForms.NumericEdit Edit_WaferStatus;
        private System.Windows.Forms.CheckBox CheckBox_NoCheck;
        private System.Windows.Forms.Label Text_NoCheck;
        private System.Windows.Forms.Label label17;
        private NationalInstruments.UI.WindowsForms.NumericEdit Edit_LoadStep;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.CheckBox SignalNeed;
        private System.Windows.Forms.Label label25;
        private NationalInstruments.UI.WindowsForms.Led LED_AIN3;
        private System.Windows.Forms.Label label27;
        private NationalInstruments.UI.WindowsForms.Led LED_YOUT7;
        private System.Windows.Forms.CheckBox NoCentreAlarm;
        private System.Windows.Forms.Button BrokenClear;
        private System.Windows.Forms.Label label18;
        private NationalInstruments.UI.WindowsForms.NumericEdit TooSkew;
        private System.Windows.Forms.Label label28;
        private NationalInstruments.UI.WindowsForms.NumericEdit Broken;
        private NationalInstruments.UI.WindowsForms.NumericEdit TotalTime;
        private NationalInstruments.UI.WindowsForms.NumericEdit TotalQulity;
        private System.Windows.Forms.Label label30;
        private NationalInstruments.UI.WindowsForms.NumericEdit Capacity;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label29;
        private NationalInstruments.UI.WindowsForms.NumericEdit CT;
        private System.Windows.Forms.Label label31;
        private NationalInstruments.UI.WindowsForms.NumericEdit LoadToughWafer;
        private System.Windows.Forms.ToolStripMenuItem aGVToolStripMenuItem;
        private System.Windows.Forms.Timer timerAGV;
        private System.Windows.Forms.Button button1;
        private NationalInstruments.UI.WindowsForms.NumericEdit CCDSTATUS;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.ToolStripMenuItem MenuMainCorrect;
        private System.Windows.Forms.CheckBox Clean;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button KEY_NoWaferClean;
        private NationalInstruments.UI.WindowsForms.NumericEdit LaserNoWafer;
        private NationalInstruments.Net.DataSocketSource dataSocketSource1;
        private System.Windows.Forms.Label label8;
        private NationalInstruments.UI.WindowsForms.Led LED_IN66;
    }
}

