namespace WindowsApplication1
{
    partial class FormPassword
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
            this.Operator = new System.Windows.Forms.Button();
            this.Login = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textpassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxPass = new System.Windows.Forms.ComboBox();
            this.PassModify = new System.Windows.Forms.Button();
            this.ResetPassword = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Operator
            // 
            this.Operator.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Operator.Location = new System.Drawing.Point(210, 96);
            this.Operator.Name = "Operator";
            this.Operator.Size = new System.Drawing.Size(70, 23);
            this.Operator.TabIndex = 11;
            this.Operator.Text = "作业员";
            this.Operator.UseVisualStyleBackColor = true;
            this.Operator.Click += new System.EventHandler(this.Operator_Click);
            // 
            // Login
            // 
            this.Login.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Login.Location = new System.Drawing.Point(68, 96);
            this.Login.Name = "Login";
            this.Login.Size = new System.Drawing.Size(70, 23);
            this.Login.TabIndex = 10;
            this.Login.Text = "登录";
            this.Login.UseVisualStyleBackColor = true;
            this.Login.Click += new System.EventHandler(this.Login_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(66, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "密码：";
            // 
            // textpassword
            // 
            this.textpassword.Location = new System.Drawing.Point(113, 57);
            this.textpassword.Name = "textpassword";
            this.textpassword.PasswordChar = '*';
            this.textpassword.Size = new System.Drawing.Size(131, 21);
            this.textpassword.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(66, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "账号：";
            // 
            // comboBoxPass
            // 
            this.comboBoxPass.FormattingEnabled = true;
            this.comboBoxPass.Location = new System.Drawing.Point(113, 19);
            this.comboBoxPass.Name = "comboBoxPass";
            this.comboBoxPass.Size = new System.Drawing.Size(131, 20);
            this.comboBoxPass.TabIndex = 6;
            this.comboBoxPass.Text = "-----请选择-----";
            this.comboBoxPass.SelectedIndexChanged += new System.EventHandler(this.comboBoxPass_SelectedIndexChanged);
            // 
            // PassModify
            // 
            this.PassModify.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PassModify.Location = new System.Drawing.Point(142, 139);
            this.PassModify.Name = "PassModify";
            this.PassModify.Size = new System.Drawing.Size(70, 23);
            this.PassModify.TabIndex = 12;
            this.PassModify.Text = "密码修改";
            this.PassModify.UseVisualStyleBackColor = true;
            this.PassModify.Click += new System.EventHandler(this.PassModify_Click);
            // 
            // ResetPassword
            // 
            this.ResetPassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ResetPassword.Location = new System.Drawing.Point(142, 168);
            this.ResetPassword.Name = "ResetPassword";
            this.ResetPassword.Size = new System.Drawing.Size(70, 23);
            this.ResetPassword.TabIndex = 13;
            this.ResetPassword.Text = "重置密码";
            this.ResetPassword.UseVisualStyleBackColor = true;
            this.ResetPassword.Visible = false;
            this.ResetPassword.Click += new System.EventHandler(this.ResetPassword_Click);
            // 
            // FormPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 195);
            this.Controls.Add(this.ResetPassword);
            this.Controls.Add(this.PassModify);
            this.Controls.Add(this.Operator);
            this.Controls.Add(this.Login);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textpassword);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxPass);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "密码登录";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public static uint g_Authority;
        private System.Windows.Forms.Button Operator;
        private System.Windows.Forms.Button Login;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textpassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxPass;
        private System.Windows.Forms.Button PassModify;
        private System.Windows.Forms.Button ResetPassword;
    }
}

