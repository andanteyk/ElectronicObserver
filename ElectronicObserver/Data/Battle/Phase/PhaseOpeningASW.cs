using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase
{

	/// <summary>
	/// 開幕対潜攻撃フェーズの処理を行います。
	/// </summary>
	public class PhaseOpeningASW : PhaseShelling
	{

		// 砲撃戦とフォーマットが同じなので流用

		public PhaseOpeningASW(BattleData data, string title)
			: base(data, title, 0, "")
		{

		}

		public override bool IsAvailable => (int)RawData.api_opening_taisen_flag != 0;

		public override dynamic ShellingData => RawData.api_opening_taisen;
	}

}
