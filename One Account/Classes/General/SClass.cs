using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;
using Microsoft.Win32;
using System.Configuration;
using System.IO;

namespace One_Account
{
    class GetConnection : DBConnection
    {
        internal string GetCurrentConnctionString()
        {
            string ss = sqlcon.ConnectionString;
            if (ss.Contains("user id=\'"))
            {
                ss = ss  + ";password='" + password + "'";
            }
            return ss;
        }
    }
    class SClass
    {
        public bool CheckFilesExists(string FileNameWithFullPath)
        {
            if (File.Exists(FileNameWithFullPath))
            {
                return true;
            }
            return false;
        }
        internal bool CheckMsSqlConnection(string serverName, string userId, string password, string ApplicationPath)
        {

            bool isTrue = false;
            SqlConnection sqlcon;
            if (serverName != null)
            {
                if (userId == null || password == null)
                {
                    sqlcon = new SqlConnection(@"Data Source=" + serverName + ";AttachDbFilename=" + ApplicationPath + "\\Data\\DBOneaccount.mdf" + ";Integrated Security=True;Connect Timeout=30;User Instance=True");
                }
                else
                {
                    sqlcon = new SqlConnection(@"Data Source=" + serverName + ";AttachDbFilename=" + ApplicationPath + "\\Data\\DBOneaccount.mdf" + ";user id='" + userId + "';password='" + password + "'; Connect Timeout=30; User Instance=False");
                }
                try
                {
                    sqlcon.Open();
                    isTrue = true;
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 18493)
                    {
                        if (userId == null || password == null)
                        {
                            sqlcon = new SqlConnection(@"Data Source=" + serverName + ";AttachDbFilename=" + ApplicationPath + "\\Data\\DBOneaccount.mdf" + ";Integrated Security=True;Connect Timeout=30");
                        }
                        else
                        {
                            sqlcon = new SqlConnection(@"Data Source=" + serverName + ";AttachDbFilename=" + ApplicationPath + "\\Data\\DBOneaccount.mdf" + ";user id='" + userId + "';password='" + password + "'; Connect Timeout=30");
                        }
                        try
                        {
                            sqlcon.Open();
                            UpdateAppConfig("isServerConnection", "true");
                            isTrue = true;
                        }
                        catch (SqlException exa)
                        {

                            MessageBox.Show(exa.Message);
                            isTrue = false;

                        }
                        finally
                        {
                            if (sqlcon.State == System.Data.ConnectionState.Open)
                            {
                                sqlcon.Close();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(ex.Message);
                        isTrue = false;
                    }
                }
                finally
                {
                    if (sqlcon.State == System.Data.ConnectionState.Open)
                    {
                        sqlcon.Close();
                    }
                }
            }
            return isTrue;

        }
        public DataTable GetLocalInstance()
        {
            DataTable LocalInstanceNames = new DataTable();
            LocalInstanceNames.Columns.Add("Server", typeof(string));
            LocalInstanceNames.Columns.Add("Instance", typeof(string));
            RegistryView registryView = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;
            using (RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView))
            {
                RegistryKey instanceKey = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL", false);
                if (instanceKey != null)
                {
                    foreach (string instanceName in instanceKey.GetValueNames())
                    {
                        LocalInstanceNames.Rows.Add(Environment.MachineName, instanceName);
                    }
                }
                else
                {
                    LocalInstanceNames.Rows.Add(Environment.MachineName, "");
                }
            }
            return LocalInstanceNames;
        }

        public void UpdateAppConfig(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch { }

        }

        internal DataTable GetOmPath(string serverName, string userId, string password)
        {
            SqlConnection sqlcon;
            DataTable dtbl;
            if (serverName != null)
            {
                if (userId == null || password == null)
                {
                    sqlcon = new SqlConnection(@"Data Source=" + serverName + ";Integrated Security=True;Connect Timeout=30;User Instance=True");
                }
                else
                {
                    sqlcon = new SqlConnection(@"Data Source=" + serverName + ";user id='" + userId + "';password='" + password + "'; Connect Timeout=30; User Instance=False");
                }
                try
                {
                    sqlcon.Open();
                    SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT name, cast(replace(physical_name, '\\Data\\DBOneaccount.mdf', '') as varchar(max)) AS [location] FROM sys.master_files WHERE physical_name like '%'+'\\Data\\DBOneaccount.mdf'", sqlcon);
                    sqlDa.Fill(dtbl = new DataTable());
                    if (dtbl.Rows.Count == 0 && (serverName.Split('\\'))[0].ToString() == Environment.MachineName)
                    {
                        dtbl.Rows.Add("MyPath", Application.StartupPath);
                    }
                    return dtbl;
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 18493)
                    {
                        if (userId == null || password == null)
                        {
                            sqlcon = new SqlConnection(@"Data Source=" + serverName + ";Integrated Security=True;Connect Timeout=30");
                        }
                        else
                        {
                            sqlcon = new SqlConnection(@"Data Source=" + serverName + ";user id='" + userId + "';password='" + password + "'; Connect Timeout=30");
                        }
                        try
                        {
                            sqlcon.Open();
                            SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT name, cast(replace(physical_name, '\\Data\\DBOneaccount.mdf', '') as varchar(max)) AS [location] FROM sys.master_files WHERE physical_name like '%'+'\\Data\\DBOneaccount.mdf'", sqlcon);
                            sqlDa.Fill(dtbl = new DataTable());
                            if (dtbl.Rows.Count == 0 && (serverName.Split('\\'))[0].ToString() == Environment.MachineName)
                            {
                                dtbl.Rows.Add("MyPath", Application.StartupPath);
                            }
                            return dtbl;
                        }
                        catch (SqlException exa)
                        {

                            MessageBox.Show(exa.Message);

                        }
                        finally
                        {
                            if (sqlcon.State == System.Data.ConnectionState.Open)
                            {
                                sqlcon.Close();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                finally
                {
                    if (sqlcon.State == System.Data.ConnectionState.Open)
                    {
                        sqlcon.Close();
                    }
                }

            }
            return dtbl = new DataTable();
        }
    }
}
