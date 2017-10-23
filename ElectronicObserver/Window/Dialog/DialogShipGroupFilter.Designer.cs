namespace ElectronicObserver.Window.Dialog
{
	partial class DialogShipGroupFilter
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
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
			this.LabelResult = new System.Windows.Forms.Label();
			this.Expression_Delete = new System.Windows.Forms.Button();
			this.Expression_Add = new System.Windows.Forms.Button();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.ExpressionDetailView = new System.Windows.Forms.DataGridView();
			this.ExpressionDetailView_Enabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.ExpressionDetailView_LeftOperand = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ExpressionDetailView_RightOperand = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ExpressionDetailView_Operator = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RightOperand_ComboBox = new System.Windows.Forms.ComboBox();
			this.RightOperand_NumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.Operator = new System.Windows.Forms.ComboBox();
			this.LeftOperand = new System.Windows.Forms.ComboBox();
			this.Description = new System.Windows.Forms.Label();
			this.ExpressionDetail_Delete = new System.Windows.Forms.Button();
			this.ExpressionDetail_Edit = new System.Windows.Forms.Button();
			this.ExpressionDetail_Add = new System.Windows.Forms.Button();
			this.RightOperand_TextBox = new System.Windows.Forms.TextBox();
			this.ButtonCancel = new System.Windows.Forms.Button();
			this.ButtonOK = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.ConvertToExpression = new System.Windows.Forms.Button();
			this.OptimizeConstFilter = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.ClearConstFilter = new System.Windows.Forms.Button();
			this.ConstFilterView = new System.Windows.Forms.DataGridView();
			this.ConstFilterView_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ConstFilterView_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ConstFilterView_Up = new System.Windows.Forms.DataGridViewButtonColumn();
			this.ConstFilterView_Down = new System.Windows.Forms.DataGridViewButtonColumn();
			this.ConstFilterView_Delete = new System.Windows.Forms.DataGridViewButtonColumn();
			this.ConstFilterSelector = new System.Windows.Forms.ComboBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.ButtonMenu = new System.Windows.Forms.Button();
			this.SubMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.SubMenu_ImportFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.SubMenu_ExportFilter = new System.Windows.Forms.ToolStripMenuItem();
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
			((System.ComponentModel.ISupportInitialize)(this.RightOperand_NumericUpDown)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ConstFilterView)).BeginInit();
			this.SubMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(3, 3);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Size = new System.Drawing.Size(610, 369);
			this.splitContainer1.SplitterDistance = 234;
			this.splitContainer1.TabIndex = 0;
			// 
			// splitContainer3
			// 
			this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
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
			this.splitContainer3.Panel2.Controls.Add(this.LabelResult);
			this.splitContainer3.Panel2.Controls.Add(this.Expression_Delete);
			this.splitContainer3.Panel2.Controls.Add(this.Expression_Add);
			this.splitContainer3.Size = new System.Drawing.Size(234, 369);
			this.splitContainer3.SplitterDistance = 253;
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
			this.ExpressionView.Size = new System.Drawing.Size(234, 253);
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
			this.ExpressionView_ExternalAndOr.HeaderText = "外条件";
			this.ExpressionView_ExternalAndOr.Items.AddRange(new object[] {
			"And",
			"Or"});
			this.ExpressionView_ExternalAndOr.Name = "ExpressionView_ExternalAndOr";
			this.ExpressionView_ExternalAndOr.Width = 50;
			// 
			// ExpressionView_Inverse
			// 
			this.ExpressionView_Inverse.HeaderText = "否";
			this.ExpressionView_Inverse.Name = "ExpressionView_Inverse";
			this.ExpressionView_Inverse.ToolTipText = "条件を反転するか";
			this.ExpressionView_Inverse.Width = 20;
			// 
			// ExpressionView_InternalAndOr
			// 
			this.ExpressionView_InternalAndOr.HeaderText = "内条件";
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
			// LabelResult
			// 
			this.LabelResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.LabelResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.LabelResult.Location = new System.Drawing.Point(3, 31);
			this.LabelResult.Name = "LabelResult";
			this.LabelResult.Size = new System.Drawing.Size(228, 78);
			this.LabelResult.TabIndex = 2;
			this.LabelResult.Text = "Result";
			this.LabelResult.Click += new System.EventHandler(this.LabelResult_Click);
			// 
			// Expression_Delete
			// 
			this.Expression_Delete.Location = new System.Drawing.Point(84, 5);
			this.Expression_Delete.Name = "Expression_Delete";
			this.Expression_Delete.Size = new System.Drawing.Size(75, 23);
			this.Expression_Delete.TabIndex = 1;
			this.Expression_Delete.Text = "削除";
			this.Expression_Delete.UseVisualStyleBackColor = true;
			this.Expression_Delete.Click += new System.EventHandler(this.Expression_Delete_Click);
			// 
			// Expression_Add
			// 
			this.Expression_Add.Location = new System.Drawing.Point(3, 5);
			this.Expression_Add.Name = "Expression_Add";
			this.Expression_Add.Size = new System.Drawing.Size(75, 23);
			this.Expression_Add.TabIndex = 0;
			this.Expression_Add.Text = "追加";
			this.Expression_Add.UseVisualStyleBackColor = true;
			this.Expression_Add.Click += new System.EventHandler(this.Expression_Add_Click);
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
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
			this.splitContainer2.Panel2.Controls.Add(this.RightOperand_ComboBox);
			this.splitContainer2.Panel2.Controls.Add(this.RightOperand_NumericUpDown);
			this.splitContainer2.Panel2.Controls.Add(this.Operator);
			this.splitContainer2.Panel2.Controls.Add(this.LeftOperand);
			this.splitContainer2.Panel2.Controls.Add(this.Description);
			this.splitContainer2.Panel2.Controls.Add(this.ExpressionDetail_Delete);
			this.splitContainer2.Panel2.Controls.Add(this.ExpressionDetail_Edit);
			this.splitContainer2.Panel2.Controls.Add(this.ExpressionDetail_Add);
			this.splitContainer2.Panel2.Controls.Add(this.RightOperand_TextBox);
			this.splitContainer2.Size = new System.Drawing.Size(372, 369);
			this.splitContainer2.SplitterDistance = 253;
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
			this.ExpressionDetailView.Size = new System.Drawing.Size(372, 253);
			this.ExpressionDetailView.TabIndex = 0;
			this.ExpressionDetailView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.ExpressionDetailView_CellFormatting);
			this.ExpressionDetailView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.ExpressionDetailView_CellValueChanged);
			this.ExpressionDetailView.CurrentCellDirtyStateChanged += new System.EventHandler(this.ExpressionDetailView_CurrentCellDirtyStateChanged);
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
			// RightOperand_ComboBox
			// 
			this.RightOperand_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.RightOperand_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.RightOperand_ComboBox.FormattingEnabled = true;
			this.RightOperand_ComboBox.Location = new System.Drawing.Point(130, 5);
			this.RightOperand_ComboBox.Name = "RightOperand_ComboBox";
			this.RightOperand_ComboBox.Size = new System.Drawing.Size(112, 23);
			this.RightOperand_ComboBox.TabIndex = 1;
			// 
			// RightOperand_NumericUpDown
			// 
			this.RightOperand_NumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.RightOperand_NumericUpDown.Location = new System.Drawing.Point(130, 5);
			this.RightOperand_NumericUpDown.Name = "RightOperand_NumericUpDown";
			this.RightOperand_NumericUpDown.Size = new System.Drawing.Size(112, 23);
			this.RightOperand_NumericUpDown.TabIndex = 3;
			this.RightOperand_NumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.RightOperand_NumericUpDown.ValueChanged += new System.EventHandler(this.RightOperand_NumericUpDown_ValueChanged);
			// 
			// Operator
			// 
			this.Operator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.Operator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.Operator.FormattingEnabled = true;
			this.Operator.Location = new System.Drawing.Point(248, 5);
			this.Operator.Name = "Operator";
			this.Operator.Size = new System.Drawing.Size(121, 23);
			this.Operator.TabIndex = 2;
			// 
			// LeftOperand
			// 
			this.LeftOperand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.LeftOperand.FormattingEnabled = true;
			this.LeftOperand.Location = new System.Drawing.Point(3, 5);
			this.LeftOperand.Name = "LeftOperand";
			this.LeftOperand.Size = new System.Drawing.Size(121, 23);
			this.LeftOperand.TabIndex = 0;
			this.LeftOperand.SelectedValueChanged += new System.EventHandler(this.LeftOperand_SelectedValueChanged);
			// 
			// Description
			// 
			this.Description.AutoSize = true;
			this.Description.Location = new System.Drawing.Point(127, 32);
			this.Description.Name = "Description";
			this.Description.Size = new System.Drawing.Size(41, 15);
			this.Description.TabIndex = 3;
			this.Description.Text = "(説明)";
			// 
			// ExpressionDetail_Delete
			// 
			this.ExpressionDetail_Delete.Location = new System.Drawing.Point(165, 86);
			this.ExpressionDetail_Delete.Name = "ExpressionDetail_Delete";
			this.ExpressionDetail_Delete.Size = new System.Drawing.Size(75, 23);
			this.ExpressionDetail_Delete.TabIndex = 6;
			this.ExpressionDetail_Delete.Text = "削除";
			this.ExpressionDetail_Delete.UseVisualStyleBackColor = true;
			this.ExpressionDetail_Delete.Click += new System.EventHandler(this.ExpressionDetail_Delete_Click);
			// 
			// ExpressionDetail_Edit
			// 
			this.ExpressionDetail_Edit.Location = new System.Drawing.Point(84, 86);
			this.ExpressionDetail_Edit.Name = "ExpressionDetail_Edit";
			this.ExpressionDetail_Edit.Size = new System.Drawing.Size(75, 23);
			this.ExpressionDetail_Edit.TabIndex = 5;
			this.ExpressionDetail_Edit.Text = "上書き";
			this.ExpressionDetail_Edit.UseVisualStyleBackColor = true;
			this.ExpressionDetail_Edit.Click += new System.EventHandler(this.ExpressionDetail_Edit_Click);
			// 
			// ExpressionDetail_Add
			// 
			this.ExpressionDetail_Add.Location = new System.Drawing.Point(3, 86);
			this.ExpressionDetail_Add.Name = "ExpressionDetail_Add";
			this.ExpressionDetail_Add.Size = new System.Drawing.Size(75, 23);
			this.ExpressionDetail_Add.TabIndex = 4;
			this.ExpressionDetail_Add.Text = "追加";
			this.ExpressionDetail_Add.UseVisualStyleBackColor = true;
			this.ExpressionDetail_Add.Click += new System.EventHandler(this.ExpressionDetail_Add_Click);
			// 
			// RightOperand_TextBox
			// 
			this.RightOperand_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.RightOperand_TextBox.Location = new System.Drawing.Point(130, 5);
			this.RightOperand_TextBox.Name = "RightOperand_TextBox";
			this.RightOperand_TextBox.Size = new System.Drawing.Size(112, 23);
			this.RightOperand_TextBox.TabIndex = 1;
			// 
			// ButtonCancel
			// 
			this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonCancel.Location = new System.Drawing.Point(537, 406);
			this.ButtonCancel.Name = "ButtonCancel";
			this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
			this.ButtonCancel.TabIndex = 3;
			this.ButtonCancel.Text = "キャンセル";
			this.ButtonCancel.UseVisualStyleBackColor = true;
			this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
			// 
			// ButtonOK
			// 
			this.ButtonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonOK.Location = new System.Drawing.Point(456, 406);
			this.ButtonOK.Name = "ButtonOK";
			this.ButtonOK.Size = new System.Drawing.Size(75, 23);
			this.ButtonOK.TabIndex = 2;
			this.ButtonOK.Text = "OK";
			this.ButtonOK.UseVisualStyleBackColor = true;
			this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(624, 403);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.splitContainer1);
			this.tabPage1.Location = new System.Drawing.Point(4, 24);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(616, 375);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "フィルタ";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.ConvertToExpression);
			this.tabPage2.Controls.Add(this.OptimizeConstFilter);
			this.tabPage2.Controls.Add(this.label1);
			this.tabPage2.Controls.Add(this.ClearConstFilter);
			this.tabPage2.Controls.Add(this.ConstFilterView);
			this.tabPage2.Controls.Add(this.ConstFilterSelector);
			this.tabPage2.Location = new System.Drawing.Point(4, 24);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(616, 375);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "包含/除外リスト";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// ConvertToExpression
			// 
			this.ConvertToExpression.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ConvertToExpression.Location = new System.Drawing.Point(452, 42);
			this.ConvertToExpression.Name = "ConvertToExpression";
			this.ConvertToExpression.Size = new System.Drawing.Size(75, 23);
			this.ConvertToExpression.TabIndex = 3;
			this.ConvertToExpression.Text = "式に変換";
			this.toolTip1.SetToolTip(this.ConvertToExpression, "包含/除外リストを式に変換します。\r\n逆変換はできないのでご注意ください。");
			this.ConvertToExpression.UseVisualStyleBackColor = true;
			this.ConvertToExpression.Click += new System.EventHandler(this.ConvertToExpression_Click);
			// 
			// OptimizeConstFilter
			// 
			this.OptimizeConstFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.OptimizeConstFilter.Location = new System.Drawing.Point(371, 42);
			this.OptimizeConstFilter.Name = "OptimizeConstFilter";
			this.OptimizeConstFilter.Size = new System.Drawing.Size(75, 23);
			this.OptimizeConstFilter.TabIndex = 2;
			this.OptimizeConstFilter.Text = "最適化";
			this.toolTip1.SetToolTip(this.OptimizeConstFilter, "存在しない艦娘をリストから削除します。");
			this.OptimizeConstFilter.UseVisualStyleBackColor = true;
			this.OptimizeConstFilter.Click += new System.EventHandler(this.OptimizeConstFilter_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 6);
			this.label1.Margin = new System.Windows.Forms.Padding(3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(306, 30);
			this.label1.TabIndex = 0;
			this.label1.Text = "フィルタの内容にかかわらず、追加/除外される艦娘のリストです。\r\n追加はグループ本体の右クリックメニューから行ってください。";
			// 
			// ClearConstFilter
			// 
			this.ClearConstFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ClearConstFilter.Location = new System.Drawing.Point(533, 42);
			this.ClearConstFilter.Name = "ClearConstFilter";
			this.ClearConstFilter.Size = new System.Drawing.Size(75, 23);
			this.ClearConstFilter.TabIndex = 4;
			this.ClearConstFilter.Text = "初期化";
			this.toolTip1.SetToolTip(this.ClearConstFilter, "リストの内容をすべて削除します。");
			this.ClearConstFilter.UseVisualStyleBackColor = true;
			this.ClearConstFilter.Click += new System.EventHandler(this.ClearConstFilter_Click);
			// 
			// ConstFilterView
			// 
			this.ConstFilterView.AllowUserToAddRows = false;
			this.ConstFilterView.AllowUserToDeleteRows = false;
			this.ConstFilterView.AllowUserToResizeColumns = false;
			this.ConstFilterView.AllowUserToResizeRows = false;
			this.ConstFilterView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.ConstFilterView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.ConstFilterView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
			this.ConstFilterView_ID,
			this.ConstFilterView_Name,
			this.ConstFilterView_Up,
			this.ConstFilterView_Down,
			this.ConstFilterView_Delete});
			this.ConstFilterView.Location = new System.Drawing.Point(8, 71);
			this.ConstFilterView.Name = "ConstFilterView";
			this.ConstFilterView.ReadOnly = true;
			this.ConstFilterView.RowHeadersVisible = false;
			this.ConstFilterView.RowTemplate.Height = 21;
			this.ConstFilterView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.ConstFilterView.Size = new System.Drawing.Size(600, 288);
			this.ConstFilterView.TabIndex = 5;
			this.ConstFilterView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ConstFilterView_CellContentClick);
			// 
			// ConstFilterView_ID
			// 
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.ConstFilterView_ID.DefaultCellStyle = dataGridViewCellStyle1;
			this.ConstFilterView_ID.HeaderText = "ID";
			this.ConstFilterView_ID.Name = "ConstFilterView_ID";
			this.ConstFilterView_ID.ReadOnly = true;
			this.ConstFilterView_ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.ConstFilterView_ID.Width = 60;
			// 
			// ConstFilterView_Name
			// 
			this.ConstFilterView_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.ConstFilterView_Name.HeaderText = "艦名";
			this.ConstFilterView_Name.Name = "ConstFilterView_Name";
			this.ConstFilterView_Name.ReadOnly = true;
			this.ConstFilterView_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// ConstFilterView_Up
			// 
			this.ConstFilterView_Up.HeaderText = "↑";
			this.ConstFilterView_Up.Name = "ConstFilterView_Up";
			this.ConstFilterView_Up.ReadOnly = true;
			this.ConstFilterView_Up.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.ConstFilterView_Up.Visible = false;
			this.ConstFilterView_Up.Width = 20;
			// 
			// ConstFilterView_Down
			// 
			this.ConstFilterView_Down.HeaderText = "↓";
			this.ConstFilterView_Down.Name = "ConstFilterView_Down";
			this.ConstFilterView_Down.ReadOnly = true;
			this.ConstFilterView_Down.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.ConstFilterView_Down.Visible = false;
			this.ConstFilterView_Down.Width = 20;
			// 
			// ConstFilterView_Delete
			// 
			this.ConstFilterView_Delete.HeaderText = "×";
			this.ConstFilterView_Delete.Name = "ConstFilterView_Delete";
			this.ConstFilterView_Delete.ReadOnly = true;
			this.ConstFilterView_Delete.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.ConstFilterView_Delete.Width = 20;
			// 
			// ConstFilterSelector
			// 
			this.ConstFilterSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.ConstFilterSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ConstFilterSelector.FormattingEnabled = true;
			this.ConstFilterSelector.Items.AddRange(new object[] {
			"包含リスト",
			"除外リスト"});
			this.ConstFilterSelector.Location = new System.Drawing.Point(8, 42);
			this.ConstFilterSelector.Name = "ConstFilterSelector";
			this.ConstFilterSelector.Size = new System.Drawing.Size(357, 23);
			this.ConstFilterSelector.TabIndex = 1;
			this.ConstFilterSelector.SelectedIndexChanged += new System.EventHandler(this.ConstFilterSelector_SelectedIndexChanged);
			// 
			// ButtonMenu
			// 
			this.ButtonMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ButtonMenu.Location = new System.Drawing.Point(12, 406);
			this.ButtonMenu.Name = "ButtonMenu";
			this.ButtonMenu.Size = new System.Drawing.Size(75, 23);
			this.ButtonMenu.TabIndex = 1;
			this.ButtonMenu.Text = "メニュー ▼";
			this.ButtonMenu.UseVisualStyleBackColor = true;
			this.ButtonMenu.Click += new System.EventHandler(this.ButtonMenu_Click);
			// 
			// SubMenu
			// 
			this.SubMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.SubMenu_ImportFilter,
			this.SubMenu_ExportFilter});
			this.SubMenu.Name = "Menu";
			this.SubMenu.Size = new System.Drawing.Size(189, 48);
			// 
			// SubMenu_ImportFilter
			// 
			this.SubMenu_ImportFilter.Name = "SubMenu_ImportFilter";
			this.SubMenu_ImportFilter.Size = new System.Drawing.Size(188, 22);
			this.SubMenu_ImportFilter.Text = "フィルタのインポート(&I)";
			this.SubMenu_ImportFilter.Click += new System.EventHandler(this.Menu_ImportFilter_Click);
			// 
			// SubMenu_ExportFilter
			// 
			this.SubMenu_ExportFilter.Name = "SubMenu_ExportFilter";
			this.SubMenu_ExportFilter.Size = new System.Drawing.Size(188, 22);
			this.SubMenu_ExportFilter.Text = "フィルタのエクスポート(&E)";
			this.SubMenu_ExportFilter.Click += new System.EventHandler(this.Menu_ExportFilter_Click);
			// 
			// DialogShipGroupFilter
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(624, 441);
			this.Controls.Add(this.ButtonMenu);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.ButtonCancel);
			this.Controls.Add(this.ButtonOK);
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogShipGroupFilter";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "フィルタ設定";
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
			this.splitContainer2.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ExpressionDetailView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.RightOperand_NumericUpDown)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ConstFilterView)).EndInit();
			this.SubMenu.ResumeLayout(false);
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
		private System.Windows.Forms.ComboBox RightOperand_ComboBox;
		private System.Windows.Forms.NumericUpDown RightOperand_NumericUpDown;
		private System.Windows.Forms.ComboBox Operator;
		private System.Windows.Forms.TextBox RightOperand_TextBox;
		private System.Windows.Forms.ComboBox LeftOperand;
		private System.Windows.Forms.Label Description;
		private System.Windows.Forms.Button ButtonCancel;
		private System.Windows.Forms.Button ButtonOK;
		private System.Windows.Forms.DataGridViewCheckBoxColumn ExpressionDetailView_Enabled;
		private System.Windows.Forms.DataGridViewTextBoxColumn ExpressionDetailView_LeftOperand;
		private System.Windows.Forms.DataGridViewTextBoxColumn ExpressionDetailView_RightOperand;
		private System.Windows.Forms.DataGridViewTextBoxColumn ExpressionDetailView_Operator;
		private System.Windows.Forms.Label LabelResult;
		private System.Windows.Forms.DataGridViewCheckBoxColumn ExpressionView_Enabled;
		private System.Windows.Forms.DataGridViewComboBoxColumn ExpressionView_ExternalAndOr;
		private System.Windows.Forms.DataGridViewCheckBoxColumn ExpressionView_Inverse;
		private System.Windows.Forms.DataGridViewComboBoxColumn ExpressionView_InternalAndOr;
		private System.Windows.Forms.DataGridViewTextBoxColumn ExpressionView_Expression;
		private System.Windows.Forms.DataGridViewButtonColumn ExpressionView_Up;
		private System.Windows.Forms.DataGridViewButtonColumn ExpressionView_Down;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button ClearConstFilter;
		private System.Windows.Forms.DataGridView ConstFilterView;
		private System.Windows.Forms.ComboBox ConstFilterSelector;
		private System.Windows.Forms.Button OptimizeConstFilter;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.DataGridViewTextBoxColumn ConstFilterView_ID;
		private System.Windows.Forms.DataGridViewTextBoxColumn ConstFilterView_Name;
		private System.Windows.Forms.DataGridViewButtonColumn ConstFilterView_Up;
		private System.Windows.Forms.DataGridViewButtonColumn ConstFilterView_Down;
		private System.Windows.Forms.DataGridViewButtonColumn ConstFilterView_Delete;
		private System.Windows.Forms.Button ConvertToExpression;
		private System.Windows.Forms.Button ButtonMenu;
		private System.Windows.Forms.ContextMenuStrip SubMenu;
		private System.Windows.Forms.ToolStripMenuItem SubMenu_ImportFilter;
		private System.Windows.Forms.ToolStripMenuItem SubMenu_ExportFilter;
	}
}