namespace ElectronicObserver.Window.Dialog {
	partial class DialogLocalAPILoader {
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
			this.PictureWarning = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.APIList = new System.Windows.Forms.ComboBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.TextFilePath = new System.Windows.Forms.TextBox();
			this.ButtonSearchFilePath = new System.Windows.Forms.Button();
			this.ButtonOpen = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.ButtonCancel = new System.Windows.Forms.Button();
			this.FileOpener = new System.Windows.Forms.OpenFileDialog();
			this.APICategory = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.PictureWarning)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// PictureWarning
			// 
			this.PictureWarning.Location = new System.Drawing.Point(13, 13);
			this.PictureWarning.Name = "PictureWarning";
			this.PictureWarning.Size = new System.Drawing.Size(32, 32);
			this.PictureWarning.TabIndex = 0;
			this.PictureWarning.TabStop = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label1.Location = new System.Drawing.Point(51, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(289, 30);
			this.label1.TabIndex = 1;
			this.label1.Text = "必ずオフラインの状態で操作してください。\r\nこの機能はデバッグ専用であるため、動作は保証できません。";
			// 
			// APIList
			// 
			this.APIList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.APIList.FormattingEnabled = true;
			this.APIList.Location = new System.Drawing.Point(142, 3);
			this.APIList.Name = "APIList";
			this.APIList.Size = new System.Drawing.Size(230, 20);
			this.APIList.TabIndex = 2;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.APICategory);
			this.panel1.Controls.Add(this.ButtonCancel);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.ButtonOpen);
			this.panel1.Controls.Add(this.ButtonSearchFilePath);
			this.panel1.Controls.Add(this.TextFilePath);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.APIList);
			this.panel1.Location = new System.Drawing.Point(0, 61);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(384, 101);
			this.panel1.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Meiryo UI", 9F);
			this.label2.Location = new System.Drawing.Point(12, 6);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 15);
			this.label2.TabIndex = 3;
			this.label2.Text = "API:";
			// 
			// TextFilePath
			// 
			this.TextFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TextFilePath.Location = new System.Drawing.Point(56, 31);
			this.TextFilePath.Name = "TextFilePath";
			this.TextFilePath.Size = new System.Drawing.Size(287, 19);
			this.TextFilePath.TabIndex = 4;
			// 
			// ButtonSearchFilePath
			// 
			this.ButtonSearchFilePath.Location = new System.Drawing.Point(349, 29);
			this.ButtonSearchFilePath.Name = "ButtonSearchFilePath";
			this.ButtonSearchFilePath.Size = new System.Drawing.Size(23, 23);
			this.ButtonSearchFilePath.TabIndex = 5;
			this.ButtonSearchFilePath.Text = "...";
			this.ButtonSearchFilePath.UseVisualStyleBackColor = true;
			this.ButtonSearchFilePath.Click += new System.EventHandler(this.ButtonSearchFilePath_Click);
			// 
			// ButtonOpen
			// 
			this.ButtonOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonOpen.Location = new System.Drawing.Point(216, 66);
			this.ButtonOpen.Name = "ButtonOpen";
			this.ButtonOpen.Size = new System.Drawing.Size(75, 23);
			this.ButtonOpen.TabIndex = 6;
			this.ButtonOpen.Text = "開く";
			this.ButtonOpen.UseVisualStyleBackColor = true;
			this.ButtonOpen.Click += new System.EventHandler(this.ButtonOpen_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Meiryo UI", 9F);
			this.label3.Location = new System.Drawing.Point(12, 32);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(38, 15);
			this.label3.TabIndex = 7;
			this.label3.Text = "Path:";
			// 
			// ButtonCancel
			// 
			this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.ButtonCancel.Location = new System.Drawing.Point(297, 66);
			this.ButtonCancel.Name = "ButtonCancel";
			this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
			this.ButtonCancel.TabIndex = 8;
			this.ButtonCancel.Text = "キャンセル";
			this.ButtonCancel.UseVisualStyleBackColor = true;
			this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
			// 
			// FileOpener
			// 
			this.FileOpener.Filter = "JSON|*.json;*.js|File|*";
			this.FileOpener.Title = "ファイルを開く";
			// 
			// APICategory
			// 
			this.APICategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.APICategory.FormattingEnabled = true;
			this.APICategory.Items.AddRange(new object[] {
            "Request",
            "Response"});
			this.APICategory.Location = new System.Drawing.Point(56, 3);
			this.APICategory.Name = "APICategory";
			this.APICategory.Size = new System.Drawing.Size(80, 20);
			this.APICategory.TabIndex = 9;
			this.APICategory.SelectedIndexChanged += new System.EventHandler(this.APICategory_SelectedIndexChanged);
			// 
			// DialogLocalAPILoader
			// 
			this.AcceptButton = this.ButtonOpen;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.ButtonCancel;
			this.ClientSize = new System.Drawing.Size(384, 162);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.PictureWarning);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogLocalAPILoader";
			this.Text = "ファイルからAPIをロード";
			this.Load += new System.EventHandler(this.DialogLocalAPILoader_Load);
			((System.ComponentModel.ISupportInitialize)(this.PictureWarning)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox PictureWarning;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox APIList;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button ButtonOpen;
		private System.Windows.Forms.Button ButtonSearchFilePath;
		private System.Windows.Forms.TextBox TextFilePath;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button ButtonCancel;
		private System.Windows.Forms.ComboBox APICategory;
		private System.Windows.Forms.OpenFileDialog FileOpener;
	}
}