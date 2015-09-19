namespace ElectronicObserver.Window.Dialog {
	partial class DialogShipGroupFilter {
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainer3 = new System.Windows.Forms.SplitContainer();
			this.ExpressionView = new System.Windows.Forms.DataGridView();
			this.ExpressionView_Enabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.ExpressionView_ExternalAndOr = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.ExpressionView_Inverse = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.ExpressionView_InternalAndOr = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.ExpressionView_Expression = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ExpressionView_Up = new System.Windows.Forms.DataGridViewButtonColumn();
			this.ExpressionView_Down = new System.Windows.Forms.DataGridViewButtonColumn();
			this.ButtonCancel = new System.Windows.Forms.Button();
			this.ButtonOK = new System.Windows.Forms.Button();
			this.Expression_Delete = new System.Windows.Forms.Button();
			this.Expression_Add = new System.Windows.Forms.Button();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.ExpressionDetailView = new System.Windows.Forms.DataGridView();
			this.ExpressionDetailView_Enabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.ExpressionDetailView_LeftOperand = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ExpressionDetailView_RightOperand = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ExpressionDetailView_Operator = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ExpressionDetail_Delete = new System.Windows.Forms.Button();
			this.ExpressionDetail_Edit = new System.Windows.Forms.Button();
			this.ExpressionDetail_Add = new System.Windows.Forms.Button();
			this.GroupExpression = new System.Windows.Forms.GroupBox();
			this.Description = new System.Windows.Forms.Label();
			this.RightOperand_ComboBox = new System.Windows.Forms.ComboBox();
			this.RightOperand_NumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.Operator = new System.Windows.Forms.ComboBox();
			this.RightOperand_TextBox = new System.Windows.Forms.TextBox();
			this.LeftOperand = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ExpressionView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ExpressionDetailView)).BeginInit();
			this.GroupExpression.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.RightOperand_NumericUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Size = new System.Drawing.Size(624, 441);
			this.splitContainer1.SplitterDistance = 240;
			this.splitContainer1.TabIndex = 0;
			// 
			// splitContainer3
			// 
			this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer3.IsSplitterFixed = true;
			this.splitContainer3.Location = new System.Drawing.Point(0, 0);
			this.splitContainer3.Name = "splitContainer3";
			this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer3.Panel1
			// 
			this.splitContainer3.Panel1.Controls.Add(this.ExpressionView);
			// 
			// splitContainer3.Panel2
			// 
			this.splitContainer3.Panel2.Controls.Add(this.ButtonCancel);
			this.splitContainer3.Panel2.Controls.Add(this.ButtonOK);
			this.splitContainer3.Panel2.Controls.Add(this.Expression_Delete);
			this.splitContainer3.Panel2.Controls.Add(this.Expression_Add);
			this.splitContainer3.Size = new System.Drawing.Size(240, 441);
			this.splitContainer3.SplitterDistance = 320;
			this.splitContainer3.TabIndex = 0;
			// 
			// ExpressionView
			// 
			this.ExpressionView.AllowUserToAddRows = false;
			this.ExpressionView.AllowUserToDeleteRows = false;
			this.ExpressionView.AllowUserToResizeColumns = false;
			this.ExpressionView.AllowUserToResizeRows = false;
			this.ExpressionView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.ExpressionView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ExpressionView_Enabled,
            this.ExpressionView_ExternalAndOr,
            this.ExpressionView_Inverse,
            this.ExpressionView_InternalAndOr,
            this.ExpressionView_Expression,
            this.ExpressionView_Up,
            this.ExpressionView_Down});
			this.ExpressionView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ExpressionView.Location = new System.Drawing.Point(0, 0);
			this.ExpressionView.MultiSelect = false;
			this.ExpressionView.Name = "ExpressionView";
			this.ExpressionView.RowHeadersVisible = false;
			this.ExpressionView.RowTemplate.Height = 21;
			this.ExpressionView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.ExpressionView.Size = new System.Drawing.Size(240, 320);
			this.ExpressionView.TabIndex = 0;
			this.ExpressionView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ExpressionView_CellClick);
			this.ExpressionView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ExpressionView_CellContentClick);
			this.ExpressionView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.ExpressionView_CellValueChanged);
			this.ExpressionView.CurrentCellDirtyStateChanged += new System.EventHandler(this.ExpressionView_CurrentCellDirtyStateChanged);
			this.ExpressionView.SelectionChanged += new System.EventHandler(this.ExpressionView_SelectionChanged);
			// 
			// ExpressionView_Enabled
			// 
			this.ExpressionView_Enabled.HeaderText = "○";
			this.ExpressionView_Enabled.Name = "ExpressionView_Enabled";
			this.ExpressionView_Enabled.ToolTipText = "有効/無効";
			this.ExpressionView_Enabled.Width = 20;
			// 
			// ExpressionView_ExternalAndOr
			// 
			this.ExpressionView_ExternalAndOr.HeaderText = "";
			this.ExpressionView_ExternalAndOr.Items.AddRange(new object[] {
            "And",
            "Or"});
			this.ExpressionView_ExternalAndOr.Name = "ExpressionView_ExternalAndOr";
			this.ExpressionView_ExternalAndOr.Width = 50;
			// 
			// ExpressionView_Inverse
			// 
			this.ExpressionView_Inverse.HeaderText = "Inv";
			this.ExpressionView_Inverse.Name = "ExpressionView_Inverse";
			this.ExpressionView_Inverse.ToolTipText = "条件を反転するか";
			this.ExpressionView_Inverse.Width = 20;
			// 
			// ExpressionView_InternalAndOr
			// 
			this.ExpressionView_InternalAndOr.HeaderText = "";
			this.ExpressionView_InternalAndOr.Items.AddRange(new object[] {
            "And",
            "Or"});
			this.ExpressionView_InternalAndOr.Name = "ExpressionView_InternalAndOr";
			this.ExpressionView_InternalAndOr.Width = 50;
			// 
			// ExpressionView_Expression
			// 
			this.ExpressionView_Expression.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.ExpressionView_Expression.HeaderText = "式";
			this.ExpressionView_Expression.Name = "ExpressionView_Expression";
			this.ExpressionView_Expression.ReadOnly = true;
			this.ExpressionView_Expression.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// ExpressionView_Up
			// 
			this.ExpressionView_Up.HeaderText = "↑";
			this.ExpressionView_Up.Name = "ExpressionView_Up";
			this.ExpressionView_Up.ToolTipText = "上へ";
			this.ExpressionView_Up.Width = 20;
			// 
			// ExpressionView_Down
			// 
			this.ExpressionView_Down.HeaderText = "↓";
			this.ExpressionView_Down.Name = "ExpressionView_Down";
			this.ExpressionView_Down.ToolTipText = "下へ";
			this.ExpressionView_Down.Width = 20;
			// 
			// ButtonCancel
			// 
			this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ButtonCancel.Location = new System.Drawing.Point(84, 91);
			this.ButtonCancel.Name = "ButtonCancel";
			this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
			this.ButtonCancel.TabIndex = 9;
			this.ButtonCancel.Text = "キャンセル";
			this.ButtonCancel.UseVisualStyleBackColor = true;
			this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
			// 
			// ButtonOK
			// 
			this.ButtonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ButtonOK.Location = new System.Drawing.Point(3, 91);
			this.ButtonOK.Name = "ButtonOK";
			this.ButtonOK.Size = new System.Drawing.Size(75, 23);
			this.ButtonOK.TabIndex = 8;
			this.ButtonOK.Text = "OK";
			this.ButtonOK.UseVisualStyleBackColor = true;
			this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
			// 
			// Expression_Delete
			// 
			this.Expression_Delete.Location = new System.Drawing.Point(84, 5);
			this.Expression_Delete.Name = "Expression_Delete";
			this.Expression_Delete.Size = new System.Drawing.Size(75, 23);
			this.Expression_Delete.TabIndex = 7;
			this.Expression_Delete.Text = "削除";
			this.Expression_Delete.UseVisualStyleBackColor = true;
			this.Expression_Delete.Click += new System.EventHandler(this.Expression_Delete_Click);
			// 
			// Expression_Add
			// 
			this.Expression_Add.Location = new System.Drawing.Point(3, 5);
			this.Expression_Add.Name = "Expression_Add";
			this.Expression_Add.Size = new System.Drawing.Size(75, 23);
			this.Expression_Add.TabIndex = 5;
			this.Expression_Add.Text = "追加";
			this.Expression_Add.UseVisualStyleBackColor = true;
			this.Expression_Add.Click += new System.EventHandler(this.Expression_Add_Click);
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer2.IsSplitterFixed = true;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.ExpressionDetailView);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.ExpressionDetail_Delete);
			this.splitContainer2.Panel2.Controls.Add(this.ExpressionDetail_Edit);
			this.splitContainer2.Panel2.Controls.Add(this.ExpressionDetail_Add);
			this.splitContainer2.Panel2.Controls.Add(this.GroupExpression);
			this.splitContainer2.Size = new System.Drawing.Size(380, 441);
			this.splitContainer2.SplitterDistance = 320;
			this.splitContainer2.TabIndex = 0;
			// 
			// ExpressionDetailView
			// 
			this.ExpressionDetailView.AllowUserToAddRows = false;
			this.ExpressionDetailView.AllowUserToDeleteRows = false;
			this.ExpressionDetailView.AllowUserToResizeColumns = false;
			this.ExpressionDetailView.AllowUserToResizeRows = false;
			this.ExpressionDetailView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.ExpressionDetailView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ExpressionDetailView_Enabled,
            this.ExpressionDetailView_LeftOperand,
            this.ExpressionDetailView_RightOperand,
            this.ExpressionDetailView_Operator});
			this.ExpressionDetailView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ExpressionDetailView.Location = new System.Drawing.Point(0, 0);
			this.ExpressionDetailView.MultiSelect = false;
			this.ExpressionDetailView.Name = "ExpressionDetailView";
			this.ExpressionDetailView.RowHeadersVisible = false;
			this.ExpressionDetailView.RowTemplate.Height = 21;
			this.ExpressionDetailView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.ExpressionDetailView.Size = new System.Drawing.Size(380, 320);
			this.ExpressionDetailView.TabIndex = 0;
			this.ExpressionDetailView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.ExpressionDetailView_CellFormatting);
			this.ExpressionDetailView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.ExpressionDetailView_CellValueChanged);
			this.ExpressionDetailView.SelectionChanged += new System.EventHandler(this.ExpressionDetailView_SelectionChanged);
			// 
			// ExpressionDetailView_Enabled
			// 
			this.ExpressionDetailView_Enabled.HeaderText = "○";
			this.ExpressionDetailView_Enabled.Name = "ExpressionDetailView_Enabled";
			this.ExpressionDetailView_Enabled.ToolTipText = "有効/無効";
			this.ExpressionDetailView_Enabled.Width = 20;
			// 
			// ExpressionDetailView_LeftOperand
			// 
			this.ExpressionDetailView_LeftOperand.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.ExpressionDetailView_LeftOperand.HeaderText = "左辺";
			this.ExpressionDetailView_LeftOperand.Name = "ExpressionDetailView_LeftOperand";
			this.ExpressionDetailView_LeftOperand.ReadOnly = true;
			this.ExpressionDetailView_LeftOperand.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.ExpressionDetailView_LeftOperand.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// ExpressionDetailView_RightOperand
			// 
			this.ExpressionDetailView_RightOperand.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.ExpressionDetailView_RightOperand.HeaderText = "右辺";
			this.ExpressionDetailView_RightOperand.Name = "ExpressionDetailView_RightOperand";
			this.ExpressionDetailView_RightOperand.ReadOnly = true;
			this.ExpressionDetailView_RightOperand.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// ExpressionDetailView_Operator
			// 
			this.ExpressionDetailView_Operator.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.ExpressionDetailView_Operator.HeaderText = "条件";
			this.ExpressionDetailView_Operator.Name = "ExpressionDetailView_Operator";
			this.ExpressionDetailView_Operator.ReadOnly = true;
			this.ExpressionDetailView_Operator.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.ExpressionDetailView_Operator.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// ExpressionDetail_Delete
			// 
			this.ExpressionDetail_Delete.Location = new System.Drawing.Point(162, 82);
			this.ExpressionDetail_Delete.Name = "ExpressionDetail_Delete";
			this.ExpressionDetail_Delete.Size = new System.Drawing.Size(75, 23);
			this.ExpressionDetail_Delete.TabIndex = 4;
			this.ExpressionDetail_Delete.Text = "削除";
			this.ExpressionDetail_Delete.UseVisualStyleBackColor = true;
			this.ExpressionDetail_Delete.Click += new System.EventHandler(this.ExpressionDetail_Delete_Click);
			// 
			// ExpressionDetail_Edit
			// 
			this.ExpressionDetail_Edit.Location = new System.Drawing.Point(81, 82);
			this.ExpressionDetail_Edit.Name = "ExpressionDetail_Edit";
			this.ExpressionDetail_Edit.Size = new System.Drawing.Size(75, 23);
			this.ExpressionDetail_Edit.TabIndex = 3;
			this.ExpressionDetail_Edit.Text = "上書き";
			this.ExpressionDetail_Edit.UseVisualStyleBackColor = true;
			this.ExpressionDetail_Edit.Click += new System.EventHandler(this.ExpressionDetail_Edit_Click);
			// 
			// ExpressionDetail_Add
			// 
			this.ExpressionDetail_Add.Location = new System.Drawing.Point(0, 82);
			this.ExpressionDetail_Add.Name = "ExpressionDetail_Add";
			this.ExpressionDetail_Add.Size = new System.Drawing.Size(75, 23);
			this.ExpressionDetail_Add.TabIndex = 1;
			this.ExpressionDetail_Add.Text = "追加";
			this.ExpressionDetail_Add.UseVisualStyleBackColor = true;
			this.ExpressionDetail_Add.Click += new System.EventHandler(this.ExpressionDetail_Add_Click);
			// 
			// GroupExpression
			// 
			this.GroupExpression.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.GroupExpression.Controls.Add(this.Description);
			this.GroupExpression.Controls.Add(this.RightOperand_ComboBox);
			this.GroupExpression.Controls.Add(this.RightOperand_NumericUpDown);
			this.GroupExpression.Controls.Add(this.Operator);
			this.GroupExpression.Controls.Add(this.RightOperand_TextBox);
			this.GroupExpression.Controls.Add(this.LeftOperand);
			this.GroupExpression.Location = new System.Drawing.Point(3, 5);
			this.GroupExpression.Name = "GroupExpression";
			this.GroupExpression.Size = new System.Drawing.Size(374, 71);
			this.GroupExpression.TabIndex = 0;
			this.GroupExpression.TabStop = false;
			this.GroupExpression.Text = "Expression";
			// 
			// Description
			// 
			this.Description.AutoSize = true;
			this.Description.Location = new System.Drawing.Point(130, 48);
			this.Description.Name = "Description";
			this.Description.Size = new System.Drawing.Size(41, 15);
			this.Description.TabIndex = 5;
			this.Description.Text = "(説明)";
			// 
			// RightOperand_ComboBox
			// 
			this.RightOperand_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.RightOperand_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.RightOperand_ComboBox.FormattingEnabled = true;
			this.RightOperand_ComboBox.Location = new System.Drawing.Point(133, 22);
			this.RightOperand_ComboBox.Name = "RightOperand_ComboBox";
			this.RightOperand_ComboBox.Size = new System.Drawing.Size(108, 23);
			this.RightOperand_ComboBox.TabIndex = 4;
			// 
			// RightOperand_NumericUpDown
			// 
			this.RightOperand_NumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.RightOperand_NumericUpDown.Location = new System.Drawing.Point(133, 22);
			this.RightOperand_NumericUpDown.Name = "RightOperand_NumericUpDown";
			this.RightOperand_NumericUpDown.Size = new System.Drawing.Size(108, 23);
			this.RightOperand_NumericUpDown.TabIndex = 3;
			this.RightOperand_NumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.RightOperand_NumericUpDown.ValueChanged += new System.EventHandler(this.RightOperand_NumericUpDown_ValueChanged);
			// 
			// Operator
			// 
			this.Operator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.Operator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.Operator.FormattingEnabled = true;
			this.Operator.Location = new System.Drawing.Point(247, 22);
			this.Operator.Name = "Operator";
			this.Operator.Size = new System.Drawing.Size(121, 23);
			this.Operator.TabIndex = 2;
			// 
			// RightOperand_TextBox
			// 
			this.RightOperand_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.RightOperand_TextBox.Location = new System.Drawing.Point(133, 22);
			this.RightOperand_TextBox.Name = "RightOperand_TextBox";
			this.RightOperand_TextBox.Size = new System.Drawing.Size(108, 23);
			this.RightOperand_TextBox.TabIndex = 1;
			// 
			// LeftOperand
			// 
			this.LeftOperand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.LeftOperand.FormattingEnabled = true;
			this.LeftOperand.Location = new System.Drawing.Point(6, 22);
			this.LeftOperand.Name = "LeftOperand";
			this.LeftOperand.Size = new System.Drawing.Size(121, 23);
			this.LeftOperand.TabIndex = 0;
			this.LeftOperand.SelectedValueChanged += new System.EventHandler(this.LeftOperand_SelectedValueChanged);
			// 
			// DialogShipGroupFilter
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(624, 441);
			this.Controls.Add(this.splitContainer1);
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.Name = "DialogShipGroupFilter";
			this.Text = "フィルタ";
			this.Load += new System.EventHandler(this.DialogShipGroupFilter_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer3.Panel1.ResumeLayout(false);
			this.splitContainer3.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
			this.splitContainer3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ExpressionView)).EndInit();
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ExpressionDetailView)).EndInit();
			this.GroupExpression.ResumeLayout(false);
			this.GroupExpression.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.RightOperand_NumericUpDown)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.DataGridView ExpressionView;
		private System.Windows.Forms.DataGridView ExpressionDetailView;
		private System.Windows.Forms.SplitContainer splitContainer3;
		private System.Windows.Forms.Button Expression_Delete;
		private System.Windows.Forms.Button Expression_Add;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.Button ExpressionDetail_Delete;
		private System.Windows.Forms.Button ExpressionDetail_Edit;
		private System.Windows.Forms.Button ExpressionDetail_Add;
		private System.Windows.Forms.GroupBox GroupExpression;
		private System.Windows.Forms.ComboBox RightOperand_ComboBox;
		private System.Windows.Forms.NumericUpDown RightOperand_NumericUpDown;
		private System.Windows.Forms.ComboBox Operator;
		private System.Windows.Forms.TextBox RightOperand_TextBox;
		private System.Windows.Forms.ComboBox LeftOperand;
		private System.Windows.Forms.Label Description;
		private System.Windows.Forms.Button ButtonCancel;
		private System.Windows.Forms.Button ButtonOK;
		private System.Windows.Forms.DataGridViewCheckBoxColumn ExpressionView_Enabled;
		private System.Windows.Forms.DataGridViewComboBoxColumn ExpressionView_ExternalAndOr;
		private System.Windows.Forms.DataGridViewCheckBoxColumn ExpressionView_Inverse;
		private System.Windows.Forms.DataGridViewComboBoxColumn ExpressionView_InternalAndOr;
		private System.Windows.Forms.DataGridViewTextBoxColumn ExpressionView_Expression;
		private System.Windows.Forms.DataGridViewButtonColumn ExpressionView_Up;
		private System.Windows.Forms.DataGridViewButtonColumn ExpressionView_Down;
		private System.Windows.Forms.DataGridViewCheckBoxColumn ExpressionDetailView_Enabled;
		private System.Windows.Forms.DataGridViewTextBoxColumn ExpressionDetailView_LeftOperand;
		private System.Windows.Forms.DataGridViewTextBoxColumn ExpressionDetailView_RightOperand;
		private System.Windows.Forms.DataGridViewTextBoxColumn ExpressionDetailView_Operator;
	}
}