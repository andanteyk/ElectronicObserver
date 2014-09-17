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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.label1 = new System.Windows.Forms.Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.TableTest = new System.Windows.Forms.TableLayoutPanel();
			this.shipStatusHP1 = new DigitalArsenal.Control.ShipStatusHP();
			this.shipStatusLevel1 = new DigitalArsenal.Control.ShipStatusLevel();
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
			this.tabPage1.Controls.Add(this.shipStatusHP1);
			this.tabPage1.Controls.Add(this.shipStatusLevel1);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(632, 454);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "FreeSpace";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label1.Location = new System.Drawing.Point(77, 56);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(185, 121);
			this.label1.TabIndex = 0;
			this.label1.Text = "label1";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
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
			// shipStatusHP1
			// 
			this.shipStatusHP1.BackColor = System.Drawing.Color.MistyRose;
			this.shipStatusHP1.Location = new System.Drawing.Point(281, 235);
			this.shipStatusHP1.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.shipStatusHP1.Name = "shipStatusHP1";
			this.shipStatusHP1.PrevValue = 66;
			this.shipStatusHP1.RepairTime = new System.DateTime(2014, 9, 17, 20, 0, 0, 0);
			this.shipStatusHP1.Size = new System.Drawing.Size(80, 20);
			this.shipStatusHP1.SubFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.shipStatusHP1.TabIndex = 2;
			this.shipStatusHP1.Value = 88;
			// 
			// shipStatusLevel1
			// 
			this.shipStatusLevel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.shipStatusLevel1.Location = new System.Drawing.Point(215, 235);
			this.shipStatusLevel1.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.shipStatusLevel1.MaximumValue = 150;
			this.shipStatusLevel1.Name = "shipStatusLevel1";
			this.shipStatusLevel1.Size = new System.Drawing.Size(60, 20);
			this.shipStatusLevel1.TabIndex = 1;
			this.shipStatusLevel1.Value = 79;
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
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TableLayoutPanel TableTest;
		private System.Windows.Forms.Label label1;
		private Control.ShipStatusLevel shipStatusLevel1;
		private Control.ShipStatusHP shipStatusHP1;

	}
}

