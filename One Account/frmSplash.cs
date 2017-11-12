using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Reflection;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;

namespace One_Account
{
    public partial class frmSplash : Form
    {
        #region Variables
        #endregion
        #region Functions
        /// <summary>
        /// create an instance of frmSplash class
        /// </summary>
        public frmSplash()
        {
            InitializeComponent();
            lblVersion.Text = Application.ProductVersion;
        }
        /// <summary>
        /// Function for check the DataBase Connection
        /// </summary>
        /// <returns></returns>
        private bool CheckDataBase()
        {
            bool isTrue = false;
            string serverName = (ConfigurationManager.AppSettings["MsSqlServer"] == null || ConfigurationManager.AppSettings["MsSqlServer"].ToString() == string.Empty) ? null : ConfigurationManager.AppSettings["MsSqlServer"].ToString();
            string userId = (ConfigurationManager.AppSettings["MsSqlUserId"] == null || ConfigurationManager.AppSettings["MsSqlUserId"].ToString() == string.Empty) ? null : ConfigurationManager.AppSettings["MsSqlUserId"].ToString();
            string password = (ConfigurationManager.AppSettings["MsSqlPassword"] == null || ConfigurationManager.AppSettings["MsSqlPassword"].ToString() == string.Empty) ? null : ConfigurationManager.AppSettings["MsSqlPassword"].ToString();
            string ApplicationPath = (ConfigurationManager.AppSettings["ApplicationPath"] == null || ConfigurationManager.AppSettings["ApplicationPath"].ToString() == string.Empty) ? null : ConfigurationManager.AppSettings["ApplicationPath"].ToString();
            string isSqlServer = (ConfigurationManager.AppSettings["isServerConnection"] == null || ConfigurationManager.AppSettings["isServerConnection"].ToString() == string.Empty) ? null : ConfigurationManager.AppSettings["isServerConnection"].ToString();
            SClass objDb = new SClass();
            if (objDb.CheckMsSqlConnection(serverName, userId, password, ApplicationPath))
            {
                isTrue = true;
            }
            else
            {
                isTrue = false;

            }
            return isTrue;
        }
        /// <summary>
        /// Function to check the version and to display the message for upfdation
        /// </summary>
        public void CheckNewVersionComesOfOneaccount()
        {
            DateTime dtLastCheckDate = DateTime.Today;
            int inInterval = 0;
            try
            {
                dtLastCheckDate = DateTime.Parse(ConfigurationManager.AppSettings["LastCheckDay"].ToString());
                inInterval = int.Parse(ConfigurationManager.AppSettings["UpdateCheck"].ToString());
            }
            catch { /*catch any exception */}
            if (DateTime.Today >= dtLastCheckDate.AddDays(inInterval))
            {
                if (CheckForInternetConnection())
                {
                    try
                    {
                        Assembly assembly = Assembly.GetExecutingAssembly();
                        FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                        string version = fileVersionInfo.ProductVersion;
                        var result = string.Empty;
                        using (WebClient client = new WebClient())
                        {
                            string htmlCode = client.DownloadString("http://www.Oneaccount.com/One_Account_mysql_version.htm");
                            var regex = new Regex(@"<span id=""version"" class=""version"">(.*?)</span>");
                            var match = regex.Match("?S_" + htmlCode);
                            result = match.Groups[1].Value;
                            var msg = new Regex(@"<span id=""msg"" class=""msg"">(.*?)</span>");
                            var message = msg.Match("?S_" + htmlCode);
                            var messageToShow = message.Groups[1].Value;
                            var head = new Regex(@"<span id=""heading"" class=""heading"">(.*?)</span>");
                            var check = head.Match("?S_" + htmlCode);
                            var heading = check.Groups[1].Value;
                            if (messageToShow.ToString().Trim() != string.Empty)
                            {
                                PublicVariables.MessageToShow = messageToShow.ToString();
                                PublicVariables.MessageHeadear = heading.ToString().Trim();
                            }
                        }
                        if (Test(version, result, -1) == "New")
                        {
                            ntfyVersionUpdate.Visible = true;
                            ntfyVersionUpdate.BalloonTipIcon = ToolTipIcon.Info;
                            ntfyVersionUpdate.BalloonTipText = "New version available";
                            ntfyVersionUpdate.BalloonTipTitle = "Oneaccount";
                            ntfyVersionUpdate.ShowBalloonTip(1000);
                            ntfyVersionUpdate.Text = "Update Oneaccount from " + version + " to " + result;
                        }
                        UpdateSetting("LastCheckDay", DateTime.Today.ToString("dd-MMM-yyyy"));
                    }
                    catch { }
                }
            }
        }
        /// <summary>
        /// Function to check the internet connection
        /// </summary>
        /// <returns></returns>
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Function to compare the versions
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <param name="expected"></param>
        /// <returns></returns>
        private static string Test(string lhs, string rhs, int expected)
        {
            int result = 0;
            try
            {
                result = CompareVersions(lhs, rhs);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            return result.Equals(expected) ? "New" : "Same";
        }
        /// <summary>
        /// Function to check the updation status messages
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void UpdateSetting(string key, string value)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings[key].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch { }

        }
        /// <summary>
        /// Function to replace the version
        /// </summary>
        /// <param name="strA"></param>
        /// <param name="strB"></param>
        /// <returns></returns>
        public static int CompareVersions(String strA, String strB)
        {
            Version vA = new Version(strA.Replace(",", "."));
            Version vB = new Version(strB.Replace(",", "."));
            return vA.CompareTo(vB);
        }
        #endregion
        #region Events
        /// <summary>
        /// On Notify versionUpdate click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ntfyVersionUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("http://www.Oneaccount.com/update.aspx");
                ntfyVersionUpdate.Visible = false;
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// On Timer  timer1 Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            CheckNewVersionComesOfOneaccount();
            if (CheckDataBase())
            {
                Environment.ExitCode = 565568;
                this.Close();
            }
            else
            {
                Environment.ExitCode = 565556;
                this.Close();
            }
        }
        /// <summary>
        /// On Form frmSplash Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSplash_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
        #endregion
    }
}
