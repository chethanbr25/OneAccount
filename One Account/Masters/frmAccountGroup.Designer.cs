namespace One_Account
{
    partial class frmAccountGroup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAccountGroup));
            this.btnSearch = new System.Windows.Forms.Button();
            this.cmbGroupUnderSearch = new System.Windows.Forms.ComboBox();
            this.lblGroupUnderSearch = new System.Windows.Forms.Label();
            this.txtAccountGroupNameSearch = new System.Windows.Forms.TextBox();
            this.lblAccountGroupNameSearch = new System.Windows.Forms.Label();
            this.dgvAccountGroup = new System.Windows.Forms.DataGridView();
            this.dgvtxtSlNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtxtAccountGroupId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtxtGroupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtxtUnderGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmbNature = new System.Windows.Forms.ComboBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtNarration = new System.Windows.Forms.TextBox();
            this.lblNarration = new System.Windows.Forms.Label();
            this.lblNature = new System.Windows.Forms.Label();
            this.txtAccountGroupName = new System.Windows.Forms.TextBox();
            this.lblAccountGroupName = new System.Windows.Forms.Label();
            this.cmbGroupUnder = new System.Windows.Forms.ComboBox();
            this.lblGroupUnder = new System.Windows.Forms.Label();
            this.cmbAffectGrossProfit = new System.Windows.Forms.ComboBox();
            this.lblAffectGrossProfit = new System.Windows.Forms.Label();
            this.gbxAccountGroupSearch = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.gbxAccountGroup = new System.Windows.Forms.GroupBox();
            this.lblGroupUnderMandatory = new System.Windows.Forms.Label();
            this.lblNatureMandatory = new System.Windows.Forms.Label();
            this.lblAccountNameMandatory = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccountGroup)).BeginInit();
            this.gbxAccountGroupSearch.SuspendLayout();
            this.gbxAccountGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.DimGray;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(672, 39);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(85, 21);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.btnSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnSearch_KeyDown);
            // 
            // cmbGroupUnderSearch
            // 
            this.cmbGroupUnderSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbGroupUnderSearch.FormattingEnabled = true;
            this.cmbGroupUnderSearch.Location = new System.Drawing.Point(427, 39);
            this.cmbGroupUnderSearch.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.cmbGroupUnderSearch.Name = "cmbGroupUnderSearch";
            this.cmbGroupUnderSearch.Size = new System.Drawing.Size(228, 21);
            this.cmbGroupUnderSearch.TabIndex = 1;
            this.cmbGroupUnderSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbGroupUnderSearch_KeyDown);
            this.cmbGroupUnderSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbGroupUnderSearch_KeyPress);
            this.cmbGroupUnderSearch.Leave += new System.EventHandler(this.cmbGroupUnderSearch_Leave);
            // 
            // lblGroupUnderSearch
            // 
            this.lblGroupUnderSearch.AutoSize = true;
            this.lblGroupUnderSearch.BackColor = System.Drawing.Color.Transparent;
            this.lblGroupUnderSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGroupUnderSearch.ForeColor = System.Drawing.Color.Black;
            this.lblGroupUnderSearch.Location = new System.Drawing.Point(382, 43);
            this.lblGroupUnderSearch.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lblGroupUnderSearch.Name = "lblGroupUnderSearch";
            this.lblGroupUnderSearch.Size = new System.Drawing.Size(42, 13);
            this.lblGroupUnderSearch.TabIndex = 122;
            this.lblGroupUnderSearch.Text = "Under :";
            // 
            // txtAccountGroupNameSearch
            // 
            this.txtAccountGroupNameSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAccountGroupNameSearch.Location = new System.Drawing.Point(57, 40);
            this.txtAccountGroupNameSearch.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.txtAccountGroupNameSearch.Name = "txtAccountGroupNameSearch";
            this.txtAccountGroupNameSearch.Size = new System.Drawing.Size(280, 20);
            this.txtAccountGroupNameSearch.TabIndex = 0;
            this.txtAccountGroupNameSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAccountGroupNameSearch_KeyDown);
            // 
            // lblAccountGroupNameSearch
            // 
            this.lblAccountGroupNameSearch.AutoSize = true;
            this.lblAccountGroupNameSearch.BackColor = System.Drawing.Color.Transparent;
            this.lblAccountGroupNameSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccountGroupNameSearch.ForeColor = System.Drawing.Color.Black;
            this.lblAccountGroupNameSearch.Location = new System.Drawing.Point(14, 43);
            this.lblAccountGroupNameSearch.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lblAccountGroupNameSearch.Name = "lblAccountGroupNameSearch";
            this.lblAccountGroupNameSearch.Size = new System.Drawing.Size(41, 13);
            this.lblAccountGroupNameSearch.TabIndex = 120;
            this.lblAccountGroupNameSearch.Text = "Name :";
            // 
            // dgvAccountGroup
            // 
            this.dgvAccountGroup.AllowUserToAddRows = false;
            this.dgvAccountGroup.AllowUserToDeleteRows = false;
            this.dgvAccountGroup.AllowUserToResizeColumns = false;
            this.dgvAccountGroup.AllowUserToResizeRows = false;
            this.dgvAccountGroup.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAccountGroup.BackgroundColor = System.Drawing.Color.White;
            this.dgvAccountGroup.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAccountGroup.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvAccountGroup.ColumnHeadersHeight = 25;
            this.dgvAccountGroup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvAccountGroup.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvtxtSlNo,
            this.dgvtxtAccountGroupId,
            this.dgvtxtGroupName,
            this.dgvtxtUnderGroup});
            this.dgvAccountGroup.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAccountGroup.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvAccountGroup.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.dgvAccountGroup.EnableHeadersVisualStyles = false;
            this.dgvAccountGroup.GridColor = System.Drawing.Color.DimGray;
            this.dgvAccountGroup.Location = new System.Drawing.Point(15, 79);
            this.dgvAccountGroup.Name = "dgvAccountGroup";
            this.dgvAccountGroup.ReadOnly = true;
            this.dgvAccountGroup.RowHeadersVisible = false;
            this.dgvAccountGroup.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAccountGroup.ShowCellToolTips = false;
            this.dgvAccountGroup.Size = new System.Drawing.Size(742, 355);
            this.dgvAccountGroup.TabIndex = 3;
            this.dgvAccountGroup.TabStop = false;
            this.dgvAccountGroup.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAccountGroup_CellDoubleClick);
            this.dgvAccountGroup.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvAccountGroup_DataBindingComplete);
            this.dgvAccountGroup.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvAccountGroup_KeyDown);
            this.dgvAccountGroup.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgvAccountGroup_KeyUp);
            // 
            // dgvtxtSlNo
            // 
            this.dgvtxtSlNo.DataPropertyName = "Sl No";
            this.dgvtxtSlNo.HeaderText = "Sl No";
            this.dgvtxtSlNo.Name = "dgvtxtSlNo";
            this.dgvtxtSlNo.ReadOnly = true;
            this.dgvtxtSlNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvtxtAccountGroupId
            // 
            this.dgvtxtAccountGroupId.DataPropertyName = "accountGroupId";
            this.dgvtxtAccountGroupId.HeaderText = "AccountGroupId";
            this.dgvtxtAccountGroupId.Name = "dgvtxtAccountGroupId";
            this.dgvtxtAccountGroupId.ReadOnly = true;
            this.dgvtxtAccountGroupId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgvtxtAccountGroupId.Visible = false;
            // 
            // dgvtxtGroupName
            // 
            this.dgvtxtGroupName.DataPropertyName = "accountGroupName";
            this.dgvtxtGroupName.HeaderText = "Group Name";
            this.dgvtxtGroupName.Name = "dgvtxtGroupName";
            this.dgvtxtGroupName.ReadOnly = true;
            this.dgvtxtGroupName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvtxtUnderGroup
            // 
            this.dgvtxtUnderGroup.DataPropertyName = "Under";
            this.dgvtxtUnderGroup.HeaderText = "Under";
            this.dgvtxtUnderGroup.Name = "dgvtxtUnderGroup";
            this.dgvtxtUnderGroup.ReadOnly = true;
            this.dgvtxtUnderGroup.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cmbNature
            // 
            this.cmbNature.FormattingEnabled = true;
            this.cmbNature.Items.AddRange(new object[] {
            "Assets",
            "Expenses",
            "Income",
            "Liabilities"});
            this.cmbNature.Location = new System.Drawing.Point(122, 42);
            this.cmbNature.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.cmbNature.Name = "cmbNature";
            this.cmbNature.Size = new System.Drawing.Size(200, 21);
            this.cmbNature.TabIndex = 2;
            this.cmbNature.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbNature_KeyDown);
            this.cmbNature.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbNature_KeyPress);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Location = new System.Drawing.Point(661, 91);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 27);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Salmon;
            this.btnDelete.Enabled = false;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Location = new System.Drawing.Point(570, 91);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(85, 27);
            this.btnDelete.TabIndex = 7;
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
            this.btnClear.Location = new System.Drawing.Point(479, 91);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(85, 27);
            this.btnClear.TabIndex = 6;
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
            this.btnSave.Location = new System.Drawing.Point(388, 91);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(85, 27);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnSave.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnSave_KeyDown);
            // 
            // txtNarration
            // 
            this.txtNarration.Location = new System.Drawing.Point(122, 68);
            this.txtNarration.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.txtNarration.MaxLength = 5000;
            this.txtNarration.Multiline = true;
            this.txtNarration.Name = "txtNarration";
            this.txtNarration.Size = new System.Drawing.Size(200, 50);
            this.txtNarration.TabIndex = 4;
            this.txtNarration.Enter += new System.EventHandler(this.txtNarration_Enter);
            this.txtNarration.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNarration_KeyDown);
            this.txtNarration.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNarration_KeyPress);
            // 
            // lblNarration
            // 
            this.lblNarration.AutoSize = true;
            this.lblNarration.BackColor = System.Drawing.Color.Transparent;
            this.lblNarration.ForeColor = System.Drawing.Color.Black;
            this.lblNarration.Location = new System.Drawing.Point(12, 68);
            this.lblNarration.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lblNarration.Name = "lblNarration";
            this.lblNarration.Size = new System.Drawing.Size(50, 13);
            this.lblNarration.TabIndex = 112;
            this.lblNarration.Text = "Narration";
            // 
            // lblNature
            // 
            this.lblNature.AutoSize = true;
            this.lblNature.BackColor = System.Drawing.Color.Transparent;
            this.lblNature.ForeColor = System.Drawing.Color.Black;
            this.lblNature.Location = new System.Drawing.Point(12, 46);
            this.lblNature.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lblNature.Name = "lblNature";
            this.lblNature.Size = new System.Drawing.Size(39, 13);
            this.lblNature.TabIndex = 111;
            this.lblNature.Text = "Nature";
            // 
            // txtAccountGroupName
            // 
            this.txtAccountGroupName.Location = new System.Drawing.Point(122, 17);
            this.txtAccountGroupName.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.txtAccountGroupName.Name = "txtAccountGroupName";
            this.txtAccountGroupName.Size = new System.Drawing.Size(200, 20);
            this.txtAccountGroupName.TabIndex = 0;
            this.txtAccountGroupName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAccountGroupName_KeyDown);
            // 
            // lblAccountGroupName
            // 
            this.lblAccountGroupName.AutoSize = true;
            this.lblAccountGroupName.BackColor = System.Drawing.Color.Transparent;
            this.lblAccountGroupName.ForeColor = System.Drawing.Color.Black;
            this.lblAccountGroupName.Location = new System.Drawing.Point(12, 21);
            this.lblAccountGroupName.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lblAccountGroupName.Name = "lblAccountGroupName";
            this.lblAccountGroupName.Size = new System.Drawing.Size(35, 13);
            this.lblAccountGroupName.TabIndex = 109;
            this.lblAccountGroupName.Text = "Name";
            // 
            // cmbGroupUnder
            // 
            this.cmbGroupUnder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGroupUnder.FormattingEnabled = true;
            this.cmbGroupUnder.Location = new System.Drawing.Point(546, 17);
            this.cmbGroupUnder.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.cmbGroupUnder.Name = "cmbGroupUnder";
            this.cmbGroupUnder.Size = new System.Drawing.Size(200, 21);
            this.cmbGroupUnder.TabIndex = 1;
            this.cmbGroupUnder.SelectedIndexChanged += new System.EventHandler(this.cmbGroupUnder_SelectedIndexChanged);
            this.cmbGroupUnder.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbGroupUnder_KeyDown);
            // 
            // lblGroupUnder
            // 
            this.lblGroupUnder.AutoSize = true;
            this.lblGroupUnder.BackColor = System.Drawing.Color.Transparent;
            this.lblGroupUnder.ForeColor = System.Drawing.Color.Black;
            this.lblGroupUnder.Location = new System.Drawing.Point(436, 21);
            this.lblGroupUnder.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lblGroupUnder.Name = "lblGroupUnder";
            this.lblGroupUnder.Size = new System.Drawing.Size(36, 13);
            this.lblGroupUnder.TabIndex = 125;
            this.lblGroupUnder.Text = "Under";
            // 
            // cmbAffectGrossProfit
            // 
            this.cmbAffectGrossProfit.FormattingEnabled = true;
            this.cmbAffectGrossProfit.Items.AddRange(new object[] {
            "Yes",
            "No"});
            this.cmbAffectGrossProfit.Location = new System.Drawing.Point(546, 42);
            this.cmbAffectGrossProfit.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.cmbAffectGrossProfit.Name = "cmbAffectGrossProfit";
            this.cmbAffectGrossProfit.Size = new System.Drawing.Size(200, 21);
            this.cmbAffectGrossProfit.TabIndex = 3;
            this.cmbAffectGrossProfit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbAffectGrossProfit_KeyDown);
            this.cmbAffectGrossProfit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbAffectGrossProfit_KeyPress);
            // 
            // lblAffectGrossProfit
            // 
            this.lblAffectGrossProfit.AutoSize = true;
            this.lblAffectGrossProfit.BackColor = System.Drawing.Color.Transparent;
            this.lblAffectGrossProfit.ForeColor = System.Drawing.Color.Black;
            this.lblAffectGrossProfit.Location = new System.Drawing.Point(436, 46);
            this.lblAffectGrossProfit.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lblAffectGrossProfit.Name = "lblAffectGrossProfit";
            this.lblAffectGrossProfit.Size = new System.Drawing.Size(92, 13);
            this.lblAffectGrossProfit.TabIndex = 127;
            this.lblAffectGrossProfit.Text = "Affect Gross Profit";
            // 
            // gbxAccountGroupSearch
            // 
            this.gbxAccountGroupSearch.BackColor = System.Drawing.Color.Transparent;
            this.gbxAccountGroupSearch.Controls.Add(this.groupBox2);
            this.gbxAccountGroupSearch.Controls.Add(this.lblSearch);
            this.gbxAccountGroupSearch.Controls.Add(this.dgvAccountGroup);
            this.gbxAccountGroupSearch.Controls.Add(this.cmbGroupUnderSearch);
            this.gbxAccountGroupSearch.Controls.Add(this.lblGroupUnderSearch);
            this.gbxAccountGroupSearch.Controls.Add(this.btnSearch);
            this.gbxAccountGroupSearch.Controls.Add(this.lblAccountGroupNameSearch);
            this.gbxAccountGroupSearch.Controls.Add(this.txtAccountGroupNameSearch);
            this.gbxAccountGroupSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxAccountGroupSearch.ForeColor = System.Drawing.Color.Black;
            this.gbxAccountGroupSearch.Location = new System.Drawing.Point(13, 156);
            this.gbxAccountGroupSearch.Name = "gbxAccountGroupSearch";
            this.gbxAccountGroupSearch.Size = new System.Drawing.Size(771, 440);
            this.gbxAccountGroupSearch.TabIndex = 129;
            this.gbxAccountGroupSearch.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Black;
            this.groupBox2.Location = new System.Drawing.Point(16, 26);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(75, 1);
            this.groupBox2.TabIndex = 124;
            this.groupBox2.TabStop = false;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearch.ForeColor = System.Drawing.Color.Maroon;
            this.lblSearch.Location = new System.Drawing.Point(13, 8);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(48, 15);
            this.lblSearch.TabIndex = 123;
            this.lblSearch.Text = "Search";
            // 
            // gbxAccountGroup
            // 
            this.gbxAccountGroup.Controls.Add(this.lblGroupUnderMandatory);
            this.gbxAccountGroup.Controls.Add(this.lblNatureMandatory);
            this.gbxAccountGroup.Controls.Add(this.lblAccountNameMandatory);
            this.gbxAccountGroup.Controls.Add(this.lblAccountGroupName);
            this.gbxAccountGroup.Controls.Add(this.txtAccountGroupName);
            this.gbxAccountGroup.Controls.Add(this.cmbAffectGrossProfit);
            this.gbxAccountGroup.Controls.Add(this.lblNature);
            this.gbxAccountGroup.Controls.Add(this.lblAffectGrossProfit);
            this.gbxAccountGroup.Controls.Add(this.lblNarration);
            this.gbxAccountGroup.Controls.Add(this.cmbGroupUnder);
            this.gbxAccountGroup.Controls.Add(this.txtNarration);
            this.gbxAccountGroup.Controls.Add(this.lblGroupUnder);
            this.gbxAccountGroup.Controls.Add(this.btnSave);
            this.gbxAccountGroup.Controls.Add(this.cmbNature);
            this.gbxAccountGroup.Controls.Add(this.btnClear);
            this.gbxAccountGroup.Controls.Add(this.btnClose);
            this.gbxAccountGroup.Controls.Add(this.btnDelete);
            this.gbxAccountGroup.Location = new System.Drawing.Point(13, 13);
            this.gbxAccountGroup.Name = "gbxAccountGroup";
            this.gbxAccountGroup.Size = new System.Drawing.Size(771, 138);
            this.gbxAccountGroup.TabIndex = 130;
            this.gbxAccountGroup.TabStop = false;
            // 
            // lblGroupUnderMandatory
            // 
            this.lblGroupUnderMandatory.AutoSize = true;
            this.lblGroupUnderMandatory.ForeColor = System.Drawing.Color.Red;
            this.lblGroupUnderMandatory.Location = new System.Drawing.Point(749, 21);
            this.lblGroupUnderMandatory.Name = "lblGroupUnderMandatory";
            this.lblGroupUnderMandatory.Size = new System.Drawing.Size(11, 13);
            this.lblGroupUnderMandatory.TabIndex = 128;
            this.lblGroupUnderMandatory.Text = "*";
            // 
            // lblNatureMandatory
            // 
            this.lblNatureMandatory.AutoSize = true;
            this.lblNatureMandatory.ForeColor = System.Drawing.Color.Red;
            this.lblNatureMandatory.Location = new System.Drawing.Point(326, 46);
            this.lblNatureMandatory.Name = "lblNatureMandatory";
            this.lblNatureMandatory.Size = new System.Drawing.Size(11, 13);
            this.lblNatureMandatory.TabIndex = 128;
            this.lblNatureMandatory.Text = "*";
            // 
            // lblAccountNameMandatory
            // 
            this.lblAccountNameMandatory.AutoSize = true;
            this.lblAccountNameMandatory.ForeColor = System.Drawing.Color.Red;
            this.lblAccountNameMandatory.Location = new System.Drawing.Point(326, 21);
            this.lblAccountNameMandatory.Name = "lblAccountNameMandatory";
            this.lblAccountNameMandatory.Size = new System.Drawing.Size(11, 13);
            this.lblAccountNameMandatory.TabIndex = 128;
            this.lblAccountNameMandatory.Text = "*";
            // 
            // frmAccountGroup
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(797, 608);
            this.Controls.Add(this.gbxAccountGroupSearch);
            this.Controls.Add(this.gbxAccountGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmAccountGroup";
            this.Opacity = 0.85D;
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Account Group";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAccountGroup_FormClosing);
            this.Load += new System.EventHandler(this.frmAccountGroup_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmAccountGroup_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccountGroup)).EndInit();
            this.gbxAccountGroupSearch.ResumeLayout(false);
            this.gbxAccountGroupSearch.PerformLayout();
            this.gbxAccountGroup.ResumeLayout(false);
            this.gbxAccountGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cmbGroupUnderSearch;
        private System.Windows.Forms.Label lblGroupUnderSearch;
        private System.Windows.Forms.TextBox txtAccountGroupNameSearch;
        private System.Windows.Forms.Label lblAccountGroupNameSearch;
        private System.Windows.Forms.DataGridView dgvAccountGroup;
        private System.Windows.Forms.ComboBox cmbNature;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtNarration;
        private System.Windows.Forms.Label lblNarration;
        private System.Windows.Forms.Label lblNature;
        private System.Windows.Forms.TextBox txtAccountGroupName;
        private System.Windows.Forms.Label lblAccountGroupName;
        private System.Windows.Forms.ComboBox cmbGroupUnder;
        private System.Windows.Forms.Label lblGroupUnder;
        private System.Windows.Forms.ComboBox cmbAffectGrossProfit;
        private System.Windows.Forms.Label lblAffectGrossProfit;
        private System.Windows.Forms.GroupBox gbxAccountGroupSearch;
        private System.Windows.Forms.GroupBox gbxAccountGroup;
        private System.Windows.Forms.Label lblGroupUnderMandatory;
        private System.Windows.Forms.Label lblNatureMandatory;
        private System.Windows.Forms.Label lblAccountNameMandatory;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtSlNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtAccountGroupId;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtGroupName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtUnderGroup;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblSearch;
    }
}