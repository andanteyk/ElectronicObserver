using System;
using System.Collections.Generic;
using System.Drawing;
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
        Service,
        Observer,

        DockContentPlugin = 4,
        DialogPlugin = 8,
        ServicePlugin = 16,
        ObserverPlugin = 32
    }

	public interface IPluginHost
	{
		PluginType PluginType { get; }

		string MenuTitle { get; }

		Form GetToolWindow();

		PluginSettingControl GetSettings();

		bool RunService( FormMain main );

		string Version { get; }

		Image MenuIcon { get; }

        PluginUpdateInformation UpdateInformation { get; }
	}


}
