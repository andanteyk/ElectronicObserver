using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Plugins
{
	public abstract class DockPlugin : IPluginHost
	{
		public virtual PluginType PluginType => PluginType.DockContent;

		public abstract string MenuTitle { get; }

		public virtual Form GetToolWindow()
		{
			return null;
		}

		public virtual PluginSettingControl GetSettings()
		{
			return null;
		}

		public virtual string Version => "1.0.0.0";

		public virtual bool RunService(FormMain main)
		{
			return false;
		}

		public virtual System.Drawing.Image MenuIcon => null;
	}
}
