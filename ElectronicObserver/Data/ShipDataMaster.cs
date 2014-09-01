using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {


	/// <summary>
	/// 艦船のマスターデータを保持します。
	/// </summary>
	public class ShipDataMaster : IResponseLoader {

		public int ShipID { get; set; }

		public int SortID { get; set; }

		public string Name { get; set; }

		public string NameReading { get; set; }

		//TODO:shiptype

		public int RemodelAfterLevel { get; set; }

		//参照にしてもいいかも
		public int RemodelAfterShipID { get; set; }

		public int RemodelBeforeShipID { get; set; }

		public int RemodelAmmo { get; set; }
		public int RemodelSteel { get; set; }

		public ParameterBase Param { get; set; }

		public ShipSpeed Speed { get; set; }

		public int SlotSize { get; set; }

		//TODO:propertize
		public int[] Aircraft;

		//TODO:Default Equipments

		public int BuildingTime { get; set; }

		//TODO:propertize
		public int[] Material;

		public int[] PowerUp;

		public ShipRarity Rarity { get; set; }

		public string MessageGet { get; set; }

		public string MessageDict { get; set; }

		public int Fuel { get; set; }
		public int Ammo { get; set; }


		public bool LoadFromResponse( dynamic data ) {
			throw new NotImplementedException();
		}
	}


}
