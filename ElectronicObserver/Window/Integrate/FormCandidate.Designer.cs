namespace ElectronicObserver.Window.Integrate {
	partial class FormCandidate {
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
			this.SuspendLayout();
			// 
			// FormCandidate
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Blue;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "FormCandidate";
			this.ShowInTaskbar = false;
			this.Text = "FormCandidate";
			this.TransparencyKey = System.Drawing.Color.Blue;
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormCandidate_FormClosed);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormCandidate_Paint);
			this.Resize += new System.EventHandler(this.FormCandidate_Resize);
			this.ResumeLayout(false);

		}

		#endregion
	}
}