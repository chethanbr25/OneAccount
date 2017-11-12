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
    public partial class frmAccountLedgerReport : Form
    {
        
        
        #region PUBLIC VARIABLES
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        DateTime dtFromDate, dtTodate;
        decimal decAccountGroupId, decLedgerId;
        #endregion

        #region FUNCTIOS
        /// <summary>
        /// Create an Instance of a frmAccountLedgerReport class
        /// </summary>
        public frmAccountLedgerReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to do on form load
        /// </summary>
        public void FormLoad()
        {
            try
            {
                txtFromDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                txtToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                dtFromDate = Convert.ToDateTime(txtFromDate.Text);
                dtTodate = Convert.ToDateTime(txtToDate.Text);
                txtFromDate.Focus();
                AccountGroupComboFill();
                decAccountGroupId = Convert.ToDecimal(cmbAccountGroup.SelectedValue.ToString());
                decLedgerId = Convert.ToDecimal(cmbAccountLedger.SelectedValue.ToString());
                FinancialYearDate();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ALREP1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the accountgroup combo box
        /// </summary>
        public void AccountGroupComboFill()
        {
            try
            {
                AccountGroupSP spAccountGroup = new AccountGroupSP();
                DataTable dtblAccountGroup = new DataTable();
                dtblAccountGroup = spAccountGroup.AccountGroupViewAllComboFillForAccountLedger();
                DataRow dr = dtblAccountGroup.NewRow();
                dr["accountGroupName"] = "All";
                dr["accountGroupId"] = -1;
                dtblAccountGroup.Rows.InsertAt(dr, 0);
                cmbAccountGroup.DataSource = dtblAccountGroup;
                cmbAccountGroup.ValueMember = "accountGroupId";
                cmbAccountGroup.DisplayMember = "accountGroupName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ALREP2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the accountledger combbox 
        /// </summary>
        public void AccountLedgerComboFill()
        {
            try
            {
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                DataTable dtblAccountLedger = new DataTable();
                dtblAccountLedger = spAccountLedger.AccountLedgerViewByAccountGroup(Convert.ToDecimal(cmbAccountGroup.SelectedValue.ToString()));
                DataRow dr = dtblAccountLedger.NewRow();
                dr["ledgerName"] = "All";
                dr["ledgerId"] = 0;
                dtblAccountLedger.Rows.InsertAt(dr, 0);
                cmbAccountLedger.DataSource = dtblAccountLedger;
                cmbAccountLedger.ValueMember = "ledgerId";
                cmbAccountLedger.DisplayMember = "ledgerName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ALREP3:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the grid
        /// </summary>
        public void AccountLedgerReportFill()
        {
            decimal decTotalClosing = 0;
            AccountLedgerSP SpAccountLedger = new AccountLedgerSP();
            try
            {
                dgvAccountLedgerReport.DataSource = SpAccountLedger.AccountLedgerReportFill(Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text), Convert.ToDecimal(cmbAccountGroup.SelectedValue.ToString()), Convert.ToDecimal(cmbAccountLedger.SelectedValue.ToString()));
                if (dgvAccountLedgerReport.RowCount > 0)
                {
                    for (int i = 0; i < dgvAccountLedgerReport.RowCount; i++)
                    {
                        decTotalClosing = decTotalClosing + Convert.ToDecimal(dgvAccountLedgerReport.Rows[i].Cells["dgvtxtClosing1"].Value.ToString());
                    }
                }
                if (decTotalClosing < 0)
                {
                    decTotalClosing = -1 * decTotalClosing;
                    lblTotalAmount.Text = decTotalClosing.ToString() + "Cr";
                }
                else
                {
                    lblTotalAmount.Text = decTotalClosing.ToString() + "Dr";
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ALREP4:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to print the report
        /// </summary>
        public void Print()
        {
            AccountLedgerSP SpAccountLedger = new AccountLedgerSP();
            try
            {
                
                DataSet dsAccountLedgerReport = SpAccountLedger.AccountLedgerReportPrinting(1, dtFromDate, dtTodate, decAccountGroupId, decLedgerId);
                frmReport frmReport = new frmReport();
                frmReport.MdiParent = formMDI.MDIObj;
                frmReport.AccountLedgerReportPrinting(dsAccountLedgerReport);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ALREP5:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to set the financial year date
        /// </summary>
        public void FinancialYearDate()
        {
            try
            {
                //-----For FromDate----//
                dtpFromDate.MinDate = PublicVariables._dtFromDate;
                dtpFromDate.MaxDate = PublicVariables._dtToDate;
                CompanyInfo infoComapany = new CompanyInfo();
                CompanySP spCompany = new CompanySP();
                infoComapany = spCompany.CompanyView(1);
                DateTime dtFromDate = infoComapany.CurrentDate;
                dtpFromDate.Value = dtFromDate;
                dtpFromDate.Text = dtFromDate.ToString("dd-MMM-yyyy");
                dtpFromDate.Value = Convert.ToDateTime(txtFromDate.Text);
                txtFromDate.Focus();
                txtFromDate.SelectAll();
                //==============================//
                //-----For ToDate-----------------//
                dtpToDate.MinDate = PublicVariables._dtFromDate;
                dtpToDate.MaxDate = PublicVariables._dtToDate;
                infoComapany = spCompany.CompanyView(1);
                DateTime dtToDate = infoComapany.CurrentDate;
                dtpToDate.Value = dtToDate;
                dtpToDate.Text = dtToDate.ToString("dd-MMM-yyyy");
                dtpToDate.Value = Convert.ToDateTime(txtToDate.Text);
                txtToDate.Focus();
                txtToDate.SelectAll();
                //=====================//
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ALREP6:" + ex.Message;
            }
        }
        #endregion
        
        #region EVENTS
        /// <summary>
        /// On form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmAccountLedgerReport_Load(object sender, EventArgs e)
        {
            try
            {
                FormLoad();
                AccountLedgerReportFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ALREP7:" + ex.Message;
            }
        }
        /// <summary>
        /// On selected value change of accountGroup combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAccountGroup_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbAccountGroup.SelectedValue != null)
                {
                    if (cmbAccountGroup.SelectedValue.ToString() != "System.Data.DataRowView" && cmbAccountGroup.Text != "System.Data.DataRowView")
                    {
                        AccountLedgerComboFill();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ALREP8:" + ex.Message;
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
                txtFromDate.Text = dtpFromDate.Value.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ALREP9:" + ex.Message;
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
                txtToDate.Text = dtpToDate.Value.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ALREP10:" + ex.Message;
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
                    txtFromDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                }
                //---for change date in Date time picker----//
                string strdate = txtFromDate.Text;
                dtpFromDate.Value = Convert.ToDateTime(strdate.ToString());
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ALREP11:" + ex.Message;
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
                //---for change date in Date time picker----//
                string strdate = txtToDate.Text;
                dtpToDate.Value = Convert.ToDateTime(strdate.ToString());
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ALREP12:" + ex.Message;
            }
        }
        /// <summary>
        /// On leave from txtToDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                dtFromDate = Convert.ToDateTime(txtFromDate.Text);//Used When double click on grid
                dtTodate = Convert.ToDateTime(txtToDate.Text);//Used When double click on grid
                decAccountGroupId= Convert.ToDecimal(cmbAccountGroup.SelectedValue.ToString());
                decLedgerId = Convert.ToDecimal(cmbAccountLedger.SelectedValue.ToString());
                AccountLedgerReportFill();
                txtFromDate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ALREP13:" + ex.Message;
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
                frmAccountLedgerReport_Load(sender, e);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ALREP14:" + ex.Message;
            }
        }
        /// <summary>
        /// When doubleclicking on the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvAccountLedgerReport_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    if (dgvAccountLedgerReport.CurrentRow != null)
                    {
                        decimal decLedgerId = Convert.ToDecimal(dgvAccountLedgerReport.CurrentRow.Cells["dgvtxLedgerId"].Value.ToString());
                        frmLedgerDetails frmLedgerDetailsObj = new frmLedgerDetails();
                        frmLedgerDetailsObj.MdiParent = formMDI.MDIObj;
                        frmLedgerDetailsObj.CallFromAccountLedgerReport(this, decLedgerId, dtFromDate, dtTodate);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ALREP15:" + ex.Message;
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
                if (dgvAccountLedgerReport.Rows.Count > 0)
                {
                    Print();
                }
                else
                {
                    Messages.InformationMessage("No data found");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ALREP16:" + ex.Message;
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
                ex.ExportExcel(dgvAccountLedgerReport, "Account Ledger Report", 0, 0, "Excel", null, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ALREP17:" + ex.Message;
            }
        }
       
        #endregion
        
        #region NAVIGATION
        /// <summary>
        /// Escape for form close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmAccountLedgerReport_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "ALREP18:" + ex.Message;
            }
        }
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
                    txtToDate.SelectionStart = txtToDate.Text.Length;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ALREP19:" + ex.Message;
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
                    cmbAccountGroup.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtToDate.SelectionStart == 0)
                    {
                        txtFromDate.Focus();
                        txtFromDate.SelectionStart = txtFromDate.Text.Length;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ALREP20:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of cmbAccountGroup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAccountGroup_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbAccountLedger.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtToDate.Focus();
                    txtToDate.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ALREP21:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of cmbAccountLedger
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAccountLedger_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbAccountGroup.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ALREP22:" + ex.Message;
            }
        }
        #endregion
        
    }
}
