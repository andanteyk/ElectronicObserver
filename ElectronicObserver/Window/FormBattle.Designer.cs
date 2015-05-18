namespace ElectronicObserver.Window {
	partial class FormBattle {
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
			this.TableBottom = new System.Windows.Forms.TableLayoutPanel();
			this.ToolTipInfo = new System.Windows.Forms.ToolTip(this.components);
			this.BaseLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.TableTop = new System.Windows.Forms.TableLayoutPanel();
			this.FormationFriend = new ElectronicObserver.Window.Control.ImageLabel();
			this.AirDamage = new ElectronicObserver.Window.Control.ImageLabel();
			this.AirDamageValue = new ElectronicObserver.Window.Control.ImageLabel();
			this.Formation = new ElectronicObserver.Window.Control.ImageLabel();
			this.FormationEnemy = new ElectronicObserver.Window.Control.ImageLabel();
			this.AirStage2Friend = new ElectronicObserver.Window.Control.ImageLabel();
			this.AACutin = new ElectronicObserver.Window.Control.ImageLabel();
			this.AirStage2Enemy = new ElectronicObserver.Window.Control.ImageLabel();
			this.AirStage1Enemy = new ElectronicObserver.Window.Control.ImageLabel();
			this.SearchingFriend = new ElectronicObserver.Window.Control.ImageLabel();
			this.Searching = new ElectronicObserver.Window.Control.ImageLabel();
			this.AirStage1Friend = new ElectronicObserver.Window.Control.ImageLabel();
			this.SearchingEnemy = new ElectronicObserver.Window.Control.ImageLabel();
			this.AirSuperiority = new ElectronicObserver.Window.Control.ImageLabel();
			this.FleetFriend = new ElectronicObserver.Window.Control.ImageLabel();
			this.DamageFriend = new ElectronicObserver.Window.Control.ImageLabel();
			this.WinRank = new ElectronicObserver.Window.Control.ImageLabel();
			this.DamageEnemy = new ElectronicObserver.Window.Control.ImageLabel();
			this.FleetCombined = new ElectronicObserver.Window.Control.ImageLabel();
			this.FleetEnemy = new ElectronicObserver.Window.Control.ImageLabel();
			this.TableBottom.SuspendLayout();
			this.BaseLayoutPanel.SuspendLayout();
			this.TableTop.SuspendLayout();
			this.SuspendLayout();
			// 
			// TableBottom
			// 
			this.TableBottom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TableBottom.ColumnCount = 4;
			this.TableBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
			this.TableBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.TableBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
			this.TableBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
			this.TableBottom.Controls.Add(this.FleetFriend, 0, 0);
			this.TableBottom.Controls.Add(this.DamageFriend, 0, 7);
			this.TableBottom.Controls.Add(this.WinRank, 2, 7);
			this.TableBottom.Controls.Add(this.DamageEnemy, 3, 7);
			this.TableBottom.Controls.Add(this.FleetCombined, 2, 0);
			this.TableBottom.Controls.Add(this.FleetEnemy, 3, 0);
			this.TableBottom.Location = new System.Drawing.Point(3, 90);
			this.TableBottom.Name = "TableBottom";
			this.TableBottom.RowCount = 8;
			this.TableBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
			this.TableBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
			this.TableBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
			this.TableBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
			this.TableBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
			this.TableBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
			this.TableBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
			this.TableBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
			this.TableBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TableBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TableBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TableBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TableBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TableBottom.Size = new System.Drawing.Size(312, 168);
			this.TableBottom.TabIndex = 1;
			this.TableBottom.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.TableBottom_CellPaint);
			// 
			// ToolTipInfo
			// 
			this.ToolTipInfo.AutoPopDelay = 30000;
			this.ToolTipInfo.InitialDelay = 500;
			this.ToolTipInfo.ReshowDelay = 100;
			this.ToolTipInfo.ShowAlways = true;
			// 
			// BaseLayoutPanel
			// 
			this.BaseLayoutPanel.Controls.Add(this.TableTop);
			this.BaseLayoutPanel.Controls.Add(this.TableBottom);
			this.BaseLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BaseLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.BaseLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.BaseLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
			this.BaseLayoutPanel.Name = "BaseLayoutPanel";
			this.BaseLayoutPanel.Size = new System.Drawing.Size(300, 300);
			this.BaseLayoutPanel.TabIndex = 2;
			// 
			// TableTop
			// 
			this.TableTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TableTop.ColumnCount = 4;
			this.TableTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
			this.TableTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.TableTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
			this.TableTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
			this.TableTop.Controls.Add(this.AirDamage, 1, 0);
			this.TableTop.Controls.Add(this.AirDamageValue, 1, 1);
			this.TableTop.Controls.Add(this.FormationFriend, 0, 0);
			this.TableTop.Controls.Add(this.Formation, 2, 0);
			this.TableTop.Controls.Add(this.FormationEnemy, 3, 0);
			this.TableTop.Controls.Add(this.AirStage2Friend, 0, 3);
			this.TableTop.Controls.Add(this.AACutin, 2, 3);
			this.TableTop.Controls.Add(this.AirStage2Enemy, 3, 3);
			this.TableTop.Controls.Add(this.AirStage1Enemy, 3, 2);
			this.TableTop.Controls.Add(this.SearchingFriend, 0, 1);
			this.TableTop.Controls.Add(this.Searching, 2, 1);
			this.TableTop.Controls.Add(this.AirStage1Friend, 0, 2);
			this.TableTop.Controls.Add(this.SearchingEnemy, 3, 1);
			this.TableTop.Controls.Add(this.AirSuperiority, 2, 2);
			this.TableTop.Location = new System.Drawing.Point(3, 3);
			this.TableTop.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.TableTop.Name = "TableTop";
			this.TableTop.RowCount = 4;
			this.TableTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
			this.TableTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
			this.TableTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
			this.TableTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
			this.TableTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TableTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TableTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TableTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TableTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TableTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TableTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TableTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TableTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TableTop.Size = new System.Drawing.Size(312, 84);
			this.TableTop.TabIndex = 18;
			this.TableTop.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.TableTop_CellPaint);
			// 
			// AirDamage
			// 
			this.AirDamage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.AirDamage.BackColor = System.Drawing.Color.Transparent;
			this.AirDamage.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.AirDamage.Location = new System.Drawing.Point(3, 3);
			this.AirDamage.Name = "AirDamage";
			this.AirDamage.Size = new System.Drawing.Size(54, 15);
			this.AirDamage.TabIndex = 0;
			this.AirDamage.Text = "航空伤害";
			this.AirDamage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// AirDamageValue
			// 
			this.AirDamageValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.AirDamageValue.BackColor = System.Drawing.Color.Transparent;
			this.AirDamageValue.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.AirDamageValue.Location = new System.Drawing.Point(3, 3);
			this.AirDamageValue.Name = "AirDamageValue";
			this.AirDamageValue.Size = new System.Drawing.Size(54, 15);
			this.AirDamageValue.TabIndex = 0;
			this.AirDamageValue.Text = "999";
			this.AirDamageValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// FormationFriend
			// 
			this.FormationFriend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.FormationFriend.BackColor = System.Drawing.Color.Transparent;
			this.FormationFriend.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.FormationFriend.Location = new System.Drawing.Point(3, 3);
			this.FormationFriend.Name = "FormationFriend";
			this.FormationFriend.Size = new System.Drawing.Size(78, 15);
			this.FormationFriend.TabIndex = 0;
			this.FormationFriend.Text = "味方陣形";
			this.FormationFriend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// Formation
			// 
			this.Formation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.Formation.BackColor = System.Drawing.Color.Transparent;
			this.Formation.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.Formation.Location = new System.Drawing.Point(87, 3);
			this.Formation.Name = "Formation";
			this.Formation.Size = new System.Drawing.Size(78, 15);
			this.Formation.TabIndex = 2;
			this.Formation.Text = "交戦形態";
			this.Formation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// FormationEnemy
			// 
			this.FormationEnemy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.FormationEnemy.BackColor = System.Drawing.Color.Transparent;
			this.FormationEnemy.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.FormationEnemy.Location = new System.Drawing.Point(171, 3);
			this.FormationEnemy.Name = "FormationEnemy";
			this.FormationEnemy.Size = new System.Drawing.Size(78, 15);
			this.FormationEnemy.TabIndex = 2;
			this.FormationEnemy.Text = "敵陣形";
			this.FormationEnemy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// AirStage2Friend
			// 
			this.AirStage2Friend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.AirStage2Friend.BackColor = System.Drawing.Color.Transparent;
			this.AirStage2Friend.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.AirStage2Friend.Location = new System.Drawing.Point(29, 66);
			this.AirStage2Friend.Name = "AirStage2Friend";
			this.AirStage2Friend.Size = new System.Drawing.Size(25, 15);
			this.AirStage2Friend.TabIndex = 5;
			this.AirStage2Friend.Text = "撃墜";
			this.AirStage2Friend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// AACutin
			// 
			this.AACutin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.AACutin.BackColor = System.Drawing.Color.Transparent;
			this.AACutin.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.AACutin.Location = new System.Drawing.Point(113, 66);
			this.AACutin.Name = "AACutin";
			this.AACutin.Size = new System.Drawing.Size(25, 15);
			this.AACutin.TabIndex = 4;
			this.AACutin.Text = "対空";
			this.AACutin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// AirStage2Enemy
			// 
			this.AirStage2Enemy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.AirStage2Enemy.BackColor = System.Drawing.Color.Transparent;
			this.AirStage2Enemy.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.AirStage2Enemy.Location = new System.Drawing.Point(197, 66);
			this.AirStage2Enemy.Name = "AirStage2Enemy";
			this.AirStage2Enemy.Size = new System.Drawing.Size(25, 15);
			this.AirStage2Enemy.TabIndex = 6;
			this.AirStage2Enemy.Text = "撃墜";
			this.AirStage2Enemy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// AirStage1Enemy
			// 
			this.AirStage1Enemy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.AirStage1Enemy.BackColor = System.Drawing.Color.Transparent;
			this.AirStage1Enemy.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.AirStage1Enemy.Location = new System.Drawing.Point(197, 45);
			this.AirStage1Enemy.Name = "AirStage1Enemy";
			this.AirStage1Enemy.Size = new System.Drawing.Size(25, 15);
			this.AirStage1Enemy.TabIndex = 5;
			this.AirStage1Enemy.Text = "撃墜";
			this.AirStage1Enemy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// SearchingFriend
			// 
			this.SearchingFriend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.SearchingFriend.BackColor = System.Drawing.Color.Transparent;
			this.SearchingFriend.Location = new System.Drawing.Point(8, 23);
			this.SearchingFriend.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.SearchingFriend.Name = "SearchingFriend";
			this.SearchingFriend.Size = new System.Drawing.Size(68, 17);
			this.SearchingFriend.TabIndex = 2;
			this.SearchingFriend.Text = "味方索敵";
			// 
			// Searching
			// 
			this.Searching.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.Searching.BackColor = System.Drawing.Color.Transparent;
			this.Searching.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.Searching.Location = new System.Drawing.Point(87, 24);
			this.Searching.Name = "Searching";
			this.Searching.Size = new System.Drawing.Size(78, 15);
			this.Searching.TabIndex = 2;
			this.Searching.Text = "索敵";
			this.Searching.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// AirStage1Friend
			// 
			this.AirStage1Friend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.AirStage1Friend.BackColor = System.Drawing.Color.Transparent;
			this.AirStage1Friend.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.AirStage1Friend.Location = new System.Drawing.Point(29, 45);
			this.AirStage1Friend.Name = "AirStage1Friend";
			this.AirStage1Friend.Size = new System.Drawing.Size(25, 15);
			this.AirStage1Friend.TabIndex = 4;
			this.AirStage1Friend.Text = "撃墜";
			this.AirStage1Friend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// SearchingEnemy
			// 
			this.SearchingEnemy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.SearchingEnemy.BackColor = System.Drawing.Color.Transparent;
			this.SearchingEnemy.Location = new System.Drawing.Point(182, 23);
			this.SearchingEnemy.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.SearchingEnemy.Name = "SearchingEnemy";
			this.SearchingEnemy.Size = new System.Drawing.Size(56, 17);
			this.SearchingEnemy.TabIndex = 2;
			this.SearchingEnemy.Text = "敵索敵";
			// 
			// AirSuperiority
			// 
			this.AirSuperiority.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.AirSuperiority.BackColor = System.Drawing.Color.Transparent;
			this.AirSuperiority.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.AirSuperiority.Location = new System.Drawing.Point(87, 45);
			this.AirSuperiority.Name = "AirSuperiority";
			this.AirSuperiority.Size = new System.Drawing.Size(78, 15);
			this.AirSuperiority.TabIndex = 4;
			this.AirSuperiority.Text = "制空権";
			this.AirSuperiority.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// FleetFriend
			// 
			this.FleetFriend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.FleetFriend.BackColor = System.Drawing.Color.Transparent;
			this.FleetFriend.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.FleetFriend.Location = new System.Drawing.Point(3, 3);
			this.FleetFriend.Name = "FleetFriend";
			this.FleetFriend.Size = new System.Drawing.Size(78, 15);
			this.FleetFriend.TabIndex = 5;
			this.FleetFriend.Text = "自軍艦隊";
			this.FleetFriend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// DamageFriend
			// 
			this.DamageFriend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.DamageFriend.BackColor = System.Drawing.Color.Transparent;
			this.DamageFriend.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.DamageFriend.Location = new System.Drawing.Point(3, 150);
			this.DamageFriend.Name = "DamageFriend";
			this.DamageFriend.Size = new System.Drawing.Size(78, 15);
			this.DamageFriend.TabIndex = 5;
			this.DamageFriend.Text = "損害率";
			this.DamageFriend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// WinRank
			// 
			this.WinRank.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.WinRank.BackColor = System.Drawing.Color.Transparent;
			this.WinRank.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.WinRank.Location = new System.Drawing.Point(87, 150);
			this.WinRank.Name = "WinRank";
			this.WinRank.Size = new System.Drawing.Size(78, 15);
			this.WinRank.TabIndex = 14;
			this.WinRank.Text = "戦績判定";
			this.WinRank.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// DamageEnemy
			// 
			this.DamageEnemy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.DamageEnemy.BackColor = System.Drawing.Color.Transparent;
			this.DamageEnemy.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.DamageEnemy.Location = new System.Drawing.Point(171, 150);
			this.DamageEnemy.Name = "DamageEnemy";
			this.DamageEnemy.Size = new System.Drawing.Size(78, 15);
			this.DamageEnemy.TabIndex = 15;
			this.DamageEnemy.Text = "損害率";
			this.DamageEnemy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// FleetCombined
			// 
			this.FleetCombined.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.FleetCombined.BackColor = System.Drawing.Color.Transparent;
			this.FleetCombined.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.FleetCombined.Location = new System.Drawing.Point(87, 3);
			this.FleetCombined.Name = "FleetCombined";
			this.FleetCombined.Size = new System.Drawing.Size(78, 15);
			this.FleetCombined.TabIndex = 16;
			this.FleetCombined.Text = "随伴艦隊";
			this.FleetCombined.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// FleetEnemy
			// 
			this.FleetEnemy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.FleetEnemy.BackColor = System.Drawing.Color.Transparent;
			this.FleetEnemy.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.FleetEnemy.Location = new System.Drawing.Point(171, 3);
			this.FleetEnemy.Name = "FleetEnemy";
			this.FleetEnemy.Size = new System.Drawing.Size(78, 15);
			this.FleetEnemy.TabIndex = 17;
			this.FleetEnemy.Text = "敵軍艦隊";
			this.FleetEnemy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// FormBattle
			// 
			this.AutoHidePortion = 150D;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(340, 300);
			this.Controls.Add(this.BaseLayoutPanel);
			this.DoubleBuffered = true;
			this.Font = Program.Window_Font;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.HideOnClose = true;
			this.Name = "FormBattle";
			this.Text = "战斗";
			this.Load += new System.EventHandler(this.FormBattle_Load);
			this.TableBottom.ResumeLayout(false);
			this.TableBottom.PerformLayout();
			this.BaseLayoutPanel.ResumeLayout(false);
			this.TableTop.ResumeLayout(false);
			this.TableTop.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel TableBottom;
		private Control.ImageLabel AirStage1Enemy;
		private Control.ImageLabel AirStage2Friend;
		private Control.ImageLabel AirStage1Friend;
		private Control.ImageLabel AirSuperiority;
		private Control.ImageLabel AACutin;
		private Control.ImageLabel SearchingEnemy;
		private Control.ImageLabel Searching;
		private Control.ImageLabel SearchingFriend;
		private Control.ImageLabel FormationEnemy;
		private Control.ImageLabel FormationFriend;
		private Control.ImageLabel AirDamage;
		private Control.ImageLabel AirDamageValue;
		private Control.ImageLabel Formation;
		private Control.ImageLabel AirStage2Enemy;
		private Control.ImageLabel FleetFriend;
		private Control.ImageLabel DamageFriend;
		private Control.ImageLabel WinRank;
		private Control.ImageLabel DamageEnemy;
		private Control.ImageLabel FleetCombined;
		private Control.ImageLabel FleetEnemy;
		private System.Windows.Forms.ToolTip ToolTipInfo;
		private System.Windows.Forms.FlowLayoutPanel BaseLayoutPanel;
		private System.Windows.Forms.TableLayoutPanel TableTop;
	}
}