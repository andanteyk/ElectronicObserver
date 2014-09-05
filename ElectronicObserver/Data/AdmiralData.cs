using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	/// <summary>
	/// 提督および司令部の情報を保持します。
	/// </summary>
	public class AdmiralData {	// : IResponseLoader

		/// <summary>
		/// 提督名
		/// </summary>
		public string AdmiralName { get; private set; }

		/// <summary>
		/// 着任日時
		/// </summary>
		public DateTime StartTime { get; private set; }

		/// <summary>
		/// 艦隊司令部Level.
		/// </summary>
		public int Level { get; private set; }

		/// <summary>
		/// 階級
		/// </summary>
		public int Rank { get; private set; }

		/// <summary>
		/// 提督経験値
		/// </summary>
		public int Exp { get; private set; }

		/// <summary>
		/// 提督コメント
		/// </summary>
		public string Comment { get; private set; }

		/// <summary>
		/// 最大保有可能艦娘数
		/// </summary>
		public int MaxShipCount { get; private set; }
		
		/// <summary>
		/// 最大保有可能装備数
		/// </summary>
		public int MaxEquipmentCount { get; private set; }
		

		//fleet / arsenal / dock count
		

		/// <summary>
		/// 家具コイン
		/// </summary>
		public int FurnitureCoin { get; private set; }
		
		/// <summary>
		/// 出撃の勝数
		/// </summary>
		public int SortieWin { get; private set; }
		
		/// <summary>
		/// 出撃の敗数
		/// </summary>
		public int SortieLose { get; private set; }
		
		/// <summary>
		/// 遠征の回数
		/// </summary>
		public int MissionCount { get; private set; }
		
		/// <summary>
		/// 遠征の成功数
		/// </summary>
		public int MissionSuccess { get; private set; }
		
		/// <summary>
		/// 演習の勝数
		/// </summary>
		public int PracticeWin { get; private set; }
		
		/// <summary>
		/// 演習の敗数
		/// </summary>
		public int PracticeLose { get; private set; }

	}


}
