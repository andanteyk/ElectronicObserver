using ElectronicObserver.Window.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

using Fiddler;

namespace CustomDeck
{
    public class Plugin : DialogPlugin
    {
        public override string MenuTitle
        {
            get { return "自定义编成"; }
        }

        public override string Version
        {
            get { return "1.0.0.0"; }
        }

        public override System.Windows.Forms.Form GetToolWindow()
        {
            return new MainForm();
        }

    }
}
