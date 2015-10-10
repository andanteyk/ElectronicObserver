namespace DBSender
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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage15 = new System.Windows.Forms.TabPage();
			this.Database_LinkKCDB = new System.Windows.Forms.LinkLabel();
			this.label22 = new System.Windows.Forms.Label();
			this.Database_SendKancolleOAuth = new System.Windows.Forms.TextBox();
			this.labelKdb = new System.Windows.Forms.Label();
			this.Database_SendDataToKancolleDB = new System.Windows.Forms.CheckBox();
			this.Database_SendWithProxy = new System.Windows.Forms.CheckBox();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.label1 = new System.Windows.Forms.Label();
			this.Poi_EnableSending = new System.Windows.Forms.CheckBox();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage15.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage15);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Multiline = true;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(400, 270);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.Poi_EnableSending);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Controls.Add(this.linkLabel1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(392, 244);
			this.tabPage1.TabIndex = 9;
			this.tabPage1.Text = "poi-statistics";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage15
			// 
			this.tabPage15.Controls.Add(this.Database_LinkKCDB);
			this.tabPage15.Controls.Add(this.label22);
			this.tabPage15.Controls.Add(this.Database_SendKancolleOAuth);
			this.tabPage15.Controls.Add(this.labelKdb);
			this.tabPage15.Controls.Add(this.Database_SendDataToKancolleDB);
			this.tabPage15.Controls.Add(this.Database_SendWithProxy);
			this.tabPage15.Location = new System.Drawing.Point(4, 22);
			this.tabPage15.Name = "tabPage15";
			this.tabPage15.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage15.Size = new System.Drawing.Size(392, 244);
			this.tabPage15.TabIndex = 8;
			this.tabPage15.Text = "数据";
			this.tabPage15.UseVisualStyleBackColor = true;
			// 
			// Database_LinkKCDB
			// 
			this.Database_LinkKCDB.AutoSize = true;
			this.Database_LinkKCDB.Location = new System.Drawing.Point(8, 25);
			this.Database_LinkKCDB.Name = "Database_LinkKCDB";
			this.Database_LinkKCDB.Size = new System.Drawing.Size(116, 13);
			this.Database_LinkKCDB.TabIndex = 17;
			this.Database_LinkKCDB.TabStop = true;
			this.Database_LinkKCDB.Text = "http://kancolle-db.net/";
			this.Database_LinkKCDB.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Database_LinkKCDB_LinkClicked);
			// 
			// label22
			// 
			this.label22.AutoSize = true;
			this.label22.Location = new System.Drawing.Point(8, 9);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(427, 13);
			this.label22.TabIndex = 16;
			this.label22.Text = "「艦これ統計データベース」是一家数据统计网站，详情请点击以下连接查询。";
			// 
			// Database_SendKancolleOAuth
			// 
			this.Database_SendKancolleOAuth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Database_SendKancolleOAuth.Location = new System.Drawing.Point(89, 75);
			this.Database_SendKancolleOAuth.Name = "Database_SendKancolleOAuth";
			this.Database_SendKancolleOAuth.Size = new System.Drawing.Size(359, 20);
			this.Database_SendKancolleOAuth.TabIndex = 14;
			// 
			// labelKdb
			// 
			this.labelKdb.AutoSize = true;
			this.labelKdb.Location = new System.Drawing.Point(8, 78);
			this.labelKdb.Name = "labelKdb";
			this.labelKdb.Size = new System.Drawing.Size(73, 13);
			this.labelKdb.TabIndex = 13;
			this.labelKdb.Text = "OAuth认证：";
			// 
			// Database_SendDataToKancolleDB
			// 
			this.Database_SendDataToKancolleDB.AutoSize = true;
			this.Database_SendDataToKancolleDB.Location = new System.Drawing.Point(11, 53);
			this.Database_SendDataToKancolleDB.Name = "Database_SendDataToKancolleDB";
			this.Database_SendDataToKancolleDB.Size = new System.Drawing.Size(242, 17);
			this.Database_SendDataToKancolleDB.TabIndex = 12;
			this.Database_SendDataToKancolleDB.Text = "发送数据到「艦これ統計データベース」";
			this.Database_SendDataToKancolleDB.UseVisualStyleBackColor = true;
			// 
			// Database_SendWithProxy
			// 
			this.Database_SendWithProxy.AutoSize = true;
			this.Database_SendWithProxy.Location = new System.Drawing.Point(11, 103);
			this.Database_SendWithProxy.Name = "Database_SendWithProxy";
			this.Database_SendWithProxy.Size = new System.Drawing.Size(158, 17);
			this.Database_SendWithProxy.TabIndex = 12;
			this.Database_SendWithProxy.Text = "通过设置的代理发送数据";
			this.Database_SendWithProxy.UseVisualStyleBackColor = true;
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Location = new System.Drawing.Point(8, 25);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(137, 13);
			this.linkLabel1.TabIndex = 18;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "http://db.kcwiki.moe/drop/";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(362, 13);
			this.label1.TabIndex = 19;
			this.label1.Text = "poi-statistics是一个开源自由、健壮成长的舰队Collection统计数据库。";
			// 
			// Poi_EnableSending
			// 
			this.Poi_EnableSending.AutoSize = true;
			this.Poi_EnableSending.Location = new System.Drawing.Point(11, 53);
			this.Poi_EnableSending.Name = "Poi_EnableSending";
			this.Poi_EnableSending.Size = new System.Drawing.Size(50, 17);
			this.Poi_EnableSending.TabIndex = 20;
			this.Poi_EnableSending.Text = "启用";
			this.Poi_EnableSending.UseVisualStyleBackColor = true;
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
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage15.ResumeLayout(false);
			this.tabPage15.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage15;
		private System.Windows.Forms.LinkLabel Database_LinkKCDB;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.TextBox Database_SendKancolleOAuth;
		private System.Windows.Forms.Label labelKdb;
		private System.Windows.Forms.CheckBox Database_SendDataToKancolleDB;
		private System.Windows.Forms.CheckBox Database_SendWithProxy;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox Poi_EnableSending;
	}
}
