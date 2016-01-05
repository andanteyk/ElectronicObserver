using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window.Support {

	public class CustomFloatWindowFactory : DockPanelExtender.IFloatWindowFactory {

		public FloatWindow CreateFloatWindow( DockPanel dockPanel, DockPane pane, System.Drawing.Rectangle bounds ) {
			return new CustomFloatWindow( dockPanel, pane, bounds );
		}

		public FloatWindow CreateFloatWindow( DockPanel dockPanel, DockPane pane ) {
			return new CustomFloatWindow( dockPanel, pane );
		}

	}


	public class CustomFloatWindow : FloatWindow {

		public CustomFloatWindow( DockPanel dockPanel, DockPane pane )
			: base( dockPanel, pane ) {

			//FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
			DoubleClickTitleBarToDock = false;
		}

		public CustomFloatWindow( DockPanel dockPanel, DockPane pane, Rectangle bounds )
			: base( dockPanel, pane, bounds ) {

			//FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
			DoubleClickTitleBarToDock = false;
		}

	}
}
