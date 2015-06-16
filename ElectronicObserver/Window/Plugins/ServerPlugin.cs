using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Plugins
{
	public abstract class ServerPlugin : IPluginHost
	{
		public PluginType PluginType
		{
			get { return PluginType.Service; }
		}

		public abstract string MenuTitle { get; }

		public virtual Form GetToolWindow()
		{
			return null;
		}

		public virtual Form GetSettingsWindow()
		{
			return null;
		}

		public virtual string Version
		{
			get { return "1.0.0.0"; }
		}

		public abstract Task<bool> RunService( FormMain main );
	}
}
