using ElectronicObserver.Window.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest
{
	public class Plugin : DockPlugin
	{
		public override string MenuTitle
		{
			get { return "任务(&Q)"; }
		}

		public override string Version
		{
			get { return "1.0.0"; }
		}
	}
}
