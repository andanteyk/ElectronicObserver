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
			WeifenLuo.WinFormsUI.Docking.DockPanelSkin dockPanelSkin2 = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
			WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin autoHideStripSkin2 = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient4 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient8 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin dockPaneStripSkin2 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient dockPaneStripGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient9 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient5 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient10 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient11 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient12 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient6 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient13 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient14 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			this.MainDockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
			this.StripMenu = new System.Windows.Forms.MenuStrip();
			this.StripMenu_Debug = new System.Windows.Forms.ToolStripMenuItem();
			this.StripMenu_Debug_LoadAPIFromFile = new System.Windows.Forms.ToolStripMenuItem();
			this.StripStatus = new System.Windows.Forms.StatusStrip();
			this.UIUpdateTimer = new System.Windows.Forms.Timer(this.components);
			this.StripMenu.SuspendLayout();
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
			this.MainDockPanel.Size = new System.Drawing.Size(284, 214);
			dockPanelGradient4.EndColor = System.Drawing.SystemColors.ControlLight;
			dockPanelGradient4.StartColor = System.Drawing.SystemColors.ControlLight;
			autoHideStripSkin2.DockStripGradient = dockPanelGradient4;
			tabGradient8.EndColor = System.Drawing.SystemColors.Control;
			tabGradient8.StartColor = System.Drawing.SystemColors.Control;
			tabGradient8.TextColor = System.Drawing.SystemColors.ControlDarkDark;
			autoHideStripSkin2.TabGradient = tabGradient8;
			autoHideStripSkin2.TextFont = new System.Drawing.Font("メイリオ", 9F);
			dockPanelSkin2.AutoHideStripSkin = autoHideStripSkin2;
			tabGradient9.EndColor = System.Drawing.SystemColors.ControlLightLight;
			tabGradient9.StartColor = System.Drawing.SystemColors.ControlLightLight;
			tabGradient9.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripGradient2.ActiveTabGradient = tabGradient9;
			dockPanelGradient5.EndColor = System.Drawing.SystemColors.Control;
			dockPanelGradient5.StartColor = System.Drawing.SystemColors.Control;
			dockPaneStripGradient2.DockStripGradient = dockPanelGradient5;
			tabGradient10.EndColor = System.Drawing.SystemColors.ControlLight;
			tabGradient10.StartColor = System.Drawing.SystemColors.ControlLight;
			tabGradient10.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripGradient2.InactiveTabGradient = tabGradient10;
			dockPaneStripSkin2.DocumentGradient = dockPaneStripGradient2;
			dockPaneStripSkin2.TextFont = new System.Drawing.Font("メイリオ", 9F);
			tabGradient11.EndColor = System.Drawing.SystemColors.ActiveCaption;
			tabGradient11.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			tabGradient11.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
			tabGradient11.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
			dockPaneStripToolWindowGradient2.ActiveCaptionGradient = tabGradient11;
			tabGradient12.EndColor = System.Drawing.SystemColors.Control;
			tabGradient12.StartColor = System.Drawing.SystemColors.Control;
			tabGradient12.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripToolWindowGradient2.ActiveTabGradient = tabGradient12;
			dockPanelGradient6.EndColor = System.Drawing.SystemColors.ControlLight;
			dockPanelGradient6.StartColor = System.Drawing.SystemColors.ControlLight;
			dockPaneStripToolWindowGradient2.DockStripGradient = dockPanelGradient6;
			tabGradient13.EndColor = System.Drawing.SystemColors.InactiveCaption;
			tabGradient13.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			tabGradient13.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
			tabGradient13.TextColor = System.Drawing.SystemColors.InactiveCaptionText;
			dockPaneStripToolWindowGradient2.InactiveCaptionGradient = tabGradient13;
			tabGradient14.EndColor = System.Drawing.Color.Transparent;
			tabGradient14.StartColor = System.Drawing.Color.Transparent;
			tabGradient14.TextColor = System.Drawing.SystemColors.ControlDarkDark;
			dockPaneStripToolWindowGradient2.InactiveTabGradient = tabGradient14;
			dockPaneStripSkin2.ToolWindowGradient = dockPaneStripToolWindowGradient2;
			dockPanelSkin2.DockPaneStripSkin = dockPaneStripSkin2;
			this.MainDockPanel.Skin = dockPanelSkin2;
			this.MainDockPanel.TabIndex = 0;
			// 
			// StripMenu
			// 
			this.StripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
			this.StripMenu_Debug.Text = "デバッグ(D)";
			// 
			// StripMenu_Debug_LoadAPIFromFile
			// 
			this.StripMenu_Debug_LoadAPIFromFile.Name = "StripMenu_Debug_LoadAPIFromFile";
			this.StripMenu_Debug_LoadAPIFromFile.Size = new System.Drawing.Size(245, 22);
			this.StripMenu_Debug_LoadAPIFromFile.Text = "ファイルからAPIをロード(L)...";
			this.StripMenu_Debug_LoadAPIFromFile.Click += new System.EventHandler(this.StripMenu_Debug_LoadAPIFromFile_Click);
			// 
			// StripStatus
			// 
			this.StripStatus.Location = new System.Drawing.Point(0, 240);
			this.StripStatus.Name = "StripStatus";
			this.StripStatus.Size = new System.Drawing.Size(284, 22);
			this.StripStatus.TabIndex = 3;
			// 
			// UIUpdateTimer
			// 
			this.UIUpdateTimer.Interval = 500;
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
	}
}

