using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {

	/// <summary>
	/// 戦闘情報を保持するデータの基底です。
	/// </summary>
	public abstract class BattleData : ResponseWrapper {

		protected int[] _resultHPs;
		public ReadOnlyCollection<int> ResultHPs { get { return Array.AsReadOnly( _resultHPs ); } }

		protected int[] _attackDamages;
		public ReadOnlyCollection<int> AttackDamages { get { return Array.AsReadOnly( _attackDamages ); } }


		public PhaseInitial Initial { get; protected set; }
		public PhaseSearching Searching { get; protected set; }


		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );

			int length = (BattleType & BattleTypeFlag.Combined ) != 0 ? 18 : 12;
			_resultHPs = new int[length];
			_attackDamages = new int[length];

			Initial = new PhaseInitial( this );
			Searching = new PhaseSearching( this );
		}


		/// <summary>
		/// 対応しているAPIの名前を取得します。
		/// </summary>
		public abstract string APIName { get; }


		[Flags]
		public enum BattleTypeFlag {
			Undefined = 0,
			Day,
			Night,
			Practice = 0x1000,
			Combined = 0x2000,
		}

		/// <summary>
		/// 戦闘モード
		/// </summary>
		public abstract BattleTypeFlag BattleType { get; }

	}

}
