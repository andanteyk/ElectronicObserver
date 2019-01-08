namespace ElectronicObserver.Window.Dialog
{
	partial class DialogExpChecker
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.TextShip = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.SearchInFleet = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.ShowAllASWEquipments = new System.Windows.Forms.CheckBox();
			this.ShowAllLevel = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.ASWModernization = new System.Windows.Forms.NumericUpDown();
			this.ExpUnit = new System.Windows.Forms.NumericUpDown();
			this.GroupExp = new System.Windows.Forms.GroupBox();
			this.LevelView = new System.Windows.Forms.DataGridView();
			this.ColumnLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColumnExp = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColumnSortieCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColumnASW = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColumnEquipment = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ToolTipInfo = new System.Windows.Forms.ToolTip(this.components);
			this.LabelAlert = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ASWModernization)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ExpUnit)).BeginInit();
			this.GroupExp.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.LevelView)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.LabelAlert);
			this.groupBox1.Controls.Add(this.TextShip);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.SearchInFleet);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.ShowAllASWEquipments);
			this.groupBox1.Controls.Add(this.ShowAllLevel);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.ASWModernization);
			this.groupBox1.Controls.Add(this.ExpUnit);
			this.groupBox1.Location = new System.Drawing.Point(13, 13);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(599, 86);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "表示条件";
			// 
			// TextShip
			// 
			this.TextShip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TextShip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TextShip.FormattingEnabled = true;
			this.TextShip.Location = new System.Drawing.Point(55, 22);
			this.TextShip.MinimumSize = new System.Drawing.Size(150, 0);
			this.TextShip.Name = "TextShip";
			this.TextShip.Size = new System.Drawing.Size(245, 23);
			this.TextShip.TabIndex = 1;
			this.TextShip.SelectedIndexChanged += new System.EventHandler(this.TextShip_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 25);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(43, 15);
			this.label3.TabIndex = 0;
			this.label3.Text = "対象艦";
			// 
			// SearchInFleet
			// 
			this.SearchInFleet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.SearchInFleet.AutoSize = true;
			this.SearchInFleet.Location = new System.Drawing.Point(306, 24);
			this.SearchInFleet.Name = "SearchInFleet";
			this.SearchInFleet.Size = new System.Drawing.Size(90, 19);
			this.SearchInFleet.TabIndex = 2;
			this.SearchInFleet.Text = "艦隊から探す";
			this.ToolTipInfo.SetToolTip(this.SearchInFleet, "現在艦隊に所属している艦のみ選択可能にします。");
			this.SearchInFleet.UseVisualStyleBackColor = true;
			this.SearchInFleet.CheckedChanged += new System.EventHandler(this.SearchInFleet_CheckedChanged);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(389, 61);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(118, 15);
			this.label1.TabIndex = 7;
			this.label1.Text = "出撃あたりの獲得Exp:";
			// 
			// ShowAllASWEquipments
			// 
			this.ShowAllASWEquipments.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ShowAllASWEquipments.AutoSize = true;
			this.ShowAllASWEquipments.Location = new System.Drawing.Point(483, 24);
			this.ShowAllASWEquipments.Name = "ShowAllASWEquipments";
			this.ShowAllASWEquipments.Size = new System.Drawing.Size(110, 19);
			this.ShowAllASWEquipments.TabIndex = 4;
			this.ShowAllASWEquipments.Text = "対潜装備全表示";
			this.ToolTipInfo.SetToolTip(this.ShowAllASWEquipments, "すべてのソナー・爆雷の組み合わせを表示します。");
			this.ShowAllASWEquipments.UseVisualStyleBackColor = true;
			this.ShowAllASWEquipments.CheckedChanged += new System.EventHandler(this.ShowAllASWEquipments_CheckedChanged);
			// 
			// ShowAllLevel
			// 
			this.ShowAllLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ShowAllLevel.AutoSize = true;
			this.ShowAllLevel.Location = new System.Drawing.Point(402, 24);
			this.ShowAllLevel.Name = "ShowAllLevel";
			this.ShowAllLevel.Size = new System.Drawing.Size(75, 19);
			this.ShowAllLevel.TabIndex = 3;
			this.ShowAllLevel.Text = "全Lv表示";
			this.ToolTipInfo.SetToolTip(this.ShowAllLevel, "有効な時は Lv. 1 から表示します。\r\n無効な時は次のレベルから表示します。");
			this.ShowAllLevel.UseVisualStyleBackColor = true;
			this.ShowAllLevel.CheckedChanged += new System.EventHandler(this.ShowAllLevel_CheckedChanged);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(282, 61);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(55, 15);
			this.label2.TabIndex = 5;
			this.label2.Text = "対潜改修";
			// 
			// ASWModernization
			// 
			this.ASWModernization.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ASWModernization.Location = new System.Drawing.Point(343, 57);
			this.ASWModernization.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
			this.ASWModernization.Name = "ASWModernization";
			this.ASWModernization.Size = new System.Drawing.Size(40, 23);
			this.ASWModernization.TabIndex = 6;
			this.ASWModernization.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.ToolTipInfo.SetToolTip(this.ASWModernization, "対潜改修値を現在値から変更します。");
			this.ASWModernization.ValueChanged += new System.EventHandler(this.ASWModernization_ValueChanged);
			// 
			// ExpUnit
			// 
			this.ExpUnit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ExpUnit.Location = new System.Drawing.Point(513, 57);
			this.ExpUnit.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
			this.ExpUnit.Name = "ExpUnit";
			this.ExpUnit.Size = new System.Drawing.Size(80, 23);
			this.ExpUnit.TabIndex = 8;
			this.ExpUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.ToolTipInfo.SetToolTip(this.ExpUnit, "1回の出撃当たりの獲得Exp. を入力すると、出撃回数が計算できます。");
			this.ExpUnit.Value = new decimal(new int[] {
            2268,
            0,
            0,
            0});
			this.ExpUnit.ValueChanged += new System.EventHandler(this.ExpUnit_ValueChanged);
			// 
			// GroupExp
			// 
			this.GroupExp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.GroupExp.Controls.Add(this.LevelView);
			this.GroupExp.Location = new System.Drawing.Point(12, 105);
			this.GroupExp.Name = "GroupExp";
			this.GroupExp.Size = new System.Drawing.Size(600, 324);
			this.GroupExp.TabIndex = 1;
			this.GroupExp.TabStop = false;
			this.GroupExp.Text = "必要Exp";
			// 
			// LevelView
			// 
			this.LevelView.AllowUserToAddRows = false;
			this.LevelView.AllowUserToDeleteRows = false;
			this.LevelView.AllowUserToResizeRows = false;
			this.LevelView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LevelView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.LevelView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnLevel,
            this.ColumnExp,
            this.ColumnSortieCount,
            this.ColumnASW,
            this.ColumnEquipment});
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle5.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(204)))));
			dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.LevelView.DefaultCellStyle = dataGridViewCellStyle5;
			this.LevelView.Location = new System.Drawing.Point(7, 22);
			this.LevelView.Name = "LevelView";
			this.LevelView.ReadOnly = true;
			this.LevelView.RowHeadersVisible = false;
			this.LevelView.RowTemplate.Height = 21;
			this.LevelView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.LevelView.Size = new System.Drawing.Size(587, 296);
			this.LevelView.TabIndex = 0;
			this.LevelView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.LevelView_CellFormatting);
			// 
			// ColumnLevel
			// 
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.ColumnLevel.DefaultCellStyle = dataGridViewCellStyle1;
			this.ColumnLevel.HeaderText = "Lv";
			this.ColumnLevel.Name = "ColumnLevel";
			this.ColumnLevel.ReadOnly = true;
			this.ColumnLevel.Width = 40;
			// 
			// ColumnExp
			// 
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.ColumnExp.DefaultCellStyle = dataGridViewCellStyle2;
			this.ColumnExp.HeaderText = "必要Exp";
			this.ColumnExp.Name = "ColumnExp";
			this.ColumnExp.ReadOnly = true;
			this.ColumnExp.Width = 72;
			// 
			// ColumnSortieCount
			// 
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.ColumnSortieCount.DefaultCellStyle = dataGridViewCellStyle3;
			this.ColumnSortieCount.HeaderText = "出撃回数";
			this.ColumnSortieCount.Name = "ColumnSortieCount";
			this.ColumnSortieCount.ReadOnly = true;
			this.ColumnSortieCount.Width = 72;
			// 
			// ColumnASW
			// 
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.ColumnASW.DefaultCellStyle = dataGridViewCellStyle4;
			this.ColumnASW.HeaderText = "対潜";
			this.ColumnASW.Name = "ColumnASW";
			this.ColumnASW.ReadOnly = true;
			this.ColumnASW.Width = 40;
			// 
			// ColumnEquipment
			// 
			this.ColumnEquipment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.ColumnEquipment.HeaderText = "先制対潜装備";
			this.ColumnEquipment.Name = "ColumnEquipment";
			this.ColumnEquipment.ReadOnly = true;
			this.ColumnEquipment.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// ToolTipInfo
			// 
			this.ToolTipInfo.AutoPopDelay = 30000;
			this.ToolTipInfo.InitialDelay = 500;
			this.ToolTipInfo.ReshowDelay = 100;
			this.ToolTipInfo.ShowAlways = true;
			// 
			// LabelAlert
			// 
			this.LabelAlert.AutoSize = true;
			this.LabelAlert.ForeColor = System.Drawing.Color.Red;
			this.LabelAlert.Location = new System.Drawing.Point(8, 61);
			this.LabelAlert.Name = "LabelAlert";
			this.LabelAlert.Size = new System.Drawing.Size(19, 15);
			this.LabelAlert.TabIndex = 9;
			this.LabelAlert.Text = "！";
			// 
			// DialogExpChecker
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(624, 441);
			this.Controls.Add(this.GroupExp);
			this.Controls.Add(this.groupBox1);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.Name = "DialogExpChecker";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "必要経験値計算";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogExpChecker_FormClosed);
			this.Load += new System.EventHandler(this.DialogExpChecker_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ASWModernization)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ExpUnit)).EndInit();
			this.GroupExp.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.LevelView)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox TextShip;
		private System.Windows.Forms.GroupBox GroupExp;
		private System.Windows.Forms.DataGridView LevelView;
		private System.Windows.Forms.NumericUpDown ExpUnit;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown ASWModernization;
		private System.Windows.Forms.CheckBox ShowAllASWEquipments;
		private System.Windows.Forms.CheckBox ShowAllLevel;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox SearchInFleet;
		private System.Windows.Forms.ToolTip ToolTipInfo;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLevel;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColumnExp;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSortieCount;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColumnASW;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColumnEquipment;
		private System.Windows.Forms.Label LabelAlert;
	}
}