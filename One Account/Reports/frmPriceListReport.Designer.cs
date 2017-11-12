namespace One_Account
{
    partial class frmPriceListReport
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPriceListReport));
            this.dgvPriceList = new System.Windows.Forms.DataGridView();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.cmbProduct = new System.Windows.Forms.ComboBox();
            this.lblProduct = new System.Windows.Forms.Label();
            this.cmbProductGroup = new System.Windows.Forms.ComboBox();
            this.lblProductGroup = new System.Windows.Forms.Label();
            this.cmbModel = new System.Windows.Forms.ComboBox();
            this.lblModelNo = new System.Windows.Forms.Label();
            this.cmbSize = new System.Windows.Forms.ComboBox();
            this.lblSize = new System.Windows.Forms.Label();
            this.cmbPricingLevel = new System.Windows.Forms.ComboBox();
            this.lblPrizingLevel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbxMrp = new System.Windows.Forms.CheckBox();
            this.CbxStandardRate = new System.Windows.Forms.CheckBox();
            this.cbxSalesRate = new System.Windows.Forms.CheckBox();
            this.cbxlastSalesRate = new System.Windows.Forms.CheckBox();
            this.cbxPurchaseRate = new System.Windows.Forms.CheckBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.dgvtxtSerialNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numbr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PurchaseRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SalesRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastSalesRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StandardRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MRP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPriceList)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvPriceList
            // 
            this.dgvPriceList.AllowUserToAddRows = false;
            this.dgvPriceList.AllowUserToDeleteRows = false;
            this.dgvPriceList.AllowUserToResizeRows = false;
            this.dgvPriceList.BackgroundColor = System.Drawing.Color.White;
            this.dgvPriceList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPriceList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPriceList.ColumnHeadersHeight = 35;
            this.dgvPriceList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPriceList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvtxtSerialNo,
            this.ProductId,
            this.numbr,
            this.Column1,
            this.Column2,
            this.PurchaseRate,
            this.SalesRate,
            this.LastSalesRate,
            this.StandardRate,
            this.MRP,
            this.Column8});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPriceList.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvPriceList.EnableHeadersVisualStyles = false;
            this.dgvPriceList.GridColor = System.Drawing.Color.DimGray;
            this.dgvPriceList.Location = new System.Drawing.Point(18, 204);
            this.dgvPriceList.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.dgvPriceList.Name = "dgvPriceList";
            this.dgvPriceList.ReadOnly = true;
            this.dgvPriceList.RowHeadersVisible = false;
            this.dgvPriceList.Size = new System.Drawing.Size(753, 315);
            this.dgvPriceList.TabIndex = 1222;
            this.dgvPriceList.TabStop = false;
            this.dgvPriceList.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvPriceList_RowsAdded);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.DimGray;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(595, 171);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(85, 22);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.btnSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnSearch_KeyDown);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.DimGray;
            this.btnReset.FlatAppearance.BorderSize = 0;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Location = new System.Drawing.Point(686, 171);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(85, 22);
            this.btnReset.TabIndex = 7;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // cmbProduct
            // 
            this.cmbProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProduct.FormattingEnabled = true;
            this.cmbProduct.Location = new System.Drawing.Point(113, 15);
            this.cmbProduct.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.cmbProduct.Name = "cmbProduct";
            this.cmbProduct.Size = new System.Drawing.Size(200, 21);
            this.cmbProduct.TabIndex = 0;
            this.cmbProduct.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbProduct_KeyDown);
            // 
            // lblProduct
            // 
            this.lblProduct.AutoSize = true;
            this.lblProduct.BackColor = System.Drawing.Color.Transparent;
            this.lblProduct.ForeColor = System.Drawing.Color.Black;
            this.lblProduct.Location = new System.Drawing.Point(18, 19);
            this.lblProduct.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(44, 13);
            this.lblProduct.TabIndex = 1217;
            this.lblProduct.Text = "Product";
            // 
            // cmbProductGroup
            // 
            this.cmbProductGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProductGroup.FormattingEnabled = true;
            this.cmbProductGroup.Location = new System.Drawing.Point(571, 15);
            this.cmbProductGroup.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.cmbProductGroup.Name = "cmbProductGroup";
            this.cmbProductGroup.Size = new System.Drawing.Size(200, 21);
            this.cmbProductGroup.TabIndex = 1;
            this.cmbProductGroup.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbProductGroup_KeyDown);
            // 
            // lblProductGroup
            // 
            this.lblProductGroup.AutoSize = true;
            this.lblProductGroup.BackColor = System.Drawing.Color.Transparent;
            this.lblProductGroup.ForeColor = System.Drawing.Color.Black;
            this.lblProductGroup.Location = new System.Drawing.Point(463, 19);
            this.lblProductGroup.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lblProductGroup.Name = "lblProductGroup";
            this.lblProductGroup.Size = new System.Drawing.Size(76, 13);
            this.lblProductGroup.TabIndex = 1215;
            this.lblProductGroup.Text = "Product Group";
            // 
            // cmbModel
            // 
            this.cmbModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbModel.FormattingEnabled = true;
            this.cmbModel.Location = new System.Drawing.Point(113, 41);
            this.cmbModel.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.cmbModel.Name = "cmbModel";
            this.cmbModel.Size = new System.Drawing.Size(200, 21);
            this.cmbModel.TabIndex = 2;
            this.cmbModel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbModel_KeyDown);
            // 
            // lblModelNo
            // 
            this.lblModelNo.AutoSize = true;
            this.lblModelNo.BackColor = System.Drawing.Color.Transparent;
            this.lblModelNo.ForeColor = System.Drawing.Color.Black;
            this.lblModelNo.Location = new System.Drawing.Point(18, 45);
            this.lblModelNo.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lblModelNo.Name = "lblModelNo";
            this.lblModelNo.Size = new System.Drawing.Size(53, 13);
            this.lblModelNo.TabIndex = 1223;
            this.lblModelNo.Text = "Model No";
            // 
            // cmbSize
            // 
            this.cmbSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSize.FormattingEnabled = true;
            this.cmbSize.Location = new System.Drawing.Point(571, 41);
            this.cmbSize.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
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
            this.lblSize.Location = new System.Drawing.Point(463, 45);
            this.lblSize.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(27, 13);
            this.lblSize.TabIndex = 1225;
            this.lblSize.Text = "Size";
            // 
            // cmbPricingLevel
            // 
            this.cmbPricingLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPricingLevel.FormattingEnabled = true;
            this.cmbPricingLevel.Location = new System.Drawing.Point(113, 67);
            this.cmbPricingLevel.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.cmbPricingLevel.Name = "cmbPricingLevel";
            this.cmbPricingLevel.Size = new System.Drawing.Size(200, 21);
            this.cmbPricingLevel.TabIndex = 4;
            this.cmbPricingLevel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbPricingLevel_KeyDown);
            // 
            // lblPrizingLevel
            // 
            this.lblPrizingLevel.AutoSize = true;
            this.lblPrizingLevel.BackColor = System.Drawing.Color.Transparent;
            this.lblPrizingLevel.ForeColor = System.Drawing.Color.Black;
            this.lblPrizingLevel.Location = new System.Drawing.Point(18, 70);
            this.lblPrizingLevel.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lblPrizingLevel.Name = "lblPrizingLevel";
            this.lblPrizingLevel.Size = new System.Drawing.Size(68, 13);
            this.lblPrizingLevel.TabIndex = 1227;
            this.lblPrizingLevel.Text = "Pricing Level";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.cbxMrp);
            this.groupBox1.Controls.Add(this.CbxStandardRate);
            this.groupBox1.Controls.Add(this.cbxSalesRate);
            this.groupBox1.Controls.Add(this.cbxlastSalesRate);
            this.groupBox1.Controls.Add(this.cbxPurchaseRate);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.Maroon;
            this.groupBox1.Location = new System.Drawing.Point(18, 91);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(753, 74);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Details";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Black;
            this.groupBox2.ForeColor = System.Drawing.Color.Black;
            this.groupBox2.Location = new System.Drawing.Point(10, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(80, 1);
            this.groupBox2.TabIndex = 1228;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // cbxMrp
            // 
            this.cbxMrp.AutoSize = true;
            this.cbxMrp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxMrp.ForeColor = System.Drawing.Color.Black;
            this.cbxMrp.Location = new System.Drawing.Point(269, 26);
            this.cbxMrp.Name = "cbxMrp";
            this.cbxMrp.Size = new System.Drawing.Size(56, 17);
            this.cbxMrp.TabIndex = 2;
            this.cbxMrp.Text = "M.R.P";
            this.cbxMrp.UseVisualStyleBackColor = true;
            this.cbxMrp.CheckedChanged += new System.EventHandler(this.cbxMrp_CheckedChanged);
            this.cbxMrp.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbxMrp_KeyDown);
            // 
            // CbxStandardRate
            // 
            this.CbxStandardRate.AutoSize = true;
            this.CbxStandardRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CbxStandardRate.ForeColor = System.Drawing.Color.Black;
            this.CbxStandardRate.Location = new System.Drawing.Point(141, 49);
            this.CbxStandardRate.Name = "CbxStandardRate";
            this.CbxStandardRate.Size = new System.Drawing.Size(95, 17);
            this.CbxStandardRate.TabIndex = 4;
            this.CbxStandardRate.Text = "Standard Rate";
            this.CbxStandardRate.UseVisualStyleBackColor = true;
            this.CbxStandardRate.CheckedChanged += new System.EventHandler(this.CbxStandardRate_CheckedChanged);
            this.CbxStandardRate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CbxStandardRate_KeyDown);
            // 
            // cbxSalesRate
            // 
            this.cbxSalesRate.AutoSize = true;
            this.cbxSalesRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxSalesRate.ForeColor = System.Drawing.Color.Black;
            this.cbxSalesRate.Location = new System.Drawing.Point(10, 49);
            this.cbxSalesRate.Name = "cbxSalesRate";
            this.cbxSalesRate.Size = new System.Drawing.Size(78, 17);
            this.cbxSalesRate.TabIndex = 3;
            this.cbxSalesRate.Text = "Sales Rate";
            this.cbxSalesRate.UseVisualStyleBackColor = true;
            this.cbxSalesRate.CheckedChanged += new System.EventHandler(this.cbxSalesRate_CheckedChanged);
            this.cbxSalesRate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbxSalesRate_KeyDown);
            // 
            // cbxlastSalesRate
            // 
            this.cbxlastSalesRate.AutoSize = true;
            this.cbxlastSalesRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxlastSalesRate.ForeColor = System.Drawing.Color.Black;
            this.cbxlastSalesRate.Location = new System.Drawing.Point(141, 26);
            this.cbxlastSalesRate.Name = "cbxlastSalesRate";
            this.cbxlastSalesRate.Size = new System.Drawing.Size(101, 17);
            this.cbxlastSalesRate.TabIndex = 1;
            this.cbxlastSalesRate.Text = "Last Sales Rate";
            this.cbxlastSalesRate.UseVisualStyleBackColor = true;
            this.cbxlastSalesRate.CheckedChanged += new System.EventHandler(this.cbxlastSalesRate_CheckedChanged);
            this.cbxlastSalesRate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbxlastSalesRate_KeyDown);
            // 
            // cbxPurchaseRate
            // 
            this.cbxPurchaseRate.AutoSize = true;
            this.cbxPurchaseRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxPurchaseRate.ForeColor = System.Drawing.Color.Black;
            this.cbxPurchaseRate.Location = new System.Drawing.Point(10, 26);
            this.cbxPurchaseRate.Name = "cbxPurchaseRate";
            this.cbxPurchaseRate.Size = new System.Drawing.Size(94, 17);
            this.cbxPurchaseRate.TabIndex = 0;
            this.cbxPurchaseRate.Text = "PurchaseRate";
            this.cbxPurchaseRate.UseVisualStyleBackColor = true;
            this.cbxPurchaseRate.CheckedChanged += new System.EventHandler(this.cbxPurchaseRate_CheckedChanged);
            this.cbxPurchaseRate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbxPurchaseRate_KeyDown);
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.ForeColor = System.Drawing.Color.Black;
            this.btnPrint.Location = new System.Drawing.Point(596, 527);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(85, 27);
            this.btnPrint.TabIndex = 8;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.ForeColor = System.Drawing.Color.Black;
            this.btnExport.Location = new System.Drawing.Point(686, 527);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(85, 26);
            this.btnExport.TabIndex = 9;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // dgvtxtSerialNo
            // 
            this.dgvtxtSerialNo.HeaderText = "Sl.No";
            this.dgvtxtSerialNo.Name = "dgvtxtSerialNo";
            this.dgvtxtSerialNo.ReadOnly = true;
            this.dgvtxtSerialNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgvtxtSerialNo.Width = 75;
            // 
            // ProductId
            // 
            this.ProductId.DataPropertyName = "productId";
            this.ProductId.HeaderText = "ProductId";
            this.ProductId.Name = "ProductId";
            this.ProductId.ReadOnly = true;
            this.ProductId.Visible = false;
            // 
            // numbr
            // 
            this.numbr.DataPropertyName = "productCode";
            this.numbr.HeaderText = "Product Code";
            this.numbr.Name = "numbr";
            this.numbr.ReadOnly = true;
            this.numbr.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.numbr.Width = 180;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "productName";
            this.Column1.HeaderText = "Product Name";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 180;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "unitName";
            this.Column2.HeaderText = "Unit";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 180;
            // 
            // PurchaseRate
            // 
            this.PurchaseRate.DataPropertyName = "purchaseRate";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.PurchaseRate.DefaultCellStyle = dataGridViewCellStyle2;
            this.PurchaseRate.HeaderText = "Purchase Rate";
            this.PurchaseRate.Name = "PurchaseRate";
            this.PurchaseRate.ReadOnly = true;
            this.PurchaseRate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.PurchaseRate.Width = 75;
            // 
            // SalesRate
            // 
            this.SalesRate.DataPropertyName = "salesRate";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.SalesRate.DefaultCellStyle = dataGridViewCellStyle3;
            this.SalesRate.HeaderText = "Sales Rate";
            this.SalesRate.Name = "SalesRate";
            this.SalesRate.ReadOnly = true;
            this.SalesRate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SalesRate.Width = 75;
            // 
            // LastSalesRate
            // 
            this.LastSalesRate.DataPropertyName = "LastSalesRate";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.LastSalesRate.DefaultCellStyle = dataGridViewCellStyle4;
            this.LastSalesRate.HeaderText = "Last Sales Rate";
            this.LastSalesRate.Name = "LastSalesRate";
            this.LastSalesRate.ReadOnly = true;
            this.LastSalesRate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LastSalesRate.Width = 75;
            // 
            // StandardRate
            // 
            this.StandardRate.DataPropertyName = "StandardRate";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.StandardRate.DefaultCellStyle = dataGridViewCellStyle5;
            this.StandardRate.HeaderText = "Standard Rate";
            this.StandardRate.Name = "StandardRate";
            this.StandardRate.ReadOnly = true;
            this.StandardRate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.StandardRate.Width = 75;
            // 
            // MRP
            // 
            this.MRP.DataPropertyName = "mrp";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.MRP.DefaultCellStyle = dataGridViewCellStyle6;
            this.MRP.HeaderText = "MRP";
            this.MRP.Name = "MRP";
            this.MRP.ReadOnly = true;
            this.MRP.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.MRP.Width = 75;
            // 
            // Column8
            // 
            this.Column8.DataPropertyName = "price";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column8.DefaultCellStyle = dataGridViewCellStyle7;
            this.Column8.HeaderText = "Price";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column8.Width = 180;
            // 
            // frmPriceListReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmbPricingLevel);
            this.Controls.Add(this.lblPrizingLevel);
            this.Controls.Add(this.cmbSize);
            this.Controls.Add(this.lblSize);
            this.Controls.Add(this.cmbModel);
            this.Controls.Add(this.lblModelNo);
            this.Controls.Add(this.dgvPriceList);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.cmbProduct);
            this.Controls.Add(this.lblProduct);
            this.Controls.Add(this.cmbProductGroup);
            this.Controls.Add(this.lblProductGroup);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmPriceListReport";
            this.Padding = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Price List";
            this.Load += new System.EventHandler(this.frmPriceListReport_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPriceListReport_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPriceList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPriceList;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.ComboBox cmbProduct;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.ComboBox cmbProductGroup;
        private System.Windows.Forms.Label lblProductGroup;
        private System.Windows.Forms.ComboBox cmbModel;
        private System.Windows.Forms.Label lblModelNo;
        private System.Windows.Forms.ComboBox cmbSize;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.ComboBox cmbPricingLevel;
        private System.Windows.Forms.Label lblPrizingLevel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox CbxStandardRate;
        private System.Windows.Forms.CheckBox cbxSalesRate;
        private System.Windows.Forms.CheckBox cbxlastSalesRate;
        private System.Windows.Forms.CheckBox cbxMrp;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.CheckBox cbxPurchaseRate;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtSerialNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductId;
        private System.Windows.Forms.DataGridViewTextBoxColumn numbr;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn PurchaseRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn SalesRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastSalesRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn StandardRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn MRP;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
    }
}