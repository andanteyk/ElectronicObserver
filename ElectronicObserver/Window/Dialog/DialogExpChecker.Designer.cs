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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.LabelShip = new System.Windows.Forms.Label();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.ButtonStart = new System.Windows.Forms.Button();
			this.TextShip = new System.Windows.Forms.ComboBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.LevelView = new System.Windows.Forms.DataGridView();
			this.ASWModernization = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.ShowAllLevel = new System.Windows.Forms.CheckBox();
			this.ShowAllASWEquipments = new System.Windows.Forms.CheckBox();
			this.ColumnLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColumnExp = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColumnSortieCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColumnASW = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColumnEquipment = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.LevelView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ASWModernization)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.ShowAllASWEquipments);
			this.groupBox1.Controls.Add(this.ShowAllLevel);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.ASWModernization);
			this.groupBox1.Controls.Add(this.LabelShip);
			this.groupBox1.Controls.Add(this.numericUpDown1);
			this.groupBox1.Controls.Add(this.ButtonStart);
			this.groupBox1.Controls.Add(this.TextShip);
			this.groupBox1.Location = new System.Drawing.Point(13, 13);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(599, 86);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "表示条件";
			// 
			// LabelShip
			// 
			this.LabelShip.AutoSize = true;
			this.LabelShip.Location = new System.Drawing.Point(6, 61);
			this.LabelShip.Name = "LabelShip";
			this.LabelShip.Size = new System.Drawing.Size(32, 15);
			this.LabelShip.TabIndex = 4;
			this.LabelShip.Text = "Exp.";
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.numericUpDown1.Location = new System.Drawing.Point(432, 57);
			this.numericUpDown1.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(80, 23);
			this.numericUpDown1.TabIndex = 8;
			this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDown1.Value = new decimal(new int[] {
            2268,
            0,
            0,
            0});
			// 
			// ButtonStart
			// 
			this.ButtonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonStart.Location = new System.Drawing.Point(518, 57);
			this.ButtonStart.Name = "ButtonStart";
			this.ButtonStart.Size = new System.Drawing.Size(75, 23);
			this.ButtonStart.TabIndex = 9;
			this.ButtonStart.Text = "表示";
			this.ButtonStart.UseVisualStyleBackColor = true;
			this.ButtonStart.Click += new System.EventHandler(this.ButtonStart_Click);
			// 
			// TextShip
			// 
			this.TextShip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TextShip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TextShip.FormattingEnabled = true;
			this.TextShip.Location = new System.Drawing.Point(55, 22);
			this.TextShip.Name = "TextShip";
			this.TextShip.Size = new System.Drawing.Size(341, 23);
			this.TextShip.TabIndex = 1;
			this.TextShip.SelectionChangeCommitted += new System.EventHandler(this.TextShip_SelectionChangeCommitted);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.LevelView);
			this.groupBox2.Location = new System.Drawing.Point(12, 105);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(600, 324);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "必要Exp";
			// 
			// LevelView
			// 
			this.LevelView.AllowUserToAddRows = false;
			this.LevelView.AllowUserToDeleteRows = false;
			this.LevelView.AllowUserToResizeRows = false;
			this.LevelView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LevelView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.LevelView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnLevel,
            this.ColumnExp,
            this.ColumnSortieCount,
            this.ColumnASW,
            this.ColumnEquipment});
			this.LevelView.Location = new System.Drawing.Point(7, 22);
			this.LevelView.Name = "LevelView";
			this.LevelView.ReadOnly = true;
			this.LevelView.RowHeadersVisible = false;
			this.LevelView.RowTemplate.Height = 21;
			this.LevelView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.LevelView.Size = new System.Drawing.Size(587, 296);
			this.LevelView.TabIndex = 0;
			// 
			// ASWModernization
			// 
			this.ASWModernization.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ASWModernization.Location = new System.Drawing.Point(262, 57);
			this.ASWModernization.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
			this.ASWModernization.Name = "ASWModernization";
			this.ASWModernization.Size = new System.Drawing.Size(40, 23);
			this.ASWModernization.TabIndex = 6;
			this.ASWModernization.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(201, 61);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(55, 15);
			this.label2.TabIndex = 5;
			this.label2.Text = "対潜改修";
			// 
			// ShowAllLevel
			// 
			this.ShowAllLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ShowAllLevel.AutoSize = true;
			this.ShowAllLevel.Location = new System.Drawing.Point(402, 24);
			this.ShowAllLevel.Name = "ShowAllLevel";
			this.ShowAllLevel.Size = new System.Drawing.Size(75, 19);
			this.ShowAllLevel.TabIndex = 2;
			this.ShowAllLevel.Text = "全Lv表示";
			this.ShowAllLevel.UseVisualStyleBackColor = true;
			// 
			// ShowAllASWEquipments
			// 
			this.ShowAllASWEquipments.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ShowAllASWEquipments.AutoSize = true;
			this.ShowAllASWEquipments.Location = new System.Drawing.Point(483, 24);
			this.ShowAllASWEquipments.Name = "ShowAllASWEquipments";
			this.ShowAllASWEquipments.Size = new System.Drawing.Size(110, 19);
			this.ShowAllASWEquipments.TabIndex = 3;
			this.ShowAllASWEquipments.Text = "対潜装備全表示";
			this.ShowAllASWEquipments.UseVisualStyleBackColor = true;
			// 
			// ColumnLevel
			// 
			dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.ColumnLevel.DefaultCellStyle = dataGridViewCellStyle9;
			this.ColumnLevel.HeaderText = "Lv";
			this.ColumnLevel.Name = "ColumnLevel";
			this.ColumnLevel.ReadOnly = true;
			this.ColumnLevel.Width = 60;
			// 
			// ColumnExp
			// 
			dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.ColumnExp.DefaultCellStyle = dataGridViewCellStyle10;
			this.ColumnExp.HeaderText = "必要Exp";
			this.ColumnExp.Name = "ColumnExp";
			this.ColumnExp.ReadOnly = true;
			// 
			// ColumnSortieCount
			// 
			dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.ColumnSortieCount.DefaultCellStyle = dataGridViewCellStyle11;
			this.ColumnSortieCount.HeaderText = "出撃回数";
			this.ColumnSortieCount.Name = "ColumnSortieCount";
			this.ColumnSortieCount.ReadOnly = true;
			this.ColumnSortieCount.Width = 80;
			// 
			// ColumnASW
			// 
			dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.ColumnASW.DefaultCellStyle = dataGridViewCellStyle12;
			this.ColumnASW.HeaderText = "対潜";
			this.ColumnASW.Name = "ColumnASW";
			this.ColumnASW.ReadOnly = true;
			this.ColumnASW.Width = 60;
			// 
			// ColumnEquipment
			// 
			this.ColumnEquipment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.ColumnEquipment.HeaderText = "先制対潜装備";
			this.ColumnEquipment.Name = "ColumnEquipment";
			this.ColumnEquipment.ReadOnly = true;
			this.ColumnEquipment.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(308, 61);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(118, 15);
			this.label1.TabIndex = 7;
			this.label1.Text = "出撃あたりの獲得Exp:";
			// 
			// DialogExpChecker
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(624, 441);
			this.Controls.Add(this.groupBox2);
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
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.LevelView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ASWModernization)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox TextShip;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.DataGridView LevelView;
		private System.Windows.Forms.Button ButtonStart;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.Label LabelShip;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown ASWModernization;
		private System.Windows.Forms.CheckBox ShowAllASWEquipments;
		private System.Windows.Forms.CheckBox ShowAllLevel;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLevel;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColumnExp;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSortieCount;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColumnASW;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColumnEquipment;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
	}
}