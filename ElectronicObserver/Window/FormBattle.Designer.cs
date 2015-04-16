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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBattle));
            this.TableBottom = new System.Windows.Forms.TableLayoutPanel();
            this.FleetFriend = new ElectronicObserver.Window.Control.ImageLabel();
            this.DamageFriend = new ElectronicObserver.Window.Control.ImageLabel();
            this.WinRank = new ElectronicObserver.Window.Control.ImageLabel();
            this.DamageEnemy = new ElectronicObserver.Window.Control.ImageLabel();
            this.FleetCombined = new ElectronicObserver.Window.Control.ImageLabel();
            this.FleetEnemy = new ElectronicObserver.Window.Control.ImageLabel();
            this.ToolTipInfo = new System.Windows.Forms.ToolTip(this.components);
            this.BaseLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.TableTop = new System.Windows.Forms.TableLayoutPanel();
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
            this.TableBottom.SuspendLayout();
            this.BaseLayoutPanel.SuspendLayout();
            this.TableTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // TableBottom
            // 
            resources.ApplyResources(this.TableBottom, "TableBottom");
            this.TableBottom.Controls.Add(this.FleetFriend, 0, 0);
            this.TableBottom.Controls.Add(this.DamageFriend, 0, 7);
            this.TableBottom.Controls.Add(this.WinRank, 1, 7);
            this.TableBottom.Controls.Add(this.DamageEnemy, 2, 7);
            this.TableBottom.Controls.Add(this.FleetCombined, 1, 0);
            this.TableBottom.Controls.Add(this.FleetEnemy, 2, 0);
            this.TableBottom.Name = "TableBottom";
            this.ToolTipInfo.SetToolTip(this.TableBottom, resources.GetString("TableBottom.ToolTip"));
            this.TableBottom.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.TableBottom_CellPaint);
            // 
            // FleetFriend
            // 
            resources.ApplyResources(this.FleetFriend, "FleetFriend");
            this.FleetFriend.BackColor = System.Drawing.Color.Transparent;
            this.FleetFriend.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.FleetFriend.Name = "FleetFriend";
            this.FleetFriend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ToolTipInfo.SetToolTip(this.FleetFriend, resources.GetString("FleetFriend.ToolTip"));
            // 
            // DamageFriend
            // 
            resources.ApplyResources(this.DamageFriend, "DamageFriend");
            this.DamageFriend.BackColor = System.Drawing.Color.Transparent;
            this.DamageFriend.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.DamageFriend.Name = "DamageFriend";
            this.DamageFriend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ToolTipInfo.SetToolTip(this.DamageFriend, resources.GetString("DamageFriend.ToolTip"));
            // 
            // WinRank
            // 
            resources.ApplyResources(this.WinRank, "WinRank");
            this.WinRank.BackColor = System.Drawing.Color.Transparent;
            this.WinRank.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.WinRank.Name = "WinRank";
            this.WinRank.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ToolTipInfo.SetToolTip(this.WinRank, resources.GetString("WinRank.ToolTip"));
            // 
            // DamageEnemy
            // 
            resources.ApplyResources(this.DamageEnemy, "DamageEnemy");
            this.DamageEnemy.BackColor = System.Drawing.Color.Transparent;
            this.DamageEnemy.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.DamageEnemy.Name = "DamageEnemy";
            this.DamageEnemy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ToolTipInfo.SetToolTip(this.DamageEnemy, resources.GetString("DamageEnemy.ToolTip"));
            // 
            // FleetCombined
            // 
            resources.ApplyResources(this.FleetCombined, "FleetCombined");
            this.FleetCombined.BackColor = System.Drawing.Color.Transparent;
            this.FleetCombined.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.FleetCombined.Name = "FleetCombined";
            this.FleetCombined.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ToolTipInfo.SetToolTip(this.FleetCombined, resources.GetString("FleetCombined.ToolTip"));
            // 
            // FleetEnemy
            // 
            resources.ApplyResources(this.FleetEnemy, "FleetEnemy");
            this.FleetEnemy.BackColor = System.Drawing.Color.Transparent;
            this.FleetEnemy.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.FleetEnemy.Name = "FleetEnemy";
            this.FleetEnemy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ToolTipInfo.SetToolTip(this.FleetEnemy, resources.GetString("FleetEnemy.ToolTip"));
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
            resources.ApplyResources(this.BaseLayoutPanel, "BaseLayoutPanel");
            this.BaseLayoutPanel.Controls.Add(this.TableTop);
            this.BaseLayoutPanel.Controls.Add(this.TableBottom);
            this.BaseLayoutPanel.Name = "BaseLayoutPanel";
            this.ToolTipInfo.SetToolTip(this.BaseLayoutPanel, resources.GetString("BaseLayoutPanel.ToolTip"));
            // 
            // TableTop
            // 
            resources.ApplyResources(this.TableTop, "TableTop");
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
            this.TableTop.Name = "TableTop";
            this.ToolTipInfo.SetToolTip(this.TableTop, resources.GetString("TableTop.ToolTip"));
            this.TableTop.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.TableTop_CellPaint);
            // 
            // FormationFriend
            // 
            resources.ApplyResources(this.FormationFriend, "FormationFriend");
            this.FormationFriend.BackColor = System.Drawing.Color.Transparent;
            this.FormationFriend.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.FormationFriend.Name = "FormationFriend";
            this.FormationFriend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ToolTipInfo.SetToolTip(this.FormationFriend, resources.GetString("FormationFriend.ToolTip"));
            // 
            // Formation
            // 
            resources.ApplyResources(this.Formation, "Formation");
            this.Formation.BackColor = System.Drawing.Color.Transparent;
            this.Formation.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Formation.Name = "Formation";
            this.Formation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ToolTipInfo.SetToolTip(this.Formation, resources.GetString("Formation.ToolTip"));
            // 
            // FormationEnemy
            // 
            resources.ApplyResources(this.FormationEnemy, "FormationEnemy");
            this.FormationEnemy.BackColor = System.Drawing.Color.Transparent;
            this.FormationEnemy.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.FormationEnemy.Name = "FormationEnemy";
            this.FormationEnemy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ToolTipInfo.SetToolTip(this.FormationEnemy, resources.GetString("FormationEnemy.ToolTip"));
            // 
            // AirStage2Friend
            // 
            resources.ApplyResources(this.AirStage2Friend, "AirStage2Friend");
            this.AirStage2Friend.BackColor = System.Drawing.Color.Transparent;
            this.AirStage2Friend.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.AirStage2Friend.Name = "AirStage2Friend";
            this.AirStage2Friend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ToolTipInfo.SetToolTip(this.AirStage2Friend, resources.GetString("AirStage2Friend.ToolTip"));
            // 
            // AACutin
            // 
            resources.ApplyResources(this.AACutin, "AACutin");
            this.AACutin.BackColor = System.Drawing.Color.Transparent;
            this.AACutin.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.AACutin.Name = "AACutin";
            this.AACutin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ToolTipInfo.SetToolTip(this.AACutin, resources.GetString("AACutin.ToolTip"));
            // 
            // AirStage2Enemy
            // 
            resources.ApplyResources(this.AirStage2Enemy, "AirStage2Enemy");
            this.AirStage2Enemy.BackColor = System.Drawing.Color.Transparent;
            this.AirStage2Enemy.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.AirStage2Enemy.Name = "AirStage2Enemy";
            this.AirStage2Enemy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ToolTipInfo.SetToolTip(this.AirStage2Enemy, resources.GetString("AirStage2Enemy.ToolTip"));
            // 
            // AirStage1Enemy
            // 
            resources.ApplyResources(this.AirStage1Enemy, "AirStage1Enemy");
            this.AirStage1Enemy.BackColor = System.Drawing.Color.Transparent;
            this.AirStage1Enemy.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.AirStage1Enemy.Name = "AirStage1Enemy";
            this.AirStage1Enemy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ToolTipInfo.SetToolTip(this.AirStage1Enemy, resources.GetString("AirStage1Enemy.ToolTip"));
            // 
            // SearchingFriend
            // 
            resources.ApplyResources(this.SearchingFriend, "SearchingFriend");
            this.SearchingFriend.BackColor = System.Drawing.Color.Transparent;
            this.SearchingFriend.Name = "SearchingFriend";
            this.ToolTipInfo.SetToolTip(this.SearchingFriend, resources.GetString("SearchingFriend.ToolTip"));
            // 
            // Searching
            // 
            resources.ApplyResources(this.Searching, "Searching");
            this.Searching.BackColor = System.Drawing.Color.Transparent;
            this.Searching.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Searching.Name = "Searching";
            this.Searching.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ToolTipInfo.SetToolTip(this.Searching, resources.GetString("Searching.ToolTip"));
            // 
            // AirStage1Friend
            // 
            resources.ApplyResources(this.AirStage1Friend, "AirStage1Friend");
            this.AirStage1Friend.BackColor = System.Drawing.Color.Transparent;
            this.AirStage1Friend.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.AirStage1Friend.Name = "AirStage1Friend";
            this.AirStage1Friend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ToolTipInfo.SetToolTip(this.AirStage1Friend, resources.GetString("AirStage1Friend.ToolTip"));
            // 
            // SearchingEnemy
            // 
            resources.ApplyResources(this.SearchingEnemy, "SearchingEnemy");
            this.SearchingEnemy.BackColor = System.Drawing.Color.Transparent;
            this.SearchingEnemy.Name = "SearchingEnemy";
            this.ToolTipInfo.SetToolTip(this.SearchingEnemy, resources.GetString("SearchingEnemy.ToolTip"));
            // 
            // AirSuperiority
            // 
            resources.ApplyResources(this.AirSuperiority, "AirSuperiority");
            this.AirSuperiority.BackColor = System.Drawing.Color.Transparent;
            this.AirSuperiority.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.AirSuperiority.Name = "AirSuperiority";
            this.AirSuperiority.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ToolTipInfo.SetToolTip(this.AirSuperiority, resources.GetString("AirSuperiority.ToolTip"));
            // 
            // FormBattle
            // 
            resources.ApplyResources(this, "$this");
            this.AutoHidePortion = 150D;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.BaseLayoutPanel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HideOnClose = true;
            this.Name = "FormBattle";
            this.ToolTipInfo.SetToolTip(this, resources.GetString("$this.ToolTip"));
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