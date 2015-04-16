namespace ElectronicObserver.Window {
	partial class FormLog {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLog));
            this.LogList = new System.Windows.Forms.ListBox();
            this.ContextMenuLog = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ContextMenuLog_Clear = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // LogList
            // 
            resources.ApplyResources(this.LogList, "LogList");
            this.LogList.BackColor = System.Drawing.SystemColors.Control;
            this.LogList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LogList.ContextMenuStrip = this.ContextMenuLog;
            this.LogList.FormattingEnabled = true;
            this.LogList.Name = "LogList";
            this.LogList.SelectionMode = System.Windows.Forms.SelectionMode.None;
            // 
            // ContextMenuLog
            // 
            resources.ApplyResources(this.ContextMenuLog, "ContextMenuLog");
            this.ContextMenuLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContextMenuLog_Clear});
            this.ContextMenuLog.Name = "ContextMenuLog";
            // 
            // ContextMenuLog_Clear
            // 
            resources.ApplyResources(this.ContextMenuLog_Clear, "ContextMenuLog_Clear");
            this.ContextMenuLog_Clear.Name = "ContextMenuLog_Clear";
            this.ContextMenuLog_Clear.Click += new System.EventHandler(this.ContextMenuLog_Clear_Click);
            // 
            // FormLog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoHidePortion = 150D;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.LogList);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HideOnClose = true;
            this.Name = "FormLog";
            this.Load += new System.EventHandler(this.FormLog_Load);
            this.ContextMenuLog.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox LogList;
		private System.Windows.Forms.ContextMenuStrip ContextMenuLog;
		private System.Windows.Forms.ToolStripMenuItem ContextMenuLog_Clear;

	}
}