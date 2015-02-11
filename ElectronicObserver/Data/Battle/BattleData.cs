using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {

	/// <summary>
	/// 戦闘情報を保持するデータの基底です。
	/// </summary>
	public abstract class BattleData : ResponseWrapper {

		//戦闘系はデータが安定しないため、特例的に生データを公開する
		//可能なら封印できるように作る事
		public dynamic Data {
			get { return RawData; }
		}


		/// <summary>
		/// 戦闘中の自軍艦隊ID
		/// </summary>
		public abstract int FleetIDFriend { get; }

		/// <summary>
		/// 敵艦隊の艦船IDリスト [1-6]
		/// </summary>
		public ReadOnlyCollection<int> EnemyFleetMembers {
			get { return Array.AsReadOnly<int>( (int[])RawData.api_ship_ke ); }
		}

		/// <summary>
		/// 全軍の初期HP [1-6]=味方, [7-12]=敵
		/// </summary>
		public ReadOnlyCollection<int> InitialHP {
			get { return Array.AsReadOnly<int>( (int[])RawData.api_nowhps ); }
		}

		/// <summary>
		/// 全軍の最大HP [1-6]=味方, [7-12]=敵
		/// </summary>
		public ReadOnlyCollection<int> MaxHP {
			get { return Array.AsReadOnly<int>( (int[])RawData.api_maxhps ); }
		}


		/// <summary>
		/// 敵艦のレベル [1-6]
		/// </summary>
		public ReadOnlyCollection<int> EnemyLevels {
			get { return Array.AsReadOnly<int>( (int[])RawData.api_ship_lv ); }
		}




		/// <summary>
		/// 戦闘をエミュレートし、戦闘終了時の各艦船のHPを求めます。
		/// </summary>
		/// <returns>戦闘終了時のHP。</returns>
		public abstract int[] EmulateBattle();


		/// <summary>
		/// 対応しているAPIの名前を取得します。
		/// </summary>
		public abstract string APIName { get; }


		[Flags]
		public enum BattleTypeFlag {
			Undefined = 0,
			Day,
			Night,
			Practice = 0x1000,
			Combined = 0x2000,
		}

		/// <summary>
		/// 戦闘モード
		/// </summary>
		public abstract BattleTypeFlag BattleType { get; }

	}

}
