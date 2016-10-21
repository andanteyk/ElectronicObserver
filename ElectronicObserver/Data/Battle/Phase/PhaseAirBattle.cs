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

			TorpedoFlags = ConcatStage3Array( "api_frai_flag", "api_erai_flag" );
			BomberFlags = ConcatStage3Array( "api_fbak_flag", "api_ebak_flag" );
			Criticals = ConcatStage3Array( "api_fcl_flag", "api_ecl_flag" );
			Damages = ConcatStage3Array( "api_fdam", "api_edam" );

		}


		public override bool IsAvailable {
			get {
				int[] stageFlag = StageFlag;
				return StageFlag != null && !stageFlag.All( i => i == 0 );
			}
		}

		public override void EmulateBattle( int[] hps, int[] damages ) {

			if ( !IsAvailable ) return;


			for ( int i = 0; i < hps.Length; i++ ) {
				AddDamage( hps, i, Damages[i] );

				if ( TorpedoFlags[i] > 0 || BomberFlags[i] > 0 ) {

					// 航空戦は miss/hit=0, critical=1 のため +1 する(通常は miss=0, hit=1, critical=2) 
					BattleDetails.Add( new BattleAirDetail( _battleData, 0, i, Damages[i], Criticals[i] + 1, ( TorpedoFlags[i] > 0 ? 1 : 0 ) | ( BomberFlags[i] > 0 ? 2 : 0 ) ) );
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
			int totalDamage = Damages.Skip( 6 ).Take( 6 ).Sum() + damages.Skip( 18 ).Take( 6 ).Sum();

			for ( int i = 0; i < 6; i++ ) {
				damages[i] += (int)Math.Round( (double)totalDamage * firepower[i] / Math.Max( totalFirepower, 1 ) );
			}
		}

		protected override IEnumerable<BattleDetail> SearchBattleDetails( int index ) {
			return BattleDetails.Where( d => d.DefenderIndex == index );
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
		public bool IsStage1Available { get { return StageFlag != null && StageFlag[0] != 0 && AirBattleData.api_stage1() && AirBattleData.api_stage1 != null; } }


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
		public bool IsStage2Available { get { return StageFlag != null && StageFlag[1] != 0 && AirBattleData.api_stage2() && AirBattleData.api_stage2 != null; } }

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
					_battleData.Initial.FriendFleetEscort.MembersInstance[index - 6];
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
		public bool IsStage3Available { get { return StageFlag != null && StageFlag[2] != 0 && AirBattleData.api_stage3() && AirBattleData.api_stage3 != null; } }

		/// <summary>
		/// Stage3(航空攻撃)(対随伴艦隊)が存在するか
		/// </summary>
		public bool IsStage3CombinedAvailable { get { return StageFlag != null && StageFlag[2] != 0 && AirBattleData.api_stage3_combined() && AirBattleData.api_stage3_combined != null; } }


		private int[] ConcatStage3Array( string friendName, string enemyName ) {

			int[] ret = new int[24];

			if ( IsStage3CombinedAvailable ) {

				int[] friend = AirBattleData.api_stage3.IsDefined( friendName ) ? (int[])AirBattleData.api_stage3[friendName] : new int[7];
				int[] enemy = AirBattleData.api_stage3.IsDefined( enemyName ) ? (int[])AirBattleData.api_stage3[enemyName] : new int[7];
				int[] friendescort = AirBattleData.api_stage3_combined.IsDefined( friendName ) ? (int[])AirBattleData.api_stage3_combined[friendName] : new int[7];
				int[] enemyescort = AirBattleData.api_stage3_combined.IsDefined( enemyName ) ? (int[])AirBattleData.api_stage3_combined[enemyName] : new int[7];

				for ( int i = 0; i < 6; i++ ) {
					ret[i] = Math.Max( friend[i + 1], 0 );
					ret[i + 6] = Math.Max( enemy[i + 1], 0 );
					ret[i + 12] = Math.Max( friendescort[i + 1], 0 );
					ret[i + 18] = Math.Max( enemyescort[i + 1], 0 );
				}

			} else if ( IsStage3Available ) {
				int[] friend = (int[])AirBattleData.api_stage3[friendName];
				int[] enemy = (int[])AirBattleData.api_stage3[enemyName];

				for ( int i = 0; i < 6; i++ ) {
					ret[i] = Math.Max( friend[i + 1], 0 );
					ret[i + 6] = Math.Max( enemy[i + 1], 0 );
					ret[i + 12] = ret[i + 18] = 0;
				}

			}

			return ret;
		}


		/// <summary>
		/// 被雷撃フラグ
		/// </summary>
		public int[] TorpedoFlags { get; private set; }

		/// <summary>
		/// 被爆撃フラグ
		/// </summary>
		public int[] BomberFlags { get; private set; }

		/// <summary>
		/// 各艦のクリティカルフラグ
		/// </summary>
		public int[] Criticals { get; private set; }

		/// <summary>
		/// 各艦の被ダメージ
		/// </summary>
		public int[] Damages { get; private set; }

	}
}
