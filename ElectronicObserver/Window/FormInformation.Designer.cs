namespace ElectronicObserver.Window
{
	partial class FormInformation
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
			this.TextInformation = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// TextInformation
			// 
			this.TextInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.TextInformation.AutoWordSelection = true;
			this.TextInformation.BackColor = System.Drawing.SystemColors.Control;
			this.TextInformation.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.TextInformation.DetectUrls = false;
			this.TextInformation.Location = new System.Drawing.Point(3, 3);
			this.TextInformation.Name = "TextInformation";
			this.TextInformation.ReadOnly = true;
			this.TextInformation.Size = new System.Drawing.Size(294, 194);
			this.TextInformation.TabIndex = 0;
			this.TextInformation.Text = "";
			// 
			// FormInformation
			// 
			this.AutoHidePortion = 150D;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(300, 200);
			this.Controls.Add(this.TextInformation);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.HideOnClose = true;
			this.Name = "FormInformation";
			this.Text = "情報";
			this.Load += new System.EventHandler(this.FormInformation_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RichTextBox TextInformation;
	}
}