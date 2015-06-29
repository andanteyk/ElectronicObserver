namespace Overview
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
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.FormInformation_ShowFailedDevelopment = new System.Windows.Forms.CheckBox();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add( this.tabPage1 );
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
			// tabPageFormBattle
			// 
			this.tabPage1.Controls.Add( this.FormInformation_ShowFailedDevelopment );
			this.tabPage1.Location = new System.Drawing.Point( 4, 24 );
			this.tabPage1.Name = "tabPageFormBattle";
			this.tabPage1.Padding = new System.Windows.Forms.Padding( 3 );
			this.tabPage1.Size = new System.Drawing.Size( 442, 219 );
			this.tabPage1.TabIndex = 6;
			this.tabPage1.Text = "情报";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// FormInformation_ShowFailedDevelopment
			// 
			this.FormInformation_ShowFailedDevelopment.AutoSize = true;
			this.FormInformation_ShowFailedDevelopment.Location = new System.Drawing.Point( 6, 6 );
			this.FormInformation_ShowFailedDevelopment.Name = "FormInformation_ShowFailedDevelopment";
			this.FormInformation_ShowFailedDevelopment.Size = new System.Drawing.Size( 129, 27 );
			this.FormInformation_ShowFailedDevelopment.TabIndex = 0;
			this.FormInformation_ShowFailedDevelopment.Text = "显示开发失败的道具名";
			this.ToolTipInfo.SetToolTip( this.FormInformation_ShowFailedDevelopment, "在开发失败时显示失败的道具名称。" );
			this.FormInformation_ShowFailedDevelopment.UseVisualStyleBackColor = true;
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
			this.tabPage1.ResumeLayout( false );
			this.ResumeLayout( false );

		}

		#endregion
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.ToolTip ToolTipInfo;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.CheckBox FormInformation_ShowFailedDevelopment;
	}
}
