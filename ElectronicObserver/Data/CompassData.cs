using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	/// <summary>
	/// 出撃時のマップ・進撃先を保持します。
	/// </summary>
	public class CompassData : ResponseWrapper {

		/// <summary>
		/// 海域カテゴリID(2-3でいう2)
		/// </summary>
		public int MapAreaID {
			get { return (int)RawData.api_maparea_id; }
		}

		/// <summary>
		/// 海域カテゴリ内番号(2-3でいう3)
		/// </summary>
		public int MapInfoID {
			get { return (int)RawData.api_mapinfo_no; }
		}

		/// <summary>
		/// 次に向かうセルのID
		/// </summary>
		public int Destination {
			get { return (int)RawData.api_no; }
		}

		/// <summary>
		/// 次のセルのグラフィック
		/// </summary>
		public int ColorID {
			get { return (int)RawData.api_color_no; }
		}

		/// <summary>
		/// イベントID
		/// 0=初期位置, 2=資源, 3=渦潮, 4=通常戦闘, 5=ボス戦闘, 6=気のせいだった, 7=航空戦, 8=船団護衛成功
		/// </summary>
		public int EventID {
			get { return (int)RawData.api_event_id; }
		}

		/// <summary>
		/// イベント種別
		/// 0=非戦闘, 1=通常戦闘, 2=夜戦, 3=夜昼戦, 4=航空戦
		/// </summary>
		public int EventKind {
			get { return (int)RawData.api_event_kind; }
		}

		/// <summary>
		/// 次のセルでの分岐の本数
		/// </summary>
		public int NextBranchCount {
			get { return (int)RawData.api_next; }
		}

		/// <summary>
		/// 行き止まりかどうか
		/// </summary>
		public bool IsEndPoint {
			get { return NextBranchCount == 0; }
		}

		/// <summary>
		/// 吹き出しの内容
		/// 0=なし, 1="敵艦隊発見!", 2="攻撃目標発見!"
		/// </summary>
		public int CommentID {
			get {
				if ( RawData.api_comment_kind() )	//startには存在しないため
					return (int)RawData.api_comment_kind;
				else
					return 0;
			}
		}

		/// <summary>
		/// 索敵に成功したか 0=失敗, 1=成功(索敵機発艦)
		/// </summary>
		public int LaunchedRecon {
			get {
				if ( RawData.api_production_kind() )
					return (int)RawData.api_production_kind;
				else
					return 0;
			}
		}


		/// <summary>
		/// 交戦する敵艦隊ID
		/// </summary>
		[Obsolete( "この API 項目は廃止されました。", true )]
		public int EnemyFleetID {
			get {
				if ( RawData.api_enemy() ) {
					return (int)RawData.api_enemy.api_enemy_id;
				} else {
					return -1;
				}
			}
		}


		/// <summary>
		/// 資源セルで入手できる資源のデータです。
		/// </summary>
		public class GetItemData {
			public int ItemID { get; set; }
			public int Metadata { get; set; }
			public int Amount { get; set; }

			public GetItemData( int itemID, int metadata, int amount ) {
				ItemID = itemID;
				Metadata = metadata;
				Amount = amount;
			}
		}

		/// <summary>
		/// 入手するアイテムリスト
		/// </summary>
		public IEnumerable<GetItemData> GetItems {
			get {
				dynamic item;
				if ( RawData.api_itemget() )
					item = RawData.api_itemget;
				else if ( RawData.api_itemget_eo_comment() )
					item = RawData.api_itemget_eo_comment;
				else
					yield break;

				// item.IsArray だと参照できないため
				if ( !( ( (dynamic)item ).IsArray ) ) {
					yield return new GetItemData( (int)item.api_usemst, (int)item.api_id, (int)item.api_getcount );

				} else {
					foreach ( dynamic i in item ) {
						yield return new GetItemData( (int)i.api_usemst, (int)i.api_id, (int)i.api_getcount );
					}
				}
			}

		}


		/// <summary>
		/// 入手するアイテムのID
		/// </summary>
		[Obsolete( "GetItem を利用してください。", false )]
		public int GetItemID {
			get {
				if ( RawData.api_itemget() ) {
					return (int)RawData.api_itemget.api_usemst;
				} else if ( RawData.api_itemget_eo_comment() ) {
					return (int)RawData.api_itemget_eo_comment.api_usemst;
				} else {
					return -1;
				}
			}
		}

		/// <summary>
		/// 入手するアイテムのメタデータ
		/// GetItemID==4のとき、1=燃, 2=弾, 3=鋼, 4=ボ, 5=バーナー, 6=バケツ, 7=開発, 8=改修
		/// </summary>
		[Obsolete( "GetItem を利用してください。", false )]
		public int GetItemIDMetadata {
			get {
				if ( RawData.api_itemget() ) {
					return (int)RawData.api_itemget.api_id;
				} else if ( RawData.api_itemget_eo_comment() ) {
					return (int)RawData.api_itemget_eo_comment.api_id;
				} else {
					return 0;
				}
			}
		}

		/// <summary>
		/// 入手するアイテムの量
		/// </summary>
		[Obsolete( "GetItem を利用してください。", false )]
		public int GetItemAmount {
			get {
				if ( RawData.api_itemget() ) {
					return (int)RawData.api_itemget.api_getcount;
				} else if ( RawData.api_itemget_eo_comment() ) {
					return (int)RawData.api_itemget_eo_comment.api_getcount;
				} else {
					return 0;
				}
			}
		}

		/// <summary>
		/// 渦潮で失うアイテムのID
		/// </summary>
		public int WhirlpoolItemID {
			get {
				if ( RawData.api_happening() ) {
					return (int)RawData.api_happening.api_mst_id;
				} else {
					return -1;
				}
			}
		}

		/// <summary>
		/// 渦潮で失うアイテムの量
		/// </summary>
		public int WhirlpoolItemAmount {
			get {
				if ( RawData.api_happening() ) {
					return (int)RawData.api_happening.api_count;
				} else {
					return 0;
				}
			}
		}

		/// <summary>
		/// 渦潮の被害を電探で軽減するか
		/// </summary>
		public bool WhirlpoolRadarFlag {
			get {
				if ( RawData.api_happening() ) {
					return (int)RawData.api_happening.api_dentan != 0;
				} else {
					return false;
				}
			}
		}

		/// <summary>
		/// 能動分岐の選択肢
		/// </summary>
		public ReadOnlyCollection<int> RouteChoices {
			get {
				if ( RawData.api_select_route() ) {
					return Array.AsReadOnly( (int[])RawData.api_select_route.api_select_cells );
				} else {
					return null;
				}
			}
		}


		/// <summary>
		/// 航空偵察の航空機
		/// 0=なし, 1=大型飛行艇, 2=水上偵察機
		/// </summary>
		public int AirReconnaissancePlane {
			get {
				if ( RawData.api_airsearch() ) {
					return (int)RawData.api_airsearch.api_plane_type;
				} else {
					return 0;
				}
			}
		}


		/// <summary>
		/// 航空偵察結果
		/// 0=失敗, 1=成功, 2=大成功
		/// </summary>
		public int AirReconnaissanceResult {
			get {
				if ( RawData.api_airsearch() ) {
					return (int)RawData.api_airsearch.api_result;
				} else {
					return 0;
				}
			}
		}




		/// <summary>
		/// 対応する海域情報
		/// </summary>
		public MapInfoData MapInfo {
			get { return KCDatabase.Instance.MapInfo[MapAreaID * 10 + MapInfoID]; }
		}
	}


}
