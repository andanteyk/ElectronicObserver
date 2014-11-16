namespace ElectronicObserver.Window {
	partial class FormFleet {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing ) {
			if ( disposing && ( components != null ) ) {
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.TableMember = new System.Windows.Forms.TableLayoutPanel();
			this.TableFleet = new System.Windows.Forms.TableLayoutPanel();
			this.ContextMenuFleet = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.ContextMenuFleet_CopyFleet = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolTipInfo = new System.Windows.Forms.ToolTip(this.components);
			this.ContextMenuFleet.SuspendLayout();
			this.SuspendLayout();
			// 
			// TableMember
			// 
			this.TableMember.AutoSize = true;
			this.TableMember.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TableMember.ColumnCount = 6;
			this.TableMember.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableMember.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableMember.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableMember.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableMember.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableMember.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableMember.Location = new System.Drawing.Point(0, 24);
			this.TableMember.Name = "TableMember";
			this.TableMember.RowCount = 1;
			this.TableMember.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
			this.TableMember.Size = new System.Drawing.Size(0, 21);
			this.TableMember.TabIndex = 1;
			this.TableMember.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.TableMember_CellPaint);
			// 
			// TableFleet
			// 
			this.TableFleet.AutoSize = true;
			this.TableFleet.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TableFleet.ColumnCount = 4;
			this.TableFleet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableFleet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableFleet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableFleet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableFleet.ContextMenuStrip = this.ContextMenuFleet;
			this.TableFleet.Location = new System.Drawing.Point(0, 0);
			this.TableFleet.Name = "TableFleet";
			this.TableFleet.RowCount = 1;
			this.TableFleet.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
			this.TableFleet.Size = new System.Drawing.Size(0, 21);
			this.TableFleet.TabIndex = 2;
			// 
			// ContextMenuFleet
			// 
			this.ContextMenuFleet.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContextMenuFleet_CopyFleet});
			this.ContextMenuFleet.Name = "ContextMenuFleet";
			this.ContextMenuFleet.Size = new System.Drawing.Size(227, 26);
			// 
			// ContextMenuFleet_CopyFleet
			// 
			this.ContextMenuFleet_CopyFleet.Name = "ContextMenuFleet_CopyFleet";
			this.ContextMenuFleet_CopyFleet.Size = new System.Drawing.Size(226, 22);
			this.ContextMenuFleet_CopyFleet.Text = "クリップボードにコピー(&C)";
			this.ContextMenuFleet_CopyFleet.Click += new System.EventHandler(this.ContextMenuFleet_CopyFleet_Click);
			// 
			// ToolTipInfo
			// 
			this.ToolTipInfo.AutoPopDelay = 30000;
			this.ToolTipInfo.InitialDelay = 500;
			this.ToolTipInfo.ReshowDelay = 100;
			this.ToolTipInfo.ShowAlways = true;
			// 
			// FormFleet
			// 
			this.AutoHidePortion = 150D;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(300, 200);
			this.Controls.Add(this.TableFleet);
			this.Controls.Add(this.TableMember);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.HideOnClose = true;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "FormFleet";
			this.Text = "*not loaded*";
			this.Load += new System.EventHandler(this.FormFleet_Load);
			this.ContextMenuFleet.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel TableMember;
		private System.Windows.Forms.TableLayoutPanel TableFleet;
		private System.Windows.Forms.ToolTip ToolTipInfo;
		private System.Windows.Forms.ContextMenuStrip ContextMenuFleet;
		private System.Windows.Forms.ToolStripMenuItem ContextMenuFleet_CopyFleet;
	}
}