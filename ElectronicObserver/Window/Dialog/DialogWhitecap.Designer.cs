namespace ElectronicObserver.Window.Dialog
{
	partial class DialogWhitecap
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
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
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogWhitecap";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "癒し処「白波」";
			this.Load += new System.EventHandler(this.DialogWhitecap_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.DialogWhitecap_Paint);
			this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DialogWhitecap_MouseClick);
			this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DialogWhitecap_MouseDoubleClick);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer UpdateTimer;
	}
}