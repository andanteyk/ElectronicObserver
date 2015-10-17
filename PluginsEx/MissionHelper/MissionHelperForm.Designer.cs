namespace MissionHelper
{
    partial class MissionHelperForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFleet2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFleet3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFleet4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.完整列表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.远征过滤ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cbAutoFill = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colID,
            this.colName,
            this.colFleet2,
            this.colFleet3,
            this.colFleet4});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.ButtonFace;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.Size = new System.Drawing.Size(329, 575);
            this.dataGridView1.TabIndex = 0;
            // 
            // colID
            // 
            this.colID.HeaderText = "ID";
            this.colID.Name = "colID";
            this.colID.ReadOnly = true;
            this.colID.Width = 25;
            // 
            // colName
            // 
            this.colName.HeaderText = "远征名";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Width = 130;
            // 
            // colFleet2
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colFleet2.DefaultCellStyle = dataGridViewCellStyle2;
            this.colFleet2.HeaderText = "二";
            this.colFleet2.Name = "colFleet2";
            this.colFleet2.ReadOnly = true;
            this.colFleet2.Width = 25;
            // 
            // colFleet3
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colFleet3.DefaultCellStyle = dataGridViewCellStyle3;
            this.colFleet3.HeaderText = "三";
            this.colFleet3.Name = "colFleet3";
            this.colFleet3.ReadOnly = true;
            this.colFleet3.Width = 25;
            // 
            // colFleet4
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colFleet4.DefaultCellStyle = dataGridViewCellStyle4;
            this.colFleet4.HeaderText = "四";
            this.colFleet4.Name = "colFleet4";
            this.colFleet4.ReadOnly = true;
            this.colFleet4.Width = 25;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.完整列表ToolStripMenuItem,
            this.远征过滤ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 70);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(148, 22);
            this.toolStripMenuItem2.Text = "自动匹配宽度";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // 完整列表ToolStripMenuItem
            // 
            this.完整列表ToolStripMenuItem.Name = "完整列表ToolStripMenuItem";
            this.完整列表ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.完整列表ToolStripMenuItem.Text = "完整列表...";
            this.完整列表ToolStripMenuItem.Click += new System.EventHandler(this.完整列表ToolStripMenuItem_Click);
            // 
            // 远征过滤ToolStripMenuItem
            // 
            this.远征过滤ToolStripMenuItem.Name = "远征过滤ToolStripMenuItem";
            this.远征过滤ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.远征过滤ToolStripMenuItem.Text = "远征过滤...";
            this.远征过滤ToolStripMenuItem.Click += new System.EventHandler(this.远征过滤ToolStripMenuItem_Click);
            // 
            // cbAutoFill
            // 
            this.cbAutoFill.AutoSize = true;
            this.cbAutoFill.Location = new System.Drawing.Point(142, 21);
            this.cbAutoFill.Name = "cbAutoFill";
            this.cbAutoFill.Size = new System.Drawing.Size(96, 16);
            this.cbAutoFill.TabIndex = 4;
            this.cbAutoFill.Text = "自动匹配宽度";
            this.cbAutoFill.UseVisualStyleBackColor = true;
            this.cbAutoFill.Visible = false;
            this.cbAutoFill.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // MissionHelperForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 575);
            this.Controls.Add(this.cbAutoFill);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MinimizeBox = false;
            this.Name = "MissionHelperForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "远征助手";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MissionHelperForm_FormClosing);
            this.Load += new System.EventHandler(this.MissionHelperForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.CheckBox cbAutoFill;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFleet2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFleet3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFleet4;
        private System.Windows.Forms.ToolStripMenuItem 完整列表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 远征过滤ToolStripMenuItem;
    }
}

