namespace ElectronicObserver.Window {
	partial class FormCompass {
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
			this.BasePanel = new System.Windows.Forms.FlowLayoutPanel();
			this.TextMapArea = new System.Windows.Forms.Label();
			this.TextDestination = new System.Windows.Forms.Label();
			this.TextEventKind = new System.Windows.Forms.Label();
			this.TextEventDetail = new System.Windows.Forms.Label();
			this.BasePanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// BasePanel
			// 
			this.BasePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.BasePanel.Controls.Add(this.TextMapArea);
			this.BasePanel.Controls.Add(this.TextDestination);
			this.BasePanel.Controls.Add(this.TextEventKind);
			this.BasePanel.Controls.Add(this.TextEventDetail);
			this.BasePanel.Location = new System.Drawing.Point(0, 0);
			this.BasePanel.Name = "BasePanel";
			this.BasePanel.Size = new System.Drawing.Size(300, 200);
			this.BasePanel.TabIndex = 0;
			// 
			// TextMapArea
			// 
			this.TextMapArea.AutoSize = true;
			this.TextMapArea.Location = new System.Drawing.Point(3, 0);
			this.TextMapArea.Name = "TextMapArea";
			this.TextMapArea.Size = new System.Drawing.Size(41, 15);
			this.TextMapArea.TabIndex = 0;
			this.TextMapArea.Text = "(海域)";
			// 
			// TextDestination
			// 
			this.TextDestination.AutoSize = true;
			this.TextDestination.Location = new System.Drawing.Point(50, 0);
			this.TextDestination.Name = "TextDestination";
			this.TextDestination.Size = new System.Drawing.Size(41, 15);
			this.TextDestination.TabIndex = 1;
			this.TextDestination.Text = "(行先)";
			// 
			// TextEventKind
			// 
			this.TextEventKind.AutoSize = true;
			this.TextEventKind.Location = new System.Drawing.Point(97, 0);
			this.TextEventKind.Name = "TextEventKind";
			this.TextEventKind.Size = new System.Drawing.Size(53, 15);
			this.TextEventKind.TabIndex = 2;
			this.TextEventKind.Text = "(イベント)";
			// 
			// TextEventDetail
			// 
			this.TextEventDetail.AutoSize = true;
			this.TextEventDetail.Location = new System.Drawing.Point(156, 0);
			this.TextEventDetail.Name = "TextEventDetail";
			this.TextEventDetail.Size = new System.Drawing.Size(99, 15);
			this.TextEventDetail.TabIndex = 3;
			this.TextEventDetail.Text = "(イベント詳細(仮))";
			// 
			// FormCompass
			// 
			this.AutoHidePortion = 150D;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(300, 200);
			this.Controls.Add(this.BasePanel);
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.HideOnClose = true;
			this.Name = "FormCompass";
			this.Text = "羅針盤";
			this.Load += new System.EventHandler(this.FormCompass_Load);
			this.BasePanel.ResumeLayout(false);
			this.BasePanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel BasePanel;
		private System.Windows.Forms.Label TextMapArea;
		private System.Windows.Forms.Label TextDestination;
		private System.Windows.Forms.Label TextEventKind;
		private System.Windows.Forms.Label TextEventDetail;
	}
}