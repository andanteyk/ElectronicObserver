using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window {

	public partial class FormHeadquarters : DockContent {

		public FormHeadquarters( FormMain parent ) {
			InitializeComponent();

			ImageList icons = ResourceManager.Instance.Icons;
			
			ShipCount.ImageList = icons;
			ShipCount.ImageIndex = (int)ResourceManager.IconContent.HQShip;
			EquipmentCount.ImageList = icons;
			EquipmentCount.ImageIndex = (int)ResourceManager.IconContent.HQEquipment;
			InstantRepair.ImageList = icons;
			InstantRepair.ImageIndex = (int)ResourceManager.IconContent.ItemInstantRepair;
			InstantConstruction.ImageList = icons;
			InstantConstruction.ImageIndex = (int)ResourceManager.IconContent.ItemInstantConstruction;
			DevelopmentMaterial.ImageList = icons;
			DevelopmentMaterial.ImageIndex = (int)ResourceManager.IconContent.ItemDevelopmentMaterial;
			FurnitureCoin.ImageList = icons;
			FurnitureCoin.ImageIndex = (int)ResourceManager.IconContent.ItemFurnitureCoin;
			Fuel.ImageList = icons;
			Fuel.ImageIndex = (int)ResourceManager.IconContent.ResourceFuel;
			Ammo.ImageList = icons;
			Ammo.ImageIndex = (int)ResourceManager.IconContent.ResourceAmmo;
			Steel.ImageList = icons;
			Steel.ImageIndex = (int)ResourceManager.IconContent.ResourceSteel;
			Bauxite.ImageList = icons;
			Bauxite.ImageIndex = (int)ResourceManager.IconContent.ResourceBauxite;
			
		}

		
		private void FormHeadquarters_Load( object sender, EventArgs e ) {

			KCDatabase Database = KCDatabase.Instance;

			Database.MaterialUpdated += ( DatabaseUpdatedEventArgs e1 ) => Invoke( new KCDatabase.DatabaseUpdatedEventHandler( Database_Updated ), e1 );
			Database.AdmiralUpdated += ( DatabaseUpdatedEventArgs e1 ) => Invoke( new KCDatabase.DatabaseUpdatedEventHandler( Database_Updated ), e1 );

			//こうしないとフォントがなぜかデフォルトにされる
			AdmiralName.Font = this.Font;
			AdmiralComment.Font = this.Font;

			FlowPanelMaster.Visible = false;

		}

		void Database_Updated( DatabaseUpdatedEventArgs e ) {

			KCDatabase db = KCDatabase.Instance;

			
			FlowPanelMaster.SuspendLayout();

			//Admiral
			AdmiralName.Text = db.Admiral.AdmiralName + " " + db.Admiral.GetRankString();
			AdmiralComment.Text = db.Admiral.Comment;

			//HQ Level
			HQLevel.Value = db.Admiral.Level;
			HQLevel.TextNext = "Exp:";				//fixme: 暫定的に現在値表示, いずれnext. にすること
			HQLevel.ValueNext = db.Admiral.Exp;

			//Fleet
			ShipCount.Text = string.Format( "{0}/{1}", db.Ships.Count, db.Admiral.MaxShipCount );
			EquipmentCount.Text = string.Format( "{0}/{1}", db.Equipments.Count, db.Admiral.MaxEquipmentCount );

			//UseItems
			InstantRepair.Text = db.Material.InstantRepair.ToString();
			InstantConstruction.Text = db.Material.InstantConstruction.ToString();
			DevelopmentMaterial.Text = db.Material.DevelopmentMaterial.ToString();
			FurnitureCoin.Text = db.Admiral.FurnitureCoin.ToString();

			//Resources
			Fuel.Text = db.Material.Fuel.ToString();
			Ammo.Text = db.Material.Ammo.ToString();
			Steel.Text = db.Material.Steel.ToString();
			Bauxite.Text = db.Material.Bauxite.ToString();

			FlowPanelMaster.ResumeLayout();
			if ( !FlowPanelMaster.Visible )
				FlowPanelMaster.Visible = true;
			AdmiralName.Refresh();
			AdmiralComment.Refresh();

		}



		protected override string GetPersistString() {
			return "HeadQuarters";
		}

	}

}
