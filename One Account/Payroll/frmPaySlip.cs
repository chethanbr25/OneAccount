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

    public partial class frmPaySlip : Form
    {
        #region Public Variables
        /// <summary>
        /// public varaible declaration part
        /// </summary>
        #endregion

        #region Functions

        /// <summary>
        /// creates an instance of frmPaySlip class
        /// </summary>
        public frmPaySlip()
        {
            InitializeComponent();
        }
       /// <summary>
       /// Function to fill Employee combobox
       /// </summary>
        public void FillEmployee()
        {
            try
            {
                DataTable dtbl = new DataTable();
                EmployeeSP spEmployee = new EmployeeSP();
                dtbl = spEmployee.EmployeeViewForPaySlip();
                DataRow dr = dtbl.NewRow();
                dr[1] = "--Select--";
                dtbl.Rows.InsertAt(dr, 0);
                cmbEmployee.DataSource = dtbl;
                cmbEmployee.ValueMember = "employeeId";
                cmbEmployee.DisplayMember = "employeeName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to close form
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
                formMDI.infoError.ErrorString = "PS2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function for print
        /// </summary>
        public void Print()
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnPrint.Text))
                {
                    if (cmbEmployee.Text == string.Empty || cmbEmployee.Text == "--Select--")
                    {
                        Messages.InformationMessage("Select an employee");
                        cmbEmployee.Focus();
                    }
                    else
                    {
                        SalaryVoucherMasterSP spSalaryVoucherMaster = new SalaryVoucherMasterSP();
                        DateTime dtMon = DateTime.Parse(dtpSalaryMonth.Text);
                        DateTime dtSalaryMonth = new DateTime(dtMon.Year, dtMon.Month, 1);
                        decimal decEmployeeId = Convert.ToDecimal(cmbEmployee.SelectedValue.ToString());
                        DataSet dsPaySlip = spSalaryVoucherMaster.PaySlipPrinting(decEmployeeId, dtSalaryMonth, 1);

                        foreach (DataTable dtbl in dsPaySlip.Tables)
                        {
                            if (dtbl.TableName == "Table1")
                            {
                                if (dtbl.Rows.Count > 0)
                                {
                                    frmReport frmReport = new frmReport();
                                    frmReport.MdiParent = formMDI.MDIObj;
                                    frmReport.PaySlipPrinting(dsPaySlip);
                                }
                                else
                                {
                                    MessageBox.Show("Salary not paid", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS3:" + ex.Message;
            }
        }

        #endregion

        #region Events
        /// <summary>
        /// On 'Close' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                FormClose();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS4:" + ex.Message;
            }
        }
        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPaySlip_Load(object sender, EventArgs e)
        {
            try
            {
                FillEmployee();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS5:" + ex.Message;
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
                Print();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS6:" + ex.Message;
            }
        }

        #endregion

        #region Navigation
        /// <summary>
        /// Enter key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpSalaryMonth_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbEmployee.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS7:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbEmployee_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnPrint.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    dtpSalaryMonth.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS8:" + ex.Message;
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
                    cmbEmployee.Focus();
                    cmbEmployee.SelectionStart = 0;
                    cmbEmployee.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS9:" + ex.Message;
            }
        }
        /// <summary>
        /// Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    btnPrint.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS10:" + ex.Message;
            }
        }
        /// <summary>
        /// Escape key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPaySlip_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "PS11:" + ex.Message;
            }
        }

        #endregion
    }
}
