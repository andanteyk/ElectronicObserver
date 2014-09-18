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
			this.TableMember = new System.Windows.Forms.TableLayoutPanel();
			this.TableFleet = new System.Windows.Forms.TableLayoutPanel();
			this.shipStatusHP1 = new ElectronicObserver.Window.Control.ShipStatusHP();
			this.SuspendLayout();
			// 
			// TableMember
			// 
			this.TableMember.AutoSize = true;
			this.TableMember.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TableMember.ColumnCount = 3;
			this.TableMember.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableMember.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableMember.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableMember.Location = new System.Drawing.Point(0, 29);
			this.TableMember.Name = "TableMember";
			this.TableMember.RowCount = 1;
			this.TableMember.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
			this.TableMember.Size = new System.Drawing.Size(0, 21);
			this.TableMember.TabIndex = 1;
			this.TableMember.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.TableMember_CellPaint);
			// 
			// TableFleet
			// 
			this.TableFleet.AutoSize = true;
			this.TableFleet.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TableFleet.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
			this.TableFleet.ColumnCount = 1;
			this.TableFleet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableFleet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TableFleet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TableFleet.Location = new System.Drawing.Point(0, 0);
			this.TableFleet.Name = "TableFleet";
			this.TableFleet.RowCount = 1;
			this.TableFleet.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
			this.TableFleet.Size = new System.Drawing.Size(2, 23);
			this.TableFleet.TabIndex = 2;
			// 
			// shipStatusHP1
			// 
			this.shipStatusHP1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.shipStatusHP1.Location = new System.Drawing.Point(12, 132);
			this.shipStatusHP1.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.shipStatusHP1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
			this.shipStatusHP1.Name = "shipStatusHP1";
			this.shipStatusHP1.PrevValue = 66;
			this.shipStatusHP1.Size = new System.Drawing.Size(276, 49);
			this.shipStatusHP1.TabIndex = 3;
			this.shipStatusHP1.Text = "shipStatusHP1";
			this.shipStatusHP1.UsePrevValue = false;
			// 
			// FormFleet
			// 
			this.AutoHidePortion = 150D;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(300, 200);
			this.Controls.Add(this.shipStatusHP1);
			this.Controls.Add(this.TableFleet);
			this.Controls.Add(this.TableMember);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.HideOnClose = true;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "FormFleet";
			this.Text = "*not loaded*";
			this.Load += new System.EventHandler(this.FormFleet_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel TableMember;
		private System.Windows.Forms.TableLayoutPanel TableFleet;
		private Control.ShipStatusHP shipStatusHP1;
	}
}