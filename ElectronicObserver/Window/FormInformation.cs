using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
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

	public partial class FormInformation : DockContent {


		public FormInformation( FormMain parent ) {
			InitializeComponent();

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.HQInformation] );
		}


		private void FormInformation_Load( object sender, EventArgs e ) {

			APIObserver o = APIObserver.Instance;

			APIReceivedEventHandler rec = ( string apiname, dynamic data ) => Invoke( new APIReceivedEventHandler( Updated ), apiname, data );

			o.APIList["api_port/port"].ResponseReceived += rec;
			o.APIList["api_req_member/get_practice_enemyinfo"].ResponseReceived += rec;
			o.APIList["api_get_member/picture_book"].ResponseReceived += rec;
			o.APIList["api_req_kousyou/createitem"].ResponseReceived += rec;
			o.APIList["api_get_member/mapinfo"].ResponseReceived += rec;
		}


		void Updated( string apiname, dynamic data ) {

			//fixme: あとでメソッドを分離させる

			switch ( apiname ) {

				case "api_port/port":
					TextInformation.Text = "";		//とりあえずクリア
					break;

				case "api_req_member/get_practice_enemyinfo": {
						StringBuilder sb = new StringBuilder();
						sb.AppendLine( "[演習情報]" );
						sb.AppendLine( "敵艦隊名 : " + data.api_deckname ); 
					
						{
							int ship1lv = (int)data.api_deck.api_ships[0].api_id != -1 ? (int)data.api_deck.api_ships[0].api_level : 1;
							int ship2lv = (int)data.api_deck.api_ships[1].api_id != -1 ? (int)data.api_deck.api_ships[1].api_level : 1;

							double expbase = ExpTable.ShipExp[ship1lv].Total / 100.0 + ExpTable.ShipExp[ship2lv].Total / 300.0;
							if ( expbase >= 500.0 )
								expbase = 500.0 + Math.Sqrt( expbase - 500.0 );

							sb.AppendLine( "獲得経験値 : " + (int)expbase );
							sb.AppendLine( "S勝利 : " + (int)( expbase * 1.2 ) );

						}

						TextInformation.Text = sb.ToString();

					} break;


				case "api_get_member/picture_book": {
						StringBuilder sb = new StringBuilder();
						sb.AppendLine( "[中破絵未回収]" );

						foreach ( dynamic elem in data.api_list ) {

							if ( !elem.IsDefined( "api_yomi" ) )
								break;		//ないほうは装備

							dynamic[] state = elem.api_state;
							for ( int i = 0; i < state.Length; i++ ) {
								if ( (int)state[i][1] == 0 ) {
									sb.AppendLine( KCDatabase.Instance.MasterShips[(int)elem.api_table_id[i]].Name );
								}
							}
						}

						TextInformation.Text = sb.ToString();
					} break;


				case "api_req_kousyou/createitem": {
					if ( (int)data.api_create_flag == 0 ) {

						StringBuilder sb = new StringBuilder();
						sb.AppendLine( "[開発失敗]" );
						sb.AppendLine( data.api_fdata );

						EquipmentDataMaster eqm = KCDatabase.Instance.MasterEquipments[int.Parse( ( (string)data.api_fdata ).Split( ",".ToCharArray() )[1] )];
						if ( eqm != null )
							sb.AppendLine( eqm.Name );


						TextInformation.Text = sb.ToString();

					} else
						TextInformation.Text = "";

					} break;


				case "api_get_member/mapinfo": {

					StringBuilder sb = new StringBuilder();
					sb.AppendLine( "[海域ゲージ]" );

					foreach ( dynamic elem in data ) {

						int mapID = (int)elem.api_id;
						MapInfoData map = KCDatabase.Instance.MapInfo[mapID];

						if ( map != null ) {
							if ( map.RequiredDefeatedCount != -1 && elem.api_defeat_count() ) {

								sb.AppendFormat( "{0}-{1} : 撃破 {2}/{3} 回\r\n", map.MapAreaID, map.MapInfoID, (int)elem.api_defeat_count, map.RequiredDefeatedCount );

							} else if ( elem.api_eventmap() ) {

								sb.AppendFormat( "{0}-{1} : HP {2}/{3}\r\n", map.MapAreaID, map.MapInfoID, (int)elem.api_eventmap.api_now_maphp, (int)elem.api_eventmap.api_max_maphp );

							}
						}
					}

					TextInformation.Text = sb.ToString();

					} break;
			}

		}



		protected override string GetPersistString() {
			return "Information";
		}
	
	}

}
