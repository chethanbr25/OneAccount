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
using System.Text;
using System.Windows.Forms;

namespace One_Account
{
    public partial class frmForms : Form
    {
        #region Variables
        public int informId = 0;
        #endregion
        #region Functions
        /// <summary>
        /// create an instance of frmForms class
        /// </summary>
        public frmForms()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function for clear all the fields
        /// </summary>
        public void Clear()
        {
            try
            {
                txtFormName.Text = "";
                DataTable dtbl = new DataTable();
                dtbl.Columns.Add("fieldName");
                dtbl.Columns.Add("fieldId");
                dtbl.Columns.Add("formId");
                dgvFields.DataSource = dtbl;
                informId = 0;
                btnDelete.Enabled = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "forms1" + ex.Message;
            }
        }
        /// <summary>
        /// Function for fill the grid
        /// </summary>
        public void FillGrid()
        {
            try
            {
                FormSP spForm = new FormSP();
                DataTable dtbl = new DataTable();
                dtbl = spForm.FormViewAll();
                dgvForms.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "forms2" + ex.Message;
            }
        }
        /// <summary>
        /// Function for save form details
        /// </summary>
        public void Save()
        {
            try
            {
                FormInfo infoForm = new FormInfo();
                FormSP spForm = new FormSP();
                FieldInfo infoField = new FieldInfo();
                FieldSP spField = new FieldSP();
                int inId = 0;
                infoForm.FormName = txtFormName.Text;
                if (informId == 0)
                {
                    inId = spForm.FormAdd(infoForm);
                    if (inId > -1)
                    {
                        infoField.FormId = inId;
                        int inCount = dgvFields.Rows.Count - 1;
                        for (int i = 0; i < inCount; i++)
                        {
                            infoField.FieldName = dgvFields.Rows[i].Cells["fieldName"].Value.ToString();
                            spField.FieldsAdd(infoField);
                        }
                        Clear();
                        MessageBox.Show("Saved successfully", "Forms", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                        MessageBox.Show("Form name alredy exist", "Forms", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    inId = informId;
                    infoForm.FormId = inId;
                    bool isOk = spForm.FormEdit(infoForm);
                    if (!isOk)
                        if (MessageBox.Show("Reference exist in print design, do you want to continue ?", "Forms", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            isOk = true;
                            spForm.FormEditFull(infoForm);
                        }
                    if (isOk)
                    {
                        infoField.FormId = inId;
                        spField.FieldsDelete(inId);
                        int inCount = dgvFields.Rows.Count - 1;
                        for (int i = 0; i < inCount; i++)
                        {
                            infoField.FieldName = dgvFields.Rows[i].Cells["fieldName"].Value.ToString();
                            spField.FieldsAdd(infoField);
                        }
                        Clear();
                        MessageBox.Show("Updated successfully", "Forms", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                frmPrintDesigner.obj.FormFill();
                FillGrid();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "forms3" + ex.Message;
            }
        }
        /// <summary>
        /// Function for delete form 
        /// </summary>
        public void Delete()
        {
            try
            {
                string str = MessageBox.Show("Are you sure to delete ?", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question).ToString();
                if (str == "Yes")
                {
                    FormSP spForm = new FormSP();
                    FieldSP spField = new FieldSP();
                    bool isOk = spForm.FormDelete(informId);
                    if (isOk)
                    {
                        spField.FieldsDelete(informId);
                        Clear();
                        FillGrid();
                        frmPrintDesigner.obj.FormFill();
                        MessageBox.Show("Deleted successfully", "Forms", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                        MessageBox.Show("Reference exist in print design", "Forms", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "forms4" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// On Button btnSave Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Save();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "forms5" + ex.Message;
            }
        }
        /// <summary>
        /// On Form frmForms Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmForms_Load(object sender, EventArgs e)
        {
            try
            {
                FillGrid();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "forms6" + ex.Message;
            }
        }
        /// <summary>
        /// On DataGridView dgvForms CellClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvForms_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvForms.Rows.Count > 0)
                {
                    informId =int.Parse(dgvForms.CurrentRow.Cells["formId"].Value.ToString());
                    FormSP spForm = new FormSP();
                    FormInfo infoForm = new FormInfo();
                    FieldSP spField = new FieldSP();
                    infoForm = spForm.FormView(informId);
                    txtFormName.Text = infoForm.FormName;
                    dgvFields.DataSource=spField.FieldsViewAll(informId);
                    btnDelete.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "forms7" + ex.Message;
            }
        }
        /// <summary>
        /// On Button btnClear Click
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
                formMDI.infoError.ErrorString = "forms8" + ex.Message;
            }
        }
        /// <summary>
        /// On Button btnDelete Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Delete();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "forms9" + ex.Message;
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// Form ShortCut Keys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmForms_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control && e.KeyCode == Keys.S)
                {
                    Save();
                }
                else if (e.Control && e.KeyCode == Keys.N)
                {
                    //New();
                }
                else if (e.Control && e.KeyCode == Keys.D)
                {
                    Delete();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "forms10" + ex.Message;
            }
        }
        #endregion
    }
}