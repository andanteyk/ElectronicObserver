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
			this.RightClickMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.RightClickMenu_ShowBattleDetail = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.RightClickMenu_ShowBattleResult = new System.Windows.Forms.ToolStripMenuItem();
			this.FormationFriend = new ElectronicObserver.Window.Control.ImageLabel();
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
			this.FleetEnemyEscort = new ElectronicObserver.Window.Control.ImageLabel();
			this.WinRank = new ElectronicObserver.Window.Control.ImageLabel();
			this.DamageEnemy = new ElectronicObserver.Window.Control.ImageLabel();
			this.FleetFriendEscort = new ElectronicObserver.Window.Control.ImageLabel();
			this.FleetEnemy = new ElectronicObserver.Window.Control.ImageLabel();
			this.TableBottom.SuspendLayout();
			this.BaseLayoutPanel.SuspendLayout();
			this.TableTop.SuspendLayout();
			this.RightClickMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// TableBottom
			// 
			this.TableBottom.AutoSize = true;
			this.TableBottom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TableBottom.ColumnCount = 4;
			this.TableBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableBottom.Controls.Add(this.FleetFriend, 0, 0);
			this.TableBottom.Controls.Add(this.DamageFriend, 0, 7);
			this.TableBottom.Controls.Add(this.FleetEnemyEscort, 2, 0);
			this.TableBottom.Controls.Add(this.WinRank, 1, 7);
			this.TableBottom.Controls.Add(this.DamageEnemy, 4, 7);
			this.TableBottom.Controls.Add(this.FleetFriendEscort, 1, 0);
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
			this.TableBottom.Size = new System.Drawing.Size(220, 168);
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
			this.TableTop.ColumnCount = 3;
			this.TableTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
			this.TableTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
			this.TableTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
			this.TableTop.Controls.Add(this.FormationFriend, 0, 0);
			this.TableTop.Controls.Add(this.Formation, 1, 0);
			this.TableTop.Controls.Add(this.FormationEnemy, 2, 0);
			this.TableTop.Controls.Add(this.AirStage2Friend, 0, 3);
			this.TableTop.Controls.Add(this.AACutin, 1, 3);
			this.TableTop.Controls.Add(this.AirStage2Enemy, 2, 3);
			this.TableTop.Controls.Add(this.AirStage1Enemy, 2, 2);
			this.TableTop.Controls.Add(this.SearchingFriend, 0, 1);
			this.TableTop.Controls.Add(this.Searching, 1, 1);
			this.TableTop.Controls.Add(this.AirStage1Friend, 0, 2);
			this.TableTop.Controls.Add(this.SearchingEnemy, 2, 1);
			this.TableTop.Controls.Add(this.AirSuperiority, 1, 2);
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
			this.TableTop.Size = new System.Drawing.Size(252, 84);
			this.TableTop.TabIndex = 18;
			this.TableTop.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.TableTop_CellPaint);
			// 
			// RightClickMenu
			// 
			this.RightClickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RightClickMenu_ShowBattleDetail,
            this.toolStripSeparator1,
            this.RightClickMenu_ShowBattleResult});
			this.RightClickMenu.Name = "RightClickMenu";
			this.RightClickMenu.Size = new System.Drawing.Size(219, 54);
			this.RightClickMenu.Opening += new System.ComponentModel.CancelEventHandler(this.RightClickMenu_Opening);
			// 
			// RightClickMenu_ShowBattleDetail
			// 
			this.RightClickMenu_ShowBattleDetail.Name = "RightClickMenu_ShowBattleDetail";
			this.RightClickMenu_ShowBattleDetail.Size = new System.Drawing.Size(218, 22);
			this.RightClickMenu_ShowBattleDetail.Text = "戦闘詳細を表示(&D)...";
			this.RightClickMenu_ShowBattleDetail.Click += new System.EventHandler(this.RightClickMenu_ShowBattleDetail_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(215, 6);
			// 
			// RightClickMenu_ShowBattleResult
			// 
			this.RightClickMenu_ShowBattleResult.Name = "RightClickMenu_ShowBattleResult";
			this.RightClickMenu_ShowBattleResult.Size = new System.Drawing.Size(218, 22);
			this.RightClickMenu_ShowBattleResult.Text = "戦闘結果を一時的に表示(&V)";
			this.RightClickMenu_ShowBattleResult.Click += new System.EventHandler(this.RightClickMenu_ShowBattleResult_Click);
			// 
			// FormationFriend
			// 
			this.FormationFriend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.FormationFriend.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.FormationFriend.Location = new System.Drawing.Point(3, 3);
			this.FormationFriend.Name = "FormationFriend";
			this.FormationFriend.Size = new System.Drawing.Size(49, 16);
			this.FormationFriend.TabIndex = 0;
			this.FormationFriend.Text = "味方陣形";
			this.FormationFriend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// Formation
			// 
			this.Formation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.Formation.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.Formation.Location = new System.Drawing.Point(87, 3);
			this.Formation.Name = "Formation";
			this.Formation.Size = new System.Drawing.Size(49, 16);
			this.Formation.TabIndex = 1;
			this.Formation.Text = "交戦形態";
			this.Formation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// FormationEnemy
			// 
			this.FormationEnemy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.FormationEnemy.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.FormationEnemy.Location = new System.Drawing.Point(171, 3);
			this.FormationEnemy.Name = "FormationEnemy";
			this.FormationEnemy.Size = new System.Drawing.Size(37, 16);
			this.FormationEnemy.TabIndex = 2;
			this.FormationEnemy.Text = "敵陣形";
			this.FormationEnemy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// AirStage2Friend
			// 
			this.AirStage2Friend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.AirStage2Friend.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.AirStage2Friend.Location = new System.Drawing.Point(29, 66);
			this.AirStage2Friend.Name = "AirStage2Friend";
			this.AirStage2Friend.Size = new System.Drawing.Size(25, 16);
			this.AirStage2Friend.TabIndex = 9;
			this.AirStage2Friend.Text = "撃墜";
			this.AirStage2Friend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// AACutin
			// 
			this.AACutin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.AACutin.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.AACutin.Location = new System.Drawing.Point(113, 66);
			this.AACutin.Name = "AACutin";
			this.AACutin.Size = new System.Drawing.Size(25, 16);
			this.AACutin.TabIndex = 10;
			this.AACutin.Text = "対空";
			this.AACutin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// AirStage2Enemy
			// 
			this.AirStage2Enemy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.AirStage2Enemy.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.AirStage2Enemy.Location = new System.Drawing.Point(197, 66);
			this.AirStage2Enemy.Name = "AirStage2Enemy";
			this.AirStage2Enemy.Size = new System.Drawing.Size(25, 16);
			this.AirStage2Enemy.TabIndex = 11;
			this.AirStage2Enemy.Text = "撃墜";
			this.AirStage2Enemy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// AirStage1Enemy
			// 
			this.AirStage1Enemy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.AirStage1Enemy.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.AirStage1Enemy.Location = new System.Drawing.Point(197, 45);
			this.AirStage1Enemy.Name = "AirStage1Enemy";
			this.AirStage1Enemy.Size = new System.Drawing.Size(25, 16);
			this.AirStage1Enemy.TabIndex = 8;
			this.AirStage1Enemy.Text = "撃墜";
			this.AirStage1Enemy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// SearchingFriend
			// 
			this.SearchingFriend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.SearchingFriend.Location = new System.Drawing.Point(8, 23);
			this.SearchingFriend.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.SearchingFriend.Name = "SearchingFriend";
			this.SearchingFriend.Size = new System.Drawing.Size(68, 16);
			this.SearchingFriend.TabIndex = 3;
			this.SearchingFriend.Text = "味方索敵";
			// 
			// Searching
			// 
			this.Searching.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.Searching.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.Searching.Location = new System.Drawing.Point(87, 23);
			this.Searching.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.Searching.Name = "Searching";
			this.Searching.Size = new System.Drawing.Size(25, 16);
			this.Searching.TabIndex = 4;
			this.Searching.Text = "索敵";
			this.Searching.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// AirStage1Friend
			// 
			this.AirStage1Friend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.AirStage1Friend.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.AirStage1Friend.Location = new System.Drawing.Point(29, 45);
			this.AirStage1Friend.Name = "AirStage1Friend";
			this.AirStage1Friend.Size = new System.Drawing.Size(25, 16);
			this.AirStage1Friend.TabIndex = 6;
			this.AirStage1Friend.Text = "撃墜";
			this.AirStage1Friend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// SearchingEnemy
			// 
			this.SearchingEnemy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.SearchingEnemy.Location = new System.Drawing.Point(182, 23);
			this.SearchingEnemy.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.SearchingEnemy.Name = "SearchingEnemy";
			this.SearchingEnemy.Size = new System.Drawing.Size(56, 16);
			this.SearchingEnemy.TabIndex = 5;
			this.SearchingEnemy.Text = "敵索敵";
			// 
			// AirSuperiority
			// 
			this.AirSuperiority.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.AirSuperiority.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.AirSuperiority.Location = new System.Drawing.Point(87, 45);
			this.AirSuperiority.Name = "AirSuperiority";
			this.AirSuperiority.Size = new System.Drawing.Size(37, 16);
			this.AirSuperiority.TabIndex = 7;
			this.AirSuperiority.Text = "制空権";
			this.AirSuperiority.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// FleetFriend
			// 
			this.FleetFriend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.FleetFriend.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.FleetFriend.Location = new System.Drawing.Point(3, 3);
			this.FleetFriend.Name = "FleetFriend";
			this.FleetFriend.Size = new System.Drawing.Size(49, 16);
			this.FleetFriend.TabIndex = 0;
			this.FleetFriend.Text = "自軍艦隊";
			this.FleetFriend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// DamageFriend
			// 
			this.DamageFriend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.DamageFriend.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.DamageFriend.Location = new System.Drawing.Point(3, 150);
			this.DamageFriend.Name = "DamageFriend";
			this.DamageFriend.Size = new System.Drawing.Size(37, 16);
			this.DamageFriend.TabIndex = 3;
			this.DamageFriend.Text = "損害率";
			this.DamageFriend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// FleetEnemyEscort
			// 
			this.FleetEnemyEscort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.FleetEnemyEscort.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.FleetEnemyEscort.Location = new System.Drawing.Point(113, 3);
			this.FleetEnemyEscort.Name = "FleetEnemyEscort";
			this.FleetEnemyEscort.Size = new System.Drawing.Size(49, 16);
			this.FleetEnemyEscort.TabIndex = 19;
			this.FleetEnemyEscort.Text = "敵軍随伴";
			this.FleetEnemyEscort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// WinRank
			// 
			this.WinRank.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.TableBottom.SetColumnSpan(this.WinRank, 2);
			this.WinRank.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.WinRank.Location = new System.Drawing.Point(58, 150);
			this.WinRank.MinimumSize = new System.Drawing.Size(80, 0);
			this.WinRank.Name = "WinRank";
			this.WinRank.Size = new System.Drawing.Size(80, 16);
			this.WinRank.TabIndex = 4;
			this.WinRank.Text = "戦績判定";
			this.WinRank.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// DamageEnemy
			// 
			this.DamageEnemy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.DamageEnemy.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.DamageEnemy.Location = new System.Drawing.Point(168, 150);
			this.DamageEnemy.Name = "DamageEnemy";
			this.DamageEnemy.Size = new System.Drawing.Size(37, 16);
			this.DamageEnemy.TabIndex = 5;
			this.DamageEnemy.Text = "損害率";
			this.DamageEnemy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// FleetFriendEscort
			// 
			this.FleetFriendEscort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.FleetFriendEscort.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.FleetFriendEscort.Location = new System.Drawing.Point(58, 3);
			this.FleetFriendEscort.Name = "FleetFriendEscort";
			this.FleetFriendEscort.Size = new System.Drawing.Size(49, 16);
			this.FleetFriendEscort.TabIndex = 1;
			this.FleetFriendEscort.Text = "自軍随伴";
			this.FleetFriendEscort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// FleetEnemy
			// 
			this.FleetEnemy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.FleetEnemy.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.FleetEnemy.Location = new System.Drawing.Point(168, 3);
			this.FleetEnemy.Name = "FleetEnemy";
			this.FleetEnemy.Size = new System.Drawing.Size(49, 16);
			this.FleetEnemy.TabIndex = 2;
			this.FleetEnemy.Text = "敵軍艦隊";
			this.FleetEnemy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// FormBattle
			// 
			this.AutoHidePortion = 150D;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(300, 300);
			this.ContextMenuStrip = this.RightClickMenu;
			this.Controls.Add(this.BaseLayoutPanel);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.HideOnClose = true;
			this.Name = "FormBattle";
			this.Text = "戦闘";
			this.Load += new System.EventHandler(this.FormBattle_Load);
			this.TableBottom.ResumeLayout(false);
			this.TableBottom.PerformLayout();
			this.BaseLayoutPanel.ResumeLayout(false);
			this.BaseLayoutPanel.PerformLayout();
			this.TableTop.ResumeLayout(false);
			this.TableTop.PerformLayout();
			this.RightClickMenu.ResumeLayout(false);
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
		private Control.ImageLabel Formation;
		private Control.ImageLabel AirStage2Enemy;
		private Control.ImageLabel FleetFriend;
		private Control.ImageLabel DamageFriend;
		private Control.ImageLabel WinRank;
		private Control.ImageLabel DamageEnemy;
		private Control.ImageLabel FleetFriendEscort;
		private Control.ImageLabel FleetEnemy;
		private System.Windows.Forms.ToolTip ToolTipInfo;
		private System.Windows.Forms.FlowLayoutPanel BaseLayoutPanel;
		private System.Windows.Forms.TableLayoutPanel TableTop;
		private Control.ImageLabel FleetEnemyEscort;
		private System.Windows.Forms.ContextMenuStrip RightClickMenu;
		private System.Windows.Forms.ToolStripMenuItem RightClickMenu_ShowBattleDetail;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem RightClickMenu_ShowBattleResult;
	}
}