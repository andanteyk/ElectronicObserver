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
			this.tabControl1.SuspendLayout();
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

	}
}

