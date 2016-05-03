namespace Notifier
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
			components = new System.ComponentModel.Container();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage11 = new System.Windows.Forms.TabPage();
			this.label10 = new System.Windows.Forms.Label();
			this.Notification_Damage = new System.Windows.Forms.Button();
            this.Notification_AnchorageRepair = new System.Windows.Forms.Button();
            this.Notification_Condition = new System.Windows.Forms.Button();
			this.Notification_Repair = new System.Windows.Forms.Button();
			this.Notification_Construction = new System.Windows.Forms.Button();
			this.Notification_Expedition = new System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.tabPage11.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Controls.Add( this.tabPage11 );
			this.tabControl1.Location = new System.Drawing.Point( 0, 0 );
			this.tabControl1.Multiline = true;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size( 524, 291 );
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage11
			// 
			this.tabPage11.Controls.Add( this.label10 );
			this.tabPage11.Controls.Add( this.Notification_Damage );
			this.tabPage11.Controls.Add( this.Notification_Condition );
			this.tabPage11.Controls.Add( this.Notification_Repair );
			this.tabPage11.Controls.Add( this.Notification_Construction );
			this.tabPage11.Controls.Add( this.Notification_Expedition );
            this.tabPage11.Controls.Add( this.Notification_AnchorageRepair );
			this.tabPage11.Location = new System.Drawing.Point( 4, 24 );
			this.tabPage11.Name = "tabPage11";
			this.tabPage11.Padding = new System.Windows.Forms.Padding( 3 );
			this.tabPage11.Size = new System.Drawing.Size( 456, 253 );
			this.tabPage11.TabIndex = 7;
			this.tabPage11.Text = "通知";
			this.tabPage11.UseVisualStyleBackColor = true;
			// 
			// label10
			// 
			this.label10.Anchor = ( (System.Windows.Forms.AnchorStyles)( System.Windows.Forms.AnchorStyles.Bottom
			| System.Windows.Forms.AnchorStyles.Left ) );
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point( 3, 235 );
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size( 238, 15 );
			this.label10.TabIndex = 5;
			this.label10.Text = "＊每个设置对话框点OK时生效";
			// 
			// Notification_AnchorageRepair
			// 
			this.Notification_AnchorageRepair.Location = new System.Drawing.Point(8, 151);
			this.Notification_AnchorageRepair.Name = "Notification_AnchorageRepair";
			this.Notification_AnchorageRepair.Size = new System.Drawing.Size(150, 23);
			this.Notification_AnchorageRepair.TabIndex = 6;
			this.Notification_AnchorageRepair.Text = "泊地修理通知の設定...";
			this.Notification_AnchorageRepair.UseVisualStyleBackColor = true;
			this.Notification_AnchorageRepair.Click += new System.EventHandler(this.Notification_AnchorageRepair_Click);
			// 
			// Notification_Damage
			// 
			this.Notification_Damage.Location = new System.Drawing.Point( 8, 122 );
			this.Notification_Damage.Name = "Notification_Damage";
			this.Notification_Damage.Size = new System.Drawing.Size( 150, 23 );
			this.Notification_Damage.TabIndex = 4;
			this.Notification_Damage.Text = "大破进击通知的设置...";
			this.Notification_Damage.UseVisualStyleBackColor = true;
			this.Notification_Damage.Click += new System.EventHandler( this.Notification_Damage_Click );
			// 
			// Notification_Condition
			// 
			this.Notification_Condition.Location = new System.Drawing.Point( 8, 93 );
			this.Notification_Condition.Name = "Notification_Condition";
			this.Notification_Condition.Size = new System.Drawing.Size( 150, 23 );
			this.Notification_Condition.TabIndex = 3;
			this.Notification_Condition.Text = "疲劳恢复通知的设置...";
			this.Notification_Condition.UseVisualStyleBackColor = true;
			this.Notification_Condition.Click += new System.EventHandler( this.Notification_Condition_Click );
			// 
			// Notification_Repair
			// 
			this.Notification_Repair.Location = new System.Drawing.Point( 8, 64 );
			this.Notification_Repair.Name = "Notification_Repair";
			this.Notification_Repair.Size = new System.Drawing.Size( 150, 23 );
			this.Notification_Repair.TabIndex = 2;
			this.Notification_Repair.Text = "入渠结束通知的设置...";
			this.Notification_Repair.UseVisualStyleBackColor = true;
			this.Notification_Repair.Click += new System.EventHandler( this.Notification_Repair_Click );
			// 
			// Notification_Construction
			// 
			this.Notification_Construction.Location = new System.Drawing.Point( 8, 35 );
			this.Notification_Construction.Name = "Notification_Construction";
			this.Notification_Construction.Size = new System.Drawing.Size( 150, 23 );
			this.Notification_Construction.TabIndex = 1;
			this.Notification_Construction.Text = "建造完毕通知的设置...";
			this.Notification_Construction.UseVisualStyleBackColor = true;
			this.Notification_Construction.Click += new System.EventHandler( this.Notification_Construction_Click );
			// 
			// Notification_Expedition
			// 
			this.Notification_Expedition.Location = new System.Drawing.Point( 8, 6 );
			this.Notification_Expedition.Name = "Notification_Expedition";
			this.Notification_Expedition.Size = new System.Drawing.Size( 150, 23 );
			this.Notification_Expedition.TabIndex = 0;
			this.Notification_Expedition.Text = "远征结束通知的设置...";
			this.Notification_Expedition.UseVisualStyleBackColor = true;
			this.Notification_Expedition.Click += new System.EventHandler( this.Notification_Expedition_Click );
			//
			// this
			//
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Size = new System.Drawing.Size( 400, 270 );
			this.Controls.Add( tabControl1 );
			this.tabControl1.ResumeLayout( false );
			this.tabPage11.ResumeLayout( false );
			this.tabPage11.PerformLayout();
			this.ResumeLayout( false );
		}

		#endregion
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage11;
		private System.Windows.Forms.Button Notification_Expedition;
		private System.Windows.Forms.Button Notification_Construction;
		private System.Windows.Forms.Button Notification_Repair;
		private System.Windows.Forms.Button Notification_Damage;
        private System.Windows.Forms.Button Notification_AnchorageRepair;
        private System.Windows.Forms.Button Notification_Condition;
		private System.Windows.Forms.Label label10;
	}
}
