using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {

	/// <summary>
	/// 戦闘結果のデータを扱います。
	/// </summary>
	public class BattleResultData : ResponseWrapper {

		/// <summary>
		/// 現在保存されているAPIの名前
		/// </summary>
		private string _APIName { get; set; }



		// :(
		/// <summary>
		/// 演習の結果かどうか
		/// </summary>
		private bool IsPractice {
			get {
				return _APIName == "api_req_practice/battle_result";
			}
		}



		/// <summary>
		/// 勝利ランク
		/// </summary>
		public string Rank {
			get { return RawData.api_win_rank; }
		}

		/// <summary>
		/// 獲得提督経験値
		/// </summary>
		public int AdmiralExp {
			get { return (int)RawData.api_get_exp; }
		}

		/// <summary>
		/// MVP艦のインデックス(1-6)
		/// </summary>
		public int MVP {
			get { return (int)RawData.api_mvp; }
		}

		/// <summary>
		/// 獲得基本経験値
		/// </summary>
		public int BaseExp {
			get { return (int)RawData.api_get_base_exp; }
		}

		//exp

		//lostflag


		/// <summary>
		/// 敵艦隊名
		/// </summary>
		public string EnemyFleetName {
			get { return RawData.api_enemy_info.api_deck_name; }
		}

		//undone: 複数の battleresult に対応させる


		/// <summary>
		/// ドロップした艦船のID
		/// </summary>
		public int DroppedShipID {
			get {
				if ( IsPractice )
					return -1;
				if ( (int)RawData.api_get_flag[1] == 0 )
					return -1;

				return (int)RawData.api_get_ship.api_ship_id;
			}
		}


		/// <summary>
		/// ドロップしたアイテムのID
		/// </summary>
		public int DroppedItemID {
			get {
				if ( IsPractice )
					return -1;
				if ( (int)RawData.api_get_flag[0] == 0 )
					return -1;

				return (int)RawData.api_get_useitem.api_useitem_id;
			}
		}


		/// <summary>
		/// ドロップした装備のID(現在未使用)
		/// </summary>
		public int DroppedEquipmentID {
			get {
				if ( IsPractice )
					return -1;
				if ( (int)RawData.api_get_flag[2] == 0 )
					return -1;

				return (int)RawData.api_get_slotitem.api_slotitem_id;
			}
		}




		/// <summary>
		/// 護衛退避可能か
		/// </summary>
		public bool CanEscape {
			get {
				if ( !RawData.api_escape() ) {
					return false;
				} else {
					return (int)RawData.api_escape != 0;
				}
			}
		}

		/// <summary>
		/// 退避艦のインデックス
		/// </summary>
		public IEnumerable<int> EscapingShipIndex {
			get {
				if ( !RawData.api_escape() ) {
					return null;
				} else {
					return new int[2] { (int)RawData.api_escape.api_escape_idx[0], (int)RawData.api_escape.api_tow_idx[0] }.AsEnumerable();
				}

			}
		}


		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );

			_APIName = apiname;

		}
	}

}

