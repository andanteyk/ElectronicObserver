namespace ElectronicObserver.Window.Dialog {
	partial class DialogShipGroupCSVOutput {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogShipGroupCSVOutput));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.RadioFormat_Data = new System.Windows.Forms.RadioButton();
            this.RadioFormat_User = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.RadioOutput_VisibleColumnOnly = new System.Windows.Forms.RadioButton();
            this.RadioOutput_All = new System.Windows.Forms.RadioButton();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ButtonOutputPathSearch = new System.Windows.Forms.Button();
            this.TextOutputPath = new System.Windows.Forms.TextBox();
            this.DialogSaveCSV = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.RadioFormat_Data);
            this.panel2.Controls.Add(this.RadioFormat_User);
            this.panel2.Name = "panel2";
            // 
            // RadioFormat_Data
            // 
            resources.ApplyResources(this.RadioFormat_Data, "RadioFormat_Data");
            this.RadioFormat_Data.Checked = true;
            this.RadioFormat_Data.Name = "RadioFormat_Data";
            this.RadioFormat_Data.TabStop = true;
            this.RadioFormat_Data.UseVisualStyleBackColor = true;
            // 
            // RadioFormat_User
            // 
            resources.ApplyResources(this.RadioFormat_User, "RadioFormat_User");
            this.RadioFormat_User.Name = "RadioFormat_User";
            this.RadioFormat_User.TabStop = true;
            this.RadioFormat_User.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.RadioOutput_VisibleColumnOnly);
            this.panel1.Controls.Add(this.RadioOutput_All);
            this.panel1.Name = "panel1";
            // 
            // RadioOutput_VisibleColumnOnly
            // 
            resources.ApplyResources(this.RadioOutput_VisibleColumnOnly, "RadioOutput_VisibleColumnOnly");
            this.RadioOutput_VisibleColumnOnly.Name = "RadioOutput_VisibleColumnOnly";
            this.RadioOutput_VisibleColumnOnly.TabStop = true;
            this.RadioOutput_VisibleColumnOnly.UseVisualStyleBackColor = true;
            // 
            // RadioOutput_All
            // 
            resources.ApplyResources(this.RadioOutput_All, "RadioOutput_All");
            this.RadioOutput_All.Checked = true;
            this.RadioOutput_All.Name = "RadioOutput_All";
            this.RadioOutput_All.TabStop = true;
            this.RadioOutput_All.UseVisualStyleBackColor = true;
            // 
            // ButtonOK
            // 
            resources.ApplyResources(this.ButtonOK, "ButtonOK");
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.UseVisualStyleBackColor = true;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // ButtonCancel
            // 
            resources.ApplyResources(this.ButtonCancel, "ButtonCancel");
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.ButtonOutputPathSearch);
            this.groupBox2.Controls.Add(this.TextOutputPath);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // ButtonOutputPathSearch
            // 
            resources.ApplyResources(this.ButtonOutputPathSearch, "ButtonOutputPathSearch");
            this.ButtonOutputPathSearch.Name = "ButtonOutputPathSearch";
            this.ButtonOutputPathSearch.UseVisualStyleBackColor = true;
            this.ButtonOutputPathSearch.Click += new System.EventHandler(this.ButtonOutputPathSearch_Click);
            // 
            // TextOutputPath
            // 
            resources.ApplyResources(this.TextOutputPath, "TextOutputPath");
            this.TextOutputPath.Name = "TextOutputPath";
            // 
            // DialogSaveCSV
            // 
            resources.ApplyResources(this.DialogSaveCSV, "DialogSaveCSV");
            // 
            // DialogShipGroupCSVOutput
            // 
            this.AcceptButton = this.ButtonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.ButtonCancel;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogShipGroupCSVOutput";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.DialogShipGroupCSVOutput_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.RadioButton RadioFormat_Data;
		private System.Windows.Forms.RadioButton RadioFormat_User;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.RadioButton RadioOutput_VisibleColumnOnly;
		private System.Windows.Forms.RadioButton RadioOutput_All;
		private System.Windows.Forms.Button ButtonOK;
		private System.Windows.Forms.Button ButtonCancel;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox TextOutputPath;
		private System.Windows.Forms.Button ButtonOutputPathSearch;
		private System.Windows.Forms.SaveFileDialog DialogSaveCSV;
	}
}