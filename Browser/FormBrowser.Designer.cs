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
			this.ToolMenu_Mute_Track = new System.Windows.Forms.ToolStripDropDownButton();
			this.trackVolume = new System.Windows.Forms.TrackBar();
			this.toolStripSeparatorAnother1 = new System.Windows.Forms.ToolStripSeparator();
			this.ToolMenu_Url = new Browser.Control.ToolStripSpringTextBox();
			this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
			this.ToolMenu_Refresh = new System.Windows.Forms.ToolStripButton();
			this.ToolMenu_NavigateToLogInPage = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
			this.ToolMenu_Other = new System.Windows.Forms.ToolStripDropDownButton();
			this.ToolMenu_Other_ScreenShot = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolMenu_Other_LastScreenShot = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
			this.ToolMenu_Other_LastScreenShot_OpenScreenShotFolder = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.ToolMenu_Other_Zoom = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolMenu_Other_Zoom_Current = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
			this.ToolMenu_Other_Zoom_Fit = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
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
			this.ToolMenu_Other_Volume = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolMenu_Other_Mute = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.ToolMenu_Other_Refresh = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolMenu_Other_NavigateToLogInPage = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolMenu_Other_Navigate = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.ToolMenu_Other_AppliesStyleSheet = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolMenu_Other_ClearCache = new System.Windows.Forms.ToolStripMenuItem();
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
			this.SizeAdjuster.Controls.Add(this.trackVolume);
			this.SizeAdjuster.Controls.Add(this.Browser);
			this.SizeAdjuster.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SizeAdjuster.Location = new System.Drawing.Point(0, 25);
			this.SizeAdjuster.Name = "SizeAdjuster";
			this.SizeAdjuster.Size = new System.Drawing.Size(284, 236);
			this.SizeAdjuster.TabIndex = 0;
			this.SizeAdjuster.SizeChanged += new System.EventHandler(this.SizeAdjuster_SizeChanged);
			this.SizeAdjuster.DoubleClick += new System.EventHandler(this.SizeAdjuster_DoubleClick);
			// 
			// trackVolume
			// 
			this.trackVolume.Location = new System.Drawing.Point(75, 3);
			this.trackVolume.Maximum = 100;
			this.trackVolume.Name = "trackVolume";
			this.trackVolume.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.trackVolume.Size = new System.Drawing.Size(50, 104);
			this.trackVolume.TabIndex = 2;
			this.trackVolume.TickFrequency = 50;
			this.trackVolume.Value = 100;
			this.trackVolume.Visible = false;
			this.trackVolume.ValueChanged += new System.EventHandler(this.trackVolume_ValueChanged);
			this.trackVolume.LostFocus += new System.EventHandler(this.trackVolume_LostFocus);
			// 
			// Browser
			// 
			this.Browser.AllowWebBrowserDrop = false;
			this.Browser.ContextMenuStrip = this.ContextMenuTool;
			this.Browser.IsWebBrowserContextMenuEnabled = false;
			this.Browser.Location = new System.Drawing.Point(0, 0);
			this.Browser.MinimumSize = new System.Drawing.Size(20, 20);
			this.Browser.Name = "Browser";
			this.Browser.ScriptErrorsSuppressed = true;
			this.Browser.Size = new System.Drawing.Size(284, 236);
			this.Browser.TabIndex = 0;
			this.Browser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.Browser_DocumentCompleted);
			this.Browser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.Browser_Navigating);
			this.Browser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.Browser_Navigated);
			// 
			// ContextMenuTool
			// 
			this.ContextMenuTool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContextMenuTool_ShowToolMenu});
			this.ContextMenuTool.Name = "ContextMenuTool";
			this.ContextMenuTool.Size = new System.Drawing.Size(172, 26);
			this.ContextMenuTool.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuTool_Opening);
			// 
			// ContextMenuTool_ShowToolMenu
			// 
			this.ContextMenuTool_ShowToolMenu.Name = "ContextMenuTool_ShowToolMenu";
			this.ContextMenuTool_ShowToolMenu.Size = new System.Drawing.Size(171, 22);
			this.ContextMenuTool_ShowToolMenu.Text = "显示工具栏";
			this.ContextMenuTool_ShowToolMenu.Click += new System.EventHandler(this.ContextMenuTool_ShowToolMenu_Click);
			// 
			// ToolMenu
			// 
			this.ToolMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolMenu_ScreenShot,
            this.toolStripSeparator1,
            this.ToolMenu_Zoom,
            this.toolStripSeparator2,
