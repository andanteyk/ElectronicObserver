using ElectronicObserver.Data;
using ElectronicObserver.Observer;
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

			APIObserver o = APIObserver.Instance;

			APIReceivedEventHandler rec = ( string apiname ) => Invoke( new APIReceivedEventHandler( HeadquartersUpdated ), apiname );

			o.RequestList["api_req_nyukyo/start"].RequestReceived += rec;
			o.RequestList["api_req_kousyou/createship"].RequestReceived += rec;
			o.RequestList["api_req_kousyou/createship_speedchange"].RequestReceived += rec;
			o.RequestList["api_req_kousyou/destroyship"].RequestReceived += rec;
			o.RequestList["api_req_kousyou/destroyitem2"].RequestReceived += rec;

			o.ResponseList["api_get_member/basic"].ResponseReceived += rec;
			o.ResponseList["api_get_member/slot_item"].ResponseReceived += rec;
			o.ResponseList["api_port/port"].ResponseReceived += rec;
			o.ResponseList["api_get_member/ship2"].ResponseReceived += rec;
			o.ResponseList["api_req_kousyou/getship"].ResponseReceived += rec;
			o.ResponseList["api_req_hokyu/charge"].ResponseReceived += rec;
			o.ResponseList["api_req_kousyou/destroyship"].ResponseReceived += rec;
			o.ResponseList["api_req_kousyou/destroyitem2"].ResponseReceived += rec;


			//こうしないとフォントがなぜかデフォルトにされる
			Font = new Font( "Meiryo UI", 12, FontStyle.Regular, GraphicsUnit.Pixel );
			AdmiralName.Font = this.Font;
			AdmiralComment.Font = this.Font;

			FlowPanelMaster.Visible = false;

		}

		void HeadquartersUpdated( string apiname ) {

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
