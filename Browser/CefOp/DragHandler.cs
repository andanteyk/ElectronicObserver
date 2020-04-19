using CefSharp;
using CefSharp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Browser.CefOp
{
	/// <summary>
	/// (たぶん)ドラッグ&ドロップを無効化します。
	/// </summary>
	public class DragHandler : IDragHandler
	{
		public bool OnDragEnter(IWebBrowser browserControl, IBrowser browser, IDragData dragData, DragOperationsMask mask)
		{
			return true;
		}

		public void OnDraggableRegionsChanged(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IList<DraggableRegion> regions)
		{
			// nop
		}
	}
}
