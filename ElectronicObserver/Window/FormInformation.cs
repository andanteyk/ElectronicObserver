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

		private int _ignorePort;

		public FormInformation( FormMain parent ) {
			InitializeComponent();

			_ignorePort = 0;

			ConfigurationChanged();

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormInformation] );
		}


		private void FormInformation_Load( object sender, EventArgs e ) {

			APIObserver o = APIObserver.Instance;

			o.APIList["api_port/port"].ResponseReceived += Updated;
			o.APIList["api_req_member/get_practice_enemyinfo"].ResponseReceived += Updated;
			o.APIList["api_get_member/picture_book"].ResponseReceived += Updated;
			o.APIList["api_req_kousyou/createitem"].ResponseReceived += Updated;
			o.APIList["api_get_member/mapinfo"].ResponseReceived += Updated;
			o.APIList["api_req_mission/result"].ResponseReceived += Updated;
			o.APIList["api_req_ranking/getlist"].ResponseReceived += Updated;

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		}


		void ConfigurationChanged() {

			Font = TextInformation.Font = Utility.Configuration.Config.UI.MainFont;

			TextInformation.BackColor = Utility.Configuration.Config.UI.BackColor;
			TextInformation.ForeColor = Utility.Configuration.Config.UI.ForeColor;
		}


		void Updated( string apiname, dynamic data ) {

			switch ( apiname ) {

				case "api_port/port":
					if ( _ignorePort > 0 )
						_ignorePort--;
					else
						TextInformation.Text = "";		//とりあえずクリア
					break;

				case "api_req_member/get_practice_enemyinfo":
					TextInformation.Text = GetPracticeEnemyInfo( data );
					break;

				case "api_get_member/picture_book":
					TextInformation.Text = GetAlbumInfo( data );
					break;

				case "api_req_kousyou/createitem":
					TextInformation.Text = GetCreateItemInfo( data );
					break;

				case "api_get_member/mapinfo":
					TextInformation.Text = GetMapGauge( data );
					break;

				case "api_req_mission/result":
					TextInformation.Text = GetExpeditionResult( data );
					_ignorePort = 1;
					break;

				case "api_req_ranking/getlist":
					TextInformation.Text = GetRankingData( data );
					break;

			}

		}


		private string GetPracticeEnemyInfo( dynamic data ) {

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

			return sb.ToString();
		}


		private string GetAlbumInfo( dynamic data ) {

			StringBuilder sb = new StringBuilder();

			if ( data.api_list != null ) {
				const int bound = 50;
				int startIndex = ( ( (int)data.api_list[0].api_index_no - 1 ) / bound ) * bound + 1;
				bool[] flags = Enumerable.Repeat<bool>( false, bound ).ToArray();

				if ( data.api_list[0].api_yomi() ) {
					//艦娘図鑑
					sb.AppendLine( "[中破絵未回収]" );

					foreach ( dynamic elem in data.api_list ) {

						flags[(int)elem.api_index_no - startIndex] = true;

						dynamic[] state = elem.api_state;
						for ( int i = 0; i < state.Length; i++ ) {
							if ( (int)state[i][1] == 0 ) {
								sb.AppendLine( KCDatabase.Instance.MasterShips[(int)elem.api_table_id[i]].Name );
							}
						}

					}

					sb.AppendLine( "[未保有艦]" );
					for ( int i = 0; i < bound; i++ ) {
						if ( !flags[i] ) {
							ShipDataMaster ship = KCDatabase.Instance.MasterShips.Values.FirstOrDefault( s => s.AlbumNo == startIndex + i );
							if ( ship != null ) {
								sb.AppendLine( ship.Name );
							}
						}
					}

				} else {
					//装備図鑑
					foreach ( dynamic elem in data.api_list ) {

						flags[(int)elem.api_index_no - startIndex] = true;
					}

					sb.AppendLine( "[未保有装備]" );
					for ( int i = 0; i < bound; i++ ) {
						if ( !flags[i] ) {
							EquipmentDataMaster eq = KCDatabase.Instance.MasterEquipments.Values.FirstOrDefault( s => s.AlbumNo == startIndex + i );
							if ( eq != null ) {
								sb.AppendLine( eq.Name );
							}
						}
					}
				}
			}

			return sb.ToString();
		}


		private string GetCreateItemInfo( dynamic data ) {

			if ( (int)data.api_create_flag == 0 ) {

				StringBuilder sb = new StringBuilder();
				sb.AppendLine( "[開発失敗]" );
				sb.AppendLine( data.api_fdata );

				EquipmentDataMaster eqm = KCDatabase.Instance.MasterEquipments[int.Parse( ( (string)data.api_fdata ).Split( ",".ToCharArray() )[1] )];
				if ( eqm != null )
					sb.AppendLine( eqm.Name );


				return sb.ToString();

			} else
				return "";
		}


		private string GetMapGauge( dynamic data ) {

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

			return sb.ToString();
		}


		private string GetExpeditionResult( dynamic data ) {
			StringBuilder sb = new StringBuilder();

			sb.AppendLine( "[遠征帰投]" );
			sb.AppendLine( data.api_quest_name );
			sb.AppendFormat( "結果: {0}\r\n", Constants.GetExpeditionResult( (int)data.api_clear_result ) );
			sb.AppendFormat( "提督経験値: +{0}\r\n", (int)data.api_get_exp );
			sb.AppendFormat( "艦娘経験値: +{0}\r\n", ( (int[])data.api_get_ship_exp ).Min() );

			return sb.ToString();
		}


		private string GetRankingData( dynamic data ) {

			StringBuilder sb = new StringBuilder();

			foreach ( dynamic elem in data.api_list ) {

				sb.AppendFormat( "{0}: {1} {2} Lv. {3} / {4} exp.\r\n",
					(int)elem.api_no,
					elem.api_nickname,
					Constants.GetAdmiralRank( (int)elem.api_rank ),
					(int)elem.api_level,
					(int)elem.api_experience
					);

			}

			return sb.ToString();
		}


		protected override string GetPersistString() {
			return "Information";
		}

	}

}
