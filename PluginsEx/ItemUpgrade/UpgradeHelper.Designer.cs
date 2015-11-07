namespace ItemUpgrade
{
    partial class UpgradeHelper
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "",
            "1111",
            "1111",
            "2222",
            ""}, -1);
            this.lvList = new System.Windows.Forms.ListView();
            this.colGraph = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colShip = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMidnight = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.cmbDayofWeek = new System.Windows.Forms.ComboBox();
            this.btnNow = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cbMidnight = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lvList
            // 
            this.lvList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colGraph,
            this.colType,
            this.colName,
            this.colShip,
            this.colMidnight});
            this.lvList.FullRowSelect = true;
            this.lvList.GridLines = true;
            this.lvList.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.lvList.LargeImageList = this.imageList1;
            this.lvList.Location = new System.Drawing.Point(2, 41);
            this.lvList.MultiSelect = false;
            this.lvList.Name = "lvList";
            this.lvList.ShowItemToolTips = true;
            this.lvList.Size = new System.Drawing.Size(558, 318);
            this.lvList.SmallImageList = this.imageList1;
            this.lvList.TabIndex = 0;
            this.lvList.UseCompatibleStateImageBehavior = false;
            this.lvList.View = System.Windows.Forms.View.Details;
            this.lvList.SelectedIndexChanged += new System.EventHandler(this.lvList_SelectedIndexChanged);
            // 
            // colGraph
            // 
            this.colGraph.Text = "";
            this.colGraph.Width = 27;
            // 
            // colType
            // 
            this.colType.Text = "类型";
            this.colType.Width = 111;
            // 
            // colName
            // 
            this.colName.Text = "名称";
            this.colName.Width = 213;
            // 
            // colShip
            // 
            this.colShip.Text = "二号舰";
            this.colShip.Width = 131;
            // 
            // colMidnight
            // 
            this.colMidnight.Text = "";
            this.colMidnight.Width = 70;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "周日选择";
            // 
            // cmbDayofWeek
            // 
            this.cmbDayofWeek.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDayofWeek.FormattingEnabled = true;
            this.cmbDayofWeek.Items.AddRange(new object[] {
            "星期日",
            "星期一",
            "星期二",
            "星期三",
            "星期四",
            "星期五",
            "星期六"});
            this.cmbDayofWeek.Location = new System.Drawing.Point(64, 8);
            this.cmbDayofWeek.Name = "cmbDayofWeek";
            this.cmbDayofWeek.Size = new System.Drawing.Size(78, 20);
            this.cmbDayofWeek.TabIndex = 2;
            this.cmbDayofWeek.TextChanged += new System.EventHandler(this.cmbDayofWeek_TextChanged);
            // 
            // btnNow
            // 
            this.btnNow.Location = new System.Drawing.Point(148, 6);
            this.btnNow.Name = "btnNow";
            this.btnNow.Size = new System.Drawing.Size(63, 23);
            this.btnNow.TabIndex = 3;
            this.btnNow.Text = "今天";
            this.btnNow.UseVisualStyleBackColor = true;
            this.btnNow.Click += new System.EventHandler(this.btnNow_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(474, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "过滤设置";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(422, 12);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(48, 16);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "过滤";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(322, 12);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(96, 16);
            this.checkBox2.TabIndex = 6;
            this.checkBox2.Text = "我能改修哪些";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.Location = new System.Drawing.Point(2, 361);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.ShowItemToolTips = true;
            this.listView1.Size = new System.Drawing.Size(558, 80);
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.TabIndex = 7;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "阶段";
            this.columnHeader1.Width = 54;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "消耗螺丝";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 111;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "消耗装备";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 196;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "更新装备";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 175;
            // 
            // cbMidnight
            // 
            this.cbMidnight.AutoSize = true;
            this.cbMidnight.Location = new System.Drawing.Point(246, 12);
            this.cbMidnight.Name = "cbMidnight";
            this.cbMidnight.Size = new System.Drawing.Size(72, 16);
            this.cbMidnight.TabIndex = 8;
            this.cbMidnight.Text = "半夜改修";
            this.cbMidnight.UseVisualStyleBackColor = true;
            this.cbMidnight.CheckedChanged += new System.EventHandler(this.cbMidnight_CheckedChanged);
            // 
            // UpgradeHelper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 442);
            this.Controls.Add(this.cbMidnight);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnNow);
            this.Controls.Add(this.cmbDayofWeek);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lvList);
            this.Name = "UpgradeHelper";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "装备改修助手";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.UpgradeHelper_FormClosed);
            this.Load += new System.EventHandler(this.UpgradeHelper_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbDayofWeek;
        private System.Windows.Forms.Button btnNow;
        private System.Windows.Forms.ColumnHeader colType;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colShip;
        private System.Windows.Forms.ColumnHeader colGraph;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.CheckBox cbMidnight;
        private System.Windows.Forms.ColumnHeader colMidnight;
    }
}

