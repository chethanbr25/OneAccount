namespace One_Account
{
    partial class frmPriceListPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPriceListPopup));
            this.btnSave = new System.Windows.Forms.Button();
            this.txtRate = new System.Windows.Forms.TextBox();
            this.lblRate = new System.Windows.Forms.Label();
            this.gbxDetails = new System.Windows.Forms.GroupBox();
            this.txtUnitName = new System.Windows.Forms.TextBox();
            this.lblUnit = new System.Windows.Forms.Label();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.lblProductName = new System.Windows.Forms.Label();
            this.txtProductCode = new System.Windows.Forms.TextBox();
            this.lblProductCode = new System.Windows.Forms.Label();
            this.txtPricingLevel = new System.Windows.Forms.TextBox();
            this.lblPricingLevel = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbBatch = new System.Windows.Forms.ComboBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvProductGroup = new System.Windows.Forms.DataGridView();
            this.dgvtxtSlNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtxtPriceListId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtxtBatchName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtxtRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtxtpricelevelId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtxtBatchId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtxtunitId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtxtProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbxDetails.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductGroup)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Location = new System.Drawing.Point(400, 151);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(85, 27);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnSave.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnSave_KeyDown);
            // 
            // txtRate
            // 
            this.txtRate.Location = new System.Drawing.Point(558, 113);
            this.txtRate.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.txtRate.Name = "txtRate";
            this.txtRate.Size = new System.Drawing.Size(200, 20);
            this.txtRate.TabIndex = 1;
            this.txtRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtRate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRate_KeyDown);
            this.txtRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRate_KeyPress);
            // 
            // lblRate
            // 
            this.lblRate.AutoSize = true;
            this.lblRate.BackColor = System.Drawing.Color.Transparent;
            this.lblRate.ForeColor = System.Drawing.Color.Black;
            this.lblRate.Location = new System.Drawing.Point(466, 116);
            this.lblRate.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lblRate.Name = "lblRate";
            this.lblRate.Size = new System.Drawing.Size(30, 13);
            this.lblRate.TabIndex = 178;
            this.lblRate.Text = "Rate";
            // 
            // gbxDetails
            // 
            this.gbxDetails.BackColor = System.Drawing.Color.Transparent;
            this.gbxDetails.Controls.Add(this.txtUnitName);
            this.gbxDetails.Controls.Add(this.lblUnit);
            this.gbxDetails.Controls.Add(this.txtProductName);
            this.gbxDetails.Controls.Add(this.lblProductName);
            this.gbxDetails.Controls.Add(this.txtProductCode);
            this.gbxDetails.Controls.Add(this.lblProductCode);
            this.gbxDetails.Controls.Add(this.txtPricingLevel);
            this.gbxDetails.Controls.Add(this.lblPricingLevel);
            this.gbxDetails.ForeColor = System.Drawing.Color.White;
            this.gbxDetails.Location = new System.Drawing.Point(18, 13);
            this.gbxDetails.Name = "gbxDetails";
            this.gbxDetails.Size = new System.Drawing.Size(758, 89);
            this.gbxDetails.TabIndex = 180;
            this.gbxDetails.TabStop = false;
            // 
            // txtUnitName
            // 
            this.txtUnitName.Location = new System.Drawing.Point(540, 46);
            this.txtUnitName.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.txtUnitName.Name = "txtUnitName";
            this.txtUnitName.Size = new System.Drawing.Size(200, 20);
            this.txtUnitName.TabIndex = 3;
            // 
            // lblUnit
            // 
            this.lblUnit.AutoSize = true;
            this.lblUnit.ForeColor = System.Drawing.Color.Black;
            this.lblUnit.Location = new System.Drawing.Point(448, 50);
            this.lblUnit.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(26, 13);
            this.lblUnit.TabIndex = 186;
            this.lblUnit.Text = "Unit";
            // 
            // txtProductName
            // 
            this.txtProductName.Location = new System.Drawing.Point(107, 46);
            this.txtProductName.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new System.Drawing.Size(200, 20);
            this.txtProductName.TabIndex = 2;
            // 
            // lblProductName
            // 
            this.lblProductName.AutoSize = true;
            this.lblProductName.ForeColor = System.Drawing.Color.Black;
            this.lblProductName.Location = new System.Drawing.Point(13, 50);
            this.lblProductName.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(75, 13);
            this.lblProductName.TabIndex = 184;
            this.lblProductName.Text = "Product Name";
            // 
            // txtProductCode
            // 
            this.txtProductCode.Location = new System.Drawing.Point(540, 21);
            this.txtProductCode.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.txtProductCode.Name = "txtProductCode";
            this.txtProductCode.Size = new System.Drawing.Size(200, 20);
            this.txtProductCode.TabIndex = 1;
            // 
            // lblProductCode
            // 
            this.lblProductCode.AutoSize = true;
            this.lblProductCode.ForeColor = System.Drawing.Color.Black;
            this.lblProductCode.Location = new System.Drawing.Point(448, 25);
            this.lblProductCode.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lblProductCode.Name = "lblProductCode";
            this.lblProductCode.Size = new System.Drawing.Size(72, 13);
            this.lblProductCode.TabIndex = 182;
            this.lblProductCode.Text = "Product Code";
            // 
            // txtPricingLevel
            // 
            this.txtPricingLevel.Location = new System.Drawing.Point(107, 21);
            this.txtPricingLevel.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.txtPricingLevel.Name = "txtPricingLevel";
            this.txtPricingLevel.Size = new System.Drawing.Size(200, 20);
            this.txtPricingLevel.TabIndex = 0;
            // 
            // lblPricingLevel
            // 
            this.lblPricingLevel.AutoSize = true;
            this.lblPricingLevel.ForeColor = System.Drawing.Color.Black;
            this.lblPricingLevel.Location = new System.Drawing.Point(13, 25);
            this.lblPricingLevel.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lblPricingLevel.Name = "lblPricingLevel";
            this.lblPricingLevel.Size = new System.Drawing.Size(68, 13);
            this.lblPricingLevel.TabIndex = 180;
            this.lblPricingLevel.Text = "Pricing Level";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Location = new System.Drawing.Point(673, 151);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 27);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            this.btnClose.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnClose_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(31, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 181;
            this.label1.Text = "Batch";
            // 
            // cmbBatch
            // 
            this.cmbBatch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbBatch.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBatch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBatch.FormattingEnabled = true;
            this.cmbBatch.Location = new System.Drawing.Point(125, 110);
            this.cmbBatch.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.cmbBatch.Name = "cmbBatch";
            this.cmbBatch.Size = new System.Drawing.Size(200, 21);
            this.cmbBatch.TabIndex = 0;
            this.cmbBatch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbBatch_KeyDown);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.ForeColor = System.Drawing.Color.Black;
            this.btnClear.Location = new System.Drawing.Point(491, 151);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(85, 27);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Salmon;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Location = new System.Drawing.Point(582, 151);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(85, 27);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.dgvProductGroup);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.Maroon;
            this.groupBox1.Location = new System.Drawing.Point(18, 188);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(758, 367);
            this.groupBox1.TabIndex = 1146;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "   Details";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Black;
            this.groupBox3.ForeColor = System.Drawing.Color.Black;
            this.groupBox3.Location = new System.Drawing.Point(16, 20);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(70, 1);
            this.groupBox3.TabIndex = 187;
            this.groupBox3.TabStop = false;
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
            this.dgvtxtPriceListId,
            this.dgvtxtBatchName,
            this.dgvtxtRate,
            this.dgvtxtpricelevelId,
            this.dgvtxtBatchId,
            this.dgvtxtunitId,
            this.dgvtxtProductId});
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
            this.dgvProductGroup.Location = new System.Drawing.Point(16, 27);
            this.dgvProductGroup.MultiSelect = false;
            this.dgvProductGroup.Name = "dgvProductGroup";
            this.dgvProductGroup.ReadOnly = true;
            this.dgvProductGroup.RowHeadersVisible = false;
            this.dgvProductGroup.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvProductGroup.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProductGroup.Size = new System.Drawing.Size(724, 321);
            this.dgvProductGroup.TabIndex = 186;
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
            // dgvtxtPriceListId
            // 
            this.dgvtxtPriceListId.DataPropertyName = "pricelistId";
            this.dgvtxtPriceListId.HeaderText = "PricelistId";
            this.dgvtxtPriceListId.Name = "dgvtxtPriceListId";
            this.dgvtxtPriceListId.ReadOnly = true;
            this.dgvtxtPriceListId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgvtxtPriceListId.Visible = false;
            // 
            // dgvtxtBatchName
            // 
            this.dgvtxtBatchName.DataPropertyName = "batchNo";
            this.dgvtxtBatchName.HeaderText = "Batch";
            this.dgvtxtBatchName.Name = "dgvtxtBatchName";
            this.dgvtxtBatchName.ReadOnly = true;
            this.dgvtxtBatchName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvtxtRate
            // 
            this.dgvtxtRate.DataPropertyName = "rate";
            this.dgvtxtRate.HeaderText = "Rate";
            this.dgvtxtRate.Name = "dgvtxtRate";
            this.dgvtxtRate.ReadOnly = true;
            this.dgvtxtRate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvtxtRate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvtxtpricelevelId
            // 
            this.dgvtxtpricelevelId.DataPropertyName = "pricinglevelId";
            this.dgvtxtpricelevelId.HeaderText = "PricinglevelId";
            this.dgvtxtpricelevelId.Name = "dgvtxtpricelevelId";
            this.dgvtxtpricelevelId.ReadOnly = true;
            this.dgvtxtpricelevelId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgvtxtpricelevelId.Visible = false;
            // 
            // dgvtxtBatchId
            // 
            this.dgvtxtBatchId.DataPropertyName = "batchId";
            this.dgvtxtBatchId.HeaderText = "BatchId";
            this.dgvtxtBatchId.Name = "dgvtxtBatchId";
            this.dgvtxtBatchId.ReadOnly = true;
            this.dgvtxtBatchId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgvtxtBatchId.Visible = false;
            // 
            // dgvtxtunitId
            // 
            this.dgvtxtunitId.DataPropertyName = "unitId";
            this.dgvtxtunitId.HeaderText = "UnitId";
            this.dgvtxtunitId.Name = "dgvtxtunitId";
            this.dgvtxtunitId.ReadOnly = true;
            this.dgvtxtunitId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgvtxtunitId.Visible = false;
            // 
            // dgvtxtProductId
            // 
            this.dgvtxtProductId.DataPropertyName = "productId";
            this.dgvtxtProductId.HeaderText = "ProductId";
            this.dgvtxtProductId.Name = "dgvtxtProductId";
            this.dgvtxtProductId.ReadOnly = true;
            this.dgvtxtProductId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgvtxtProductId.Visible = false;
            // 
            // frmPriceListPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(794, 568);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.cmbBatch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gbxDetails);
            this.Controls.Add(this.txtRate);
            this.Controls.Add(this.lblRate);
            this.Controls.Add(this.btnSave);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmPriceListPopup";
            this.Padding = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Price List Popup";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPriceListPopup_FormClosing);
            this.Load += new System.EventHandler(this.frmPriceListPopup_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPriceListPopup_KeyDown);
            this.gbxDetails.ResumeLayout(false);
            this.gbxDetails.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductGroup)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtRate;
        private System.Windows.Forms.Label lblRate;
        private System.Windows.Forms.GroupBox gbxDetails;
        private System.Windows.Forms.TextBox txtUnitName;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.TextBox txtProductCode;
        private System.Windows.Forms.Label lblProductCode;
        private System.Windows.Forms.TextBox txtPricingLevel;
        private System.Windows.Forms.Label lblPricingLevel;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbBatch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvProductGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtSlNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtPriceListId;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtBatchName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtpricelevelId;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtBatchId;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtunitId;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtProductId;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}