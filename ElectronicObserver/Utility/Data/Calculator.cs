using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility.Data {

	/// <summary>
	/// 汎用計算クラス
	/// </summary>
	public static class Calculator {

		/// <summary>
		/// レベルに依存するパラメータ値を求めます。
		/// </summary>
		/// <param name="min">初期値。</param>
		/// <param name="max">最大値。</param>
		/// <param name="lv">レベル。</param>
		/// <returns></returns>
		public static int GetParameterFromLevel( int min, int max, int lv ) {
			return min + ( max - min ) * lv / 99;
		}



		/// <summary>
		/// 制空戦力を求めます。
		/// </summary>
		/// <param name="slot">装備スロット。</param>
		/// <param name="aircraft">艦載機搭載量の配列。</param>
		public static int GetAirSuperiority( int[] slot, int[] aircraft ) {

			int air = 0;
			int length = Math.Min( slot.Length, aircraft.Length );

			for ( int s = 0; s < length; s++ ) {

				EquipmentDataMaster eq = KCDatabase.Instance.MasterEquipments[slot[s]];

				if ( eq == null ) continue;

				switch ( eq.EquipmentType[2] ) {
					case 6:
					case 7:
					case 8:
					case 11:
						air += (int)( eq.AA * Math.Sqrt( aircraft[s] ) );
						break;
				}
			}

			return air;
		}

		/// <summary>
		/// 制空戦力を求めます。
		/// </summary>
		/// <param name="fleet">艦船IDの配列。</param>
		public static int GetAirSuperiority( int[] fleet ) {

			int air = 0;

			for ( int i = 0; i < fleet.Length; i++ ) {
				ShipDataMaster ship = KCDatabase.Instance.MasterShips[fleet[i]];
				if ( ship == null ) continue;

				air += GetAirSuperiority( ship );
			}

			return air;
		}

		/// <summary>
		/// 制空戦力を求めます。
		/// </summary>
		/// <param name="fleet">艦船IDの配列。</param>
		/// <param name="slot">各艦船の装備スロット。</param>
		public static int GetAirSuperiority( int[] fleet, int[][] slot ) {

			int air = 0;
			int length = Math.Min( fleet.Length, slot.GetLength( 0 ) );

			for ( int i = 0; i < length; i++ ) {
				ShipDataMaster ship = KCDatabase.Instance.MasterShips[fleet[i]];
				if ( ship == null ) continue;

				air += GetAirSuperiority( slot[i], ship.Aircraft.ToArray() );

			}

			return air;
		}

		/// <summary>
		/// 制空戦力を求めます。
		/// </summary>
		/// <param name="ship">対象の艦船。</param>
		public static int GetAirSuperiority( ShipData ship ) {

			return GetAirSuperiority( ship.Slot.ToArray(), ship.Aircraft.ToArray() );
	
		}

		/// <summary>
		/// 制空戦力を求めます。
		/// </summary>
		/// <param name="ship">対象の艦船。</param>
		public static int GetAirSuperiority( ShipDataMaster ship ) {

			if ( ship.DefaultSlot == null ) return 0;
			return GetAirSuperiority( ship.DefaultSlot.ToArray(), ship.Aircraft.ToArray() );

		}

		/// <summary>
		/// 制空戦力を求めます。
		/// </summary>
		/// <param name="fleet">対象の艦隊。</param>
		public static int GetAirSuperiority( FleetData fleet ) {

			int air = 0;

			foreach ( var ship in fleet.MembersInstance ) {
				if ( ship == null ) continue;

				air += GetAirSuperiority( ship );
			}

			return air;
		}



		public static int GetDayAttackKind( int shipID, int[] slot ) {

			//undone: check the battle.
			return 0;
		}

	}


}
