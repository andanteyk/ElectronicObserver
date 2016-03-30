using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Plugins
{
    [Serializable]
    public abstract class ElectronicPlugin
    {
        [Obsolete]
        public bool Active
        {
            get;
            set;
        }

        public virtual Image MenuIcon
        {
            get
            {
                return null;
            }
        }
        public abstract string PluginName
        {
            get;
        }
        public virtual string Version
        {
            get
            {
                return "1.0.0.0";
            }
        }

        public virtual bool StartPlugin(FormMain main)
        {
            return true;
        }

        public virtual bool StopPlugin()
        {
            return true;
        }

        public virtual PluginSettingControl GetSettings()
        {
            return null;
        }
    }

    public interface IDialogPlugin
    {
        string ToolMenuTitle { get; }
        Form GetToolWindow();
    }

    public interface IDockPlugin
    {
        string ViewMenuTitle { get; }
        Form GetDockWindow();
    }

    public interface IObserverPlugin
    {
        bool OnBeforeRequest(Fiddler.Session oSession);

        bool OnBeforeResponse(Fiddler.Session oSession);

        bool OnAfterSessionComplete(Fiddler.Session oSession);
    }
}
