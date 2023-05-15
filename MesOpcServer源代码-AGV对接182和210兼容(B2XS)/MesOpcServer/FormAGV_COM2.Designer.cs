namespace MesOpcServer
{
    partial class FormAGV_COM2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAGV_COM2));
            this.timerAGV = new System.Windows.Forms.Timer(this.components);
            this.buttonClearList = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxAGVstep = new System.Windows.Forms.TextBox();
            this.buttonManualFinished = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.timerAGVIO = new System.Windows.Forms.Timer(this.components);
            this.OutLedUp_3 = new NationalInstruments.UI.WindowsForms.Led();
            this.OutLedUp_4 = new NationalInstruments.UI.WindowsForms.Led();
            this.LED_SafeOut2 = new NationalInstruments.UI.WindowsForms.Led();
            this.LED_SafeIn2 = new NationalInstruments.UI.WindowsForms.Led();
            this.label11 = new System.Windows.Forms.Label();
            this.LED_SafeOut1 = new NationalInstruments.UI.WindowsForms.Led();
            this.label10 = new System.Windows.Forms.Label();
            this.LED_SafeIn1 = new NationalInstruments.UI.WindowsForms.Led();
            this.label14 = new System.Windows.Forms.Label();
            this.LED_AGVWorkOut = new NationalInstruments.UI.WindowsForms.Led();
            this.label9 = new System.Windows.Forms.Label();
            this.led_AGVAllowOut = new NationalInstruments.UI.WindowsForms.Led();
            this.label7 = new System.Windows.Forms.Label();
            this.led_AGVAllowIn = new NationalInstruments.UI.WindowsForms.Led();
            this.label40 = new System.Windows.Forms.Label();
            this.LED_AGVWorkIn = new NationalInstruments.UI.WindowsForms.Led();
            this.label15 = new System.Windows.Forms.Label();
            this.timerConnect = new System.Windows.Forms.Timer(this.components);
            this.buttonConnect = new System.Windows.Forms.Button();
            this.checkBoxNoLower = new System.Windows.Forms.CheckBox();
            this.checkBoxNoUpper = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.LED_PLC = new NationalInstruments.UI.WindowsForms.Led();
            this.label19 = new System.Windows.Forms.Label();
            this.textBoxBout_num2 = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.textBoxIN_num2 = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.textBoxBout_num1 = new System.Windows.Forms.TextBox();
            this.textBoxIN_num1 = new System.Windows.Forms.TextBox();
            this.ledSyl_In2 = new NationalInstruments.UI.WindowsForms.Led();
            this.ledSyl_In1 = new NationalInstruments.UI.WindowsForms.Led();
            this.label5 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.LogBox = new System.Windows.Forms.RichTextBox();
            this.axActUtlType1 = new AxActUtlTypeLib.AxActUtlType();
            this.ComHandlerTimer = new System.Windows.Forms.Timer(this.components);
            this.LogTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.OutLedUp_3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutLedUp_4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_SafeOut2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_SafeIn2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_SafeOut1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_SafeIn1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_AGVWorkOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.led_AGVAllowOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.led_AGVAllowIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_AGVWorkIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_PLC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ledSyl_In2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ledSyl_In1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axActUtlType1)).BeginInit();
            this.SuspendLayout();
            // 
            // timerAGV
            // 
            this.timerAGV.Interval = 500;
            this.timerAGV.Tick += new System.EventHandler(this.timerAGV_Tick);
            // 
            // buttonClearList
            // 
            this.buttonClearList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.buttonClearList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClearList.Location = new System.Drawing.Point(668, 546);
            this.buttonClearList.Name = "buttonClearList";
            this.buttonClearList.Size = new System.Drawing.Size(103, 43);
            this.buttonClearList.TabIndex = 149;
            this.buttonClearList.Text = "清除";
            this.buttonClearList.UseVisualStyleBackColor = false;
            this.buttonClearList.Click += new System.EventHandler(this.buttonClearList_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(173, 397);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(74, 21);
            this.label12.TabIndex = 196;
            this.label12.Text = "当前步骤";
            // 
            // textBoxAGVstep
            // 
            this.textBoxAGVstep.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.textBoxAGVstep.Location = new System.Drawing.Point(255, 394);
            this.textBoxAGVstep.Name = "textBoxAGVstep";
            this.textBoxAGVstep.Size = new System.Drawing.Size(124, 29);
            this.textBoxAGVstep.TabIndex = 195;
            // 
            // buttonManualFinished
            // 
            this.buttonManualFinished.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.buttonManualFinished.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonManualFinished.Location = new System.Drawing.Point(523, 546);
            this.buttonManualFinished.Name = "buttonManualFinished";
            this.buttonManualFinished.Size = new System.Drawing.Size(120, 43);
            this.buttonManualFinished.TabIndex = 221;
            this.buttonManualFinished.Text = "强制对接完成";
            this.buttonManualFinished.UseVisualStyleBackColor = false;
            this.buttonManualFinished.Click += new System.EventHandler(this.buttonManualFinished_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 413);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 21);
            this.label2.TabIndex = 222;
            this.label2.Text = "出料1伸出";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 442);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 21);
            this.label1.TabIndex = 264;
            this.label1.Text = "出料2伸出";
            // 
            // timerAGVIO
            // 
            this.timerAGVIO.Enabled = true;
            this.timerAGVIO.Interval = 200;
            this.timerAGVIO.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // OutLedUp_3
            // 
            this.OutLedUp_3.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.OutLedUp_3.Location = new System.Drawing.Point(104, 413);
            this.OutLedUp_3.Name = "OutLedUp_3";
            this.OutLedUp_3.OffColor = System.Drawing.Color.Black;
            this.OutLedUp_3.Size = new System.Drawing.Size(28, 28);
            this.OutLedUp_3.TabIndex = 268;
            // 
            // OutLedUp_4
            // 
            this.OutLedUp_4.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.OutLedUp_4.Location = new System.Drawing.Point(104, 442);
            this.OutLedUp_4.Name = "OutLedUp_4";
            this.OutLedUp_4.OffColor = System.Drawing.Color.Black;
            this.OutLedUp_4.Size = new System.Drawing.Size(28, 28);
            this.OutLedUp_4.TabIndex = 270;
            // 
            // LED_SafeOut2
            // 
            this.LED_SafeOut2.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.LED_SafeOut2.Location = new System.Drawing.Point(52, 209);
            this.LED_SafeOut2.Name = "LED_SafeOut2";
            this.LED_SafeOut2.OffColor = System.Drawing.Color.Black;
            this.LED_SafeOut2.Size = new System.Drawing.Size(28, 28);
            this.LED_SafeOut2.TabIndex = 341;
            // 
            // LED_SafeIn2
            // 
            this.LED_SafeIn2.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.LED_SafeIn2.Location = new System.Drawing.Point(52, 291);
            this.LED_SafeIn2.Name = "LED_SafeIn2";
            this.LED_SafeIn2.OffColor = System.Drawing.Color.Black;
            this.LED_SafeIn2.Size = new System.Drawing.Size(28, 28);
            this.LED_SafeIn2.TabIndex = 340;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(21, 154);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(100, 21);
            this.label11.TabIndex = 331;
            this.label11.Text = "安全光电(出)";
            // 
            // LED_SafeOut1
            // 
            this.LED_SafeOut1.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.LED_SafeOut1.Location = new System.Drawing.Point(52, 180);
            this.LED_SafeOut1.Name = "LED_SafeOut1";
            this.LED_SafeOut1.OffColor = System.Drawing.Color.Black;
            this.LED_SafeOut1.Size = new System.Drawing.Size(28, 28);
            this.LED_SafeOut1.TabIndex = 330;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(21, 240);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(100, 21);
            this.label10.TabIndex = 329;
            this.label10.Text = "安全光电(进)";
            // 
            // LED_SafeIn1
            // 
            this.LED_SafeIn1.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.LED_SafeIn1.Location = new System.Drawing.Point(52, 264);
            this.LED_SafeIn1.Name = "LED_SafeIn1";
            this.LED_SafeIn1.OffColor = System.Drawing.Color.Black;
            this.LED_SafeIn1.Size = new System.Drawing.Size(28, 28);
            this.LED_SafeIn1.TabIndex = 328;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(15, 77);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(108, 21);
            this.label14.TabIndex = 395;
            this.label14.Text = "AGV出料对接";
            // 
            // LED_AGVWorkOut
            // 
            this.LED_AGVWorkOut.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.LED_AGVWorkOut.Location = new System.Drawing.Point(126, 73);
            this.LED_AGVWorkOut.Name = "LED_AGVWorkOut";
            this.LED_AGVWorkOut.OffColor = System.Drawing.Color.Black;
            this.LED_AGVWorkOut.OnColor = System.Drawing.Color.Red;
            this.LED_AGVWorkOut.Size = new System.Drawing.Size(33, 32);
            this.LED_AGVWorkOut.TabIndex = 394;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 113);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(108, 21);
            this.label9.TabIndex = 393;
            this.label9.Text = "AGV出料允许";
            // 
            // led_AGVAllowOut
            // 
            this.led_AGVAllowOut.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.led_AGVAllowOut.Location = new System.Drawing.Point(126, 108);
            this.led_AGVAllowOut.Name = "led_AGVAllowOut";
            this.led_AGVAllowOut.OffColor = System.Drawing.Color.Black;
            this.led_AGVAllowOut.Size = new System.Drawing.Size(33, 32);
            this.led_AGVAllowOut.TabIndex = 392;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(108, 21);
            this.label7.TabIndex = 391;
            this.label7.Text = "AGV进料允许";
            // 
            // led_AGVAllowIn
            // 
            this.led_AGVAllowIn.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.led_AGVAllowIn.Location = new System.Drawing.Point(124, 41);
            this.led_AGVAllowIn.Name = "led_AGVAllowIn";
            this.led_AGVAllowIn.OffColor = System.Drawing.Color.Black;
            this.led_AGVAllowIn.Size = new System.Drawing.Size(33, 32);
            this.led_AGVAllowIn.TabIndex = 390;
            this.led_AGVAllowIn.StateChanged += new NationalInstruments.UI.ActionEventHandler(this.led_AGVAllowIn_StateChanged);
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(15, 11);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(108, 21);
            this.label40.TabIndex = 389;
            this.label40.Text = "AGV进料对接";
            // 
            // LED_AGVWorkIn
            // 
            this.LED_AGVWorkIn.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.LED_AGVWorkIn.Location = new System.Drawing.Point(124, 8);
            this.LED_AGVWorkIn.Name = "LED_AGVWorkIn";
            this.LED_AGVWorkIn.OffColor = System.Drawing.Color.Black;
            this.LED_AGVWorkIn.OnColor = System.Drawing.Color.Red;
            this.LED_AGVWorkIn.Size = new System.Drawing.Size(33, 32);
            this.LED_AGVWorkIn.TabIndex = 388;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.ForeColor = System.Drawing.Color.Blue;
            this.label15.Location = new System.Drawing.Point(253, 15);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(126, 21);
            this.label15.TabIndex = 408;
            this.label15.Text = "右侧 COM22";
            // 
            // timerConnect
            // 
            this.timerConnect.Enabled = true;
            this.timerConnect.Interval = 10000;
            this.timerConnect.Tick += new System.EventHandler(this.timerConnect_Tick);
            // 
            // buttonConnect
            // 
            this.buttonConnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.buttonConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonConnect.Location = new System.Drawing.Point(797, 546);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(122, 43);
            this.buttonConnect.TabIndex = 409;
            this.buttonConnect.Text = "重连接驳台";
            this.buttonConnect.UseVisualStyleBackColor = false;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // checkBoxNoLower
            // 
            this.checkBoxNoLower.AutoSize = true;
            this.checkBoxNoLower.Location = new System.Drawing.Point(16, 549);
            this.checkBoxNoLower.Name = "checkBoxNoLower";
            this.checkBoxNoLower.Size = new System.Drawing.Size(125, 25);
            this.checkBoxNoLower.TabIndex = 412;
            this.checkBoxNoLower.Text = "屏蔽出空花篮";
            this.checkBoxNoLower.UseVisualStyleBackColor = true;
            this.checkBoxNoLower.CheckedChanged += new System.EventHandler(this.checkBoxNoLower_CheckedChanged);
            // 
            // checkBoxNoUpper
            // 
            this.checkBoxNoUpper.AutoSize = true;
            this.checkBoxNoUpper.Location = new System.Drawing.Point(16, 527);
            this.checkBoxNoUpper.Name = "checkBoxNoUpper";
            this.checkBoxNoUpper.Size = new System.Drawing.Size(125, 25);
            this.checkBoxNoUpper.TabIndex = 411;
            this.checkBoxNoUpper.Text = "屏蔽进满花篮";
            this.checkBoxNoUpper.UseVisualStyleBackColor = true;
            this.checkBoxNoUpper.CheckedChanged += new System.EventHandler(this.checkBoxNoUpper_CheckedChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("宋体", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label16.Location = new System.Drawing.Point(282, 114);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(46, 21);
            this.label16.TabIndex = 423;
            this.label16.Text = "PLC";
            // 
            // LED_PLC
            // 
            this.LED_PLC.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.LED_PLC.Location = new System.Drawing.Point(334, 102);
            this.LED_PLC.Name = "LED_PLC";
            this.LED_PLC.OffColor = System.Drawing.Color.Black;
            this.LED_PLC.Size = new System.Drawing.Size(45, 45);
            this.LED_PLC.TabIndex = 422;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(166, 355);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(83, 21);
            this.label19.TabIndex = 434;
            this.label19.Text = "出花篮数2";
            // 
            // textBoxBout_num2
            // 
            this.textBoxBout_num2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.textBoxBout_num2.Location = new System.Drawing.Point(255, 352);
            this.textBoxBout_num2.Name = "textBoxBout_num2";
            this.textBoxBout_num2.Size = new System.Drawing.Size(124, 29);
            this.textBoxBout_num2.TabIndex = 433;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(166, 274);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(83, 21);
            this.label18.TabIndex = 432;
            this.label18.Text = "进花篮数2";
            this.label18.Click += new System.EventHandler(this.label18_Click);
            // 
            // textBoxIN_num2
            // 
            this.textBoxIN_num2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.textBoxIN_num2.Location = new System.Drawing.Point(255, 271);
            this.textBoxIN_num2.Name = "textBoxIN_num2";
            this.textBoxIN_num2.Size = new System.Drawing.Size(124, 29);
            this.textBoxIN_num2.TabIndex = 431;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(166, 313);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(83, 21);
            this.label20.TabIndex = 430;
            this.label20.Text = "出花篮数1";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(166, 235);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(83, 21);
            this.label21.TabIndex = 429;
            this.label21.Text = "进花篮数1";
            this.label21.Click += new System.EventHandler(this.label21_Click);
            // 
            // textBoxBout_num1
            // 
            this.textBoxBout_num1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.textBoxBout_num1.Location = new System.Drawing.Point(255, 310);
            this.textBoxBout_num1.Name = "textBoxBout_num1";
            this.textBoxBout_num1.Size = new System.Drawing.Size(124, 29);
            this.textBoxBout_num1.TabIndex = 428;
            // 
            // textBoxIN_num1
            // 
            this.textBoxIN_num1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.textBoxIN_num1.Location = new System.Drawing.Point(255, 232);
            this.textBoxIN_num1.Name = "textBoxIN_num1";
            this.textBoxIN_num1.Size = new System.Drawing.Size(124, 29);
            this.textBoxIN_num1.TabIndex = 427;
            // 
            // ledSyl_In2
            // 
            this.ledSyl_In2.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.ledSyl_In2.Location = new System.Drawing.Point(104, 382);
            this.ledSyl_In2.Name = "ledSyl_In2";
            this.ledSyl_In2.OffColor = System.Drawing.Color.Black;
            this.ledSyl_In2.Size = new System.Drawing.Size(28, 28);
            this.ledSyl_In2.TabIndex = 448;
            // 
            // ledSyl_In1
            // 
            this.ledSyl_In1.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.ledSyl_In1.Location = new System.Drawing.Point(104, 353);
            this.ledSyl_In1.Name = "ledSyl_In1";
            this.ledSyl_In1.OffColor = System.Drawing.Color.Black;
            this.ledSyl_In1.Size = new System.Drawing.Size(28, 28);
            this.ledSyl_In1.TabIndex = 447;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 382);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 21);
            this.label5.TabIndex = 446;
            this.label5.Text = "进料2伸出";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(15, 353);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(83, 21);
            this.label13.TabIndex = 445;
            this.label13.Text = "进料1伸出";
            // 
            // LogBox
            // 
            this.LogBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.LogBox.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LogBox.ForeColor = System.Drawing.Color.Black;
            this.LogBox.Location = new System.Drawing.Point(404, 8);
            this.LogBox.Name = "LogBox";
            this.LogBox.Size = new System.Drawing.Size(630, 524);
            this.LogBox.TabIndex = 450;
            this.LogBox.Text = "";
            // 
            // axActUtlType1
            // 
            this.axActUtlType1.Enabled = true;
            this.axActUtlType1.Location = new System.Drawing.Point(305, 50);
            this.axActUtlType1.Name = "axActUtlType1";
            this.axActUtlType1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axActUtlType1.OcxState")));
            this.axActUtlType1.Size = new System.Drawing.Size(32, 32);
            this.axActUtlType1.TabIndex = 449;
            // 
            // ComHandlerTimer
            // 
            this.ComHandlerTimer.Enabled = true;
            this.ComHandlerTimer.Interval = 50;
            this.ComHandlerTimer.Tick += new System.EventHandler(this.ComHandlerTimer_Tick);
            // 
            // LogTimer
            // 
            this.LogTimer.Enabled = true;
            this.LogTimer.Interval = 20;
            this.LogTimer.Tick += new System.EventHandler(this.LogTimer_Tick);
            // 
            // FormAGV_COM2
            // 
            this.AllowShowTitle = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1046, 604);
            this.ControlBox = false;
            this.ControlBoxFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(160)))), ((int)(((byte)(165)))));
            this.Controls.Add(this.LogBox);
            this.Controls.Add(this.axActUtlType1);
            this.Controls.Add(this.ledSyl_In2);
            this.Controls.Add(this.ledSyl_In1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.textBoxBout_num2);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.textBoxIN_num2);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.textBoxBout_num1);
            this.Controls.Add(this.textBoxIN_num1);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.LED_PLC);
            this.Controls.Add(this.checkBoxNoLower);
            this.Controls.Add(this.checkBoxNoUpper);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.LED_AGVWorkOut);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.led_AGVAllowOut);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.led_AGVAllowIn);
            this.Controls.Add(this.label40);
            this.Controls.Add(this.LED_AGVWorkIn);
            this.Controls.Add(this.LED_SafeOut2);
            this.Controls.Add(this.LED_SafeIn2);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.LED_SafeOut1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.LED_SafeIn1);
            this.Controls.Add(this.OutLedUp_4);
            this.Controls.Add(this.OutLedUp_3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonManualFinished);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.textBoxAGVstep);
            this.Controls.Add(this.buttonClearList);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAGV_COM2";
            this.Padding = new System.Windows.Forms.Padding(0);
            this.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.ShowTitle = false;
            this.Style = Sunny.UI.UIStyle.Gray;
            this.StyleCustomMode = true;
            this.Text = "FormAGV_COM(面对接驳台右侧)";
            this.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAGV_COM2_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormAGV_COM_FormClosed);
            this.Load += new System.EventHandler(this.FormAGV_COM_Load);
            ((System.ComponentModel.ISupportInitialize)(this.OutLedUp_3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutLedUp_4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_SafeOut2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_SafeIn2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_SafeOut1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_SafeIn1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_AGVWorkOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.led_AGVAllowOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.led_AGVAllowIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_AGVWorkIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LED_PLC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ledSyl_In2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ledSyl_In1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axActUtlType1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timerAGV;
        private System.Windows.Forms.Button buttonClearList;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxAGVstep;
        private System.Windows.Forms.Button buttonManualFinished;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timerAGVIO;
        public NationalInstruments.UI.WindowsForms.Led OutLedUp_3;
        public NationalInstruments.UI.WindowsForms.Led OutLedUp_4;
        public NationalInstruments.UI.WindowsForms.Led LED_SafeOut2;
        public NationalInstruments.UI.WindowsForms.Led LED_SafeIn2;
        private System.Windows.Forms.Label label11;
        public NationalInstruments.UI.WindowsForms.Led LED_SafeOut1;
        private System.Windows.Forms.Label label10;
        public NationalInstruments.UI.WindowsForms.Led LED_SafeIn1;
        private System.Windows.Forms.Label label14;
        private NationalInstruments.UI.WindowsForms.Led LED_AGVWorkOut;
        private System.Windows.Forms.Label label9;
        public NationalInstruments.UI.WindowsForms.Led led_AGVAllowOut;
        private System.Windows.Forms.Label label7;
        public NationalInstruments.UI.WindowsForms.Led led_AGVAllowIn;
        private System.Windows.Forms.Label label40;
        private NationalInstruments.UI.WindowsForms.Led LED_AGVWorkIn;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Timer timerConnect;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.CheckBox checkBoxNoLower;
        private System.Windows.Forms.CheckBox checkBoxNoUpper;
        private System.Windows.Forms.Label label16;
        public NationalInstruments.UI.WindowsForms.Led LED_PLC;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox textBoxBout_num2;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox textBoxIN_num2;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox textBoxBout_num1;
        private System.Windows.Forms.TextBox textBoxIN_num1;
        public NationalInstruments.UI.WindowsForms.Led ledSyl_In2;
        public NationalInstruments.UI.WindowsForms.Led ledSyl_In1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label13;
        private AxActUtlTypeLib.AxActUtlType axActUtlType1;
        private System.Windows.Forms.RichTextBox LogBox;
        private System.Windows.Forms.Timer ComHandlerTimer;
        private System.Windows.Forms.Timer LogTimer;
    }
}