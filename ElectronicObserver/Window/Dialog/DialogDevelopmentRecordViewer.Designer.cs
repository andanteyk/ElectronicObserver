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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.DevelopmentView = new System.Windows.Forms.DataGridView();
			this.DevelopmentView_Header = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DevelopmentView_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DevelopmentView_Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DevelopmentView_Fuel = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DevelopmentView_Ammo = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DevelopmentView_Steel = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DevelopmentView_Bauxite = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DevelopmentView_FlagshipType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DevelopmentView_Flagship = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.DevelopmentView)).BeginInit();
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
			this.splitContainer1.Panel1.Controls.Add(this.comboBox1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.DevelopmentView);
			this.splitContainer1.Size = new System.Drawing.Size(624, 441);
			this.splitContainer1.SplitterDistance = 100;
			this.splitContainer1.TabIndex = 0;
			// 
			// DevelopmentView
			// 
			this.DevelopmentView.AllowUserToAddRows = false;
			this.DevelopmentView.AllowUserToDeleteRows = false;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.DevelopmentView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.DevelopmentView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DevelopmentView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DevelopmentView_Header,
            this.DevelopmentView_Name,
            this.DevelopmentView_Date,
            this.DevelopmentView_Fuel,
            this.DevelopmentView_Ammo,
            this.DevelopmentView_Steel,
            this.DevelopmentView_Bauxite,
            this.DevelopmentView_FlagshipType,
            this.DevelopmentView_Flagship});
			this.DevelopmentView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DevelopmentView.Location = new System.Drawing.Point(0, 0);
			this.DevelopmentView.Name = "DevelopmentView";
			this.DevelopmentView.ReadOnly = true;
			this.DevelopmentView.RowHeadersVisible = false;
			this.DevelopmentView.RowTemplate.Height = 21;
			this.DevelopmentView.Size = new System.Drawing.Size(624, 337);
			this.DevelopmentView.TabIndex = 0;
			// 
			// DevelopmentView_Header
			// 
			this.DevelopmentView_Header.HeaderText = "";
			this.DevelopmentView_Header.Name = "DevelopmentView_Header";
			this.DevelopmentView_Header.ReadOnly = true;
			this.DevelopmentView_Header.Width = 50;
			// 
			// DevelopmentView_Name
			// 
			this.DevelopmentView_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.DevelopmentView_Name.HeaderText = "名前";
			this.DevelopmentView_Name.Name = "DevelopmentView_Name";
			this.DevelopmentView_Name.ReadOnly = true;
			// 
			// DevelopmentView_Date
			// 
			this.DevelopmentView_Date.HeaderText = "日付";
			this.DevelopmentView_Date.Name = "DevelopmentView_Date";
			this.DevelopmentView_Date.ReadOnly = true;
			// 
			// DevelopmentView_Fuel
			// 
			this.DevelopmentView_Fuel.HeaderText = "燃料";
			this.DevelopmentView_Fuel.Name = "DevelopmentView_Fuel";
			this.DevelopmentView_Fuel.ReadOnly = true;
			this.DevelopmentView_Fuel.Width = 50;
			// 
			// DevelopmentView_Ammo
			// 
			this.DevelopmentView_Ammo.HeaderText = "弾薬";
			this.DevelopmentView_Ammo.Name = "DevelopmentView_Ammo";
			this.DevelopmentView_Ammo.ReadOnly = true;
			this.DevelopmentView_Ammo.Width = 50;
			// 
			// DevelopmentView_Steel
			// 
			this.DevelopmentView_Steel.HeaderText = "鋼材";
			this.DevelopmentView_Steel.Name = "DevelopmentView_Steel";
			this.DevelopmentView_Steel.ReadOnly = true;
			this.DevelopmentView_Steel.Width = 50;
			// 
			// DevelopmentView_Bauxite
			// 
			this.DevelopmentView_Bauxite.HeaderText = "ボーキ";
			this.DevelopmentView_Bauxite.Name = "DevelopmentView_Bauxite";
			this.DevelopmentView_Bauxite.ReadOnly = true;
			this.DevelopmentView_Bauxite.Width = 50;
			// 
			// DevelopmentView_FlagshipType
			// 
			this.DevelopmentView_FlagshipType.HeaderText = "艦種";
			this.DevelopmentView_FlagshipType.Name = "DevelopmentView_FlagshipType";
			this.DevelopmentView_FlagshipType.ReadOnly = true;
			// 
			// DevelopmentView_Flagship
			// 
			this.DevelopmentView_Flagship.HeaderText = "秘書艦";
			this.DevelopmentView_Flagship.Name = "DevelopmentView_Flagship";
			this.DevelopmentView_Flagship.ReadOnly = true;
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(12, 12);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(121, 23);
			this.comboBox1.TabIndex = 0;
			// 
			// DialogDevelopmentRecordViewer
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(624, 441);
			this.Controls.Add(this.splitContainer1);
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "DialogDevelopmentRecordViewer";
			this.Text = "開発記録";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.DevelopmentView)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.DataGridView DevelopmentView;
		private System.Windows.Forms.DataGridViewTextBoxColumn DevelopmentView_Header;
		private System.Windows.Forms.DataGridViewTextBoxColumn DevelopmentView_Name;
		private System.Windows.Forms.DataGridViewTextBoxColumn DevelopmentView_Date;
		private System.Windows.Forms.DataGridViewTextBoxColumn DevelopmentView_Fuel;
		private System.Windows.Forms.DataGridViewTextBoxColumn DevelopmentView_Ammo;
		private System.Windows.Forms.DataGridViewTextBoxColumn DevelopmentView_Steel;
		private System.Windows.Forms.DataGridViewTextBoxColumn DevelopmentView_Bauxite;
		private System.Windows.Forms.DataGridViewTextBoxColumn DevelopmentView_FlagshipType;
		private System.Windows.Forms.DataGridViewTextBoxColumn DevelopmentView_Flagship;
	}
}