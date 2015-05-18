namespace ElectronicObserver.Window {
	partial class FormShipGroup {
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
			this.ShipView = new System.Windows.Forms.DataGridView();
			this.ShipView_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_ShipType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_Level = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_Exp = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_Next = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_NextRemodel = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_HP = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_Condition = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_Fuel = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_Ammo = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_Equipment1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_Equipment2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_Equipment3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_Equipment4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_Equipment5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_Fleet = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_RepairTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_Firepower = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_FirepowerRemain = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_Torpedo = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_TorpedoRemain = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_AA = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_AARemain = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_Armor = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_ArmorRemain = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_ASW = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_Evasion = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_LOS = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_Luck = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_LuckRemain = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_Locked = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShipView_SallyArea = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.MenuMember = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.MenuMember_AddToGroup = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMember_AddCurrentFleet_Group = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMember_CreateGroup = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuMember_ColumnFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMember_ColumnAutoSize = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMember_LockShipNameScroll = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuMember_CSVOutput = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuMember_Delete = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuGroup = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.MenuGroup_Add = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuGroup_Rename = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuGroup_Delete = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuGroup_AutoUpdate = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuGroup_ShowStatusBar = new System.Windows.Forms.ToolStripMenuItem();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.TabPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.StatusBar = new System.Windows.Forms.StatusStrip();
			this.Status_ShipCount = new System.Windows.Forms.ToolStripStatusLabel();
			this.Status_LevelTotal = new System.Windows.Forms.ToolStripStatusLabel();
			this.Status_LevelAverage = new System.Windows.Forms.ToolStripStatusLabel();
			((System.ComponentModel.ISupportInitialize)(this.ShipView)).BeginInit();
			this.MenuMember.SuspendLayout();
			this.MenuGroup.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.StatusBar.SuspendLayout();
			this.SuspendLayout();
			// 
			// ShipView
			// 
			this.ShipView.AllowUserToAddRows = false;
			this.ShipView.AllowUserToDeleteRows = false;
			this.ShipView.AllowUserToResizeRows = false;
			this.ShipView.BackgroundColor = System.Drawing.SystemColors.Control;
			this.ShipView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.ShipView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ShipView_ID,
            this.ShipView_ShipType,
            this.ShipView_Name,
            this.ShipView_Level,
            this.ShipView_Exp,
            this.ShipView_Next,
            this.ShipView_NextRemodel,
            this.ShipView_HP,
            this.ShipView_Condition,
            this.ShipView_Fuel,
            this.ShipView_Ammo,
            this.ShipView_Equipment1,
            this.ShipView_Equipment2,
            this.ShipView_Equipment3,
            this.ShipView_Equipment4,
            this.ShipView_Equipment5,
            this.ShipView_Fleet,
            this.ShipView_RepairTime,
            this.ShipView_Firepower,
            this.ShipView_FirepowerRemain,
            this.ShipView_Torpedo,
            this.ShipView_TorpedoRemain,
            this.ShipView_AA,
            this.ShipView_AARemain,
            this.ShipView_Armor,
            this.ShipView_ArmorRemain,
            this.ShipView_ASW,
            this.ShipView_Evasion,
            this.ShipView_LOS,
            this.ShipView_Luck,
            this.ShipView_LuckRemain,
            this.ShipView_Locked,
            this.ShipView_SallyArea});
			this.ShipView.ContextMenuStrip = this.MenuMember;
			dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle8.Font = Program.Window_Font;
			dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.ShipView.DefaultCellStyle = dataGridViewCellStyle8;
			this.ShipView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ShipView.Location = new System.Drawing.Point(0, 0);
			this.ShipView.Name = "ShipView";
			this.ShipView.ReadOnly = true;
			this.ShipView.RowHeadersVisible = false;
			this.ShipView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.ShipView.Size = new System.Drawing.Size(300, 134);
			this.ShipView.TabIndex = 0;
			this.ShipView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.ShipView_CellFormatting);
			this.ShipView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.ShipView_SortCompare);
			this.ShipView.Sorted += new System.EventHandler(this.ShipView_Sorted);
			// 
			// ShipView_ID
			// 
			this.ShipView_ID.Frozen = true;
			this.ShipView_ID.HeaderText = "ID";
			this.ShipView_ID.Name = "ShipView_ID";
			this.ShipView_ID.ReadOnly = true;
			this.ShipView_ID.Width = 50;
			// 
			// ShipView_ShipType
			// 
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			this.ShipView_ShipType.DefaultCellStyle = dataGridViewCellStyle1;
			this.ShipView_ShipType.Frozen = true;
			this.ShipView_ShipType.HeaderText = "艦種";
			this.ShipView_ShipType.Name = "ShipView_ShipType";
			this.ShipView_ShipType.ReadOnly = true;
			this.ShipView_ShipType.Width = 80;
			// 
			// ShipView_Name
			// 
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			this.ShipView_Name.DefaultCellStyle = dataGridViewCellStyle2;
			this.ShipView_Name.Frozen = true;
			this.ShipView_Name.HeaderText = "艦名";
			this.ShipView_Name.Name = "ShipView_Name";
			this.ShipView_Name.ReadOnly = true;
			// 
			// ShipView_Level
			// 
			this.ShipView_Level.HeaderText = "Lv";
			this.ShipView_Level.Name = "ShipView_Level";
			this.ShipView_Level.ReadOnly = true;
			this.ShipView_Level.Width = 40;
			// 
			// ShipView_Exp
			// 
			this.ShipView_Exp.HeaderText = "Exp";
			this.ShipView_Exp.Name = "ShipView_Exp";
			this.ShipView_Exp.ReadOnly = true;
			this.ShipView_Exp.Width = 60;
			// 
			// ShipView_Next
			// 
			this.ShipView_Next.HeaderText = "next";
			this.ShipView_Next.Name = "ShipView_Next";
			this.ShipView_Next.ReadOnly = true;
			this.ShipView_Next.Width = 60;
			// 
			// ShipView_NextRemodel
			// 
			this.ShipView_NextRemodel.HeaderText = "改装まで";
			this.ShipView_NextRemodel.Name = "ShipView_NextRemodel";
			this.ShipView_NextRemodel.ReadOnly = true;
			this.ShipView_NextRemodel.Width = 60;
			// 
			// ShipView_HP
			// 
			this.ShipView_HP.HeaderText = "HP";
			this.ShipView_HP.Name = "ShipView_HP";
			this.ShipView_HP.ReadOnly = true;
			this.ShipView_HP.Width = 60;
			// 
			// ShipView_Condition
			// 
			this.ShipView_Condition.HeaderText = "Cond";
			this.ShipView_Condition.Name = "ShipView_Condition";
			this.ShipView_Condition.ReadOnly = true;
			this.ShipView_Condition.Width = 40;
			// 
			// ShipView_Fuel
			// 
			this.ShipView_Fuel.HeaderText = "燃料";
			this.ShipView_Fuel.Name = "ShipView_Fuel";
			this.ShipView_Fuel.ReadOnly = true;
			this.ShipView_Fuel.Width = 60;
			// 
			// ShipView_Ammo
			// 
			this.ShipView_Ammo.HeaderText = "弾薬";
			this.ShipView_Ammo.Name = "ShipView_Ammo";
			this.ShipView_Ammo.ReadOnly = true;
			this.ShipView_Ammo.Width = 60;
			// 
			// ShipView_Equipment1
			// 
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			this.ShipView_Equipment1.DefaultCellStyle = dataGridViewCellStyle3;
			this.ShipView_Equipment1.HeaderText = "装備1";
			this.ShipView_Equipment1.Name = "ShipView_Equipment1";
			this.ShipView_Equipment1.ReadOnly = true;
			this.ShipView_Equipment1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.ShipView_Equipment1.Width = 150;
			// 
			// ShipView_Equipment2
			// 
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			this.ShipView_Equipment2.DefaultCellStyle = dataGridViewCellStyle4;
			this.ShipView_Equipment2.HeaderText = "装備2";
			this.ShipView_Equipment2.Name = "ShipView_Equipment2";
			this.ShipView_Equipment2.ReadOnly = true;
			this.ShipView_Equipment2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.ShipView_Equipment2.Width = 150;
			// 
			// ShipView_Equipment3
			// 
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			this.ShipView_Equipment3.DefaultCellStyle = dataGridViewCellStyle5;
			this.ShipView_Equipment3.HeaderText = "装備3";
			this.ShipView_Equipment3.Name = "ShipView_Equipment3";
			this.ShipView_Equipment3.ReadOnly = true;
			this.ShipView_Equipment3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.ShipView_Equipment3.Width = 150;
			// 
			// ShipView_Equipment4
			// 
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			this.ShipView_Equipment4.DefaultCellStyle = dataGridViewCellStyle6;
			this.ShipView_Equipment4.HeaderText = "装備4";
			this.ShipView_Equipment4.Name = "ShipView_Equipment4";
			this.ShipView_Equipment4.ReadOnly = true;
			this.ShipView_Equipment4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.ShipView_Equipment4.Width = 150;
			// 
			// ShipView_Equipment5
			// 
			dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			this.ShipView_Equipment5.DefaultCellStyle = dataGridViewCellStyle7;
			this.ShipView_Equipment5.HeaderText = "装備5";
			this.ShipView_Equipment5.Name = "ShipView_Equipment5";
			this.ShipView_Equipment5.ReadOnly = true;
			this.ShipView_Equipment5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.ShipView_Equipment5.Width = 150;
			// 
			// ShipView_Fleet
			// 
			this.ShipView_Fleet.HeaderText = "艦隊";
			this.ShipView_Fleet.Name = "ShipView_Fleet";
			this.ShipView_Fleet.ReadOnly = true;
			this.ShipView_Fleet.Width = 40;
			// 
			// ShipView_RepairTime
			// 
			this.ShipView_RepairTime.HeaderText = "入渠";
			this.ShipView_RepairTime.Name = "ShipView_RepairTime";
			this.ShipView_RepairTime.ReadOnly = true;
			this.ShipView_RepairTime.Width = 60;
			// 
			// ShipView_Firepower
			// 
			this.ShipView_Firepower.HeaderText = "火力";
			this.ShipView_Firepower.Name = "ShipView_Firepower";
			this.ShipView_Firepower.ReadOnly = true;
			this.ShipView_Firepower.Width = 40;
			// 
			// ShipView_FirepowerRemain
			// 
			this.ShipView_FirepowerRemain.HeaderText = "火力改修";
			this.ShipView_FirepowerRemain.Name = "ShipView_FirepowerRemain";
			this.ShipView_FirepowerRemain.ReadOnly = true;
			this.ShipView_FirepowerRemain.Width = 40;
			// 
			// ShipView_Torpedo
			// 
			this.ShipView_Torpedo.HeaderText = "雷装";
			this.ShipView_Torpedo.Name = "ShipView_Torpedo";
			this.ShipView_Torpedo.ReadOnly = true;
			this.ShipView_Torpedo.Width = 40;
			// 
			// ShipView_TorpedoRemain
			// 
			this.ShipView_TorpedoRemain.HeaderText = "雷装改修";
			this.ShipView_TorpedoRemain.Name = "ShipView_TorpedoRemain";
			this.ShipView_TorpedoRemain.ReadOnly = true;
			this.ShipView_TorpedoRemain.Width = 40;
			// 
			// ShipView_AA
			// 
			this.ShipView_AA.HeaderText = "対空";
			this.ShipView_AA.Name = "ShipView_AA";
			this.ShipView_AA.ReadOnly = true;
			this.ShipView_AA.Width = 40;
			// 
			// ShipView_AARemain
			// 
			this.ShipView_AARemain.HeaderText = "対空改修";
			this.ShipView_AARemain.Name = "ShipView_AARemain";
			this.ShipView_AARemain.ReadOnly = true;
			this.ShipView_AARemain.Width = 40;
			// 
			// ShipView_Armor
			// 
			this.ShipView_Armor.HeaderText = "装甲";
			this.ShipView_Armor.Name = "ShipView_Armor";
			this.ShipView_Armor.ReadOnly = true;
			this.ShipView_Armor.Width = 40;
			// 
			// ShipView_ArmorRemain
			// 
			this.ShipView_ArmorRemain.HeaderText = "装甲改修";
			this.ShipView_ArmorRemain.Name = "ShipView_ArmorRemain";
			this.ShipView_ArmorRemain.ReadOnly = true;
			this.ShipView_ArmorRemain.Width = 40;
			// 
			// ShipView_ASW
			// 
			this.ShipView_ASW.HeaderText = "対潜";
			this.ShipView_ASW.Name = "ShipView_ASW";
			this.ShipView_ASW.ReadOnly = true;
			this.ShipView_ASW.Width = 40;
			// 
			// ShipView_Evasion
			// 
			this.ShipView_Evasion.HeaderText = "回避";
			this.ShipView_Evasion.Name = "ShipView_Evasion";
			this.ShipView_Evasion.ReadOnly = true;
			this.ShipView_Evasion.Width = 40;
			// 
			// ShipView_LOS
			// 
			this.ShipView_LOS.HeaderText = "索敵";
			this.ShipView_LOS.Name = "ShipView_LOS";
			this.ShipView_LOS.ReadOnly = true;
			this.ShipView_LOS.Width = 40;
			// 
			// ShipView_Luck
			// 
			this.ShipView_Luck.HeaderText = "運";
			this.ShipView_Luck.Name = "ShipView_Luck";
			this.ShipView_Luck.ReadOnly = true;
			this.ShipView_Luck.Width = 40;
			// 
			// ShipView_LuckRemain
			// 
			this.ShipView_LuckRemain.HeaderText = "運改修";
			this.ShipView_LuckRemain.Name = "ShipView_LuckRemain";
			this.ShipView_LuckRemain.ReadOnly = true;
			this.ShipView_LuckRemain.Width = 40;
			// 
			// ShipView_Locked
			// 
			this.ShipView_Locked.HeaderText = "ロック";
			this.ShipView_Locked.Name = "ShipView_Locked";
			this.ShipView_Locked.ReadOnly = true;
			this.ShipView_Locked.Width = 40;
			// 
			// ShipView_SallyArea
			// 
			this.ShipView_SallyArea.HeaderText = "出撃先";
			this.ShipView_SallyArea.Name = "ShipView_SallyArea";
			this.ShipView_SallyArea.ReadOnly = true;
			this.ShipView_SallyArea.Width = 40;
			// 
			// MenuMember
			// 
			ToolStripCustomizer.ToolStripRender.SetRender(this.MenuMember);
			this.MenuMember.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuMember_AddToGroup,
            this.MenuMember_CreateGroup,
            this.MenuMember_AddCurrentFleet_Group,
            this.toolStripSeparator1,
            this.MenuMember_ColumnFilter,
            this.MenuMember_ColumnAutoSize,
            this.MenuMember_LockShipNameScroll,
            this.toolStripSeparator2,
            this.MenuMember_CSVOutput,
            this.toolStripSeparator3,
            this.MenuMember_Delete});
			this.MenuMember.Name = "MenuMember";
			this.MenuMember.Size = new System.Drawing.Size(300, 220);
			this.MenuMember.Opening += new System.ComponentModel.CancelEventHandler(this.MenuMember_Opening);
			// 
			// MenuMember_AddToGroup
			// 
			this.MenuMember_AddToGroup.Name = "MenuMember_AddToGroup";
			this.MenuMember_AddToGroup.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
			this.MenuMember_AddToGroup.Size = new System.Drawing.Size(299, 22);
			this.MenuMember_AddToGroup.Text = "グループへ追加(&A)...";
			this.MenuMember_AddToGroup.Click += new System.EventHandler(this.MenuMember_AddToGroup_Click);
			// 
			// MenuMember_AddCurrentFleet_Group
			// 
			this.MenuMember_AddCurrentFleet_Group.Name = "MenuMember_AddCurrentFleet_Group";
			this.MenuMember_AddCurrentFleet_Group.Size = new System.Drawing.Size(299, 22);
			this.MenuMember_AddCurrentFleet_Group.Text = "現在の艦隊を追加(&G)";
			// 
			// MenuMember_CreateGroup
			// 
			this.MenuMember_CreateGroup.Name = "MenuMember_CreateGroup";
			this.MenuMember_CreateGroup.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D)));
			this.MenuMember_CreateGroup.Size = new System.Drawing.Size(299, 22);
			this.MenuMember_CreateGroup.Text = "新規グループを作成(&C)...";
			this.MenuMember_CreateGroup.Click += new System.EventHandler(this.MenuMember_CreateGroup_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(296, 6);
			// 
			// MenuMember_ColumnFilter
			// 
			this.MenuMember_ColumnFilter.Name = "MenuMember_ColumnFilter";
			this.MenuMember_ColumnFilter.Size = new System.Drawing.Size(299, 22);
			this.MenuMember_ColumnFilter.Text = "列フィルタ(&F)...";
			this.MenuMember_ColumnFilter.Click += new System.EventHandler(this.MenuMember_ColumnFilter_Click);
			// 
			// MenuMember_ColumnAutoSize
			// 
			this.MenuMember_ColumnAutoSize.CheckOnClick = true;
			this.MenuMember_ColumnAutoSize.Name = "MenuMember_ColumnAutoSize";
			this.MenuMember_ColumnAutoSize.Size = new System.Drawing.Size(299, 22);
			this.MenuMember_ColumnAutoSize.Text = "列の自動調整(&D)";
			this.MenuMember_ColumnAutoSize.Click += new System.EventHandler(this.MenuMember_ColumnAutoSize_Click);
			// 
			// MenuMember_LockShipNameScroll
			// 
			this.MenuMember_LockShipNameScroll.CheckOnClick = true;
			this.MenuMember_LockShipNameScroll.Name = "MenuMember_LockShipNameScroll";
			this.MenuMember_LockShipNameScroll.Size = new System.Drawing.Size(299, 22);
			this.MenuMember_LockShipNameScroll.Text = "艦名をスクロールしない(&S)";
			this.MenuMember_LockShipNameScroll.Click += new System.EventHandler(this.MenuMember_LockShipNameScroll_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(296, 6);
			// 
			// MenuMember_CSVOutput
			// 
			this.MenuMember_CSVOutput.Name = "MenuMember_CSVOutput";
			this.MenuMember_CSVOutput.Size = new System.Drawing.Size(299, 22);
			this.MenuMember_CSVOutput.Text = "CSV出力(&O)...";
			this.MenuMember_CSVOutput.Click += new System.EventHandler(this.MenuMember_CSVOutput_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(296, 6);
			// 
			// MenuMember_Delete
			// 
			this.MenuMember_Delete.Name = "MenuMember_Delete";
			this.MenuMember_Delete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.MenuMember_Delete.Size = new System.Drawing.Size(299, 22);
			this.MenuMember_Delete.Text = "削除(&D)";
			this.MenuMember_Delete.Click += new System.EventHandler(this.MenuMember_Delete_Click);
			// 
			// MenuGroup
			// 
			ToolStripCustomizer.ToolStripRender.SetRender(this.MenuGroup);
			this.MenuGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuGroup_Add,
            this.MenuGroup_Rename,
            this.MenuGroup_Delete,
            this.toolStripSeparator4,
            this.MenuGroup_AutoUpdate,
            this.MenuGroup_ShowStatusBar});
			this.MenuGroup.Name = "MenuGroup";
			this.MenuGroup.Size = new System.Drawing.Size(221, 120);
			this.MenuGroup.Opening += new System.ComponentModel.CancelEventHandler(this.MenuGroup_Opening);
			// 
			// MenuGroup_Add
			// 
			this.MenuGroup_Add.Name = "MenuGroup_Add";
			this.MenuGroup_Add.Size = new System.Drawing.Size(220, 22);
			this.MenuGroup_Add.Text = "グループを追加(&A)";
			this.MenuGroup_Add.Click += new System.EventHandler(this.MenuGroup_Add_Click);
			// 
			// MenuGroup_Rename
			// 
			this.MenuGroup_Rename.Name = "MenuGroup_Rename";
			this.MenuGroup_Rename.Size = new System.Drawing.Size(220, 22);
			this.MenuGroup_Rename.Text = "グループ名の変更(&R)...";
			this.MenuGroup_Rename.Click += new System.EventHandler(this.MenuGroup_Rename_Click);
			// 
			// MenuGroup_Delete
			// 
			this.MenuGroup_Delete.Name = "MenuGroup_Delete";
			this.MenuGroup_Delete.Size = new System.Drawing.Size(220, 22);
			this.MenuGroup_Delete.Text = "グループを削除(&D)";
			this.MenuGroup_Delete.Click += new System.EventHandler(this.MenuGroup_Delete_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(217, 6);
			// 
			// MenuGroup_AutoUpdate
			// 
			this.MenuGroup_AutoUpdate.CheckOnClick = true;
			this.MenuGroup_AutoUpdate.Name = "MenuGroup_AutoUpdate";
			this.MenuGroup_AutoUpdate.Size = new System.Drawing.Size(220, 22);
			this.MenuGroup_AutoUpdate.Text = "自動更新する";
			// 
			// MenuGroup_ShowStatusBar
			// 
			this.MenuGroup_ShowStatusBar.Checked = true;
			this.MenuGroup_ShowStatusBar.CheckOnClick = true;
			this.MenuGroup_ShowStatusBar.CheckState = System.Windows.Forms.CheckState.Checked;
			this.MenuGroup_ShowStatusBar.Name = "MenuGroup_ShowStatusBar";
			this.MenuGroup_ShowStatusBar.Size = new System.Drawing.Size(220, 22);
			this.MenuGroup_ShowStatusBar.Text = "ステータスバーを表示する";
			this.MenuGroup_ShowStatusBar.CheckedChanged += new System.EventHandler(this.MenuGroup_ShowStatusBar_CheckedChanged);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.TabPanel);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.ShipView);
			this.splitContainer1.Panel2.Controls.Add(this.StatusBar);
			this.splitContainer1.Size = new System.Drawing.Size(300, 200);
			this.splitContainer1.SplitterDistance = 40;
			this.splitContainer1.TabIndex = 1;
			// 
			// TabPanel
			// 
			this.TabPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TabPanel.AutoScroll = true;
			this.TabPanel.ContextMenuStrip = this.MenuGroup;
			this.TabPanel.Location = new System.Drawing.Point(0, 0);
			this.TabPanel.Name = "TabPanel";
			this.TabPanel.Size = new System.Drawing.Size(300, 40);
			this.TabPanel.TabIndex = 0;
			this.TabPanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.TabPanel_DragDrop);
			this.TabPanel.DragEnter += new System.Windows.Forms.DragEventHandler(this.TabPanel_DragEnter);
			this.TabPanel.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.TabPanel_QueryContinueDrag);
			this.TabPanel.DoubleClick += new System.EventHandler(this.TabPanel_DoubleClick);
			// 
			// StatusBar
			// 
			ToolStripCustomizer.ToolStripRender.SetRender(this.StatusBar);
			this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Status_ShipCount,
            this.Status_LevelTotal,
            this.Status_LevelAverage});
			this.StatusBar.Location = new System.Drawing.Point(0, 134);
			this.StatusBar.Name = "StatusBar";
			this.StatusBar.Size = new System.Drawing.Size(300, 22);
			this.StatusBar.SizingGrip = false;
			this.StatusBar.TabIndex = 1;
			this.StatusBar.Text = "statusStrip1";
			// 
			// Status_ShipCount
			// 
			this.Status_ShipCount.Name = "Status_ShipCount";
			this.Status_ShipCount.Size = new System.Drawing.Size(0, 17);
			// 
			// Status_LevelTotal
			// 
			this.Status_LevelTotal.Name = "Status_LevelTotal";
			this.Status_LevelTotal.Size = new System.Drawing.Size(0, 17);
			// 
			// Status_LevelAverage
			// 
			this.Status_LevelAverage.Name = "Status_LevelAverage";
			this.Status_LevelAverage.Size = new System.Drawing.Size(0, 17);
			// 
			// FormShipGroup
			// 
			this.AutoHidePortion = 150D;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(300, 200);
			this.Controls.Add(this.splitContainer1);
			this.DoubleBuffered = true;
			this.Font = Program.Window_Font;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.HideOnClose = true;
			this.Name = "FormShipGroup";
			this.Text = "舰队编成";
			this.Load += new System.EventHandler(this.FormShipGroup_Load);
			((System.ComponentModel.ISupportInitialize)(this.ShipView)).EndInit();
			this.MenuMember.ResumeLayout(false);
			this.MenuGroup.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.StatusBar.ResumeLayout(false);
			this.StatusBar.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView ShipView;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.FlowLayoutPanel TabPanel;
		private System.Windows.Forms.ContextMenuStrip MenuGroup;
		private System.Windows.Forms.ToolStripMenuItem MenuGroup_Add;
		private System.Windows.Forms.ToolStripMenuItem MenuGroup_Delete;
		private System.Windows.Forms.ContextMenuStrip MenuMember;
		private System.Windows.Forms.ToolStripMenuItem MenuMember_AddToGroup;
		private System.Windows.Forms.ToolStripMenuItem MenuMember_CreateGroup;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem MenuMember_Delete;
		private System.Windows.Forms.ToolStripMenuItem MenuGroup_Rename;
		private System.Windows.Forms.ToolStripMenuItem MenuMember_ColumnFilter;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_ID;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_ShipType;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_Name;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_Level;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_Exp;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_Next;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_NextRemodel;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_HP;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_Condition;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_Fuel;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_Ammo;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_Equipment1;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_Equipment2;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_Equipment3;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_Equipment4;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_Equipment5;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_Fleet;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_RepairTime;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_Firepower;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_FirepowerRemain;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_Torpedo;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_TorpedoRemain;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_AA;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_AARemain;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_Armor;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_ArmorRemain;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_ASW;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_Evasion;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_LOS;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_Luck;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_LuckRemain;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_Locked;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipView_SallyArea;
		private System.Windows.Forms.ToolStripMenuItem MenuMember_CSVOutput;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem MenuMember_ColumnAutoSize;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem MenuGroup_AutoUpdate;
		private System.Windows.Forms.ToolStripMenuItem MenuMember_LockShipNameScroll;
		private System.Windows.Forms.StatusStrip StatusBar;
		private System.Windows.Forms.ToolStripStatusLabel Status_ShipCount;
		private System.Windows.Forms.ToolStripStatusLabel Status_LevelTotal;
		private System.Windows.Forms.ToolStripStatusLabel Status_LevelAverage;
		private System.Windows.Forms.ToolStripMenuItem MenuGroup_ShowStatusBar;
        private System.Windows.Forms.ToolStripMenuItem MenuMember_AddCurrentFleet_Group;
	}
}