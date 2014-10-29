using ElectronicObserver.Data;
using ElectronicObserver.Observer;
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

	public partial class FormCompass : DockContent {

		public FormCompass( FormMain parent ) {
			InitializeComponent();

			BasePanel.SetFlowBreak( TextMapArea, true );
			BasePanel.SetFlowBreak( TextDestination, true );
			BasePanel.SetFlowBreak( TextEventDetail, true );

		}


		private void FormCompass_Load( object sender, EventArgs e ) {

			BasePanel.Visible = false;


			APIObserver o = APIObserver.Instance;

			APIReceivedEventHandler rec = ( string apiname, dynamic data ) => Invoke( new APIReceivedEventHandler( Updated ), apiname, data );

			o.ResponseList["api_port/port"].ResponseReceived += rec;
			o.ResponseList["api_req_map/start"].ResponseReceived += rec;
			o.ResponseList["api_req_map/next"].ResponseReceived += rec;
			
		}


		void Updated( string apiname, dynamic data ) {

			if ( apiname == "api_port/port" ) {

				BasePanel.Visible = false;

			} else {

				CompassData map = KCDatabase.Instance.Compass;


				BasePanel.SuspendLayout();

				TextMapArea.Text = "出撃海域 : " + map.MapAreaID + "-" + map.MapInfoID;
				TextDestination.Text = "次のセル : " + map.Destination + ( map.IsEndPoint ? " (終点)" : "" );
				
				{
					string eventkind = "";
					switch ( map.EventID ) {
						case 0:
							eventkind += "初期位置";
							TextEventDetail.Text = "";
							break;
						case 2:
							eventkind += "資源";
							{
								string materialname;
								if ( map.GetItemID == 4 ) {		//"※"　大方資源専用ID

									materialname = MaterialData.GetMaterialName( map.GetItemIDMetadata );
								
								} else {
									materialname = KCDatabase.Instance.MasterUseItems[map.GetItemID].Name;
								}

								TextEventDetail.Text = materialname + " x " + map.GetItemAmount;
							}

							break;
						case 3:
							eventkind += "渦潮";
							{
								string materialname = MaterialData.GetMaterialName( map.WhirlpoolItemID );

								//fixme:第一艦隊以外の艦隊が出撃していた場合誤った値を返す
								int materialmax = KCDatabase.Instance.Fleet.Fleets[1].FleetMember.Max( n => 
								{
									if ( n != -1 )
										if ( map.WhirlpoolItemID == 1 )
											return KCDatabase.Instance.Ships[n].MasterShip.Fuel;
										else if ( map.WhirlpoolItemID == 2 )
											return KCDatabase.Instance.Ships[n].MasterShip.Ammo;
										else return 0;
									else return 0;
								} );

								int percent = map.WhirlpoolItemAmount * 100 / materialmax;

								TextEventDetail.Text = materialname + " x " + map.WhirlpoolItemAmount + " (" + percent + "%)";
							}
							break;
						case 4:
							eventkind += "通常戦闘";
							TextEventDetail.Text = "敵編成ID : " + map.EnemyFleetID;
							break;
						case 5:
							eventkind += "ボス戦闘";
							TextEventDetail.Text = "敵編成ID : " + map.EnemyFleetID;
							break;
						case 6:
							eventkind += "気のせいだった";
							TextEventDetail.Text = "";
							break;
						case 7:
							eventkind += "機動部隊航空戦";
							TextEventDetail.Text = "敵編成ID : " + map.EnemyFleetID;
							break;
						default:
							eventkind += "不明";
							TextEventDetail.Text = "";
							break;
					}
					TextEventKind.Text = eventkind;
				}

				BasePanel.ResumeLayout();

				BasePanel.Visible = true;
			}


		}


		protected override string GetPersistString() {
			return "Compass";
		}

	}

}
