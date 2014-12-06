namespace ElectronicObserver.Window {
	partial class FormQuest {
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.QuestView = new System.Windows.Forms.DataGridView();
			this.QuestView_State = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.QuestView_Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.QuestView_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.QuestView_Progress = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.QuestView)).BeginInit();
			this.SuspendLayout();
			// 
			// QuestView
			// 
			this.QuestView.AllowUserToAddRows = false;
			this.QuestView.AllowUserToDeleteRows = false;
			this.QuestView.AllowUserToResizeRows = false;
			this.QuestView.BackgroundColor = System.Drawing.SystemColors.Control;
			this.QuestView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.QuestView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.QuestView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.QuestView_State,
            this.QuestView_Type,
            this.QuestView_Name,
            this.QuestView_Progress});
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.QuestView.DefaultCellStyle = dataGridViewCellStyle2;
			this.QuestView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.QuestView.Location = new System.Drawing.Point(0, 0);
			this.QuestView.MultiSelect = false;
			this.QuestView.Name = "QuestView";
			this.QuestView.ReadOnly = true;
			this.QuestView.RowHeadersVisible = false;
			this.QuestView.RowTemplate.Height = 21;
			this.QuestView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.QuestView.Size = new System.Drawing.Size(300, 200);
			this.QuestView.TabIndex = 0;
			// 
			// QuestView_State
			// 
			this.QuestView_State.FalseValue = "";
			this.QuestView_State.HeaderText = "";
			this.QuestView_State.IndeterminateValue = "";
			this.QuestView_State.Name = "QuestView_State";
			this.QuestView_State.ReadOnly = true;
			this.QuestView_State.ThreeState = true;
			this.QuestView_State.TrueValue = "";
			this.QuestView_State.Width = 24;
			// 
			// QuestView_Type
			// 
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.QuestView_Type.DefaultCellStyle = dataGridViewCellStyle1;
			this.QuestView_Type.HeaderText = "種";
			this.QuestView_Type.Name = "QuestView_Type";
			this.QuestView_Type.ReadOnly = true;
			this.QuestView_Type.Width = 24;
			// 
			// QuestView_Name
			// 
			this.QuestView_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.QuestView_Name.FillWeight = 200F;
			this.QuestView_Name.HeaderText = "任務名";
			this.QuestView_Name.Name = "QuestView_Name";
			this.QuestView_Name.ReadOnly = true;
			// 
			// QuestView_Progress
			// 
			this.QuestView_Progress.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.QuestView_Progress.HeaderText = "進捗";
			this.QuestView_Progress.Name = "QuestView_Progress";
			this.QuestView_Progress.ReadOnly = true;
			// 
			// FormQuest
			// 
			this.AutoHidePortion = 150D;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(300, 200);
			this.Controls.Add(this.QuestView);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.HideOnClose = true;
			this.Name = "FormQuest";
			this.Text = "任務";
			this.Load += new System.EventHandler(this.FormQuest_Load);
			((System.ComponentModel.ISupportInitialize)(this.QuestView)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView QuestView;
		private System.Windows.Forms.DataGridViewCheckBoxColumn QuestView_State;
		private System.Windows.Forms.DataGridViewTextBoxColumn QuestView_Type;
		private System.Windows.Forms.DataGridViewTextBoxColumn QuestView_Name;
		private System.Windows.Forms.DataGridViewTextBoxColumn QuestView_Progress;
	}
}