namespace EquipmentsDetail
{
	partial class DialogEquipmentList {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing ) {
			if ( disposing && ( components != null ) ) {
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.EquipmentView = new System.Windows.Forms.DataGridView();
            this.EquipmentView_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EquipmentView_Icon = new System.Windows.Forms.DataGridViewImageColumn();
            this.EquipmentView_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EquipmentView_CountAll = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColShips = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.EquipmentView)).BeginInit();
            this.SuspendLayout();
            // 
            // EquipmentView
            // 
            this.EquipmentView.AllowUserToAddRows = false;
            this.EquipmentView.AllowUserToDeleteRows = false;
            this.EquipmentView.AllowUserToResizeRows = false;
            this.EquipmentView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EquipmentView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.EquipmentView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.EquipmentView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EquipmentView_ID,
            this.EquipmentView_Icon,
            this.EquipmentView_Name,
            this.EquipmentView_CountAll,
            this.ColLevel,
            this.ColTotal,
            this.ColShips});
            this.EquipmentView.Location = new System.Drawing.Point(0, 39);
            this.EquipmentView.Name = "EquipmentView";
            this.EquipmentView.ReadOnly = true;
            this.EquipmentView.RowHeadersVisible = false;
            this.EquipmentView.RowTemplate.Height = 21;
            this.EquipmentView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.EquipmentView.Size = new System.Drawing.Size(924, 543);
            this.EquipmentView.TabIndex = 2;
            this.EquipmentView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.EquipmentView_CellFormatting);
            this.EquipmentView.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.EquipmentView_CellPainting);
            // 
            // EquipmentView_ID
            // 
            this.EquipmentView_ID.HeaderText = "ID";
            this.EquipmentView_ID.Name = "EquipmentView_ID";
            this.EquipmentView_ID.ReadOnly = true;
            this.EquipmentView_ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.EquipmentView_ID.Width = 40;
            // 
            // EquipmentView_Icon
            // 
            this.EquipmentView_Icon.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.EquipmentView_Icon.HeaderText = "";
            this.EquipmentView_Icon.Name = "EquipmentView_Icon";
            this.EquipmentView_Icon.ReadOnly = true;
            this.EquipmentView_Icon.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.EquipmentView_Icon.Width = 5;
            // 
            // EquipmentView_Name
            // 
            this.EquipmentView_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.EquipmentView_Name.HeaderText = "装備名";
            this.EquipmentView_Name.Name = "EquipmentView_Name";
            this.EquipmentView_Name.ReadOnly = true;
            this.EquipmentView_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.EquipmentView_Name.Width = 49;
            // 
            // EquipmentView_CountAll
            // 
            this.EquipmentView_CountAll.HeaderText = "個数";
            this.EquipmentView_CountAll.Name = "EquipmentView_CountAll";
            this.EquipmentView_CountAll.ReadOnly = true;
            this.EquipmentView_CountAll.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.EquipmentView_CountAll.Width = 40;
            // 
            // ColLevel
            // 
            this.ColLevel.HeaderText = "改修";
            this.ColLevel.Name = "ColLevel";
            this.ColLevel.ReadOnly = true;
            this.ColLevel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColLevel.Width = 40;
            // 
            // ColTotal
            // 
            this.ColTotal.HeaderText = "个数";
            this.ColTotal.Name = "ColTotal";
            this.ColTotal.ReadOnly = true;
            this.ColTotal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColTotal.Width = 40;
            // 
            // ColShips
            // 
            this.ColShips.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ColShips.DefaultCellStyle = dataGridViewCellStyle6;
            this.ColShips.HeaderText = "装备于";
            this.ColShips.Name = "ColShips";
            this.ColShips.ReadOnly = true;
            this.ColShips.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 30);
            this.button1.TabIndex = 3;
            this.button1.Text = "过滤设置";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(99, 9);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(74, 19);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "启用过滤";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // DialogEquipmentList
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(925, 582);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.EquipmentView);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "DialogEquipmentList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "装备一览";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogEquipmentList_FormClosed);
            this.Load += new System.EventHandler(this.DialogEquipmentList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.EquipmentView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.DataGridView EquipmentView;
        private System.Windows.Forms.DataGridViewTextBoxColumn EquipmentView_ID;
        private System.Windows.Forms.DataGridViewImageColumn EquipmentView_Icon;
        private System.Windows.Forms.DataGridViewTextBoxColumn EquipmentView_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn EquipmentView_CountAll;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColShips;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox1;
	}
}