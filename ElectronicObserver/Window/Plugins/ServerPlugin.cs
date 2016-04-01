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
        public virtual PluginType PluginType
		{
			get { return PluginType.Service; }
		}

		public abstract string MenuTitle { get; }

		public virtual Form GetToolWindow()
		{
			return null;
		}

		public virtual PluginSettingControl GetSettings()
		{
			return null;
		}

		public virtual string Version
		{
			get { return "1.0.0.0"; }
		}

		public abstract bool RunService( FormMain main );

		public virtual System.Drawing.Image MenuIcon
		{
			get { return null; }
		}

        public virtual PluginUpdateInformation UpdateInformation
        {
            get { return new PluginUpdateInformation(PluginUpdateInformation.UpdateType.None); }
        }
	}
}
