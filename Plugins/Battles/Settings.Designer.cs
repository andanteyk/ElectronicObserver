namespace Battles
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
			this.tabPageFormBattle = new System.Windows.Forms.TabPage();
			this.FormBattle_IsShortDamage = new System.Windows.Forms.CheckBox();
			this.tabControl1.SuspendLayout();
			this.tabPageFormBattle.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add( this.tabPageFormBattle );
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
			this.tabPageFormBattle.Controls.Add( this.FormBattle_IsShortDamage );
			this.tabPageFormBattle.Location = new System.Drawing.Point( 4, 24 );
			this.tabPageFormBattle.Name = "tabPageFormBattle";
			this.tabPageFormBattle.Padding = new System.Windows.Forms.Padding( 3 );
			this.tabPageFormBattle.Size = new System.Drawing.Size( 442, 219 );
			this.tabPageFormBattle.TabIndex = 6;
			this.tabPageFormBattle.Text = "战斗";
			this.tabPageFormBattle.UseVisualStyleBackColor = true;
			// 
			// FormBattle_IsShortDamage
			// 
			this.FormBattle_IsShortDamage.AutoSize = true;
			this.FormBattle_IsShortDamage.Location = new System.Drawing.Point( 6, 6 );
			this.FormBattle_IsShortDamage.Name = "FormBattle_IsShortDamage";
			this.FormBattle_IsShortDamage.Size = new System.Drawing.Size( 129, 27 );
			this.FormBattle_IsShortDamage.TabIndex = 0;
			this.FormBattle_IsShortDamage.Text = "简洁化伤害显示";
			this.ToolTipInfo.SetToolTip( this.FormBattle_IsShortDamage, "不显示具体伤害数字，可节省横向空间。" );
			this.FormBattle_IsShortDamage.UseVisualStyleBackColor = true;
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
			this.tabPageFormBattle.ResumeLayout( false );
			this.ResumeLayout( false );

		}

		#endregion
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.ToolTip ToolTipInfo;
		private System.Windows.Forms.TabPage tabPageFormBattle;
		private System.Windows.Forms.CheckBox FormBattle_IsShortDamage;
	}
}
