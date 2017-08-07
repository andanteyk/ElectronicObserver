using ElectronicObserver.Data.Battle.Detail;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase {

	/// <summary>
	/// 噴式航空機による基地航空隊攻撃フェーズの処理を行います。
	/// </summary>
	public class PhaseJetBaseAirAttack : PhaseBase {

		/// <summary>
		/// 噴式航空機による基地航空隊攻撃フェーズの、個々の攻撃フェーズの処理を行います。
		/// </summary>
		public class PhaseJetBaseAirAttackUnit : PhaseAirBattleBase {

			public PhaseJetBaseAirAttackUnit( BattleData data, string title, int index )
				: base( data, title ) {

				if ( index == -1 ) {
					AirAttackIndex = 0;
					AirBattleData = data.RawData.api_air_base_injection;
				} else {
					AirAttackIndex = index;
					AirBattleData = data.RawData.api_air_base_injection[index];
				}

				if ( AirBattleData != null ) {
					StageFlag = new int[] { 
						AirBattleData.api_stage1() ? 1 : 0,
						AirBattleData.api_stage2() ? 1 : 0,
						AirBattleData.api_stage3() ? 1 : 0,
					};
				}

				_squadrons = GetSquadrons().ToArray();

				TorpedoFlags = ConcatStage3Array<int>( "api_frai_flag", "api_erai_flag" );
				BomberFlags = ConcatStage3Array<int>( "api_fbak_flag", "api_ebak_flag" );
				Criticals = ConcatStage3Array<int>( "api_fcl_flag", "api_ecl_flag" );
				Damages = ConcatStage3Array<double>( "api_fdam", "api_edam" );
			}


			public override void EmulateBattle( int[] hps, int[] damages ) {

				if ( !IsAvailable ) return;

				CalculateAttack( AirAttackIndex + 1, hps );
			}


			/// <summary>
			/// 攻撃ID (第n波, 0から始まる)
			/// </summary>
			public int AirAttackIndex { get; private set; }

			
			private BattleBaseAirCorpsSquadron[] _squadrons;
			/// <summary>
			/// 参加した航空中隊データ
			/// </summary>
			public ReadOnlyCollection<BattleBaseAirCorpsSquadron> Squadrons {
				get { return Array.AsReadOnly( _squadrons ); }
			}

			private IEnumerable<BattleBaseAirCorpsSquadron> GetSquadrons() {
				foreach ( dynamic d in AirBattleData.api_air_base_data )
					yield return new BattleBaseAirCorpsSquadron( d );
			}

		}



		public PhaseJetBaseAirAttack( BattleData data, string title )
			: base( data, title ) {

			AirAttackUnits = new List<PhaseJetBaseAirAttackUnit>();

			if ( !IsAvailable )
				return;


			dynamic attackData = RawData.api_air_base_injection;
			if ( attackData.IsArray ) {
				int i = 0;
				foreach ( var unit in RawData.api_air_base_injection ) {
					AirAttackUnits.Add( new PhaseJetBaseAirAttackUnit( data, title, i ) );
					i++;
				}

			} else if ( attackData.IsObject ) {
				AirAttackUnits.Add( new PhaseJetBaseAirAttackUnit( data, title, -1 ) );
			}
		}


		/// <summary>
		/// 個々の攻撃フェーズのデータ
		/// </summary>
		public List<PhaseJetBaseAirAttackUnit> AirAttackUnits { get; private set; }



		public override bool IsAvailable {
			get { return RawData.api_air_base_injection(); }
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
