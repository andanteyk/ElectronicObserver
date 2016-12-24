using ElectronicObserver.Data.Battle.Detail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase {

	/// <summary>
	/// 航空戦フェーズ処理の基底となるクラスです。
	/// </summary>
	public abstract class PhaseAirBattleBase : PhaseBase {

		public PhaseAirBattleBase( BattleData data, string title )
			: base( data, title ) {

		}

		public override bool IsAvailable {
			get { return StageFlag != null && StageFlag.Any( i => i != 0 ); }
		}



		/// <summary>
		/// 被ダメージ処理を行います。
		/// </summary>
		/// <param name="waveIndex">(基地航空隊の場合)現在の攻撃ウェーブのインデックス。それ以外の場合は 0</param>
		/// <param name="hps">現在のHPリスト。</param>
		protected void CalculateAttack( int waveIndex, int[] hps ) {
			for ( int i = 0; i < hps.Length; i++ ) {

				int attackType = ( TorpedoFlags[i] > 0 ? 1 : 0 ) | ( BomberFlags[i] > 0 ? 2 : 0 );
				if ( attackType > 0 ) {

					// 航空戦は miss/hit=0, critical=1 のため +1 する(通常は miss=0, hit=1, critical=2) 
					BattleDetails.Add( new BattleAirDetail( _battleData, waveIndex, i, Damages[i], Criticals[i] + 1, attackType, hps[i] ) );
					AddDamage( hps, i, Damages[i] );
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
		protected int[] GetLaunchedShipIndex( int index ) {
			if ( AirBattleData == null )
				return null;

			if ( !AirBattleData.api_plane_from() )
				return new int[0];

			dynamic data = AirBattleData.api_plane_from;

			if ( data == null || !data.IsArray )
				return new int[0];

			var planes = ( dynamic[] )data;
			if ( planes.Length > index ) {
				return ( (int[])planes[index] ).Where( i => i > 0 ).Select( i => i - 1 ).ToArray();
			}

			return new int[0];
		}


		// stage 1

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
		public int AirSuperiority { get { return !AirBattleData.api_stage1.api_disp_seiku() ? -1 : (int)AirBattleData.api_stage1.api_disp_seiku; } }

		/// <summary>
		/// 自軍触接機ID
		/// </summary>
		public int TouchAircraftFriend { get { return !AirBattleData.api_stage1.api_touch_plane() ? -1 : (int)AirBattleData.api_stage1.api_touch_plane[0]; } }

		/// <summary>
		/// 敵軍触接機ID
		/// </summary>
		public int TouchAircraftEnemy { get { return !AirBattleData.api_stage1.api_touch_plane() ? -1 : (int)AirBattleData.api_stage1.api_touch_plane[1]; } }


		// stage 2

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


		// stage 3

		/// <summary>
		/// Stage3(航空攻撃)が存在するか
		/// </summary>
		public bool IsStage3Available { get { return StageFlag != null && StageFlag[2] != 0 && AirBattleData.api_stage3() && AirBattleData.api_stage3 != null; } }

		/// <summary>
		/// Stage3(航空攻撃)(対随伴艦隊)が存在するか
		/// </summary>
		public bool IsStage3CombinedAvailable { get { return StageFlag != null && StageFlag[2] != 0 && AirBattleData.api_stage3_combined() && AirBattleData.api_stage3_combined != null; } }


		protected int[] ConcatStage3Array( string friendName, string enemyName ) {

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
				int[] friend = AirBattleData.api_stage3.IsDefined( friendName ) ? (int[])AirBattleData.api_stage3[friendName] : new int[7];
				int[] enemy = AirBattleData.api_stage3.IsDefined( enemyName ) ? (int[])AirBattleData.api_stage3[enemyName] : new int[7];

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
		public int[] Damages { get; protected set; }
	}
}
