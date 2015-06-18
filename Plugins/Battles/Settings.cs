using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Notifier;
using ElectronicObserver.Window.Plugins;

namespace Battles
{
	public partial class Settings : PluginSettingControl
	{
		public Settings()
		{
			InitializeComponent();
		}


		public override bool Save()
		{
			var config = ElectronicObserver.Utility.Configuration.Config;

			config.FormBattle.IsShortDamage = FormBattle_IsShortDamage.Checked;

			return true;
		}

		private void Settings_Load( object sender, EventArgs e )
		{
			var config = ElectronicObserver.Utility.Configuration.Config;

			FormBattle_IsShortDamage.Checked = config.FormBattle.IsShortDamage;
		}
	}
}
