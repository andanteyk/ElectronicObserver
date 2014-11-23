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
			ModdingMaterial.ImageList = icons;
			ModdingMaterial.ImageIndex = (int)ResourceManager.IconContent.ItemModdingMaterial;
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

			APIReceivedEventHandler rec = ( string apiname, dynamic data ) => Invoke( new APIReceivedEventHandler( Updated ), apiname, data );

			o.APIList["api_req_nyukyo/start"].RequestReceived += rec;
			o.APIList["api_req_nyukyo/speedchange"].RequestReceived += rec;
			o.APIList["api_req_kousyou/createship"].RequestReceived += rec;
			o.APIList["api_req_kousyou/createship_speedchange"].RequestReceived += rec;
			o.APIList["api_req_kousyou/destroyship"].RequestReceived += rec;
			o.APIList["api_req_kousyou/destroyitem2"].RequestReceived += rec;

			o.APIList["api_get_member/basic"].ResponseReceived += rec;
			o.APIList["api_get_member/slot_item"].ResponseReceived += rec;
			o.APIList["api_port/port"].ResponseReceived += rec;
			o.APIList["api_get_member/ship2"].ResponseReceived += rec;
			o.APIList["api_req_kousyou/getship"].ResponseReceived += rec;
			o.APIList["api_req_hokyu/charge"].ResponseReceived += rec;
			o.APIList["api_req_kousyou/destroyship"].ResponseReceived += rec;
			o.APIList["api_req_kousyou/destroyitem2"].ResponseReceived += rec;
			o.APIList["api_req_kaisou/powerup"].ResponseReceived += rec;
			o.APIList["api_req_kousyou/createitem"].ResponseReceived += rec;


			//こうしないとフォントがなぜかデフォルトにされる
			Font = new Font( "Meiryo UI", 12, FontStyle.Regular, GraphicsUnit.Pixel );
			AdmiralName.Font = this.Font;
			AdmiralComment.Font = this.Font;

			FlowPanelResource.SetFlowBreak( Ammo, true );

			FlowPanelMaster.Visible = false;

		}

		void Updated( string apiname, dynamic data ) {

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
			if ( db.Ships.Count > db.Admiral.MaxShipCount - 5 )
				ShipCount.BackColor = Color.LightCoral;
			else
				ShipCount.BackColor = Color.Transparent;

			EquipmentCount.Text = string.Format( "{0}/{1}", db.Equipments.Count, db.Admiral.MaxEquipmentCount );
			if ( db.Equipments.Count > db.Admiral.MaxEquipmentCount - 20 )
				EquipmentCount.BackColor = Color.LightCoral;
			else
				EquipmentCount.BackColor = Color.Transparent;
			
			//UseItems
			InstantRepair.Text = db.Material.InstantRepair.ToString();
			InstantConstruction.Text = db.Material.InstantConstruction.ToString();
			DevelopmentMaterial.Text = db.Material.DevelopmentMaterial.ToString();
			ModdingMaterial.Text = db.Material.ModdingMaterial.ToString();
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
