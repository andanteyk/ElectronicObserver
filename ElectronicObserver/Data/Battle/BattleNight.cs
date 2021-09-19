using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle
{

	/// <summary>
	/// 夜戦の基底クラス
	/// </summary>
	public abstract class BattleNight : BattleData
	{
		public PhaseNightInitial NightInitial { get; protected set; }
		public PhaseFriendlyShelling FriendlyShelling { get; protected set; }
		public PhaseNightBattle NightBattle { get; protected set; }
		
	}
}
