using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace One_Account
{
    public partial class SQLErrorLog : Form
    {
        public SQLErrorLog()
        {
            InitializeComponent();
        }

        private void SQLErrorLog_Load(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\\Microsoft SQL Server\100\Setup Bootstrap\Log"))
                {
                    if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\\Microsoft SQL Server\100\Setup Bootstrap\Log\Summary.txt"))
                    {
                        RichTextBox rtbx = new RichTextBox();
                        rtbx.Dock = DockStyle.Fill;
                        rtbx.ReadOnly = true;
                        rtbx.Font = new Font("Arial", 10);
                        this.Controls.Add(rtbx);
                        rtbx.LoadFile(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\\Microsoft SQL Server\100\Setup Bootstrap\Log\Summary.txt", RichTextBoxStreamType.PlainText);
                    }
                }
            }
            catch (Exception ex)
            {
                Label lbl = new Label();
                lbl.Text = ex.Message;
                this.Controls.Add(lbl);
            }
          
        }
    }
}
