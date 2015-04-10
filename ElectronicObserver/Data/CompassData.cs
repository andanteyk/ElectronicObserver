using System;
using System.Collections.Generic;
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
		/// イベントID
		/// 0=初期位置, 2=資源, 3=渦潮, 4=通常戦闘, 5=ボス戦闘, 6=気のせいだった, 7=連合艦隊航空戦
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
		/// 交戦する敵艦隊ID
		/// </summary>
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
		/// 入手するアイテムのID
		/// </summary>
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

	}


}
