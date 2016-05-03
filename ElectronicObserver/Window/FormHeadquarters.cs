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
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Support;
using ElectronicObserver.Resource.Record;
using System.IO;
using Codeplex.Data;

namespace ElectronicObserver.Window {

	public partial class FormHeadquarters : DockContent {

		private Form _parentForm;

		public FormHeadquarters( FormMain parent ) {
			InitializeComponent();

			_parentForm = parent;

			ImageCollection icons = ResourceManager.Instance.Icons;

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
			o.APIList["api_req_member/updatecomment"].RequestReceived += Updated;

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

			// custom flow break/visible
			CustomFlowSettings();

			FlowPanelMaster.Visible = false;

		}

		void CustomFlowSettings()
		{
			var settings = Headquarters.Settings.settings;

			if ( settings == null )
			{
				try
				{
					if ( File.Exists( Headquarters.Settings.PLUGIN_SETTINGS ) )
						settings = DynamicJson.Parse( File.ReadAllText( Headquarters.Settings.PLUGIN_SETTINGS ) );
					else
						settings = DynamicJson.Parse( Headquarters.Settings.DEFAULT_SETTINGS );
				}
				catch
				{
					settings = DynamicJson.Parse( Headquarters.Settings.DEFAULT_SETTINGS );
				}

				Headquarters.Settings.settings = settings;
            }

			if ( settings != null )
			{
				AdmiralName.Visible = settings.AdmiralName.show;
				//FlowPanelAdmiral.SetFlowBreak( AdmiralName, settings.AdmiralName.@break );

				AdmiralComment.Visible = settings.AdmiralComment.show;
				//FlowPanelMaster.SetFlowBreak( FlowPanelAdmiral, settings.AdmiralComment.@break );

				HQLevel.Visible = settings.HQLevel.show;
				//FlowPanelMaster.SetFlowBreak( HQLevel, settings.HQLevel.@break );

				ShipCount.Visible = settings.ShipCount.show;
				//FlowPanelFleet.SetFlowBreak( ShipCount, settings.ShipCount.@break );

				EquipmentCount.Visible = settings.EquipmentCount.show;
				//FlowPanelMaster.SetFlowBreak( FlowPanelFleet, settings.EquipmentCount.@break );

				InstantRepair.Visible = settings.InstantRepair.show;
				//FlowPanelUseItem.SetFlowBreak( InstantRepair, settings.InstantRepair.@break );

				InstantConstruction.Visible = settings.InstantConstruction.show;
				//FlowPanelUseItem.SetFlowBreak( InstantConstruction, settings.InstantConstruction.@break );

				DevelopmentMaterial.Visible = settings.DevelopmentMaterial.show;
				//FlowPanelUseItem.SetFlowBreak( DevelopmentMaterial, settings.DevelopmentMaterial.@break );

				ModdingMaterial.Visible = settings.ModdingMaterial.show;
				//FlowPanelUseItem.SetFlowBreak( ModdingMaterial, settings.ModdingMaterial.@break );

				FurnitureCoin.Visible = settings.FurnitureCoin.show;
				//FlowPanelMaster.SetFlowBreak( FlowPanelUseItem, settings.FurnitureCoin.@break );

				FlowPanelResource.Visible = settings.Resources.show;
			}
		}


		void ConfigurationChanged() {

			Font = FlowPanelMaster.Font = Utility.Configuration.Config.UI.MainFont;
			HQLevel.MainFont = Utility.Configuration.Config.UI.MainFont;
			HQLevel.SubFont = Utility.Configuration.Config.UI.SubFont;
			HQLevel.MainFontColor = Utility.Configuration.Config.UI.ForeColor;
			HQLevel.SubFontColor = Utility.Configuration.Config.UI.SubForeColor;

			// 点滅しない設定にしたときに消灯状態で固定されるのを防ぐ
			if ( !Utility.Configuration.Config.FormHeadquarters.BlinkAtMaximum ) {
				if ( ShipCount.Tag as bool? ?? false ) {
					ShipCount.BackColor = Utility.Configuration.Config.UI.FleetDamageColor.ColorData;
					ShipCount.ForeColor = Utility.Configuration.Config.UI.HighlightForeColor.ColorData;
				}

				if ( EquipmentCount.Tag as bool? ?? false ) {
					EquipmentCount.BackColor = Utility.Configuration.Config.UI.FleetDamageColor.ColorData;
					EquipmentCount.ForeColor = Utility.Configuration.Config.UI.HighlightForeColor.ColorData;
				}
			}

			CustomFlowSettings();
        }

