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
			this.SuspendLayout();
			// 
			// ShipStatusHP
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.DoubleBuffered = true;
			this.Name = "ShipStatusHP";
			this.Size = new System.Drawing.Size(80, 20);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.ShipStatusHP_Paint);
			this.MouseEnter += new System.EventHandler(this.ShipStatusHP_MouseEnter);
			this.MouseLeave += new System.EventHandler(this.ShipStatusHP_MouseLeave);
			this.ResumeLayout(false);

		}

		#endregion


	}
}
