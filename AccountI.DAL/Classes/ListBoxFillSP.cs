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
using System.Data;
using Entity;

namespace AccountI.DAL
{
  public class ListBoxFillSP : DBConnection
    {
      public List<DataTable> GetList(CategoryInfo infoCategory)
        {
            DataTable dtbl = new DataTable();
            List<DataTable> dataTableList = new List<DataTable>();
            SqlDataAdapter sqlDa = new SqlDataAdapter("AccountI_ListBoxFill", sqlCon);
            sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDa.SelectCommand.Parameters.Add("@module", SqlDbType.VarChar).Value = (infoCategory.ModuleName != string.Empty) ? infoCategory.ModuleName : "Overall Statistics";
            sqlDa.SelectCommand.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = infoCategory.DateFrom;
            sqlDa.SelectCommand.Parameters.Add("@toDate", SqlDbType.DateTime).Value = infoCategory.DateTo;
            sqlDa.SelectCommand.Parameters.Add("@type", SqlDbType.VarChar).Value = infoCategory.TypeName;
            sqlDa.SelectCommand.Parameters.Add("@category", SqlDbType.VarChar).Value = (infoCategory.CatagoryName != string.Empty) ? infoCategory.CatagoryName : "Overall Statistics";
            sqlDa.SelectCommand.Parameters.Add("@subCategory", SqlDbType.VarChar).Value = (infoCategory.SubCatagoryName != string.Empty) ? infoCategory.SubCatagoryName : "Overall Statistics";
            sqlDa.Fill(dtbl);
            dataTableList.Add(dtbl);
            return dataTableList;
        }
    }
}
