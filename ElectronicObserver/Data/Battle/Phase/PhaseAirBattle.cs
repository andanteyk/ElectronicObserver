using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase {

	public class PhaseAirBattle : PhaseBase {

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
			
			int[] dmg = Damages;

			for ( int i = 0; i < hps.Length; i++ ) {
				AddDamage( hps, i, dmg[i] );
			}

			//undone: 与ダメージ計算
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
