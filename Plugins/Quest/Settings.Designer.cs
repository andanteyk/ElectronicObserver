﻿namespace Quest
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
			this.tabPage10 = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.FormQuest_ShowMonthly = new System.Windows.Forms.CheckBox();
			this.FormQuest_ShowWeekly = new System.Windows.Forms.CheckBox();
			this.FormQuest_ShowDaily = new System.Windows.Forms.CheckBox();
			this.FormQuest_ShowOnce = new System.Windows.Forms.CheckBox();
			this.FormQuest_ShowRunningOnly = new System.Windows.Forms.CheckBox();
			this.tabControl1.SuspendLayout();
			this.tabPage10.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add( this.tabPage10 );
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Multiline = true;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(400, 270);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage10
			// 
			this.tabPage10.Controls.Add( this.groupBox1 );
			this.tabPage10.Controls.Add( this.FormQuest_ShowRunningOnly );
			this.tabPage10.Location = new System.Drawing.Point( 4, 24 );
			this.tabPage10.Name = "tabPage10";
			this.tabPage10.Padding = new System.Windows.Forms.Padding( 3 );
			this.tabPage10.Size = new System.Drawing.Size( 442, 199 );
			this.tabPage10.TabIndex = 2;
			this.tabPage10.Text = "任务";
			this.tabPage10.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add( this.FormQuest_ShowMonthly );
			this.groupBox1.Controls.Add( this.FormQuest_ShowWeekly );
			this.groupBox1.Controls.Add( this.FormQuest_ShowDaily );
			this.groupBox1.Controls.Add( this.FormQuest_ShowOnce );
			this.groupBox1.Location = new System.Drawing.Point( 6, 31 );
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size( 156, 123 );
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "显示过滤";
			// 
			// FormQuest_ShowMonthly
			// 
			this.FormQuest_ShowMonthly.AutoSize = true;
			this.FormQuest_ShowMonthly.Location = new System.Drawing.Point( 6, 97 );
			this.FormQuest_ShowMonthly.Name = "FormQuest_ShowMonthly";
			this.FormQuest_ShowMonthly.Size = new System.Drawing.Size( 83, 27 );
			this.FormQuest_ShowMonthly.TabIndex = 4;
			this.FormQuest_ShowMonthly.Text = "月常";
			this.FormQuest_ShowMonthly.UseVisualStyleBackColor = true;
			// 
			// FormQuest_ShowWeekly
			// 
			this.FormQuest_ShowWeekly.AutoSize = true;
			this.FormQuest_ShowWeekly.Location = new System.Drawing.Point( 6, 72 );
			this.FormQuest_ShowWeekly.Name = "FormQuest_ShowWeekly";
			this.FormQuest_ShowWeekly.Size = new System.Drawing.Size( 90, 27 );
			this.FormQuest_ShowWeekly.TabIndex = 3;
			this.FormQuest_ShowWeekly.Text = "周常";
			this.FormQuest_ShowWeekly.UseVisualStyleBackColor = true;
			// 
			// FormQuest_ShowDaily
			// 
			this.FormQuest_ShowDaily.AutoSize = true;
			this.FormQuest_ShowDaily.Location = new System.Drawing.Point( 6, 47 );
			this.FormQuest_ShowDaily.Name = "FormQuest_ShowDaily";
			this.FormQuest_ShowDaily.Size = new System.Drawing.Size( 75, 27 );
			this.FormQuest_ShowDaily.TabIndex = 2;
			this.FormQuest_ShowDaily.Text = "日常";
			this.FormQuest_ShowDaily.UseVisualStyleBackColor = true;
			// 
			// FormQuest_ShowOnce
			// 
			this.FormQuest_ShowOnce.AutoSize = true;
			this.FormQuest_ShowOnce.Location = new System.Drawing.Point( 6, 22 );
			this.FormQuest_ShowOnce.Name = "FormQuest_ShowOnce";
			this.FormQuest_ShowOnce.Size = new System.Drawing.Size( 120, 27 );
			this.FormQuest_ShowOnce.TabIndex = 1;
			this.FormQuest_ShowOnce.Text = "一次性任务以及其他";
			this.FormQuest_ShowOnce.UseVisualStyleBackColor = true;
			// 
			// FormQuest_ShowRunningOnly
			// 
			this.FormQuest_ShowRunningOnly.AutoSize = true;
			this.FormQuest_ShowRunningOnly.Location = new System.Drawing.Point( 6, 6 );
			this.FormQuest_ShowRunningOnly.Name = "FormQuest_ShowRunningOnly";
			this.FormQuest_ShowRunningOnly.Size = new System.Drawing.Size( 139, 27 );
			this.FormQuest_ShowRunningOnly.TabIndex = 0;
			this.FormQuest_ShowRunningOnly.Text = "显示执行中的任务";
			this.FormQuest_ShowRunningOnly.UseVisualStyleBackColor = true;
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
			this.tabPage10.ResumeLayout( false );
			this.tabPage10.PerformLayout();
			this.groupBox1.ResumeLayout( false );
			this.groupBox1.PerformLayout();
			this.ResumeLayout( false );

		}

		#endregion
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.CheckBox FormQuest_ShowRunningOnly;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox FormQuest_ShowMonthly;
		private System.Windows.Forms.CheckBox FormQuest_ShowWeekly;
		private System.Windows.Forms.CheckBox FormQuest_ShowDaily;
		private System.Windows.Forms.CheckBox FormQuest_ShowOnce;
		private System.Windows.Forms.TabPage tabPage10;
	}
}
