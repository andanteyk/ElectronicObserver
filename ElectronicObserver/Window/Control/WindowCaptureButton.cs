using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Integrate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Control {

	public partial class WindowCaptureButton : Button {

		private FormCapturing CapturingImageWindow = new FormCapturing();
		private FormCandidate CandidateBoxWindow = new FormCandidate();

		private bool selectingWindow = false;
		private IntPtr currentCandidate;
		
		public delegate void WindowCapturedDelegate( IntPtr hWnd );
		public event WindowCapturedDelegate WindowCaptured = delegate { };

		public WindowCaptureButton() {
			InitializeComponent();
		}

		private void OnMouseMoved() {
			Point cursor = System.Windows.Forms.Cursor.Position;

			CapturingImageWindow.Location = new Point(
				cursor.X - this.Image.Width / 2,
				cursor.Y - this.Image.Height / 2
				);

			IntPtr newCandidate = RootWindowFromPoint( cursor );
			if ( currentCandidate != newCandidate ) {
				if ( newCandidate == IntPtr.Zero ) {
					CandidateBoxWindow.Visible = false;
				} else {
					// ウィンドウ選択が変わったので移動
					WinAPI.RECT candidateRect;
					WinAPI.GetWindowRect( newCandidate, out candidateRect );
					CandidateBoxWindow.Bounds = new Rectangle( candidateRect.left, candidateRect.top,
						candidateRect.right - candidateRect.left, candidateRect.bottom - candidateRect.top );
					if ( !CandidateBoxWindow.Visible ) {
						CandidateBoxWindow.Visible = true;
					}
				}
				currentCandidate = newCandidate;
			}
		}

		private void OnMouseUp() {

			IntPtr selected = currentCandidate;
			OnCanceled();

			if ( selected != IntPtr.Zero ) {
				WindowCaptured( selected );
				/*
				int capacity = WinAPI.GetWindowTextLength( selected ) * 2;
				StringBuilder stringBuilder = new StringBuilder( capacity );
				WinAPI.GetWindowText( selected, stringBuilder, stringBuilder.Capacity );

				MessageBox.Show( stringBuilder.ToString() );
				 * */
			}
		}

		private void OnCanceled() {
			CapturingImageWindow.Visible = false;
			CandidateBoxWindow.Visible = false;
			currentCandidate = IntPtr.Zero;
			selectingWindow = false;
		}

		private IntPtr RootWindowFromPoint( Point cursor ) {
			StringBuilder className = new StringBuilder( 256 );
			StringBuilder windowText = new StringBuilder( 256 );
			IntPtr result = IntPtr.Zero;
			int currentProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;
			WinAPI.EnumWindows( (WinAPI.EnumWindowsDelegate)( ( hWnd, lparam ) => {
				if ( CapturingImageWindow.Handle != hWnd &&
					CandidateBoxWindow.Handle != hWnd ) {
					WinAPI.GetClassName( hWnd, className, className.Capacity );
					WinAPI.GetWindowText( hWnd, windowText, windowText.Capacity );
					uint processId;
					WinAPI.GetWindowThreadProcessId( hWnd, out processId );
					if ( className.Length > 0 &&
						windowText.Length > 0 &&
						WinAPI.IsWindowVisible( hWnd ) &&
						windowText.ToString() != "Program Manager" &&
						processId != currentProcessId ) {
						WinAPI.RECT rect;
						WinAPI.GetWindowRect( hWnd, out rect );
						if ( rect.left <= cursor.X && cursor.X <= rect.right && rect.top <= cursor.Y && cursor.Y <= rect.bottom ) {
							result = hWnd;
							return false;
						}
					}
				}
				return true;
			} ), IntPtr.Zero );
			return result;
		}

		protected override void WndProc( ref Message m ) {
			if ( selectingWindow ) {
				// マウスをキャプチャしている時だけ
				switch ( m.Msg ) {
					case WinAPI.WM_MOUSEMOVE:
						OnMouseMoved();
						break;
					case WinAPI.WM_LBUTTONUP:
						OnMouseUp();
						break;
					case WinAPI.WM_CANCELMODE:
					case WinAPI.WM_CAPTURECHANGED:
						OnCanceled();
						break;
				}
				return;
			}
			base.WndProc( ref m );
		}

		protected override void OnMouseDown( MouseEventArgs mevent ) {
			base.OnMouseDown( mevent );
			Capture = true;
			selectingWindow = true;

			Point cursor = System.Windows.Forms.Cursor.Position;
			CapturingImageWindow.Location = new Point(
				cursor.X - this.Image.Width / 2,
				cursor.Y - this.Image.Height / 2
				);

			CapturingImageWindow.BackgroundImage = Image;
			CapturingImageWindow.Show();
			CapturingImageWindow.Size = this.Image.Size;
		}
	}
}
