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
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
namespace One_Account
{
    class MasterSP:DBConnection
    {
        #region Function
        /// <summary>
        /// Function to get DotMatrxPrinter Format ComboFill For VoucherType
        /// </summary>
        /// <returns></returns>
        public DataTable DotMatrxPrinterFormatComboFillForVoucherType()
        {
            DataTable dtbl = new DataTable();
            try
            {
                SqlDataAdapter sqlda = new SqlDataAdapter("DotMatrxPrinterFormatComboFillForVoucherType", sqlcon);
                sqlda.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlda.Fill(dtbl);
            }
            catch (Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
            return dtbl;
        }
        #endregion
    }
}
