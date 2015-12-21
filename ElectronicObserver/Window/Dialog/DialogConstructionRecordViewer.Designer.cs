namespace ElectronicObserver.Window.Dialog {
	partial class DialogConstructionRecordViewer {
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.StatusInfo = new System.Windows.Forms.ToolStripStatusLabel();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.label8 = new System.Windows.Forms.Label();
			this.DevelopmentMaterial = new System.Windows.Forms.ComboBox();
			this.IsLargeConstruction = new System.Windows.Forms.CheckBox();
			this.label7 = new System.Windows.Forms.Label();
			this.EmptyDock = new System.Windows.Forms.ComboBox();
			this.ButtonRun = new System.Windows.Forms.Button();
			this.MergeRows = new System.Windows.Forms.CheckBox();
			this.label6 = new System.Windows.Forms.Label();
			this.Recipe = new System.Windows.Forms.ComboBox();
			this.SecretaryName = new System.Windows.Forms.ComboBox();
			this.SecretaryCategory = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.DateEnd = new System.Windows.Forms.DateTimePicker();
			this.DateBegin = new System.Windows.Forms.DateTimePicker();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.ShipCategory = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.ShipName = new System.Windows.Forms.ComboBox();
			this.RecordView = new System.Windows.Forms.DataGridView();
			this.RecordView_Header = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Recipe = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_SecretaryShip = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Material100 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Material20 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Material1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Searcher = new System.ComponentModel.BackgroundWorker();
			this.statusStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.RecordView)).BeginInit();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusInfo});
			this.statusStrip1.Location = new System.Drawing.Point(0, 419);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(624, 22);
			this.statusStrip1.TabIndex = 0;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// StatusInfo
			// 
			this.StatusInfo.Name = "StatusInfo";
			this.StatusInfo.Size = new System.Drawing.Size(12, 17);
			this.StatusInfo.Text = "-";
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.IsSplitterFixed = true;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.label8);
			this.splitContainer1.Panel1.Controls.Add(this.DevelopmentMaterial);
			this.splitContainer1.Panel1.Controls.Add(this.IsLargeConstruction);
			this.splitContainer1.Panel1.Controls.Add(this.label7);
			this.splitContainer1.Panel1.Controls.Add(this.EmptyDock);
			this.splitContainer1.Panel1.Controls.Add(this.ButtonRun);
			this.splitContainer1.Panel1.Controls.Add(this.MergeRows);
			this.splitContainer1.Panel1.Controls.Add(this.label6);
			this.splitContainer1.Panel1.Controls.Add(this.Recipe);
			this.splitContainer1.Panel1.Controls.Add(this.SecretaryName);
			this.splitContainer1.Panel1.Controls.Add(this.SecretaryCategory);
			this.splitContainer1.Panel1.Controls.Add(this.label5);
			this.splitContainer1.Panel1.Controls.Add(this.DateEnd);
			this.splitContainer1.Panel1.Controls.Add(this.DateBegin);
			this.splitContainer1.Panel1.Controls.Add(this.label4);
			this.splitContainer1.Panel1.Controls.Add(this.label3);
			this.splitContainer1.Panel1.Controls.Add(this.label2);
			this.splitContainer1.Panel1.Controls.Add(this.ShipCategory);
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			this.splitContainer1.Panel1.Controls.Add(this.ShipName);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.RecordView);
			this.splitContainer1.Size = new System.Drawing.Size(624, 419);
			this.splitContainer1.SplitterDistance = 98;
			this.splitContainer1.TabIndex = 1;
			// 
			// label8
			// 
			this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(368, 42);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(55, 15);
			this.label8.TabIndex = 42;
			this.label8.Text = "開発資材";
			// 
			// DevelopmentMaterial
			// 
			this.DevelopmentMaterial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.DevelopmentMaterial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.DevelopmentMaterial.FormattingEnabled = true;
			this.DevelopmentMaterial.Location = new System.Drawing.Point(429, 39);
			this.DevelopmentMaterial.Name = "DevelopmentMaterial";
			this.DevelopmentMaterial.Size = new System.Drawing.Size(60, 23);
			this.DevelopmentMaterial.TabIndex = 41;
			// 
			// IsLargeConstruction
			// 
			this.IsLargeConstruction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.IsLargeConstruction.AutoSize = true;
			this.IsLargeConstruction.Checked = true;
			this.IsLargeConstruction.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			this.IsLargeConstruction.Location = new System.Drawing.Point(377, 69);
			this.IsLargeConstruction.Name = "IsLargeConstruction";
			this.IsLargeConstruction.Size = new System.Drawing.Size(86, 19);
			this.IsLargeConstruction.TabIndex = 40;
			this.IsLargeConstruction.Text = "大型艦建造";
			this.IsLargeConstruction.ThreeState = true;
			this.IsLargeConstruction.UseVisualStyleBackColor = true;
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(495, 42);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(51, 15);
			this.label7.TabIndex = 39;
			this.label7.Text = "空きドック";
			// 
			// EmptyDock
			// 
			this.EmptyDock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.EmptyDock.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.EmptyDock.FormattingEnabled = true;
			this.EmptyDock.Location = new System.Drawing.Point(552, 39);
			this.EmptyDock.Name = "EmptyDock";
			this.EmptyDock.Size = new System.Drawing.Size(60, 23);
			this.EmptyDock.TabIndex = 38;
			// 
			// ButtonRun
			// 
			this.ButtonRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonRun.Location = new System.Drawing.Point(537, 66);
			this.ButtonRun.Name = "ButtonRun";
			this.ButtonRun.Size = new System.Drawing.Size(75, 23);
			this.ButtonRun.TabIndex = 37;
			this.ButtonRun.Text = "検索";
			this.ButtonRun.UseVisualStyleBackColor = true;
			this.ButtonRun.Click += new System.EventHandler(this.ButtonRun_Click);
			// 
			// MergeRows
			// 
			this.MergeRows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.MergeRows.AutoSize = true;
			this.MergeRows.Location = new System.Drawing.Point(469, 69);
			this.MergeRows.Name = "MergeRows";
			this.MergeRows.Size = new System.Drawing.Size(62, 19);
			this.MergeRows.TabIndex = 36;
			this.MergeRows.Text = "まとめる";
			this.MergeRows.UseVisualStyleBackColor = true;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(371, 13);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(35, 15);
			this.label6.TabIndex = 35;
			this.label6.Text = "レシピ";
			// 
			// Recipe
			// 
			this.Recipe.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Recipe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.Recipe.FormattingEnabled = true;
			this.Recipe.Location = new System.Drawing.Point(412, 10);
			this.Recipe.Name = "Recipe";
			this.Recipe.Size = new System.Drawing.Size(200, 23);
			this.Recipe.TabIndex = 34;
			// 
			// SecretaryName
			// 
			this.SecretaryName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.SecretaryName.FormattingEnabled = true;
			this.SecretaryName.Location = new System.Drawing.Point(188, 68);
			this.SecretaryName.Name = "SecretaryName";
			this.SecretaryName.Size = new System.Drawing.Size(121, 23);
			this.SecretaryName.TabIndex = 33;
			this.SecretaryName.SelectedIndexChanged += new System.EventHandler(this.SecretaryName_SelectedIndexChanged);
			// 
			// SecretaryCategory
			// 
			this.SecretaryCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.SecretaryCategory.FormattingEnabled = true;
			this.SecretaryCategory.Location = new System.Drawing.Point(61, 68);
			this.SecretaryCategory.Name = "SecretaryCategory";
			this.SecretaryCategory.Size = new System.Drawing.Size(121, 23);
			this.SecretaryCategory.TabIndex = 32;
			this.SecretaryCategory.SelectedIndexChanged += new System.EventHandler(this.SecretaryCategory_SelectedIndexChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 71);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(43, 15);
			this.label5.TabIndex = 31;
			this.label5.Text = "秘書艦";
			// 
			// DateEnd
			// 
			this.DateEnd.Location = new System.Drawing.Point(225, 39);
			this.DateEnd.Name = "DateEnd";
			this.DateEnd.Size = new System.Drawing.Size(140, 23);
			this.DateEnd.TabIndex = 30;
			// 
			// DateBegin
			// 
			this.DateBegin.Location = new System.Drawing.Point(225, 10);
			this.DateBegin.Name = "DateBegin";
			this.DateBegin.Size = new System.Drawing.Size(140, 23);
			this.DateBegin.TabIndex = 29;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(188, 42);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(31, 15);
			this.label4.TabIndex = 28;
			this.label4.Text = "終了";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(188, 13);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(31, 15);
			this.label3.TabIndex = 27;
			this.label3.Text = "開始";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 13);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(41, 15);
			this.label2.TabIndex = 26;
			this.label2.Text = "カテゴリ";
			// 
			// ShipCategory
			// 
			this.ShipCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ShipCategory.FormattingEnabled = true;
			this.ShipCategory.Location = new System.Drawing.Point(61, 10);
			this.ShipCategory.Name = "ShipCategory";
			this.ShipCategory.Size = new System.Drawing.Size(121, 23);
			this.ShipCategory.TabIndex = 25;
			this.ShipCategory.SelectedIndexChanged += new System.EventHandler(this.ShipCategory_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 42);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(31, 15);
			this.label1.TabIndex = 24;
			this.label1.Text = "艦名";
			// 
			// ShipName
			// 
			this.ShipName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.ShipName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.ShipName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ShipName.FormattingEnabled = true;
			this.ShipName.Location = new System.Drawing.Point(61, 39);
			this.ShipName.Name = "ShipName";
			this.ShipName.Size = new System.Drawing.Size(121, 23);
			this.ShipName.TabIndex = 23;
			this.ShipName.SelectedIndexChanged += new System.EventHandler(this.ShipName_SelectedIndexChanged);
			// 
			// RecordView
			// 
			this.RecordView.AllowUserToAddRows = false;
			this.RecordView.AllowUserToDeleteRows = false;
			this.RecordView.AllowUserToResizeRows = false;
			this.RecordView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.RecordView.ColumnHeadersVisible = false;
			this.RecordView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RecordView_Header,
            this.RecordView_Name,
            this.RecordView_Date,
            this.RecordView_Recipe,
            this.RecordView_SecretaryShip,
            this.RecordView_Material100,
            this.RecordView_Material20,
            this.RecordView_Material1});
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(204)))));
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.RecordView.DefaultCellStyle = dataGridViewCellStyle1;
			this.RecordView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RecordView.Location = new System.Drawing.Point(0, 0);
			this.RecordView.Name = "RecordView";
			this.RecordView.ReadOnly = true;
			this.RecordView.RowHeadersVisible = false;
			this.RecordView.RowTemplate.Height = 21;
			this.RecordView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.RecordView.Size = new System.Drawing.Size(624, 317);
			this.RecordView.TabIndex = 0;
			this.RecordView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.RecordView_CellFormatting);
			this.RecordView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.RecordView_SortCompare);
			this.RecordView.Sorted += new System.EventHandler(this.RecordView_Sorted);
			// 
			// RecordView_Header
			// 
			this.RecordView_Header.HeaderText = "";
			this.RecordView_Header.Name = "RecordView_Header";
			this.RecordView_Header.ReadOnly = true;
			// 
			// RecordView_Name
			// 
			this.RecordView_Name.HeaderText = "名前";
			this.RecordView_Name.Name = "RecordView_Name";
			this.RecordView_Name.ReadOnly = true;
			// 
			// RecordView_Date
			// 
			this.RecordView_Date.HeaderText = "日付";
			this.RecordView_Date.Name = "RecordView_Date";
			this.RecordView_Date.ReadOnly = true;
			// 
			// RecordView_Recipe
			// 
			this.RecordView_Recipe.HeaderText = "レシピ";
			this.RecordView_Recipe.Name = "RecordView_Recipe";
			this.RecordView_Recipe.ReadOnly = true;
			// 
			// RecordView_SecretaryShip
			// 
			this.RecordView_SecretaryShip.HeaderText = "秘書艦";
			this.RecordView_SecretaryShip.Name = "RecordView_SecretaryShip";
			this.RecordView_SecretaryShip.ReadOnly = true;
			// 
			// RecordView_Material100
			// 
			this.RecordView_Material100.HeaderText = "100";
			this.RecordView_Material100.Name = "RecordView_Material100";
			this.RecordView_Material100.ReadOnly = true;
			// 
			// RecordView_Material20
			// 
			this.RecordView_Material20.HeaderText = "20";
			this.RecordView_Material20.Name = "RecordView_Material20";
			this.RecordView_Material20.ReadOnly = true;
			// 
			// RecordView_Material1
			// 
			this.RecordView_Material1.HeaderText = "1";
			this.RecordView_Material1.Name = "RecordView_Material1";
			this.RecordView_Material1.ReadOnly = true;
			// 
			// Searcher
			// 
			this.Searcher.WorkerSupportsCancellation = true;
			this.Searcher.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Searcher_DoWork);
			this.Searcher.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Searcher_RunWorkerCompleted);
			// 
			// DialogConstructionRecordViewer
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(624, 441);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.statusStrip1);
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.Name = "DialogConstructionRecordViewer";
			this.Text = "建造記録";
			this.Load += new System.EventHandler(this.DialogConstructionRecordViewer_Load);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.RecordView)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.CheckBox IsLargeConstruction;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox EmptyDock;
		private System.Windows.Forms.Button ButtonRun;
		private System.Windows.Forms.CheckBox MergeRows;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ComboBox Recipe;
		private System.Windows.Forms.ComboBox SecretaryName;
		private System.Windows.Forms.ComboBox SecretaryCategory;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.DateTimePicker DateEnd;
		private System.Windows.Forms.DateTimePicker DateBegin;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox ShipCategory;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox ShipName;
		private System.Windows.Forms.DataGridView RecordView;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox DevelopmentMaterial;
		private System.ComponentModel.BackgroundWorker Searcher;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Header;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Name;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Date;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Recipe;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_SecretaryShip;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Material100;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Material20;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Material1;
		private System.Windows.Forms.ToolStripStatusLabel StatusInfo;
	}
}