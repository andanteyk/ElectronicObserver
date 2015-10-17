namespace RecordView
{
    partial class ConstructionViewer
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
            this.cbShip = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbBOSS = new System.Windows.Forms.CheckBox();
            this.timeSelection1 = new RecordView.TimeSelection();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colShip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPoint = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBoss = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colDif = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLv = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.colShip,
            this.colArea,
            this.colPoint,
            this.colRank,
            this.colAl,
            this.colMaterial,
            this.colBoss,
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
            // cbShip
            // 
            this.cbShip.FormattingEnabled = true;
            this.cbShip.Location = new System.Drawing.Point(353, 45);
            this.cbShip.Name = "cbShip";
            this.cbShip.Size = new System.Drawing.Size(121, 20);
            this.cbShip.TabIndex = 10;
            this.cbShip.TextChanged += new System.EventHandler(this.cbShip_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(305, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "舰船名";
            // 
            // cbBOSS
            // 
            this.cbBOSS.AutoSize = true;
            this.cbBOSS.Location = new System.Drawing.Point(502, 46);
            this.cbBOSS.Name = "cbBOSS";
            this.cbBOSS.Size = new System.Drawing.Size(84, 16);
            this.cbBOSS.TabIndex = 11;
            this.cbBOSS.Text = "大型舰建造";
            this.cbBOSS.UseVisualStyleBackColor = true;
            this.cbBOSS.CheckedChanged += new System.EventHandler(this.cbBOSS_CheckedChanged);
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
            // colShip
            // 
            this.colShip.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colShip.DataPropertyName = "ShipName";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colShip.DefaultCellStyle = dataGridViewCellStyle3;
            this.colShip.HeaderText = "舰船名";
            this.colShip.Name = "colShip";
            this.colShip.ReadOnly = true;
            this.colShip.Width = 66;
            // 
            // colArea
            // 
            this.colArea.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colArea.DataPropertyName = "Fuel";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colArea.DefaultCellStyle = dataGridViewCellStyle4;
            this.colArea.HeaderText = "燃料";
            this.colArea.Name = "colArea";
            this.colArea.ReadOnly = true;
            this.colArea.Width = 54;
            // 
            // colPoint
            // 
            this.colPoint.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colPoint.DataPropertyName = "Ammo";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colPoint.DefaultCellStyle = dataGridViewCellStyle5;
            this.colPoint.HeaderText = "弹药";
            this.colPoint.Name = "colPoint";
            this.colPoint.ReadOnly = true;
            this.colPoint.Width = 54;
            // 
            // colRank
            // 
            this.colRank.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colRank.DataPropertyName = "Steel";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colRank.DefaultCellStyle = dataGridViewCellStyle6;
            this.colRank.HeaderText = "钢材";
            this.colRank.Name = "colRank";
            this.colRank.ReadOnly = true;
            this.colRank.Width = 54;
            // 
            // colAl
            // 
            this.colAl.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colAl.DataPropertyName = "Al";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colAl.DefaultCellStyle = dataGridViewCellStyle7;
            this.colAl.HeaderText = "铝材";
            this.colAl.Name = "colAl";
            this.colAl.ReadOnly = true;
            this.colAl.Width = 54;
            // 
            // colMaterial
            // 
            this.colMaterial.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colMaterial.DataPropertyName = "Material";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colMaterial.DefaultCellStyle = dataGridViewCellStyle8;
            this.colMaterial.HeaderText = "资材";
            this.colMaterial.Name = "colMaterial";
            this.colMaterial.ReadOnly = true;
            this.colMaterial.Width = 54;
            // 
            // colBoss
            // 
            this.colBoss.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colBoss.DataPropertyName = "GreatConstruction";
            this.colBoss.HeaderText = "大建";
            this.colBoss.Name = "colBoss";
            this.colBoss.ReadOnly = true;
            this.colBoss.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colBoss.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colBoss.Width = 54;
            // 
            // colDif
            // 
            this.colDif.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colDif.DataPropertyName = "Secretary";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colDif.DefaultCellStyle = dataGridViewCellStyle9;
            this.colDif.HeaderText = "秘书舰";
            this.colDif.Name = "colDif";
            this.colDif.ReadOnly = true;
            this.colDif.Width = 66;
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
            // ConstructionViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 552);
            this.Controls.Add(this.cbBOSS);
            this.Controls.Add(this.cbShip);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.timeSelection1);
            this.Name = "ConstructionViewer";
            this.Text = "ConstructionViewer";
            this.Shown += new System.EventHandler(this.DropViewer_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TimeSelection timeSelection1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox cbShip;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbBOSS;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colShip;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArea;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPoint;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRank;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAl;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterial;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colBoss;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDif;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLv;
    }
}