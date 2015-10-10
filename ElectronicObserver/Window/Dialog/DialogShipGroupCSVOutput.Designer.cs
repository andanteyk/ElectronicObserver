namespace ElectronicObserver.Window.Dialog {
	partial class DialogShipGroupCSVOutput {
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.RadioFormat_Data = new System.Windows.Forms.RadioButton();
			this.RadioFormat_User = new System.Windows.Forms.RadioButton();
			this.panel1 = new System.Windows.Forms.Panel();
			this.RadioOutput_VisibleColumnOnly = new System.Windows.Forms.RadioButton();
			this.RadioOutput_All = new System.Windows.Forms.RadioButton();
			this.ButtonOK = new System.Windows.Forms.Button();
			this.ButtonCancel = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.ButtonOutputPathSearch = new System.Windows.Forms.Button();
			this.TextOutputPath = new System.Windows.Forms.TextBox();
			this.DialogSaveCSV = new System.Windows.Forms.SaveFileDialog();
			this.groupBox1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.panel2);
			this.groupBox1.Controls.Add(this.panel1);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(260, 84);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "オプション";
			// 
			// panel2
			// 
			this.panel2.AutoSize = true;
			this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel2.Controls.Add(this.RadioFormat_Data);
			this.panel2.Controls.Add(this.RadioFormat_User);
			this.panel2.Location = new System.Drawing.Point(6, 22);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(71, 50);
			this.panel2.TabIndex = 2;
			// 
			// RadioFormat_Data
			// 
			this.RadioFormat_Data.AutoSize = true;
			this.RadioFormat_Data.Checked = true;
			this.RadioFormat_Data.Location = new System.Drawing.Point(3, 28);
			this.RadioFormat_Data.Name = "RadioFormat_Data";
			this.RadioFormat_Data.Size = new System.Drawing.Size(65, 19);
			this.RadioFormat_Data.TabIndex = 1;
			this.RadioFormat_Data.TabStop = true;
			this.RadioFormat_Data.Text = "データ用";
			this.RadioFormat_Data.UseVisualStyleBackColor = true;
			// 
			// RadioFormat_User
			// 
			this.RadioFormat_User.AutoSize = true;
			this.RadioFormat_User.Location = new System.Drawing.Point(3, 3);
			this.RadioFormat_User.Name = "RadioFormat_User";
			this.RadioFormat_User.Size = new System.Drawing.Size(61, 19);
			this.RadioFormat_User.TabIndex = 0;
			this.RadioFormat_User.TabStop = true;
			this.RadioFormat_User.Text = "閲覧用";
			this.RadioFormat_User.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.AutoSize = true;
			this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel1.Controls.Add(this.RadioOutput_VisibleColumnOnly);
			this.panel1.Controls.Add(this.RadioOutput_All);
			this.panel1.Location = new System.Drawing.Point(83, 22);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(149, 50);
			this.panel1.TabIndex = 0;
			this.panel1.Visible = false;
			// 
			// RadioOutput_VisibleColumnOnly
			// 
			this.RadioOutput_VisibleColumnOnly.AutoSize = true;
			this.RadioOutput_VisibleColumnOnly.Location = new System.Drawing.Point(3, 28);
			this.RadioOutput_VisibleColumnOnly.Name = "RadioOutput_VisibleColumnOnly";
			this.RadioOutput_VisibleColumnOnly.Size = new System.Drawing.Size(143, 19);
			this.RadioOutput_VisibleColumnOnly.TabIndex = 1;
			this.RadioOutput_VisibleColumnOnly.TabStop = true;
			this.RadioOutput_VisibleColumnOnly.Text = "表示している列のみ出力";
			this.RadioOutput_VisibleColumnOnly.UseVisualStyleBackColor = true;
			// 
			// RadioOutput_All
			// 
			this.RadioOutput_All.AutoSize = true;
			this.RadioOutput_All.Checked = true;
			this.RadioOutput_All.Location = new System.Drawing.Point(3, 3);
			this.RadioOutput_All.Name = "RadioOutput_All";
			this.RadioOutput_All.Size = new System.Drawing.Size(70, 19);
			this.RadioOutput_All.TabIndex = 0;
			this.RadioOutput_All.TabStop = true;
			this.RadioOutput_All.Text = "全て出力";
			this.RadioOutput_All.UseVisualStyleBackColor = true;
			// 
			// ButtonOK
			// 
			this.ButtonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonOK.Location = new System.Drawing.Point(116, 171);
			this.ButtonOK.Name = "ButtonOK";
			this.ButtonOK.Size = new System.Drawing.Size(75, 23);
			this.ButtonOK.TabIndex = 1;
			this.ButtonOK.Text = "OK";
			this.ButtonOK.UseVisualStyleBackColor = true;
			this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
			// 
			// ButtonCancel
			// 
			this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.ButtonCancel.Location = new System.Drawing.Point(197, 171);
			this.ButtonCancel.Name = "ButtonCancel";
			this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
			this.ButtonCancel.TabIndex = 2;
			this.ButtonCancel.Text = "キャンセル";
			this.ButtonCancel.UseVisualStyleBackColor = true;
			this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.ButtonOutputPathSearch);
			this.groupBox2.Controls.Add(this.TextOutputPath);
			this.groupBox2.Location = new System.Drawing.Point(12, 102);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(260, 58);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "出力先";
			// 
			// ButtonOutputPathSearch
			// 
			this.ButtonOutputPathSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonOutputPathSearch.Location = new System.Drawing.Point(222, 22);
			this.ButtonOutputPathSearch.Name = "ButtonOutputPathSearch";
			this.ButtonOutputPathSearch.Size = new System.Drawing.Size(32, 23);
			this.ButtonOutputPathSearch.TabIndex = 4;
			this.ButtonOutputPathSearch.Text = "...";
			this.ButtonOutputPathSearch.UseVisualStyleBackColor = true;
			this.ButtonOutputPathSearch.Click += new System.EventHandler(this.ButtonOutputPathSearch_Click);
			// 
			// TextOutputPath
			// 
			this.TextOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TextOutputPath.Location = new System.Drawing.Point(6, 22);
			this.TextOutputPath.Name = "TextOutputPath";
			this.TextOutputPath.Size = new System.Drawing.Size(210, 23);
			this.TextOutputPath.TabIndex = 0;
			// 
			// DialogSaveCSV
			// 
			this.DialogSaveCSV.Filter = "CSV|*.csv|File|*";
			this.DialogSaveCSV.Title = "CSVの保存";
			// 
			// DialogShipGroupCSVOutput
			// 
			this.AcceptButton = this.ButtonOK;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.ButtonCancel;
			this.ClientSize = new System.Drawing.Size(284, 206);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.ButtonCancel);
			this.Controls.Add(this.ButtonOK);
			this.Controls.Add(this.groupBox1);
			this.Font = Program.Window_Font;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogShipGroupCSVOutput";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "输出舰队编成到CSV";
			this.Load += new System.EventHandler(this.DialogShipGroupCSVOutput_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.RadioButton RadioFormat_Data;
		private System.Windows.Forms.RadioButton RadioFormat_User;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.RadioButton RadioOutput_VisibleColumnOnly;
		private System.Windows.Forms.RadioButton RadioOutput_All;
		private System.Windows.Forms.Button ButtonOK;
		private System.Windows.Forms.Button ButtonCancel;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox TextOutputPath;
		private System.Windows.Forms.Button ButtonOutputPathSearch;
		private System.Windows.Forms.SaveFileDialog DialogSaveCSV;
	}
}