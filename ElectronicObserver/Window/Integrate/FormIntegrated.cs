using ElectronicObserver.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window.Integrate {

	public partial class FormIntegrated : DockContent {

		FormMain mainForm;

		IntPtr attachingWindow;
		// 戻すときに必要になる情報
		uint origStyle;
		IntPtr origOwner;
		WinAPI.RECT origWindowRect;
		IntPtr origMenu;

		// メニュー
		IntPtr menuDisplayedWindow;
		IntPtr prevDisplayedMenu;

		public FormIntegrated( FormMain parent ) {
			InitializeComponent();

			this.mainForm = parent;
		}

		private void Attach( IntPtr hWnd ) {

			if ( attachingWindow != IntPtr.Zero ) {
				if ( attachingWindow == hWnd ) {
					// 既にアタッチ済み
					return;
				}
				Detach();
			}

			origStyle = (uint)WinAPI.GetWindowLong( hWnd, WinAPI.GWL_STYLE );
			origOwner = WinAPI.GetWindowLong( hWnd, WinAPI.GWL_HWNDPARENT );

			// ターゲットが最大化されていたら戻す
			if ( ( origStyle & WinAPI.WS_MAXIMIZE ) != 0 ) {
				origStyle &= unchecked( (uint)~WinAPI.WS_MAXIMIZE );
				WinAPI.SetWindowLong( hWnd, WinAPI.GWL_STYLE, new IntPtr( unchecked( (int)origStyle ) ) );
			}

			// キャプションを設定
			StringBuilder stringBuilder = new StringBuilder( 32 );
			WinAPI.GetWindowText( hWnd, stringBuilder, stringBuilder.Capacity );
			Text = stringBuilder.ToString();

			// アイコンを設定
			IntPtr hicon = WinAPI.SendMessage( hWnd, WinAPI.WM_GETICON, (IntPtr)WinAPI.ICON_SMALL, IntPtr.Zero );
			if ( hicon == IntPtr.Zero ) {
				hicon = WinAPI.GeClassLong( hWnd, WinAPI.GCLP_HICON );
			}
			if ( hicon != IntPtr.Zero ) {
				this.Icon = Icon.FromHandle( hicon );
			}

			// メニューを取得
			origMenu = WinAPI.GetMenu( hWnd );

			WinAPI.GetWindowRect( hWnd, out origWindowRect );

			// このウィンドウの大きさ・位置を設定
			Show( mainForm.MainDockPanel, new Rectangle(
				origWindowRect.left,
				origWindowRect.top,
				origWindowRect.right - origWindowRect.left,
				origWindowRect.bottom - origWindowRect.top ) );

			// ターゲットを子ウィンドウに設定
			uint newStyle = origStyle;
			newStyle &= unchecked( (uint)~( WinAPI.WS_POPUP |
				WinAPI.WS_CAPTION |
				WinAPI.WS_BORDER |
				WinAPI.WS_THICKFRAME |
				WinAPI.WS_MINIMIZEBOX |
				WinAPI.WS_MAXIMIZEBOX ) );
			newStyle |= WinAPI.WS_CHILD;
			WinAPI.SetWindowLong( hWnd, WinAPI.GWL_STYLE, new IntPtr( unchecked( (int)newStyle ) ) );
			WinAPI.SetParent( hWnd, this.Handle );
			WinAPI.MoveWindow( hWnd, 0, 0, this.Width, this.Height, true );

			this.attachingWindow = hWnd;
		}

		private void Detach() {
			if ( attachingWindow != IntPtr.Zero ) {
				DetachMenu();
				WinAPI.SetParent( attachingWindow, IntPtr.Zero );
				WinAPI.SetWindowLong( attachingWindow, WinAPI.GWL_STYLE, new IntPtr( unchecked( (int)origStyle ) ) );
				WinAPI.SetWindowLong( attachingWindow, WinAPI.GWL_HWNDPARENT, origOwner );
				WinAPI.SetMenu( attachingWindow, origMenu );
				WinAPI.MoveWindow( attachingWindow, origWindowRect.left, origWindowRect.top,
					origWindowRect.right - origWindowRect.left,
					origWindowRect.bottom - origWindowRect.top, true );
				attachingWindow = IntPtr.Zero;
			}
		}

		private void DetachMenu() {
			// 現在表示されていたら消す
			if ( menuDisplayedWindow != IntPtr.Zero ) {
				WinAPI.SetMenu( menuDisplayedWindow, prevDisplayedMenu );
				menuDisplayedWindow = IntPtr.Zero;
			}
		}

		private void AttachMenu() {
			IntPtr frameWindow = WinAPI.GetAncestor( this.Handle, WinAPI.GA_ROOT );
			IntPtr currentMenu = WinAPI.GetMenu( frameWindow );
			if ( currentMenu != origMenu ) {
				DetachMenu();
				WinAPI.SetMenu( frameWindow, origMenu );
				menuDisplayedWindow = frameWindow;
				prevDisplayedMenu = currentMenu;
			}
		}

		public void Show( IntPtr hWnd ) {
			Attach( hWnd );
		}

		private void FormIntegrated_FormClosing( object sender, FormClosingEventArgs e ) {
			Detach();
		}

		private void FormIntegrated_Resize( object sender, EventArgs e ) {
			if ( attachingWindow != IntPtr.Zero ) {
				Size size = ClientSize;
				WinAPI.MoveWindow( attachingWindow, 0, 0, size.Width, size.Height, true );
			}
		}

		private void FormIntegrated_Activated( object sender, EventArgs e ) {
			BeginInvoke( (Action)( () => {
				if ( !this.IsDisposed ) {
					AttachMenu();
				}
			} ) );
		}

	}

}
