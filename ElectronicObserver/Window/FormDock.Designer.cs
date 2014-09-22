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
			this.TableDock = new System.Windows.Forms.TableLayoutPanel();
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
			// 
			// FormDock
			// 
			this.AutoHidePortion = 150D;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(300, 200);
			this.Controls.Add(this.TableDock);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.HideOnClose = true;
			this.Name = "FormDock";
			this.Text = "入渠";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel TableDock;
	}
}