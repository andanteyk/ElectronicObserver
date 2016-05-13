namespace ElectronicObserver.Window.Control {
	partial class ShipStatusEquipment {
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
			this.SuspendLayout();
			// 
			// ShipStatusEquipment
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.Transparent;
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Meiryo UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.Name = "ShipStatusEquipment";
			this.Size = new System.Drawing.Size(100, 20);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.ShipStatusEquipment_Paint);
			this.MouseEnter += new System.EventHandler(this.ShipStatusEquipment_MouseEnter);
			this.MouseLeave += new System.EventHandler(this.ShipStatusEquipment_MouseLeave);
			this.ResumeLayout(false);

		}

		#endregion
	}
}
