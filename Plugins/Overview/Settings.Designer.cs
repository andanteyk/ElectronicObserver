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
			this.tabPage9 = new System.Windows.Forms.TabPage();
			this.FormArsenal_ShowShipName = new System.Windows.Forms.CheckBox();
			this.tabControl1.SuspendLayout();
			this.tabPage9.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage9);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Multiline = true;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(400, 270);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage9
			// 
			this.tabPage9.Controls.Add( this.FormArsenal_ShowShipName );
			this.tabPage9.Location = new System.Drawing.Point( 4, 24 );
			this.tabPage9.Name = "tabPage9";
			this.tabPage9.Padding = new System.Windows.Forms.Padding( 3 );
			this.tabPage9.Size = new System.Drawing.Size( 442, 199 );
			this.tabPage9.TabIndex = 1;
			this.tabPage9.Text = "工厂";
			this.tabPage9.UseVisualStyleBackColor = true;
			// 
			// FormArsenal_ShowShipName
			// 
			this.FormArsenal_ShowShipName.AutoSize = true;
			this.FormArsenal_ShowShipName.Location = new System.Drawing.Point( 6, 6 );
			this.FormArsenal_ShowShipName.Name = "FormArsenal_ShowShipName";
			this.FormArsenal_ShowShipName.Size = new System.Drawing.Size( 115, 27 );
			this.FormArsenal_ShowShipName.TabIndex = 1;
			this.FormArsenal_ShowShipName.Text = "显示舰名";
			this.FormArsenal_ShowShipName.UseVisualStyleBackColor = true;
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
			this.tabPage9.ResumeLayout( false );
			this.tabPage9.PerformLayout();
			this.ResumeLayout( false );

		}

		#endregion
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage9;
		private System.Windows.Forms.CheckBox FormArsenal_ShowShipName;
	}
}
