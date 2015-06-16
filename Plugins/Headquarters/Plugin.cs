using ElectronicObserver.Window.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overview
{
	public class Plugin : DockPlugin
	{
		public override string MenuTitle
		{
			get { return "司令部(&H)"; }
		}

		public override string Version
		{
			get { return "1.0.0"; }
		}
	}
}
