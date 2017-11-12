//This is a source code or part of OneAccount project
//Copyright (C) 2013 OneAccount
//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.
//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//GNU General Public License for more details.
//You should have received a copy of the GNU General Public License
//along with this program. If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Microsoft.Win32;
using Entity;
using System.IO;
using System.Xml;

namespace AccountI.DAL
{
    public class DBConnection
    {
        protected SqlConnection sqlCon;
        public DBConnection()
        {
            try
            {
                if (GeneralInfo.strCon == null)
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load("Connection.xml");
                    GeneralInfo.strCon = xDoc.DocumentElement.SelectSingleNode("conection").InnerText;
                    sqlCon = new SqlConnection(GeneralInfo.strCon);
                    if (sqlCon.State == System.Data.ConnectionState.Closed)
                        sqlCon.Open();
                    ConnectionString.IsConnectionTrue = true;
                    xDoc.DocumentElement.RemoveChild(xDoc.DocumentElement.ChildNodes[0]);
                    FileStream WRITER = new FileStream("Connection.xml", FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite);
                    xDoc.Save(WRITER);
                    WRITER.Close();

                }
                else
                {
                    sqlCon = new SqlConnection(GeneralInfo.strCon);
                    if (sqlCon.State == System.Data.ConnectionState.Closed)
                        sqlCon.Open();
                    ConnectionString.IsConnectionTrue = true;
                }
            }
            catch (Exception)
            { }

        }
        public class GeneralInfo
        {
            public static string strCon { get; set; }
        }
    }
}
