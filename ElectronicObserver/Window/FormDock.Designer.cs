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
			this.TableDock = new System.Windows.Forms.TableLayoutPanel();
			this.ToolTipInfo = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// TableDock
			// 
			this.TableDock.AutoSize = true;
			this.TableDock.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TableDock.ColumnCount = 2;
			this.TableDock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableDock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableDock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TableDock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TableDock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TableDock.Location = new System.Drawing.Point(0, 0);
			this.TableDock.Name = "TableDock";
			this.TableDock.RowCount = 1;
			this.TableDock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
			this.TableDock.Size = new System.Drawing.Size(0, 21);
			this.TableDock.TabIndex = 2;
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
			this.AutoHidePortion = 150D;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(300, 200);
			this.Controls.Add(this.TableDock);
			this.DoubleBuffered = true;
			this.Font = Program.Window_Font;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.HideOnClose = true;
			this.Name = "FormDock";
			this.Text = "入渠";
			this.Load += new System.EventHandler(this.FormDock_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel TableDock;
		private System.Windows.Forms.ToolTip ToolTipInfo;
	}
}