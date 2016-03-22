namespace APILoader
{
	partial class Settings
	{
		/// <summary> 
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region 组件设计器生成的代码

		/// <summary> 
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.ToolTipInfo = new System.Windows.Forms.ToolTip( this.components );
			this.tabPage5 = new System.Windows.Forms.TabPage();
			this.Debug_AlertOnError = new System.Windows.Forms.CheckBox();
			this.Debug_SealingPanel = new System.Windows.Forms.Panel();
			this.Debug_APIListPath = new System.Windows.Forms.TextBox();
			this.Debug_LoadAPIListOnLoad = new System.Windows.Forms.CheckBox();
			this.Debug_APIListPathSearch = new System.Windows.Forms.Button();
			this.Debug_EnableDebugMenu = new System.Windows.Forms.CheckBox();
			this.APIListBrowser = new System.Windows.Forms.OpenFileDialog();
			this.tabControl1.SuspendLayout();
			this.tabPage5.SuspendLayout();
			this.Debug_SealingPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add( this.tabPage5 );
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Multiline = true;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(400, 270);
			this.tabControl1.TabIndex = 0;
			// 
			// ToolTipInfo
			// 
			this.ToolTipInfo.AutoPopDelay = 60000;
			this.ToolTipInfo.InitialDelay = 500;
			this.ToolTipInfo.ReshowDelay = 100;
			this.ToolTipInfo.ShowAlways = true;
			// 
			// tabPage5
			// 
			this.tabPage5.Controls.Add( this.Debug_AlertOnError );
			this.tabPage5.Controls.Add( this.Debug_SealingPanel );
			this.tabPage5.Controls.Add( this.Debug_EnableDebugMenu );
			this.tabPage5.Location = new System.Drawing.Point( 4, 24 );
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.Padding = new System.Windows.Forms.Padding( 3 );
			this.tabPage5.Size = new System.Drawing.Size( 456, 253 );
			this.tabPage5.TabIndex = 4;
			this.tabPage5.Text = "调试";
			this.tabPage5.UseVisualStyleBackColor = true;
			// 
			// Debug_AlertOnError
			// 
			this.Debug_AlertOnError.AutoSize = true;
			this.Debug_AlertOnError.Location = new System.Drawing.Point(8, 31);
			this.Debug_AlertOnError.Name = "Debug_AlertOnError";
			this.Debug_AlertOnError.Size = new System.Drawing.Size(104, 19);
			this.Debug_AlertOnError.TabIndex = 2;
			this.Debug_AlertOnError.Text = "エラー音を鳴らす";
			this.Debug_AlertOnError.UseVisualStyleBackColor = true;
			// 
			// Debug_SealingPanel
			// 
			this.Debug_SealingPanel.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
			| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.Debug_SealingPanel.Controls.Add( this.Debug_APIListPath );
			this.Debug_SealingPanel.Controls.Add( this.Debug_LoadAPIListOnLoad );
			this.Debug_SealingPanel.Controls.Add( this.Debug_APIListPathSearch );
			this.Debug_SealingPanel.Location = new System.Drawing.Point( 0, 31 );
			this.Debug_SealingPanel.Name = "Debug_SealingPanel";
			this.Debug_SealingPanel.Size = new System.Drawing.Size( 456, 222 );
			this.Debug_SealingPanel.TabIndex = 7;
			// 
			// Debug_APIListPath
			// 
			this.Debug_APIListPath.AllowDrop = true;
			this.Debug_APIListPath.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
			| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.Debug_APIListPath.Location = new System.Drawing.Point( 8, 28 );
			this.Debug_APIListPath.Name = "Debug_APIListPath";
			this.Debug_APIListPath.Size = new System.Drawing.Size( 402, 23 );
			this.Debug_APIListPath.TabIndex = 5;
			// 
			// Debug_LoadAPIListOnLoad
			// 
			this.Debug_LoadAPIListOnLoad.AutoSize = true;
			this.Debug_LoadAPIListOnLoad.Location = new System.Drawing.Point( 8, 3 );
			this.Debug_LoadAPIListOnLoad.Name = "Debug_LoadAPIListOnLoad";
			this.Debug_LoadAPIListOnLoad.Size = new System.Drawing.Size( 164, 27 );
			this.Debug_LoadAPIListOnLoad.TabIndex = 1;
			this.Debug_LoadAPIListOnLoad.Text = "启动时加载API列表";
			this.ToolTipInfo.SetToolTip( this.Debug_LoadAPIListOnLoad, "启动时，下列文本框中的API列表将自动加载。\r\nAPI列表的格式和用法请参照在线帮助。" );
			this.Debug_LoadAPIListOnLoad.UseVisualStyleBackColor = true;
			// 
			// Debug_APIListPathSearch
			// 
			this.Debug_APIListPathSearch.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.Debug_APIListPathSearch.Location = new System.Drawing.Point( 416, 28 );
			this.Debug_APIListPathSearch.Name = "Debug_APIListPathSearch";
			this.Debug_APIListPathSearch.Size = new System.Drawing.Size( 32, 23 );
			this.Debug_APIListPathSearch.TabIndex = 6;
			this.Debug_APIListPathSearch.Text = "...";
			this.Debug_APIListPathSearch.UseVisualStyleBackColor = true;
			this.Debug_APIListPathSearch.Click += new System.EventHandler( this.Debug_APIListPathSearch_Click );
			// 
			// Debug_EnableDebugMenu
			// 
			this.Debug_EnableDebugMenu.AutoSize = true;
			this.Debug_EnableDebugMenu.Location = new System.Drawing.Point( 8, 6 );
			this.Debug_EnableDebugMenu.Name = "Debug_EnableDebugMenu";
			this.Debug_EnableDebugMenu.Size = new System.Drawing.Size( 175, 27 );
			this.Debug_EnableDebugMenu.TabIndex = 0;
			this.Debug_EnableDebugMenu.Text = "开启调试菜单";
			this.ToolTipInfo.SetToolTip( this.Debug_EnableDebugMenu, "开启主界面的 [调试] 菜单。\r\n不推荐普通用户开启调试功能。\r\n无法保证操作安全，行为请＊自己负责＊。" );
			this.Debug_EnableDebugMenu.UseVisualStyleBackColor = true;
			this.Debug_EnableDebugMenu.CheckedChanged += new System.EventHandler( this.Debug_EnableDebugMenu_CheckedChanged );
			// 
			// APIListBrowser
			// 
			this.APIListBrowser.Filter = "Text File|*.txt|File|*";
			this.APIListBrowser.Title = "打开API列表";
			// 
			// Settings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabControl1);
			this.Name = "Settings";
			this.Size = new System.Drawing.Size(400, 270);
			this.Load += new System.EventHandler(this.Settings_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage5.ResumeLayout( false );
			this.tabPage5.PerformLayout();
			this.Debug_SealingPanel.ResumeLayout( false );
			this.Debug_SealingPanel.PerformLayout();
			this.ResumeLayout( false );

		}

		#endregion
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.ToolTip ToolTipInfo;
		private System.Windows.Forms.TabPage tabPage5;
		private System.Windows.Forms.CheckBox Debug_EnableDebugMenu;
		private System.Windows.Forms.Button Debug_APIListPathSearch;
		private System.Windows.Forms.TextBox Debug_APIListPath;
		private System.Windows.Forms.CheckBox Debug_LoadAPIListOnLoad;
		private System.Windows.Forms.OpenFileDialog APIListBrowser;
		private System.Windows.Forms.Panel Debug_SealingPanel;
		private System.Windows.Forms.CheckBox Debug_AlertOnError;
	}
}
