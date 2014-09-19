namespace DigitalArsenal {
	partial class FormMain {
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose( bool disposing ) {
			if ( disposing && ( components != null ) ) {
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.TableTest = new System.Windows.Forms.TableLayoutPanel();
			this.imageLabel9 = new DigitalArsenal.Control.ImageLabel();
			this.imageLabel8 = new DigitalArsenal.Control.ImageLabel();
			this.imageLabel7 = new DigitalArsenal.Control.ImageLabel();
			this.imageLabel6 = new DigitalArsenal.Control.ImageLabel();
			this.imageLabel5 = new DigitalArsenal.Control.ImageLabel();
			this.imageLabel4 = new DigitalArsenal.Control.ImageLabel();
			this.imageLabel3 = new DigitalArsenal.Control.ImageLabel();
			this.imageLabel2 = new DigitalArsenal.Control.ImageLabel();
			this.imageLabel1 = new DigitalArsenal.Control.ImageLabel();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(640, 480);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.imageLabel9);
			this.tabPage1.Controls.Add(this.imageLabel8);
			this.tabPage1.Controls.Add(this.imageLabel7);
			this.tabPage1.Controls.Add(this.imageLabel6);
			this.tabPage1.Controls.Add(this.imageLabel5);
			this.tabPage1.Controls.Add(this.imageLabel4);
			this.tabPage1.Controls.Add(this.imageLabel3);
			this.tabPage1.Controls.Add(this.imageLabel2);
			this.tabPage1.Controls.Add(this.imageLabel1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(632, 454);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "FreeSpace";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "fuel.png");
			this.imageList1.Images.SetKeyName(1, "ammo.png");
			this.imageList1.Images.SetKeyName(2, "steel.png");
			this.imageList1.Images.SetKeyName(3, "bauxite.png");
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.TableTest);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(632, 454);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Table";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// TableTest
			// 
			this.TableTest.AutoSize = true;
			this.TableTest.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TableTest.ColumnCount = 3;
			this.TableTest.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableTest.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableTest.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableTest.Location = new System.Drawing.Point(9, 7);
			this.TableTest.MinimumSize = new System.Drawing.Size(40, 20);
			this.TableTest.Name = "TableTest";
			this.TableTest.RowCount = 1;
			this.TableTest.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TableTest.Size = new System.Drawing.Size(40, 20);
			this.TableTest.TabIndex = 0;
			this.TableTest.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.TableTest_CellPaint);
			// 
			// imageLabel9
			// 
			this.imageLabel9.AutoSize = true;
			this.imageLabel9.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.imageLabel9.BackColor = System.Drawing.Color.Transparent;
			this.imageLabel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.imageLabel9.Image = ((System.Drawing.Image)(resources.GetObject("imageLabel9.Image")));
			this.imageLabel9.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.imageLabel9.ImageIndex = 0;
			this.imageLabel9.ImageList = this.imageList1;
			this.imageLabel9.ImageMargin = 3;
			this.imageLabel9.ImageSize = new System.Drawing.Size(16, 16);
			this.imageLabel9.Location = new System.Drawing.Point(190, 77);
			this.imageLabel9.Name = "imageLabel9";
			this.imageLabel9.Size = new System.Drawing.Size(97, 22);
			this.imageLabel9.TabIndex = 8;
			this.imageLabel9.Text = "imageLabel9";
			this.imageLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// imageLabel8
			// 
			this.imageLabel8.AutoSize = true;
			this.imageLabel8.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.imageLabel8.BackColor = System.Drawing.Color.Transparent;
			this.imageLabel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.imageLabel8.Image = ((System.Drawing.Image)(resources.GetObject("imageLabel8.Image")));
			this.imageLabel8.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.imageLabel8.ImageIndex = 0;
			this.imageLabel8.ImageList = this.imageList1;
			this.imageLabel8.ImageMargin = 3;
			this.imageLabel8.ImageSize = new System.Drawing.Size(16, 16);
			this.imageLabel8.Location = new System.Drawing.Point(109, 77);
			this.imageLabel8.Name = "imageLabel8";
			this.imageLabel8.Size = new System.Drawing.Size(75, 37);
			this.imageLabel8.TabIndex = 7;
			this.imageLabel8.Text = "imageLabel8";
			this.imageLabel8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// imageLabel7
			// 
			this.imageLabel7.AutoSize = true;
			this.imageLabel7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.imageLabel7.BackColor = System.Drawing.Color.Transparent;
			this.imageLabel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.imageLabel7.Image = ((System.Drawing.Image)(resources.GetObject("imageLabel7.Image")));
			this.imageLabel7.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.imageLabel7.ImageIndex = 0;
			this.imageLabel7.ImageList = this.imageList1;
			this.imageLabel7.ImageMargin = 3;
			this.imageLabel7.ImageSize = new System.Drawing.Size(16, 16);
			this.imageLabel7.Location = new System.Drawing.Point(6, 77);
			this.imageLabel7.Name = "imageLabel7";
			this.imageLabel7.Size = new System.Drawing.Size(97, 22);
			this.imageLabel7.TabIndex = 6;
			this.imageLabel7.Text = "imageLabel7";
			this.imageLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// imageLabel6
			// 
			this.imageLabel6.AutoSize = true;
			this.imageLabel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.imageLabel6.BackColor = System.Drawing.Color.Transparent;
			this.imageLabel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.imageLabel6.Image = ((System.Drawing.Image)(resources.GetObject("imageLabel6.Image")));
			this.imageLabel6.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.imageLabel6.ImageIndex = 0;
			this.imageLabel6.ImageList = this.imageList1;
			this.imageLabel6.ImageMargin = 3;
			this.imageLabel6.ImageSize = new System.Drawing.Size(16, 16);
			this.imageLabel6.Location = new System.Drawing.Point(190, 49);
			this.imageLabel6.Name = "imageLabel6";
			this.imageLabel6.Size = new System.Drawing.Size(97, 22);
			this.imageLabel6.TabIndex = 5;
			this.imageLabel6.Text = "imageLabel6";
			this.imageLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// imageLabel5
			// 
			this.imageLabel5.AutoSize = true;
			this.imageLabel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.imageLabel5.BackColor = System.Drawing.Color.Transparent;
			this.imageLabel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.imageLabel5.Image = ((System.Drawing.Image)(resources.GetObject("imageLabel5.Image")));
			this.imageLabel5.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.imageLabel5.ImageIndex = 0;
			this.imageLabel5.ImageList = this.imageList1;
			this.imageLabel5.ImageMargin = 3;
			this.imageLabel5.ImageSize = new System.Drawing.Size(16, 16);
			this.imageLabel5.Location = new System.Drawing.Point(109, 49);
			this.imageLabel5.Name = "imageLabel5";
			this.imageLabel5.Size = new System.Drawing.Size(75, 22);
			this.imageLabel5.TabIndex = 4;
			this.imageLabel5.Text = "imageLabel5";
			this.imageLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// imageLabel4
			// 
			this.imageLabel4.AutoSize = true;
			this.imageLabel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.imageLabel4.BackColor = System.Drawing.Color.Transparent;
			this.imageLabel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.imageLabel4.Image = ((System.Drawing.Image)(resources.GetObject("imageLabel4.Image")));
			this.imageLabel4.ImageIndex = 0;
			this.imageLabel4.ImageList = this.imageList1;
			this.imageLabel4.ImageMargin = 3;
			this.imageLabel4.ImageSize = new System.Drawing.Size(16, 16);
			this.imageLabel4.Location = new System.Drawing.Point(6, 49);
			this.imageLabel4.Name = "imageLabel4";
			this.imageLabel4.Size = new System.Drawing.Size(97, 22);
			this.imageLabel4.TabIndex = 3;
			this.imageLabel4.Text = "imageLabel4";
			this.imageLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// imageLabel3
			// 
			this.imageLabel3.AutoSize = true;
			this.imageLabel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.imageLabel3.BackColor = System.Drawing.Color.Transparent;
			this.imageLabel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.imageLabel3.Image = ((System.Drawing.Image)(resources.GetObject("imageLabel3.Image")));
			this.imageLabel3.ImageAlign = System.Drawing.ContentAlignment.TopRight;
			this.imageLabel3.ImageIndex = 0;
			this.imageLabel3.ImageList = this.imageList1;
			this.imageLabel3.ImageMargin = 3;
			this.imageLabel3.ImageSize = new System.Drawing.Size(16, 16);
			this.imageLabel3.Location = new System.Drawing.Point(190, 21);
			this.imageLabel3.Name = "imageLabel3";
			this.imageLabel3.Size = new System.Drawing.Size(97, 22);
			this.imageLabel3.TabIndex = 2;
			this.imageLabel3.Text = "imageLabel3";
			this.imageLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// imageLabel2
			// 
			this.imageLabel2.AutoSize = true;
			this.imageLabel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.imageLabel2.BackColor = System.Drawing.Color.Transparent;
			this.imageLabel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.imageLabel2.Image = ((System.Drawing.Image)(resources.GetObject("imageLabel2.Image")));
			this.imageLabel2.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.imageLabel2.ImageIndex = 0;
			this.imageLabel2.ImageList = this.imageList1;
			this.imageLabel2.ImageMargin = 3;
			this.imageLabel2.ImageSize = new System.Drawing.Size(16, 16);
			this.imageLabel2.Location = new System.Drawing.Point(109, 6);
			this.imageLabel2.Name = "imageLabel2";
			this.imageLabel2.Size = new System.Drawing.Size(75, 37);
			this.imageLabel2.TabIndex = 1;
			this.imageLabel2.Text = "imageLabel2";
			this.imageLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// imageLabel1
			// 
			this.imageLabel1.AutoSize = true;
			this.imageLabel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.imageLabel1.BackColor = System.Drawing.Color.Transparent;
			this.imageLabel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.imageLabel1.Image = ((System.Drawing.Image)(resources.GetObject("imageLabel1.Image")));
			this.imageLabel1.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
			this.imageLabel1.ImageIndex = 0;
			this.imageLabel1.ImageList = this.imageList1;
			this.imageLabel1.ImageMargin = 3;
			this.imageLabel1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageLabel1.Location = new System.Drawing.Point(6, 21);
			this.imageLabel1.Name = "imageLabel1";
			this.imageLabel1.Size = new System.Drawing.Size(97, 22);
			this.imageLabel1.TabIndex = 0;
			this.imageLabel1.Text = "imageLabel1";
			this.imageLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(640, 480);
			this.Controls.Add(this.tabControl1);
			this.Name = "FormMain";
			this.Text = "DigitalArsenal";
			this.Shown += new System.EventHandler(this.FormMain_Shown);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TableLayoutPanel TableTest;
		private System.Windows.Forms.ImageList imageList1;
		private Control.ImageLabel imageLabel1;
		private Control.ImageLabel imageLabel9;
		private Control.ImageLabel imageLabel8;
		private Control.ImageLabel imageLabel7;
		private Control.ImageLabel imageLabel6;
		private Control.ImageLabel imageLabel5;
		private Control.ImageLabel imageLabel4;
		private Control.ImageLabel imageLabel3;
		private Control.ImageLabel imageLabel2;

	}
}

