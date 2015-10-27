namespace ElectronicObserver.Window.Dialog {
	partial class DialogHalloween {
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
			this.components = new System.ComponentModel.Container();
			this.Updater = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// Updater
			// 
			this.Updater.Enabled = true;
			this.Updater.Tick += new System.EventHandler(this.Updater_Tick);
			// 
			// DialogHalloween
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.Magenta;
			this.ClientSize = new System.Drawing.Size(624, 441);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "DialogHalloween";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Happy Halloween!!";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogHalloween_FormClosed);
			this.Load += new System.EventHandler(this.DialogHalloween_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.DialogHalloween_Paint);
			this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DialogHalloween_MouseClick);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DialogHalloween_MouseMove);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer Updater;
	}
}