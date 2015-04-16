namespace ElectronicObserver.Window {
	partial class FormQuest {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormQuest));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.QuestView = new System.Windows.Forms.DataGridView();
            this.QuestView_State = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.QuestView_Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QuestView_Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QuestView_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QuestView_Progress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MenuMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuMain_ShowRunningOnly = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuMain_ShowOnce = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuMain_ShowDaily = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuMain_ShowWeekly = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuMain_ShowMonthly = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuMain_ColumnFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuMain_ColumnFilter_State = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuMain_ColumnFilter_Type = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuMain_ColumnFilter_Category = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuMain_ColumnFilter_Name = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuMain_ColumnFilter_Progress = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuMain_Initialize = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolTipInfo = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.QuestView)).BeginInit();
            this.MenuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // QuestView
            // 
            resources.ApplyResources(this.QuestView, "QuestView");
            this.QuestView.AllowUserToAddRows = false;
            this.QuestView.AllowUserToDeleteRows = false;
            this.QuestView.AllowUserToResizeRows = false;
            this.QuestView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.QuestView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.QuestView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.QuestView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.QuestView_State,
            this.QuestView_Type,
            this.QuestView_Category,
            this.QuestView_Name,
            this.QuestView_Progress});
            this.QuestView.ContextMenuStrip = this.MenuMain;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.QuestView.DefaultCellStyle = dataGridViewCellStyle3;
            this.QuestView.MultiSelect = false;
            this.QuestView.Name = "QuestView";
            this.QuestView.ReadOnly = true;
            this.QuestView.RowHeadersVisible = false;
            this.QuestView.RowTemplate.Height = 21;
            this.QuestView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ToolTipInfo.SetToolTip(this.QuestView, resources.GetString("QuestView.ToolTip"));
            this.QuestView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.QuestView_CellFormatting);
            this.QuestView.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.QuestView_CellPainting);
            this.QuestView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.QuestView_SortCompare);
            this.QuestView.Sorted += new System.EventHandler(this.QuestView_Sorted);
            // 
            // QuestView_State
            // 
            this.QuestView_State.FalseValue = "";
            resources.ApplyResources(this.QuestView_State, "QuestView_State");
            this.QuestView_State.IndeterminateValue = "";
            this.QuestView_State.Name = "QuestView_State";
            this.QuestView_State.ReadOnly = true;
            this.QuestView_State.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.QuestView_State.ThreeState = true;
            this.QuestView_State.TrueValue = "";
            // 
            // QuestView_Type
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.QuestView_Type.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.QuestView_Type, "QuestView_Type");
            this.QuestView_Type.Name = "QuestView_Type";
            this.QuestView_Type.ReadOnly = true;
            // 
            // QuestView_Category
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.QuestView_Category.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.QuestView_Category, "QuestView_Category");
            this.QuestView_Category.Name = "QuestView_Category";
            this.QuestView_Category.ReadOnly = true;
            // 
            // QuestView_Name
            // 
            this.QuestView_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.QuestView_Name.FillWeight = 200F;
            resources.ApplyResources(this.QuestView_Name, "QuestView_Name");
            this.QuestView_Name.Name = "QuestView_Name";
            this.QuestView_Name.ReadOnly = true;
            // 
            // QuestView_Progress
            // 
            this.QuestView_Progress.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.QuestView_Progress, "QuestView_Progress");
            this.QuestView_Progress.Name = "QuestView_Progress";
            this.QuestView_Progress.ReadOnly = true;
            this.QuestView_Progress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MenuMain
            // 
            resources.ApplyResources(this.MenuMain, "MenuMain");
            this.MenuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuMain_ShowRunningOnly,
            this.toolStripSeparator2,
            this.MenuMain_ShowOnce,
            this.MenuMain_ShowDaily,
            this.MenuMain_ShowWeekly,
            this.MenuMain_ShowMonthly,
            this.toolStripMenuItem1,
            this.MenuMain_ColumnFilter,
            this.toolStripSeparator1,
            this.MenuMain_Initialize});
            this.MenuMain.Name = "MenuMain";
            this.ToolTipInfo.SetToolTip(this.MenuMain, resources.GetString("MenuMain.ToolTip"));
            // 
            // MenuMain_ShowRunningOnly
            // 
            resources.ApplyResources(this.MenuMain_ShowRunningOnly, "MenuMain_ShowRunningOnly");
            this.MenuMain_ShowRunningOnly.CheckOnClick = true;
            this.MenuMain_ShowRunningOnly.Name = "MenuMain_ShowRunningOnly";
            this.MenuMain_ShowRunningOnly.Click += new System.EventHandler(this.MenuMain_ShowRunningOnly_Click);
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // MenuMain_ShowOnce
            // 
            resources.ApplyResources(this.MenuMain_ShowOnce, "MenuMain_ShowOnce");
            this.MenuMain_ShowOnce.CheckOnClick = true;
            this.MenuMain_ShowOnce.Name = "MenuMain_ShowOnce";
            this.MenuMain_ShowOnce.Click += new System.EventHandler(this.MenuMain_ShowOnce_Click);
            // 
            // MenuMain_ShowDaily
            // 
            resources.ApplyResources(this.MenuMain_ShowDaily, "MenuMain_ShowDaily");
            this.MenuMain_ShowDaily.CheckOnClick = true;
            this.MenuMain_ShowDaily.Name = "MenuMain_ShowDaily";
            this.MenuMain_ShowDaily.Click += new System.EventHandler(this.MenuMain_ShowDaily_Click);
            // 
            // MenuMain_ShowWeekly
            // 
            resources.ApplyResources(this.MenuMain_ShowWeekly, "MenuMain_ShowWeekly");
            this.MenuMain_ShowWeekly.CheckOnClick = true;
            this.MenuMain_ShowWeekly.Name = "MenuMain_ShowWeekly";
            this.MenuMain_ShowWeekly.Click += new System.EventHandler(this.MenuMain_ShowWeekly_Click);
            // 
            // MenuMain_ShowMonthly
            // 
            resources.ApplyResources(this.MenuMain_ShowMonthly, "MenuMain_ShowMonthly");
            this.MenuMain_ShowMonthly.CheckOnClick = true;
            this.MenuMain_ShowMonthly.Name = "MenuMain_ShowMonthly";
            this.MenuMain_ShowMonthly.Click += new System.EventHandler(this.MenuMain_ShowMonthly_Click);
            // 
            // toolStripMenuItem1
            // 
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            // 
            // MenuMain_ColumnFilter
            // 
            resources.ApplyResources(this.MenuMain_ColumnFilter, "MenuMain_ColumnFilter");
            this.MenuMain_ColumnFilter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuMain_ColumnFilter_State,
            this.MenuMain_ColumnFilter_Type,
            this.MenuMain_ColumnFilter_Category,
            this.MenuMain_ColumnFilter_Name,
            this.MenuMain_ColumnFilter_Progress});
            this.MenuMain_ColumnFilter.Name = "MenuMain_ColumnFilter";
            // 
            // MenuMain_ColumnFilter_State
            // 
            resources.ApplyResources(this.MenuMain_ColumnFilter_State, "MenuMain_ColumnFilter_State");
            this.MenuMain_ColumnFilter_State.CheckOnClick = true;
            this.MenuMain_ColumnFilter_State.Name = "MenuMain_ColumnFilter_State";
            this.MenuMain_ColumnFilter_State.Click += new System.EventHandler(this.MenuMain_ColumnFilter_Click);
            // 
            // MenuMain_ColumnFilter_Type
            // 
            resources.ApplyResources(this.MenuMain_ColumnFilter_Type, "MenuMain_ColumnFilter_Type");
            this.MenuMain_ColumnFilter_Type.CheckOnClick = true;
            this.MenuMain_ColumnFilter_Type.Name = "MenuMain_ColumnFilter_Type";
            this.MenuMain_ColumnFilter_Type.Click += new System.EventHandler(this.MenuMain_ColumnFilter_Click);
            // 
            // MenuMain_ColumnFilter_Category
            // 
            resources.ApplyResources(this.MenuMain_ColumnFilter_Category, "MenuMain_ColumnFilter_Category");
            this.MenuMain_ColumnFilter_Category.CheckOnClick = true;
            this.MenuMain_ColumnFilter_Category.Name = "MenuMain_ColumnFilter_Category";
            this.MenuMain_ColumnFilter_Category.Click += new System.EventHandler(this.MenuMain_ColumnFilter_Click);
            // 
            // MenuMain_ColumnFilter_Name
            // 
            resources.ApplyResources(this.MenuMain_ColumnFilter_Name, "MenuMain_ColumnFilter_Name");
            this.MenuMain_ColumnFilter_Name.CheckOnClick = true;
            this.MenuMain_ColumnFilter_Name.Name = "MenuMain_ColumnFilter_Name";
            this.MenuMain_ColumnFilter_Name.Click += new System.EventHandler(this.MenuMain_ColumnFilter_Click);
            // 
            // MenuMain_ColumnFilter_Progress
            // 
            resources.ApplyResources(this.MenuMain_ColumnFilter_Progress, "MenuMain_ColumnFilter_Progress");
            this.MenuMain_ColumnFilter_Progress.CheckOnClick = true;
            this.MenuMain_ColumnFilter_Progress.Name = "MenuMain_ColumnFilter_Progress";
            this.MenuMain_ColumnFilter_Progress.Click += new System.EventHandler(this.MenuMain_ColumnFilter_Click);
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // MenuMain_Initialize
            // 
            resources.ApplyResources(this.MenuMain_Initialize, "MenuMain_Initialize");
            this.MenuMain_Initialize.Name = "MenuMain_Initialize";
            this.MenuMain_Initialize.Click += new System.EventHandler(this.MenuMain_Initialize_Click);
            // 
            // ToolTipInfo
            // 
            this.ToolTipInfo.AutoPopDelay = 30000;
            this.ToolTipInfo.InitialDelay = 500;
            this.ToolTipInfo.ReshowDelay = 100;
            this.ToolTipInfo.ShowAlways = true;
            // 
            // FormQuest
            // 
            resources.ApplyResources(this, "$this");
            this.AutoHidePortion = 150D;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.QuestView);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HideOnClose = true;
            this.Name = "FormQuest";
            this.ToolTipInfo.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.Load += new System.EventHandler(this.FormQuest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.QuestView)).EndInit();
            this.MenuMain.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView QuestView;
		private System.Windows.Forms.ToolTip ToolTipInfo;
		private System.Windows.Forms.ContextMenuStrip MenuMain;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ShowRunningOnly;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_Initialize;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ShowOnce;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ShowDaily;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ShowWeekly;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ShowMonthly;
		private System.Windows.Forms.DataGridViewCheckBoxColumn QuestView_State;
		private System.Windows.Forms.DataGridViewTextBoxColumn QuestView_Type;
		private System.Windows.Forms.DataGridViewTextBoxColumn QuestView_Category;
		private System.Windows.Forms.DataGridViewTextBoxColumn QuestView_Name;
		private System.Windows.Forms.DataGridViewTextBoxColumn QuestView_Progress;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ColumnFilter;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ColumnFilter_State;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ColumnFilter_Type;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ColumnFilter_Category;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ColumnFilter_Name;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ColumnFilter_Progress;
	}
}