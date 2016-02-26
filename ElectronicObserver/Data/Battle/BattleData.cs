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
		/// <summary>
		/// 戦闘終了時の各艦のHP
		/// </summary>
		public ReadOnlyCollection<int> ResultHPs { get { return Array.AsReadOnly( _resultHPs ); } }

		protected int[] _attackDamages;
		/// <summary>
		/// 各艦の与ダメージ
		/// </summary>
		public ReadOnlyCollection<int> AttackDamages { get { return Array.AsReadOnly( _attackDamages ); } }

		protected int[] _attackAirDamages;
		/// <summary>
		/// 各艦の航空戦の与ダメージ
		/// </summary>
		public ReadOnlyCollection<int> AttackAirDamages { get { return Array.AsReadOnly( _attackAirDamages ); } }


		public PhaseInitial Initial { get; protected set; }
		public PhaseSearching Searching { get; protected set; }


		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );

			Initial = new PhaseInitial( this );
			Searching = new PhaseSearching( this );

			_resultHPs = Initial.InitialHPs.ToArray();
			if ( _attackDamages == null )
				_attackDamages = new int[_resultHPs.Length];
			if ( _attackAirDamages == null )
				_attackAirDamages = new int[_resultHPs.Length];

		}


		/// <summary>
		/// MVP 取得候補艦のインデックス [0-5]
		/// </summary>
		public IEnumerable<int> MVPShipIndexes {
			get {
				int max = _attackDamages.Take( 6 ).Zip( _attackAirDamages.Take( 6 ), ( dmg, air ) => ( dmg + air ) ).Max();
				if ( max == 0 ) {		// 全員ノーダメージなら旗艦MVP
					yield return 0;

				} else {
					for ( int i = 0; i < 6; i++ ) {
						if ( _attackDamages[i] + _attackAirDamages[i] == max )
							yield return i;
					}
				}
				//return index < 0 ? 0 : index;
			}
		}


		/// <summary>
		/// 連合艦隊随伴艦隊の MVP 取得候補艦のインデックス [0-5]
		/// </summary>
		public IEnumerable<int> MVPShipCombinedIndexes {
			get {
				int max = _attackDamages.Skip( 12 ).Take( 6 ).Zip( _attackAirDamages.Skip( 12 ).Take( 6 ), ( dmg, air ) => ( dmg + air ) ).Max();
				if ( max == 0 ) {		// 全員ノーダメージなら旗艦MVP
					yield return 0;

				} else {
					for ( int i = 0; i < 6; i++ ) {
						if ( _attackDamages[i + 12] + _attackAirDamages[i + 12] == max )
							yield return i;
					}
				}
				//return index < 0 ? 0 : index;
			}
		}


		/// <summary>
		/// 前回の戦闘データからパラメータを引き継ぎます。
		/// </summary>
		internal void TakeOverParameters( BattleData prev ) {
			_attackDamages = (int[])prev._attackDamages.Clone();
			_attackAirDamages = (int[])prev._attackAirDamages.Clone();
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
