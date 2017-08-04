using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace ElectronicObserver.Utility.Data {

	/// <summary>
	/// 経験値テーブルを管理します。
	/// </summary>
	public static class ExpTable {

		/// <summary>
		/// 経験値データを保持します。
		/// </summary>
		public class Experience {

			/// <summary>
			/// 現在のレベル
			/// </summary>
			public int Level { get; private set; }
			
			/// <summary>
			/// このレベルに到達するのに必要な累積経験値
			/// </summary>
			public int Total { get; private set; }

			/// <summary>
			/// 次のレベルに到達するのに必要な経験値
			/// </summary>
			public int Next { get; private set; }

			internal Experience( int level, int total, int next ) {
				Level = level;
				Total = total;
				Next = next;
			}

		}


		/// <summary>
		/// 艦娘経験値テーブル
		/// </summary>
		public static ReadOnlyDictionary<int, Experience> ShipExp { get; private set; }
		
		/// <summary>
		/// 提督経験値テーブル
		/// </summary>
		public static ReadOnlyDictionary<int, Experience> AdmiralExp { get; private set; }

		/// <summary>
		/// 艦娘レベル最大値
		/// </summary>
		public static int ShipMaximumLevel { get { return 165; } }

		/// <summary>
		/// 提督レベル最大値
		/// </summary>
		public static int AdmiralMaximumLevel { get { return 120; } }


		/// <summary>
		/// 次のレベルに上がるのに必要な経験値の量を取得します。
		/// </summary>
		/// <param name="expTable">経験値テーブル。</param>
		/// <param name="current">現在の累積経験値。</param>
		private static int GetNextExp( ReadOnlyDictionary<int, Experience> expTable, int current ) {

			Experience l = expTable.Values.FirstOrDefault( e => e.Total + e.Next > current );

			if ( l == null || !expTable.ContainsKey( l.Level + 1 ) )
				return 0;

			return expTable[l.Level + 1].Total - current;
		}


		/// <summary>
		/// 艦娘が次のレベルに上がるのに必要な経験値の量を取得します。
		/// </summary>
		/// <param name="current">現在の累積経験値。</param>
		public static int GetNextExpShip( int current ) {
			return GetNextExp( ShipExp, current );
		}

		/// <summary>
		/// 司令部レベルが次のレベルに上がるのに必要な経験値の量を取得します。
		/// </summary>
		/// <param name="current">現在の累積経験値。</param>
		public static int GetNextExpAdmiral( int current ) {
			return GetNextExp( AdmiralExp, current );
		}


		/// <summary>
		/// 指定したレベルに上がるのに必要な経験値の量を取得します。
		/// </summary>
		/// <param name="expTable">経験値テーブル。</param>
		/// <param name="current">現在の累積経験値。</param>
		/// <param name="level">対象のレベル。</param>
		private static int GetExpToLevel( ReadOnlyDictionary<int, Experience> expTable, int current, int level ) {

			if ( !expTable.ContainsKey( level ) )
				return 0;

			return expTable[level].Total - current;
		}

		/// <summary>
		/// 艦娘が指定したレベルに上がるのに必要な経験値の量を取得します。
		/// </summary>
		/// <param name="current">現在の累積経験値。</param>
		/// <param name="level">対象のレベル。</param>
		public static int GetExpToLevelShip( int current, int level ) {
			return GetExpToLevel( ShipExp, current, level );
		}

		/// <summary>
		/// 司令部レベルが指定したレベルに上がるのに必要な経験値の量を取得します。
		/// </summary>
		/// <param name="current">現在の累積経験値。</param>
		/// <param name="level">対象のレベル。</param>
		public static int GetExpToLevelAdmiral( int current, int level ) {
			return GetExpToLevel( AdmiralExp, current, level );
		}



		static ExpTable() {

			#region Initialize table

			Experience[] shipexp = new Experience[] {
				new Experience( 1, 0, 100 ), 
				new Experience( 2, 100, 200 ), 
				new Experience( 3, 300, 300 ), 
				new Experience( 4, 600, 400 ), 
				new Experience( 5, 1000, 500 ), 
				new Experience( 6, 1500, 600 ), 
				new Experience( 7, 2100, 700 ), 
				new Experience( 8, 2800, 800 ), 
				new Experience( 9, 3600, 900 ), 
				new Experience( 10, 4500, 1000 ), 
				new Experience( 11, 5500, 1100 ), 
				new Experience( 12, 6600, 1200 ), 
				new Experience( 13, 7800, 1300 ), 
				new Experience( 14, 9100, 1400 ), 
				new Experience( 15, 10500, 1500 ), 
				new Experience( 16, 12000, 1600 ), 
				new Experience( 17, 13600, 1700 ), 
				new Experience( 18, 15300, 1800 ), 
				new Experience( 19, 17100, 1900 ), 
				new Experience( 20, 19000, 2000 ), 
				new Experience( 21, 21000, 2100 ), 
				new Experience( 22, 23100, 2200 ), 
				new Experience( 23, 25300, 2300 ), 
				new Experience( 24, 27600, 2400 ), 
				new Experience( 25, 30000, 2500 ), 
				new Experience( 26, 32500, 2600 ), 
				new Experience( 27, 35100, 2700 ), 
				new Experience( 28, 37800, 2800 ), 
				new Experience( 29, 40600, 2900 ), 
				new Experience( 30, 43500, 3000 ), 
				new Experience( 31, 46500, 3100 ), 
				new Experience( 32, 49600, 3200 ), 
				new Experience( 33, 52800, 3300 ), 
				new Experience( 34, 56100, 3400 ), 
				new Experience( 35, 59500, 3500 ), 
				new Experience( 36, 63000, 3600 ), 
				new Experience( 37, 66600, 3700 ), 
				new Experience( 38, 70300, 3800 ), 
				new Experience( 39, 74100, 3900 ), 
				new Experience( 40, 78000, 4000 ), 
				new Experience( 41, 82000, 4100 ), 
				new Experience( 42, 86100, 4200 ), 
				new Experience( 43, 90300, 4300 ), 
				new Experience( 44, 94600, 4400 ), 
				new Experience( 45, 99000, 4500 ), 
				new Experience( 46, 103500, 4600 ), 
				new Experience( 47, 108100, 4700 ), 
				new Experience( 48, 112800, 4800 ), 
				new Experience( 49, 117600, 4900 ), 
				new Experience( 50, 122500, 5000 ), 
				new Experience( 51, 127500, 5200 ), 
				new Experience( 52, 132700, 5400 ), 
				new Experience( 53, 138100, 5600 ), 
				new Experience( 54, 143700, 5800 ), 
				new Experience( 55, 149500, 6000 ), 
				new Experience( 56, 155500, 6200 ), 
				new Experience( 57, 161700, 6400 ), 
				new Experience( 58, 168100, 6600 ), 
				new Experience( 59, 174700, 6800 ), 
				new Experience( 60, 181500, 7000 ), 
				new Experience( 61, 188500, 7300 ), 
				new Experience( 62, 195800, 7600 ), 
				new Experience( 63, 203400, 7900 ), 
				new Experience( 64, 211300, 8200 ), 
				new Experience( 65, 219500, 8500 ), 
				new Experience( 66, 228000, 8800 ), 
				new Experience( 67, 236800, 9100 ), 
				new Experience( 68, 245900, 9400 ), 
				new Experience( 69, 255300, 9700 ), 
				new Experience( 70, 265000, 10000 ), 
				new Experience( 71, 275000, 10400 ), 
				new Experience( 72, 285400, 10800 ), 
				new Experience( 73, 296200, 11200 ), 
				new Experience( 74, 307400, 11600 ), 
				new Experience( 75, 319000, 12000 ), 
				new Experience( 76, 331000, 12400 ), 
				new Experience( 77, 343400, 12800 ), 
				new Experience( 78, 356200, 13200 ), 
				new Experience( 79, 369400, 13600 ), 
				new Experience( 80, 383000, 14000 ), 
				new Experience( 81, 397000, 14500 ), 
				new Experience( 82, 411500, 15000 ), 
				new Experience( 83, 426500, 15500 ), 
				new Experience( 84, 442000, 16000 ), 
				new Experience( 85, 458000, 16500 ), 
				new Experience( 86, 474500, 17000 ), 
				new Experience( 87, 491500, 17500 ), 
				new Experience( 88, 509000, 18000 ), 
				new Experience( 89, 527000, 18500 ), 
				new Experience( 90, 545500, 19000 ), 
				new Experience( 91, 564500, 20000 ), 
				new Experience( 92, 584500, 22000 ), 
				new Experience( 93, 606500, 25000 ), 
				new Experience( 94, 631500, 30000 ), 
				new Experience( 95, 661500, 40000 ), 
				new Experience( 96, 701500, 60000 ), 
				new Experience( 97, 761500, 90000 ), 
				new Experience( 98, 851500, 148500 ), 
				new Experience( 99, 1000000, 0 ), 
				new Experience( 100, 1000000, 10000 ), 
				new Experience( 101, 1010000, 1000 ), 
				new Experience( 102, 1011000, 2000 ), 
				new Experience( 103, 1013000, 3000 ), 
				new Experience( 104, 1016000, 4000 ), 
				new Experience( 105, 1020000, 5000 ), 
				new Experience( 106, 1025000, 6000 ), 
				new Experience( 107, 1031000, 7000 ), 
				new Experience( 108, 1038000, 8000 ), 
				new Experience( 109, 1046000, 9000 ), 
				new Experience( 110, 1055000, 10000 ), 
				new Experience( 111, 1065000, 12000 ), 
				new Experience( 112, 1077000, 14000 ), 
				new Experience( 113, 1091000, 16000 ), 
				new Experience( 114, 1107000, 18000 ), 
				new Experience( 115, 1125000, 20000 ), 
				new Experience( 116, 1145000, 23000 ), 
				new Experience( 117, 1168000, 26000 ), 
				new Experience( 118, 1194000, 29000 ), 
				new Experience( 119, 1223000, 32000 ), 
				new Experience( 120, 1255000, 35000 ), 
				new Experience( 121, 1290000, 39000 ), 
				new Experience( 122, 1329000, 43000 ), 
				new Experience( 123, 1372000, 47000 ), 
				new Experience( 124, 1419000, 51000 ), 
				new Experience( 125, 1470000, 55000 ), 
				new Experience( 126, 1525000, 59000 ), 
				new Experience( 127, 1584000, 63000 ), 
				new Experience( 128, 1647000, 67000 ), 
				new Experience( 129, 1714000, 71000 ), 
				new Experience( 130, 1785000, 75000 ), 
				new Experience( 131, 1860000, 80000 ), 
				new Experience( 132, 1940000, 85000 ), 
				new Experience( 133, 2025000, 90000 ), 
				new Experience( 134, 2115000, 95000 ), 
				new Experience( 135, 2210000, 100000 ), 
				new Experience( 136, 2310000, 105000 ), 
				new Experience( 137, 2415000, 110000 ), 
				new Experience( 138, 2525000, 115000 ), 
				new Experience( 139, 2640000, 120000 ), 
				new Experience( 140, 2760000, 127000 ), 
				new Experience( 141, 2887000, 134000 ), 
				new Experience( 142, 3021000, 141000 ), 
				new Experience( 143, 3162000, 148000 ), 
				new Experience( 144, 3310000, 155000 ), 
				new Experience( 145, 3465000, 163000 ), 
				new Experience( 146, 3628000, 171000 ), 
				new Experience( 147, 3799000, 179000 ), 
				new Experience( 148, 3978000, 187000 ), 
				new Experience( 149, 4165000, 195000 ), 
				new Experience( 150, 4360000, 204000 ),
				new Experience( 151, 4564000, 213000 ),
				new Experience( 152, 4777000, 222000 ),
				new Experience( 153, 4999000, 231000 ),
				new Experience( 154, 5230000, 240000 ),
				new Experience( 155, 5470000, 250000 ),
				new Experience( 156, 5720000, 60000 ),
				new Experience( 157, 5780000, 80000 ),
				new Experience( 158, 5860000, 110000 ),
				new Experience( 159, 5970000, 150000 ),
				new Experience( 160, 6120000, 200000 ),
				new Experience( 161, 6320000, 260000 ),
				new Experience( 162, 6580000, 330000 ),
				new Experience( 163, 6910000, 410000 ),
				new Experience( 164, 7320000, 500000 ),
				new Experience( 165, 7820000, 0 ),
			};


			Experience[] admiralexp = new Experience[] {
				new Experience( 1, 0, 100 ), 
				new Experience( 2, 100, 200 ), 
				new Experience( 3, 300, 300 ), 
				new Experience( 4, 600, 400 ), 
				new Experience( 5, 1000, 500 ), 
				new Experience( 6, 1500, 600 ), 
				new Experience( 7, 2100, 700 ), 
				new Experience( 8, 2800, 800 ), 
				new Experience( 9, 3600, 900 ), 
				new Experience( 10, 4500, 1000 ), 
				new Experience( 11, 5500, 1100 ), 
				new Experience( 12, 6600, 1200 ), 
				new Experience( 13, 7800, 1300 ), 
				new Experience( 14, 9100, 1400 ), 
				new Experience( 15, 10500, 1500 ), 
				new Experience( 16, 12000, 1600 ), 
				new Experience( 17, 13600, 1700 ), 
				new Experience( 18, 15300, 1800 ), 
				new Experience( 19, 17100, 1900 ), 
				new Experience( 20, 19000, 2000 ), 
				new Experience( 21, 21000, 2100 ), 
				new Experience( 22, 23100, 2200 ), 
				new Experience( 23, 25300, 2300 ), 
				new Experience( 24, 27600, 2400 ), 
				new Experience( 25, 30000, 2500 ), 
				new Experience( 26, 32500, 2600 ), 
				new Experience( 27, 35100, 2700 ), 
				new Experience( 28, 37800, 2800 ), 
				new Experience( 29, 40600, 2900 ), 
				new Experience( 30, 43500, 3000 ), 
				new Experience( 31, 46500, 3100 ), 
				new Experience( 32, 49600, 3200 ), 
				new Experience( 33, 52800, 3300 ), 
				new Experience( 34, 56100, 3400 ), 
				new Experience( 35, 59500, 3500 ), 
				new Experience( 36, 63000, 3600 ), 
				new Experience( 37, 66600, 3700 ), 
				new Experience( 38, 70300, 3800 ), 
				new Experience( 39, 74100, 3900 ), 
				new Experience( 40, 78000, 4000 ), 
				new Experience( 41, 82000, 4100 ), 
				new Experience( 42, 86100, 4200 ), 
				new Experience( 43, 90300, 4300 ), 
				new Experience( 44, 94600, 4400 ), 
				new Experience( 45, 99000, 4500 ), 
				new Experience( 46, 103500, 4600 ), 
				new Experience( 47, 108100, 4700 ), 
				new Experience( 48, 112800, 4800 ), 
				new Experience( 49, 117600, 4900 ), 
				new Experience( 50, 122500, 5000 ), 
				new Experience( 51, 127500, 5200 ), 
				new Experience( 52, 132700, 5400 ), 
				new Experience( 53, 138100, 5600 ), 
				new Experience( 54, 143700, 5800 ), 
				new Experience( 55, 149500, 6000 ), 
				new Experience( 56, 155500, 6200 ), 
				new Experience( 57, 161700, 6400 ), 
				new Experience( 58, 168100, 6600 ), 
				new Experience( 59, 174700, 6800 ), 
				new Experience( 60, 181500, 7000 ), 
				new Experience( 61, 188500, 7300 ), 
				new Experience( 62, 195800, 7600 ), 
				new Experience( 63, 203400, 7900 ), 
				new Experience( 64, 211300, 8200 ), 
				new Experience( 65, 219500, 8500 ), 
				new Experience( 66, 228000, 8800 ), 
				new Experience( 67, 236800, 9100 ), 
				new Experience( 68, 245900, 9400 ), 
				new Experience( 69, 255300, 9700 ), 
				new Experience( 70, 265000, 10000 ), 
				new Experience( 71, 275000, 10400 ), 
				new Experience( 72, 285400, 10800 ), 
				new Experience( 73, 296200, 11200 ), 
				new Experience( 74, 307400, 11600 ), 
				new Experience( 75, 319000, 12000 ), 
				new Experience( 76, 331000, 12400 ), 
				new Experience( 77, 343400, 12800 ), 
				new Experience( 78, 356200, 13200 ), 
				new Experience( 79, 369400, 13600 ), 
				new Experience( 80, 383000, 14000 ), 
				new Experience( 81, 397000, 14500 ), 
				new Experience( 82, 411500, 15000 ), 
				new Experience( 83, 426500, 15500 ), 
				new Experience( 84, 442000, 16000 ), 
				new Experience( 85, 458000, 16500 ), 
				new Experience( 86, 474500, 17000 ), 
				new Experience( 87, 491500, 17500 ), 
				new Experience( 88, 509000, 18000 ), 
				new Experience( 89, 527000, 18500 ), 
				new Experience( 90, 545500, 19000 ), 
				new Experience( 91, 564500, 20000 ), 
				new Experience( 92, 584500, 22000 ), 
				new Experience( 93, 606500, 25000 ), 
				new Experience( 94, 631500, 30000 ), 
				new Experience( 95, 661500, 40000 ), 
				new Experience( 96, 701500, 60000 ), 
				new Experience( 97, 761500, 90000 ), 
				new Experience( 98, 851500, 148500 ), 
				new Experience( 99, 1000000, 300000 ), 
				new Experience( 100, 1300000, 300000 ), 
				new Experience( 101, 1600000, 300000 ), 
				new Experience( 102, 1900000, 300000 ), 
				new Experience( 103, 2200000, 400000 ), 
				new Experience( 104, 2600000, 400000 ), 
				new Experience( 105, 3000000, 500000 ), 
				new Experience( 106, 3500000, 500000 ), 
				new Experience( 107, 4000000, 600000 ), 
				new Experience( 108, 4600000, 600000 ), 
				new Experience( 109, 5200000, 700000 ), 
				new Experience( 110, 5900000, 700000 ), 
				new Experience( 111, 6600000, 800000 ), 
				new Experience( 112, 7400000, 800000 ), 
				new Experience( 113, 8200000, 900000 ), 
				new Experience( 114, 9100000, 900000 ), 
				new Experience( 115, 10000000, 1000000 ), 
				new Experience( 116, 11000000, 1000000 ), 
				new Experience( 117, 12000000, 1000000 ), 
				new Experience( 118, 13000000, 1000000 ), 
				new Experience( 119, 14000000, 1000000 ), 
				new Experience( 120, 15000000, 0 )
			};

			ShipExp = new ReadOnlyDictionary<int, Experience>( shipexp.ToDictionary( exp => exp.Level ) );
			AdmiralExp = new ReadOnlyDictionary<int, Experience>( admiralexp.ToDictionary( exp => exp.Level ) );

			#endregion

		}

	}

}
