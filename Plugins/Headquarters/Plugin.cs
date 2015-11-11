using ElectronicObserver.Resource;
using ElectronicObserver.Window.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headquarters
{
	public class Plugin : DockPlugin
	{

		public override string MenuTitle
		{
			get { return "司令部(&H)"; }
		}

		public override string Version
		{
			get { return "1.0.0.1"; }
		}

		public override PluginSettingControl GetSettings()
		{
			return new Settings();
		}

		public override System.Drawing.Image MenuIcon
		{
			get { return ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormHeadQuarters]; }
		}
	}
}
