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

				SortieMapData map = KCDatabase.Instance.SortieMap;


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
								if ( map.GetItemID == 4 ) {		//"※"　大方ドロップアイテム専用ID
									switch ( map.GetItemIDMetadata ) {	//どこかにマスターはないものでしょうか…
										case 1:
											materialname = "燃料"; break;
										case 2:
											materialname = "弾薬"; break;
										case 3:
											materialname = "鋼材"; break;
										case 4:
											materialname = "ボーキサイト"; break;
										case 5:
											materialname = "高速建造材"; break;
										case 6:
											materialname = "高速修復材"; break;
										case 7:
											materialname = "開発資材"; break;
										default:
											materialname = "不明"; break;
									}
								} else {
									materialname = KCDatabase.Instance.MasterUseItems[map.GetItemID].Name;
								}

								TextEventDetail.Text = materialname + " x " + map.GetItemAmount;
							}

							break;
						case 3:
							eventkind += "渦潮";
							break;
						case 4:
							eventkind += "通常戦闘";
							TextEventDetail.Text = "敵編成ID : " + map.EnemyFleetID;
							break;
						case 5:
							eventkind += "ボス戦闘";
							TextEventDetail.Text = "敵編成ID : " + map.EnemyFleetID;
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
