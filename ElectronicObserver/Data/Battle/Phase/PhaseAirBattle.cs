using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase {

	/// <summary>
	/// 航空戦フェーズの処理を行います。
	/// </summary>
	public class PhaseAirBattle : PhaseBase {

		/// <summary>
		/// API データの接尾辞(第二次航空戦用)
		/// </summary>
		protected readonly string suffix;


		public PhaseAirBattle( BattleData data, string suffix = "" )
			: base( data ) {

			this.suffix = suffix;
		}


		public override bool IsAvailable {
			get {
				int[] stageFlag = StageFlag;
				return StageFlag != null && !stageFlag.All( i => i == 0 );
			}
		}

		public override void EmulateBattle( int[] hps, int[] damages ) {

			if ( !IsAvailable || !IsStage3Available ) return;

			{
				int[] dmg = Damages;

				for ( int i = 0; i < hps.Length; i++ ) {
					AddDamage( hps, i, dmg[i] );
				}
			}

			CalculateAttackDamage( damages );

		}

		/// <summary>
		/// 航空戦での与ダメージを推測します。
		/// </summary>
		/// <param name="damages">与ダメージリスト。</param>
		private void CalculateAttackDamage( int[] damages ) {
			// 敵はめんどくさすぎるので省略
			// 仮想火力を求め、それに従って合計ダメージを分配

			var firepower = new int[6];
			var members = _battleData.Initial.FriendFleet.MembersWithoutEscaped;

			for ( int i = 0; i < members.Count; i++ ) {
				var ship = members[i];
				if ( ship == null ) continue;

				var slots = ship.SlotInstanceMaster;
				var aircrafts = ship.Aircraft;
				for ( int s = 0; s < slots.Count; s++ ) {

					if ( slots[s] == null ) continue;

					switch ( slots[s].CategoryType ) {
						case 7:		//艦上爆撃機
						case 11:	//水上爆撃機
							firepower[i] += (int)( 1.0 * ( slots[s].Bomber * Math.Sqrt( aircrafts[s] ) + 25 ) );
							break;

						case 8:		//艦上攻撃機 (80%と150%はランダムのため係数は平均値)
							firepower[i] += (int)( 1.15 * ( slots[s].Torpedo * Math.Sqrt( aircrafts[s] ) + 25 ) );
							break;
					}

				}

			}

			int totalFirepower = firepower.Sum();
			int totalDamage = Damages.Sum();

			for ( int i = 0; i < 6; i++ ) {
				damages[i] += (int)( (double)totalDamage * firepower[i] / Math.Max( totalFirepower, 1 ) );
			}
		}


		/// <summary>
		/// 各Stageが存在するか
		/// </summary>
		public int[] StageFlag {
			get {
				return RawData.IsDefined( "api_stage_flag" + suffix ) ? (int[])RawData["api_stage_flag" + suffix] : null;
			}
		}

		/// <summary>
		/// 航空戦の生データ
		/// </summary>
		public dynamic AirBattleData { get { return RawData["api_kouku" + suffix]; } }


		//stage 1

		/// <summary>
		/// Stage1(空対空戦闘)が存在するか
		/// </summary>
		public bool IsStage1Available { get { return StageFlag != null && StageFlag[0] != 0; } }


		/// <summary>
		/// 自軍Stage1参加機数
		/// </summary>
		public int AircraftTotalStage1Friend { get { return (int)AirBattleData.api_stage1.api_f_count; } }

		/// <summary>
		/// 敵軍Stage1参加機数
		/// </summary>
		public int AircraftTotalStage1Enemy { get { return (int)AirBattleData.api_stage1.api_e_count; } }

		/// <summary>
		/// 自軍Stage1撃墜機数
		/// </summary>
		public int AircraftLostStage1Friend { get { return (int)AirBattleData.api_stage1.api_f_lostcount; } }

		/// <summary>
		/// 敵軍Stage1撃墜機数
		/// </summary>
		public int AircraftLostStage1Enemy { get { return (int)AirBattleData.api_stage1.api_e_lostcount; } }

		/// <summary>
		/// 制空権
		/// </summary>
		public int AirSuperiority { get { return (int)AirBattleData.api_stage1.api_disp_seiku; } }

		/// <summary>
		/// 自軍触接機ID
		/// </summary>
		public int TouchAircraftFriend { get { return (int)AirBattleData.api_stage1.api_touch_plane[0]; } }

		/// <summary>
		/// 敵軍触接機ID
		/// </summary>
		public int TouchAircraftEnemy { get { return (int)AirBattleData.api_stage1.api_touch_plane[1]; } }


		//stage 2

		/// <summary>
		/// Stage2(艦対空戦闘)が存在するか
		/// </summary>
		public bool IsStage2Available { get { return StageFlag != null && StageFlag[1] != 0; } }

		/// <summary>
		/// 自軍Stage2参加機数
		/// </summary>
		public int AircraftTotalStage2Friend { get { return (int)AirBattleData.api_stage2.api_f_count; } }

		/// <summary>
		/// 敵軍Stage2参加機数
		/// </summary>
		public int AircraftTotalStage2Enemy { get { return (int)AirBattleData.api_stage2.api_e_count; } }

		/// <summary>
		/// 自軍Stage2撃墜機数
		/// </summary>
		public int AircraftLostStage2Friend { get { return (int)AirBattleData.api_stage2.api_f_lostcount; } }

		/// <summary>
		/// 敵軍Stage2撃墜機数
		/// </summary>
		public int AircraftLostStage2Enemy { get { return (int)AirBattleData.api_stage2.api_e_lostcount; } }


		/// <summary>
		/// 対空カットインが発動したか
		/// </summary>
		public bool IsAACutinAvailable { get { return AirBattleData.api_stage2.api_air_fire(); } }

		/// <summary>
		/// 対空カットイン発動艦番号
		/// </summary>
		public int AACutInIndex { get { return (int)AirBattleData.api_stage2.api_air_fire.api_idx; } }

		/// <summary>
		/// 対空カットイン発動艦
		/// </summary>
		public ShipData AACutInShip {
			get {
				int index = AACutInIndex;
				return index < 6 ?
					_battleData.Initial.FriendFleet.MembersInstance[index] :
					KCDatabase.Instance.Fleet[2].MembersInstance[index - 6];
			}
		}

		/// <summary>
		/// 対空カットイン種別
		/// </summary>
		public int AACutInKind { get { return (int)AirBattleData.api_stage2.api_air_fire.api_kind; } }


		//stage 3

		/// <summary>
		/// Stage3(航空攻撃)が存在するか
		/// </summary>
		public bool IsStage3Available { get { return StageFlag != null && StageFlag[2] != 0; } }

		/// <summary>
		/// 各艦の被ダメージ
		/// </summary>
		public int[] Damages {
			get {
				if ( AirBattleData.api_stage3_combined() ) {
					return ( (int[])AirBattleData.api_stage3.api_fdam ).Skip( 1 )
						.Concat( ( (int[])AirBattleData.api_stage3.api_edam ).Skip( 1 ) )
						.Concat( ( (int[])AirBattleData.api_stage3_combined.api_fdam ).Skip( 1 ) )
						.ToArray();
				} else {
					return ( (int[])AirBattleData.api_stage3.api_fdam ).Skip( 1 )
						.Concat( ( (int[])AirBattleData.api_stage3.api_edam ).Skip( 1 ) )
						.ToArray();
				}
			}
		}

	}
}
