using ElectronicObserver.Data.Battle.Detail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase
{

	/// <summary>
	/// 航空戦フェーズ処理の基底となるクラスです。
	/// </summary>
	public abstract class PhaseAirBattleBase : PhaseBase
	{

		public PhaseAirBattleBase(BattleData data, string title)
			: base(data, title)
		{

		}

		public override bool IsAvailable => StageFlag != null && StageFlag.Any(i => i != 0);



		/// <summary>
		/// 被ダメージ処理を行います。
		/// </summary>
		/// <param name="waveIndex">(基地航空隊の場合)現在の攻撃ウェーブのインデックス。それ以外の場合は 0</param>
		/// <param name="hps">現在のHPリスト。</param>
		protected void CalculateAttack(int waveIndex, int[] hps)
		{
			for (int i = 0; i < hps.Length; i++)
			{

				int attackType = (TorpedoFlags[i] > 0 ? 1 : 0) | (BomberFlags[i] > 0 ? 2 : 0);
				if (attackType > 0)
				{

					// 航空戦は miss/hit=0, critical=1 のため +1 する(通常は miss=0, hit=1, critical=2) 
					BattleDetails.Add(new BattleAirDetail(Battle, waveIndex, new BattleIndex(i, Battle.IsFriendCombined, Battle.IsEnemyCombined), Damages[i], Criticals[i] + 1, attackType, hps[i]));
					AddDamage(hps, i, (int)Damages[i]);
				}
			}
		}



		/// <summary>
		/// 各 Stage が存在するか
		/// </summary>
		public int[] StageFlag { get; protected set; }

		/// <summary>
		/// 航空戦の生データ
		/// </summary>
		public dynamic AirBattleData { get; protected set; }


		/// <summary>
		/// 航空機を発艦させた自軍艦船のインデックス 値は[0-11]
		/// </summary>
		public int[] LaunchedShipIndexFriend { get; protected set; }

		/// <summary>
		/// 航空機を発艦させた敵軍艦船のインデックス 値は[0-11]
		/// </summary>
		public int[] LaunchedShipIndexEnemy { get; protected set; }


		/// <summary>
		/// 航空機を発艦させた艦船のインデックスを取得します。
		/// </summary>
		/// <param name="index">取得する配列のインデックス。基本的に 0=自軍, 1=敵軍</param>
		protected int[] GetLaunchedShipIndex(int index)
		{
			if (AirBattleData == null)
				return null;

			if (!AirBattleData.api_plane_from())
				return new int[0];

			dynamic data = AirBattleData.api_plane_from;

			if (data == null || !data.IsArray)
				return new int[0];

			var planes = (dynamic[])data;
			if (index < planes.Length)
			{
				var plane = (int[])planes[index];

				if (plane == null)
					return new int[0];

				return plane.Where(i => i > 0).Select(i => i - 1).ToArray();
			}

			return new int[0];
		}


		// stage 1

		/// <summary>
		/// Stage1(空対空戦闘)が存在するか
		/// </summary>
		public bool IsStage1Available => StageFlag != null && StageFlag[0] != 0 && AirBattleData.api_stage1() && AirBattleData.api_stage1 != null;

		/// <summary>
		/// 自軍Stage1参加機数
		/// </summary>
		public int AircraftTotalStage1Friend => (int)AirBattleData.api_stage1.api_f_count;

		/// <summary>
		/// 敵軍Stage1参加機数
		/// </summary>
		public int AircraftTotalStage1Enemy => (int)AirBattleData.api_stage1.api_e_count;

		/// <summary>
		/// 自軍Stage1撃墜機数
		/// </summary>
		public int AircraftLostStage1Friend => (int)AirBattleData.api_stage1.api_f_lostcount;

		/// <summary>
		/// 敵軍Stage1撃墜機数
		/// </summary>
		public int AircraftLostStage1Enemy => (int)AirBattleData.api_stage1.api_e_lostcount;

		/// <summary>
		/// 制空権
		/// </summary>
		public int AirSuperiority => !AirBattleData.api_stage1.api_disp_seiku() ? -1 : (int)AirBattleData.api_stage1.api_disp_seiku;

		/// <summary>
		/// 自軍触接機ID
		/// </summary>
		public int TouchAircraftFriend => !AirBattleData.api_stage1.api_touch_plane() ? -1 : (int)AirBattleData.api_stage1.api_touch_plane[0];

		/// <summary>
		/// 敵軍触接機ID
		/// </summary>
		public int TouchAircraftEnemy => !AirBattleData.api_stage1.api_touch_plane() ? -1 : (int)AirBattleData.api_stage1.api_touch_plane[1];


		// stage 2

		/// <summary>
		/// Stage2(艦対空戦闘)が存在するか
		/// </summary>
		public bool IsStage2Available => StageFlag != null && StageFlag[1] != 0 && AirBattleData.api_stage2() && AirBattleData.api_stage2 != null;

		/// <summary>
		/// 自軍Stage2参加機数
		/// </summary>
		public int AircraftTotalStage2Friend => (int)AirBattleData.api_stage2.api_f_count;

		/// <summary>
		/// 敵軍Stage2参加機数
		/// </summary>
		public int AircraftTotalStage2Enemy => (int)AirBattleData.api_stage2.api_e_count;

		/// <summary>
		/// 自軍Stage2撃墜機数
		/// </summary>
		public int AircraftLostStage2Friend => (int)AirBattleData.api_stage2.api_f_lostcount;

		/// <summary>
		/// 敵軍Stage2撃墜機数
		/// </summary>
		public int AircraftLostStage2Enemy => (int)AirBattleData.api_stage2.api_e_lostcount;


		/// <summary>
		/// 対空カットインが発動したか
		/// </summary>
		public bool IsAACutinAvailable => AirBattleData.api_stage2.api_air_fire();

		/// <summary>
		/// 対空カットイン発動艦番号
		/// </summary>
		public int AACutInIndex => (int)AirBattleData.api_stage2.api_air_fire.api_idx;

		/// <summary>
		/// 対空カットイン発動艦
		/// </summary>
		public ShipData AACutInShip => Battle.Initial.GetFriendShip(AACutInIndex);

		/// <summary>
		/// 対空カットイン種別
		/// </summary>
		public int AACutInKind => (int)AirBattleData.api_stage2.api_air_fire.api_kind;


		// stage 3

		/// <summary>
		/// Stage3(航空攻撃)が存在するか
		/// </summary>
		public bool IsStage3Available => StageFlag != null && StageFlag[2] != 0 && AirBattleData.api_stage3() && AirBattleData.api_stage3 != null;

		/// <summary>
		/// Stage3(航空攻撃)(対随伴艦隊)が存在するか
		/// </summary>
		public bool IsStage3CombinedAvailable => StageFlag != null && StageFlag[2] != 0 && AirBattleData.api_stage3_combined() && AirBattleData.api_stage3_combined != null;


		protected T[] ConcatStage3Array<T>(string friendName, string enemyName) where T : struct, IComparable
		{

			T[] ret = new T[24];

			void SetArray(string stage3Name, string sideName, int retIndex)
			{
				if (!AirBattleData[stage3Name].IsDefined(sideName))
					return;

				var ar = (dynamic[])AirBattleData[stage3Name][sideName];
				if (ar == null)
					return;

				for (int i = 0; i < ar.Length; i++)
				{
					var value = (T?)ar[i] ?? default(T);

					// Max(value, 0)
					if (value.CompareTo(default(T)) < 0)
						value = default(T);

					ret[retIndex + i] = value;
				}
			}


			if (IsStage3Available)
			{
				SetArray("api_stage3", friendName, 0);
				SetArray("api_stage3", enemyName, 12);
			}
			if (IsStage3CombinedAvailable)
			{
				SetArray("api_stage3_combined", friendName, 6);
				SetArray("api_stage3_combined", enemyName, 18);
			}

			return ret;
		}


		/// <summary>
		/// 被雷撃フラグ
		/// </summary>
		public int[] TorpedoFlags { get; protected set; }

		/// <summary>
		/// 被爆撃フラグ
		/// </summary>
		public int[] BomberFlags { get; protected set; }

		/// <summary>
		/// 各艦のクリティカルフラグ
		/// </summary>
		public int[] Criticals { get; protected set; }

		/// <summary>
		/// 各艦の被ダメージ
		/// </summary>
		public double[] Damages { get; protected set; }
	}
}
