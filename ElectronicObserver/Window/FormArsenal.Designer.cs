namespace ElectronicObserver.Window {
	partial class FormArsenal {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormArsenal));
            this.TableArsenal = new System.Windows.Forms.TableLayoutPanel();
            this.ToolTipInfo = new System.Windows.Forms.ToolTip(this.components);
            this.MenuMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuMain_ShowShipName = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // TableArsenal
            // 
            resources.ApplyResources(this.TableArsenal, "TableArsenal");
            this.TableArsenal.Name = "TableArsenal";
            this.ToolTipInfo.SetToolTip(this.TableArsenal, resources.GetString("TableArsenal.ToolTip"));
            this.TableArsenal.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.TableArsenal_CellPaint);
            // 
            // ToolTipInfo
            // 
            this.ToolTipInfo.AutoPopDelay = 30000;
            this.ToolTipInfo.InitialDelay = 500;
            this.ToolTipInfo.ReshowDelay = 100;
            this.ToolTipInfo.ShowAlways = true;
            // 
            // MenuMain
            // 
            resources.ApplyResources(this.MenuMain, "MenuMain");
            this.MenuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuMain_ShowShipName});
            this.MenuMain.Name = "MenuMain";
            this.ToolTipInfo.SetToolTip(this.MenuMain, resources.GetString("MenuMain.ToolTip"));
            // 
            // MenuMain_ShowShipName
            // 
            resources.ApplyResources(this.MenuMain_ShowShipName, "MenuMain_ShowShipName");
            this.MenuMain_ShowShipName.CheckOnClick = true;
            this.MenuMain_ShowShipName.Name = "MenuMain_ShowShipName";
            this.MenuMain_ShowShipName.CheckedChanged += new System.EventHandler(this.MenuMain_ShowShipName_CheckedChanged);
            // 
            // FormArsenal
            // 
            resources.ApplyResources(this, "$this");
            this.AutoHidePortion = 150D;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ContextMenuStrip = this.MenuMain;
            this.Controls.Add(this.TableArsenal);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HideOnClose = true;
            this.Name = "FormArsenal";
            this.ToolTipInfo.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.Load += new System.EventHandler(this.FormArsenal_Load);
            this.MenuMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel TableArsenal;
		private System.Windows.Forms.ToolTip ToolTipInfo;
		private System.Windows.Forms.ContextMenuStrip MenuMain;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ShowShipName;
	}
}