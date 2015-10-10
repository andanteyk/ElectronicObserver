namespace ElectronicObserver.Window.Dialog {
	partial class DialogBattleReport {
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
			this.webBrowser1 = new System.Windows.Forms.WebBrowser();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.MenuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuFile_Save = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// webBrowser1
			// 
			this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webBrowser1.Location = new System.Drawing.Point(0, 25);
			this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
			this.webBrowser1.Name = "webBrowser1";
			this.webBrowser1.Size = new System.Drawing.Size(1030, 605);
			this.webBrowser1.TabIndex = 0;
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.Filter = "网页文件|*.html|所有文件|*.*";
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuFile});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(830, 25);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// MenuFile
			// 
			this.MenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuFile_Save});
			this.MenuFile.Name = "MenuFile";
			this.MenuFile.Size = new System.Drawing.Size(58, 21);
			this.MenuFile.Text = "文件(&F)";
			// 
			// MenuFile_Save
			// 
			this.MenuFile_Save.Name = "MenuFile_Save";
			this.MenuFile_Save.Size = new System.Drawing.Size(151, 22);
			this.MenuFile_Save.Text = "保存为文件(&S)";
			this.MenuFile_Save.Click += new System.EventHandler(this.MenuFile_Save_Click);
			// 
			// DialogBattleReport
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1030, 630);
			this.Controls.Add(this.webBrowser1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "DialogBattleReport";
			this.Font = Program.Window_Font;
			this.ShowInTaskbar = false;
			this.Text = "战斗报告";
			this.Load += new System.EventHandler(this.DialogBattleReport_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.WebBrowser webBrowser1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem MenuFile;
		private System.Windows.Forms.ToolStripMenuItem MenuFile_Save;
	}
}