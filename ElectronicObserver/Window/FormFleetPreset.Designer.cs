namespace ElectronicObserver.Window
{
	partial class FormFleetPreset
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
            this.ToolTipInfo = new System.Windows.Forms.ToolTip(this.components);
            this.TablePresets = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // ToolTipInfo
            // 
            this.ToolTipInfo.AutoPopDelay = 30000;
            this.ToolTipInfo.InitialDelay = 500;
            this.ToolTipInfo.ReshowDelay = 100;
            // 
            // TablePresets
            // 
            this.TablePresets.AutoSize = true;
            this.TablePresets.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.TablePresets.ColumnCount = 7;
            this.TablePresets.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TablePresets.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TablePresets.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TablePresets.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TablePresets.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TablePresets.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TablePresets.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TablePresets.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TablePresets.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TablePresets.Location = new System.Drawing.Point(0, 0);
            this.TablePresets.Name = "TablePresets";
            this.TablePresets.RowCount = 1;
            this.TablePresets.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.TablePresets.Size = new System.Drawing.Size(0, 21);
            this.TablePresets.TabIndex = 4;
            this.TablePresets.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.TablePresets_CellPaint);
            // 
            // FormFleetPreset
            // 
            this.AutoHidePortion = 150D;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(300, 200);
            this.Controls.Add(this.TablePresets);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HideOnClose = true;
            this.Name = "FormFleetPreset";
            this.Text = "編成プリセット";
            this.Load += new System.EventHandler(this.FormFleetPreset_Load);
            this.Click += new System.EventHandler(this.FormFleetPreset_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolTip ToolTipInfo;
		private System.Windows.Forms.TableLayoutPanel TablePresets;
	}
}