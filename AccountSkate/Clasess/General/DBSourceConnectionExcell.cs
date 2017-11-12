using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace AccountSkate
{
    class DBSourceConnectionExcell:CommonSource
    {
        protected System.Data.OleDb.OleDbConnection oledbcon;
        public DBSourceConnectionExcell()
        {
            var ofd = new Microsoft.Win32.OpenFileDialog() { Filter = "Excel Files (*.xls)|*.xls" };
            var result = ofd.ShowDialog();
            if (result == false) return;
            oledbcon = new System.Data.OleDb.OleDbConnection(@"provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + ofd.FileName + "';Extended Properties=Excel 8.0;");
        }
    }
}
