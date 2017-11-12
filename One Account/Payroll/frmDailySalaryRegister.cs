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
    public partial class frmDailySalaryRegister : Form
    {
        #region Public Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        int q = 0;
        int inCurrenRowIndex = 0;

        #endregion

        #region Function
        /// <summary>
        /// Creates an instance of frmDailySalaryRegister class
        /// </summary>
        public frmDailySalaryRegister()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to reset the form
        /// </summary>
        public void Clear()
        {
            try
            {
                txtSalaryDateFrom.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                txtSalaryDateTo.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                txtVoucherDateFrom.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                txtVoucherDateTo.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                dtpSalaryDateFrom.Value = PublicVariables._dtCurrentDate;
                dtpSalaryDateFrom.MinDate = PublicVariables._dtFromDate;
                dtpSalaryDateFrom.MaxDate = PublicVariables._dtToDate;
                dtpSalaryDateTo.Value = PublicVariables._dtCurrentDate;
                dtpSalaryDateTo.MinDate = PublicVariables._dtFromDate;
                dtpSalaryDateTo.MaxDate = PublicVariables._dtToDate;
                dtpVoucherDateFrom.Value = PublicVariables._dtCurrentDate;
                dtpVoucherDateFrom.MinDate = PublicVariables._dtFromDate;
                dtpVoucherDateFrom.MaxDate = PublicVariables._dtToDate;
                dtpVoucherDateTo.Value = PublicVariables._dtCurrentDate;
                dtpVoucherDateTo.MinDate = PublicVariables._dtFromDate;
                dtpVoucherDateTo.MaxDate = PublicVariables._dtToDate;
                txtVoucherNo.Text = string.Empty;
                txtSalaryDateFrom.Focus();
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSR1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Datagridview
        /// </summary>
        public void GridFill()
        {
            try
            {
                DataTable dtblDailySalaryRegister = new DataTable();
                DailySalaryVoucherMasterSP spDailySalaryVoucherMasterSP = new DailySalaryVoucherMasterSP();
                dtblDailySalaryRegister = spDailySalaryVoucherMasterSP.DailySalaryRegisterSearch(Convert.ToDateTime(txtVoucherDateFrom.Text.ToString()), Convert.ToDateTime(txtVoucherDateTo.Text.ToString()), Convert.ToDateTime(txtSalaryDateFrom.Text.ToString()), Convert.ToDateTime(txtSalaryDateTo.Text.ToString()), txtVoucherNo.Text);
                dgvDailySalaryRegister.DataSource = dtblDailySalaryRegister;

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSR2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to select the row in Datagridview
        /// </summary>
        public void GridSelection()
        {
            try
            {
                if (inCurrenRowIndex >= 0 && dgvDailySalaryRegister.Rows.Count > 0 && inCurrenRowIndex < dgvDailySalaryRegister.Rows.Count)
                {
                    dgvDailySalaryRegister.CurrentCell = dgvDailySalaryRegister.Rows[inCurrenRowIndex].Cells[0];
                    dgvDailySalaryRegister.CurrentCell.Selected = true;
                }
                else
                {
                    inCurrenRowIndex = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSR3:" + ex.Message;
            }
        }
        #endregion

        #region Events
       /// <summary>
       /// On 'Clear' button click
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Clear();
               
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSR4:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Search' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSR5:" + ex.Message;
            }
        }
        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDailySalaryRegister_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
              
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSR6:" + ex.Message;
            }
        }
        /// <summary>
        /// Clears selection on Databind
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDailySalaryRegister_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                dgvDailySalaryRegister.ClearSelection();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSR7:" + ex.Message;
            }
        }
        /// <summary>
        /// Fills Datagridview when form activated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDailySalaryRegister_Activated(object sender, EventArgs e)
        {
            try
            {
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSR8:" + ex.Message;
            }
        }
        /// <summary>
        /// Calls frmDailySalaryVoucher form when cell double click for updation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDailySalaryRegister_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                if (e.RowIndex != -1)
                {
                    if (dgvDailySalaryRegister.CurrentRow.Index == e.RowIndex)
                    {
                        frmDailySalaryVoucher dailySalaryVoucherObj = new frmDailySalaryVoucher();
                        dailySalaryVoucherObj.MdiParent = formMDI.MDIObj;

                        frmDailySalaryVoucher open = Application.OpenForms["frmDailySalaryVoucher"] as frmDailySalaryVoucher;
                        if (open == null)
                        {
                            dailySalaryVoucherObj = new frmDailySalaryVoucher();
                            dailySalaryVoucherObj.MdiParent = formMDI.MDIObj;
                            dailySalaryVoucherObj.CallFromDailySalaryVoucherRegister(Convert.ToDecimal(dgvDailySalaryRegister.Rows[e.RowIndex].Cells["dgvtxtDailySalaryVoucherMasterId"].Value.ToString()), this);
                        }
                        else
                        {
                            open.MdiParent = formMDI.MDIObj;
                            open.BringToFront();
                            open.CallFromDailySalaryVoucherRegister(Convert.ToDecimal(dgvDailySalaryRegister.Rows[e.RowIndex].Cells["dgvtxtDailySalaryVoucherMasterId"].Value.ToString()), this);
                            if (open.WindowState == FormWindowState.Minimized)
                            {
                                open.WindowState = FormWindowState.Normal;
                            }
                        }
                        inCurrenRowIndex = dgvDailySalaryRegister.CurrentRow.Index;
                    }
                    this.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSR9:" + ex.Message;
            }
        }
        /// <summary>
        /// Fills txtSalaryDateFrom textbox on dtpSalaryDateFrom datetimepicker ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpSalaryDateFrom_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime dtdate = this.dtpSalaryDateFrom.Value;
                dtpSalaryDateFrom.Format = DateTimePickerFormat.Custom;
                this.txtSalaryDateFrom.Text = dtdate.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSR10:" + ex.Message;
            }
        }
        /// <summary>
        /// Date validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSalaryDateFrom_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation objDateValidation = new DateValidation();
                objDateValidation.DateValidationFunction(txtSalaryDateFrom);
                if (txtSalaryDateFrom.Text == string.Empty)
                {
                    txtSalaryDateFrom.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSR11:" + ex.Message;
            }
        }
        /// <summary>
        /// Datevalidation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSalaryDateTo_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation objDateValidation = new DateValidation();
                objDateValidation.DateValidationFunction(txtSalaryDateTo);
                if (txtSalaryDateTo.Text == string.Empty)
                {
                    txtSalaryDateTo.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSR12:" + ex.Message;
            }
        }
        /// <summary>
        /// Fills txtSalaryDateTo textbox on dtpSalaryDateTo datetimepicker ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpSalaryDateTo_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime dtdate = this.dtpSalaryDateFrom.Value;
                dtpSalaryDateFrom.Format = DateTimePickerFormat.Custom;
                this.txtSalaryDateTo.Text = dtdate.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSR13:" + ex.Message;
            }
        }
        /// <summary>
        /// Date validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVoucherDateFrom_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation objDateValidation = new DateValidation();
                objDateValidation.DateValidationFunction(txtVoucherDateFrom);
                if (txtVoucherDateFrom.Text == string.Empty)
                {
                    txtVoucherDateFrom.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSR14:" + ex.Message;
            }
        }
        /// <summary>
        /// Fills txtVoucherDateFrom textbox on dtpVoucherDateFrom datetimepicker ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpVoucherDateFrom_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime dtdate = this.dtpSalaryDateFrom.Value;
                dtpSalaryDateFrom.Format = DateTimePickerFormat.Custom;
                this.txtVoucherDateFrom.Text = dtdate.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSR15:" + ex.Message;
            }
        }
        /// <summary>
        /// Date validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVoucherDateTo_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation objDateValidation = new DateValidation();
                objDateValidation.DateValidationFunction(txtVoucherDateTo);
                if (txtVoucherDateTo.Text == string.Empty)
                {
                    txtVoucherDateTo.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSR16:" + ex.Message;
            }
        }
        /// <summary>
        /// Fills txtVoucherDateTo textbox on dtpVoucherDateTo datetimepicker ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpVoucherDateTo_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime dtdate = this.dtpSalaryDateFrom.Value;
                dtpSalaryDateFrom.Format = DateTimePickerFormat.Custom;
                this.txtVoucherDateTo.Text = dtdate.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSR17:" + ex.Message;
            }
        }
        #endregion

        #region Navigation
        /// <summary>
        /// Escape key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDailySalaryRegister_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "DSR18:" + ex.Message;
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
                    btnSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtVoucherNo.Text == string.Empty || txtVoucherNo.SelectionStart == 0)
                    {
                        txtVoucherDateTo.SelectionStart = 0;
                        txtVoucherDateTo.SelectionLength = 0;
                        txtVoucherDateTo.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSR19:" + ex.Message;
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
                    txtVoucherNo.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSR20:" + ex.Message;
            }
        }

       /// <summary>
       /// Enter key navigation
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void dgvDailySalaryRegister_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvDailySalaryRegister.CurrentRow != null)
                    {
                        DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(dgvDailySalaryRegister.CurrentCell.ColumnIndex, dgvDailySalaryRegister.CurrentCell.RowIndex);
                        dgvDailySalaryRegister_CellDoubleClick(sender, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSR21:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSalaryDateFrom_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtSalaryDateTo.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSR22:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSalaryDateTo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtVoucherDateFrom.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtSalaryDateTo.Text == string.Empty || txtSalaryDateTo.SelectionStart == 0)
                    {
                        txtSalaryDateFrom.SelectionStart = 0;
                        txtSalaryDateFrom.SelectionLength = 0;
                        txtSalaryDateFrom.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSR23:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVoucherDateFrom_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtVoucherDateTo.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtVoucherDateFrom.Text == string.Empty || txtVoucherDateFrom.SelectionStart == 0)
                    {
                        txtSalaryDateTo.SelectionStart = 0;
                        txtSalaryDateTo.SelectionLength = 0;
                        txtSalaryDateTo.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSR24:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVoucherDateTo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtVoucherNo.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtVoucherDateTo.Text == string.Empty || txtVoucherDateTo.SelectionStart == 0)
                    {
                        txtVoucherDateFrom.SelectionStart = 0;
                        txtVoucherDateFrom.SelectionLength = 0;
                        txtVoucherDateFrom.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSR25:" + ex.Message;
            }
        }
 
        #endregion
    }
}
