using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Resource.SaveData {
	
	//undone
	public class ConstructionData : SaveData {


		public class ConstructionElement {

			/// <summary>
			/// 建造した艦のID
			/// </summary>
			public int ShipID { get; set; }

			/// <summary>
			/// 建造日時
			/// </summary>
			public DateTime Date { get; set; }

			/// <summary>
			/// 投入燃料
			/// </summary>
			public int Fuel { get; set; }

			/// <summary>
			/// 投入弾薬
			/// </summary>
			public int Ammo { get; set; }

			/// <summary>
			/// 投入鋼材
			/// </summary>
			public int Steel { get; set; }

			/// <summary>
			/// 投入ボーキサイト
			/// </summary>
			public int Bauxite { get; set; }

			/// <summary>
			/// 投入開発資材
			/// </summary>
			public int DevelopmentMaterial { get; set; }

			/// <summary>
			/// 大型艦建造かのフラグ
			/// </summary>
			public bool IsLargeDock { get; set; }

			/// <summary>
			/// 空きドック数
			/// </summary>
			public int EmptyDockAmount { get; set; }

			/// <summary>
			/// 旗艦の艦船ID
			/// </summary>
			public int Flagship { get; set; }

			/// <summary>
			/// 司令部Lv.
			/// </summary>
			public int HQLevel { get; set; }


			public ConstructionElement() {
				ShipID = -1;
				Date = DateTime.Now;
			}

			public ConstructionElement( int shipID, int fuel, int ammo, int steel, int bauxite, int developmentMaterial, int emptyDock, int flagship, int hqLevel ) {
				ShipID = shipID;
				Date = DateTime.Now;
				Fuel = fuel;
				Ammo = ammo;
				Steel = steel;
				Bauxite = bauxite;
				DevelopmentMaterial = developmentMaterial;
				IsLargeDock = fuel >= 1000;
				EmptyDockAmount = emptyDock;
				Flagship = flagship;
				HQLevel = hqLevel;
			}
		}


		public class InternalData : InternalBaseData {

			public List<ConstructionElement> ConstructionList { get; set; }


			public InternalData() {
				ConstructionList = new List<ConstructionElement>();
			}

		}


		public ConstructionData()
			: base() {

			DataInstance = new InternalData();
		}


		public InternalData Data {
			get { return (InternalData)DataInstance; }
			set { DataInstance = value; }
		}



		public void Add( int shipID, int fuel, int ammo, int steel, int bauxite, int developmentMaterial, int emptyDock, int flagship, int hqLevel ) {

			Data.ConstructionList.Add( new ConstructionElement( shipID, fuel, ammo, steel, bauxite, developmentMaterial, emptyDock, flagship, hqLevel ) );

		}



		public override string SaveFileName {
			get { return "ConstructionData.json"; }
		}

	}
}
