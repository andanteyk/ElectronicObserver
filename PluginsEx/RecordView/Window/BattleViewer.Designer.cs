namespace RecordView
{
    partial class BattleViewer
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.导出所选记录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出当前所有记录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置导出文件名格式ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.cbArea = new System.Windows.Forms.ComboBox();
            this.cbShip = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.cbBoss = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbSup = new System.Windows.Forms.CheckBox();
            this.cbNb = new System.Windows.Forms.CheckBox();
            this.cbClT = new System.Windows.Forms.CheckBox();
            this.cbSh3 = new System.Windows.Forms.CheckBox();
            this.cbSh2 = new System.Windows.Forms.CheckBox();
            this.cbSh1 = new System.Windows.Forms.CheckBox();
            this.cbOpT = new System.Windows.Forms.CheckBox();
            this.cbAir2 = new System.Windows.Forms.CheckBox();
            this.cbAir = new System.Windows.Forms.CheckBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.colNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPoint = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFlagship = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMVP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBoss = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colShip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNo,
            this.colTime,
            this.colArea,
            this.colPoint,
            this.colType,
            this.colFlagship,
            this.colMVP,
            this.colBoss,
            this.colRank,
            this.colShip,
            this.colText});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Location = new System.Drawing.Point(2, 109);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.Size = new System.Drawing.Size(683, 442);
            this.dataGridView1.TabIndex = 6;
            this.dataGridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导出所选记录ToolStripMenuItem,
            this.导出当前所有记录ToolStripMenuItem,
            this.设置导出文件名格式ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(185, 70);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // 导出所选记录ToolStripMenuItem
            // 
            this.导出所选记录ToolStripMenuItem.Name = "导出所选记录ToolStripMenuItem";
            this.导出所选记录ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.导出所选记录ToolStripMenuItem.Text = "导出所有选中记录";
            this.导出所选记录ToolStripMenuItem.Click += new System.EventHandler(this.导出所选记录ToolStripMenuItem_Click);
            // 
            // 导出当前所有记录ToolStripMenuItem
            // 
            this.导出当前所有记录ToolStripMenuItem.Name = "导出当前所有记录ToolStripMenuItem";
            this.导出当前所有记录ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.导出当前所有记录ToolStripMenuItem.Text = "导出当前表格记录";
            this.导出当前所有记录ToolStripMenuItem.Click += new System.EventHandler(this.导出当前所有记录ToolStripMenuItem_Click);
            // 
            // 设置导出文件名格式ToolStripMenuItem
            // 
            this.设置导出文件名格式ToolStripMenuItem.Name = "设置导出文件名格式ToolStripMenuItem";
            this.设置导出文件名格式ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.设置导出文件名格式ToolStripMenuItem.Text = "设置导出文件名格式";
            this.设置导出文件名格式ToolStripMenuItem.Click += new System.EventHandler(this.设置导出文件名格式ToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(292, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "海域";
            // 
            // cbArea
            // 
            this.cbArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbArea.FormattingEnabled = true;
            this.cbArea.Location = new System.Drawing.Point(338, 27);
            this.cbArea.Name = "cbArea";
            this.cbArea.Size = new System.Drawing.Size(79, 20);
            this.cbArea.TabIndex = 8;
            this.cbArea.TextChanged += new System.EventHandler(this.cbArea_TextChanged);
            // 
            // cbShip
            // 
            this.cbShip.FormattingEnabled = true;
            this.cbShip.Location = new System.Drawing.Point(337, 60);
            this.cbShip.Name = "cbShip";
            this.cbShip.Size = new System.Drawing.Size(121, 20);
            this.cbShip.TabIndex = 10;
            this.cbShip.TextChanged += new System.EventHandler(this.cbShip_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(292, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "掉落舰";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(12, 18);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(130, 21);
            this.dateTimePicker1.TabIndex = 11;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(26, 48);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "前一日";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(107, 48);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "今天";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(188, 48);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 14;
            this.button3.Text = "后一日";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // cbBoss
            // 
            this.cbBoss.AutoSize = true;
            this.cbBoss.Location = new System.Drawing.Point(428, 29);
            this.cbBoss.Name = "cbBoss";
            this.cbBoss.Size = new System.Drawing.Size(48, 16);
            this.cbBoss.TabIndex = 15;
            this.cbBoss.Text = "BOSS";
            this.cbBoss.UseVisualStyleBackColor = true;
            this.cbBoss.CheckedChanged += new System.EventHandler(this.cbBoss_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbSup);
            this.groupBox1.Controls.Add(this.cbNb);
            this.groupBox1.Controls.Add(this.cbClT);
            this.groupBox1.Controls.Add(this.cbSh3);
            this.groupBox1.Controls.Add(this.cbSh2);
            this.groupBox1.Controls.Add(this.cbSh1);
            this.groupBox1.Controls.Add(this.cbOpT);
            this.groupBox1.Controls.Add(this.cbAir2);
            this.groupBox1.Controls.Add(this.cbAir);
            this.groupBox1.Location = new System.Drawing.Point(486, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(193, 91);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "必须战斗阶段";
            // 
            // cbSup
            // 
            this.cbSup.AutoSize = true;
            this.cbSup.Location = new System.Drawing.Point(130, 20);
            this.cbSup.Name = "cbSup";
            this.cbSup.Size = new System.Drawing.Size(48, 16);
            this.cbSup.TabIndex = 8;
            this.cbSup.Text = "支援";
            this.cbSup.UseVisualStyleBackColor = true;
            this.cbSup.CheckedChanged += new System.EventHandler(this.cbAir_CheckedChanged);
            // 
            // cbNb
            // 
            this.cbNb.AutoSize = true;
            this.cbNb.Location = new System.Drawing.Point(130, 64);
            this.cbNb.Name = "cbNb";
            this.cbNb.Size = new System.Drawing.Size(48, 16);
            this.cbNb.TabIndex = 7;
            this.cbNb.Text = "夜战";
            this.cbNb.UseVisualStyleBackColor = true;
            this.cbNb.CheckedChanged += new System.EventHandler(this.cbAir_CheckedChanged);
            // 
            // cbClT
            // 
            this.cbClT.AutoSize = true;
            this.cbClT.Location = new System.Drawing.Point(68, 64);
            this.cbClT.Name = "cbClT";
            this.cbClT.Size = new System.Drawing.Size(60, 16);
            this.cbClT.TabIndex = 6;
            this.cbClT.Text = "闭幕雷";
            this.cbClT.UseVisualStyleBackColor = true;
            this.cbClT.CheckedChanged += new System.EventHandler(this.cbAir_CheckedChanged);
            // 
            // cbSh3
            // 
            this.cbSh3.AutoSize = true;
            this.cbSh3.Location = new System.Drawing.Point(130, 42);
            this.cbSh3.Name = "cbSh3";
            this.cbSh3.Size = new System.Drawing.Size(54, 16);
            this.cbSh3.TabIndex = 5;
            this.cbSh3.Text = "炮击3";
            this.cbSh3.UseVisualStyleBackColor = true;
            this.cbSh3.CheckedChanged += new System.EventHandler(this.cbAir_CheckedChanged);
            // 
            // cbSh2
            // 
            this.cbSh2.AutoSize = true;
            this.cbSh2.Location = new System.Drawing.Point(68, 42);
            this.cbSh2.Name = "cbSh2";
            this.cbSh2.Size = new System.Drawing.Size(54, 16);
            this.cbSh2.TabIndex = 4;
            this.cbSh2.Text = "炮击2";
            this.cbSh2.UseVisualStyleBackColor = true;
            this.cbSh2.CheckedChanged += new System.EventHandler(this.cbAir_CheckedChanged);
            // 
            // cbSh1
            // 
            this.cbSh1.AutoSize = true;
            this.cbSh1.Location = new System.Drawing.Point(6, 42);
            this.cbSh1.Name = "cbSh1";
            this.cbSh1.Size = new System.Drawing.Size(54, 16);
            this.cbSh1.TabIndex = 3;
            this.cbSh1.Text = "炮击1";
            this.cbSh1.UseVisualStyleBackColor = true;
            this.cbSh1.CheckedChanged += new System.EventHandler(this.cbAir_CheckedChanged);
            // 
            // cbOpT
            // 
            this.cbOpT.AutoSize = true;
            this.cbOpT.Location = new System.Drawing.Point(6, 64);
            this.cbOpT.Name = "cbOpT";
            this.cbOpT.Size = new System.Drawing.Size(60, 16);
            this.cbOpT.TabIndex = 2;
            this.cbOpT.Text = "开幕雷";
            this.cbOpT.UseVisualStyleBackColor = true;
            this.cbOpT.CheckedChanged += new System.EventHandler(this.cbAir_CheckedChanged);
            // 
            // cbAir2
            // 
            this.cbAir2.AutoSize = true;
            this.cbAir2.Location = new System.Drawing.Point(68, 20);
            this.cbAir2.Name = "cbAir2";
            this.cbAir2.Size = new System.Drawing.Size(54, 16);
            this.cbAir2.TabIndex = 1;
            this.cbAir2.Text = "航空2";
            this.cbAir2.UseVisualStyleBackColor = true;
            this.cbAir2.CheckedChanged += new System.EventHandler(this.cbAir_CheckedChanged);
            // 
            // cbAir
            // 
            this.cbAir.AutoSize = true;
            this.cbAir.Location = new System.Drawing.Point(6, 20);
            this.cbAir.Name = "cbAir";
            this.cbAir.Size = new System.Drawing.Size(60, 16);
            this.cbAir.TabIndex = 0;
            this.cbAir.Text = "航空战";
            this.cbAir.UseVisualStyleBackColor = true;
            this.cbAir.CheckedChanged += new System.EventHandler(this.cbAir_CheckedChanged);
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Location = new System.Drawing.Point(188, 74);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 19;
            this.button4.Text = "后一月";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Location = new System.Drawing.Point(107, 74);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 18;
            this.button5.Text = "本月";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Location = new System.Drawing.Point(26, 74);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 17;
            this.button6.Text = "前一月";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(148, 18);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(130, 21);
            this.dateTimePicker2.TabIndex = 20;
            this.dateTimePicker2.ValueChanged += new System.EventHandler(this.dateTimePicker2_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(141, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 12);
            this.label3.TabIndex = 21;
            this.label3.Text = "-";
            // 
            // colNo
            // 
            this.colNo.DataPropertyName = "No";
            this.colNo.HeaderText = "Column1";
            this.colNo.Name = "colNo";
            this.colNo.ReadOnly = true;
            this.colNo.Visible = false;
            // 
            // colTime
            // 
            this.colTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colTime.DataPropertyName = "Time";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colTime.DefaultCellStyle = dataGridViewCellStyle12;
            this.colTime.HeaderText = "时间";
            this.colTime.Name = "colTime";
            this.colTime.ReadOnly = true;
            this.colTime.Width = 54;
            // 
            // colArea
            // 
            this.colArea.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colArea.DataPropertyName = "Area";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colArea.DefaultCellStyle = dataGridViewCellStyle13;
            this.colArea.HeaderText = "海域";
            this.colArea.Name = "colArea";
            this.colArea.ReadOnly = true;
            this.colArea.Width = 54;
            // 
            // colPoint
            // 
            this.colPoint.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colPoint.DataPropertyName = "Point";
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colPoint.DefaultCellStyle = dataGridViewCellStyle14;
            this.colPoint.HeaderText = "地图点";
            this.colPoint.Name = "colPoint";
            this.colPoint.ReadOnly = true;
            this.colPoint.Width = 66;
            // 
            // colType
            // 
            this.colType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colType.DataPropertyName = "Types";
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colType.DefaultCellStyle = dataGridViewCellStyle15;
            this.colType.HeaderText = "战斗类型";
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            this.colType.Width = 78;
            // 
            // colFlagship
            // 
            this.colFlagship.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colFlagship.DataPropertyName = "Flagship";
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colFlagship.DefaultCellStyle = dataGridViewCellStyle16;
            this.colFlagship.HeaderText = "旗舰名";
            this.colFlagship.Name = "colFlagship";
            this.colFlagship.ReadOnly = true;
            this.colFlagship.Width = 66;
            // 
            // colMVP
            // 
            this.colMVP.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colMVP.DataPropertyName = "MVP";
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colMVP.DefaultCellStyle = dataGridViewCellStyle17;
            this.colMVP.HeaderText = "MVP";
            this.colMVP.Name = "colMVP";
            this.colMVP.ReadOnly = true;
            this.colMVP.Width = 48;
            // 
            // colBoss
            // 
            this.colBoss.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colBoss.DataPropertyName = "Boss";
            this.colBoss.HeaderText = "BOSS战";
            this.colBoss.Name = "colBoss";
            this.colBoss.ReadOnly = true;
            this.colBoss.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colBoss.Width = 66;
            // 
            // colRank
            // 
            this.colRank.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colRank.DataPropertyName = "Rank";
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colRank.DefaultCellStyle = dataGridViewCellStyle18;
            this.colRank.HeaderText = "评价";
            this.colRank.Name = "colRank";
            this.colRank.ReadOnly = true;
            this.colRank.Width = 54;
            // 
            // colShip
            // 
            this.colShip.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colShip.DataPropertyName = "DropShipName";
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colShip.DefaultCellStyle = dataGridViewCellStyle19;
            this.colShip.HeaderText = "掉落";
            this.colShip.Name = "colShip";
            this.colShip.ReadOnly = true;
            this.colShip.Width = 54;
            // 
            // colText
            // 
            this.colText.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colText.DataPropertyName = "Memo";
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colText.DefaultCellStyle = dataGridViewCellStyle20;
            this.colText.HeaderText = "备注";
            this.colText.Name = "colText";
            this.colText.ReadOnly = true;
            this.colText.Width = 54;
            // 
            // BattleViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 552);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cbBoss);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cbShip);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbArea);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "BattleViewer";
            this.Text = "BattleViewer";
            this.Shown += new System.EventHandler(this.DropViewer_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbArea;
        private System.Windows.Forms.ComboBox cbShip;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox cbBoss;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbOpT;
        private System.Windows.Forms.CheckBox cbAir2;
        private System.Windows.Forms.CheckBox cbAir;
        private System.Windows.Forms.CheckBox cbNb;
        private System.Windows.Forms.CheckBox cbClT;
        private System.Windows.Forms.CheckBox cbSh3;
        private System.Windows.Forms.CheckBox cbSh2;
        private System.Windows.Forms.CheckBox cbSh1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 导出所选记录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导出当前所有记录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置导出文件名格式ToolStripMenuItem;
        private System.Windows.Forms.CheckBox cbSup;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArea;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPoint;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFlagship;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMVP;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colBoss;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRank;
        private System.Windows.Forms.DataGridViewTextBoxColumn colShip;
        private System.Windows.Forms.DataGridViewTextBoxColumn colText;
    }
}