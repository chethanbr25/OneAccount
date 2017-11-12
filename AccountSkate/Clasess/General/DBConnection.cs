using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Xml;
using System.Windows.Forms;
using System.IO;

namespace AccountSkate
{
    class DBConnection
    {
        protected SqlConnection sqlcon;
        public DBConnection()
        {
            try
            {
                if (GeneralInfo.strcon == null)
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load("Connection.xml");
                    GeneralInfo.strcon = xDoc.DocumentElement.SelectSingleNode("conection").InnerText;
                    sqlcon = new SqlConnection(GeneralInfo.strcon);
                    xDoc.DocumentElement.RemoveChild(xDoc.DocumentElement.ChildNodes[0]);
                    FileStream WRITER = new FileStream("Connection.xml", FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite);
                    xDoc.Save(WRITER);
                    WRITER.Close();

                }
                else
                {
                    sqlcon = new SqlConnection(GeneralInfo.strcon);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
           

        }
    }
   public class GeneralInfo
    {
        public static string strcon { get; set; }
    }
}
