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
using Codeplex.Data;
using System.IO;

namespace Headquarters
{
	public partial class Settings : PluginSettingControl
	{

		internal const string PLUGIN_SETTINGS = @"Settings\Headquarters\Settings.json";
		internal const string DEFAULT_SETTINGS = "{" +
			"\"AdmiralName\":{\"show\":true,\"break\":false}," +
			"\"AdmiralComment\":{\"show\":true,\"break\":false}," +
			"\"HQLevel\":{\"show\":true,\"break\":false}," +
			"\"ShipCount\":{\"show\":true,\"break\":false}," +
			"\"EquipmentCount\":{\"show\":true,\"break\":false}," +
			"\"InstantRepair\":{\"show\":true,\"break\":false}," +
			"\"InstantConstruction\":{\"show\":true,\"break\":false}," +
			"\"DevelopmentMaterial\":{\"show\":true,\"break\":false}," +
			"\"ModdingMaterial\":{\"show\":true,\"break\":false}," +
			"\"FurnitureCoin\":{\"show\":true,\"break\":false}," +
			"\"Resources\":{\"show\":true}" +
			"}";

		internal static dynamic settings;

		public void SaveSettings()
		{
			if ( !Directory.Exists( "Settings" ) )
				Directory.CreateDirectory( "Settings" );

			if ( !Directory.Exists( @"Settings\Headquarters" ) )
				Directory.CreateDirectory( @"Settings\Headquarters" );

			File.WriteAllText( PLUGIN_SETTINGS, settings.ToString() );
		}

		public Settings()
		{
			InitializeComponent();
		}


		public override bool Save()
		{
			var config = ElectronicObserver.Utility.Configuration.Config;

			config.FormHeadquarters.BlinkAtMaximum = FormHeadquarters_BlinkAtMaximum.Checked;

			if ( settings == null )
				settings = DynamicJson.Parse( DEFAULT_SETTINGS );

			settings.AdmiralName.show = AdmiralName_Show.Checked;
			settings.AdmiralName.@break = AdmiralName_Break.Checked;
			settings.HQLevel.show = HQLevel_Show.Checked;
			settings.HQLevel.@break = HQLevel_Break.Checked;
			settings.ShipCount.show = ShipCount_Show.Checked;
			settings.ShipCount.@break = ShipCount_Break.Checked;
			settings.EquipmentCount.show = EquipmentCount_Show.Checked;
			settings.EquipmentCount.@break = EquipmentCount_Break.Checked;
			settings.InstantRepair.show = InstantRepair_Show.Checked;
			settings.InstantRepair.@break = InstantRepair_Break.Checked;
			settings.InstantConstruction.show = InstantConstruction_Show.Checked;
			settings.InstantConstruction.@break = InstantConstruction_Break.Checked;
			settings.DevelopmentMaterial.show = DevelopmentMaterial_Show.Checked;
			settings.DevelopmentMaterial.@break = DevelopmentMaterial_Break.Checked;
			settings.ModdingMaterial.show = ModdingMaterial_Show.Checked;
			settings.ModdingMaterial.@break = ModdingMaterial_Break.Checked;
			settings.FurnitureCoin.show = FurnitureCoin_Show.Checked;
			settings.FurnitureCoin.@break = FurnitureCoin_Break.Checked;
			settings.Resources.show = Resources_Show.Checked;

			SaveSettings();

			return true;
		}

		private void Settings_Load( object sender, EventArgs e )
		{
			var config = ElectronicObserver.Utility.Configuration.Config;

			FormHeadquarters_BlinkAtMaximum.Checked = config.FormHeadquarters.BlinkAtMaximum;

			if ( settings == null )
				settings = DynamicJson.Parse( DEFAULT_SETTINGS );

			AdmiralName_Show.Checked = settings.AdmiralName.show;
			AdmiralName_Break.Checked = settings.AdmiralName.@break;
			HQLevel_Show.Checked = settings.HQLevel.show;
			HQLevel_Break.Checked = settings.HQLevel.@break;
			ShipCount_Show.Checked = settings.ShipCount.show;
			ShipCount_Break.Checked = settings.ShipCount.@break;
			EquipmentCount_Show.Checked = settings.EquipmentCount.show;
			EquipmentCount_Break.Checked = settings.EquipmentCount.@break;
			InstantRepair_Show.Checked = settings.InstantRepair.show;
			InstantRepair_Break.Checked = settings.InstantRepair.@break;
			InstantConstruction_Show.Checked = settings.InstantConstruction.show;
			InstantConstruction_Break.Checked = settings.InstantConstruction.@break;
			DevelopmentMaterial_Show.Checked = settings.DevelopmentMaterial.show;
			DevelopmentMaterial_Break.Checked = settings.DevelopmentMaterial.@break;
			ModdingMaterial_Show.Checked = settings.ModdingMaterial.show;
			ModdingMaterial_Break.Checked = settings.ModdingMaterial.@break;
			FurnitureCoin_Show.Checked = settings.FurnitureCoin.show;
			FurnitureCoin_Break.Checked = settings.FurnitureCoin.@break;
			Resources_Show.Checked = settings.Resources.show;
		}
	}
}
