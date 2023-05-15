namespace DRMotion
{
    partial class FormAgv
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textUpStep = new System.Windows.Forms.TextBox();
            this.textDownStep = new System.Windows.Forms.TextBox();
            this.textDownTrack = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textUpTrack = new System.Windows.Forms.TextBox();
            this.led1 = new NationalInstruments.UI.WindowsForms.Led();
            this.led2 = new NationalInstruments.UI.WindowsForms.Led();
            this.led3 = new NationalInstruments.UI.WindowsForms.Led();
            this.led4 = new NationalInstruments.UI.WindowsForms.Led();
            this.led5 = new NationalInstruments.UI.WindowsForms.Led();
            this.led6 = new NationalInstruments.UI.WindowsForms.Led();
            this.led7 = new NationalInstruments.UI.WindowsForms.Led();
            this.led8 = new NationalInstruments.UI.WindowsForms.Led();
            this.label4 = new System.Windows.Forms.Label();
            this.textUpWcs = new System.Windows.Forms.TextBox();
            this.textDownWcs = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.timerMonitor = new System.Windows.Forms.Timer(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.textDownAgv = new System.Windows.Forms.TextBox();
            this.textUpAgv = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.label21 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.led1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.led2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.led3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.led4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.led5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.led6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.led7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.led8)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "上轨道对接";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 143);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "下轨道对接";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(91, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "步骤(Step)";
            // 
            // textUpStep
            // 
            this.textUpStep.Location = new System.Drawing.Point(93, 78);
            this.textUpStep.Name = "textUpStep";
            this.textUpStep.ReadOnly = true;
            this.textUpStep.Size = new System.Drawing.Size(51, 21);
            this.textUpStep.TabIndex = 4;
            // 
            // textDownStep
            // 
            this.textDownStep.Location = new System.Drawing.Point(93, 138);
            this.textDownStep.Name = "textDownStep";
            this.textDownStep.ReadOnly = true;
            this.textDownStep.Size = new System.Drawing.Size(51, 21);
            this.textDownStep.TabIndex = 5;
            // 
            // textDownTrack
            // 
            this.textDownTrack.Location = new System.Drawing.Point(157, 139);
            this.textDownTrack.Name = "textDownTrack";
            this.textDownTrack.ReadOnly = true;
            this.textDownTrack.Size = new System.Drawing.Size(83, 21);
            this.textDownTrack.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(164, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "轨道花篮数";
            // 
            // textUpTrack
            // 
            this.textUpTrack.Location = new System.Drawing.Point(157, 78);
            this.textUpTrack.Name = "textUpTrack";
            this.textUpTrack.ReadOnly = true;
            this.textUpTrack.Size = new System.Drawing.Size(79, 21);
            this.textUpTrack.TabIndex = 9;
            // 
            // led1
            // 
            this.led1.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.led1.Location = new System.Drawing.Point(462, 57);
            this.led1.Name = "led1";
            this.led1.Size = new System.Drawing.Size(37, 42);
            this.led1.TabIndex = 10;
            // 
            // led2
            // 
            this.led2.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.led2.Location = new System.Drawing.Point(515, 57);
            this.led2.Name = "led2";
            this.led2.Size = new System.Drawing.Size(40, 42);
            this.led2.TabIndex = 11;
            // 
            // led3
            // 
            this.led3.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.led3.Location = new System.Drawing.Point(568, 57);
            this.led3.Name = "led3";
            this.led3.Size = new System.Drawing.Size(36, 42);
            this.led3.TabIndex = 12;
            // 
            // led4
            // 
            this.led4.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.led4.Location = new System.Drawing.Point(621, 57);
            this.led4.Name = "led4";
            this.led4.Size = new System.Drawing.Size(37, 42);
            this.led4.TabIndex = 13;
            // 
            // led5
            // 
            this.led5.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.led5.Location = new System.Drawing.Point(462, 133);
            this.led5.Name = "led5";
            this.led5.Size = new System.Drawing.Size(37, 40);
            this.led5.TabIndex = 14;
            // 
            // led6
            // 
            this.led6.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.led6.Location = new System.Drawing.Point(515, 133);
            this.led6.Name = "led6";
            this.led6.Size = new System.Drawing.Size(40, 40);
            this.led6.TabIndex = 15;
            // 
            // led7
            // 
            this.led7.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.led7.Location = new System.Drawing.Point(567, 133);
            this.led7.Name = "led7";
            this.led7.Size = new System.Drawing.Size(37, 40);
            this.led7.TabIndex = 16;
            // 
            // led8
            // 
            this.led8.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
            this.led8.Location = new System.Drawing.Point(621, 133);
            this.led8.Name = "led8";
            this.led8.Size = new System.Drawing.Size(37, 40);
            this.led8.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(268, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 12);
            this.label4.TabIndex = 18;
            this.label4.Text = "WCS";
            // 
            // textUpWcs
            // 
            this.textUpWcs.Location = new System.Drawing.Point(246, 78);
            this.textUpWcs.Name = "textUpWcs";
            this.textUpWcs.ReadOnly = true;
            this.textUpWcs.Size = new System.Drawing.Size(79, 21);
            this.textUpWcs.TabIndex = 19;
            // 
            // textDownWcs
            // 
            this.textDownWcs.Location = new System.Drawing.Point(246, 139);
            this.textDownWcs.Name = "textDownWcs";
            this.textDownWcs.ReadOnly = true;
            this.textDownWcs.Size = new System.Drawing.Size(79, 21);
            this.textDownWcs.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(563, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 21;
            this.label6.Text = "出货中";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(502, 42);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 22;
            this.label7.Text = "请求出货";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(459, 42);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 23;
            this.label8.Text = "到位";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(615, 42);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 24;
            this.label9.Text = "出货完成";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(166, 199);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(132, 29);
            this.button1.TabIndex = 25;
            this.button1.Text = "下轨道强制对接完成";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(319, 206);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(102, 16);
            this.checkBox1.TabIndex = 26;
            this.checkBox1.Text = "不进行AGV对接";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // timerMonitor
            // 
            this.timerMonitor.Enabled = true;
            this.timerMonitor.Interval = 500;
            this.timerMonitor.Tick += new System.EventHandler(this.timerMonitor_Tick);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 199);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(132, 29);
            this.button2.TabIndex = 27;
            this.button2.Text = "上轨道强制对接完成";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(429, 206);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(126, 16);
            this.checkBox2.TabIndex = 28;
            this.checkBox2.Text = "不进行AGV失败报警";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // textDownAgv
            // 
            this.textDownAgv.Location = new System.Drawing.Point(351, 138);
            this.textDownAgv.Name = "textDownAgv";
            this.textDownAgv.ReadOnly = true;
            this.textDownAgv.Size = new System.Drawing.Size(79, 21);
            this.textDownAgv.TabIndex = 33;
            // 
            // textUpAgv
            // 
            this.textUpAgv.Location = new System.Drawing.Point(351, 78);
            this.textUpAgv.Name = "textUpAgv";
            this.textUpAgv.ReadOnly = true;
            this.textUpAgv.Size = new System.Drawing.Size(79, 21);
            this.textUpAgv.TabIndex = 32;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(373, 49);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(41, 12);
            this.label19.TabIndex = 31;
            this.label19.Text = "agvNum";
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(319, 184);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(120, 16);
            this.checkBox5.TabIndex = 36;
            this.checkBox5.Text = "不检查对接传感器";
            this.checkBox5.UseVisualStyleBackColor = true;
            this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBox5_CheckedChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(615, 188);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(29, 12);
            this.label21.TabIndex = 35;
            this.label21.Text = "心跳";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(589, 204);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(79, 21);
            this.textBox1.TabIndex = 34;
            // 
            // FormAgv
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 240);
            this.Controls.Add(this.checkBox5);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.textDownAgv);
            this.Controls.Add(this.textUpAgv);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textDownWcs);
            this.Controls.Add(this.textUpWcs);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.led8);
            this.Controls.Add(this.led7);
            this.Controls.Add(this.led6);
            this.Controls.Add(this.led5);
            this.Controls.Add(this.led4);
            this.Controls.Add(this.led3);
            this.Controls.Add(this.led2);
            this.Controls.Add(this.led1);
            this.Controls.Add(this.textUpTrack);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textDownTrack);
            this.Controls.Add(this.textDownStep);
            this.Controls.Add(this.textUpStep);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FormAgv";
            this.Text = "AGVMonitor";
            ((System.ComponentModel.ISupportInitialize)(this.led1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.led2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.led3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.led4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.led5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.led6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.led7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.led8)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textUpStep;
        private System.Windows.Forms.TextBox textDownStep;
        private System.Windows.Forms.TextBox textDownTrack;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textUpTrack;
        private NationalInstruments.UI.WindowsForms.Led led1;
        private NationalInstruments.UI.WindowsForms.Led led2;
        private NationalInstruments.UI.WindowsForms.Led led3;
        private NationalInstruments.UI.WindowsForms.Led led4;
        private NationalInstruments.UI.WindowsForms.Led led5;
        private NationalInstruments.UI.WindowsForms.Led led6;
        private NationalInstruments.UI.WindowsForms.Led led7;
        private NationalInstruments.UI.WindowsForms.Led led8;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textUpWcs;
        private System.Windows.Forms.TextBox textDownWcs;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Timer timerMonitor;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.TextBox textDownAgv;
        private System.Windows.Forms.TextBox textUpAgv;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox textBox1;
    }
}