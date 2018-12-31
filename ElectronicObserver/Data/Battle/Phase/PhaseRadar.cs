using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase
{
	/// <summary>
	/// レーダー射撃フェーズの処理を行います。
	/// </summary>
	public class PhaseRadar : PhaseShelling
	{

		// 砲撃戦とフォーマットが同じなので流用

		public PhaseRadar(BattleData data, string title)
			: base(data, title, 1, "1")
		{
		}

		public override bool IsAvailable => RawData.api_hougeki1();

	}
}
