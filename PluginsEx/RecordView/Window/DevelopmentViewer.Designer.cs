namespace RecordView
{
    partial class DevelopmentViewer
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPoint = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSecretary = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLv = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cbShip = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.timeSelection1 = new RecordView.TimeSelection();
            this.Fmin = new System.Windows.Forms.TextBox();
            this.Fmax = new System.Windows.Forms.TextBox();
            this.Amax = new System.Windows.Forms.TextBox();
            this.Amin = new System.Windows.Forms.TextBox();
            this.Smax = new System.Windows.Forms.TextBox();
            this.Smin = new System.Windows.Forms.TextBox();
            this.Xmax = new System.Windows.Forms.TextBox();
            this.Xmin = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
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
            this.colName,
            this.colArea,
            this.colPoint,
            this.colRank,
            this.colAl,
            this.colSecretary,
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
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colName.DataPropertyName = "Name";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colName.DefaultCellStyle = dataGridViewCellStyle3;
            this.colName.HeaderText = "装备名";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Width = 66;
            // 
            // colArea
            // 
            this.colArea.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colArea.DataPropertyName = "Fuel";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
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
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
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
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
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
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colAl.DefaultCellStyle = dataGridViewCellStyle7;
            this.colAl.HeaderText = "铝材";
            this.colAl.Name = "colAl";
            this.colAl.ReadOnly = true;
            this.colAl.Width = 54;
            // 
            // colSecretary
            // 
            this.colSecretary.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSecretary.DataPropertyName = "Secretary";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colSecretary.DefaultCellStyle = dataGridViewCellStyle8;
            this.colSecretary.HeaderText = "秘书舰";
            this.colSecretary.Name = "colSecretary";
            this.colSecretary.ReadOnly = true;
            this.colSecretary.Width = 66;
            // 
            // colLv
            // 
            this.colLv.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colLv.DataPropertyName = "Lv";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colLv.DefaultCellStyle = dataGridViewCellStyle9;
            this.colLv.HeaderText = "司令部等级";
            this.colLv.Name = "colLv";
            this.colLv.ReadOnly = true;
            this.colLv.Width = 90;
            // 
            // cbShip
            // 
            this.cbShip.FormattingEnabled = true;
            this.cbShip.Location = new System.Drawing.Point(352, 14);
            this.cbShip.Name = "cbShip";
            this.cbShip.Size = new System.Drawing.Size(121, 20);
            this.cbShip.TabIndex = 10;
            this.cbShip.TextChanged += new System.EventHandler(this.cbShip_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(304, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "装备名";
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
            // Fmin
            // 
            this.Fmin.Location = new System.Drawing.Point(328, 48);
            this.Fmin.Name = "Fmin";
            this.Fmin.Size = new System.Drawing.Size(38, 21);
            this.Fmin.TabIndex = 11;
            this.Fmin.TextChanged += new System.EventHandler(this.Fmin_TextChanged);
            // 
            // Fmax
            // 
            this.Fmax.Location = new System.Drawing.Point(372, 48);
            this.Fmax.Name = "Fmax";
            this.Fmax.Size = new System.Drawing.Size(38, 21);
            this.Fmax.TabIndex = 12;
            this.Fmax.TextChanged += new System.EventHandler(this.Fmin_TextChanged);
            // 
            // Amax
            // 
            this.Amax.Location = new System.Drawing.Point(372, 75);
            this.Amax.Name = "Amax";
            this.Amax.Size = new System.Drawing.Size(38, 21);
            this.Amax.TabIndex = 14;
            this.Amax.TextChanged += new System.EventHandler(this.Fmin_TextChanged);
            // 
            // Amin
            // 
            this.Amin.Location = new System.Drawing.Point(328, 75);
            this.Amin.Name = "Amin";
            this.Amin.Size = new System.Drawing.Size(38, 21);
            this.Amin.TabIndex = 13;
            this.Amin.TextChanged += new System.EventHandler(this.Fmin_TextChanged);
            // 
            // Smax
            // 
            this.Smax.Location = new System.Drawing.Point(500, 48);
            this.Smax.Name = "Smax";
            this.Smax.Size = new System.Drawing.Size(38, 21);
            this.Smax.TabIndex = 16;
            this.Smax.TextChanged += new System.EventHandler(this.Fmin_TextChanged);
            // 
            // Smin
            // 
            this.Smin.Location = new System.Drawing.Point(456, 48);
            this.Smin.Name = "Smin";
            this.Smin.Size = new System.Drawing.Size(38, 21);
            this.Smin.TabIndex = 15;
            this.Smin.TextChanged += new System.EventHandler(this.Fmin_TextChanged);
            // 
            // Xmax
            // 
            this.Xmax.Location = new System.Drawing.Point(500, 75);
            this.Xmax.Name = "Xmax";
            this.Xmax.Size = new System.Drawing.Size(38, 21);
            this.Xmax.TabIndex = 18;
            this.Xmax.TextChanged += new System.EventHandler(this.Fmin_TextChanged);
            // 
            // Xmin
            // 
            this.Xmin.Location = new System.Drawing.Point(456, 75);
            this.Xmin.Name = "Xmin";
            this.Xmin.Size = new System.Drawing.Size(38, 21);
            this.Xmin.TabIndex = 17;
            this.Xmin.TextChanged += new System.EventHandler(this.Fmin_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(303, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 19;
            this.label1.Text = "燃";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(303, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 20;
            this.label3.Text = "弹";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(433, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 21;
            this.label4.Text = "钢";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(433, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 22;
            this.label5.Text = "铝";
            // 
            // DevelopmentViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 552);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Xmax);
            this.Controls.Add(this.Xmin);
            this.Controls.Add(this.Smax);
            this.Controls.Add(this.Smin);
            this.Controls.Add(this.Amax);
            this.Controls.Add(this.Amin);
            this.Controls.Add(this.Fmax);
            this.Controls.Add(this.Fmin);
            this.Controls.Add(this.cbShip);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.timeSelection1);
            this.Name = "DevelopmentViewer";
            this.Text = "DropViewer";
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
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArea;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPoint;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRank;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAl;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSecretary;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLv;
        private System.Windows.Forms.TextBox Fmin;
        private System.Windows.Forms.TextBox Fmax;
        private System.Windows.Forms.TextBox Amax;
        private System.Windows.Forms.TextBox Amin;
        private System.Windows.Forms.TextBox Smax;
        private System.Windows.Forms.TextBox Smin;
        private System.Windows.Forms.TextBox Xmax;
        private System.Windows.Forms.TextBox Xmin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}