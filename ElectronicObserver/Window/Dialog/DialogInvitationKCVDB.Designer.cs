namespace ElectronicObserver.Window.Dialog {
	partial class DialogInvitationKCVDB {
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
			this.ButtonYes = new System.Windows.Forms.Button();
			this.ButtonNo = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.LinkGuideline = new System.Windows.Forms.LinkLabel();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// ButtonYes
			// 
			this.ButtonYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonYes.Location = new System.Drawing.Point(296, 166);
			this.ButtonYes.Name = "ButtonYes";
			this.ButtonYes.Size = new System.Drawing.Size(75, 23);
			this.ButtonYes.TabIndex = 1;
			this.ButtonYes.Text = "はい";
			this.ButtonYes.UseVisualStyleBackColor = true;
			this.ButtonYes.Click += new System.EventHandler(this.ButtonYes_Click);
			// 
			// ButtonNo
			// 
			this.ButtonNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonNo.Location = new System.Drawing.Point(377, 166);
			this.ButtonNo.Name = "ButtonNo";
			this.ButtonNo.Size = new System.Drawing.Size(75, 23);
			this.ButtonNo.TabIndex = 0;
			this.ButtonNo.Text = "いいえ";
			this.ButtonNo.UseVisualStyleBackColor = true;
			this.ButtonNo.Click += new System.EventHandler(this.ButtonNo_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(428, 75);
			this.label1.TabIndex = 2;
			this.label1.Text = "ver. 2.2.0 から、本ツールは艦これ検証データベースに対応しました。\r\n\r\n艦これ検証データベースでは、艦これの検証をする上で必要なプレイデータを集めてい" +
    "ます。\r\n集められたデータをもとに様々な検証を行い、その結果わかったことを広く公開します。\r\n検証データベースの詳しい説明については、以下の運営ガイドラインをご" +
    "確認ください。";
			// 
			// LinkGuideline
			// 
			this.LinkGuideline.AutoSize = true;
			this.LinkGuideline.Location = new System.Drawing.Point(21, 94);
			this.LinkGuideline.Margin = new System.Windows.Forms.Padding(12, 6, 12, 6);
			this.LinkGuideline.Name = "LinkGuideline";
			this.LinkGuideline.Size = new System.Drawing.Size(158, 15);
			this.LinkGuideline.TabIndex = 3;
			this.LinkGuideline.TabStop = true;
			this.LinkGuideline.Text = "http://kcvdb.jp/guidelines";
			this.LinkGuideline.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkGuideline_LinkClicked);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 115);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(283, 60);
			this.label2.TabIndex = 4;
			this.label2.Text = "\r\n艦これ検証データベースへプレイデータを送信しますか？\r\n\r\n（[設定→データベース] からいつでも on/off できます。）";
			// 
			// DialogInvitationKCVDB
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(464, 201);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.LinkGuideline);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.ButtonNo);
			this.Controls.Add(this.ButtonYes);
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogInvitationKCVDB";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "艦これ検証データベース連携";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button ButtonYes;
		private System.Windows.Forms.Button ButtonNo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.LinkLabel LinkGuideline;
		private System.Windows.Forms.Label label2;
	}
}