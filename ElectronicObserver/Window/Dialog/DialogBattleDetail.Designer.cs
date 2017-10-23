namespace ElectronicObserver.Window.Dialog
{
	partial class DialogBattleDetail
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
			this.TextBattleDetail = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// TextBattleDetail
			// 
			this.TextBattleDetail.AutoSize = true;
			this.TextBattleDetail.Location = new System.Drawing.Point(13, 13);
			this.TextBattleDetail.Name = "TextBattleDetail";
			this.TextBattleDetail.Size = new System.Drawing.Size(55, 15);
			this.TextBattleDetail.TabIndex = 0;
			this.TextBattleDetail.Text = "戦闘詳細";
			// 
			// DialogBattleDetail
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.TextBattleDetail);
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogBattleDetail";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "戦闘詳細";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogBattleDetail_FormClosed);
			this.Load += new System.EventHandler(this.DialogBattleDetail_Load);
			this.Shown += new System.EventHandler(this.DialogBattleDetail_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label TextBattleDetail;
	}
}