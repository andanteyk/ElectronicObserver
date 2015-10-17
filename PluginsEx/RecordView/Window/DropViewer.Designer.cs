namespace RecordView
{
    partial class DropViewer
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.cbArea = new System.Windows.Forms.ComboBox();
            this.cbShip = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbBOSS = new System.Windows.Forms.CheckBox();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPoint = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBoss = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colShip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEquipment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDif = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLv = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeSelection1 = new RecordView.TimeSelection();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
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
            this.colTime,
            this.colArea,
            this.colPoint,
            this.colBoss,
            this.colRank,
            this.colShip,
            this.colItem,
            this.colEquipment,
            this.colDif,
            this.colLv});
            this.dataGridView1.Location = new System.Drawing.Point(2, 109);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.Size = new System.Drawing.Size(633, 442);
            this.dataGridView1.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(296, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "海域选择";
            // 
            // cbArea
            // 
            this.cbArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbArea.FormattingEnabled = true;
            this.cbArea.Location = new System.Drawing.Point(354, 27);
            this.cbArea.Name = "cbArea";
            this.cbArea.Size = new System.Drawing.Size(121, 20);
            this.cbArea.TabIndex = 8;
            this.cbArea.TextChanged += new System.EventHandler(this.cbArea_TextChanged);
            // 
            // cbShip
            // 
            this.cbShip.FormattingEnabled = true;
            this.cbShip.Location = new System.Drawing.Point(354, 63);
            this.cbShip.Name = "cbShip";
            this.cbShip.Size = new System.Drawing.Size(121, 20);
            this.cbShip.TabIndex = 10;
            this.cbShip.TextChanged += new System.EventHandler(this.cbShip_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(306, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "掉落舰";
            // 
            // cbBOSS
            // 
            this.cbBOSS.AutoSize = true;
            this.cbBOSS.Location = new System.Drawing.Point(505, 30);
            this.cbBOSS.Name = "cbBOSS";
            this.cbBOSS.Size = new System.Drawing.Size(60, 16);
            this.cbBOSS.TabIndex = 11;
            this.cbBOSS.Text = "BOSS点";
            this.cbBOSS.UseVisualStyleBackColor = true;
            this.cbBOSS.CheckedChanged += new System.EventHandler(this.cbBOSS_CheckedChanged);
            // 
            // colTime
            // 
            this.colTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colTime.DataPropertyName = "Time";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colTime.DefaultCellStyle = dataGridViewCellStyle2;
            this.colTime.HeaderText = "时间";
            this.colTime.Name = "colTime";
            this.colTime.ReadOnly = true;
            this.colTime.Width = 54;
            // 
            // colArea
            // 
            this.colArea.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colArea.DataPropertyName = "Area";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colArea.DefaultCellStyle = dataGridViewCellStyle3;
            this.colArea.HeaderText = "海域";
            this.colArea.Name = "colArea";
            this.colArea.ReadOnly = true;
            this.colArea.Width = 54;
            // 
            // colPoint
            // 
            this.colPoint.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colPoint.DataPropertyName = "Point";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colPoint.DefaultCellStyle = dataGridViewCellStyle4;
            this.colPoint.HeaderText = "地图点";
            this.colPoint.Name = "colPoint";
            this.colPoint.ReadOnly = true;
            this.colPoint.Width = 66;
            // 
            // colBoss
            // 
            this.colBoss.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colBoss.DataPropertyName = "Boss";
            this.colBoss.HeaderText = "Boss点";
            this.colBoss.Name = "colBoss";
            this.colBoss.ReadOnly = true;
            this.colBoss.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colBoss.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colBoss.Width = 66;
            // 
            // colRank
            // 
            this.colRank.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colRank.DataPropertyName = "Rank";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colRank.DefaultCellStyle = dataGridViewCellStyle5;
            this.colRank.HeaderText = "评价";
            this.colRank.Name = "colRank";
            this.colRank.ReadOnly = true;
            this.colRank.Width = 54;
            // 
            // colShip
            // 
            this.colShip.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colShip.DataPropertyName = "DropShipName";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colShip.DefaultCellStyle = dataGridViewCellStyle6;
            this.colShip.HeaderText = "掉落";
            this.colShip.Name = "colShip";
            this.colShip.ReadOnly = true;
            this.colShip.Width = 54;
            // 
            // colItem
            // 
            this.colItem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colItem.DataPropertyName = "ItemName";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colItem.DefaultCellStyle = dataGridViewCellStyle7;
            this.colItem.HeaderText = "物品";
            this.colItem.Name = "colItem";
            this.colItem.ReadOnly = true;
            this.colItem.Width = 54;
            // 
            // colEquipment
            // 
            this.colEquipment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colEquipment.DataPropertyName = "EquipmentName";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colEquipment.DefaultCellStyle = dataGridViewCellStyle8;
            this.colEquipment.HeaderText = "装备";
            this.colEquipment.Name = "colEquipment";
            this.colEquipment.ReadOnly = true;
            this.colEquipment.Width = 54;
            // 
            // colDif
            // 
            this.colDif.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colDif.DataPropertyName = "Difficulty";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colDif.DefaultCellStyle = dataGridViewCellStyle9;
            this.colDif.HeaderText = "难度";
            this.colDif.Name = "colDif";
            this.colDif.ReadOnly = true;
            this.colDif.Width = 54;
            // 
            // colLv
            // 
            this.colLv.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colLv.DataPropertyName = "Lv";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colLv.DefaultCellStyle = dataGridViewCellStyle10;
            this.colLv.HeaderText = "司令部等级";
            this.colLv.Name = "colLv";
            this.colLv.ReadOnly = true;
            this.colLv.Width = 90;
            // 
            // timeSelection1
            // 
            this.timeSelection1.Location = new System.Drawing.Point(2, 1);
            this.timeSelection1.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.timeSelection1.Name = "timeSelection1";
            this.timeSelection1.Since = new System.DateTime(((long)(0)));
            this.timeSelection1.Size = new System.Drawing.Size(280, 103);
            this.timeSelection1.TabIndex = 5;
            this.timeSelection1.Until = new System.DateTime(((long)(0)));
            this.timeSelection1.DateChanged += new System.EventHandler(this.timeSelection1_DateChanged);
            // 
            // DropViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 552);
            this.Controls.Add(this.cbBOSS);
            this.Controls.Add(this.cbShip);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbArea);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.timeSelection1);
            this.Name = "DropViewer";
            this.Text = "DropViewer";
            this.Shown += new System.EventHandler(this.DropViewer_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TimeSelection timeSelection1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbArea;
        private System.Windows.Forms.ComboBox cbShip;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbBOSS;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArea;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPoint;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colBoss;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRank;
        private System.Windows.Forms.DataGridViewTextBoxColumn colShip;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEquipment;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDif;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLv;
    }
}