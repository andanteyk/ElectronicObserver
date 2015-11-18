namespace CustomDeck
{
    partial class CustomDeckForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.MenuShip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.更换舰船ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.改造ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.复制CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.粘贴VToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清除DToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenu舰船图鉴 = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuEquipment = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.更换装备ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置等级ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.复制CToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.粘贴VToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.清除DToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.查看图鉴ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ColData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColEquipmentG1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.ColEquipment1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColEquipmentG2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.ColEquipment2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColEquipmentG3 = new System.Windows.Forms.DataGridViewImageColumn();
            this.ColEquipment3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColEquipmentG4 = new System.Windows.Forms.DataGridViewImageColumn();
            this.ColEquipment4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColEquipmentGX = new System.Windows.Forms.DataGridViewImageColumn();
            this.ColEquipmentX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.MenuShip.SuspendLayout();
            this.MenuEquipment.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColData,
            this.ColName,
            this.ColLevel,
            this.ColEquipmentG1,
            this.ColEquipment1,
            this.ColEquipmentG2,
            this.ColEquipment2,
            this.ColEquipmentG3,
            this.ColEquipment3,
            this.ColEquipmentG4,
            this.ColEquipment4,
            this.ColEquipmentGX,
            this.ColEquipmentX});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.Size = new System.Drawing.Size(538, 359);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseClick);
            this.dataGridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
            // 
            // MenuShip
            // 
            this.MenuShip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.更换舰船ToolStripMenuItem,
            this.改造ToolStripMenuItem,
            this.复制CToolStripMenuItem,
            this.粘贴VToolStripMenuItem,
            this.清除DToolStripMenuItem,
            this.toolStripMenu舰船图鉴});
            this.MenuShip.Name = "MenuShip";
            this.MenuShip.Size = new System.Drawing.Size(134, 136);
            this.MenuShip.Opening += new System.ComponentModel.CancelEventHandler(this.MenuShip_Opening);
            // 
            // 更换舰船ToolStripMenuItem
            // 
            this.更换舰船ToolStripMenuItem.Name = "更换舰船ToolStripMenuItem";
            this.更换舰船ToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.更换舰船ToolStripMenuItem.Text = "更换舰船...";
            this.更换舰船ToolStripMenuItem.Click += new System.EventHandler(this.更换舰船ToolStripMenuItem_Click);
            // 
            // 改造ToolStripMenuItem
            // 
            this.改造ToolStripMenuItem.Name = "改造ToolStripMenuItem";
            this.改造ToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.改造ToolStripMenuItem.Text = "改造";
            // 
            // 复制CToolStripMenuItem
            // 
            this.复制CToolStripMenuItem.Enabled = false;
            this.复制CToolStripMenuItem.Name = "复制CToolStripMenuItem";
            this.复制CToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.复制CToolStripMenuItem.Text = "复制(&C)";
            // 
            // 粘贴VToolStripMenuItem
            // 
            this.粘贴VToolStripMenuItem.Enabled = false;
            this.粘贴VToolStripMenuItem.Name = "粘贴VToolStripMenuItem";
            this.粘贴VToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.粘贴VToolStripMenuItem.Text = "粘贴(&V)";
            // 
            // 清除DToolStripMenuItem
            // 
            this.清除DToolStripMenuItem.Name = "清除DToolStripMenuItem";
            this.清除DToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.清除DToolStripMenuItem.Text = "清除(&D)";
            this.清除DToolStripMenuItem.Click += new System.EventHandler(this.清除舰船_Click);
            // 
            // toolStripMenu舰船图鉴
            // 
            this.toolStripMenu舰船图鉴.Name = "toolStripMenu舰船图鉴";
            this.toolStripMenu舰船图鉴.Size = new System.Drawing.Size(133, 22);
            this.toolStripMenu舰船图鉴.Text = "查看图鉴";
            this.toolStripMenu舰船图鉴.Click += new System.EventHandler(this.toolStrip舰船图鉴_Click);
            // 
            // MenuEquipment
            // 
            this.MenuEquipment.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.更换装备ToolStripMenuItem,
            this.设置等级ToolStripMenuItem,
            this.复制CToolStripMenuItem1,
            this.粘贴VToolStripMenuItem1,
            this.清除DToolStripMenuItem1,
            this.查看图鉴ToolStripMenuItem});
            this.MenuEquipment.Name = "contextMenuStrip1";
            this.MenuEquipment.Size = new System.Drawing.Size(134, 136);
            this.MenuEquipment.Opening += new System.ComponentModel.CancelEventHandler(this.MenuEquipment_Opening);
            // 
            // 更换装备ToolStripMenuItem
            // 
            this.更换装备ToolStripMenuItem.Name = "更换装备ToolStripMenuItem";
            this.更换装备ToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.更换装备ToolStripMenuItem.Text = "更换装备...";
            this.更换装备ToolStripMenuItem.Click += new System.EventHandler(this.更换装备ToolStripMenuItem_Click);
            // 
            // 设置等级ToolStripMenuItem
            // 
            this.设置等级ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.toolStripMenuItem7,
            this.toolStripMenuItem8});
            this.设置等级ToolStripMenuItem.Name = "设置等级ToolStripMenuItem";
            this.设置等级ToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.设置等级ToolStripMenuItem.Text = "设置等级";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(83, 22);
            this.toolStripMenuItem1.Tag = "0";
            this.toolStripMenuItem1.Text = "0";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.SetLevel_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(83, 22);
            this.toolStripMenuItem2.Tag = "1";
            this.toolStripMenuItem2.Text = "1";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.SetLevel_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(83, 22);
            this.toolStripMenuItem3.Tag = "2";
            this.toolStripMenuItem3.Text = "2";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.SetLevel_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(83, 22);
            this.toolStripMenuItem4.Tag = "3";
            this.toolStripMenuItem4.Text = "3";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.SetLevel_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(83, 22);
            this.toolStripMenuItem5.Tag = "4";
            this.toolStripMenuItem5.Text = "4";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.SetLevel_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(83, 22);
            this.toolStripMenuItem6.Tag = "5";
            this.toolStripMenuItem6.Text = "5";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.SetLevel_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(83, 22);
            this.toolStripMenuItem7.Tag = "6";
            this.toolStripMenuItem7.Text = "6";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.SetLevel_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(83, 22);
            this.toolStripMenuItem8.Tag = "7";
            this.toolStripMenuItem8.Text = "7";
            this.toolStripMenuItem8.Click += new System.EventHandler(this.SetLevel_Click);
            // 
            // 复制CToolStripMenuItem1
            // 
            this.复制CToolStripMenuItem1.Enabled = false;
            this.复制CToolStripMenuItem1.Name = "复制CToolStripMenuItem1";
            this.复制CToolStripMenuItem1.Size = new System.Drawing.Size(133, 22);
            this.复制CToolStripMenuItem1.Text = "复制(&C)";
            // 
            // 粘贴VToolStripMenuItem1
            // 
            this.粘贴VToolStripMenuItem1.Enabled = false;
            this.粘贴VToolStripMenuItem1.Name = "粘贴VToolStripMenuItem1";
            this.粘贴VToolStripMenuItem1.Size = new System.Drawing.Size(133, 22);
            this.粘贴VToolStripMenuItem1.Text = "粘贴(&V)";
            // 
            // 清除DToolStripMenuItem1
            // 
            this.清除DToolStripMenuItem1.Name = "清除DToolStripMenuItem1";
            this.清除DToolStripMenuItem1.Size = new System.Drawing.Size(133, 22);
            this.清除DToolStripMenuItem1.Text = "清除(&D)";
            this.清除DToolStripMenuItem1.Click += new System.EventHandler(this.清除装备_Click);
            // 
            // 查看图鉴ToolStripMenuItem
            // 
            this.查看图鉴ToolStripMenuItem.Name = "查看图鉴ToolStripMenuItem";
            this.查看图鉴ToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.查看图鉴ToolStripMenuItem.Text = "查看图鉴";
            this.查看图鉴ToolStripMenuItem.Click += new System.EventHandler(this.查看图鉴ToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // ColData
            // 
            this.ColData.HeaderText = "Column1";
            this.ColData.Name = "ColData";
            this.ColData.ReadOnly = true;
            this.ColData.Visible = false;
            // 
            // ColName
            // 
            this.ColName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            this.ColName.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColName.HeaderText = "名称";
            this.ColName.Name = "ColName";
            this.ColName.ReadOnly = true;
            this.ColName.Width = 54;
            // 
            // ColLevel
            // 
            this.ColLevel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            this.ColLevel.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColLevel.HeaderText = "等级";
            this.ColLevel.Name = "ColLevel";
            this.ColLevel.ReadOnly = true;
            this.ColLevel.Width = 54;
            // 
            // ColEquipmentG1
            // 
            this.ColEquipmentG1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColEquipmentG1.HeaderText = "装备1";
            this.ColEquipmentG1.Name = "ColEquipmentG1";
            this.ColEquipmentG1.ReadOnly = true;
            this.ColEquipmentG1.Visible = false;
            this.ColEquipmentG1.Width = 41;
            // 
            // ColEquipment1
            // 
            this.ColEquipment1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColEquipment1.HeaderText = "装备1";
            this.ColEquipment1.Name = "ColEquipment1";
            this.ColEquipment1.ReadOnly = true;
            this.ColEquipment1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColEquipment1.Width = 60;
            // 
            // ColEquipmentG2
            // 
            this.ColEquipmentG2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColEquipmentG2.HeaderText = "装备2";
            this.ColEquipmentG2.Name = "ColEquipmentG2";
            this.ColEquipmentG2.ReadOnly = true;
            this.ColEquipmentG2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColEquipmentG2.Visible = false;
            this.ColEquipmentG2.Width = 41;
            // 
            // ColEquipment2
            // 
            this.ColEquipment2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColEquipment2.HeaderText = "装备2";
            this.ColEquipment2.Name = "ColEquipment2";
            this.ColEquipment2.ReadOnly = true;
            this.ColEquipment2.Width = 60;
            // 
            // ColEquipmentG3
            // 
            this.ColEquipmentG3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColEquipmentG3.HeaderText = "装备3";
            this.ColEquipmentG3.Name = "ColEquipmentG3";
            this.ColEquipmentG3.ReadOnly = true;
            this.ColEquipmentG3.Visible = false;
            this.ColEquipmentG3.Width = 41;
            // 
            // ColEquipment3
            // 
            this.ColEquipment3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColEquipment3.HeaderText = "装备3";
            this.ColEquipment3.Name = "ColEquipment3";
            this.ColEquipment3.ReadOnly = true;
            this.ColEquipment3.Width = 60;
            // 
            // ColEquipmentG4
            // 
            this.ColEquipmentG4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColEquipmentG4.HeaderText = "装备4";
            this.ColEquipmentG4.Name = "ColEquipmentG4";
            this.ColEquipmentG4.ReadOnly = true;
            this.ColEquipmentG4.Visible = false;
            this.ColEquipmentG4.Width = 41;
            // 
            // ColEquipment4
            // 
            this.ColEquipment4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColEquipment4.HeaderText = "装备4";
            this.ColEquipment4.Name = "ColEquipment4";
            this.ColEquipment4.ReadOnly = true;
            this.ColEquipment4.Width = 60;
            // 
            // ColEquipmentGX
            // 
            this.ColEquipmentGX.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColEquipmentGX.HeaderText = "补强";
            this.ColEquipmentGX.Name = "ColEquipmentGX";
            this.ColEquipmentGX.ReadOnly = true;
            this.ColEquipmentGX.Visible = false;
            this.ColEquipmentGX.Width = 35;
            // 
            // ColEquipmentX
            // 
            this.ColEquipmentX.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColEquipmentX.HeaderText = "补强";
            this.ColEquipmentX.Name = "ColEquipmentX";
            this.ColEquipmentX.ReadOnly = true;
            this.ColEquipmentX.Width = 54;
            // 
            // CustomDeckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 359);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CustomDeckForm";
            this.Text = "自定义编成";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.MenuShip.ResumeLayout(false);
            this.MenuEquipment.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip MenuShip;
        private System.Windows.Forms.ContextMenuStrip MenuEquipment;
        private System.Windows.Forms.ToolStripMenuItem 更换舰船ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 改造ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 复制CToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 粘贴VToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清除DToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 更换装备ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置等级ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem 复制CToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 粘贴VToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 清除DToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenu舰船图鉴;
        private System.Windows.Forms.ToolStripMenuItem 查看图鉴ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColData;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColLevel;
        private System.Windows.Forms.DataGridViewImageColumn ColEquipmentG1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColEquipment1;
        private System.Windows.Forms.DataGridViewImageColumn ColEquipmentG2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColEquipment2;
        private System.Windows.Forms.DataGridViewImageColumn ColEquipmentG3;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColEquipment3;
        private System.Windows.Forms.DataGridViewImageColumn ColEquipmentG4;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColEquipment4;
        private System.Windows.Forms.DataGridViewImageColumn ColEquipmentGX;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColEquipmentX;

    }
}

