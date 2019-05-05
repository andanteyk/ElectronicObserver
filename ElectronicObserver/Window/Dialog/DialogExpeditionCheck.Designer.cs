namespace ElectronicObserver.Window.Dialog
{
    partial class DialogExpeditionCheck
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
            this.CheckView = new System.Windows.Forms.DataGridView();
            this.CheckView_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CheckView_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CheckView_Fleet2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CheckView_Fleet3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CheckView_Fleet4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CheckView_Condition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.CheckView)).BeginInit();
            this.SuspendLayout();
            // 
            // CheckView
            // 
            this.CheckView.AllowUserToAddRows = false;
            this.CheckView.AllowUserToDeleteRows = false;
            this.CheckView.AllowUserToResizeRows = false;
            this.CheckView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CheckView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CheckView_ID,
            this.CheckView_Name,
            this.CheckView_Fleet2,
            this.CheckView_Fleet3,
            this.CheckView_Fleet4,
            this.CheckView_Condition});
            this.CheckView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CheckView.Location = new System.Drawing.Point(0, 0);
            this.CheckView.Name = "CheckView";
            this.CheckView.ReadOnly = true;
            this.CheckView.RowHeadersVisible = false;
            this.CheckView.RowTemplate.Height = 21;
            this.CheckView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CheckView.Size = new System.Drawing.Size(800, 450);
            this.CheckView.TabIndex = 0;
            this.CheckView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.CheckView_CellFormatting);
            this.CheckView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.CheckView_SortCompare);
            // 
            // CheckView_ID
            // 
            this.CheckView_ID.HeaderText = "ID";
            this.CheckView_ID.Name = "CheckView_ID";
            this.CheckView_ID.ReadOnly = true;
            this.CheckView_ID.Width = 60;
            // 
            // CheckView_Name
            // 
            this.CheckView_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CheckView_Name.HeaderText = "遠征名";
            this.CheckView_Name.Name = "CheckView_Name";
            this.CheckView_Name.ReadOnly = true;
            this.CheckView_Name.Width = 68;
            // 
            // CheckView_Fleet2
            // 
            this.CheckView_Fleet2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CheckView_Fleet2.HeaderText = "第2艦隊";
            this.CheckView_Fleet2.Name = "CheckView_Fleet2";
            this.CheckView_Fleet2.ReadOnly = true;
            this.CheckView_Fleet2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // CheckView_Fleet3
            // 
            this.CheckView_Fleet3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CheckView_Fleet3.HeaderText = "第3艦隊";
            this.CheckView_Fleet3.Name = "CheckView_Fleet3";
            this.CheckView_Fleet3.ReadOnly = true;
            this.CheckView_Fleet3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // CheckView_Fleet4
            // 
            this.CheckView_Fleet4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CheckView_Fleet4.HeaderText = "第4艦隊";
            this.CheckView_Fleet4.Name = "CheckView_Fleet4";
            this.CheckView_Fleet4.ReadOnly = true;
            this.CheckView_Fleet4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // CheckView_Condition
            // 
            this.CheckView_Condition.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CheckView_Condition.HeaderText = "成功条件";
            this.CheckView_Condition.Name = "CheckView_Condition";
            this.CheckView_Condition.ReadOnly = true;
            this.CheckView_Condition.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DialogExpeditionCheck
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.CheckView);
            this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "DialogExpeditionCheck";
            this.Text = "遠征可否チェック";
            this.Activated += new System.EventHandler(this.DialogExpeditionCheck_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogExpeditionCheck_FormClosed);
            this.Load += new System.EventHandler(this.DialogExpeditionCheck_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CheckView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView CheckView;
        private System.Windows.Forms.DataGridViewTextBoxColumn CheckView_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CheckView_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn CheckView_Fleet2;
        private System.Windows.Forms.DataGridViewTextBoxColumn CheckView_Fleet3;
        private System.Windows.Forms.DataGridViewTextBoxColumn CheckView_Fleet4;
        private System.Windows.Forms.DataGridViewTextBoxColumn CheckView_Condition;
    }
}