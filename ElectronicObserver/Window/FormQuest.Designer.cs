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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.QuestView = new System.Windows.Forms.DataGridView();
			this.MenuMain = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.MenuMain_ShowRunningOnly = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMain_SaveNow = new System.Windows.Forms.ToolStripMenuItem();
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
			this.MenuProgress = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.MenuProgress_Increment = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuProgress_Decrement = new System.Windows.Forms.ToolStripMenuItem();
			this.QuestView_State = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.QuestView_Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.QuestView_Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.QuestView_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.QuestView_Progress = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.QuestView)).BeginInit();
			this.MenuMain.SuspendLayout();
			this.MenuProgress.SuspendLayout();
			this.SuspendLayout();
			// 
			// QuestView
			// 
			this.QuestView.AllowUserToAddRows = false;
			this.QuestView.AllowUserToDeleteRows = false;
			this.QuestView.AllowUserToResizeRows = false;
			this.QuestView.BackgroundColor = Utility.Configuration.Config.UI.BackColor.ColorData;
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
			dataGridViewCellStyle3.Font = Program.Window_Font;
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.QuestView.DefaultCellStyle = dataGridViewCellStyle3;
			this.QuestView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.QuestView.Location = new System.Drawing.Point(0, 0);
			this.QuestView.MultiSelect = false;
			this.QuestView.Name = "QuestView";
			this.QuestView.ReadOnly = true;
			this.QuestView.RowHeadersVisible = false;
			this.QuestView.RowTemplate.Height = 21;
			this.QuestView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.QuestView.Size = new System.Drawing.Size(300, 200);
			this.QuestView.TabIndex = 0;
			this.QuestView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.QuestView_CellFormatting);
			this.QuestView.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.QuestView_CellMouseDown);
			this.QuestView.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.QuestView_CellPainting);
			this.QuestView.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.QuestView_ColumnWidthChanged);
			this.QuestView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.QuestView_SortCompare);
			this.QuestView.Sorted += new System.EventHandler(this.QuestView_Sorted);
			// 
			// MenuMain
			// 
			ToolStripCustomizer.ToolStripRender.SetRender(this.MenuMain);
			this.MenuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuMain_ShowRunningOnly,
            this.MenuMain_SaveNow,
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
			this.MenuMain.Size = new System.Drawing.Size(205, 176);
			// 
			// MenuMain_ShowRunningOnly
			// 
			this.MenuMain_ShowRunningOnly.CheckOnClick = true;
			this.MenuMain_ShowRunningOnly.Name = "MenuMain_ShowRunningOnly";
			this.MenuMain_ShowRunningOnly.Size = new System.Drawing.Size(204, 22);
			this.MenuMain_ShowRunningOnly.Text = "显示执行中的任务(&R)";
			this.MenuMain_ShowRunningOnly.Click += new System.EventHandler(this.MenuMain_ShowRunningOnly_Click);
			// 
			// MenuMain_SaveNow
			// 
			this.MenuMain_SaveNow.Name = "MenuMain_SaveNow";
			this.MenuMain_SaveNow.Size = new System.Drawing.Size(230, 22);
			this.MenuMain_SaveNow.Text = "立即保存进度(&S)";
			this.MenuMain_SaveNow.Click += new System.EventHandler(this.MenuMain_SaveNow_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(201, 6);
			// 
			// MenuMain_ShowOnce
			// 
			this.MenuMain_ShowOnce.CheckOnClick = true;
			this.MenuMain_ShowOnce.Name = "MenuMain_ShowOnce";
			this.MenuMain_ShowOnce.Size = new System.Drawing.Size(204, 22);
			this.MenuMain_ShowOnce.Text = "显示一次性的任务(&O)";
			this.MenuMain_ShowOnce.Click += new System.EventHandler(this.MenuMain_ShowOnce_Click);
			// 
			// MenuMain_ShowDaily
			// 
			this.MenuMain_ShowDaily.CheckOnClick = true;
			this.MenuMain_ShowDaily.Name = "MenuMain_ShowDaily";
			this.MenuMain_ShowDaily.Size = new System.Drawing.Size(204, 22);
			this.MenuMain_ShowDaily.Text = "显示日常任务(&D)";
			this.MenuMain_ShowDaily.Click += new System.EventHandler(this.MenuMain_ShowDaily_Click);
			// 
			// MenuMain_ShowWeekly
			// 
			this.MenuMain_ShowWeekly.CheckOnClick = true;
			this.MenuMain_ShowWeekly.Name = "MenuMain_ShowWeekly";
			this.MenuMain_ShowWeekly.Size = new System.Drawing.Size(204, 22);
			this.MenuMain_ShowWeekly.Text = "显示周常任务(&W)";
			this.MenuMain_ShowWeekly.Click += new System.EventHandler(this.MenuMain_ShowWeekly_Click);
			// 
			// MenuMain_ShowMonthly
			// 
			this.MenuMain_ShowMonthly.CheckOnClick = true;
			this.MenuMain_ShowMonthly.Name = "MenuMain_ShowMonthly";
			this.MenuMain_ShowMonthly.Size = new System.Drawing.Size(204, 22);
			this.MenuMain_ShowMonthly.Text = "显示月常任务(&M)";
			this.MenuMain_ShowMonthly.Click += new System.EventHandler(this.MenuMain_ShowMonthly_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(201, 6);
			// 
			// MenuMain_ColumnFilter
			// 
			this.MenuMain_ColumnFilter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuMain_ColumnFilter_State,
            this.MenuMain_ColumnFilter_Type,
            this.MenuMain_ColumnFilter_Category,
            this.MenuMain_ColumnFilter_Name,
            this.MenuMain_ColumnFilter_Progress});
			this.MenuMain_ColumnFilter.Name = "MenuMain_ColumnFilter";
			this.MenuMain_ColumnFilter.Size = new System.Drawing.Size(204, 22);
			this.MenuMain_ColumnFilter.Text = "列过滤(&C)";
			// 
			// MenuMain_ColumnFilter_State
			// 
			this.MenuMain_ColumnFilter_State.CheckOnClick = true;
			this.MenuMain_ColumnFilter_State.Name = "MenuMain_ColumnFilter_State";
			this.MenuMain_ColumnFilter_State.Size = new System.Drawing.Size(140, 22);
			this.MenuMain_ColumnFilter_State.Text = "执行中(&S)";
			this.MenuMain_ColumnFilter_State.Click += new System.EventHandler(this.MenuMain_ColumnFilter_Click);
			// 
			// MenuMain_ColumnFilter_Type
			// 
			this.MenuMain_ColumnFilter_Type.CheckOnClick = true;
			this.MenuMain_ColumnFilter_Type.Name = "MenuMain_ColumnFilter_Type";
			this.MenuMain_ColumnFilter_Type.Size = new System.Drawing.Size(140, 22);
			this.MenuMain_ColumnFilter_Type.Text = "出现种别(&T)";
			this.MenuMain_ColumnFilter_Type.Click += new System.EventHandler(this.MenuMain_ColumnFilter_Click);
			// 
			// MenuMain_ColumnFilter_Category
			// 
			this.MenuMain_ColumnFilter_Category.CheckOnClick = true;
			this.MenuMain_ColumnFilter_Category.Name = "MenuMain_ColumnFilter_Category";
			this.MenuMain_ColumnFilter_Category.Size = new System.Drawing.Size(140, 22);
			this.MenuMain_ColumnFilter_Category.Text = "分类(&C)";
			this.MenuMain_ColumnFilter_Category.Click += new System.EventHandler(this.MenuMain_ColumnFilter_Click);
			// 
			// MenuMain_ColumnFilter_Name
			// 
			this.MenuMain_ColumnFilter_Name.CheckOnClick = true;
			this.MenuMain_ColumnFilter_Name.Name = "MenuMain_ColumnFilter_Name";
			this.MenuMain_ColumnFilter_Name.Size = new System.Drawing.Size(140, 22);
			this.MenuMain_ColumnFilter_Name.Text = "任务名(&N)";
			this.MenuMain_ColumnFilter_Name.Click += new System.EventHandler(this.MenuMain_ColumnFilter_Click);
			// 
			// MenuMain_ColumnFilter_Progress
			// 
			this.MenuMain_ColumnFilter_Progress.CheckOnClick = true;
			this.MenuMain_ColumnFilter_Progress.Name = "MenuMain_ColumnFilter_Progress";
			this.MenuMain_ColumnFilter_Progress.Size = new System.Drawing.Size(140, 22);
			this.MenuMain_ColumnFilter_Progress.Text = "进度(&P)";
			this.MenuMain_ColumnFilter_Progress.Click += new System.EventHandler(this.MenuMain_ColumnFilter_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(201, 6);
			// 
			// MenuMain_Initialize
			// 
			this.MenuMain_Initialize.Name = "MenuMain_Initialize";
			this.MenuMain_Initialize.Size = new System.Drawing.Size(204, 22);
			this.MenuMain_Initialize.Text = "初始化(&I)";
			this.MenuMain_Initialize.Click += new System.EventHandler(this.MenuMain_Initialize_Click);
			// 
			// ToolTipInfo
			// 
			this.ToolTipInfo.AutoPopDelay = 30000;
			this.ToolTipInfo.InitialDelay = 500;
			this.ToolTipInfo.ReshowDelay = 100;
			this.ToolTipInfo.ShowAlways = true;
			// 
			// MenuProgress
			// 
			this.MenuProgress.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuProgress_Increment,
            this.MenuProgress_Decrement});
			this.MenuProgress.Name = "MenuProgress";
			this.MenuProgress.Size = new System.Drawing.Size(135, 48);
			// 
			// MenuProgress_Increment
			// 
			this.MenuProgress_Increment.Name = "MenuProgress_Increment";
			this.MenuProgress_Increment.Size = new System.Drawing.Size(134, 22);
			this.MenuProgress_Increment.Text = "進捗 +1(&I)";
			this.MenuProgress_Increment.Click += new System.EventHandler(this.MenuProgress_Increment_Click);
			// 
			// MenuProgress_Decrement
			// 
			this.MenuProgress_Decrement.Name = "MenuProgress_Decrement";
			this.MenuProgress_Decrement.Size = new System.Drawing.Size(134, 22);
			this.MenuProgress_Decrement.Text = "進捗 -1(&D)";
			this.MenuProgress_Decrement.Click += new System.EventHandler(this.MenuProgress_Decrement_Click);
			// 
			// QuestView_State
			// 
			this.QuestView_State.FalseValue = "";
			this.QuestView_State.HeaderText = "";
			this.QuestView_State.IndeterminateValue = "";
			this.QuestView_State.Name = "QuestView_State";
			this.QuestView_State.ReadOnly = true;
			this.QuestView_State.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.QuestView_State.ThreeState = true;
			this.QuestView_State.TrueValue = "";
			this.QuestView_State.Width = 24;
			// 
			// QuestView_Type
			// 
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.QuestView_Type.DefaultCellStyle = dataGridViewCellStyle1;
			this.QuestView_Type.HeaderText = "种";
			this.QuestView_Type.Name = "QuestView_Type";
			this.QuestView_Type.ReadOnly = true;
			this.QuestView_Type.Width = 26;
			// 
			// QuestView_Category
			// 
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.QuestView_Category.DefaultCellStyle = dataGridViewCellStyle2;
			this.QuestView_Category.HeaderText = "分类";
			this.QuestView_Category.Name = "QuestView_Category";
			this.QuestView_Category.ReadOnly = true;
			this.QuestView_Category.Width = 40;
			// 
			// QuestView_Name
			// 
			this.QuestView_Name.FillWeight = 200F;
			this.QuestView_Name.HeaderText = "任务名";
			this.QuestView_Name.Name = "QuestView_Name";
			this.QuestView_Name.ReadOnly = true;
			this.QuestView_Name.Width = 143;
			// 
			// QuestView_Progress
			// 
			this.QuestView_Progress.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.QuestView_Progress.ContextMenuStrip = this.MenuProgress;
			this.QuestView_Progress.HeaderText = "进度";
			this.QuestView_Progress.Name = "QuestView_Progress";
			this.QuestView_Progress.ReadOnly = true;
			this.QuestView_Progress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// FormQuest
			// 
			this.AutoHidePortion = 150D;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(300, 200);
			this.Controls.Add(this.QuestView);
			this.DoubleBuffered = true;
			this.Font = Program.Window_Font;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.HideOnClose = true;
			this.Name = "FormQuest";
			this.Text = "任务";
			this.Load += new System.EventHandler(this.FormQuest_Load);
			((System.ComponentModel.ISupportInitialize)(this.QuestView)).EndInit();
			this.MenuMain.ResumeLayout(false);
			this.MenuProgress.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView QuestView;
		private System.Windows.Forms.ToolTip ToolTipInfo;
		private System.Windows.Forms.ContextMenuStrip MenuMain;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ShowRunningOnly;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_SaveNow;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_Initialize;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ShowOnce;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ShowDaily;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ShowWeekly;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ShowMonthly;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ColumnFilter;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ColumnFilter_State;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ColumnFilter_Type;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ColumnFilter_Category;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ColumnFilter_Name;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ColumnFilter_Progress;
		private System.Windows.Forms.ContextMenuStrip MenuProgress;
		private System.Windows.Forms.ToolStripMenuItem MenuProgress_Increment;
		private System.Windows.Forms.ToolStripMenuItem MenuProgress_Decrement;
		private System.Windows.Forms.DataGridViewCheckBoxColumn QuestView_State;
		private System.Windows.Forms.DataGridViewTextBoxColumn QuestView_Type;
		private System.Windows.Forms.DataGridViewTextBoxColumn QuestView_Category;
		private System.Windows.Forms.DataGridViewTextBoxColumn QuestView_Name;
		private System.Windows.Forms.DataGridViewTextBoxColumn QuestView_Progress;
	}
}