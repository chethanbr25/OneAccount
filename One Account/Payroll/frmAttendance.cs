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
    public partial class frmAttendance : Form
    {

        #region Public Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        decimal decMasterIdForEdit = 0; // To keep the master id for edit and delete
        int inNarrationCount = 0;
        int inGridRowCount = 0;

        #endregion

        #region Functions
        /// <summary>
        /// Creates an instance of frmAtendance class
        /// </summary>
        public frmAttendance()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to fill Datagridview
        /// </summary>
        public void GridFill()
        {
            try
            {
                DailyAttendanceDetailsInfo infoDailyAttendanceDetails = new DailyAttendanceDetailsInfo();
                DailyAttendanceDetailsSP spDailyAttendanceDetails = new DailyAttendanceDetailsSP();
                DailyAttendanceMasterInfo infoDailyAttendanceMaster = new DailyAttendanceMasterInfo();
                DailyAttendanceMasterSP spDailyAttendanceMaster = new DailyAttendanceMasterSP();
                string strDate = txtCompanyCurrentdate.Text;
                if (spDailyAttendanceMaster.DailyAttendanceMasterMasterIdSearch(strDate))
                {
                    DataTable dtblAttendance = new DataTable();
                    infoDailyAttendanceMaster.Date = Convert.ToDateTime(txtCompanyCurrentdate.Text.ToString());
                    dtblAttendance = spDailyAttendanceDetails.DailyAttendanceDetailsSearchGridFill(txtCompanyCurrentdate.Text.ToString());
                    dgvAttendance.DataSource = dtblAttendance;
                    btnDelete.Enabled = true;
                    btnSave.Text = "Update";
                    int inRowCount = dgvAttendance.RowCount;
                    string strMasterNarration = string.Empty;
                    for (int i = 0; i < inRowCount; i++)
                    {
                        if (dgvAttendance.Rows[i].Cells["MasterNarration"].Value.ToString() != "")
                        {
                            strMasterNarration = dgvAttendance.Rows[i].Cells["MasterNarration"].Value.ToString();
                        }
                    }
                    txtNarrationInMaster.Text = strMasterNarration;
                }
                else
                {
                    DataTable dtblAttendance = new DataTable();
                    dtblAttendance = spDailyAttendanceDetails.DailyAttendanceDetailsSearchGridFill(txtCompanyCurrentdate.Text.ToString());
                    dgvAttendance.DataSource = dtblAttendance;
                    btnDelete.Enabled = false;
                    btnSave.Text = "Save";
                    int inRowCount = dgvAttendance.RowCount;
                    string strMasterNarration = string.Empty;
                    for (int i = 0; i < inRowCount; i++)
                    {
                        if (dgvAttendance.Rows[i].Cells["MasterNarration"].Value.ToString() != null)
                        {
                            strMasterNarration = dgvAttendance.Rows[i].Cells["MasterNarration"].Value.ToString();
                        }
                    }
                    txtNarrationInMaster.Text = strMasterNarration;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "A1:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to save
        /// </summary>
        public void SaveFunction()
        {
            try
            {


                decimal decResult = HolidaySettings();
                if (decResult != 1)
                {
                    DailyAttendanceDetailsInfo infoDailyAttendanceDetails = new DailyAttendanceDetailsInfo();
                    DailyAttendanceDetailsSP spDailyAttendanceDetails = new DailyAttendanceDetailsSP();
                    DailyAttendanceMasterInfo infoDailyAttendanceMaster = new DailyAttendanceMasterInfo();
                    DailyAttendanceMasterSP spDailyAttendanceMaster = new DailyAttendanceMasterSP();
                    infoDailyAttendanceMaster.Date = Convert.ToDateTime(txtCompanyCurrentdate.Text.ToString());
                    infoDailyAttendanceMaster.Narration = txtNarrationInMaster.Text.Trim();
                    infoDailyAttendanceMaster.Extra1 = string.Empty;
                    infoDailyAttendanceMaster.Extra2 = string.Empty;
                    int inrowcount = dgvAttendance.RowCount;
                    decMasterIdForEdit = spDailyAttendanceMaster.DailyAttendanceAddToMaster(infoDailyAttendanceMaster);  // calling @@identity
                    infoDailyAttendanceDetails.DailyAttendanceMasterId = decMasterIdForEdit;
                    for (int i = 0; i <= inrowcount - 1; i++)
                    {
                        if (dgvAttendance.Rows[i].Cells["dgvtxtColumnEmployeeId"].Value != null && dgvAttendance.Rows[i].Cells["dgvtxtColumnEmployeeId"].Value.ToString() != "")
                        {
                            infoDailyAttendanceDetails.EmployeeId = Convert.ToDecimal(dgvAttendance.Rows[i].Cells["dgvtxtColumnEmployeeId"].Value.ToString());
                        }
                        if (dgvAttendance.Rows[i].Cells["dgvcmbcolumnStatus"].Value != null
                            && dgvAttendance.Rows[i].Cells["dgvcmbcolumnStatus"].Value.ToString() != "")
                        {
                            infoDailyAttendanceDetails.Status = dgvAttendance.Rows[i].Cells["dgvcmbcolumnStatus"].Value.ToString();
                        }
                        else
                        {
                            infoDailyAttendanceDetails.Status = "Present";
                        }
                        if (dgvAttendance.Rows[i].Cells["dgvtxtColumnnarration"].Value != null &&
                       dgvAttendance.Rows[i].Cells["dgvtxtColumnnarration"].Value.ToString() != "")
                        {
                            infoDailyAttendanceDetails.Narration = dgvAttendance.Rows[i].Cells["dgvtxtColumnnarration"].Value.ToString();
                        }
                        else
                        {
                            infoDailyAttendanceDetails.Narration = "";
                        }
                        infoDailyAttendanceDetails.Extra1 = string.Empty;
                        infoDailyAttendanceDetails.Extra2 = string.Empty;
                        infoDailyAttendanceDetails.DailyAttendanceMasterId = decMasterIdForEdit;
                        spDailyAttendanceDetails.DailyAttendanceDetailsAddUsingMasterId(infoDailyAttendanceDetails);
                    }
                    Messages.SavedMessage();
                    Clear();

                }
                else
                {
                    Messages.InformationMessage("Selected date is holiday");

                }
            }

            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "A2:" + ex.Message;
            }
        }
        //Function to edit
        public void EditFunction()
        {
            try
            {

                decimal decResult = HolidaySettings();
                if (decResult != 1)
                {
                    DailyAttendanceDetailsInfo infoDailyAttendanceDetails = new DailyAttendanceDetailsInfo();
                    DailyAttendanceDetailsSP spDailyAttendanceDetails = new DailyAttendanceDetailsSP();
                    DailyAttendanceMasterInfo infoDailyAttendanceMaster = new DailyAttendanceMasterInfo();
                    DailyAttendanceMasterSP spDailyAttendanceMaster = new DailyAttendanceMasterSP();
                    infoDailyAttendanceMaster.Date = DateTime.Parse(txtCompanyCurrentdate.Text.ToString());
                    infoDailyAttendanceMaster.Narration = txtNarrationInMaster.Text.Trim();
                    infoDailyAttendanceMaster.Extra1 = string.Empty;
                    infoDailyAttendanceMaster.Extra2 = string.Empty;
                    int inrowcount = dgvAttendance.RowCount;
                    for (int i = 0; i <= inrowcount - 1; i++)
                    {
                        if (dgvAttendance.Rows[i].Cells["dgvtxtdailyAttendanceMasterId"].Value.ToString() != "")
                        {
                            decMasterIdForEdit = Convert.ToDecimal(dgvAttendance.Rows[i].Cells["dgvtxtdailyAttendanceMasterId"].Value.ToString());   //storing Dailymasterid
                        }
                    }
                    infoDailyAttendanceMaster.DailyAttendanceMasterId = decMasterIdForEdit;
                    spDailyAttendanceMaster.DailyAttendanceEditMaster(infoDailyAttendanceMaster);
                    infoDailyAttendanceDetails.DailyAttendanceMasterId = decMasterIdForEdit;
                    for (int i = 0; i <= inrowcount - 1; i++)
                    {
                        if (dgvAttendance.Rows[i].Cells["dgvtxtDailyAttendanceDetailsId"].Value.ToString() != "")
                        {
                            // for updation of saved employees
                            if (dgvAttendance.Rows[i].Cells["dgvtxtColumnEmployeeId"].Value != null && dgvAttendance.Rows[i].Cells["dgvtxtColumnEmployeeId"].Value.ToString() != "")
                            {
                                infoDailyAttendanceDetails.EmployeeId = Convert.ToDecimal(dgvAttendance.Rows[i].Cells["dgvtxtColumnEmployeeId"].Value.ToString());
                            }
                            if (dgvAttendance.Rows[i].Cells["dgvtxtDailyAttendanceDetailsId"].Value != null && dgvAttendance.Rows[i].Cells["dgvtxtDailyAttendanceDetailsId"].Value.ToString() != "")
                            {
                                infoDailyAttendanceDetails.DailyAttendanceDetailsId = Convert.ToDecimal(dgvAttendance.Rows[i].Cells["dgvtxtDailyAttendanceDetailsId"].Value.ToString());
                            }
                            if (dgvAttendance.Rows[i].Cells["dgvtxtdailyAttendanceMasterId"].Value != null && dgvAttendance.Rows[i].Cells["dgvtxtdailyAttendanceMasterId"].Value.ToString() != "")
                            {
                                infoDailyAttendanceDetails.DailyAttendanceMasterId = Convert.ToDecimal(dgvAttendance.Rows[i].Cells["dgvtxtdailyAttendanceMasterId"].Value.ToString());
                            }
                            if (dgvAttendance.Rows[i].Cells["dgvcmbcolumnStatus"].Value != null
                           && dgvAttendance.Rows[i].Cells["dgvcmbcolumnStatus"].Value.ToString() != "")
                            {
                                infoDailyAttendanceDetails.Status = dgvAttendance.Rows[i].Cells["dgvcmbcolumnStatus"].Value.ToString();
                            }
                            else
                            {
                                infoDailyAttendanceDetails.Status = "Present";
                            }
                            if (dgvAttendance.Rows[i].Cells["dgvtxtColumnnarration"].Value != null &&
                            dgvAttendance.Rows[i].Cells["dgvtxtColumnnarration"].Value.ToString() != "")
                            {
                                infoDailyAttendanceDetails.Narration = dgvAttendance.Rows[i].Cells["dgvtxtColumnnarration"].Value.ToString();
                            }
                            else
                            {
                                infoDailyAttendanceDetails.Narration = "";
                            }
                            infoDailyAttendanceDetails.Extra1 = string.Empty;
                            infoDailyAttendanceDetails.Extra2 = string.Empty;
                            spDailyAttendanceDetails.DailyAttendanceDetailsEditUsingMasterId(infoDailyAttendanceDetails);
                        }
                        else
                        {
                            // for new employees to add
                            if (dgvAttendance.Rows[i].Cells["dgvtxtColumnEmployeeId"].Value != null && dgvAttendance.Rows[i].Cells["dgvtxtColumnEmployeeId"].Value.ToString() != "")
                            {
                                infoDailyAttendanceDetails.EmployeeId = Convert.ToDecimal(dgvAttendance.Rows[i].Cells["dgvtxtColumnEmployeeId"].Value.ToString());
                            }
                            if (dgvAttendance.Rows[i].Cells["dgvcmbcolumnStatus"].Value != null
                                && dgvAttendance.Rows[i].Cells["dgvcmbcolumnStatus"].Value.ToString() != "")
                            {
                                infoDailyAttendanceDetails.Status = dgvAttendance.Rows[i].Cells["dgvcmbcolumnStatus"].Value.ToString();
                            }
                            else
                            {
                                infoDailyAttendanceDetails.Status = "Present";
                            }
                            if (dgvAttendance.Rows[i].Cells["dgvtxtColumnnarration"].Value != null &&
                           dgvAttendance.Rows[i].Cells["dgvtxtColumnnarration"].Value.ToString() != "")
                            {
                                infoDailyAttendanceDetails.Narration = dgvAttendance.Rows[i].Cells["dgvtxtColumnnarration"].Value.ToString();
                            }
                            else
                            {
                                infoDailyAttendanceDetails.Narration = "";
                            }
                            infoDailyAttendanceDetails.Extra1 = string.Empty;
                            infoDailyAttendanceDetails.Extra2 = string.Empty;
                            infoDailyAttendanceDetails.DailyAttendanceMasterId = decMasterIdForEdit;
                            spDailyAttendanceDetails.DailyAttendanceDetailsAddUsingMasterId(infoDailyAttendanceDetails);
                        }
                    } //   updation of old employees & addition of new employees closes here
                    Messages.UpdatedMessage();
                    Clear();
                }
                else
                {
                    Messages.InformationMessage("Selected date is holiday");
                }
            }

            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "A3:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call save or edit
        /// </summary>
        public void SaveOrEdit()
        {
            try
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

                            EditFunction();
                        }
                    }
                    else
                    {
                        EditFunction();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "A4:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to Delete
        /// </summary>
        public void DeleteFunction()
        {
            try
            {
                DailyAttendanceDetailsSP spDailyAttendanceDetails = new DailyAttendanceDetailsSP();
                spDailyAttendanceDetails.DailyAttendanceDetailsDeleteAll(Convert.ToDecimal(dgvAttendance.CurrentRow.Cells["dgvtxtdailyAttendanceMasterId"].Value.ToString()));
                Messages.DeletedMessage();
                Clear();
                btnDelete.Enabled = false;
                btnSave.Text = "Save";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "A5:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call Delete
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
                formMDI.infoError.ErrorString = "A6:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to clear controls
        /// </summary>
        public void Clear()
        {
            try
            {
                int rowcount = dgvAttendance.RowCount;
                txtNarrationInMaster.Text = String.Empty;
                for (int i = 0; i <= rowcount - 1; i++)
                {
                    dgvAttendance.Rows[i].Cells["dgvcmbcolumnStatus"].Value = null;
                    dgvAttendance.Rows[i].Cells["dgvtxtColumnnarration"].Value = null;
                }
                txtCompanyCurrentdate.Text = PublicVariables._dtCurrentDate.ToString("dd-mmm-yyyy");
                txtCompanyCurrentdate.Text = PublicVariables._dtCurrentDate.ToString();
                dtpCompanyCurrentDate.MinDate = PublicVariables._dtFromDate;
                dtpCompanyCurrentDate.MaxDate = PublicVariables._dtToDate;
                dtpCompanyCurrentDate.Value = PublicVariables._dtCurrentDate;
                txtCompanyCurrentdate.Text = dtpCompanyCurrentDate.Value.ToString("dd-MMM-yyyy");
                GridFill();
                txtCompanyCurrentdate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "A7:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to check the selected date is holiday or not
        /// </summary>
        /// <returns></returns>
        public decimal HolidaySettings()
        {
            HolidaySP spHoliday = new HolidaySP();
            DateTime date = this.dtpCompanyCurrentDate.Value;
            decimal decResult = spHoliday.HolliDayChecking(date);
            try
            {
                this.txtCompanyCurrentdate.Text = date.ToString("dd MMM yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "A8" + ex.Message;
            }
            return decResult;
        }
        /// <summary>
        /// Function to display the Selected date is holiday
        /// </summary>
        public void HolidayIndication()
        {
            try
            {
                decimal decResult = HolidaySettings();
                if (decResult == 1)
                {
                    lblHolidayChecking.Text = "Selected date is holiday";
                }
                else
                {
                    lblHolidayChecking.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "A9:" + ex.Message;
            }

        }
        #endregion

        #region Events
        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmAttendance_Load(object sender, EventArgs e)
        {
            try
            {
                dtpCompanyCurrentDate.MinDate = PublicVariables._dtFromDate;
                dtpCompanyCurrentDate.MaxDate = PublicVariables._dtCurrentDate;
                dtpCompanyCurrentDate.Value = PublicVariables._dtCurrentDate;
                HolidayIndication();
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "A10:" + ex.Message;
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
                formMDI.infoError.ErrorString = "A11:" + ex.Message;
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
                formMDI.infoError.ErrorString = "A12:" + ex.Message;
            }
        }
        /// <summary>
        /// Clears selection on Databind
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvAttendance_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                dgvAttendance.ClearSelection();
                foreach (DataGridViewRow drRow in dgvAttendance.Rows)
                {
                    if (drRow != null)
                    {
                        if (drRow.Cells["dgvtxtDailyAttendanceDetailsId"].FormattedValue.ToString() == "")
                        {
                            drRow.Cells["dgvcmbcolumnStatus"].Style.BackColor = Color.LightSeaGreen;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "A13:" + ex.Message;
            }
        }
        /// <summary>
        /// Commit edit on cell dirtystate changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvAttendance_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvAttendance.IsCurrentCellDirty)
                {
                    dgvAttendance.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "A14:" + ex.Message;
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
                    if (dgvAttendance.RowCount == 0)
                    {
                        Messages.InformationMessage("There is no employee in the selected date.Select another date");
                        txtCompanyCurrentdate.Focus();
                    }
                    else
                    {
                        SaveOrEdit();
                    }
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "A15:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'delete' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnDelete.Text))
                {
                    Delete();
                }

                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "A16:" + ex.Message;
            }
        }
        //Fills Datagridview on dtpCompanyCurrentDate Datetimepicker ValueChanged
        private void dtpCompanyCurrentDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {

                DateTime date = this.dtpCompanyCurrentDate.Value;
                this.txtCompanyCurrentdate.Text = date.ToString("dd MMM yyyy");
                
                GridFill();
                HolidayIndication();

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "A17:" + ex.Message;
            }
        }

        #endregion

        #region Navigation
        /// <summary>
        /// Escape key navigation and Form keydown for quick access
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmAttendance_KeyDown(object sender, KeyEventArgs e)
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
                if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control) //Save
                {
                    if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnSave.Text))
                    {
                        btnSave.Focus();
                        btnSave_Click(sender, e);
                    }
                    else
                    {
                        Messages.NoPrivillageMessage();
                    }
                }
                if (e.KeyCode == Keys.D && Control.ModifierKeys == Keys.Control) //Delete
                {
                    if (btnDelete.Enabled == true)
                    {
                        if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnDelete.Text))
                        {
                            btnDelete_Click(sender, e);
                        }
                        else
                        {
                            Messages.NoPrivillageMessage();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "A18:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key navigation 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarrationInMaster_KeyPress(object sender, KeyPressEventArgs e)
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
                formMDI.infoError.ErrorString = "A19:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter Key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarrationInMaster_Enter(object sender, EventArgs e)
        {
            try
            {
                inNarrationCount = 0;
                txtNarrationInMaster.Text = txtNarrationInMaster.Text.Trim();
                if (txtNarrationInMaster.Text == String.Empty)
                {
                    txtNarrationInMaster.SelectionStart = 0;
                    txtNarrationInMaster.SelectionLength = 0;
                    txtNarrationInMaster.Focus();
                }
                else
                {
                    txtNarrationInMaster.SelectionStart = txtNarrationInMaster.Text.Length;
                    txtNarrationInMaster.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "A20:" + ex.Message;
            }
        }
        /// <summary>
        /// Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    txtNarrationInMaster.Focus();
                    txtNarrationInMaster.SelectionStart = 0;
                    txtNarrationInMaster.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "A21:" + ex.Message;
            }
        }
        /// <summary>
        ///  Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarrationInMaster_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (txtNarrationInMaster.Text == String.Empty || txtNarrationInMaster.SelectionStart == 0)
                    {
                        if (dgvAttendance.Rows.Count > 0)
                        {
                            dgvAttendance.Focus();
                            dgvAttendance.ClearSelection();
                            dgvAttendance.CurrentCell = dgvAttendance.Rows[dgvAttendance.Rows.Count - 1].Cells["dgvcmbcolumnStatus"];
                            dgvAttendance.Rows[dgvAttendance.Rows.Count - 1].Cells["dgvcmbcolumnStatus"].Selected = true;
                        }
                        else
                        {
                            txtCompanyCurrentdate.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "A22:" + ex.Message;
            }
        }
        /// <summary>
        ///  Enter key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCompanyCurrentdate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //To focus to cmbStatus combo on pressing enter key
                if (e.KeyCode == Keys.Enter)
                {
                    dgvAttendance.Focus();
                    dgvAttendance.ClearSelection();
                    if (dgvAttendance.Rows.Count > 0)
                    {
                        dgvAttendance.CurrentCell = dgvAttendance.Rows[0].Cells["dgvcmbcolumnStatus"];
                        dgvAttendance.Rows[0].Cells["dgvcmbcolumnStatus"].Selected = true;
                    }
                    else
                    {
                        btnSave.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "A23:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvAttendance_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvAttendance.Rows.Count > 0)
                    {
                        if ((dgvAttendance.CurrentCell == dgvAttendance[dgvAttendance.Columns["dgvcmbcolumnStatus"].Index, dgvAttendance.Rows.Count - 1]) || dgvAttendance.CurrentRow.Index == dgvAttendance.RowCount - 1)
                        {
                            if (inGridRowCount == 1)
                            {
                                inGridRowCount = 0;
                                txtNarrationInMaster.Focus();
                                dgvAttendance.ClearSelection();
                                e.Handled = true;
                            }
                            else
                            {
                                inGridRowCount++;
                            }
                        }
                    }
                    else
                    {
                        btnSave.Focus();
                    }
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (dgvAttendance.Rows.Count > 0)
                    {
                        if (dgvAttendance.CurrentCell == dgvAttendance[dgvAttendance.Columns["dgvcmbcolumnStatus"].Index, 0])
                        {
                            txtCompanyCurrentdate.Focus();
                        }
                        else
                        {
                            if (dgvAttendance.CurrentCell.RowIndex > 0)
                            {
                                dgvAttendance.CurrentCell = dgvAttendance[dgvAttendance.Columns["dgvcmbcolumnStatus"].Index, dgvAttendance.CurrentRow.Index - 1];
                            }
                            else
                            {
                                txtCompanyCurrentdate.Focus();
                            }
                        }
                    }
                    else
                    {
                        txtCompanyCurrentdate.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "A24:" + ex.Message;
            }
        }
        /// <summary>
        /// Date validation and fill datagridview 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCompanyCurrentdate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation obj = new DateValidation();
                obj.DateValidationFunction(txtCompanyCurrentdate);
                if (txtCompanyCurrentdate.Text == String.Empty)
                    txtCompanyCurrentdate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                if (DateTime.Parse(txtCompanyCurrentdate.Text)>PublicVariables._dtCurrentDate)
                {
                    txtCompanyCurrentdate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                dtpCompanyCurrentDate.Value = DateTime.Parse(txtCompanyCurrentdate.Text);
                GridFill();
                HolidayIndication();

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "A25:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpCompanyCurrenttime_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtCompanyCurrentdate.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "A26:" + ex.Message;
            }
        }

        #endregion

    }
}
