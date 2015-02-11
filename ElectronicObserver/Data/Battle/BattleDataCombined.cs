using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {

	public abstract class BattleDataCombined : BattleData {

		public abstract int FleetIDFriendCombined { get; }

		/// <summary>
		/// 随伴護衛艦隊の初期HP [1-6]
		/// </summary>
		public ReadOnlyCollection<int> InitialHPCombined {
			get { return Array.AsReadOnly<int>( (int[])RawData.api_nowhps_combined ); }
		}

		/// <summary>
		/// 随伴護衛艦隊の最大HP [1-6]
		/// </summary>
		public ReadOnlyCollection<int> MaxHPCombined {
			get { return Array.AsReadOnly<int>( (int[])RawData.api_maxhps_combined ); }
		}

	}
}
