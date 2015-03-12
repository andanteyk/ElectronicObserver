namespace ElectronicObserver.Window.Integrate {
	partial class FormIntegrated {
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
			this.SuspendLayout();
			// 
			// FormIntegrated
			// 
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Name = "FormIntegrated";
			this.Activated += new System.EventHandler(this.FormIntegrated_Activated);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormIntegrated_FormClosing);
			this.Resize += new System.EventHandler(this.FormIntegrated_Resize);
			this.ResumeLayout(false);

		}

		#endregion
	}
}
