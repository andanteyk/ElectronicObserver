using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResourceChart
{
	public class Plugin : DialogPlugin
	{
		public override string MenuTitle
		{
			get
			{
				return "资源图表(&G)";
			}
		}

		public override Form GetSettingsWindow()
		{
			return null;
		}

		public override Form GetToolWindow()
		{
			return new DialogResourcesGraph();
		}
	}
}
