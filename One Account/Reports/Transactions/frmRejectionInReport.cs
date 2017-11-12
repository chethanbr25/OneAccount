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
using System.Collections;
namespace One_Account
{
    public partial class frmRejectionInReport : Form
    {
        #region Public Variables and Instantiation
        /// <summary>
        /// Public Variable declaration
        /// </summary>
        DateTime dtFromDate, dtToDate;
        decimal decVoucherType, decCashOrParty, decDeliveryNoteNo, decSalesManId;
        string strVocherNo, strProductCode;
        /// <summary>
        /// Create fromRejectionInReport instance
        /// </summary>
        public frmRejectionInReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to set default value in contorls in form load
        /// </summary>
        public void LoadRejectionInReport()
        {
            try
            {
                txtFromDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                txtToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                txtVoucherNo.Text = String.Empty;
                txtProductCode.Text = String.Empty;
                dtFromDate = Convert.ToDateTime(txtFromDate.Text);
                dtToDate = Convert.ToDateTime(txtToDate.Text);
                decVoucherType = Convert.ToDecimal(cmbVoucherType.SelectedValue);
                strVocherNo = txtVoucherNo.Text;
                decCashOrParty = Convert.ToDecimal(cmbCashorParty.SelectedValue);
                if (cmbDeliveryNoteNo.SelectedIndex != -1)
                {
                    decDeliveryNoteNo = Convert.ToDecimal(cmbDeliveryNoteNo.SelectedValue.ToString());
                }
                decSalesManId = Convert.ToDecimal(cmbSalesMan.SelectedValue);
                strProductCode = txtProductCode.Text;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fetch ladger Name Cash/Party ComboBox 
        /// </summary>
        public void CashOrPartyComboFill()
        {
            TransactionsGeneralFill transactionGeneralFillObj = new TransactionsGeneralFill();
            try
            {
                transactionGeneralFillObj.CashOrPartyUnderSundryDrComboFill(cmbCashorParty, true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Salesman combobox
        /// </summary>
        public void salesmancombofill()
        {
            TransactionsGeneralFill transactionGeneralFillObj = new TransactionsGeneralFill();
            try
            {
                transactionGeneralFillObj.SalesmanViewAllForComboFill(cmbSalesMan, true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport3:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Datagirdview
        /// </summary>
        public void RejectionInReportFill()
        {
            try
            {
                RejectionInMasterSP SpRejectionInMaster = new RejectionInMasterSP();
                try
                {
                    decimal decDeliveryNoteNo = 0;
                    if (cmbDeliveryNoteNo.SelectedIndex != -1)
                    {
                        decDeliveryNoteNo = Convert.ToDecimal(cmbDeliveryNoteNo.SelectedValue.ToString());
                    }
                    string strVoucherNo = txtVoucherNo.Text;
                    string strProductCode = txtProductCode.Text;
                    dgvRejectionInReport.DataSource = SpRejectionInMaster.RejectionInReportFill(Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text), Convert.ToDecimal(cmbVoucherType.SelectedValue.ToString()), strVoucherNo, Convert.ToDecimal(cmbCashorParty.SelectedValue.ToString()), decDeliveryNoteNo, Convert.ToDecimal(cmbSalesMan.SelectedValue.ToString()), strProductCode);
                    txtFromDate.Focus();
                }
                catch (Exception ex)
                {
                    formMDI.infoError.ErrorString = "RIReport4:" + ex.Message;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport5:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill VoucherType combobox
        /// </summary>
        public void VoucherTypeComboFill()
        {
            RejectionInMasterSP spRejectionInMaster = new RejectionInMasterSP();
            try
            {
                spRejectionInMaster.VoucherTypeSelectionFill(cmbVoucherType, "Rejection In", true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport6:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill DeliveryNote No combobox
        /// </summary>
        public void DeliveryNoteComboFill()
        {

            decimal decLedgerId = 0;
            RejectionInMasterSP spRejectionInMaster = new RejectionInMasterSP();
            try
            {
                spRejectionInMaster.DeliveryNoteNoComboFillToLedger(cmbDeliveryNoteNo, decLedgerId, true);

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport7:" + ex.Message;
            }
        }
        public void GridDoubleClick()
        {
            try
            {
                if (dgvRejectionInReport.CurrentRow != null)
                {
                    decimal decMasterId = Convert.ToDecimal(dgvRejectionInReport.CurrentRow.Cells["dgvtxtRejectionInMaterId"].Value.ToString());
                    frmRejectionIn frmRejectionInObj = new frmRejectionIn();
                    frmRejectionIn frmRejectionInOpen = Application.OpenForms["frmRejectionIn"] as frmRejectionIn;
                    if (frmRejectionInOpen == null)
                    {
                        frmRejectionInObj.MdiParent = formMDI.MDIObj;
                        frmRejectionInObj.CallFromRejectionInReport(this, decMasterId);
                    }
                    else
                    {
                        frmRejectionInOpen.CallFromRejectionInReport(this, decMasterId);
                        if (frmRejectionInOpen.WindowState == FormWindowState.Minimized)
                        {
                            frmRejectionInOpen.WindowState = FormWindowState.Normal;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport8:" + ex.Message;
            }
        }
        /// <summary>
        /// Function for Print
        /// </summary>
        public void PrintReport()
        {
            try
            {
                RejectionInMasterSP SpRejectionInMaster = new RejectionInMasterSP();
                DataSet dsRejectionInReport = SpRejectionInMaster.RejectionInReportPrinting(dtFromDate, dtToDate, decVoucherType, strVocherNo, decCashOrParty, decDeliveryNoteNo, decSalesManId, strProductCode);
                frmReport frmReport = new frmReport();
                frmReport.MdiParent = formMDI.MDIObj;
                frmReport.RejectionInReportPrinting(dsRejectionInReport);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport9:" + ex.Message;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmRejectionInReport_Load(object sender, EventArgs e)
        {
            try
            {
                VoucherTypeComboFill();
                CashOrPartyComboFill();
                salesmancombofill();
                LoadRejectionInReport();
                RejectionInReportFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport10:" + ex.Message;
            }
        }
        /// <summary>
        /// Fills txtFromDate textbox on dtpFrmDate datetimepicker ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpFrmDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                txtFromDate.Text = dtpFrmDate.Value.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport11:" + ex.Message;
            }
        }
        /// <summary>
        /// Fills txtToDate textbox on dtpToDate datetimepicker ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                txtToDate.Text = dtpToDate.Value.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport12:" + ex.Message;
            }
        }
        /// <summary>
        /// DateValidation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFromDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation dateValidationObj = new DateValidation();
                dateValidationObj.DateValidationFunction(txtFromDate);

                if (txtFromDate.Text == String.Empty)
                {
                    txtFromDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                }
                dtpFrmDate.Value = DateTime.Parse(txtFromDate.Text);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport13:" + ex.Message;
            }
        }
        /// <summary>
        /// DateValidation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtToDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation dateValidationObj = new DateValidation();
                dateValidationObj.DateValidationFunction(txtToDate);

                if (txtToDate.Text == String.Empty)
                {
                    txtToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }

                dtpToDate.Value = DateTime.Parse(txtToDate.Text);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport14:" + ex.Message;
            }
        }
        /// <summary>
        /// Fills DeliveryNoteNo combobox on cmbCashorParty combobx SelectedValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCashorParty_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCashorParty.SelectedValue != null)
                {
                    if (cmbCashorParty.SelectedValue.ToString() != "System.Data.DataRowView" && cmbCashorParty.Text != "System.Data.DataRowView")
                    {
                        DeliveryNoteComboFill();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport15:" + ex.Message;
            }
        }
        /// <summary>
        /// Fills Datagirdview on 'Search' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DateValidation ObjValidation = new DateValidation();
                ObjValidation.DateValidationFunction(txtToDate);
                dtFromDate = Convert.ToDateTime(txtFromDate.Text);
                dtToDate = Convert.ToDateTime(txtToDate.Text);
                decVoucherType = Convert.ToDecimal(cmbVoucherType.SelectedValue);
                strVocherNo = txtVoucherNo.Text;
                decCashOrParty = Convert.ToDecimal(cmbCashorParty.SelectedValue);
                if (cmbDeliveryNoteNo.SelectedIndex != -1)
                {
                    decDeliveryNoteNo = Convert.ToDecimal(cmbDeliveryNoteNo.SelectedValue.ToString());
                }
                decSalesManId = Convert.ToDecimal(cmbSalesMan.SelectedValue);
                strProductCode = txtProductCode.Text;
                if (Convert.ToDateTime(txtToDate.Text) < Convert.ToDateTime(txtFromDate.Text))
                {
                    MessageBox.Show("todate should be greater than fromdate", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtToDate.Text = PublicVariables._dtToDate.ToString("dd-MMM-yyyy");
                    txtFromDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                    DateTime dt;
                    DateTime.TryParse(txtToDate.Text, out dt);
                    dtpToDate.Value = dt;
                    RejectionInReportFill();
                }
                else
                {
                    DateTime dt;
                    DateTime.TryParse(txtToDate.Text, out dt);
                    dtpToDate.Value = dt;
                    RejectionInReportFill();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport16:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Reset' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                VoucherTypeComboFill();
                CashOrPartyComboFill();
                salesmancombofill();
                LoadRejectionInReport();
                RejectionInReportFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport17:" + ex.Message;
            }
        }
        /// <summary>
        /// Calls corresponding RejectionIn voucher for updation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRejectionInReport_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                GridDoubleClick();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport18:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Print' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvRejectionInReport.Rows.Count > 0)
                {
                    PrintReport();
                }
                else
                {
                    Messages.InformationMessage("No data found");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport19:" + ex.Message;
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
                ex.ExportExcel(dgvRejectionInReport, "Rejection In Report", 0, 0, "Excel", null, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport20:" + ex.Message;
            }
        }
        #endregion

        #region Navigation
        /// <summary>
        /// Escape key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmRejectionInReport_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "RIReport21:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
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
                    txtToDate.SelectionStart = txtToDate.TextLength;
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtFromDate.SelectionStart == 0 || txtFromDate.Text == string.Empty)
                    {
                        txtFromDate.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport22:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtToDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbVoucherType.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtToDate.SelectionLength != 11)
                    {
                        if (txtToDate.SelectionStart == 0 || txtToDate.Text == string.Empty)
                        {
                            txtFromDate.Focus();
                            txtFromDate.SelectionStart = txtFromDate.TextLength;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport23:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbVoucherType_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtVoucherNo.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (cmbVoucherType.SelectionStart == 0)
                    {
                        txtToDate.Focus();
                        txtToDate.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport24:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVoucherNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbCashorParty.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtVoucherNo.SelectionStart == 0)
                        cmbVoucherType.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport25:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCashorParty_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbDeliveryNoteNo.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    txtVoucherNo.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport26:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDeliveryNoteNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbSalesMan.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    cmbCashorParty.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport27:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtProductCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtProductCode.Text.Trim() == string.Empty || txtProductCode.SelectionStart == 0)
                    {
                        cmbSalesMan.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport28:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSalesMan_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtProductCode.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    cmbDeliveryNoteNo.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport29:" + ex.Message;
            }
        }
        /// <summary>
        /// Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    txtProductCode.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport30:" + ex.Message;
            }
        }
     
        private void dgvRejectionInReport_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    GridDoubleClick();  
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIReport31:" + ex.Message;
            }

        }

        #endregion

    }
}
