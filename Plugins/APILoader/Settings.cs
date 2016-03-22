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
using ElectronicObserver.Utility;

namespace APILoader
{
	public partial class Settings : PluginSettingControl
	{
		private Plugin plugin;

		public Settings( Plugin plugin )
		{
			this.plugin = plugin;
			InitializeComponent();
		}


		public override bool Save()
		{
			var config = ElectronicObserver.Utility.Configuration.Config;

			//[デバッグ]
			config.Debug.EnableDebugMenu = Debug_EnableDebugMenu.Checked;
			config.Debug.LoadAPIListOnLoad = Debug_LoadAPIListOnLoad.Checked;
			config.Debug.APIListPath = Debug_APIListPath.Text;
			config.Debug.AlertOnError = Debug_AlertOnError.Checked;

			plugin.ConfigurationChanged();
			return true;
		}

		private void Settings_Load( object sender, EventArgs e )
		{
			var config = ElectronicObserver.Utility.Configuration.Config;

			//[デバッグ]
			Debug_EnableDebugMenu.Checked = config.Debug.EnableDebugMenu;
			Debug_LoadAPIListOnLoad.Checked = config.Debug.LoadAPIListOnLoad;
			Debug_APIListPath.Text = config.Debug.APIListPath;
			Debug_AlertOnError.Checked = config.Debug.AlertOnError;

			Debug_EnableDebugMenu_CheckedChanged( null, null );
		}

		private void Debug_APIListPathSearch_Click( object sender, EventArgs e )
		{

			Debug_APIListPath.Text = PathHelper.ProcessOpenFileDialog( Debug_APIListPath.Text, APIListBrowser );

		}


		private void Debug_EnableDebugMenu_CheckedChanged( object sender, EventArgs e )
		{

			Debug_SealingPanel.Visible = Debug_EnableDebugMenu.Checked;
		}
	}
}
