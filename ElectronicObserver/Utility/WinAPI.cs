using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility {
	
	public static class WinAPI {

		[DllImport( "user32.dll", SetLastError = true )]
		public static extern bool GetWindowRect( IntPtr hWnd, out RECT lpRect );

		public delegate bool EnumWindowsDelegate( IntPtr hWnd, IntPtr lparam );

		[DllImport( "user32.dll", SetLastError = true )]
		public extern static bool EnumWindows( EnumWindowsDelegate lpEnumFunc, IntPtr lparam );

		[DllImport( "user32.dll", SetLastError = true )]
		public static extern bool IsWindowVisible( IntPtr hWnd );

		[DllImport( "user32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		public static extern int GetWindowText( IntPtr hWnd, StringBuilder lpString, int nMaxCount );

		[DllImport( "user32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		public static extern int GetWindowTextLength( IntPtr hWnd );

		[DllImport( "user32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		public static extern int GetClassName( IntPtr hWnd, StringBuilder lpClassName, int nMaxCount );

		[DllImport( "user32.dll", SetLastError = true )]
		public static extern uint GetWindowThreadProcessId( IntPtr hWnd, out uint lpdwProcessId );

		[DllImport( "user32.dll", SetLastError = true )]
		public static extern bool ClientToScreen( IntPtr hWnd, out Point lpPoint );

		[DllImport( "user32.dll", SetLastError = true )]
		public static extern bool GetClientRect( IntPtr hWnd, out RECT lpRect );

		[DllImport( "user32.dll", SetLastError = true )]
		public static extern bool AdjustWindowRect( ref RECT lpRect, uint dwStyle, bool bMenu );

		[DllImport( "user32.dll", EntryPoint = "GetWindowLong", SetLastError = true )]
		private static extern IntPtr GetWindowLong32( IntPtr hWnd, int nIndex );

		[DllImport( "user32.dll", EntryPoint = "GetWindowLongPtr", SetLastError = true )]
		private static extern IntPtr GetWindowLong64( IntPtr hWnd, int nIndex );

		public static IntPtr GetWindowLong( IntPtr hWnd, int nIndex ) {
			if ( IntPtr.Size == 4 ) {
				return GetWindowLong32( hWnd, nIndex );
			}
			return GetWindowLong64( hWnd, nIndex );
		}

		[DllImport( "user32.dll", EntryPoint = "GetClassLong", SetLastError = true )]
		private static extern IntPtr GetClassLong32( IntPtr hWnd, int nIndex );

		[DllImport( "user32.dll", EntryPoint = "GetClassLongPtr", SetLastError = true )]
		private static extern IntPtr GetClassLong64( IntPtr hWnd, int nIndex );

		public static IntPtr GeClassLong( IntPtr hWnd, int nIndex ) {
			if ( IntPtr.Size == 4 ) {
				return GetClassLong32( hWnd, nIndex );
			}
			return GetClassLong64( hWnd, nIndex );
		}

		[DllImport( "user32.dll", EntryPoint = "SetWindowLong", SetLastError = true )]
		public static extern IntPtr SetWindowLong32( IntPtr hwnd, int nIndex, IntPtr dwNewLong );

		[DllImport( "user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true )]
		public static extern IntPtr SetWindowLong64( IntPtr hwnd, int nIndex, IntPtr dwNewLong );

		public static IntPtr SetWindowLong( IntPtr hWnd, int nIndex, IntPtr dwNewLong ) {
			if ( IntPtr.Size == 4 ) {
				return SetWindowLong32( hWnd, nIndex, dwNewLong );
			}
			return SetWindowLong64( hWnd, nIndex, dwNewLong );
		}

		[DllImport( "user32.dll", SetLastError = true )]
		public static extern uint SetParent( IntPtr hWndChild, IntPtr hWndNewParent );

		[DllImport( "user32.dll", SetLastError = true )]
		public static extern bool MoveWindow( IntPtr hwnd, int x, int y, int cx, int cy, bool repaint );

		[DllImport( "user32.dll", SetLastError = true )]
		public static extern IntPtr SendMessage( IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam );

		[DllImport( "user32.dll", SetLastError = true )]
		public static extern bool PostMessage( IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam );

		[DllImport( "user32.dll", SetLastError = true )]
		public static extern IntPtr GetMenu( IntPtr hWnd );

		[DllImport( "user32.dll", SetLastError = true )]
		public static extern bool SetMenu( IntPtr hWnd, IntPtr hMenu );

		[DllImport( "user32.dll", SetLastError = true )]
		public static extern IntPtr GetAncestor( IntPtr hWnd, int gaFlags );

		[StructLayout( LayoutKind.Sequential )]
		public struct RECT {
			public int left;
			public int top;
			public int right;
			public int bottom;
		}

		public const int WM_MOUSEMOVE = 0x0200;
		public const int WM_LBUTTONUP = 0x0202;
		public const int WM_CANCELMODE = 0x001F;
		public const int WM_CAPTURECHANGED = 0x0215;
		public const int WM_GETICON = 0x007F;

		public const int ICON_SMALL = 0;

		public const int GCLP_HICON = ( -14 );

		public const int GA_PARENT = 1;
		public const int GA_ROOT = 2;
		public const int GA_ROOTOWNER = 3;

		public const int GWL_HWNDPARENT = ( -8 );
		public const int GWL_STYLE = ( -16 );

		public const int WS_OVERLAPPED = 0x00000000;
		public const uint WS_POPUP = 0x80000000U;
		public const int WS_CHILD = 0x40000000;
		public const int WS_MINIMIZE = 0x20000000;
		public const int WS_VISIBLE = 0x10000000;
		public const int WS_DISABLED = 0x08000000;
		public const int WS_CLIPSIBLINGS = 0x04000000;
		public const int WS_CLIPCHILDREN = 0x02000000;
		public const int WS_MAXIMIZE = 0x01000000;
		public const int WS_CAPTION = 0x00C00000;/* WS_BORDER | WS_DLGFRAME  */
		public const int WS_BORDER = 0x00800000;
		public const int WS_DLGFRAME = 0x00400000;
		public const int WS_VSCROLL = 0x00200000;
		public const int WS_HSCROLL = 0x00100000;
		public const int WS_SYSMENU = 0x00080000;
		public const int WS_THICKFRAME = 0x00040000;
		public const int WS_GROUP = 0x00020000;
		public const int WS_TABSTOP = 0x00010000;

		public const int WS_MINIMIZEBOX = 0x00020000;
		public const int WS_MAXIMIZEBOX = 0x00010000;

		public const int WS_TILED = WS_OVERLAPPED;
		public const int WS_ICONIC = WS_MINIMIZE;
		public const int WS_SIZEBOX = WS_THICKFRAME;
		public const int WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW;

		public const int WS_OVERLAPPEDWINDOW = ( WS_OVERLAPPED |
										 WS_CAPTION |
										 WS_SYSMENU |
										 WS_THICKFRAME |
										 WS_MINIMIZEBOX |
										 WS_MAXIMIZEBOX );

		public const uint WS_POPUPWINDOW = ( WS_POPUP |
										 WS_BORDER |
										 WS_SYSMENU );

		public const int WS_CHILDWINDOW = ( WS_CHILD );
	}
}
