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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogLocalAPILoader));
            this.PictureWarning = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.APIList = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.APICategory = new System.Windows.Forms.ComboBox();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.ButtonOpen = new System.Windows.Forms.Button();
            this.ButtonSearchFilePath = new System.Windows.Forms.Button();
            this.TextFilePath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.FileOpener = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.PictureWarning)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PictureWarning
            // 
            resources.ApplyResources(this.PictureWarning, "PictureWarning");
            this.PictureWarning.Name = "PictureWarning";
            this.PictureWarning.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // APIList
            // 
            resources.ApplyResources(this.APIList, "APIList");
            this.APIList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.APIList.FormattingEnabled = true;
            this.APIList.Name = "APIList";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.APICategory);
            this.panel1.Controls.Add(this.ButtonCancel);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.ButtonOpen);
            this.panel1.Controls.Add(this.ButtonSearchFilePath);
            this.panel1.Controls.Add(this.TextFilePath);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.APIList);
            this.panel1.Name = "panel1";
            // 
            // APICategory
            // 
            resources.ApplyResources(this.APICategory, "APICategory");
            this.APICategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.APICategory.FormattingEnabled = true;
            this.APICategory.Items.AddRange(new object[] {
            resources.GetString("APICategory.Items"),
            resources.GetString("APICategory.Items1")});
            this.APICategory.Name = "APICategory";
            this.APICategory.SelectedIndexChanged += new System.EventHandler(this.APICategory_SelectedIndexChanged);
            // 
            // ButtonCancel
            // 
            resources.ApplyResources(this.ButtonCancel, "ButtonCancel");
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // ButtonOpen
            // 
            resources.ApplyResources(this.ButtonOpen, "ButtonOpen");
            this.ButtonOpen.Name = "ButtonOpen";
            this.ButtonOpen.UseVisualStyleBackColor = true;
            this.ButtonOpen.Click += new System.EventHandler(this.ButtonOpen_Click);
            // 
            // ButtonSearchFilePath
            // 
            resources.ApplyResources(this.ButtonSearchFilePath, "ButtonSearchFilePath");
            this.ButtonSearchFilePath.Name = "ButtonSearchFilePath";
            this.ButtonSearchFilePath.UseVisualStyleBackColor = true;
            this.ButtonSearchFilePath.Click += new System.EventHandler(this.ButtonSearchFilePath_Click);
            // 
            // TextFilePath
            // 
            resources.ApplyResources(this.TextFilePath, "TextFilePath");
            this.TextFilePath.AllowDrop = true;
            this.TextFilePath.Name = "TextFilePath";
            this.TextFilePath.DragDrop += new System.Windows.Forms.DragEventHandler(this.TextFilePath_DragDrop);
            this.TextFilePath.DragEnter += new System.Windows.Forms.DragEventHandler(this.TextFilePath_DragEnter);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // FileOpener
            // 
            resources.ApplyResources(this.FileOpener, "FileOpener");
            this.FileOpener.RestoreDirectory = true;
            // 
            // DialogLocalAPILoader
            // 
            this.AcceptButton = this.ButtonOpen;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.ButtonCancel;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PictureWarning);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogLocalAPILoader";
            this.ShowInTaskbar = false;
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