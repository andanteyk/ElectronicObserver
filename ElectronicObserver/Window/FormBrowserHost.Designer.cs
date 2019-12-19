namespace ElectronicObserver.Window
{
	partial class FormBrowserHost
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.SuspendLayout();
            // 
            // FormBrowserHost
            // 
            this.AutoHidePortion = 150D;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(300, 200);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HideOnClose = true;
            this.Name = "FormBrowserHost";
            this.Text = "ブラウザ";
            this.Load += new System.EventHandler(this.FormBrowser_Load);
            this.Click += new System.EventHandler(this.FormBrowserHost_Click);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormBrowserHost_Paint);
            this.Resize += new System.EventHandler(this.FormBrowserHost_Resize);
            this.ResumeLayout(false);

		}

		#endregion


	}
}