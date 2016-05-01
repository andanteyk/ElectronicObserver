using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	/// <summary>
	/// 提督および司令部の情報を保持します。
	/// </summary>
	public class AdmiralData : APIWrapper {

		/// <summary>
		/// 提督ID
		/// </summary>
		public int AdmiralID {
			get {
				if ( RawData.api_member_id is string )
					return int.Parse( RawData.api_member_id );
				else
					return (int)RawData.api_member_id;
			}
		}

		/// <summary>
		/// 提督名
		/// </summary>
		public string AdmiralName {
			get { return RawData.api_nickname; }
		}

		/// <summary>
		/// 起動日時
		/// </summary>
		public DateTime StartTime {
			get { return DateTimeHelper.FromAPITime( (long)RawData.api_starttime ); }
		}

		/// <summary>
		/// 艦隊司令部Level.
		/// </summary>
		public int Level {
			get { return (int)RawData.api_level; }
		}

		/// <summary>
		/// 階級
		/// </summary>
		public int Rank {
			get { return (int)RawData.api_rank; }
		}

		/// <summary>
		/// 提督経験値
		/// </summary>
		public int Exp {
			get { return (int)RawData.api_experience; }
		}

		/// <summary>
		/// 提督コメント
		/// </summary>
		public string Comment {
			get { return RawData.api_comment; }
		}

		/// <summary>
		/// 最大保有可能艦娘数
		/// </summary>
		public int MaxShipCount {
			get { return (int)RawData.api_max_chara; }
		}

		/// <summary>
		/// 最大保有可能装備数
		/// </summary>
		public int MaxEquipmentCount {
			get { return (int)RawData.api_max_slotitem; }
		}

		/// <summary>
		/// 最大保有可能艦隊数
		/// </summary>
		public int FleetCount {
			get { return (int)RawData.api_count_deck; }
		}

		/// <summary>
		/// 工廠ドック数
		/// </summary>
		public int ArsenalCount {
			get { return (int)RawData.api_count_kdock; }
		}

		/// <summary>
		/// 入渠ドック数
		/// </summary>
		public int DockCount {
			get { return (int)RawData.api_count_ndock; }
		}


		/// <summary>
		/// 家具コイン
		/// </summary>
		public int FurnitureCoin {
			get { return (int)RawData.api_fcoin; }
		}

		/// <summary>
		/// 出撃の勝数
		/// </summary>
		public int SortieWin {
			get { return (int)RawData.api_st_win; }
		}

		/// <summary>
		/// 出撃の敗数
		/// </summary>
		public int SortieLose {
			get { return (int)RawData.api_st_lose; }
		}

		/// <summary>
		/// 遠征の回数
		/// </summary>
		public int MissionCount {
			get { return (int)RawData.api_ms_count; }
		}

		/// <summary>
		/// 遠征の成功数
		/// </summary>
		public int MissionSuccess {
			get { return (int)RawData.api_ms_success; }
		}

		/// <summary>
		/// 演習の勝数
		/// </summary>
		public int PracticeWin {
			get { return (int)RawData.api_pt_win; }
		}

		/// <summary>
		/// 演習の敗数
		/// </summary>
		public int PracticeLose {
			get { return (int)RawData.api_pt_lose; }
		}



		/// <summary>
		/// 資源の自然回復上限
		/// </summary>
		public int MaxResourceRegenerationAmount {
			get { return Level * 250 + 750; }
		}


		public override void LoadFromRequest( string apiname, Dictionary<string, string> data ) {
			base.LoadFromRequest( apiname, data );

			if ( apiname == "api_req_member/updatecomment" ) {
				if ( RawData != null )
					RawData.api_comment = data["api_cmt"];
			}
		}
	}


}
