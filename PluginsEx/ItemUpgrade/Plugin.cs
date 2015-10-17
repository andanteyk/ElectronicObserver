using ElectronicObserver.Window.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ItemUpgrade
{
	public class Plugin : DialogPlugin
	{

		public override string MenuTitle
		{
			get { return "装备改修助手"; }
		}

		public override string Version
		{
			get { return "1.0.0.3"; }
		}

		public override Form GetToolWindow()
		{
            return new UpgradeHelper();
		}
	}
}
