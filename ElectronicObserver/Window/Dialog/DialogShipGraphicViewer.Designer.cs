namespace ElectronicObserver.Window.Dialog {
	partial class DialogShipGraphicViewer {
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
			this.TopMenu = new System.Windows.Forms.MenuStrip();
			this.TopMenu_File = new System.Windows.Forms.ToolStripMenuItem();
			this.TopMenu_File_Open = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.TopMenu_File_SaveImage = new System.Windows.Forms.ToolStripMenuItem();
			this.TopMenu_File_SaveAllImage = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.TopMenu_File_CopyToClipboard = new System.Windows.Forms.ToolStripMenuItem();
			this.TopMenu_View = new System.Windows.Forms.ToolStripMenuItem();
			this.TopMenu_View_InterpolationMode = new System.Windows.Forms.ToolStripMenuItem();
			this.TopMenu_View_InterpolationMode_Sharp = new System.Windows.Forms.ToolStripMenuItem();
			this.TopMenu_View_InterpolationMode_Smooth = new System.Windows.Forms.ToolStripMenuItem();
			this.TopMenu_View_Zoom = new System.Windows.Forms.ToolStripMenuItem();
			this.TopMenu_View_Zoom_In = new System.Windows.Forms.ToolStripMenuItem();
			this.TopMenu_View_Zoom_Out = new System.Windows.Forms.ToolStripMenuItem();
			this.TopMenu_View_Zoom_100 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.TopMenu_View_Zoom_Fit = new System.Windows.Forms.ToolStripMenuItem();
			this.DrawingPanel = new System.Windows.Forms.Panel();
			this.OpenSwfDialog = new System.Windows.Forms.OpenFileDialog();
			this.SaveImageDialog = new System.Windows.Forms.SaveFileDialog();
			this.SaveFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.TopMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// TopMenu
			// 
			this.TopMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TopMenu_File,
            this.TopMenu_View});
			this.TopMenu.Location = new System.Drawing.Point(0, 0);
			this.TopMenu.Name = "TopMenu";
			this.TopMenu.Size = new System.Drawing.Size(784, 24);
			this.TopMenu.TabIndex = 0;
			this.TopMenu.Text = "menuStrip1";
			// 
			// TopMenu_File
			// 
			this.TopMenu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TopMenu_File_Open,
            this.toolStripSeparator2,
            this.TopMenu_File_SaveImage,
            this.TopMenu_File_SaveAllImage,
            this.toolStripSeparator1,
            this.TopMenu_File_CopyToClipboard});
			this.TopMenu_File.Name = "TopMenu_File";
			this.TopMenu_File.Size = new System.Drawing.Size(70, 20);
			this.TopMenu_File.Text = "ファイル(&F)";
			// 
			// TopMenu_File_Open
			// 
			this.TopMenu_File_Open.Name = "TopMenu_File_Open";
			this.TopMenu_File_Open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.TopMenu_File_Open.Size = new System.Drawing.Size(271, 22);
			this.TopMenu_File_Open.Text = "開く(&O)";
			this.TopMenu_File_Open.Click += new System.EventHandler(this.TopMenu_File_Open_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(268, 6);
			// 
			// TopMenu_File_SaveImage
			// 
			this.TopMenu_File_SaveImage.Name = "TopMenu_File_SaveImage";
			this.TopMenu_File_SaveImage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.TopMenu_File_SaveImage.Size = new System.Drawing.Size(271, 22);
			this.TopMenu_File_SaveImage.Text = "現在の画像を保存(&S)";
			this.TopMenu_File_SaveImage.Click += new System.EventHandler(this.TopMenu_File_SaveImage_Click);
			// 
			// TopMenu_File_SaveAllImage
			// 
			this.TopMenu_File_SaveAllImage.Name = "TopMenu_File_SaveAllImage";
			this.TopMenu_File_SaveAllImage.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
			this.TopMenu_File_SaveAllImage.Size = new System.Drawing.Size(271, 22);
			this.TopMenu_File_SaveAllImage.Text = "すべての画像を保存(&A)";
			this.TopMenu_File_SaveAllImage.Click += new System.EventHandler(this.TopMenu_File_SaveAllImage_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(268, 6);
			// 
			// TopMenu_File_CopyToClipboard
			// 
			this.TopMenu_File_CopyToClipboard.Name = "TopMenu_File_CopyToClipboard";
			this.TopMenu_File_CopyToClipboard.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.TopMenu_File_CopyToClipboard.Size = new System.Drawing.Size(271, 22);
			this.TopMenu_File_CopyToClipboard.Text = "クリップボードにコピー(&C)";
			this.TopMenu_File_CopyToClipboard.Click += new System.EventHandler(this.TopMenu_File_CopyToClipboard_Click);
			// 
			// TopMenu_View
			// 
			this.TopMenu_View.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TopMenu_View_InterpolationMode,
            this.TopMenu_View_Zoom});
			this.TopMenu_View.Name = "TopMenu_View";
			this.TopMenu_View.Size = new System.Drawing.Size(61, 20);
			this.TopMenu_View.Text = "表示(&V)";
			// 
			// TopMenu_View_InterpolationMode
			// 
			this.TopMenu_View_InterpolationMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TopMenu_View_InterpolationMode_Sharp,
            this.TopMenu_View_InterpolationMode_Smooth});
			this.TopMenu_View_InterpolationMode.Name = "TopMenu_View_InterpolationMode";
			this.TopMenu_View_InterpolationMode.Size = new System.Drawing.Size(122, 22);
			this.TopMenu_View_InterpolationMode.Text = "描画(&I)";
			// 
			// TopMenu_View_InterpolationMode_Sharp
			// 
			this.TopMenu_View_InterpolationMode_Sharp.Checked = true;
			this.TopMenu_View_InterpolationMode_Sharp.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			this.TopMenu_View_InterpolationMode_Sharp.Name = "TopMenu_View_InterpolationMode_Sharp";
			this.TopMenu_View_InterpolationMode_Sharp.Size = new System.Drawing.Size(130, 22);
			this.TopMenu_View_InterpolationMode_Sharp.Text = "くっきり(&N)";
			this.TopMenu_View_InterpolationMode_Sharp.Click += new System.EventHandler(this.TopMenu_View_InterpolationMode_Sharp_Click);
			// 
			// TopMenu_View_InterpolationMode_Smooth
			// 
			this.TopMenu_View_InterpolationMode_Smooth.Name = "TopMenu_View_InterpolationMode_Smooth";
			this.TopMenu_View_InterpolationMode_Smooth.Size = new System.Drawing.Size(130, 22);
			this.TopMenu_View_InterpolationMode_Smooth.Text = "なめらか(&B)";
			this.TopMenu_View_InterpolationMode_Smooth.Click += new System.EventHandler(this.TopMenu_View_InterpolationMode_Sharp_Click);
			// 
			// TopMenu_View_Zoom
			// 
			this.TopMenu_View_Zoom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TopMenu_View_Zoom_In,
            this.TopMenu_View_Zoom_Out,
            this.TopMenu_View_Zoom_100,
            this.toolStripSeparator3,
            this.TopMenu_View_Zoom_Fit});
			this.TopMenu_View_Zoom.Name = "TopMenu_View_Zoom";
			this.TopMenu_View_Zoom.Size = new System.Drawing.Size(122, 22);
			this.TopMenu_View_Zoom.Text = "ズーム(&Z)";
			// 
			// TopMenu_View_Zoom_In
			// 
			this.TopMenu_View_Zoom_In.Name = "TopMenu_View_Zoom_In";
			this.TopMenu_View_Zoom_In.ShortcutKeyDisplayString = "Ctrl++";
			this.TopMenu_View_Zoom_In.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Oemplus)));
			this.TopMenu_View_Zoom_In.Size = new System.Drawing.Size(169, 22);
			this.TopMenu_View_Zoom_In.Text = "拡大(&E)";
			this.TopMenu_View_Zoom_In.Click += new System.EventHandler(this.TopMenu_View_Zoom_In_Click);
			// 
			// TopMenu_View_Zoom_Out
			// 
			this.TopMenu_View_Zoom_Out.Name = "TopMenu_View_Zoom_Out";
			this.TopMenu_View_Zoom_Out.ShortcutKeyDisplayString = "Ctrl+-";
			this.TopMenu_View_Zoom_Out.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.OemMinus)));
			this.TopMenu_View_Zoom_Out.Size = new System.Drawing.Size(169, 22);
			this.TopMenu_View_Zoom_Out.Text = "縮小(&S)";
			this.TopMenu_View_Zoom_Out.Click += new System.EventHandler(this.TopMenu_View_Zoom_Out_Click);
			// 
			// TopMenu_View_Zoom_100
			// 
			this.TopMenu_View_Zoom_100.Name = "TopMenu_View_Zoom_100";
			this.TopMenu_View_Zoom_100.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D0)));
			this.TopMenu_View_Zoom_100.Size = new System.Drawing.Size(169, 22);
			this.TopMenu_View_Zoom_100.Text = "100%(&1)";
			this.TopMenu_View_Zoom_100.Click += new System.EventHandler(this.TopMenu_View_Zoom_100_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(166, 6);
			// 
			// TopMenu_View_Zoom_Fit
			// 
			this.TopMenu_View_Zoom_Fit.CheckOnClick = true;
			this.TopMenu_View_Zoom_Fit.Name = "TopMenu_View_Zoom_Fit";
			this.TopMenu_View_Zoom_Fit.Size = new System.Drawing.Size(169, 22);
			this.TopMenu_View_Zoom_Fit.Text = "ぴったり(&F)";
			this.TopMenu_View_Zoom_Fit.Click += new System.EventHandler(this.TopMenu_View_Zoom_Fit_Click);
			// 
			// DrawingPanel
			// 
			this.DrawingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DrawingPanel.Location = new System.Drawing.Point(0, 24);
			this.DrawingPanel.Name = "DrawingPanel";
			this.DrawingPanel.Size = new System.Drawing.Size(784, 897);
			this.DrawingPanel.TabIndex = 1;
			this.DrawingPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawingPanel_Paint);
			this.DrawingPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DrawingPanel_MouseClick);
			this.DrawingPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawingPanel_MouseMove);
			this.DrawingPanel.Resize += new System.EventHandler(this.DrawingPanel_Resize);
			// 
			// OpenSwfDialog
			// 
			this.OpenSwfDialog.Filter = "SWF|*.swf|File|*";
			this.OpenSwfDialog.Multiselect = true;
			this.OpenSwfDialog.Title = "SWF ファイルを開く";
			// 
			// SaveImageDialog
			// 
			this.SaveImageDialog.Filter = "PNG|*.png|File|*";
			this.SaveImageDialog.Title = "画像の保存";
			// 
			// DialogShipGraphicViewer
			// 
			this.AllowDrop = true;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(784, 921);
			this.Controls.Add(this.DrawingPanel);
			this.Controls.Add(this.TopMenu);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.MainMenuStrip = this.TopMenu;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogShipGraphicViewer";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "艦船画像ビューア";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogShipGraphicViewer_FormClosed);
			this.Load += new System.EventHandler(this.DialogShipGraphicViewer_Load);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.DialogShipGraphicViewer_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.DialogShipGraphicViewer_DragEnter);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DialogShipGraphicViewer_KeyDown);
			this.TopMenu.ResumeLayout(false);
			this.TopMenu.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip TopMenu;
		private System.Windows.Forms.ToolStripMenuItem TopMenu_File;
		private System.Windows.Forms.ToolStripMenuItem TopMenu_File_SaveImage;
		private System.Windows.Forms.ToolStripMenuItem TopMenu_File_SaveAllImage;
		private System.Windows.Forms.ToolStripMenuItem TopMenu_View;
		private System.Windows.Forms.ToolStripMenuItem TopMenu_View_InterpolationMode;
		private System.Windows.Forms.ToolStripMenuItem TopMenu_View_InterpolationMode_Sharp;
		private System.Windows.Forms.ToolStripMenuItem TopMenu_View_InterpolationMode_Smooth;
		private System.Windows.Forms.ToolStripMenuItem TopMenu_View_Zoom;
		private System.Windows.Forms.ToolStripMenuItem TopMenu_View_Zoom_100;
		private System.Windows.Forms.ToolStripMenuItem TopMenu_View_Zoom_Fit;
		private System.Windows.Forms.Panel DrawingPanel;
		private System.Windows.Forms.ToolStripMenuItem TopMenu_File_Open;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem TopMenu_File_CopyToClipboard;
		private System.Windows.Forms.OpenFileDialog OpenSwfDialog;
		private System.Windows.Forms.SaveFileDialog SaveImageDialog;
		private System.Windows.Forms.FolderBrowserDialog SaveFolderDialog;
		private System.Windows.Forms.ToolStripMenuItem TopMenu_View_Zoom_In;
		private System.Windows.Forms.ToolStripMenuItem TopMenu_View_Zoom_Out;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
	}
}