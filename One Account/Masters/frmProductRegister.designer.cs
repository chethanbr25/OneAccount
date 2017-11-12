namespace One_Account
{
    partial class frmProductRegister
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProductRegister));
            this.lblModelNo = new System.Windows.Forms.Label();
            this.txtProductCode = new System.Windows.Forms.TextBox();
            this.lblProductCode = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.dgvProductRegister = new System.Windows.Forms.DataGridView();
            this.dgvtxtSlNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtxtproductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtxtCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtxtName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtxtBrand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtxtSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtxtModalNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtxtSalesRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmbProductGroup = new System.Windows.Forms.ComboBox();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.lblProductName = new System.Windows.Forms.Label();
            this.cmbSize = new System.Windows.Forms.ComboBox();
            this.lblSize = new System.Windows.Forms.Label();
            this.cmbModelNo = new System.Windows.Forms.ComboBox();
            this.cmbBrand = new System.Windows.Forms.ComboBox();
            this.lblBrand = new System.Windows.Forms.Label();
            this.cmbTaxApplicableOn = new System.Windows.Forms.ComboBox();
            this.lblTaxApplicationon = new System.Windows.Forms.Label();
            this.cmbTax = new System.Windows.Forms.ComboBox();
            this.lblTax = new System.Windows.Forms.Label();
            this.txtSalesRateFrom = new System.Windows.Forms.TextBox();
            this.lblSalesRateForm = new System.Windows.Forms.Label();
            this.txtSalesRateTo = new System.Windows.Forms.TextBox();
            this.lblSalesRateTo = new System.Windows.Forms.Label();
            this.cmbActive = new System.Windows.Forms.ComboBox();
            this.lblActive = new System.Windows.Forms.Label();
            this.btnViewDetails = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductRegister)).BeginInit();
            this.SuspendLayout();
            // 
            // lblModelNo
            // 
            this.lblModelNo.AutoSize = true;
            this.lblModelNo.BackColor = System.Drawing.Color.Transparent;
            this.lblModelNo.ForeColor = System.Drawing.Color.Black;
            this.lblModelNo.Location = new System.Drawing.Point(18, 74);
            this.lblModelNo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblModelNo.Name = "lblModelNo";
            this.lblModelNo.Size = new System.Drawing.Size(56, 13);
            this.lblModelNo.TabIndex = 85;
            this.lblModelNo.Text = "Model No.";
            // 
            // txtProductCode
            // 
            this.txtProductCode.Location = new System.Drawing.Point(129, 45);
            this.txtProductCode.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.txtProductCode.Name = "txtProductCode";
            this.txtProductCode.Size = new System.Drawing.Size(200, 20);
            this.txtProductCode.TabIndex = 2;
            this.txtProductCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProductCode_KeyDown);
            // 
            // lblProductCode
            // 
            this.lblProductCode.AutoSize = true;
            this.lblProductCode.BackColor = System.Drawing.Color.Transparent;
            this.lblProductCode.ForeColor = System.Drawing.Color.Black;
            this.lblProductCode.Location = new System.Drawing.Point(18, 49);
            this.lblProductCode.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblProductCode.Name = "lblProductCode";
            this.lblProductCode.Size = new System.Drawing.Size(72, 13);
            this.lblProductCode.TabIndex = 83;
            this.lblProductCode.Text = "Product Code";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.BackColor = System.Drawing.Color.Transparent;
            this.lblName.ForeColor = System.Drawing.Color.Black;
            this.lblName.Location = new System.Drawing.Point(18, 23);
            this.lblName.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(76, 13);
            this.lblName.TabIndex = 73;
            this.lblName.Text = "Product Group";
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.DimGray;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(606, 147);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(85, 21);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.btnSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnSearch_KeyDown);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.DimGray;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(697, 147);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(85, 21);
            this.btnClear.TabIndex = 12;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            this.btnClear.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnClear_KeyDown);
            // 
            // dgvProductRegister
            // 
            this.dgvProductRegister.AllowUserToAddRows = false;
            this.dgvProductRegister.AllowUserToResizeColumns = false;
            this.dgvProductRegister.AllowUserToResizeRows = false;
            this.dgvProductRegister.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProductRegister.BackgroundColor = System.Drawing.Color.White;
            this.dgvProductRegister.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvProductRegister.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvProductRegister.ColumnHeadersHeight = 25;
            this.dgvProductRegister.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvProductRegister.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvtxtSlNo,
            this.dgvtxtproductId,
            this.dgvtxtCode,
            this.dgvtxtName,
            this.dgvtxtBrand,
            this.dgvtxtSize,
            this.dgvtxtModalNo,
            this.dgvtxtSalesRate});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvProductRegister.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvProductRegister.EnableHeadersVisualStyles = false;
            this.dgvProductRegister.GridColor = System.Drawing.Color.DimGray;
            this.dgvProductRegister.Location = new System.Drawing.Point(18, 179);
            this.dgvProductRegister.Name = "dgvProductRegister";
            this.dgvProductRegister.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dgvProductRegister.RowHeadersVisible = false;
            this.dgvProductRegister.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProductRegister.Size = new System.Drawing.Size(764, 350);
            this.dgvProductRegister.TabIndex = 13;
            this.dgvProductRegister.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProductRegister_CellDoubleClick);
            this.dgvProductRegister.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvProductRegister_KeyDown);
            // 
            // dgvtxtSlNo
            // 
            this.dgvtxtSlNo.DataPropertyName = "SL.NO";
            this.dgvtxtSlNo.HeaderText = "Sl No";
            this.dgvtxtSlNo.Name = "dgvtxtSlNo";
            this.dgvtxtSlNo.ReadOnly = true;
            this.dgvtxtSlNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvtxtproductId
            // 
            this.dgvtxtproductId.DataPropertyName = "productId";
            this.dgvtxtproductId.HeaderText = "Product Id";
            this.dgvtxtproductId.Name = "dgvtxtproductId";
            this.dgvtxtproductId.ReadOnly = true;
            this.dgvtxtproductId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgvtxtproductId.Visible = false;
            // 
            // dgvtxtCode
            // 
            this.dgvtxtCode.DataPropertyName = "productCode";
            this.dgvtxtCode.HeaderText = "Code";
            this.dgvtxtCode.Name = "dgvtxtCode";
            this.dgvtxtCode.ReadOnly = true;
            this.dgvtxtCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvtxtName
            // 
            this.dgvtxtName.DataPropertyName = "productName";
            this.dgvtxtName.HeaderText = "Name";
            this.dgvtxtName.Name = "dgvtxtName";
            this.dgvtxtName.ReadOnly = true;
            this.dgvtxtName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvtxtBrand
            // 
            this.dgvtxtBrand.DataPropertyName = "brandName";
            this.dgvtxtBrand.HeaderText = "Brand";
            this.dgvtxtBrand.Name = "dgvtxtBrand";
            this.dgvtxtBrand.ReadOnly = true;
            this.dgvtxtBrand.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvtxtSize
            // 
            this.dgvtxtSize.DataPropertyName = "size";
            this.dgvtxtSize.HeaderText = "Size";
            this.dgvtxtSize.Name = "dgvtxtSize";
            this.dgvtxtSize.ReadOnly = true;
            this.dgvtxtSize.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvtxtModalNo
            // 
            this.dgvtxtModalNo.DataPropertyName = "modelNo";
            this.dgvtxtModalNo.HeaderText = "Model No.";
            this.dgvtxtModalNo.Name = "dgvtxtModalNo";
            this.dgvtxtModalNo.ReadOnly = true;
            this.dgvtxtModalNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvtxtSalesRate
            // 
            this.dgvtxtSalesRate.DataPropertyName = "salesRate";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dgvtxtSalesRate.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvtxtSalesRate.HeaderText = "Sales Rate";
            this.dgvtxtSalesRate.Name = "dgvtxtSalesRate";
            this.dgvtxtSalesRate.ReadOnly = true;
            this.dgvtxtSalesRate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cmbProductGroup
            // 
            this.cmbProductGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProductGroup.FormattingEnabled = true;
            this.cmbProductGroup.Location = new System.Drawing.Point(129, 19);
            this.cmbProductGroup.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.cmbProductGroup.Name = "cmbProductGroup";
            this.cmbProductGroup.Size = new System.Drawing.Size(200, 21);
            this.cmbProductGroup.TabIndex = 0;
            this.cmbProductGroup.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbProductGroup_KeyDown);
            // 
            // txtProductName
            // 
            this.txtProductName.Location = new System.Drawing.Point(582, 19);
            this.txtProductName.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new System.Drawing.Size(200, 20);
            this.txtProductName.TabIndex = 1;
            this.txtProductName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProductName_KeyDown);
            // 
            // lblProductName
            // 
            this.lblProductName.AutoSize = true;
            this.lblProductName.BackColor = System.Drawing.Color.Transparent;
            this.lblProductName.ForeColor = System.Drawing.Color.Black;
            this.lblProductName.Location = new System.Drawing.Point(474, 23);
            this.lblProductName.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(75, 13);
            this.lblProductName.TabIndex = 91;
            this.lblProductName.Text = "Product Name";
            // 
            // cmbSize
            // 
            this.cmbSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSize.FormattingEnabled = true;
            this.cmbSize.Location = new System.Drawing.Point(582, 45);
            this.cmbSize.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.cmbSize.Name = "cmbSize";
            this.cmbSize.Size = new System.Drawing.Size(200, 21);
            this.cmbSize.TabIndex = 3;
            this.cmbSize.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbSize_KeyDown);
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.BackColor = System.Drawing.Color.Transparent;
            this.lblSize.ForeColor = System.Drawing.Color.Black;
            this.lblSize.Location = new System.Drawing.Point(474, 49);
            this.lblSize.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(27, 13);
            this.lblSize.TabIndex = 93;
            this.lblSize.Text = "Size";
            // 
            // cmbModelNo
            // 
            this.cmbModelNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbModelNo.FormattingEnabled = true;
            this.cmbModelNo.Location = new System.Drawing.Point(129, 70);
            this.cmbModelNo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.cmbModelNo.Name = "cmbModelNo";
            this.cmbModelNo.Size = new System.Drawing.Size(200, 21);
            this.cmbModelNo.TabIndex = 4;
            this.cmbModelNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbModelNo_KeyDown);
            // 
            // cmbBrand
            // 
            this.cmbBrand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBrand.FormattingEnabled = true;
            this.cmbBrand.Location = new System.Drawing.Point(582, 70);
            this.cmbBrand.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.cmbBrand.Name = "cmbBrand";
            this.cmbBrand.Size = new System.Drawing.Size(200, 21);
            this.cmbBrand.TabIndex = 5;
            this.cmbBrand.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbBrand_KeyDown);
            // 
            // lblBrand
            // 
            this.lblBrand.AutoSize = true;
            this.lblBrand.BackColor = System.Drawing.Color.Transparent;
            this.lblBrand.ForeColor = System.Drawing.Color.Black;
            this.lblBrand.Location = new System.Drawing.Point(474, 74);
            this.lblBrand.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblBrand.Name = "lblBrand";
            this.lblBrand.Size = new System.Drawing.Size(35, 13);
            this.lblBrand.TabIndex = 96;
            this.lblBrand.Text = "Brand";
            // 
            // cmbTaxApplicableOn
            // 
            this.cmbTaxApplicableOn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTaxApplicableOn.FormattingEnabled = true;
            this.cmbTaxApplicableOn.Items.AddRange(new object[] {
            "All",
            "MRP",
            "SalesRate",
            "PurchaseRate"});
            this.cmbTaxApplicableOn.Location = new System.Drawing.Point(582, 96);
            this.cmbTaxApplicableOn.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.cmbTaxApplicableOn.Name = "cmbTaxApplicableOn";
            this.cmbTaxApplicableOn.Size = new System.Drawing.Size(200, 21);
            this.cmbTaxApplicableOn.TabIndex = 7;
            this.cmbTaxApplicableOn.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbTaxApplicationon_KeyDown);
            // 
            // lblTaxApplicationon
            // 
            this.lblTaxApplicationon.AutoSize = true;
            this.lblTaxApplicationon.BackColor = System.Drawing.Color.Transparent;
            this.lblTaxApplicationon.ForeColor = System.Drawing.Color.Black;
            this.lblTaxApplicationon.Location = new System.Drawing.Point(474, 100);
            this.lblTaxApplicationon.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblTaxApplicationon.Name = "lblTaxApplicationon";
            this.lblTaxApplicationon.Size = new System.Drawing.Size(92, 13);
            this.lblTaxApplicationon.TabIndex = 100;
            this.lblTaxApplicationon.Text = "Tax Applicable on";
            // 
            // cmbTax
            // 
            this.cmbTax.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTax.FormattingEnabled = true;
            this.cmbTax.Location = new System.Drawing.Point(129, 96);
            this.cmbTax.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.cmbTax.Name = "cmbTax";
            this.cmbTax.Size = new System.Drawing.Size(200, 21);
            this.cmbTax.TabIndex = 6;
            this.cmbTax.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbTax_KeyDown);
            // 
            // lblTax
            // 
            this.lblTax.AutoSize = true;
            this.lblTax.BackColor = System.Drawing.Color.Transparent;
            this.lblTax.ForeColor = System.Drawing.Color.Black;
            this.lblTax.Location = new System.Drawing.Point(18, 100);
            this.lblTax.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblTax.Name = "lblTax";
            this.lblTax.Size = new System.Drawing.Size(25, 13);
            this.lblTax.TabIndex = 98;
            this.lblTax.Text = "Tax";
            // 
            // txtSalesRateFrom
            // 
            this.txtSalesRateFrom.Location = new System.Drawing.Point(129, 122);
            this.txtSalesRateFrom.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.txtSalesRateFrom.MaxLength = 13;
            this.txtSalesRateFrom.Name = "txtSalesRateFrom";
            this.txtSalesRateFrom.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtSalesRateFrom.Size = new System.Drawing.Size(200, 20);
            this.txtSalesRateFrom.TabIndex = 8;
            this.txtSalesRateFrom.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSalesRateForm_KeyDown);
            this.txtSalesRateFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSalesRateForm_KeyPress);
            // 
            // lblSalesRateForm
            // 
            this.lblSalesRateForm.AutoSize = true;
            this.lblSalesRateForm.BackColor = System.Drawing.Color.Transparent;
            this.lblSalesRateForm.ForeColor = System.Drawing.Color.Black;
            this.lblSalesRateForm.Location = new System.Drawing.Point(18, 126);
            this.lblSalesRateForm.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblSalesRateForm.Name = "lblSalesRateForm";
            this.lblSalesRateForm.Size = new System.Drawing.Size(85, 13);
            this.lblSalesRateForm.TabIndex = 102;
            this.lblSalesRateForm.Text = "Sales Rate From";
            // 
            // txtSalesRateTo
            // 
            this.txtSalesRateTo.Location = new System.Drawing.Point(582, 122);
            this.txtSalesRateTo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.txtSalesRateTo.MaxLength = 13;
            this.txtSalesRateTo.Name = "txtSalesRateTo";
            this.txtSalesRateTo.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtSalesRateTo.Size = new System.Drawing.Size(200, 20);
            this.txtSalesRateTo.TabIndex = 9;
            this.txtSalesRateTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSalesRateTo_KeyDown);
            this.txtSalesRateTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSalesRateTo_KeyPress);
            // 
            // lblSalesRateTo
            // 
            this.lblSalesRateTo.AutoSize = true;
            this.lblSalesRateTo.BackColor = System.Drawing.Color.Transparent;
            this.lblSalesRateTo.ForeColor = System.Drawing.Color.Black;
            this.lblSalesRateTo.Location = new System.Drawing.Point(474, 126);
            this.lblSalesRateTo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblSalesRateTo.Name = "lblSalesRateTo";
            this.lblSalesRateTo.Size = new System.Drawing.Size(75, 13);
            this.lblSalesRateTo.TabIndex = 104;
            this.lblSalesRateTo.Text = "Sales Rate To";
            // 
            // cmbActive
            // 
            this.cmbActive.DisplayMember = "All";
            this.cmbActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbActive.FormattingEnabled = true;
            this.cmbActive.Items.AddRange(new object[] {
            "All",
            "Yes",
            "No"});
            this.cmbActive.Location = new System.Drawing.Point(129, 147);
            this.cmbActive.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.cmbActive.Name = "cmbActive";
            this.cmbActive.Size = new System.Drawing.Size(200, 21);
            this.cmbActive.TabIndex = 10;
            this.cmbActive.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbStatus_KeyDown);
            // 
            // lblActive
            // 
            this.lblActive.AutoSize = true;
            this.lblActive.BackColor = System.Drawing.Color.Transparent;
            this.lblActive.ForeColor = System.Drawing.Color.Black;
            this.lblActive.Location = new System.Drawing.Point(18, 151);
            this.lblActive.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblActive.Name = "lblActive";
            this.lblActive.Size = new System.Drawing.Size(37, 13);
            this.lblActive.TabIndex = 106;
            this.lblActive.Text = "Active";
            // 
            // btnViewDetails
            // 
            this.btnViewDetails.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnViewDetails.FlatAppearance.BorderSize = 0;
            this.btnViewDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewDetails.ForeColor = System.Drawing.Color.Black;
            this.btnViewDetails.Location = new System.Drawing.Point(606, 535);
            this.btnViewDetails.Name = "btnViewDetails";
            this.btnViewDetails.Size = new System.Drawing.Size(85, 27);
            this.btnViewDetails.TabIndex = 14;
            this.btnViewDetails.Text = "View Details";
            this.btnViewDetails.UseVisualStyleBackColor = false;
            this.btnViewDetails.Click += new System.EventHandler(this.btnViewDetails_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Location = new System.Drawing.Point(697, 535);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 27);
            this.btnClose.TabIndex = 15;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmProductRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(800, 570);
            this.Controls.Add(this.btnViewDetails);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.cmbActive);
            this.Controls.Add(this.lblActive);
            this.Controls.Add(this.txtSalesRateTo);
            this.Controls.Add(this.lblSalesRateTo);
            this.Controls.Add(this.txtSalesRateFrom);
            this.Controls.Add(this.lblSalesRateForm);
            this.Controls.Add(this.cmbTaxApplicableOn);
            this.Controls.Add(this.lblTaxApplicationon);
            this.Controls.Add(this.cmbTax);
            this.Controls.Add(this.lblTax);
            this.Controls.Add(this.cmbBrand);
            this.Controls.Add(this.lblBrand);
            this.Controls.Add(this.cmbModelNo);
            this.Controls.Add(this.cmbSize);
            this.Controls.Add(this.lblSize);
            this.Controls.Add(this.txtProductName);
            this.Controls.Add(this.lblProductName);
            this.Controls.Add(this.cmbProductGroup);
            this.Controls.Add(this.lblModelNo);
            this.Controls.Add(this.txtProductCode);
            this.Controls.Add(this.lblProductCode);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.dgvProductRegister);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmProductRegister";
            this.Opacity = 0.85D;
            this.Padding = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Product Register";
            this.Load += new System.EventHandler(this.frmProductRegister_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmProductRegister_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductRegister)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblModelNo;
        private System.Windows.Forms.TextBox txtProductCode;
        private System.Windows.Forms.Label lblProductCode;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ComboBox cmbProductGroup;
        private System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.ComboBox cmbSize;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.ComboBox cmbModelNo;
        private System.Windows.Forms.ComboBox cmbBrand;
        private System.Windows.Forms.Label lblBrand;
        private System.Windows.Forms.ComboBox cmbTaxApplicableOn;
        private System.Windows.Forms.Label lblTaxApplicationon;
        private System.Windows.Forms.ComboBox cmbTax;
        private System.Windows.Forms.Label lblTax;
        private System.Windows.Forms.TextBox txtSalesRateFrom;
        private System.Windows.Forms.Label lblSalesRateForm;
        private System.Windows.Forms.TextBox txtSalesRateTo;
        private System.Windows.Forms.Label lblSalesRateTo;
        private System.Windows.Forms.ComboBox cmbActive;
        private System.Windows.Forms.Label lblActive;
        private System.Windows.Forms.Button btnViewDetails;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgvProductRegister;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtSlNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtproductId;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtBrand;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtModalNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtSalesRate;
    }
}