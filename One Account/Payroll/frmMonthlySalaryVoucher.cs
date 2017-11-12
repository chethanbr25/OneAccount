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

    public partial class frmMonthlySalaryVoucher : Form
    {
        #region Public Variables
        /// <summary>
        /// Public varaible declaration part
        /// </summary>
        decimal decMonthlyVoucherTypeId = 0;
        decimal decMonthlySuffixPrefixId = 0;
        decimal decMasterId = 0;
        decimal decIsEditModeMasterId = 0;

        string strUpdatedVoucherNo = string.Empty;
        string strUpdatedInvoiceNo = string.Empty;
        string tableName = "SalaryVoucherMaster";
        string strVoucherNo = string.Empty;
        string strInvoiceNo = string.Empty;
        string strPrefix = string.Empty;
        string strSuffix = string.Empty;
        string strEployeeNames = string.Empty;
        string strledgerId;
        string strVoucherNoforEdit = "0";

        int q = 0;
        int inNarrationCount = 0;

        bool isAutomatic = false;
        bool @isEditMode = false;


        frmMonthlySalaryRegister frmMonthlySalaryRegisterObj;
        frmLedgerPopup frmLedgerPopupObj;
        frmVoucherSearch objVoucherSearch = null;
        frmDayBook frmDayBookObj = null;
        frmLedgerDetails frmLedgerDetailsObj;
        #endregion

        #region Functions
        /// <summary>
        /// Creates an instance of frmMonthlySalaryVoucher class
        /// </summary>
        public frmMonthlySalaryVoucher()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Function to fill Datagridview
        /// </summary>
        /// <param name="isEditMode"></param>
        public void GridFill(bool @isEditMode)
        {
            try
            {
                SalaryVoucherDetailsSP spSalaryVoucherDetails = new SalaryVoucherDetailsSP();
                DataTable dtbl = new DataTable();
                string strMonth = dtpMonth.Value.ToString("MMMMyyyy");
                string Month = strMonth.Substring(0, 3);
                string strmonthYear = Convert.ToDateTime(strMonth.ToString()).Year.ToString();
                string monthYear = Month + " " + strmonthYear;
                dtbl = spSalaryVoucherDetails.MonthlySalaryVoucherDetailsViewAll(strMonth, Month, monthYear, isEditMode, strVoucherNoforEdit);
                dgvMonthlySalary.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV1:" + ex.Message;
            }

        }
        /// <summary>
        /// Function to call this form from frmVoucherSearch form to view voucher details
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="decId"></param>
        public void CallFromVoucherSerach(frmVoucherSearch frm, decimal decId)
        {
            try
            {

                isEditMode = true;
                decIsEditModeMasterId = decId;
                base.Show();
                SalaryVoucherMasterSP SpMaster = new SalaryVoucherMasterSP();
                SalaryVoucherMasterInfo InfoMaster = new SalaryVoucherMasterInfo();
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                btnDelete.Enabled = true;
                btnSave.Text = "Update";
                InfoMaster = SpMaster.SalaryVoucherMasterView(decIsEditModeMasterId);
                strVoucherNoforEdit = InfoMaster.VoucherNo;
                strVoucherNo = InfoMaster.VoucherNo;
                txtVoucherNo.Text = InfoMaster.InvoiceNo;
                strInvoiceNo = InfoMaster.InvoiceNo;
                txtVoucherDate.Text = InfoMaster.Date.ToString("dd-MMMM-yyyy");
                string stra = Convert.ToDateTime(InfoMaster.Month.ToString()).ToString("MMM yyyy");
                dtpMonth.Value = Convert.ToDateTime(InfoMaster.Month.ToString("MMM yyyy"));
                txtNarration.Text = InfoMaster.Narration;
                lblTotalAmount.Text = InfoMaster.TotalAmount.ToString();
                decMonthlySuffixPrefixId = InfoMaster.SuffixPrefixId;
                decMonthlyVoucherTypeId = InfoMaster.VoucherTypeId;
                this.objVoucherSearch = frm;
                cmbCashOrBankAcc.SelectedValue = InfoMaster.LedgerId;
                isAutomatic = spVoucherType.CheckMethodOfVoucherNumbering(decMonthlyVoucherTypeId);
                if (isAutomatic)
                {
                    txtVoucherNo.Enabled = false;
                }
                else
                {
                    txtVoucherNo.Enabled = true;
                }
                dtpMonth.Enabled = false;
                GridFill(isEditMode);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmVoucherTypeSelection form 
        /// </summary>
        /// <param name="decVoucherTypeId"></param>
        /// <param name="strVoucherTypeName"></param>
        public void CallFromVoucherTypeSelection(decimal decVoucherTypeId, string strVoucherTypeName)
        {
            try
            {
                decMonthlyVoucherTypeId = decVoucherTypeId;
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                isAutomatic = spVoucherType.CheckMethodOfVoucherNumbering(decMonthlyVoucherTypeId);
                SuffixPrefixSP spSuffisprefix = new SuffixPrefixSP();
                SuffixPrefixInfo infoSuffixPrefix = new SuffixPrefixInfo();
                infoSuffixPrefix = spSuffisprefix.GetSuffixPrefixDetails(decMonthlyVoucherTypeId, dtpVoucherDate.Value);
                decMonthlySuffixPrefixId = infoSuffixPrefix.SuffixprefixId;
                this.Text = strVoucherTypeName;
                base.Show();
                if (isAutomatic)
                {
                    txtVoucherDate.Focus();
                }
                else
                {
                    txtVoucherNo.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV3:" + ex.Message;
            }

        }
        /// <summary>
        /// Function to save ledgerposting table
        /// </summary>
        /// <param name="decid"></param>
        public void LedgerPosting(decimal decid)
        {
            try
            {
                LedgerPostingSP spLedgerPosting = new LedgerPostingSP();
                LedgerPostingInfo infoLedgerPosting = new LedgerPostingInfo();
                infoLedgerPosting.Debit = 0;
                infoLedgerPosting.Credit = Convert.ToDecimal(lblTotalAmount.Text.ToString());
                infoLedgerPosting.VoucherTypeId = decMonthlyVoucherTypeId;
                if (isAutomatic)
                {
                    infoLedgerPosting.VoucherNo = strVoucherNo;
                }
                else
                {
                    infoLedgerPosting.VoucherNo = txtVoucherNo.Text;
                }
                infoLedgerPosting.Date = Convert.ToDateTime(dtpVoucherDate.Value.ToString());
                infoLedgerPosting.LedgerId = decid;
                infoLedgerPosting.DetailsId = 0;
                if (isAutomatic)
                {
                    infoLedgerPosting.InvoiceNo = strInvoiceNo;
                }
                else
                {
                    infoLedgerPosting.InvoiceNo = txtVoucherNo.Text;
                }
                infoLedgerPosting.ChequeNo = string.Empty;
                infoLedgerPosting.ChequeDate = DateTime.Now;
                infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                infoLedgerPosting.Extra1 = string.Empty;
                infoLedgerPosting.Extra2 = string.Empty;
                spLedgerPosting.LedgerPostingAdd(infoLedgerPosting);

                infoLedgerPosting.Debit = Convert.ToDecimal(lblTotalAmount.Text.ToString());
                infoLedgerPosting.Credit = 0;
                infoLedgerPosting.VoucherTypeId = decMonthlyVoucherTypeId;
                if (isAutomatic)
                {
                    infoLedgerPosting.VoucherNo = strVoucherNo;
                }
                else
                {
                    infoLedgerPosting.VoucherNo = txtVoucherNo.Text;
                }
                infoLedgerPosting.Date = PublicVariables._dtCurrentDate;
                infoLedgerPosting.LedgerId = 4; //ledgerId of salarys
                infoLedgerPosting.DetailsId = 0;
                if (isAutomatic)
                {
                    infoLedgerPosting.InvoiceNo = strInvoiceNo;
                }
                else
                {
                    infoLedgerPosting.InvoiceNo = txtVoucherNo.Text;
                }
                infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                infoLedgerPosting.Extra1 = string.Empty;
                infoLedgerPosting.Extra2 = string.Empty;
                spLedgerPosting.LedgerPostingAdd(infoLedgerPosting);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV4:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to update ledger posting table
        /// </summary>
        public void LedgerUpdate()
        {
            try
            {
                decimal decLedgerPostingId = 0;
                LedgerPostingSP spLedgerPosting = new LedgerPostingSP();
                LedgerPostingInfo infoLedgerPosting = new LedgerPostingInfo();
                DataTable dtbl = new DataTable();
                dtbl = spLedgerPosting.GetLedgerPostingIds(strVoucherNo, decMonthlyVoucherTypeId);
                int ini = 0;
                foreach (DataRow dr in dtbl.Rows)
                {
                    ini++;

                    if (ini == 2)
                    {
                        decLedgerPostingId = Convert.ToDecimal(dr.ItemArray[0].ToString());
                        infoLedgerPosting.LedgerPostingId = decLedgerPostingId;
                        infoLedgerPosting.Date = Convert.ToDateTime(dtpVoucherDate.Value.ToString());
                        if (isAutomatic)
                        {
                            infoLedgerPosting.VoucherNo = strVoucherNo;
                        }
                        else
                        {
                            infoLedgerPosting.VoucherNo = txtVoucherNo.Text;
                        }
                        infoLedgerPosting.Debit = Convert.ToDecimal(lblTotalAmount.Text.ToString());
                        infoLedgerPosting.Credit = 0;
                        infoLedgerPosting.VoucherTypeId = decMonthlyVoucherTypeId;
                        infoLedgerPosting.LedgerId = 4;
                        infoLedgerPosting.DetailsId = 0;
                        if (isAutomatic)
                        {
                            infoLedgerPosting.InvoiceNo = strInvoiceNo;
                        }
                        else
                        {
                            infoLedgerPosting.InvoiceNo = txtVoucherNo.Text;
                        }

                        infoLedgerPosting.ChequeNo = string.Empty;
                        infoLedgerPosting.ChequeDate = DateTime.Now;

                        infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                        infoLedgerPosting.Extra1 = string.Empty;
                        infoLedgerPosting.Extra2 = string.Empty;

                        spLedgerPosting.LedgerPostingEdit(infoLedgerPosting);
                    }
                    if (ini == 1)
                    {
                        decLedgerPostingId = Convert.ToDecimal(dr.ItemArray[0].ToString());
                        infoLedgerPosting.LedgerPostingId = decLedgerPostingId;
                        infoLedgerPosting.Date = PublicVariables._dtCurrentDate;
                        if (isAutomatic)
                        {
                            infoLedgerPosting.VoucherNo = strVoucherNo;
                        }
                        else
                        {
                            infoLedgerPosting.VoucherNo = txtVoucherNo.Text;
                        }
                        infoLedgerPosting.Debit = 0;
                        infoLedgerPosting.Credit = Convert.ToDecimal(lblTotalAmount.Text.ToString());
                        infoLedgerPosting.VoucherTypeId = decMonthlyVoucherTypeId;
                        infoLedgerPosting.LedgerId = Convert.ToDecimal(cmbCashOrBankAcc.SelectedValue.ToString());
                        infoLedgerPosting.DetailsId = 0;
                        if (isAutomatic)
                        {
                            infoLedgerPosting.InvoiceNo = strInvoiceNo;
                        }
                        else
                        {
                            infoLedgerPosting.InvoiceNo = txtVoucherNo.Text;
                        }
                        infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                        infoLedgerPosting.Extra1 = string.Empty;
                        infoLedgerPosting.Extra2 = string.Empty;
                        infoLedgerPosting.ChequeNo = string.Empty;
                        infoLedgerPosting.ChequeDate = DateTime.Now;
                        spLedgerPosting.LedgerPostingEdit(infoLedgerPosting);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV5:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call Save or Edit
        /// </summary>
        public void SaveOrEdit()
        {
            try
            {
                SalaryVoucherMasterSP spMaster = new SalaryVoucherMasterSP();
                cmbCashOrBankAcc.DropDownStyle = ComboBoxStyle.DropDownList;
                if (txtVoucherNo.Text == string.Empty)
                {

                    Messages.InformationMessage("Enter voucher number");

                    lblVoucherNoIndicator.Visible = true;
                    txtVoucherNo.Focus();

                }
                else if (dtpVoucherDate.Value.ToString() == string.Empty)
                {
                    Messages.InformationMessage("Select voucher date");
                    lblVoucherDateIndicator.Visible = true;
                    dtpVoucherDate.Focus();
                }
                else if (cmbCashOrBankAcc.SelectedIndex == -1)
                {

                    Messages.InformationMessage("Select cash or bank account");
                    lblCashOrBankIndicator.Visible = true;
                    cmbCashOrBankAcc.Focus();
                }
                else
                {

                    if (isEditMode == false)
                    {
                        if (PublicVariables.isMessageAdd)
                        {
                            if (Messages.SaveMessage())
                            {
                                if (!isAutomatic)
                                {
                                    if (spMaster.MonthlySalaryVoucherCheckExistence(txtVoucherNo.Text.Trim(), decMonthlyVoucherTypeId, 0) == false)
                                    {
                                        SaveFunction();
                                    }
                                    else
                                    {
                                        Messages.InformationMessage("Voucher number already exist");
                                    }
                                }
                                else
                                {
                                    SaveFunction();
                                }
                            }
                        }
                        else
                        {
                            if (!isAutomatic)
                            {
                                if (spMaster.MonthlySalaryVoucherCheckExistence(txtVoucherNo.Text.Trim(), decMonthlyVoucherTypeId, 0) == false)
                                {
                                    SaveFunction();
                                }
                                else
                                {
                                    Messages.InformationMessage("Voucher number already exist");
                                }
                            }
                            else
                            {
                                SaveFunction();
                            }
                        }

                    }
                    //------ Update-------------//
                    else if (isEditMode)
                    {
                        if (PublicVariables.isMessageEdit)
                        {
                            if (Messages.UpdateMessage())
                            {
                                if (!isAutomatic)
                                {
                                    if (spMaster.MonthlySalaryVoucherCheckExistence(txtVoucherNo.Text.Trim(), decMonthlyVoucherTypeId, decIsEditModeMasterId) == false)
                                    {
                                        EditFunction();
                                    }
                                    else
                                    {
                                        Messages.InformationMessage("Voucher number already exist");
                                    }
                                }
                                else
                                {
                                    EditFunction();
                                }
                            }
                        }
                        else
                        {
                            if (!isAutomatic)
                            {
                                if (spMaster.MonthlySalaryVoucherCheckExistence(txtVoucherNo.Text.Trim(), decMonthlyVoucherTypeId, decIsEditModeMasterId) == false)
                                {
                                    EditFunction();
                                }
                                else
                                {
                                    Messages.InformationMessage("Voucher number already exist");
                                }
                            }
                            else
                            {
                                EditFunction();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV6:" + ex.Message;
            }

        }
        /// <summary>
        /// Function for Save
        /// </summary>
        public void SaveFunction()
        {
            try
            {
                SalaryVoucherMasterSP spMaster = new SalaryVoucherMasterSP();
                SalaryVoucherMasterInfo infoMaster = new SalaryVoucherMasterInfo();
                SalaryVoucherDetailsSP spDetails = new SalaryVoucherDetailsSP();
                SalaryVoucherDetailsInfo infoDetails = new SalaryVoucherDetailsInfo();

                //------------------------------- In the case of multi user check whether salary is paying for the sam person ----------------//
                int inCounts = dgvMonthlySalary.RowCount;
                int incont = 0;
                decimal decVal = 0;
                for (int i = 0; i < inCounts; i++)
                {
                    decVal = Convert.ToDecimal(dgvMonthlySalary.Rows[i].Cells["txtEmployeeId"].Value.ToString());
                    if (spDetails.CheckWhetherSalaryAlreadyPaid(decVal, dtpMonth.Value) != "0")
                    {
                        strEployeeNames = strEployeeNames + spDetails.CheckWhetherSalaryAlreadyPaid(decVal, dtpMonth.Value) + ",";
                        foreach (char ch in strEployeeNames)
                        {
                            if (ch == ',')
                            {
                                incont++;
                            }
                        }
                        if (incont == 15)
                        {
                            incont = 0;
                            strEployeeNames = strEployeeNames + Environment.NewLine;
                        }

                    }
                }
                if (spDetails.CheckWhetherSalaryAlreadyPaid(decVal, dtpMonth.Value) != "0")
                {

                    Messages.InformationMessage("Salary already paid for - " + " " + strEployeeNames);
                    GridFill(isEditMode);
                }
                infoMaster.LedgerId = Convert.ToDecimal(cmbCashOrBankAcc.SelectedValue.ToString());
                if (isAutomatic)
                {
                    infoMaster.VoucherNo = strVoucherNo;
                }
                else
                {
                    infoMaster.VoucherNo = txtVoucherNo.Text;
                }
                infoMaster.Month = Convert.ToDateTime(dtpMonth.Text.ToString());
                infoMaster.Date = Convert.ToDateTime(dtpVoucherDate.Value.ToString());
                infoMaster.Narration = txtNarration.Text.Trim();
                if (isAutomatic)
                {
                    infoMaster.InvoiceNo = strInvoiceNo;
                }
                else
                {
                    infoMaster.InvoiceNo = txtVoucherNo.Text;
                }
                if (lblTotalAmount.Text.ToString() != string.Empty)
                {
                    infoMaster.TotalAmount = Math.Round(Convert.ToDecimal(lblTotalAmount.Text.ToString()), PublicVariables._inNoOfDecimalPlaces);
                }
                infoMaster.Extra1 = string.Empty; // Fields not in design//
                infoMaster.Extra2 = string.Empty; // Fields not in design//
                infoMaster.SuffixPrefixId = decMonthlySuffixPrefixId;
                infoMaster.VoucherTypeId = decMonthlyVoucherTypeId;
                infoMaster.FinancialYearId = PublicVariables._decCurrentFinancialYearId;

                int inCount = dgvMonthlySalary.RowCount;
                int inValue = 0;
                for (int i = 0; i < inCount; i++)
                {
                    if (dgvMonthlySalary.Rows[i].Cells["cmbStatus"].Value.ToString() == "Paid")
                    {
                        inValue++;
                    }
                }
                if (inValue > 0)
                {
                    //-------------------------In the case of Multi-User Check the VoucherNo. again (Max of VoucherNumber )---------------------//
                    DataTable dtbl = new DataTable();
                    dtbl = spMaster.MonthlySalaryVoucherMasterAddWithIdentity(infoMaster, isAutomatic);
                    foreach (DataRow dr in dtbl.Rows)
                    {
                        decMasterId = Convert.ToDecimal(dr.ItemArray[0].ToString());
                        strUpdatedVoucherNo = dr.ItemArray[1].ToString();
                        strUpdatedInvoiceNo = dr.ItemArray[2].ToString();
                    }
                    if (!isAutomatic)
                    {
                        strVoucherNo = txtVoucherNo.Text;
                    }
                    if (isAutomatic)
                    {
                        if (strUpdatedVoucherNo != "" && Convert.ToDecimal(strUpdatedVoucherNo) != Convert.ToDecimal(strVoucherNo))
                        {
                            Messages.InformationMessage("Voucher number changed from  " + strInvoiceNo + "  to  " + strUpdatedInvoiceNo);
                            strVoucherNo = strUpdatedVoucherNo.ToString();
                            strInvoiceNo = strUpdatedInvoiceNo;
          

                        }
                    }
                    LedgerPosting(Convert.ToDecimal(cmbCashOrBankAcc.SelectedValue.ToString()));
                    infoDetails.Extra1 = string.Empty;
                    infoDetails.Extra2 = string.Empty;
                    infoDetails.SalaryVoucherMasterId = decMasterId;

                    int inRowCount = dgvMonthlySalary.RowCount;
                    for (int i = 0; i < inRowCount; i++)
                    {

                        if (dgvMonthlySalary.Rows[i].Cells["txtEmployeeId"].Value != null && dgvMonthlySalary.Rows[i].Cells["txtEmployeeId"].Value.ToString() != string.Empty)
                        {
                            infoDetails.EmployeeId = Convert.ToDecimal(dgvMonthlySalary.Rows[i].Cells["txtEmployeeId"].Value.ToString());
                        }
                        if (dgvMonthlySalary.Rows[i].Cells["txtBonus"].Value != null && dgvMonthlySalary.Rows[i].Cells["txtBonus"].Value.ToString() != string.Empty)
                        {
                            infoDetails.Bonus = Convert.ToDecimal(dgvMonthlySalary.Rows[i].Cells["txtBonus"].Value.ToString());
                        }
                        if (dgvMonthlySalary.Rows[i].Cells["txtDeduction"].Value != null && dgvMonthlySalary.Rows[i].Cells["txtDeduction"].Value.ToString() != string.Empty)
                        {
                            infoDetails.Deduction = Convert.ToDecimal(dgvMonthlySalary.Rows[i].Cells["txtDeduction"].Value.ToString());
                        }
                        if (dgvMonthlySalary.Rows[i].Cells["txtAdvance"].Value != null && dgvMonthlySalary.Rows[i].Cells["txtAdvance"].Value.ToString() != string.Empty)
                        {
                            infoDetails.Advance = Convert.ToDecimal(dgvMonthlySalary.Rows[i].Cells["txtAdvance"].Value.ToString());
                        }
                        if (dgvMonthlySalary.Rows[i].Cells["txtLop"].Value != null && dgvMonthlySalary.Rows[i].Cells["txtLop"].Value.ToString() != string.Empty)
                        {
                            infoDetails.Lop = Convert.ToDecimal(dgvMonthlySalary.Rows[i].Cells["txtLop"].Value.ToString());
                        }
                        if (dgvMonthlySalary.Rows[i].Cells["txtSalary"].Value != null && dgvMonthlySalary.Rows[i].Cells["txtSalary"].Value.ToString() != string.Empty)
                        {
                            infoDetails.Salary = Convert.ToDecimal(dgvMonthlySalary.Rows[i].Cells["txtSalary"].Value.ToString());
                        }
                        if (dgvMonthlySalary.Rows[i].Cells["cmbStatus"].Value != null && dgvMonthlySalary.Rows[i].Cells["cmbStatus"].Value.ToString() != string.Empty)
                        {
                            infoDetails.Status = dgvMonthlySalary.Rows[i].Cells["cmbStatus"].Value.ToString();
                        }
                        if (dgvMonthlySalary.Rows[i].Cells["cmbStatus"].Value.ToString() == "Paid" && dgvMonthlySalary.Rows[i].Cells["txtMasterId"].Value.ToString() == "0")
                        {
                            infoDetails.SalaryVoucherMasterId = decMasterId;
                            spDetails.MonthlySalaryVoucherDetailsAdd(infoDetails);
                        }
                    }

                    Messages.SavedMessage();
                    GridFill(isEditMode);
                    Clear();
                }
                else
                {
                    Messages.InformationMessage("Can't save without atleast one employee");
                    strVoucherNo = spMaster.SalaryVoucherMasterGetMax(decMonthlyVoucherTypeId);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV7:" + ex.Message;
            }
        }
        /// <summary>
        /// Function for edit
        /// </summary>
        public void EditFunction()
        {
            try
            {
                SalaryVoucherMasterSP spMaster = new SalaryVoucherMasterSP();
                SalaryVoucherMasterInfo infoMaster = new SalaryVoucherMasterInfo();
                SalaryVoucherDetailsSP spDetails = new SalaryVoucherDetailsSP();
                SalaryVoucherDetailsInfo infoDetails = new SalaryVoucherDetailsInfo();
                LedgerPostingSP spLedgerPosting = new LedgerPostingSP();

                infoMaster.SalaryVoucherMasterId = decIsEditModeMasterId;
                infoMaster.Date = Convert.ToDateTime(dtpVoucherDate.Value.ToString());
                infoMaster.LedgerId = Convert.ToDecimal(cmbCashOrBankAcc.SelectedValue.ToString());
                infoMaster.Narration = txtNarration.Text.Trim();
                infoMaster.TotalAmount = Convert.ToDecimal(lblTotalAmount.Text.ToString());
                if (isAutomatic)
                {
                    infoMaster.VoucherNo = strVoucherNo;
                }
                else
                {
                    infoMaster.VoucherNo = txtVoucherNo.Text;
                }

                if (isAutomatic)
                {
                    infoMaster.InvoiceNo = strInvoiceNo;
                }
                else
                {
                    infoMaster.InvoiceNo = txtVoucherNo.Text;
                }
                infoMaster.Extra1 = string.Empty;
                infoMaster.Extra2 = string.Empty;
                infoMaster.SuffixPrefixId = decMonthlySuffixPrefixId;
                infoMaster.VoucherTypeId = decMonthlyVoucherTypeId;
                infoMaster.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                infoMaster.Month = Convert.ToDateTime(dtpMonth.Text.ToString());

                infoDetails.Extra1 = string.Empty;
                infoDetails.Extra2 = string.Empty;

                int inRowCount = dgvMonthlySalary.RowCount;
                for (int i = 0; i < inRowCount; i++)
                {


                    if (dgvMonthlySalary.Rows[i].Cells["txtEmployeeId"].Value != null && dgvMonthlySalary.Rows[i].Cells["txtEmployeeId"].Value.ToString() != string.Empty)
                    {
                        infoDetails.EmployeeId = Convert.ToDecimal(dgvMonthlySalary.Rows[i].Cells["txtEmployeeId"].Value.ToString());
                    }
                    if (dgvMonthlySalary.Rows[i].Cells["txtBonus"].Value != null && dgvMonthlySalary.Rows[i].Cells["txtBonus"].Value.ToString() != string.Empty)
                    {
                        infoDetails.Bonus = Convert.ToDecimal(dgvMonthlySalary.Rows[i].Cells["txtBonus"].Value.ToString());
                    }
                    if (dgvMonthlySalary.Rows[i].Cells["txtDeduction"].Value != null && dgvMonthlySalary.Rows[i].Cells["txtDeduction"].Value.ToString() != string.Empty)
                    {
                        infoDetails.Deduction = Convert.ToDecimal(dgvMonthlySalary.Rows[i].Cells["txtDeduction"].Value.ToString());
                    }
                    if (dgvMonthlySalary.Rows[i].Cells["txtAdvance"].Value != null && dgvMonthlySalary.Rows[i].Cells["txtAdvance"].Value.ToString() != string.Empty)
                    {
                        infoDetails.Advance = Convert.ToDecimal(dgvMonthlySalary.Rows[i].Cells["txtAdvance"].Value.ToString());
                    }
                    if (dgvMonthlySalary.Rows[i].Cells["txtLop"].Value != null && dgvMonthlySalary.Rows[i].Cells["txtLop"].Value.ToString() != string.Empty)
                    {
                        infoDetails.Lop = Convert.ToDecimal(dgvMonthlySalary.Rows[i].Cells["txtLop"].Value.ToString());
                    }
                    if (dgvMonthlySalary.Rows[i].Cells["txtSalary"].Value != null && dgvMonthlySalary.Rows[i].Cells["txtSalary"].Value.ToString() != string.Empty)
                    {
                        infoDetails.Salary = Convert.ToDecimal(dgvMonthlySalary.Rows[i].Cells["txtSalary"].Value.ToString());
                    }
                    if (dgvMonthlySalary.Rows[i].Cells["cmbStatus"].Value != null && dgvMonthlySalary.Rows[i].Cells["cmbStatus"].Value.ToString() != string.Empty)
                    {
                        infoDetails.Status = dgvMonthlySalary.Rows[i].Cells["cmbStatus"].Value.ToString();
                    }


                    if (dgvMonthlySalary.Rows[i].Cells["cmbStatus"].Value.ToString() == string.Empty && dgvMonthlySalary.Rows[i].Cells["txtMasterId"].Value.ToString() != "0")
                    {
                        decimal SalaryVoucherDetailsId = Convert.ToDecimal(dgvMonthlySalary.Rows[i].Cells["txtDetailsId"].Value.ToString());
                        spDetails.SalaryVoucherDetailsDelete(SalaryVoucherDetailsId);
                        spMaster.SalaryVoucherMasterEdit(infoMaster);

                        LedgerUpdate();

                    }

                    if (dgvMonthlySalary.Rows[i].Cells["cmbStatus"].Value.ToString() == "Paid" && dgvMonthlySalary.Rows[i].Cells["txtMasterId"].Value.ToString() == "0")
                    {
                        infoDetails.SalaryVoucherMasterId = decIsEditModeMasterId;
                        spDetails.MonthlySalaryVoucherDetailsAdd(infoDetails);
                        spMaster.SalaryVoucherMasterEdit(infoMaster);

                        LedgerUpdate();
                    }
                    if (dgvMonthlySalary.Rows[i].Cells["cmbStatus"].Value.ToString() == string.Empty && dgvMonthlySalary.Rows[i].Cells["txtMasterId"].Value.ToString() == "0")
                    {
                        spMaster.SalaryVoucherMasterEdit(infoMaster);
                        LedgerUpdate();
                    }
                }
                if (spDetails.SalaryVoucherDetailsCount(decIsEditModeMasterId) == 0)
                {
                    spMaster.SalaryVoucherMasterDelete(decIsEditModeMasterId);
                }
                Messages.UpdatedMessage();
                GridFill(isEditMode);

                if (frmMonthlySalaryRegisterObj != null)
                {
                    this.Close();
                }
                if (frmLedgerDetailsObj != null)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV8:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmDayBook 
        /// </summary>
        /// <param name="frmDayBook"></param>
        /// <param name="decMasterId"></param>
        public void CallFromDayBook(frmDayBook frmDayBook, decimal decMasterId)
        {
            try
            {
                isEditMode = true;
                frmDayBook.Enabled = false;
                decIsEditModeMasterId = decMasterId;
                base.Show();
                btnDelete.Enabled = true;
                btnSave.Text = "Update";
                frmDayBookObj = frmDayBook;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV9:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill controls for updation
        /// </summary>
        public void FillFunction()
        {
            try
            {
                SalaryVoucherMasterSP SpMaster = new SalaryVoucherMasterSP();
                SalaryVoucherMasterInfo InfoMaster = new SalaryVoucherMasterInfo();
                VoucherTypeSP spVoucherType = new VoucherTypeSP();

                InfoMaster = SpMaster.SalaryVoucherMasterView(decIsEditModeMasterId);

                strVoucherNoforEdit = InfoMaster.VoucherNo;
                strVoucherNo = InfoMaster.VoucherNo;
                txtVoucherNo.Text = InfoMaster.InvoiceNo;
                strInvoiceNo = InfoMaster.InvoiceNo;
              
                txtVoucherDate.Text = InfoMaster.Date.ToString("dd-MMMM-yyyy");
                string stra = Convert.ToDateTime(InfoMaster.Month.ToString()).ToString("MMM yyyy");
                dtpMonth.Value = Convert.ToDateTime(InfoMaster.Month.ToString("MMM yyyy"));
                txtNarration.Text = InfoMaster.Narration;

                decimal decTotalAmont = Math.Round(Convert.ToDecimal(InfoMaster.TotalAmount.ToString()), PublicVariables._inNoOfDecimalPlaces);
                lblTotalAmount.Text = decTotalAmont.ToString();
                decMonthlySuffixPrefixId = InfoMaster.SuffixPrefixId;
                decMonthlyVoucherTypeId = InfoMaster.VoucherTypeId;

                cmbCashOrBankAcc.SelectedValue = InfoMaster.LedgerId;
                isAutomatic = spVoucherType.CheckMethodOfVoucherNumbering(decMonthlyVoucherTypeId);
                if (isAutomatic)
                {
                    txtVoucherNo.Enabled = false;
                }
                else
                {
                    txtVoucherNo.Enabled = true;
                }
                dtpMonth.Enabled = false;

                GridFill(isEditMode);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV10:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmMonthlySalaryRegister to view details for updation
        /// </summary>
        /// <param name="decMasterId"></param>
        /// <param name="frmMonthlySalaryRegister"></param>
        public void CallFromMonthlySalaryRegister(decimal decMasterId, frmMonthlySalaryRegister frmMonthlySalaryRegister)
        {
            try
            {
                isEditMode = true;
                decIsEditModeMasterId = decMasterId;
                base.Show();
                btnDelete.Enabled = true;
                btnSave.Text = "Update";
                this.frmMonthlySalaryRegisterObj = frmMonthlySalaryRegister;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV11:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmLedgerPopup to view details
        /// </summary>
        /// <param name="frmLedgerPopup"></param>
        /// <param name="decId"></param>
        public void CallFromLedgerPopup(frmLedgerPopup frmLedgerPopup, decimal decId) //PopUp
        {
            try
            {
                base.Show();
                this.frmLedgerPopupObj = frmLedgerPopup;
                TransactionsGeneralFill Obj = new TransactionsGeneralFill();
                Obj.CashOrBankComboFill(cmbCashOrBankAcc, false);
                cmbCashOrBankAcc.SelectedValue = decId;
                cmbCashOrBankAcc.Focus();
                frmLedgerPopupObj.Close();
                frmLedgerPopupObj = null;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV12:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to reset the forms
        /// </summary>
        public void Clear()
        {
            try
            {

                if (isEditMode == false)
                {
                    if (isAutomatic)
                    {
                        voucherNumberGeneration();
                        txtVoucherDate.Focus();
                    }
                    else
                    {
                        txtVoucherNo.Text = string.Empty;
                        txtVoucherNo.ReadOnly = false;
                        txtVoucherNo.Focus();
                    }

                }

                dtpVoucherDate.Value = PublicVariables._dtCurrentDate;
                dtpVoucherDate.MinDate = PublicVariables._dtFromDate;
                dtpVoucherDate.MaxDate = PublicVariables._dtToDate;
                dtpMonth.Value = PublicVariables._dtCurrentDate;
                dtpMonth.MinDate = PublicVariables._dtFromDate;
                dtpMonth.MaxDate = PublicVariables._dtToDate;
                cmbCashOrBankAcc.SelectedIndex = -1;            
                txtNarration.Text = string.Empty;
                lblTotalAmount.Text = string.Empty;
                int inCount = dgvMonthlySalary.RowCount;
                for (int i = 0; i < inCount; i++)
                {
                    dgvMonthlySalary.Rows[i].Cells["cmbStatus"].Value = null;
                }
                dgvMonthlySalary.ClearSelection();
                if (frmMonthlySalaryRegisterObj != null)
                {
                    frmMonthlySalaryRegisterObj.Close();
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV13:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to generate the voucher number as per settings
        /// </summary>
        public void voucherNumberGeneration()
        {
            try
            {
                TransactionsGeneralFill obj = new TransactionsGeneralFill();
                SalaryVoucherMasterSP spMaster = new SalaryVoucherMasterSP();
                //-----------------------------------------Voucher number Automatic generation ------------------------------------------------//
                if (strVoucherNo == string.Empty)
                {

                   strVoucherNo = "0"; 
                }
                strVoucherNo = obj.VoucherNumberAutomaicGeneration(decMonthlyVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpVoucherDate.Value, tableName);
                if (Convert.ToDecimal(strVoucherNo) != spMaster.SalaryVoucherMasterGetMaxPlusOne(decMonthlyVoucherTypeId))
                {
                    strVoucherNo = spMaster.SalaryVoucherMasterGetMax(decMonthlyVoucherTypeId).ToString();
                    strVoucherNo = obj.VoucherNumberAutomaicGeneration(decMonthlyVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpVoucherDate.Value, tableName);
                    if (spMaster.SalaryVoucherMasterGetMax(decMonthlyVoucherTypeId) == "0")
                    {
                        strVoucherNo = "0";
                        strVoucherNo = obj.VoucherNumberAutomaicGeneration(decMonthlyVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpVoucherDate.Value, tableName);
                    }
                }
                if (isAutomatic)
                {
                    SuffixPrefixSP spSuffisprefix = new SuffixPrefixSP();
                    SuffixPrefixInfo infoSuffixPrefix = new SuffixPrefixInfo();

                    infoSuffixPrefix = spSuffisprefix.GetSuffixPrefixDetails(decMonthlyVoucherTypeId, dtpVoucherDate.Value);
                    strPrefix = infoSuffixPrefix.Prefix;
                    strSuffix = infoSuffixPrefix.Suffix;
                    strInvoiceNo = strPrefix + strVoucherNo + strSuffix;
                    txtVoucherNo.Text = strInvoiceNo;
                    txtVoucherNo.ReadOnly = true;
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV14:" + ex.Message;
            }

        }
        /// <summary>
        /// Function to calculate the totalamount
        /// </summary>
        public void GetTotalAmount()
        {
            try
            {
                decimal decTotal = 0;

                if (dgvMonthlySalary.RowCount > 0)
                {
                    foreach (DataGridViewRow dgvRow in dgvMonthlySalary.Rows)
                    {
                        if (dgvRow.Cells["txtSalary"].Value != null && dgvRow.Cells["txtSalary"].Value.ToString() != string.Empty)
                        {

                            if (dgvRow.Cells["cmbStatus"].Value != null)
                            {
                                if (dgvRow.Cells["cmbStatus"].Value.ToString() == "Paid")
                                {
                                    decTotal += Convert.ToDecimal(dgvRow.Cells["txtSalary"].Value.ToString());
                                }
                            }
                        }

                    }
                }
               
                if (decTotal == 0)
                {
                    lblTotalAmount.Text = "0.00";
                }
                else
                {
                    lblTotalAmount.Text = Math.Round(Convert.ToDecimal(decTotal.ToString()), PublicVariables._inNoOfDecimalPlaces).ToString();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV15:" + ex.Message;
            }
        }
        /// <summary>
        /// Function for delete
        /// </summary>
        public void DeleteFunction()
        {
            try
            {
                SalaryVoucherMasterSP spMaster = new SalaryVoucherMasterSP();
                SalaryVoucherDetailsSP spDetails = new SalaryVoucherDetailsSP();
                LedgerPostingSP spLedgerPosting = new LedgerPostingSP();
                spMaster.SalaryVoucherMasterDelete(decIsEditModeMasterId);
                spDetails.SalaryVoucherDetailsDeleteUsingMasterId(decIsEditModeMasterId);
                spLedgerPosting.LedgerPostDelete(txtVoucherNo.Text, decMonthlyVoucherTypeId);

                Messages.DeletedMessage();
                Clear();
                GridFill(isEditMode);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV16:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill cash/Bank combobox while return from AccountLedger when creating new ledger 
        /// </summary>
        /// <param name="decId"></param>
        public void ReturnFromAccountLedgerForm(decimal decId)
        {
            try
            {
                TransactionsGeneralFill Obj = new TransactionsGeneralFill();
                Obj.CashOrBankComboFill(cmbCashOrBankAcc, false);
                cmbCashOrBankAcc.SelectedValue = decId.ToString();
                this.Enabled = true;
                cmbCashOrBankAcc.Focus();

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV17:" + ex.Message;
            }


        }
        /// <summary>
        /// Function to call this form from frmLedgerDetails to view and update
        /// </summary>
        /// <param name="frmLedgerDetails"></param>
        /// <param name="decMasterId"></param>
        public void CallFromLedgerDetails(frmLedgerDetails frmLedgerDetails, decimal decMasterId)
        {
            try
            {
                isEditMode = true;
                decIsEditModeMasterId = decMasterId;
                base.Show();
                btnDelete.Enabled = true;
                btnSave.Text = "Update";
                frmLedgerDetailsObj = frmLedgerDetails;
                frmLedgerDetailsObj.Enabled = false;
                FillFunction();

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV18:" + ex.Message;
            }
        }
        #endregion

        #region Events
       /// <summary>
       /// Load
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void frmMonthlySalaryVoucher_Load(object sender, EventArgs e)
        {
            try
            {
                lblCashOrBankIndicator.Visible = true;
                lblVoucherDateIndicator.Visible = true;
                lblVoucherNoIndicator.Visible = true;             
                TransactionsGeneralFill Obj = new TransactionsGeneralFill();
                Obj.CashOrBankComboFill(cmbCashOrBankAcc, false);
                dtpMonth.Value = PublicVariables._dtCurrentDate;
                dtpMonth.MinDate = PublicVariables._dtFromDate;
                dtpMonth.MaxDate = PublicVariables._dtToDate;
                dtpVoucherDate.Value = PublicVariables._dtCurrentDate;
                dtpVoucherDate.MinDate = PublicVariables._dtFromDate;
                dtpVoucherDate.MaxDate = PublicVariables._dtToDate;             
                dtpVoucherDate.CustomFormat = "dd-MMMM-yyyy";
             
                if (isEditMode == false)
                {
                    Clear();
                }
                GridFill(isEditMode);

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV19:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Save'button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnSave.Text))
                {
                    SaveOrEdit();

                }
                else
                {
                    Messages.NoPrivillageMessage();

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV20:" + ex.Message;
            }
        }
        /// <summary>
        /// Clears selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvMonthlySalary_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                dgvMonthlySalary.ClearSelection();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV21:" + ex.Message;
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
                formMDI.infoError.ErrorString = "MSV22:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Close' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
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
                formMDI.infoError.ErrorString = "MSV23:" + ex.Message;
            }

        }
        /// <summary>
        /// Commits Edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvMonthlySalary_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvMonthlySalary.IsCurrentCellDirty)
                {
                    dgvMonthlySalary.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV24:" + ex.Message;
            }
        }
        /// <summary>
        /// Fills Datagridview on dtpMonth datetimepicker ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpMonth_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                GridFill(isEditMode);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV25:" + ex.Message;
            }
        }
        /// <summary>
        /// Calculates the TotalAmount on CellValueChanged whether selecting status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvMonthlySalary_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (e.RowIndex != -1 && e.ColumnIndex != -1)
                {
                    GetTotalAmount();

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV26:" + ex.Message;
            }
        }
        /// <summary>
        /// Date validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVoucherDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation objVal = new DateValidation();
                bool isInvalid = objVal.DateValidationFunction(txtVoucherDate);
                if (!isInvalid)
                {
                    txtVoucherDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
               
                dtpVoucherDate.Value = Convert.ToDateTime(txtVoucherDate.Text);
                //-------------Re fill ---------------------------//
                if (!isEditMode)
                {

                    voucherNumberGeneration();
                }
              
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV27:" + ex.Message;
            }
        }
        /// <summary>
        /// Fills txtVoucherDate textbox on dtpVoucherDate datetimepicker ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpVoucherDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpVoucherDate.Value;
                this.txtVoucherDate.Text = date.ToString("dd-MMM-yyyy");
                txtVoucherDate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV28:" + ex.Message;
            }
        }
        //To create a new Account Ledger using btnAccountLedgerAdd
        private void btnAccountLedgerAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbCashOrBankAcc.SelectedValue != null)
                {
                    strledgerId = cmbCashOrBankAcc.SelectedValue.ToString();
                }
                else
                {
                    strledgerId = string.Empty;
                }
                frmAccountLedger frmAccountledger = new frmAccountLedger();
                frmAccountledger.MdiParent = formMDI.MDIObj;

                frmAccountLedger open = Application.OpenForms["frmAccountLedger"] as frmAccountLedger;
                if (open == null)
                {
                    frmAccountledger.WindowState = FormWindowState.Normal;
                    frmAccountledger.MdiParent = formMDI.MDIObj;
                    frmAccountledger.CallFromMonthlySalaryVoucher(this);
                }
                else
                {
                    open.MdiParent = formMDI.MDIObj;
                    open.BringToFront();
                    open.CallFromMonthlySalaryVoucher(this);
                    if (open.WindowState == FormWindowState.Minimized)
                    {
                        open.WindowState = FormWindowState.Normal;
                    }
                }
                this.Enabled = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV29:" + ex.Message;
            }
        }
        /// <summary>
        /// Enables the object of other forms on form closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMonthlySalaryVoucher_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (frmMonthlySalaryRegisterObj != null)
                {
                    frmMonthlySalaryRegisterObj.Enabled = true;
                    frmMonthlySalaryRegisterObj.BringToFront();
                    frmMonthlySalaryRegisterObj.Clear();
                }
                if (objVoucherSearch != null)
                {
                    objVoucherSearch.Enabled = true;
                    objVoucherSearch.GridFill();
                    objVoucherSearch.Activate();
                }
                if (frmDayBookObj != null)
                {
                    frmDayBookObj.Enabled = true;
                    frmDayBookObj.dayBookGridFill();
                    frmDayBookObj.Activate();
                    frmDayBookObj = null;
                }
                if (frmLedgerDetailsObj != null)
                {
                    frmLedgerDetailsObj.Enabled = true;
                    frmLedgerDetailsObj.Activate();

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV30:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Delete' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {

                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnDelete.Text))
                {
                    if (PublicVariables.isMessageDelete)
                    {
                        if (Messages.DeleteMessage())
                        {
                            DeleteFunction();
                        }
                    }
                    else
                    {
                        DeleteFunction();
                    }
                }
                else
                {
                    Messages.NoPrivillageMessage();

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV31:" + ex.Message;
            }
        }

        /// <summary>
        /// Quick acess on form keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMonthlySalaryVoucher_KeyDown(object sender, KeyEventArgs e)
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
                if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control)
                {
                    btnSave_Click(sender, e);
                }
                if (e.KeyCode == Keys.D && Control.ModifierKeys == Keys.Control)
                {
                    if (btnDelete.Enabled)
                    {
                        btnDelete_Click(sender, e);
                    }
                }               
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV32:" + ex.Message;
            }
        }   
        #endregion

        #region Navigation
        /// <summary>
        /// Enter key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVoucherNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtVoucherDate.Focus();
                    txtVoucherDate.SelectionStart = 0;
                    txtVoucherDate.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV33:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvMonthlySalary_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvMonthlySalary.CurrentRow == dgvMonthlySalary.Rows[dgvMonthlySalary.Rows.Count - 1])
                    {
                        if (q == 1)
                        {
                            q = 0;
                            txtNarration.Focus();
                            dgvMonthlySalary.ClearSelection();
                            e.Handled = true;
                        }
                        else
                        {
                            q++;
                        }
                    }
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (dgvMonthlySalary.Rows.Count > 0)
                    {
                        if (dgvMonthlySalary.CurrentCell == dgvMonthlySalary.Rows[0].Cells["cmbStatus"])
                        {
                            cmbCashOrBankAcc.Focus();
                        }
                        else
                        {
                            if (dgvMonthlySalary.CurrentCell.RowIndex > 0)
                            {
                                dgvMonthlySalary.CurrentCell = dgvMonthlySalary.Rows[dgvMonthlySalary.CurrentCell.RowIndex - 1].Cells["cmbStatus"];
                            }
                            else
                            {
                                cmbCashOrBankAcc.Focus();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV34:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVoucherDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dtpMonth.Enabled != false)
                    {
                        dtpMonth.Focus();
                    }
                    else
                    {
                        cmbCashOrBankAcc.Focus();
                    }
                }
                if (txtVoucherDate.Text == string.Empty || txtVoucherDate.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        if (!txtVoucherNo.ReadOnly)
                        {
                            txtVoucherNo.Focus();
                            txtVoucherNo.SelectionStart = 0;
                            txtVoucherNo.SelectionLength = 0;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV35:" + ex.Message;
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
                    cmbCashOrBankAcc.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtVoucherDate.Focus();
                    txtVoucherDate.SelectionStart = 0;
                    txtVoucherDate.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV36:" + ex.Message;
            }

        }
        /// <summary>
        /// Enter key and Backspace navigation and quick access
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCashOrBankAcc_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvMonthlySalary.RowCount > 0)
                    {
                        dgvMonthlySalary.Focus();
                        dgvMonthlySalary.Rows[0].Cells["cmbStatus"].Selected = true;
                        dgvMonthlySalary.CurrentCell = dgvMonthlySalary.Rows[0].Cells["cmbStatus"];
                    }
                    else
                    {
                        txtNarration.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (dtpMonth.Enabled != false)
                    {
                        dtpMonth.Focus();
                    }
                    else
                    {
                        txtVoucherDate.Focus();
                    }
                }
                if (e.KeyCode == Keys.F && Control.ModifierKeys == Keys.Control) //Pop Up
                {
                    if (cmbCashOrBankAcc.SelectedIndex != -1)
                    {
                        frmLedgerPopupObj = new frmLedgerPopup();
                        frmLedgerPopupObj.MdiParent = formMDI.MDIObj;
                        frmLedgerPopupObj.CallFromMonthlySalaryVoucher(this, Convert.ToDecimal(cmbCashOrBankAcc.SelectedValue.ToString()), "CashOrBank");
                    }
                    else
                    {
                        Messages.InformationMessage("Select any cash or bank account ");
                    }
                }
                if (e.KeyCode == Keys.C && Control.ModifierKeys == Keys.Alt)
                {
                    SendKeys.Send("{F10}");
                    btnAccountLedgerAdd_Click(sender, e);

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV37:" + ex.Message;
            }

        }
        /// <summary>
        /// Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {

                    if (txtNarration.Text.Trim() == string.Empty || txtNarration.SelectionStart == 0)
                    {
                        if (dgvMonthlySalary.RowCount > 0)
                        {
                            dgvMonthlySalary.Focus();
                            dgvMonthlySalary.Rows[dgvMonthlySalary.RowCount - 1].Cells["cmbStatus"].Selected = true;
                        }
                        else
                        {
                            cmbCashOrBankAcc.Focus();
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV38:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    inNarrationCount++;
                    if (inNarrationCount == 2)
                    {
                        inNarrationCount = 0;
                        btnSave.Focus();
                    }
                }
                else
                {
                    inNarrationCount = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV39:" + ex.Message;
            }
        }
        /// <summary>
        /// navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_Enter(object sender, EventArgs e)
        {
            try
            {
                inNarrationCount = 0;
                txtNarration.Text = txtNarration.Text.Trim();
                if (txtNarration.Text.Trim() == string.Empty)
                {
                    txtNarration.SelectionStart = 0;
                    txtNarration.SelectionLength = 0;
                    txtNarration.Focus();
                }
                else
                {
                    txtNarration.SelectionStart = txtNarration.Text.Trim().Length;
                    txtNarration.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV40:" + ex.Message;
            }
        }
        /// <summary>
        /// Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    txtNarration.Focus();
                    txtNarration.SelectionStart = 0;
                    txtNarration.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MSV41:" + ex.Message;
            }
        }
     
        #endregion

    }

}
