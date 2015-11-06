namespace ElectronicObserver.Window.Dialog {
	partial class DialogDevelopmentRecordViewer {
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
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
			this.EquipmentCategory = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.EquipmentName = new System.Windows.Forms.ComboBox();
			this.RecordView = new System.Windows.Forms.DataGridView();
			this.ToolTipInfo = new System.Windows.Forms.ToolTip(this.components);
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.StatusInfo = new System.Windows.Forms.ToolStripStatusLabel();
			this.Searcher = new System.ComponentModel.BackgroundWorker();
			this.RecordView_Header = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Recipe = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_FlagshipType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Flagship = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Detail = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.RecordView)).BeginInit();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
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
			this.splitContainer1.Panel1.Controls.Add(this.EquipmentCategory);
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			this.splitContainer1.Panel1.Controls.Add(this.EquipmentName);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.RecordView);
			this.splitContainer1.Size = new System.Drawing.Size(624, 419);
			this.splitContainer1.SplitterDistance = 100;
			this.splitContainer1.TabIndex = 0;
			// 
			// ButtonRun
			// 
			this.ButtonRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonRun.Location = new System.Drawing.Point(537, 68);
			this.ButtonRun.Name = "ButtonRun";
			this.ButtonRun.Size = new System.Drawing.Size(75, 23);
			this.ButtonRun.TabIndex = 22;
			this.ButtonRun.Text = "検索";
			this.ButtonRun.UseVisualStyleBackColor = true;
			this.ButtonRun.Click += new System.EventHandler(this.ButtonRun_Click);
			// 
			// MergeRows
			// 
			this.MergeRows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.MergeRows.AutoSize = true;
			this.MergeRows.Location = new System.Drawing.Point(469, 71);
			this.MergeRows.Name = "MergeRows";
			this.MergeRows.Size = new System.Drawing.Size(62, 19);
			this.MergeRows.TabIndex = 21;
			this.MergeRows.Text = "まとめる";
			this.MergeRows.UseVisualStyleBackColor = true;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(371, 13);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(35, 15);
			this.label6.TabIndex = 20;
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
			this.Recipe.TabIndex = 19;
			// 
			// SecretaryName
			// 
			this.SecretaryName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.SecretaryName.FormattingEnabled = true;
			this.SecretaryName.Location = new System.Drawing.Point(188, 68);
			this.SecretaryName.Name = "SecretaryName";
			this.SecretaryName.Size = new System.Drawing.Size(121, 23);
			this.SecretaryName.TabIndex = 18;
			this.SecretaryName.SelectedIndexChanged += new System.EventHandler(this.SecretaryName_SelectedIndexChanged);
			// 
			// SecretaryCategory
			// 
			this.SecretaryCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.SecretaryCategory.FormattingEnabled = true;
			this.SecretaryCategory.Location = new System.Drawing.Point(61, 68);
			this.SecretaryCategory.Name = "SecretaryCategory";
			this.SecretaryCategory.Size = new System.Drawing.Size(121, 23);
			this.SecretaryCategory.TabIndex = 17;
			this.SecretaryCategory.SelectedIndexChanged += new System.EventHandler(this.SecretaryCategory_SelectedIndexChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 71);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(43, 15);
			this.label5.TabIndex = 16;
			this.label5.Text = "秘書艦";
			// 
			// DateEnd
			// 
			this.DateEnd.Location = new System.Drawing.Point(225, 39);
			this.DateEnd.Name = "DateEnd";
			this.DateEnd.Size = new System.Drawing.Size(140, 23);
			this.DateEnd.TabIndex = 15;
			// 
			// DateBegin
			// 
			this.DateBegin.Location = new System.Drawing.Point(225, 10);
			this.DateBegin.Name = "DateBegin";
			this.DateBegin.Size = new System.Drawing.Size(140, 23);
			this.DateBegin.TabIndex = 14;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(188, 42);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(31, 15);
			this.label4.TabIndex = 13;
			this.label4.Text = "終了";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(188, 13);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(31, 15);
			this.label3.TabIndex = 12;
			this.label3.Text = "開始";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 13);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(41, 15);
			this.label2.TabIndex = 3;
			this.label2.Text = "カテゴリ";
			// 
			// EquipmentCategory
			// 
			this.EquipmentCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.EquipmentCategory.FormattingEnabled = true;
			this.EquipmentCategory.Location = new System.Drawing.Point(61, 10);
			this.EquipmentCategory.Name = "EquipmentCategory";
			this.EquipmentCategory.Size = new System.Drawing.Size(121, 23);
			this.EquipmentCategory.TabIndex = 2;
			this.EquipmentCategory.SelectedIndexChanged += new System.EventHandler(this.EquipmentCategory_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 42);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(43, 15);
			this.label1.TabIndex = 1;
			this.label1.Text = "装備名";
			// 
			// EquipmentName
			// 
			this.EquipmentName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.EquipmentName.FormattingEnabled = true;
			this.EquipmentName.Location = new System.Drawing.Point(61, 39);
			this.EquipmentName.Name = "EquipmentName";
			this.EquipmentName.Size = new System.Drawing.Size(121, 23);
			this.EquipmentName.TabIndex = 0;
			this.EquipmentName.SelectedIndexChanged += new System.EventHandler(this.EquipmentName_SelectedIndexChanged);
			// 
			// RecordView
			// 
			this.RecordView.AllowUserToAddRows = false;
			this.RecordView.AllowUserToDeleteRows = false;
			this.RecordView.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.RecordView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.RecordView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.RecordView.ColumnHeadersVisible = false;
			this.RecordView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RecordView_Header,
            this.RecordView_Name,
            this.RecordView_Date,
            this.RecordView_Recipe,
            this.RecordView_FlagshipType,
            this.RecordView_Flagship,
            this.RecordView_Detail});
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(204)))));
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.RecordView.DefaultCellStyle = dataGridViewCellStyle2;
			this.RecordView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RecordView.Location = new System.Drawing.Point(0, 0);
			this.RecordView.Name = "RecordView";
			this.RecordView.ReadOnly = true;
			this.RecordView.RowHeadersVisible = false;
			this.RecordView.RowTemplate.Height = 21;
			this.RecordView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.RecordView.Size = new System.Drawing.Size(624, 315);
			this.RecordView.TabIndex = 0;
			this.RecordView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.RecordView_CellFormatting);
			this.RecordView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.RecordView_SortCompare);
			this.RecordView.Sorted += new System.EventHandler(this.RecordView_Sorted);
			// 
			// ToolTipInfo
			// 
			this.ToolTipInfo.AutoPopDelay = 30000;
			this.ToolTipInfo.InitialDelay = 500;
			this.ToolTipInfo.ReshowDelay = 100;
			this.ToolTipInfo.ShowAlways = true;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusInfo});
			this.statusStrip1.Location = new System.Drawing.Point(0, 419);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(624, 22);
			this.statusStrip1.TabIndex = 1;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// StatusInfo
			// 
			this.StatusInfo.Name = "StatusInfo";
			this.StatusInfo.Size = new System.Drawing.Size(12, 17);
			this.StatusInfo.Text = "-";
			// 
			// Searcher
			// 
			this.Searcher.WorkerSupportsCancellation = true;
			this.Searcher.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Searcher_DoWork);
			this.Searcher.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Searcher_RunWorkerCompleted);
			// 
			// RecordView_Header
			// 
			this.RecordView_Header.HeaderText = "";
			this.RecordView_Header.Name = "RecordView_Header";
			this.RecordView_Header.ReadOnly = true;
			this.RecordView_Header.Width = 50;
			// 
			// RecordView_Name
			// 
			this.RecordView_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
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
			this.RecordView_Recipe.Width = 200;
			// 
			// RecordView_FlagshipType
			// 
			this.RecordView_FlagshipType.HeaderText = "艦種";
			this.RecordView_FlagshipType.Name = "RecordView_FlagshipType";
			this.RecordView_FlagshipType.ReadOnly = true;
			// 
			// RecordView_Flagship
			// 
			this.RecordView_Flagship.HeaderText = "秘書艦";
			this.RecordView_Flagship.Name = "RecordView_Flagship";
			this.RecordView_Flagship.ReadOnly = true;
			// 
			// RecordView_Detail
			// 
			this.RecordView_Detail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.RecordView_Detail.HeaderText = "詳細";
			this.RecordView_Detail.Name = "RecordView_Detail";
			this.RecordView_Detail.ReadOnly = true;
			// 
			// DialogDevelopmentRecordViewer
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(624, 441);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.statusStrip1);
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "DialogDevelopmentRecordViewer";
			this.Text = "開発記録";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogDevelopmentRecordViewer_FormClosed);
			this.Load += new System.EventHandler(this.DialogDevelopmentRecordViewer_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.RecordView)).EndInit();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ComboBox EquipmentName;
		private System.Windows.Forms.DataGridView RecordView;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox EquipmentCategory;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.DateTimePicker DateBegin;
		private System.Windows.Forms.DateTimePicker DateEnd;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ComboBox Recipe;
		private System.Windows.Forms.ComboBox SecretaryName;
		private System.Windows.Forms.ComboBox SecretaryCategory;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button ButtonRun;
		private System.Windows.Forms.CheckBox MergeRows;
		private System.Windows.Forms.ToolTip ToolTipInfo;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.ComponentModel.BackgroundWorker Searcher;
		private System.Windows.Forms.ToolStripStatusLabel StatusInfo;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Header;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Name;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Date;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Recipe;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_FlagshipType;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Flagship;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Detail;
	}
}