		void Updated( string apiname, dynamic data ) {

			KCDatabase db = KCDatabase.Instance;


			if ( !db.Admiral.IsAvailable )
				return;


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
				if ( RealShipCount > db.Admiral.MaxShipCount - 5 ) {
					ShipCount.BackColor = Utility.Configuration.Config.UI.FleetDamageColor.ColorData;
					ShipCount.ForeColor = Utility.Configuration.Config.UI.HighlightForeColor.ColorData;
				} else {
					ShipCount.BackColor = Color.Transparent;
					ShipCount.ForeColor = Utility.Configuration.Config.UI.ForeColor.ColorData;
				}
				ShipCount.Tag = RealShipCount >= db.Admiral.MaxShipCount;

				EquipmentCount.Text = string.Format( "{0}/{1}", RealEquipmentCount, db.Admiral.MaxEquipmentCount );
				if ( RealEquipmentCount > db.Admiral.MaxEquipmentCount + 3 - 20 ) {
					EquipmentCount.BackColor = Utility.Configuration.Config.UI.FleetDamageColor.ColorData;
					EquipmentCount.ForeColor = Utility.Configuration.Config.UI.HighlightForeColor.ColorData;
				} else {
					EquipmentCount.BackColor = Color.Transparent;
					EquipmentCount.ForeColor = Utility.Configuration.Config.UI.ForeColor.ColorData;
				}
				EquipmentCount.Tag = RealEquipmentCount >= db.Admiral.MaxEquipmentCount;

			}
			FlowPanelFleet.ResumeLayout();



			var resday = RecordManager.Instance.Resource.GetRecord( DateTime.Now.AddHours( -5 ).Date.AddHours( 5 ) );
			var resweek = RecordManager.Instance.Resource.GetRecord( DateTime.Now.AddHours( -5 ).Date.AddDays( -( ( (int)DateTime.Now.AddHours( -5 ).DayOfWeek + 6 ) % 7 ) ).AddHours( 5 ) );	//月曜日起点
			var resmonth = RecordManager.Instance.Resource.GetRecord( new DateTime( DateTime.Now.Year, DateTime.Now.Month, 1 ).AddHours( 5 ) );


			//UseItems
			FlowPanelUseItem.SuspendLayout();

			InstantRepair.Text = db.Material.InstantRepair.ToString();
			ToolTipInfo.SetToolTip( InstantRepair, string.Format( "今日: {0:+##;-##;±0}\n今週: {1:+##;-##;±0}\n今月: {2:+##;-##;±0}",
					resday == null ? 0 : ( db.Material.InstantRepair - resday.InstantRepair ),
					resweek == null ? 0 : ( db.Material.InstantRepair - resweek.InstantRepair ),
					resmonth == null ? 0 : ( db.Material.InstantRepair - resmonth.InstantRepair ) ) );

			InstantConstruction.Text = db.Material.InstantConstruction.ToString();
			ToolTipInfo.SetToolTip( InstantConstruction, string.Format( "今日: {0:+##;-##;±0}\n今週: {1:+##;-##;±0}\n今月: {2:+##;-##;±0}",
					resday == null ? 0 : ( db.Material.InstantConstruction - resday.InstantConstruction ),
					resweek == null ? 0 : ( db.Material.InstantConstruction - resweek.InstantConstruction ),
					resmonth == null ? 0 : ( db.Material.InstantConstruction - resmonth.InstantConstruction ) ) );

			DevelopmentMaterial.Text = db.Material.DevelopmentMaterial.ToString();
			ToolTipInfo.SetToolTip( DevelopmentMaterial, string.Format( "今日: {0:+##;-##;±0}\n今週: {1:+##;-##;±0}\n今月: {2:+##;-##;±0}",
					resday == null ? 0 : ( db.Material.DevelopmentMaterial - resday.DevelopmentMaterial ),
					resweek == null ? 0 : ( db.Material.DevelopmentMaterial - resweek.DevelopmentMaterial ),
					resmonth == null ? 0 : ( db.Material.DevelopmentMaterial - resmonth.DevelopmentMaterial ) ) );

			ModdingMaterial.Text = db.Material.ModdingMaterial.ToString();
			ToolTipInfo.SetToolTip( ModdingMaterial, string.Format( "今日: {0:+##;-##;±0}\n今週: {1:+##;-##;±0}\n今月: {2:+##;-##;±0}",
					resday == null ? 0 : ( db.Material.ModdingMaterial - resday.ModdingMaterial ),
					resweek == null ? 0 : ( db.Material.ModdingMaterial - resweek.ModdingMaterial ),
					resmonth == null ? 0 : ( db.Material.ModdingMaterial - resmonth.ModdingMaterial ) ) );

			FurnitureCoin.Text = db.Admiral.FurnitureCoin.ToString();
			{
				int small = db.UseItems[10] != null ? db.UseItems[10].Count : 0;
				int medium = db.UseItems[11] != null ? db.UseItems[11].Count : 0;
				int large = db.UseItems[12] != null ? db.UseItems[12].Count : 0;

				ToolTipInfo.SetToolTip( FurnitureCoin,
						string.Format( "(小) x {0} ( +{1} )\r\n(中) x {2} ( +{3} )\r\n(大) x {4} ( +{5} )\r\n",
							small, small * 200,
							medium, medium * 400,
							large, large * 700 ) );
			}
			FlowPanelUseItem.ResumeLayout();


