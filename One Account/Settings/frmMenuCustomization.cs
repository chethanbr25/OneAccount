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
    public partial class frmMenuCustomization : Form
    {
        #region Public Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        DataTable dtblNonSelected = new DataTable();
        DataTable dtblSelected = new DataTable();
        ucQuickLaunch ucQuick = new ucQuickLaunch();
        #endregion
        #region Functions
        /// <summary>
        /// Create An instance for frmMenuCustomization class
        /// </summary>
        public frmMenuCustomization()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to fill NonSelected Items List
        /// </summary>
        public void FillNonSelectedList()
        {
            try
            {
                QuickLaunchItemsSP spQuickLaunchItems = new QuickLaunchItemsSP();
                dtblNonSelected = spQuickLaunchItems.QuickLaunchNonSelectedViewAll(false);
                lstbxNonSelected.DataSource = dtblNonSelected;
                lstbxNonSelected.ValueMember = "quickLaunchItemsId";
                lstbxNonSelected.DisplayMember = "itemsName";
                dtblSelected = spQuickLaunchItems.QuickLaunchNonSelectedViewAll(true);
                lstbxSelected.DataSource = dtblSelected;
                lstbxSelected.ValueMember = "quickLaunchItemsId";
                lstbxSelected.DisplayMember = "itemsName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MC:1 " + ex.Message;
             
            }
        }
        /// <summary>
        /// Form controls will be reset here
        /// </summary>
        public void Clear()
        {
            try
            {
                lstbxNonSelected.SelectedIndex = -1;
                lstbxSelected.SelectedIndex = -1;
                lstbxNonSelected.ClearSelected();
                lstbxSelected.ClearSelected();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MC:2 " + ex.Message;
            }
        }
        /// <summary>
        /// Save or edit function, Here checking the items in the list
        /// </summary>
        private void SaveorEdit()
        {
            try
            {

                QuickLaunchItemsInfo infoQuickLaunchItems = new QuickLaunchItemsInfo();
                QuickLaunchItemsSP spQuickLaunchItems = new QuickLaunchItemsSP();
                if (dtblSelected.Rows.Count >= 1)
                {
                    foreach (DataRow dRoW in dtblSelected.Rows)
                    {
                        infoQuickLaunchItems.ItemsName = dRoW.ItemArray[0].ToString();
                        infoQuickLaunchItems.QuickLaunchItemsId = Convert.ToDecimal(dRoW.ItemArray[1]);
                        infoQuickLaunchItems.Status = true;
                        infoQuickLaunchItems.Extra1 = string.Empty;
                        infoQuickLaunchItems.Extra2 = string.Empty;
                        spQuickLaunchItems.QuickLaunchItemsEdit(infoQuickLaunchItems);
                    }
                    foreach (DataRow dRoW in dtblNonSelected.Rows)
                    {
                        infoQuickLaunchItems.ItemsName = dRoW.ItemArray[0].ToString();
                        infoQuickLaunchItems.QuickLaunchItemsId = Convert.ToDecimal(dRoW.ItemArray[1]);
                        infoQuickLaunchItems.Status = false;
                        infoQuickLaunchItems.Extra1 = string.Empty;
                        infoQuickLaunchItems.Extra2 = string.Empty;
                        spQuickLaunchItems.QuickLaunchItemsEdit(infoQuickLaunchItems);
                    }
                    Messages.SavedMessage();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Select atleast one form in quick launch", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MC:3 " + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// When form load clear the controls in form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMenuCustomization_Load(object sender, EventArgs e)
        {
            try
            {
                FillNonSelectedList();
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MC:4 " + ex.Message;
            }
        }
        /// <summary>
        /// Butten Forwad Click to add a new item into the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnForward_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstbxSelected.Items.Count < 19 && (lstbxNonSelected.SelectedItems.Count + lstbxSelected.Items.Count <= 19))
                {
                    foreach (DataRowView drv in lstbxNonSelected.SelectedItems)
                    {
                        if (lstbxSelected.Items.Count < 19)
                        {
                            DataRow dr = dtblSelected.NewRow();
                            dr[0] = drv.Row.ItemArray[0].ToString();
                            dr[1] = Convert.ToDecimal(drv.Row.ItemArray[1]);
                            dtblSelected.Rows.Add(dr);
                        }
                    }
                    int[] selectedIndx = new int[20];
                    int index = 0;
                    for (int i = 0; i < lstbxNonSelected.Items.Count; i++)
                    {
                        if (lstbxNonSelected.GetSelected(i))
                        {
                            selectedIndx[index] = i;
                            index++;
                        }
                    }
                    for (int j = 0; j < index; j++)
                    {
                        dtblNonSelected.Rows.RemoveAt(selectedIndx[j] - j);
                    }
                    Clear();
                }
                else
                {
                    Messages.InformationMessage("Cannot add more than nineteen item");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MC:5 " + ex.Message;
            }
        }
        /// <summary>
        /// Butten Backward Click to Remove a Selected item from the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBackward_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataRowView drv in lstbxSelected.SelectedItems)
                {
                    DataRow dr = dtblNonSelected.NewRow();
                    dr[0] = drv.Row.ItemArray[0].ToString();
                    dr[1] = Convert.ToDecimal(drv.Row.ItemArray[1]);
                    dtblNonSelected.Rows.Add(dr);
                }
                int[] selectedIndx = new int[20];
                int index = 0;
                for (int i = 0; i < lstbxSelected.Items.Count; i++)
                {
                    if (lstbxSelected.GetSelected(i))
                    {
                        selectedIndx[index] = i;
                        index++;
                    }
                }
                for (int j = 0; j < index; j++)
                {
                    dtblSelected.Rows.RemoveAt(selectedIndx[j] - j);
                }
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MC:6 " + ex.Message;
            }
        }
        /// <summary>
        /// Clear button click, clear the controls and call the fill function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                FillNonSelectedList();
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MC:7 " + ex.Message;
            }
        }
        /// <summary>
        /// Close button click
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
                formMDI.infoError.ErrorString = "MC:8 " + ex.Message;
            }
        }
        /// <summary>
        /// Save button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (PublicVariables.isMessageAdd)
                {
                    if (Messages.SaveConfirmation())
                        SaveorEdit();
                }
                else
                    SaveorEdit();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MC:9 " + ex.Message;
            }
        }
        /// <summary>
        /// Form closing call the quick launch menu items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMenuCustomization_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                ucQuick.Enabled = true;
                ucQuick.ReturnFromCustomization();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MC:10 " + ex.Message;
            }
        }
        /// <summary>
        /// Call the ShortCutKey class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMenuCustomization_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                Common.ExecuteShortCutKey(e, btnSave, btnClose);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MC:11 " + ex.Message;
            }
        }
        #endregion

        public void Shows(ucQuickLaunch ucObj)
        {
            try
            {
                ucQuick = ucObj;
                base.Show();
            }
            catch (Exception ex)
            {

                formMDI.infoError.ErrorString = "MC:12 " + ex.Message;
            }
        }
    }
}
