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
using Entity;
using System.Data.SqlClient;
namespace AccountI.DAL
{
    public class PlotterFillSP : DBConnection
    {
        public List<DataTable> GetPlotterData(CategoryInfo infoCategory)
        {
            string strStoredProcedure = string.Empty;

            if (infoCategory.ModuleName == "Finance")
            {
                if (!(infoCategory.CatagoryName != "Overall Statistics" && infoCategory.CatagoryName != "Tax Details" &&
                    infoCategory.CatagoryName != "Exchange Rate" && infoCategory.CatagoryName != "Account Ledger" && infoCategory.CatagoryName != "Account Group" && infoCategory.CatagoryName != "Budget" &&
                    infoCategory.CatagoryName != "Trial Balance" && infoCategory.CatagoryName != "Balance Sheet" && infoCategory.CatagoryName != "Profit And Loss" &&
                    infoCategory.CatagoryName != "Cash Flow" && infoCategory.CatagoryName != "Fund Flow" && infoCategory.CatagoryName != "Daily Report"))
                {
                    if (infoCategory.CatagoryName == "Trial Balance")
                    {
                        strStoredProcedure = "AccountI_AccountGroup";
                    }
                    else if (infoCategory.CatagoryName == "Overall Statistics")
                        {
                            strStoredProcedure = "AccountI_ProfitAndLoss";
                        }
                    else
                    {
                        strStoredProcedure = "AccountI_" + Convert.ToString(infoCategory.CatagoryName).Trim();
                    }
                }
                else
                {
                    strStoredProcedure = "AccountI_ProfitAndLoss";
                }

            }
            else if (infoCategory.ModuleName == "Overall Statistics")
            {
                strStoredProcedure = "AccountI_ProfitAndLoss";
            }
            else
            {
                strStoredProcedure = "AccountI_" + Convert.ToString(infoCategory.ModuleName).Trim();
            }
            List<char> resutl = strStoredProcedure.ToList();
            resutl.RemoveAll(c => c == ' ');
            strStoredProcedure = new string(resutl.ToArray());

            string SplitValue = FunctionSplitValue(infoCategory);
            infoCategory.SplitValue = SplitValue.Split('&');
            List<DataTable> lstDataTable = new List<DataTable>();
            lstDataTable.Clear();
            foreach (string word in infoCategory.SplitValue)
            {
                DataTable dtbl = new DataTable();
                SqlDataAdapter sqlDa = new SqlDataAdapter(strStoredProcedure, sqlCon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.CommandTimeout = 300;
                sqlDa.SelectCommand.Parameters.Add("@type", SqlDbType.VarChar).Value = (infoCategory.TypeName != string.Empty && infoCategory.TypeName != null) ? infoCategory.TypeName : "Month";
                sqlDa.SelectCommand.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = infoCategory.DateFrom;
                sqlDa.SelectCommand.Parameters.Add("@toDate", SqlDbType.DateTime).Value = infoCategory.DateTo;
                sqlDa.SelectCommand.Parameters.Add("@checkValue", SqlDbType.VarChar).Value = (infoCategory.CheckedValueName != string.Empty && infoCategory.CheckedValueName != null) ? infoCategory.CheckedValueName : "Overall Statistics";
                sqlDa.SelectCommand.Parameters.Add("@sortBy", SqlDbType.VarChar).Value = (infoCategory.SortByName != string.Empty && infoCategory.SortByName != null) ? infoCategory.SortByName : "Amount";
                sqlDa.SelectCommand.Parameters.Add("@category", SqlDbType.VarChar).Value = (infoCategory.CatagoryName != string.Empty && infoCategory.CatagoryName != null) ? infoCategory.CatagoryName : "Overall Statistics";
                sqlDa.SelectCommand.Parameters.Add("@subCategory", SqlDbType.VarChar).Value = (word.TrimEnd() != string.Empty && word != null) ? word.Trim() : "Overall Statistics";
                if (infoCategory.CatagoryName == "Budget")
                {
                    sqlDa.SelectCommand.Parameters.Add("@budgetName", SqlDbType.VarChar).Value = (infoCategory.SubCatagoryName != string.Empty && infoCategory.SubCatagoryName != null) ? infoCategory.SubCatagoryName : "Select Budget";
                }
                sqlDa.Fill(dtbl);
                lstDataTable.Add(dtbl);
            }
            return lstDataTable;
        }

        private string FunctionSplitValue(CategoryInfo infoCategory)
        {
            string SplitValue = string.Empty;

            if (infoCategory.CatagoryName == "Bill Allocation" || infoCategory.CatagoryName == "Budget")
            {
                SplitValue = "Debit & Credit";
            }
            else if (infoCategory.CatagoryName == "Rate")
            {
                SplitValue = "Purchase & Sales";
            }
            else if (infoCategory.CatagoryName == "Balance Sheet")
            {
                SplitValue = "Asset & Liability";
            }
            else if (infoCategory.CatagoryName == "Daily Report")
            {
                SplitValue = (infoCategory.SortByName == "Amount") ? "Debit & Credit" : "InWardQty & OutWardQty";
            }
            else if (infoCategory.CatagoryName == "Bank Reconciliation")
            {
                if (infoCategory.SubCatagoryName == "Overall Statistics")
                {
                    SplitValue = "Unreconciled & Reconciled";
                }
                else if (infoCategory.SubCatagoryName == "Reconciled")
                {
                    SplitValue = "Reconciled Deposit & Reconciled Withdrawal";
                }
                else if (infoCategory.SubCatagoryName == "Unreconciled")
                {
                    SplitValue = "Unreconciled Deposit & Unreconciled Withdrawal";
                }
            }
            else if (infoCategory.CatagoryName.Contains('&'))
            {
                SplitValue = infoCategory.CatagoryName;
            }
            else
            {
                if (infoCategory.ModuleName == "Party Balance" && (infoCategory.CatagoryName == "Overall Statistics" || infoCategory.CatagoryName == string.Empty))
                {
                    SplitValue = "Payable & Receivable";
                }
                else if (infoCategory.ModuleName == "Bank" && (infoCategory.CatagoryName == "Overall Statistics" || infoCategory.CatagoryName == string.Empty))
                {
                    SplitValue = "Deposit & Withdrawal";
                }
                else
                {
                    SplitValue = infoCategory.SubCatagoryName;
                }
            }
            if (infoCategory.ModuleName == "Product")
            {
                if (infoCategory.CatagoryName == "Transactions" && infoCategory.SubCatagoryName == "Overall Statistics")
                {
                    SplitValue = "SalesRate & PurchaseRate";
                }
            }
            return SplitValue;
        }
    }
}
