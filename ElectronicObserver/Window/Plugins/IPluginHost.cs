using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Window.Plugins
{
	public enum PluginType
	{
		DockForm = 0,
		Server,
		Dialog
	}

	public interface IPluginHost
	{
		PluginType PluginType { get; }

		string MenuTitle { get; }

		// todo...



	}
}
