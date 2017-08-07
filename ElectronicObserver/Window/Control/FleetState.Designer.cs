namespace ElectronicObserver.Window.Control {
	partial class FleetState {
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
			this.LayoutBase = new System.Windows.Forms.FlowLayoutPanel();
			this.SuspendLayout();
			// 
			// LayoutBase
			// 
			this.LayoutBase.AutoSize = true;
			this.LayoutBase.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.LayoutBase.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LayoutBase.Location = new System.Drawing.Point(0, 0);
			this.LayoutBase.Name = "LayoutBase";
			this.LayoutBase.Size = new System.Drawing.Size(0, 0);
			this.LayoutBase.TabIndex = 0;
			// 
			// FleetState
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.LayoutBase);
			this.Name = "FleetState";
			this.Size = new System.Drawing.Size(0, 0);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel LayoutBase;
	}
}
