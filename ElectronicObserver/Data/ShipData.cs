using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {


	/// <summary>
	/// 個別の艦船データを保持します。
	/// </summary>
	public class ShipData : IResponseLoader {

		//マスターデータか何かに定義しておくといいかも
		//public static const int SlotMax = 5;


		public int MasterID { get; set; }

		public int SortID { get; set; }

		public int ShipID { get; set; }

		public int Level { get; set; }

		public int ExpTotal { get; set; }

		public int ExpNext { get; set; }

		public ParameterBase Param { get; set; }

		//TODO: slotitem

		//TODO: propertize
		public Fraction[] Aircraft;

		public Fraction Fuel { get; set; }

		public Fraction Ammo { get; set; }

		public int RepairTime { get; set; }

		public int RepairSteel { get; set; }
		
		public int RepairFuel { get; set; }

		public int Condition { get; set; }

		public bool IsLocked { get; set; }

		public int SallyArea { get; set; }




		public bool LoadFromResponse( dynamic data ) {
			throw new NotImplementedException();
		}
	}


}

