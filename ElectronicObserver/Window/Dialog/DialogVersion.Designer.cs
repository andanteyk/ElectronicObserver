namespace ElectronicObserver.Window.Dialog {
	partial class DialogVersion {
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
			this.TextVersion = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.TextAuthor = new System.Windows.Forms.LinkLabel();
			this.ButtonClose = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// TextVersion
			// 
			this.TextVersion.AutoSize = true;
			this.TextVersion.Location = new System.Drawing.Point(12, 9);
			this.TextVersion.Margin = new System.Windows.Forms.Padding(3);
			this.TextVersion.Name = "TextVersion";
			this.TextVersion.Size = new System.Drawing.Size(208, 15);
			this.TextVersion.TabIndex = 0;
			this.TextVersion.Text = "試製七四式電子観測儀零型 (ver. 0.0)";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 30);
			this.label1.Margin = new System.Windows.Forms.Padding(3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(43, 15);
			this.label1.TabIndex = 1;
			this.label1.Text = "開発：";
			// 
			// TextAuthor
			// 
			this.TextAuthor.AutoSize = true;
			this.TextAuthor.Location = new System.Drawing.Point(61, 30);
			this.TextAuthor.Name = "TextAuthor";
			this.TextAuthor.Size = new System.Drawing.Size(55, 15);
			this.TextAuthor.TabIndex = 2;
			this.TextAuthor.TabStop = true;
			this.TextAuthor.Text = "Andante";
			this.TextAuthor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.TextAuthor_LinkClicked);
			// 
			// ButtonClose
			// 
			this.ButtonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonClose.Location = new System.Drawing.Point(297, 127);
			this.ButtonClose.Name = "ButtonClose";
			this.ButtonClose.Size = new System.Drawing.Size(75, 23);
			this.ButtonClose.TabIndex = 3;
			this.ButtonClose.Text = "閉じる";
			this.ButtonClose.UseVisualStyleBackColor = true;
			this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
			// 
			// DialogVersion
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(384, 162);
			this.Controls.Add(this.ButtonClose);
			this.Controls.Add(this.TextAuthor);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.TextVersion);
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogVersion";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "バージョン";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label TextVersion;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.LinkLabel TextAuthor;
		private System.Windows.Forms.Button ButtonClose;
	}
}