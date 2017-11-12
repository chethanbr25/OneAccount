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
    public partial class frmProductVsBatchReport : Form
    {
        #region PublicVariable
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        TransactionsGeneralFill TransactionsGeneralFillObj = new TransactionsGeneralFill();
        VoucherTypeSP spVoucherType = new VoucherTypeSP();
        ProductSP spProduct = new ProductSP();
        DateValidation dateValidationObj = new DateValidation();
        DataTable dtbl = new DataTable();
        #endregion
        #region Functions
        /// <summary>
        /// Create an Instance of a frmProductVsBatchReport class
        /// </summary>
        public frmProductVsBatchReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to clear the fields
        /// </summary>
        public void clear()
        {
            try
            {
                dtpFromDate.Value = PublicVariables._dtFromDate;
                dtpFromDate.MinDate = PublicVariables._dtFromDate;
                dtpFromDate.MaxDate = PublicVariables._dtToDate;
                dtpToDate.Value = PublicVariables._dtCurrentDate;
                dtpToDate.MinDate = PublicVariables._dtFromDate;
                dtpToDate.MaxDate = PublicVariables._dtToDate;
                VoucherTypeComboFill();
                ProductGroupComboFill();
                ProductCodeComboFill();
                BatchComboFill();
                txtFromDate.Focus();
                txtVoucherNo.Text = string.Empty;
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PVSBR1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the grid
        /// </summary>
        public void GridFill()
        {
            try
            {
                ProductSP spProduct = new ProductSP();
                DateTime FromDate = this.dtpFromDate.Value;
                DateTime ToDate = this.dtpToDate.Value;
                dtbl = spProduct.ProductVsBatchReportGridFill(Convert.ToDecimal(cmbVoucherType.SelectedValue.ToString()), txtVoucherNo.Text, Convert.ToDecimal(cmbProductGroup.SelectedValue.ToString()), cmbProductCode.Text, Convert.ToDecimal(cmbBatchNo.SelectedValue.ToString()), FromDate, ToDate);
                dgvProductBatch.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PVSBR2:" + ex.Message;
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
                formMDI.infoError.ErrorString = "PVSBR3:" + ex.Message;
            }
        }
        /// <summary>
        /// Productgroup combobox fill
        /// </summary>
        public void ProductGroupComboFill()
        {
            try
            {
                DataTable dtbl = TransactionsGeneralFillObj.ProductGroupViewAll(cmbProductGroup, true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PVSBR4:" + ex.Message;
            }
        }
        /// <summary>
        /// Productcode combobox fill
        /// </summary>
        public void ProductCodeComboFill()
        {
            try
            {
                DataTable dtbl = spProduct.ProductCodeViewAll(cmbProductCode, true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PVSBR5:" + ex.Message;
            }
        }
        /// <summary>
        /// Batch combobox fill
        /// </summary>
        public void BatchComboFill()
        {
            try
            {
                BatchSP spBatch = new BatchSP();
                DataTable dtbl = spBatch.BatchViewAll();
                cmbBatchNo.DataSource = dtbl;
                cmbBatchNo.DisplayMember = "batchNo";
                cmbBatchNo.ValueMember = "batchId";
                DataRow dr = dtbl.NewRow();
                dr["batchId"] = -1;
                dr["batchNo"] = "All";
                dtbl.Rows.InsertAt(dr, 0);
                cmbBatchNo.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PVSBR6:" + ex.Message;
            }
        }
        #endregion
        #region Event
        /// <summary>
        /// On load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmProductVsBatchReport_Load(object sender, EventArgs e)
        {
            try
            {
                clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PVSBR7:" + ex.Message;
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
                        txtToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                        txtFromDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                        DateTime dt;
                        DateTime.TryParse(txtToDate.Text, out dt);
                        dtpToDate.Value = dt;
                        dtpFromDate.Value = dt;
                    }
                }
                else if (txtFromDate.Text == string.Empty)
                {
                    txtFromDate.Text = PublicVariables._dtCurrentDate.ToString();
                    txtToDate.Text = PublicVariables._dtCurrentDate.ToString();
                    DateTime dt;
                    DateTime.TryParse(txtToDate.Text, out dt);
                    dtpToDate.Value = dt;
                    dtpFromDate.Value = dt;
                }
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PVSBR8:" + ex.Message;
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
                DateTime FromDate = this.dtpFromDate.Value;
                DateTime ToDate = this.dtpToDate.Value;
                DataSet ds = new DataSet();
                CompanySP spcompany = new CompanySP();
                DataTable dtblProduct = dtbl.Copy();
                DataTable dtblCompany = spcompany.CompanyViewDataTable(1);
                if (dgvProductBatch.Rows.Count > 0)
                {
                    DataTable dtblNew = dtbl.Copy();
                    ds.Tables.Add(dtblCompany);
                    ds.Tables.Add(dtblProduct);
                    frmReport frmReport = new frmReport();
                    frmReport.MdiParent = formMDI.MDIObj;
                    frmReport.ProductBatchReportPrinting(ds);
                }
                else
                {
                    Messages.InformationMessage("No data found");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PVSBR9:" + ex.Message;
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
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PVSBR10:" + ex.Message;
            }
        }
        /// <summary>
        /// On value change of dtpFromDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime datetime = this.dtpFromDate.Value;
                txtFromDate.Text = datetime.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PVSBR11:" + ex.Message;
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
                DateTime datetime = this.dtpToDate.Value;
                txtToDate.Text = datetime.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PVSBR12:" + ex.Message;
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
                dateValidationObj.DateValidationFunction(txtFromDate);
                if (txtFromDate.Text == string.Empty)
                {
                    txtFromDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                string strdate = txtFromDate.Text;
                dtpFromDate.Value = Convert.ToDateTime(strdate);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PVSBR13:" + ex.Message;
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
                dateValidationObj.DateValidationFunction(txtToDate);
                if (txtToDate.Text == string.Empty)
                {
                    txtToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                string strdate = txtToDate.Text;
                dtpToDate.Value = Convert.ToDateTime(strdate);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PVSBR14:" + ex.Message;
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
                ex.ExportExcel(dgvProductBatch, "Product Batch Report", 0, 0, "Excel", null, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PVSBR15:" + ex.Message;
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
                formMDI.infoError.ErrorString = "PVSBR16:" + ex.Message;
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
                if (e.KeyCode == Keys.Enter)
                {
                    cmbVoucherType.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtToDate.Text == string.Empty || txtToDate.SelectionLength == 0)
                    {
                        txtFromDate.Focus();
                        txtFromDate.SelectionLength = 0;
                        txtFromDate.SelectionStart = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PVSBR17:" + ex.Message;
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
                if (e.KeyCode == Keys.Enter)
                {
                    txtVoucherNo.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtToDate.Focus();
                    txtToDate.SelectionStart = 0;
                    txtToDate.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PVSBR18:" + ex.Message;
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
                if (e.KeyCode == Keys.Enter)
                {
                    cmbProductGroup.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtVoucherNo.Text == string.Empty)
                    {
                        cmbVoucherType.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PVSBR19:" + ex.Message;
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
                if (e.KeyCode == Keys.Enter)
                {
                    cmbProductCode.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtVoucherNo.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PVSBR20:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of cmbProductCode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbProductCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbBatchNo.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbProductGroup.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PVSBR21:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of cmbBatchNo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBatchNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbProductCode.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PVSBR22:" + ex.Message;
            }
        }
        /// <summary>
        /// Esc for Formclose
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmProductVsBatchReport_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "PVSBR23:" + ex.Message;
            }
        }
        #endregion
    }
}
        
