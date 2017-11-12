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
    public partial class frmAboutUs : Form
    {
        #region Function
        /// <summary>
        /// Creates an instance of frmAboutUs class
        /// </summary>
        public frmAboutUs()
        {
            InitializeComponent();
        }
        #endregion
        #region Events
        /// <summary>
        /// On link label click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbtnOM_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.Oneaccount.com/");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "AU1:" + ex.Message;
            }
        }
        /// <summary>
        /// On link label click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbtnCybro_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://C-Macys.com/");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "AU2:" + ex.Message;
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// Escape key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmAboutUs_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "AU3:" + ex.Message;
            }
        }
        #endregion

        private void frmAboutUs_Load(object sender, EventArgs e)
        {
            try
            {
                label5.Text = Application.ProductName;
                label1.Text = "Version : " + Application.ProductVersion;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "AU4:" + ex.Message;
            }
        }
    }
}
