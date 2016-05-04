namespace ElectronicObserver.Window.Dialog {
	partial class DialogAlbumShipParameter {
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
			this.ParameterView = new System.Windows.Forms.DataGridView();
			this.ParameterView_Key = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ParameterView_Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ButtonOK = new System.Windows.Forms.Button();
			this.ButtonCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.ParameterView)).BeginInit();
			this.SuspendLayout();
			// 
			// ParameterView
			// 
			this.ParameterView.AllowUserToAddRows = false;
			this.ParameterView.AllowUserToDeleteRows = false;
			this.ParameterView.AllowUserToResizeColumns = false;
			this.ParameterView.AllowUserToResizeRows = false;
			this.ParameterView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ParameterView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.ParameterView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ParameterView_Key,
            this.ParameterView_Value});
			this.ParameterView.Location = new System.Drawing.Point(13, 13);
			this.ParameterView.Name = "ParameterView";
			this.ParameterView.RowHeadersVisible = false;
			this.ParameterView.RowTemplate.Height = 21;
			this.ParameterView.Size = new System.Drawing.Size(359, 507);
			this.ParameterView.TabIndex = 0;
			// 
			// ParameterView_Key
			// 
			this.ParameterView_Key.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.ParameterView_Key.HeaderText = "項目";
			this.ParameterView_Key.Name = "ParameterView_Key";
			this.ParameterView_Key.ReadOnly = true;
			this.ParameterView_Key.Width = 56;
			// 
			// ParameterView_Value
			// 
			this.ParameterView_Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.ParameterView_Value.HeaderText = "値";
			this.ParameterView_Value.Name = "ParameterView_Value";
			// 
			// ButtonOK
			// 
			this.ButtonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonOK.Location = new System.Drawing.Point(216, 526);
			this.ButtonOK.Name = "ButtonOK";
			this.ButtonOK.Size = new System.Drawing.Size(75, 23);
			this.ButtonOK.TabIndex = 1;
			this.ButtonOK.Text = "OK";
			this.ButtonOK.UseVisualStyleBackColor = true;
			this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
			// 
			// ButtonCancel
			// 
			this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonCancel.Location = new System.Drawing.Point(297, 526);
			this.ButtonCancel.Name = "ButtonCancel";
			this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
			this.ButtonCancel.TabIndex = 2;
			this.ButtonCancel.Text = "キャンセル";
			this.ButtonCancel.UseVisualStyleBackColor = true;
			this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 523);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(191, 30);
			this.label1.TabIndex = 3;
			this.label1.Text = "※意味が分かる方のみご利用ください。\r\n一部適用されない項目もあります。";
			// 
			// DialogAlbumShipParameter
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(384, 561);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.ButtonCancel);
			this.Controls.Add(this.ButtonOK);
			this.Controls.Add(this.ParameterView);
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.Name = "DialogAlbumShipParameter";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "艦船パラメータの編集";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogAlbumShipParameter_FormClosed);
			this.Load += new System.EventHandler(this.DialogAlbumShipParameter_Load);
			((System.ComponentModel.ISupportInitialize)(this.ParameterView)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView ParameterView;
		private System.Windows.Forms.DataGridViewTextBoxColumn ParameterView_Key;
		private System.Windows.Forms.DataGridViewTextBoxColumn ParameterView_Value;
		private System.Windows.Forms.Button ButtonOK;
		private System.Windows.Forms.Button ButtonCancel;
		private System.Windows.Forms.Label label1;
	}
}