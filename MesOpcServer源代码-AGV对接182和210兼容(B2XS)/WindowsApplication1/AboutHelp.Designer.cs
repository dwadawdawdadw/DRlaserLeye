namespace WindowsApplication1
{
    partial class AboutHelp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutHelp));
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Items.AddRange(new object[] {
            "系统由武汉帝尔科技股份有限公司研制。",
            "Download：https://control/svn/ManufacturingCode/trunk/04.M2/M2-旋转丢料-接驳台TPM(6+3+1下满" +
                "上空+mes)-V2.0.0",
            "",
            "快捷键定义：",
            "F1----自动化运行",
            "-----------------------------",
            "F2----打开主机界面",
            "F3----打开上料接驳台界面",
            "",
            "-----------------------------",
            "F5----打开图形调用对话框",
            "-----------------------------",
            "F6----打开主机参数设置对话框",
            "F7----打开上料接驳台参数设置对话框",
            "",
            "-----------------------------",
            "F9----自动化运行暂停",
            "F10----打开权限设置对话框",
            "F12----CCD拍照",
            "-----------------------------",
            "Esc----自动化运行停止"});
            this.listBox1.Location = new System.Drawing.Point(206, 19);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(290, 112);
            this.listBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(398, 252);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "www.drlaser.com.cn";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label2.Location = new System.Drawing.Point(12, 163);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 21);
            this.label2.TabIndex = 3;
            this.label2.Text = "V2.0.0 SP37.3";
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.HorizontalScrollbar = true;
            this.listBox2.ItemHeight = 12;
            this.listBox2.Items.AddRange(new object[] {
            "V2.0.0 SP37.3",
            "UID:202211091130 YP",
            "1.优化花篮打齐气缸异常的报警，优化条件",
            "",
            "V2.0.0 SP37.2",
            "UID:202210131910 YP",
            "1.优化手动出花篮压舌头",
            "",
            "V2.0.0 SP37.1",
            "UID:202209081230 YP",
            "1.优化抽尘罩动作频率(15S)",
            "",
            "V2.0.0 SP37",
            "UID:202208091630 GH",
            "1.开放出料旋转气缸延时参数",
            "2.分离DD测功率旋转角度",
            "",
            "V2.0.0 SP36.1",
            "UID:202207311540 GH",
            "1.增加花篮打齐运动位置",
            "优化花篮打齐运动逻辑",
            "",
            "V2.0.0 SP36",
            "UID:202207151330 YP",
            "1.182改造，增加抽尘罩气缸控制动作，增加勾选",
            "2.增加打齐电机，增加勾选",
            "3.增加花篮硅片打齐气缸，增加勾选",
            "",
            "V2.0.0 SP35",
            "UID:202206272010 GH",
            "1.增加Lock/UnLock功能",
            "",
            "V2.0.0 SP34",
            "优化AGV到位传感器判断",
            "",
            "V2.0.0 SP33",
            "AGV到位传感器传给MES",
            "",
            "V2.0.0 SP32",
            "1.优化安全门防护，安全门打开的情况下，DD和旋转臂不允许运动",
            "",
            "V2.0.0 SP31",
            "1.屏蔽安全门勾选功能；",
            "",
            "V2.0.0 SP30",
            "1.修复软件重启后必须套用参数，进料模组硅片才能走准；",
            "2.开放出料1和出料2模组的运动延时，之前默认850，请根据实际情况调节，太小有倒花篮风险，不要小于500",
            "",
            "V2.0.0 SP29",
            "1.fAutoWaferTransfore()进料端逻辑修改，case 0：不在判断buffer的完成信号，",
            "加快buffer上片后，皮带送料的节奏;",
            "2.用新的线程扫描IO；",
            "--产能优化",
            "1.加快KK和舌头皮带的加速；",
            "2.加快主机皮带的加减速，开放参数，推荐值：9；",
            "3.buffer在KK模组case为7的时候也可以升。",
            "",
            "V2.0.0 SP28",
            "1.增加心跳检查",
            "2.增加AGV传感器检测",
            "3.agv报警急停",
            "",
            "V2.0.0 SP27",
            "1.增加下料旋转臂气缸屏蔽功能；",
            "2.解决溜片问题，在报警和运行前，重新检测AIN0和AIN2，重新给m_In1AhomeFlag和m_MainIn2赋值。",
            "",
            "V2.0.0 SP26",
            "1.支持没有设置花篮数量的AGV对接；",
            "",
            "V2.0.0 SP25",
            "1.优化主机进料逻辑；",
            "",
            "V2.0.0 SP24",
            "1.优化mes，支持小于6个花篮传输；",
            "",
            "V2.0.0 SP23",
            "1.提产能；",
            "",
            "V2.0.0 SP22",
            "1.修改MES为Datasocket通讯；",
            "",
            "V2.0.0 SP21",
            "1.解决CT不稳的问题；",
            "",
            "V2.0.0 SP20",
            "1.修改上料OFF功能；",
            "",
            "V2.0.0 SP19",
            "1.增加AGV不报警功能",
            "",
            "V2.0.0 SP18",
            "1.进料模组2改标志位，防止溜片；",
            "2.优化因碎片导致的进料模组2遛片（补偿机械调试问题）",
            "3.相机拍照由3次改为2次；",
            "4.将BUFFER上升到位延时缩短；",
            "",
            "V2.0.0 SP17",
            "1.开放BUFFER格数，减小上BUFFER间隔；",
            "",
            "V2.0.0 SP16",
            "1.长按LoadIN62时，花篮IN；",
            "2.增加OUT键功能；",
            "3.优化激光未出光报警；",
            "",
            "V2.0.0 SP15",
            "1.增加KK模组手动上升一格功能；",
            "2.增加接驳台进花篮电机1即按即走功能（LoadIN62）；",
            "",
            "V2.0.0 SP14",
            "1.增加四点校正功能(暂时屏蔽)；",
            "2.增加皮带清理功能；",
            "3.激光位是否加工，改为根据CCD位是否拍照来判断；",
            "",
            "V2.0.0 SP13",
            "1.软件退出时，将READY信号清零；",
            "2.解决上料BUFFER不上升的问题；",
            "3.将下料吸附与台面释放之间的延时从5降至2；",
            "",
            "V2.0.0 SP11",
            "1.解决上料接驳台检修后依然动作的问题；",
            "2.解决破片清零不记录的问题；",
            "3.每张硅片拍照3次取平均值，差值大于0.1报警；",
            "",
            "V2.0.0 SP10",
            "1.解决上料接驳台下压气缸上升不到位误报警；",
            "2.解决上料接驳台阻挡气缸2伸出不到位误报警；",
            "",
            "V2.0.0 SP09",
            "1.复位时把吸盘吸附和反吹都关闭；",
            "2.光闸关闭时把主界面的开关光闸按键复位；",
            "",
            "V2.0.0 SP08",
            "1.增加上料接驳台硅片没走到舌头传感器位置的报警；",
            "",
            "V2.0.0 SP07",
            "1.由于气压异常导致没拍照DD转过去打标时，报警；",
            "",
            "V2.0.0 SP06",
            "1.自动创建Broken路径。",
            "",
            "V2.0.0 SP05",
            "1.强化舌头压碎片逻辑，引进一个线程，单独检查舌头是否有硅片。",
            "2.界面上显示CT和实时产能。",
            "3.复位时，停止电机运动。",
            "",
            "V2.0.0 SP04",
            "1.报警后暂停；",
            "2.完善权限登录；",
            "3.增加轴报警；",
            "4.报警及破片记录按班次区分；",
            "5.增加冷水机报警；",
            "",
            "V2.0.0 SP03",
            "1.解决主机出料模组堵料误报警的BUG；",
            "",
            "V2.0.0 SP02",
            "1.规范SVN，修改主界面软件版本信息 , 将软件版本号由V1.0.0改为V2.0.0；",
            "",
            "V1.0.0 SP01",
            "1.接驳台采用6+3+1；",
            "2.接驳台全部采用感应电机；",
            "3.主机出料模组有2个传感器，放2张硅片；",
            "4.加MES及AGV对接；"});
            this.listBox2.Location = new System.Drawing.Point(206, 137);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(290, 112);
            this.listBox2.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 15.75F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) 
                | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(222, 252);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(154, 21);
            this.label3.TabIndex = 5;
            this.label3.Text = "M2:6+3+1 TPM";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 37);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(145, 76);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // AboutHelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 277);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.pictureBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutHelp";
            this.Text = "AboutHelp";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Label label3;
    }
}