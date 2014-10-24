using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Resource {


	public sealed class ResourceManager {


		#region Singleton

		private static readonly ResourceManager instance = new ResourceManager();

		public static ResourceManager Instance {
			get { return instance; }
		}

		#endregion


		#region Properties

		public ImageList Icons { get; private set; }

		public ImageList Equipments { get; private set; }

		#endregion


		public enum IconContent {
			Nothing = -1,
			ResourceFuel,
			ResourceAmmo,
			ResourceSteel,
			ResourceBauxite,
			ConditionSparkle,
			ConditionNormal,
			ConditionLittleTired,
			ConditionTired,
			ConditionVeryTired,
			ItemInstantRepair,
			ItemInstantConstruction,
			ItemDevelopmentMaterial,
			ItemModdingMaterial,
			ItemFurnitureCoin,
			HQShip,
			HQEquipment,
		}

		public enum EquipmentContent {
			Nothing,			//0
			MainGunS,
			MainGunM,
			MainGunL,
			SecondaryGun,
			Torpedo,
			CarrierBasedFighter,
			CarrierBasedBomber,
			CarrierBasedTorpedo,
			CarrierBasedRecon,
			Seaplane,
			Radar,
			AAShell,
			APShell,
			DamageControl,
			AAGun,
			HighAngleGun,
			DepthCharge,
			Sonar,
			Engine,
			LandingCraft,
			Autogyro,
			ASPatrol,
			Bulge,
			Searchlight,
			DrumCanister,
			RepairFacility,
			Flare,
			CommandFacility,
			MaintenanceTeam,
			Locked,
			Unknown,
		}


		private ResourceManager() {

			Icons = new ImageList();
			Icons.ColorDepth = ColorDepth.Depth32Bit;
			Icons.ImageSize = new Size( 16, 16 );

			Equipments = new ImageList();
			Equipments.ColorDepth = ColorDepth.Depth32Bit;
			Equipments.ImageSize = new Size( 16, 16 );

		}


		public void Load() {
			const string masterpath = @"assets\";

			#region Icons
			Icons.Images.Add( "Fuel", LoadImage( masterpath + @"Resource\fuel.png" ) );
			Icons.Images.Add( "Ammo", LoadImage( masterpath + @"Resource\ammo.png" ) );
			Icons.Images.Add( "Steel", LoadImage( masterpath + @"Resource\steel.png" ) );
			Icons.Images.Add( "Bauxite", LoadImage( masterpath + @"Resource\bauxite.png" ) );
			Icons.Images.Add( "Cond_Sparkle", LoadImage( masterpath + @"Condition\sparkle.png" ) );
			Icons.Images.Add( "Cond_Normal", LoadImage( masterpath + @"Condition\normal.png" ) );
			Icons.Images.Add( "Cond_LittleTired", LoadImage( masterpath + @"Condition\littletired.png" ) );
			Icons.Images.Add( "Cond_Tired", LoadImage( masterpath + @"Condition\tired.png" ) );
			Icons.Images.Add( "Cond_VeryTired", LoadImage( masterpath + @"Condition\verytired.png" ) );
			Icons.Images.Add( "Item_InstantRepair", LoadImage( masterpath + @"Item\instantRepair.png" ) );
			Icons.Images.Add( "Item_InstantConstruction", LoadImage( masterpath + @"Item\instantConstruction.png" ) );
			Icons.Images.Add( "Item_DevelopmentMaterial", LoadImage( masterpath + @"Item\developmentMaterial.png" ) );
			Icons.Images.Add( "Item_ModdingMaterial", LoadImage( masterpath + @"Item\moddingMaterial.png" ) );
			Icons.Images.Add( "Item_FurnitureCoin", LoadImage( masterpath + @"Item\furnitureCoin.png" ) );
			Icons.Images.Add( "HQ_Ship", LoadImage( masterpath + @"Ship.png" ) );
			Icons.Images.Add( "HQ_Equipment", LoadImage( masterpath + @"Equipment.png" ) );
			#endregion

			#region Equipments
			Equipments.Images.Add( "Nothing", LoadImage( masterpath + @"Equipment\nothing.png" ) );
			Equipments.Images.Add( "MainGunS", LoadImage( masterpath + @"Equipment\maingunS.png" ) );
			Equipments.Images.Add( "MainGunM", LoadImage( masterpath + @"Equipment\maingunM.png" ) );
			Equipments.Images.Add( "MainGunL", LoadImage( masterpath + @"Equipment\maingunL.png" ) );
			Equipments.Images.Add( "SecondaryGun", LoadImage( masterpath + @"Equipment\secondarygun.png" ) );
			Equipments.Images.Add( "Torpedo", LoadImage( masterpath + @"Equipment\torpedo.png" ) );
			Equipments.Images.Add( "CarrierBasedFighter", LoadImage( masterpath + @"Equipment\aircraft_fighter.png" ) );
			Equipments.Images.Add( "CarrierBasedBomber", LoadImage( masterpath + @"Equipment\aircraft_bomber.png" ) );
			Equipments.Images.Add( "CarrierBasedTorpedo", LoadImage( masterpath + @"Equipment\aircraft_torpedo.png" ) );
			Equipments.Images.Add( "CarrierBasedRecon", LoadImage( masterpath + @"Equipment\aircraft_recon.png" ) );
			Equipments.Images.Add( "Seaplane", LoadImage( masterpath + @"Equipment\seaplane.png" ) );
			Equipments.Images.Add( "RADAR", LoadImage( masterpath + @"Equipment\radar.png" ) );
			Equipments.Images.Add( "AAShell", LoadImage( masterpath + @"Equipment\aashell.png" ) );
			Equipments.Images.Add( "APShell", LoadImage( masterpath + @"Equipment\apshell.png" ) );
			Equipments.Images.Add( "DamageControl", LoadImage( masterpath + @"Equipment\damagecontrol.png" ) );
			Equipments.Images.Add( "AAGun", LoadImage( masterpath + @"Equipment\aagun.png" ) );
			Equipments.Images.Add( "HighAngleGun", LoadImage( masterpath + @"Equipment\highanglegun.png" ) );
			Equipments.Images.Add( "DepthCharge", LoadImage( masterpath + @"Equipment\depthcharge.png" ) );
			Equipments.Images.Add( "SONAR", LoadImage( masterpath + @"Equipment\sonar.png" ) );
			Equipments.Images.Add( "Engine", LoadImage( masterpath + @"Equipment\engine.png" ) );
			Equipments.Images.Add( "LandingCraft", LoadImage( masterpath + @"Equipment\landingcraft.png" ) );
			Equipments.Images.Add( "Autogyro", LoadImage( masterpath + @"Equipment\autogyro.png" ) );
			Equipments.Images.Add( "ASPatrol", LoadImage( masterpath + @"Equipment\aspatrol.png" ) );
			Equipments.Images.Add( "Bulge", LoadImage( masterpath + @"Equipment\bulge.png" ) );
			Equipments.Images.Add( "Searchlight", LoadImage( masterpath + @"Equipment\searchlight.png" ) );
			Equipments.Images.Add( "DrumCanister", LoadImage( masterpath + @"Equipment\drumcanister.png" ) );
			Equipments.Images.Add( "RepairFacility", LoadImage( masterpath + @"Equipment\repairfacility.png" ) );
			Equipments.Images.Add( "Flare", LoadImage( masterpath + @"Equipment\flare.png" ) );
			Equipments.Images.Add( "CommandFacility", LoadImage( masterpath + @"Equipment\commandfacility.png" ) );
			Equipments.Images.Add( "MaintenanceTeam", LoadImage( masterpath + @"Equipment\maintenanceteam.png" ) );
			Equipments.Images.Add( "Locked", LoadImage( masterpath + @"Equipment\locked.png" ) );
			Equipments.Images.Add( "Unknown", LoadImage( masterpath + @"Equipment\unknown.png" ) );
			
			#endregion

		}



		private static Image LoadImage( string path ) {
			try {

				using ( FileStream fs = new FileStream( path, FileMode.Open, FileAccess.Read ) ) {
					return Image.FromStream( fs );
				}

			} catch ( Exception e ) {

				System.Diagnostics.Debug.WriteLine( e.Message );
			}

			return null;
		}

		

	}


}
