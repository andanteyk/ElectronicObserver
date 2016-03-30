using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Plugins
{
	public abstract class DialogPlugin : IPluginHost
	{
        public virtual PluginType PluginType
		{
			get { return PluginType.Dialog; }
		}

		public abstract string MenuTitle { get; }

		public abstract Form GetToolWindow();

		public virtual PluginSettingControl GetSettings()
		{
			return null;
		}

		public virtual string Version
		{
			get { return "1.0.0.0"; }
		}

		public virtual bool RunService( FormMain main )
		{
			return false;
		}

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
