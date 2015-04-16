namespace ElectronicObserver.Window.Dialog {
	partial class DialogNotifier {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogNotifier));
            this.CloseTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // CloseTimer
            // 
            this.CloseTimer.Tick += new System.EventHandler(this.CloseTimer_Tick);
            // 
            // DialogNotifier
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogNotifier";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.DialogNotifier_Load);
            this.Click += new System.EventHandler(this.DialogNotifier_Click);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DialogNotifier_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DialogNotifier_KeyDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DialogNotifier_MouseMove);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer CloseTimer;
	}
}