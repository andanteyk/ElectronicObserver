namespace ElectronicObserver.Window {
	partial class FormBrowserHost {
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
			this.LabelWarning = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// LabelWarning
			// 
			this.LabelWarning.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LabelWarning.Location = new System.Drawing.Point(12, 9);
			this.LabelWarning.Name = "LabelWarning";
			this.LabelWarning.Size = new System.Drawing.Size(276, 182);
			this.LabelWarning.TabIndex = 0;
			this.LabelWarning.Text = "何らかの原因によってブラウザプロセスの起動に失敗したか、通信が途絶しました。\r\n申し訳ありませんが再起動してください。";
			// 
			// FormBrowserHost
			// 
			this.AutoHidePortion = 150D;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(300, 200);
			this.Controls.Add(this.LabelWarning);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.HideOnClose = true;
			this.Name = "FormBrowserHost";
			this.Text = "ブラウザ";
			this.Load += new System.EventHandler(this.FormBrowser_Load);
			this.Resize += new System.EventHandler(this.FormBrowserHost_Resize);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label LabelWarning;

    }
}