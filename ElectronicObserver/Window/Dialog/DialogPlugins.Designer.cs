namespace ElectronicObserver.Window.Dialog
{
	partial class DialogPlugins
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("浮动内容", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("服务类", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("对话框类", System.Windows.Forms.HorizontalAlignment.Left);
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.listViewPlugins = new ElectronicObserver.Window.Dialog.PluginListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.panelSettings = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.listViewPlugins);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.panelSettings);
			this.splitContainer1.Size = new System.Drawing.Size(684, 408);
			this.splitContainer1.SplitterDistance = 227;
			this.splitContainer1.TabIndex = 0;
			// 
			// listViewPlugins
			// 
			this.listViewPlugins.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
			this.listViewPlugins.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewPlugins.FullRowSelect = true;
			listViewGroup1.Header = "浮动内容";
			listViewGroup1.Name = "groupDockContent";
			listViewGroup2.Header = "服务类";
			listViewGroup2.Name = "groupService";
			listViewGroup3.Header = "对话框类";
			listViewGroup3.Name = "groupDialog";
			this.listViewPlugins.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
			this.listViewPlugins.HideSelection = false;
			this.listViewPlugins.Location = new System.Drawing.Point(0, 0);
			this.listViewPlugins.Name = "listViewPlugins";
			this.listViewPlugins.Size = new System.Drawing.Size(227, 408);
			this.listViewPlugins.TabIndex = 0;
			this.listViewPlugins.UseCompatibleStateImageBehavior = false;
			this.listViewPlugins.View = System.Windows.Forms.View.Details;
			this.listViewPlugins.SelectedIndexChanged += new System.EventHandler(this.listViewPlugins_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "插件名";
			this.columnHeader1.Width = 110;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "模块地址";
			this.columnHeader2.Width = 300;
			// 
			// panelSettings
			// 
			this.panelSettings.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelSettings.Location = new System.Drawing.Point(0, 0);
			this.panelSettings.Name = "panelSettings";
			this.panelSettings.Size = new System.Drawing.Size(453, 408);
			this.panelSettings.TabIndex = 0;
			// 
			// DialogPlugins
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(684, 408);
			this.Controls.Add(this.splitContainer1);
			this.MinimumSize = new System.Drawing.Size(520, 330);
			this.Name = "DialogPlugins";
			this.Text = "插件管理";
			this.Load += new System.EventHandler(this.DialogPlugins_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private PluginListView listViewPlugins;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.Panel panelSettings;
	}
}