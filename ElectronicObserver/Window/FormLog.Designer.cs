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
			this.LogList = new System.Windows.Forms.ListBox();
			this.ContextMenuLog = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.ContextMenuLog_Clear = new System.Windows.Forms.ToolStripMenuItem();
			this.ContextMenuLog.SuspendLayout();
			this.SuspendLayout();
			// 
			// LogList
			// 
			this.LogList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LogList.BackColor = Utility.Configuration.Config.UI.BackColor.ColorData;
			this.LogList.ForeColor = Utility.Configuration.Config.UI.ForeColor.ColorData;
			this.LogList.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.LogList.ContextMenuStrip = this.ContextMenuLog;
			this.LogList.FormattingEnabled = true;
			this.LogList.HorizontalScrollbar = true;
			this.LogList.IntegralHeight = false;
			this.LogList.ItemHeight = 15;
			this.LogList.Location = new System.Drawing.Point(0, 0);
			this.LogList.Name = "LogList";
			this.LogList.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.LogList.Size = new System.Drawing.Size(300, 200);
			this.LogList.TabIndex = 0;
			// 
			// ContextMenuLog
			// 
			ToolStripCustomizer.ToolStripRender.SetRender(this.ContextMenuLog);
			this.ContextMenuLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContextMenuLog_Clear});
			this.ContextMenuLog.Name = "ContextMenuLog";
			this.ContextMenuLog.Size = new System.Drawing.Size(131, 26);
			// 
			// ContextMenuLog_Clear
			// 
			this.ContextMenuLog_Clear.Name = "ContextMenuLog_Clear";
			this.ContextMenuLog_Clear.Size = new System.Drawing.Size(152, 22);
			this.ContextMenuLog_Clear.Text = "クリア(&C)";
			this.ContextMenuLog_Clear.Click += new System.EventHandler(this.ContextMenuLog_Clear_Click);
			// 
			// FormLog
			// 
			this.AutoHidePortion = 150D;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(300, 200);
			this.Controls.Add(this.LogList);
			this.DoubleBuffered = true;
			this.Font = Program.Window_Font;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.HideOnClose = true;
			this.Name = "FormLog";
			this.Text = "日志";
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