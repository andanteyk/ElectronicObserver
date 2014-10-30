using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {
	
	public class BattleResultData : ResponseWrapper {

		/// <summary>
		/// 現在保存されているAPIの名前
		/// </summary>
		private string _APIName { get; set; }


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
		/// MVP
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

		//enemyinfo/name

		//undone: 複数の battleresult に対応させる


		/// <summary>
		/// ドロップした艦船のID
		/// </summary>
		public int DroppedShipID {
			get {
				if ( _APIName == "api_req_practice/battle_result" )
					return -1;
				if ( (int)RawData.api_get_flag[1] == 0 )
					return -1;

				return (int)RawData.api_get_ship.api_ship_id;
			}
		}

		
		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );

			_APIName = apiname;
		}
	}

}

