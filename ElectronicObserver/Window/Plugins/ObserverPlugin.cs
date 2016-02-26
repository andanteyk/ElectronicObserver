using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Plugins
{
	public abstract class ObserverPlugin : IPluginHost
	{
		public PluginType PluginType
		{
			get { return PluginType.Observer; }
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

		public virtual bool RunService( FormMain main )
		{
			return false;
		}

		public virtual System.Drawing.Image MenuIcon
		{
			get { return null; }
		}

		public abstract bool OnBeforeRequest( Fiddler.Session oSession );

		public abstract bool OnBeforeResponse( Fiddler.Session oSession );

		public abstract bool OnAfterSessionComplete( Fiddler.Session oSession );
	}
}
