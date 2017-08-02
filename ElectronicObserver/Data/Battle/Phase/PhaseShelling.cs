using ElectronicObserver.Data.Battle.Detail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase {

	/// <summary>
	/// 砲撃戦フェーズの処理を行います。
	/// </summary>
	public class PhaseShelling : PhaseBase {

		protected readonly int phaseID;
		protected readonly string suffix;
		protected readonly bool isEscort;
		protected readonly bool isEnemyEscort;

		public List<PhaseShellingAttack> Attacks { get; private set; }


		public class PhaseShellingAttack {
			public int Attacker;
			public int AttackType;
			public List<PhaseShellingDefender> Defenders;
			public int[] EquipmentIDs;

			public PhaseShellingAttack() { }

			public override string ToString() {
				return string.Format( "{0}[{1}] -> [{2}]", Attacker, AttackType, string.Join( ", ", Defenders ) );
			}
		}
		public class PhaseShellingDefender {
			public int Defender;
			public int CriticalFlag;
			public double RawDamage;
			public bool GuardsFlagship { get { return RawDamage != Math.Floor( RawDamage ); } }
			public int Damage { get { return (int)RawDamage; } }

			public override string ToString() {
				return string.Format( "{0};{1}-{2}{3}", Defender, Damage,
					CriticalFlag == 0 ? "miss" : CriticalFlag == 1 ? "dmg" : CriticalFlag == 2 ? "crit" : "INVALID",
					GuardsFlagship ? " (guard)" : "" );
			}
		}



		public PhaseShelling( BattleData data, string title, int phaseID, string suffix, bool isEscort, bool isEnemyEscort = false )
			: base( data, title ) {

			this.phaseID = phaseID;
			this.suffix = suffix;
			this.isEscort = isEscort;
			this.isEnemyEscort = isEnemyEscort;

			if ( !IsAvailable )
				return;

			// "translate"

			int[] fleetflag = !ShellingData.api_at_eflag() ? null : ( (int[])ShellingData.api_at_eflag ).Skip( 1 ).ToArray();
			int[] attackers = ( (int[])ShellingData.api_at_list ).Skip( 1 ).ToArray();
			int[] attackTypes = ( (int[])ShellingData.api_at_type ).Skip( 1 ).ToArray();
			int[][] defenders = ( (dynamic[])ShellingData.api_df_list ).Skip( 1 ).Select( elem => (int[])elem ).ToArray();
			int[][] attackEquipments = ( (dynamic[])ShellingData.api_si_list ).Skip( 1 ).Select( elem => ((dynamic[])elem).Select<dynamic, int>( ch => ch is string ? int.Parse( ch ) : (int)ch ).ToArray() ).ToArray();
			int[][] criticalFlags = ( (dynamic[])ShellingData.api_cl_list ).Skip( 1 ).Select( elem => (int[])elem ).ToArray();
			double[][] rawDamages = ( (dynamic[])ShellingData.api_damage ).Skip( 1 ).Select( elem => ( (double[])elem ).Select( p => Math.Max( p, 0 ) ).ToArray() ).ToArray();
			
			Attacks = new List<PhaseShellingAttack>();

			for ( int i = 0; i < attackers.Length; i++ ) {
				var attack = new PhaseShellingAttack();

				attack.Attacker = attackers[i] - 1;
				attack.Defenders = new List<PhaseShellingDefender>();


				if ( fleetflag != null ) {

					if ( fleetflag[i] == 1 ) {	// enemy
						attack.Attacker += 6;
						if ( isEnemyEscort )
							attack.Attacker += 6;

					} else if ( isEscort ) {	// friend escort
						attack.Attacker += 6;
					}

					for ( int k = 0; k < defenders[i].Length; k++ ) {

						var defender = new PhaseShellingDefender();
						defender.Defender = defenders[i][k] - 1;

						if ( defender.Defender >= 6 ) // escort
							defender.Defender += 6;
						if ( fleetflag[i] == 0 ) // friend -> *enemy*
							defender.Defender += 6;

						defender.CriticalFlag = criticalFlags[i][k];
						defender.RawDamage = rawDamages[i][k];
						
						attack.Defenders.Add( defender );
					}

				} else if ( isEscort ) {

					if ( attack.Attacker < 6 )	// friend
						attack.Attacker += 12;

					for ( int k = 0; k < defenders[i].Length; k++ ) {

						var defender = new PhaseShellingDefender();
						defender.Defender = defenders[i][k] - 1;

						if ( PhaseBase.IsIndexEnemy( attack.Attacker ) )	// enemy -> *friend escort*
							defender.Defender += 12;

						defender.CriticalFlag = criticalFlags[i][k];
						defender.RawDamage = rawDamages[i][k];

						attack.Defenders.Add( defender );
					}

				} else {

					for ( int k = 0; k < defenders[i].Length; k++ ) {

						var defender = new PhaseShellingDefender();

						defender.Defender = defenders[i][k] - 1;
						defender.CriticalFlag = criticalFlags[i][k];
						defender.RawDamage = rawDamages[i][k];
						
						attack.Defenders.Add( defender );
					}

				}

				attack.AttackType = attackTypes[i];
				attack.EquipmentIDs = attackEquipments[i];

				Attacks.Add( attack );
			}

		}


		public override bool IsAvailable {
			get { return (int)RawData.api_hourai_flag[phaseID - 1] != 0; }
		}


		public virtual dynamic ShellingData {
			get { return RawData["api_hougeki" + suffix]; }
		}


		public override void EmulateBattle( int[] hps, int[] damages ) {

			if ( !IsAvailable ) return;


			foreach ( var attack in Attacks ) {

				foreach ( var defs in attack.Defenders.GroupBy( d => d.Defender ) ) {
					BattleDetails.Add( new BattleDayDetail( _battleData, attack.Attacker, defs.Key, defs.Select( d => d.RawDamage ).ToArray(), defs.Select( d => d.CriticalFlag ).ToArray(), attack.AttackType, attack.EquipmentIDs, hps[defs.Key] ) );
					AddDamage( hps, defs.Key, defs.Sum( d => d.Damage ) );
				}

				damages[attack.Attacker] += attack.Defenders.Sum( d => d.Damage );
			}

		}

	}
}
