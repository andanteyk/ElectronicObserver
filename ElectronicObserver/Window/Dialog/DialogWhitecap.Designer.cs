namespace ElectronicObserver.Window.Dialog {
	partial class DialogWhitecap {
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
			this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// UpdateTimer
			// 
			this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
			// 
			// DialogWhitecap
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.DoubleBuffered = true;
			this.Font = Program.Window_Font;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogWhitecap";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "癒し処「白波」";
			this.Load += new System.EventHandler(this.DialogWhitecap_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.DialogWhitecap_Paint);
			this.DoubleClick += new System.EventHandler(this.DialogWhitecap_DoubleClick);
			this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DialogWhitecap_MouseClick);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer UpdateTimer;
	}
}