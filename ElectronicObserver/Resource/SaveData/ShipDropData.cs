using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Resource.SaveData {

	public class ShipDropData : SaveData {


		public class ShipDropElement {

			/// <summary>
			/// ドロップした艦のID　-1=なし
			/// </summary>
			public int ShipID { get; set; }

			/// <summary>
			/// ドロップした日時
			/// </summary>
			public DateTime Date { get; set; }

			/// <summary>
			/// 海域カテゴリID
			/// </summary>
			public int MapAreaID { get; set; }

			/// <summary>
			/// 海域カテゴリ内番号
			/// </summary>
			public int MapInfoID { get; set; }

			/// <summary>
			/// 海域セルID
			/// </summary>
			public int CellID { get; set; }

			/// <summary>
			/// 敵編成ID
			/// </summary>
			public int EnemyFleetID { get; set; }
			
			/// <summary>
			/// 勝利ランク
			/// 1=B, 2=A, 3=S, 4=SS?
			/// </summary>
			public int Rank { get; set; }

			/// <summary>
			/// 司令部Lv.
			/// </summary>
			public int HQLevel { get; set; }


			public ShipDropElement() {
				ShipID = -1;
				Date = DateTime.Now;
			}

			public ShipDropElement( int shipID, int mapAreaID, int mapInfoID, int cellID, int enemyFleetID, int rank, int hqLevel ) {
				ShipID = shipID;
				Date = DateTime.Now;
				MapAreaID = mapAreaID;
				MapInfoID = mapInfoID;
				CellID = cellID;
				EnemyFleetID = enemyFleetID;
				Rank = rank;
				HQLevel = hqLevel;
			}

		}

		public class InternalData : InternalBaseData {

			public List<ShipDropElement> ShipDropList { get; set; }

		
			public InternalData() {
				ShipDropList = new List<ShipDropElement>();
			}

		}


		public ShipDropData()
			: base() {

			DataInstance = new InternalData();
		}


		public InternalData Data {
			get { return (InternalData)DataInstance; }
			set { DataInstance = value; }
		}


		public void Add( int shipID, int mapAreaID, int mapInfoID, int cellID, int enemyFleetID, string rank, int hqLevel ) {

			int r = 0;
			switch ( rank ) {
				case "B":
					r = 1; break;
				case "A":
					r = 2; break;
				case "S":
					r = 3; break;
				case "SS":			//todo:未実装
					r = 4; break;
			}

			Add( shipID, mapAreaID, mapInfoID, cellID, enemyFleetID, r, hqLevel );
		}

		public void Add( int shipID, int mapAreaID, int mapInfoID, int cellID, int enemyFleetID, int rank, int hqLevel ) {

			Data.ShipDropList.Add( new ShipDropElement( shipID, mapAreaID, mapInfoID, cellID, enemyFleetID, rank, hqLevel ) );

		}


		public override string SaveFileName {
			get { return "ShipDropData.json"; }
		}
	}

}
