using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase {

	/// <summary>
	/// 基地航空隊攻撃フェーズの処理を行います。
	/// </summary>
	public class PhaseBaseAirAttack : PhaseBase {


		/// <summary>
		/// 基地航空隊攻撃フェーズの、個々の攻撃フェーズの処理を行います。
		/// </summary>
		public class PhaseBaseAirAttackUnit : PhaseBase {


			public PhaseBaseAirAttackUnit( BattleData data, int index )
				: base( data ) {

				AirAttackIndex = index;
				AirBattleData = data.RawData.api_air_base_attack[index];

				TorpedoFlags = ConcatStage3Array( "api_erai_flag" );
				BomberFlags = ConcatStage3Array( "api_ebak_flag" );
				Criticals = ConcatStage3Array( "api_ecl_flag" );
				Damages = ConcatStage3Array( "api_edam" );

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
						BattleDetails.Add( new BattleAirDetail( _battleData, AirAttackIndex + 1, i, Damages[i], Criticals[i] + 1, ( TorpedoFlags[i] > 0 ? 1 : 0 ) | ( BomberFlags[i] > 0 ? 2 : 0 ) ) );
					}
				}

			}

			protected override IEnumerable<BattleDetail> SearchBattleDetails( int index ) {
				return BattleDetails.Where( d => d.DefenderIndex == index );
			}


			public int AirAttackIndex { get; private set; }

			/// <summary>
			/// 各Stageが存在するか
			/// </summary>
			public int[] StageFlag {
				get {
					return AirBattleData.IsDefined( "api_stage_flag" ) ? (int[])AirBattleData["api_stage_flag"] : null;
				}
			}

			/// <summary>
			/// 航空戦の生データ
			/// </summary>
			public dynamic AirBattleData { get; set; }


			/// <summary>
			/// 航空隊ID
			/// </summary>
			public int AirUnitID {
				get {
					return (int)AirBattleData.api_base_id;
				}
			}


			/*/
			public class SquadronData {
				public int EquipmentID;
				public int AircraftCount;

				public SquadronData( dynamic data ) {
					EquipmentID = (int)data.api_mst_id;
					AircraftCount = (int)data.api_count;
				}
			}

			public IEnumerable<SquadronData> Squadrons {
				get {
					foreach ( dynamic d in AirBattleData.api_squadron_plane )
						yield return new SquadronData( d );
				}
			}
			//*/


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



			//stage 3

			/// <summary>
			/// Stage3(航空攻撃)が存在するか
			/// </summary>
			public bool IsStage3Available { get { return StageFlag != null && StageFlag[2] != 0 && AirBattleData.api_stage3() && AirBattleData.api_stage3 != null; } }

			/// <summary>
			/// Stage3(航空攻撃)(対随伴艦隊)が存在するか
			/// </summary>
			public bool IsStage3CombinedAvailable { get { return StageFlag != null && StageFlag[2] != 0 && AirBattleData.api_stage3_combined() && AirBattleData.api_stage3_combined != null; } }


			private int[] ConcatStage3Array( string enemyName ) {

				int[] ret = new int[24];

				if ( IsStage3CombinedAvailable ) {

					int[] enemy = (int[])AirBattleData.api_stage3[enemyName];
					int[] enemyescort = (int[])AirBattleData.api_stage3_combined[enemyName];

					for ( int i = 0; i < 6; i++ ) {
						ret[i + 6] = Math.Max( enemy[i + 1], 0 );
						ret[i + 18] = Math.Max( enemyescort[i + 1], 0 );
						ret[i] = ret[i + 12] = 0;
					}

				} else if ( IsStage3Available ) {
					int[] enemy = (int[])AirBattleData.api_stage3[enemyName];

					for ( int i = 0; i < 6; i++ ) {
						ret[i + 6] = Math.Max( enemy[i + 1], 0 );
						ret[i] = ret[i + 12] = ret[i + 18] = 0;
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



		public PhaseBaseAirAttack( BattleData data )
			: base( data ) {

			AirAttackUnits = new List<PhaseBaseAirAttackUnit>();

			if ( !IsAvailable )
				return;


			int i = 0;
			foreach ( var unit in RawData.api_air_base_attack ) {
				AirAttackUnits.Add( new PhaseBaseAirAttackUnit( data, i ) );
				i++;
			}

		}


		/// <summary>
		/// 個々の攻撃フェーズのデータ
		/// </summary>
		public List<PhaseBaseAirAttackUnit> AirAttackUnits { get; private set; }



		public override bool IsAvailable {
			get { return RawData.api_air_base_attack(); }
		}


		public override void EmulateBattle( int[] hps, int[] damages ) {

			if ( !IsAvailable )
				return;

			foreach ( var a in AirAttackUnits ) {

				a.EmulateBattle( hps, damages );
			}
		}

		protected override IEnumerable<BattleDetail> SearchBattleDetails( int index ) {
			return AirAttackUnits.SelectMany( p => p.BattleDetails ).Where( d => d.DefenderIndex == index );
		}


	}
}
