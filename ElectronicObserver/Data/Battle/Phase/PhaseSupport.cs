using ElectronicObserver.Data.Battle.Detail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase
{

	/// <summary>
	/// 支援攻撃フェーズの処理を行います。
	/// </summary>
	public class PhaseSupport : PhaseBase
	{

		public readonly bool IsNight;

		public PhaseSupport(BattleData data, string title, bool isNight = false)
			: base(data, title)
		{
			IsNight = isNight;

			switch (SupportFlag)
			{
				case 1:     // 空撃
				case 4:     // 対潜
					{
						if ((int)SupportData.api_support_airatack.api_stage_flag[2] != 0)
						{
							// 敵連合でも api_stage3_combined は存在しない？

							Damages = ((double[])SupportData.api_support_airatack.api_stage3.api_edam).ToArray();
							Criticals = ((int[])SupportData.api_support_airatack.api_stage3.api_ecl_flag).ToArray();

							// 航空戦なので crit フラグが違う
							for (int i = 0; i < Criticals.Length; i++)
								Criticals[i]++;
						}
						else
						{
							Damages = new double[12];
							Criticals = new int[12];
						}
					}
					break;
				case 2:     // 砲撃
				case 3:     // 雷撃
					{
						var dmg = (double[])SupportData.api_support_hourai.api_damage;
						var cl = (int[])SupportData.api_support_hourai.api_cl_list;

						Damages = new double[12];
						Array.Copy(dmg, Damages, dmg.Length);

						Criticals = new int[12];
						Array.Copy(cl, Criticals, cl.Length);
					}
					break;
				default:
					Damages = new double[12];
					Criticals = new int[12];
					break;
			}
		}


		public override bool IsAvailable => SupportFlag != 0;

		public override void EmulateBattle(int[] hps, int[] damages)
		{

			if (!IsAvailable) return;

			for (int i = 0; i < Battle.Initial.EnemyMembers.Length; i++)
			{
				if (Battle.Initial.EnemyMembers[i] > 0)
				{
					var index = new BattleIndex(BattleSides.EnemyMain, i);
					BattleDetails.Add(new BattleSupportDetail(Battle, index, Damages[i], Criticals[i], SupportFlag, hps[index]));
					AddDamage(hps, index, (int)Damages[i]);
				}
			}
			if (Battle.IsEnemyCombined)
			{
				for (int i = 0; i < Battle.Initial.EnemyMembersEscort[i]; i++)
				{
					if (Battle.Initial.EnemyMembersEscort[i] > 0)
					{
						var index = new BattleIndex(BattleSides.EnemyEscort, i);
						BattleDetails.Add(new BattleSupportDetail(Battle, index, Damages[i + 6], Criticals[i + 6], SupportFlag, hps[index]));
						AddDamage(hps, index, (int)Damages[i + 6]);
					}
				}
			}
		}

		protected override IEnumerable<BattleDetail> SearchBattleDetails(int index)
		{
			return BattleDetails.Where(d => d.DefenderIndex == index);
		}


		/// <summary>
		/// 支援艦隊フラグ
		/// </summary>
		public int SupportFlag
		{
			get
			{
				if (IsNight)
					return RawData.api_n_support_flag() ? (int)RawData.api_n_support_flag : 0;
				else
					return RawData.api_support_flag() ? (int)RawData.api_support_flag : 0;
			}
		}

		public dynamic SupportData => IsNight ? RawData.api_n_support_info : RawData.api_support_info;

		/// <summary>
		/// 支援艦隊ID
		/// </summary>
		public int SupportFleetID
		{
			get
			{
				switch (SupportFlag)
				{
					case 1:
					case 4:
						return (int)SupportData.api_support_airatack.api_deck_id;

					case 2:
					case 3:
						return (int)SupportData.api_support_hourai.api_deck_id;

					default:
						return -1;

				}
			}
		}

		/// <summary>
		/// 支援艦隊
		/// </summary>
		public FleetData SupportFleet
		{
			get
			{
				int id = SupportFleetID;
				if (id != -1)
					return KCDatabase.Instance.Fleet[id];
				else
					return null;
			}
		}


		/// <summary>
		/// 与ダメージ [12]
		/// </summary>
		public double[] Damages { get; private set; }

		/// <summary>
		/// クリティカルフラグ [12]
		/// </summary>
		public int[] Criticals { get; private set; }


	}
}
