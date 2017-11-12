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
    class OpeningAndClosingStockDetails : DBConnection
    {
        public string GetProfitAndLossSettings()
        {
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            SqlCommand sqlCmd = new SqlCommand("SettingsStatusCheck", sqlCon);
            sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@settingsName", SqlDbType.VarChar).Value = "StockValueCalculationMethod";
            return Convert.ToString(sqlCmd.ExecuteScalar().ToString());
        }

        public decimal ClosingStockValueGetOnDate(CategoryInfo infoCatagory)
        {
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            string calculationMethod = GetProfitAndLossSettings();
            SqlCommand sqlCmd = new SqlCommand();
            if (calculationMethod == "FIFO")
            {

                sqlCmd = new SqlCommand("StockValueOnDateByFIFO", sqlCon);

            }
            else if (calculationMethod == "Average Cost")
            {

                sqlCmd = new SqlCommand("StockValueOnDateByAVCO", sqlCon);

            }
            else if (calculationMethod == "High Cost")
            {

                sqlCmd = new SqlCommand("StockValueOnDateByHighCost", sqlCon);

            }
            else if (calculationMethod == "Low Cost")
            {

                sqlCmd = new SqlCommand("StockValueOnDateByLowCost", sqlCon);

            }
            else if (calculationMethod == "Last Purchase Rate")
            {

                sqlCmd = new SqlCommand("StockValueOnDateByLastPurchaseRate", sqlCon);

            }
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@date", SqlDbType.DateTime).Value = infoCatagory.DateTo;
            decimal outVariable = 0;
            object obj = sqlCmd.ExecuteScalar().ToString();
            if (obj != null)
            {
                decimal.TryParse(obj.ToString(), out outVariable);
            }
            return outVariable;
        }

        public decimal OpeningStockValueGetOnDate(CategoryInfo infoCatagory)
        {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }
                string calculationMethod = GetProfitAndLossSettings();
                SqlCommand sqlCmd = new SqlCommand();
                if (calculationMethod == "FIFO")
                {
                    sqlCmd = new SqlCommand("StockValueOnDateByFIFOForOpeningStockForBalancesheet", sqlCon);
                }
                else if (calculationMethod == "Average Cost")
                {
                    sqlCmd = new SqlCommand("StockValueOnDateByAVCOForOpeningStockForBalanceSheet", sqlCon);

                }
                else if (calculationMethod == "High Cost")
                {
                    sqlCmd = new SqlCommand("StockValueOnDateByHighCostForOpeningStockBlncSheet", sqlCon);
                }
                else if (calculationMethod == "Low Cost")
                {
                    sqlCmd = new SqlCommand("StockValueOnDateByLowCostForOpeningStockForBlncSheet", sqlCon);
                }
                else if (calculationMethod == "Last Purchase Rate")
                {
                    sqlCmd = new SqlCommand("StockValueOnDateByLastPurchaseRateForOpeningStockBlncSheet", sqlCon);
                }
                decimal dcstockValue = 0;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@date", SqlDbType.DateTime).Value = infoCatagory.DateFrom;
                sqlCmd.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = infoCatagory.DateTo;
                object obj = sqlCmd.ExecuteScalar();
                if (obj != null)
                {
                    decimal.TryParse(obj.ToString(), out dcstockValue);
                }
            return dcstockValue;
        }


    }
}
