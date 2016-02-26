namespace LocalCacher
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
			this.tabPageCache = new System.Windows.Forms.TabPage();
			this.labelCache = new System.Windows.Forms.Label();
			this.textCacheFolder = new System.Windows.Forms.TextBox();
			this.buttonCacheFolderBrowse = new System.Windows.Forms.Button();
			this.checkCache = new System.Windows.Forms.CheckBox();
			this.FolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
			this.ToolTipInfo = new System.Windows.Forms.ToolTip(this.components);
			this.tabControl1.SuspendLayout();
			this.tabPageCache.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPageCache);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Multiline = true;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(400, 270);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPageCache
			// 
			this.tabPageCache.Controls.Add( this.buttonCacheFolderBrowse );
			this.tabPageCache.Controls.Add( this.textCacheFolder );
			this.tabPageCache.Controls.Add( this.labelCache );
			this.tabPageCache.Controls.Add( this.checkCache );
			this.tabPageCache.Location = new System.Drawing.Point( 4, 44 );
			this.tabPageCache.Name = "tabPageCache";
			this.tabPageCache.Padding = new System.Windows.Forms.Padding( 3 );
			this.tabPageCache.Size = new System.Drawing.Size( 392, 211 );
			this.tabPageCache.TabIndex = 8;
			this.tabPageCache.Text = "缓存";
			this.tabPageCache.UseVisualStyleBackColor = true;
			// 
			// labelCache
			// 
			this.labelCache.AutoSize = true;
			this.labelCache.Location = new System.Drawing.Point( 8, 9 );
			this.labelCache.Name = "labelCache";
			this.labelCache.Size = new System.Drawing.Size( 103, 15 );
			this.labelCache.TabIndex = 0;
			this.labelCache.Text = "缓存文件夹路径：";
			// 
			// textCacheFolder
			// 
			this.textCacheFolder.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
			| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.textCacheFolder.Location = new System.Drawing.Point( 118, 6 );
			this.textCacheFolder.Name = "textCacheFolder";
			this.textCacheFolder.Size = new System.Drawing.Size( 199, 23 );
			this.textCacheFolder.TabIndex = 1;
			this.textCacheFolder.TextChanged += new System.EventHandler( this.textCacheFolder_TextChanged );
			// 
			// buttonCacheFolderBrowse
			// 
			this.buttonCacheFolderBrowse.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.buttonCacheFolderBrowse.Location = new System.Drawing.Point( 323, 6 );
			this.buttonCacheFolderBrowse.Name = "buttonCacheFolderBrowse";
			this.buttonCacheFolderBrowse.Size = new System.Drawing.Size( 61, 23 );
			this.buttonCacheFolderBrowse.TabIndex = 2;
			this.buttonCacheFolderBrowse.Text = "浏览";
			this.buttonCacheFolderBrowse.UseVisualStyleBackColor = true;
			this.buttonCacheFolderBrowse.Click += new System.EventHandler( this.buttonCacheFolderBrowse_Click );
			// 
			// checkCache
			// 
			this.checkCache.AutoSize = true;
			this.checkCache.Location = new System.Drawing.Point( 8, 38 );
			this.checkCache.Name = "checkCache";
			this.checkCache.Size = new System.Drawing.Size( 139, 19 );
			this.checkCache.TabIndex = 3;
			this.checkCache.Text = "启用缓存";
			this.checkCache.UseVisualStyleBackColor = true;
			// 
			// FolderBrowser
			// 
			this.FolderBrowser.Description = "参考文件夹";
			// 
			// ToolTipInfo
			// 
			this.ToolTipInfo.AutoPopDelay = 60000;
			this.ToolTipInfo.InitialDelay = 500;
			this.ToolTipInfo.ReshowDelay = 100;
			this.ToolTipInfo.ShowAlways = true;
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
			this.tabPageCache.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPageCache;
		private System.Windows.Forms.Label labelCache;
		private System.Windows.Forms.TextBox textCacheFolder;
		private System.Windows.Forms.Button buttonCacheFolderBrowse;
		private System.Windows.Forms.CheckBox checkCache;
		private System.Windows.Forms.FolderBrowserDialog FolderBrowser;
		private System.Windows.Forms.ToolTip ToolTipInfo;
	}
}
