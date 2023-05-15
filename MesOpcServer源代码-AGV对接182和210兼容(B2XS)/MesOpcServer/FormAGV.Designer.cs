namespace MesOpcServer
{
    partial class FormAGV
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAGV));
            this.panelAGV = new System.Windows.Forms.Panel();
            this.agvFormCom1 = new DeviceManeger.AGVComForm();
            this.panelAGV.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelAGV
            // 
            this.panelAGV.Controls.Add(this.agvFormCom1);
            this.panelAGV.Location = new System.Drawing.Point(12, 44);
            this.panelAGV.Name = "panelAGV";
            this.panelAGV.Size = new System.Drawing.Size(1060, 629);
            this.panelAGV.TabIndex = 0;
            // 
            // agvFormCom1
            // 
            this.agvFormCom1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.agvFormCom1.Location = new System.Drawing.Point(0, 0);
            this.agvFormCom1.Margin = new System.Windows.Forms.Padding(5);
            this.agvFormCom1.Name = "agvFormCom1";
            this.agvFormCom1.Size = new System.Drawing.Size(1053, 627);
            this.agvFormCom1.TabIndex = 0;
            this.agvFormCom1.Load += new System.EventHandler(this.agvFormCom1_Load);
            // 
            // FormAGV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1075, 681);
            this.ControlBoxFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(160)))), ((int)(((byte)(165)))));
            this.Controls.Add(this.panelAGV);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormAGV";
            this.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.Style = Sunny.UI.UIStyle.Custom;
            this.StyleCustomMode = true;
            this.Text = "AGV对接监控(B2XS)";
            this.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAGV_FormClosing);
            this.Load += new System.EventHandler(this.FormAGV_Load);
            this.panelAGV.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelAGV;
        private DeviceManeger.AGVComForm agvFormCom1;
    }
}