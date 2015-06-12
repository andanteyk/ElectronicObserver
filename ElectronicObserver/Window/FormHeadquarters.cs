﻿using ElectronicObserver.Data;
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
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Support;
using ElectronicObserver.Resource.Record;

namespace ElectronicObserver.Window {

	public partial class FormHeadquarters : DockContent {

		public FormHeadquarters( FormMain parent ) {
			InitializeComponent();



			ImageList icons = ResourceManager.Instance.Icons;

			ShipCount.ImageList = icons;
			ShipCount.ImageIndex = (int)ResourceManager.IconContent.HeadQuartersShip;
			EquipmentCount.ImageList = icons;
			EquipmentCount.ImageIndex = (int)ResourceManager.IconContent.HeadQuartersEquipment;
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


			ControlHelper.SetDoubleBuffered( FlowPanelMaster );
			ControlHelper.SetDoubleBuffered( FlowPanelAdmiral );
			ControlHelper.SetDoubleBuffered( FlowPanelFleet );
			ControlHelper.SetDoubleBuffered( FlowPanelUseItem );
			ControlHelper.SetDoubleBuffered( FlowPanelResource );


			ConfigurationChanged();

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormHeadQuarters] );

		}


		private void FormHeadquarters_Load( object sender, EventArgs e ) {

			APIObserver o = APIObserver.Instance;

			o.APIList["api_req_nyukyo/start"].RequestReceived += Updated;
			o.APIList["api_req_nyukyo/speedchange"].RequestReceived += Updated;
			o.APIList["api_req_kousyou/createship"].RequestReceived += Updated;
			o.APIList["api_req_kousyou/createship_speedchange"].RequestReceived += Updated;
			o.APIList["api_req_kousyou/destroyship"].RequestReceived += Updated;
			o.APIList["api_req_kousyou/destroyitem2"].RequestReceived += Updated;

			o.APIList["api_get_member/basic"].ResponseReceived += Updated;
			o.APIList["api_get_member/slot_item"].ResponseReceived += Updated;
			o.APIList["api_port/port"].ResponseReceived += Updated;
			o.APIList["api_get_member/ship2"].ResponseReceived += Updated;
			o.APIList["api_req_kousyou/getship"].ResponseReceived += Updated;
			o.APIList["api_req_hokyu/charge"].ResponseReceived += Updated;
			o.APIList["api_req_kousyou/destroyship"].ResponseReceived += Updated;
			o.APIList["api_req_kousyou/destroyitem2"].ResponseReceived += Updated;
			o.APIList["api_req_kaisou/powerup"].ResponseReceived += Updated;
			o.APIList["api_req_kousyou/createitem"].ResponseReceived += Updated;
			o.APIList["api_req_kousyou/remodel_slot"].ResponseReceived += Updated;
			o.APIList["api_get_member/material"].ResponseReceived += Updated;
			o.APIList["api_get_member/ship_deck"].ResponseReceived += Updated;


			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
			Utility.SystemEvents.UpdateTimerTick += SystemEvents_UpdateTimerTick;

			FlowPanelResource.SetFlowBreak( Ammo, true );

			FlowPanelMaster.Visible = false;

		}



		void ConfigurationChanged() {

			Font = FlowPanelMaster.Font = Utility.Configuration.Config.UI.MainFont;
			HQLevel.MainFont = Utility.Configuration.Config.UI.MainFont;
			HQLevel.SubFont = Utility.Configuration.Config.UI.SubFont;
		}

		void Updated( string apiname, dynamic data ) {

			KCDatabase db = KCDatabase.Instance;


			FlowPanelMaster.SuspendLayout();

			//Admiral
			FlowPanelAdmiral.SuspendLayout();
			AdmiralName.Text = string.Format( "{0} {1}", db.Admiral.AdmiralName, Constants.GetAdmiralRank( db.Admiral.Rank ) );
			AdmiralComment.Text = db.Admiral.Comment;
			FlowPanelAdmiral.ResumeLayout();

			//HQ Level
			HQLevel.Value = db.Admiral.Level;
			{
				StringBuilder tooltip = new StringBuilder();
				if ( db.Admiral.Level < ExpTable.AdmiralExp.Max( e => e.Key ) ) {
					HQLevel.TextNext = "next:";
					HQLevel.ValueNext = ExpTable.GetNextExpAdmiral( db.Admiral.Exp );
					tooltip.AppendFormat( "{0} / {1}\r\n", db.Admiral.Exp, ExpTable.AdmiralExp[db.Admiral.Level + 1].Total );
				} else {
					HQLevel.TextNext = "exp:";
					HQLevel.ValueNext = db.Admiral.Exp;
				}

				//戦果ツールチップ
				//fixme: もっとましな書き方はなかっただろうか
				{
					var res = RecordManager.Instance.Resource.GetRecordPrevious();
					if ( res != null ) {
						int diff = db.Admiral.Exp - res.HQExp;
						tooltip.AppendFormat( "今回: +{0} exp. / 戦果 {1:n2}\r\n", diff, diff * 7 / 10000.0 );
					}
				}
				{
					var res = RecordManager.Instance.Resource.GetRecordDay();
					if ( res != null ) {
						int diff = db.Admiral.Exp - res.HQExp;
						tooltip.AppendFormat( "今日: +{0} exp. / 戦果 {1:n2}\r\n", diff, diff * 7 / 10000.0 );
					}
				}
				{
					var res = RecordManager.Instance.Resource.GetRecordMonth();
					if ( res != null ) {
						int diff = db.Admiral.Exp - res.HQExp;
						tooltip.AppendFormat( "今月: +{0} exp. / 戦果 {1:n2}\r\n", diff, diff * 7 / 10000.0 );
					}
				}

				ToolTipInfo.SetToolTip( HQLevel, tooltip.ToString() );
			}

			//Fleet
			FlowPanelFleet.SuspendLayout();
			{

				ShipCount.Text = string.Format( "{0}/{1}", RealShipCount, db.Admiral.MaxShipCount );
				if ( RealShipCount > db.Admiral.MaxShipCount - 5 )
					ShipCount.BackColor = Color.LightCoral;
				else
					ShipCount.BackColor = Color.Transparent;

				EquipmentCount.Text = string.Format( "{0}/{1}", RealEquipmentCount, db.Admiral.MaxEquipmentCount );
				if ( RealEquipmentCount > db.Admiral.MaxEquipmentCount + 3 - 20 )
					EquipmentCount.BackColor = Color.LightCoral;
				else
					EquipmentCount.BackColor = Color.Transparent;
			}
			FlowPanelFleet.ResumeLayout();

			//UseItems
			FlowPanelUseItem.SuspendLayout();
			InstantRepair.Text = db.Material.InstantRepair.ToString();
			InstantConstruction.Text = db.Material.InstantConstruction.ToString();
			DevelopmentMaterial.Text = db.Material.DevelopmentMaterial.ToString();
			ModdingMaterial.Text = db.Material.ModdingMaterial.ToString();
			FurnitureCoin.Text = db.Admiral.FurnitureCoin.ToString();
			FlowPanelUseItem.ResumeLayout();


			//Resources
			FlowPanelResource.SuspendLayout();
			{
				Color overcolor = Color.Moccasin;

				var resday = RecordManager.Instance.Resource.GetRecord( DateTime.Now.AddHours( -5 ).Date.AddHours( 5 ) );
				var resweek = RecordManager.Instance.Resource.GetRecord( DateTime.Now.AddHours( -5 ).Date.AddDays( -( ( (int)DateTime.Now.AddHours( -5 ).DayOfWeek + 6 ) % 7 ) ).AddHours( 5 ) );	//月曜日起点
				var resmonth = RecordManager.Instance.Resource.GetRecord( new DateTime( DateTime.Now.Year, DateTime.Now.Month, 1 ).AddHours( 5 ) );
				

				Fuel.Text = db.Material.Fuel.ToString();
				Fuel.BackColor = db.Material.Fuel < db.Admiral.MaxResourceRegenerationAmount ? Color.Transparent : overcolor;
				if ( resday != null && resweek != null && resmonth != null )
					ToolTipInfo.SetToolTip( Fuel, string.Format( "今日: {0:+##;-##;±0}\n今週: {1:+##;-##;±0}\n今月: {2:+##;-##;±0}",
						db.Material.Fuel - resday.Fuel, db.Material.Fuel - resweek.Fuel, db.Material.Fuel - resmonth.Fuel ) );

				Ammo.Text = db.Material.Ammo.ToString();
				Ammo.BackColor = db.Material.Ammo < db.Admiral.MaxResourceRegenerationAmount ? Color.Transparent : overcolor;
				if ( resday != null && resweek != null && resmonth != null )
					ToolTipInfo.SetToolTip( Ammo, string.Format( "今日: {0:+##;-##;±0}\n今週: {1:+##;-##;±0}\n今月: {2:+##;-##;±0}",
						db.Material.Ammo - resday.Ammo, db.Material.Ammo - resweek.Ammo, db.Material.Ammo - resmonth.Ammo ) );

				Steel.Text = db.Material.Steel.ToString();
				Steel.BackColor = db.Material.Steel < db.Admiral.MaxResourceRegenerationAmount ? Color.Transparent : overcolor;
				if ( resday != null && resweek != null && resmonth != null )
					ToolTipInfo.SetToolTip( Steel, string.Format( "今日: {0:+##;-##;±0}\n今週: {1:+##;-##;±0}\n今月: {2:+##;-##;±0}",
						db.Material.Steel - resday.Steel, db.Material.Steel - resweek.Steel, db.Material.Steel - resmonth.Steel ) );

				Bauxite.Text = db.Material.Bauxite.ToString();
				Bauxite.BackColor = db.Material.Bauxite < db.Admiral.MaxResourceRegenerationAmount ? Color.Transparent : overcolor;
				if ( resday != null && resweek != null && resmonth != null )
					ToolTipInfo.SetToolTip( Bauxite, string.Format( "今日: {0:+##;-##;±0}\n今週: {1:+##;-##;±0}\n今月: {2:+##;-##;±0}",
						db.Material.Bauxite - resday.Bauxite, db.Material.Bauxite - resweek.Bauxite, db.Material.Bauxite - resmonth.Bauxite ) );

			}
			FlowPanelResource.ResumeLayout();

			FlowPanelMaster.ResumeLayout();
			if ( !FlowPanelMaster.Visible )
				FlowPanelMaster.Visible = true;
			AdmiralName.Refresh();
			AdmiralComment.Refresh();

		}


		void SystemEvents_UpdateTimerTick() {

			KCDatabase db = KCDatabase.Instance;

			if ( db.Ships.Count <= 0 ) return;

			if ( RealShipCount >= db.Admiral.MaxShipCount ) {
				ShipCount.BackColor = DateTime.Now.Second % 2 == 0 ? Color.LightCoral : Color.Transparent;
			}

			if ( RealEquipmentCount >= db.Admiral.MaxEquipmentCount ) {
				EquipmentCount.BackColor = DateTime.Now.Second % 2 == 0 ? Color.LightCoral : Color.Transparent;
			}
		}


		private void Resource_MouseClick( object sender, MouseEventArgs e ) {

			new Dialog.DialogResourceChart().Show( this );

		}


		private int RealShipCount {
			get {
				if ( KCDatabase.Instance.Battle != null )
					return KCDatabase.Instance.Ships.Count + KCDatabase.Instance.Battle.DroppedShipCount;

				return KCDatabase.Instance.Ships.Count;
			}
		}

		private int RealEquipmentCount {
			get {
				if ( KCDatabase.Instance.Battle != null )
					return KCDatabase.Instance.Equipments.Count + KCDatabase.Instance.Battle.DroppedEquipmentCount;

				return KCDatabase.Instance.Equipments.Count;
			}
		}


		protected override string GetPersistString() {
			return "HeadQuarters";
		}

	}

}
