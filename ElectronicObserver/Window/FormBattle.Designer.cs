namespace ElectronicObserver.Window {
	partial class FormBattle {
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
			this.TextDebug = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// TextDebug
			// 
			this.TextDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TextDebug.Location = new System.Drawing.Point(9, 9);
			this.TextDebug.Margin = new System.Windows.Forms.Padding(0);
			this.TextDebug.MaxLength = 0;
			this.TextDebug.Multiline = true;
			this.TextDebug.Name = "TextDebug";
			this.TextDebug.ReadOnly = true;
			this.TextDebug.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.TextDebug.Size = new System.Drawing.Size(282, 182);
			this.TextDebug.TabIndex = 0;
			// 
			// FormBattle
			// 
			this.AutoHidePortion = 150D;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(300, 200);
			this.Controls.Add(this.TextDebug);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.HideOnClose = true;
			this.Name = "FormBattle";
			this.Text = "戦闘";
			this.Load += new System.EventHandler(this.FormBattle_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox TextDebug;
	}
}