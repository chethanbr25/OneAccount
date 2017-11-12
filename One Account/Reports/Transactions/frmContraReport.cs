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
    public partial class frmContraReport : Form
    {
        #region PublicVariables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        string strType; 
        #endregion

        #region Functions
        /// <summary>
        /// Creates an instance of frmContraReport class
        /// </summary>
        public frmContraReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to fill Cash/Bank combobox
        /// </summary>
        public void BankOrCashComboFill()
        {
            try
            {
                ContraMasterSP spContraMaster = new ContraMasterSP();
                DataTable dtbl = new DataTable();
                dtbl = spContraMaster.CashOrBankComboFill();
                DataRow dr = dtbl.NewRow();
                dr[0] = "ALL";
                dtbl.Rows.InsertAt(dr, 0);
                cmbCashOrBank.DataSource = dtbl;
                cmbCashOrBank.ValueMember = "ledgerId";
                cmbCashOrBank.DisplayMember = "ledgerName";
                cmbCashOrBank.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRT1:" + ex.Message;
              
            }
        }
        /// <summary>
        /// Function to fill VoucherType combobox
        /// </summary>
        public void VoucherTypeComboFill()
        {
            try
            {
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                DataTable dtblVouchetType = new DataTable();
                dtblVouchetType = spVoucherType.VoucherTypeSelectionComboFill("Contra Voucher");
                DataRow dr = dtblVouchetType.NewRow();
                dr[1] = "ALL";
                dtblVouchetType.Rows.InsertAt(dr, 0);
                cmbVoucherType.DataSource = dtblVouchetType;
                cmbVoucherType.ValueMember = "voucherTypeId";
                cmbVoucherType.DisplayMember = "voucherTypeName";
                cmbVoucherType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRT2:" + ex.Message;

            }
        }
        /// <summary>
        /// Function to fill Datagridview
        /// </summary>
        public void GridFill()
        {
            try
            {
                ContraMasterSP spContraMaster = new ContraMasterSP();
                DataTable dtbl = new DataTable();
                if (rbtnDeposit.Checked)
                {
                    strType = "Deposit";
                }
                else
                {
                    strType = "Withdraw";
                }
                dtbl = spContraMaster.ContraReport(Convert.ToDateTime(dtpFromDate.Value.ToString()), Convert.ToDateTime(dtpToDate.Value.ToString()), cmbVoucherType.Text, cmbCashOrBank.Text, strType);
                dgvContraReport.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRT3:" + ex.Message;

            }
        }
        /// <summary>
        /// Function to reset form
        /// </summary>
        public void Clear()
        {
            try
            {
                BankOrCashComboFill();
                VoucherTypeComboFill();
                rbtnDeposit.Checked = true;
                dtpFromDate.Value = PublicVariables._dtFromDate;
                dtpFromDate.MinDate = PublicVariables._dtFromDate;
                dtpFromDate.MaxDate = PublicVariables._dtToDate;
                dtpToDate.Value = PublicVariables._dtCurrentDate;
                dtpToDate.MinDate = PublicVariables._dtFromDate;
                dtpToDate.MaxDate = PublicVariables._dtToDate;
                txtFromDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                txtToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                cmbVoucherType.SelectedIndex = 0;
                cmbCashOrBank.SelectedIndex = 0;
                GridFill();
                cmbVoucherType.Select();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRT4:" + ex.Message;

            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmContraReport_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRT5:" + ex.Message;

            }
        }
        /// <summary>
        /// On 'Search' button click fills Datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DateValidation ObjValidation = new DateValidation();
                ObjValidation.DateValidationFunction(txtToDate);
                if (Convert.ToDateTime(txtToDate.Text) < Convert.ToDateTime(txtFromDate.Text))
                {
                    MessageBox.Show("todate should be greater than fromdate", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtToDate.Text = PublicVariables._dtToDate.ToString("dd-MMM-yyyy");
                    txtFromDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                    DateTime dt;
                    DateTime.TryParse(txtToDate.Text, out dt);
                    dtpToDate.Value = dt;
                    GridFill();
                }
                else
                {
                    DateTime dt;
                    DateTime.TryParse(txtToDate.Text, out dt);
                    dtpToDate.Value = dt;
                    GridFill();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRT6:" + ex.Message;

            }
        }
        /// <summary>
        /// Fills txtToDate textbox on dtpToDate ValueChanged
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
                formMDI.infoError.ErrorString = "CRT7:" + ex.Message;

            }
        }
        /// <summary>
        /// Fills txtFromDate textbox on dtpFromDate ValueChanged
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
                formMDI.infoError.ErrorString = "CRT8:" + ex.Message;

            }
        }
        /// <summary>
        /// Datevalidation
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
                //---for change date in Date time picker----//
                string strdate = txtToDate.Text;
                dtpToDate.Value = Convert.ToDateTime(strdate.ToString());
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRT9:" + ex.Message;

            }
        }
        /// <summary>
        /// Datevalidation
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
                //---for change date in Date time picker----//
                string strdate = txtFromDate.Text;
                dtpFromDate.Value = Convert.ToDateTime(strdate.ToString());
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRT10:" + ex.Message;

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
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRT11:" + ex.Message;

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
                if (dgvContraReport.RowCount > 0)
                {
                    DataSet ds = new DataSet();
                    CompanySP spCompany = new CompanySP();
                    frmReport reportobj = new frmReport();
                    ContraMasterSP spContraMaster = new ContraMasterSP();
                    DataTable dtblCompany = spCompany.CompanyViewDataTable(1);
                    DataTable dtblCotraReport = spContraMaster.ContraReport(Convert.ToDateTime(dtpFromDate.Value.ToString()), Convert.ToDateTime(dtpToDate.Value.ToString()), cmbVoucherType.Text, cmbCashOrBank.Text, strType);
                    ds.Tables.Add(dtblCompany);
                    ds.Tables.Add(dtblCotraReport);
                    reportobj.MdiParent = formMDI.MDIObj;
                    reportobj.ContraVoucherReport(ds);
                }
                else
                {
                    MessageBox.Show("No data found", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRT12:" + ex.Message;

            }
        }
        /// <summary>
        /// Calls corresponding ContraVoucher to view details and for updation on cell doubleclick in Datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvContraReport_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                frmContraVoucher objfrmContraVoucher = new frmContraVoucher();
                if (e.RowIndex != -1)
                {
                    decimal decMasterId = Convert.ToDecimal(dgvContraReport.Rows[e.RowIndex].Cells["dgvtxtMasterId"].Value.ToString());
                    frmContraVoucher open = Application.OpenForms["frmContraVoucher"] as frmContraVoucher;
                    if (open == null)
                    {
                        objfrmContraVoucher.WindowState = FormWindowState.Normal;
                        objfrmContraVoucher = new frmContraVoucher();
                        objfrmContraVoucher.MdiParent = formMDI.MDIObj;
                        objfrmContraVoucher.CallFromContraReport(this, decMasterId);
                    }
                    else
                    {
                        open.CallFromContraReport(this, decMasterId);
                        if (open.WindowState == FormWindowState.Minimized)
                        {
                            open.WindowState = FormWindowState.Normal;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRT13:" + ex.Message;

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
                ex.ExportExcel(dgvContraReport, "Contra Voucher Report", 0, 0, "Excel", null, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRT14:" + ex.Message;

            }
        }
        #endregion

        #region Navigation
        /// <summary>
        /// Enter key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnDeposit_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    rbtnWithdrawal.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRT15:" + ex.Message;

            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnWithdrawal_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbVoucherType.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    rbtnDeposit.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRT16:" + ex.Message;

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
                    cmbCashOrBank.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    rbtnWithdrawal.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRT17:" + ex.Message;

            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCashOrBank_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtFromDate.Focus();
                    txtFromDate.SelectionLength = 0;
                    txtFromDate.SelectionStart = txtFromDate.Text.Length;
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbVoucherType.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRT18:" + ex.Message;

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
                    txtToDate.SelectionLength = 0;
                    txtToDate.SelectionStart = txtFromDate.Text.Length;
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtFromDate.Text == string.Empty || txtFromDate.SelectionStart == 0)
                    {
                        cmbCashOrBank.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRT19:" + ex.Message;

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
                    btnSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtToDate.Text == string.Empty && txtToDate.SelectionStart == 0)
                    {
                        txtFromDate.Focus();
                        txtFromDate.SelectionLength = 0;
                        txtFromDate.SelectionStart = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRT20:" + ex.Message;

            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvContraReport_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnPrint.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    btnSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRT21:" + ex.Message;

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
                    txtToDate.Focus();
                    txtToDate.SelectionStart = 0;
                    txtToDate.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRT22:" + ex.Message;

            }
        }
        /// <summary>
        /// Escape key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmContraReport_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    if (Messages.CloseConfirmation())
                        this.Close();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRT23:" + ex.Message;

            }
        }
        #endregion


    }
}
