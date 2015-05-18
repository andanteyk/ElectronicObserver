namespace ElectronicObserver.Window.Dialog {
	partial class DialogLocalAPILoader2 {
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
			this.APIView = new System.Windows.Forms.DataGridView();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.APIView_FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
			this.Menu_File_OpenFolder = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.APIView)).BeginInit();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// APIView
			// 
			this.APIView.AllowUserToAddRows = false;
			this.APIView.AllowUserToDeleteRows = false;
			this.APIView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.APIView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.APIView_FileName});
			this.APIView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.APIView.Location = new System.Drawing.Point(0, 0);
			this.APIView.Name = "APIView";
			this.APIView.ReadOnly = true;
			this.APIView.RowHeadersVisible = false;
			this.APIView.RowTemplate.Height = 21;
			this.APIView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.APIView.Size = new System.Drawing.Size(624, 291);
			this.APIView.TabIndex = 0;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(624, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 24);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.APIView);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.button2);
			this.splitContainer1.Panel2.Controls.Add(this.button1);
			this.splitContainer1.Size = new System.Drawing.Size(624, 417);
			this.splitContainer1.SplitterDistance = 291;
			this.splitContainer1.TabIndex = 2;
			// 
			// APIView_FileName
			// 
			this.APIView_FileName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.APIView_FileName.HeaderText = "ファイル名";
			this.APIView_FileName.Name = "APIView_FileName";
			this.APIView_FileName.ReadOnly = true;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(12, 3);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "実行";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(93, 3);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 1;
			this.button2.Text = "次を実行";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// Menu_File
			// 
			this.Menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File_OpenFolder});
			this.Menu_File.Name = "Menu_File";
			this.Menu_File.Size = new System.Drawing.Size(70, 20);
			this.Menu_File.Text = "ファイル(&F)";
			// 
			// Menu_File_OpenFolder
			// 
			this.Menu_File_OpenFolder.Name = "Menu_File_OpenFolder";
			this.Menu_File_OpenFolder.Size = new System.Drawing.Size(167, 22);
			this.Menu_File_OpenFolder.Text = "フォルダを開く(&O)...";
			this.Menu_File_OpenFolder.Click += new System.EventHandler(this.Menu_File_OpenFolder_Click);
			// 
			// DialogLocalAPILoader2
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(624, 441);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.menuStrip1);
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "DialogLocalAPILoader2";
			this.Text = "DialogLocalAPILoader2";
			((System.ComponentModel.ISupportInitialize)(this.APIView)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView APIView;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.DataGridViewTextBoxColumn APIView_FileName;
		private System.Windows.Forms.ToolStripMenuItem Menu_File;
		private System.Windows.Forms.ToolStripMenuItem Menu_File_OpenFolder;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
	}
}