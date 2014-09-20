namespace ElectronicObserver.Window.Control {
	partial class ShipStatusResource {
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
			this.BarAmmo = new ElectronicObserver.Window.Control.StatusBar();
			this.BarFuel = new ElectronicObserver.Window.Control.StatusBar();
			this.SuspendLayout();
			// 
			// BarAmmo
			// 
			this.BarAmmo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.BarAmmo.BarColor0 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BarAmmo.BarColorBackground = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
			this.BarAmmo.BarColorDecrement = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
			this.BarAmmo.BarColorIncrement = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
			this.BarAmmo.Location = new System.Drawing.Point(0, 13);
			this.BarAmmo.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.BarAmmo.Name = "BarAmmo";
			this.BarAmmo.PrevValue = 66;
			this.BarAmmo.Size = new System.Drawing.Size(60, 5);
			this.BarAmmo.TabIndex = 1;
			this.BarAmmo.UsePrevValue = false;
			// 
			// BarFuel
			// 
			this.BarFuel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.BarFuel.BarColor0 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BarFuel.BarColorBackground = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
			this.BarFuel.BarColorDecrement = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
			this.BarFuel.BarColorIncrement = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
			this.BarFuel.Location = new System.Drawing.Point(0, 2);
			this.BarFuel.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.BarFuel.Name = "BarFuel";
			this.BarFuel.PrevValue = 66;
			this.BarFuel.Size = new System.Drawing.Size(60, 5);
			this.BarFuel.TabIndex = 0;
			this.BarFuel.UsePrevValue = false;
			// 
			// ShipStatusResource
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.BarAmmo);
			this.Controls.Add(this.BarFuel);
			this.Name = "ShipStatusResource";
			this.Size = new System.Drawing.Size(60, 20);
			this.ResumeLayout(false);

		}

		#endregion

		private StatusBar BarFuel;
		private StatusBar BarAmmo;

	}
}
