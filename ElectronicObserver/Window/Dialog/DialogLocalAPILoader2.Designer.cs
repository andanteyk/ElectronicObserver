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
			this.components = new System.ComponentModel.Container();
			this.APIView = new System.Windows.Forms.DataGridView();
			this.APIView_FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ViewMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.ViewMenu_Execute = new System.Windows.Forms.ToolStripMenuItem();
			this.ViewMenu_Delete = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
			this.Menu_File_OpenFolder = new System.Windows.Forms.ToolStripMenuItem();
			this.Menu_File_Reload = new System.Windows.Forms.ToolStripMenuItem();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.ButtonSearchPrev = new System.Windows.Forms.Button();
			this.ButtonSearchLastStart2 = new System.Windows.Forms.Button();
			this.ButtonSearch = new System.Windows.Forms.Button();
			this.TextFilter = new System.Windows.Forms.TextBox();
			this.ButtonExecuteNext = new System.Windows.Forms.Button();
			this.ButtonExecute = new System.Windows.Forms.Button();
			this.FolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
			this.APICaller = new System.ComponentModel.BackgroundWorker();
			((System.ComponentModel.ISupportInitialize)(this.APIView)).BeginInit();
			this.ViewMenu.SuspendLayout();
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
			this.APIView.AllowUserToResizeRows = false;
			this.APIView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.APIView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.APIView_FileName});
			this.APIView.ContextMenuStrip = this.ViewMenu;
			this.APIView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.APIView.Location = new System.Drawing.Point(0, 0);
			this.APIView.Name = "APIView";
			this.APIView.ReadOnly = true;
			this.APIView.RowHeadersVisible = false;
			this.APIView.RowTemplate.Height = 21;
			this.APIView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.APIView.Size = new System.Drawing.Size(624, 331);
			this.APIView.TabIndex = 0;
			// 
			// APIView_FileName
			// 
			this.APIView_FileName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.APIView_FileName.HeaderText = "ファイル名";
			this.APIView_FileName.Name = "APIView_FileName";
			this.APIView_FileName.ReadOnly = true;
			this.APIView_FileName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// ViewMenu
			// 
			this.ViewMenu.ImageScalingSize = new System.Drawing.Size(32, 32);
			this.ViewMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewMenu_Execute,
            this.ViewMenu_Delete});
			this.ViewMenu.Name = "ContextMenu";
			this.ViewMenu.Size = new System.Drawing.Size(144, 48);
			// 
			// ViewMenu_Execute
			// 
			this.ViewMenu_Execute.Name = "ViewMenu_Execute";
			this.ViewMenu_Execute.Size = new System.Drawing.Size(143, 22);
			this.ViewMenu_Execute.Text = "実行(&E)";
			this.ViewMenu_Execute.Click += new System.EventHandler(this.ViewMenu_Execute_Click);
			// 
			// ViewMenu_Delete
			// 
			this.ViewMenu_Delete.Name = "ViewMenu_Delete";
			this.ViewMenu_Delete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.ViewMenu_Delete.Size = new System.Drawing.Size(143, 22);
			this.ViewMenu_Delete.Text = "削除(&D)";
			this.ViewMenu_Delete.Click += new System.EventHandler(this.ViewMenu_Delete_Click);
			// 
			// menuStrip1
			// 
			this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(624, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// Menu_File
			// 
			this.Menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File_OpenFolder,
            this.Menu_File_Reload});
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
			// Menu_File_Reload
			// 
			this.Menu_File_Reload.Name = "Menu_File_Reload";
			this.Menu_File_Reload.Size = new System.Drawing.Size(167, 22);
			this.Menu_File_Reload.Text = "再読み込み(&R)";
			this.Menu_File_Reload.Click += new System.EventHandler(this.Menu_File_Reload_Click);
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
			this.splitContainer1.Panel2.Controls.Add(this.ButtonSearchPrev);
			this.splitContainer1.Panel2.Controls.Add(this.ButtonSearchLastStart2);
			this.splitContainer1.Panel2.Controls.Add(this.ButtonSearch);
			this.splitContainer1.Panel2.Controls.Add(this.TextFilter);
			this.splitContainer1.Panel2.Controls.Add(this.ButtonExecuteNext);
			this.splitContainer1.Panel2.Controls.Add(this.ButtonExecute);
			this.splitContainer1.Size = new System.Drawing.Size(624, 417);
			this.splitContainer1.SplitterDistance = 331;
			this.splitContainer1.TabIndex = 2;
			// 
			// ButtonSearchPrev
			// 
			this.ButtonSearchPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonSearchPrev.Location = new System.Drawing.Point(537, 32);
			this.ButtonSearchPrev.Name = "ButtonSearchPrev";
			this.ButtonSearchPrev.Size = new System.Drawing.Size(75, 23);
			this.ButtonSearchPrev.TabIndex = 5;
			this.ButtonSearchPrev.Text = "前に検索";
			this.ButtonSearchPrev.UseVisualStyleBackColor = true;
			this.ButtonSearchPrev.Click += new System.EventHandler(this.ButtonSearchPrev_Click);
			// 
			// ButtonSearchLastStart2
			// 
			this.ButtonSearchLastStart2.Location = new System.Drawing.Point(174, 3);
			this.ButtonSearchLastStart2.Name = "ButtonSearchLastStart2";
			this.ButtonSearchLastStart2.Size = new System.Drawing.Size(80, 23);
			this.ButtonSearchLastStart2.TabIndex = 4;
			this.ButtonSearchLastStart2.Text = "Last start2";
			this.ButtonSearchLastStart2.UseVisualStyleBackColor = true;
			this.ButtonSearchLastStart2.Click += new System.EventHandler(this.ButtonSearchLastStart2_Click);
			// 
			// ButtonSearch
			// 
			this.ButtonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonSearch.Location = new System.Drawing.Point(456, 32);
			this.ButtonSearch.Name = "ButtonSearch";
			this.ButtonSearch.Size = new System.Drawing.Size(75, 23);
			this.ButtonSearch.TabIndex = 3;
			this.ButtonSearch.Text = "検索";
			this.ButtonSearch.UseVisualStyleBackColor = true;
			this.ButtonSearch.Click += new System.EventHandler(this.ButtonSearch_Click);
			// 
			// TextFilter
			// 
			this.TextFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TextFilter.Location = new System.Drawing.Point(12, 32);
			this.TextFilter.Name = "TextFilter";
			this.TextFilter.Size = new System.Drawing.Size(438, 23);
			this.TextFilter.TabIndex = 2;
			this.TextFilter.Text = "S@api_start2";
			this.TextFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextFilter_KeyDown);
			// 
			// ButtonExecuteNext
			// 
			this.ButtonExecuteNext.Location = new System.Drawing.Point(93, 3);
			this.ButtonExecuteNext.Name = "ButtonExecuteNext";
			this.ButtonExecuteNext.Size = new System.Drawing.Size(75, 23);
			this.ButtonExecuteNext.TabIndex = 1;
			this.ButtonExecuteNext.Text = "次へ実行";
			this.ButtonExecuteNext.UseVisualStyleBackColor = true;
			this.ButtonExecuteNext.Click += new System.EventHandler(this.ButtonExecuteNext_Click);
			// 
			// ButtonExecute
			// 
			this.ButtonExecute.Location = new System.Drawing.Point(12, 3);
			this.ButtonExecute.Name = "ButtonExecute";
			this.ButtonExecute.Size = new System.Drawing.Size(75, 23);
			this.ButtonExecute.TabIndex = 0;
			this.ButtonExecute.Text = "実行";
			this.ButtonExecute.UseVisualStyleBackColor = true;
			this.ButtonExecute.Click += new System.EventHandler(this.ViewMenu_Execute_Click);
			// 
			// APICaller
			// 
			this.APICaller.WorkerSupportsCancellation = true;
			this.APICaller.DoWork += new System.ComponentModel.DoWorkEventHandler(this.APICaller_DoWork);
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
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ファイルからAPIをロード";
			this.Load += new System.EventHandler(this.DialogLocalAPILoader2_Load);
			((System.ComponentModel.ISupportInitialize)(this.APIView)).EndInit();
			this.ViewMenu.ResumeLayout(false);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView APIView;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ToolStripMenuItem Menu_File;
		private System.Windows.Forms.ToolStripMenuItem Menu_File_OpenFolder;
		private System.Windows.Forms.Button ButtonExecuteNext;
		private System.Windows.Forms.Button ButtonExecute;
		private System.Windows.Forms.ContextMenuStrip ViewMenu;
		private System.Windows.Forms.ToolStripMenuItem ViewMenu_Execute;
		private System.Windows.Forms.ToolStripMenuItem ViewMenu_Delete;
		private System.Windows.Forms.FolderBrowserDialog FolderBrowser;
		private System.Windows.Forms.DataGridViewTextBoxColumn APIView_FileName;
		private System.ComponentModel.BackgroundWorker APICaller;
		private System.Windows.Forms.ToolStripMenuItem Menu_File_Reload;
		private System.Windows.Forms.Button ButtonSearch;
		private System.Windows.Forms.TextBox TextFilter;
		private System.Windows.Forms.Button ButtonSearchLastStart2;
		private System.Windows.Forms.Button ButtonSearchPrev;
	}
}