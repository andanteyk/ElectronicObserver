namespace ElectronicObserver.Window.Dialog
{
	partial class DialogShipGroupColumnFilter
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
            this.ButtonOK = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.ColumnView = new System.Windows.Forms.DataGridView();
            this.ColumnView_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnView_Visible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnView_AutoSize = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnView_Width = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnView_Up = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ColumnView_Down = new System.Windows.Forms.DataGridViewButtonColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.ScrLkColumnCount = new System.Windows.Forms.NumericUpDown();
            this.ButtonSelectedUp = new System.Windows.Forms.Button();
            this.ButtonSelectedDown = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ColumnView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScrLkColumnCount)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonOK
            // 
            this.ButtonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonOK.Location = new System.Drawing.Point(296, 327);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(75, 23);
            this.ButtonOK.TabIndex = 2;
            this.ButtonOK.Text = "OK";
            this.ButtonOK.UseVisualStyleBackColor = true;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(377, 327);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ButtonCancel.TabIndex = 3;
            this.ButtonCancel.Text = "キャンセル";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // ColumnView
            // 
            this.ColumnView.AllowUserToAddRows = false;
            this.ColumnView.AllowUserToDeleteRows = false;
            this.ColumnView.AllowUserToResizeColumns = false;
            this.ColumnView.AllowUserToResizeRows = false;
            this.ColumnView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ColumnView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ColumnView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnView_Name,
            this.ColumnView_Visible,
            this.ColumnView_AutoSize,
            this.ColumnView_Width,
            this.ColumnView_Up,
            this.ColumnView_Down});
            this.ColumnView.Location = new System.Drawing.Point(12, 12);
            this.ColumnView.MultiSelect = false;
            this.ColumnView.Name = "ColumnView";
            this.ColumnView.RowHeadersVisible = false;
            this.ColumnView.RowTemplate.Height = 21;
            this.ColumnView.Size = new System.Drawing.Size(440, 309);
            this.ColumnView.TabIndex = 4;
            this.ColumnView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ColumnView_CellContentClick);
            this.ColumnView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.ColumnView_CellValidating);
            this.ColumnView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.ColumnView_CellValueChanged);
            this.ColumnView.CurrentCellDirtyStateChanged += new System.EventHandler(this.ColumnView_CurrentCellDirtyStateChanged);
            this.ColumnView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.ColumnView_DataError);
            // 
            // ColumnView_Name
            // 
            this.ColumnView_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnView_Name.HeaderText = "列名";
            this.ColumnView_Name.Name = "ColumnView_Name";
            this.ColumnView_Name.ReadOnly = true;
            this.ColumnView_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnView_Visible
            // 
            this.ColumnView_Visible.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnView_Visible.HeaderText = "表示";
            this.ColumnView_Visible.Name = "ColumnView_Visible";
            this.ColumnView_Visible.Width = 37;
            // 
            // ColumnView_AutoSize
            // 
            this.ColumnView_AutoSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnView_AutoSize.HeaderText = "自動サイズ";
            this.ColumnView_AutoSize.Name = "ColumnView_AutoSize";
            this.ColumnView_AutoSize.Width = 66;
            // 
            // ColumnView_Width
            // 
            this.ColumnView_Width.HeaderText = "幅";
            this.ColumnView_Width.Name = "ColumnView_Width";
            this.ColumnView_Width.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnView_Up
            // 
            this.ColumnView_Up.HeaderText = "↑";
            this.ColumnView_Up.Name = "ColumnView_Up";
            this.ColumnView_Up.Width = 20;
            // 
            // ColumnView_Down
            // 
            this.ColumnView_Down.HeaderText = "↓";
            this.ColumnView_Down.Name = "ColumnView_Down";
            this.ColumnView_Down.Width = 20;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 331);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "列の固定: ";
            // 
            // ScrLkColumnCount
            // 
            this.ScrLkColumnCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ScrLkColumnCount.Location = new System.Drawing.Point(80, 327);
            this.ScrLkColumnCount.Name = "ScrLkColumnCount";
            this.ScrLkColumnCount.Size = new System.Drawing.Size(60, 23);
            this.ScrLkColumnCount.TabIndex = 6;
            this.ScrLkColumnCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ButtonSelectedUp
            // 
            this.ButtonSelectedUp.Location = new System.Drawing.Point(200, 327);
            this.ButtonSelectedUp.Name = "ButtonSelectedUp";
            this.ButtonSelectedUp.Size = new System.Drawing.Size(25, 23);
            this.ButtonSelectedUp.TabIndex = 7;
            this.ButtonSelectedUp.Text = "↑";
            this.ButtonSelectedUp.UseVisualStyleBackColor = true;
            this.ButtonSelectedUp.Click += new System.EventHandler(this.ButtonSelectedUp_Click);
            // 
            // ButtonSelectedDown
            // 
            this.ButtonSelectedDown.Location = new System.Drawing.Point(231, 327);
            this.ButtonSelectedDown.Name = "ButtonSelectedDown";
            this.ButtonSelectedDown.Size = new System.Drawing.Size(25, 23);
            this.ButtonSelectedDown.TabIndex = 8;
            this.ButtonSelectedDown.Text = "↓";
            this.ButtonSelectedDown.UseVisualStyleBackColor = true;
            this.ButtonSelectedDown.Click += new System.EventHandler(this.ButtonSelectedDown_Click);
            // 
            // DialogShipGroupColumnFilter
            // 
            this.AcceptButton = this.ButtonOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.ButtonCancel;
            this.ClientSize = new System.Drawing.Size(464, 362);
            this.Controls.Add(this.ButtonSelectedDown);
            this.Controls.Add(this.ButtonSelectedUp);
            this.Controls.Add(this.ScrLkColumnCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ColumnView);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOK);
            this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogShipGroupColumnFilter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "列の表示設定";
            this.Load += new System.EventHandler(this.DialogShipGroupColumnFilter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ColumnView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScrLkColumnCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button ButtonOK;
		private System.Windows.Forms.Button ButtonCancel;
		private System.Windows.Forms.DataGridView ColumnView;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColumnView_Name;
		private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnView_Visible;
		private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnView_AutoSize;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColumnView_Width;
		private System.Windows.Forms.DataGridViewButtonColumn ColumnView_Up;
		private System.Windows.Forms.DataGridViewButtonColumn ColumnView_Down;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown ScrLkColumnCount;
        private System.Windows.Forms.Button ButtonSelectedUp;
        private System.Windows.Forms.Button ButtonSelectedDown;
    }
}