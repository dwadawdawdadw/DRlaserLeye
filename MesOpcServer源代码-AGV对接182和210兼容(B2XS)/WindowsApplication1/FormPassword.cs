using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using Microsoft.VisualBasic;


namespace WindowsApplication1
{
    public delegate void ChangePWFormData(bool topmost);
    public partial class FormPassword : Form
    {
        string LoginPath = @"C:\Security\Login.xml";
        string LoginDir = @"C:\Security";
  
        string AdminPass , EngineerPass, VendorPass;

        public event ChangePWFormData ChangeData;
        public FormPassword()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            XmlFile xmlLogin = new XmlFile();
            

             // 判断配置文件是否存在
            if (!File.Exists(LoginPath))
            {

                xmlLogin.CreateXml(LoginPath);
                xmlLogin.OpenXml(LoginPath);

                xmlLogin.WriteValue("Administrator", GetPass("#DRLASER#", "#DRLASER#"));
                xmlLogin.WriteValue("Engineer", GetPass("engineer", "engineer"));
                xmlLogin.WriteValue("Vendor", GetPass("vendor", "vendor"));
            

                File.SetAttributes(LoginDir, FileAttributes.Hidden);
                
            }
            else
            {
                xmlLogin.OpenXml(LoginPath);
  
            }

            xmlLogin.ReadValue("Administrator", ref AdminPass);
            xmlLogin.ReadValue("Engineer", ref EngineerPass);
            xmlLogin.ReadValue("Vendor", ref VendorPass);

            comboBoxPass.Items.Insert(0, "Administrator");
            comboBoxPass.Items.Insert(1, "Engineer");
            comboBoxPass.Items.Insert(2, "Vendor");



           

  
        }

        #region "加密函数"
        public static string GetPass(string pass, string key)
        {
            //调用MD5生成密码
            return MD5(MD5(pass) + key);
        }

        /// MD5加密（去除“-”）得到字符串      
        /// <returns>加密后的字符串</returns>
        public static string MD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(str)));
            t2 = t2.Replace("-", "").ToLower();
            return t2;
        }
        #endregion "加密函数"


        private void Operator_Click(object sender, EventArgs e)
        {
            ResetPassword.Visible = false;
            g_Authority = 0;
        }

        private void Login_Click(object sender, EventArgs e)
        {


            string Person = comboBoxPass.Text;

            if (textpassword.Text != "")
            {

                switch (Person)
                {
                    case "Administrator":

                        if (GetPass(textpassword.Text, "#DRLASER#") == AdminPass)
                        {
                            ResetPassword.Visible = true;
                            g_Authority = 1;
                           // MessageBox.Show("管理员模式登录成功！");
                            ChangeData(true);
                        }
                        else
                        {
                            
                            MessageBox.Show("密码错误！");
                        }

                        break;

                    case "Engineer":

                        if (GetPass(textpassword.Text, "engineer") == EngineerPass)
                        {
                            ResetPassword.Visible = false;
                            g_Authority = 2;
                           // MessageBox.Show("工程师模式登录成功！");
                            ChangeData(true);
                        }
                        else
                        {
                           
                            MessageBox.Show("密码错误！");
                        }

                        break;

                    case "Vendor":

                        if (GetPass(textpassword.Text, "vendor") == VendorPass)
                        {
                            ResetPassword.Visible = false;
                            g_Authority = 3;
                           // MessageBox.Show("厂商模式登录成功！");
                            ChangeData(true);
                        }
                        else
                        {
                           
                            MessageBox.Show("密码错误！");
                        }

                        break;

                    default:



                        break;
                }

                textpassword.Text = "";
            }
            else
            {
                MessageBox.Show("密码不能为空！");
            }

        }

     

        private void PassModify_Click(object sender, EventArgs e)
        {

            string Person = comboBoxPass.Text;
            string str1, str2;

            XmlFile xmlLogin = new XmlFile();
            xmlLogin.OpenXml(LoginPath);

            switch (Person)
            {
                case "Administrator":

                    if (GetPass(textpassword.Text, "#DRLASER#") == AdminPass)
                    {

                        str1 = Interaction.InputBox("请输入新的密码", "密码修改", "", -1, -1);
                        if (str1 == "")
                        {
                            textpassword.Text = "";
                            MessageBox.Show("密码不能为空！");
                            return;
                        }

                        str2 = Interaction.InputBox("请再次输入新的密码", "密码修改", "", -1, -1);
                        textpassword.Text = "";

                        if (str1 != str2)
                        {
                            MessageBox.Show("两次输入密码不一致");
                        }
                        else
                        {
                            
                           xmlLogin.WriteValue("Administrator", GetPass(str1, "#DRLASER#"));
                           xmlLogin.ReadValue("Administrator", ref AdminPass);
                          
                          
                           MessageBox.Show("密码修改成功！");

                           

                        }
                     
                    }
                    else
                    {
                        textpassword.Text = "";
                        MessageBox.Show("密码错误！");
                    }

                    break;

                case "Engineer":

                    if (GetPass(textpassword.Text, "engineer") == EngineerPass)
                    {

                        str1 = Interaction.InputBox("请输入新的密码", "密码修改", "", -1, -1);

                        if (str1 == "")
                        {
                            textpassword.Text = "";
                            MessageBox.Show("密码不能为空");
                            return;
                        }

                        str2 = Interaction.InputBox("请再次输入新的密码", "密码修改", "", -1, -1);
                        
                        textpassword.Text = "";

                        if (str1 != str2)
                        {
                            MessageBox.Show("两次输入密码不一致");
                        }
                        else
                        {

                            xmlLogin.WriteValue("Engineer", GetPass(str1, "engineer"));
                            xmlLogin.ReadValue("Engineer", ref EngineerPass);
                            
                          
                            MessageBox.Show("密码修改成功！");


                        }
                        
                    }
                    else
                    {
                        textpassword.Text = "";
                        MessageBox.Show("密码错误！");
                    }

                    break;

                case "Vendor":

                    if (GetPass(textpassword.Text, "vendor") == VendorPass)
                    {

                        str1 = Interaction.InputBox("请输入新的密码", "密码修改", "", -1, -1);

                        if (str1 == "")
                        {
                            textpassword.Text = "";
                            MessageBox.Show("密码不能为空");
                            return;
                        }

                        str2 = Interaction.InputBox("请再次输入新的密码", "密码修改", "", -1, -1);
                        textpassword.Text = "";

                        if (str1 != str2)
                        {
                            MessageBox.Show("两次输入密码不一致");
                        }
                        else
                        {

                            xmlLogin.WriteValue("Vendor", GetPass(str1, "vendor"));
                            xmlLogin.ReadValue("Vendor", ref VendorPass);
                            
                            MessageBox.Show("密码修改成功！");


                        }
                    }
                    else
                    {
                        textpassword.Text = "";
                        MessageBox.Show("密码错误！");
                    }

                    break;

                default:

                    textpassword.Text = "";


                    break;
            }



        }

        private void ResetPassword_Click(object sender, EventArgs e)
        {
            XmlFile xmlLogin = new XmlFile();

            xmlLogin.OpenXml(LoginPath);

            xmlLogin.WriteValue("Engineer", GetPass("engineer", "engineer"));
            xmlLogin.WriteValue("Vendor", GetPass("vendor", "vendor"));

            MessageBox.Show("密码已重置！");



        }

        private void comboBoxPass_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Person = comboBoxPass.Text;

            textpassword.Text = "";

            if(Person != "Administrator")
                ResetPassword.Visible = false;
        }


















    }
}
