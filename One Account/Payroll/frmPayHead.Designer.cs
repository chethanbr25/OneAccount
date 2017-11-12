namespace One_Account
{
    partial class frmPayHead
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPayHead));
            this.btnPayheadClose = new System.Windows.Forms.Button();
            this.btnPayheadDelete = new System.Windows.Forms.Button();
            this.btnPayheadClear = new System.Windows.Forms.Button();
            this.btnPayheadSave = new System.Windows.Forms.Button();
            this.txtPayheadSearch = new System.Windows.Forms.TextBox();
            this.lblPayheadSearch = new System.Windows.Forms.Label();
            this.dgvPayhead = new System.Windows.Forms.DataGridView();
            this.dgvtxtSerialNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtxtPayheadId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtxtPayheadName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtPayheadNarration = new System.Windows.Forms.TextBox();
            this.txtPayheadName = new System.Windows.Forms.TextBox();
            this.lblPayheadNarration = new System.Windows.Forms.Label();
            this.lblpayheadType = new System.Windows.Forms.Label();
            this.lblPayheadName = new System.Windows.Forms.Label();
            this.cmbPayheadType = new System.Windows.Forms.ComboBox();
            this.lblPayheadTypeValidator = new System.Windows.Forms.Label();
            this.lblPayheadTNameValidator = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayhead)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPayheadClose
            // 
            this.btnPayheadClose.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnPayheadClose.FlatAppearance.BorderSize = 0;
            this.btnPayheadClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayheadClose.ForeColor = System.Drawing.Color.Black;
            this.btnPayheadClose.Location = new System.Drawing.Point(663, 100);
            this.btnPayheadClose.Name = "btnPayheadClose";
            this.btnPayheadClose.Size = new System.Drawing.Size(85, 27);
            this.btnPayheadClose.TabIndex = 3;
            this.btnPayheadClose.Text = "Close";
            this.btnPayheadClose.UseVisualStyleBackColor = false;
            this.btnPayheadClose.Click += new System.EventHandler(this.btnPayheadClose_Click);
            // 
            // btnPayheadDelete
            // 
            this.btnPayheadDelete.BackColor = System.Drawing.Color.Salmon;
            this.btnPayheadDelete.FlatAppearance.BorderSize = 0;
            this.btnPayheadDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayheadDelete.ForeColor = System.Drawing.Color.Black;
            this.btnPayheadDelete.Location = new System.Drawing.Point(572, 100);
            this.btnPayheadDelete.Name = "btnPayheadDelete";
            this.btnPayheadDelete.Size = new System.Drawing.Size(85, 27);
            this.btnPayheadDelete.TabIndex = 2;
            this.btnPayheadDelete.Text = "Delete";
            this.btnPayheadDelete.UseVisualStyleBackColor = false;
            this.btnPayheadDelete.Click += new System.EventHandler(this.btnPayheadDelete_Click);
            // 
            // btnPayheadClear
            // 
            this.btnPayheadClear.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnPayheadClear.FlatAppearance.BorderSize = 0;
            this.btnPayheadClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayheadClear.ForeColor = System.Drawing.Color.Black;
            this.btnPayheadClear.Location = new System.Drawing.Point(481, 100);
            this.btnPayheadClear.Name = "btnPayheadClear";
            this.btnPayheadClear.Size = new System.Drawing.Size(85, 27);
            this.btnPayheadClear.TabIndex = 1;
            this.btnPayheadClear.Text = "Clear";
            this.btnPayheadClear.UseVisualStyleBackColor = false;
            this.btnPayheadClear.Click += new System.EventHandler(this.btnPayheadClear_Click);
            // 
            // btnPayheadSave
            // 
            this.btnPayheadSave.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnPayheadSave.FlatAppearance.BorderSize = 0;
            this.btnPayheadSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayheadSave.ForeColor = System.Drawing.Color.Black;
            this.btnPayheadSave.Location = new System.Drawing.Point(390, 100);
            this.btnPayheadSave.Name = "btnPayheadSave";
            this.btnPayheadSave.Size = new System.Drawing.Size(85, 27);
            this.btnPayheadSave.TabIndex = 0;
            this.btnPayheadSave.Text = "Save";
            this.btnPayheadSave.UseVisualStyleBackColor = false;
            this.btnPayheadSave.Click += new System.EventHandler(this.btnPayheadSave_Click);
            this.btnPayheadSave.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnPayheadSave_KeyDown);
            // 
            // txtPayheadSearch
            // 
            this.txtPayheadSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPayheadSearch.Location = new System.Drawing.Point(492, 27);
            this.txtPayheadSearch.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.txtPayheadSearch.Name = "txtPayheadSearch";
            this.txtPayheadSearch.Size = new System.Drawing.Size(255, 20);
            this.txtPayheadSearch.TabIndex = 4;
            this.txtPayheadSearch.TextChanged += new System.EventHandler(this.txtPayheadSearch_TextChanged);
            this.txtPayheadSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPayheadSearch_KeyDown);
            // 
            // lblPayheadSearch
            // 
            this.lblPayheadSearch.AutoSize = true;
            this.lblPayheadSearch.BackColor = System.Drawing.Color.Gainsboro;
            this.lblPayheadSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPayheadSearch.ForeColor = System.Drawing.Color.Black;
            this.lblPayheadSearch.Location = new System.Drawing.Point(427, 29);
            this.lblPayheadSearch.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lblPayheadSearch.Name = "lblPayheadSearch";
            this.lblPayheadSearch.Size = new System.Drawing.Size(47, 13);
            this.lblPayheadSearch.TabIndex = 27;
            this.lblPayheadSearch.Text = "Search :";
            // 
            // dgvPayhead
            // 
            this.dgvPayhead.AllowUserToAddRows = false;
            this.dgvPayhead.AllowUserToDeleteRows = false;
            this.dgvPayhead.AllowUserToResizeColumns = false;
            this.dgvPayhead.AllowUserToResizeRows = false;
            this.dgvPayhead.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPayhead.BackgroundColor = System.Drawing.Color.White;
            this.dgvPayhead.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPayhead.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPayhead.ColumnHeadersHeight = 25;
            this.dgvPayhead.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPayhead.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvtxtSerialNo,
            this.dgvtxtPayheadId,
            this.dgvtxtPayheadName});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPayhead.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPayhead.EnableHeadersVisualStyles = false;
            this.dgvPayhead.GridColor = System.Drawing.Color.DimGray;
            this.dgvPayhead.Location = new System.Drawing.Point(35, 228);
            this.dgvPayhead.MultiSelect = false;
            this.dgvPayhead.Name = "dgvPayhead";
            this.dgvPayhead.ReadOnly = true;
            this.dgvPayhead.RowHeadersVisible = false;
            this.dgvPayhead.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPayhead.Size = new System.Drawing.Size(731, 303);
            this.dgvPayhead.TabIndex = 5;
            this.dgvPayhead.TabStop = false;
            this.dgvPayhead.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPayhead_CellDoubleClick);
            this.dgvPayhead.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvPayhead_DataBindingComplete);
            this.dgvPayhead.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvPayhead_KeyDown);
            this.dgvPayhead.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgvPayhead_KeyUp);
            // 
            // dgvtxtSerialNo
            // 
            this.dgvtxtSerialNo.DataPropertyName = "Slno:";
            this.dgvtxtSerialNo.HeaderText = "Sl.No";
            this.dgvtxtSerialNo.Name = "dgvtxtSerialNo";
            this.dgvtxtSerialNo.ReadOnly = true;
            this.dgvtxtSerialNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvtxtPayheadId
            // 
            this.dgvtxtPayheadId.DataPropertyName = "payHeadId";
            this.dgvtxtPayheadId.HeaderText = "PayHeadId";
            this.dgvtxtPayheadId.Name = "dgvtxtPayheadId";
            this.dgvtxtPayheadId.ReadOnly = true;
            this.dgvtxtPayheadId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgvtxtPayheadId.Visible = false;
            // 
            // dgvtxtPayheadName
            // 
            this.dgvtxtPayheadName.DataPropertyName = "payHeadName";
            this.dgvtxtPayheadName.HeaderText = "Payhead Name";
            this.dgvtxtPayheadName.Name = "dgvtxtPayheadName";
            this.dgvtxtPayheadName.ReadOnly = true;
            this.dgvtxtPayheadName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // txtPayheadNarration
            // 
            this.txtPayheadNarration.Location = new System.Drawing.Point(140, 90);
            this.txtPayheadNarration.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.txtPayheadNarration.MaxLength = 5000;
            this.txtPayheadNarration.Multiline = true;
            this.txtPayheadNarration.Name = "txtPayheadNarration";
            this.txtPayheadNarration.Size = new System.Drawing.Size(247, 50);
            this.txtPayheadNarration.TabIndex = 2;
            this.txtPayheadNarration.Enter += new System.EventHandler(this.txtPayheadNarration_Enter);
            this.txtPayheadNarration.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPayheadNarration_KeyDown);
            this.txtPayheadNarration.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPayheadNarration_KeyPress);
            // 
            // txtPayheadName
            // 
            this.txtPayheadName.Location = new System.Drawing.Point(140, 39);
            this.txtPayheadName.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.txtPayheadName.Name = "txtPayheadName";
            this.txtPayheadName.Size = new System.Drawing.Size(247, 20);
            this.txtPayheadName.TabIndex = 0;
            this.txtPayheadName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPayheadName_KeyDown);
            // 
            // lblPayheadNarration
            // 
            this.lblPayheadNarration.AutoSize = true;
            this.lblPayheadNarration.ForeColor = System.Drawing.Color.Black;
            this.lblPayheadNarration.Location = new System.Drawing.Point(43, 92);
            this.lblPayheadNarration.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lblPayheadNarration.Name = "lblPayheadNarration";
            this.lblPayheadNarration.Size = new System.Drawing.Size(50, 13);
            this.lblPayheadNarration.TabIndex = 21;
            this.lblPayheadNarration.Text = "Narration";
            // 
            // lblpayheadType
            // 
            this.lblpayheadType.AutoSize = true;
            this.lblpayheadType.ForeColor = System.Drawing.Color.Black;
            this.lblpayheadType.Location = new System.Drawing.Point(43, 70);
            this.lblpayheadType.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lblpayheadType.Name = "lblpayheadType";
            this.lblpayheadType.Size = new System.Drawing.Size(31, 13);
            this.lblpayheadType.TabIndex = 19;
            this.lblpayheadType.Text = "Type";
            // 
            // lblPayheadName
            // 
            this.lblPayheadName.AutoSize = true;
            this.lblPayheadName.ForeColor = System.Drawing.Color.Black;
            this.lblPayheadName.Location = new System.Drawing.Point(43, 45);
            this.lblPayheadName.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lblPayheadName.Name = "lblPayheadName";
            this.lblPayheadName.Size = new System.Drawing.Size(35, 13);
            this.lblPayheadName.TabIndex = 18;
            this.lblPayheadName.Text = "Name";
            // 
            // cmbPayheadType
            // 
            this.cmbPayheadType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPayheadType.FormattingEnabled = true;
            this.cmbPayheadType.Items.AddRange(new object[] {
            "Addition",
            "Deduction"});
            this.cmbPayheadType.Location = new System.Drawing.Point(140, 64);
            this.cmbPayheadType.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.cmbPayheadType.Name = "cmbPayheadType";
            this.cmbPayheadType.Size = new System.Drawing.Size(247, 21);
            this.cmbPayheadType.TabIndex = 1;
            this.cmbPayheadType.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbPayheadType_KeyDown);
            // 
            // lblPayheadTypeValidator
            // 
            this.lblPayheadTypeValidator.AutoSize = true;
            this.lblPayheadTypeValidator.ForeColor = System.Drawing.Color.Red;
            this.lblPayheadTypeValidator.Location = new System.Drawing.Point(389, 68);
            this.lblPayheadTypeValidator.Name = "lblPayheadTypeValidator";
            this.lblPayheadTypeValidator.Size = new System.Drawing.Size(11, 13);
            this.lblPayheadTypeValidator.TabIndex = 28;
            this.lblPayheadTypeValidator.Text = "*";
            // 
            // lblPayheadTNameValidator
            // 
            this.lblPayheadTNameValidator.AutoSize = true;
            this.lblPayheadTNameValidator.ForeColor = System.Drawing.Color.Red;
            this.lblPayheadTNameValidator.Location = new System.Drawing.Point(389, 43);
            this.lblPayheadTNameValidator.Name = "lblPayheadTNameValidator";
            this.lblPayheadTNameValidator.Size = new System.Drawing.Size(11, 13);
            this.lblPayheadTNameValidator.TabIndex = 29;
            this.lblPayheadTNameValidator.Text = "*";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnPayheadSave);
            this.groupBox1.Controls.Add(this.btnPayheadClear);
            this.groupBox1.Controls.Add(this.btnPayheadDelete);
            this.groupBox1.Controls.Add(this.btnPayheadClose);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(18, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(764, 150);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.lblPayheadSearch);
            this.groupBox2.Controls.Add(this.txtPayheadSearch);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.Maroon;
            this.groupBox2.Location = new System.Drawing.Point(19, 169);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(763, 370);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "      Search";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Black;
            this.groupBox3.ForeColor = System.Drawing.Color.Black;
            this.groupBox3.Location = new System.Drawing.Point(26, 18);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(70, 1);
            this.groupBox3.TabIndex = 28;
            this.groupBox3.TabStop = false;
            // 
            // frmPayHead
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(800, 552);
            this.Controls.Add(this.lblPayheadTNameValidator);
            this.Controls.Add(this.lblPayheadTypeValidator);
            this.Controls.Add(this.cmbPayheadType);
            this.Controls.Add(this.dgvPayhead);
            this.Controls.Add(this.txtPayheadNarration);
            this.Controls.Add(this.txtPayheadName);
            this.Controls.Add(this.lblPayheadNarration);
            this.Controls.Add(this.lblpayheadType);
            this.Controls.Add(this.lblPayheadName);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmPayHead";
            this.Opacity = 0.85D;
            this.Padding = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pay Head";
            this.Load += new System.EventHandler(this.frmPayHead_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPayHead_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayhead)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPayheadClose;
        private System.Windows.Forms.Button btnPayheadDelete;
        private System.Windows.Forms.Button btnPayheadClear;
        private System.Windows.Forms.Button btnPayheadSave;
        private System.Windows.Forms.TextBox txtPayheadSearch;
        private System.Windows.Forms.Label lblPayheadSearch;
        private System.Windows.Forms.TextBox txtPayheadNarration;
        private System.Windows.Forms.TextBox txtPayheadName;
        private System.Windows.Forms.Label lblPayheadNarration;
        private System.Windows.Forms.Label lblpayheadType;
        private System.Windows.Forms.Label lblPayheadName;
        private System.Windows.Forms.ComboBox cmbPayheadType;
        private System.Windows.Forms.DataGridView dgvPayhead;
        private System.Windows.Forms.Label lblPayheadTypeValidator;
        private System.Windows.Forms.Label lblPayheadTNameValidator;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtSerialNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtPayheadId;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtPayheadName;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}