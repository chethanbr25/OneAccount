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
   
    public partial class frmBonusDeductionRegister : Form
    {
        #region Public Variables
        /// <summary>
        /// public variable declaration part
        /// </summary>
        int inCurrentRowIndex = 0;
        frmEmployeePopup frmEmployeeObj;
       
        #endregion

        #region Function
        /// <summary>
        /// Creates an instance of frmBonusDeductionRegister class
        /// </summary>
        public frmBonusDeductionRegister()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to fill EmployeeName combobox
        /// </summary>
        public void EmployeeNameComboFill()
        {
            try
            {
                DataTable dtblEmployeeName = new DataTable();
                EmployeeSP spEmployee = new EmployeeSP();
                dtblEmployeeName = spEmployee.EmployeeViewAll();
                DataRow dr = dtblEmployeeName.NewRow();
                dr[2] = "All";
                dtblEmployeeName.Rows.InsertAt(dr, 0);
                cmbEmployeeName.DataSource = dtblEmployeeName;
                cmbEmployeeName.ValueMember = "employeeId";
                cmbEmployeeName.DisplayMember = "employeeName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to reset the form
        /// </summary>
        public void Clear()
        {
            try
            {
                cmbEmployeeName.SelectedIndex = 0;
                dtpMonth.Value = PublicVariables._dtCurrentDate;
                dtpMonth.MinDate = PublicVariables._dtFromDate;
                dtpMonth.MaxDate = PublicVariables._dtToDate;
                GridFill();
                cmbEmployeeName.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Datagridview
        /// </summary>
        public void GridFill()
        {
            try
            {
                DataTable dtblBonusDeduction = new DataTable();
                BonusDedutionSP spBonusDeduction = new BonusDedutionSP();
                dtblBonusDeduction = spBonusDeduction.BonusDeductionSearch(cmbEmployeeName.Text, Convert.ToDateTime(dtpMonth.Text.ToString()));
                dgvBonusDeduction.DataSource = dtblBonusDeduction;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR3:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to Select the row in Datagridview
        /// </summary>
        public void GridSelection()
        {
            try
            {
                if (inCurrentRowIndex >= 0 && dgvBonusDeduction.Rows.Count > 0 && inCurrentRowIndex < dgvBonusDeduction.Rows.Count)
                {
                    dgvBonusDeduction.CurrentCell = dgvBonusDeduction.Rows[inCurrentRowIndex].Cells[0];
                    dgvBonusDeduction.CurrentCell.Selected = true;
                }
                else
                {
                    inCurrentRowIndex = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR4:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call frmEmployeePopup form to select and view Employee
        /// </summary>
        /// <param name="frmEmployee"></param>
        /// <param name="decId"></param>
        public void CallFromEmployee(frmEmployeePopup frmEmployee, decimal decId) //POP UP
        {
            try
            {
                base.Show();
                this.frmEmployeeObj = frmEmployee;
                EmployeeNameComboFill();
                cmbEmployeeName.SelectedValue = decId;
                cmbEmployeeName.Focus();
                frmEmployeeObj.Close();
                frmEmployeeObj = null;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR5:" + ex.Message;
            }
        }

        #endregion

        #region Events
        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmBonusDeductionRegister_Load(object sender, EventArgs e)
        {
            try
            {
                EmployeeNameComboFill();
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR6:" + ex.Message;
            }
        }
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
                formMDI.infoError.ErrorString = "BDR7:" + ex.Message;
            }
        }
        /// <summary>
        /// Clears selection on databinding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvBonusDeduction_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                dgvBonusDeduction.ClearSelection();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR8:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call frmBonusDeduction form on cell doubleclick for updation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvBonusDeduction_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    if (dgvBonusDeduction.CurrentRow.Index == e.RowIndex)
                    {
                        frmBonusDeduction frmBonusDeductionObj = new frmBonusDeduction();
                        frmBonusDeduction open = Application.OpenForms["frmBonusDeduction"] as frmBonusDeduction;
                        if (open == null)
                        {
                            frmBonusDeductionObj.WindowState = FormWindowState.Normal;
                            frmBonusDeductionObj.MdiParent = formMDI.MDIObj;
                            frmBonusDeductionObj.CallFromBonusDeductionRegister(Convert.ToDecimal(dgvBonusDeduction.Rows[e.RowIndex].Cells["dgvtxtBonusDeductionId"].Value.ToString()), this);
                        }
                        else
                        {
                         
                            open.BringToFront();
                            open.CallFromBonusDeductionRegister(Convert.ToDecimal(dgvBonusDeduction.Rows[e.RowIndex].Cells["dgvtxtBonusDeductionId"].Value.ToString()), this);
                            if (open.WindowState == FormWindowState.Minimized)
                            {
                                open.WindowState = FormWindowState.Normal;
                            }
                        }
                        inCurrentRowIndex = dgvBonusDeduction.CurrentRow.Index;
                    }
                    this.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR9:" + ex.Message;
            }
        }
       /// <summary>
       /// On 'Search' button click
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void Search_Click(object sender, EventArgs e)
        {
            try
            {
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR10:" + ex.Message;
            }
        }

        #endregion

        #region Navigation
        /// <summary>
        /// Escape key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmBonusDeductionRegister_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "BDR11:" + ex.Message;
            }
        }
        /// <summary>
        /// Calls frmEmployeePopup form to select and view Employee and enter key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbEmployeeName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    dtpMonth.Focus();
                }
                if (e.KeyCode == Keys.F && Control.ModifierKeys == Keys.Control) //Pop Up
                {

                    frmEmployeeObj = new frmEmployeePopup();
                    frmEmployeeObj.MdiParent = formMDI.MDIObj;
                    if (cmbEmployeeName.SelectedIndex > 0)
                    {
                        frmEmployeeObj.CallFromBonusDeductionregister(this, Convert.ToDecimal(cmbEmployeeName.SelectedValue.ToString()));
                    }
                    else
                    {
                        Messages.InformationMessage("Select employeename");
                        cmbEmployeeName.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR12:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpMonth_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbEmployeeName.Focus();
                    cmbEmployeeName.SelectionStart = 0;
                    cmbEmployeeName.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR13:" + ex.Message;
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
                    dtpMonth.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR14:" + ex.Message;
            }
        }

        #endregion
    }
}
