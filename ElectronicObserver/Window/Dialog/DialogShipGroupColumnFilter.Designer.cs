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
			this.FilterList = new System.Windows.Forms.CheckedListBox();
			this.Description = new System.Windows.Forms.Label();
			this.ButtonOK = new System.Windows.Forms.Button();
			this.ButtonCancel = new System.Windows.Forms.Button();
			this.AllCheck = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// FilterList
			// 
			this.FilterList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.FilterList.CheckOnClick = true;
			this.FilterList.FormattingEnabled = true;
			this.FilterList.HorizontalScrollbar = true;
			this.FilterList.IntegralHeight = false;
			this.FilterList.Location = new System.Drawing.Point(16, 31);
			this.FilterList.MultiColumn = true;
			this.FilterList.Name = "FilterList";
			this.FilterList.Size = new System.Drawing.Size(436, 290);
			this.FilterList.TabIndex = 0;
			this.FilterList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.FilterList_ItemCheck);
			// 
			// Description
			// 
			this.Description.AutoSize = true;
			this.Description.Location = new System.Drawing.Point(13, 13);
			this.Description.Name = "Description";
			this.Description.Size = new System.Drawing.Size(165, 15);
			this.Description.TabIndex = 1;
			this.Description.Text = "请选中需要显示的列：";
			// 
			// ButtonOK
			// 
			this.ButtonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonOK.Location = new System.Drawing.Point(296, 327);
			this.ButtonOK.Name = "ButtonOK";
			this.ButtonOK.Size = new System.Drawing.Size(75, 23);
			this.ButtonOK.TabIndex = 2;
			this.ButtonOK.Text = "OK";
			this.ButtonOK.UseVisualStyleBackColor = true;
			this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
			// 
			// ButtonCancel
			// 
			this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.ButtonCancel.Location = new System.Drawing.Point(377, 327);
			this.ButtonCancel.Name = "ButtonCancel";
			this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
			this.ButtonCancel.TabIndex = 3;
			this.ButtonCancel.Text = "取消";
			this.ButtonCancel.UseVisualStyleBackColor = true;
			this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
			// 
			// AllCheck
			// 
			this.AllCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.AllCheck.AutoSize = true;
			this.AllCheck.Location = new System.Drawing.Point(16, 330);
			this.AllCheck.Name = "AllCheck";
			this.AllCheck.Size = new System.Drawing.Size(57, 19);
			this.AllCheck.TabIndex = 4;
			this.AllCheck.Text = "(全选)";
			this.AllCheck.UseVisualStyleBackColor = true;
			this.AllCheck.CheckedChanged += new System.EventHandler(this.AllCheck_CheckedChanged);
			// 
			// DialogShipGroupColumnFilter
			// 
			this.AcceptButton = this.ButtonOK;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.ButtonCancel;
			this.ClientSize = new System.Drawing.Size(464, 362);
			this.Controls.Add(this.AllCheck);
			this.Controls.Add(this.ButtonCancel);
			this.Controls.Add(this.ButtonOK);
			this.Controls.Add(this.Description);
			this.Controls.Add(this.FilterList);
			this.Font = Program.Window_Font;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "DialogShipGroupColumnFilter";
			this.Text = "列过滤";
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