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

		private ImageList _icons;
		public ImageList Icons {
			get { return _icons; }
			set { _icons = value; }
		}
		
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
			ItemFurnitureCoin,
			HQShip,
			HQEquipment,
		}



		private ResourceManager() {

			_icons = new ImageList();
			_icons.ColorDepth = ColorDepth.Depth32Bit;
			_icons.ImageSize = new Size( 16, 16 );

		}


		public void Load() {
			const string masterpath = @"assets\";

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
			Icons.Images.Add( "Item_FurnitureCoin", LoadImage( masterpath + @"Item\furnitureCoin.png" ) );
			Icons.Images.Add( "HQ_Ship", LoadImage( masterpath + @"Ship.png" ) );
			Icons.Images.Add( "HQ_Equipment", LoadImage( masterpath + @"Equipment.png" ) );

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
