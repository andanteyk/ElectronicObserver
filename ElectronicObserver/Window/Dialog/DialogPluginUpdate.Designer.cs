namespace ElectronicObserver.Window.Dialog
{
    partial class DialogPluginUpdate
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColUpdate = new ElectronicObserver.Window.Control.DataGridViewDisableButtonColumn();
            this.ColManual = new ElectronicObserver.Window.Control.DataGridViewDisableButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColName,
            this.ColStatus,
            this.ColUpdate,
            this.ColManual});
            this.dataGridView1.Location = new System.Drawing.Point(0, 34);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 35;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.Size = new System.Drawing.Size(714, 444);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "支持更新功能的插件:";
            // 
            // ColName
            // 
            this.ColName.HeaderText = "插件名称";
            this.ColName.Name = "ColName";
            this.ColName.ReadOnly = true;
            // 
            // ColStatus
            // 
            this.ColStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColStatus.HeaderText = "状态";
            this.ColStatus.Name = "ColStatus";
            this.ColStatus.ReadOnly = true;
            // 
            // ColUpdate
            // 
            this.ColUpdate.HeaderText = "更新";
            this.ColUpdate.Name = "ColUpdate";
            this.ColUpdate.ReadOnly = true;
            this.ColUpdate.Width = 80;
            // 
            // ColManual
            // 
            this.ColManual.HeaderText = "手动";
            this.ColManual.Name = "ColManual";
            this.ColManual.ReadOnly = true;
            this.ColManual.Width = 80;
            // 
            // DialogPluginUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 478);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "DialogPluginUpdate";
            this.ShowInTaskbar = false;
            this.Text = "DialogPluginUpdate";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogPluginUpdate_FormClosed);
            this.Load += new System.EventHandler(this.DialogPluginUpdate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColStatus;
        private Control.DataGridViewDisableButtonColumn ColUpdate;
        private Control.DataGridViewDisableButtonColumn ColManual;
    }
}