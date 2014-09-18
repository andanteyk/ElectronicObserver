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
			this.shipStatusHP2 = new DigitalArsenal.Control.ShipStatusHP();
			this.shipStatusLevel2 = new DigitalArsenal.Control.ShipStatusLevel();
			this.shipStatusLevel1 = new DigitalArsenal.Control.ShipStatusLevel();
			this.shipStatusHP1 = new DigitalArsenal.Control.ShipStatusHP();
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
			this.tabPage1.Controls.Add(this.shipStatusHP2);
			this.tabPage1.Controls.Add(this.shipStatusLevel2);
			this.tabPage1.Controls.Add(this.shipStatusLevel1);
			this.tabPage1.Controls.Add(this.shipStatusHP1);
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
			// shipStatusHP2
			// 
			this.shipStatusHP2.Location = new System.Drawing.Point(435, 191);
			this.shipStatusHP2.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.shipStatusHP2.Name = "shipStatusHP2";
			this.shipStatusHP2.PrevValue = 66;
			this.shipStatusHP2.Size = new System.Drawing.Size(80, 20);
			this.shipStatusHP2.TabIndex = 5;
			this.shipStatusHP2.Text = "MP:";
			this.shipStatusHP2.UsePrevValue = false;
			this.shipStatusHP2.Value = 22;
			// 
			// shipStatusLevel2
			// 
			this.shipStatusLevel2.AutoSize = true;
			this.shipStatusLevel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.shipStatusLevel2.Location = new System.Drawing.Point(381, 68);
			this.shipStatusLevel2.MainFont = new System.Drawing.Font("Meiryo UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.shipStatusLevel2.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.shipStatusLevel2.Name = "shipStatusLevel2";
			this.shipStatusLevel2.Size = new System.Drawing.Size(161, 30);
			this.shipStatusLevel2.SubFont = new System.Drawing.Font("Meiryo UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.shipStatusLevel2.TabIndex = 4;
			this.shipStatusLevel2.Text = "HQ Level.";
			this.shipStatusLevel2.TextNext = "next...";
			this.shipStatusLevel2.Value = 134;
			this.shipStatusLevel2.ValueNext = 23396;
			// 
			// shipStatusLevel1
			// 
			this.shipStatusLevel1.AutoSize = true;
			this.shipStatusLevel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.shipStatusLevel1.Location = new System.Drawing.Point(182, 235);
			this.shipStatusLevel1.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.shipStatusLevel1.MaximumValue = 150;
			this.shipStatusLevel1.Name = "shipStatusLevel1";
			this.shipStatusLevel1.Size = new System.Drawing.Size(78, 20);
			this.shipStatusLevel1.TabIndex = 3;
			this.shipStatusLevel1.TextNext = "Next:";
			this.shipStatusLevel1.Value = 89;
			this.shipStatusLevel1.ValueNext = 162534;
			// 
			// shipStatusHP1
			// 
			this.shipStatusHP1.BackColor = System.Drawing.Color.MistyRose;
			this.shipStatusHP1.Location = new System.Drawing.Point(281, 235);
			this.shipStatusHP1.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.shipStatusHP1.Name = "shipStatusHP1";
			this.shipStatusHP1.PrevValue = 66;
			this.shipStatusHP1.RepairTime = new System.DateTime(2014, 9, 19, 0, 0, 0, 0);
			this.shipStatusHP1.Size = new System.Drawing.Size(80, 20);
			this.shipStatusHP1.SubFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.shipStatusHP1.TabIndex = 2;
			this.shipStatusHP1.UsePrevValue = false;
			this.shipStatusHP1.Value = 88;
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
		private System.Windows.Forms.Label label1;
		private Control.ShipStatusHP shipStatusHP1;
		private Control.ShipStatusLevel shipStatusLevel1;
		private Control.ShipStatusLevel shipStatusLevel2;
		private Control.ShipStatusHP shipStatusHP2;

	}
}

