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
		public PluginType PluginType
		{
			get { return Plugins.PluginType.DockContent; }
		}

		public abstract string MenuTitle { get; }

		public virtual Form GetToolWindow()
		{
			return null;
		}

		public virtual UserControl GetSettings()
		{
			return null;
		}

		public virtual string Version
		{
			get { return "1.0.0.0"; }
		}

		public virtual Task<bool> RunService( FormMain main )
		{
			return new Task<bool>( () => false );
		}
	}
}
