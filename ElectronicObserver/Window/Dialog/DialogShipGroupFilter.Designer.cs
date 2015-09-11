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
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.ExpressionView = new System.Windows.Forms.DataGridView();
			this.ExpressionDetailView = new System.Windows.Forms.DataGridView();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.GroupExpression = new System.Windows.Forms.GroupBox();
			this.LeftOperand = new System.Windows.Forms.ComboBox();
			this.Operator = new System.Windows.Forms.ComboBox();
			this.RightOperand_TextBox = new System.Windows.Forms.TextBox();
			this.RightOperand_NumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.RightOperand_ComboBox = new System.Windows.Forms.ComboBox();
			this.ButtonAdd = new System.Windows.Forms.Button();
			this.ButtonEdit = new System.Windows.Forms.Button();
			this.ButtonRemove = new System.Windows.Forms.Button();
			this.ExpressionView_Enabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.ExpressionView_AndOr = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.ExpressionView_Inverse = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.ExpressionView_Expression = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ExpressionView_Up = new System.Windows.Forms.DataGridViewButtonColumn();
			this.ExpressionView_Down = new System.Windows.Forms.DataGridViewButtonColumn();
			this.ExpressionDetailView_Enabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.ExpressionDetailView_LeftOperand = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ExpressionDetailView_RightOperand = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ExpressionDetailView_Operator = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ExpressionView)).BeginInit();
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
			this.splitContainer1.Panel1.Controls.Add(this.ExpressionView);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Size = new System.Drawing.Size(624, 441);
			this.splitContainer1.SplitterDistance = 208;
			this.splitContainer1.TabIndex = 0;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
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
			this.splitContainer2.Panel2.Controls.Add(this.ButtonRemove);
			this.splitContainer2.Panel2.Controls.Add(this.ButtonEdit);
			this.splitContainer2.Panel2.Controls.Add(this.ButtonAdd);
			this.splitContainer2.Panel2.Controls.Add(this.GroupExpression);
			this.splitContainer2.Panel2.Controls.Add(this.groupBox1);
			this.splitContainer2.Size = new System.Drawing.Size(412, 441);
			this.splitContainer2.SplitterDistance = 268;
			this.splitContainer2.TabIndex = 0;
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
            this.ExpressionView_AndOr,
            this.ExpressionView_Inverse,
            this.ExpressionView_Expression,
            this.ExpressionView_Up,
            this.ExpressionView_Down});
			this.ExpressionView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ExpressionView.Location = new System.Drawing.Point(0, 0);
			this.ExpressionView.MultiSelect = false;
			this.ExpressionView.Name = "ExpressionView";
			this.ExpressionView.ReadOnly = true;
			this.ExpressionView.RowHeadersVisible = false;
			this.ExpressionView.RowTemplate.Height = 21;
			this.ExpressionView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.ExpressionView.Size = new System.Drawing.Size(208, 441);
			this.ExpressionView.TabIndex = 0;
			this.ExpressionView.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.ExpressionView_CellMouseClick);
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
			this.ExpressionDetailView.ReadOnly = true;
			this.ExpressionDetailView.RowHeadersVisible = false;
			this.ExpressionDetailView.RowTemplate.Height = 21;
			this.ExpressionDetailView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.ExpressionDetailView.Size = new System.Drawing.Size(412, 268);
			this.ExpressionDetailView.TabIndex = 0;
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(406, 40);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Group";
			// 
			// GroupExpression
			// 
			this.GroupExpression.Controls.Add(this.RightOperand_ComboBox);
			this.GroupExpression.Controls.Add(this.RightOperand_NumericUpDown);
			this.GroupExpression.Controls.Add(this.RightOperand_TextBox);
			this.GroupExpression.Controls.Add(this.Operator);
			this.GroupExpression.Controls.Add(this.LeftOperand);
			this.GroupExpression.Location = new System.Drawing.Point(3, 49);
			this.GroupExpression.Name = "GroupExpression";
			this.GroupExpression.Size = new System.Drawing.Size(406, 57);
			this.GroupExpression.TabIndex = 1;
			this.GroupExpression.TabStop = false;
			this.GroupExpression.Text = "Detail";
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
			// Operator
			// 
			this.Operator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.Operator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.Operator.FormattingEnabled = true;
			this.Operator.Location = new System.Drawing.Point(300, 22);
			this.Operator.Name = "Operator";
			this.Operator.Size = new System.Drawing.Size(100, 23);
			this.Operator.TabIndex = 1;
			// 
			// RightOperand_TextBox
			// 
			this.RightOperand_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.RightOperand_TextBox.Location = new System.Drawing.Point(133, 22);
			this.RightOperand_TextBox.Name = "RightOperand_TextBox";
			this.RightOperand_TextBox.Size = new System.Drawing.Size(161, 23);
			this.RightOperand_TextBox.TabIndex = 2;
			// 
			// RightOperand_NumericUpDown
			// 
			this.RightOperand_NumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.RightOperand_NumericUpDown.Location = new System.Drawing.Point(133, 22);
			this.RightOperand_NumericUpDown.Name = "RightOperand_NumericUpDown";
			this.RightOperand_NumericUpDown.Size = new System.Drawing.Size(161, 23);
			this.RightOperand_NumericUpDown.TabIndex = 2;
			this.RightOperand_NumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// RightOperand_ComboBox
			// 
			this.RightOperand_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.RightOperand_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.RightOperand_ComboBox.FormattingEnabled = true;
			this.RightOperand_ComboBox.Location = new System.Drawing.Point(133, 22);
			this.RightOperand_ComboBox.Name = "RightOperand_ComboBox";
			this.RightOperand_ComboBox.Size = new System.Drawing.Size(161, 23);
			this.RightOperand_ComboBox.TabIndex = 3;
			// 
			// ButtonAdd
			// 
			this.ButtonAdd.Location = new System.Drawing.Point(9, 112);
			this.ButtonAdd.Name = "ButtonAdd";
			this.ButtonAdd.Size = new System.Drawing.Size(75, 23);
			this.ButtonAdd.TabIndex = 2;
			this.ButtonAdd.Text = "追加";
			this.ButtonAdd.UseVisualStyleBackColor = true;
			// 
			// ButtonEdit
			// 
			this.ButtonEdit.Location = new System.Drawing.Point(90, 112);
			this.ButtonEdit.Name = "ButtonEdit";
			this.ButtonEdit.Size = new System.Drawing.Size(75, 23);
			this.ButtonEdit.TabIndex = 3;
			this.ButtonEdit.Text = "編集";
			this.ButtonEdit.UseVisualStyleBackColor = true;
			// 
			// ButtonRemove
			// 
			this.ButtonRemove.Location = new System.Drawing.Point(171, 112);
			this.ButtonRemove.Name = "ButtonRemove";
			this.ButtonRemove.Size = new System.Drawing.Size(75, 23);
			this.ButtonRemove.TabIndex = 4;
			this.ButtonRemove.Text = "削除";
			this.ButtonRemove.UseVisualStyleBackColor = true;
			// 
			// ExpressionView_Enabled
			// 
			this.ExpressionView_Enabled.HeaderText = "";
			this.ExpressionView_Enabled.Name = "ExpressionView_Enabled";
			this.ExpressionView_Enabled.ReadOnly = true;
			this.ExpressionView_Enabled.Width = 20;
			// 
			// ExpressionView_AndOr
			// 
			this.ExpressionView_AndOr.HeaderText = "";
			this.ExpressionView_AndOr.Name = "ExpressionView_AndOr";
			this.ExpressionView_AndOr.ReadOnly = true;
			this.ExpressionView_AndOr.Width = 40;
			// 
			// ExpressionView_Inverse
			// 
			this.ExpressionView_Inverse.HeaderText = "Inv";
			this.ExpressionView_Inverse.Name = "ExpressionView_Inverse";
			this.ExpressionView_Inverse.ReadOnly = true;
			this.ExpressionView_Inverse.Width = 20;
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
			this.ExpressionView_Up.ReadOnly = true;
			this.ExpressionView_Up.Width = 20;
			// 
			// ExpressionView_Down
			// 
			this.ExpressionView_Down.HeaderText = "↓";
			this.ExpressionView_Down.Name = "ExpressionView_Down";
			this.ExpressionView_Down.ReadOnly = true;
			this.ExpressionView_Down.Width = 20;
			// 
			// ExpressionDetailView_Enabled
			// 
			this.ExpressionDetailView_Enabled.HeaderText = "";
			this.ExpressionDetailView_Enabled.Name = "ExpressionDetailView_Enabled";
			this.ExpressionDetailView_Enabled.ReadOnly = true;
			this.ExpressionDetailView_Enabled.Width = 20;
			// 
			// ExpressionDetailView_LeftOperand
			// 
			this.ExpressionDetailView_LeftOperand.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.ExpressionDetailView_LeftOperand.HeaderText = "Left";
			this.ExpressionDetailView_LeftOperand.Name = "ExpressionDetailView_LeftOperand";
			this.ExpressionDetailView_LeftOperand.ReadOnly = true;
			this.ExpressionDetailView_LeftOperand.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// ExpressionDetailView_RightOperand
			// 
			this.ExpressionDetailView_RightOperand.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.ExpressionDetailView_RightOperand.HeaderText = "Right";
			this.ExpressionDetailView_RightOperand.Name = "ExpressionDetailView_RightOperand";
			this.ExpressionDetailView_RightOperand.ReadOnly = true;
			this.ExpressionDetailView_RightOperand.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// ExpressionDetailView_Operator
			// 
			this.ExpressionDetailView_Operator.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.ExpressionDetailView_Operator.HeaderText = "Operator";
			this.ExpressionDetailView_Operator.Name = "ExpressionDetailView_Operator";
			this.ExpressionDetailView_Operator.ReadOnly = true;
			this.ExpressionDetailView_Operator.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// DialogShipGroupFilter
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(624, 441);
			this.Controls.Add(this.splitContainer1);
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.Name = "DialogShipGroupFilter";
			this.Text = "DialogShipGroupFilter";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ExpressionView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ExpressionDetailView)).EndInit();
			this.GroupExpression.ResumeLayout(false);
			this.GroupExpression.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.RightOperand_NumericUpDown)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.DataGridView ExpressionView;
		private System.Windows.Forms.DataGridView ExpressionDetailView;
		private System.Windows.Forms.GroupBox GroupExpression;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox RightOperand_ComboBox;
		private System.Windows.Forms.NumericUpDown RightOperand_NumericUpDown;
		private System.Windows.Forms.TextBox RightOperand_TextBox;
		private System.Windows.Forms.ComboBox Operator;
		private System.Windows.Forms.ComboBox LeftOperand;
		private System.Windows.Forms.Button ButtonRemove;
		private System.Windows.Forms.Button ButtonEdit;
		private System.Windows.Forms.Button ButtonAdd;
		private System.Windows.Forms.DataGridViewCheckBoxColumn ExpressionView_Enabled;
		private System.Windows.Forms.DataGridViewComboBoxColumn ExpressionView_AndOr;
		private System.Windows.Forms.DataGridViewCheckBoxColumn ExpressionView_Inverse;
		private System.Windows.Forms.DataGridViewTextBoxColumn ExpressionView_Expression;
		private System.Windows.Forms.DataGridViewButtonColumn ExpressionView_Up;
		private System.Windows.Forms.DataGridViewButtonColumn ExpressionView_Down;
		private System.Windows.Forms.DataGridViewCheckBoxColumn ExpressionDetailView_Enabled;
		private System.Windows.Forms.DataGridViewTextBoxColumn ExpressionDetailView_LeftOperand;
		private System.Windows.Forms.DataGridViewTextBoxColumn ExpressionDetailView_RightOperand;
		private System.Windows.Forms.DataGridViewTextBoxColumn ExpressionDetailView_Operator;
	}
}