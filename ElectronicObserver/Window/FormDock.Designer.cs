namespace ElectronicObserver.Window {
	partial class FormDock {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDock));
            this.TableDock = new System.Windows.Forms.TableLayoutPanel();
            this.ToolTipInfo = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // TableDock
            // 
            resources.ApplyResources(this.TableDock, "TableDock");
            this.TableDock.Name = "TableDock";
            this.ToolTipInfo.SetToolTip(this.TableDock, resources.GetString("TableDock.ToolTip"));
            this.TableDock.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.TableDock_CellPaint);
            // 
            // ToolTipInfo
            // 
            this.ToolTipInfo.AutoPopDelay = 30000;
            this.ToolTipInfo.InitialDelay = 500;
            this.ToolTipInfo.ReshowDelay = 100;
            this.ToolTipInfo.ShowAlways = true;
            // 
            // FormDock
            // 
            resources.ApplyResources(this, "$this");
            this.AutoHidePortion = 150D;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.TableDock);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HideOnClose = true;
            this.Name = "FormDock";
            this.ToolTipInfo.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.Load += new System.EventHandler(this.FormDock_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel TableDock;
		private System.Windows.Forms.ToolTip ToolTipInfo;
	}
}