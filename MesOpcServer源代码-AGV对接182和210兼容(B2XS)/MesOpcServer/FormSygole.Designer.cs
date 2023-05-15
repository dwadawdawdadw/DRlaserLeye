namespace MesOpcServer
{
    partial class FormSygoleRFID
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSygoleRFID));
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisConnect = new System.Windows.Forms.Button();
            this.CheckAndUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.LED_State = new NationalInstruments.UI.WindowsForms.Led();
            this.LogBox = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.清除内容ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadRFID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnReadRFID = new System.Windows.Forms.Button();
            this.LoadRFIDDisable = new System.Windows.Forms.CheckBox();
            this.UnLoadRFID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnReadUnLoadRFID = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.LED_State)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnect.Location = new System.Drawing.Point(66, 47);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(91, 33);
            this.btnConnect.TabIndex = 10;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisConnect
            // 
            this.btnDisConnect.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnDisConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisConnect.Location = new System.Drawing.Point(186, 46);
            this.btnDisConnect.Name = "btnDisConnect";
            this.btnDisConnect.Size = new System.Drawing.Size(91, 34);
            this.btnDisConnect.TabIndex = 11;
            this.btnDisConnect.Text = "断开";
            this.btnDisConnect.UseVisualStyleBackColor = false;
            this.btnDisConnect.Click += new System.EventHandler(this.btnDisConnect_Click);
            // 
            // CheckAndUpdateTimer
            // 
            this.CheckAndUpdateTimer.Tick += new System.EventHandler(this.CheckAndUpdateTimer_Tick);
            // 
            // LED_State
            // 
            this.LED_State.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.LED_State.Location = new System.Drawing.Point(13, 41);
            this.LED_State.Name = "LED_State";
            this.LED_State.Size = new System.Drawing.Size(45, 45);
            this.LED_State.TabIndex = 16;
            // 
            // LogBox
            // 
            this.LogBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.LogBox.ContextMenuStrip = this.contextMenuStrip;
            this.LogBox.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LogBox.ForeColor = System.Drawing.Color.Black;
            this.LogBox.Location = new System.Drawing.Point(425, 51);
            this.LogBox.Name = "LogBox";
            this.LogBox.Size = new System.Drawing.Size(648, 538);
            this.LogBox.TabIndex = 17;
            this.LogBox.Text = "";
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.清除内容ToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(125, 26);
            // 
            // 清除内容ToolStripMenuItem
            // 
            this.清除内容ToolStripMenuItem.Name = "清除内容ToolStripMenuItem";
            this.清除内容ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.清除内容ToolStripMenuItem.Text = "清除内容";
            this.清除内容ToolStripMenuItem.Click += new System.EventHandler(this.清除内容ToolStripMenuItem_Click);
            // 
            // LoadRFID
            // 
            this.LoadRFID.BackColor = System.Drawing.SystemColors.Info;
            this.LoadRFID.Location = new System.Drawing.Point(138, 169);
            this.LoadRFID.Name = "LoadRFID";
            this.LoadRFID.Size = new System.Drawing.Size(231, 29);
            this.LoadRFID.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 172);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 21);
            this.label1.TabIndex = 20;
            this.label1.Text = "进料花篮ID:";
            // 
            // btnReadRFID
            // 
            this.btnReadRFID.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnReadRFID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReadRFID.Location = new System.Drawing.Point(160, 218);
            this.btnReadRFID.Name = "btnReadRFID";
            this.btnReadRFID.Size = new System.Drawing.Size(178, 35);
            this.btnReadRFID.TabIndex = 21;
            this.btnReadRFID.Text = "进料花篮ID读取";
            this.btnReadRFID.UseVisualStyleBackColor = false;
            this.btnReadRFID.Click += new System.EventHandler(this.btnReadRFID_Click);
            // 
            // LoadRFIDDisable
            // 
            this.LoadRFIDDisable.AutoSize = true;
            this.LoadRFIDDisable.Location = new System.Drawing.Point(66, 99);
            this.LoadRFIDDisable.Name = "LoadRFIDDisable";
            this.LoadRFIDDisable.Size = new System.Drawing.Size(129, 25);
            this.LoadRFIDDisable.TabIndex = 22;
            this.LoadRFIDDisable.Text = "进料RFID屏蔽";
            this.LoadRFIDDisable.UseVisualStyleBackColor = true;
            this.LoadRFIDDisable.CheckedChanged += new System.EventHandler(this.LoadRFIDDisable_CheckedChanged);
            // 
            // UnLoadRFID
            // 
            this.UnLoadRFID.BackColor = System.Drawing.SystemColors.Info;
            this.UnLoadRFID.Location = new System.Drawing.Point(138, 325);
            this.UnLoadRFID.Name = "UnLoadRFID";
            this.UnLoadRFID.Size = new System.Drawing.Size(231, 29);
            this.UnLoadRFID.TabIndex = 23;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 328);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 21);
            this.label2.TabIndex = 24;
            this.label2.Text = "出料花篮ID:";
            // 
            // btnReadUnLoadRFID
            // 
            this.btnReadUnLoadRFID.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnReadUnLoadRFID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReadUnLoadRFID.Location = new System.Drawing.Point(160, 377);
            this.btnReadUnLoadRFID.Name = "btnReadUnLoadRFID";
            this.btnReadUnLoadRFID.Size = new System.Drawing.Size(178, 35);
            this.btnReadUnLoadRFID.TabIndex = 25;
            this.btnReadUnLoadRFID.Text = "出料花篮ID读取";
            this.btnReadUnLoadRFID.UseVisualStyleBackColor = false;
            this.btnReadUnLoadRFID.Click += new System.EventHandler(this.btnReadUnLoadRFID_Click);
            // 
            // FormSygoleRFID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1086, 628);
            this.ControlBoxFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(160)))), ((int)(((byte)(165)))));
            this.Controls.Add(this.btnReadUnLoadRFID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.UnLoadRFID);
            this.Controls.Add(this.LoadRFIDDisable);
            this.Controls.Add(this.btnReadRFID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LoadRFID);
            this.Controls.Add(this.LogBox);
            this.Controls.Add(this.LED_State);
            this.Controls.Add(this.btnDisConnect);
            this.Controls.Add(this.btnConnect);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSygoleRFID";
            this.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.Style = Sunny.UI.UIStyle.Gray;
            this.StyleCustomMode = true;
            this.Text = "RFID监控";
            this.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSygoleRFID_FormClosing);
            this.Load += new System.EventHandler(this.FormSygoleRFID_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LED_State)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisConnect;
        private System.Windows.Forms.Timer CheckAndUpdateTimer;
        private NationalInstruments.UI.WindowsForms.Led LED_State;
        private System.Windows.Forms.RichTextBox LogBox;
        private System.Windows.Forms.TextBox LoadRFID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnReadRFID;
        private System.Windows.Forms.CheckBox LoadRFIDDisable;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 清除内容ToolStripMenuItem;
        private System.Windows.Forms.TextBox UnLoadRFID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnReadUnLoadRFID;
    }
}

