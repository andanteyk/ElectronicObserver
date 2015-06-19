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
			this.components = new System.ComponentModel.Container();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage15 = new System.Windows.Forms.TabPage();
			this.Database_LinkKCDB = new System.Windows.Forms.LinkLabel();
			this.label22 = new System.Windows.Forms.Label();
			this.Database_SendKancolleOAuth = new System.Windows.Forms.TextBox();
			this.labelKdb = new System.Windows.Forms.Label();
			this.Database_SendDataToKancolleDB = new System.Windows.Forms.CheckBox();
			this.tabControl1.SuspendLayout();
			this.tabPage15.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add( this.tabPage15 );
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Multiline = true;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(400, 270);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage15
			// 
			this.tabPage15.Controls.Add( this.Database_LinkKCDB );
			this.tabPage15.Controls.Add( this.label22 );
			this.tabPage15.Controls.Add( this.Database_SendKancolleOAuth );
			this.tabPage15.Controls.Add( this.labelKdb );
			this.tabPage15.Controls.Add( this.Database_SendDataToKancolleDB );
			this.tabPage15.Location = new System.Drawing.Point( 4, 44 );
			this.tabPage15.Name = "tabPage15";
			this.tabPage15.Padding = new System.Windows.Forms.Padding( 3 );
			this.tabPage15.Size = new System.Drawing.Size( 456, 233 );
			this.tabPage15.TabIndex = 8;
			this.tabPage15.Text = "数据";
			this.tabPage15.UseVisualStyleBackColor = true;
			// 
			// Database_LinkKCDB
			// 
			this.Database_LinkKCDB.AutoSize = true;
			this.Database_LinkKCDB.Location = new System.Drawing.Point( 8, 25 );
			this.Database_LinkKCDB.Name = "Database_LinkKCDB";
			this.Database_LinkKCDB.Size = new System.Drawing.Size( 141, 15 );
			this.Database_LinkKCDB.TabIndex = 17;
			this.Database_LinkKCDB.TabStop = true;
			this.Database_LinkKCDB.Text = "http://kancolle-db.net/";
			this.Database_LinkKCDB.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler( this.Database_LinkKCDB_LinkClicked );
			// 
			// label22
			// 
			this.label22.AutoSize = true;
			this.label22.Location = new System.Drawing.Point( 8, 9 );
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size( 414, 30 );
			this.label22.TabIndex = 16;
			this.label22.Text = "「艦これ統計データベース」是一家数据统计网站，详情请点击以下连接查询。";
			// 
			// Database_SendKancolleOAuth
			// 
			this.Database_SendKancolleOAuth.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
			| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.Database_SendKancolleOAuth.Location = new System.Drawing.Point( 89, 75 );
			this.Database_SendKancolleOAuth.Name = "Database_SendKancolleOAuth";
			this.Database_SendKancolleOAuth.Size = new System.Drawing.Size( 359, 23 );
			this.Database_SendKancolleOAuth.TabIndex = 14;
			// 
			// labelKdb
			// 
			this.labelKdb.AutoSize = true;
			this.labelKdb.Location = new System.Drawing.Point( 8, 78 );
			this.labelKdb.Name = "labelKdb";
			this.labelKdb.Size = new System.Drawing.Size( 75, 15 );
			this.labelKdb.TabIndex = 13;
			this.labelKdb.Text = "OAuth认证：";
			// 
			// Database_SendDataToKancolleDB
			// 
			this.Database_SendDataToKancolleDB.AutoSize = true;
			this.Database_SendDataToKancolleDB.Location = new System.Drawing.Point( 6, 53 );
			this.Database_SendDataToKancolleDB.Name = "Database_SendDataToKancolleDB";
			this.Database_SendDataToKancolleDB.Size = new System.Drawing.Size( 203, 27 );
			this.Database_SendDataToKancolleDB.TabIndex = 12;
			this.Database_SendDataToKancolleDB.Text = "发送数据到「艦これ統計データベース」";
			this.Database_SendDataToKancolleDB.UseVisualStyleBackColor = true;
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
			this.tabPage15.ResumeLayout( false );
			this.tabPage15.PerformLayout();
			this.ResumeLayout( false );

		}

		#endregion
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage15;
		private System.Windows.Forms.LinkLabel Database_LinkKCDB;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.TextBox Database_SendKancolleOAuth;
		private System.Windows.Forms.Label labelKdb;
		private System.Windows.Forms.CheckBox Database_SendDataToKancolleDB;
	}
}
