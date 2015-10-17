using ElectronicObserver.Window;
using ElectronicObserver.Window.Control;

namespace MapAreaInfo
{
	partial class FormFleetData {
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
            this.TextEnemyFleetName = new ElectronicObserver.Window.Control.ImageLabel();
            this.TextFormation = new ElectronicObserver.Window.Control.ImageLabel();
            this.TextAirSuperiority = new ElectronicObserver.Window.Control.ImageLabel();
            this.PanelEnemyFleet = new System.Windows.Forms.Panel();
            this.TableEnemyMember = new System.Windows.Forms.TableLayoutPanel();
            this.ToolTipInfo = new System.Windows.Forms.ToolTip(this.components);
            this.BasePanel.SuspendLayout();
            this.PanelEnemyFleet.SuspendLayout();
            this.SuspendLayout();
            // 
            // BasePanel
            // 
            this.BasePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BasePanel.Controls.Add(this.TextEnemyFleetName);
            this.BasePanel.Controls.Add(this.TextFormation);
            this.BasePanel.Controls.Add(this.TextAirSuperiority);
            this.BasePanel.Controls.Add(this.PanelEnemyFleet);
            this.BasePanel.Location = new System.Drawing.Point(0, 0);
            this.BasePanel.Name = "BasePanel";
            this.BasePanel.Size = new System.Drawing.Size(245, 164);
            this.BasePanel.TabIndex = 0;
            // 
            // TextEnemyFleetName
            // 
            this.TextEnemyFleetName.BackColor = System.Drawing.Color.Transparent;
            this.TextEnemyFleetName.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TextEnemyFleetName.Location = new System.Drawing.Point(3, 3);
            this.TextEnemyFleetName.Name = "TextEnemyFleetName";
            this.TextEnemyFleetName.Size = new System.Drawing.Size(61, 16);
            this.TextEnemyFleetName.TabIndex = 0;
            this.TextEnemyFleetName.Text = "(敵艦隊名)";
            // 
            // TextFormation
            // 
            this.TextFormation.BackColor = System.Drawing.Color.Transparent;
            this.TextFormation.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TextFormation.Location = new System.Drawing.Point(70, 3);
            this.TextFormation.Name = "TextFormation";
            this.TextFormation.Size = new System.Drawing.Size(37, 16);
            this.TextFormation.TabIndex = 1;
            this.TextFormation.Text = "(陣形)";
            // 
            // TextAirSuperiority
            // 
            this.TextAirSuperiority.BackColor = System.Drawing.Color.Transparent;
            this.TextAirSuperiority.Location = new System.Drawing.Point(113, 3);
            this.TextAirSuperiority.Name = "TextAirSuperiority";
            this.TextAirSuperiority.Size = new System.Drawing.Size(80, 16);
            this.TextAirSuperiority.TabIndex = 2;
            this.TextAirSuperiority.Text = "(制空戦力)";
            // 
            // PanelEnemyFleet
            // 
            this.PanelEnemyFleet.AutoSize = true;
            this.PanelEnemyFleet.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PanelEnemyFleet.Controls.Add(this.TableEnemyMember);
            this.PanelEnemyFleet.Location = new System.Drawing.Point(3, 25);
            this.PanelEnemyFleet.Name = "PanelEnemyFleet";
            this.PanelEnemyFleet.Size = new System.Drawing.Size(240, 10);
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
            this.TableEnemyMember.MinimumSize = new System.Drawing.Size(240, 10);
            this.TableEnemyMember.Name = "TableEnemyMember";
            this.TableEnemyMember.RowCount = 1;
            this.TableEnemyMember.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableEnemyMember.Size = new System.Drawing.Size(240, 10);
            this.TableEnemyMember.TabIndex = 1;
            this.TableEnemyMember.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.TableEnemyMember_CellPaint);
            // 
            // ToolTipInfo
            // 
            this.ToolTipInfo.AutoPopDelay = 30000;
            this.ToolTipInfo.InitialDelay = 500;
            this.ToolTipInfo.ReshowDelay = 100;
            this.ToolTipInfo.ShowAlways = true;
            // 
            // FormFleetData
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(245, 164);
            this.ControlBox = false;
            this.Controls.Add(this.BasePanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFleetData";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "罗针盘";
            this.Load += new System.EventHandler(this.Form_Load);
            this.BasePanel.ResumeLayout(false);
            this.BasePanel.PerformLayout();
            this.PanelEnemyFleet.ResumeLayout(false);
            this.PanelEnemyFleet.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel BasePanel;
		private System.Windows.Forms.Panel PanelEnemyFleet;
		private ElectronicObserver.Window.Control.ImageLabel TextEnemyFleetName;
        private ElectronicObserver.Window.Control.ImageLabel TextFormation;
        private ElectronicObserver.Window.Control.ImageLabel TextAirSuperiority;
		private System.Windows.Forms.TableLayoutPanel TableEnemyMember;
        private System.Windows.Forms.ToolTip ToolTipInfo;
	}
}