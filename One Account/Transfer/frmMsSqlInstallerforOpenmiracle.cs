using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Configuration;
using System.Data.Sql;
using System.Web.Services.Protocols;
using System.Text.RegularExpressions;
namespace One_Account
{

    public partial class frmMsSqlInstallerforOneaccount : Form
    {
        public frmMsSqlInstallerforOneaccount()
        {
            try
            {
                InitializeComponent();
                bw.WorkerSupportsCancellation = true;
                bw.WorkerReportsProgress = true;
                bw.DoWork += new DoWorkEventHandler(bw_DoWork);
                bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

                bwDB.WorkerSupportsCancellation = true;
                bwDB.WorkerReportsProgress = true;
                bwDB.DoWork += new DoWorkEventHandler(bwDB_DoWork);
                bwDB.ProgressChanged += new ProgressChangedEventHandler(bwDB_ProgressChanged);
                bwDB.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwDB_RunWorkerCompleted);
            }
            catch (Exception)
            {

            }
        }

        void bwDB_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (objTbl != null)
            {
                cmbServers1.DataSource = objTbl.DefaultView.ToTable(true, "Server");
                cmbServers1.DisplayMember = "Server";
                cmbServers1.ValueMember = "Server";
            }
        }

        void bwDB_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 5)
            {
                panel3.Enabled = false;
                panel2.Enabled = false;
                progressBar1.Visible = true;
            }
            else if (e.ProgressPercentage == 95)
            {
                panel3.Enabled = true;
                panel2.Enabled = true;
                progressBar1.Visible = false;
            }
        }

        void bwDB_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker1 = sender as BackgroundWorker;
            worker1.ReportProgress((5));
            if (rbtnLocalServer.Checked)
            {
                objTbl = new SClass().GetLocalInstance();
            }
            else
            {
                objTbl = Microsoft.SqlServer.Management.Smo.SmoApplication.EnumAvailableSqlServers();
            }

            worker1.ReportProgress((95));
        }

        private void frmMySqlInstallerforOneaccount_Load(object sender, EventArgs e)
        {
            if (bwDB.IsBusy != true)
            {
                bwDB.RunWorkerAsync();
            }
        }


        BackgroundWorker bw = new BackgroundWorker();
        BackgroundWorker bwDB = new BackgroundWorker();
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            #region InstallServer
            if ((worker.CancellationPending == true))
            {
                e.Cancel = true;
            }
            else
            {
                if (cbxMssqlServer.Checked)
                {
                    worker.ReportProgress(10);
                    int ExitCode = 0;
                    if (rbtnSingleUser.Checked)
                    {
                        if (Environment.Is64BitOperatingSystem)
                        {
                            ExitCode = new CommandlineInstaller().Install64(txtPassword.Text, "SQLAccount");
                        }
                        else
                        {
                            ExitCode = new CommandlineInstaller().Install32(txtPassword.Text, "SQLAccount");
                        }
                    }
                    else
                    {
                        if (Environment.Is64BitOperatingSystem)
                        {
                            ExitCode = new CommandlineInstaller().Install64Multi(txtPassword.Text, "SQLAccountMULTI");
                        }
                        else
                        {
                            ExitCode = new CommandlineInstaller().Install32Multi(txtPassword.Text, "SQLAccountMULTI");
                        }
                    }
                    var message = string.Empty;
                    switch (ExitCode)
                    {
                        case 0:
                            message = "MSSQL Installation done.";
                            break;
                        case 1223:
                            message = "Installation canceled.";
                            break;
                        default:
                            worker.ReportProgress(50);
                            message = "MSSQL Installation failed. ExitCode : " + ExitCode;
                            break;
                    }
                    lblMessage.BeginInvoke(new Action(() => lblMessage.Text = message));
                    if (bw.WorkerSupportsCancellation == true && ExitCode != 0)
                    {
                        bw.CancelAsync();
                    }
                }
                worker.ReportProgress(70);
            }
            #endregion

            #region InstallSAP

            if ((worker.CancellationPending == true))
            {
                e.Cancel = true;
            }
            else
            {
                if (cbxSap.Checked)
                {
                    worker.ReportProgress(71);
                    Process install = new Process();
                    install.StartInfo.FileName = "msiexec.exe";
                    install.StartInfo.Arguments = string.Format("/package \"{0}\" /quiet INSTALLDIR=\"{1}\"", Application.StartupPath + "\\CRRuntime_32bit_13_0_1.msi", Environment.CurrentDirectory + "\\CristalReport");
                    try
                    {
                        install.StartInfo.Verb = "runas";
                        install.Start();
                        install.WaitForExit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        if (bw.WorkerSupportsCancellation == true)
                        {
                            bw.CancelAsync();
                        }
                    }
                }
                worker.ReportProgress(99);
                Thread.Sleep(10000);
            }
            #endregion
        }
        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                this.pbxEvent.Value = e.ProgressPercentage;
                if (e.ProgressPercentage == 10)
                {
                    lblMessage.Text = "INSTALLING MSSQL SERVER 2008 EXPRESS.";
                    lblEvent.Text = "Installing MSSql Server.";
                    lblProcess.Text = "Installing..";
                }
                if (e.ProgressPercentage == 71)
                {
                    lblMessage.Text = "INSTALLING SAP CRISTAL REPORT.";
                    lblEvent.Text = "Installing SAP Cristal Report.";
                    lblProcess.Text = "Installing..";
                }
                if (e.ProgressPercentage == 99)
                {
                    lblMessage.Text = (rbtnMultyUser.Checked) ? "Make sure any firewall is not blocking the access of the systems." : "Configure Oneaccount with MSSQL Server.";
                    lblEvent.Text = "Oneaccount configuration";
                    lblProcess.Text = "Configuring..";
                }
                if (e.ProgressPercentage == 50)
                {
                     SQLErrorLog errorlog = new SQLErrorLog();
                            errorlog.Show();
                }
            }
            catch (Exception)
            {

            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                pbxEvent.Value = 0;
                lblEvent.Text = "Program";
                lblProcess.Text = "Process";
                panel1.Enabled = true;
                pbxProcess.Style = ProgressBarStyle.Blocks;
            }
            else
            {
                lblEvent.Text = "Program";
                lblProcess.Text = "Process";
                lblMessage.Text = string.Empty;
                panel1.Enabled = true;
                pbxEvent.Value = 0;
                pbxProcess.Style = ProgressBarStyle.Blocks;
                if (bwDB.IsBusy != true)
                {
                    bwDB.RunWorkerAsync();
                }
                panel1.Visible = false;
                panel2.Visible = true;
                panel3.Visible = false;
            }
           
        }
        private void ConfigureDataBase(string serverName, string userId, string password, string ApplicationPath)
        {
            try
            {
                SClass ClassS = new SClass();
                if (ClassS.CheckMsSqlConnection(serverName, userId, password, ApplicationPath))
                {
                   
                    ClassS.UpdateAppConfig("MsSqlServer", serverName);
                    ClassS.UpdateAppConfig("MsSqlUserId", userId);
                    ClassS.UpdateAppConfig("MsSqlPassword", password);
                    ClassS.UpdateAppConfig("ApplicationPath", ApplicationPath);

                    serverName = (ConfigurationManager.AppSettings["MsSqlServer"] == null || ConfigurationManager.AppSettings["MsSqlServer"].ToString() == string.Empty) ? null : ConfigurationManager.AppSettings["MsSqlServer"].ToString();
                    userId = (ConfigurationManager.AppSettings["MsSqlUserId"] == null || ConfigurationManager.AppSettings["MsSqlUserId"].ToString() == string.Empty) ? null : ConfigurationManager.AppSettings["MsSqlUserId"].ToString();
                    password = (ConfigurationManager.AppSettings["MsSqlPassword"] == null || ConfigurationManager.AppSettings["MsSqlPassword"].ToString() == string.Empty) ? null : ConfigurationManager.AppSettings["MsSqlPassword"].ToString();
                    ApplicationPath = (ConfigurationManager.AppSettings["ApplicationPath"] == null || ConfigurationManager.AppSettings["ApplicationPath"].ToString() == string.Empty) ? null : ConfigurationManager.AppSettings["ApplicationPath"].ToString();
                    if (ClassS.CheckMsSqlConnection(serverName, userId, password, ApplicationPath))
                    {
                        Environment.ExitCode = 100;
                        this.Close();
                    }
                    else
                    {
                        Environment.ExitCode = 101;
                        this.Close();
                    }
                }
            }
            catch (Exception)
            {

            }
        }


        private void linkLabel1_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = true;
            panel3.Visible = false;
        }

        private void linkLabel2_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;
            panel3.Visible = false;
        }



        private void btnInstall_Click(object sender, EventArgs e)
        {
            if (cbxMssqlServer.Checked)
            {
                if (txtPassword.Text == txtconfpassword.Text)
                {
                    if (CheckStrength(txtPassword.Text))
                    {
                        panel1.Enabled = false;
                        pbxProcess.Style = ProgressBarStyle.Marquee;
                        if (bw.IsBusy != true)
                        {
                            bw.RunWorkerAsync();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Password contains following items. Minimum 8 characters in length, Uppercase Letters , Lowercase Letters , Numbers , Symbols","Oneaccount");
                    }
                }
                else
                {
                    MessageBox.Show("Passwords are mismatch ! Please re-enter.","Oneaccount");
                }
               
            }
            else
            {
                panel1.Enabled = false;
                pbxProcess.Style = ProgressBarStyle.Marquee;
                if (bw.IsBusy != true)
                {
                    bw.RunWorkerAsync();
                }
            }
           
        }
        public bool CheckStrength(string password)
        {
            if (password.Length < 8)
                return false;
            if (Regex.IsMatch(password, @"[0-9]+(\.[0-9][0-9]?)?", RegexOptions.ECMAScript))
                if (Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z]).+$", RegexOptions.ECMAScript))
                    if (Regex.IsMatch(password, @"[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]", RegexOptions.ECMAScript))
                        return true;
                    else return false;
                else return false;
            else return false;
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = true;
        }


        DataTable objTbl;

        private void cmbServers1_SelectedValueChanged(object sender, EventArgs e)
        {
            cmbInstance1.Text = string.Empty;
            cmbPath.DataSource = null;
            if (objTbl != null)
            {
                List<string> lstResult = (from table in objTbl.AsEnumerable()
                                          where table.Field<string>("Server").ToString() == (cmbServers1.Text).ToString()
                                          select table.Field<string>("Instance")).Distinct().ToList();

                cmbInstance1.DataSource = lstResult;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                label10.Enabled = true;
                label11.Enabled = true;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
            }
            else
            {
                label10.Enabled = false;
                label11.Enabled = false;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
            }
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
        }

        private void cmbServers1_BindingContextChanged(object sender, EventArgs e)
        {
            if (objTbl != null)
            {
                List<string> lstResult = (from table in objTbl.AsEnumerable()
                                          where table.Field<string>("Server").ToString() == (cmbServers1.Text).ToString()
                                          select table.Field<string>("Instance")).Distinct().ToList();

                cmbInstance1.DataSource = lstResult;
            }
        }
        private void btnOkServer_Click(object sender, EventArgs e)
        {

            string server = cmbServers1.Text + @"\" + cmbInstance1.Text;
            string userId = textBox1.Text;
            string password = textBox2.Text;
            progressBar1.Visible = true;
            panel2.Enabled = false;
            Thread backgroundThread = new Thread(
                new ThreadStart(() =>
                {
                    SClass classS = new SClass();

                    if (!radioButton3.Checked)
                    {
                        cmbPath.DataSource = classS.GetOmPath(server, null, null);
                    }
                    else
                    {
                        cmbPath.DataSource = classS.GetOmPath(server, userId, password);
                    }
                    if (cmbPath.InvokeRequired)
                    {
                        cmbPath.BeginInvoke(new Action(() => cmbPath.DisplayMember = "location"));
                    }
                    try
                    {
                        progressBar1.BeginInvoke(new Action(() => progressBar1.Visible = false));
                        panel2.BeginInvoke(new Action(() => panel2.Enabled = true));

                    }
                    catch (Exception)
                    { }

                }
            ));
            backgroundThread.Start();
        }


        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (bwDB.IsBusy != true)
            {
                bwDB.RunWorkerAsync();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (cmbPath.Text != string.Empty)
            {
                if (!radioButton3.Checked)
                {
                    ConfigureDataBase(cmbServers1.Text + @"\" + cmbInstance1.Text, null, null, cmbPath.Text);
                }
                else
                {
                    ConfigureDataBase(cmbServers1.Text + @"\" + cmbInstance1.Text, textBox1.Text, textBox2.Text, cmbPath.Text);
                }
            }
            else
            {
                cmbPath.Focus();
                MessageBox.Show("Oneaccount connection path is empty.! \nTry to connect server with credentials.");
            }

        }

        private void cmbInstance1_SelectedValueChanged(object sender, EventArgs e)
        {
            cmbPath.DataSource = null;
        }

        private void rbtnLocalServer_CheckedChanged(object sender, EventArgs e)
        {
            linkLabel4_LinkClicked(null, null);
        }

        private void cbxMssqlServer_CheckedChanged(object sender, EventArgs e)
        {

                pnlCredencial.Enabled = (cbxMssqlServer.Checked)?true:false;
            
        }
    }
}
