using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Integrate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window {

	/// <summary>
	/// ウィンドウキャプチャ
	/// </summary>
	public partial class FormWindowCapture : DockContent {

		public static readonly String WARNING_MESSAGE = 
				"このウィンドウをキャプチャします。よろしいですか？\r\n\r\n" +
				"注意: ウィンドウによっては正常に取り込めず、不安定になる可能性があります。\r\n" +
				"データを保存する・母港に戻るなど、安全を確保してから実行してください。\r\n";

		private FormMain parent;

		private List<FormIntegrate> capturedWindows = new List<FormIntegrate>();

		public FormWindowCapture( FormMain parent ) {
			InitializeComponent();

			this.parent = parent;
			this.Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormWindowCapture] );
			this.windowCaptureButton.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormWindowCapture];

			SystemEvents.SystemShuttingDown += SystemEvents_SystemShuttingDown;
		}

		private void SystemEvents_SystemShuttingDown() {
			DetachAll();
		}

		/// <summary>
		/// FormIntegrateが新しく作られたら追加
		/// </summary>
		public void AddCapturedWindow( FormIntegrate form ) {
			capturedWindows.Add( form );
		}

		/// <summary>
		/// ウィンドウを取り込めていないFormIntegrateでウィンドウの検索と取り込みを実行
		/// </summary>
		public void AttachAll() {
			capturedWindows.ForEach( form => form.Grab() );
		}

		/// <summary>
		/// 取り込んだウィンドウを全て開放
		/// </summary>
		public void DetachAll() {
			// ウィンドウのzオーダー維持のためデタッチはアタッチの逆順で行う
			for ( int i = capturedWindows.Count; i > 0; --i ) {
				capturedWindows[i - 1].Detach();
			}
		}

		/// <summary>
		/// FormIntegrateを全て破棄する
		/// </summary>
		public void CloseAll() {
			DetachAll();
			capturedWindows.ForEach( form => form.Close() );
			capturedWindows.Clear();
		}

		private void windowCaptureButton_WindowCaptured( IntPtr hWnd ) {

			int capacity = WinAPI.GetWindowTextLength( hWnd ) * 2;
			StringBuilder stringBuilder = new StringBuilder( capacity );
			WinAPI.GetWindowText( hWnd, stringBuilder, stringBuilder.Capacity );

			if ( MessageBox.Show( stringBuilder.ToString() + "\r\n" + WARNING_MESSAGE,
				"ウィンドウキャプチャの確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question )
				== System.Windows.Forms.DialogResult.Yes ) {

				FormIntegrate form = new FormIntegrate( parent );
				form.Show( hWnd );
			}
		}

		protected override string GetPersistString() {
			return "WindowCapture";
		}


	}

}
