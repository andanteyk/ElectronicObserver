namespace ElectronicObserver.Window {
	partial class FormFleetOverview {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFleetOverview));
            this.ToolTipInfo = new System.Windows.Forms.ToolTip(this.components);
            this.TableFleet = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // ToolTipInfo
            // 
            this.ToolTipInfo.AutoPopDelay = 3000;
            this.ToolTipInfo.InitialDelay = 500;
            this.ToolTipInfo.ReshowDelay = 100;
            this.ToolTipInfo.ShowAlways = true;
            // 
            // TableFleet
            // 
            resources.ApplyResources(this.TableFleet, "TableFleet");
            this.TableFleet.Name = "TableFleet";
            this.ToolTipInfo.SetToolTip(this.TableFleet, resources.GetString("TableFleet.ToolTip"));
            this.TableFleet.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.TableFleet_CellPaint);
            // 
            // FormFleetOverview
            // 
            resources.ApplyResources(this, "$this");
            this.AutoHidePortion = 150D;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.TableFleet);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HideOnClose = true;
            this.Name = "FormFleetOverview";
            this.ToolTipInfo.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.Load += new System.EventHandler(this.FormFleetOverview_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolTip ToolTipInfo;
		private System.Windows.Forms.TableLayoutPanel TableFleet;
	}
}