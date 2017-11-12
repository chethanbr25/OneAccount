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
    public partial class frmDailySalaryVoucher : Form
    {

        #region PUBLIC VARIABLES
        /// <summary>
        /// Public variable declaration part
        /// </summary>

        decimal decDailyVoucherTypeId = 0;
        decimal decDailySuffixPrefixId = 0;
        decimal decMasterId = 0;
        decimal decMasterIdforEdit = 0;

        int inNarrationCount = 0;
        int dgvcell = 0;

        string strTableName = "DailySalaryVoucherMaster";
        string strVoucherNo = string.Empty;
        string strInvoiceNo = string.Empty;
        string strPrefix = string.Empty;
        string strSuffix = string.Empty;
        string strEployeeNames = string.Empty;
        string strVoucherNoforEdit = "0";
        string strUpdatedVoucherNo = string.Empty;
        string strUpdatedInvoiceNo = string.Empty;
        string strledgerId;

        bool isAutomatic = false;
        bool @isEditmode = false;

        frmDailySalaryRegister frmDailySalaryRegisterobj;
        frmLedgerPopup frmLedgerPopupObj;
        frmVoucherSearch objVoucherSearch = null;
        frmDayBook frmDayBookObj = null;

        #endregion

        #region FUNCTION
        /// <summary>
        /// Creates an Instance of frmDailySalaryVoucher class
        /// </summary>
        public frmDailySalaryVoucher()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Function to Fill Cash Or Bank Combobox
        /// </summary>
        public void DailySalaryVoucherCashorBankComboFill()
        {
            try
            {
                DailySalaryVoucherMasterSP spMaster = new DailySalaryVoucherMasterSP();
                DataTable dtblDailySalaryVoucherMaster = new DataTable();
                dtblDailySalaryVoucherMaster = spMaster.DailySalaryVoucherCashOrBankLedgersComboFill();
                cmbCashorBankAccount.DataSource = dtblDailySalaryVoucherMaster;
                cmbCashorBankAccount.ValueMember = "ledgerId";
                cmbCashorBankAccount.DisplayMember = "ledgerName";
                cmbCashorBankAccount.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Datagridview
        /// </summary>
        /// <param name="isEditmode"></param>
        public void DailySalaryVoucherDetailsGridfill(bool @isEditmode)
        {
            try
            {
                DataTable dtbl = new DataTable();
                DailySalaryVoucherDetailsSP spDetails = new DailySalaryVoucherDetailsSP();
                dtbl = spDetails.DailySalaryVoucherDetailsGridViewAll(dtpSalaryDate.Value.ToString(), isEditmode, strVoucherNoforEdit);
                dgvDailySalaryVoucher.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV2:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to call this form from VoucherType Selection form
        /// </summary>
        /// <param name="decDailySalaryVoucherTypeId"></param>
        /// <param name="strVoucherTypeNames1"></param>
        public void CallFromVoucherTypeSelection(decimal decDailySalaryVoucherTypeId, string strVoucherTypeNames1)
        {
            try
            {
                decDailyVoucherTypeId = decDailySalaryVoucherTypeId;
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                isAutomatic = spVoucherType.CheckMethodOfVoucherNumbering(decDailyVoucherTypeId);
                SuffixPrefixSP spSuffisprefix = new SuffixPrefixSP();
                SuffixPrefixInfo infoSuffixPrefix = new SuffixPrefixInfo();

                infoSuffixPrefix = spSuffisprefix.GetSuffixPrefixDetails(decDailyVoucherTypeId, dtpDate.Value);
                decDailySuffixPrefixId = infoSuffixPrefix.SuffixprefixId;
                strPrefix = infoSuffixPrefix.Prefix;
                strSuffix = infoSuffixPrefix.Suffix;

                this.Text = strVoucherTypeNames1;
                base.Show();
                if (isAutomatic)
                {
                    txtDate.Focus();
                }
                else
                {
                    txtVoucherNo.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV3:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmDailySalaryRegister to view details and for updation 
        /// </summary>
        /// <param name="decdailySalaryVoucehrMasterId"></param>
        /// <param name="frmDailySalaryRegister"></param>
        public void CallFromDailySalaryVoucherRegister(decimal decdailySalaryVoucehrMasterId, frmDailySalaryRegister frmDailySalaryRegister)
        {
            try
            {
                isEditmode = true;
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
                base.Show();
                frmDailySalaryRegisterobj = frmDailySalaryRegister;
                decMasterIdforEdit = decdailySalaryVoucehrMasterId;
                fillFunction();

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV4:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmDayBook to view details and for updation 
        /// </summary>
        /// <param name="frmDayBook"></param>
        /// <param name="decMasterId"></param>
        public void CallFromDayBook(frmDayBook frmDayBook, decimal decMasterId)
        {
            try
            {
                isEditmode = true;
                btnSave.Text = "Update";
                frmDayBook.Enabled = false;
                btnDelete.Enabled = true;
                base.Show();
                frmDayBookObj = frmDayBook;
                decMasterIdforEdit = decMasterId;
                fillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV5:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill controls for updation
        /// </summary>
        public void fillFunction()
        {
            try
            {
                DailySalaryVoucherMasterSP SpMaster = new DailySalaryVoucherMasterSP();
                DailySalaryVoucherMasterInfo InfoMaster = new DailySalaryVoucherMasterInfo();
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                InfoMaster = SpMaster.DailySalaryVoucherViewFromRegister(decMasterIdforEdit);

                strVoucherNoforEdit = InfoMaster.VoucherNo;
                strVoucherNo = InfoMaster.VoucherNo;
                txtVoucherNo.Text = InfoMaster.InvoiceNo;
                strInvoiceNo = InfoMaster.InvoiceNo;
                dtpDate.Text = InfoMaster.Date.ToString();
                dtpSalaryDate.Text = InfoMaster.SalaryDate.ToString();
                lblShowTotelamount.Text = Math.Round(Convert.ToDecimal(InfoMaster.TotalAmount.ToString("0.00000")), PublicVariables._inNoOfDecimalPlaces).ToString();
                txtNarration.Text = InfoMaster.Narration;
                decDailySuffixPrefixId = InfoMaster.SuffixPrefixId;
                decDailyVoucherTypeId = InfoMaster.VoucherTypeId;
                cmbCashorBankAccount.SelectedValue = InfoMaster.LedgerId.ToString();
                isAutomatic = spVoucherType.CheckMethodOfVoucherNumbering(decDailyVoucherTypeId);
                if (isAutomatic)
                {
                    txtVoucherNo.Enabled = false;
                }
                else
                {
                    txtVoucherNo.Enabled = true;
                }

                dtpSalaryDate.Enabled = false;
                txtSalaryDate.Enabled = false;
                DailySalaryVoucherDetailsGridfill(isEditmode);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV6:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmVoucherSearch to view details and for updation 
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="decId"></param>
        public void CallThisFormFromVoucherSearch(frmVoucherSearch frm, decimal decId)
        {

            try
            {
                this.objVoucherSearch = frm;
                decMasterIdforEdit = decId;

                fillFunction();


            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV7:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to calculate the wages paid
        /// </summary>
        public void TotelWageAmount()
        {
            try
            {
                decimal decPayTotal = 0;

                if (dgvDailySalaryVoucher.RowCount > 0)
                {
                    foreach (DataGridViewRow dgvRow in dgvDailySalaryVoucher.Rows)
                    {
                        if (dgvRow.Cells["txtWage"].Value != null && dgvRow.Cells["txtWage"].Value.ToString() != string.Empty)
                        {
                            if (dgvRow.Visible == true)
                            {

                                if (dgvRow.Cells["cmbStatus"].Value != null)
                                {
                                    if (dgvRow.Cells["cmbStatus"].Value.ToString() == "paid")
                                    {
                                        if (dgvRow.Cells["txtAttendanceStatus"].Value.ToString() == "Present")
                                        {
                                            decPayTotal = decPayTotal + Convert.ToDecimal(dgvRow.Cells["txtWage"].Value.ToString());
                                        }
                                        else
                                        {
                                            decPayTotal = decPayTotal + ((Convert.ToDecimal(dgvRow.Cells["txtWage"].Value.ToString())) * decimal.Parse("0.5".ToString()));
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
                lblShowTotelamount.Text = Math.Round(Convert.ToDecimal(decPayTotal.ToString("0.00000")), PublicVariables._inNoOfDecimalPlaces).ToString();

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV8:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to save data to ledgerposting table
        /// </summary>
        /// <param name="decid"></param>
        public void LedgerPosting(decimal decid)
        {
            try
            {
                LedgerPostingSP spLedgerPosting = new LedgerPostingSP();
                LedgerPostingInfo infoLedgerPosting = new LedgerPostingInfo();

                infoLedgerPosting.Debit = 0;
                infoLedgerPosting.Credit = Convert.ToDecimal(lblShowTotelamount.Text.ToString());
                infoLedgerPosting.VoucherTypeId = decDailyVoucherTypeId;
                if (isAutomatic)
                {
                    infoLedgerPosting.VoucherNo = strVoucherNo;
                }
                else
                {
                    infoLedgerPosting.VoucherNo = txtVoucherNo.Text.Trim();
                }
                infoLedgerPosting.Date = Convert.ToDateTime(dtpDate.Value.ToString());
                infoLedgerPosting.LedgerId = decid;
                infoLedgerPosting.DetailsId = 0;
                if (isAutomatic)
                {
                    infoLedgerPosting.InvoiceNo = strInvoiceNo;
                }
                else
                {
                    infoLedgerPosting.InvoiceNo = txtVoucherNo.Text.Trim();
                }
                infoLedgerPosting.ChequeNo = string.Empty;
                infoLedgerPosting.ChequeDate = DateTime.Now;
                infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                infoLedgerPosting.Extra1 = string.Empty;
                infoLedgerPosting.Extra2 = string.Empty;

                spLedgerPosting.LedgerPostingAdd(infoLedgerPosting);

                infoLedgerPosting.Debit = Convert.ToDecimal(lblShowTotelamount.Text.ToString());
                infoLedgerPosting.Credit = 0;
                infoLedgerPosting.VoucherTypeId = decDailyVoucherTypeId;
                if (isAutomatic)
                {
                    infoLedgerPosting.VoucherNo = strVoucherNo;
                }
                else
                {
                    infoLedgerPosting.VoucherNo = txtVoucherNo.Text.Trim();
                }
                infoLedgerPosting.Date = PublicVariables._dtCurrentDate;
                infoLedgerPosting.LedgerId = 4;
                infoLedgerPosting.DetailsId = 0;
                if (isAutomatic)
                {
                    infoLedgerPosting.InvoiceNo = strInvoiceNo;
                }
                else
                {
                    infoLedgerPosting.InvoiceNo = txtVoucherNo.Text.Trim();
                }
                infoLedgerPosting.ChequeNo = string.Empty;
                infoLedgerPosting.ChequeDate = DateTime.Now;
                infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                infoLedgerPosting.Extra1 = string.Empty;
                infoLedgerPosting.Extra2 = string.Empty;
                spLedgerPosting.LedgerPostingAdd(infoLedgerPosting);

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV9:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to edit LedgerPosting table
        /// </summary>
        public void LedgerUpdate()
        {
            try
            {
                decimal decLedgerPostingId = 0;
                LedgerPostingSP spLedgerPosting = new LedgerPostingSP();
                LedgerPostingInfo infoLedgerPosting = new LedgerPostingInfo();
                DataTable dtbl = new DataTable();
                dtbl = spLedgerPosting.GetLedgerPostingIds(strVoucherNo, decDailyVoucherTypeId);
                int ini = 0;
                foreach (DataRow dr in dtbl.Rows)
                {
                    ini++;

                    if (ini == 2)
                    {
                        decLedgerPostingId = Convert.ToDecimal(dr.ItemArray[0].ToString());
                        infoLedgerPosting.LedgerPostingId = decLedgerPostingId;
                        infoLedgerPosting.Date = Convert.ToDateTime(dtpDate.Value.ToString());
                        if (isAutomatic)
                        {
                            infoLedgerPosting.VoucherNo = strVoucherNo;
                        }
                        else
                        {
                            infoLedgerPosting.VoucherNo = txtVoucherNo.Text.Trim();
                        }
                        infoLedgerPosting.Debit = 0;
                        infoLedgerPosting.Credit = Convert.ToDecimal(lblShowTotelamount.Text.ToString());
                        infoLedgerPosting.VoucherTypeId = decDailyVoucherTypeId;
                        infoLedgerPosting.LedgerId = 4;
                        infoLedgerPosting.DetailsId = 0;
                        if (isAutomatic)
                        {
                            infoLedgerPosting.InvoiceNo = strInvoiceNo;
                        }
                        else
                        {
                            infoLedgerPosting.InvoiceNo = txtVoucherNo.Text.Trim();
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
                        infoLedgerPosting.Date = Convert.ToDateTime(dtpDate.Value.ToString());
                        if (isAutomatic)
                        {
                            infoLedgerPosting.VoucherNo = strVoucherNo;
                        }
                        else
                        {
                            infoLedgerPosting.VoucherNo = txtVoucherNo.Text.Trim();
                        }
                        infoLedgerPosting.Debit = Convert.ToDecimal(lblShowTotelamount.Text.ToString());
                        infoLedgerPosting.Credit = 0;
                        infoLedgerPosting.VoucherTypeId = decDailyVoucherTypeId;
                        infoLedgerPosting.LedgerId = Convert.ToDecimal(cmbCashorBankAccount.SelectedValue.ToString());
                        infoLedgerPosting.DetailsId = 0;
                        if (isAutomatic)
                        {
                            infoLedgerPosting.InvoiceNo = strInvoiceNo;
                        }
                        else
                        {
                            infoLedgerPosting.InvoiceNo = txtVoucherNo.Text.Trim();
                        }
                        infoLedgerPosting.ChequeNo = string.Empty;
                        infoLedgerPosting.ChequeDate = DateTime.Now;
                        infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                        infoLedgerPosting.Extra1 = string.Empty;
                        infoLedgerPosting.Extra2 = string.Empty;

                        spLedgerPosting.LedgerPostingEdit(infoLedgerPosting);
                    }
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV10:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call Save Or Edit
        /// </summary>
        public void SaveorEdit()
        {
            try
            {
                DailySalaryVoucherMasterSP spMaster = new DailySalaryVoucherMasterSP();
                if (txtVoucherNo.Text == string.Empty)
                {
                    Messages.InformationMessage("Enter voucher number");
                    txtVoucherNo.Focus();

                }
                else if (dtpDate.Value.ToString() == string.Empty)
                {
                    Messages.InformationMessage("Select voucher date");
                    dtpDate.Focus();
                }

                else if (dtpSalaryDate.Value.ToString() == string.Empty)
                {
                    Messages.InformationMessage("Select salary date");
                    txtSalaryDate.Focus();

                }
                else if (cmbCashorBankAccount.Text == string.Empty)
                {
                    Messages.InformationMessage("Select cash or bank account");
                    cmbCashorBankAccount.Focus();
                }
                else if (dgvDailySalaryVoucher.Rows.Count == 0)
                {
                    Messages.InformationMessage("Cant Save without atleast one employee");
                }
                else
                {

                    if (isEditmode == false)
                    {
                        if (PublicVariables.isMessageAdd)
                        {
                            if (Messages.SaveMessage())
                            {
                                if (!isAutomatic)
                                {
                                    if (spMaster.DailySalaryVoucherCheckExistence(txtVoucherNo.Text.Trim(), decDailyVoucherTypeId, 0) == false)
                                    {
                                        SaveFunction();
                                    }
                                    else
                                    {
                                        Messages.InformationMessage("Voucher number already exist");
                                        txtVoucherNo.Focus();
                                    }
                                }
                                else
                                {
                                    SaveFunction();
                                }
                            }

                            //else
                            //{
                            //    if (!isAutomatic)
                            //    {
                            //        if (spMaster.DailySalaryVoucherCheckExistence(txtVoucherNo.Text.Trim(), decDailyVoucherTypeId, 0) == false)
                            //        {
                            //            SaveFunction();
                            //        }
                            //        else
                            //        {
                            //            Messages.InformationMessage("Voucher number already exist");
                            //            txtVoucherNo.Focus();
                            //        }
                            //    }
                            //    else
                            //    {
                            //        SaveFunction();
                            //    }
                            //}

                        }
                        else
                        {
                            if (!isAutomatic)
                            {
                                if (spMaster.DailySalaryVoucherCheckExistence(txtVoucherNo.Text.Trim(), decDailyVoucherTypeId, 0) == false)
                                {
                                    SaveFunction();
                                }
                                else
                                {
                                    Messages.InformationMessage("Voucher number already exist");
                                    txtVoucherNo.Focus();
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

                        if (isEditmode)
                        {
                            if (PublicVariables.isMessageEdit)
                            {
                                if (Messages.UpdateMessage())
                                {
                                    if (!isAutomatic)
                                    {
                                        if (spMaster.DailySalaryVoucherCheckExistence(txtVoucherNo.Text.Trim(), decDailyVoucherTypeId, 0) == false)
                                        {
                                            EditFunction();
                                        }
                                        else
                                        {
                                            Messages.InformationMessage("Voucher number already exist");
                                            txtVoucherNo.Focus();
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
                                    if (spMaster.DailySalaryVoucherCheckExistence(txtVoucherNo.Text.Trim(), decDailyVoucherTypeId, 0) == false)
                                    {
                                        EditFunction();
                                    }
                                    else
                                    {
                                        Messages.InformationMessage("Voucher number already exist");
                                        txtVoucherNo.Focus();
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
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV11:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to Save
        /// </summary>
        public void SaveFunction()
        {
            try
            {

                DailySalaryVoucherMasterInfo infoMaster = new DailySalaryVoucherMasterInfo();
                DailySalaryVoucherMasterSP spMaster = new DailySalaryVoucherMasterSP();
                DailySalaryVoucherDetailsInfo infoDetails = new DailySalaryVoucherDetailsInfo();
                DailySalaryVoucherDetailsSP spDetails = new DailySalaryVoucherDetailsSP();

                //-------------In multi user case check whether salary is paying for the same persone--------------//
                int inCounts = dgvDailySalaryVoucher.RowCount;
                int incont = 0;
                decimal decVal = 0;
                for (int i = 0; i < inCounts; i++)
                {
                    decVal = Convert.ToDecimal(dgvDailySalaryVoucher.Rows[i].Cells["txtEmployeeId"].Value.ToString());
                    if (spDetails.CheckWhetherDailySalaryAlreadyPaid(decVal, dtpSalaryDate.Value) != "0")
                    {
                        strEployeeNames = strEployeeNames + spDetails.CheckWhetherDailySalaryAlreadyPaid(decVal, dtpSalaryDate.Value) + ",";
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
                if (spDetails.CheckWhetherDailySalaryAlreadyPaid(decVal, dtpSalaryDate.Value) != "0")
                {

                    Messages.InformationMessage("Salary already paid for - " + " " + strEployeeNames);
                    DailySalaryVoucherDetailsGridfill(isEditmode);
                }
                else
                {
                    if (isAutomatic)
                    {
                        infoMaster.VoucherNo = strVoucherNo;
                    }
                    else
                    {
                        infoMaster.VoucherNo = txtVoucherNo.Text.Trim();
                    }
                    infoMaster.Date = DateTime.Parse(dtpDate.Text.ToString());
                    infoMaster.SalaryDate = DateTime.Parse(dtpSalaryDate.Text.ToString());
                    infoMaster.LedgerId = Convert.ToDecimal(cmbCashorBankAccount.SelectedValue.ToString());
                    infoMaster.Narration = txtNarration.Text.Trim();
                    infoMaster.TotalAmount = Convert.ToDecimal(lblShowTotelamount.Text.ToString());
                    infoMaster.Extra1 = string.Empty; // Fields not in design//
                    infoMaster.Extra2 = string.Empty; // Fields not in design//
                    if (isAutomatic)
                    {
                        infoMaster.InvoiceNo = strInvoiceNo;
                    }
                    else
                    {
                        infoMaster.InvoiceNo = txtVoucherNo.Text.Trim();
                    }
                    infoMaster.SuffixPrefixId = decDailySuffixPrefixId;
                    infoMaster.VoucherTypeId = decDailyVoucherTypeId;
                    infoMaster.FinancialYearId = PublicVariables._decCurrentFinancialYearId;

                    int inval = 0;
                    int inCount = dgvDailySalaryVoucher.RowCount;
                    for (int i = 0; i < inCount; i++)
                    {
                        if (dgvDailySalaryVoucher.Rows[i].Cells["cmbStatus"].Value.ToString() == "paid")
                        {
                            inval++;
                        }

                    }
                    if (inval >= 0)
                    {
                        //-------------checks Voucher No. repeating in Multi user case----------//
                        DataTable dtbl = new DataTable();
                        dtbl = spMaster.DailySalaryVoucherMasterAddWithIdentity(infoMaster, isAutomatic);
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
                            if (Convert.ToDecimal(strUpdatedVoucherNo) != Convert.ToDecimal(strVoucherNo))
                            {
                                Messages.InformationMessage("Voucher number changed from  " + strInvoiceNo + "  to  " + strUpdatedInvoiceNo);
                                strVoucherNo = strUpdatedVoucherNo.ToString();
                                strInvoiceNo = strUpdatedInvoiceNo;
                            }
                        }
                        //-------------------------------------//
                        LedgerPosting(Convert.ToDecimal(cmbCashorBankAccount.SelectedValue.ToString()));

                        infoDetails.DailySalaryVocherMasterId = decMasterId;
                        infoDetails.Extra1 = string.Empty;// Fields not in design//
                        infoDetails.Extra2 = string.Empty;// Fields not in design//
                        int inRowCount = dgvDailySalaryVoucher.RowCount;
                        for (int i = 0; i < inRowCount; i++)
                        {

                            if (dgvDailySalaryVoucher.Rows[i].Cells["txtEmployeeId"].Value != null && dgvDailySalaryVoucher.Rows[i].Cells["txtEmployeeId"].Value.ToString() != string.Empty)
                            {
                                infoDetails.EmployeeId = Convert.ToDecimal(dgvDailySalaryVoucher.Rows[i].Cells["txtEmployeeId"].Value.ToString());
                            }
                            if (dgvDailySalaryVoucher.Rows[i].Cells["txtWage"].Value != null && dgvDailySalaryVoucher.Rows[i].Cells["txtWage"].Value.ToString() != string.Empty)
                            {
                                infoDetails.Wage = Convert.ToDecimal(dgvDailySalaryVoucher.Rows[i].Cells["txtWage"].Value.ToString());

                            }
                            if (dgvDailySalaryVoucher.Rows[i].Cells["cmbStatus"].Value != null && dgvDailySalaryVoucher.Rows[i].Cells["cmbStatus"].Value.ToString() != string.Empty)
                            {
                                infoDetails.Status = dgvDailySalaryVoucher.Rows[i].Cells["cmbStatus"].Value.ToString();

                            }

                            if (dgvDailySalaryVoucher.Rows[i].Cells["cmbStatus"].Value.ToString() == "paid" && dgvDailySalaryVoucher.Rows[i].Cells["dgvtxtDailySalaryVocherMasterId"].Value.ToString() == string.Empty)
                            {
                                infoDetails.DailySalaryVocherMasterId = decMasterId;
                                spDetails.DailySalaryVoucherDetailsAdd(infoDetails);
                            }
                        }
                        Messages.SavedMessage();
                        DailySalaryVoucherDetailsGridfill(isEditmode);
                        Clear();

                    }
                    else
                    {
                        strVoucherNo = spMaster.DailySalaryVoucherMasterGetMax(decDailyVoucherTypeId);
                        Messages.InformationMessage("Can't save without atleast one employee");
                        Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV12:" + ex.Message;
            }
        }
        /// <summary>
        /// Function for Edit
        /// </summary>
        public void EditFunction()
        {
            try
            {
                DailySalaryVoucherMasterInfo infoMaster = new DailySalaryVoucherMasterInfo();
                DailySalaryVoucherMasterSP spMaster = new DailySalaryVoucherMasterSP();
                DailySalaryVoucherDetailsInfo infoDetails = new DailySalaryVoucherDetailsInfo();
                DailySalaryVoucherDetailsSP spDetails = new DailySalaryVoucherDetailsSP();
                LedgerPostingSP spLedgerPosting = new LedgerPostingSP();

                infoMaster.DailySalaryVoucehrMasterId = decMasterIdforEdit;
                infoMaster.Date = Convert.ToDateTime(dtpDate.Value.ToString());
                infoMaster.LedgerId = Convert.ToDecimal(cmbCashorBankAccount.SelectedValue.ToString());
                infoMaster.Narration = txtNarration.Text;
                infoMaster.TotalAmount = Convert.ToDecimal(lblShowTotelamount.Text.ToString());
                if (isAutomatic)
                {
                    infoMaster.VoucherNo = strVoucherNo;
                }
                else
                {
                    infoMaster.VoucherNo = txtVoucherNo.Text.Trim();
                }

                if (isAutomatic)
                {
                    infoMaster.InvoiceNo = strInvoiceNo;
                }
                else
                {
                    infoMaster.InvoiceNo = txtVoucherNo.Text.Trim();
                }
                infoMaster.Extra1 = string.Empty;
                infoMaster.Extra2 = string.Empty;
                infoMaster.SuffixPrefixId = decDailySuffixPrefixId;
                infoMaster.VoucherTypeId = decDailyVoucherTypeId;
                infoMaster.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                infoMaster.SalaryDate = Convert.ToDateTime(dtpSalaryDate.Text.ToString());
                infoDetails.Extra1 = string.Empty;// Fields not in design//
                infoDetails.Extra2 = string.Empty;// Fields not in design//
                spMaster.DailySalaryVoucherMasterEdit(infoMaster);
                int inRowCount = dgvDailySalaryVoucher.RowCount;
                for (int i = 0; i < inRowCount; i++)
                {
                    if (dgvDailySalaryVoucher.Rows[i].Cells["txtEmployeeId"].Value != null && dgvDailySalaryVoucher.Rows[i].Cells["txtEmployeeId"].Value.ToString() != string.Empty)
                    {
                        infoDetails.EmployeeId = Convert.ToDecimal(dgvDailySalaryVoucher.Rows[i].Cells["txtEmployeeId"].Value.ToString());
                    }
                    if (dgvDailySalaryVoucher.Rows[i].Cells["txtWage"].Value != null && dgvDailySalaryVoucher.Rows[i].Cells["txtWage"].Value.ToString() != string.Empty)
                    {
                        infoDetails.Wage = Convert.ToDecimal(dgvDailySalaryVoucher.Rows[i].Cells["txtWage"].Value.ToString());
                    }
                    if (dgvDailySalaryVoucher.Rows[i].Cells["cmbStatus"].Value != null && dgvDailySalaryVoucher.Rows[i].Cells["cmbStatus"].Value.ToString() != string.Empty)
                    {
                        infoDetails.Status = dgvDailySalaryVoucher.Rows[i].Cells["cmbStatus"].Value.ToString();
                    }

                    if (dgvDailySalaryVoucher.Rows[i].Cells["cmbStatus"].Value.ToString() == string.Empty && dgvDailySalaryVoucher.Rows[i].Cells["dgvtxtDailySalaryVocherMasterId"].Value.ToString() != string.Empty)
                    {
                        decimal DailySalaryVoucherDetailsId = Convert.ToDecimal(dgvDailySalaryVoucher.Rows[i].Cells["dgvtxtdailySalaryVoucherDetailsId"].Value.ToString());
                        spDetails.DailySalaryVoucherDetailsDelete(DailySalaryVoucherDetailsId);

                        LedgerUpdate();

                    }
                    if (dgvDailySalaryVoucher.Rows[i].Cells["cmbStatus"].Value.ToString() == "paid" && dgvDailySalaryVoucher.Rows[i].Cells["dgvtxtDailySalaryVocherMasterId"].Value.ToString() == string.Empty)
                    {
                        infoDetails.DailySalaryVocherMasterId = decMasterIdforEdit;
                        spDetails.DailySalaryVoucherDetailsAdd(infoDetails);
                        //spMaster.DailySalaryVoucherMasterEdit(infoMaster);
                        LedgerUpdate();

                    }
                    if (dgvDailySalaryVoucher.Rows[i].Cells["cmbStatus"].Value.ToString() == string.Empty && dgvDailySalaryVoucher.Rows[i].Cells["dgvtxtDailySalaryVocherMasterId"].Value.ToString() == string.Empty)
                    {
                        spMaster.DailySalaryVoucherMasterEdit(infoMaster);
                        LedgerUpdate();

                    }
                }
                if (spDetails.DailySalaryVoucherDetailsCount(decMasterIdforEdit) == 0)
                {
                    spMaster.DailySalaryVoucherMasterDelete(decMasterIdforEdit);
                }
                Messages.UpdatedMessage();
                Clear();
                if (frmDailySalaryRegisterobj != null)
                {

                    this.Close();
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV13:" + ex.Message;
            }
        }
        /// <summary>
        /// Function for Delete
        /// </summary>
        public void DeleteFunction()
        {
            try
            {
                DailySalaryVoucherMasterSP spMaster = new DailySalaryVoucherMasterSP();
                DailySalaryVoucherDetailsSP spDetails = new DailySalaryVoucherDetailsSP();
                LedgerPostingSP spLedgerPosting = new LedgerPostingSP();

                if (PublicVariables.isMessageDelete)
                {
                    if (Messages.DeleteMessage())
                    {
                        spMaster.DailySalaryVoucherMasterDelete(decMasterIdforEdit);
                        spDetails.DailySalaryVoucherDetailsDeleteUsingMasterId(decMasterIdforEdit);
                        spLedgerPosting.LedgerPostDelete(txtVoucherNo.Text, decDailyVoucherTypeId);
                        Messages.DeletedMessage();
                        Clear();
                        DailySalaryVoucherDetailsGridfill(isEditmode);
                    }
                }
                else
                {
                    spMaster.DailySalaryVoucherMasterDelete(decMasterIdforEdit);
                    spDetails.DailySalaryVoucherDetailsDeleteUsingMasterId(decMasterIdforEdit);
                    spLedgerPosting.LedgerPostDelete(txtVoucherNo.Text, decDailyVoucherTypeId);
                    Messages.DeletedMessage();
                    Clear();
                    DailySalaryVoucherDetailsGridfill(isEditmode);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV14:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to generate Voucher number as per settings
        /// </summary>
        public void VoucherNumberGeneration()
        {
            try
            {
                TransactionsGeneralFill TransactionsGeneralFillobj = new TransactionsGeneralFill();
                DailySalaryVoucherMasterSP spmaster = new DailySalaryVoucherMasterSP();
                //--------------------------Automatic Generation------------------------//
                if (strVoucherNo == string.Empty)
                {


                    strVoucherNo = "0";
                }
                strVoucherNo = TransactionsGeneralFillobj.VoucherNumberAutomaicGeneration(decDailyVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpDate.Value, strTableName);
                if (Convert.ToDecimal(strVoucherNo) != spmaster.SalaryVoucherMasterGetMaxPlusOne(decDailyVoucherTypeId))
                {
                    strVoucherNo = spmaster.SalaryVoucherMasterGetMaxPlusOne(decDailyVoucherTypeId).ToString();
                    strVoucherNo = TransactionsGeneralFillobj.VoucherNumberAutomaicGeneration(decDailyVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpDate.Value, strTableName);
                    if (spmaster.DailySalaryVoucherMasterGetMax(decDailyVoucherTypeId) == "0")
                    {
                        strVoucherNo = "0";
                        strVoucherNo = TransactionsGeneralFillobj.VoucherNumberAutomaicGeneration(decDailyVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpDate.Value, strTableName);
                    }
                }
                //---------------------------------------------------------------------------------//
                if (isAutomatic)
                {
                    SuffixPrefixInfo infoSuffixPrefix = new SuffixPrefixInfo();
                    SuffixPrefixSP spSuffisprefix = new SuffixPrefixSP();
                    infoSuffixPrefix = spSuffisprefix.GetSuffixPrefixDetails(decDailyVoucherTypeId, dtpDate.Value);
                    strPrefix = infoSuffixPrefix.Prefix;
                    strSuffix = infoSuffixPrefix.Suffix;
                    strInvoiceNo = strPrefix + strVoucherNo + strSuffix;
                    txtVoucherNo.Text = strInvoiceNo;
                    txtVoucherNo.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV15:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to reset the form
        /// </summary>
        public void Clear()
        {
            try
            {

                if (isEditmode == false)
                {
                    if (isAutomatic)
                    {
                        VoucherNumberGeneration();
                        txtDate.Focus();
                    }
                    else
                    {
                        txtVoucherNo.Text = string.Empty;
                        txtVoucherNo.ReadOnly = false;
                        txtVoucherNo.Focus();
                    }
                    dtpSalaryDate.Value = PublicVariables._dtCurrentDate;
                    dtpSalaryDate.MinDate = PublicVariables._dtFromDate;
                    dtpSalaryDate.MaxDate = PublicVariables._dtToDate;
                }

                dtpDate.Value = PublicVariables._dtCurrentDate;
                dtpDate.MinDate = PublicVariables._dtFromDate;
                dtpDate.MaxDate = PublicVariables._dtToDate;

                dtpSalaryDate.Value = PublicVariables._dtCurrentDate;
                dtpSalaryDate.MinDate = PublicVariables._dtFromDate;
                dtpSalaryDate.MaxDate = PublicVariables._dtToDate;

                cmbCashorBankAccount.Text = string.Empty;
                txtNarration.Text = string.Empty;
                lblShowTotelamount.Text = string.Empty;
                int inCount = dgvDailySalaryVoucher.RowCount;
                for (int i = 0; i < inCount; i++)
                {
                    dgvDailySalaryVoucher.Rows[i].Cells["cmbStatus"].Value = null;
                }
                cmbCashorBankAccount.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV16:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call frmLedgerPopup form to select and view Ledger
        /// </summary>
        /// <param name="frmLedgerPopup"></param>
        /// <param name="decId"></param>
        public void CallFromLedgerPopup(frmLedgerPopup frmLedgerPopup, decimal decId) //PopUp
        {
            try
            {
                base.Show();
                this.frmLedgerPopupObj = frmLedgerPopup;
                cmbCashorBankAccount.SelectedValue = decId;
                cmbCashorBankAccount.Focus();
                frmLedgerPopupObj.Close();
                frmLedgerPopupObj = null;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV17:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Cash/Bank  combobox while return from AccountLedger Form when creating new ledger 
        /// </summary>
        /// <param name="decId"></param>
        public void ReturnFromAccountLedgerForm(decimal decId)
        {
            try
            {
                DailySalaryVoucherCashorBankComboFill();
                if (decId != 0m)   //Decimal Zero
                {
                    cmbCashorBankAccount.SelectedValue = decId.ToString();
                }
                else if (strledgerId != string.Empty)
                {
                    cmbCashorBankAccount.SelectedValue = strledgerId;
                }
                else
                {
                    cmbCashorBankAccount.SelectedIndex = -1;
                }

                this.Enabled = true;
                cmbCashorBankAccount.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV18:" + ex.Message;
            }
        }

        #endregion

        #region EVENTS


        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDailySalaryVoucher_Load(object sender, EventArgs e)
        {
            try
            {

                txtVoucherNo.Select();
                dtpDate.Value = PublicVariables._dtCurrentDate;
                dtpDate.MinDate = PublicVariables._dtFromDate;
                dtpDate.MaxDate = PublicVariables._dtToDate;
                dtpDate.CustomFormat = "dd-MMMM-yyyy";

                dtpSalaryDate.Value = PublicVariables._dtCurrentDate;
                dtpSalaryDate.MinDate = PublicVariables._dtFromDate;
                dtpSalaryDate.MaxDate = PublicVariables._dtToDate;

                TransactionsGeneralFill Obj = new TransactionsGeneralFill();
                Obj.CashOrBankComboFill(cmbCashorBankAccount, false);
                if (isEditmode == false)
                {
                    Clear();
                }

                DailySalaryVoucherDetailsGridfill(isEditmode);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV19:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Save' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnSave.Text))
                {
                    SaveorEdit();
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV20:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'clear' button click
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
                formMDI.infoError.ErrorString = "DSV21:" + ex.Message;
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
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, "Delete"))
                {
                    DeleteFunction();
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV22:" + ex.Message;
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
                formMDI.infoError.ErrorString = "DSV23:" + ex.Message;
            }
        }
        private void btnAccountLedgerAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbCashorBankAccount.SelectedValue != null)
                {
                    strledgerId = cmbCashorBankAccount.SelectedValue.ToString();
                }
                else
                {
                    strledgerId = string.Empty;
                }
                frmAccountLedger frmaccountledger = new frmAccountLedger();
                frmaccountledger.MdiParent = formMDI.MDIObj;

                frmAccountLedger open = Application.OpenForms["frmAccountLedger"] as frmAccountLedger;
                if (open == null)
                {
                    frmaccountledger.WindowState = FormWindowState.Normal;
                    frmaccountledger.MdiParent = formMDI.MDIObj;
                    frmaccountledger.CallFromDailySalaryVoucher(this);
                }
                else
                {
                    open.MdiParent = formMDI.MDIObj;
                    open.BringToFront();
                    open.CallFromDailySalaryVoucher(this);
                    if (open.WindowState == FormWindowState.Minimized)
                    {
                        open.WindowState = FormWindowState.Normal;
                    }
                }

                this.Enabled = false;

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV24:" + ex.Message;
            }
        }
        /// <summary>
        /// Form keydown for quick access
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDailySalaryVoucher_KeyDown(object sender, KeyEventArgs e)
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
                    if (btnDelete.Enabled == true)
                    {
                        btnDelete_Click(sender, e);

                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV25:" + ex.Message;
            }
        }
        /// <summary>
        /// Claculation of total wage on cell valuechanged of Datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDailySalaryVoucher_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (e.RowIndex != -1 && e.ColumnIndex != -1)
                {
                    TotelWageAmount();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV26:" + ex.Message;
            }
        }
        /// <summary>
        /// Fills Datagridview on dtpSalaryDate datetimepicker ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpSalaryDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpSalaryDate.Value;
                this.txtSalaryDate.Text = date.ToString("dd-MMM-yyyy");
                DailySalaryVoucherDetailsGridfill(isEditmode);
                lblShowTotelamount.Text = Math.Round(Convert.ToDecimal(0.00000), PublicVariables._inNoOfDecimalPlaces).ToString();
                txtNarration.Text = string.Empty;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV27:" + ex.Message;
            }
        }
        /// <summary>
        /// Commits edit on Datagridview celldirtystatechanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDailySalaryVoucher_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvDailySalaryVoucher.IsCurrentCellDirty)
                {
                    dgvDailySalaryVoucher.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV28:" + ex.Message;
            }
        }
        /// <summary>
        /// Clears selection on Databinding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDailySalaryVoucher_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                dgvDailySalaryVoucher.ClearSelection();

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV29:" + ex.Message;
            }
        }
        /// <summary>
        /// Fills txtdate textbox on dtpDate datetimepicker ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpDate.Value;
                this.txtDate.Text = date.ToString("dd-MMM-yyyy");
                txtDate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV30:" + ex.Message;
            }
        }
        /// <summary>
        /// Date vakidation and fills Datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSalaryDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation obj = new DateValidation();
                obj.DateValidationFunction(txtSalaryDate);
                if (txtSalaryDate.Text == string.Empty)
                {
                    txtSalaryDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                //---for change date in Date time picker----//
                string strdate = txtSalaryDate.Text;
                dtpSalaryDate.Value = Convert.ToDateTime(strdate.ToString());

                DataTable dtbl = new DataTable();
                DailySalaryVoucherDetailsSP spDetails = new DailySalaryVoucherDetailsSP();
                dtbl = spDetails.DailySalaryVoucherDetailsGridViewAll(txtSalaryDate.Text.ToString(), isEditmode, strVoucherNoforEdit);
                dgvDailySalaryVoucher.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV31:" + ex.Message;
            }
        }
        /// <summary>
        /// Enables the objects of other forms on Formclosing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDailySalaryVoucher_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (frmDailySalaryRegisterobj != null)
                {
                    frmDailySalaryRegisterobj.Enabled = true;
                    frmDailySalaryRegisterobj.BringToFront();
                    frmDailySalaryRegisterobj.Clear();
                    frmDailySalaryRegisterobj.GridSelection();
                }
                if (objVoucherSearch != null)
                {
                    objVoucherSearch.Enabled = true;
                    objVoucherSearch.GridFill();

                }
                if (frmDayBookObj != null)
                {
                    frmDayBookObj.Enabled = true;
                    frmDayBookObj.dayBookGridFill();
                    frmDayBookObj = null;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV32:" + ex.Message;
            }
        }
        /// <summary>
        /// Decimal validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWage_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (dgvDailySalaryVoucher.CurrentCell != null)
                {
                    if (dgvDailySalaryVoucher.Columns[dgvDailySalaryVoucher.CurrentCell.ColumnIndex].Name == "txtWage")
                    {
                        Common.DecimalValidation(sender, e, false);

                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV33:" + ex.Message;
            }
        }
        /// <summary>
        /// Checking the DatagridView's Keypress event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDailySalaryVoucher_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                DataGridViewTextBoxEditingControl txtWage = e.Control as DataGridViewTextBoxEditingControl;
                if (txtWage != null)
                {
                    txtWage.KeyPress += txtWage_KeyPress;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV34:" + ex.Message;
            }
        }
        /// <summary>
        /// Displaying the TotalAmount on txtSalaryDate textbox textchanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSalaryDate_TextChanged(object sender, EventArgs e)
        {
            try
            {

                lblShowTotelamount.Text = Math.Round(Convert.ToDecimal("0.00000"), PublicVariables._inNoOfDecimalPlaces).ToString(); //"0.00000";
                txtNarration.Text = string.Empty;

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV35:" + ex.Message;
            }
        }
        /// <summary>
        /// Date Validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation obj = new DateValidation();
                bool isValidate = obj.DateValidationFunction(txtDate);

                if (!isValidate)
                {
                    txtDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                //---for change date in Date time picker----//

                dtpDate.Value = Convert.ToDateTime(txtDate.Text);

                if (!isEditmode)//Re fill
                {
                    VoucherNumberGeneration();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV36:" + ex.Message;
            }
        }

        #endregion

        #region NAVIGATION

        /// <summary>
        /// Enter key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_Enter(object sender, EventArgs e)
        {
            try
            {
                inNarrationCount = 0;
                txtNarration.Text = txtNarration.Text.Trim();
                if (txtNarration.Text == string.Empty)
                {
                    txtNarration.SelectionStart = 0;
                    txtNarration.SelectionLength = 0;
                    txtNarration.Focus();
                }
                else
                {
                    txtNarration.SelectionStart = txtNarration.Text.Length;
                    txtNarration.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV37:" + ex.Message;
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
                formMDI.infoError.ErrorString = "DSV38:" + ex.Message;
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
                    if (txtNarration.Text == string.Empty || txtNarration.SelectionStart == 0)
                    {
                        if (dgvDailySalaryVoucher.RowCount > 0)
                        {
                            dgvDailySalaryVoucher.Focus();
                            dgvDailySalaryVoucher.Rows[dgvDailySalaryVoucher.RowCount - 1].Cells["cmbStatus"].Selected = true;
                        }
                        else
                        {
                            cmbCashorBankAccount.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV39:" + ex.Message;
            }
        }
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
                    txtDate.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV40:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (txtSalaryDate.Enabled != false)
                    {
                        txtSalaryDate.Focus();
                    }
                    else
                    {
                        cmbCashorBankAccount.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtDate.Text == string.Empty || txtDate.SelectionStart == 0)
                    {
                        txtVoucherNo.Focus();
                        txtVoucherNo.SelectionStart = 0;
                        txtVoucherNo.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV41:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSalaryDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbCashorBankAccount.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtSalaryDate.Text == string.Empty || txtSalaryDate.SelectionStart == 0)
                    {
                        txtDate.Focus();
                        txtDate.SelectionStart = 0;
                        txtDate.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV42:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and backspace navigation and quick access
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCashorBankAccount_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvDailySalaryVoucher.RowCount > 0)
                    {
                        dgvDailySalaryVoucher.Focus();
                        dgvDailySalaryVoucher.Rows[0].Cells["cmbStatus"].Selected = true;
                        dgvDailySalaryVoucher.CurrentCell = dgvDailySalaryVoucher.Rows[0].Cells["cmbStatus"];
                    }
                    else
                    {
                        txtNarration.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtSalaryDate.Enabled != false)
                    {
                        txtSalaryDate.Focus();
                        txtSalaryDate.SelectionStart = 0;
                        txtSalaryDate.SelectionLength = 0;
                    }
                    else
                    {
                        txtDate.Focus();
                        txtDate.SelectionStart = 0;
                        txtDate.SelectionLength = 0;
                    }

                }
                if (e.KeyCode == Keys.F && Control.ModifierKeys == Keys.Control) //Pop Up
                {
                    if (cmbCashorBankAccount.SelectedIndex != -1)
                    {
                        frmLedgerPopupObj = new frmLedgerPopup();
                        frmLedgerPopupObj.MdiParent = formMDI.MDIObj;
                        frmLedgerPopupObj.CallFromDailySalaryVoucher(this, Convert.ToDecimal(cmbCashorBankAccount.SelectedValue.ToString()), "CashOrBank");
                    }
                    else
                    {
                        Messages.InformationMessage("Select any cash or bank account ");
                    }
                }

                if (e.Alt && e.KeyCode == Keys.C)//To open Acc.Ledger Form
                {
                    SendKeys.Send("{F10}");
                    btnAccountLedgerAdd_Click(sender, e);
                }



            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV43:" + ex.Message;
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
                formMDI.infoError.ErrorString = "DSV44:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDailySalaryVoucher_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvcell == 1)
                    {
                        dgvcell = 0;
                        txtNarration.Focus();
                        dgvDailySalaryVoucher.ClearSelection();
                        e.Handled = true;
                    }
                    else
                    {
                        dgvcell++;
                    }

                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (dgvDailySalaryVoucher.Rows.Count > 0)
                    {
                        if (dgvDailySalaryVoucher.CurrentCell == dgvDailySalaryVoucher.Rows[0].Cells["cmbStatus"])
                        {
                            cmbCashorBankAccount.Focus();
                        }
                        else
                        {
                            if (dgvDailySalaryVoucher.CurrentCell.RowIndex > 0)
                            {
                                dgvDailySalaryVoucher.CurrentCell = dgvDailySalaryVoucher.Rows[dgvDailySalaryVoucher.CurrentCell.RowIndex - 1].Cells["cmbStatus"];
                            }
                            else
                            {
                                cmbCashorBankAccount.Focus();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DSV45:" + ex.Message;
            }
        }
        #endregion
    }
}


