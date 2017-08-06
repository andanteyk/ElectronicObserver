using ElectronicObserver.Data.Battle.Detail;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		public class PhaseBaseAirAttackUnit : PhaseAirBattleBase {


			public PhaseBaseAirAttackUnit( BattleData data, string title, int index )
				: base( data, title ) {

				AirAttackIndex = index;
				AirBattleData = data.RawData.api_air_base_attack[index];
				StageFlag = AirBattleData.api_stage_flag() ? (int[])AirBattleData.api_stage_flag : null;

				LaunchedShipIndexEnemy = GetLaunchedShipIndex( 0 );

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

			/// <summary>
			/// 航空隊ID
			/// </summary>
			public int AirUnitID {
				get {
					return (int)AirBattleData.api_base_id;
				}
			}


			private BattleBaseAirCorpsSquadron[] _squadrons;
			/// <summary>
			/// 参加した航空中隊データ
			/// </summary>
			public ReadOnlyCollection<BattleBaseAirCorpsSquadron> Squadrons {
				get { return Array.AsReadOnly( _squadrons ); }
			}

			private IEnumerable<BattleBaseAirCorpsSquadron> GetSquadrons() {
				foreach ( dynamic d in AirBattleData.api_squadron_plane )
					yield return new BattleBaseAirCorpsSquadron( d );
			}

		}



		public PhaseBaseAirAttack( BattleData data, string title )
			: base( data, title ) {

			AirAttackUnits = new List<PhaseBaseAirAttackUnit>();

			if ( !IsAvailable )
				return;


			int i = 0;
			foreach ( var unit in RawData.api_air_base_attack ) {
				AirAttackUnits.Add( new PhaseBaseAirAttackUnit( data, title, i ) );
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


	/// <summary>
	/// 戦闘に参加した基地航空隊中隊のデータ
	/// </summary>
	public class BattleBaseAirCorpsSquadron {
		public int EquipmentID { get; private set; }
		public int AircraftCount { get; private set; }
		public EquipmentDataMaster EquipmentInstance { get { return KCDatabase.Instance.MasterEquipments[EquipmentID]; } }

		public BattleBaseAirCorpsSquadron( dynamic data ) {
			EquipmentID = (int)data.api_mst_id;
			AircraftCount = (int)data.api_count;
		}

		public override string ToString() {
			return string.Format( "{0} x {1}", EquipmentInstance != null ? EquipmentInstance.Name : "未確認飛行物体", AircraftCount );
		}
	}

}
