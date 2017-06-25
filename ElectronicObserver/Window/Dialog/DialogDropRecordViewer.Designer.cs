namespace ElectronicObserver.Window.Dialog {
	partial class DialogDropRecordViewer {
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			this.ItemName = new System.Windows.Forms.ComboBox();
			this.ShipName = new System.Windows.Forms.ComboBox();
			this.EquipmentName = new System.Windows.Forms.ComboBox();
			this.DateBegin = new System.Windows.Forms.DateTimePicker();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.DateEnd = new System.Windows.Forms.DateTimePicker();
			this.RankS = new System.Windows.Forms.CheckBox();
			this.RankA = new System.Windows.Forms.CheckBox();
			this.RankB = new System.Windows.Forms.CheckBox();
			this.RankX = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.MapDifficulty = new System.Windows.Forms.ComboBox();
			this.ButtonRun = new System.Windows.Forms.Button();
			this.RecordView = new System.Windows.Forms.DataGridView();
			this.IsBossOnly = new System.Windows.Forms.CheckBox();
			this.MapAreaID = new System.Windows.Forms.ComboBox();
			this.MapInfoID = new System.Windows.Forms.ComboBox();
			this.MapCellID = new System.Windows.Forms.ComboBox();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.MergeRows = new System.Windows.Forms.CheckBox();
			this.LabelShipName = new ElectronicObserver.Window.Control.ImageLabel();
			this.LabelItemName = new ElectronicObserver.Window.Control.ImageLabel();
			this.LabelEquipmentName = new ElectronicObserver.Window.Control.ImageLabel();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.StatusInfo = new System.Windows.Forms.ToolStripStatusLabel();
			this.ToolTipInfo = new System.Windows.Forms.ToolTip(this.components);
			this.Searcher = new System.ComponentModel.BackgroundWorker();
			this.RecordView_Header = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Map = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Rank = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_RankS = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_RankA = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_RankB = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.RecordView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// ItemName
			// 
			this.ItemName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ItemName.FormattingEnabled = true;
			this.ItemName.Location = new System.Drawing.Point(75, 39);
			this.ItemName.Name = "ItemName";
			this.ItemName.Size = new System.Drawing.Size(121, 23);
			this.ItemName.TabIndex = 3;
			this.ToolTipInfo.SetToolTip(this.ItemName, "検索するアイテム名を指定します。\r\n(ドロップ) はアイテムのドロップが発生した場合のみ抽出します。 ");
			// 
			// ShipName
			// 
			this.ShipName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.ShipName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.ShipName.FormattingEnabled = true;
			this.ShipName.Location = new System.Drawing.Point(75, 10);
			this.ShipName.Name = "ShipName";
			this.ShipName.Size = new System.Drawing.Size(121, 23);
			this.ShipName.TabIndex = 1;
			this.ToolTipInfo.SetToolTip(this.ShipName, "検索する艦船名を指定します。\r\n(ドロップ) は艦娘のドロップが発生した場合のみ抽出します。 ");
			// 
			// EquipmentName
			// 
			this.EquipmentName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.EquipmentName.Enabled = false;
			this.EquipmentName.FormattingEnabled = true;
			this.EquipmentName.Location = new System.Drawing.Point(75, 68);
			this.EquipmentName.Name = "EquipmentName";
			this.EquipmentName.Size = new System.Drawing.Size(121, 23);
			this.EquipmentName.TabIndex = 5;
			this.ToolTipInfo.SetToolTip(this.EquipmentName, "検索する装備名を指定します。\r\n(ドロップ) は装備のドロップが発生した場合のみ抽出します。 ");
			this.EquipmentName.Visible = false;
			// 
			// DateBegin
			// 
			this.DateBegin.CustomFormat = "yyyy/MM/dd";
			this.DateBegin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.DateBegin.Location = new System.Drawing.Point(239, 10);
			this.DateBegin.Name = "DateBegin";
			this.DateBegin.Size = new System.Drawing.Size(140, 23);
			this.DateBegin.TabIndex = 7;
			this.ToolTipInfo.SetToolTip(this.DateBegin, "検索する日時の始点を指定します。");
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(202, 13);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(31, 15);
			this.label2.TabIndex = 6;
			this.label2.Text = "開始";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(202, 42);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(31, 15);
			this.label3.TabIndex = 8;
			this.label3.Text = "終了";
			// 
			// DateEnd
			// 
			this.DateEnd.CustomFormat = "yyyy/MM/dd";
			this.DateEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.DateEnd.Location = new System.Drawing.Point(239, 39);
			this.DateEnd.Name = "DateEnd";
			this.DateEnd.Size = new System.Drawing.Size(140, 23);
			this.DateEnd.TabIndex = 9;
			this.ToolTipInfo.SetToolTip(this.DateEnd, "検索する日時の終点を指定します。");
			// 
			// RankS
			// 
			this.RankS.AutoSize = true;
			this.RankS.Checked = true;
			this.RankS.CheckState = System.Windows.Forms.CheckState.Checked;
			this.RankS.Location = new System.Drawing.Point(225, 70);
			this.RankS.Name = "RankS";
			this.RankS.Size = new System.Drawing.Size(34, 19);
			this.RankS.TabIndex = 10;
			this.RankS.Text = "S";
			this.ToolTipInfo.SetToolTip(this.RankS, "S勝利");
			this.RankS.UseVisualStyleBackColor = true;
			// 
			// RankA
			// 
			this.RankA.AutoSize = true;
			this.RankA.Checked = true;
			this.RankA.CheckState = System.Windows.Forms.CheckState.Checked;
			this.RankA.Location = new System.Drawing.Point(265, 70);
			this.RankA.Name = "RankA";
			this.RankA.Size = new System.Drawing.Size(34, 19);
			this.RankA.TabIndex = 11;
			this.RankA.Text = "A";
			this.ToolTipInfo.SetToolTip(this.RankA, "A勝利");
			this.RankA.UseVisualStyleBackColor = true;
			// 
			// RankB
			// 
			this.RankB.AutoSize = true;
			this.RankB.Checked = true;
			this.RankB.CheckState = System.Windows.Forms.CheckState.Checked;
			this.RankB.Location = new System.Drawing.Point(305, 70);
			this.RankB.Name = "RankB";
			this.RankB.Size = new System.Drawing.Size(34, 19);
			this.RankB.TabIndex = 12;
			this.RankB.Text = "B";
			this.ToolTipInfo.SetToolTip(this.RankB, "B勝利");
			this.RankB.UseVisualStyleBackColor = true;
			// 
			// RankX
			// 
			this.RankX.AutoSize = true;
			this.RankX.Checked = true;
			this.RankX.CheckState = System.Windows.Forms.CheckState.Checked;
			this.RankX.Location = new System.Drawing.Point(345, 70);
			this.RankX.Name = "RankX";
			this.RankX.Size = new System.Drawing.Size(34, 19);
			this.RankX.TabIndex = 13;
			this.RankX.Text = "X";
			this.ToolTipInfo.SetToolTip(this.RankX, "敗北");
			this.RankX.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(385, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(57, 15);
			this.label1.TabIndex = 14;
			this.label1.Text = "海域・セル";
			// 
			// MapDifficulty
			// 
			this.MapDifficulty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.MapDifficulty.FormattingEnabled = true;
			this.MapDifficulty.Location = new System.Drawing.Point(444, 10);
			this.MapDifficulty.Name = "MapDifficulty";
			this.MapDifficulty.Size = new System.Drawing.Size(69, 23);
			this.MapDifficulty.TabIndex = 15;
			this.ToolTipInfo.SetToolTip(this.MapDifficulty, "難易度を指定します。\r\n* はすべての難易度を抽出します。");
			// 
			// ButtonRun
			// 
			this.ButtonRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonRun.Location = new System.Drawing.Point(543, 68);
			this.ButtonRun.Name = "ButtonRun";
			this.ButtonRun.Size = new System.Drawing.Size(69, 23);
			this.ButtonRun.TabIndex = 21;
			this.ButtonRun.Text = "検索";
			this.ButtonRun.UseVisualStyleBackColor = true;
			this.ButtonRun.Click += new System.EventHandler(this.ButtonRun_Click);
			// 
			// RecordView
			// 
			this.RecordView.AllowUserToAddRows = false;
			this.RecordView.AllowUserToDeleteRows = false;
			this.RecordView.AllowUserToResizeRows = false;
			this.RecordView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.RecordView.ColumnHeadersVisible = false;
			this.RecordView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RecordView_Header,
            this.RecordView_Name,
            this.RecordView_Date,
            this.RecordView_Map,
            this.RecordView_Rank,
            this.RecordView_RankS,
            this.RecordView_RankA,
            this.RecordView_RankB});
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle5.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(204)))));
			dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.RecordView.DefaultCellStyle = dataGridViewCellStyle5;
			this.RecordView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RecordView.Location = new System.Drawing.Point(0, 0);
			this.RecordView.Name = "RecordView";
			this.RecordView.ReadOnly = true;
			this.RecordView.RowHeadersVisible = false;
			this.RecordView.RowTemplate.Height = 21;
			this.RecordView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.RecordView.Size = new System.Drawing.Size(624, 315);
			this.RecordView.TabIndex = 1;
			this.RecordView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.RecordView_CellFormatting);
			this.RecordView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.RecordView_SortCompare);
			this.RecordView.Sorted += new System.EventHandler(this.RecordView_Sorted);
			this.RecordView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.RecordView_CellDoubleClick);
			// 
			// IsBossOnly
			// 
			this.IsBossOnly.AutoSize = true;
			this.IsBossOnly.Checked = true;
			this.IsBossOnly.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			this.IsBossOnly.Location = new System.Drawing.Point(519, 12);
			this.IsBossOnly.Name = "IsBossOnly";
			this.IsBossOnly.Size = new System.Drawing.Size(53, 19);
			this.IsBossOnly.TabIndex = 16;
			this.IsBossOnly.Text = "Boss";
			this.IsBossOnly.ThreeState = true;
			this.IsBossOnly.UseVisualStyleBackColor = true;
			// 
			// MapAreaID
			// 
			this.MapAreaID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.MapAreaID.FormattingEnabled = true;
			this.MapAreaID.Location = new System.Drawing.Point(388, 39);
			this.MapAreaID.Name = "MapAreaID";
			this.MapAreaID.Size = new System.Drawing.Size(50, 23);
			this.MapAreaID.TabIndex = 17;
			this.ToolTipInfo.SetToolTip(this.MapAreaID, "海域IDを指定します。\r\n* はすべての海域を抽出します。\r\n");
			this.MapAreaID.SelectedIndexChanged += new System.EventHandler(this.MapAreaID_SelectedIndexChanged);
			// 
			// MapInfoID
			// 
			this.MapInfoID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.MapInfoID.FormattingEnabled = true;
			this.MapInfoID.Location = new System.Drawing.Point(444, 39);
			this.MapInfoID.Name = "MapInfoID";
			this.MapInfoID.Size = new System.Drawing.Size(50, 23);
			this.MapInfoID.TabIndex = 18;
			this.ToolTipInfo.SetToolTip(this.MapInfoID, "海域IDを指定します。\r\n* はすべての海域を抽出します。\r\n");
			this.MapInfoID.SelectedIndexChanged += new System.EventHandler(this.MapAreaID_SelectedIndexChanged);
			// 
			// MapCellID
			// 
			this.MapCellID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.MapCellID.FormattingEnabled = true;
			this.MapCellID.Location = new System.Drawing.Point(500, 39);
			this.MapCellID.Name = "MapCellID";
			this.MapCellID.Size = new System.Drawing.Size(50, 23);
			this.MapCellID.TabIndex = 19;
			this.ToolTipInfo.SetToolTip(this.MapCellID, "セルIDを指定します。\r\n* はすべてのセルを抽出します。\r\n");
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
			this.splitContainer1.Panel1.Controls.Add(this.MergeRows);
			this.splitContainer1.Panel1.Controls.Add(this.MapCellID);
			this.splitContainer1.Panel1.Controls.Add(this.LabelShipName);
			this.splitContainer1.Panel1.Controls.Add(this.MapInfoID);
			this.splitContainer1.Panel1.Controls.Add(this.LabelItemName);
			this.splitContainer1.Panel1.Controls.Add(this.MapAreaID);
			this.splitContainer1.Panel1.Controls.Add(this.LabelEquipmentName);
			this.splitContainer1.Panel1.Controls.Add(this.IsBossOnly);
			this.splitContainer1.Panel1.Controls.Add(this.ItemName);
			this.splitContainer1.Panel1.Controls.Add(this.ButtonRun);
			this.splitContainer1.Panel1.Controls.Add(this.ShipName);
			this.splitContainer1.Panel1.Controls.Add(this.MapDifficulty);
			this.splitContainer1.Panel1.Controls.Add(this.EquipmentName);
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			this.splitContainer1.Panel1.Controls.Add(this.DateBegin);
			this.splitContainer1.Panel1.Controls.Add(this.RankX);
			this.splitContainer1.Panel1.Controls.Add(this.label2);
			this.splitContainer1.Panel1.Controls.Add(this.RankB);
			this.splitContainer1.Panel1.Controls.Add(this.DateEnd);
			this.splitContainer1.Panel1.Controls.Add(this.RankA);
			this.splitContainer1.Panel1.Controls.Add(this.label3);
			this.splitContainer1.Panel1.Controls.Add(this.RankS);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.RecordView);
			this.splitContainer1.Size = new System.Drawing.Size(624, 419);
			this.splitContainer1.SplitterDistance = 100;
			this.splitContainer1.TabIndex = 2;
			// 
			// MergeRows
			// 
			this.MergeRows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.MergeRows.AutoSize = true;
			this.MergeRows.Location = new System.Drawing.Point(475, 70);
			this.MergeRows.Name = "MergeRows";
			this.MergeRows.Size = new System.Drawing.Size(62, 19);
			this.MergeRows.TabIndex = 20;
			this.MergeRows.Text = "まとめる";
			this.ToolTipInfo.SetToolTip(this.MergeRows, "チェックすると同じドロップ項目をまとめて表示します。\r\n");
			this.MergeRows.UseVisualStyleBackColor = true;
			// 
			// LabelShipName
			// 
			this.LabelShipName.AutoSize = false;
			this.LabelShipName.BackColor = System.Drawing.Color.Transparent;
			this.LabelShipName.Location = new System.Drawing.Point(12, 13);
			this.LabelShipName.Name = "LabelShipName";
			this.LabelShipName.Size = new System.Drawing.Size(57, 16);
			this.LabelShipName.TabIndex = 0;
			this.LabelShipName.Text = "艦船";
			this.LabelShipName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// LabelItemName
			// 
			this.LabelItemName.AutoSize = false;
			this.LabelItemName.BackColor = System.Drawing.Color.Transparent;
			this.LabelItemName.Location = new System.Drawing.Point(12, 42);
			this.LabelItemName.Name = "LabelItemName";
			this.LabelItemName.Size = new System.Drawing.Size(57, 16);
			this.LabelItemName.TabIndex = 2;
			this.LabelItemName.Text = "アイテム";
			this.LabelItemName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// LabelEquipmentName
			// 
			this.LabelEquipmentName.AutoSize = false;
			this.LabelEquipmentName.BackColor = System.Drawing.Color.Transparent;
			this.LabelEquipmentName.Enabled = false;
			this.LabelEquipmentName.Location = new System.Drawing.Point(12, 70);
			this.LabelEquipmentName.Name = "LabelEquipmentName";
			this.LabelEquipmentName.Size = new System.Drawing.Size(57, 16);
			this.LabelEquipmentName.TabIndex = 4;
			this.LabelEquipmentName.Text = "装備";
			this.LabelEquipmentName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.LabelEquipmentName.Visible = false;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusInfo});
			this.statusStrip1.Location = new System.Drawing.Point(0, 419);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(624, 22);
			this.statusStrip1.TabIndex = 3;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// StatusInfo
			// 
			this.StatusInfo.Name = "StatusInfo";
			this.StatusInfo.Size = new System.Drawing.Size(12, 17);
			this.StatusInfo.Text = "-";
			// 
			// ToolTipInfo
			// 
			this.ToolTipInfo.AutoPopDelay = 30000;
			this.ToolTipInfo.InitialDelay = 500;
			this.ToolTipInfo.ReshowDelay = 100;
			this.ToolTipInfo.ShowAlways = true;
			// 
			// Searcher
			// 
			this.Searcher.WorkerSupportsCancellation = true;
			this.Searcher.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Searcher_DoWork);
			this.Searcher.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Searcher_RunWorkerCompleted);
			// 
			// RecordView_Header
			// 
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.RecordView_Header.DefaultCellStyle = dataGridViewCellStyle1;
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
			this.RecordView_Date.Width = 150;
			// 
			// RecordView_Map
			// 
			this.RecordView_Map.HeaderText = "海域";
			this.RecordView_Map.Name = "RecordView_Map";
			this.RecordView_Map.ReadOnly = true;
			this.RecordView_Map.Width = 120;
			// 
			// RecordView_Rank
			// 
			this.RecordView_Rank.HeaderText = "ランク";
			this.RecordView_Rank.Name = "RecordView_Rank";
			this.RecordView_Rank.ReadOnly = true;
			this.RecordView_Rank.Width = 40;
			// 
			// RecordView_RankS
			// 
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.RecordView_RankS.DefaultCellStyle = dataGridViewCellStyle2;
			this.RecordView_RankS.HeaderText = "S勝利";
			this.RecordView_RankS.Name = "RecordView_RankS";
			this.RecordView_RankS.ReadOnly = true;
			// 
			// RecordView_RankA
			// 
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.RecordView_RankA.DefaultCellStyle = dataGridViewCellStyle3;
			this.RecordView_RankA.HeaderText = "A勝利";
			this.RecordView_RankA.Name = "RecordView_RankA";
			this.RecordView_RankA.ReadOnly = true;
			// 
			// RecordView_RankB
			// 
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.RecordView_RankB.DefaultCellStyle = dataGridViewCellStyle4;
			this.RecordView_RankB.HeaderText = "B勝利";
			this.RecordView_RankB.Name = "RecordView_RankB";
			this.RecordView_RankB.ReadOnly = true;
			// 
			// DialogDropRecordViewer
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(624, 441);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.statusStrip1);
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.Name = "DialogDropRecordViewer";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ドロップ記録";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogDropRecordViewer_FormClosed);
			this.Load += new System.EventHandler(this.DialogDropRecordViewer_Load);
			((System.ComponentModel.ISupportInitialize)(this.RecordView)).EndInit();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox IsBossOnly;
		private System.Windows.Forms.Button ButtonRun;
		private System.Windows.Forms.ComboBox MapDifficulty;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox RankX;
		private System.Windows.Forms.CheckBox RankB;
		private System.Windows.Forms.CheckBox RankA;
		private System.Windows.Forms.CheckBox RankS;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.DateTimePicker DateEnd;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.DateTimePicker DateBegin;
		private System.Windows.Forms.ComboBox EquipmentName;
		private System.Windows.Forms.ComboBox ShipName;
		private System.Windows.Forms.ComboBox ItemName;
		private Control.ImageLabel LabelEquipmentName;
		private Control.ImageLabel LabelItemName;
		private Control.ImageLabel LabelShipName;
		private System.Windows.Forms.DataGridView RecordView;
		private System.Windows.Forms.ComboBox MapCellID;
		private System.Windows.Forms.ComboBox MapInfoID;
		private System.Windows.Forms.ComboBox MapAreaID;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.CheckBox MergeRows;
		private System.Windows.Forms.ToolTip ToolTipInfo;
		private System.Windows.Forms.ToolStripStatusLabel StatusInfo;
		private System.ComponentModel.BackgroundWorker Searcher;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Header;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Name;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Date;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Map;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Rank;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_RankS;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_RankA;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_RankB;
	}
}