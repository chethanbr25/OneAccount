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
using System.Data;
using System.Data.SqlClient;
using Entity;

namespace AccountI.DAL
{
    public class ViewDetailsFillSP:DBConnection
    {
        public List<DataTable> GetViewDetailsData()
        {
            string strStoredProcedure = null;
            if (ViewDetailsInfo.ModuleName == "Finance" || ViewDetailsInfo.ModuleName == null || ViewDetailsInfo.ModuleName == "Overall Statistics")
            {
                if ((ViewDetailsInfo.Catagory == "Overall Statistics") || (ViewDetailsInfo.Catagory == null))
                {
                    strStoredProcedure = ("AccountI_ProfitAndLoss_View_Details");
                }
                else
                {
                    strStoredProcedure = "AccountI_" + ViewDetailsInfo.Catagory.Trim() + "_View_Details";
                }
            }
            else
            {
                strStoredProcedure = "AccountI_" + ViewDetailsInfo.ModuleName.Trim() + "_View_Details";
            }
            List<char> resutl = strStoredProcedure.ToList();
            resutl.RemoveAll(c => c == ' ');
            strStoredProcedure = new string(resutl.ToArray());
            List<DataTable> lstDataTable = new List<DataTable>();
            DataTable dtbl = new DataTable();
            SqlDataAdapter sqlDa = new SqlDataAdapter(strStoredProcedure, sqlCon);
            sqlDa.SelectCommand.CommandTimeout = 300;
            sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDa.SelectCommand.Parameters.Add("@type", SqlDbType.VarChar).Value = (ViewDetailsInfo.Type != string.Empty && ViewDetailsInfo.Type != null) ? ViewDetailsInfo.Type : "Month";
            sqlDa.SelectCommand.Parameters.Add("@onDate", SqlDbType.DateTime).Value = ViewDetailsInfo.OnDate;
            sqlDa.SelectCommand.Parameters.Add("@checkValue", SqlDbType.VarChar).Value = (ViewDetailsInfo.CheckValue != string.Empty && ViewDetailsInfo.CheckValue !=  null) ? ViewDetailsInfo.CheckValue : "Overall Statistics";
            sqlDa.SelectCommand.Parameters.Add("@subCategory", SqlDbType.VarChar).Value = (ViewDetailsInfo.SubCatagory != string.Empty && ViewDetailsInfo.SubCatagory != null) ? ViewDetailsInfo.SubCatagory : "Overall Statistics";
            sqlDa.SelectCommand.Parameters.Add("@Category", SqlDbType.VarChar).Value = (ViewDetailsInfo.Catagory != string.Empty && ViewDetailsInfo.Catagory != null) ? ViewDetailsInfo.Catagory : "Overall Statistics";
            if (ViewDetailsInfo.ModuleName == "Finance" && ViewDetailsInfo.Catagory == "Tax Details")
            {
                sqlDa.SelectCommand.Parameters.Add("@sortBy", SqlDbType.VarChar).Value =(ViewDetailsInfo.SortBy != null && ViewDetailsInfo.SortBy != string.Empty) ? ViewDetailsInfo.SortBy:"Amount";
            }
            else if (ViewDetailsInfo.ModuleName == "Finance" && ViewDetailsInfo.Catagory == "Budget")
            {
                sqlDa.SelectCommand.Parameters.Add("@budgetName", SqlDbType.VarChar).Value = (ViewDetailsInfo.SubCatagory != null && ViewDetailsInfo.SubCatagory != string.Empty) ? ViewDetailsInfo.SubCatagory : "Overall Statistics";
            }


            dtbl.Columns.Add("Sl No", typeof(int));
            dtbl.Columns["Sl No"].AutoIncrement = true;
            dtbl.Columns["Sl No"].AutoIncrementSeed = 1;
            dtbl.Columns["Sl No"].AutoIncrementStep = 1;
            sqlDa.Fill(dtbl);
            lstDataTable.Add(dtbl);
            if (dtbl.Columns.Contains("ledgerId"))
            {
                dtbl.Columns.Remove("ledgerId");
            }
            return lstDataTable;
        }
    }
}
