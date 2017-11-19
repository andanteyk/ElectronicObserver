namespace ElectronicObserver.Window.Dialog
{
	partial class DialogShipGroupSortOrder
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
			this.AutoSortFlag = new System.Windows.Forms.CheckBox();
			this.EnabledView = new System.Windows.Forms.DataGridView();
			this.EnabledView_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.EnabledView_SortDirection = new System.Windows.Forms.DataGridViewButtonColumn();
			this.EnabledView_Up = new System.Windows.Forms.DataGridViewButtonColumn();
			this.EnabledView_Down = new System.Windows.Forms.DataGridViewButtonColumn();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.ButtonDown = new System.Windows.Forms.Button();
			this.ButtonUp = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.ButtonRight = new System.Windows.Forms.Button();
			this.ButtonLeft = new System.Windows.Forms.Button();
			this.panel2 = new System.Windows.Forms.Panel();
			this.ButtonRightAll = new System.Windows.Forms.Button();
			this.ButtonLeftAll = new System.Windows.Forms.Button();
			this.DisabledView = new System.Windows.Forms.DataGridView();
			this.DisabledView_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ButtonOK = new System.Windows.Forms.Button();
			this.ButtonCancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.EnabledView)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.DisabledView)).BeginInit();
			this.SuspendLayout();
			// 
			// AutoSortFlag
			// 
			this.AutoSortFlag.AutoSize = true;
			this.AutoSortFlag.Location = new System.Drawing.Point(12, 12);
			this.AutoSortFlag.Name = "AutoSortFlag";
			this.AutoSortFlag.Size = new System.Drawing.Size(96, 19);
			this.AutoSortFlag.TabIndex = 0;
			this.AutoSortFlag.Text = "自動ソートする";
			this.AutoSortFlag.UseVisualStyleBackColor = true;
			// 
			// EnabledView
			// 
			this.EnabledView.AllowUserToAddRows = false;
			this.EnabledView.AllowUserToDeleteRows = false;
			this.EnabledView.AllowUserToResizeColumns = false;
			this.EnabledView.AllowUserToResizeRows = false;
			this.EnabledView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.EnabledView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.EnabledView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
			this.EnabledView_Name,
			this.EnabledView_SortDirection,
			this.EnabledView_Up,
			this.EnabledView_Down});
			this.EnabledView.Location = new System.Drawing.Point(3, 3);
			this.EnabledView.Name = "EnabledView";
			this.EnabledView.RowHeadersVisible = false;
			this.tableLayoutPanel1.SetRowSpan(this.EnabledView, 3);
			this.EnabledView.RowTemplate.Height = 21;
			this.EnabledView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.EnabledView.ShowCellToolTips = false;
			this.EnabledView.Size = new System.Drawing.Size(254, 357);
			this.EnabledView.TabIndex = 1;
			this.EnabledView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.EnabledView_CellContentClick);
			this.EnabledView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.EnabledView_CellFormatting);
			// 
			// EnabledView_Name
			// 
			this.EnabledView_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.EnabledView_Name.HeaderText = "列名";
			this.EnabledView_Name.Name = "EnabledView_Name";
			this.EnabledView_Name.ReadOnly = true;
			this.EnabledView_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// EnabledView_SortDirection
			// 
			this.EnabledView_SortDirection.HeaderText = "順";
			this.EnabledView_SortDirection.Name = "EnabledView_SortDirection";
			this.EnabledView_SortDirection.Width = 40;
			// 
			// EnabledView_Up
			// 
			this.EnabledView_Up.HeaderText = "↑";
			this.EnabledView_Up.Name = "EnabledView_Up";
			this.EnabledView_Up.Width = 20;
			// 
			// EnabledView_Down
			// 
			this.EnabledView_Down.HeaderText = "↓";
			this.EnabledView_Down.Name = "EnabledView_Down";
			this.EnabledView_Down.Width = 20;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.panel3, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.EnabledView, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.DisabledView, 2, 0);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 37);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(600, 363);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// panel3
			// 
			this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.panel3.Controls.Add(this.ButtonDown);
			this.panel3.Controls.Add(this.ButtonUp);
			this.panel3.Location = new System.Drawing.Point(260, 0);
			this.panel3.Margin = new System.Windows.Forms.Padding(0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(80, 100);
			this.panel3.TabIndex = 7;
			// 
			// ButtonDown
			// 
			this.ButtonDown.Location = new System.Drawing.Point(3, 32);
			this.ButtonDown.Name = "ButtonDown";
			this.ButtonDown.Size = new System.Drawing.Size(75, 23);
			this.ButtonDown.TabIndex = 1;
			this.ButtonDown.Text = "↓";
			this.ButtonDown.UseVisualStyleBackColor = true;
			this.ButtonDown.Click += new System.EventHandler(this.ButtonDown_Click);
			// 
			// ButtonUp
			// 
			this.ButtonUp.Location = new System.Drawing.Point(3, 3);
			this.ButtonUp.Name = "ButtonUp";
			this.ButtonUp.Size = new System.Drawing.Size(75, 23);
			this.ButtonUp.TabIndex = 0;
			this.ButtonUp.Text = "↑";
			this.ButtonUp.UseVisualStyleBackColor = true;
			this.ButtonUp.Click += new System.EventHandler(this.ButtonUp_Click);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.ButtonRight);
			this.panel1.Controls.Add(this.ButtonLeft);
			this.panel1.Location = new System.Drawing.Point(260, 151);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(80, 60);
			this.panel1.TabIndex = 2;
			// 
			// ButtonRight
			// 
			this.ButtonRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonRight.Location = new System.Drawing.Point(3, 32);
			this.ButtonRight.Name = "ButtonRight";
			this.ButtonRight.Size = new System.Drawing.Size(75, 23);
			this.ButtonRight.TabIndex = 5;
			this.ButtonRight.Text = ">>";
			this.ButtonRight.UseVisualStyleBackColor = true;
			this.ButtonRight.Click += new System.EventHandler(this.ButtonRight_Click);
			// 
			// ButtonLeft
			// 
			this.ButtonLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonLeft.Location = new System.Drawing.Point(3, 3);
			this.ButtonLeft.Name = "ButtonLeft";
			this.ButtonLeft.Size = new System.Drawing.Size(74, 23);
			this.ButtonLeft.TabIndex = 4;
			this.ButtonLeft.Text = "<<";
			this.ButtonLeft.UseVisualStyleBackColor = true;
			this.ButtonLeft.Click += new System.EventHandler(this.ButtonLeft_Click);
			// 
			// panel2
			// 
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.panel2.Controls.Add(this.ButtonRightAll);
			this.panel2.Controls.Add(this.ButtonLeftAll);
			this.panel2.Location = new System.Drawing.Point(260, 263);
			this.panel2.Margin = new System.Windows.Forms.Padding(0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(80, 100);
			this.panel2.TabIndex = 3;
			// 
			// ButtonRightAll
			// 
			this.ButtonRightAll.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.ButtonRightAll.Location = new System.Drawing.Point(3, 74);
			this.ButtonRightAll.Name = "ButtonRightAll";
			this.ButtonRightAll.Size = new System.Drawing.Size(74, 23);
			this.ButtonRightAll.TabIndex = 6;
			this.ButtonRightAll.Text = "全て >>";
			this.ButtonRightAll.UseVisualStyleBackColor = true;
			this.ButtonRightAll.Click += new System.EventHandler(this.ButtonRightAll_Click);
			// 
			// ButtonLeftAll
			// 
			this.ButtonLeftAll.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.ButtonLeftAll.Location = new System.Drawing.Point(3, 45);
			this.ButtonLeftAll.Name = "ButtonLeftAll";
			this.ButtonLeftAll.Size = new System.Drawing.Size(74, 23);
			this.ButtonLeftAll.TabIndex = 5;
			this.ButtonLeftAll.Text = "<< 全て";
			this.ButtonLeftAll.UseVisualStyleBackColor = true;
			this.ButtonLeftAll.Click += new System.EventHandler(this.ButtonLeftAll_Click);
			// 
			// DisabledView
			// 
			this.DisabledView.AllowUserToAddRows = false;
			this.DisabledView.AllowUserToDeleteRows = false;
			this.DisabledView.AllowUserToResizeColumns = false;
			this.DisabledView.AllowUserToResizeRows = false;
			this.DisabledView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.DisabledView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DisabledView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
			this.DisabledView_Name});
			this.DisabledView.Location = new System.Drawing.Point(343, 3);
			this.DisabledView.Name = "DisabledView";
			this.DisabledView.RowHeadersVisible = false;
			this.tableLayoutPanel1.SetRowSpan(this.DisabledView, 3);
			this.DisabledView.RowTemplate.Height = 21;
			this.DisabledView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.DisabledView.ShowCellToolTips = false;
			this.DisabledView.Size = new System.Drawing.Size(254, 357);
			this.DisabledView.TabIndex = 8;
			this.DisabledView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.DisabledView_SortCompare);
			// 
			// DisabledView_Name
			// 
			this.DisabledView_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.DisabledView_Name.HeaderText = "列名";
			this.DisabledView_Name.Name = "DisabledView_Name";
			this.DisabledView_Name.ReadOnly = true;
			// 
			// ButtonOK
			// 
			this.ButtonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonOK.Location = new System.Drawing.Point(456, 406);
			this.ButtonOK.Name = "ButtonOK";
			this.ButtonOK.Size = new System.Drawing.Size(75, 23);
			this.ButtonOK.TabIndex = 3;
			this.ButtonOK.Text = "OK";
			this.ButtonOK.UseVisualStyleBackColor = true;
			this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
			// 
			// ButtonCancel
			// 
			this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonCancel.Location = new System.Drawing.Point(537, 406);
			this.ButtonCancel.Name = "ButtonCancel";
			this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
			this.ButtonCancel.TabIndex = 4;
			this.ButtonCancel.Text = "キャンセル";
			this.ButtonCancel.UseVisualStyleBackColor = true;
			this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
			// 
			// DialogShipGroupSortOrder
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(624, 441);
			this.Controls.Add(this.ButtonCancel);
			this.Controls.Add(this.ButtonOK);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.AutoSortFlag);
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogShipGroupSortOrder";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "自動ソート順";
			this.Load += new System.EventHandler(this.DialogShipGroupSortOrder_Load);
			((System.ComponentModel.ISupportInitialize)(this.EnabledView)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.DisabledView)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox AutoSortFlag;
		private System.Windows.Forms.DataGridView EnabledView;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button ButtonOK;
		private System.Windows.Forms.Button ButtonCancel;
		private System.Windows.Forms.Button ButtonLeft;
		private System.Windows.Forms.Button ButtonRight;
		private System.Windows.Forms.Button ButtonRightAll;
		private System.Windows.Forms.Button ButtonLeftAll;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Button ButtonDown;
		private System.Windows.Forms.Button ButtonUp;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.DataGridView DisabledView;
		private System.Windows.Forms.DataGridViewTextBoxColumn EnabledView_Name;
		private System.Windows.Forms.DataGridViewButtonColumn EnabledView_SortDirection;
		private System.Windows.Forms.DataGridViewButtonColumn EnabledView_Up;
		private System.Windows.Forms.DataGridViewButtonColumn EnabledView_Down;
		private System.Windows.Forms.DataGridViewTextBoxColumn DisabledView_Name;
	}
}