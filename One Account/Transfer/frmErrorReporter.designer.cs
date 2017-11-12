namespace One_Account
{
    partial class frmErrorReporter
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmErrorReporter));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtError = new System.Windows.Forms.TextBox();
            this.btnSendErrorReport = new System.Windows.Forms.Button();
            this.btnSendError = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(15, 25);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(408, 52);
            this.label1.TabIndex = 0;
            this.label1.Text = "Oneaccount has found encountered a problem. We are sorry for  the  inconvenience" +
    "";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(16, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(412, 33);
            this.label2.TabIndex = 1;
            this.label2.Text = "If you were in the middle of something, the information you were working on might" +
    " be lost.";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label3.Location = new System.Drawing.Point(112, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(311, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "We have created an error report that you can send";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(294, 125);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(126, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "The error report contains:";
            // 
            // txtError
            // 
            this.txtError.Location = new System.Drawing.Point(15, 141);
            this.txtError.Multiline = true;
            this.txtError.Name = "txtError";
            this.txtError.ReadOnly = true;
            this.txtError.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtError.Size = new System.Drawing.Size(405, 108);
            this.txtError.TabIndex = 4;
            // 
            // btnSendErrorReport
            // 
            this.btnSendErrorReport.Location = new System.Drawing.Point(345, 256);
            this.btnSendErrorReport.Name = "btnSendErrorReport";
            this.btnSendErrorReport.Size = new System.Drawing.Size(75, 23);
            this.btnSendErrorReport.TabIndex = 5;
            this.btnSendErrorReport.Text = "Don\'t Send";
            this.btnSendErrorReport.UseVisualStyleBackColor = true;
            this.btnSendErrorReport.Click += new System.EventHandler(this.btnSendErrorReport_Click);
            // 
            // btnSendError
            // 
            this.btnSendError.Location = new System.Drawing.Point(233, 256);
            this.btnSendError.Name = "btnSendError";
            this.btnSendError.Size = new System.Drawing.Size(105, 23);
            this.btnSendError.TabIndex = 6;
            this.btnSendError.Text = "Send Error Report";
            this.btnSendError.UseVisualStyleBackColor = true;
            this.btnSendError.Click += new System.EventHandler(this.btnSendError_Click);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label5.Location = new System.Drawing.Point(16, 108);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(311, 19);
            this.label5.TabIndex = 7;
            this.label5.Text = "to help us improve Oneaccount.";
            // 
            // frmErrorReporter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 283);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnSendError);
            this.Controls.Add(this.btnSendErrorReport);
            this.Controls.Add(this.txtError);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmErrorReporter";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Error Reporting";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmErrorReporter_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtError;
        private System.Windows.Forms.Button btnSendErrorReport;
        private System.Windows.Forms.Button btnSendError;
        private System.Windows.Forms.Label label5;
    }
}