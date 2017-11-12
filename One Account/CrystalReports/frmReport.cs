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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using One_Account.CrystalReports.Reports;
using One_Account.CrystalReports.Reports.ProductStatisticsReports;
using CrystalDecisions.CrystalReports.Engine;

namespace One_Account
{
    public partial class frmReport : Form
    {
        #region Public Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        frmPaySlip frmPaySlipObject;
        #endregion

        #region Function
        /// <summary>
        /// Create an instance for frmReport Class
        /// </summary>
        public frmReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to print StockJournal in curresponding Crystal report
        /// </summary>
        /// <param name="dsStockJournal"></param>
        internal void StockJournalPrinting(DataSet dsStockJournal)
        {
            try
            {

                crptStockJournal crptStockJournalObj = new crptStockJournal();
                foreach (DataTable dtbl in dsStockJournal.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptStockJournalObj.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {

                        crptStockJournalObj.Database.Tables["dtblStockJournalMaster"].SetDataSource(dtbl);

                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        crptStockJournalObj.Database.Tables["dtblStockJournalDetailsConsumption"].SetDataSource(dtbl);

                    }
                    else if (dtbl.TableName == "Table3")
                    {
                        crptStockJournalObj.Database.Tables["dtblStockJournalDetailsProduction"].SetDataSource(dtbl);
                    }

                }
                this.crptViewer.ReportSource = crptStockJournalObj;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptStockJournalObj.PrintToPrinter(1, false, 0, 0);
                }
            }

            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV1 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to call frmPaySlip form to select and view Ledger
        /// </summary>
        /// <param name="frmPaySlip"></param>
        /// <param name="decEmployeeId"></param>
        /// <param name="dtSalaryMonth"></param>
        public void CallFromPaySlip(frmPaySlip frmPaySlip, decimal decEmployeeId, DateTime dtSalaryMonth)
        {
            try
            {
                this.frmPaySlipObject = frmPaySlip;
                base.Show();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV2 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print PaySlip in curresponding Crystal report
        /// </summary>
        /// <param name="dsPaySlip"></param>
        internal void PaySlipPrinting(DataSet dsPaySlip)
        {
            try
            {
                decimal decTotalAdd = 0;
                decimal decTotalDed = 0;
                decimal decNetPay = 0;
                crptPaySlip crptPaySlip = new crptPaySlip();

                foreach (DataTable dtbl in dsPaySlip.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptPaySlip.Database.Tables["dtblCompanyDetails"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        crptPaySlip.Database.Tables["dtblEmployeeDetails"].SetDataSource(dtbl);

                        foreach (DataRow drow in dtbl.Rows)
                        {
                            if (drow["ADDamount"].ToString() != string.Empty)
                            {
                                decTotalAdd += Convert.ToDecimal(drow["ADDamount"].ToString());
                            }
                            if (drow["DEDamount"].ToString() != string.Empty)
                            {
                                decTotalDed += Convert.ToDecimal(drow["DEDamount"].ToString());
                            }
                        }

                        foreach (DataRow drow in dtbl.Rows)
                        {
                            if (drow["LOP"].ToString() != string.Empty)
                            {
                                decTotalDed += Convert.ToDecimal(drow["LOP"].ToString());
                            }

                            if (drow["Deduction"].ToString() != string.Empty)
                            {
                                decTotalDed += Convert.ToDecimal(drow["Deduction"].ToString());
                            }

                            if (drow["Advance"].ToString() != string.Empty)
                            {
                                decTotalDed += Convert.ToDecimal(drow["Advance"].ToString());
                            }

                            if (drow["Bonus"].ToString() != string.Empty)
                            {
                                decTotalAdd += Convert.ToDecimal(drow["Bonus"].ToString());
                            }

                            break;

                        }
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        DataColumn dtClmn = new DataColumn("AmountInWords");
                        dtbl.Columns.Add(dtClmn);
                        decNetPay = decTotalAdd - decTotalDed;
                        foreach (DataRow drow in dtbl.Rows)
                        {
                            drow["AmountInWords"] = new NumToText().AmountWords(decNetPay, PublicVariables._decCurrencyId);
                        }

                        crptPaySlip.Database.Tables["dtblOther"].SetDataSource(dtbl);
                    }
                }

                this.crptViewer.ReportSource = crptPaySlip;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptPaySlip.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV3 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print PhysicalStock in curresponding Crystal report
        /// </summary>
        /// <param name="dsPhysicalStock"></param>
        internal void PhysicalStockPrinting(DataSet dsPhysicalStock)
        {
            try
            {

                crptPhysicalStock crptPhysicalStock = new crptPhysicalStock();
                decimal decTotalAmount = 0;
                foreach (DataTable dtbl in dsPhysicalStock.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptPhysicalStock.Database.Tables["dtblCompanyDetails"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {

                        foreach (DataRow drow in dtbl.Rows)
                        {
                            DataColumn dtClmn = new DataColumn("AmountInWords");
                            dtbl.Columns.Add(dtClmn);
                            if (drow["totalAmount"].ToString() != string.Empty)
                            {
                                decTotalAmount = Convert.ToDecimal(drow["totalAmount"].ToString());
                                drow["AmountInWords"] = new NumToText().AmountWords(decTotalAmount, PublicVariables._decCurrencyId);
                            }

                            crptPhysicalStock.Database.Tables["dtblOtherDetails"].SetDataSource(dtbl);
                        }
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        DataColumn dtClmnSlNo = new DataColumn("SlNo");
                        dtbl.Columns.Add(dtClmnSlNo);
                        int inRowIndex = 0;

                        foreach (DataRow drSlNo in dtbl.Rows)
                        {
                            drSlNo["SlNo"] = ++inRowIndex;
                        }

                        crptPhysicalStock.Database.Tables["dtblGridDetails"].SetDataSource(dtbl);
                    }

                }
                this.crptViewer.ReportSource = crptPhysicalStock;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptPhysicalStock.PrintToPrinter(1, false, 0, 0);
                }
            }

            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV4 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to print StockJournalReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsStockJournalReport"></param>
        internal void StockJournalReportPrint(DataSet dsStockJournalReport)
        {
            try
            {

                crptStockJournalReport crptStockJournalReportObj = new crptStockJournalReport();

                foreach (DataTable dtbl in dsStockJournalReport.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptStockJournalReportObj.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {

                        crptStockJournalReportObj.Database.Tables["dtblStockJournalReport"].SetDataSource(dtbl);

                    }


                }
                this.crptViewer.ReportSource = crptStockJournalReportObj;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptStockJournalReportObj.PrintToPrinter(1, false, 0, 0);
                }
            }

            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV5 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to print ContraVoucher in curresponding Crystal report
        /// </summary>
        /// <param name="dsCOntraVoucher"></param>
        internal void ContraVoucherPrinting(DataSet dsCOntraVoucher)
        {
            try
            {

                crptContraVoucher crptContraVoucher = new crptContraVoucher();
                decimal decTotalAmount = 0;
                foreach (DataTable dtbl in dsCOntraVoucher.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptContraVoucher.Database.Tables["dtblCompanyDetails"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {

                        foreach (DataRow drow in dtbl.Rows)
                        {
                            DataColumn dtClmn = new DataColumn("AmountInWords");
                            dtbl.Columns.Add(dtClmn);
                            if (drow["totalAmount"].ToString() != string.Empty)
                            {
                                decTotalAmount = Convert.ToDecimal(drow["totalAmount"].ToString());
                                drow["AmountInWords"] = new NumToText().AmountWords(decTotalAmount, PublicVariables._decCurrencyId);
                            }

                            crptContraVoucher.Database.Tables["dtblOtherDetails"].SetDataSource(dtbl);
                        }
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        foreach (DataRow drow in dtbl.Rows)
                        {

                            if (drow["chequeDate"].ToString() == "01 Jan 1753")
                            {

                                drow["chequeDate"] = string.Empty;
                            }

                        }
                        DataColumn dtClmnSlNo = new DataColumn("SlNo");
                        dtbl.Columns.Add(dtClmnSlNo);
                        int inRowIndex = 0;

                        foreach (DataRow drSlNo in dtbl.Rows)
                        {
                            drSlNo["SlNo"] = ++inRowIndex;
                        }
                        crptContraVoucher.Database.Tables["dtblGridDetails"].SetDataSource(dtbl);
                    }

                }
                this.crptViewer.ReportSource = crptContraVoucher;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptContraVoucher.PrintToPrinter(1, false, 0, 0);
                }
            }

            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV6 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to print PaymentVoucher in curresponding Crystal report
        /// </summary>
        /// <param name="dsPaymentVoucher"></param>
        internal void PaymentVoucherPrinting(DataSet dsPaymentVoucher)
        {
            try
            {

                crptPaymentVoucher crptPaymentVoucher = new crptPaymentVoucher();
                decimal decTotalAmount = 0;
                foreach (DataTable dtbl in dsPaymentVoucher.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptPaymentVoucher.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {

                        foreach (DataRow drow in dtbl.Rows)
                        {
                            DataColumn dtClmn = new DataColumn("Amount In Words");
                            dtbl.Columns.Add(dtClmn);
                            if (drow["totalAmount"].ToString() != string.Empty)
                            {
                                decTotalAmount = Convert.ToDecimal(drow["totalAmount"].ToString());
                                drow["Amount In Words"] = new NumToText().AmountWords(decTotalAmount, PublicVariables._decCurrencyId);
                            }
                            crptPaymentVoucher.Database.Tables["dtblPaymentMaster"].SetDataSource(dtbl);
                        }
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        foreach (DataRow drow in dtbl.Rows)
                        {
                            if (drow["chequeNo"].ToString() == string.Empty)
                            {
                                drow["chequeDate"] = string.Empty;
                            }
                        }
                        DataColumn dtClmnSlNo = new DataColumn("SlNo");
                        dtbl.Columns.Add(dtClmnSlNo);
                        int inRowIndex = 0;

                        foreach (DataRow drSlNo in dtbl.Rows)
                        {
                            drSlNo["SlNo"] = ++inRowIndex;
                        }
                        crptPaymentVoucher.Database.Tables["dtblPaymentDetails"].SetDataSource(dtbl);
                    }
                }
                this.crptViewer.ReportSource = crptPaymentVoucher;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptPaymentVoucher.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV7 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to print PaymentReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsPaymentReport"></param>
        internal void PaymentReportPrinting(DataSet dsPaymentReport)
        {
            try
            {

                crptPaymentReport crptPaymentReport = new crptPaymentReport();
                foreach (DataTable dtbl in dsPaymentReport.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptPaymentReport.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }

                    else if (dtbl.TableName == "Table1")
                    {

                        crptPaymentReport.Database.Tables["dtblPaymentMaster"].SetDataSource(dtbl);
                    }

                }
                this.crptViewer.ReportSource = crptPaymentReport;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptPaymentReport.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV8 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to print ReceiptReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsReceiptReport"></param>
        internal void ReceiptReportPrinting(DataSet dsReceiptReport)
        {
            try
            {

                crptReceiptReport crptReceiptReport = new crptReceiptReport();
                foreach (DataTable dtbl in dsReceiptReport.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptReceiptReport.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    //else if (dtbl.TableName == "Table1")
                    //{
                    //    crptPaymentVoucher.Database.Tables["dtblPaymentMaster"].SetDataSource(dtbl);
                    //}
                    else if (dtbl.TableName == "Table1")
                    {
                        //dtbl.Columns.Add("Sl No", typeof(decimal));
                        //dtbl.Columns["Sl No"].AutoIncrement = true;
                        //dtbl.Columns["Sl No"].AutoIncrementSeed = 1;
                        //dtbl.Columns["Sl No"].AutoIncrementStep = 1;
                        crptReceiptReport.Database.Tables["dtblReceiptMaster"].SetDataSource(dtbl);
                    }

                }
                this.crptViewer.ReportSource = crptReceiptReport;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptReceiptReport.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV9 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to print ReceiptVoucher in curresponding Crystal report
        /// </summary>
        /// <param name="dsReceiptVoucher"></param>
        internal void ReceiptVoucherPrinting(DataSet dsReceiptVoucher)
        {
            try
            {

                crptReceiptVoucher crptReceiptVoucher = new crptReceiptVoucher();
                decimal decTotalAmount = 0;
                foreach (DataTable dtbl in dsReceiptVoucher.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptReceiptVoucher.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {

                        foreach (DataRow drow in dtbl.Rows)
                        {
                            DataColumn dtClmn = new DataColumn("Amount In Words");
                            dtbl.Columns.Add(dtClmn);
                            if (drow["totalAmount"].ToString() != string.Empty)
                            {
                                decTotalAmount = Convert.ToDecimal(drow["totalAmount"].ToString());
                                drow["Amount In Words"] = new NumToText().AmountWords(decTotalAmount, PublicVariables._decCurrencyId);
                            }
                            crptReceiptVoucher.Database.Tables["dtblReceiptMaster"].SetDataSource(dtbl);
                        }
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        foreach (DataRow drow in dtbl.Rows)
                        {

                            if (drow["chequeNo"].ToString() == string.Empty)
                            {

                                drow["chequeDate"] = string.Empty;
                            }

                        }
                        DataColumn dtClmnSlNo = new DataColumn("SlNo");
                        dtbl.Columns.Add(dtClmnSlNo);
                        int inRowIndex = 0;

                        foreach (DataRow drSlNo in dtbl.Rows)
                        {
                            drSlNo["SlNo"] = ++inRowIndex;
                        }
                        crptReceiptVoucher.Database.Tables["dtblReceiptDetails"].SetDataSource(dtbl);
                    }

                }
                this.crptViewer.ReportSource = crptReceiptVoucher;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptReceiptVoucher.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV10 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to print JournalVoucher in curresponding Crystal report
        /// </summary>
        /// <param name="dsJournalVoucher"></param>
        internal void JournalVoucherPrinting(DataSet dsJournalVoucher)
        {
            try
            {

                crptJournalVoucher crptJournalVoucher = new crptJournalVoucher();
                decimal decTotalAmount = 0;
                foreach (DataTable dtbl in dsJournalVoucher.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptJournalVoucher.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {

                        foreach (DataRow drow in dtbl.Rows)
                        {
                            DataColumn dtClmn = new DataColumn("Amount In Words");
                            dtbl.Columns.Add(dtClmn);
                            if (drow["totalAmount"].ToString() != string.Empty)
                            {
                                decTotalAmount = Convert.ToDecimal(drow["totalAmount"].ToString());
                                drow["Amount In Words"] = new NumToText().AmountWords(decTotalAmount, PublicVariables._decCurrencyId);
                            }
                            crptJournalVoucher.Database.Tables["dtblJournalMaster"].SetDataSource(dtbl);
                        }
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        DataColumn dtClmn = new DataColumn("CreditOrDebit");
                        dtbl.Columns.Add(dtClmn);
                        DataColumn dtClmn1 = new DataColumn("Amount");
                        dtbl.Columns.Add(dtClmn1);
                        foreach (DataRow drow in dtbl.Rows)
                        {

                            if (drow["chequeNo"].ToString() == string.Empty)
                            {

                                drow["chequeDate"] = string.Empty;
                            }


                            if (Convert.ToDecimal(drow["debit"].ToString()) == 0)
                            {
                                drow["Amount"] = Convert.ToDecimal(drow["credit"].ToString());
                                drow["CreditOrDebit"] = "Cr";
                            }
                            else
                            {
                                drow["Amount"] = Convert.ToDecimal(drow["debit"].ToString());
                                drow["CreditOrDebit"] = "Dr";
                            }

                        }
                        crptJournalVoucher.Database.Tables["dtblJournalDetails"].SetDataSource(dtbl);
                    }

                }
                this.crptViewer.ReportSource = crptJournalVoucher;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptJournalVoucher.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV11 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print ServiceVoucher in curresponding Crystal report
        /// </summary>
        /// <param name="dsServiceVoucher"></param>
        internal void ServiceVoucherPrinting(DataSet dsServiceVoucher)
        {
            try
            {
                decimal decGrandTotal = 0;
                crptServiceVoucher crptServiceVoucher = new crptServiceVoucher();

                foreach (DataTable dtbl in dsServiceVoucher.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptServiceVoucher.Database.Tables["dtblCompanyDetails"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        crptServiceVoucher.Database.Tables["dtblServiceMaster"].SetDataSource(dtbl);

                        foreach (DataRow drow in dtbl.Rows)
                        {
                            decGrandTotal = Convert.ToDecimal(drow["grandTotal"].ToString());
                        }

                        //decimal decExchangeRateID = 0;

                        //foreach (DataRow drow in dtbl.Rows)
                        //{
                        //    decExchangeRateID = Convert.ToDecimal(drow["exchangeRateId"].ToString());
                        //}

                        //CurrencyInfo infoCurrency = new CurrencyInfo();
                        //ExchangeRateSP spExchangeRate = new ExchangeRateSP();
                        //infoCurrency = spExchangeRate.GetCurrencyByExchangeRateID(decExchangeRateID);

                        //DataColumn dtClmnCurrency = new DataColumn("currency");
                        //dtbl.Columns.Add(dtClmnCurrency);

                        //foreach (DataRow drowCurrency in dtbl.Rows)
                        //{
                        //    drowCurrency["currency"] = infoCurrency.CurrencyName;
                        //}

                        crptServiceVoucher.Database.Tables["dtblServiceMaster"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        DataColumn dtClmn = new DataColumn("AmountInWords");
                        dtbl.Columns.Add(dtClmn);

                        foreach (DataRow drow in dtbl.Rows)
                        {
                            drow["AmountInWords"] = new NumToText().AmountWords(decGrandTotal, PublicVariables._decCurrencyId);
                        }

                        //decimal decExchangeRateID = 0;

                        //foreach (DataRow drow in dtbl.Rows)
                        //{
                        //    decExchangeRateID = Convert.ToDecimal(drow["exchangeRateId"].ToString());
                        //}

                        //CurrencyInfo infoCurrency = new CurrencyInfo();
                        //ExchangeRateSP spExchangeRate = new ExchangeRateSP();
                        //infoCurrency = spExchangeRate.GetCurrencyByExchangeRateID(decExchangeRateID);

                        //DataColumn dtClmnCurrency = new DataColumn("currency");
                        //dtbl.Columns.Add(dtClmnCurrency);

                        //foreach (DataRow drowCurrency in dtbl.Rows)
                        //{
                        //    drowCurrency["currency"] = infoCurrency.CurrencyName;
                        //}

                        DataColumn dtClmnSlNo = new DataColumn("SlNo");
                        dtbl.Columns.Add(dtClmnSlNo);
                        int inRowIndex = 0;

                        foreach (DataRow drSlNo in dtbl.Rows)
                        {
                            drSlNo["SlNo"] = ++inRowIndex;
                        }

                        crptServiceVoucher.Database.Tables["dtblServiceVoucherDetails"].SetDataSource(dtbl);
                    }
                }

                this.crptViewer.ReportSource = crptServiceVoucher;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptServiceVoucher.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV12 " + ex.Message;
            }
        }
        /// <summary>
        ///   Function to print EmployeeAddressBook in curresponding Crystal report
        /// </summary>
        /// <param name="dsEmployeeAddressBook"></param>
        internal void EmployeeAddressBookPrinting(DataSet dsEmployeeAddressBook)
        {
            try
            {
                crptEmployeeAddressBook crptEmployeeAddressBook = new crptEmployeeAddressBook();
                foreach (DataTable dtbl in dsEmployeeAddressBook.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptEmployeeAddressBook.Database.Tables["dtblCompanyDetails"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        crptEmployeeAddressBook.Database.Tables["dtblGridDetails"].SetDataSource(dtbl);
                    }
                }
                this.crptViewer.ReportSource = crptEmployeeAddressBook;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptEmployeeAddressBook.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV13 " + ex.Message;
            }

        }
        /// <summary>
        /// Function to print EmployeeAddressBook in curresponding Crystal report
        /// </summary>
        /// <param name="dsProductBatchReport"></param>
        internal void ProductBatchReportPrinting(DataSet dsProductBatchReport)
        {
            try
            {
                crptProductBatchReport crptProductBatchReport = new crptProductBatchReport();
                foreach (DataTable dtbl in dsProductBatchReport.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptProductBatchReport.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        crptProductBatchReport.Database.Tables["dtblProductBatch"].SetDataSource(dtbl);
                    }
                }
                this.crptViewer.ReportSource = crptProductBatchReport;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptProductBatchReport.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV14 " + ex.Message;
            }

        }
        /// <summary>
        /// Function to print MonthlyAttendance in curresponding Crystal report
        /// </summary>
        /// <param name="dsMonthlyAttendance"></param>
        internal void MonthlyAttendancePrinting(DataSet dsMonthlyAttendance)
        {
            try
            {
                crptMonthlyAttendance crptMonthlyAttendance = new crptMonthlyAttendance();
                foreach (DataTable dtbl in dsMonthlyAttendance.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptMonthlyAttendance.Database.Tables["dtblCompanyDetails"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        crptMonthlyAttendance.Database.Tables["dtblGridDetails"].SetDataSource(dtbl);
                    }
                }
                this.crptViewer.ReportSource = crptMonthlyAttendance;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptMonthlyAttendance.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV15 " + ex.Message;
            }

        }
        /// <summary>
        /// Function to print DailyAttendanceReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsDailyAttendanceReport"></param>
        internal void DailyAttendanceReportPrinting(DataSet dsDailyAttendanceReport)
        {
            try
            {
                crptDailyAttendanceReport crptDailyAttendanceReport = new crptDailyAttendanceReport();

                foreach (DataTable dtbl in dsDailyAttendanceReport.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptDailyAttendanceReport.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        crptDailyAttendanceReport.Database.Tables["dtblAttendance"].SetDataSource(dtbl);
                    }
                }
                this.crptViewer.ReportSource = crptDailyAttendanceReport;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptDailyAttendanceReport.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV16 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to print DailySalaryReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsDailySalaryReport"></param>
        internal void DailySalaryReport(DataSet dsDailySalaryReport)
        {
            try
            {
                crptDailySalaryReport crptdailySalaryReport = new crptDailySalaryReport();

                foreach (DataTable dtbl in dsDailySalaryReport.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptdailySalaryReport.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {

                        crptdailySalaryReport.Database.Tables["dtblDailySalaryreport"].SetDataSource(dtbl);
                    }
                }
                this.crptViewer.ReportSource = crptdailySalaryReport;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptdailySalaryReport.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV17 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print PurchaseOrder in curresponding Crystal report
        /// </summary>
        /// <param name="dsPurchaseOrderReport"></param>
        internal void PurchaseOrderPrinting(DataSet dsPurchaseOrderReport)
        {
            try
            {
                decimal decTotalAmount = 0;
                crptPurchaseOrderReport crptPurchaseOrderobj = new crptPurchaseOrderReport();
                foreach (DataTable dtbl in dsPurchaseOrderReport.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptPurchaseOrderobj.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        foreach (DataRow drow in dtbl.Rows)
                        {
                            DataColumn dtClmn = new DataColumn("amountInWords");
                            dtbl.Columns.Add(dtClmn);
                            if (drow["totalAmount"].ToString() != string.Empty)
                            {
                                decTotalAmount = Convert.ToDecimal(drow["totalAmount"].ToString());
                                drow["amountInWords"] = new NumToText().AmountWords(decTotalAmount, PublicVariables._decCurrencyId);
                            }
                            crptPurchaseOrderobj.Database.Tables["dtblPurchaseOrderMaster"].SetDataSource(dtbl);
                        }
                    }
                    else
                    {
                        crptPurchaseOrderobj.Database.Tables["dtblPurchaseOrderDetails"].SetDataSource(dtbl);

                    }
                }
                this.crptViewer.ReportSource = crptPurchaseOrderobj;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptPurchaseOrderobj.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV18 " + ex.Message;
            }

        }
        /// <summary>
        ///  Function to print MaterialReceipt in curresponding Crystal report
        /// </summary>
        /// <param name="dsMaterialReceiptReport"></param>
        internal void MaterialReceiptPrinting(DataSet dsMaterialReceiptReport)
        {
            try
            {
                decimal decTotalAmount = 0;
                crptMaterialreceipt crptMaterialreceiptObj = new crptMaterialreceipt();
                foreach (DataTable dtbl in dsMaterialReceiptReport.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptMaterialreceiptObj.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {

                        
                        foreach (DataRow drow in dtbl.Rows)
                        {
                            DataColumn dtClmn = new DataColumn("amountInWords");
                            dtbl.Columns.Add(dtClmn);
                            if (drow["totalAmount"].ToString() != string.Empty)
                            {
                                decTotalAmount = Convert.ToDecimal(drow["totalAmount"].ToString());
                                drow["amountInWords"] = new NumToText().AmountWords(decTotalAmount, PublicVariables._decCurrencyId);
                            }
                            crptMaterialreceiptObj.Database.Tables["dtblMaterialReceiptMaster"].SetDataSource(dtbl);
                        }
                    }
                    else
                    {
                        crptMaterialreceiptObj.Database.Tables["dtblMaterialReceiptDetails"].SetDataSource(dtbl);
                    }
                }
                this.crptViewer.ReportSource = crptMaterialreceiptObj;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptMaterialreceiptObj.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV19 " + ex.Message;
            }

        }
        /// <summary>
        /// Function to print PurchaseInvoice in curresponding Crystal report
        /// </summary>
        /// <param name="dsPurchaseInvoice"></param>
        internal void PurchaseInvoicePrinting(DataSet dsPurchaseInvoice)
        {
            try
            {
                decimal decTotalAmount = 0;
                crptPurchaseInvoice crptPurchaseInvoiceObj = new crptPurchaseInvoice();
                foreach (DataTable dtbl in dsPurchaseInvoice.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptPurchaseInvoiceObj.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        foreach (DataRow drow in dtbl.Rows)
                        {
                            DataColumn dtClmn = new DataColumn("amountInWords");
                            dtbl.Columns.Add(dtClmn);
                            if (drow["grandTotal"].ToString() != string.Empty)
                            {
                                decTotalAmount = Convert.ToDecimal(drow["grandTotal"].ToString());
                                drow["amountInWords"] = new NumToText().AmountWords(decTotalAmount, PublicVariables._decCurrencyId);
                            }
                            crptPurchaseInvoiceObj.Database.Tables["dtblPurchaseMaster"].SetDataSource(dtbl);
                        }
                        
                    }
                    else
                    {
                        crptPurchaseInvoiceObj.Database.Tables["dtblPurchaseDetails"].SetDataSource(dtbl);
                    }
                }
                this.crptViewer.ReportSource = crptPurchaseInvoiceObj;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptPurchaseInvoiceObj.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV20 " + ex.Message;
            }

        }
        /// <summary>
        ///  Function to print PurchaseReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsPurchaseReport"></param>
        /// <param name="strTotal"></param>
        internal void PurchaseReportPrinting(DataSet dsPurchaseReport, string strTotal)
        {
            try
            {
                crptPurchaseReport crptPurchaseReportObj = new crptPurchaseReport();
                foreach (DataTable dtbl in dsPurchaseReport.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptPurchaseReportObj.Database.Tables["dtblCompanyReport"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        crptPurchaseReportObj.Database.Tables["dtblPurchaseReport"].SetDataSource(dtbl);
                    }
                }
                this.crptViewer.ReportSource = crptPurchaseReportObj;

                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptPurchaseReportObj.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV21 " + ex.Message;
            }

        }
        /// <summary>
        /// Function to print SalesOrder in curresponding Crystal report
        /// </summary>
        /// <param name="dsSalesOrderReport"></param>
        internal void SalesOrderPrinting(DataSet dsSalesOrderReport)
        {
            try
            {
                decimal decTotalAmount = 0;
                crptSalesOrderReport crptSalesOrderobj = new crptSalesOrderReport();
                foreach (DataTable dtbl in dsSalesOrderReport.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptSalesOrderobj.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        foreach (DataRow drow in dtbl.Rows)
                        {
                            DataColumn dtClmn = new DataColumn("amountInWords");
                            dtbl.Columns.Add(dtClmn);
                            if (drow["totalAmount"].ToString() != string.Empty)
                            {
                                decTotalAmount = Convert.ToDecimal(drow["totalAmount"].ToString());
                                drow["amountInWords"] = new NumToText().AmountWords(decTotalAmount, PublicVariables._decCurrencyId);
                            }
                            crptSalesOrderobj.Database.Tables["dtblSalesOrderMaster"].SetDataSource(dtbl);
                        }
                      
                    }
                    else
                    {
                        crptSalesOrderobj.Database.Tables["dtblSalesOrderDetails"].SetDataSource(dtbl);

                    }
                }
                this.crptViewer.ReportSource = crptSalesOrderobj;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptSalesOrderobj.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV22 " + ex.Message;
            }

        }
        /// <summary>
        /// Function to print SalesQuotation in curresponding Crystal report
        /// </summary>
        /// <param name="dsSalesQuotation"></param>
        internal void SalesQuotationPrinting(DataSet dsSalesQuotation)
        {
            try
            {

                crptSalesQuotation crptSalesQuotationObj = new crptSalesQuotation();
                decimal decTotalAmount = 0;
                foreach (DataTable dtbl in dsSalesQuotation.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptSalesQuotationObj.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        
                        foreach (DataRow drow in dtbl.Rows)
                        {
                            DataColumn dtClmn = new DataColumn("amountInWords");
                            dtbl.Columns.Add(dtClmn);
                            if (drow["totalAmount"].ToString() != string.Empty)
                            {
                                decTotalAmount = Convert.ToDecimal(drow["totalAmount"].ToString());
                                drow["amountInWords"] = new NumToText().AmountWords(decTotalAmount, PublicVariables._decCurrencyId);
                            }
                            crptSalesQuotationObj.Database.Tables["dtblSalesQuotationMaster"].SetDataSource(dtbl);
                        }
                    }
                    else
                    {
                        crptSalesQuotationObj.Database.Tables["dtblSalesQuotationDetails"].SetDataSource(dtbl);

                    }
                }
                this.crptViewer.ReportSource = crptSalesQuotationObj;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptSalesQuotationObj.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV23 " + ex.Message;
            }

        }
        /// <summary>
        /// Function to print SalesQuotationReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsSalesQuotation"></param>
        /// <param name="strGrandTotal"></param>
        internal void SalesQuotationReportPrinting(DataSet dsSalesQuotation, string strGrandTotal)
        {
            try
            {
                DataTable dtblGrandTotal = new DataTable();
                dtblGrandTotal.Columns.Add("GrandTotal", typeof(string));
                DataRow dr = dtblGrandTotal.NewRow();
                dr[0] = strGrandTotal;
                dtblGrandTotal.Rows.InsertAt(dr, 0);

                crptSalesQuotationReport crptSalesQuotationObj = new crptSalesQuotationReport();
                foreach (DataTable dtbl in dsSalesQuotation.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptSalesQuotationObj.Database.Tables["dtblCompanyReport"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {

                        crptSalesQuotationObj.Database.Tables["dtblSalesQuotationReportDetails"].SetDataSource(dtbl);
                    }

                }
                crptSalesQuotationObj.Database.Tables["dtblGrandTotal"].SetDataSource(dtblGrandTotal);
                this.crptViewer.ReportSource = crptSalesQuotationObj;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptSalesQuotationObj.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV24 " + ex.Message;
            }

        }
        /// <summary>
        /// Function to print PriceListReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsPriceListReport"></param>
        internal void PriceListReportPrinting(DataSet dsPriceListReport)
        {
            try
            {


                crptPricelistDynamic crptDynamic = new crptPricelistDynamic();
                DataTable dtblnew = new DataTable();
                foreach (DataTable dtbl in dsPriceListReport.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptDynamic.Database.Tables["dtblCompanyReport"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        crptDynamic.Database.Tables["dtblPriceListGridFill"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table3")
                    {
                        dtblnew = dtbl;
                        crptDynamic.Database.Tables["dtblOptions"].SetDataSource(dtbl);
                    }
                }
                LineObject line11 = ((LineObject)crptDynamic.ReportDefinition.ReportObjects["Line11"]);
                LineObject line12 = ((LineObject)crptDynamic.ReportDefinition.ReportObjects["Line12"]);
                LineObject line14 = ((LineObject)crptDynamic.ReportDefinition.ReportObjects["Line14"]);
                foreach (DataRow dR in dtblnew.Rows)
                {
                    LineObject line01;
                    int i = 1, j = 6;
                    if (dR["PurchaseRate"].ToString() == "True")
                    {
                        crptDynamic.SetParameterValue("field" + i, "Purchase Rate");
                        i++;
                    }
                    else
                    {
                        crptDynamic.SetParameterValue("field" + j, "None");
                        line01 = ((LineObject)crptDynamic.ReportDefinition.ReportObjects["Line0" + j]);
                        line01.ObjectFormat.EnableSuppress = true;
                        line11.Right = int.Parse(line11.Right.ToString()) - 1790;
                        line12.Right = int.Parse(line12.Right.ToString()) - 1790;
                        line14.Right = int.Parse(line14.Right.ToString()) - 1790;
                        j--;
                    }
                    if (dR["SalesRate"].ToString() == "True")
                    {
                        crptDynamic.SetParameterValue("field" + i, "Sales Rate");
                        i++;
                    }
                    else
                    {
                        crptDynamic.SetParameterValue("field" + j, "None");
                        line01 = ((LineObject)crptDynamic.ReportDefinition.ReportObjects["Line0" + j]);
                        line01.ObjectFormat.EnableSuppress = true;
                        line11.Right = int.Parse(line11.Right.ToString()) - 1790;
                        line12.Right = int.Parse(line12.Right.ToString()) - 1790;
                        line14.Right = int.Parse(line14.Right.ToString()) - 1790;
                        j--;
                    }
                    if (dR["LastSalesRate"].ToString() == "True")
                    {
                        crptDynamic.SetParameterValue("field" + i, "Last Sales Rate");
                        i++;
                    }
                    else
                    {
                        crptDynamic.SetParameterValue("field" + j, "None");
                        line01 = ((LineObject)crptDynamic.ReportDefinition.ReportObjects["Line0" + j]);
                        line01.ObjectFormat.EnableSuppress = true;
                        line11.Right = int.Parse(line11.Right.ToString()) - 1790;
                        line12.Right = int.Parse(line12.Right.ToString()) - 1790;
                        line14.Right = int.Parse(line14.Right.ToString()) - 1790;
                        j--;
                    }
                    if (dR["StandardRate"].ToString() == "True")
                    {
                        crptDynamic.SetParameterValue("field" + i, "Standard Rate");
                        i++;
                    }
                    else
                    {
                        crptDynamic.SetParameterValue("field" + j, "None");
                        line01 = ((LineObject)crptDynamic.ReportDefinition.ReportObjects["Line0" + j]);
                        line01.ObjectFormat.EnableSuppress = true;
                        line11.Right = int.Parse(line11.Right.ToString()) - 1790;
                        line12.Right = int.Parse(line12.Right.ToString()) - 1790;
                        line14.Right = int.Parse(line14.Right.ToString()) - 1790;
                        j--;
                    }
                    if (dR["MRP"].ToString() == "True")
                    {
                        crptDynamic.SetParameterValue("field" + i, "MRP");
                        i++;
                    }
                    else
                    {
                        crptDynamic.SetParameterValue("field" + j, "None");
                        line01 = ((LineObject)crptDynamic.ReportDefinition.ReportObjects["Line0" + j]);
                        line01.ObjectFormat.EnableSuppress = true;
                        line11.Right = int.Parse(line11.Right.ToString()) - 1790;
                        line12.Right = int.Parse(line12.Right.ToString()) - 1790;
                        line14.Right = int.Parse(line14.Right.ToString()) - 1790;
                        j--;
                    }
                    if (dR["Price"].ToString() == "True")
                    {
                        crptDynamic.SetParameterValue("field" + i, "Price");
                        i++;
                    }
                    else
                    {
                        crptDynamic.SetParameterValue("field" + j, "None");
                        line01 = ((LineObject)crptDynamic.ReportDefinition.ReportObjects["Line0" + j]);
                        line01.ObjectFormat.EnableSuppress = true;
                        line11.Right = int.Parse(line11.Right.ToString()) - 1790;
                        line12.Right = int.Parse(line12.Right.ToString()) - 1790;
                        line14.Right = int.Parse(line14.Right.ToString()) - 1790;
                        j--;
                    }
                }
                this.crptViewer.ReportSource = crptDynamic;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptDynamic.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV25 " + ex.Message;
            }

        }
        /// <summary>
        /// Function to print RejectionOut in curresponding Crystal report
        /// </summary>
        /// <param name="dsRejectionOut"></param>
        internal void RejectionOutPrinting(DataSet dsRejectionOut)
        {

            try
            {

                crptRejectionOut crptRejectionOutObj = new crptRejectionOut();
                decimal decTotalAmount = 0;
                foreach (DataTable dtbl in dsRejectionOut.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptRejectionOutObj.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        foreach (DataRow drow in dtbl.Rows)
                        {
                            DataColumn dtClmn = new DataColumn("AmountInWords");
                            dtbl.Columns.Add(dtClmn);
                            if (drow["TotalAmount"].ToString() != string.Empty)
                            {
                                decTotalAmount = Convert.ToDecimal(drow["TotalAmount"].ToString());
                                drow["AmountInWords"] = new NumToText().AmountWords(decTotalAmount, PublicVariables._decCurrencyId);
                            }
                            crptRejectionOutObj.Database.Tables["dtblRejectionOutMaster"].SetDataSource(dtbl);
                        }
                    }
                    else if (dtbl.TableName == "Table2")
                    {

                        crptRejectionOutObj.Database.Tables["dtblRejectionOutDetails"].SetDataSource(dtbl);

                    }
                }
                this.crptViewer.ReportSource = crptRejectionOutObj;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptRejectionOutObj.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV26 " + ex.Message;
            }

        }
        /// <summary>
        /// Function to print RejectionOutReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsRejectionOutReport"></param>
        internal void RejectionOutReportPrinting(DataSet dsRejectionOutReport)
        {
            try
            {
                crptRejectionOutReport crptRejectionOutReportObj = new crptRejectionOutReport();
                foreach (DataTable dtbl in dsRejectionOutReport.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptRejectionOutReportObj.Database.Tables["dtblCompanyReport"].SetDataSource(dtbl);

                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        crptRejectionOutReportObj.Database.Tables["dtblDetails"].SetDataSource(dtbl);
                    }

                }
                this.crptViewer.ReportSource = crptRejectionOutReportObj;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptRejectionOutReportObj.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV27 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to print BonusDeductionReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsBonusDeductionReport"></param>
        internal void BonusDeductionReportPrinting(DataSet dsBonusDeductionReport)
        {
            try
            {
                crptBonusAndDeduction crptBonusAndDeduction = new CrystalReports.Reports.crptBonusAndDeduction();


                foreach (DataTable dtbl in dsBonusDeductionReport.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptBonusAndDeduction.Database.Tables["dtblCompanyDetails"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        crptBonusAndDeduction.Database.Tables["dtblBonusAndDeductionReport"].SetDataSource(dtbl);
                    }
                }
                this.crptViewer.ReportSource = crptBonusAndDeduction;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptBonusAndDeduction.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV28 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to print EmployeeReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsEmployeeReport"></param>
        internal void EmployeeReportPrinting(DataSet dsEmployeeReport)
        {
            try
            {
                crptEmployeeReport crptEmployeeReport = new crptEmployeeReport();

                foreach (DataTable dtbl in dsEmployeeReport.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptEmployeeReport.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        crptEmployeeReport.Database.Tables["dtblEmployee"].SetDataSource(dtbl);
                    }
                }
                this.crptViewer.ReportSource = crptEmployeeReport;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptEmployeeReport.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV29 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to print MonthlySalaryReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsMonthlySalaryReport"></param>
        internal void MonthlySalaryReportPrinting(DataSet dsMonthlySalaryReport)
        {
            try
            {
                crptMonthlySalaryReport crptMonthlySalaryReport = new crptMonthlySalaryReport();

                foreach (DataTable dtbl in dsMonthlySalaryReport.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptMonthlySalaryReport.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        crptMonthlySalaryReport.Database.Tables["dtblMonthlySalary"].SetDataSource(dtbl);
                    }
                }
                this.crptViewer.ReportSource = crptMonthlySalaryReport;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptMonthlySalaryReport.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV30 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to print SalaryPackageReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsSalaryPackageReport"></param>
        internal void SalaryPackageReport(DataSet dsSalaryPackageReport)
        {
            try
            {
                crptSalaryPackageReport crptsalaryPackageReport = new crptSalaryPackageReport();

                foreach (DataTable dtbl in dsSalaryPackageReport.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptsalaryPackageReport.Database.Tables["dtblSalaryPackageReport"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {

                        crptsalaryPackageReport.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                }
                this.crptViewer.ReportSource = crptsalaryPackageReport;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptsalaryPackageReport.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV31 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to print PayHeadReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsPayHeadReport"></param>
        internal void PayHeadReport(DataSet dsPayHeadReport)
        {
            try
            {
                crptPayHeadReport crptpayHeadReport = new crptPayHeadReport();

                foreach (DataTable dtbl in dsPayHeadReport.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptpayHeadReport.Database.Tables["dtblPayHeadReport"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {

                        crptpayHeadReport.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                }
                this.crptViewer.ReportSource = crptpayHeadReport;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptpayHeadReport.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV32 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to print AdvancePaymentReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsAdvancePaymentReport"></param>
        internal void AdvancePaymentReportPrinting(DataSet dsAdvancePaymentReport)
        {
            try
            {
                crptAdvancePayment crptAdvancePayment = new crptAdvancePayment();



                foreach (DataTable dtbl in dsAdvancePaymentReport.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptAdvancePayment.Database.Tables["dtblCompanyDetails"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        crptAdvancePayment.Database.Tables["dtblAdvancePayment"].SetDataSource(dtbl);
                    }
                }
                this.crptViewer.ReportSource = crptAdvancePayment;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptAdvancePayment.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV33 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to print  SalaryPackageDetailsReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsSalaryPakegedetailReport"></param>
        internal void SalaryPackageDetailsReport(DataSet dsSalaryPakegedetailReport)
        {
            try
            {


                crptSalaryPackageDetails crptsalaryPackageDetails = new crptSalaryPackageDetails();

                foreach (DataTable dtbl in dsSalaryPakegedetailReport.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptsalaryPackageDetails.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        crptsalaryPackageDetails.Database.Tables["dtblSalaryPackageDetails"].SetDataSource(dtbl);
                    }
                    else
                    {
                        crptsalaryPackageDetails.Database.Tables["dtblOtherDetails"].SetDataSource(dtbl);
                    }
                }
                this.crptViewer.ReportSource = crptsalaryPackageDetails;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptsalaryPackageDetails.PrintToPrinter(1, false, 0, 0);
                }
            }

            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV34 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to print  JournalreportReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsJournalreport"></param>
        public void JournalreportReportPrinting(DataSet dsJournalreport)
        {
            try
            {
                crptJournalReport crptJournalReport = new crptJournalReport();

                foreach (DataTable dtbl in dsJournalreport.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptJournalReport.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        crptJournalReport.Database.Tables["dtblJournalReport"].SetDataSource(dtbl);
                    }
                }
                this.crptViewer.ReportSource = crptJournalReport;

                SettingsSP spSettings = new SettingsSP();
                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptJournalReport.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV35 " + ex.Message;

            }
        }
        /// <summary>
        /// Function to print  PurchaseOrderReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsPurchaseOrderReport"></param>
        /// <param name="strTotal"></param>
        public void PurchaseOrderReportPrinting(DataSet dsPurchaseOrderReport, string strTotal)
        {
            try
            {
                DataTable dtblTotalAmount = new DataTable();
                dtblTotalAmount.Columns.Add("Grandtotal", typeof(string));
                DataRow dr = dtblTotalAmount.NewRow();
                dr[0] = strTotal;
                dtblTotalAmount.Rows.InsertAt(dr, 0);
                crptPurchaseOrderReport1 crptPurchaseOrderReport1 = new crptPurchaseOrderReport1();
                foreach (DataTable dtbl in dsPurchaseOrderReport.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptPurchaseOrderReport1.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {

                        crptPurchaseOrderReport1.Database.Tables["dtblPurchaseOrderReport"].SetDataSource(dtbl);
                    }
                }
                crptPurchaseOrderReport1.Database.Tables["dtblTotal"].SetDataSource(dtblTotalAmount);
                this.crptViewer.ReportSource = crptPurchaseOrderReport1;
                SettingsSP spSettings = new SettingsSP();
                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptPurchaseOrderReport1.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV36 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to print  PhysicalStockReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsPhysicalStock"></param>
        internal void PhysicalStockReport(DataSet dsPhysicalStock)
        {
            try
            {
                crptPhysicalStockReport crptPhysicalStockReport = new crptPhysicalStockReport();
                foreach (DataTable dtbl in dsPhysicalStock.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptPhysicalStockReport.Database.Tables["dtblCompanyDetails"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {

                        crptPhysicalStockReport.Database.Tables["dtblPhysicalStockReport"].SetDataSource(dtbl);
                    }
                }
                this.crptViewer.ReportSource = crptPhysicalStockReport;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptPhysicalStockReport.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV37 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to print  ContraVoucherReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsContraVoucher"></param>
        internal void ContraVoucherReport(DataSet dsContraVoucher)
        {
            try
            {
                crptContraVoucherReport crptContrReport = new crptContraVoucherReport();

                foreach (DataTable dtbl in dsContraVoucher.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptContrReport.Database.Tables["dtblCompanyDetails"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {

                        crptContrReport.Database.Tables["dtblContraVoucherReport"].SetDataSource(dtbl);
                    }
                }
                this.crptViewer.ReportSource = crptContrReport;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptContrReport.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV38 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to print  ServiceVoucherReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsServiceVoucherReport"></param>
        internal void ServiceVoucherReport(DataSet dsServiceVoucherReport)
        {
            try
            {
                crptServiceVoucherReport crptServiceVoucherReportObj = new crptServiceVoucherReport();

                foreach (DataTable dtbl in dsServiceVoucherReport.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptServiceVoucherReportObj.Database.Tables["dtblCompanyDetails"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        crptServiceVoucherReportObj.Database.Tables["dtblServiceVoucherReport"].SetDataSource(dtbl);
                    }
                }

                this.crptViewer.ReportSource = crptServiceVoucherReportObj;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptServiceVoucherReportObj.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV39 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to print  DebitNote in curresponding Crystal report
        /// </summary>
        /// <param name="dsDebitNote"></param>
        internal void DebitNotePrinting(DataSet dsDebitNote)
        {
            try
            {

                crptDebitNote crptDebitNote = new crptDebitNote();
                decimal decTotalAmount = 0;
                foreach (DataTable dtbl in dsDebitNote.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptDebitNote.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {

                        foreach (DataRow drow in dtbl.Rows)
                        {
                            DataColumn dtClmn = new DataColumn("Amount In Words");
                            dtbl.Columns.Add(dtClmn);
                            if (drow["totalAmount"].ToString() != string.Empty)
                            {
                                decTotalAmount = Convert.ToDecimal(drow["totalAmount"].ToString());
                                drow["Amount In Words"] = new NumToText().AmountWords(decTotalAmount, PublicVariables._decCurrencyId);
                            }
                            crptDebitNote.Database.Tables["dtblDebitNoteMaster"].SetDataSource(dtbl);
                        }
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        DataColumn dtClmn = new DataColumn("CreditOrDebit");
                        dtbl.Columns.Add(dtClmn);
                        DataColumn dtClmn1 = new DataColumn("Amount");
                        dtbl.Columns.Add(dtClmn1);
                        foreach (DataRow drow in dtbl.Rows)
                        {

                            if (drow["chequeNo"].ToString() == string.Empty)
                            {

                                drow["chequeDate"] = string.Empty;
                            }


                            if (Convert.ToDecimal(drow["debit"].ToString()) == 0)
                            {
                                drow["Amount"] = Convert.ToDecimal(drow["credit"].ToString());
                                drow["CreditOrDebit"] = "Cr";
                            }
                            else
                            {
                                drow["Amount"] = Convert.ToDecimal(drow["debit"].ToString());
                                drow["CreditOrDebit"] = "Dr";
                            }

                        }
                        crptDebitNote.Database.Tables["dtblDebitNoteDetails"].SetDataSource(dtbl);
                    }

                }
                this.crptViewer.ReportSource = crptDebitNote;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptDebitNote.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV40 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print  DebitNoteReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsDebitNoteReport"></param>
        public void DebitNoteReportPrinting(DataSet dsDebitNoteReport)
        {
            try
            {
                crptDebitNoteReport crptDebitNoteReport = new crptDebitNoteReport();

                foreach (DataTable dtbl in dsDebitNoteReport.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptDebitNoteReport.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        crptDebitNoteReport.Database.Tables["dtblDebitNoteReport"].SetDataSource(dtbl);
                    }
                }
                this.crptViewer.ReportSource = crptDebitNoteReport;

                SettingsSP spSettings = new SettingsSP();
                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptDebitNoteReport.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV41 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print  CreditNote in curresponding Crystal report
        /// </summary>
        /// <param name="dsCreditNote"></param>
        internal void CreditNotePrinting(DataSet dsCreditNote)
        {
            try
            {

                crptCreditNote crptCreditNote = new crptCreditNote();
                decimal decTotalAmount = 0;
                foreach (DataTable dtbl in dsCreditNote.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptCreditNote.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {

                        foreach (DataRow drow in dtbl.Rows)
                        {
                            DataColumn dtClmn = new DataColumn("Amount In Words");
                            dtbl.Columns.Add(dtClmn);
                            if (drow["totalAmount"].ToString() != string.Empty)
                            {
                                decTotalAmount = Convert.ToDecimal(drow["totalAmount"].ToString());
                                drow["Amount In Words"] = new NumToText().AmountWords(decTotalAmount, PublicVariables._decCurrencyId);
                            }
                            crptCreditNote.Database.Tables["dtblCredittNoteMaster"].SetDataSource(dtbl);
                        }
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        DataColumn dtClmn = new DataColumn("CreditOrDebit");
                        dtbl.Columns.Add(dtClmn);
                        DataColumn dtClmn1 = new DataColumn("Amount");
                        dtbl.Columns.Add(dtClmn1);
                        foreach (DataRow drow in dtbl.Rows)
                        {

                            if (drow["chequeNo"].ToString() == string.Empty)
                            {

                                drow["chequeDate"] = string.Empty;
                            }


                            if (Convert.ToDecimal(drow["debit"].ToString()) == 0)
                            {
                                drow["Amount"] = Convert.ToDecimal(drow["credit"].ToString());
                                drow["CreditOrDebit"] = "Cr";
                            }
                            else
                            {
                                drow["Amount"] = Convert.ToDecimal(drow["debit"].ToString());
                                drow["CreditOrDebit"] = "Dr";
                            }

                        }
                        crptCreditNote.Database.Tables["dtblCreditNoteDetails"].SetDataSource(dtbl);
                    }

                }
                this.crptViewer.ReportSource = crptCreditNote;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptCreditNote.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV42 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print  CreditNoteReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsCreditNoteReport"></param>
        public void CreditNoteReportPrinting(DataSet dsCreditNoteReport)
        {
            try
            {
                crptCreditNoteReport crptCreditNoteReport = new crptCreditNoteReport();


                foreach (DataTable dtbl in dsCreditNoteReport.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptCreditNoteReport.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        crptCreditNoteReport.Database.Tables["dtblCreditNoteReport"].SetDataSource(dtbl);
                    }
                }
                this.crptViewer.ReportSource = crptCreditNoteReport;

                SettingsSP spSettings = new SettingsSP();
                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptCreditNoteReport.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV43 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print  PDCpayableReport in curresponding Crystal report
        /// </summary>
        /// <param name="dspdcPayableReport"></param>
        internal void PDCpayableReportPrinting(DataSet dspdcPayableReport)
        {
            try
            {

                crptPdcPayable crptpdcpayable = new crptPdcPayable();

                foreach (DataTable dtbl in dspdcPayableReport.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptpdcpayable.Database.Tables["dtblCompany"].SetDataSource(dtbl);

                    }
                    else if (dtbl.TableName == "Table1")
                    {

                        crptpdcpayable.Database.Tables["dtblOtherDetails"].SetDataSource(dtbl);


                    }


                }

                this.crptViewer.ReportSource = crptpdcpayable;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptpdcpayable.PrintToPrinter(1, false, 0, 0);

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV44" + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print  RejectionIn in curresponding Crystal report
        /// </summary>
        /// <param name="dsRejectionIn"></param>
        internal void RejectionInPrinting(DataSet dsRejectionIn)
        {
            try
            {
                decimal decTotalAmount = 0;
                crptRejectionIn crptRejectionInObj = new crptRejectionIn();

                foreach (DataTable dtbl in dsRejectionIn.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptRejectionInObj.Database.Tables["dtblCompanyDetails"].SetDataSource(dtbl);

                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        foreach (DataRow drow in dtbl.Rows)
                        {
                            DataColumn dtClmn = new DataColumn("amountInWords");
                            dtbl.Columns.Add(dtClmn);
                            if (drow["totalAmount"].ToString() != string.Empty)
                            {
                                decTotalAmount = Convert.ToDecimal(drow["totalAmount"].ToString());
                                drow["amountInWords"] = new NumToText().AmountWords(decTotalAmount, PublicVariables._decCurrencyId);
                            }
                            crptRejectionInObj.Database.Tables["dtblRejectionInMaster"].SetDataSource(dtbl);
                        }
                        crptRejectionInObj.Database.Tables["dtblRejectionInMaster"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        crptRejectionInObj.Database.Tables["dtblRejectionInDetails"].SetDataSource(dtbl);
                    }
                }

                this.crptViewer.ReportSource = crptRejectionInObj;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptRejectionInObj.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV45 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print  RejectionInReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsRejectionInReport"></param>
        internal void RejectionInReportPrinting(DataSet dsRejectionInReport)
        {
            try
            {
                crptRejectionInReport crptRejectionInReportObj = new crptRejectionInReport();

                foreach (DataTable dtbl in dsRejectionInReport.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptRejectionInReportObj.Database.Tables["dtblCompanyDetails"].SetDataSource(dtbl);

                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        dtbl.Columns.Add("slNo");
                        int inSlNo = 1;
                        foreach (DataRow drdtbl in dtbl.Rows)
                        {
                            drdtbl["slNo"] = inSlNo++;
                        }
                        crptRejectionInReportObj.Database.Tables["dtblRejectionInMaster"].SetDataSource(dtbl);
                    }

                }
                this.crptViewer.ReportSource = crptRejectionInReportObj;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptRejectionInReportObj.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV46 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print  DeliveryNote in curresponding Crystal report
        /// </summary>
        /// <param name="dsDeliveryNote"></param>
        internal void DeliveryNotePrinting(DataSet dsDeliveryNote)
        {
            try
            {
                crptDeliveryNote crptDeliveryNoteObj = new crptDeliveryNote();
                decimal decTotalAmount = 0;
                foreach (DataTable dtbl in dsDeliveryNote.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptDeliveryNoteObj.Database.Tables["dtblCompany"].SetDataSource(dtbl);

                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        
                        foreach (DataRow drow in dtbl.Rows)
                        {
                            DataColumn dtClmn = new DataColumn("amountInWords");
                            dtbl.Columns.Add(dtClmn);
                            if (drow["totalAmount"].ToString() != string.Empty)
                            {
                                decTotalAmount = Convert.ToDecimal(drow["totalAmount"].ToString());
                                drow["amountInWords"] = new NumToText().AmountWords(decTotalAmount, PublicVariables._decCurrencyId);
                            }
                            crptDeliveryNoteObj.Database.Tables["dtblDeliveryNoteMster"].SetDataSource(dtbl);
                        }
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        crptDeliveryNoteObj.Database.Tables["dtblDeliveryNoteDetails"].SetDataSource(dtbl);
                    }
                }
                this.crptViewer.ReportSource = crptDeliveryNoteObj;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptDeliveryNoteObj.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV47 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print  PurchaseReturnReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsPurchaseReturnReportPrinting"></param>
        /// <param name="strTotalAmount"></param>
        internal void PurchaseReturnReportPrinting(DataSet dsPurchaseReturnReportPrinting, string strTotalAmount)
        {
            try
            {
                DataTable dtblGrandTotal = new DataTable();
                dtblGrandTotal.Columns.Add("GrandTotal", typeof(string));
                DataRow dr = dtblGrandTotal.NewRow();
                dr[0] = strTotalAmount;
                dtblGrandTotal.Rows.InsertAt(dr, 0);
                crptPurchaseReturnReport crptPurchaseReturnReportObj = new crptPurchaseReturnReport();
                foreach (DataTable dtbl in dsPurchaseReturnReportPrinting.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptPurchaseReturnReportObj.Database.Tables["dtblPurchaseReturnMaster"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        crptPurchaseReturnReportObj.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                  crptPurchaseReturnReportObj.Database.Tables["dtblGrandTotal"].SetDataSource(dtblGrandTotal);
                }
                this.crptViewer.ReportSource = crptPurchaseReturnReportObj;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptPurchaseReturnReportObj.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV48 " + ex.Message;
            }

        }
        /// <summary>
        ///   Function to print  SalesReturnReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsSalesReturnReportPrinting"></param>
        internal void SalesReturnReportPrinting(DataSet dsSalesReturnReportPrinting)
        {
            try
            {
                decimal decTotalAmount = 0;
                crptSalesReturnReporting crptSalesReturnReportObj = new crptSalesReturnReporting();
                foreach (DataTable dtbl in dsSalesReturnReportPrinting.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptSalesReturnReportObj.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        crptSalesReturnReportObj.Database.Tables["dtblSalesReturnMaster"].SetDataSource(dtbl);

                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        foreach (DataRow drow in dtbl.Rows)
                        {
                            DataColumn dtClmn = new DataColumn("amountInWords");
                            dtbl.Columns.Add(dtClmn);
                            if (drow["GrandTotal"].ToString() != string.Empty)
                            {
                                decTotalAmount = Convert.ToDecimal(drow["GrandTotal"].ToString());
                                drow["amountInWords"] = new NumToText().AmountWords(decTotalAmount, PublicVariables._decCurrencyId);
                            }
                            crptSalesReturnReportObj.Database.Tables["dtblSalesReturnMasterGrandTotal"].SetDataSource(dtbl);
                        }
                    }
                }
                this.crptViewer.ReportSource = crptSalesReturnReportObj;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptSalesReturnReportObj.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV49" + ex.Message;
            }

        }
        /// <summary>
        ///   Function to print  SalesReturn in curresponding Crystal report
        /// </summary>
        /// <param name="dsSalesReturnPrinting"></param>
        internal void SalesReturnPrinting(DataSet dsSalesReturnPrinting)
        {
            try
            {
                crptSalesReturn crptSalesReturnobj = new crptSalesReturn();
                decimal decTotalAmount = 0;
                foreach (DataTable dtbl in dsSalesReturnPrinting.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptSalesReturnobj.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        foreach (DataRow drow in dtbl.Rows)
                        {
                            DataColumn dtClmn = new DataColumn("amountInWords");
                            dtbl.Columns.Add(dtClmn);
                            if (drow["grandTotal"].ToString() != string.Empty)
                            {
                                decTotalAmount = Convert.ToDecimal(drow["grandTotal"].ToString());
                                drow["amountInWords"] = new NumToText().AmountWords(decTotalAmount, PublicVariables._decCurrencyId);
                            }
                            crptSalesReturnobj.Database.Tables["dtblSalesReturnMaster"].SetDataSource(dtbl);
                        }
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        crptSalesReturnobj.Database.Tables["dtblSalesReturnDetails"].SetDataSource(dtbl);

                    }

                }
                this.crptViewer.ReportSource = crptSalesReturnobj;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptSalesReturnobj.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV50 " + ex.Message;
            }

        }
        /// <summary>
        ///   Function to print  PurchaseReturn in curresponding Crystal report
        /// </summary>
        /// <param name="dsPurchaseReturnReport"></param>
        internal void PurchaseReturnPrinting(DataSet dsPurchaseReturnReport)
        {
            try
            {
                crptPurchaseReturn crptPurchaseReturnObj = new crptPurchaseReturn();
                decimal decGrandAmount = 0;
                foreach (DataTable dtbl in dsPurchaseReturnReport.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptPurchaseReturnObj.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {
                       
                        foreach (DataRow drow in dtbl.Rows)
                        {
                            DataColumn dtClmn = new DataColumn("amountInWords");
                            dtbl.Columns.Add(dtClmn);
                            if (drow["grandTotal"].ToString() != string.Empty)
                            {
                                decGrandAmount = Convert.ToDecimal(drow["grandTotal"].ToString());
                                drow["amountInWords"] = new NumToText().AmountWords(decGrandAmount, PublicVariables._decCurrencyId);
                            }
                            crptPurchaseReturnObj.Database.Tables["dtblPurchaseReturnMaster"].SetDataSource(dtbl);
                        }
                    }
                    else
                    {
                        crptPurchaseReturnObj.Database.Tables["dtblPurchaseReturnDetails"].SetDataSource(dtbl);

                    }
                }
                this.crptViewer.ReportSource = crptPurchaseReturnObj;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptPurchaseReturnObj.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV51 " + ex.Message;
            }

        }
        /// <summary>
        ///   Function to print  PdcpayablereportReport in curresponding Crystal report
        /// </summary>
        /// <param name="dspdcpayablereport"></param>
        public void PdcpayablereportReportPrinting(DataSet dspdcpayablereport)
        {
            try
            {
                crptPdcPayableReport crptPdcpayableReport = new crptPdcPayableReport();


                foreach (DataTable dtbl in dspdcpayablereport.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptPdcpayableReport.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        crptPdcpayableReport.Database.Tables["dtblpdcpayableReport"].SetDataSource(dtbl);
                    }
                }
                this.crptViewer.ReportSource = crptPdcpayableReport;

                SettingsSP spSettings = new SettingsSP();
                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptPdcpayableReport.PrintToPrinter(1, false, 0, 0);
                }
            }

            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV52 " + ex.Message;

            }
        }
        /// <summary>
        ///  Function to print  PdcreceivablereportReport in curresponding Crystal report
        /// </summary>
        /// <param name="dspdcpayablereport"></param>
        public void PdcreceivablereportReportPrinting(DataSet dspdcpayablereport)
        {
            try
            {
                crptPDCReceivableReport crptPDCReceivableReport = new crptPDCReceivableReport();


                foreach (DataTable dtbl in dspdcpayablereport.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptPDCReceivableReport.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        crptPDCReceivableReport.Database.Tables["dtblpdcReceivableReport"].SetDataSource(dtbl);
                    }
                }
                this.crptViewer.ReportSource = crptPDCReceivableReport;

                SettingsSP spSettings = new SettingsSP();
                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptPDCReceivableReport.PrintToPrinter(1, false, 0, 0);
                }
            }

            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV53 " + ex.Message;

            }
        }
        /// <summary>
        ///  Function to print  SalesOrderReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsSalesOrderReport"></param>
        /// <param name="strTotal"></param>
        public void SalesOrderReportPrinting(DataSet dsSalesOrderReport, string strTotal)
        {
            try
            {
                DataTable dtblTotalAmount = new DataTable();
                dtblTotalAmount.Columns.Add("Grandtotal", typeof(string));
                DataRow dr = dtblTotalAmount.NewRow();
                dr[0] = strTotal;
                dtblTotalAmount.Rows.InsertAt(dr, 0);

                crptSalesOrderReportPrinting crptSalesOrderReportObj = new crptSalesOrderReportPrinting();
                foreach (DataTable dtbl in dsSalesOrderReport.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptSalesOrderReportObj.Database.Tables["dtblCompanyReport"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {

                        crptSalesOrderReportObj.Database.Tables["dtblSalesOrderReport"].SetDataSource(dtbl);


                    }

                }
                crptSalesOrderReportObj.Database.Tables["dtblTotal"].SetDataSource(dtblTotalAmount);
                this.crptViewer.ReportSource = crptSalesOrderReportObj;

                SettingsSP spSettings = new SettingsSP();
                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptSalesOrderReportObj.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV54 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print  MaterialReceiptReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsMaterialReceiptReport"></param>
        public void MaterialReceiptReportPrinting(DataSet dsMaterialReceiptReport)
        {
            try
            {
                crptMaterialReceiptReport crptMaterialReceiptReportObj = new crptMaterialReceiptReport();

                foreach (DataTable dtbl in dsMaterialReceiptReport.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptMaterialReceiptReportObj.Database.Tables["dtblCompanyReport"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {

                        crptMaterialReceiptReportObj.Database.Tables["dtblMaterialReceiptReport"].SetDataSource(dtbl);


                    }

                }

                this.crptViewer.ReportSource = crptMaterialReceiptReportObj;

                SettingsSP spSettings = new SettingsSP();
                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }

                else
                {
                    crptMaterialReceiptReportObj.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV55 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print  SalesInvoice And POS in curresponding Crystal report
        /// </summary>
        /// <param name="dsSalesInvoiceReport"></param>
        internal void SalesInvoicePrinting(DataSet dsSalesInvoiceReport)
        {
            try
            {
                crptSalesInvoice crptSalesInvoiceObj = new crptSalesInvoice();
                decimal decTotalAmount = 0;
                foreach (DataTable dtbl in dsSalesInvoiceReport.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptSalesInvoiceObj.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        foreach (DataRow drow in dtbl.Rows)
                        {
                            DataColumn dtClmn = new DataColumn("amountInWords");
                            dtbl.Columns.Add(dtClmn);
                            if (drow["grandTotal"].ToString() != string.Empty)
                            {
                                decTotalAmount = Convert.ToDecimal(drow["grandTotal"].ToString());
                                drow["amountInWords"] = new NumToText().AmountWords(decTotalAmount, PublicVariables._decCurrencyId);
                            }
                            crptSalesInvoiceObj.Database.Tables["dtblSalesMaster"].SetDataSource(dtbl);
                        }
                        
                    }
                    else
                    {
                        crptSalesInvoiceObj.Database.Tables["dtblSalesDetails"].SetDataSource(dtbl);

                    }
                }
                this.crptViewer.ReportSource = crptSalesInvoiceObj;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptSalesInvoiceObj.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV56 " + ex.Message;
            }

        }
        /// <summary>
        ///  Function to print  SalesInvoiceReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsSalesInvoiceReport"></param>
        internal void SalesInvoiceReportPrinting(DataSet dsSalesInvoiceReport)
        {
            try
            {
                crptSalesInvoiceReport crptSalesInvoiceReportObj = new crptSalesInvoiceReport();
                foreach (DataTable dtbl in dsSalesInvoiceReport.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptSalesInvoiceReportObj.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        crptSalesInvoiceReportObj.Database.Tables["dtblSalesMaster"].SetDataSource(dtbl);
                    }

                }
                this.crptViewer.ReportSource = crptSalesInvoiceReportObj;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptSalesInvoiceReportObj.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV57 " + ex.Message;
            }

        }
        /// <summary>
        ///  Function to print  DeliveryNoteReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsDeliveryNoteReport"></param>
        public void DeliveryNoteReportPrinting(DataSet dsDeliveryNoteReport)
        {
            try
            {

                crptDeliveryNoteReport crptDeliveryNoteReportObj = new crptDeliveryNoteReport();
                foreach (DataTable dtbl in dsDeliveryNoteReport.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptDeliveryNoteReportObj.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {

                        crptDeliveryNoteReportObj.Database.Tables["dtblDeliveryNoteReport"].SetDataSource(dtbl);


                    }

                }

                this.crptViewer.ReportSource = crptDeliveryNoteReportObj;

                SettingsSP spSettings = new SettingsSP();
                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptDeliveryNoteReportObj.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV58 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print  PDCreceivableVoucher in curresponding Crystal report
        /// </summary>
        /// <param name="dspdcreceivablePrinting"></param>
        internal void PDCreceivableVoucherPrinting(DataSet dspdcreceivablePrinting)
        {
            try
            {
                crptPDCreceivable crptPdcReceivable = new crptPDCreceivable();
                decimal decTotalAmount = 0;


                foreach (DataTable dtbl in dspdcreceivablePrinting.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptPdcReceivable.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        foreach (DataRow drow in dtbl.Rows)
                        {
                            DataColumn dtClmn = new DataColumn("AmountInWords");
                            dtbl.Columns.Add(dtClmn);
                            if (drow["Amount"].ToString() != string.Empty)
                            {
                                decTotalAmount = Convert.ToDecimal(drow["Amount"].ToString());
                                drow["AmountInWords"] = new NumToText().AmountWords(decTotalAmount, PublicVariables._decCurrencyId);
                            }

                            crptPdcReceivable.Database.Tables["dtblOtherDetails"].SetDataSource(dtbl);
                        }





                    }
                }
                this.crptViewer.ReportSource = crptPdcReceivable;

                SettingsSP spSettings = new SettingsSP();
                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptPdcReceivable.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV59" + ex.Message;
            }
        }
       /// <summary>
        ///  Function to print  ChartOfAccounts in curresponding Crystal report
       /// </summary>
       /// <param name="dtblChartOfAccounts"></param>
        internal void PrintChartOfAccounts(DataTable dtblChartOfAccounts)
        {
            try
            {
                crptChartOfAccount crptChartofAccounts = new crptChartOfAccount();
                crptChartofAccounts.Database.Tables["dtblChartOfAccounts"].SetDataSource(dtblChartOfAccounts);
                this.crptViewer.ReportSource = crptChartofAccounts;
                SettingsSP spSettings = new SettingsSP();
                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptChartofAccounts.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV60 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print  PdcReceivablereportReport in curresponding Crystal report
        /// </summary>
        /// <param name="dspdcReceivablereport"></param>
        public void PdcReceivablereportReportPrinting(DataSet dspdcReceivablereport)
        {
            try
            {

                crptPDCReceivableReport crptPdcreport = new crptPDCReceivableReport();



                foreach (DataTable dtbl in dspdcReceivablereport.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptPdcreport.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        crptPdcreport.Database.Tables["dtblpdcreceivableReport"].SetDataSource(dtbl);
                    }
                }
                this.crptViewer.ReportSource = crptPdcreport;

                SettingsSP spSettings = new SettingsSP();
                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptPdcreport.PrintToPrinter(1, false, 0, 0);
                }
            }

            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV61 " + ex.Message;

            }
        }
        /// <summary>
        ///  Function to print  PartyAddressBook in curresponding Crystal report
        /// </summary>
        /// <param name="dsPartyAddressBook"></param>
        public void PartyAddressBookPrint(DataSet dsPartyAddressBook)
        {

            try
            {

                crptPartyAddressBook crptPartyAddressBook = new crptPartyAddressBook();
                foreach (DataTable dtbl in dsPartyAddressBook.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptPartyAddressBook.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        crptPartyAddressBook.Database.Tables["PartyAddressDetails"].SetDataSource(dtbl);
                    }

                }
                this.crptViewer.ReportSource = crptPartyAddressBook;
                SettingsSP spSettings = new SettingsSP();
                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptPartyAddressBook.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {

                formMDI.infoError.ErrorString = "CRV62 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print  ProfitAndLossReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsProfitAndLossReport"></param>
        internal void ProfitAndLossReportPrinting(DataSet dsProfitAndLossReport)
        {
            try
            {
                crptProfitAndLoss crptProfitAndLossReport = new crptProfitAndLoss();



                foreach (DataTable dtbl in dsProfitAndLossReport.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptProfitAndLossReport.Database.Tables["dtblProfit"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        crptProfitAndLossReport.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }

                }

                this.crptViewer.ReportSource = crptProfitAndLossReport;

                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptProfitAndLossReport.PrintToPrinter(1, false, 0, 0);
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV63 " + ex.Message;
            }

        }
        /// <summary>
        ///  Function to print  BalanceSheetReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsAgeing"></param>
        internal void BalanceSheetReportPrint(DataSet dsAgeing)
        {
            try
            {
                crptBalanceSheet crptBalanceSheet = new crptBalanceSheet();

                foreach (DataTable dtbl in dsAgeing.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptBalanceSheet.Database.Tables["dtblBalanceSheet"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        crptBalanceSheet.Database.Tables["dtblCompanyReport1"].SetDataSource(dtbl);
                    }

                }
                this.crptViewer.ReportSource = crptBalanceSheet;

                SettingsSP spSettings = new SettingsSP();
                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptBalanceSheet.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {

                formMDI.infoError.ErrorString = "CRV64 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print  AgeingReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsAgeing"></param>
        internal void AgeingReportPrint(DataSet dsAgeing)
        {
            try
            {

                crptAgeing crptAgeing = new crptAgeing();

                foreach (DataTable dtbl in dsAgeing.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptAgeing.Database.Tables["dtblageing"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        crptAgeing.Database.Tables["dtblCompanyReport1"].SetDataSource(dtbl);
                    }

                }
                this.crptViewer.ReportSource = crptAgeing;

                SettingsSP spSettings = new SettingsSP();
                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptAgeing.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {

                formMDI.infoError.ErrorString = "CRV65 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print  AgeingReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsAgeing"></param>
        internal void AgeingReportPrint1(DataSet dsAgeing)
        {
            try
            {
                crptAgeing1 crptAgeing = new crptAgeing1();

                foreach (DataTable dtbl in dsAgeing.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptAgeing.Database.Tables["dtblageing1"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        crptAgeing.Database.Tables["dtblCompanyReport1"].SetDataSource(dtbl);
                    }

                }
                this.crptViewer.ReportSource = crptAgeing;

                SettingsSP spSettings = new SettingsSP();
                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptAgeing.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {

                formMDI.infoError.ErrorString = "CRV66 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print  ProductStatisticsReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsProductStatistics"></param>
        /// <param name="strLevels"></param>
        internal void ProductStatisticsReport(DataSet dsProductStatistics, string strLevels)
        {
            try
            {
                crptProductStatisticsReport crptProductStatisticsReport = new crptProductStatisticsReport();
                crptProductStatisticsFastMoving crptProductStatisticsFastMoving = new crptProductStatisticsFastMoving();
                crptProductStatisticsMaximumLevel crptProductStatisticsMaximumLevel = new crptProductStatisticsMaximumLevel();
                crptProductStatisticsMinimumLevel crptProductStatisticsMinimumLevel = new crptProductStatisticsMinimumLevel();
                crptProductStatisticsNegativeStock crptProductStatisticsNegativeStock = new crptProductStatisticsNegativeStock();
                crptProductStatisticsReorderLevel crptProductStatisticsReorderLevel = new crptProductStatisticsReorderLevel();
                crptProductStatisticsSlowMoving crptProductStatisticsSlowMoving = new crptProductStatisticsSlowMoving();
                crptProductStatisticsUnUsed crptProductStatisticsUnused = new crptProductStatisticsUnUsed();
                foreach (DataTable dtbl in dsProductStatistics.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        if (strLevels == "Minimum Level")
                        {
                            crptProductStatisticsMinimumLevel.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                        }
                        if (strLevels == "Maximum Level")
                        {
                            crptProductStatisticsMaximumLevel.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                        }
                        if (strLevels == "Negative Stock")
                        {
                            crptProductStatisticsNegativeStock.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                        }
                        if (strLevels == "Reorder Level")
                        {
                            crptProductStatisticsReorderLevel.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                        }
                        if (strLevels == "UnUsed")
                        {
                            crptProductStatisticsUnused.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                        }
                        if (strLevels == "Fast Movings")
                        {
                            crptProductStatisticsFastMoving.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                        }
                        if (strLevels == "Slow Movings")
                        {
                            crptProductStatisticsSlowMoving.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                        }
                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        if (strLevels == "Minimum Level")
                        {
                            crptProductStatisticsMinimumLevel.Database.Tables["dtblProductStatistics"].SetDataSource(dtbl);
                        }
                        if (strLevels == "Maximum Level")
                        {
                            crptProductStatisticsMaximumLevel.Database.Tables["dtblProductStatistics"].SetDataSource(dtbl);
                        }
                        if (strLevels == "Negative Stock")
                        {
                            crptProductStatisticsNegativeStock.Database.Tables["dtblProductStatistics"].SetDataSource(dtbl);
                        }
                        if (strLevels == "Reorder Level")
                        {
                            crptProductStatisticsReorderLevel.Database.Tables["dtblProductStatistics"].SetDataSource(dtbl);
                        }
                        if (strLevels == "UnUsed")
                        {
                            crptProductStatisticsUnused.Database.Tables["dtblProductStatistics"].SetDataSource(dtbl);
                        }
                        if (strLevels == "Fast Movings")
                        {
                            crptProductStatisticsFastMoving.Database.Tables["dtblProductStatistics"].SetDataSource(dtbl);
                        }
                        if (strLevels == "Slow Movings")
                        {
                            crptProductStatisticsSlowMoving.Database.Tables["dtblProductStatistics"].SetDataSource(dtbl);
                        }
                    }

                }
                if (strLevels == "Minimum Level")
                {
                    this.crptViewer.ReportSource = crptProductStatisticsMinimumLevel;
                }
                if (strLevels == "Maximum Level")
                {
                    this.crptViewer.ReportSource = crptProductStatisticsMaximumLevel;
                }
                if (strLevels == "Negative Stock")
                {
                    this.crptViewer.ReportSource = crptProductStatisticsNegativeStock;
                }
                if (strLevels == "Reorder Level")
                {
                    this.crptViewer.ReportSource = crptProductStatisticsReorderLevel;
                }
                if (strLevels == "UnUsed")
                {
                    this.crptViewer.ReportSource = crptProductStatisticsUnused;
                }
                if (strLevels == "Fast Movings")
                {
                    this.crptViewer.ReportSource = crptProductStatisticsFastMoving;
                }
                if (strLevels == "Slow Movings")
                {
                    this.crptViewer.ReportSource = crptProductStatisticsSlowMoving;
                }
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptProductStatisticsReport.PrintToPrinter(1, false, 0, 0);
                }

            }

            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV67 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print  FundFlow in curresponding Crystal report
        /// </summary>
        /// <param name="dsFundFlow"></param>
        internal void FundFlow(DataSet dsFundFlow)
        {
            try
            {

                crptFundFlow crptFundFlow = new crptFundFlow();

                foreach (DataTable dtbl in dsFundFlow.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptFundFlow.Database.Tables["dtblFund"].SetDataSource(dtbl);
                    }
                    if (dtbl.TableName == "Table2")
                    {

                        crptFundFlow.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }

                    if (dtbl.TableName == "Table3")
                    {
                        crptFundFlow.Database.Tables["dtblWC"].SetDataSource(dtbl);
                    }


                }
                this.crptViewer.ReportSource = crptFundFlow;

                SettingsSP spSettings = new SettingsSP();
                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptFundFlow.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV68 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print  Budget in curresponding Crystal report
        /// </summary>
        /// <param name="dsBudget"></param>
        internal void Budget(DataSet dsBudget)
        {
            try
            {

                crptBudget crptBudget = new crptBudget();

                foreach (DataTable dtbl in dsBudget.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptBudget.Database.Tables["dtblBudget"].SetDataSource(dtbl);
                    }
                    if (dtbl.TableName == "Table2")
                    {

                        crptBudget.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }

                }
                this.crptViewer.ReportSource = crptBudget;

                SettingsSP spSettings = new SettingsSP();
                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptBudget.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV69" + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print TrailBalanceReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsTrailReport"></param>
        internal void TrailBalanceReportPrinting(DataSet dsTrailReport)
        {
            try
            {
                crptTrialBalance crpttrail = new crptTrialBalance();
                foreach (DataTable dtbl in dsTrailReport.Tables)
                {
                    if (dtbl.TableName == "Table2")
                    {
                        crpttrail.Database.Tables["dtbl_Company"].SetDataSource(dtbl);

                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        crpttrail.Database.Tables["dtbltrailbal"].SetDataSource(dtbl);
                    }

                }
                this.crptViewer.ReportSource = crpttrail;
                SettingsSP spstting = new SettingsSP();
                if (spstting.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crpttrail.PrintToPrinter(1, false, 0, 0);
                }
            }

            catch (Exception ex)
            {

                formMDI.infoError.ErrorString = "CRV70 " + ex.Message;
            }

        }
        /// <summary>
        ///  Function to print TaxReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsTaxReport"></param>
        /// <param name="isBillwise"></param>
        internal void TaxCrystalReportPrint(DataSet dsTaxReport, bool isBillwise)
        {
            try
            {
                crptTaxReportBillWise crptTaxReportBillWise = new crptTaxReportBillWise();
                crptTaxReportProductWise crptTaxReportProductWise = new crptTaxReportProductWise();

                foreach (DataTable dtbl in dsTaxReport.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        if (isBillwise == true)
                        {
                            crptTaxReportBillWise.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                        }
                        else
                        {
                            crptTaxReportProductWise.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                        }
                    }
                    if (dtbl.TableName == "Table1")
                    {

                        if (isBillwise == true)
                        {
                            crptTaxReportBillWise.Database.Tables["dtblTaxReport"].SetDataSource(dtbl);
                        }
                        else
                        {
                            crptTaxReportProductWise.Database.Tables["dtblTaxReport"].SetDataSource(dtbl);
                        }
                    }

                }
                if (isBillwise == true)
                {
                    this.crptViewer.ReportSource = crptTaxReportBillWise;
                }
                else
                {
                    this.crptViewer.ReportSource = crptTaxReportProductWise;
                }

                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptTaxReportBillWise.PrintToPrinter(1, false, 0, 0);
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV71 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print ProductSearchReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsProductSearch"></param>
        /// <param name="strLevels"></param>
        internal void ProductSearchReport(DataSet dsProductSearch, string strLevels)
        {
            try
            {

                crptProductSearchReport crptProductSearchReport = new crptProductSearchReport();
                crptProductSearchMinimumStock crptProductSearchMinimumStock = new crptProductSearchMinimumStock();
                crptProductSearchMaximum crptProductSearchMaximum = new crptProductSearchMaximum();
                crptProductSearchNegativeStock crptProductSearchNegativeStock = new crptProductSearchNegativeStock();
                crptProductSearchReorderStock crptProductSearchReorderStock = new crptProductSearchReorderStock();

                foreach (DataTable dtbl in dsProductSearch.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        if (strLevels == "All")
                        {
                            crptProductSearchReport.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                        }
                        if (strLevels == "Minimum Level")
                        {
                            crptProductSearchMinimumStock.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                        }
                        if (strLevels == "Maximum Level")
                        {
                            crptProductSearchMaximum.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                        }
                        if (strLevels == "Negative Stock")
                        {
                            crptProductSearchNegativeStock.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                        }
                        if (strLevels == "Reorder Level")
                        {
                            crptProductSearchReorderStock.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                        }
                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        if (strLevels == "All")
                        {
                            crptProductSearchReport.Database.Tables["dtblProductSearch"].SetDataSource(dtbl);
                        }
                        if (strLevels == "Minimum Level")
                        {
                            crptProductSearchMinimumStock.Database.Tables["dtblProductSearch"].SetDataSource(dtbl);
                        }
                        if (strLevels == "Maximum Level")
                        {
                            crptProductSearchMaximum.Database.Tables["dtblProductSearch"].SetDataSource(dtbl);
                        }
                        if (strLevels == "Negative Stock")
                        {
                            crptProductSearchNegativeStock.Database.Tables["dtblProductSearch"].SetDataSource(dtbl);
                        }
                        if (strLevels == "Reorder Level")
                        {
                            crptProductSearchReorderStock.Database.Tables["dtblProductSearch"].SetDataSource(dtbl);
                        }
                    }

                }
                if (strLevels == "All")
                {
                    this.crptViewer.ReportSource = crptProductSearchReport;
                }
                if (strLevels == "Minimum Level")
                {
                    this.crptViewer.ReportSource = crptProductSearchMinimumStock;
                }
                if (strLevels == "Maximum Level")
                {
                    this.crptViewer.ReportSource = crptProductSearchMaximum;
                }
                if (strLevels == "Negative Stock")
                {
                    this.crptViewer.ReportSource = crptProductSearchNegativeStock;
                }
                if (strLevels == "Reorder Level")
                {
                    this.crptViewer.ReportSource = crptProductSearchReorderStock;
                }
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptProductSearchReport.PrintToPrinter(1, false, 0, 0);
                }

            }

            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV72 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print CashBankBook in curresponding Crystal report
        /// </summary>
        /// <param name="dsCashBankBook"></param>
        internal void CashBankBookPrinting(DataSet dsCashBankBook)
        {
            try
            {

                crptCashBankBookReport crptCashbank = new crptCashBankBookReport();
                foreach (DataTable dtbl in dsCashBankBook.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptCashbank.Database.Tables["dtblCompany"].SetDataSource(dtbl);

                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        crptCashbank.Database.Tables["dtblCashBankBookDetails"].SetDataSource(dtbl);
                    }

                }
                this.crptViewer.ReportSource = crptCashbank;
                SettingsSP spstting = new SettingsSP();
                if (spstting.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptCashbank.PrintToPrinter(1, false, 0, 0);
                }
            }

            catch (Exception ex)
            {

                formMDI.infoError.ErrorString = "CRV73 " + ex.Message;
            }

        }
        /// <summary>
        ///  Function to print AccountLedgerReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsAccountLedgerReport"></param>
        internal void AccountLedgerReportPrinting(DataSet dsAccountLedgerReport)
        {

            try
            {
                crptAccountLedgerReport crptAccountledgerReportObj = new crptAccountLedgerReport();
                foreach (DataTable dtbl in dsAccountLedgerReport.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptAccountledgerReportObj.Database.Tables["dtblCompanyDetails"].SetDataSource(dtbl);

                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        crptAccountledgerReportObj.Database.Tables["dtblAccountLedgerReport"].SetDataSource(dtbl);

                    }

                }
                this.crptViewer.ReportSource = crptAccountledgerReportObj;

                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptAccountledgerReportObj.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV74 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print ShortExpiryReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsShortExpiryReport"></param>
        internal void ShortExpiryReportPrinting(DataSet dsShortExpiryReport)
        {
            try
            {
                crptShortExpiryReport crptShortExpiryReportObj = new crptShortExpiryReport();

                foreach (DataTable dtbl in dsShortExpiryReport.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptShortExpiryReportObj.Database.Tables["dtblCompany"].SetDataSource(dtbl);

                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        dtbl.Columns.Add("slNo");
                        int inSlNo = 1;
                        foreach (DataRow drdtbl in dtbl.Rows)
                        {
                            drdtbl["slNo"] = inSlNo++;
                        }
                        crptShortExpiryReportObj.Database.Tables["dtblShortExpiryGrid"].SetDataSource(dtbl);
                    }

                }
                this.crptViewer.ReportSource = crptShortExpiryReportObj;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptShortExpiryReportObj.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV75 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print CashflowReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsCashFlowReport"></param>
        internal void CashflowReportPrinting(DataSet dsCashFlowReport)
        {
            try
            {
                crptCashFlow crptCashFlow = new crptCashFlow();
                foreach (DataTable dtbl in dsCashFlowReport.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptCashFlow.Database.Tables["dtblCashflow"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        crptCashFlow.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }

                }

                this.crptViewer.ReportSource = crptCashFlow;

                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptCashFlow.PrintToPrinter(1, false, 0, 0);
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV76 " + ex.Message;
            }

        }
        /// <summary>
        ///  Function to print OutstandingReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsOutstandingPrinting"></param>
        internal void OutstandingPrinting(DataSet dsOutstandingPrinting)
        {
            try
            {

                crptOutstanding crptOutstandingObj = new crptOutstanding();
                foreach (DataTable dtbl in dsOutstandingPrinting.Tables)
                {
                    if (dtbl.TableName == "Table2")
                    {
                        crptOutstandingObj.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        crptOutstandingObj.Database.Tables["dtblOutStanding"].SetDataSource(dtbl);
                    }

                }
                this.crptViewer.ReportSource = crptOutstandingObj;



                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptOutstandingObj.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV77 " + ex.Message;
            }

        }
        /// <summary>
        ///  Function to print dayBookReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsDayBookReport"></param>
 
        internal void dayBookReportPrintingDetailed(DataSet dsDayBookReport)
        {

            try
            {
                crptDayBookReportDetailed crptDayBookReportDetailed = new crptDayBookReportDetailed();

                foreach (DataTable dtbl in dsDayBookReport.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptDayBookReportDetailed.Database.Tables["dtblDayBook"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        crptDayBookReportDetailed.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                }
                this.crptViewer.ReportSource = crptDayBookReportDetailed;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptDayBookReportDetailed.PrintToPrinter(1, false, 0, 0);
                }

            }
            catch (Exception ex)
            {

                formMDI.infoError.ErrorString = "CRV78 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print dayBookReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsDayBookReportCondensed"></param>
        internal void dayBookReportPrintingCondensed(DataSet dsDayBookReportCondensed)
        {

            try
            {
                crptDayBookReportCondenced crptDaybookReportCondensed = new crptDayBookReportCondenced();

                foreach (DataTable dtbl in dsDayBookReportCondensed.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptDaybookReportCondensed.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        crptDaybookReportCondensed.Database.Tables["dtblDayBook"].SetDataSource(dtbl);
                    }
                }
                this.crptViewer.ReportSource = crptDaybookReportCondensed;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptDaybookReportCondensed.PrintToPrinter(1, false, 0, 0);
                }

            }
            catch (Exception ex)
            {

                formMDI.infoError.ErrorString = "CRV79 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print StockReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsStockReport"></param>
        /// <param name="strGrandTotal"></param>
        internal void StockReportPrinting(DataSet dsStockReport, string strGrandTotal)
        {
            try
            {
                DataTable dtblGrandTotal = new DataTable();
                dtblGrandTotal.Columns.Add("Total", typeof(string));
                DataRow dr = dtblGrandTotal.NewRow();
                dr[0] = strGrandTotal;
                dtblGrandTotal.Rows.InsertAt(dr, 0);

                crptStockReport crptStockReportObj = new crptStockReport();
                foreach (DataTable dtbl in dsStockReport.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptStockReportObj.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {

                        crptStockReportObj.Database.Tables["dtblStockReport"].SetDataSource(dtbl);
                    }

                }
                crptStockReportObj.Database.Tables["dtblTotal"].SetDataSource(dtblGrandTotal);
                this.crptViewer.ReportSource = crptStockReportObj;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptStockReportObj.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV80 " + ex.Message;
            }

        }
        /// <summary>
        ///  Function to print ChequeReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsChequeReport"></param>
        internal void ChequeReportPrinting(DataSet dsChequeReport)
        {
            try
            {

                crptChequeReport crptChequeReport = new crptChequeReport();
                foreach (DataTable dtbl in dsChequeReport.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptChequeReport.Database.Tables["dtblChequeReport"].SetDataSource(dtbl);

                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        crptChequeReport.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }

                }

                this.crptViewer.ReportSource = crptChequeReport;

                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {

                    crptChequeReport.PrintToPrinter(1, false, 0, 0);
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV81 " + ex.Message;
            }

        }
        /// <summary>
        ///  Function to print freeSaleReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsFreeSale"></param>
        internal void freeSaleReport(DataSet dsFreeSale)
        {
            try
            {

                crptFreeSaleReport crptFreeSaleReport = new crptFreeSaleReport();

                foreach (DataTable dtbl in dsFreeSale.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptFreeSaleReport.Database.Tables["dtblCompanyDetails"].SetDataSource(dtbl);
                    }
                    if (dtbl.TableName == "Table1")
                    {

                        crptFreeSaleReport.Database.Tables["dtblFreeSaleReport"].SetDataSource(dtbl);
                    }

                }
                this.crptViewer.ReportSource = crptFreeSaleReport;

                SettingsSP spSettings = new SettingsSP();
                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptFreeSaleReport.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV82 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print vatreturnReport in curresponding Crystal report
        /// </summary>
        /// <param name="dtblReport"></param>
        internal void vatreturnReport(DataSet dtblReport)
        {
            try
            {
                crptVatreturnReport crptVatreport = new crptVatreturnReport();
                foreach (DataTable dtbl in dtblReport.Tables)
                {
                    if (dtbl.TableName == "Table2")
                    {
                        crptVatreport.Database.Tables["dtblCompany"].SetDataSource(dtbl);

                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        crptVatreport.Database.Tables["dtblVatreturn"].SetDataSource(dtbl);
                    }
                }

                this.crptViewer.ReportSource = crptVatreport;
                base.Show();

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV83 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print PDCClearancevoucher in curresponding Crystal report
        /// </summary>
        /// <param name="dsPDCClearanceVoucher"></param>
        internal void PDCClearancevoucherPrinting(DataSet dsPDCClearanceVoucher)
        {
            try
            {

                crptPDCClearanceVoucher crptClearanceReport = new crptPDCClearanceVoucher();

                foreach (DataTable dtbl in dsPDCClearanceVoucher.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptClearanceReport.Database.Tables["dtblCompany"].SetDataSource(dtbl);


                    }

                    else if (dtbl.TableName == "Table1")
                    {

                        crptClearanceReport.Database.Tables["dtblPDCClearance"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {

                        crptClearanceReport.Database.Tables["dtblPDCClearanceDetails"].SetDataSource(dtbl);
                    }

                }

                this.crptViewer.ReportSource = crptClearanceReport;
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptClearanceReport.PrintToPrinter(1, false, 0, 0);

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV84" + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print vatreturnReport in curresponding Crystal report
        /// </summary>
        /// <param name="dtblReport"></param>
        internal void vatreturnReportFormat(DataSet dtblReport)
        {
            try
            {
                crptVatReport crptVatreport = new crptVatReport();
                foreach (DataTable dtbl in dtblReport.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptVatreport.Database.Tables["dtblCompany"].SetDataSource(dtbl);

                    }
                    else if (dtbl.TableName == "Table2")
                    {
                        crptVatreport.Database.Tables["dtblVat2"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table3")
                    {
                        crptVatreport.Database.Tables["dtblTotal"].SetDataSource(dtbl);
                    }
                }

                this.crptViewer.ReportSource = crptVatreport;
                base.Show();


            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV85 " + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print PDCClearanceReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsPDCClearanceReport"></param>
        internal void PDCClearanceReportPrinting(DataSet dsPDCClearanceReport)
        {
            try
            {
                crptPDCClearanceReport crptPDCClear = new crptPDCClearanceReport();
                foreach (DataTable dtbl in dsPDCClearanceReport.Tables)
                {
                    if (dtbl.TableName == "Table")
                    {
                        crptPDCClear.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table1")
                    {
                        crptPDCClear.Database.Tables["dtblPDCClearance"].SetDataSource(dtbl);
                    }

                }
                this.crptViewer.ReportSource = crptPDCClear;
                SettingsSP spSettings = new SettingsSP();
                base.Show();
                this.BringToFront();
                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptPDCClear.PrintToPrinter(1, false, 0, 0);

                }

            }
            catch (Exception ex)
            {

                formMDI.infoError.ErrorString = "CRV86" + ex.Message;
            }
        }
        /// <summary>
        ///  Function to print AccountGroupReport in curresponding Crystal report
        /// </summary>
        /// <param name="dsAccountGroup"></param>
        internal void AccountGroup(DataSet dsAccountGroup)
        {
            try
            {

                crptAccountGroupReport crptAccountGroupReport = new crptAccountGroupReport();

                foreach (DataTable dtbl in dsAccountGroup.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        crptAccountGroupReport.Database.Tables["dtblAccountGroup"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table2")
                    {

                        crptAccountGroupReport.Database.Tables["dtblCompany"].SetDataSource(dtbl);
                    }
                    else if (dtbl.TableName == "Table3")
                    {
                        crptAccountGroupReport.Database.Tables["dtblSum"].SetDataSource(dtbl);
                    }




                }
                this.crptViewer.ReportSource = crptAccountGroupReport;

                SettingsSP spSettings = new SettingsSP();
                if (spSettings.SettingsStatusCheck("DirectPrint") == "No")
                {
                    base.Show();
                    this.BringToFront();
                }
                else
                {
                    crptAccountGroupReport.PrintToPrinter(1, false, 0, 0);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV87 " + ex.Message;
            }
        }

        #endregion

        #region Events
        /// <summary>
        /// Form key down for Quick access
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmReport_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    if (PublicVariables.isMessageClose)
                    {
                        Messages.CloseMessage(this);
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRV88 " + ex.Message;
            }
        }

        #endregion

    }
}
