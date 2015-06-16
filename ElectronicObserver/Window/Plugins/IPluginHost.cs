using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Plugins
{
	public enum PluginType
	{
		DockContent = 0,
		Dialog,
		Service
	}

	public interface IPluginHost
	{
		PluginType PluginType { get; }

		string MenuTitle { get; }

		Form GetToolWindow();

		Form GetSettingsWindow();

		bool RunService( FormMain main );

		string Version { get; }
	}
}
