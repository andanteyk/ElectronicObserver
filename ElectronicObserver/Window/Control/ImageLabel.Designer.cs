namespace ElectronicObserver.Window.Control {
	partial class ImageLabel {
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
			this.Image = new System.Windows.Forms.PictureBox();
			this.Label = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.Image)).BeginInit();
			this.SuspendLayout();
			// 
			// Image
			// 
			this.Image.Location = new System.Drawing.Point(3, 3);
			this.Image.Name = "Image";
			this.Image.Size = new System.Drawing.Size(16, 16);
			this.Image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.Image.TabIndex = 0;
			this.Image.TabStop = false;
			this.Image.SizeChanged += new System.EventHandler(this.Image_SizeChanged);
			// 
			// Label
			// 
			this.Label.AutoSize = true;
			this.Label.Location = new System.Drawing.Point(22, 5);
			this.Label.Name = "Label";
			this.Label.Size = new System.Drawing.Size(0, 12);
			this.Label.TabIndex = 1;
			this.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.Label.SizeChanged += new System.EventHandler(this.Label_SizeChanged);
			// 
			// ImageLabel
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.Label);
			this.Controls.Add(this.Image);
			this.Name = "ImageLabel";
			this.Size = new System.Drawing.Size(25, 22);
			((System.ComponentModel.ISupportInitialize)(this.Image)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.PictureBox Image;
		public System.Windows.Forms.Label Label;
	}
}
