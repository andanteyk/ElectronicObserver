namespace ElectronicObserver.Window.Control {
	partial class ShipStatusHP {
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

		#region コンポーネント デザイナーで生成されたコード

		/// <summary> 
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent() {
			this._HPBar = new ElectronicObserver.Window.Control.StatusBar();
			this.SuspendLayout();
			// 
			// _HPBar
			// 
			this._HPBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._HPBar.BarColor0 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this._HPBar.BarColorBackground = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
			this._HPBar.BarColorDecrement = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
			this._HPBar.BarColorIncrement = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
			this._HPBar.Location = new System.Drawing.Point(0, 15);
			this._HPBar.Name = "_HPBar";
			this._HPBar.PrevValue = 66;
			this._HPBar.Size = new System.Drawing.Size(80, 5);
			this._HPBar.TabIndex = 0;
			this._HPBar.UsePrevValue = false;
			// 
			// ShipStatusHP
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._HPBar);
			this.DoubleBuffered = true;
			this.Name = "ShipStatusHP";
			this.Size = new System.Drawing.Size(80, 20);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.ShipStatusHP_Paint);
			this.ResumeLayout(false);

		}

		#endregion

		private StatusBar _HPBar;

	}
}
