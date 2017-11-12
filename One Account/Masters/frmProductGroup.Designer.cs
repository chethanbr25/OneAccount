namespace One_Account
{
    partial class frmProductGroup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProductGroup));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblProductGroupValidator = new System.Windows.Forms.Label();
            this.cmbUnder = new System.Windows.Forms.ComboBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtNarration = new System.Windows.Forms.TextBox();
            this.lblNarration = new System.Windows.Forms.Label();
            this.lblUnder = new System.Windows.Forms.Label();
            this.txtProductGroupName = new System.Windows.Forms.TextBox();
            this.lblProductGroupName = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cmbUnderSearch = new System.Windows.Forms.ComboBox();
            this.lblUnderSearch = new System.Windows.Forms.Label();
            this.txtProductGroupSearch = new System.Windows.Forms.TextBox();
            this.lblProductGroupSearch = new System.Windows.Forms.Label();
            this.dgvProductGroup = new System.Windows.Forms.DataGridView();
            this.dgvtxtSlNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtxtgroupId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtxtName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtxtUnder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductGroup)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.lblProductGroupValidator);
            this.groupBox1.Controls.Add(this.cmbUnder);
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnClear);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.txtNarration);
            this.groupBox1.Controls.Add(this.lblNarration);
            this.groupBox1.Controls.Add(this.lblUnder);
            this.groupBox1.Controls.Add(this.txtProductGroupName);
            this.groupBox1.Controls.Add(this.lblProductGroupName);
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(18, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(764, 138);
            this.groupBox1.TabIndex = 1146;
            this.groupBox1.TabStop = false;
            // 
            // lblProductGroupValidator
            // 
            this.lblProductGroupValidator.AutoSize = true;
            this.lblProductGroupValidator.ForeColor = System.Drawing.Color.Red;
            this.lblProductGroupValidator.Location = new System.Drawing.Point(331, 23);
            this.lblProductGroupValidator.Name = "lblProductGroupValidator";
            this.lblProductGroupValidator.Size = new System.Drawing.Size(11, 13);
            this.lblProductGroupValidator.TabIndex = 118;
            this.lblProductGroupValidator.Text = "*";
            // 
            // cmbUnder
            // 
            this.cmbUnder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUnder.FormattingEnabled = true;
            this.cmbUnder.Location = new System.Drawing.Point(127, 43);
            this.cmbUnder.Margin = new System.Windows.Forms.Padding(5);
            this.cmbUnder.Name = "cmbUnder";
            this.cmbUnder.Size = new System.Drawing.Size(200, 21);
            this.cmbUnder.TabIndex = 1;
            this.cmbUnder.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbUnder_KeyDown);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Location = new System.Drawing.Point(658, 90);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 27);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Salmon;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Location = new System.Drawing.Point(567, 90);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(85, 27);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.ForeColor = System.Drawing.Color.Black;
            this.btnClear.Location = new System.Drawing.Point(476, 90);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(85, 27);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Location = new System.Drawing.Point(385, 90);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(85, 27);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnSave.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnSave_KeyDown);
            // 
            // txtNarration
            // 
            this.txtNarration.Location = new System.Drawing.Point(127, 68);
            this.txtNarration.Margin = new System.Windows.Forms.Padding(5);
            this.txtNarration.MaxLength = 5000;
            this.txtNarration.Multiline = true;
            this.txtNarration.Name = "txtNarration";
            this.txtNarration.Size = new System.Drawing.Size(200, 50);
            this.txtNarration.TabIndex = 2;
            this.txtNarration.Enter += new System.EventHandler(this.txtNarration_Enter);
            this.txtNarration.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNarration_KeyDown);
            // 
            // lblNarration
            // 
            this.lblNarration.AutoSize = true;
            this.lblNarration.ForeColor = System.Drawing.Color.Black;
            this.lblNarration.Location = new System.Drawing.Point(17, 68);
            this.lblNarration.Margin = new System.Windows.Forms.Padding(5);
            this.lblNarration.Name = "lblNarration";
            this.lblNarration.Size = new System.Drawing.Size(50, 13);
            this.lblNarration.TabIndex = 117;
            this.lblNarration.Text = "Narration";
            // 
            // lblUnder
            // 
            this.lblUnder.AutoSize = true;
            this.lblUnder.ForeColor = System.Drawing.Color.Black;
            this.lblUnder.Location = new System.Drawing.Point(17, 47);
            this.lblUnder.Margin = new System.Windows.Forms.Padding(5);
            this.lblUnder.Name = "lblUnder";
            this.lblUnder.Size = new System.Drawing.Size(36, 13);
            this.lblUnder.TabIndex = 116;
            this.lblUnder.Text = "Under";
            // 
            // txtProductGroupName
            // 
            this.txtProductGroupName.Location = new System.Drawing.Point(127, 19);
            this.txtProductGroupName.Margin = new System.Windows.Forms.Padding(5);
            this.txtProductGroupName.Name = "txtProductGroupName";
            this.txtProductGroupName.Size = new System.Drawing.Size(200, 20);
            this.txtProductGroupName.TabIndex = 0;
            this.txtProductGroupName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProductGroupName_KeyDown);
            // 
            // lblProductGroupName
            // 
            this.lblProductGroupName.AutoSize = true;
            this.lblProductGroupName.ForeColor = System.Drawing.Color.Black;
            this.lblProductGroupName.Location = new System.Drawing.Point(17, 23);
            this.lblProductGroupName.Margin = new System.Windows.Forms.Padding(5);
            this.lblProductGroupName.Name = "lblProductGroupName";
            this.lblProductGroupName.Size = new System.Drawing.Size(35, 13);
            this.lblProductGroupName.TabIndex = 115;
            this.lblProductGroupName.Text = "Name";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.btnSearch);
            this.groupBox2.Controls.Add(this.cmbUnderSearch);
            this.groupBox2.Controls.Add(this.lblUnderSearch);
            this.groupBox2.Controls.Add(this.txtProductGroupSearch);
            this.groupBox2.Controls.Add(this.lblProductGroupSearch);
            this.groupBox2.Controls.Add(this.dgvProductGroup);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.Maroon;
            this.groupBox2.Location = new System.Drawing.Point(18, 149);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(764, 387);
            this.groupBox2.TabIndex = 1147;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "  Search";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Black;
            this.groupBox3.Location = new System.Drawing.Point(15, 18);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(65, 1);
            this.groupBox3.TabIndex = 113;
            this.groupBox3.TabStop = false;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.DimGray;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(660, 25);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(85, 21);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.btnSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnSearch_KeyDown);
            // 
            // cmbUnderSearch
            // 
            this.cmbUnderSearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUnderSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbUnderSearch.ForeColor = System.Drawing.Color.Black;
            this.cmbUnderSearch.FormattingEnabled = true;
            this.cmbUnderSearch.Location = new System.Drawing.Point(440, 25);
            this.cmbUnderSearch.Margin = new System.Windows.Forms.Padding(5);
            this.cmbUnderSearch.Name = "cmbUnderSearch";
            this.cmbUnderSearch.Size = new System.Drawing.Size(200, 21);
            this.cmbUnderSearch.TabIndex = 1;
            this.cmbUnderSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbUnderSearch_KeyDown);
            // 
            // lblUnderSearch
            // 
            this.lblUnderSearch.AutoSize = true;
            this.lblUnderSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnderSearch.ForeColor = System.Drawing.Color.Black;
            this.lblUnderSearch.Location = new System.Drawing.Point(386, 28);
            this.lblUnderSearch.Margin = new System.Windows.Forms.Padding(5);
            this.lblUnderSearch.Name = "lblUnderSearch";
            this.lblUnderSearch.Size = new System.Drawing.Size(42, 13);
            this.lblUnderSearch.TabIndex = 112;
            this.lblUnderSearch.Text = "Under  ";
            // 
            // txtProductGroupSearch
            // 
            this.txtProductGroupSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProductGroupSearch.ForeColor = System.Drawing.Color.Black;
            this.txtProductGroupSearch.Location = new System.Drawing.Point(127, 25);
            this.txtProductGroupSearch.Margin = new System.Windows.Forms.Padding(5);
            this.txtProductGroupSearch.Name = "txtProductGroupSearch";
            this.txtProductGroupSearch.Size = new System.Drawing.Size(200, 20);
            this.txtProductGroupSearch.TabIndex = 0;
            this.txtProductGroupSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProductGroupSearch_KeyDown);
            // 
            // lblProductGroupSearch
            // 
            this.lblProductGroupSearch.AutoSize = true;
            this.lblProductGroupSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductGroupSearch.ForeColor = System.Drawing.Color.Black;
            this.lblProductGroupSearch.Location = new System.Drawing.Point(17, 28);
            this.lblProductGroupSearch.Margin = new System.Windows.Forms.Padding(5);
            this.lblProductGroupSearch.Name = "lblProductGroupSearch";
            this.lblProductGroupSearch.Size = new System.Drawing.Size(41, 13);
            this.lblProductGroupSearch.TabIndex = 111;
            this.lblProductGroupSearch.Text = "Name  ";
            // 
            // dgvProductGroup
            // 
            this.dgvProductGroup.AllowUserToAddRows = false;
            this.dgvProductGroup.AllowUserToDeleteRows = false;
            this.dgvProductGroup.AllowUserToResizeColumns = false;
            this.dgvProductGroup.AllowUserToResizeRows = false;
            this.dgvProductGroup.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProductGroup.BackgroundColor = System.Drawing.Color.White;
            this.dgvProductGroup.CausesValidation = false;
            this.dgvProductGroup.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvProductGroup.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvProductGroup.ColumnHeadersHeight = 25;
            this.dgvProductGroup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvProductGroup.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvtxtSlNo,
            this.dgvtxtgroupId,
            this.dgvtxtName,
            this.dgvtxtUnder});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Maroon;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvProductGroup.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvProductGroup.EnableHeadersVisualStyles = false;
            this.dgvProductGroup.GridColor = System.Drawing.Color.DimGray;
            this.dgvProductGroup.Location = new System.Drawing.Point(16, 56);
            this.dgvProductGroup.MultiSelect = false;
            this.dgvProductGroup.Name = "dgvProductGroup";
            this.dgvProductGroup.RowHeadersVisible = false;
            this.dgvProductGroup.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProductGroup.Size = new System.Drawing.Size(729, 313);
            this.dgvProductGroup.TabIndex = 110;
            this.dgvProductGroup.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProductGroup_CellDoubleClick);
            this.dgvProductGroup.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvProductGroup_KeyDown);
            this.dgvProductGroup.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgvProductGroup_KeyUp);
            // 
            // dgvtxtSlNo
            // 
            this.dgvtxtSlNo.DataPropertyName = "SL.NO";
            this.dgvtxtSlNo.HeaderText = "Sl No";
            this.dgvtxtSlNo.Name = "dgvtxtSlNo";
            this.dgvtxtSlNo.ReadOnly = true;
            this.dgvtxtSlNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvtxtgroupId
            // 
            this.dgvtxtgroupId.DataPropertyName = "groupId";
            this.dgvtxtgroupId.HeaderText = "groupId";
            this.dgvtxtgroupId.Name = "dgvtxtgroupId";
            this.dgvtxtgroupId.ReadOnly = true;
            this.dgvtxtgroupId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgvtxtgroupId.Visible = false;
            // 
            // dgvtxtName
            // 
            this.dgvtxtName.DataPropertyName = "groupName";
            this.dgvtxtName.HeaderText = "Name";
            this.dgvtxtName.Name = "dgvtxtName";
            this.dgvtxtName.ReadOnly = true;
            this.dgvtxtName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvtxtUnder
            // 
            this.dgvtxtUnder.DataPropertyName = "Under";
            this.dgvtxtUnder.HeaderText = "Under";
            this.dgvtxtUnder.Name = "dgvtxtUnder";
            this.dgvtxtUnder.ReadOnly = true;
            this.dgvtxtUnder.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvtxtUnder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // frmProductGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(800, 551);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmProductGroup";
            this.Opacity = 0.85D;
            this.Padding = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Product Group";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmProductGroup_FormClosing);
            this.Load += new System.EventHandler(this.frmProductGroup_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmProductGroup_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductGroup)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblProductGroupValidator;
        private System.Windows.Forms.ComboBox cmbUnder;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtNarration;
        private System.Windows.Forms.Label lblNarration;
        private System.Windows.Forms.Label lblUnder;
        private System.Windows.Forms.TextBox txtProductGroupName;
        private System.Windows.Forms.Label lblProductGroupName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cmbUnderSearch;
        private System.Windows.Forms.Label lblUnderSearch;
        private System.Windows.Forms.TextBox txtProductGroupSearch;
        private System.Windows.Forms.Label lblProductGroupSearch;
        private System.Windows.Forms.DataGridView dgvProductGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtSlNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtgroupId;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtUnder;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}