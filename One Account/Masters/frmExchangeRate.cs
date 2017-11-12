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
    public partial class frmExchangeRate : Form
    {
        #region Public Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        decimal decExchangeRateId;
        int inNarrationCount = 0;
        decimal decId;
        frmCurrencyDetails frmCurrencyObj;
        string strCurrencyName;
        static int decimalCount = 0;
        #endregion
        #region Functions
        /// <summary>
        /// Creates an instance of frmExchangeRate class
        /// </summary>
        public frmExchangeRate()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to check exchangerate is greater than zero or not
        /// </summary>
        /// <returns></returns>
        public bool ExchangeRateCheck()
        {
            bool isOk = true;
            try
            {
                if (Convert.ToDecimal(txtExchangeRate.Text.Trim().ToString()) == 0)
                {
                    isOk = false;
                    Messages.InformationMessage("Exchange rate Should be greater than zero");
                    txtExchangeRate.Focus();
                }
            }
            catch (Exception ex)
            {
                isOk = false;
                formMDI.infoError.ErrorString = "ER1:" + ex.Message;
                
            }
            return isOk;
        }
        /// <summary>
        /// Function to save
        /// </summary>
        public void SaveFunction()
        {
            try
            {
                ExchangeRateInfo infoExchangeRate = new ExchangeRateInfo();
                ExchangeRateSP spExchangeRate = new ExchangeRateSP();
                infoExchangeRate.CurrencyId = Convert.ToDecimal(cmbCurrency.SelectedValue.ToString());
                infoExchangeRate.Date = Convert.ToDateTime(dtpDate.Text.Trim().ToString());
                infoExchangeRate.Rate = Convert.ToDecimal(txtExchangeRate.Text.Trim().ToString());
                infoExchangeRate.Narration = txtNarration.Text.Trim();
                infoExchangeRate.Extra1 = string.Empty;
                infoExchangeRate.Extra2 = string.Empty;
                if (spExchangeRate.ExchangeRateCheckExistence(Convert.ToDateTime(txtDate.Text.Trim().ToString()), Convert.ToDecimal(cmbCurrency.SelectedValue.ToString()), 0) == false)
                {
                    if (ExchangeRateCheck())
                    {
                        spExchangeRate.ExchangeRateAddParticularFields(infoExchangeRate);
                        Messages.SavedMessage();
                        Clear();
                    }
                }
                else
                {
                    Messages.ReferenceExistsMessageForUpdate();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to edit
        /// </summary>
        public void Editfunction()
        {
            try
            {
                ExchangeRateInfo infoExchangeRate = new ExchangeRateInfo();
                ExchangeRateSP spExchangeRate = new ExchangeRateSP();
                infoExchangeRate.CurrencyId = Convert.ToDecimal(cmbCurrency.SelectedValue.ToString());
                infoExchangeRate.Date = Convert.ToDateTime(dtpDate.Text.Trim().ToString());
                infoExchangeRate.Rate = Convert.ToDecimal(txtExchangeRate.Text.Trim().ToString());
                infoExchangeRate.Narration = txtNarration.Text.Trim();
                infoExchangeRate.Extra1 = String.Empty;
                infoExchangeRate.Extra2 = String.Empty;
                infoExchangeRate.ExchangeRateId = decId;
                if (spExchangeRate.ExchangeRateCheckExistence(Convert.ToDateTime(txtDate.Text.Trim().ToString()), Convert.ToDecimal(cmbCurrency.SelectedValue.ToString()), decExchangeRateId) == false)
                {
                    if (ExchangeRateCheck())
                    {
                        spExchangeRate.ExchangeRateEdit(infoExchangeRate);
                        Messages.UpdatedMessage();
                        SearchClear();
                        Clear();
                    }
                }
                else
                {
                    Messages.InformationMessage("Already exists");
                    cmbCurrency.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER3:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call save or edit
        /// </summary>
        public void SaveOrEdit()
        {
            try
            {
                if (cmbCurrency.SelectedIndex == -1)
                {
                    Messages.InformationMessage("Select currency");
                    cmbCurrency.Focus();
                }
                else if (txtExchangeRate.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Enter exchange rate");
                    txtExchangeRate.Focus();
                }
                else
                {
                    if (btnSave.Text == "Save")
                    {
                        if (PublicVariables.isMessageAdd)
                        {
                            if (Messages.SaveMessage())
                            {
                                SaveFunction();
                            }
                        }
                        else
                        {
                            SaveFunction();
                        }
                    }
                    else
                    {
                        if (PublicVariables.isMessageEdit)
                        {
                            if (Messages.UpdateMessage())
                            {
                                Editfunction();
                            }
                        }
                        else
                        {
                            Editfunction();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER4:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Currency combobox
        /// </summary>
        public void CurrencyComboFill()
        {
            try
            {
                DataTable dtblCurrency = new DataTable();
                CurrencySP spCurrency = new CurrencySP();
                dtblCurrency = spCurrency.CurrencyViewAllForExchangeRateCombo();
                cmbCurrency.DataSource = dtblCurrency;
                cmbCurrency.ValueMember = "currencyId";
                cmbCurrency.DisplayMember = "currencyName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER5:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Currency combobox
        /// </summary>
        public void CurrencyRateComboFill()
        {
            try
            {
                DataTable dtblCurrencyRate = new DataTable();
                CurrencySP spCurrency = new CurrencySP();
                dtblCurrencyRate = spCurrency.CurrencyViewAllForCombo();
                DataRow dr = dtblCurrencyRate.NewRow();
                dr[2] = "All";
                dtblCurrencyRate.Rows.InsertAt(dr, 0);
                cmbCurrencyRate.DataSource = dtblCurrencyRate;
                cmbCurrencyRate.ValueMember = "currencyId";
                cmbCurrencyRate.DisplayMember = "currencyName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER6:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to clear
        /// </summary>
        public void Clear()
        {
            try
            {
                txtExchangeRate.Clear();
                txtNarration.Clear();
                cmbCurrency.SelectedIndex = -1;
                dtpDate.Value = PublicVariables._dtCurrentDate;
                dtpDate.MinDate = PublicVariables._dtFromDate;
                dtpDate.MaxDate = PublicVariables._dtToDate;
                txtDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                btnDelete.Enabled = false;
                pnlExchange.Visible = false;
                btnSave.Text = "Save";
                cmbCurrency.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER7:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to clear
        /// </summary>
        public void SearchClear()
        {
            try
            {
                cmbCurrencyRate.SelectedIndex = 0;
                dtpDateTo.MinDate = PublicVariables._dtFromDate;
                dtpDateTo.MaxDate = PublicVariables._dtToDate;
                txtDateTo.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                dtpDateFrom.MinDate = PublicVariables._dtFromDate;
                dtpDateFrom.MaxDate = PublicVariables._dtToDate;
                txtDatefrom.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                cmbCurrencyRate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER8:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill datagridview
        /// </summary>
        public void GridFill()
        {
            try
            {
                ExchangeRateSP spExchangeRaterate = new ExchangeRateSP();
                DataTable dtbl = new DataTable();
                dtbl = spExchangeRaterate.ExchangeRateSearch(cmbCurrencyRate.Text.ToString(), Convert.ToDateTime(txtDatefrom.Text.ToString()), Convert.ToDateTime(txtDateTo.Text.ToString()));
                dgvExchangeRate.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER9:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to delete
        /// </summary>
        public void DeleteFunction()
        {
            try
            {
                ExchangeRateSP spExchangeRate = new ExchangeRateSP();
                if (spExchangeRate.ExchangeRateCheckReferences(decId) == -1)
                {
                    Messages.ReferenceExistsMessage();
                }
                else
                {
                    Messages.DeletedMessage();
                    Clear();
                    GridFill();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER10:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call delete
        /// </summary>
        public void Delete()
        {
            try
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
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER11:" + ex.Message;
            }
        }
        /// <summary>
        /// Function toi call this form from Currency Form
        /// </summary>
        /// <param name="frmCurrencyDetails"></param>
        /// <param name="decId"></param>
        public void CallFromCurrenCyDetails(frmCurrencyDetails frmCurrencyDetails, decimal decId) //PopUp
        {
            try
            {
                base.Show();
                this.frmCurrencyObj = frmCurrencyDetails;
                CurrencyComboFill();
                cmbCurrency.SelectedValue = decId;
                cmbCurrency.Focus();
                frmCurrencyObj.Close();
                frmCurrencyObj = null;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER12:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill controls for update
        /// </summary>
        public void FillControls()
        {
            try
            {
                ExchangeRateInfo infoExchangeRate = new ExchangeRateInfo();
                ExchangeRateSP spExchangeRate = new ExchangeRateSP();
                infoExchangeRate = spExchangeRate.ExchangeRateView(decId);
                int inNoOfDecimalPlaces = spExchangeRate.NoOfDecimalNumberViewByExchangeRateId(decId);
                cmbCurrency.SelectedValue = infoExchangeRate.CurrencyId.ToString();
                dtpDate.Text = infoExchangeRate.Date.ToString();
                txtExchangeRate.Text = Math.Round(Convert.ToDecimal(infoExchangeRate.Rate.ToString()), inNoOfDecimalPlaces).ToString();
                txtNarration.Text = infoExchangeRate.Narration;
                decExchangeRateId = infoExchangeRate.ExchangeRateId;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER13:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Currency combobox when return from Currency Form
        /// </summary>
        /// <param name="decId"></param>
        public void ReturnFromCurrencyForm(decimal decId)//Form Currency 
        {
            try
            {
                CurrencyRateComboFill();
                CurrencyComboFill();
                if (decId.ToString() != "0")
                {
                    cmbCurrency.SelectedValue = decId;
                }
                else if (strCurrencyName != string.Empty)
                {
                    cmbCurrency.SelectedValue = strCurrencyName;
                }
                else
                {
                    cmbCurrency.SelectedIndex = -1;
                }
                this.Enabled = true;
                cmbCurrency.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER14:" + ex.Message;
            }
        }
        public int GetDecimalCount(decimal val)
        {
            try
            {
                if (val == val * 10)
                {
                    return int.MaxValue; // no decimal.Epsilon I don't use this type enough to know why... this will work
                }
                while (val != Math.Floor(val))
                {
                    val = (val - Math.Floor(val)) * 10;
                    decimalCount++;
                }
               
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER15:" + ex.Message;
            }
            return decimalCount;
        }
        public int NoofDecimalPlacesFind()
        {
            int inNoOfDecimalPlaces = 0;
            try
            {
                ExchangeRateSP spExchangeRate = new ExchangeRateSP();
                if (cmbCurrency.SelectedValue != null)
                {
                    inNoOfDecimalPlaces = spExchangeRate.NoOfDecimalNumberViewByCurrencyId(Convert.ToDecimal(cmbCurrency.SelectedValue.ToString()));
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER16:" + ex.Message;
            }
            return inNoOfDecimalPlaces;
        }
        #endregion
        #region Events
        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmExchangeRate_Load(object sender, EventArgs e)
        {
            try
            {
                CurrencyComboFill();
                CurrencyRateComboFill();
                dtpDateTo.Value = PublicVariables._dtCurrentDate;
                dtpDateTo.MinDate = PublicVariables._dtFromDate;
                dtpDateTo.MaxDate = PublicVariables._dtToDate;
                txtDateTo.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                dtpDateFrom.Value = PublicVariables._dtCurrentDate;
                dtpDateFrom.MinDate = PublicVariables._dtFromDate;
                dtpDateFrom.MaxDate = PublicVariables._dtToDate;
                txtDatefrom.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                Clear();
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER17:" + ex.Message;
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
                    SaveOrEdit();
                    GridFill();
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER18:" + ex.Message;
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
                formMDI.infoError.ErrorString = "ER19:" + ex.Message;
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
                    {
                        Delete();
                    }
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER20:" + ex.Message;
            }
        }
        /// <summary>
        /// On Form close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFrmClose_Click(object sender, EventArgs e)
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
                formMDI.infoError.ErrorString = "ER21:" + ex.Message;
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
                formMDI.infoError.ErrorString = "ER22:" + ex.Message;
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
                formMDI.infoError.ErrorString = "ER23:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Clear' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchClear_Click(object sender, EventArgs e)
        {
            try
            {
                SearchClear();
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER24:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'ExchangeRate' textbox textchanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtExchangeRate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CurrencySP SpCurrency = new CurrencySP();
                String strSymbol = SpCurrency.GetDefaultCurrencySymbol();
                pnlExchange.Visible = true;
                lblSymbol.Text = "1" + cmbCurrency.Text;
                lblRate.Text = txtExchangeRate.Text + strSymbol/*"Rs"*/;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER25:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Narration' textbox key enter
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
                formMDI.infoError.ErrorString = "ER26:" + ex.Message;
            }
        }
        /// <summary>
        /// On datagridview cell double click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExchangeRate_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ExchangeRateSP spExcahangeRate = new ExchangeRateSP();
                if (e.RowIndex != -1)
                {
                  decId = Convert.ToDecimal(dgvExchangeRate.Rows[e.RowIndex].Cells["dgvtxtExchangeRateId"].Value.ToString());
                  bool Status=  spExcahangeRate.ExchangeRateCheckExistanceForUpdationAndDelete(Convert.ToDateTime(txtDate.Text.ToString()), decId);
                  if (Status)
                  {
                      FillControls();
                      btnSave.Text = "Update";
                      btnDelete.Enabled = true;
                      cmbCurrency.Focus();
                  }
                  else
                  {
                      Messages.ReferenceExistsMessageForUpdate();
                  }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER27:" + ex.Message;
            }
        }
        /// <summary>
        /// On Currency add button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCurrencyAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbCurrency.SelectedValue != null)
                {
                    strCurrencyName = cmbCurrency.SelectedValue.ToString();
                }
                else
                {
                    strCurrencyName = string.Empty;
                }
                frmCurrency frmCurrency = new frmCurrency();
                frmCurrency.MdiParent = formMDI.MDIObj;
                frmCurrency open = Application.OpenForms["frmCurrency"] as frmCurrency;
                if (open == null)
                {
                    frmCurrency.WindowState = FormWindowState.Normal;
                    frmCurrency.MdiParent = formMDI.MDIObj;
                    frmCurrency.CallFromExchangerate(this);
                }
                else
                {
                    open.MdiParent = formMDI.MDIObj;
                    open.CallFromExchangerate(this);
                    open.BringToFront();
                    if (open.WindowState == FormWindowState.Minimized)
                    {
                        open.WindowState = FormWindowState.Normal;
                    }
                }
                this.Enabled = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER28:" + ex.Message;
            }
        }
        /// <summary>
        /// Clear selection on Datagridview binding complete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExchangeRate_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                dgvExchangeRate.ClearSelection();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER29:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Date' datetimepicker valuechanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.txtDate.Text = this.dtpDate.Value.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER30:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Date' textbox key leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation objDateValidation = new DateValidation();
                objDateValidation.DateValidationFunction(txtDate);
                if (txtDate.Text == string.Empty)
                {
                    txtDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER31:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Date To' textbox key leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDateTo_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation objDateValidation = new DateValidation();
                objDateValidation.DateValidationFunction(txtDateTo);
                if (txtDateTo.Text == string.Empty)
                {
                    txtDateTo.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER32:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Date To' datetimepicker valuechanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpDateTo_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.txtDateTo.Text = this.dtpDateTo.Value.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER33:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Date From' textbox key leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDatefrom_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation objDateValidation = new DateValidation();
                objDateValidation.DateValidationFunction(txtDatefrom);
                if (txtDatefrom.Text == string.Empty)
                {
                    txtDatefrom.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER34:" + ex.Message;
            }
        }
        /// <summary>
        ///  On 'Date From' datetimepicker valuechanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpDateFrom_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.txtDatefrom.Text = this.dtpDateFrom.Value.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER35:" + ex.Message;
            }
        }
        /// <summary>
        ///  On 'ExchangeRate' textbox keypress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtExchangeRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Common.DecimalValidation(sender, e, false);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER36:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Date' textbox textchanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtDate.Text.Trim() == string.Empty)
                {
                    txtDate_Leave(sender, e);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER37:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Date To' textbox textchanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDateTo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtDateTo.Text.Trim() == string.Empty)
                {
                    txtDateTo_Leave(sender, e);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER38:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Date From' textbox textchanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDatefrom_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtDatefrom.Text.Trim() == string.Empty)
                {
                    txtDatefrom_Leave(sender, e);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER39:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Currency' combobox selected valuechanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCurrency_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                CurrencySP SpCurrency = new CurrencySP();
                String strSymbol = SpCurrency.GetDefaultCurrencySymbol();
                pnlExchange.Visible = true;
                lblSymbol.Text = "1" + cmbCurrency.Text;
                lblRate.Text = txtExchangeRate.Text + strSymbol;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER40:" + ex.Message;
                
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCurrency_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtDate.Focus();
                }
                if (e.KeyCode == Keys.F && Control.ModifierKeys == Keys.Control) //POP UP
                {
                    frmCurrencyObj = new frmCurrencyDetails();
                    frmCurrencyObj.MdiParent = formMDI.MDIObj;
                    if (cmbCurrency.SelectedIndex >= 0)
                    {
                        frmCurrencyObj.CallFromExchangerate(this, Convert.ToDecimal(cmbCurrency.SelectedValue.ToString()));
                    }
                    else
                    {
                        Messages.InformationMessage("Select Currency");
                        cmbCurrency.Focus();
                    }
                }
                if (e.KeyCode == Keys.C && Control.ModifierKeys == Keys.Alt)
                {
                    SendKeys.Send("{F10}");
                    btnCurrencyAdd_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER41:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'ExchangeRate' textbox KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtExchangeRate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtNarration.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtExchangeRate.Text == string.Empty || txtExchangeRate.SelectionStart == 0)
                    {
                        txtDate.Focus();
                        txtDate.SelectionStart = 0;
                        txtDate.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER42:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Narration' textbox KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
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
                if (e.KeyCode == Keys.Back)
                {
                    if (txtNarration.Text == string.Empty || txtNarration.SelectionStart == 0)
                    {
                        txtExchangeRate.Focus();
                        txtExchangeRate.SelectionStart = 0;
                        txtExchangeRate.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER43:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Save' button KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "ER44:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Date' textbox KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtExchangeRate.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtDate.Text == string.Empty || txtDate.SelectionStart == 0)
                    {
                        cmbCurrency.Focus();
                        cmbCurrency.SelectionStart = 0;
                        cmbCurrency.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER45:" + ex.Message;
            }
        }
        /// <summary>
        /// On datagridview keyUp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExchangeRate_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                {
                    if (dgvExchangeRate.CurrentRow != null)
                    {
                        DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(dgvExchangeRate.CurrentCell.ColumnIndex, dgvExchangeRate.CurrentCell.RowIndex);
                        dgvExchangeRate_CellDoubleClick(sender, ex);
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtDatefrom.Focus();
                    txtDatefrom.SelectionLength = 0;
                    txtDatefrom.SelectionStart = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER46:" + ex.Message;
            }
        }
        /// <summary>
        /// On Currency' combobox key down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCurrencyRate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtDateTo.Focus();
                    txtDateTo.SelectionLength = 0;
                    txtDateTo.SelectionStart = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER47:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Date To' textbox KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDateTo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtDatefrom.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtDateTo.Text == string.Empty || txtDateTo.SelectionStart == 0)
                    {
                        cmbCurrencyRate.Focus();
                        cmbCurrencyRate.SelectionStart = 0;
                        cmbCurrencyRate.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER48:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Date From' textbox KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDatefrom_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtDatefrom.Text == string.Empty || txtDatefrom.SelectionStart == 0)
                    {
                        txtDateTo.Focus();
                        txtDateTo.SelectionStart = 0;
                        txtDateTo.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER49:" + ex.Message;
            }
        }
        /// <summary>
        /// On form KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmExchangeRate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    btnClose_Click(sender, e);
                }
                if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control) //Save
                {
                    if (cmbCurrency.Focused)
                    {
                        cmbCurrency.DropDownStyle = ComboBoxStyle.DropDown;
                    }
                    else
                    {
                        cmbCurrency.DropDownStyle = ComboBoxStyle.DropDownList;
                    }
                    btnSave_Click(sender, e);
                }
                if (e.KeyCode == Keys.D && Control.ModifierKeys == Keys.Control) //Delete
                {
                    if (btnDelete.Enabled)
                    {
                        btnDelete_Click(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER50:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Clear'  button KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchClear_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "ER51:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Search' button KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    txtDatefrom.Focus();
                    txtDatefrom.SelectionLength = 0;
                    txtDatefrom.SelectionStart = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ER52:" + ex.Message;
            }
        }
        #endregion
    }
}
