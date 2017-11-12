using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace AccountSkate
{
    class OpenAccountExport:DBConnection
    {

        public DataTable PricingLevelToExcel()
        {
            if (sqlcon.State == ConnectionState.Closed)
            {
                sqlcon.Open();
            }
            SqlDataAdapter sqlDa = new SqlDataAdapter("PricingLevelGrid", sqlcon);
            sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtbl = new DataTable();
            sqlDa.Fill(dtbl);
            sqlcon.Close();
            return dtbl;
        }

        public DataTable GodownExportToExcel()
        {
            if (sqlcon.State == ConnectionState.Closed)
            {
                sqlcon.Open();
            }
            SqlDataAdapter sqlDa = new SqlDataAdapter("GodownGrid", sqlcon);
            sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtbl = new DataTable();
            sqlDa.Fill(dtbl);
            sqlcon.Close();
            return dtbl;
        }

        public DataTable UnitExportToExcel()
        {
            if (sqlcon.State == ConnectionState.Closed)
            {
                sqlcon.Open();
            }
            SqlDataAdapter sqlDa = new SqlDataAdapter("UnitGrid", sqlcon);
            sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtbl = new DataTable();
            sqlDa.Fill(dtbl);
            sqlcon.Close();
            return dtbl;
        }

        public DataTable VoucherTypeExportToExcel()
        {
            if (sqlcon.State == ConnectionState.Closed)
            {
                sqlcon.Open();
            }
            SqlDataAdapter sqlDa = new SqlDataAdapter("VoucherTypeGrid", sqlcon);
            sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtbl = new DataTable();
            sqlDa.Fill(dtbl);
            sqlcon.Close();
            return dtbl;
        }

        public DataTable CurrencyExportToexcel()
        {
            if (sqlcon.State == ConnectionState.Closed)
            {
                sqlcon.Open();
            }
            SqlDataAdapter sqlDa = new SqlDataAdapter("CurrencyGrid", sqlcon);
            sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtbl = new DataTable();
            sqlDa.Fill(dtbl);
            sqlcon.Close();
            return dtbl;
        }

        public DataTable ProductGroupExportToExcel()
        {
            if (sqlcon.State == ConnectionState.Closed)
            {
                sqlcon.Open();
            }
            SqlDataAdapter sqlDa = new SqlDataAdapter("ProductGroupgrid", sqlcon);
            sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtbl = new DataTable();
            sqlDa.Fill(dtbl);
            sqlcon.Close();
            return dtbl;
        }

        public DataTable ProductExportTOExcel()
        {
            if (sqlcon.State == ConnectionState.Closed)
            {
                sqlcon.Open();
            }
            SqlDataAdapter sqlDa = new SqlDataAdapter("ProductGride", sqlcon);
            sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtbl = new DataTable();
            sqlDa.Fill(dtbl);
            sqlcon.Close();
            return dtbl;
        }

        public DataTable SupplierExportToExcel()
        {
            if (sqlcon.State == ConnectionState.Closed)
            {
                sqlcon.Open();
            }
            SqlDataAdapter sqlDa = new SqlDataAdapter("SupplierGrid", sqlcon);
            sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtbl = new DataTable();
            sqlDa.Fill(dtbl);
            sqlcon.Close();
            return dtbl;
        }

        public DataTable SalesManExportToExcel()
        {
            if (sqlcon.State == ConnectionState.Closed)
            {
                sqlcon.Open();
            }
            SqlDataAdapter sqlDa = new SqlDataAdapter("SalesmanGrid", sqlcon);
            sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtbl = new DataTable();
            sqlDa.Fill(dtbl);
            sqlcon.Close();
            return dtbl;
        }

        public DataTable CustomerExportToExcel()
        {
            if (sqlcon.State == ConnectionState.Closed)
            {
                sqlcon.Open();
            }
            SqlDataAdapter sqlDa = new SqlDataAdapter("CustomerGrid", sqlcon);
            sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtbl = new DataTable();
            sqlDa.Fill(dtbl);
            sqlcon.Close();
            return dtbl;
        }

        public DataTable AccountLegderExportToExcel()
        {
            if (sqlcon.State == ConnectionState.Closed)
            {
                sqlcon.Open();
            }
            SqlDataAdapter sqlDa = new SqlDataAdapter("AccountLegderGrid", sqlcon);
            sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtbl = new DataTable();
            sqlDa.Fill(dtbl);
            sqlcon.Close();
            return dtbl;
        }

        public DataTable AccountGroupExportToExcel()
        {
            if (sqlcon.State == ConnectionState.Closed)
            {
                sqlcon.Open();
            }
            SqlDataAdapter sqlDa = new SqlDataAdapter("AccountGroupGrid", sqlcon);
            sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtbl = new DataTable();
            sqlDa.Fill(dtbl);
            sqlcon.Close();
            return dtbl;
        }

        public DataTable BatchExportToExcel()
        {
            if (sqlcon.State == ConnectionState.Closed)
            {
                sqlcon.Open();
            }
            SqlDataAdapter sqlDa = new SqlDataAdapter("BatchGrid", sqlcon);
            sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtbl = new DataTable();
            sqlDa.Fill(dtbl);
            sqlcon.Close();
            return dtbl;
        }

        public DataTable StockExportToExcel()
        {
            if (sqlcon.State == ConnectionState.Closed)
            {
                sqlcon.Open();
            }
            SqlDataAdapter sqlDa = new SqlDataAdapter("StockGride", sqlcon);
            sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtbl = new DataTable();
            sqlDa.Fill(dtbl);
            sqlcon.Close();
            return dtbl;
        }
    }
}
