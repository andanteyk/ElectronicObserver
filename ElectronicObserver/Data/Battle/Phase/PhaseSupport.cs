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


		public PhaseSupport( BattleData data )
			: base( data ) { }


		public override bool IsAvailable {
			get { return SupportFlag != 0; }
		}

		public override void EmulateBattle( int[] hps, int[] damages ) {

			if ( !IsAvailable ) return;
			
			if ( SupportFlag == 1 ) {
				//空撃
				int[] dmg = AirRaidDamages;

				for ( int i =0; i < 6; i++ ) {
					AddDamage( hps, i + 6, dmg[i] );
				}

			} else if ( SupportFlag == 2 || SupportFlag == 3 ) {
				//砲雷撃
				int[] dmg = ShellingTorpedoDamages;

				for ( int i =0; i < 6; i++ ) {
					AddDamage( hps, i + 6, dmg[i] );
				}
			}

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
		/// 航空支援ダメージ
		/// </summary>
		public int[] AirRaidDamages {
			get {
				if ( SupportFlag == 1 && (int)RawData.api_support_info.api_stage_flag[2] != 0 ) {

					return ( (int[])RawData.api_support_info.api_support_airatack.api_stage3.api_edam ).Skip( 1 ).ToArray();

				} else {
					return Enumerable.Repeat( 0, 6 ).ToArray();
				}
			}
		}

		/// <summary>
		/// 砲雷撃支援ダメージ
		/// </summary>
		public int[] ShellingTorpedoDamages {
			get {
				if ( ( SupportFlag == 2 || SupportFlag == 3 ) ) {

					return ( (int[])RawData.api_support_info.api_support_hourai.api_damage ).Skip( 1 ).ToArray();

				} else {
					return Enumerable.Repeat( 0, 6 ).ToArray();
				}
			}
		}

	}
}
