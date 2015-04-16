namespace ElectronicObserver.Window {
	partial class FormCompass {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCompass));
            this.BasePanel = new System.Windows.Forms.FlowLayoutPanel();
            this.TextMapArea = new ElectronicObserver.Window.Control.ImageLabel();
            this.TextDestination = new ElectronicObserver.Window.Control.ImageLabel();
            this.TextEventKind = new ElectronicObserver.Window.Control.ImageLabel();
            this.TextEventDetail = new ElectronicObserver.Window.Control.ImageLabel();
            this.PanelEnemyFleet = new System.Windows.Forms.Panel();
            this.TableEnemyMember = new System.Windows.Forms.TableLayoutPanel();
            this.TableEnemyFleet = new System.Windows.Forms.TableLayoutPanel();
            this.TextEnemyFleetName = new ElectronicObserver.Window.Control.ImageLabel();
            this.TextFormation = new ElectronicObserver.Window.Control.ImageLabel();
            this.TextAirSuperiority = new ElectronicObserver.Window.Control.ImageLabel();
            this.ToolTipInfo = new System.Windows.Forms.ToolTip(this.components);
            this.BasePanel.SuspendLayout();
            this.PanelEnemyFleet.SuspendLayout();
            this.TableEnemyFleet.SuspendLayout();
            this.SuspendLayout();
            // 
            // BasePanel
            // 
            resources.ApplyResources(this.BasePanel, "BasePanel");
            this.BasePanel.Controls.Add(this.TextMapArea);
            this.BasePanel.Controls.Add(this.TextDestination);
            this.BasePanel.Controls.Add(this.TextEventKind);
            this.BasePanel.Controls.Add(this.TextEventDetail);
            this.BasePanel.Controls.Add(this.PanelEnemyFleet);
            this.BasePanel.Name = "BasePanel";
            this.ToolTipInfo.SetToolTip(this.BasePanel, resources.GetString("BasePanel.ToolTip"));
            // 
            // TextMapArea
            // 
            resources.ApplyResources(this.TextMapArea, "TextMapArea");
            this.TextMapArea.BackColor = System.Drawing.Color.Transparent;
            this.TextMapArea.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TextMapArea.Name = "TextMapArea";
            this.ToolTipInfo.SetToolTip(this.TextMapArea, resources.GetString("TextMapArea.ToolTip"));
            // 
            // TextDestination
            // 
            resources.ApplyResources(this.TextDestination, "TextDestination");
            this.TextDestination.BackColor = System.Drawing.Color.Transparent;
            this.TextDestination.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TextDestination.Name = "TextDestination";
            this.ToolTipInfo.SetToolTip(this.TextDestination, resources.GetString("TextDestination.ToolTip"));
            // 
            // TextEventKind
            // 
            resources.ApplyResources(this.TextEventKind, "TextEventKind");
            this.TextEventKind.BackColor = System.Drawing.Color.Transparent;
            this.TextEventKind.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TextEventKind.Name = "TextEventKind";
            this.ToolTipInfo.SetToolTip(this.TextEventKind, resources.GetString("TextEventKind.ToolTip"));
            // 
            // TextEventDetail
            // 
            resources.ApplyResources(this.TextEventDetail, "TextEventDetail");
            this.TextEventDetail.BackColor = System.Drawing.Color.Transparent;
            this.TextEventDetail.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TextEventDetail.Name = "TextEventDetail";
            this.ToolTipInfo.SetToolTip(this.TextEventDetail, resources.GetString("TextEventDetail.ToolTip"));
            // 
            // PanelEnemyFleet
            // 
            resources.ApplyResources(this.PanelEnemyFleet, "PanelEnemyFleet");
            this.PanelEnemyFleet.Controls.Add(this.TableEnemyMember);
            this.PanelEnemyFleet.Controls.Add(this.TableEnemyFleet);
            this.PanelEnemyFleet.Name = "PanelEnemyFleet";
            this.ToolTipInfo.SetToolTip(this.PanelEnemyFleet, resources.GetString("PanelEnemyFleet.ToolTip"));
            // 
            // TableEnemyMember
            // 
            resources.ApplyResources(this.TableEnemyMember, "TableEnemyMember");
            this.TableEnemyMember.Name = "TableEnemyMember";
            this.ToolTipInfo.SetToolTip(this.TableEnemyMember, resources.GetString("TableEnemyMember.ToolTip"));
            this.TableEnemyMember.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.TableEnemyMember_CellPaint);
            // 
            // TableEnemyFleet
            // 
            resources.ApplyResources(this.TableEnemyFleet, "TableEnemyFleet");
            this.TableEnemyFleet.Controls.Add(this.TextEnemyFleetName, 0, 0);
            this.TableEnemyFleet.Controls.Add(this.TextFormation, 1, 0);
            this.TableEnemyFleet.Controls.Add(this.TextAirSuperiority, 2, 0);
            this.TableEnemyFleet.Name = "TableEnemyFleet";
            this.ToolTipInfo.SetToolTip(this.TableEnemyFleet, resources.GetString("TableEnemyFleet.ToolTip"));
            // 
            // TextEnemyFleetName
            // 
            resources.ApplyResources(this.TextEnemyFleetName, "TextEnemyFleetName");
            this.TextEnemyFleetName.BackColor = System.Drawing.Color.Transparent;
            this.TextEnemyFleetName.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TextEnemyFleetName.Name = "TextEnemyFleetName";
            this.ToolTipInfo.SetToolTip(this.TextEnemyFleetName, resources.GetString("TextEnemyFleetName.ToolTip"));
            // 
            // TextFormation
            // 
            resources.ApplyResources(this.TextFormation, "TextFormation");
            this.TextFormation.BackColor = System.Drawing.Color.Transparent;
            this.TextFormation.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TextFormation.Name = "TextFormation";
            this.ToolTipInfo.SetToolTip(this.TextFormation, resources.GetString("TextFormation.ToolTip"));
            // 
            // TextAirSuperiority
            // 
            resources.ApplyResources(this.TextAirSuperiority, "TextAirSuperiority");
            this.TextAirSuperiority.BackColor = System.Drawing.Color.Transparent;
            this.TextAirSuperiority.Name = "TextAirSuperiority";
            this.ToolTipInfo.SetToolTip(this.TextAirSuperiority, resources.GetString("TextAirSuperiority.ToolTip"));
            // 
            // ToolTipInfo
            // 
            this.ToolTipInfo.AutoPopDelay = 30000;
            this.ToolTipInfo.InitialDelay = 500;
            this.ToolTipInfo.ReshowDelay = 100;
            this.ToolTipInfo.ShowAlways = true;
            // 
            // FormCompass
            // 
            resources.ApplyResources(this, "$this");
            this.AutoHidePortion = 150D;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.BasePanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HideOnClose = true;
            this.Name = "FormCompass";
            this.ToolTipInfo.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.Load += new System.EventHandler(this.FormCompass_Load);
            this.BasePanel.ResumeLayout(false);
            this.BasePanel.PerformLayout();
            this.PanelEnemyFleet.ResumeLayout(false);
            this.PanelEnemyFleet.PerformLayout();
            this.TableEnemyFleet.ResumeLayout(false);
            this.TableEnemyFleet.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel BasePanel;
		private System.Windows.Forms.Panel PanelEnemyFleet;
		private System.Windows.Forms.TableLayoutPanel TableEnemyFleet;
		private Control.ImageLabel TextEnemyFleetName;
		private Control.ImageLabel TextFormation;
		private Control.ImageLabel TextAirSuperiority;
		private System.Windows.Forms.TableLayoutPanel TableEnemyMember;
		private System.Windows.Forms.ToolTip ToolTipInfo;
		private Control.ImageLabel TextMapArea;
		private Control.ImageLabel TextDestination;
		private Control.ImageLabel TextEventKind;
		private Control.ImageLabel TextEventDetail;
	}
}