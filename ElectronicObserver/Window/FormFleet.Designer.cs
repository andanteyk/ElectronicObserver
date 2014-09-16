namespace ElectronicObserver.Window {
	partial class FormFleet {
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
			this.TextDebug.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.TextDebug.Location = new System.Drawing.Point(15, 16);
			this.TextDebug.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.TextDebug.MaxLength = 0;
			this.TextDebug.Multiline = true;
			this.TextDebug.Name = "TextDebug";
			this.TextDebug.ReadOnly = true;
			this.TextDebug.Size = new System.Drawing.Size(270, 168);
			this.TextDebug.TabIndex = 0;
			// 
			// FormFleet
			// 
			this.AutoHidePortion = 150D;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(300, 200);
			this.Controls.Add(this.TextDebug);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.HideOnClose = true;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "FormFleet";
			this.Text = "*not loaded*";
			this.Load += new System.EventHandler(this.FormFleet_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox TextDebug;
	}
}