#if !NOVOL
            this.ToolMenu_Mute,
			this.ToolMenu_Mute_Track,
            this.toolStripSeparator13,
#endif
            this.ToolMenu_Refresh,
            this.ToolMenu_NavigateToLogInPage,
            this.toolStripSeparator8,
            this.ToolMenu_Other,
			this.toolStripSeparatorAnother1,
			this.ToolMenu_Url} );
			this.ToolMenu.Location = new System.Drawing.Point(0, 0);
			this.ToolMenu.Name = "ToolMenu";
			this.ToolMenu.Size = new System.Drawing.Size(284, 25);
			this.ToolMenu.Stretch = true;
			this.ToolMenu.TabIndex = 1;
			this.ToolMenu.Text = "toolStrip1";
			// 
			// ToolMenu_ScreenShot
			// 
			this.ToolMenu_ScreenShot.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ToolMenu_ScreenShot.Name = "ToolMenu_ScreenShot";
			this.ToolMenu_ScreenShot.Size = new System.Drawing.Size(23, 22);
			this.ToolMenu_ScreenShot.Text = "屏幕截图";
			this.ToolMenu_ScreenShot.Click += new System.EventHandler(this.ToolMenu_ScreenShot_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// ToolMenu_Zoom
			// 
			this.ToolMenu_Zoom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ToolMenu_Zoom.Name = "ToolMenu_Zoom";
			this.ToolMenu_Zoom.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.ToolMenu_Zoom.Size = new System.Drawing.Size(15, 22);
			this.ToolMenu_Zoom.Text = "缩放";
			this.ToolMenu_Zoom.DropDownOpening += new System.EventHandler(this.ToolMenu_Zoom_DropDownOpening);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// ToolMenu_Mute
			// 
			this.ToolMenu_Mute.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ToolMenu_Mute.Name = "ToolMenu_Mute";
			this.ToolMenu_Mute.Size = new System.Drawing.Size(23, 22);
			this.ToolMenu_Mute.Text = "静音";
			this.ToolMenu_Mute.Click += new System.EventHandler(ToolMenu_Mute_Click);
			// 
			// ToolMenu_Mute_Track
			// 
			this.ToolMenu_Mute_Track.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
			this.ToolMenu_Mute_Track.Enabled = false;
			this.ToolMenu_Mute_Track.Name = "ToolMenu_Mute_Track";
			this.ToolMenu_Mute_Track.Size = new System.Drawing.Size(23, 22);
			this.ToolMenu_Mute_Track.Text = "音量调节";
			this.ToolMenu_Mute_Track.DropDownOpening += new System.EventHandler(this.ToolMenu_Mute_DropDownOpening);
			// 
			// toolStripSeparator13
			// 
			this.toolStripSeparator13.Name = "toolStripSeparator13";
			this.toolStripSeparator13.Size = new System.Drawing.Size(6, 25);
			// 
			// ToolMenu_Refresh
			// 
			this.ToolMenu_Refresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ToolMenu_Refresh.Name = "ToolMenu_Refresh";
			this.ToolMenu_Refresh.Size = new System.Drawing.Size(23, 22);
			this.ToolMenu_Refresh.Text = "刷新";
			this.ToolMenu_Refresh.Click += new System.EventHandler(this.ToolMenu_Refresh_Click);
			// 
			// ToolMenu_NavigateToLogInPage
			// 
			this.ToolMenu_NavigateToLogInPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ToolMenu_NavigateToLogInPage.Name = "ToolMenu_NavigateToLogInPage";
			this.ToolMenu_NavigateToLogInPage.Size = new System.Drawing.Size(23, 22);
			this.ToolMenu_NavigateToLogInPage.Text = "移动至登录页";
			this.ToolMenu_NavigateToLogInPage.Click += new System.EventHandler(this.ToolMenu_NavigateToLogInPage_Click);
			// 
			// toolStripSeparator8
			// 
			this.toolStripSeparator8.Name = "toolStripSeparator8";
			this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripSeparatorAnother1
			// 
			this.toolStripSeparatorAnother1.Name = "toolStripSeparatorAnother1";
			this.toolStripSeparatorAnother1.Size = new System.Drawing.Size(6, 38);
			// 
			// ToolMenu_Url
			// 
			this.ToolMenu_Url.Name = "ToolMenu_Url";
			this.ToolMenu_Url.ReadOnly = true;
			this.ToolMenu_Url.Size = new System.Drawing.Size( 160, 35 );
			this.ToolMenu_Url.Text = "";
			// 
			// ToolMenu_Other
			// 
			this.ToolMenu_Other.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ToolMenu_Other.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolMenu_Other_ScreenShot,
            this.ToolMenu_Other_LastScreenShot,
            this.toolStripSeparator4,
            this.ToolMenu_Other_Zoom,
            this.toolStripSeparator3,
            this.ToolMenu_Other_Volume,
            this.ToolMenu_Other_Mute,
            this.toolStripSeparator7,
            this.ToolMenu_Other_Refresh,
            this.ToolMenu_Other_NavigateToLogInPage,
            this.ToolMenu_Other_Navigate,
            this.toolStripSeparator5,
            this.ToolMenu_Other_AppliesStyleSheet,
            this.ToolMenu_Other_ClearCache,
            this.toolStripSeparator6,
            this.ToolMenu_Other_Alignment});
			this.ToolMenu_Other.Name = "ToolMenu_Other";
			this.ToolMenu_Other.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.ToolMenu_Other.Size = new System.Drawing.Size(15, 22);
			this.ToolMenu_Other.Text = "其他";
			this.ToolMenu_Other.DropDownOpening += new System.EventHandler(this.ToolMenu_Other_DropDownOpening);
			// 
			// ToolMenu_Other_ScreenShot
			// 
			this.ToolMenu_Other_ScreenShot.Name = "ToolMenu_Other_ScreenShot";
			this.ToolMenu_Other_ScreenShot.ShortcutKeys = System.Windows.Forms.Keys.F2;
			this.ToolMenu_Other_ScreenShot.Size = new System.Drawing.Size(199, 22);
			this.ToolMenu_Other_ScreenShot.Text = "屏幕截图(&S)";
			this.ToolMenu_Other_ScreenShot.Click += new System.EventHandler(this.ToolMenu_Other_ScreenShot_Click);
			// 
			// ToolMenu_Other_LastScreenShot
			// 
			this.ToolMenu_Other_LastScreenShot.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator15,
            this.ToolMenu_Other_LastScreenShot_OpenScreenShotFolder});
			this.ToolMenu_Other_LastScreenShot.Name = "ToolMenu_Other_LastScreenShot";
			this.ToolMenu_Other_LastScreenShot.Size = new System.Drawing.Size(199, 22);
			this.ToolMenu_Other_LastScreenShot.Text = "直前のスクリーンショット(&P)";
			this.ToolMenu_Other_LastScreenShot.DropDownOpening += new System.EventHandler(this.ToolMenu_Other_LastScreenShot_DropDownOpening);
			// 
			// toolStripSeparator15
			// 
			this.toolStripSeparator15.Name = "toolStripSeparator15";
			this.toolStripSeparator15.Size = new System.Drawing.Size(176, 6);
			// 
			// ToolMenu_Other_LastScreenShot_OpenScreenShotFolder
			// 
			this.ToolMenu_Other_LastScreenShot_OpenScreenShotFolder.Name = "ToolMenu_Other_LastScreenShot_OpenScreenShotFolder";
			this.ToolMenu_Other_LastScreenShot_OpenScreenShotFolder.Size = new System.Drawing.Size(179, 22);
			this.ToolMenu_Other_LastScreenShot_OpenScreenShotFolder.Text = "保存フォルダを開く(&O)";
			this.ToolMenu_Other_LastScreenShot_OpenScreenShotFolder.Click += new System.EventHandler(this.ToolMenu_Other_LastScreenShot_OpenScreenShotFolder_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(196, 6);
			// 
			// ToolMenu_Other_Zoom
			// 
			this.ToolMenu_Other_Zoom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolMenu_Other_Zoom_Current,
            this.toolStripSeparator9,
            this.ToolMenu_Other_Zoom_Fit,
            this.toolStripSeparator14,
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
			this.ToolMenu_Other_Zoom.Size = new System.Drawing.Size(199, 22);
			this.ToolMenu_Other_Zoom.Text = "缩放(&Z)";
			// 
			// ToolMenu_Other_Zoom_Current
			// 
			this.ToolMenu_Other_Zoom_Current.Enabled = false;
			this.ToolMenu_Other_Zoom_Current.Name = "ToolMenu_Other_Zoom_Current";
			this.ToolMenu_Other_Zoom_Current.Size = new System.Drawing.Size(110, 22);
			this.ToolMenu_Other_Zoom_Current.Text = "当前%";
			// 
			// toolStripSeparator9
			// 
			this.toolStripSeparator9.Name = "toolStripSeparator9";
			this.toolStripSeparator9.Size = new System.Drawing.Size(107, 6);
			// 
			// ToolMenu_Other_Zoom_Fit
			// 
			this.ToolMenu_Other_Zoom_Fit.CheckOnClick = true;
			this.ToolMenu_Other_Zoom_Fit.Name = "ToolMenu_Other_Zoom_Fit";
			this.ToolMenu_Other_Zoom_Fit.Size = new System.Drawing.Size(110, 22);
			this.ToolMenu_Other_Zoom_Fit.Text = "自适应";
			this.ToolMenu_Other_Zoom_Fit.Click += new System.EventHandler(this.ToolMenu_Other_Zoom_Fit_Click);
			// 
			// toolStripSeparator14
			// 
			this.toolStripSeparator14.Name = "toolStripSeparator14";
			this.toolStripSeparator14.Size = new System.Drawing.Size(107, 6);
			// 
			// ToolMenu_Other_Zoom_Decrement
			// 
			this.ToolMenu_Other_Zoom_Decrement.Name = "ToolMenu_Other_Zoom_Decrement";
			this.ToolMenu_Other_Zoom_Decrement.Size = new System.Drawing.Size(110, 22);
			this.ToolMenu_Other_Zoom_Decrement.Text = "-20%";
			this.ToolMenu_Other_Zoom_Decrement.Click += new System.EventHandler(this.ToolMenu_Other_Zoom_Decrement_Click);
			// 
			// ToolMenu_Other_Zoom_Increment
			// 
			this.ToolMenu_Other_Zoom_Increment.Name = "ToolMenu_Other_Zoom_Increment";
			this.ToolMenu_Other_Zoom_Increment.Size = new System.Drawing.Size(110, 22);
			this.ToolMenu_Other_Zoom_Increment.Text = "+20%";
			this.ToolMenu_Other_Zoom_Increment.Click += new System.EventHandler(this.ToolMenu_Other_Zoom_Increment_Click);
			// 
			// toolStripSeparator10
			// 
			this.toolStripSeparator10.Name = "toolStripSeparator10";
			this.toolStripSeparator10.Size = new System.Drawing.Size(107, 6);
			// 
			// ToolMenu_Other_Zoom_25
			// 
			this.ToolMenu_Other_Zoom_25.Name = "ToolMenu_Other_Zoom_25";
			this.ToolMenu_Other_Zoom_25.Size = new System.Drawing.Size(110, 22);
			this.ToolMenu_Other_Zoom_25.Text = "25%";
			this.ToolMenu_Other_Zoom_25.Click += new System.EventHandler(this.ToolMenu_Other_Zoom_Click);
			// 
			// ToolMenu_Other_Zoom_50
			// 
			this.ToolMenu_Other_Zoom_50.Name = "ToolMenu_Other_Zoom_50";
			this.ToolMenu_Other_Zoom_50.Size = new System.Drawing.Size(110, 22);
			this.ToolMenu_Other_Zoom_50.Text = "50%";
			this.ToolMenu_Other_Zoom_50.Click += new System.EventHandler(this.ToolMenu_Other_Zoom_Click);
			// 
			// ToolMenu_Other_Zoom_75
			// 
			this.ToolMenu_Other_Zoom_75.Name = "ToolMenu_Other_Zoom_75";
			this.ToolMenu_Other_Zoom_75.Size = new System.Drawing.Size(110, 22);
			this.ToolMenu_Other_Zoom_75.Text = "75%";
			this.ToolMenu_Other_Zoom_75.Click += new System.EventHandler(this.ToolMenu_Other_Zoom_Click);
			// 
			// toolStripSeparator11
			// 
			this.toolStripSeparator11.Name = "toolStripSeparator11";
			this.toolStripSeparator11.Size = new System.Drawing.Size(107, 6);
			// 
			// ToolMenu_Other_Zoom_100
			// 
			this.ToolMenu_Other_Zoom_100.Name = "ToolMenu_Other_Zoom_100";
			this.ToolMenu_Other_Zoom_100.Size = new System.Drawing.Size(110, 22);
			this.ToolMenu_Other_Zoom_100.Text = "100%";
			this.ToolMenu_Other_Zoom_100.Click += new System.EventHandler(this.ToolMenu_Other_Zoom_Click);
			// 
			// toolStripSeparator12
			// 
			this.toolStripSeparator12.Name = "toolStripSeparator12";
			this.toolStripSeparator12.Size = new System.Drawing.Size(107, 6);
			// 
			// ToolMenu_Other_Zoom_150
			// 
			this.ToolMenu_Other_Zoom_150.Name = "ToolMenu_Other_Zoom_150";
			this.ToolMenu_Other_Zoom_150.Size = new System.Drawing.Size(110, 22);
			this.ToolMenu_Other_Zoom_150.Text = "150%";
			this.ToolMenu_Other_Zoom_150.Click += new System.EventHandler(this.ToolMenu_Other_Zoom_Click);
			// 
			// ToolMenu_Other_Zoom_200
			// 
			this.ToolMenu_Other_Zoom_200.Name = "ToolMenu_Other_Zoom_200";
			this.ToolMenu_Other_Zoom_200.Size = new System.Drawing.Size(110, 22);
			this.ToolMenu_Other_Zoom_200.Text = "200%";
			this.ToolMenu_Other_Zoom_200.Click += new System.EventHandler(this.ToolMenu_Other_Zoom_Click);
			// 
			// ToolMenu_Other_Zoom_250
			// 
			this.ToolMenu_Other_Zoom_250.Name = "ToolMenu_Other_Zoom_250";
			this.ToolMenu_Other_Zoom_250.Size = new System.Drawing.Size(110, 22);
			this.ToolMenu_Other_Zoom_250.Text = "250%";
			this.ToolMenu_Other_Zoom_250.Click += new System.EventHandler(this.ToolMenu_Other_Zoom_Click);
			// 
			// ToolMenu_Other_Zoom_300
			// 
			this.ToolMenu_Other_Zoom_300.Name = "ToolMenu_Other_Zoom_300";
			this.ToolMenu_Other_Zoom_300.Size = new System.Drawing.Size(110, 22);
			this.ToolMenu_Other_Zoom_300.Text = "300%";
			this.ToolMenu_Other_Zoom_300.Click += new System.EventHandler(this.ToolMenu_Other_Zoom_Click);
			// 
			// ToolMenu_Other_Zoom_400
			// 
			this.ToolMenu_Other_Zoom_400.Name = "ToolMenu_Other_Zoom_400";
			this.ToolMenu_Other_Zoom_400.Size = new System.Drawing.Size(110, 22);
			this.ToolMenu_Other_Zoom_400.Text = "400%";
			this.ToolMenu_Other_Zoom_400.Click += new System.EventHandler(this.ToolMenu_Other_Zoom_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(196, 6);
			// 
			// ToolMenu_Other_Volume
			// 
			this.ToolMenu_Other_Volume.Name = "ToolMenu_Other_Volume";
			this.ToolMenu_Other_Volume.Size = new System.Drawing.Size(199, 22);
			this.ToolMenu_Other_Volume.Text = "音量(&V)";
			// 
			// ToolMenu_Other_Mute
			// 
			this.ToolMenu_Other_Mute.Name = "ToolMenu_Other_Mute";
			this.ToolMenu_Other_Mute.ShortcutKeys = System.Windows.Forms.Keys.F7;
			this.ToolMenu_Other_Mute.Size = new System.Drawing.Size(199, 22);
			this.ToolMenu_Other_Mute.Text = "静音(&M)";
			this.ToolMenu_Other_Mute.Click += new System.EventHandler(this.ToolMenu_Other_Mute_Click);
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size(196, 6);
			// 
			// ToolMenu_Other_Refresh
			// 
			this.ToolMenu_Other_Refresh.Name = "ToolMenu_Other_Refresh";
			this.ToolMenu_Other_Refresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.ToolMenu_Other_Refresh.Size = new System.Drawing.Size(199, 22);
			this.ToolMenu_Other_Refresh.Text = "刷新(&R)";
			this.ToolMenu_Other_Refresh.Click += new System.EventHandler(this.ToolMenu_Other_Refresh_Click);
			// 
			// ToolMenu_Other_NavigateToLogInPage
			// 
			this.ToolMenu_Other_NavigateToLogInPage.Name = "ToolMenu_Other_NavigateToLogInPage";
			this.ToolMenu_Other_NavigateToLogInPage.ShortcutKeys = System.Windows.Forms.Keys.F6;
			this.ToolMenu_Other_NavigateToLogInPage.Size = new System.Drawing.Size(199, 22);
			this.ToolMenu_Other_NavigateToLogInPage.Text = "ログインページへ移動(&L)";
			this.ToolMenu_Other_NavigateToLogInPage.Text = "移动至登录页(&L)";
			this.ToolMenu_Other_NavigateToLogInPage.Click += new System.EventHandler(this.ToolMenu_Other_NavigateToLogInPage_Click);
			// 
			// ToolMenu_Other_Navigate
			// 
			this.ToolMenu_Other_Navigate.Name = "ToolMenu_Other_Navigate";
			this.ToolMenu_Other_Navigate.Size = new System.Drawing.Size(199, 22);
			this.ToolMenu_Other_Navigate.Text = "移动(&N)...";
			this.ToolMenu_Other_Navigate.Click += new System.EventHandler(this.ToolMenu_Other_Navigate_Click);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(196, 6);
			// 
			// ToolMenu_Other_AppliesStyleSheet
			// 
			this.ToolMenu_Other_AppliesStyleSheet.CheckOnClick = true;
			this.ToolMenu_Other_AppliesStyleSheet.Name = "ToolMenu_Other_AppliesStyleSheet";
			this.ToolMenu_Other_AppliesStyleSheet.Size = new System.Drawing.Size(199, 22);
			this.ToolMenu_Other_AppliesStyleSheet.Text = "应用样式表";
			this.ToolMenu_Other_AppliesStyleSheet.Click += new System.EventHandler(this.ToolMenu_Other_AppliesStyleSheet_Click);
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size(196, 6);
			// 
			// ToolMenu_Other_Alignment
			// 
			this.ToolMenu_Other_Alignment.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolMenu_Other_Alignment_Top,
            this.ToolMenu_Other_Alignment_Bottom,
            this.ToolMenu_Other_Alignment_Left,
            this.ToolMenu_Other_Alignment_Right,
            this.ToolMenu_Other_Alignment_Invisible});
			this.ToolMenu_Other_Alignment.Name = "ToolMenu_Other_Alignment";
			this.ToolMenu_Other_Alignment.Size = new System.Drawing.Size(199, 22);
			this.ToolMenu_Other_Alignment.Text = "位置(&A)";
			this.ToolMenu_Other_Alignment.DropDownOpening += new System.EventHandler(this.ToolMenu_Other_Alignment_DropDownOpening);
			// 
			// ToolMenu_Other_Alignment_Top
			// 
			this.ToolMenu_Other_Alignment_Top.Name = "ToolMenu_Other_Alignment_Top";
			this.ToolMenu_Other_Alignment_Top.Size = new System.Drawing.Size(125, 22);
			this.ToolMenu_Other_Alignment_Top.Text = "上(&T)";
			this.ToolMenu_Other_Alignment_Top.Click += new System.EventHandler(this.ToolMenu_Other_Alignment_Click);
			// 
			// ToolMenu_Other_Alignment_Bottom
			// 
			this.ToolMenu_Other_Alignment_Bottom.Name = "ToolMenu_Other_Alignment_Bottom";
			this.ToolMenu_Other_Alignment_Bottom.Size = new System.Drawing.Size(125, 22);
			this.ToolMenu_Other_Alignment_Bottom.Text = "下(&B)";
			this.ToolMenu_Other_Alignment_Bottom.Click += new System.EventHandler(this.ToolMenu_Other_Alignment_Click);
			// 
			// ToolMenu_Other_Alignment_Left
			// 
			this.ToolMenu_Other_Alignment_Left.Name = "ToolMenu_Other_Alignment_Left";
			this.ToolMenu_Other_Alignment_Left.Size = new System.Drawing.Size(125, 22);
			this.ToolMenu_Other_Alignment_Left.Text = "左(&L)";
			this.ToolMenu_Other_Alignment_Left.Click += new System.EventHandler(this.ToolMenu_Other_Alignment_Click);
			// 
			// ToolMenu_Other_Alignment_Right
			// 
			this.ToolMenu_Other_Alignment_Right.Name = "ToolMenu_Other_Alignment_Right";
			this.ToolMenu_Other_Alignment_Right.Size = new System.Drawing.Size(125, 22);
			this.ToolMenu_Other_Alignment_Right.Text = "右(&R)";
			this.ToolMenu_Other_Alignment_Right.Click += new System.EventHandler(this.ToolMenu_Other_Alignment_Click);
			// 
			// ToolMenu_Other_Alignment_Invisible
			// 
			this.ToolMenu_Other_Alignment_Invisible.Name = "ToolMenu_Other_Alignment_Invisible";
			this.ToolMenu_Other_Alignment_Invisible.Size = new System.Drawing.Size(125, 22);
			this.ToolMenu_Other_Alignment_Invisible.Text = "隐藏(&I)";
			this.ToolMenu_Other_Alignment_Invisible.Click += new System.EventHandler(this.ToolMenu_Other_Alignment_Invisible_Click);
			// 
			// ToolMenu_Other_ClearCache
			// 
			this.ToolMenu_Other_ClearCache.Name = "ToolMenu_Other_ClearCache";
			this.ToolMenu_Other_ClearCache.Size = new System.Drawing.Size(319, 34);
			this.ToolMenu_Other_ClearCache.Text = "清除缓存(&C)";
			this.ToolMenu_Other_ClearCache.Click += new System.EventHandler(this.ToolMenu_Other_ClearCache_Click);
			// 
			// Icons
			// 
			this.Icons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.Icons.ImageSize = new System.Drawing.Size(16, 16);
			this.Icons.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// FormBrowser
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.SizeAdjuster);
			this.Controls.Add(this.ToolMenu);
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.KeyPreview = true;
			this.Name = "FormBrowser";
			this.Text = "七四式電子観測儀 ブラウザ";
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
		private System.Windows.Forms.TrackBar trackVolume;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparatorAnother1;
		private Browser.Control.ToolStripSpringTextBox ToolMenu_Url;
		private System.Windows.Forms.ToolStripDropDownButton ToolMenu_Mute_Track;
		private System.Windows.Forms.ToolStripButton ToolMenu_Mute;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
		private System.Windows.Forms.ContextMenuStrip ContextMenuTool;
		private System.Windows.Forms.ToolStripMenuItem ContextMenuTool_ShowToolMenu;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom_Fit;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_ClearCache;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Volume;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_LastScreenShot;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
		private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_LastScreenShot_OpenScreenShotFolder;

    }
}

