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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFleet));
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
            resources.ApplyResources(this.TableMember, "TableMember");
            this.TableMember.Name = "TableMember";
            this.ToolTipInfo.SetToolTip(this.TableMember, resources.GetString("TableMember.ToolTip"));
            this.TableMember.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.TableMember_CellPaint);
            // 
            // TableFleet
            // 
            resources.ApplyResources(this.TableFleet, "TableFleet");
            this.TableFleet.ContextMenuStrip = this.ContextMenuFleet;
            this.TableFleet.Name = "TableFleet";
            this.ToolTipInfo.SetToolTip(this.TableFleet, resources.GetString("TableFleet.ToolTip"));
            // 
            // ContextMenuFleet
            // 
            resources.ApplyResources(this.ContextMenuFleet, "ContextMenuFleet");
            this.ContextMenuFleet.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContextMenuFleet_CopyFleet,
            this.ContextMenuFleet_Capture,
            this.toolStripSeparator1,
            this.ContextMenuFleet_IsScrollable,
            this.ContextMenuFleet_FixShipNameWidth});
            this.ContextMenuFleet.Name = "ContextMenuFleet";
            this.ToolTipInfo.SetToolTip(this.ContextMenuFleet, resources.GetString("ContextMenuFleet.ToolTip"));
            this.ContextMenuFleet.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuFleet_Opening);
            // 
            // ContextMenuFleet_CopyFleet
            // 
            resources.ApplyResources(this.ContextMenuFleet_CopyFleet, "ContextMenuFleet_CopyFleet");
            this.ContextMenuFleet_CopyFleet.Name = "ContextMenuFleet_CopyFleet";
            this.ContextMenuFleet_CopyFleet.Click += new System.EventHandler(this.ContextMenuFleet_CopyFleet_Click);
            // 
            // ContextMenuFleet_Capture
            // 
            resources.ApplyResources(this.ContextMenuFleet_Capture, "ContextMenuFleet_Capture");
            this.ContextMenuFleet_Capture.Name = "ContextMenuFleet_Capture";
            this.ContextMenuFleet_Capture.Click += new System.EventHandler(this.ContextMenuFleet_Capture_Click);
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // ContextMenuFleet_IsScrollable
            // 
            resources.ApplyResources(this.ContextMenuFleet_IsScrollable, "ContextMenuFleet_IsScrollable");
            this.ContextMenuFleet_IsScrollable.CheckOnClick = true;
            this.ContextMenuFleet_IsScrollable.Name = "ContextMenuFleet_IsScrollable";
            this.ContextMenuFleet_IsScrollable.Click += new System.EventHandler(this.ContextMenuFleet_IsScrollable_Click);
            // 
            // ContextMenuFleet_FixShipNameWidth
            // 
            resources.ApplyResources(this.ContextMenuFleet_FixShipNameWidth, "ContextMenuFleet_FixShipNameWidth");
            this.ContextMenuFleet_FixShipNameWidth.CheckOnClick = true;
            this.ContextMenuFleet_FixShipNameWidth.Name = "ContextMenuFleet_FixShipNameWidth";
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
            resources.ApplyResources(this, "$this");
            this.AutoHidePortion = 150D;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.TableFleet);
            this.Controls.Add(this.TableMember);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HideOnClose = true;
            this.Name = "FormFleet";
            this.ToolTipInfo.SetToolTip(this, resources.GetString("$this.ToolTip"));
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