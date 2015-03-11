namespace Browser
{
    partial class FormBrowser
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
			this.SizeAdjuster = new System.Windows.Forms.Panel();
			this.Browser = new System.Windows.Forms.WebBrowser();
			this.SizeAdjuster.SuspendLayout();
			this.SuspendLayout();
			// 
			// SizeAdjuster
			// 
			this.SizeAdjuster.Controls.Add(this.Browser);
			this.SizeAdjuster.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SizeAdjuster.Location = new System.Drawing.Point(0, 0);
			this.SizeAdjuster.Name = "SizeAdjuster";
			this.SizeAdjuster.Size = new System.Drawing.Size(284, 261);
			this.SizeAdjuster.TabIndex = 0;
			this.SizeAdjuster.SizeChanged += new System.EventHandler(this.SizeAdjuster_SizeChanged);
			// 
			// Browser
			// 
			this.Browser.AllowWebBrowserDrop = false;
			this.Browser.IsWebBrowserContextMenuEnabled = false;
			this.Browser.Location = new System.Drawing.Point(0, 0);
			this.Browser.MinimumSize = new System.Drawing.Size(20, 20);
			this.Browser.Name = "Browser";
			this.Browser.ScriptErrorsSuppressed = true;
			this.Browser.Size = new System.Drawing.Size(284, 261);
			this.Browser.TabIndex = 0;
			this.Browser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.Browser_DocumentCompleted);
			// 
			// FormBrowser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.SizeAdjuster);
			this.Name = "FormBrowser";
			this.Text = "FormBrowser";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormBrowser_FormClosed);
			this.Load += new System.EventHandler(this.FormBrowser_Load);
			this.SizeAdjuster.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel SizeAdjuster;
        private System.Windows.Forms.WebBrowser Browser;

    }
}

