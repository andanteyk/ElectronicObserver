namespace ElectronicObserver.Window
{
	partial class FormArsenal
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.TableArsenal = new System.Windows.Forms.TableLayoutPanel();
			this.ToolTipInfo = new System.Windows.Forms.ToolTip(this.components);
			this.MenuMain = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.MenuMain_ShowShipName = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// TableArsenal
			// 
			this.TableArsenal.AutoSize = true;
			this.TableArsenal.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TableArsenal.ColumnCount = 2;
			this.TableArsenal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableArsenal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableArsenal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TableArsenal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TableArsenal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TableArsenal.Location = new System.Drawing.Point(0, 0);
			this.TableArsenal.Name = "TableArsenal";
			this.TableArsenal.RowCount = 1;
			this.TableArsenal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
			this.TableArsenal.Size = new System.Drawing.Size(0, 21);
			this.TableArsenal.TabIndex = 3;
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
			this.MenuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.MenuMain_ShowShipName});
			this.MenuMain.Name = "MenuMain";
			this.MenuMain.Size = new System.Drawing.Size(155, 26);
			// 
			// MenuMain_ShowShipName
			// 
			this.MenuMain_ShowShipName.CheckOnClick = true;
			this.MenuMain_ShowShipName.Name = "MenuMain_ShowShipName";
			this.MenuMain_ShowShipName.Size = new System.Drawing.Size(154, 22);
			this.MenuMain_ShowShipName.Text = "艦名を表示(&V)";
			this.MenuMain_ShowShipName.CheckedChanged += new System.EventHandler(this.MenuMain_ShowShipName_CheckedChanged);
			// 
			// FormArsenal
			// 
			this.AutoHidePortion = 150D;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(300, 200);
			this.ContextMenuStrip = this.MenuMain;
			this.Controls.Add(this.TableArsenal);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.HideOnClose = true;
			this.Name = "FormArsenal";
			this.Text = "工廠";
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