			//Resources
			FlowPanelResource.SuspendLayout();
			{
				Color overcolor = Color.Moccasin;
				Color fore = Utility.Configuration.Config.UI.ForeColor;
				Color back = Utility.Configuration.Config.UI.HighlightForeColor;

				Fuel.Text = db.Material.Fuel.ToString();
				Fuel.BackColor = db.Material.Fuel < db.Admiral.MaxResourceRegenerationAmount ? Color.Transparent : overcolor;
				Fuel.ForeColor = db.Material.Fuel < db.Admiral.MaxResourceRegenerationAmount ? fore : back;
				ToolTipInfo.SetToolTip( Fuel, string.Format( "今日: {0:+##;-##;±0}\n今週: {1:+##;-##;±0}\n今月: {2:+##;-##;±0}",
					resday == null ? 0 : ( db.Material.Fuel - resday.Fuel ),
					resweek == null ? 0 : ( db.Material.Fuel - resweek.Fuel ),
					resmonth == null ? 0 : ( db.Material.Fuel - resmonth.Fuel ) ) );

				Ammo.Text = db.Material.Ammo.ToString();
				Ammo.BackColor = db.Material.Ammo < db.Admiral.MaxResourceRegenerationAmount ? Color.Transparent : overcolor;
				Ammo.ForeColor = db.Material.Ammo < db.Admiral.MaxResourceRegenerationAmount ? fore : back;
				ToolTipInfo.SetToolTip( Ammo, string.Format( "今日: {0:+##;-##;±0}\n今週: {1:+##;-##;±0}\n今月: {2:+##;-##;±0}",
					resday == null ? 0 : ( db.Material.Ammo - resday.Ammo ),
					resweek == null ? 0 : ( db.Material.Ammo - resweek.Ammo ),
					resmonth == null ? 0 : ( db.Material.Ammo - resmonth.Ammo ) ) );

				Steel.Text = db.Material.Steel.ToString();
				Steel.BackColor = db.Material.Steel < db.Admiral.MaxResourceRegenerationAmount ? Color.Transparent : overcolor;
				Steel.ForeColor = db.Material.Steel < db.Admiral.MaxResourceRegenerationAmount ? fore : back;
				ToolTipInfo.SetToolTip( Steel, string.Format( "今日: {0:+##;-##;±0}\n今週: {1:+##;-##;±0}\n今月: {2:+##;-##;±0}",
					resday == null ? 0 : ( db.Material.Steel - resday.Steel ),
					resweek == null ? 0 : ( db.Material.Steel - resweek.Steel ),
					resmonth == null ? 0 : ( db.Material.Steel - resmonth.Steel ) ) );

				Bauxite.Text = db.Material.Bauxite.ToString();
				Bauxite.BackColor = db.Material.Bauxite < db.Admiral.MaxResourceRegenerationAmount ? Color.Transparent : overcolor;
				Bauxite.ForeColor = db.Material.Bauxite < db.Admiral.MaxResourceRegenerationAmount ? fore : back;
				ToolTipInfo.SetToolTip( Bauxite, string.Format( "今日: {0:+##;-##;±0}\n今週: {1:+##;-##;±0}\n今月: {2:+##;-##;±0}",
					resday == null ? 0 : ( db.Material.Bauxite - resday.Bauxite ),
					resweek == null ? 0 : ( db.Material.Bauxite - resweek.Bauxite ),
					resmonth == null ? 0 : ( db.Material.Bauxite - resmonth.Bauxite ) ) );

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

			if ( Utility.Configuration.Config.FormHeadquarters.BlinkAtMaximum ) {
				if ( ShipCount.Tag as bool? ?? false ) {
					if ( DateTime.Now.Second % 2 == 0 ) {
						ShipCount.BackColor = Utility.Configuration.Config.UI.FleetDamageColor.ColorData;
						ShipCount.ForeColor = Utility.Configuration.Config.UI.HighlightForeColor.ColorData;
					} else {
						ShipCount.BackColor = Color.Transparent;
						ShipCount.ForeColor = Utility.Configuration.Config.UI.ForeColor.ColorData;
					}
				}

				if ( EquipmentCount.Tag as bool? ?? false ) {
					if ( DateTime.Now.Second % 2 == 0 ) {
						EquipmentCount.BackColor = Utility.Configuration.Config.UI.FleetDamageColor.ColorData;
						EquipmentCount.ForeColor = Utility.Configuration.Config.UI.HighlightForeColor.ColorData;
					} else {
						EquipmentCount.BackColor = Color.Transparent;
						EquipmentCount.ForeColor = Utility.Configuration.Config.UI.ForeColor.ColorData;
					}
				}
			}
		}


		private void Resource_MouseClick( object sender, MouseEventArgs e ) {

			if ( e.Button == System.Windows.Forms.MouseButtons.Right )
				new Dialog.DialogResourceChart().Show( _parentForm );

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


		public override string GetPersistString()
		{
			return "HeadQuarters";
		}

	}

}
