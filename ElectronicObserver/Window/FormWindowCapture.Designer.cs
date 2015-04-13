namespace ElectronicObserver.Window {
	partial class FormWindowCapture {
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
			this.windowCaptureButton = new ElectronicObserver.Window.Control.WindowCaptureButton();
			this.SuspendLayout();
			// 
			// windowCaptureButton
			// 
			this.windowCaptureButton.Location = new System.Drawing.Point(12, 12);
			this.windowCaptureButton.Name = "windowCaptureButton";
			this.windowCaptureButton.Size = new System.Drawing.Size(27, 27);
			this.windowCaptureButton.TabIndex = 0;
			this.windowCaptureButton.UseVisualStyleBackColor = true;
			this.windowCaptureButton.WindowCaptured += new ElectronicObserver.Window.Control.WindowCaptureButton.WindowCapturedDelegate(this.windowCaptureButton_WindowCaptured);
			// 
			// FormWindowCapture
			// 
			this.AutoHidePortion = 150D;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(300, 200);
			this.Controls.Add(this.windowCaptureButton);
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.HideOnClose = true;
			this.Name = "FormWindowCapture";
			this.Text = "ウィンドウキャプチャ";
			this.ResumeLayout(false);

		}

		#endregion

		private Control.WindowCaptureButton windowCaptureButton;

	}
}
