using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace One_Account
{
   abstract class MSSQLInstalling
    {
        public void CreateMsSQLConfigurationFile64Single(string fileName, string instanceName)
        {
            using (var configFile = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                StreamWriter writer = new StreamWriter(configFile);
                writer.WriteLine("[SQLSERVER2008]");
                writer.WriteLine("INSTANCEID=\"" + instanceName + "\"");
                writer.WriteLine("ACTION=\"Install\"");
                writer.WriteLine("FEATURES=SQLENGINE,REPLICATION");
                writer.WriteLine("HELP=\"False\"");
                writer.WriteLine("INDICATEPROGRESS=\"False\"");
                writer.WriteLine("QUIET=\"True\"");
                writer.WriteLine("QUIETSIMPLE=\"False\"");
                writer.WriteLine("X86=\"False\"");
                writer.WriteLine("ERRORREPORTING=\"False\"");
                writer.WriteLine("INSTALLSHAREDDIR=\"" + Application.StartupPath + "\"");
                writer.WriteLine("INSTALLSHAREDWOWDIR=\"" + Application.StartupPath + "\"");
                writer.WriteLine("INSTANCEDIR=\"" + Application.StartupPath + "\"");
                writer.WriteLine("SQMREPORTING=\"False\"");
                writer.WriteLine("INSTANCENAME=\"" + instanceName + "\"");
                writer.WriteLine("AGTSVCACCOUNT=\"Manual\"");
                writer.WriteLine("ISSVCSTARTUPTYPE=\"Automatic\"");
                writer.WriteLine("ISSVCACCOUNT=\"NT AUTHORITY\\NetworkService\"");
                writer.WriteLine("ASSVCSTARTUPTYPE=\"Automatic\"");
                writer.WriteLine("ASCOLLATION=\"Latin1_General_CI_AS\"");
                writer.WriteLine("ASDATADIR=\"Data\"");
                writer.WriteLine("ASLOGDIR=\"Log\"");
                writer.WriteLine("ASBACKUPDIR=\"Backup\"");
                writer.WriteLine("ASTEMPDIR=\"Temp\"");
                writer.WriteLine("ASCONFIGDIR=\"Config\"");
                writer.WriteLine("ASPROVIDERMSOLAP=\"1\"");
                writer.WriteLine("SQLSVCSTARTUPTYPE=\"Automatic\"");
                writer.WriteLine("FILESTREAMLEVEL=\"0\"");
                writer.WriteLine("ENABLERANU=\"True\"");
                writer.WriteLine("SQLCOLLATION=\"SQL_Latin1_General_CP1_CI_AS\"");
                writer.WriteLine("SQLSVCACCOUNT=\"NT AUTHORITY\\NETWORK SERVICE\"");
                writer.WriteLine("SQLSYSADMINACCOUNTS=\"" + Environment.MachineName + "\\" + Environment.UserName + "\"");
                writer.WriteLine("SECURITYMODE=\"SQL\"");
                writer.WriteLine("ADDCURRENTUSERASSQLADMIN=\"False\"");
                writer.WriteLine("TCPENABLED=\"0\"");
                writer.WriteLine("NPENABLED=\"0\"");
                writer.WriteLine("BROWSERSVCSTARTUPTYPE=\"Automatic\"");
                writer.WriteLine("RSSVCSTARTUPTYPE=\"Automatic\"");
                writer.WriteLine("RSINSTALLMODE=\"FilesOnlyMode\"");

                writer.Flush();
            }
        }
        public void CreateMsSQLConfigurationFile32Single(string fileName, string instanceName)
        {
            using (var configFile = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                StreamWriter writer = new StreamWriter(configFile);
                writer.WriteLine("[SQLSERVER2008]");
                writer.WriteLine("INSTANCEID=\"" + instanceName + "\"");
                writer.WriteLine("ACTION=\"Install\"");
                writer.WriteLine("FEATURES=SQLENGINE,REPLICATION");
                writer.WriteLine("HELP=\"False\"");
                writer.WriteLine("INDICATEPROGRESS=\"False\"");
                writer.WriteLine("QUIET=\"True\"");
                writer.WriteLine("QUIETSIMPLE=\"False\"");
                writer.WriteLine("X86=\"False\"");
                writer.WriteLine("ERRORREPORTING=\"False\"");
                writer.WriteLine("INSTALLSHAREDDIR=\"" + Application.StartupPath + "\"");
                writer.WriteLine("INSTANCEDIR=\"" + Application.StartupPath + "\"");
                writer.WriteLine("SQMREPORTING=\"False\"");
                writer.WriteLine("INSTANCENAME=\"" + instanceName + "\"");
                writer.WriteLine("AGTSVCACCOUNT=\"Manual\"");
                writer.WriteLine("ISSVCSTARTUPTYPE=\"Automatic\"");
                writer.WriteLine("ISSVCACCOUNT=\"NT AUTHORITY\\NetworkService\"");
                writer.WriteLine("ASSVCSTARTUPTYPE=\"Automatic\"");
                writer.WriteLine("ASCOLLATION=\"Latin1_General_CI_AS\"");
                writer.WriteLine("ASDATADIR=\"Data\"");
                writer.WriteLine("ASLOGDIR=\"Log\"");
                writer.WriteLine("ASBACKUPDIR=\"Backup\"");
                writer.WriteLine("ASTEMPDIR=\"Temp\"");
                writer.WriteLine("ASCONFIGDIR=\"Config\"");
                writer.WriteLine("ASPROVIDERMSOLAP=\"1\"");
                writer.WriteLine("SQLSVCSTARTUPTYPE=\"Automatic\"");
                writer.WriteLine("FILESTREAMLEVEL=\"0\"");
                writer.WriteLine("ENABLERANU=\"True\"");
                writer.WriteLine("SQLCOLLATION=\"SQL_Latin1_General_CP1_CI_AS\"");
                writer.WriteLine("SQLSVCACCOUNT=\"NT AUTHORITY\\NETWORK SERVICE\"");
                writer.WriteLine("SQLSYSADMINACCOUNTS=\"" + Environment.MachineName + "\\" + Environment.UserName + "\"");
                writer.WriteLine("SECURITYMODE=\"SQL\"");
                writer.WriteLine("ADDCURRENTUSERASSQLADMIN=\"False\"");
                writer.WriteLine("TCPENABLED=\"0\"");
                writer.WriteLine("NPENABLED=\"0\"");
                writer.WriteLine("BROWSERSVCSTARTUPTYPE=\"Automatic\"");
                writer.WriteLine("RSSVCSTARTUPTYPE=\"Automatic\"");
                writer.WriteLine("RSINSTALLMODE=\"FilesOnlyMode\"");
                writer.Flush();
            }
        }
        public void CreateMsSQLConfigurationFile32Multi(string fileName, string instanceName)
        {
            using (var configFile = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                StreamWriter writer = new StreamWriter(configFile);
                writer.WriteLine("[SQLSERVER2008]");
                writer.WriteLine("INSTANCEID=\"" + instanceName + "\"");
                writer.WriteLine("ACTION=\"Install\"");
                writer.WriteLine("FEATURES=SQLENGINE,REPLICATION,SNAC_SDK");
                writer.WriteLine("HELP=\"False\"");
                writer.WriteLine("INDICATEPROGRESS=\"False\"");
                writer.WriteLine("QUIET=\"True\"");
                writer.WriteLine("QUIETSIMPLE=\"False\"");
                writer.WriteLine("X86=\"False\"");
                writer.WriteLine("ERRORREPORTING=\"False\"");
                writer.WriteLine("INSTALLSHAREDDIR=\"" + Application.StartupPath + "\"");
                writer.WriteLine("INSTANCEDIR=\"" + Application.StartupPath + "\"");
                writer.WriteLine("SQMREPORTING=\"False\"");
                writer.WriteLine("INSTANCENAME=\"" + instanceName + "\"");
                writer.WriteLine("AGTSVCACCOUNT=\"Manual\"");
                writer.WriteLine("ISSVCSTARTUPTYPE=\"Automatic\"");
                writer.WriteLine("ISSVCACCOUNT=\"NT AUTHORITY\\NetworkService\"");
                writer.WriteLine("ASSVCSTARTUPTYPE=\"Automatic\"");
                writer.WriteLine("ASCOLLATION=\"Latin1_General_CI_AS\"");
                writer.WriteLine("ASDATADIR=\"Data\"");
                writer.WriteLine("ASLOGDIR=\"Log\"");
                writer.WriteLine("ASBACKUPDIR=\"Backup\"");
                writer.WriteLine("ASTEMPDIR=\"Temp\"");
                writer.WriteLine("ASCONFIGDIR=\"Config\"");
                writer.WriteLine("ASPROVIDERMSOLAP=\"1\"");
                writer.WriteLine("SQLSVCSTARTUPTYPE=\"Automatic\"");
                writer.WriteLine("FILESTREAMLEVEL=\"0\"");
                writer.WriteLine("ENABLERANU=\"True\"");
                writer.WriteLine("SQLCOLLATION=\"SQL_Latin1_General_CP1_CI_AS\"");
                writer.WriteLine("SQLSVCACCOUNT=\"NT AUTHORITY\\NETWORK SERVICE\"");
                writer.WriteLine("SQLSYSADMINACCOUNTS=\"" + Environment.MachineName + "\\" + Environment.UserName + "\"");
                writer.WriteLine("SECURITYMODE=\"SQL\"");
                writer.WriteLine("ADDCURRENTUSERASSQLADMIN=\"False\"");
                writer.WriteLine("TCPENABLED=\"1\"");
                writer.WriteLine("NPENABLED=\"1\"");
                writer.WriteLine("BROWSERSVCSTARTUPTYPE=\"Automatic\"");
                writer.WriteLine("RSSVCSTARTUPTYPE=\"Automatic\"");
                writer.WriteLine("RSINSTALLMODE=\"FilesOnlyMode\"");
                writer.Flush();
            }
        }
        public void CreateMsSQLConfigurationFile64Multi(string fileName, string instanceName)
        {
            using (var configFile = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                StreamWriter writer = new StreamWriter(configFile);
                writer.WriteLine("[SQLSERVER2008]");
                writer.WriteLine("INSTANCEID=\"" + instanceName + "\"");
                writer.WriteLine("ACTION=\"Install\"");
                writer.WriteLine("FEATURES=SQLENGINE,REPLICATION,SNAC_SDK");
                writer.WriteLine("HELP=\"False\"");
                writer.WriteLine("INDICATEPROGRESS=\"False\"");
                writer.WriteLine("QUIET=\"True\"");
                writer.WriteLine("QUIETSIMPLE=\"False\"");
                writer.WriteLine("X86=\"False\"");
                writer.WriteLine("ERRORREPORTING=\"False\"");
                writer.WriteLine("INSTALLSHAREDDIR=\"" + Application.StartupPath + "\"");
                writer.WriteLine("INSTALLSHAREDWOWDIR=\"" + Application.StartupPath + "\"");
                writer.WriteLine("INSTANCEDIR=\"" + Application.StartupPath + "\"");
                writer.WriteLine("SQMREPORTING=\"False\"");
                writer.WriteLine("INSTANCENAME=\"" + instanceName + "\"");
                writer.WriteLine("AGTSVCACCOUNT=\"Manual\"");
                writer.WriteLine("ISSVCSTARTUPTYPE=\"Automatic\"");
                writer.WriteLine("ISSVCACCOUNT=\"NT AUTHORITY\\NetworkService\"");
                writer.WriteLine("ASSVCSTARTUPTYPE=\"Automatic\"");
                writer.WriteLine("ASCOLLATION=\"Latin1_General_CI_AS\"");
                writer.WriteLine("ASDATADIR=\"Data\"");
                writer.WriteLine("ASLOGDIR=\"Log\"");
                writer.WriteLine("ASBACKUPDIR=\"Backup\"");
                writer.WriteLine("ASTEMPDIR=\"Temp\"");
                writer.WriteLine("ASCONFIGDIR=\"Config\"");
                writer.WriteLine("ASPROVIDERMSOLAP=\"1\"");
                writer.WriteLine("SQLSVCSTARTUPTYPE=\"Automatic\"");
                writer.WriteLine("FILESTREAMLEVEL=\"0\"");
                writer.WriteLine("ENABLERANU=\"True\"");
                writer.WriteLine("SQLCOLLATION=\"SQL_Latin1_General_CP1_CI_AS\"");
                writer.WriteLine("SQLSVCACCOUNT=\"NT AUTHORITY\\NETWORK SERVICE\"");
                writer.WriteLine("SQLSYSADMINACCOUNTS=\"" + Environment.MachineName + "\\" + Environment.UserName + "\"");
                writer.WriteLine("SECURITYMODE=\"SQL\"");
                writer.WriteLine("ADDCURRENTUSERASSQLADMIN=\"False\"");
                writer.WriteLine("TCPENABLED=\"1\"");
                writer.WriteLine("NPENABLED=\"1\"");
                writer.WriteLine("BROWSERSVCSTARTUPTYPE=\"Automatic\"");
                writer.WriteLine("RSSVCSTARTUPTYPE=\"Automatic\"");
                writer.WriteLine("RSINSTALLMODE=\"FilesOnlyMode\"");

                writer.Flush();
            }
        }
    }
}
