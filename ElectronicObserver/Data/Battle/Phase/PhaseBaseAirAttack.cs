using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codeplex.Data;

namespace ElectronicObserver.Data.Battle.Phase {

	/// <summary>
	/// 基地航空隊攻撃フェーズの処理を行います。
	/// </summary>
	public class PhaseBaseAirAttack : PhaseBase {


        public class SquadronPlane
        {
            public int Plane { get; set; }
            public int Count { get; set; }

            public SquadronPlane(int plane, int count)
            {
                Plane = plane;
                Count = count;
            }
        }
        /// <summary>
        /// 基地航空隊攻撃フェーズの、個々の攻撃フェーズの処理を行います。
        /// </summary>
        public class PhaseBaseAirAttackUnit : PhaseBase {


			public PhaseBaseAirAttackUnit( BattleData data, int index )
				: base( data ) {
				AirBattleData = data.RawData.api_air_base_attack[index];
                int count = ((DynamicJson)AirBattleData.api_squadron_plane).GetCount();
                SquadronPlane = new PhaseBaseAirAttack.SquadronPlane[count];
                for (int i = 0; i < count; i++)
                {
                    SquadronPlane[i] = new PhaseBaseAirAttack.SquadronPlane((int)AirBattleData.api_squadron_plane[i].api_mst_id, (int)AirBattleData.api_squadron_plane[i].api_count);
                }
            }

            public SquadronPlane[] SquadronPlane
            {
                get;
                set;
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

			}


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
			/// 各艦の被ダメージ
			/// </summary>
			public int[] Damages {
				get {
					// 敵のみです

					int[] ret =　new int[IsCombined ? 18 : 12];
					int[] enemy = (int[])AirBattleData.api_stage3.api_edam;

					for ( int i = 0; i < 6; i++ ) {
						ret[i + 6] = Math.Max( enemy[i + 1], 0 );
					}

					return ret;
				}
			}

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
	}
}
