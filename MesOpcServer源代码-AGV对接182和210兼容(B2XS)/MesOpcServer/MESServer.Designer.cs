namespace MesOpcServer
{
    partial class MESServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MESServer));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timerServerOpen = new System.Windows.Forms.Timer();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuProduction = new System.Windows.Forms.ToolStripMenuItem();
            this.RFIDMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AGVMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.通信设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LogBox = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
            this.清空内容ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LED_PLC = new NationalInstruments.UI.WindowsForms.Led();
            this.label2 = new System.Windows.Forms.Label();
            this.btnReConnect = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.timerMES = new System.Windows.Forms.Timer();
            this.CurrentCellIn = new System.Windows.Forms.TextBox();
            this.LastCellIn = new System.Windows.Forms.TextBox();
            this.ProClearFinish = new System.Windows.Forms.TextBox();
            this.ProClear = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.AlarmCode = new System.Windows.Forms.TextBox();
            this.MachineStatus = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.LoadWaferids = new System.Windows.Forms.TextBox();
            this.LoadRFID = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.LoadReadRFIDCom = new System.Windows.Forms.TextBox();
            this.IssueWaferidCom = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.LoadMesMode = new System.Windows.Forms.TextBox();
            this.LoadCasCheck = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.CasLeftCellNum = new System.Windows.Forms.TextBox();
            this.LoadRFIDDisable = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LED_PLC)).BeginInit();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "帝尔激光MES监控服务软件(B2XS)";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(97, 26);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // timerServerOpen
            // 
            this.timerServerOpen.Enabled = true;
            this.timerServerOpen.Interval = 20;
            this.timerServerOpen.Tick += new System.EventHandler(this.timerServerOpen_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.menuStrip1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuProduction,
            this.RFIDMenuItem,
            this.AGVMenuItem,
            this.通信设置ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 35);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1232, 34);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // menuProduction
            // 
            this.menuProduction.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.menuProduction.Name = "menuProduction";
            this.menuProduction.Size = new System.Drawing.Size(100, 30);
            this.menuProduction.Text = "产能统计";
            this.menuProduction.Click += new System.EventHandler(this.menuProduction_Click);
            // 
            // RFIDMenuItem
            // 
            this.RFIDMenuItem.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RFIDMenuItem.Name = "RFIDMenuItem";
            this.RFIDMenuItem.Size = new System.Drawing.Size(107, 30);
            this.RFIDMenuItem.Text = "RFID监控";
            this.RFIDMenuItem.Click += new System.EventHandler(this.RFIDMenuItem_Click);
            // 
            // AGVMenuItem
            // 
            this.AGVMenuItem.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AGVMenuItem.Name = "AGVMenuItem";
            this.AGVMenuItem.Size = new System.Drawing.Size(143, 30);
            this.AGVMenuItem.Text = "AGV对接监控";
            this.AGVMenuItem.Click += new System.EventHandler(this.AGVMenuItem_Click);
            // 
            // 通信设置ToolStripMenuItem
            // 
            this.通信设置ToolStripMenuItem.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.通信设置ToolStripMenuItem.Name = "通信设置ToolStripMenuItem";
            this.通信设置ToolStripMenuItem.Size = new System.Drawing.Size(100, 30);
            this.通信设置ToolStripMenuItem.Text = "通信设置";
            this.通信设置ToolStripMenuItem.Click += new System.EventHandler(this.通信设置ToolStripMenuItem_Click);
            // 
            // LogBox
            // 
            this.LogBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.LogBox.ContextMenuStrip = this.contextMenuStrip1;
            this.LogBox.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LogBox.ForeColor = System.Drawing.Color.Black;
            this.LogBox.Location = new System.Drawing.Point(351, 83);
            this.LogBox.Name = "LogBox";
            this.LogBox.Size = new System.Drawing.Size(867, 694);
            this.LogBox.TabIndex = 2;
            this.LogBox.Text = "";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.清空内容ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 26);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // 清空内容ToolStripMenuItem
            // 
            this.清空内容ToolStripMenuItem.Name = "清空内容ToolStripMenuItem";
            this.清空内容ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.清空内容ToolStripMenuItem.Text = "清空内容";
            this.清空内容ToolStripMenuItem.Click += new System.EventHandler(this.清空内容ToolStripMenuItem_Click);
            // 
            // LED_PLC
            // 
            this.LED_PLC.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.LED_PLC.Location = new System.Drawing.Point(69, 80);
            this.LED_PLC.Name = "LED_PLC";
            this.LED_PLC.Size = new System.Drawing.Size(40, 40);
            this.LED_PLC.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(14, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 25);
            this.label2.TabIndex = 6;
            this.label2.Text = "PLC";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // btnReConnect
            // 
            this.btnReConnect.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnReConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReConnect.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReConnect.Location = new System.Drawing.Point(129, 83);
            this.btnReConnect.Name = "btnReConnect";
            this.btnReConnect.Size = new System.Drawing.Size(98, 37);
            this.btnReConnect.TabIndex = 7;
            this.btnReConnect.Text = "PLC重连";
            this.btnReConnect.UseVisualStyleBackColor = false;
            this.btnReConnect.Click += new System.EventHandler(this.btnReConnect_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 153);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 21);
            this.label3.TabIndex = 9;
            this.label3.Text = "CurrentCellIn";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 192);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 21);
            this.label4.TabIndex = 11;
            this.label4.Text = "LastCellIn";
            // 
            // timerMES
            // 
            this.timerMES.Enabled = true;
            this.timerMES.Tick += new System.EventHandler(this.timerMES_Tick);
            // 
            // CurrentCellIn
            // 
            this.CurrentCellIn.BackColor = System.Drawing.SystemColors.Info;
            this.CurrentCellIn.Location = new System.Drawing.Point(177, 150);
            this.CurrentCellIn.Name = "CurrentCellIn";
            this.CurrentCellIn.Size = new System.Drawing.Size(154, 29);
            this.CurrentCellIn.TabIndex = 16;
            // 
            // LastCellIn
            // 
            this.LastCellIn.BackColor = System.Drawing.SystemColors.Info;
            this.LastCellIn.Location = new System.Drawing.Point(177, 189);
            this.LastCellIn.Name = "LastCellIn";
            this.LastCellIn.Size = new System.Drawing.Size(154, 29);
            this.LastCellIn.TabIndex = 17;
            // 
            // ProClearFinish
            // 
            this.ProClearFinish.BackColor = System.Drawing.SystemColors.Info;
            this.ProClearFinish.Location = new System.Drawing.Point(177, 267);
            this.ProClearFinish.Name = "ProClearFinish";
            this.ProClearFinish.Size = new System.Drawing.Size(154, 29);
            this.ProClearFinish.TabIndex = 21;
            // 
            // ProClear
            // 
            this.ProClear.BackColor = System.Drawing.SystemColors.Info;
            this.ProClear.Location = new System.Drawing.Point(177, 228);
            this.ProClear.Name = "ProClear";
            this.ProClear.Size = new System.Drawing.Size(154, 29);
            this.ProClear.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 270);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(119, 21);
            this.label5.TabIndex = 19;
            this.label5.Text = "ProClearFinish";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 231);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 21);
            this.label6.TabIndex = 18;
            this.label6.Text = "ProClear";
            // 
            // AlarmCode
            // 
            this.AlarmCode.BackColor = System.Drawing.SystemColors.Info;
            this.AlarmCode.Location = new System.Drawing.Point(177, 346);
            this.AlarmCode.Name = "AlarmCode";
            this.AlarmCode.Size = new System.Drawing.Size(154, 29);
            this.AlarmCode.TabIndex = 25;
            // 
            // MachineStatus
            // 
            this.MachineStatus.BackColor = System.Drawing.SystemColors.Info;
            this.MachineStatus.Location = new System.Drawing.Point(177, 306);
            this.MachineStatus.Name = "MachineStatus";
            this.MachineStatus.Size = new System.Drawing.Size(154, 29);
            this.MachineStatus.TabIndex = 24;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 349);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 21);
            this.label7.TabIndex = 23;
            this.label7.Text = "AlarmCode";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 310);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(123, 21);
            this.label8.TabIndex = 22;
            this.label8.Text = "MachineStatus";
            // 
            // LoadWaferids
            // 
            this.LoadWaferids.BackColor = System.Drawing.SystemColors.Info;
            this.LoadWaferids.Location = new System.Drawing.Point(177, 423);
            this.LoadWaferids.Name = "LoadWaferids";
            this.LoadWaferids.Size = new System.Drawing.Size(154, 29);
            this.LoadWaferids.TabIndex = 29;
            // 
            // LoadRFID
            // 
            this.LoadRFID.BackColor = System.Drawing.SystemColors.Info;
            this.LoadRFID.Location = new System.Drawing.Point(177, 385);
            this.LoadRFID.Name = "LoadRFID";
            this.LoadRFID.Size = new System.Drawing.Size(154, 29);
            this.LoadRFID.TabIndex = 28;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 427);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(114, 21);
            this.label9.TabIndex = 27;
            this.label9.Text = "LoadWaferids";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 388);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(83, 21);
            this.label10.TabIndex = 26;
            this.label10.Text = "LoadRFID";
            // 
            // LoadReadRFIDCom
            // 
            this.LoadReadRFIDCom.BackColor = System.Drawing.SystemColors.Info;
            this.LoadReadRFIDCom.Location = new System.Drawing.Point(177, 508);
            this.LoadReadRFIDCom.Name = "LoadReadRFIDCom";
            this.LoadReadRFIDCom.Size = new System.Drawing.Size(154, 29);
            this.LoadReadRFIDCom.TabIndex = 33;
            // 
            // IssueWaferidCom
            // 
            this.IssueWaferidCom.BackColor = System.Drawing.SystemColors.Info;
            this.IssueWaferidCom.Location = new System.Drawing.Point(177, 465);
            this.IssueWaferidCom.Name = "IssueWaferidCom";
            this.IssueWaferidCom.Size = new System.Drawing.Size(154, 29);
            this.IssueWaferidCom.TabIndex = 32;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 508);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(157, 21);
            this.label11.TabIndex = 31;
            this.label11.Text = "LoadReadRFIDCom";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(15, 468);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(144, 21);
            this.label12.TabIndex = 30;
            this.label12.Text = "IssueWaferidCom";
            // 
            // LoadMesMode
            // 
            this.LoadMesMode.BackColor = System.Drawing.SystemColors.Info;
            this.LoadMesMode.Location = new System.Drawing.Point(177, 581);
            this.LoadMesMode.Name = "LoadMesMode";
            this.LoadMesMode.Size = new System.Drawing.Size(154, 29);
            this.LoadMesMode.TabIndex = 37;
            // 
            // LoadCasCheck
            // 
            this.LoadCasCheck.BackColor = System.Drawing.SystemColors.Info;
            this.LoadCasCheck.Location = new System.Drawing.Point(177, 546);
            this.LoadCasCheck.Name = "LoadCasCheck";
            this.LoadCasCheck.Size = new System.Drawing.Size(154, 29);
            this.LoadCasCheck.TabIndex = 36;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(14, 579);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(124, 21);
            this.label13.TabIndex = 35;
            this.label13.Text = "LoadMesMode";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(14, 546);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(121, 21);
            this.label14.TabIndex = 34;
            this.label14.Text = "LoadCasCheck";
            // 
            // CasLeftCellNum
            // 
            this.CasLeftCellNum.BackColor = System.Drawing.SystemColors.Info;
            this.CasLeftCellNum.Location = new System.Drawing.Point(177, 654);
            this.CasLeftCellNum.Name = "CasLeftCellNum";
            this.CasLeftCellNum.Size = new System.Drawing.Size(154, 29);
            this.CasLeftCellNum.TabIndex = 41;
            // 
            // LoadRFIDDisable
            // 
            this.LoadRFIDDisable.BackColor = System.Drawing.SystemColors.Info;
            this.LoadRFIDDisable.Location = new System.Drawing.Point(177, 616);
            this.LoadRFIDDisable.Name = "LoadRFIDDisable";
            this.LoadRFIDDisable.Size = new System.Drawing.Size(154, 29);
            this.LoadRFIDDisable.TabIndex = 40;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(14, 654);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(132, 21);
            this.label15.TabIndex = 39;
            this.label15.Text = "CasLeftCellNum";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(14, 615);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(138, 21);
            this.label16.TabIndex = 38;
            this.label16.Text = "LoadRFIDDisable";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label17.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label17.Location = new System.Drawing.Point(479, 798);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(306, 28);
            this.label17.TabIndex = 42;
            this.label17.Text = "武汉帝尔激光科技股份有限公司";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.LightGray;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(1041, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 28);
            this.label1.TabIndex = 44;
            this.label1.Text = "182/210兼容版本";
            // 
            // MESServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1232, 840);
            this.ControlBoxFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(160)))), ((int)(((byte)(165)))));
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.CasLeftCellNum);
            this.Controls.Add(this.LoadRFIDDisable);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.LoadMesMode);
            this.Controls.Add(this.LoadCasCheck);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.LoadReadRFIDCom);
            this.Controls.Add(this.IssueWaferidCom);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.LoadWaferids);
            this.Controls.Add(this.LoadRFID);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.AlarmCode);
            this.Controls.Add(this.MachineStatus);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.ProClearFinish);
            this.Controls.Add(this.ProClear);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.LastCellIn);
            this.Controls.Add(this.CurrentCellIn);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnReConnect);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.LED_PLC);
            this.Controls.Add(this.LogBox);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MESServer";
            this.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.ShowTitleIcon = true;
            this.Style = Sunny.UI.UIStyle.Custom;
            this.StyleCustomMode = true;
            this.Text = "帝尔激光MES监控服务软件(B2XS) - V2.6.2";
            this.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MESServer_FormClosing);
            this.Load += new System.EventHandler(this.MESServer_Load);
            this.SizeChanged += new System.EventHandler(this.MESServer_SizeChanged);
            this.contextMenuStrip.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LED_PLC)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Timer timerServerOpen;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuProduction;
        private System.Windows.Forms.RichTextBox LogBox;
        private NationalInstruments.UI.WindowsForms.Led LED_PLC;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnReConnect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer timerMES;
        private System.Windows.Forms.ToolStripMenuItem RFIDMenuItem;
        private System.Windows.Forms.TextBox CurrentCellIn;
        private System.Windows.Forms.TextBox LastCellIn;
        private System.Windows.Forms.TextBox ProClearFinish;
        private System.Windows.Forms.TextBox ProClear;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox AlarmCode;
        private System.Windows.Forms.TextBox MachineStatus;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox LoadWaferids;
        private System.Windows.Forms.TextBox LoadRFID;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox LoadReadRFIDCom;
        private System.Windows.Forms.TextBox IssueWaferidCom;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox LoadMesMode;
        private System.Windows.Forms.TextBox LoadCasCheck;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox CasLeftCellNum;
        private System.Windows.Forms.TextBox LoadRFIDDisable;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ToolStripMenuItem AGVMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 清空内容ToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem 通信设置ToolStripMenuItem;
    }
}

