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
    public partial class frmBarcodeSettings : Form
    {
        #region Functions
        /// <summary>
        /// Create an Instance for frmBarcodeSettings Class
        /// </summary>
        public frmBarcodeSettings()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Clear Grop box Items
        /// </summary>
        public void ClearGroupBox()
        {
            try
            {
                txtZero.Clear();
                txtOne.Clear();
                txtTwo.Clear();
                txtThree.Clear();
                txtFour.Clear();
                txtFive.Clear();
                txtSix.Clear();
                txtSeven.Clear();
                txtEight.Clear();
                txtNine.Clear();
                txtPoint.Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS1:" + ex.Message;
              
            }
        }
        /// <summary>
        /// To Clear the form here
        /// </summary>
        public void Clear()
        {
            try
            {
                cbxShowMrp.Checked = false;
                cbxShowCompanyNAmeAs.Checked = false;
                txtShowCompanyName.Clear();
                txtShowCompanyName.Clear();
                ClearGroupBox();
                cbxShowPurchaseRate.Checked = false;
                cbxShowMrp.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS2:" + ex.Message;
            }
        }
        /// <summary>
        /// To Display the company name in text box
        /// </summary>
        public void txtcompanynamefill()
        {
            try
            {
                CompanySP spcompany = new CompanySP();
                CompanyInfo infoCompany = new CompanyInfo();
                infoCompany = spcompany.CompanyView(1);
                string strCompanyName = infoCompany.CompanyName;
                txtShowCompanyName.Text = strCompanyName;
                txtShowCompanyName.ReadOnly = true;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS3:" + ex.Message;
            }
        }
        /// <summary>
        /// Setting the check box Status
        /// </summary>
        public void FillSettings()
        {
            try
            {
                BarcodeSettingsInfo Info = new BarcodeSettingsSP().BarcodeSettingsView(1);
                if (Info.ShowMRP == true)
                {
                    cbxShowMrp.Checked = true;
                }
                else
                {
                    cbxShowMrp.Checked = false;
                }
                if (Info.ShowProductCode == true)
                {
                    rbtnShowProductCode.Checked = true;
                }
                else
                {
                    rbtnShowProductName.Checked = true;
                }
                if (Info.ShowCompanyName == true)
                {
                    cbxShowCompanyNAmeAs.Checked = true;
                }
                else
                {
                    cbxShowCompanyNAmeAs.Checked = false;
                }
                if (Info.ShowPurchaseRate == true)
                {
                    cbxShowPurchaseRate.Checked = true;
                }
                else
                {
                    cbxShowPurchaseRate.Checked = false;
                }
                txtShowCompanyName.Text = Info.CompanyName;
                if (txtShowCompanyName.Text == string.Empty)
                {
                    CompanyInfo InfoCompany = new CompanyInfo();
                    CompanySP Sp = new CompanySP();
                    InfoCompany = Sp.CompanyView(1);
                    txtShowCompanyName.Text = InfoCompany.CompanyName;
                }
                txtZero.Text = Info.Zero;
                txtOne.Text = Info.One;
                txtTwo.Text = Info.Two;
                txtThree.Text = Info.Three;
                txtFour.Text = Info.Four;
                txtFive.Text = Info.Five;
                txtSix.Text = Info.Six;
                txtSeven.Text = Info.Seven;
                txtEight.Text = Info.Eight;
                txtNine.Text = Info.Nine;
                txtPoint.Text = Info.Point;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS4:" + ex.Message;
            }
        }
        /// <summary>
        /// Save Or Edit Function
        /// </summary>
        public void SaveOrEdit()
        {
            try
            {
                bool isOk = true;
                if (cbxShowCompanyNAmeAs.Checked && txtShowCompanyName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Enter company code", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtShowCompanyName.Focus();
                    isOk = false;
                }
                else if ((cbxShowPurchaseRate.Checked && CheckGroupBox()) || (!cbxShowPurchaseRate.Checked))
                {
                    if (PublicVariables.isMessageAdd)
                    {
                        if (Messages.SaveMessage())
                        {
                            isOk = true;
                        }
                        else
                        {
                            isOk = false;
                        }
                    }
                    if (isOk)
                    {
                        BarcodeSettingsInfo InfoSettings = new BarcodeSettingsInfo();
                        InfoSettings.ShowMRP = cbxShowMrp.Checked;
                        InfoSettings.ShowProductCode = rbtnShowProductCode.Checked;
                        InfoSettings.ShowCompanyName = cbxShowCompanyNAmeAs.Checked;
                        InfoSettings.ShowPurchaseRate = cbxShowPurchaseRate.Checked;
                        InfoSettings.CompanyName = txtShowCompanyName.Text.Trim();
                        InfoSettings.Zero = txtZero.Text.Trim();
                        InfoSettings.One = txtOne.Text.Trim();
                        InfoSettings.Two = txtTwo.Text.Trim();
                        InfoSettings.Three = txtThree.Text.Trim();
                        InfoSettings.Four = txtFour.Text.Trim();
                        InfoSettings.Five = txtFive.Text.Trim();
                        InfoSettings.Six = txtSix.Text.Trim();
                        InfoSettings.Seven = txtSeven.Text.Trim();
                        InfoSettings.Eight = txtEight.Text.Trim();
                        InfoSettings.Nine = txtNine.Text.Trim();
                        InfoSettings.Point = txtPoint.Text.Trim();
                        InfoSettings.Extra1 = string.Empty;
                        InfoSettings.Extra2 = string.Empty;
                        BarcodeSettingsSP spbarcodesetting = new BarcodeSettingsSP();
                        spbarcodesetting.BarcodeSettingsAdd(InfoSettings);
                        Messages.SavedMessage();
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS5:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to check the Barcode settings
        /// </summary>
        public void BarcodeSettingsViewAll()
        {
            try
            {
                BarcodeSettingsSP spbarcodesettings = new BarcodeSettingsSP();
                DataTable dtbl = new DataTable();
                dtbl = spbarcodesettings.BarcodeSettingsViewAll();
                foreach (DataRow dr in dtbl.Rows)
                {
                    if (dr["showMRP"].ToString() == "True")
                    {
                        cbxShowMrp.Checked = true;
                    }
                    else
                    {
                        cbxShowMrp.Checked = false;
                    }
                    if (dr["showPurchaseRate"].ToString() == "True")
                    {
                        cbxShowPurchaseRate.Checked = true;
                    }
                    else
                    {
                        cbxShowPurchaseRate.Checked = false;
                    }
                    if (dr["showCompanyName"].ToString() == "True")
                    {
                        cbxShowCompanyNAmeAs.Checked = true;
                    }
                    else
                    {
                        cbxShowCompanyNAmeAs.Checked = false;
                    }
                    if (dr["showProductCode"].ToString() == "True")
                    {
                        rbtnShowProductCode.Checked = true;
                    }
                    else
                    {
                        rbtnShowProductName.Checked = true;
                    }
                    txtShowCompanyName.Text = dr["companyName"].ToString();
                    if (cbxShowPurchaseRate.Checked == true)
                    {
                        txtEight.Text = dr["eight"].ToString();
                        txtFive.Text = dr["five"].ToString();
                        txtFour.Text = dr["four"].ToString();
                        txtNine.Text = dr["nine"].ToString();
                        txtOne.Text = dr["one"].ToString();
                        txtPoint.Text = dr["point"].ToString();
                        txtSeven.Text = dr["seven"].ToString();
                        txtSix.Text = dr["six"].ToString();
                        txtThree.Text = dr["three"].ToString();
                        txtTwo.Text = dr["two"].ToString();
                        txtZero.Text = dr["zero"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS6:" + ex.Message;
            }
        }
        /// <summary>
        /// Checking the TextBox value here
        /// </summary>
        /// <returns></returns>
        public bool CheckGroupBox()
        {
            bool isOk = true;
            try
            {
                if (txtZero.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Enter code for zero", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtZero.Focus();
                    isOk = false;
                }
                else if (txtOne.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Enter code for one", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtOne.Focus();
                    isOk = false;
                }
                else if (txtTwo.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Enter code for two", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtTwo.Focus();
                    isOk = false;
                }
                else if (txtThree.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Enter code for three", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtThree.Focus();
                    isOk = false;
                }
                else if (txtFour.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Enter code for four", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtFour.Focus();
                    isOk = false;
                }
                else if (txtFive.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Enter code for five", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtFive.Focus();
                    isOk = false;
                }
                else if (txtSix.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Enter code for six", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtSix.Focus();
                    isOk = false;
                }
                else if (txtSeven.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Enter code for seven", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtSeven.Focus();
                    isOk = false;
                }
                else if (txtEight.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Enter code for eight", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtEight.Focus();
                    isOk = false;
                }
                else if (txtNine.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Enter code for nine", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtNine.Focus();
                    isOk = false;
                }
                else if (txtPoint.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Enter code for point", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPoint.Focus();
                    isOk = false;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS7:" + ex.Message;
            }
            return isOk;
        }
        /// <summary>
        /// Keypress Event for 
        /// </summary>
        /// <param name="e"></param>
        public void KeyPressFunction(KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 40 || e.KeyChar == 41)
                {
                    e.Handled = true;
                }
                else
                {
                    e.Handled = false;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS8:" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// Clear Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                ClearGroupBox();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS9:" + ex.Message;
            }
        }
        /// <summary>
        /// Save button Click
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
                formMDI.infoError.ErrorString = "BS10:" + ex.Message;
            }
        }
        /// <summary>
        /// Reset Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS11:" + ex.Message;
            }
        }
        /// <summary>
        /// Close button Click
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
                formMDI.infoError.ErrorString = "BS12:" + ex.Message;
            }
        }
        
       /// <summary>
       /// When Load The form , Call the clear function and check settings
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void frmBarcodeSettings_Load(object sender, EventArgs e)
        {
            try
            {
                rbtnShowProductCode.Checked = true;
                gbxRate.Enabled = false;
                Clear();
                BarcodeSettingsViewAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS13:" + ex.Message;
            }
        }
        /// <summary>
        /// Checkbox Company name checked changed to fill company name 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxShowCompanyNAmeAs_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxShowCompanyNAmeAs.Checked)
                {
                    txtcompanynamefill();
                }
                else
                {
                    txtShowCompanyName.ReadOnly = false;
                    txtShowCompanyName.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS14:" + ex.Message;
            }
        }
        /// <summary>
        /// checkbox ShowPurchaseRate CheckedChanged for Group box rate make enable or desable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxShowPurchaseRate_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxShowPurchaseRate.Checked)
                {
                    gbxRate.Enabled = true;
                }
                else
                {
                    gbxRate.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS15:" + ex.Message;
            }
        }
        /// <summary>
        /// Call KeyPressFunction to check the Input Values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtZero_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                KeyPressFunction(e);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS16:" + ex.Message;
            }
        }
        /// <summary>
        /// Call KeyPressFunction to check the Input Values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOne_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                KeyPressFunction(e);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS17:" + ex.Message;
            }
        }
        /// <summary>
        /// Call KeyPressFunction to check the Input Values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTwo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                KeyPressFunction(e);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS18:" + ex.Message;
            }
        }
        /// <summary>
        /// Keypress Event for Blocking Opening and Closing Brackets
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtThree_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                KeyPressFunction(e);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS19:" + ex.Message;
            }
        }
        /// <summary>
        /// Call KeyPressFunction to check the Input Values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFour_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                KeyPressFunction(e);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS20:" + ex.Message;
            }
        }
        /// <summary>
        /// Call KeyPressFunction to check the Input Values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFive_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                KeyPressFunction(e);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS21:" + ex.Message;
            }
        }
        /// <summary>
        /// Call KeyPressFunction to check the Input Values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSix_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                KeyPressFunction(e);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS22:" + ex.Message;
            }
        }
        /// <summary>
        /// Call KeyPressFunction to check the Input Values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSeven_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                KeyPressFunction(e);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS23:" + ex.Message;
            }
        }
        /// <summary>
        /// Call KeyPressFunction to check the Input Values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtEight_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                KeyPressFunction(e);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS24:" + ex.Message;
            }
        }
        /// <summary>
        /// Call KeyPressFunction to check the Input Values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNine_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                KeyPressFunction(e);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS25:" + ex.Message;
            }
        }
        /// <summary>
        /// Call KeyPressFunction to check the Input Values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPoint_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                KeyPressFunction(e);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS26:" + ex.Message;
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// Form Key down for Quick Access
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmBarcodeSettings_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    btnClose_Click(sender, e);
                }
                if (e.Control && e.KeyCode == Keys.S)
                {
                    SaveOrEdit();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS27:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxShowMrp_KeyDown(object sender, KeyEventArgs e)
        {
            {
                try
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        rbtnShowProductName.Focus();
                    }
                }
                catch (Exception ex)
                {
                    formMDI.infoError.ErrorString = "BS28:" + ex.Message;
                }
            }
        }
        /// <summary>
        /// For Enter key and backspace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnShowProductName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    rbtnShowProductCode.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    cbxShowMrp.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS29:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnShowProductCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cbxShowCompanyNAmeAs.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    rbtnShowProductName.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS30:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxShowCompanyNAmeAs_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (!txtShowCompanyName.ReadOnly)
                    {
                        txtShowCompanyName.Focus();
                        txtShowCompanyName.SelectionStart = 0;
                        txtShowCompanyName.SelectionLength = 0;
                    }
                    else
                    {
                        cbxShowPurchaseRate.Focus();
                    }
                }
                else if (e.KeyCode == Keys.Back)
                {
                    rbtnShowProductName.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS31:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtShowCompanyName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cbxShowPurchaseRate.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtShowCompanyName.Text.Trim() == string.Empty || txtShowCompanyName.SelectionStart == 0)
                    {
                        cbxShowCompanyNAmeAs.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS32:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxShowPurchaseRate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (!cbxShowPurchaseRate.Checked)
                    {
                        btnSave.Focus();
                    }
                    else
                    {
                        txtZero.Focus();
                    }
                }
                else if (e.KeyCode == Keys.Back)
                {
                    cbxShowCompanyNAmeAs.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS33:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtZero_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtOne.Focus();
                    txtOne.SelectionStart = 0;
                    txtOne.SelectionLength = 0;
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtZero.Text.Trim() == string.Empty || txtZero.SelectionStart == 0)
                    {
                        cbxShowPurchaseRate.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS34:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOne_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtTwo.Focus();
                    txtTwo.SelectionStart = 0;
                    txtTwo.SelectionLength = 0;
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtOne.Text.Trim() == string.Empty || txtOne.SelectionStart == 0)
                    {
                        txtZero.Focus();
                        txtZero.SelectionStart = 0;
                        txtZero.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS35:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTwo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtThree.Focus();
                    txtThree.SelectionStart = 0;
                    txtThree.SelectionLength = 0;
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtTwo.Text.Trim() == string.Empty || txtTwo.SelectionStart == 0)
                    {
                        txtOne.Focus();
                        txtOne.SelectionStart = 0;
                        txtOne.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS36:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtThree_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtFour.Focus();
                    txtFour.SelectionStart = 0;
                    txtFour.SelectionLength = 0;
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtThree.Text.Trim() == string.Empty || txtThree.SelectionStart == 0)
                    {
                        txtTwo.Focus();
                        txtTwo.SelectionStart = 0;
                        txtTwo.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS37:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFour_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtFive.Focus();
                    txtFive.SelectionStart = 0;
                    txtFive.SelectionLength = 0;
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtFour.Text.Trim() == string.Empty || txtFour.SelectionStart == 0)
                    {
                        txtThree.Focus();
                        txtThree.SelectionStart = 0;
                        txtThree.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS38:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFive_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtSix.Focus();
                    txtSix.SelectionStart = 0;
                    txtSix.SelectionLength = 0;
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtFive.Text.Trim() == string.Empty || txtFive.SelectionStart == 0)
                    {
                        txtFour.Focus();
                        txtFour.SelectionStart = 0;
                        txtFour.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS39:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSix_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtSeven.Focus();
                    txtSeven.SelectionStart = 0;
                    txtSeven.SelectionLength = 0;
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtSix.Text.Trim() == string.Empty || txtSix.SelectionStart == 0)
                    {
                        txtFive.Focus();
                        txtFive.SelectionStart = 0;
                        txtFive.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS40:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSeven_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtEight.Focus();
                    txtEight.SelectionStart = 0;
                    txtEight.SelectionLength = 0;
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtSeven.Text.Trim() == string.Empty || txtSeven.SelectionStart == 0)
                    {
                        txtSix.Focus();
                        txtSix.SelectionStart = 0;
                        txtSix.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS41:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtEight_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtNine.Focus();
                    txtNine.SelectionStart = 0;
                    txtNine.SelectionLength = 0;
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtEight.Text.Trim() == string.Empty || txtEight.SelectionStart == 0)
                    {
                        txtSeven.Focus();
                        txtSeven.SelectionStart = 0;
                        txtSeven.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS42:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNine_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtPoint.Focus();
                    txtPoint.SelectionStart = 0;
                    txtPoint.SelectionLength = 0;
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtNine.Text.Trim() == string.Empty || txtNine.SelectionStart == 0)
                    {
                        txtEight.Focus();
                        txtEight.SelectionStart = 0;
                        txtEight.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS43:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPoint_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSave.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtPoint.Text.Trim() == string.Empty || txtPoint.SelectionStart == 0)
                    {
                        txtNine.Focus();
                        txtNine.SelectionStart = 0;
                        txtNine.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS44:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    txtPoint.Focus();
                    txtPoint.SelectionStart = 0;
                    txtPoint.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS45:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    btnClear.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS46:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    btnSave.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS47:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    btnReset.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS48:" + ex.Message;
            }
        }
        
        #endregion
    }
}
