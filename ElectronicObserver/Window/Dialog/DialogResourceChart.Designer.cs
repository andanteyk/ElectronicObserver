namespace ElectronicObserver.Window.Dialog {
	partial class DialogResourceChart {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing ) {
			if ( disposing && ( components != null ) ) {
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.ResourceChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.Menu_Graph = new System.Windows.Forms.ToolStripMenuItem();
			this.Menu_Graph_Resource = new System.Windows.Forms.ToolStripMenuItem();
			this.Menu_Graph_Material = new System.Windows.Forms.ToolStripMenuItem();
			this.Menu_Graph_Experience = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ResourceChart)).BeginInit();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Graph});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(774, 38);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// ResourceChart
			// 
			this.ResourceChart.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ResourceChart.Location = new System.Drawing.Point(0, 38);
			this.ResourceChart.Name = "ResourceChart";
			this.ResourceChart.Size = new System.Drawing.Size(774, 491);
			this.ResourceChart.TabIndex = 1;
			this.ResourceChart.Text = "資源チャート";
			this.ResourceChart.GetToolTipText += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs>(this.ResourceChart_GetToolTipText);
			// 
			// Menu_Graph
			// 
			this.Menu_Graph.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Graph_Resource,
            this.Menu_Graph_Material,
            this.Menu_Graph_Experience});
			this.Menu_Graph.Name = "Menu_Graph";
			this.Menu_Graph.Size = new System.Drawing.Size(183, 34);
			this.Menu_Graph.Text = "グラフの選択(&G)";
			// 
			// Menu_Graph_Resource
			// 
			this.Menu_Graph_Resource.Name = "Menu_Graph_Resource";
			this.Menu_Graph_Resource.Size = new System.Drawing.Size(244, 34);
			this.Menu_Graph_Resource.Text = "資源(&R)";
			// 
			// Menu_Graph_Material
			// 
			this.Menu_Graph_Material.Name = "Menu_Graph_Material";
			this.Menu_Graph_Material.Size = new System.Drawing.Size(244, 34);
			this.Menu_Graph_Material.Text = "資材(&M)";
			// 
			// Menu_Graph_Experience
			// 
			this.Menu_Graph_Experience.Name = "Menu_Graph_Experience";
			this.Menu_Graph_Experience.Size = new System.Drawing.Size(244, 34);
			this.Menu_Graph_Experience.Text = "経験値(&E)";
			// 
			// DialogResourceChart
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(774, 529);
			this.Controls.Add(this.ResourceChart);
			this.Controls.Add(this.menuStrip1);
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "DialogResourceChart";
			this.Text = "資源チャート";
			this.Load += new System.EventHandler(this.DialogResourceChart_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ResourceChart)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem Menu_Graph;
		private System.Windows.Forms.ToolStripMenuItem Menu_Graph_Resource;
		private System.Windows.Forms.ToolStripMenuItem Menu_Graph_Material;
		private System.Windows.Forms.ToolStripMenuItem Menu_Graph_Experience;
		private System.Windows.Forms.DataVisualization.Charting.Chart ResourceChart;
	}
}