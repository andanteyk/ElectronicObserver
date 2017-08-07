using ElectronicObserver.Data.Battle.Detail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase {

	/// <summary>
	/// 支援攻撃フェーズの処理を行います。
	/// </summary>
	public class PhaseSupport : PhaseBase {


		public PhaseSupport( BattleData data, string title )
			: base( data, title ) {

			switch ( SupportFlag ) {
				case 1:		// 空撃
					{
						if ( (int)RawData.api_support_info.api_support_airatack.api_stage_flag[2] != 0 ) {

							// 敵連合でも api_stage3_combined は存在せず、[13] になる

							Damages = ( (double[])RawData.api_support_info.api_support_airatack.api_stage3.api_edam ).Skip( 1 ).ToArray();
							Criticals = ( (int[])RawData.api_support_info.api_support_airatack.api_stage3.api_ecl_flag ).Skip( 1 ).ToArray();

							// 航空戦なので crit フラグが違う
							for ( int i = 0; i < Criticals.Length; i++ )
								Criticals[i]++;

						} else {
							goto default;
						}
					} break;
				case 2:		// 砲撃
				case 3:		// 雷撃
					{
						var dmg = ( (double[])RawData.api_support_info.api_support_hourai.api_damage ).Skip( 1 );
						var cl = ( (int[])RawData.api_support_info.api_support_hourai.api_cl_list ).Skip( 1 );

						if ( dmg.Count() == 12 )
							Damages = dmg.ToArray();
						else if ( dmg.Count() == 6 )
							Damages = dmg.Concat( Enumerable.Repeat( 0.0, 6 ) ).ToArray();


						if ( cl.Count() == 12 )
							Criticals = cl.ToArray();
						else if ( cl.Count() == 6 )
							Criticals = cl.Concat( Enumerable.Repeat( 0, 6 ) ).ToArray();

					} break;
				default:
					Damages = new double[12];
					Criticals = new int[12];
					break;
			}
		}


		public override bool IsAvailable {
			get { return SupportFlag != 0; }
		}

		public override void EmulateBattle( int[] hps, int[] damages ) {

			if ( !IsAvailable ) return;

			for ( int i = 0; i < 6; i++ ) {
				if ( _battleData.Initial.EnemyMembers[i] > 0 ) {
					BattleDetails.Add( new BattleSupportDetail( _battleData, i + 6, Damages[i], Criticals[i], SupportFlag, hps[i + 6] ) );
					AddDamage( hps, i + 6, (int)Damages[i] );
				}
			}
			if ( ( _battleData.BattleType & BattleData.BattleTypeFlag.EnemyCombined ) != 0 ) {
				for ( int i = 0; i < 6; i++ ) {
					if ( _battleData.Initial.EnemyMembersEscort[i] > 0 ) {
						BattleDetails.Add( new BattleSupportDetail( _battleData, i + 18, Damages[i + 6], Criticals[i + 6], SupportFlag, hps[i + 18] ) );
						AddDamage( hps, i + 18, (int)Damages[i + 6] );
					}
				}
			}
		}

		protected override IEnumerable<BattleDetail> SearchBattleDetails( int index ) {
			return BattleDetails.Where( d => d.DefenderIndex == index );
		}


		/// <summary>
		/// 支援艦隊フラグ
		/// </summary>
		public int SupportFlag { get { return RawData.api_support_flag() ? (int)RawData.api_support_flag : 0; } }

		/// <summary>
		/// 支援艦隊ID
		/// </summary>
		public int SupportFleetID {
			get {
				if ( SupportFlag == 1 )
					return (int)RawData.api_support_info.api_support_airatack.api_deck_id;
				else if ( SupportFlag == 2 || SupportFlag == 3 )
					return (int)RawData.api_support_info.api_support_hourai.api_deck_id;
				else
					return -1;
			}
		}

		/// <summary>
		/// 支援艦隊
		/// </summary>
		public FleetData SupportFleet {
			get {
				int id = SupportFleetID;
				if ( id != -1 )
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
