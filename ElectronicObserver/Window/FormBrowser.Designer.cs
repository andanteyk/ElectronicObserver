namespace ElectronicObserver.Window {
	partial class FormBrowser {
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
			this.Browser = new System.Windows.Forms.WebBrowser();
			this.SizeAdjuster = new System.Windows.Forms.Panel();
			this.SizeAdjuster.SuspendLayout();
			this.SuspendLayout();
			// 
			// Browser
			// 
			this.Browser.AllowWebBrowserDrop = false;
			this.Browser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Browser.IsWebBrowserContextMenuEnabled = false;
			this.Browser.Location = new System.Drawing.Point(0, 0);
			this.Browser.Margin = new System.Windows.Forms.Padding(0);
			this.Browser.MinimumSize = new System.Drawing.Size(20, 20);
			this.Browser.Name = "Browser";
			this.Browser.ScriptErrorsSuppressed = true;
			this.Browser.Size = new System.Drawing.Size(300, 200);
			this.Browser.TabIndex = 0;
			this.Browser.WebBrowserShortcutsEnabled = false;
			this.Browser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.Browser_DocumentCompleted);
			this.Browser.SizeChanged += new System.EventHandler(this.SizeAdjuster_SizeChanged);
			// 
			// SizeAdjuster
			// 
			this.SizeAdjuster.AutoScroll = true;
			this.SizeAdjuster.Controls.Add(this.Browser);
			this.SizeAdjuster.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SizeAdjuster.Location = new System.Drawing.Point(0, 0);
			this.SizeAdjuster.Margin = new System.Windows.Forms.Padding(0);
			this.SizeAdjuster.Name = "SizeAdjuster";
			this.SizeAdjuster.Size = new System.Drawing.Size(300, 200);
			this.SizeAdjuster.TabIndex = 1;
			this.SizeAdjuster.SizeChanged += new System.EventHandler(this.SizeAdjuster_SizeChanged);
			// 
			// FormBrowser
			// 
			this.AutoHidePortion = 150D;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(300, 200);
			this.Controls.Add(this.SizeAdjuster);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.HideOnClose = true;
			this.Name = "FormBrowser";
			this.Text = "ブラウザ";
			this.Load += new System.EventHandler(this.FormBrowser_Load);
			this.SizeAdjuster.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.WebBrowser Browser;
		private System.Windows.Forms.Panel SizeAdjuster;
	}
}