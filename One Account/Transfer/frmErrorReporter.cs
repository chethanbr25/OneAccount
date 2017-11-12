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
using System.Threading;
using System.Web.Mail;

namespace One_Account
{
    public partial class frmErrorReporter : Form
    {
        Thread oThread;
        #region Function
        /// <summary>
        /// Creates instance of frmErrorReporter class
        /// </summary>
        public frmErrorReporter(string infoError)
        {
            try
            {
                InitializeComponent();
                txtError.Text = infoError;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "err-R1" + ex.Message;
            }
        }

        /// <summary>
        /// Function for Start the thread
        /// </summary>
        private void threadStart()
        {
            try
            {
                CompanySP spCompany = new CompanySP();
                CompanyInfo infoCompany = new CompanyInfo();
                try
                {
                    infoCompany = spCompany.CompanyView(1);
                }
                catch (Exception)
                { }


                MailMessage mailMsg = new MailMessage();
                mailMsg.From = "";
                mailMsg.To = "";
                mailMsg.Subject = "Oneaccount MsSql Error!" + " Version : " + Application.ProductVersion;
                mailMsg.BodyFormat = MailFormat.Text;
                mailMsg.Body = infoCompany.EmailId + " - " + txtError.Text;
                mailMsg.Priority = MailPriority.High;
                SmtpMail.SmtpServer = "smtp.gmail.com";//smtp is :smtp.gmail.com
                // SmtpMail.SmtpServer = "plus.smtp.mail.yahoo.com";//smtp is :smtp.yahoo.com
                mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
                mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", "");
                mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "");
                // - smtp.gmail.com use port 465 or 587
                // - plus.smtp.mail.yahoo.com use port 465 or 587
                mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", "465");
                mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");
                SmtpMail.Send(mailMsg);
                Cursor.Current = Cursors.Default;

                this.Invoke(new MethodInvoker(delegate { this.Close(); }));
            }
            catch
            {

            }
        }

        #endregion
        #region Events
        /// <summary>
        /// On Button btnSendError Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendError_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                oThread = new Thread(threadStart);
                oThread.Start();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "err-R2" + ex.Message;
            }
        }

        /// <summary>
        /// On Button btnSendErrorReport Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendErrorReport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "err-R3" + ex.Message;
            }
        }

        /// <summary>
        /// On Form frmErrorReporter Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmErrorReporter_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (oThread != null && oThread.IsAlive)
                {
                    oThread.Abort();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "err-R4" + ex.Message;
            }
        }
        #endregion
    }
}
