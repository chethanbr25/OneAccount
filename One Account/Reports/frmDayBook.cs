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
namespace One_Account
{
    public partial class frmDayBook : Form
    {
        #region Public variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        int inCurrenRowIndex = 0;
        DataTable dtbl = new DataTable();
        #endregion
        #region functions
        /// <summary>
        /// Create an Instance of a frmDayBook class
        /// </summary>
        public frmDayBook()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to fill vouchertype combobox
        /// </summary>
        public void voucherTypeComboFill()
        {
            try
            {
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                DataTable dtbl = new DataTable();
                dtbl = spVoucherType.VoucherTypeViewAll();
                DataRow dr = dtbl.NewRow();
                dr[0] = 0;
                dr[1] = "All";
                dtbl.Rows.InsertAt(dr, 0);
                cmbVoucherType.DataSource = dtbl;
                cmbVoucherType.DisplayMember = "voucherTypeName";
                cmbVoucherType.ValueMember = "voucherTypeId";
                cmbVoucherType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB1:" + ex.Message;
            }
        }
        /// <summary>
        /// combo ledger Fill
        /// </summary>
        public void ledgerComboFill()
        {
            try
            {
                TransactionsGeneralFill obj = new TransactionsGeneralFill();
                DataTable dtbl = new DataTable();
                dtbl = obj.AccountLedgerComboFill();
                DataRow dr = dtbl.NewRow();
                dr[0] = 0;
                dr[2] = "All";
                dtbl.Rows.InsertAt(dr, 0);
                cmbLedger.DataSource = dtbl;
                cmbLedger.DisplayMember = "ledgerName";
                cmbLedger.ValueMember = "ledgerId";
                cmbLedger.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to clear the fields
        /// </summary>
        public void clear()
        {
            try
            {
                rbtnCondensed.Checked = true;
                cmbLedger.SelectedIndex = 0;
                cmbVoucherType.SelectedIndex = 0;
                /*-------date setting at the time of loading*/
                dtpFromDate.MinDate = PublicVariables._dtFromDate;
                dtpFromDate.MaxDate = PublicVariables._dtToDate;
                dtpFromDate.Value = PublicVariables._dtCurrentDate;
                dtpToDate.MinDate = PublicVariables._dtFromDate;
                dtpToDate.MaxDate = PublicVariables._dtToDate;
                dtpToDate.Value = PublicVariables._dtCurrentDate;
                //----------------------------------------------*/
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB3:" + ex.Message;
            }
        }
        /// <summary>
        /// Function for daybook gridfill
        /// </summary>
        public void dayBookGridFill()
        {
            try
            {
                decimal decDebitTotal = 0;
                decimal decCreditTotal = 0;
                decimal decInwrdTotal = 0;
                decimal decOtwrdTotal = 0;
                decimal decBalance = 0;
                decimal decBalQty = 0;
                FinancialStatementSP spFinance = new FinancialStatementSP();
                dgvDayBook.DataSource = null;
                dtbl = spFinance.DayBook(Convert.ToDateTime(dtpFromDate.Value.ToString("dd/MMM/yyyy")), Convert.ToDateTime(dtpToDate.Value.ToString("dd/MMM/yyyy")), Convert.ToDecimal(cmbVoucherType.SelectedValue), Convert.ToDecimal(cmbLedger.SelectedValue), rbtnCondensed.Checked);
                if (dtbl.Rows.Count > 0)
                {
                    decDebitTotal = Convert.ToDecimal(dtbl.Compute("Sum(Debit)", string.Empty).ToString());
                    decCreditTotal = Convert.ToDecimal(dtbl.Compute("Sum(Credit)", string.Empty).ToString());
                    decInwrdTotal = Convert.ToDecimal(dtbl.Compute("Sum([Inward Qty])", string.Empty).ToString());
                    decOtwrdTotal = Convert.ToDecimal(dtbl.Compute("Sum([Outward Qty])", string.Empty).ToString());
                }
                else
                {
                    decDebitTotal = 0;
                    decCreditTotal = 0;
                    decOtwrdTotal = 0;
                    decInwrdTotal = 0;
                }
                if (decDebitTotal > decCreditTotal)
                {
                    decBalance = decDebitTotal - decCreditTotal;
                }
                else
                {
                    decBalance = decCreditTotal - decDebitTotal;
                }
                if (decInwrdTotal > decOtwrdTotal)
                {
                    decBalQty = decInwrdTotal - decOtwrdTotal;
                }
                else
                {
                    decBalQty = decOtwrdTotal - decInwrdTotal;
                }
                dtbl.Rows.Add();
                dtbl.Rows[dtbl.Rows.Count - 1]["Invoice No"] = string.Empty;
                dtbl.Rows[dtbl.Rows.Count - 1]["Voucher Type"] = string.Empty;
                dtbl.Rows.Add();
                if (rbtnCondensed.Checked == true)
                {
                    dtbl.Rows[dtbl.Rows.Count - 1]["Invoice No"] = "Total :";
                }
                else
                {
                    dtbl.Rows[dtbl.Rows.Count - 1]["Ledger"] = "Total :";
                }
                dtbl.Rows[dtbl.Rows.Count - 1]["Debit"] = decDebitTotal.ToString();
                dtbl.Rows[dtbl.Rows.Count - 1]["Credit"] = decCreditTotal.ToString();
                dtbl.Rows[dtbl.Rows.Count - 1]["Inward Qty"] = decInwrdTotal.ToString();
                dtbl.Rows[dtbl.Rows.Count - 1]["Outward Qty"] = decOtwrdTotal.ToString();
                dtbl.Rows.Add();
                if (rbtnCondensed.Checked == true)
                {
                    dtbl.Rows[dtbl.Rows.Count - 1]["Voucher Type"] = "Balance :";
                    dtbl.Rows[dtbl.Rows.Count - 1]["Invoice No"] = (decDebitTotal > decCreditTotal ? decBalance + "Dr" : decBalance + "Cr");
                }
                else
                {
                    dtbl.Rows[dtbl.Rows.Count - 1]["Invoice No"] = "Balance :";
                    dtbl.Rows[dtbl.Rows.Count - 1]["Ledger"] = (decDebitTotal > decCreditTotal ? decBalance + "Dr" : decBalance + "Cr");
                }
                dtbl.Rows[dtbl.Rows.Count - 1]["Inward Qty"] = decBalQty.ToString();
                dgvDayBook.DataSource = dtbl;
                dgvDayBook.Columns["typeOfVoucher"].Visible = false;
                dgvDayBook.Columns["voucherTypeId"].Visible = false;
                dgvDayBook.Columns["voucherNo"].Visible = false;
                dgvDayBook.Columns["MasterId"].Visible = false;
                int index = 0;
                if (dgvDayBook.RowCount > 0)
                {
                    index++;
                    dgvDayBook.Rows[0].Cells["Sl No"].Value = index.ToString();
                    int i = 0;
                    string date = dgvDayBook.Rows[i].Cells["Date"].Value.ToString();
                    string voucherType = dgvDayBook.Rows[i].Cells["Voucher Type"].Value.ToString();
                    string voucherNo = dgvDayBook.Rows[i].Cells["Invoice No"].Value.ToString();
                    i++;
                    for (; i < dgvDayBook.RowCount; )
                    {
                        if (i >= dgvDayBook.RowCount)
                            break;
                        while ((dgvDayBook.Rows[i].Cells["Date"].Value.ToString() == date || dgvDayBook.Rows[i].Cells["Date"].Value.ToString() == "") && dgvDayBook.Rows[i].Cells["Voucher Type"].Value.ToString() == voucherType && dgvDayBook.Rows[i].Cells["Invoice No"].Value.ToString() == voucherNo)
                        {
                            dgvDayBook.Rows[i].Cells["Date"].Value = string.Empty;
                            dgvDayBook.Rows[i].Cells["Voucher Type"].Value = string.Empty;
                            dgvDayBook.Rows[i].Cells["Invoice No"].Value = string.Empty;
                            i++;
                            if (i >= dgvDayBook.RowCount)
                                break;
                        }
                        if (i >= dgvDayBook.RowCount)
                            break;
                        index++;
                        dgvDayBook.Rows[i].Cells["Sl No"].Value = index.ToString();
                        date = dgvDayBook.Rows[i].Cells["Date"].Value.ToString();
                        voucherType = dgvDayBook.Rows[i].Cells["Voucher Type"].Value.ToString();
                        voucherNo = dgvDayBook.Rows[i].Cells["Invoice No"].Value.ToString();
                        i++;
                        dgvDayBook.Rows[dgvDayBook.Rows.Count - 1].Cells["Sl No"].Value = string.Empty;
                        dgvDayBook.Rows[dgvDayBook.Rows.Count - 2].Cells["Sl No"].Value = string.Empty;
                        dgvDayBook.Rows[dgvDayBook.Rows.Count - 3].Cells["Sl No"].Value = string.Empty;
                    }
                    for (int incolIndex = 0; incolIndex < dgvDayBook.Columns.Count; ++incolIndex)//to set columns in the grid not sortable
                        dgvDayBook.Columns[incolIndex].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                if (inCurrenRowIndex >= 0 && dgvDayBook.Rows.Count > 0 && inCurrenRowIndex < dgvDayBook.Rows.Count)
                {
                    dgvDayBook.CurrentCell = dgvDayBook.Rows[inCurrenRowIndex].Cells["Date"];
                    dgvDayBook.CurrentCell.Selected = true;
                }
                inCurrenRowIndex = 0;
                if (dgvDayBook.Columns.Count > 0)
                {
                    dgvDayBook.Columns["Debit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvDayBook.Columns["Credit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvDayBook.Columns["Inward Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvDayBook.Columns["Outward Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB4:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to create datatable for daybook gridfill
        /// </summary>
        /// <returns></returns>
        public DataTable GetDataTable()
        {
            DataTable dtblDayBook = new DataTable();
            try
            {
                if (rbtnCondensed.Checked == true)
                {
                    dtblDayBook.Columns.Add("Sl No");
                    dtblDayBook.Columns.Add("Date");
                    dtblDayBook.Columns.Add("Voucher Type");
                    dtblDayBook.Columns.Add("Invoice No");
                    dtblDayBook.Columns.Add("Debit");
                    dtblDayBook.Columns.Add("Credit");
                    dtblDayBook.Columns.Add("Inward Qty");
                    dtblDayBook.Columns.Add("Outward Qty");
                    DataRow drow = null;
                    foreach (DataGridViewRow dr in dgvDayBook.Rows)
                    {
                        drow = dtblDayBook.NewRow();
                        drow["Sl No"] = dr.Cells["Sl No"].Value;
                        drow["Date"] = dr.Cells["Date"].Value;
                        drow["Voucher Type"] = dr.Cells["Voucher Type"].Value;
                        drow["Invoice No"] = dr.Cells["Invoice No"].Value;
                        drow["Debit"] = dr.Cells["Debit"].Value;
                        drow["Credit"] = dr.Cells["Credit"].Value;
                        drow["Inward Qty"] = dr.Cells["Inward Qty"].Value;
                        drow["Outward Qty"] = dr.Cells["Outward Qty"].Value;
                        dtblDayBook.Rows.Add(drow);
                    }
                }
                else
                {
                    dtblDayBook.Columns.Add("Sl No");
                    dtblDayBook.Columns.Add("Date");
                    dtblDayBook.Columns.Add("Voucher Type");
                    dtblDayBook.Columns.Add("Invoice No");
                    dtblDayBook.Columns.Add("Ledger");
                    dtblDayBook.Columns.Add("Debit");
                    dtblDayBook.Columns.Add("Credit");
                    dtblDayBook.Columns.Add("Inward Qty");
                    dtblDayBook.Columns.Add("Outward Qty");
                    DataRow drow = null;
                    foreach (DataGridViewRow dr in dgvDayBook.Rows)
                    {
                        drow = dtblDayBook.NewRow();
                        drow["Sl No"] = dr.Cells["Sl No"].Value;
                        drow["Date"] = dr.Cells["Date"].Value;
                        drow["Voucher Type"] = dr.Cells["Voucher Type"].Value;
                        drow["Invoice No"] = dr.Cells["Invoice No"].Value;
                        drow["Ledger"] = dr.Cells["Ledger"].Value;
                        drow["Debit"] = dr.Cells["Debit"].Value;
                        drow["Credit"] = dr.Cells["Credit"].Value;
                        drow["Inward Qty"] = dr.Cells["Inward Qty"].Value;
                        drow["Outward Qty"] = dr.Cells["Outward Qty"].Value;
                        dtblDayBook.Rows.Add(drow);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB5:" + ex.Message;
            }
            return dtblDayBook;
        }
        /// <summary>
        /// Convert DataTable to DataSet
        /// </summary>
        /// <returns></returns>
        public DataSet getdataset()
        {
            DataSet dsDayBookReport = new DataSet();
            try
            {
                FinancialStatementSP spfinancial = new FinancialStatementSP();
                DataTable dtblDayBook = GetDataTable();
                DataTable dtblCompany = new DataTable();
                dtblCompany = spfinancial.DayBookReportPrintCompany();
                dsDayBookReport.Tables.Add(dtblDayBook);
                dsDayBookReport.Tables.Add(dtblCompany);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB6:" + ex.Message;
            }
            return dsDayBookReport;
        }
        /// <summary>
        /// Convert DataTable to DataSet for condensed
        /// </summary>
        /// <returns></returns>
        public DataSet getdatasetCondensed()
        {
            DataSet dsDayBookReportCondensed = new DataSet();
            try
            {
                FinancialStatementSP spfinancial = new FinancialStatementSP();
                
                DataTable dtblDayBook = GetDataTable();
                DataTable dtblCompany = new DataTable();
                dtblCompany = spfinancial.DayBookReportPrintCompany();
                dsDayBookReportCondensed.Tables.Add(dtblCompany);
                dsDayBookReportCondensed.Tables.Add(dtblDayBook);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB7:" + ex.Message;
            }
            return dsDayBookReportCondensed;
        }
        
        /// <summary>
        /// Function to print the DayBook
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="decVoucherTypeId"></param>
        /// <param name="decLedgerId"></param>
        /// <param name="blCondenced"></param>
        public void Print(DateTime fromDate, DateTime toDate, decimal decVoucherTypeId, decimal decLedgerId, bool blCondenced)
        {
            try
            {
                DataSet dsDayBookReport = getdataset();
                DataSet dsDayBookReportCondensed = getdatasetCondensed();
                frmReport frmReport = new frmReport();
                frmReport.MdiParent = formMDI.MDIObj;
                if (rbtnCondensed.Checked == true)
                {
                    frmReport.dayBookReportPrintingCondensed(dsDayBookReportCondensed);
                }
                else
                {
                    frmReport.dayBookReportPrintingDetailed(dsDayBookReport);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB8:" + ex.Message;
            }
        }
        #endregion
        #region events
       /// <summary>
       /// On load
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void frmDayBook_Load(object sender, EventArgs e)
        {
            try
            {
                voucherTypeComboFill();
                ledgerComboFill();
                txtFromDate.Focus();
                clear();
                dayBookGridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB9:" + ex.Message;
            }
        }
        /// <summary>
        /// On Value change of dtpFromDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpFromDate.Value;
                this.txtFromDate.Text = date.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB10:" + ex.Message;
            }
        }
        /// <summary>
        /// On leave from txtFromDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFromDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation obj = new DateValidation();
                obj.DateValidationFunction(txtFromDate);
                if (txtFromDate.Text == string.Empty)
                {
                    txtFromDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                //----for change date in date time picker------//
                dtpFromDate.Value = Convert.ToDateTime(txtFromDate.Text);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB11:" + ex.Message;
            }
        }
        /// <summary>
        /// On Value change of dtpToDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpToDate.Value;
                this.txtToDate.Text = date.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB12:" + ex.Message;
            }
        }
        /// <summary>
        /// On leave from txtToDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtToDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation obj = new DateValidation();
                obj.DateValidationFunction(txtToDate);
                if (txtToDate.Text == string.Empty)
                {
                    txtToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                //----for change date in date time picker------//
                dtpToDate.Value = Convert.ToDateTime(txtToDate.Text);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB13:" + ex.Message;
            }
        }
        /// <summary>
        /// On search button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtpToDate.Value >= dtpFromDate.Value)
                {
                    dayBookGridFill();
                }
                else
                {
                    Messages.InformationMessage("To date less than from date");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB14:" + ex.Message;
            }
        }
        /// <summary>
        /// On reset button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                clear();
                dayBookGridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB15:" + ex.Message;
            }
        }
        /// <summary>
        /// on databinding complete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDataBook_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                if (dgvDayBook.RowCount > 2)
                {
                    dgvDayBook.Rows[dgvDayBook.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Red;
                    dgvDayBook.Rows[dgvDayBook.Rows.Count - 1].DefaultCellStyle.Font = new Font(dgvDayBook.Font, FontStyle.Bold);
                    dgvDayBook.Rows[dgvDayBook.Rows.Count - 2].DefaultCellStyle.ForeColor = Color.Red;
                    dgvDayBook.Rows[dgvDayBook.Rows.Count - 2].DefaultCellStyle.Font = new Font(dgvDayBook.Font, FontStyle.Bold);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB16:" + ex.Message;
            }
        }
        /// <summary>
        /// On print button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                btnSearch_Click(sender, e);
                if (dgvDayBook.RowCount > 3)
                {
                    Print(this.dtpFromDate.Value, this.dtpToDate.Value, Convert.ToDecimal(cmbVoucherType.SelectedValue), Convert.ToDecimal(cmbLedger.SelectedValue), rbtnCondensed.Checked);
                }
                else
                {
                    Messages.InformationMessage("No data found");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB17:" + ex.Message;
            }
        }
        
        
        /// <summary>
        /// When doubleclicking on the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDayBook_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string strVoucherType = string.Empty;
                decimal decMasterId = 0;
                SalesMasterSP spSalesMaster = new SalesMasterSP();
                if (dgvDayBook.CurrentRow.Index == e.RowIndex)
                {
                    int inI = dgvDayBook.CurrentCell.RowIndex;
                    int inCount = dgvDayBook.Rows.Count;
                    if (dgvDayBook.CurrentCell.RowIndex < dgvDayBook.Rows[inCount - 3].Cells["Date"].RowIndex)
                    {
                        foreach (DataGridViewRow dgv in dgvDayBook.Rows)
                        {
                            if (dgv.Cells["Date"].Value.ToString() != string.Empty)
                            {
                                strVoucherType = dgv.Cells["typeOfVoucher"].Value.ToString();
                                decMasterId = Convert.ToDecimal(dgv.Cells["MasterId"].Value.ToString());
                            }
                            if (dgv.Index == inI)
                            {
                                break;
                            }
                        }
                        if (strVoucherType == "Contra Voucher")
                        {
                            frmContraVoucher frmContraVoucher = new frmContraVoucher();
                            frmContraVoucher open = Application.OpenForms["frmContraVoucher"] as frmContraVoucher;
                            if (open == null)
                            {
                                frmContraVoucher.WindowState = FormWindowState.Normal;
                                frmContraVoucher.MdiParent = formMDI.MDIObj;
                                frmContraVoucher.callFromDayBook(this, decMasterId);
                            }
                            else
                            {
                                open.callFromDayBook(this, decMasterId);
                                if (open.WindowState == FormWindowState.Minimized)
                                {
                                    open.WindowState = FormWindowState.Normal;
                                }
                            }
                        }
                        else if (strVoucherType == "Payment Voucher")
                        {
                            frmPaymentVoucher frmPaymentVoucher = new frmPaymentVoucher();
                            frmPaymentVoucher open = Application.OpenForms["frmPaymentVoucher"] as frmPaymentVoucher;
                            if (open == null)
                            {
                                frmPaymentVoucher.WindowState = FormWindowState.Normal;
                                frmPaymentVoucher.MdiParent = formMDI.MDIObj;
                                frmPaymentVoucher.callFromDayBook(this, decMasterId);
                            }
                            else
                            {
                                open.callFromDayBook(this, decMasterId);
                                if (open.WindowState == FormWindowState.Minimized)
                                {
                                    open.WindowState = FormWindowState.Normal;
                                }
                            }
                        }
                        else if (strVoucherType == "Receipt Voucher")
                        {
                            frmReceiptVoucher frmReceiptVoucher = new frmReceiptVoucher();
                            frmReceiptVoucher open = Application.OpenForms["frmReceiptVoucher"] as frmReceiptVoucher;
                            if (open == null)
                            {
                                frmReceiptVoucher.WindowState = FormWindowState.Normal;
                                frmReceiptVoucher.MdiParent = formMDI.MDIObj;
                                frmReceiptVoucher.callFromDayBook(this, decMasterId);
                            }
                            else
                            {
                                if (open.WindowState == FormWindowState.Minimized)
                                {
                                    open.WindowState = FormWindowState.Normal;
                                }
                                open.callFromDayBook(this, decMasterId);
                            }
                        }
                        else if (strVoucherType == "Journal Voucher")
                        {
                            frmJournalVoucher frmJournalVoucher = new frmJournalVoucher();
                            frmJournalVoucher open = Application.OpenForms["frmJournalVoucher"] as frmJournalVoucher;
                            if (open == null)
                            {
                                frmJournalVoucher.WindowState = FormWindowState.Normal;
                                frmJournalVoucher.MdiParent = formMDI.MDIObj;
                                frmJournalVoucher.callFromDayBook(this, decMasterId);
                            }
                            else
                            {
                                open.callFromDayBook(this, decMasterId);
                                if (open.WindowState == FormWindowState.Minimized)
                                {
                                    open.WindowState = FormWindowState.Normal;
                                }
                            }
                        }
                        else if (strVoucherType == "PDC Payable")
                        {
                            frmPdcPayable frmPdcPayable = new frmPdcPayable();
                            frmPdcPayable open = Application.OpenForms["frmPdcPayable"] as frmPdcPayable;
                            if (open == null)
                            {
                                frmPdcPayable.WindowState = FormWindowState.Normal;
                                frmPdcPayable.MdiParent = formMDI.MDIObj;
                                frmPdcPayable.callFromDayBook(this, decMasterId);
                            }
                            else
                            {
                                if (open.WindowState == FormWindowState.Minimized)
                                {
                                    open.WindowState = FormWindowState.Normal;
                                }
                                open.callFromDayBook(this, decMasterId);
                            }
                        }
                        else if (strVoucherType == "PDC Receivable")
                        {
                            frmPdcReceivable frmPdcReceivable = new frmPdcReceivable();
                            frmPdcReceivable open = Application.OpenForms["frmPdcReceivable"] as frmPdcReceivable;
                            if (open == null)
                            {
                                frmPdcReceivable.WindowState = FormWindowState.Normal;
                                frmPdcReceivable.MdiParent = formMDI.MDIObj;
                                frmPdcReceivable.callFromDayBook(this, decMasterId);
                            }
                            else
                            {
                                if (open.WindowState == FormWindowState.Minimized)
                                {
                                    open.WindowState = FormWindowState.Normal;
                                }
                                open.callFromDayBook(this, decMasterId);
                            }
                        }
                        else if (strVoucherType == "Purchase Invoice")
                        {
                            frmPurchaseInvoice frmPurchaseInvoice = new frmPurchaseInvoice();
                            frmPurchaseInvoice open = Application.OpenForms["frmPurchaseInvoice"] as frmPurchaseInvoice;
                            if (open == null)
                            {
                                frmPurchaseInvoice.WindowState = FormWindowState.Normal;
                                frmPurchaseInvoice.MdiParent = formMDI.MDIObj;
                                frmPurchaseInvoice.callFromDayBook(this, decMasterId);
                            }
                            else
                            {
                                if (open.WindowState == FormWindowState.Minimized)
                                {
                                    open.WindowState = FormWindowState.Normal;
                                }
                                open.callFromDayBook(this, decMasterId);
                            }
                        }
                        else if (strVoucherType == "Purchase Return")
                        {
                            frmPurchaseReturn frmPurchaseReturn = new frmPurchaseReturn();
                            frmPurchaseReturn open = Application.OpenForms["frmPurchaseReturn"] as frmPurchaseReturn;
                            if (open == null)
                            {
                                frmPurchaseReturn.WindowState = FormWindowState.Normal;
                                frmPurchaseReturn.MdiParent = formMDI.MDIObj;
                                frmPurchaseReturn.callFromDayBook(this, decMasterId);
                            }
                            else
                            {
                                if (open.WindowState == FormWindowState.Minimized)
                                {
                                    open.WindowState = FormWindowState.Normal;
                                }
                                open.callFromDayBook(this, decMasterId);
                            }
                        }
                        else if (strVoucherType == "Sales Invoice")
                        {
                            decimal decSalesId = Convert.ToDecimal(dgvDayBook.CurrentRow.Cells["MasterId"].Value.ToString());
                            decimal decVoucherId = Convert.ToDecimal(dgvDayBook.CurrentRow.Cells["voucherTypeId"].Value.ToString());
                            bool isPOS = spSalesMaster.DayBookSalesInvoiceOrPOS(decSalesId, decVoucherId);
                            if (isPOS == true)
                            {
                                frmPOS frmPOSObj = new frmPOS();
                                frmPOS open = Application.OpenForms["frmPOS"] as frmPOS;
                                if (open == null)
                                {
                                    frmPOSObj.WindowState = FormWindowState.Normal;
                                    frmPOSObj.MdiParent = formMDI.MDIObj;
                                    frmPOSObj.callFromDayBook(this, decMasterId);
                                }
                                else
                                {
                                    if (open.WindowState == FormWindowState.Minimized)
                                    {
                                        open.WindowState = FormWindowState.Normal;
                                    }
                                    open.callFromDayBook(this, decMasterId);
                                }
                            }
                            else
                            {
                                frmSalesInvoice frmSalesInvoiceObj = new frmSalesInvoice();
                                frmSalesInvoice open = Application.OpenForms["frmSalesInvoice"] as frmSalesInvoice;
                                if (open == null)
                                {
                                    frmSalesInvoiceObj.WindowState = FormWindowState.Normal;
                                    frmSalesInvoiceObj.MdiParent = formMDI.MDIObj;
                                    frmSalesInvoiceObj.callFromDayBook(this, decMasterId);
                                }
                                else
                                {
                                    if (open.WindowState == FormWindowState.Minimized)
                                    {
                                        open.WindowState = FormWindowState.Normal;
                                    }
                                    open.callFromDayBook(this, decMasterId);
                                }
                            }
                        }
                        else if (strVoucherType == "Sales Return")
                        {
                            frmSalesReturn frmSalesReturn = new frmSalesReturn();
                            frmSalesReturn open = Application.OpenForms["frmSalesReturn"] as frmSalesReturn;
                            if (open == null)
                            {
                                frmSalesReturn.WindowState = FormWindowState.Normal;
                                frmSalesReturn.MdiParent = formMDI.MDIObj;
                                frmSalesReturn.callFromDayBook(this, decMasterId);
                            }
                            else
                            {
                                if (open.WindowState == FormWindowState.Minimized)
                                {
                                    open.WindowState = FormWindowState.Normal;
                                }
                                open.callFromDayBook(this, decMasterId);
                            }
                        }
                        else if (strVoucherType == "Service Voucher")
                        {
                            frmServiceVoucher frmServiceVoucher = new frmServiceVoucher();
                            frmServiceVoucher open = Application.OpenForms["frmServiceVoucher"] as frmServiceVoucher;
                            if (open == null)
                            {
                                frmServiceVoucher.WindowState = FormWindowState.Normal;
                                frmServiceVoucher.MdiParent = formMDI.MDIObj;
                                frmServiceVoucher.callFromDayBook(this, decMasterId);
                            }
                            else
                            {
                                if (open.WindowState == FormWindowState.Minimized)
                                {
                                    open.WindowState = FormWindowState.Normal;
                                }
                                open.callFromDayBook(this, decMasterId);
                            }
                        }
                        else if (strVoucherType == "Credit Note")
                        {
                            frmCreditNote frmCreditNote = new frmCreditNote();
                            frmCreditNote open = Application.OpenForms["frmCreditNote"] as frmCreditNote;
                            if (open == null)
                            {
                                frmCreditNote.WindowState = FormWindowState.Normal;
                                frmCreditNote.MdiParent = formMDI.MDIObj;
                                frmCreditNote.callFromDayBook(this, decMasterId);
                            }
                            else
                            {
                                if (open.WindowState == FormWindowState.Minimized)
                                {
                                    open.WindowState = FormWindowState.Normal;
                                }
                                open.callFromDayBook(this, decMasterId);
                            }
                        }
                        else if (strVoucherType == "Debit Note")
                        {
                            frmDebitNote frmDebitNote = new frmDebitNote();
                            frmDebitNote open = Application.OpenForms["frmDebitNote"] as frmDebitNote;
                            if (open == null)
                            {
                                frmDebitNote.WindowState = FormWindowState.Normal;
                                frmDebitNote.MdiParent = formMDI.MDIObj;
                                frmDebitNote.callFromDayBook(this, decMasterId);
                            }
                            else
                            {
                                if (open.WindowState == FormWindowState.Minimized)
                                {
                                    open.WindowState = FormWindowState.Normal;
                                }
                                open.callFromDayBook(this, decMasterId);
                            }
                        }
                        else if (strVoucherType == "Rejection In")
                        {
                            frmRejectionIn frmRejectionIn = new frmRejectionIn();
                            frmRejectionIn open = Application.OpenForms["frmRejectionIn"] as frmRejectionIn;
                            if (open == null)
                            {
                                frmRejectionIn.WindowState = FormWindowState.Normal;
                                frmRejectionIn.MdiParent = formMDI.MDIObj;
                                frmRejectionIn.callFromDayBook(this, decMasterId);
                            }
                            else
                            {
                                if (open.WindowState == FormWindowState.Minimized)
                                {
                                    open.WindowState = FormWindowState.Normal;
                                }
                                open.callFromDayBook(this, decMasterId);
                            }
                        }
                        else if (strVoucherType == "Rejection Out")
                        {
                            frmRejectionOut frmRejectionOut = new frmRejectionOut();
                            frmRejectionOut open = Application.OpenForms["frmRejectionOut"] as frmRejectionOut;
                            if (open == null)
                            {
                                frmRejectionOut.WindowState = FormWindowState.Normal;
                                frmRejectionOut.MdiParent = formMDI.MDIObj;
                                frmRejectionOut.callFromDayBook(this, decMasterId);
                            }
                            else
                            {
                                if (open.WindowState == FormWindowState.Minimized)
                                {
                                    open.WindowState = FormWindowState.Normal;
                                }
                                open.callFromDayBook(this, decMasterId);
                            }
                        }
                        else if (strVoucherType == "Delivery Note")
                        {
                            frmDeliveryNote frmDeliveryNote = new frmDeliveryNote();
                            frmDeliveryNote open = Application.OpenForms["frmDeliveryNote"] as frmDeliveryNote;
                            if (open == null)
                            {
                                frmDeliveryNote.WindowState = FormWindowState.Normal;
                                frmDeliveryNote.MdiParent = formMDI.MDIObj;
                                frmDeliveryNote.callFromDayBook(this, decMasterId);
                            }
                            else
                            {
                                if (open.WindowState == FormWindowState.Minimized)
                                {
                                    open.WindowState = FormWindowState.Normal;
                                }
                                open.callFromDayBook(this, decMasterId);
                            }
                        }
                        else if (strVoucherType == "Material Receipt")
                        {
                            frmMaterialReceipt frmMaterialReceipt = new frmMaterialReceipt();
                            frmMaterialReceipt open = Application.OpenForms["frmMaterialReceipt"] as frmMaterialReceipt;
                            if (open == null)
                            {
                                frmMaterialReceipt.WindowState = FormWindowState.Normal;
                                frmMaterialReceipt.MdiParent = formMDI.MDIObj;
                                frmMaterialReceipt.callFromDayBook(this, decMasterId);
                            }
                            else
                            {
                                if (open.WindowState == FormWindowState.Minimized)
                                {
                                    open.WindowState = FormWindowState.Normal;
                                }
                                open.callFromDayBook(this, decMasterId);
                            }
                        }
                        else if (strVoucherType == "PDC Clearance")
                        {
                            frmPdcClearance frmPdcClearance = new frmPdcClearance();
                            frmPdcClearance open = Application.OpenForms["frmPdcClearance"] as frmPdcClearance;
                            if (open == null)
                            {
                                frmPdcClearance.WindowState = FormWindowState.Normal;
                                frmPdcClearance.MdiParent = formMDI.MDIObj;
                                frmPdcClearance.callFromDayBook(this, decMasterId);
                            }
                            else
                            {
                                if (open.WindowState == FormWindowState.Minimized)
                                {
                                    open.WindowState = FormWindowState.Normal;
                                }
                                open.callFromDayBook(this, decMasterId);
                            }
                        }
                        else if (strVoucherType == "Purchase Order")
                        {
                            frmPurchaseOrder frmPurchaseOrder = new frmPurchaseOrder();
                            frmPurchaseOrder open = Application.OpenForms["frmPurchaseOrder"] as frmPurchaseOrder;
                            if (open == null)
                            {
                                frmPurchaseOrder.WindowState = FormWindowState.Normal;
                                frmPurchaseOrder.MdiParent = formMDI.MDIObj;
                                frmPurchaseOrder.callFromDayBook(this, decMasterId);
                            }
                            else
                            {
                                if (open.WindowState == FormWindowState.Minimized)
                                {
                                    open.WindowState = FormWindowState.Normal;
                                }
                                open.callFromDayBook(this, decMasterId);
                            }
                        }
                        else if (strVoucherType == "Sales Order")
                        {
                            frmSalesOrder frmSalesOrder = new frmSalesOrder();
                            frmSalesOrder open = Application.OpenForms["frmSalesOrder"] as frmSalesOrder;
                            if (open == null)
                            {
                                frmSalesOrder.WindowState = FormWindowState.Normal;
                                frmSalesOrder.MdiParent = formMDI.MDIObj;
                                frmSalesOrder.callfromDayBook(this, decMasterId);
                            }
                            else
                            {
                                if (open.WindowState == FormWindowState.Minimized)
                                {
                                    open.WindowState = FormWindowState.Normal;
                                }
                                open.callfromDayBook(this, decMasterId);
                            }
                        }
                        else if (strVoucherType == "Sales Quotation")
                        {
                            frmSalesQuotation frmSalesQuotation = new frmSalesQuotation();
                            frmSalesQuotation open = Application.OpenForms["frmSalesQuotation"] as frmSalesQuotation;
                            if (open == null)
                            {
                                frmSalesQuotation.WindowState = FormWindowState.Normal;
                                frmSalesQuotation.MdiParent = formMDI.MDIObj;
                                frmSalesQuotation.callfromDayBook(this, decMasterId);
                            }
                            else
                            {
                                if (open.WindowState == FormWindowState.Minimized)
                                {
                                    open.WindowState = FormWindowState.Normal;
                                }
                                open.callfromDayBook(this, decMasterId);
                            }
                        }
                        else if (strVoucherType == "Physical Stock")
                        {
                            frmPhysicalStock frmPhysicalStock = new frmPhysicalStock();
                            frmPhysicalStock open = Application.OpenForms["frmPhysicalStock"] as frmPhysicalStock;
                            if (open == null)
                            {
                                frmPhysicalStock.WindowState = FormWindowState.Normal;
                                frmPhysicalStock.MdiParent = formMDI.MDIObj;
                                frmPhysicalStock.callFromDayBook(this, decMasterId);
                            }
                            else
                            {
                                if (open.WindowState == FormWindowState.Minimized)
                                {
                                    open.WindowState = FormWindowState.Normal;
                                }
                                open.callFromDayBook(this, decMasterId);
                            }
                        }
                        else if (strVoucherType == "Stock Journal")
                        {
                            frmStockJournal frmStockJournal = new frmStockJournal();
                            frmStockJournal open = Application.OpenForms["frmStockJournal"] as frmStockJournal;
                            if (open == null)
                            {
                                frmStockJournal.WindowState = FormWindowState.Normal;
                                frmStockJournal.MdiParent = formMDI.MDIObj;
                                frmStockJournal.CallFromDayBook(this, decMasterId);
                            }
                            else
                            {
                                if (open.WindowState == FormWindowState.Minimized)
                                {
                                    open.WindowState = FormWindowState.Normal;
                                }
                                open.CallFromDayBook(this, decMasterId);
                            }
                        }
                        else if (strVoucherType == "Daily Salary Voucher")
                        {
                            frmDailySalaryVoucher frmDailySalaryVoucher = new frmDailySalaryVoucher();
                            frmDailySalaryVoucher open = Application.OpenForms["frmDailySalaryVoucher"] as frmDailySalaryVoucher;
                            if (open == null)
                            {
                                frmDailySalaryVoucher.WindowState = FormWindowState.Normal;
                                frmDailySalaryVoucher.MdiParent = formMDI.MDIObj;
                                frmDailySalaryVoucher.CallFromDayBook(this, decMasterId);
                            }
                            else
                            {
                                if (open.WindowState == FormWindowState.Minimized)
                                {
                                    open.WindowState = FormWindowState.Normal;
                                }
                                open.CallFromDayBook(this, decMasterId);
                            }
                        }
                        else if (strVoucherType == "Monthly Salary Voucher")
                        {
                            frmMonthlySalaryVoucher frmMonthlySalaryVoucher = new frmMonthlySalaryVoucher();
                            frmMonthlySalaryVoucher open = Application.OpenForms["frmMonthlySalaryVoucher"] as frmMonthlySalaryVoucher;
                            if (open == null)
                            {
                                frmMonthlySalaryVoucher.WindowState = FormWindowState.Normal;
                                frmMonthlySalaryVoucher.MdiParent = formMDI.MDIObj;
                                frmMonthlySalaryVoucher.CallFromDayBook(this, decMasterId);
                            }
                            else
                            {
                                if (open.WindowState == FormWindowState.Minimized)
                                {
                                    open.WindowState = FormWindowState.Normal;
                                }
                                open.CallFromDayBook(this, decMasterId);
                            }
                        }
                        else if (strVoucherType == "Advance Payment")
                        {
                            frmAdvancePayment frmAdvancePayment = new frmAdvancePayment();
                            frmAdvancePayment open = Application.OpenForms["frmAdvancePayment"] as frmAdvancePayment;
                            if (open == null)
                            {
                                frmAdvancePayment.WindowState = FormWindowState.Normal;
                                frmAdvancePayment.MdiParent = formMDI.MDIObj;
                                frmAdvancePayment.CallFromDayBook(this, decMasterId);
                            }
                            else
                            {
                                if (open.WindowState == FormWindowState.Minimized)
                                {
                                    open.WindowState = FormWindowState.Normal;
                                }
                                open.CallFromDayBook(this, decMasterId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB18:" + ex.Message;
            }
        }

        /// <summary>
        /// On 'Export' button click to export the report to Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                ExportNew ex = new ExportNew();
                ex.ExportExcel(dgvDayBook, "Day Book", 0, 0, "Excel", null, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB19:" + ex.Message;
            }
        }
        #endregion
        #region Navigations
        /// <summary>
        /// For shortcut keys
        /// Esc for formclosing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDayBook_KeyDown(object sender, KeyEventArgs e)
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
                if (e.KeyCode == Keys.P && Control.ModifierKeys == Keys.Control)
                {
                    btnPrint.PerformClick();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB20:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of txtToDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtToDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (txtToDate.Text == string.Empty || txtToDate.SelectionStart == 0)
                    {
                        txtFromDate.Focus();
                        txtFromDate.SelectionLength = 0;
                        txtFromDate.SelectionStart = 0;
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    cmbVoucherType.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB21:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of cmbVoucherType
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbVoucherType_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    txtToDate.Focus();
                    txtToDate.SelectionLength = 0;
                    txtToDate.SelectionStart = 0;
                }
                if (e.KeyCode == Keys.Enter)
                {
                    cmbLedger.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB22:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of cmbLedger
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbLedger_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    rbtnCondensed.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbVoucherType.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB23:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey navigation of cmbLedger
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFromDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtToDate.Focus();
                    txtToDate.SelectionLength = 0;
                    txtToDate.SelectionStart = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB24:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of rbtnCondensed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnCondensed_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    rbtnDetailed.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbLedger.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB25:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of rbtnDetailed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnDetailed_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    rbtnCondensed.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB26:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of btnSearch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    dgvDayBook.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    rbtnDetailed.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB27:" + ex.Message;
            }
        }
        /// <summary>
        /// backspace navigation of btnReset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    btnSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB28:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of dgvDayBook
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDayBook_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (dgvDayBook.CurrentCell == dgvDayBook.Rows[0].Cells["Sl No"])
                    {
                        btnReset.Focus();
                        dgvDayBook.ClearSelection();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DB29:" + ex.Message;
            }
        }
        #endregion


    }
}
