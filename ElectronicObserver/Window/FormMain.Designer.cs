namespace ElectronicObserver.Window {
	partial class FormMain {
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose( bool disposing ) {
			if ( disposing && ( components != null ) ) {
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			WeifenLuo.WinFormsUI.Docking.DockPanelSkin dockPanelSkin6 = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
			WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin autoHideStripSkin6 = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient16 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient36 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin dockPaneStripSkin6 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient dockPaneStripGradient6 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient37 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient17 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient38 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient6 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient39 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient40 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient18 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient41 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient42 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			this.MainDockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
			this.StripMenu = new System.Windows.Forms.MenuStrip();
			this.StripMenu_Debug = new System.Windows.Forms.ToolStripMenuItem();
			this.StripMenu_Debug_LoadAPIFromFile = new System.Windows.Forms.ToolStripMenuItem();
			this.StripStatus = new System.Windows.Forms.StatusStrip();
			this.StripStatus_Information = new System.Windows.Forms.ToolStripStatusLabel();
			this.StripStatus_Padding = new System.Windows.Forms.ToolStripStatusLabel();
			this.StripStatus_Clock = new System.Windows.Forms.ToolStripStatusLabel();
			this.UIUpdateTimer = new System.Windows.Forms.Timer(this.components);
			this.StripMenu_View = new System.Windows.Forms.ToolStripMenuItem();
			this.StripMenu_View_Fleet = new System.Windows.Forms.ToolStripMenuItem();
			this.StripMenu_View_Fleet_1 = new System.Windows.Forms.ToolStripMenuItem();
			this.StripMenu_View_Fleet_2 = new System.Windows.Forms.ToolStripMenuItem();
			this.StripMenu_View_Fleet_3 = new System.Windows.Forms.ToolStripMenuItem();
			this.StripMenu_View_Fleet_4 = new System.Windows.Forms.ToolStripMenuItem();
			this.StripMenu.SuspendLayout();
			this.StripStatus.SuspendLayout();
			this.SuspendLayout();
			// 
			// MainDockPanel
			// 
			this.MainDockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainDockPanel.DockBottomPortion = 150D;
			this.MainDockPanel.DockLeftPortion = 150D;
			this.MainDockPanel.DockRightPortion = 150D;
			this.MainDockPanel.DockTopPortion = 150D;
			this.MainDockPanel.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
			this.MainDockPanel.Location = new System.Drawing.Point(0, 26);
			this.MainDockPanel.Name = "MainDockPanel";
			this.MainDockPanel.ShowDocumentIcon = true;
			this.MainDockPanel.Size = new System.Drawing.Size(284, 213);
			dockPanelGradient16.EndColor = System.Drawing.SystemColors.ControlLight;
			dockPanelGradient16.StartColor = System.Drawing.SystemColors.ControlLight;
			autoHideStripSkin6.DockStripGradient = dockPanelGradient16;
			tabGradient36.EndColor = System.Drawing.SystemColors.Control;
			tabGradient36.StartColor = System.Drawing.SystemColors.Control;
			tabGradient36.TextColor = System.Drawing.SystemColors.ControlDarkDark;
			autoHideStripSkin6.TabGradient = tabGradient36;
			autoHideStripSkin6.TextFont = new System.Drawing.Font("メイリオ", 9F);
			dockPanelSkin6.AutoHideStripSkin = autoHideStripSkin6;
			tabGradient37.EndColor = System.Drawing.SystemColors.ControlLightLight;
			tabGradient37.StartColor = System.Drawing.SystemColors.ControlLightLight;
			tabGradient37.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripGradient6.ActiveTabGradient = tabGradient37;
			dockPanelGradient17.EndColor = System.Drawing.SystemColors.Control;
			dockPanelGradient17.StartColor = System.Drawing.SystemColors.Control;
			dockPaneStripGradient6.DockStripGradient = dockPanelGradient17;
			tabGradient38.EndColor = System.Drawing.SystemColors.ControlLight;
			tabGradient38.StartColor = System.Drawing.SystemColors.ControlLight;
			tabGradient38.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripGradient6.InactiveTabGradient = tabGradient38;
			dockPaneStripSkin6.DocumentGradient = dockPaneStripGradient6;
			dockPaneStripSkin6.TextFont = new System.Drawing.Font("メイリオ", 9F);
			tabGradient39.EndColor = System.Drawing.SystemColors.ActiveCaption;
			tabGradient39.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			tabGradient39.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
			tabGradient39.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
			dockPaneStripToolWindowGradient6.ActiveCaptionGradient = tabGradient39;
			tabGradient40.EndColor = System.Drawing.SystemColors.Control;
			tabGradient40.StartColor = System.Drawing.SystemColors.Control;
			tabGradient40.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripToolWindowGradient6.ActiveTabGradient = tabGradient40;
			dockPanelGradient18.EndColor = System.Drawing.SystemColors.ControlLight;
			dockPanelGradient18.StartColor = System.Drawing.SystemColors.ControlLight;
			dockPaneStripToolWindowGradient6.DockStripGradient = dockPanelGradient18;
			tabGradient41.EndColor = System.Drawing.SystemColors.InactiveCaption;
			tabGradient41.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			tabGradient41.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
			tabGradient41.TextColor = System.Drawing.SystemColors.InactiveCaptionText;
			dockPaneStripToolWindowGradient6.InactiveCaptionGradient = tabGradient41;
			tabGradient42.EndColor = System.Drawing.Color.Transparent;
			tabGradient42.StartColor = System.Drawing.Color.Transparent;
			tabGradient42.TextColor = System.Drawing.SystemColors.ControlDarkDark;
			dockPaneStripToolWindowGradient6.InactiveTabGradient = tabGradient42;
			dockPaneStripSkin6.ToolWindowGradient = dockPaneStripToolWindowGradient6;
			dockPanelSkin6.DockPaneStripSkin = dockPaneStripSkin6;
			this.MainDockPanel.Skin = dockPanelSkin6;
			this.MainDockPanel.TabIndex = 0;
			// 
			// StripMenu
			// 
			this.StripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StripMenu_View,
            this.StripMenu_Debug});
			this.StripMenu.Location = new System.Drawing.Point(0, 0);
			this.StripMenu.Name = "StripMenu";
			this.StripMenu.Size = new System.Drawing.Size(284, 26);
			this.StripMenu.TabIndex = 2;
			this.StripMenu.Text = "menuStrip1";
			// 
			// StripMenu_Debug
			// 
			this.StripMenu_Debug.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StripMenu_Debug_LoadAPIFromFile});
			this.StripMenu_Debug.Name = "StripMenu_Debug";
			this.StripMenu_Debug.Size = new System.Drawing.Size(87, 22);
			this.StripMenu_Debug.Text = "デバッグ(&D)";
			// 
			// StripMenu_Debug_LoadAPIFromFile
			// 
			this.StripMenu_Debug_LoadAPIFromFile.Name = "StripMenu_Debug_LoadAPIFromFile";
			this.StripMenu_Debug_LoadAPIFromFile.Size = new System.Drawing.Size(245, 22);
			this.StripMenu_Debug_LoadAPIFromFile.Text = "ファイルからAPIをロード(&L)...";
			this.StripMenu_Debug_LoadAPIFromFile.Click += new System.EventHandler(this.StripMenu_Debug_LoadAPIFromFile_Click);
			// 
			// StripStatus
			// 
			this.StripStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StripStatus_Information,
            this.StripStatus_Padding,
            this.StripStatus_Clock});
			this.StripStatus.Location = new System.Drawing.Point(0, 239);
			this.StripStatus.Name = "StripStatus";
			this.StripStatus.Size = new System.Drawing.Size(284, 23);
			this.StripStatus.TabIndex = 3;
			// 
			// StripStatus_Information
			// 
			this.StripStatus_Information.Name = "StripStatus_Information";
			this.StripStatus_Information.Size = new System.Drawing.Size(105, 18);
			this.StripStatus_Information.Text = "Now Preparing...";
			// 
			// StripStatus_Padding
			// 
			this.StripStatus_Padding.Name = "StripStatus_Padding";
			this.StripStatus_Padding.Size = new System.Drawing.Size(125, 18);
			this.StripStatus_Padding.Spring = true;
			// 
			// StripStatus_Clock
			// 
			this.StripStatus_Clock.Name = "StripStatus_Clock";
			this.StripStatus_Clock.Size = new System.Drawing.Size(39, 18);
			this.StripStatus_Clock.Text = "Clock";
			// 
			// UIUpdateTimer
			// 
			this.UIUpdateTimer.Interval = 500;
			this.UIUpdateTimer.Tick += new System.EventHandler(this.UIUpdateTimer_Tick);
			// 
			// StripMenu_View
			// 
			this.StripMenu_View.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StripMenu_View_Fleet});
			this.StripMenu_View.Name = "StripMenu_View";
			this.StripMenu_View.Size = new System.Drawing.Size(62, 22);
			this.StripMenu_View.Text = "表示(&V)";
			// 
			// StripMenu_View_Fleet
			// 
			this.StripMenu_View_Fleet.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StripMenu_View_Fleet_1,
            this.StripMenu_View_Fleet_2,
            this.StripMenu_View_Fleet_3,
            this.StripMenu_View_Fleet_4});
			this.StripMenu_View_Fleet.Name = "StripMenu_View_Fleet";
			this.StripMenu_View_Fleet.Size = new System.Drawing.Size(152, 22);
			this.StripMenu_View_Fleet.Text = "艦隊(&F)";
			// 
			// StripMenu_View_Fleet_1
			// 
			this.StripMenu_View_Fleet_1.Name = "StripMenu_View_Fleet_1";
			this.StripMenu_View_Fleet_1.Size = new System.Drawing.Size(152, 22);
			this.StripMenu_View_Fleet_1.Text = "#&1";
			this.StripMenu_View_Fleet_1.Click += new System.EventHandler(this.StripMenu_View_Fleet_1_Click);
			// 
			// StripMenu_View_Fleet_2
			// 
			this.StripMenu_View_Fleet_2.Name = "StripMenu_View_Fleet_2";
			this.StripMenu_View_Fleet_2.Size = new System.Drawing.Size(152, 22);
			this.StripMenu_View_Fleet_2.Text = "#&2";
			this.StripMenu_View_Fleet_2.Click += new System.EventHandler(this.StripMenu_View_Fleet_2_Click);
			// 
			// StripMenu_View_Fleet_3
			// 
			this.StripMenu_View_Fleet_3.Name = "StripMenu_View_Fleet_3";
			this.StripMenu_View_Fleet_3.Size = new System.Drawing.Size(152, 22);
			this.StripMenu_View_Fleet_3.Text = "#&3";
			this.StripMenu_View_Fleet_3.Click += new System.EventHandler(this.StripMenu_View_Fleet_3_Click);
			// 
			// StripMenu_View_Fleet_4
			// 
			this.StripMenu_View_Fleet_4.Name = "StripMenu_View_Fleet_4";
			this.StripMenu_View_Fleet_4.Size = new System.Drawing.Size(152, 22);
			this.StripMenu_View_Fleet_4.Text = "#&4";
			this.StripMenu_View_Fleet_4.Click += new System.EventHandler(this.StripMenu_View_Fleet_4_Click);
			// 
			// FormMain
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.MainDockPanel);
			this.Controls.Add(this.StripStatus);
			this.Controls.Add(this.StripMenu);
			this.MainMenuStrip = this.StripMenu;
			this.Name = "FormMain";
			this.Text = "試製七四式電子観測儀";
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.StripMenu.ResumeLayout(false);
			this.StripMenu.PerformLayout();
			this.StripStatus.ResumeLayout(false);
			this.StripStatus.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private WeifenLuo.WinFormsUI.Docking.DockPanel MainDockPanel;
		private System.Windows.Forms.MenuStrip StripMenu;
		private System.Windows.Forms.StatusStrip StripStatus;
		private System.Windows.Forms.ToolStripMenuItem StripMenu_Debug;
		private System.Windows.Forms.ToolStripMenuItem StripMenu_Debug_LoadAPIFromFile;
		private System.Windows.Forms.Timer UIUpdateTimer;
		private System.Windows.Forms.ToolStripStatusLabel StripStatus_Information;
		private System.Windows.Forms.ToolStripStatusLabel StripStatus_Padding;
		private System.Windows.Forms.ToolStripStatusLabel StripStatus_Clock;
		private System.Windows.Forms.ToolStripMenuItem StripMenu_View;
		private System.Windows.Forms.ToolStripMenuItem StripMenu_View_Fleet;
		private System.Windows.Forms.ToolStripMenuItem StripMenu_View_Fleet_1;
		private System.Windows.Forms.ToolStripMenuItem StripMenu_View_Fleet_2;
		private System.Windows.Forms.ToolStripMenuItem StripMenu_View_Fleet_3;
		private System.Windows.Forms.ToolStripMenuItem StripMenu_View_Fleet_4;
	}
}

