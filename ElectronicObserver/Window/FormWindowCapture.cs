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

	public partial class FormWindowCapture : DockContent {

		public static readonly String WARNING_MESSAGE = 
				"このウィンドウをキャプチャします。よろしいですか？\r\n\r\n" +
				"注意: 取り込んでも安全なウィンドウだけ取り込んでください。\r\n" +
				"非対応ウィンドウを取り込むとシステムが不安定になる恐れがあります。\r\n" +
				"（取り込んでも安全かどうかは取り込んでみないと分かりませんが・・・）";

		FormMain parent;

		List<FormIntegrated> capturedWindows = new List<FormIntegrated>();

		public FormWindowCapture( FormMain parent ) {
			InitializeComponent();

			this.parent = parent;
			this.Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormConfiguration] );
			this.windowCaptureButton.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormConfiguration];

			SystemEvents.SystemShuttingDown += SystemEvents_SystemShuttingDown;
		}

		private void SystemEvents_SystemShuttingDown() {
			DetachAll();
		}

		public void AddCapturedWindow( FormIntegrated form ) {
			capturedWindows.Add( form );
		}

		public void AttachAll() {
			capturedWindows.ForEach( form => form.Grab() );
		}

		public void DetachAll() {
			// ウィンドウのzオーダー維持のためデタッチはアタッチの逆順で行う
			for ( int i = capturedWindows.Count; i > 0; --i ) {
				capturedWindows[i - 1].Detach();
			}
		}

		public void CloseAll() {
			DetachAll();
			capturedWindows.ForEach( form => form.Close() );
			capturedWindows.Clear();
		}

		private void windowCaptureButton_WindowCaptured( IntPtr hWnd ) {

			int capacity = WinAPI.GetWindowTextLength( hWnd ) * 2;
			StringBuilder stringBuilder = new StringBuilder( capacity );
			WinAPI.GetWindowText( hWnd, stringBuilder, stringBuilder.Capacity );

			var result = MessageBox.Show( stringBuilder.ToString() + "\r\n" + WARNING_MESSAGE,
				SoftwareInformation.SoftwareNameJapanese, MessageBoxButtons.YesNoCancel);

			if ( result == System.Windows.Forms.DialogResult.Yes ) {
				FormIntegrated form = new FormIntegrated( parent );
				form.Show( hWnd );
			}
		}

		protected override string GetPersistString() {
			return "WindowCapture";
		}


	}

}
