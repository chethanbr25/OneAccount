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
    public partial class frmFreeSaleReport : Form
    {
        
       
        #region public variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        TransactionsGeneralFill TransactionGeneralFillObj = new TransactionsGeneralFill();
        TransactionsGeneralFill TransactionsGeneralFillObj = new TransactionsGeneralFill();
        VoucherTypeSP spVoucherType = new VoucherTypeSP();
        ProductSP spproduct = new ProductSP();
        SalesMasterSP spsalemaster = new SalesMasterSP();
        bool isFormLoad = false;
        #endregion
        
        #region Functions
        /// <summary>
        /// Create an Instance of a frmDayBook class
        /// </summary>
        public frmFreeSaleReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to fill the grid
        /// </summary>
        public void GridFill()
        {
            try
            {
                SalesMasterSP spSalesMaster = new SalesMasterSP();
                DataTable dtbl = new DataTable();
                DateTime fromDate, toDate;
                fromDate = Convert.ToDateTime(txtFromDate.Text);
                toDate = Convert.ToDateTime(txtToDate.Text);
                string voucherNo = txtVoucherNo.Text.ToString();
                decimal voucherTypeId = Convert.ToDecimal(cmbVoucherType.SelectedValue.ToString());
                decimal groupId = Convert.ToDecimal(cmbProductGroup.SelectedValue.ToString());
                decimal ledgerId = Convert.ToDecimal(cmbCashOrParty.SelectedValue.ToString());
                decimal employeeId = Convert.ToDecimal(cmbSalesMan.SelectedValue.ToString());
                string productCode = cmbProductCode.Text.ToString();
                dgvFreeSalesReport.DataSource = spSalesMaster.FreeSaleReportGridFill(fromDate, toDate, voucherNo, voucherTypeId, groupId, productCode, ledgerId, employeeId);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "FSR1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to reset the form
        /// </summary>
        public void reset()
        {
            try
            {
                dtpFrmDate.Value = PublicVariables._dtFromDate;
                dtpToDate.Value = PublicVariables._dtCurrentDate;
                txtVoucherNo.Text = string.Empty;
                txtFromDate.Focus();
                productComboFill();
                ProductCodeComboFill();
                salesManComboFill();
                CashorPartyComboFill();
                VoucherTypeComboFill();
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "FSR2:" + ex.Message;
            }
        }
        /// <summary>
        /// Vouchertype combobox fill
        /// </summary>
        public void VoucherTypeComboFill()
        {
            try
            {
                spVoucherType.voucherTypeComboFill(cmbVoucherType, true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "FSR3:" + ex.Message;
            }
        }
        /// <summary>
        /// Productcode combobox fill
        /// </summary>
        public void ProductCodeComboFill()
        {
            try
            {
                DataTable dtbl = spproduct.ProductCodeViewAll(cmbProductCode, true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "FSR4:" + ex.Message;
            }
        }
        /// <summary>
        /// Product combobox fill
        /// </summary>
        public void productComboFill()
        {
            try
            {
                DataTable dtbl = TransactionsGeneralFillObj.ProductGroupViewAll(cmbProductGroup, true);
                cmbProductGroup.DataSource = dtbl;
                cmbProductGroup.DisplayMember = "groupName";
                cmbProductGroup.ValueMember = "groupId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "FSR5:" + ex.Message;
            }
        }
        /// <summary>
        /// Cash or party combobox fill
        /// </summary>
        public void CashorPartyComboFill()
        {
            try
            {
                TransactionGeneralFillObj.CashOrPartyUnderSundryDrComboFill(cmbCashOrParty, true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "FSR6:" + ex.Message;
            }
        }
        /// <summary>
        /// Salesman combobox fill
        /// </summary>
        public void salesManComboFill()
        {
            try
            {
                TransactionGeneralFillObj.SalesmanViewAllForComboFill(cmbSalesMan, true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "FSR7:" + ex.Message;
            }
        }
        #endregion
        
        #region Events
       /// <summary>
       /// On reset button click
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                reset();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "FSR8:" + ex.Message;
            }
        }
        /// <summary>
        /// On load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmFreeSaleReport_Load(object sender, EventArgs e)
        {
            try
            {
                isFormLoad = true;
                dtpFrmDate.MinDate = PublicVariables._dtFromDate;
                dtpFrmDate.MaxDate = PublicVariables._dtToDate;
                dtpFrmDate.Value = PublicVariables._dtFromDate;
                dtpToDate.MinDate = PublicVariables._dtFromDate;
                dtpToDate.MaxDate = PublicVariables._dtToDate;
                dtpToDate.Value = PublicVariables._dtCurrentDate;
                txtFromDate.Text = dtpFrmDate.Value.ToString("dd-MMM-yyyy");
                txtToDate.Text = dtpToDate.Value.ToString("dd-MMM-yyyy");
                isFormLoad = false;
                productComboFill();
                ProductCodeComboFill();
                salesManComboFill();
                CashorPartyComboFill();
                VoucherTypeComboFill();
                GridFill();
                txtFromDate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "FSR9:" + ex.Message;
            }
        }
        /// <summary>
        /// On value change of dtpFrmDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpFrmDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpFrmDate.Value;
                this.txtFromDate.Text = date.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "FSR10:" + ex.Message;
            }
        }
        /// <summary>
        /// On value change of dtpToDate
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
                formMDI.infoError.ErrorString = "FSR11:" + ex.Message;
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
                DateValidation objValidation = new DateValidation();
                objValidation.DateValidationFunction(txtFromDate);
                if (txtFromDate.Text == "")
                    txtFromDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                DateTime dt;
                DateTime.TryParse(txtFromDate.Text, out dt);
                dtpFrmDate.Value = dt;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "FSR12:" + ex.Message;
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
                DateValidation objValidation = new DateValidation();
                objValidation.DateValidationFunction(txtToDate);
                if (txtToDate.Text == string.Empty)
                {
                    txtToDate.Text = PublicVariables._dtToDate.ToString("dd-MMM-yyyy");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "FSR13:" + ex.Message;
            }
        }
        /// <summary>
        /// On closeup event of dtpFrmDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpFrmDate_CloseUp(object sender, EventArgs e)
        {
            try
            {
                txtFromDate.Text = dtpFrmDate.Value.ToString("dd-MMM-yyyy");
                txtFromDate.SelectAll();
                txtFromDate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "FSR14:" + ex.Message;
            }
        }
        /// <summary>
        /// On closeup event of dtpToDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpToDate_CloseUp(object sender, EventArgs e)
        {
            try
            {
                txtToDate.Text = dtpToDate.Value.ToString("dd-MMM-yyyy");
                txtToDate.SelectAll();
                txtToDate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "FSR15:" + ex.Message;
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
                if (txtFromDate.Text != string.Empty && txtToDate.Text != string.Empty)
                {
                    if (Convert.ToDateTime(txtToDate.Text) < Convert.ToDateTime(txtFromDate.Text))
                    {
                        MessageBox.Show("Todate should be greater than fromdate", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtToDate.Text = PublicVariables._dtToDate.ToString("dd-MMM-yyyy");
                        txtFromDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                        DateTime dt;
                        DateTime.TryParse(txtToDate.Text, out dt);
                        dtpToDate.Value = dt;
                        dtpFrmDate.Value = dt;
                    }
                }
                else if (txtFromDate.Text == string.Empty)
                {
                    txtFromDate.Text = PublicVariables._dtFromDate.ToString();
                    txtToDate.Text = PublicVariables._dtToDate.ToString();
                    DateTime dt;
                    DateTime.TryParse(txtToDate.Text, out dt);
                    dtpToDate.Value = dt;
                    dtpFrmDate.Value = dt;
                }
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "FSR16:" + ex.Message;
            }
        }
        /// <summary>
        /// On print button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            DataSet dsfree = new DataSet();
            try
            {
                SalesMasterSP spSalesMaster = new SalesMasterSP();
                DateTime fromDate, toDate;
                fromDate = Convert.ToDateTime(txtFromDate.Text);
                toDate = Convert.ToDateTime(txtToDate.Text);
                string voucherNo = txtVoucherNo.Text.ToString();
                decimal voucherTypeId = Convert.ToDecimal(cmbVoucherType.SelectedValue.ToString());
                decimal groupId = Convert.ToDecimal(cmbProductGroup.SelectedValue.ToString());
                decimal companyId = 1; 
                decimal ledgerId = Convert.ToDecimal(cmbCashOrParty.SelectedValue.ToString());
                decimal employeeId = Convert.ToDecimal(cmbSalesMan.SelectedValue.ToString());
                string productCode = cmbProductCode.Text.ToString();
                dsfree = spSalesMaster.FreeSaleReportPrint(fromDate, toDate, voucherNo, voucherTypeId, groupId, productCode, ledgerId, employeeId, companyId);
                frmReport frmReport = new frmReport();
                frmReport.MdiParent = formMDI.MDIObj;
                if (dgvFreeSalesReport.Rows.Count > 0)
                {
                    frmReport.freeSaleReport(dsfree);
                }
                else
                {
                    Messages.InformationMessage("No data found");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "FSR17:" + ex.Message;
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
                ex.ExportExcel(dgvFreeSalesReport, "Free Sale Report", 0, 0, "Excel", null, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "FSR18:" + ex.Message;
            }
        }
        #endregion
        
        #region Navigation
        /// <summary>
        /// Enterkey navigation of txtFromDate
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
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "FSR19:" + ex.Message;
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
                        txtFromDate.SelectionStart = 0;
                        txtFromDate.SelectionLength = 0;
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    cmbVoucherType.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "FSR20:" + ex.Message;
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
                    {
                        txtToDate.Focus();
                        txtToDate.SelectionStart = 0;
                        txtToDate.SelectionLength = 0;
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    txtVoucherNo.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "FSR21:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of txtVoucherNo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVoucherNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (txtVoucherNo.Text == "" || txtVoucherNo.SelectionStart == 0)
                    {
                        cmbVoucherType.Focus();
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    cmbCashOrParty.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "FSR22:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of cmbCashOrParty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCashOrParty_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    {
                        txtVoucherNo.Focus();
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    cmbSalesMan.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "FSR23:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of cmbSalesMan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSalesMan_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    {
                        cmbCashOrParty.Focus();
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    cmbProductGroup.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "FSR24:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of cmbProductGroup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbProductGroup_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    {
                        cmbSalesMan.Focus();
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    cmbProductCode.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "FSR25:" + ex.Message;
            }
        }
        /// <summary>
        /// backspace navigation of cmbProductCode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbProductCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    {
                        cmbProductGroup.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "FSR26:" + ex.Message;
            }
        }
        /// <summary>
        /// Esc for formclose
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmFreeSaleReport_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "FSR27:" + ex.Message;
            }
        }
        #endregion
    }
}
