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
using System.IO;
using Codeplex.Data;

namespace Overview
{
	public partial class Settings : PluginSettingControl
	{

		internal const string PLUGIN_SETTINGS = @"Settings\Information\Settings.json";
		internal const string DEFAULT_SETTINGS = "{\"ShowFailedDevelopment\":true}";

		internal static dynamic settings;

		public void SaveSettings()
		{
			if ( !Directory.Exists( "Settings" ) )
				Directory.CreateDirectory( "Settings" );

			if ( !Directory.Exists( @"Settings\Information" ) )
				Directory.CreateDirectory( @"Settings\Information" );

			File.WriteAllText( PLUGIN_SETTINGS, settings.ToString() );
		}


		public Settings()
		{
			InitializeComponent();
		}


		public override bool Save()
		{
			if ( settings == null )
				settings = DynamicJson.Parse( DEFAULT_SETTINGS );

			settings.ShowFailedDevelopment = FormInformation_ShowFailedDevelopment.Checked;

			SaveSettings();

			return true;
		}

		private void Settings_Load( object sender, EventArgs e )
		{
			if ( settings == null )
				settings = DynamicJson.Parse( DEFAULT_SETTINGS );

			FormInformation_ShowFailedDevelopment.Checked = settings.ShowFailedDevelopment;
		}
	}
}
