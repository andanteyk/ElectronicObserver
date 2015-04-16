namespace ElectronicObserver.Window.Dialog {
	partial class DialogEquipmentList {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogEquipmentList));
            this.EquipmentView = new System.Windows.Forms.DataGridView();
            this.EquipmentView_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EquipmentView_Icon = new System.Windows.Forms.DataGridViewImageColumn();
            this.EquipmentView_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EquipmentView_CountAll = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EquipmentView_CountRemain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TopMenu = new System.Windows.Forms.MenuStrip();
            this.TopMenu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.TopMenu_File_CSVOutput = new System.Windows.Forms.ToolStripMenuItem();
            this.TopMenu_File_Update = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveCSVDialog = new System.Windows.Forms.SaveFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.DetailView = new System.Windows.Forms.DataGridView();
            this.DetailView_Level = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DetailView_CountAll = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DetailView_CountRemain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DetailView_EquippedShip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.EquipmentView)).BeginInit();
            this.TopMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DetailView)).BeginInit();
            this.SuspendLayout();
            // 
            // EquipmentView
            // 
            resources.ApplyResources(this.EquipmentView, "EquipmentView");
            this.EquipmentView.AllowUserToAddRows = false;
            this.EquipmentView.AllowUserToDeleteRows = false;
            this.EquipmentView.AllowUserToResizeRows = false;
            this.EquipmentView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.EquipmentView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.EquipmentView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EquipmentView_ID,
            this.EquipmentView_Icon,
            this.EquipmentView_Name,
            this.EquipmentView_CountAll,
            this.EquipmentView_CountRemain});
            this.EquipmentView.Name = "EquipmentView";
            this.EquipmentView.ReadOnly = true;
            this.EquipmentView.RowHeadersVisible = false;
            this.EquipmentView.RowTemplate.Height = 21;
            this.EquipmentView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.EquipmentView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.EquipmentView_CellFormatting);
            this.EquipmentView.SelectionChanged += new System.EventHandler(this.EquipmentView_SelectionChanged);
            this.EquipmentView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.EquipmentView_SortCompare);
            this.EquipmentView.Sorted += new System.EventHandler(this.EquipmentView_Sorted);
            // 
            // EquipmentView_ID
            // 
            resources.ApplyResources(this.EquipmentView_ID, "EquipmentView_ID");
            this.EquipmentView_ID.Name = "EquipmentView_ID";
            this.EquipmentView_ID.ReadOnly = true;
            // 
            // EquipmentView_Icon
            // 
            this.EquipmentView_Icon.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            resources.ApplyResources(this.EquipmentView_Icon, "EquipmentView_Icon");
            this.EquipmentView_Icon.Name = "EquipmentView_Icon";
            this.EquipmentView_Icon.ReadOnly = true;
            this.EquipmentView_Icon.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.EquipmentView_Icon.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // EquipmentView_Name
            // 
            this.EquipmentView_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.EquipmentView_Name, "EquipmentView_Name");
            this.EquipmentView_Name.Name = "EquipmentView_Name";
            this.EquipmentView_Name.ReadOnly = true;
            // 
            // EquipmentView_CountAll
            // 
            resources.ApplyResources(this.EquipmentView_CountAll, "EquipmentView_CountAll");
            this.EquipmentView_CountAll.Name = "EquipmentView_CountAll";
            this.EquipmentView_CountAll.ReadOnly = true;
            // 
            // EquipmentView_CountRemain
            // 
            resources.ApplyResources(this.EquipmentView_CountRemain, "EquipmentView_CountRemain");
            this.EquipmentView_CountRemain.Name = "EquipmentView_CountRemain";
            this.EquipmentView_CountRemain.ReadOnly = true;
            // 
            // TopMenu
            // 
            resources.ApplyResources(this.TopMenu, "TopMenu");
            this.TopMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TopMenu_File});
            this.TopMenu.Name = "TopMenu";
            // 
            // TopMenu_File
            // 
            resources.ApplyResources(this.TopMenu_File, "TopMenu_File");
            this.TopMenu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TopMenu_File_CSVOutput,
            this.TopMenu_File_Update});
            this.TopMenu_File.Name = "TopMenu_File";
            // 
            // TopMenu_File_CSVOutput
            // 
            resources.ApplyResources(this.TopMenu_File_CSVOutput, "TopMenu_File_CSVOutput");
            this.TopMenu_File_CSVOutput.Name = "TopMenu_File_CSVOutput";
            this.TopMenu_File_CSVOutput.Click += new System.EventHandler(this.Menu_File_CSVOutput_Click);
            // 
            // TopMenu_File_Update
            // 
            resources.ApplyResources(this.TopMenu_File_Update, "TopMenu_File_Update");
            this.TopMenu_File_Update.Name = "TopMenu_File_Update";
            this.TopMenu_File_Update.Click += new System.EventHandler(this.TopMenu_File_Update_Click);
            // 
            // SaveCSVDialog
            // 
            resources.ApplyResources(this.SaveCSVDialog, "SaveCSVDialog");
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel1.Controls.Add(this.EquipmentView);
            // 
            // splitContainer1.Panel2
            // 
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            this.splitContainer1.Panel2.Controls.Add(this.DetailView);
            // 
            // DetailView
            // 
            resources.ApplyResources(this.DetailView, "DetailView");
            this.DetailView.AllowUserToAddRows = false;
            this.DetailView.AllowUserToDeleteRows = false;
            this.DetailView.AllowUserToResizeRows = false;
            this.DetailView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.DetailView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.DetailView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DetailView_Level,
            this.DetailView_CountAll,
            this.DetailView_CountRemain,
            this.DetailView_EquippedShip});
            this.DetailView.Name = "DetailView";
            this.DetailView.ReadOnly = true;
            this.DetailView.RowHeadersVisible = false;
            this.DetailView.RowTemplate.Height = 21;
            this.DetailView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DetailView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DetailView_CellFormatting);
            this.DetailView.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DetailView_CellPainting);
            // 
            // DetailView_Level
            // 
            resources.ApplyResources(this.DetailView_Level, "DetailView_Level");
            this.DetailView_Level.Name = "DetailView_Level";
            this.DetailView_Level.ReadOnly = true;
            // 
            // DetailView_CountAll
            // 
            resources.ApplyResources(this.DetailView_CountAll, "DetailView_CountAll");
            this.DetailView_CountAll.Name = "DetailView_CountAll";
            this.DetailView_CountAll.ReadOnly = true;
            // 
            // DetailView_CountRemain
            // 
            resources.ApplyResources(this.DetailView_CountRemain, "DetailView_CountRemain");
            this.DetailView_CountRemain.Name = "DetailView_CountRemain";
            this.DetailView_CountRemain.ReadOnly = true;
            // 
            // DetailView_EquippedShip
            // 
            this.DetailView_EquippedShip.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.DetailView_EquippedShip, "DetailView_EquippedShip");
            this.DetailView_EquippedShip.Name = "DetailView_EquippedShip";
            this.DetailView_EquippedShip.ReadOnly = true;
            this.DetailView_EquippedShip.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DialogEquipmentList
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.TopMenu);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.TopMenu;
            this.Name = "DialogEquipmentList";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogEquipmentList_FormClosed);
            this.Load += new System.EventHandler(this.DialogEquipmentList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.EquipmentView)).EndInit();
            this.TopMenu.ResumeLayout(false);
            this.TopMenu.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DetailView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView EquipmentView;
		private System.Windows.Forms.MenuStrip TopMenu;
		private System.Windows.Forms.ToolStripMenuItem TopMenu_File;
		private System.Windows.Forms.ToolStripMenuItem TopMenu_File_CSVOutput;
		private System.Windows.Forms.SaveFileDialog SaveCSVDialog;
		private System.Windows.Forms.ToolStripMenuItem TopMenu_File_Update;
		private System.Windows.Forms.DataGridViewTextBoxColumn EquipmentView_ID;
		private System.Windows.Forms.DataGridViewImageColumn EquipmentView_Icon;
		private System.Windows.Forms.DataGridViewTextBoxColumn EquipmentView_Name;
		private System.Windows.Forms.DataGridViewTextBoxColumn EquipmentView_CountAll;
		private System.Windows.Forms.DataGridViewTextBoxColumn EquipmentView_CountRemain;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.DataGridView DetailView;
		private System.Windows.Forms.DataGridViewTextBoxColumn DetailView_Level;
		private System.Windows.Forms.DataGridViewTextBoxColumn DetailView_CountAll;
		private System.Windows.Forms.DataGridViewTextBoxColumn DetailView_CountRemain;
		private System.Windows.Forms.DataGridViewTextBoxColumn DetailView_EquippedShip;
	}
}