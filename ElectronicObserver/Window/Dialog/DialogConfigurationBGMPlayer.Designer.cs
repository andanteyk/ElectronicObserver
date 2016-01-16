namespace ElectronicObserver.Window.Dialog {
	partial class DialogConfigurationBGMPlayer {
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
			this.label1 = new System.Windows.Forms.Label();
			this.FilePath = new System.Windows.Forms.TextBox();
			this.FilePathSearch = new System.Windows.Forms.Button();
			this.IsLoop = new System.Windows.Forms.CheckBox();
			this.LoopHeadPosition = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.Volume = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.ButtonCancel = new System.Windows.Forms.Button();
			this.ButtonAccept = new System.Windows.Forms.Button();
			this.OpenMusicDialog = new System.Windows.Forms.OpenFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.LoopHeadPosition)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Volume)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = "路径：";
			// 
			// FilePath
			// 
			this.FilePath.AllowDrop = true;
			this.FilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.FilePath.Location = new System.Drawing.Point(56, 12);
			this.FilePath.Name = "FilePath";
			this.FilePath.Size = new System.Drawing.Size(344, 23);
			this.FilePath.TabIndex = 1;
			this.FilePath.DragDrop += new System.Windows.Forms.DragEventHandler(this.FilePath_DragDrop);
			this.FilePath.DragEnter += new System.Windows.Forms.DragEventHandler(this.FilePath_DragEnter);
			// 
			// FilePathSearch
			// 
			this.FilePathSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.FilePathSearch.Location = new System.Drawing.Point(406, 12);
			this.FilePathSearch.Name = "FilePathSearch";
			this.FilePathSearch.Size = new System.Drawing.Size(46, 23);
			this.FilePathSearch.TabIndex = 2;
			this.FilePathSearch.Text = "...";
			this.FilePathSearch.UseVisualStyleBackColor = true;
			this.FilePathSearch.Click += new System.EventHandler(this.FilePathSearch_Click);
			// 
			// IsLoop
			// 
			this.IsLoop.AutoSize = true;
			this.IsLoop.Location = new System.Drawing.Point(12, 42);
			this.IsLoop.Name = "IsLoop";
			this.IsLoop.Size = new System.Drawing.Size(55, 19);
			this.IsLoop.TabIndex = 3;
			this.IsLoop.Text = "循环";
			this.IsLoop.UseVisualStyleBackColor = true;
			// 
			// LoopHeadPosition
			// 
			this.LoopHeadPosition.DecimalPlaces = 3;
			this.LoopHeadPosition.Location = new System.Drawing.Point(122, 41);
			this.LoopHeadPosition.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            196608});
			this.LoopHeadPosition.Name = "LoopHeadPosition";
			this.LoopHeadPosition.Size = new System.Drawing.Size(100, 23);
			this.LoopHeadPosition.TabIndex = 4;
			this.LoopHeadPosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(73, 43);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(43, 15);
			this.label2.TabIndex = 5;
			this.label2.Text = "开始：";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(228, 43);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(19, 15);
			this.label3.TabIndex = 6;
			this.label3.Text = "秒";
			// 
			// Volume
			// 
			this.Volume.Location = new System.Drawing.Point(61, 70);
			this.Volume.Name = "Volume";
			this.Volume.Size = new System.Drawing.Size(60, 23);
			this.Volume.TabIndex = 7;
			this.Volume.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 72);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(43, 15);
			this.label4.TabIndex = 8;
			this.label4.Text = "音量：";
			// 
			// ButtonCancel
			// 
			this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.ButtonCancel.Location = new System.Drawing.Point(377, 86);
			this.ButtonCancel.Name = "ButtonCancel";
			this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
			this.ButtonCancel.TabIndex = 9;
			this.ButtonCancel.Text = "取消";
			this.ButtonCancel.UseVisualStyleBackColor = true;
			this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
			// 
			// ButtonAccept
			// 
			this.ButtonAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonAccept.Location = new System.Drawing.Point(296, 86);
			this.ButtonAccept.Name = "ButtonAccept";
			this.ButtonAccept.Size = new System.Drawing.Size(75, 23);
			this.ButtonAccept.TabIndex = 10;
			this.ButtonAccept.Text = "OK";
			this.ButtonAccept.UseVisualStyleBackColor = true;
			this.ButtonAccept.Click += new System.EventHandler(this.ButtonAccept_Click);
			// 
			// OpenMusicDialog
			// 
			this.OpenMusicDialog.Title = "选择音量文件";
			// 
			// DialogConfigurationBGMPlayer
			// 
			this.AcceptButton = this.ButtonAccept;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.ButtonCancel;
			this.ClientSize = new System.Drawing.Size(464, 121);
			this.Controls.Add(this.ButtonAccept);
			this.Controls.Add(this.ButtonCancel);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.Volume);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.LoopHeadPosition);
			this.Controls.Add(this.IsLoop);
			this.Controls.Add(this.FilePathSearch);
			this.Controls.Add(this.FilePath);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "DialogConfigurationBGMPlayer";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "BGMの設定";
			this.Load += new System.EventHandler(this.DialogConfigurationBGMPlayer_Load);
			((System.ComponentModel.ISupportInitialize)(this.LoopHeadPosition)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Volume)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox FilePath;
		private System.Windows.Forms.Button FilePathSearch;
		private System.Windows.Forms.CheckBox IsLoop;
		private System.Windows.Forms.NumericUpDown LoopHeadPosition;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown Volume;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button ButtonCancel;
		private System.Windows.Forms.Button ButtonAccept;
		private System.Windows.Forms.OpenFileDialog OpenMusicDialog;
	}
}