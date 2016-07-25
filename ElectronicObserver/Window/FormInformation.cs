using Codeplex.Data;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window {

	public partial class FormInformation : DockContent {

		private int _ignorePort;
		private List<int> _inSortie;

		public FormInformation( FormMain parent ) {
			InitializeComponent();

			_ignorePort = 0;
			_inSortie = null;

			ConfigurationChanged();

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormInformation] );
		}


		private void FormInformation_Load( object sender, EventArgs e ) {

			APIObserver o = APIObserver.Instance;

			o["api_port/port"].ResponseReceived += Updated;
			o["api_req_member/get_practice_enemyinfo"].ResponseReceived += Updated;
			o["api_get_member/picture_book"].ResponseReceived += Updated;
			o["api_req_kousyou/createitem"].ResponseReceived += Updated;
			o["api_get_member/mapinfo"].ResponseReceived += Updated;
			o["api_req_mission/result"].ResponseReceived += Updated;
			o["api_req_practice/battle_result"].ResponseReceived += Updated;
			o["api_req_sortie/battleresult"].ResponseReceived += Updated;
			o["api_req_combined_battle/battleresult"].ResponseReceived += Updated;
			o["api_req_hokyu/charge"].ResponseReceived += Updated;
			o["api_req_map/start"].ResponseReceived += Updated;
			o["api_req_practice/battle"].ResponseReceived += Updated;
            o["api_req_map/next"].ResponseReceived += Updated;
            o["api_req_sortie/battle"].ResponseReceived += Updated;
            o["api_req_combined_battle/battle"].ResponseReceived += Updated;
            o["api_req_combined_battle/battle_water"].ResponseReceived += Updated;

            Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		}

		private bool ShowFailedDevelopment;

		void ConfigurationChanged() {

			Font = TextInformation.Font = Utility.Configuration.Config.UI.MainFont;

			TextInformation.BackColor = Utility.Configuration.Config.UI.BackColor;
			TextInformation.ForeColor = Utility.Configuration.Config.UI.ForeColor;

			var settings = Information.Settings.settings;

			if ( settings == null )
			{
				try
				{
					if ( File.Exists( Information.Settings.PLUGIN_SETTINGS ) )
						settings = DynamicJson.Parse( File.ReadAllText( Information.Settings.PLUGIN_SETTINGS ) );
					else
						settings = DynamicJson.Parse( Information.Settings.DEFAULT_SETTINGS );
				}
				catch
				{
					settings = DynamicJson.Parse( Information.Settings.DEFAULT_SETTINGS );
				}

				Information.Settings.settings = settings;
			}

			if ( settings != null )
			{
				ShowFailedDevelopment = settings.ShowFailedDevelopment;
			}
			else
			{
				ShowFailedDevelopment = true;
			}
		}


		void Updated( string apiname, dynamic data ) {

			switch ( apiname ) {

				case "api_port/port":
					if ( _ignorePort > 0 )
						_ignorePort--;
					else
						TextInformation.Text = "";		//とりあえずクリア

					if ( _inSortie != null ) {
						TextInformation.Text = GetConsumptionResource( data );
					}
					_inSortie = null;
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

				case "api_req_practice/battle_result":
				case "api_req_sortie/battleresult":
				case "api_req_combined_battle/battleresult":
					TextInformation.Text = GetBattleResult( data );
					break;

				case "api_req_hokyu/charge":
					TextInformation.Text = GetSupplyInformation( data );
					break;

				case "api_req_map/start":
					_inSortie = KCDatabase.Instance.Fleet.Fleets.Values.Where( f => f.IsInSortie || f.ExpeditionState == 1 ).Select( f => f.FleetID ).ToList();
					break;

				case "api_req_practice/battle":
					_inSortie = new List<int>() { KCDatabase.Instance.Battle.BattleDay.Initial.FriendFleetID };
					break;

                case "api_req_map/next":
                    var info = GetAirBaseAttackImformation(data);
                    if (info != null)
                        TextInformation.Text = info;
                    break;

                case "api_req_sortie/battle":
                case "api_req_combined_battle/battle":
                case "api_req_combined_battle/battle_water":
                    var Damaged = GetBossDamaged(data);
                    if (Damaged != null)
                        TextInformation.Text = Damaged;
                    break;
            }

		}

        private string GetBossDamaged(dynamic data)
        {
            if (data.api_boss_damaged())
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("[BOSS受损情报]");
                int BossDamaged = (int)(data.api_boss_damaged);
                sb.AppendFormat("BOSS受损状态:{0}", BossDamaged == 1 ? "是" : "否");
                return sb.ToString();
            }
            return null;
        }

        private string GetAirBaseAttackImformation(dynamic data)
        {
            if (data.api_destruction_battle())
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("[基地遭受空袭]");

                int[] NowHP = (int[])(data.api_destruction_battle.api_nowhps);
                int[] MaxHP = (int[])(data.api_destruction_battle.api_maxhps);
                var air_base_attack = data.api_destruction_battle.api_air_base_attack;
                int[] stageFlag = (int[])(air_base_attack.api_stage_flag);
                if ((int)stageFlag[0] == 1)
                {
                    int seiku = (int)(air_base_attack.api_stage1.api_disp_seiku);
                    sb.AppendLine(Constants.GetAirSuperiority(seiku));
                    int fcount = (int)(air_base_attack.api_stage1.api_f_count);
                    int flost = (int)(air_base_attack.api_stage1.api_f_lostcount);
                    int ecount = (int)(air_base_attack.api_stage1.api_e_count);
                    int elost = (int)(air_base_attack.api_stage1.api_e_lostcount);
                    if (fcount > 0)
                    {
                        sb.AppendFormat("我方:{0}->{1}", fcount, fcount - flost);
                        sb.AppendLine();
                    }
                    sb.AppendFormat("敌方:{0}->{1}", ecount, ecount - elost);
                    sb.AppendLine();

                    if ((int)stageFlag[2] == 1)
                    {
                        int[] dam = (int[])(air_base_attack.api_stage3.api_fdam);
                        for (int index = 1; index <= 6; index++)
                        {
                            if (MaxHP[index] == -1)
                                break;
                            sb.AppendFormat("基地{0}:{1}->{2}/{3}", index, NowHP[index], NowHP[index] - dam[index], MaxHP[index]);
                            sb.AppendLine();
                        }
                    }
                }
                return sb.ToString();
            }

            return null;
        }

		private string GetPracticeEnemyInfo( dynamic data ) {

			StringBuilder sb = new StringBuilder();
			sb.AppendLine( "[演習情報]" );
			sb.AppendLine( "敵提督名 : " + data.api_nickname );
			sb.AppendLine( "敵艦隊名 : " + data.api_deckname );

			{
				int ship1lv = (int)data.api_deck.api_ships[0].api_id != -1 ? (int)data.api_deck.api_ships[0].api_level : 1;
				int ship2lv = (int)data.api_deck.api_ships[1].api_id != -1 ? (int)data.api_deck.api_ships[1].api_level : 1;

				// 経験値テーブルが拡張されたとき用の対策
				ship1lv = Math.Min( ship1lv, ExpTable.ShipExp.Keys.Max() );
				ship2lv = Math.Min( ship2lv, ExpTable.ShipExp.Keys.Max() );

				double expbase = ExpTable.ShipExp[ship1lv].Total / 100.0 + ExpTable.ShipExp[ship2lv].Total / 300.0;
				if ( expbase >= 500.0 )
					expbase = 500.0 + Math.Sqrt( expbase - 500.0 );

				expbase = (int)expbase;

				sb.AppendFormat( "獲得経験値: {0} / S勝利: {1}\r\n", expbase, (int)( expbase * 1.2 ) );


				// 練巡ボーナス計算 - きたない
				var fleet = KCDatabase.Instance.Fleet[1];
				if ( fleet.MembersInstance.Any( s => s != null && s.MasterShip.ShipType == 21 ) ) {
					var members = fleet.MembersInstance;
					var subCT = members.Skip( 1 ).Where( s => s != null && s.MasterShip.ShipType == 21 );

					double bonus;

					// 旗艦が練巡
					if ( members[0] != null && members[0].MasterShip.ShipType == 21 ) {

						int level = members[0].Level;

						if ( subCT != null && subCT.Any() ) {
							// 旗艦+随伴
							if ( level < 10 ) bonus = 1.10;
							else if ( level < 30 ) bonus = 1.13;
							else if ( level < 60 ) bonus = 1.16;
							else if ( level < 100 ) bonus = 1.20;
							else bonus = 1.25;

						} else {
							// 旗艦のみ
							if ( level < 10 ) bonus = 1.05;
							else if ( level < 30 ) bonus = 1.08;
							else if ( level < 60 ) bonus = 1.12;
							else if ( level < 100 ) bonus = 1.15;
							else bonus = 1.20;
						}

					} else {

						int level = subCT.Max( s => s.Level );

						if ( subCT.Count() > 1 ) {
							// 随伴複数	
							if ( level < 10 ) bonus = 1.04;
							else if ( level < 30 ) bonus = 1.06;
							else if ( level < 60 ) bonus = 1.08;
							else if ( level < 100 ) bonus = 1.12;
							else bonus = 1.175;

						} else {
							// 随伴単艦
							if ( level < 10 ) bonus = 1.03;
							else if ( level < 30 ) bonus = 1.05;
							else if ( level < 60 ) bonus = 1.07;
							else if ( level < 100 ) bonus = 1.10;
							else bonus = 1.15;
						}
					}

					sb.AppendFormat( "(練巡強化: {0} / S勝利: {1})\r\n", (int)( expbase * bonus ), (int)( (int)( expbase * 1.2 ) * bonus ) );


				}
			}

			return sb.ToString();
		}


		private string GetAlbumInfo( dynamic data ) {

			StringBuilder sb = new StringBuilder();

			if ( data != null && data.api_list() && data.api_list != null ) {

				if ( data.api_list[0].api_yomi() ) {

					//艦娘図鑑
					const int bound = 70;		// 図鑑1ページあたりの艦船数
					int startIndex = ( ( (int)data.api_list[0].api_index_no - 1 ) / bound ) * bound + 1;
					bool[] flags = Enumerable.Repeat<bool>( false, bound ).ToArray();

					sb.AppendLine( "[中破絵未回収]" );

					foreach ( dynamic elem in data.api_list ) {

						flags[(int)elem.api_index_no - startIndex] = true;

						dynamic[] state = elem.api_state;
						for ( int i = 0; i < state.Length; i++ ) {
							if ( (int)state[i][1] == 0 ) {
								var target = KCDatabase.Instance.MasterShips[(int)elem.api_table_id[i]];
								if ( target != null )		//季節の衣替え艦娘の場合存在しないことがある
									sb.AppendLine( target.Name );
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
					const int bound = 70;		// 図鑑1ページあたりの装備数
					int startIndex = ( ( (int)data.api_list[0].api_index_no - 1 ) / bound ) * bound + 1;
					bool[] flags = Enumerable.Repeat<bool>( false, bound ).ToArray();

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

				if ( ShowFailedDevelopment )
				{
					EquipmentDataMaster eqm = KCDatabase.Instance.MasterEquipments[int.Parse( ( (string)data.api_fdata ).Split( ",".ToCharArray() )[1] )];
					if ( eqm != null )
						sb.AppendLine( eqm.Name );
				}


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

						int now_maphp = (int)elem.api_eventmap.api_now_maphp;
						if ( now_maphp > 0 ) {
							string difficulty = "";
							if ( elem.api_eventmap.api_selected_rank() ) {
								difficulty = "[" + Constants.GetDifficulty( (int)elem.api_eventmap.api_selected_rank ) + "] ";
							}

							sb.AppendFormat( "{0}-{1} {2}: {3} {4}/{5}\r\n",
								map.MapAreaID, map.MapInfoID, difficulty,
								elem.api_eventmap.api_gauge_type() && (int)elem.api_eventmap.api_gauge_type == 3 ? "TP" : "HP",
								(int)elem.api_eventmap.api_now_maphp, (int)elem.api_eventmap.api_max_maphp );
						}

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


		private string GetBattleResult( dynamic data ) {
			StringBuilder sb = new StringBuilder();

			sb.AppendLine( "[戦闘終了]" );
			sb.AppendFormat( "敵艦隊名: {0}\r\n", data.api_enemy_info.api_deck_name );
			sb.AppendFormat( "勝敗判定: {0}\r\n", data.api_win_rank );
			sb.AppendFormat( "提督経験値: +{0}\r\n", (int)data.api_get_exp );

			return sb.ToString();
		}


		private string GetSupplyInformation( dynamic data ) {

			StringBuilder sb = new StringBuilder();

			sb.AppendLine( "[補給完了]" );
			sb.AppendFormat( "ボーキサイト: {0} ( {1}機 )\r\n", (int)data.api_use_bou, (int)data.api_use_bou / 5 );

			return sb.ToString();
		}


		private string GetConsumptionResource( dynamic data ) {

			StringBuilder sb = new StringBuilder();
			int fuel_supply = 0,
				fuel_repair = 0,
				ammo = 0,
				steel = 0,
				bauxite = 0;


			sb.AppendLine( "[艦隊帰投]" );

			foreach ( var f in KCDatabase.Instance.Fleet.Fleets.Values.Where( f => _inSortie.Contains( f.FleetID ) ) ) {

				fuel_supply += f.MembersInstance.Sum( s => s == null ? 0 : (int)Math.Floor( ( s.FuelMax - s.Fuel ) * ( s.IsMarried ? 0.85 : 1.0 ) ) );
				ammo += f.MembersInstance.Sum( s => s == null ? 0 : (int)Math.Floor( ( s.AmmoMax - s.Ammo ) * ( s.IsMarried ? 0.85 : 1.0 ) ) );
				bauxite += f.MembersInstance.Sum( s => s == null ? 0 : s.Aircraft.Zip( s.MasterShip.Aircraft, ( current, max ) => new { Current = current, Max = max } ).Sum( a => ( a.Max - a.Current ) * 5 ) );

				fuel_repair += f.MembersInstance.Sum( s => s == null ? 0 : s.RepairFuel );
				steel += f.MembersInstance.Sum( s => s == null ? 0 : s.RepairSteel );

			}

			sb.AppendFormat( "燃料: {0} (補給) + {1} (入渠) = {2}\r\n弾薬: {3}\r\n鋼材: {4}\r\nボーキ: {5} ( {6}機 )\r\n",
				fuel_supply, fuel_repair, fuel_supply + fuel_repair, ammo, steel, bauxite, bauxite / 5 );

			return sb.ToString();
		}

		public override string GetPersistString() {
			return "Information";
		}

	}

}
