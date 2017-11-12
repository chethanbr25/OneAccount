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
    public partial class frmOutstandingReport : Form
    {
        # region Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        decimal decLedgerId;
        string strAccountGroup = string.Empty;
        PartyBalanceSP spPartyBalance = new PartyBalanceSP();
        # endregion
        # region Functions
        /// <summary>
        /// Create an Instance of a frmOutstandingReport class
        /// </summary>
        public frmOutstandingReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to load the default values on load
        /// </summary>
        public void DefaultLoadFun()
        {
            try
            {
                dtpfromdate.Value = PublicVariables._dtFromDate;
                dtpfromdate.MaxDate = PublicVariables._dtToDate;
                dtpfromdate.MinDate = PublicVariables._dtFromDate;
                dtptodate.Value = PublicVariables._dtCurrentDate;
                dtptodate.MaxDate = PublicVariables._dtToDate;
                dtptodate.MinDate = PublicVariables._dtFromDate;
                txtfromdate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                txttodate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "OR1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill party combobox
        /// </summary>
        public void PartyCombofill()
        {
            try
            {
                cmbParty.Enabled = true;
                DataTable dtbl = new DataTable();
                dtbl = spPartyBalance.OutstandingPartyView();
                DataRow dr = dtbl.NewRow();
                dr["ledgerId"] = 0;
                dr["ledgerName"] = "All";
                dtbl.Rows.InsertAt(dr, 0);
                cmbParty.DataSource = dtbl;
                cmbParty.ValueMember = "ledgerId";
                cmbParty.DisplayMember = "ledgerName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "OR2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to generate serial number
        /// </summary>
        public void SerialNumber()
        {
            int inCount = 1;
            try
            {
                foreach (DataGridViewRow row in dgvOutstanding.Rows)
                {
                    row.Cells["dgvtxtslno"].Value = inCount.ToString();
                    inCount++;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "OR3:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to clear the fields
        /// </summary>
        public void clear()
        {
            try
            {
                dgvOutstanding.Rows.Clear();
                cmbParty.SelectedIndex = 0;
                txtfromdate.Focus();
                txtTotalCredit.Clear();
                txtTotalDebit.Clear();
                DefaultLoadFun();
                decLedgerId = Convert.ToDecimal(cmbParty.SelectedValue.ToString());
                OutstandingGridFill(decLedgerId, strAccountGroup, dtpfromdate.Value, dtptodate.Value);
                TotalAmtCalculationDebit();
                TotalAmtCalculationCredit();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "OR4:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the grid
        /// </summary>
        /// <param name="decledgerId"></param>
        /// <param name="strAccountGroup"></param>
        /// <param name="dtfromdate"></param>
        /// <param name="dttodate"></param>
        public void OutstandingGridFill(decimal decledgerId, string strAccountGroup, DateTime dtfromdate, DateTime dttodate)
        {
            strAccountGroup = string.Empty;
            try
            {
                DataSet ds = spPartyBalance.OutstandingViewAll(decledgerId, strAccountGroup, dtfromdate, dttodate);
                foreach (DataTable dtbl in ds.Tables)
                {
                    if (dtbl.TableName == "Table1")
                    {
                        foreach (DataRow item in dtbl.Rows)
                        {
                           
                            
                            if (item["accountGroupName"].ToString() == "Sundry Creditors")
                            {
                                if (Convert.ToDecimal(item["balance"].ToString()) > 0)
                                {
                                    dgvOutstanding.Rows.Add();
                                    dgvOutstanding.Rows[dgvOutstanding.Rows.Count - 1].Cells["dgvtxtParty"].Value = item["ledgerName"].ToString();
                                    dgvOutstanding.Rows[dgvOutstanding.Rows.Count - 1].Cells["dgvtxtVoucherType"].Value = item["voucherTypeName"].ToString();
                                    dgvOutstanding.Rows[dgvOutstanding.Rows.Count - 1].Cells["dgvtxtVoucherNo"].Value = item["voucherNo"].ToString();
                                    dgvOutstanding.Rows[dgvOutstanding.Rows.Count - 1].Cells["dgvtxtCredit"].Value = Math.Round(Convert.ToDecimal(item["balance"].ToString()), PublicVariables._inNoOfDecimalPlaces);
                                    dgvOutstanding.Rows[dgvOutstanding.Rows.Count - 1].Cells["dgvtxtDebit"].Value = 0;
                                }
                                else
                                {
                                    if (Convert.ToDecimal(item["balance"].ToString()) < 0)
                                    {
                                        dgvOutstanding.Rows.Add();
                                        dgvOutstanding.Rows[dgvOutstanding.Rows.Count - 1].Cells["dgvtxtParty"].Value = item["ledgerName"].ToString();
                                        dgvOutstanding.Rows[dgvOutstanding.Rows.Count - 1].Cells["dgvtxtVoucherType"].Value = item["voucherTypeName"].ToString();
                                        dgvOutstanding.Rows[dgvOutstanding.Rows.Count - 1].Cells["dgvtxtVoucherNo"].Value = item["voucherNo"].ToString();
                                        dgvOutstanding.Rows[dgvOutstanding.Rows.Count - 1].Cells["dgvtxtDebit"].Value = Math.Round(Convert.ToDecimal(item["balance"].ToString()), PublicVariables._inNoOfDecimalPlaces) * -1;
                                        dgvOutstanding.Rows[dgvOutstanding.Rows.Count - 1].Cells["dgvtxtCredit"].Value = 0;
                                    }
                                }
                            }
                            if (item["accountGroupName"].ToString() == "Sundry Debtors")
                            {
                                if (Convert.ToDecimal(item["balance"].ToString()) > 0)
                                {
                                    dgvOutstanding.Rows.Add();
                                    dgvOutstanding.Rows[dgvOutstanding.Rows.Count - 1].Cells["dgvtxtParty"].Value = item["ledgerName"].ToString();
                                    dgvOutstanding.Rows[dgvOutstanding.Rows.Count - 1].Cells["dgvtxtVoucherType"].Value = item["voucherTypeName"].ToString();
                                    dgvOutstanding.Rows[dgvOutstanding.Rows.Count - 1].Cells["dgvtxtVoucherNo"].Value = item["voucherNo"].ToString();
                                    dgvOutstanding.Rows[dgvOutstanding.Rows.Count - 1].Cells["dgvtxtDebit"].Value = Math.Round(Convert.ToDecimal(item["balance"].ToString()), PublicVariables._inNoOfDecimalPlaces);
                                    dgvOutstanding.Rows[dgvOutstanding.Rows.Count - 1].Cells["dgvtxtCredit"].Value = 0;
                                }
                                else
                                {
                                    if (Convert.ToDecimal(item["balance"].ToString()) < 0)
                                    {
                                        dgvOutstanding.Rows.Add();
                                        dgvOutstanding.Rows[dgvOutstanding.Rows.Count - 1].Cells["dgvtxtParty"].Value = item["ledgerName"].ToString();
                                        dgvOutstanding.Rows[dgvOutstanding.Rows.Count - 1].Cells["dgvtxtVoucherType"].Value = item["voucherTypeName"].ToString();
                                        dgvOutstanding.Rows[dgvOutstanding.Rows.Count - 1].Cells["dgvtxtVoucherNo"].Value = item["voucherNo"].ToString();
                                        dgvOutstanding.Rows[dgvOutstanding.Rows.Count - 1].Cells["dgvtxtCredit"].Value = Math.Round(Convert.ToDecimal(item["balance"].ToString()), PublicVariables._inNoOfDecimalPlaces) * -1;
                                        dgvOutstanding.Rows[dgvOutstanding.Rows.Count - 1].Cells["dgvtxtDebit"].Value = 0;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "OR5:" + ex.Message;
            }
        }
        /// <summary>
        /// Total debit amount calculation
        /// </summary>
        public void TotalAmtCalculationDebit()
        {
            decimal decDebitTotal = 0;
            try
            {
                foreach (DataGridViewRow dgvrow in dgvOutstanding.Rows)
                {
                    if (dgvrow.Cells["dgvtxtDebit"].Value != null)
                    {
                        if (dgvrow.Cells["dgvtxtDebit"].Value.ToString() != string.Empty)
                        {
                            decDebitTotal = decDebitTotal + Convert.ToDecimal(dgvrow.Cells["dgvtxtDebit"].Value.ToString());
                            decDebitTotal = Math.Round(decDebitTotal, PublicVariables._inNoOfDecimalPlaces);
                            txtTotalDebit.Text = decDebitTotal.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "OR6:" + ex.Message;
            }
        }
        /// <summary>
        /// Total credit amount calculation
        /// </summary>
        public void TotalAmtCalculationCredit()
        {
            decimal decCreditTotal = 0;
            try
            {
                foreach (DataGridViewRow dgvrow in dgvOutstanding.Rows)
                {
                    if (dgvrow.Cells["dgvtxtCredit"].Value != null)
                    {
                        if (dgvrow.Cells["dgvtxtCredit"].Value.ToString() != string.Empty)
                        {
                            decCreditTotal = decCreditTotal + Convert.ToDecimal(dgvrow.Cells["dgvtxtCredit"].Value.ToString());
                            decCreditTotal = Math.Round(decCreditTotal, PublicVariables._inNoOfDecimalPlaces);
                            txtTotalCredit.Text = decCreditTotal.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "OR7:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to print the report
        /// </summary>
        /// <param name="decledgerId"></param>
        /// <param name="strAccountGroup"></param>
        /// <param name="dtfromdate"></param>
        /// <param name="dttodate"></param>
        public void print(decimal decledgerId, string strAccountGroup, DateTime dtfromdate, DateTime dttodate)
        {
            try
            {
                DataSet dsoutstanding = spPartyBalance.OutstandingPrint(decledgerId, strAccountGroup, dtfromdate, dttodate, 1);
                frmReport frmReport = new frmReport();
                frmReport.MdiParent = formMDI.MDIObj;
                frmReport.OutstandingPrinting(dsoutstanding);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "OR8:" + ex.Message;
            }
        }
        #endregion
        # region Events
        /// <summary>
        /// On load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmOutstandingReport_Load(object sender, EventArgs e)
        {
            try
            {
                cmbParty.Enabled = true;
                PartyCombofill();
                cmbParty.SelectedIndex = 0;
                DefaultLoadFun();
                if (cmbParty.SelectedValue != null)
                {
                    decLedgerId = Convert.ToDecimal(cmbParty.SelectedValue.ToString());
                    OutstandingGridFill(decLedgerId, strAccountGroup, Convert.ToDateTime(dtpfromdate.Value.ToString()), Convert.ToDateTime(dtptodate.Value.ToString()));
                    TotalAmtCalculationDebit();
                    TotalAmtCalculationCredit();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "OR9:" + ex.Message;
            }
        }
        /// <summary>
        /// On value change of dtpFromDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpfromdate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpfromdate.Value;
                this.txtfromdate.Text = date.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "OR10:" + ex.Message;
            }
        }
        /// <summary>
        /// On value change of dtptodate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtptodate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtptodate.Value;
                this.txttodate.Text = date.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "OR11:" + ex.Message;
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
                if (txtfromdate.Text != string.Empty && txttodate.Text != string.Empty)
                {
                    if (Convert.ToDateTime(txttodate.Text) < Convert.ToDateTime(txtfromdate.Text))
                    {
                        MessageBox.Show("Todate should be greater than fromdate", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txttodate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                        txtfromdate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                        DateTime dt;
                        DateTime.TryParse(txttodate.Text, out dt);
                        dtpfromdate.Value = dt;
                        dtptodate.Value = dt;
                    }
                }
                else if (txtfromdate.Text == string.Empty)
                {
                    txtfromdate.Text = PublicVariables._dtCurrentDate.ToString();
                    txttodate.Text = PublicVariables._dtCurrentDate.ToString();
                    DateTime dt;
                    DateTime.TryParse(txttodate.Text, out dt);
                    dtpfromdate.Value = dt;
                    dtptodate.Value = dt;
                }
                if (cmbParty.SelectedIndex > -1)
                {
                    if (cmbParty.SelectedValue.ToString() != "System.Data.DataRowView" && cmbParty.Text != "System.Data.DataRowView")
                    {
                        dgvOutstanding.Rows.Clear();
                        decLedgerId = Convert.ToDecimal(cmbParty.SelectedValue.ToString());
                        OutstandingGridFill(decLedgerId, strAccountGroup, Convert.ToDateTime(dtpfromdate.Value.ToString()), Convert.ToDateTime(dtptodate.Value.ToString()));
                        TotalAmtCalculationDebit();
                        TotalAmtCalculationCredit();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "OR12:" + ex.Message;
            }
        }
        /// <summary>
        /// Generate serial number on rows added
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvOutstanding_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                SerialNumber();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "OR13:" + ex.Message;
            }
        }
        /// <summary>
        /// On print button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strAccountGroup = string.Empty;
            try
            {
                if (dgvOutstanding.Rows.Count > 0)
                {
                    print(Convert.ToDecimal(cmbParty.SelectedValue.ToString()), strAccountGroup, Convert.ToDateTime(txtfromdate.Text.ToString()), Convert.ToDateTime(txttodate.Text.ToString()));
                }
                else
                {
                    Messages.InformationMessage("No data found");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "OR14:" + ex.Message;
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
                formMDI.infoError.ErrorString = "OR15:" + ex.Message;
            }
        }
        /// <summary>
        /// On leave from txtfromdate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtfromdate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation obj = new DateValidation();
                obj.DateValidationFunction(txtfromdate);
                if (txtfromdate.Text == string.Empty)
                {
                    txtfromdate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                }
                //---for change date in Date time picker----
                string strdate = txtfromdate.Text;
                dtpfromdate.Value = Convert.ToDateTime(strdate.ToString());
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "OR16:" + ex.Message;
            }
        }
        /// <summary>
        /// On leave from txttodate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txttodate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation obj = new DateValidation();
                obj.DateValidationFunction(txttodate);
                if (txttodate.Text == string.Empty)
                {
                    txttodate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                //---for change date in Date time picker----
                string strdate = txttodate.Text;
                dtptodate.Value = Convert.ToDateTime(strdate.ToString());
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "OR17:" + ex.Message;
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
                ex.ExportExcel(dgvOutstanding, "Outstanding Report", 0, 0, "Excel", null, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "OR18:" + ex.Message;
            }
        }
        # endregion
        # region Navigation
        /// <summary>
        /// Enterkey navigation of txtfromdate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtfromdate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txttodate.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "OR19:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of txttodate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txttodate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbParty.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txttodate.SelectionStart == 0 && txttodate.Text == string.Empty)
                    {
                        txtfromdate.Focus();
                        txtfromdate.SelectionStart = 0;
                        txtfromdate.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "OR20:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of cmbParty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbParty_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txttodate.SelectionStart == 0)
                    {
                        txttodate.Focus();
                        txttodate.SelectionStart = 0;
                        txttodate.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "OR21:" + ex.Message;
            }
        }
        /// <summary>
        /// backspace navigation of btnSearch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    cmbParty.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "OR22:" + ex.Message;
            }
        }
        /// <summary>
        /// Esc for formclose
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmOutstandingReport_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "OR23:" + ex.Message;
            }
        }
        # endregion


    }
}
