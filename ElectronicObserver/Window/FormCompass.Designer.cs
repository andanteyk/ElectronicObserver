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
			this.BasePanel = new System.Windows.Forms.FlowLayoutPanel();
			this.TextMapArea = new ElectronicObserver.Window.Control.ImageLabel();
			this.TextDestination = new ElectronicObserver.Window.Control.ImageLabel();
			this.TextEventKind = new ElectronicObserver.Window.Control.ImageLabel();
			this.TextEventDetail = new ElectronicObserver.Window.Control.ImageLabel();
			this.PanelEnemyCandidate = new System.Windows.Forms.Panel();
			this.TableEnemyCandidate = new System.Windows.Forms.TableLayoutPanel();
			this.PanelEnemyFleet = new System.Windows.Forms.Panel();
			this.TableEnemyMember = new System.Windows.Forms.TableLayoutPanel();
			this.TextEnemyFleetName = new ElectronicObserver.Window.Control.ImageLabel();
			this.TextFormation = new ElectronicObserver.Window.Control.ImageLabel();
			this.TextAirSuperiority = new ElectronicObserver.Window.Control.ImageLabel();
			this.TextAA = new ElectronicObserver.Window.Control.ImageLabel();
			this.ToolTipInfo = new System.Windows.Forms.ToolTip(this.components);
			this.BasePanel.SuspendLayout();
			this.PanelEnemyCandidate.SuspendLayout();
			this.PanelEnemyFleet.SuspendLayout();
			this.SuspendLayout();
			// 
			// BasePanel
			// 
			this.BasePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.BasePanel.Controls.Add(this.TextMapArea);
			this.BasePanel.Controls.Add(this.TextDestination);
			this.BasePanel.Controls.Add(this.TextEventKind);
			this.BasePanel.Controls.Add(this.TextEventDetail);
			this.BasePanel.Controls.Add(this.TextEnemyFleetName);
			this.BasePanel.Controls.Add(this.TextFormation);
			this.BasePanel.Controls.Add(this.TextAirSuperiority);
			this.BasePanel.Controls.Add(this.TextAA);
			this.BasePanel.Controls.Add(this.PanelEnemyCandidate);
			this.BasePanel.Controls.Add(this.PanelEnemyFleet);
			this.BasePanel.Location = new System.Drawing.Point(0, 0);
			this.BasePanel.Name = "BasePanel";
			this.BasePanel.Size = new System.Drawing.Size(300, 200);
			this.BasePanel.TabIndex = 0;
			// 
			// TextMapArea
			// 
			this.TextMapArea.BackColor = System.Drawing.Color.Transparent;
			this.TextMapArea.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.TextMapArea.Location = new System.Drawing.Point(3, 3);
			this.TextMapArea.Name = "TextMapArea";
			this.TextMapArea.Size = new System.Drawing.Size(35, 16);
			this.TextMapArea.TabIndex = 5;
			this.TextMapArea.Text = "(海域)";
			// 
			// TextDestination
			// 
			this.TextDestination.BackColor = System.Drawing.Color.Transparent;
			this.TextDestination.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.TextDestination.Location = new System.Drawing.Point(44, 3);
			this.TextDestination.Name = "TextDestination";
			this.TextDestination.Size = new System.Drawing.Size(44, 16);
			this.TextDestination.TabIndex = 6;
			this.TextDestination.Text = "(行き先)";
			// 
			// TextEventKind
			// 
			this.TextEventKind.BackColor = System.Drawing.Color.Transparent;
			this.TextEventKind.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.TextEventKind.Location = new System.Drawing.Point(94, 3);
			this.TextEventKind.Name = "TextEventKind";
			this.TextEventKind.Size = new System.Drawing.Size(47, 16);
			this.TextEventKind.TabIndex = 7;
			this.TextEventKind.Text = "(イベント)";
			// 
			// TextEventDetail
			// 
			this.TextEventDetail.BackColor = System.Drawing.Color.Transparent;
			this.TextEventDetail.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.TextEventDetail.Location = new System.Drawing.Point(147, 3);
			this.TextEventDetail.Name = "TextEventDetail";
			this.TextEventDetail.Size = new System.Drawing.Size(71, 16);
			this.TextEventDetail.TabIndex = 8;
			this.TextEventDetail.Text = "(イベント詳細)";
			this.TextEventDetail.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TextEnemyFleetName_MouseDown);
			// 
			// PanelEnemyCandidate
			// 
			this.PanelEnemyCandidate.AutoSize = true;
			this.PanelEnemyCandidate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.PanelEnemyCandidate.Controls.Add(this.TableEnemyCandidate);
			this.PanelEnemyCandidate.Location = new System.Drawing.Point(224, 3);
			this.PanelEnemyCandidate.Name = "PanelEnemyCandidate";
			this.PanelEnemyCandidate.Size = new System.Drawing.Size(0, 0);
			this.PanelEnemyCandidate.TabIndex = 10;
			// 
			// TableEnemyCandidate
			// 
			this.TableEnemyCandidate.AutoSize = true;
			this.TableEnemyCandidate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TableEnemyCandidate.ColumnCount = 2;
			this.TableEnemyCandidate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TableEnemyCandidate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TableEnemyCandidate.Location = new System.Drawing.Point(0, 0);
			this.TableEnemyCandidate.Margin = new System.Windows.Forms.Padding(0);
			this.TableEnemyCandidate.Name = "TableEnemyCandidate";
			this.TableEnemyCandidate.RowCount = 2;
			this.TableEnemyCandidate.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TableEnemyCandidate.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TableEnemyCandidate.Size = new System.Drawing.Size(0, 0);
			this.TableEnemyCandidate.TabIndex = 9;
			this.TableEnemyCandidate.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.TableEnemyCandidateMember_CellPaint);
			// 
			// PanelEnemyFleet
			// 
			this.PanelEnemyFleet.AutoSize = true;
			this.PanelEnemyFleet.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.PanelEnemyFleet.Controls.Add(this.TableEnemyMember);
			this.PanelEnemyFleet.Location = new System.Drawing.Point(3, 25);
			this.PanelEnemyFleet.Margin = new System.Windows.Forms.Padding(3);
			this.PanelEnemyFleet.Name = "PanelEnemyFleet";
			this.PanelEnemyFleet.Size = new System.Drawing.Size(190, 22);
			this.PanelEnemyFleet.TabIndex = 4;
			// 
			// TableEnemyMember
			// 
			this.TableEnemyMember.AutoSize = true;
			this.TableEnemyMember.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TableEnemyMember.ColumnCount = 2;
			this.TableEnemyMember.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableEnemyMember.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableEnemyMember.Location = new System.Drawing.Point(0, 0);
			this.TableEnemyMember.Margin = new System.Windows.Forms.Padding(0);
			this.TableEnemyMember.Name = "TableEnemyMember";
			this.TableEnemyMember.RowCount = 1;
			this.TableEnemyMember.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TableEnemyMember.MinimumSize = new System.Drawing.Size(240, 10);
			this.TableEnemyMember.TabIndex = 1;
			this.TableEnemyMember.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.TableEnemyMember_CellPaint);
			// 
			// TextEnemyFleetName
			// 
			this.TextEnemyFleetName.BackColor = System.Drawing.Color.Transparent;
			this.TextEnemyFleetName.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.TextEnemyFleetName.Location = new System.Drawing.Point(3, 0);
			this.TextEnemyFleetName.Name = "TextEnemyFleetName";
			this.TextEnemyFleetName.Size = new System.Drawing.Size(59, 16);
			this.TextEnemyFleetName.TabIndex = 0;
			this.TextEnemyFleetName.Text = "(敵艦隊名)";
			this.TextEnemyFleetName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TextEnemyFleetName_MouseDown);
			// 
			// TextFormation
			// 
			this.TextFormation.BackColor = System.Drawing.Color.Transparent;
			this.TextFormation.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.TextFormation.Location = new System.Drawing.Point(68, 0);
			this.TextFormation.Name = "TextFormation";
			this.TextFormation.Size = new System.Drawing.Size(35, 16);
			this.TextFormation.TabIndex = 1;
			this.TextFormation.Text = "(陣形)";
			// 
			// TextAirSuperiority
			// 
			this.TextAirSuperiority.BackColor = System.Drawing.Color.Transparent;
			this.TextAirSuperiority.Location = new System.Drawing.Point(109, 0);
			this.TextAirSuperiority.Name = "TextAirSuperiority";
			this.TextAirSuperiority.Size = new System.Drawing.Size(175, 16);
			this.TextAirSuperiority.TabIndex = 2;
			this.TextAirSuperiority.Text = "(制空戦力)";
			// 
			// TextAA
			// 
			this.TextAA.BackColor = System.Drawing.Color.Transparent;
			this.TextAA.Location = new System.Drawing.Point(290, 0);
			this.TextAA.Name = "TextAA";
			this.TextAA.Size = new System.Drawing.Size(175, 16);
			this.TextAA.TabIndex = 3;
			this.TextAA.Text = "(舰队防空)";
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
			this.AutoHidePortion = 150D;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(300, 200);
			this.Controls.Add(this.BasePanel);
			this.Font = Program.Window_Font;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.HideOnClose = true;
			this.Name = "FormCompass";
			this.Text = "罗针盘";
			this.Load += new System.EventHandler(this.FormCompass_Load);
			this.BasePanel.ResumeLayout(false);
			this.BasePanel.PerformLayout();
			this.PanelEnemyCandidate.ResumeLayout(false);
			this.PanelEnemyCandidate.PerformLayout();
			this.PanelEnemyFleet.ResumeLayout(false);
			this.PanelEnemyFleet.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel BasePanel;
		private System.Windows.Forms.Panel PanelEnemyFleet;
		private Control.ImageLabel TextEnemyFleetName;
		private Control.ImageLabel TextFormation;
		private Control.ImageLabel TextAirSuperiority;
		private Control.ImageLabel TextAA;
		private System.Windows.Forms.TableLayoutPanel TableEnemyMember;
		private System.Windows.Forms.ToolTip ToolTipInfo;
		private Control.ImageLabel TextMapArea;
		private Control.ImageLabel TextDestination;
		private Control.ImageLabel TextEventKind;
		private Control.ImageLabel TextEventDetail;
		private System.Windows.Forms.Panel PanelEnemyCandidate;
		private System.Windows.Forms.TableLayoutPanel TableEnemyCandidate;
	}
}