using ElectronicObserver.Data.Battle.Detail;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicObserver.Data.Battle.Phase
{
	/// <summary>
	/// 昼戦における友軍艦隊航空攻撃フェーズの処理を行います。
	/// </summary>
	public class PhaseFriendlyAirBattle : PhaseAirBattleBase
	{
		public PhaseFriendlyAirBattle(BattleData battle, string title)
			: base(battle, title)
		{
			if (!IsAvailable)
				return;

			AirBattleData = RawData.api_friendly_kouku;
			StageFlag = AirBattleData.api_stage_flag;

			LaunchedShipIndexFriend = GetLaunchedShipIndex(0);
			LaunchedShipIndexEnemy = GetLaunchedShipIndex(1);

			TorpedoFlags = ConcatStage3Array<int>("api_frai_flag", "api_erai_flag");
			BomberFlags = ConcatStage3Array<int>("api_fbak_flag", "api_ebak_flag");
			Criticals = ConcatStage3Array<int>("api_fcl_flag", "api_ecl_flag");
			Damages = ConcatStage3Array<double>("api_fdam", "api_edam");
		}

		public override bool IsAvailable => RawData.api_friendly_kouku();

		public override void EmulateBattle(int[] hps, int[] damages)
		{
			if (!IsAvailable)
				return;

			var friendHps = Battle.FriendlySupportInfo.FriendlyInitialHPs.ToArray();

			for (int i = 0; i < TorpedoFlags.Length; i++)
			{
				int attackType = (TorpedoFlags[i] > 0 ? 1 : 0) | (BomberFlags[i] > 0 ? 2 : 0);
				if (attackType > 0)
				{
					bool isEnemy = new BattleIndex(i, false, Battle.IsEnemyCombined).IsEnemy;


					// 航空戦は miss/hit=0, critical=1 のため +1 する(通常は miss=0, hit=1, critical=2) 
					BattleDetails.Add(new BattleFriendlyAirDetail(
						Battle,
						new BattleIndex(i, Battle.IsFriendCombined, Battle.IsEnemyCombined),
						Damages[i],
						Criticals[i] + 1,
						attackType,
						isEnemy ? hps[i] : friendHps[i]));

					if (isEnemy)
						AddDamage(hps, i, (int)Damages[i]);
				}
			}
		}

		protected override IEnumerable<BattleDetail> SearchBattleDetails(int index)
		{
			return BattleDetails.Where(d => d.DefenderIndex.IsEnemy && d.DefenderIndex == index);
		}

		public override string AACutInShipName => Battle.FriendlySupportInfo.FriendlyMembersInstance[AACutInIndex].Name + " Lv. " + Battle.FriendlySupportInfo.FriendlyLevels[AACutInIndex];
	}
}
