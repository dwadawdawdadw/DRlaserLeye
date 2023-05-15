namespace MesOpcServer
{
    partial class FormSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.EqpNum = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ProcessNum = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioBtn10In10Out = new System.Windows.Forms.RadioButton();
            this.radioBtn12In12Out = new System.Windows.Forms.RadioButton();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // EqpNum
            // 
            this.EqpNum.Location = new System.Drawing.Point(88, 51);
            this.EqpNum.Name = "EqpNum";
            this.EqpNum.Size = new System.Drawing.Size(89, 26);
            this.EqpNum.TabIndex = 5;
            this.EqpNum.TextChanged += new System.EventHandler(this.EqpNum_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "设备号：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.ProcessNum);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.EqpNum);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(14, 49);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(430, 255);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "帝尔设备信息设置";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(183, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(224, 16);
            this.label1.TabIndex = 9;
            this.label1.Text = "（01~26，奇数A侧，偶数B侧）";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(183, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(128, 16);
            this.label5.TabIndex = 8;
            this.label5.Text = "（C1:53 C2:73）";
            // 
            // ProcessNum
            // 
            this.ProcessNum.Location = new System.Drawing.Point(88, 122);
            this.ProcessNum.Name = "ProcessNum";
            this.ProcessNum.Size = new System.Drawing.Size(89, 26);
            this.ProcessNum.TabIndex = 7;
            this.ProcessNum.TextChanged += new System.EventHandler(this.ProcessNum_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 125);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "工序号：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioBtn10In10Out);
            this.groupBox3.Controls.Add(this.radioBtn12In12Out);
            this.groupBox3.Font = new System.Drawing.Font("黑体", 12F);
            this.groupBox3.Location = new System.Drawing.Point(464, 49);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(227, 255);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "AGV对接参数设置";
            // 
            // radioBtn10In10Out
            // 
            this.radioBtn10In10Out.AutoSize = true;
            this.radioBtn10In10Out.Location = new System.Drawing.Point(7, 72);
            this.radioBtn10In10Out.Name = "radioBtn10In10Out";
            this.radioBtn10In10Out.Size = new System.Drawing.Size(90, 20);
            this.radioBtn10In10Out.TabIndex = 10;
            this.radioBtn10In10Out.TabStop = true;
            this.radioBtn10In10Out.Text = "10进10出";
            this.radioBtn10In10Out.UseVisualStyleBackColor = true;
            this.radioBtn10In10Out.MouseClick += new System.Windows.Forms.MouseEventHandler(this.radioBtn10In10Out_MouseClick);
            // 
            // radioBtn12In12Out
            // 
            this.radioBtn12In12Out.AutoSize = true;
            this.radioBtn12In12Out.Location = new System.Drawing.Point(7, 37);
            this.radioBtn12In12Out.Name = "radioBtn12In12Out";
            this.radioBtn12In12Out.Size = new System.Drawing.Size(90, 20);
            this.radioBtn12In12Out.TabIndex = 9;
            this.radioBtn12In12Out.TabStop = true;
            this.radioBtn12In12Out.Text = "12进12出";
            this.radioBtn12In12Out.UseVisualStyleBackColor = true;
            this.radioBtn12In12Out.CheckedChanged += new System.EventHandler(this.radioBtn12In12Out_CheckedChanged);
            this.radioBtn12In12Out.MouseClick += new System.Windows.Forms.MouseEventHandler(this.radioBtn12In12Out_MouseClick);
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 324);
            this.ControlBoxFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(160)))), ((int)(((byte)(165)))));
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.Style = Sunny.UI.UIStyle.Custom;
            this.StyleCustomMode = true;
            this.Text = "通信设置";
            this.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSettings_FormClosing);
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.Shown += new System.EventHandler(this.FormSettings_Shown);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox EqpNum;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox ProcessNum;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioBtn10In10Out;
        private System.Windows.Forms.RadioButton radioBtn12In12Out;
    }
}