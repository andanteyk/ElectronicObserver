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
			ConditionLittleTired,
			ConditionTired,
			ConditionVeryTired,
		}



		private ResourceManager() {

			_icons = new ImageList();
			_icons.ColorDepth = ColorDepth.Depth32Bit;
			_icons.ImageSize = new Size( 16, 16 );

		}


		public void Load() {
			const string masterpath = @"assets\";

			Icons.Images.Add( "Fuel", LoadImage( masterpath + @"fuel.png" ) );
			Icons.Images.Add( "Ammo", LoadImage( masterpath + @"ammo.png" ) );
			Icons.Images.Add( "Steel", LoadImage( masterpath + @"steel.png" ) );
			Icons.Images.Add( "Bauxite", LoadImage( masterpath + @"bauxite.png" ) );
			Icons.Images.Add( "Cond_Sparkle", LoadImage( masterpath + @"cond_sparkle.png" ) );
			Icons.Images.Add( "Cond_LittleTired", LoadImage( masterpath + @"cond_littletired.png" ) );
			Icons.Images.Add( "Cond_Tired", LoadImage( masterpath + @"cond_tired.png" ) );
			Icons.Images.Add( "Cond_VeryTired", LoadImage( masterpath + @"cond_verytired.png" ) );

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

		public static int GetIconIndex( IconContent icon ) {
			return (int)icon;
		}

	}


}
