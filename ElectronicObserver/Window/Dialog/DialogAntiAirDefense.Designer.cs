namespace ElectronicObserver.Window.Dialog {
	partial class DialogAntiAirDefense {
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.AnnihilationProbability = new System.Windows.Forms.TextBox();
			this.AdjustedFleetAA = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.ShowAll = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.AACutinKind = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.Formation = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.EnemySlotCount = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.FleetID = new System.Windows.Forms.ComboBox();
			this.ResultView = new System.Windows.Forms.DataGridView();
			this.ResultView_ShipName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ResultView_AntiAir = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ResultView_AdjustedAntiAir = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ResultView_ProportionalAirDefense = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ResultView_FixedAirDefense = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ResultView_ShootDownBoth = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ResultView_ShootDownProportional = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ResultView_ShootDownFixed = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ResultView_ShootDownFailed = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ToolTipInfo = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.EnemySlotCount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ResultView)).BeginInit();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.AnnihilationProbability);
			this.splitContainer1.Panel1.Controls.Add(this.AdjustedFleetAA);
			this.splitContainer1.Panel1.Controls.Add(this.label6);
			this.splitContainer1.Panel1.Controls.Add(this.ShowAll);
			this.splitContainer1.Panel1.Controls.Add(this.label5);
			this.splitContainer1.Panel1.Controls.Add(this.label4);
			this.splitContainer1.Panel1.Controls.Add(this.AACutinKind);
			this.splitContainer1.Panel1.Controls.Add(this.label3);
			this.splitContainer1.Panel1.Controls.Add(this.Formation);
			this.splitContainer1.Panel1.Controls.Add(this.label2);
			this.splitContainer1.Panel1.Controls.Add(this.EnemySlotCount);
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			this.splitContainer1.Panel1.Controls.Add(this.FleetID);
			this.splitContainer1.Panel1MinSize = 75;
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.ResultView);
			this.splitContainer1.Size = new System.Drawing.Size(784, 361);
			this.splitContainer1.SplitterDistance = 75;
			this.splitContainer1.TabIndex = 0;
			// 
			// AnnihilationProbability
			// 
			this.AnnihilationProbability.Location = new System.Drawing.Point(600, 41);
			this.AnnihilationProbability.Name = "AnnihilationProbability";
			this.AnnihilationProbability.ReadOnly = true;
			this.AnnihilationProbability.Size = new System.Drawing.Size(80, 23);
			this.AnnihilationProbability.TabIndex = 13;
			this.AnnihilationProbability.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// AdjustedFleetAA
			// 
			this.AdjustedFleetAA.Location = new System.Drawing.Point(441, 41);
			this.AdjustedFleetAA.Name = "AdjustedFleetAA";
			this.AdjustedFleetAA.ReadOnly = true;
			this.AdjustedFleetAA.Size = new System.Drawing.Size(80, 23);
			this.AdjustedFleetAA.TabIndex = 12;
			this.AdjustedFleetAA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(527, 44);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(67, 15);
			this.label6.TabIndex = 11;
			this.label6.Text = "全滅確率：";
			// 
			// ShowAll
			// 
			this.ShowAll.AutoSize = true;
			this.ShowAll.Location = new System.Drawing.Point(539, 14);
			this.ShowAll.Name = "ShowAll";
			this.ShowAll.Size = new System.Drawing.Size(112, 19);
			this.ShowAll.TabIndex = 10;
			this.ShowAll.Text = "すべて選択可能に";
			this.ToolTipInfo.SetToolTip(this.ShowAll, "有効な場合、すべての対空カットインを選択肢に表示します。\r\n無効な場合、現在の艦隊で発動可能なカットインのみを表示します。");
			this.ShowAll.UseVisualStyleBackColor = true;
			this.ShowAll.CheckedChanged += new System.EventHandler(this.ShowAll_CheckedChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(368, 44);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(67, 15);
			this.label5.TabIndex = 8;
			this.label5.Text = "艦隊防空：";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(188, 15);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(85, 15);
			this.label4.TabIndex = 7;
			this.label4.Text = "対空カットイン：";
			// 
			// AACutinKind
			// 
			this.AACutinKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.AACutinKind.FormattingEnabled = true;
			this.AACutinKind.Location = new System.Drawing.Point(282, 12);
			this.AACutinKind.Name = "AACutinKind";
			this.AACutinKind.Size = new System.Drawing.Size(240, 23);
			this.AACutinKind.TabIndex = 6;
			this.AACutinKind.SelectedIndexChanged += new System.EventHandler(this.AACutinKind_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 44);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(43, 15);
			this.label3.TabIndex = 5;
			this.label3.Text = "陣形：";
			// 
			// Formation
			// 
			this.Formation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.Formation.FormattingEnabled = true;
			this.Formation.Items.AddRange(new object[] {
            "単縦陣ほか",
            "複縦陣",
            "輪形陣"});
			this.Formation.Location = new System.Drawing.Point(61, 41);
			this.Formation.Name = "Formation";
			this.Formation.Size = new System.Drawing.Size(121, 23);
			this.Formation.TabIndex = 4;
			this.Formation.SelectedIndexChanged += new System.EventHandler(this.Formation_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(188, 44);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 15);
			this.label2.TabIndex = 3;
			this.label2.Text = "敵スロット機数：";
			// 
			// EnemySlotCount
			// 
			this.EnemySlotCount.Location = new System.Drawing.Point(282, 41);
			this.EnemySlotCount.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
			this.EnemySlotCount.Name = "EnemySlotCount";
			this.EnemySlotCount.Size = new System.Drawing.Size(80, 23);
			this.EnemySlotCount.TabIndex = 2;
			this.EnemySlotCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.EnemySlotCount.Value = new decimal(new int[] {
            36,
            0,
            0,
            0});
			this.EnemySlotCount.ValueChanged += new System.EventHandler(this.EnemySlotCount_ValueChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(43, 15);
			this.label1.TabIndex = 1;
			this.label1.Text = "艦隊：";
			// 
			// FleetID
			// 
			this.FleetID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.FleetID.FormattingEnabled = true;
			this.FleetID.Items.AddRange(new object[] {
            "第1艦隊",
            "第2艦隊",
            "第3艦隊",
            "第4艦隊",
            "連合艦隊"});
			this.FleetID.Location = new System.Drawing.Point(61, 12);
			this.FleetID.Name = "FleetID";
			this.FleetID.Size = new System.Drawing.Size(121, 23);
			this.FleetID.TabIndex = 0;
			this.FleetID.SelectedIndexChanged += new System.EventHandler(this.FleetID_SelectedIndexChanged);
			// 
			// ResultView
			// 
			this.ResultView.AllowUserToAddRows = false;
			this.ResultView.AllowUserToDeleteRows = false;
			this.ResultView.AllowUserToResizeColumns = false;
			this.ResultView.AllowUserToResizeRows = false;
			this.ResultView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.ResultView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ResultView_ShipName,
            this.ResultView_AntiAir,
            this.ResultView_AdjustedAntiAir,
            this.ResultView_ProportionalAirDefense,
            this.ResultView_FixedAirDefense,
            this.ResultView_ShootDownBoth,
            this.ResultView_ShootDownProportional,
            this.ResultView_ShootDownFixed,
            this.ResultView_ShootDownFailed});
			dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle9.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.ResultView.DefaultCellStyle = dataGridViewCellStyle9;
			this.ResultView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ResultView.Location = new System.Drawing.Point(0, 0);
			this.ResultView.Name = "ResultView";
			this.ResultView.ReadOnly = true;
			this.ResultView.RowHeadersVisible = false;
			this.ResultView.RowTemplate.Height = 21;
			this.ResultView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.ResultView.Size = new System.Drawing.Size(784, 282);
			this.ResultView.TabIndex = 0;
			this.ResultView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.ResultView_CellFormatting);
			// 
			// ResultView_ShipName
			// 
			this.ResultView_ShipName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			this.ResultView_ShipName.DefaultCellStyle = dataGridViewCellStyle7;
			this.ResultView_ShipName.HeaderText = "艦名";
			this.ResultView_ShipName.Name = "ResultView_ShipName";
			this.ResultView_ShipName.ReadOnly = true;
			this.ResultView_ShipName.Width = 56;
			// 
			// ResultView_AntiAir
			// 
			this.ResultView_AntiAir.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.ResultView_AntiAir.HeaderText = "対空";
			this.ResultView_AntiAir.Name = "ResultView_AntiAir";
			this.ResultView_AntiAir.ReadOnly = true;
			this.ResultView_AntiAir.Width = 56;
			// 
			// ResultView_AdjustedAntiAir
			// 
			this.ResultView_AdjustedAntiAir.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.ResultView_AdjustedAntiAir.HeaderText = "加重対空";
			this.ResultView_AdjustedAntiAir.Name = "ResultView_AdjustedAntiAir";
			this.ResultView_AdjustedAntiAir.ReadOnly = true;
			this.ResultView_AdjustedAntiAir.Width = 80;
			// 
			// ResultView_ProportionalAirDefense
			// 
			this.ResultView_ProportionalAirDefense.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			dataGridViewCellStyle8.Format = "p2";
			this.ResultView_ProportionalAirDefense.DefaultCellStyle = dataGridViewCellStyle8;
			this.ResultView_ProportionalAirDefense.HeaderText = "割合撃墜";
			this.ResultView_ProportionalAirDefense.Name = "ResultView_ProportionalAirDefense";
			this.ResultView_ProportionalAirDefense.ReadOnly = true;
			this.ResultView_ProportionalAirDefense.Width = 80;
			// 
			// ResultView_FixedAirDefense
			// 
			this.ResultView_FixedAirDefense.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.ResultView_FixedAirDefense.HeaderText = "固定撃墜";
			this.ResultView_FixedAirDefense.Name = "ResultView_FixedAirDefense";
			this.ResultView_FixedAirDefense.ReadOnly = true;
			this.ResultView_FixedAirDefense.Width = 80;
			// 
			// ResultView_ShootDownBoth
			// 
			this.ResultView_ShootDownBoth.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.ResultView_ShootDownBoth.HeaderText = "両方成功";
			this.ResultView_ShootDownBoth.Name = "ResultView_ShootDownBoth";
			this.ResultView_ShootDownBoth.ReadOnly = true;
			this.ResultView_ShootDownBoth.ToolTipText = "割合撃墜・固定撃墜の両方に成功した場合の撃墜数";
			this.ResultView_ShootDownBoth.Width = 80;
			// 
			// ResultView_ShootDownProportional
			// 
			this.ResultView_ShootDownProportional.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.ResultView_ShootDownProportional.HeaderText = "割合のみ";
			this.ResultView_ShootDownProportional.Name = "ResultView_ShootDownProportional";
			this.ResultView_ShootDownProportional.ReadOnly = true;
			this.ResultView_ShootDownProportional.ToolTipText = "割合撃墜に成功した場合の撃墜数";
			this.ResultView_ShootDownProportional.Width = 77;
			// 
			// ResultView_ShootDownFixed
			// 
			this.ResultView_ShootDownFixed.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.ResultView_ShootDownFixed.HeaderText = "固定のみ";
			this.ResultView_ShootDownFixed.Name = "ResultView_ShootDownFixed";
			this.ResultView_ShootDownFixed.ReadOnly = true;
			this.ResultView_ShootDownFixed.ToolTipText = "固定撃墜に成功した場合の撃墜数";
			this.ResultView_ShootDownFixed.Width = 77;
			// 
			// ResultView_ShootDownFailed
			// 
			this.ResultView_ShootDownFailed.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.ResultView_ShootDownFailed.HeaderText = "両方失敗";
			this.ResultView_ShootDownFailed.Name = "ResultView_ShootDownFailed";
			this.ResultView_ShootDownFailed.ReadOnly = true;
			this.ResultView_ShootDownFailed.ToolTipText = "固定撃墜・割合撃墜の両方に失敗した場合の撃墜数";
			this.ResultView_ShootDownFailed.Width = 80;
			// 
			// ToolTipInfo
			// 
			this.ToolTipInfo.AutoPopDelay = 30000;
			this.ToolTipInfo.InitialDelay = 500;
			this.ToolTipInfo.ReshowDelay = 100;
			this.ToolTipInfo.ShowAlways = true;
			// 
			// DialogAntiAirDefense
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(784, 361);
			this.Controls.Add(this.splitContainer1);
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.Name = "DialogAntiAirDefense";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "対空砲火詳細";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogAntiAirDefense_FormClosed);
			this.Load += new System.EventHandler(this.DialogAntiAirDefense_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.EnemySlotCount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ResultView)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox AACutinKind;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox Formation;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown EnemySlotCount;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox FleetID;
		private System.Windows.Forms.DataGridView ResultView;
		private System.Windows.Forms.CheckBox ShowAll;
		private System.Windows.Forms.ToolTip ToolTipInfo;
		private System.Windows.Forms.DataGridViewTextBoxColumn ResultView_ShipName;
		private System.Windows.Forms.DataGridViewTextBoxColumn ResultView_AntiAir;
		private System.Windows.Forms.DataGridViewTextBoxColumn ResultView_AdjustedAntiAir;
		private System.Windows.Forms.DataGridViewTextBoxColumn ResultView_ProportionalAirDefense;
		private System.Windows.Forms.DataGridViewTextBoxColumn ResultView_FixedAirDefense;
		private System.Windows.Forms.DataGridViewTextBoxColumn ResultView_ShootDownBoth;
		private System.Windows.Forms.DataGridViewTextBoxColumn ResultView_ShootDownProportional;
		private System.Windows.Forms.DataGridViewTextBoxColumn ResultView_ShootDownFixed;
		private System.Windows.Forms.DataGridViewTextBoxColumn ResultView_ShootDownFailed;
		private System.Windows.Forms.TextBox AdjustedFleetAA;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox AnnihilationProbability;
	}
}