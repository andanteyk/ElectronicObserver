namespace Browser
{
    partial class FormBrowser
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBrowser));
            this.SizeAdjuster = new System.Windows.Forms.Panel();
            this.Browser = new Browser.ExtraWebBrowser();
            this.ContextMenuTool = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ContextMenuTool_ShowToolMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu = new System.Windows.Forms.ToolStrip();
            this.ToolMenu_ScreenShot = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Zoom = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Mute = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Refresh = new System.Windows.Forms.ToolStripButton();
            this.ToolMenu_NavigateToLogInPage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Other = new System.Windows.Forms.ToolStripDropDownButton();
            this.ToolMenu_Other_ScreenShot = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Other_Zoom = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Zoom_Current = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Other_Zoom_Decrement = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Zoom_Increment = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Other_Zoom_25 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Zoom_50 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Zoom_75 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Other_Zoom_100 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Other_Zoom_150 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Zoom_200 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Zoom_250 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Zoom_300 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Zoom_400 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Other_Mute = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Other_Refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_NavigateToLogInPage = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Navigate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Other_AppliesStyleSheet = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Other_Alignment = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Alignment_Top = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Alignment_Bottom = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Alignment_Left = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Alignment_Right = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Alignment_Invisible = new System.Windows.Forms.ToolStripMenuItem();
            this.Icons = new System.Windows.Forms.ImageList(this.components);
            this.SizeAdjuster.SuspendLayout();
            this.ContextMenuTool.SuspendLayout();
            this.ToolMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // SizeAdjuster
            // 
            resources.ApplyResources(this.SizeAdjuster, "SizeAdjuster");
            this.SizeAdjuster.Controls.Add(this.Browser);
            this.SizeAdjuster.Name = "SizeAdjuster";
            this.SizeAdjuster.SizeChanged += new System.EventHandler(this.SizeAdjuster_SizeChanged);
            this.SizeAdjuster.Click += new System.EventHandler(this.SizeAdjuster_Click);
            // 
            // Browser
            // 
            resources.ApplyResources(this.Browser, "Browser");
            this.Browser.AllowWebBrowserDrop = false;
            this.Browser.ContextMenuStrip = this.ContextMenuTool;
            this.Browser.IsWebBrowserContextMenuEnabled = false;
            this.Browser.Name = "Browser";
            this.Browser.ScriptErrorsSuppressed = true;
            this.Browser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.Browser_DocumentCompleted);
            this.Browser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.Browser_Navigating);
            // 
            // ContextMenuTool
            // 
            resources.ApplyResources(this.ContextMenuTool, "ContextMenuTool");
            this.ContextMenuTool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContextMenuTool_ShowToolMenu});
            this.ContextMenuTool.Name = "ContextMenuTool";
            this.ContextMenuTool.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuTool_Opening);
            // 
            // ContextMenuTool_ShowToolMenu
            // 
            resources.ApplyResources(this.ContextMenuTool_ShowToolMenu, "ContextMenuTool_ShowToolMenu");
            this.ContextMenuTool_ShowToolMenu.Name = "ContextMenuTool_ShowToolMenu";
            this.ContextMenuTool_ShowToolMenu.Click += new System.EventHandler(this.ContextMenuTool_ShowToolMenu_Click);
            // 
            // ToolMenu
            // 
            resources.ApplyResources(this.ToolMenu, "ToolMenu");
            this.ToolMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolMenu_ScreenShot,
            this.toolStripSeparator1,
            this.ToolMenu_Zoom,
            this.toolStripSeparator2,
            this.ToolMenu_Mute,
            this.toolStripSeparator13,
            this.ToolMenu_Refresh,
            this.ToolMenu_NavigateToLogInPage,
            this.toolStripSeparator8,
            this.ToolMenu_Other});
            this.ToolMenu.Name = "ToolMenu";
            // 
            // ToolMenu_ScreenShot
            // 
            resources.ApplyResources(this.ToolMenu_ScreenShot, "ToolMenu_ScreenShot");
            this.ToolMenu_ScreenShot.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolMenu_ScreenShot.Name = "ToolMenu_ScreenShot";
            this.ToolMenu_ScreenShot.Click += new System.EventHandler(this.ToolMenu_ScreenShot_Click);
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // ToolMenu_Zoom
            // 
            resources.ApplyResources(this.ToolMenu_Zoom, "ToolMenu_Zoom");
            this.ToolMenu_Zoom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolMenu_Zoom.Name = "ToolMenu_Zoom";
            this.ToolMenu_Zoom.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.ToolMenu_Zoom.DropDownOpening += new System.EventHandler(this.ToolMenu_Zoom_DropDownOpening);
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // ToolMenu_Mute
            // 
            resources.ApplyResources(this.ToolMenu_Mute, "ToolMenu_Mute");
            this.ToolMenu_Mute.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolMenu_Mute.Name = "ToolMenu_Mute";
            this.ToolMenu_Mute.Click += new System.EventHandler(this.ToolMenu_Mute_Click);
            // 
            // toolStripSeparator13
            // 
            resources.ApplyResources(this.toolStripSeparator13, "toolStripSeparator13");
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            // 
            // ToolMenu_Refresh
            // 
            resources.ApplyResources(this.ToolMenu_Refresh, "ToolMenu_Refresh");
            this.ToolMenu_Refresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolMenu_Refresh.Name = "ToolMenu_Refresh";
            this.ToolMenu_Refresh.Click += new System.EventHandler(this.ToolMenu_Refresh_Click);
            // 
            // ToolMenu_NavigateToLogInPage
            // 
            resources.ApplyResources(this.ToolMenu_NavigateToLogInPage, "ToolMenu_NavigateToLogInPage");
            this.ToolMenu_NavigateToLogInPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolMenu_NavigateToLogInPage.Name = "ToolMenu_NavigateToLogInPage";
            this.ToolMenu_NavigateToLogInPage.Click += new System.EventHandler(this.ToolMenu_NavigateToLogInPage_Click);
            // 
            // toolStripSeparator8
            // 
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            // 
            // ToolMenu_Other
            // 
            resources.ApplyResources(this.ToolMenu_Other, "ToolMenu_Other");
            this.ToolMenu_Other.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolMenu_Other.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolMenu_Other_ScreenShot,
            this.toolStripSeparator4,
            this.ToolMenu_Other_Zoom,
            this.toolStripSeparator3,
            this.ToolMenu_Other_Mute,
            this.toolStripSeparator7,
            this.ToolMenu_Other_Refresh,
            this.ToolMenu_Other_NavigateToLogInPage,
            this.ToolMenu_Other_Navigate,
            this.toolStripSeparator5,
            this.ToolMenu_Other_AppliesStyleSheet,
            this.toolStripSeparator6,
            this.ToolMenu_Other_Alignment});
            this.ToolMenu_Other.Name = "ToolMenu_Other";
            this.ToolMenu_Other.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.ToolMenu_Other.DropDownOpening += new System.EventHandler(this.ToolMenu_Other_DropDownOpening);
            // 
            // ToolMenu_Other_ScreenShot
            // 
            resources.ApplyResources(this.ToolMenu_Other_ScreenShot, "ToolMenu_Other_ScreenShot");
            this.ToolMenu_Other_ScreenShot.Name = "ToolMenu_Other_ScreenShot";
            this.ToolMenu_Other_ScreenShot.Click += new System.EventHandler(this.ToolMenu_Other_ScreenShot_Click);
            // 
            // toolStripSeparator4
            // 
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            // 
            // ToolMenu_Other_Zoom
            // 
            resources.ApplyResources(this.ToolMenu_Other_Zoom, "ToolMenu_Other_Zoom");
            this.ToolMenu_Other_Zoom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolMenu_Other_Zoom_Current,
            this.toolStripSeparator9,
            this.ToolMenu_Other_Zoom_Decrement,
            this.ToolMenu_Other_Zoom_Increment,
            this.toolStripSeparator10,
            this.ToolMenu_Other_Zoom_25,
            this.ToolMenu_Other_Zoom_50,
            this.ToolMenu_Other_Zoom_75,
            this.toolStripSeparator11,
            this.ToolMenu_Other_Zoom_100,
            this.toolStripSeparator12,
            this.ToolMenu_Other_Zoom_150,
            this.ToolMenu_Other_Zoom_200,
            this.ToolMenu_Other_Zoom_250,
            this.ToolMenu_Other_Zoom_300,
            this.ToolMenu_Other_Zoom_400});
            this.ToolMenu_Other_Zoom.Name = "ToolMenu_Other_Zoom";
            // 
            // ToolMenu_Other_Zoom_Current
            // 
            resources.ApplyResources(this.ToolMenu_Other_Zoom_Current, "ToolMenu_Other_Zoom_Current");
            this.ToolMenu_Other_Zoom_Current.Name = "ToolMenu_Other_Zoom_Current";
            // 
            // toolStripSeparator9
            // 
            resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            // 
            // ToolMenu_Other_Zoom_Decrement
            // 
            resources.ApplyResources(this.ToolMenu_Other_Zoom_Decrement, "ToolMenu_Other_Zoom_Decrement");
            this.ToolMenu_Other_Zoom_Decrement.Name = "ToolMenu_Other_Zoom_Decrement";
            this.ToolMenu_Other_Zoom_Decrement.Click += new System.EventHandler(this.ToolMenu_Other_Zoom_Decrement_Click);
            // 
            // ToolMenu_Other_Zoom_Increment
            // 
            resources.ApplyResources(this.ToolMenu_Other_Zoom_Increment, "ToolMenu_Other_Zoom_Increment");
            this.ToolMenu_Other_Zoom_Increment.Name = "ToolMenu_Other_Zoom_Increment";
            this.ToolMenu_Other_Zoom_Increment.Click += new System.EventHandler(this.ToolMenu_Other_Zoom_Increment_Click);
            // 
            // toolStripSeparator10
            // 
            resources.ApplyResources(this.toolStripSeparator10, "toolStripSeparator10");
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            // 
            // ToolMenu_Other_Zoom_25
            // 
            resources.ApplyResources(this.ToolMenu_Other_Zoom_25, "ToolMenu_Other_Zoom_25");
            this.ToolMenu_Other_Zoom_25.Name = "ToolMenu_Other_Zoom_25";
            this.ToolMenu_Other_Zoom_25.Click += new System.EventHandler(this.ToolMenu_Other_Zoom_Click);
            // 
            // ToolMenu_Other_Zoom_50
            // 
            resources.ApplyResources(this.ToolMenu_Other_Zoom_50, "ToolMenu_Other_Zoom_50");
            this.ToolMenu_Other_Zoom_50.Name = "ToolMenu_Other_Zoom_50";
            this.ToolMenu_Other_Zoom_50.Click += new System.EventHandler(this.ToolMenu_Other_Zoom_Click);
            // 
            // ToolMenu_Other_Zoom_75
            // 
            resources.ApplyResources(this.ToolMenu_Other_Zoom_75, "ToolMenu_Other_Zoom_75");
            this.ToolMenu_Other_Zoom_75.Name = "ToolMenu_Other_Zoom_75";
            this.ToolMenu_Other_Zoom_75.Click += new System.EventHandler(this.ToolMenu_Other_Zoom_Click);
            // 
            // toolStripSeparator11
            // 
            resources.ApplyResources(this.toolStripSeparator11, "toolStripSeparator11");
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            // 
            // ToolMenu_Other_Zoom_100
            // 
            resources.ApplyResources(this.ToolMenu_Other_Zoom_100, "ToolMenu_Other_Zoom_100");
            this.ToolMenu_Other_Zoom_100.Name = "ToolMenu_Other_Zoom_100";
            this.ToolMenu_Other_Zoom_100.Click += new System.EventHandler(this.ToolMenu_Other_Zoom_Click);
            // 
            // toolStripSeparator12
            // 
            resources.ApplyResources(this.toolStripSeparator12, "toolStripSeparator12");
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            // 
            // ToolMenu_Other_Zoom_150
            // 
            resources.ApplyResources(this.ToolMenu_Other_Zoom_150, "ToolMenu_Other_Zoom_150");
            this.ToolMenu_Other_Zoom_150.Name = "ToolMenu_Other_Zoom_150";
            this.ToolMenu_Other_Zoom_150.Click += new System.EventHandler(this.ToolMenu_Other_Zoom_Click);
            // 
            // ToolMenu_Other_Zoom_200
            // 
            resources.ApplyResources(this.ToolMenu_Other_Zoom_200, "ToolMenu_Other_Zoom_200");
            this.ToolMenu_Other_Zoom_200.Name = "ToolMenu_Other_Zoom_200";
            this.ToolMenu_Other_Zoom_200.Click += new System.EventHandler(this.ToolMenu_Other_Zoom_Click);
            // 
            // ToolMenu_Other_Zoom_250
            // 
            resources.ApplyResources(this.ToolMenu_Other_Zoom_250, "ToolMenu_Other_Zoom_250");
            this.ToolMenu_Other_Zoom_250.Name = "ToolMenu_Other_Zoom_250";
            this.ToolMenu_Other_Zoom_250.Click += new System.EventHandler(this.ToolMenu_Other_Zoom_Click);
            // 
            // ToolMenu_Other_Zoom_300
            // 
            resources.ApplyResources(this.ToolMenu_Other_Zoom_300, "ToolMenu_Other_Zoom_300");
            this.ToolMenu_Other_Zoom_300.Name = "ToolMenu_Other_Zoom_300";
            this.ToolMenu_Other_Zoom_300.Click += new System.EventHandler(this.ToolMenu_Other_Zoom_Click);
            // 
            // ToolMenu_Other_Zoom_400
            // 
            resources.ApplyResources(this.ToolMenu_Other_Zoom_400, "ToolMenu_Other_Zoom_400");
            this.ToolMenu_Other_Zoom_400.Name = "ToolMenu_Other_Zoom_400";
            this.ToolMenu_Other_Zoom_400.Click += new System.EventHandler(this.ToolMenu_Other_Zoom_Click);
            // 
            // toolStripSeparator3
            // 
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            // 
            // ToolMenu_Other_Mute
            // 
            resources.ApplyResources(this.ToolMenu_Other_Mute, "ToolMenu_Other_Mute");
            this.ToolMenu_Other_Mute.Name = "ToolMenu_Other_Mute";
            this.ToolMenu_Other_Mute.Click += new System.EventHandler(this.ToolMenu_Other_Mute_Click);
            // 
            // toolStripSeparator7
            // 
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            // 
            // ToolMenu_Other_Refresh
            // 
            resources.ApplyResources(this.ToolMenu_Other_Refresh, "ToolMenu_Other_Refresh");
            this.ToolMenu_Other_Refresh.Name = "ToolMenu_Other_Refresh";
            this.ToolMenu_Other_Refresh.Click += new System.EventHandler(this.ToolMenu_Other_Refresh_Click);
            // 
            // ToolMenu_Other_NavigateToLogInPage
            // 
            resources.ApplyResources(this.ToolMenu_Other_NavigateToLogInPage, "ToolMenu_Other_NavigateToLogInPage");
            this.ToolMenu_Other_NavigateToLogInPage.Name = "ToolMenu_Other_NavigateToLogInPage";
            this.ToolMenu_Other_NavigateToLogInPage.Click += new System.EventHandler(this.ToolMenu_Other_NavigateToLogInPage_Click);
            // 
            // ToolMenu_Other_Navigate
            // 
            resources.ApplyResources(this.ToolMenu_Other_Navigate, "ToolMenu_Other_Navigate");
            this.ToolMenu_Other_Navigate.Name = "ToolMenu_Other_Navigate";
            this.ToolMenu_Other_Navigate.Click += new System.EventHandler(this.ToolMenu_Other_Navigate_Click);
            // 
            // toolStripSeparator5
            // 
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            // 
            // ToolMenu_Other_AppliesStyleSheet
            // 
            resources.ApplyResources(this.ToolMenu_Other_AppliesStyleSheet, "ToolMenu_Other_AppliesStyleSheet");
            this.ToolMenu_Other_AppliesStyleSheet.CheckOnClick = true;
            this.ToolMenu_Other_AppliesStyleSheet.Name = "ToolMenu_Other_AppliesStyleSheet";
            this.ToolMenu_Other_AppliesStyleSheet.Click += new System.EventHandler(this.ToolMenu_Other_AppliesStyleSheet_Click);
            // 
            // toolStripSeparator6
            // 
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            // 
            // ToolMenu_Other_Alignment
            // 
            resources.ApplyResources(this.ToolMenu_Other_Alignment, "ToolMenu_Other_Alignment");
            this.ToolMenu_Other_Alignment.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolMenu_Other_Alignment_Top,
            this.ToolMenu_Other_Alignment_Bottom,
            this.ToolMenu_Other_Alignment_Left,
            this.ToolMenu_Other_Alignment_Right,
            this.ToolMenu_Other_Alignment_Invisible});
            this.ToolMenu_Other_Alignment.Name = "ToolMenu_Other_Alignment";
            this.ToolMenu_Other_Alignment.DropDownOpening += new System.EventHandler(this.ToolMenu_Other_Alignment_DropDownOpening);
            // 
            // ToolMenu_Other_Alignment_Top
            // 
            resources.ApplyResources(this.ToolMenu_Other_Alignment_Top, "ToolMenu_Other_Alignment_Top");
            this.ToolMenu_Other_Alignment_Top.Name = "ToolMenu_Other_Alignment_Top";
            this.ToolMenu_Other_Alignment_Top.Click += new System.EventHandler(this.ToolMenu_Other_Alignment_Click);
            // 
            // ToolMenu_Other_Alignment_Bottom
            // 
            resources.ApplyResources(this.ToolMenu_Other_Alignment_Bottom, "ToolMenu_Other_Alignment_Bottom");
            this.ToolMenu_Other_Alignment_Bottom.Name = "ToolMenu_Other_Alignment_Bottom";
            this.ToolMenu_Other_Alignment_Bottom.Click += new System.EventHandler(this.ToolMenu_Other_Alignment_Click);
            // 
            // ToolMenu_Other_Alignment_Left
            // 
            resources.ApplyResources(this.ToolMenu_Other_Alignment_Left, "ToolMenu_Other_Alignment_Left");
            this.ToolMenu_Other_Alignment_Left.Name = "ToolMenu_Other_Alignment_Left";
            this.ToolMenu_Other_Alignment_Left.Click += new System.EventHandler(this.ToolMenu_Other_Alignment_Click);
            // 
            // ToolMenu_Other_Alignment_Right
            // 
            resources.ApplyResources(this.ToolMenu_Other_Alignment_Right, "ToolMenu_Other_Alignment_Right");
            this.ToolMenu_Other_Alignment_Right.Name = "ToolMenu_Other_Alignment_Right";
            this.ToolMenu_Other_Alignment_Right.Click += new System.EventHandler(this.ToolMenu_Other_Alignment_Click);
            // 
            // ToolMenu_Other_Alignment_Invisible
            // 
            resources.ApplyResources(this.ToolMenu_Other_Alignment_Invisible, "ToolMenu_Other_Alignment_Invisible");
            this.ToolMenu_Other_Alignment_Invisible.Name = "ToolMenu_Other_Alignment_Invisible";
            this.ToolMenu_Other_Alignment_Invisible.Click += new System.EventHandler(this.ToolMenu_Other_Alignment_Invisible_Click);
            // 
            // Icons
            // 
            this.Icons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            resources.ApplyResources(this.Icons, "Icons");
            this.Icons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // FormBrowser
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SizeAdjuster);
            this.Controls.Add(this.ToolMenu);
            this.KeyPreview = true;
            this.Name = "FormBrowser";
            this.Activated += new System.EventHandler(this.FormBrowser_Activated);
            this.Load += new System.EventHandler(this.FormBrowser_Load);
            this.SizeAdjuster.ResumeLayout(false);
            this.ContextMenuTool.ResumeLayout(false);
            this.ToolMenu.ResumeLayout(false);
            this.ToolMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel SizeAdjuster;
		private ExtraWebBrowser Browser;
		private System.Windows.Forms.ToolStrip ToolMenu;
		private System.Windows.Forms.ToolStripButton ToolMenu_ScreenShot;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripDropDownButton ToolMenu_Zoom;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton ToolMenu_Refresh;
		private System.Windows.Forms.ToolStripButton ToolMenu_NavigateToLogInPage;
		private System.Windows.Forms.ToolStripDropDownButton ToolMenu_Other;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_ScreenShot;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Refresh;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_NavigateToLogInPage;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Navigate;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_AppliesStyleSheet;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Alignment;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Alignment_Top;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Alignment_Bottom;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Alignment_Left;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Alignment_Right;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Alignment_Invisible;
		private System.Windows.Forms.ImageList Icons;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Mute;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom_Current;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom_Decrement;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom_Increment;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom_25;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom_50;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom_75;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom_100;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom_150;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom_200;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom_250;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom_300;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom_400;
		private System.Windows.Forms.ToolStripButton ToolMenu_Mute;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
		private System.Windows.Forms.ContextMenuStrip ContextMenuTool;
		private System.Windows.Forms.ToolStripMenuItem ContextMenuTool_ShowToolMenu;

    }
}

