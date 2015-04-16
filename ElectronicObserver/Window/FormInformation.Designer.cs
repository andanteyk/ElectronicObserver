namespace ElectronicObserver.Window {
	partial class FormInformation {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInformation));
            this.TextInformation = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // TextInformation
            // 
            resources.ApplyResources(this.TextInformation, "TextInformation");
            this.TextInformation.AutoWordSelection = true;
            this.TextInformation.BackColor = System.Drawing.SystemColors.Control;
            this.TextInformation.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextInformation.Name = "TextInformation";
            this.TextInformation.ReadOnly = true;
            // 
            // FormInformation
            // 
            resources.ApplyResources(this, "$this");
            this.AutoHidePortion = 150D;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.TextInformation);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HideOnClose = true;
            this.Name = "FormInformation";
            this.Load += new System.EventHandler(this.FormInformation_Load);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RichTextBox TextInformation;
	}
}