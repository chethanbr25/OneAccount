//This is a source code or part of OpenAccount project
//Copyright (C) 2013 OpenAccount
//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.
//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//GNU General Public License for more details.
//You should have received a copy of the GNU General Public License
//along with this program. If not, see <http://www.gnu.org/licenses/>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
using System.Collections;
using System.Data;

namespace AccountSkate
{
    class DBSourceConnectionTally:CommonSource
    {
        #region Variables
        protected OdbcConnection odbcCon;
        public static string strODBCConnectionString;
        #endregion


        #region Function
        /// <summary>
        /// Function For Database Source Connection
        /// </summary>
        public DBSourceConnectionTally()
        {
            try
            {
                if (strODBCConnectionString == null || functionForCheckTallyinTaskbar())
                {
                    GetUserDataSourceNames();
                }
                else
                {
                    odbcCon = new OdbcConnection(strODBCConnectionString);
                }
            }
            catch (Exception)
            {}

        }
        /// <summary>
        /// Function To List All DSN in Sorted List
        /// </summary>
        /// <returns></returns>

        public static SortedList listAllDSN()
        {
            SortedList allDSN = new SortedList();
            RegistryKey reg = (Registry.CurrentUser).OpenSubKey("Software");
            reg = reg.OpenSubKey("ODBC");
            reg = reg.OpenSubKey("ODBC.INI");
            reg = reg.OpenSubKey("ODBC Data Sources");

            if (reg != null)
            {
                foreach (string s in reg.GetValueNames())
                {
                    allDSN.Add(s, null);
                }
            }
            try
            {
                reg.Close();
            }
            catch (System.Exception ex)
            {
            }
            reg = (Registry.LocalMachine).OpenSubKey("Software");
            reg = reg.OpenSubKey("ODBC");
            reg = reg.OpenSubKey("ODBC.INI");
            reg = reg.OpenSubKey("ODBC Data Sources");

            if (reg != null)
            {
                foreach (string s in reg.GetValueNames())
                {
                    try
                    {
                        allDSN.Add(s, null);
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
            }
            try
            {
                reg.Close();
            }
            catch (System.Exception)
            {
            }

            return allDSN;
        }
        /// <summary>
        /// Function To Get User Data Source Names
        /// </summary>
        public bool GetUserDataSourceNames()
        {
            bool isTrue = false;
            foreach (string GetKey in listAllDSN().GetKeyList())
            {
                if (GetKey.Contains("TallyODBC"))
                {
                    try
                    {
                        odbcCon = new OdbcConnection("Dsn=" + GetKey + "");

                        if (odbcCon.State == System.Data.ConnectionState.Closed)
                        {
                            odbcCon.Open();
                        }
                        OdbcDataAdapter odbcCom = new OdbcDataAdapter("SELECT `$PriceLevel` FROM PriceLevels", odbcCon);
                        if (odbcCom != null)
                        {
                            odbcCom.Fill(new DataTable());
                            odbcCon.Close();
                            strODBCConnectionString = odbcCon.ConnectionString;
                            isTrue = true;
                            break;
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }

            return isTrue;
        }
        /// <summary>
        /// Function To Check Tally in Taskbar
        /// </summary>
        /// <returns></returns>
        private bool functionForCheckTallyinTaskbar()
        {
            bool isTrue = false;
            try
            {
                Process[] processes = Process.GetProcesses();
                foreach (var proc in processes)
                {
                    if (!string.IsNullOrEmpty(proc.MainWindowTitle))
                    {
                        if (proc.MainWindowTitle.Contains("Tally"))
                        {
                            isTrue = true;
                            break;
                        }
                    }
                }

            }
            catch (Exception)
            {            }
            return isTrue;
        }
        #endregion
    }
}
