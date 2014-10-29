using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {

	/// <summary>
	/// 戦闘情報を保持するデータの基底です。
	/// </summary>
	public abstract class BattleData : ResponseWrapper {

		//public dynamic RawData { get; protected set; }


		/// <summary>
		/// 戦闘をエミュレートし、戦闘終了時の各艦船のHPを求めます。
		/// </summary>
		/// <returns>戦闘終了時のHP。</returns>
		public abstract int[] EmulateBattle();


		/// <summary>
		/// 対応しているAPIの名前を取得します。
		/// </summary>
		public abstract string APIName { get; }

	}

}
