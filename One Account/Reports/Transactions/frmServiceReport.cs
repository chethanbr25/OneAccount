//This is a source code or part of Oneaccount project
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
    public partial class frmServiceReport : Form
    {
        #region Functions
        /// <summary>
        /// Creates an instance of frmServiceReport class
        /// </summary>
        public frmServiceReport()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill controls on Datetimepicker closes
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="dtp"></param>
        public void dtpCloseUpEventFunction(TextBox txt, DateTimePicker dtp)
        {
            try
            {
                txt.Text = dtp.Value.ToString("dd-MMM-yyyy");
                txt.Focus();
                txt.SelectionStart = 0;
                txt.SelectionLength = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to closeform as per settings 
        /// </summary>
        public void FormClose()
        {
            try
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
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT3:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill dates
        /// </summary>
        public void DateFill()
        {
            try
            {
                dtpFromDate.MinDate = PublicVariables._dtFromDate;
                dtpFromDate.MaxDate = PublicVariables._dtToDate;
                dtpToDate.MinDate = PublicVariables._dtFromDate;
                dtpToDate.MaxDate = PublicVariables._dtToDate;
                dtpFromDate.Value = PublicVariables._dtFromDate;
                dtpToDate.Value = PublicVariables._dtToDate;
                txtFromDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                txtToDate.Text = PublicVariables._dtToDate.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT4:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call other functions to reset form
        /// </summary>
        public void Clear()
        {
            try
            {
                DateFill();
                VoucherTypeComboFill();
                CashOrPartyComboFill();
                SalesmanComboFill();
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT5:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill VoucherType combobox
        /// </summary>
        public void VoucherTypeComboFill()
        {
            try
            {
                DataTable dtblVoucherTypes = new DataTable();
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                dtblVoucherTypes = spVoucherType.VoucherTypeSelectionComboFill("Service Voucher");
                DataRow dr = dtblVoucherTypes.NewRow();
                dr["voucherTypeId"] = 0;
                dr["voucherTypeName"] = "All";
                dtblVoucherTypes.Rows.InsertAt(dr, 0);
                cmbVoucherType.DataSource = dtblVoucherTypes;
                cmbVoucherType.DisplayMember = "voucherTypeName";
                cmbVoucherType.ValueMember = "voucherTypeId";
                cmbVoucherType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT6:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Cash/Party combobox
        /// </summary>
        public void CashOrPartyComboFill()
        {
            try
            {
                TransactionsGeneralFill TransactionGeneralFillObj = new TransactionsGeneralFill();
                TransactionGeneralFillObj.CashOrPartyUnderSundryDrComboFill(cmbCashOrParty, true);
                cmbCashOrParty.SelectedValue = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT7:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Salesman combobox
        /// </summary>
        public void SalesmanComboFill()
        {
            try
            {
                DataTable dtblSalesmen = new DataTable();
                EmployeeSP spEmployee = new EmployeeSP();
                dtblSalesmen = spEmployee.EmployeeViewSalesman();
                DataRow dr = dtblSalesmen.NewRow();
                dr["employeeId"] = 0;
                dr["employeeName"] = "All";
                dtblSalesmen.Rows.InsertAt(dr, 0);
                cmbSalesman.DataSource = dtblSalesmen;
                cmbSalesman.DisplayMember = "employeeName";
                cmbSalesman.ValueMember = "employeeId";
                cmbSalesman.SelectedValue = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT8:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Datagridview
        /// </summary>
        public void GridFill()
        {
            try
            {
                ServiceMasterSP spServiceMaster = new ServiceMasterSP();
                DataTable dtbl = new DataTable();
                dtbl = spServiceMaster.ServiceReportSearch(Convert.ToDateTime(dtpFromDate.Value.ToString()), Convert.ToDateTime(dtpToDate.Value.ToString()), cmbVoucherType.Text.Trim(), cmbCashOrParty.Text.ToString(), cmbSalesman.Text.ToString());
                dgvServiceReport.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT9:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call Clear function
        /// </summary>
        public void Reset()
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT10:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to check or validate dates
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="dtp"></param>
        public void DateValidation(TextBox txt, DateTimePicker dtp)
        {
            try
            {
                DateValidation obj = new DateValidation();
                obj.DateValidationFunction(txt);
                if (txtFromDate.Text == String.Empty)
                {
                    txt.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                }
                if (txtToDate.Text == String.Empty)
                {
                    txt.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                dtp.Value = DateTime.Parse(txt.Text);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT11:" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmServiceReport_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT12:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Search' button clcik fills datagirdview
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
                formMDI.infoError.ErrorString = "SVRT13:" + ex.Message;
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
                Reset();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT14:" + ex.Message;
            }
        }
        /// <summary>
        /// on "print'  button click for print
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvServiceReport.RowCount > 0)
                {
                    DataSet dsServiceVoucherReport = new DataSet();
                    CompanySP spCompany = new CompanySP();
                    frmReport reportobj = new frmReport();
                    ServiceMasterSP spServiceMaster = new ServiceMasterSP();
                    DataTable dtblCompany = spCompany.CompanyViewDataTable(1);
                    DataTable dtblServiceVoucherReport = spServiceMaster.ServiceReport(Convert.ToDateTime(dtpFromDate.Value.ToString()), Convert.ToDateTime(dtpToDate.Value.ToString()), cmbVoucherType.Text, cmbCashOrParty.Text, cmbSalesman.Text);
                    dsServiceVoucherReport.Tables.Add(dtblCompany);
                    dsServiceVoucherReport.Tables.Add(dtblServiceVoucherReport);
                    reportobj.MdiParent = formMDI.MDIObj;
                    reportobj.ServiceVoucherReport(dsServiceVoucherReport);
                }
                else
                {
                    Messages.InformationMessage("No data found");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT15:" + ex.Message;
            }
        }
        /// <summary>
        /// validates date on Datetimepicker closes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpFromDate_CloseUp(object sender, EventArgs e)
        {
            try
            {
                dtpCloseUpEventFunction(txtFromDate, dtpFromDate);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT16:" + ex.Message;
            }
        }
        /// <summary>
        /// validates date on Datetimepicker closes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpToDate_CloseUp(object sender, EventArgs e)
        {
            try
            {
                dtpCloseUpEventFunction(txtToDate, dtpToDate);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT17:" + ex.Message;
            }
        }
        /// <summary>
        /// Calls corresponding ServiceVoucher for updation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvServiceReport_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    frmServiceVoucher objfrmServiceVoucher = new frmServiceVoucher();
                    if (dgvServiceReport.CurrentRow.Index == e.RowIndex)
                    {
                        decimal decMasterId = Convert.ToDecimal(dgvServiceReport.Rows[e.RowIndex].Cells["dgvtxtServiceMasterId"].Value.ToString());
                        decimal decVoucherNo = Convert.ToDecimal(dgvServiceReport.CurrentRow.Cells["dgvtxtVoucherNo"].Value.ToString());
                        frmServiceVoucher open = Application.OpenForms["frmServiceVoucher"] as frmServiceVoucher;
                        if (open == null)
                        {
                            objfrmServiceVoucher.MdiParent = formMDI.MDIObj;
                            objfrmServiceVoucher.WindowState = FormWindowState.Normal;
                            objfrmServiceVoucher.CallFromServiceReport(this, decMasterId, decVoucherNo);
                        }
                        else
                        {
                            open.CallFromServiceReport(this, decMasterId, decVoucherNo);
                            if (open.WindowState == FormWindowState.Minimized)
                            {
                                open.WindowState = FormWindowState.Normal;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT18:" + ex.Message;
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
                DateValidation(txtFromDate, dtpFromDate);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT19:" + ex.Message;
            }
        }
        /// <summary>
        /// Date validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtToDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation(txtToDate, dtpToDate);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT20:" + ex.Message;
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
                ex.ExportExcel(dgvServiceReport, "Service Report", 0, 0, "Excel", null, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT21:" + ex.Message;
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// Enter key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvServiceReport_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    btnPrint.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT22:" + ex.Message;
            }
        }
        //Escape key navigation
        private void frmServiceReport_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    FormClose();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT23:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key navigation
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
                    txtToDate.SelectionStart = txtToDate.Text.Length;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT24:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbVoucherType_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbCashOrParty.Focus();
                    cmbCashOrParty.SelectionLength = 0;
                    cmbCashOrParty.SelectionStart = cmbCashOrParty.Text.Length;
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (cmbCashOrParty.Text == string.Empty || cmbCashOrParty.SelectionStart == 0)
                    {
                        txtToDate.SelectionStart = 0;
                        txtToDate.SelectionLength = 0;
                        txtToDate.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT25:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCashOrParty_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbSalesman.Focus();
                    cmbSalesman.SelectionLength = 0;
                    cmbSalesman.SelectionStart = cmbSalesman.Text.Length;
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (cmbCashOrParty.Text == string.Empty || cmbCashOrParty.SelectionStart == 0)
                    {
                        cmbVoucherType.SelectionStart = 0;
                        cmbVoucherType.SelectionLength = 0;
                        cmbVoucherType.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT26:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSalesman_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (cmbSalesman.Text == string.Empty || cmbSalesman.SelectionStart == 0)
                    {
                        cmbCashOrParty.SelectionStart = 0;
                        cmbCashOrParty.SelectionLength = 0;
                        cmbCashOrParty.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT27:" + ex.Message;
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
                    cmbSalesman.Focus();
                    cmbSalesman.SelectionStart = 0;
                    cmbSalesman.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT28:" + ex.Message;
            }
        }
        /// <summary>
        /// Backspace navigation
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
                formMDI.infoError.ErrorString = "SVRT29:" + ex.Message;
            }
        }
        /// <summary>
        /// Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    dgvServiceReport.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT30:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and backspace navigation
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
                    cmbVoucherType.SelectionLength = 0;
                    cmbVoucherType.SelectionStart = cmbVoucherType.Text.Length;
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtToDate.Text == string.Empty || txtToDate.SelectionStart == 0)
                    {
                        txtFromDate.SelectionStart = 0;
                        txtFromDate.SelectionLength = 0;
                        txtFromDate.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SVRT31:" + ex.Message;
            }
        }
        #endregion
       
    }
}
