namespace Headquarters
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
			this.tabPage16 = new System.Windows.Forms.TabPage();
			this.FormHeadquarters_BlinkAtMaximum = new System.Windows.Forms.CheckBox();
			this.tabControl1.SuspendLayout();
			this.tabPage16.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add( this.tabPage16 );
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Multiline = true;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(400, 270);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage16
			// 
			this.tabPage16.Controls.Add( this.FormHeadquarters_BlinkAtMaximum );
			this.tabPage16.Location = new System.Drawing.Point( 4, 24 );
			this.tabPage16.Name = "tabPage16";
			this.tabPage16.Padding = new System.Windows.Forms.Padding( 3 );
			this.tabPage16.Size = new System.Drawing.Size( 442, 199 );
			this.tabPage16.TabIndex = 6;
			this.tabPage16.Text = "司令部";
			this.tabPage16.UseVisualStyleBackColor = true;
			// 
			// FormHeadquarters_BlinkAtMaximum
			// 
			this.FormHeadquarters_BlinkAtMaximum.AutoSize = true;
			this.FormHeadquarters_BlinkAtMaximum.Location = new System.Drawing.Point( 6, 6 );
			this.FormHeadquarters_BlinkAtMaximum.Name = "FormHeadquarters_BlinkAtMaximum";
			this.FormHeadquarters_BlinkAtMaximum.Size = new System.Drawing.Size( 196, 27 );
			this.FormHeadquarters_BlinkAtMaximum.TabIndex = 0;
			this.FormHeadquarters_BlinkAtMaximum.Text = "舰船/装备已达最大值时闪烁";
			this.FormHeadquarters_BlinkAtMaximum.UseVisualStyleBackColor = true;
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
			this.tabPage16.ResumeLayout( false );
			this.tabPage16.PerformLayout();
			this.ResumeLayout( false );

		}

		#endregion
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage16;
		private System.Windows.Forms.CheckBox FormHeadquarters_BlinkAtMaximum;
	}
}
