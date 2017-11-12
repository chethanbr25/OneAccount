/* One account Main Program File */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;

namespace One_Account
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmSplash());
            if (Environment.ExitCode == 565568)
            {
                Application.Run(new formMDI());
            }
            else
            {
                string userId = (ConfigurationManager.AppSettings["MsSqlUserId"] == null || ConfigurationManager.AppSettings["MsSqlUserId"].ToString() == string.Empty) ? null : ConfigurationManager.AppSettings["MsSqlUserId"].ToString();
                string password = (ConfigurationManager.AppSettings["MsSqlPassword"] == null || ConfigurationManager.AppSettings["MsSqlPassword"].ToString() == string.Empty) ? null : ConfigurationManager.AppSettings["MsSqlPassword"].ToString();
                if (password==null)
                {
                    Application.Run(new frmMsSqlInstallerforOneaccount());
                }
                else
                {
                    Login obj = new Login(userId ,password);
                    if (obj.ShowDialog()==DialogResult.OK)
                    {
                        Application.Run(new frmMsSqlInstallerforOneaccount());
                    }
                }

            }
            if (Environment.ExitCode == 100)
            {
                Application.Run(new formMDI());
            }
            else if (Environment.ExitCode == 101)
            {
                MessageBox.Show("Oneaccount database configuration process error. there have some privileges issue!. Pease contact Administrator", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (Environment.ExitCode == 102)
            {
                MessageBox.Show("Oneaccount database configuration process error. can't get database files!. Pease contact Administrator", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
