namespace MesOpcServer
{
    partial class FormProductionStatistics
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProductionStatistics));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.comboBoxDateChoose = new System.Windows.Forms.ComboBox();
            this.checkBoxHistoryData = new System.Windows.Forms.CheckBox();
            this.CurrentShiftProduction = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.LastShiftProduction = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.LastShiftInput = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.CurrentShiftInput = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonShadow;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 12F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column0,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            this.dataGridView1.GridColor = System.Drawing.Color.Black;
            this.dataGridView1.Location = new System.Drawing.Point(12, 128);
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 12F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.RowHeadersWidth = 20;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(918, 474);
            this.dataGridView1.TabIndex = 0;
            // 
            // Column0
            // 
            this.Column0.FillWeight = 50.76142F;
            this.Column0.HeaderText = "时间";
            this.Column0.Name = "Column0";
            // 
            // Column1
            // 
            this.Column1.FillWeight = 112.3096F;
            this.Column1.HeaderText = "硅片流入";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.FillWeight = 112.3096F;
            this.Column2.HeaderText = "硅片流出";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.FillWeight = 112.3096F;
            this.Column3.HeaderText = "花篮流入";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Column4.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column4.FillWeight = 112.3096F;
            this.Column4.HeaderText = "花篮流出";
            this.Column4.Name = "Column4";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(400, 608);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 31);
            this.button1.TabIndex = 1;
            this.button1.Text = "刷新";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // comboBoxDateChoose
            // 
            this.comboBoxDateChoose.FormattingEnabled = true;
            this.comboBoxDateChoose.Location = new System.Drawing.Point(12, 46);
            this.comboBoxDateChoose.Name = "comboBoxDateChoose";
            this.comboBoxDateChoose.Size = new System.Drawing.Size(195, 29);
            this.comboBoxDateChoose.TabIndex = 2;
            this.comboBoxDateChoose.SelectedIndexChanged += new System.EventHandler(this.comboBoxDateChoose_SelectedIndexChanged);
            // 
            // checkBoxHistoryData
            // 
            this.checkBoxHistoryData.AutoSize = true;
            this.checkBoxHistoryData.Location = new System.Drawing.Point(224, 50);
            this.checkBoxHistoryData.Name = "checkBoxHistoryData";
            this.checkBoxHistoryData.Size = new System.Drawing.Size(93, 25);
            this.checkBoxHistoryData.TabIndex = 3;
            this.checkBoxHistoryData.Text = "历史数据";
            this.checkBoxHistoryData.UseVisualStyleBackColor = true;
            this.checkBoxHistoryData.CheckedChanged += new System.EventHandler(this.checkBoxHistoryData_CheckedChanged);
            // 
            // CurrentShiftProduction
            // 
            this.CurrentShiftProduction.Location = new System.Drawing.Point(768, 86);
            this.CurrentShiftProduction.Name = "CurrentShiftProduction";
            this.CurrentShiftProduction.Size = new System.Drawing.Size(159, 29);
            this.CurrentShiftProduction.TabIndex = 4;
            this.CurrentShiftProduction.TextChanged += new System.EventHandler(this.CurrentShiftProduction_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(640, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 21);
            this.label1.TabIndex = 5;
            this.label1.Text = "当前班次产量：";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(348, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 21);
            this.label2.TabIndex = 7;
            this.label2.Text = "上一班次产量：";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // LastShiftProduction
            // 
            this.LastShiftProduction.Location = new System.Drawing.Point(476, 86);
            this.LastShiftProduction.Name = "LastShiftProduction";
            this.LastShiftProduction.Size = new System.Drawing.Size(158, 29);
            this.LastShiftProduction.TabIndex = 6;
            this.LastShiftProduction.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(348, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 21);
            this.label3.TabIndex = 11;
            this.label3.Text = "上一班次投入：";
            // 
            // LastShiftInput
            // 
            this.LastShiftInput.Location = new System.Drawing.Point(476, 44);
            this.LastShiftInput.Name = "LastShiftInput";
            this.LastShiftInput.Size = new System.Drawing.Size(158, 29);
            this.LastShiftInput.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(640, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 21);
            this.label4.TabIndex = 9;
            this.label4.Text = "当前班次投入：";
            // 
            // CurrentShiftInput
            // 
            this.CurrentShiftInput.Location = new System.Drawing.Point(768, 42);
            this.CurrentShiftInput.Name = "CurrentShiftInput";
            this.CurrentShiftInput.Size = new System.Drawing.Size(159, 29);
            this.CurrentShiftInput.TabIndex = 8;
            // 
            // FormProductionStatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(943, 649);
            this.ControlBoxFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(160)))), ((int)(((byte)(165)))));
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LastShiftInput);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CurrentShiftInput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.LastShiftProduction);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CurrentShiftProduction);
            this.Controls.Add(this.checkBoxHistoryData);
            this.Controls.Add(this.comboBoxDateChoose);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormProductionStatistics";
            this.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.Style = Sunny.UI.UIStyle.Gray;
            this.StyleCustomMode = true;
            this.Text = "产能统计";
            this.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormProductionStatistics_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox comboBoxDateChoose;
        private System.Windows.Forms.CheckBox checkBoxHistoryData;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column0;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.TextBox CurrentShiftProduction;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox LastShiftProduction;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox LastShiftInput;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox CurrentShiftInput;
    }
}

