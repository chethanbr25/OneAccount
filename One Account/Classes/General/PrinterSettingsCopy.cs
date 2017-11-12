﻿//This is a source code or part of Oneaccount project
//Copyright (C) 2013  Oneaccount
//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.
//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.
//You should have received a copy of the GNU General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
namespace One_Account
{
    class PrinterSettingsCopy
    {
        protected SqlConnection sqlconSource;
        protected SqlConnection sqlconDestination;
        /// <summary>
        /// Function for copying printer settings data
        /// </summary>
        /// <param name="strSourceDBPath"></param>
        /// <param name="strDestinationDBPath"></param>
        /// <returns></returns>
        public string CopyData(string strSourceDBPath, string strDestinationDBPath)
        {
            string strFailed = string.Empty;
            try
            {

                string strServer = (ConfigurationManager.AppSettings["MsSqlServer"] == null || ConfigurationManager.AppSettings["MsSqlServer"].ToString() == null) ? null : ConfigurationManager.AppSettings["MsSqlServer"].ToString();
                sqlconSource = new SqlConnection(@"Data Source=" + strServer + ";AttachDbFilename=" + strSourceDBPath + ";Integrated Security=True;Connect Timeout=30;User Instance=True");
                sqlconDestination = new SqlConnection(@"Data Source=" + strServer + ";AttachDbFilename=" + strDestinationDBPath + ";Integrated Security=True;Connect Timeout=30;User Instance=True");
                sqlconSource.Open();
                sqlconDestination.Open();
                DataTable dtblTables = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter("select table_name as name from information_schema.tables where TABLE_TYPE='BASE TABLE'", sqlconSource);
                adp.Fill(dtblTables);
                SqlBulkCopy sbc = new SqlBulkCopy(@"Data Source=" + strServer + ";AttachDbFilename=" + strDestinationDBPath + ";Integrated Security=True;Connect Timeout=30;User Instance=True", SqlBulkCopyOptions.KeepIdentity);
                for (int i = 0; i < dtblTables.Rows.Count; i++)
                {
                    frmCopyData.strTable = dtblTables.Rows[i][0].ToString();
                    if (frmCopyData.strTable == "tbl_FormCopy" || frmCopyData.strTable == "tbl_MasterCopy" || frmCopyData.strTable == "tbl_FieldsCopy" || frmCopyData.strTable == "tbl_DetailsCopy" || frmCopyData.strTable == "tbl_Form" || frmCopyData.strTable == "tbl_Master" || frmCopyData.strTable == "tbl_Fields" || frmCopyData.strTable == "tbl_Details")
                    {
                        SqlCommand sccmdTrucate = new SqlCommand("TRUNCATE TABLE " + frmCopyData.strTable, sqlconDestination);
                        try
                        {
                            sccmdTrucate.ExecuteNonQuery();
                        }
                        catch { }
                        SqlCommand sccmd = new SqlCommand("SELECT * FROM " + frmCopyData.strTable, sqlconSource);
                        SqlDataReader sdrreader = sccmd.ExecuteReader();
                        sbc.DestinationTableName = frmCopyData.strTable;
                        try
                        {
                            sbc.WriteToServer(sdrreader);
                        }
                        catch
                        {
                            strFailed = strFailed + frmCopyData.strTable + "\n";
                        }
                        sdrreader.Close();
                    }
                }
                sbc.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                sqlconSource.Close();
            }
            return strFailed;
        }
    }
}
