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
			this.ContextMenuFleet_Capture = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.ContextMenuFleet_IsScrollable = new System.Windows.Forms.ToolStripMenuItem();
			this.ContextMenuFleet_FixShipNameWidth = new System.Windows.Forms.ToolStripMenuItem();
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
            this.ContextMenuFleet_CopyFleet,
            this.ContextMenuFleet_Capture,
            this.toolStripSeparator1,
            this.ContextMenuFleet_IsScrollable,
            this.ContextMenuFleet_FixShipNameWidth});
			this.ContextMenuFleet.Name = "ContextMenuFleet";
			this.ContextMenuFleet.Size = new System.Drawing.Size(227, 98);
			this.ContextMenuFleet.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuFleet_Opening);
			// 
			// ContextMenuFleet_CopyFleet
			// 
			this.ContextMenuFleet_CopyFleet.Name = "ContextMenuFleet_CopyFleet";
			this.ContextMenuFleet_CopyFleet.Size = new System.Drawing.Size(226, 22);
			this.ContextMenuFleet_CopyFleet.Text = "クリップボードにコピー(&C)";
			this.ContextMenuFleet_CopyFleet.Click += new System.EventHandler(this.ContextMenuFleet_CopyFleet_Click);
			// 
			// ContextMenuFleet_Capture
			// 
			this.ContextMenuFleet_Capture.Name = "ContextMenuFleet_Capture";
			this.ContextMenuFleet_Capture.Size = new System.Drawing.Size(226, 22);
			this.ContextMenuFleet_Capture.Text = "この画面をキャプチャ(&S)";
			this.ContextMenuFleet_Capture.Click += new System.EventHandler(this.ContextMenuFleet_Capture_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(223, 6);
			this.toolStripSeparator1.Visible = false;
			// 
			// ContextMenuFleet_IsScrollable
			// 
			this.ContextMenuFleet_IsScrollable.CheckOnClick = true;
			this.ContextMenuFleet_IsScrollable.Enabled = false;
			this.ContextMenuFleet_IsScrollable.Name = "ContextMenuFleet_IsScrollable";
			this.ContextMenuFleet_IsScrollable.Size = new System.Drawing.Size(226, 22);
			this.ContextMenuFleet_IsScrollable.Text = "スクロール可能";
			this.ContextMenuFleet_IsScrollable.Visible = false;
			this.ContextMenuFleet_IsScrollable.Click += new System.EventHandler(this.ContextMenuFleet_IsScrollable_Click);
			// 
			// ContextMenuFleet_FixShipNameWidth
			// 
			this.ContextMenuFleet_FixShipNameWidth.CheckOnClick = true;
			this.ContextMenuFleet_FixShipNameWidth.Enabled = false;
			this.ContextMenuFleet_FixShipNameWidth.Name = "ContextMenuFleet_FixShipNameWidth";
			this.ContextMenuFleet_FixShipNameWidth.Size = new System.Drawing.Size(226, 22);
			this.ContextMenuFleet_FixShipNameWidth.Text = "艦名の幅を固定する";
			this.ContextMenuFleet_FixShipNameWidth.Visible = false;
			this.ContextMenuFleet_FixShipNameWidth.Click += new System.EventHandler(this.ContextMenuFleet_FixShipNameWidth_Click);
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
			this.AutoScroll = true;
			this.BackColor = System.Drawing.SystemColors.Control;
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
		private System.Windows.Forms.ToolStripMenuItem ContextMenuFleet_Capture;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem ContextMenuFleet_IsScrollable;
		private System.Windows.Forms.ToolStripMenuItem ContextMenuFleet_FixShipNameWidth;
	}
}