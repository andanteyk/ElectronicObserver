using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase {

	/// <summary>
	/// 開幕対潜攻撃フェーズの処理を行います。
	/// </summary>
	public class PhaseOpeningASW : PhaseShelling {

		// 砲撃戦とフォーマットが同じなので流用

		public PhaseOpeningASW( BattleData data, bool isEscort  )
			: base( data, 0, "", isEscort ) {

		}

		public override bool IsAvailable {
			get { return (int)RawData.api_opening_taisen_flag != 0; }
		}

		public override dynamic ShellingData {
			get { return RawData.api_opening_taisen; }
		}

	}
}
