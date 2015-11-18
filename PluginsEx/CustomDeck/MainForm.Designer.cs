namespace CustomDeck
{
    partial class DeckMainForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button3 = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button1 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.新增编成ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除编成ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导入游戏当前舰队ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.上移CtrlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.下移CtrlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.导入游戏舰队数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导入剪切板数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.cbImageShow = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.contextMenuStrip3.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.button3);
            this.splitContainer1.Panel1.Controls.Add(this.listView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.cbImageShow);
            this.splitContainer1.Panel2.Controls.Add(this.button1);
            this.splitContainer1.Panel2.Controls.Add(this.button4);
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel2.Enabled = false;
            this.splitContainer1.Size = new System.Drawing.Size(794, 456);
            this.splitContainer1.SplitterDistance = 226;
            this.splitContainer1.TabIndex = 4;
            // 
            // button3
            // 
            this.button3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button3.Location = new System.Drawing.Point(12, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "操作";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(3, 41);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.ShowItemToolTips = true;
            this.listView1.Size = new System.Drawing.Size(220, 412);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "配置名称";
            this.columnHeader1.Width = 200;
            // 
            // button1
            // 
            this.button1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button1.Location = new System.Drawing.Point(109, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(69, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "导出";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button4
            // 
            this.button4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button4.Location = new System.Drawing.Point(19, 12);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(69, 23);
            this.button4.TabIndex = 9;
            this.button4.Text = "导入";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(3, 41);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(557, 415);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(549, 389);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "第一舰队";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(549, 389);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "第二舰队";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(549, 389);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "第三舰队";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(549, 389);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "第四舰队";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.新增编成ToolStripMenuItem,
            this.删除编成ToolStripMenuItem,
            this.导入游戏当前舰队ToolStripMenuItem,
            this.上移CtrlToolStripMenuItem,
            this.下移CtrlToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(182, 136);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(181, 22);
            this.toolStripMenuItem1.Text = "更改名称...";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // 新增编成ToolStripMenuItem
            // 
            this.新增编成ToolStripMenuItem.Name = "新增编成ToolStripMenuItem";
            this.新增编成ToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.新增编成ToolStripMenuItem.Text = "新增编成...";
            this.新增编成ToolStripMenuItem.Click += new System.EventHandler(this.新增编成ToolStripMenuItem_Click);
            // 
            // 删除编成ToolStripMenuItem
            // 
            this.删除编成ToolStripMenuItem.Name = "删除编成ToolStripMenuItem";
            this.删除编成ToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.删除编成ToolStripMenuItem.Text = "删除编成";
            this.删除编成ToolStripMenuItem.Click += new System.EventHandler(this.删除编成ToolStripMenuItem_Click);
            // 
            // 导入游戏当前舰队ToolStripMenuItem
            // 
            this.导入游戏当前舰队ToolStripMenuItem.Name = "导入游戏当前舰队ToolStripMenuItem";
            this.导入游戏当前舰队ToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.导入游戏当前舰队ToolStripMenuItem.Text = "导入游戏当前舰队...";
            this.导入游戏当前舰队ToolStripMenuItem.Click += new System.EventHandler(this.导入游戏当前舰队ToolStripMenuItem_Click);
            // 
            // 上移CtrlToolStripMenuItem
            // 
            this.上移CtrlToolStripMenuItem.Name = "上移CtrlToolStripMenuItem";
            this.上移CtrlToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.上移CtrlToolStripMenuItem.Text = "上移(Ctrl+↑)";
            this.上移CtrlToolStripMenuItem.Click += new System.EventHandler(this.上移CtrlToolStripMenuItem_Click);
            // 
            // 下移CtrlToolStripMenuItem
            // 
            this.下移CtrlToolStripMenuItem.Name = "下移CtrlToolStripMenuItem";
            this.下移CtrlToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.下移CtrlToolStripMenuItem.Text = "下移(Ctrl+↓)";
            this.下移CtrlToolStripMenuItem.Click += new System.EventHandler(this.下移CtrlToolStripMenuItem_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导入游戏舰队数据ToolStripMenuItem,
            this.导入剪切板数据ToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(173, 48);
            // 
            // 导入游戏舰队数据ToolStripMenuItem
            // 
            this.导入游戏舰队数据ToolStripMenuItem.Name = "导入游戏舰队数据ToolStripMenuItem";
            this.导入游戏舰队数据ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.导入游戏舰队数据ToolStripMenuItem.Text = "导入游戏舰队数据";
            this.导入游戏舰队数据ToolStripMenuItem.Click += new System.EventHandler(this.button4_Click);
            // 
            // 导入剪切板数据ToolStripMenuItem
            // 
            this.导入剪切板数据ToolStripMenuItem.Name = "导入剪切板数据ToolStripMenuItem";
            this.导入剪切板数据ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.导入剪切板数据ToolStripMenuItem.Text = "导入剪切板数据";
            this.导入剪切板数据ToolStripMenuItem.Click += new System.EventHandler(this.button1_Click);
            // 
            // contextMenuStrip3
            // 
            this.contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem4,
            this.toolStripMenuItem5});
            this.contextMenuStrip3.Name = "contextMenuStrip2";
            this.contextMenuStrip3.Size = new System.Drawing.Size(233, 48);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(232, 22);
            this.toolStripMenuItem4.Text = "导出配置至剪切板";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.button2_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(232, 22);
            this.toolStripMenuItem5.Text = "导出至艦これ電子計算儀倉庫";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.button5_Click);
            // 
            // cbImageShow
            // 
            this.cbImageShow.AutoSize = true;
            this.cbImageShow.Location = new System.Drawing.Point(198, 17);
            this.cbImageShow.Name = "cbImageShow";
            this.cbImageShow.Size = new System.Drawing.Size(72, 16);
            this.cbImageShow.TabIndex = 11;
            this.cbImageShow.Text = "显示图标";
            this.cbImageShow.UseVisualStyleBackColor = true;
            this.cbImageShow.CheckedChanged += new System.EventHandler(this.cbImageShow_CheckedChanged);
            // 
            // DeckMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 456);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HideOnClose = true;
            this.Name = "DeckMainForm";
            this.Text = "历史编成";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.contextMenuStrip3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ToolStripMenuItem 新增编成ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除编成ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导入游戏当前舰队ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem 导入游戏舰队数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导入剪切板数据ToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem 上移CtrlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 下移CtrlToolStripMenuItem;
        private System.Windows.Forms.CheckBox cbImageShow;

    }
}