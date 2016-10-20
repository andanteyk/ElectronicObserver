namespace ElectronicObserver.Window.Control {
	partial class ShipStatusLevel {
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
			// ShipStatusLevel
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.DoubleBuffered = true;
			this.Name = "ShipStatusLevel";
			this.Size = new System.Drawing.Size(80, 20);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.ShipStatusLevel_Paint);
			this.ResumeLayout(false);

		}

		#endregion
	}
}
