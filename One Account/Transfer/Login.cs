using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace One_Account
{
    public partial class Login : Form
    {
        string UserNames;
        string Passwords;
        public Login(string UserName,string Password)
        {
            InitializeComponent();
            UserNames = UserName;
            Passwords = Password;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (UserNames != null && Passwords != null)
            {
                if (textBox1.Text == UserNames && Passwords == textBox2.Text)
                {
                     this.DialogResult = System.Windows.Forms.DialogResult.OK;
                     this.Close();
                }
                else
                {
                    MessageBox.Show("Sorry, password mismatch.","Oneaccount");
                }
            }
        }
    }
}
