namespace ElectronicObserver.Window.Dialog {
	partial class DialogShipGroupColumnFilter {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogShipGroupColumnFilter));
            this.FilterList = new System.Windows.Forms.CheckedListBox();
            this.Description = new System.Windows.Forms.Label();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.AllCheck = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // FilterList
            // 
            resources.ApplyResources(this.FilterList, "FilterList");
            this.FilterList.CheckOnClick = true;
            this.FilterList.FormattingEnabled = true;
            this.FilterList.MultiColumn = true;
            this.FilterList.Name = "FilterList";
            this.FilterList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.FilterList_ItemCheck);
            // 
            // Description
            // 
            resources.ApplyResources(this.Description, "Description");
            this.Description.Name = "Description";
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
            // AllCheck
            // 
            resources.ApplyResources(this.AllCheck, "AllCheck");
            this.AllCheck.Name = "AllCheck";
            this.AllCheck.UseVisualStyleBackColor = true;
            this.AllCheck.CheckedChanged += new System.EventHandler(this.AllCheck_CheckedChanged);
            // 
            // DialogShipGroupColumnFilter
            // 
            this.AcceptButton = this.ButtonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.ButtonCancel;
            this.Controls.Add(this.AllCheck);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.Description);
            this.Controls.Add(this.FilterList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DialogShipGroupColumnFilter";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckedListBox FilterList;
		private System.Windows.Forms.Label Description;
		private System.Windows.Forms.Button ButtonOK;
		private System.Windows.Forms.Button ButtonCancel;
		private System.Windows.Forms.CheckBox AllCheck;
	}
}