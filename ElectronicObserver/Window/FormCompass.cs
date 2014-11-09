using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Resource.SaveData;
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

			o.APIList["api_port/port"].ResponseReceived += rec;
			o.APIList["api_req_map/start"].ResponseReceived += rec;
			o.APIList["api_req_map/next"].ResponseReceived += rec;
			
		}


		void Updated( string apiname, dynamic data ) {

			if ( apiname == "api_port/port" ) {

				BasePanel.Visible = false;

			} else {

				CompassData compass = KCDatabase.Instance.Battle.Compass;


				BasePanel.SuspendLayout();

				TextMapArea.Text = "出撃海域 : " + compass.MapAreaID + "-" + compass.MapInfoID;
				TextDestination.Text = "次のセル : " + compass.Destination + ( compass.IsEndPoint ? " (終点)" : "" );
				
				{
					string eventkind = "";
					switch ( compass.EventID ) {
						case 0:
							eventkind += "初期位置";
							TextEventDetail.Text = "どうしてこうなった";
							break;
						case 2:
							eventkind += "資源";
							{
								string materialname;
								if ( compass.GetItemID == 4 ) {		//"※"　大方資源専用ID

									materialname = MaterialData.GetMaterialName( compass.GetItemIDMetadata );
								
								} else {
									materialname = KCDatabase.Instance.MasterUseItems[compass.GetItemID].Name;
								}

								TextEventDetail.Text = materialname + " x " + compass.GetItemAmount;
							}

							break;
						case 3:
							eventkind += "渦潮";
							{
								string materialname = MaterialData.GetMaterialName( compass.WhirlpoolItemID );

								//fixme:第一艦隊以外の艦隊が出撃していた場合誤った値を返す
								int materialmax = KCDatabase.Instance.Fleet.Fleets[1].FleetMember.Max( n => 
								{
									if ( n != -1 )
										if ( compass.WhirlpoolItemID == 1 )
											return KCDatabase.Instance.Ships[n].MasterShip.Fuel;
										else if ( compass.WhirlpoolItemID == 2 )
											return KCDatabase.Instance.Ships[n].MasterShip.Ammo;
										else return 0;
									else return 0;
								} );

								int percent = compass.WhirlpoolItemAmount * 100 / Math.Max( materialmax, 1 );

								TextEventDetail.Text = materialname + " x " + compass.WhirlpoolItemAmount + " (" + percent + "%)";
							}
							break;
						case 4:
							eventkind += "通常戦闘";
							TextEventDetail.Text = GetEnemyFleetInformation( compass.EnemyFleetID );
							break;
						case 5:
							eventkind += "ボス戦闘";
							TextEventDetail.Text = GetEnemyFleetInformation( compass.EnemyFleetID );
							break;
						case 6:
							eventkind += "気のせいだった";
							TextEventDetail.Text = "";
							break;
						case 7:
							eventkind += "機動部隊航空戦";
							TextEventDetail.Text = GetEnemyFleetInformation( compass.EnemyFleetID );
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


		//for debug
		private string GetEnemyFleetInformation( int fleetID ) {

			StringBuilder sb = new StringBuilder();
			var efleet = RecordManager.Instance.EnemyFleet;

			sb.AppendFormat( "敵艦隊ID : {0}\r\n", fleetID );


			if ( !efleet.Record.ContainsKey( fleetID ) ) {

				sb.AppendLine( "(敵艦隊情報不明)" );

			} else {

				var fdata = efleet[fleetID];

				if ( fdata.FleetName != null )
					sb.Append( fdata.FleetName + " - " );

				switch ( fdata.Formation ) {
					case 1:
						sb.AppendLine( "単縦陣" ); break;
					case 2:
						sb.AppendLine( "複縦陣" ); break;
					case 3:
						sb.AppendLine( "輪形陣" ); break;
					case 4:
						sb.AppendLine( "梯形陣" ); break;
					case 5:
						sb.AppendLine( "単横陣" ); break;
					default:
						sb.AppendLine( "未定義" ); break;
				}


				int[] fmembers = fdata.FleetMember;

				for ( int i = 0; i < fmembers.Length; i++ ) {
					if ( fmembers[i] == -1 ) continue;

					ShipDataMaster ship = KCDatabase.Instance.MasterShips[fmembers[i]];
					sb.Append( ship.Name );
					if ( ship.NameReading != null &&
						 ship.NameReading != "" &&
						 ship.NameReading != "-" ) {
						sb.AppendFormat( " {0}", ship.NameReading );
					}
					sb.AppendLine();
				}
			}

			return sb.ToString();

		}
 


		protected override string GetPersistString() {
			return "Compass";
		}

	}

}
