using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {


	/// <summary>
	/// 装備のマスターデータを保持します。
	/// </summary>
	public class EquipmentDataMaster : IResponseLoader {

		public int EquipmentID { get; set; }
		public int SortID { get; set; }
		public string Name { get; set; }

		public int[] EquipmentType;

		public ParameterBase Param { get; set; }

		public EquipmentRarity Rarity { get; set; }
		
		public int[] Material;
		
		public string Message { get; set; }


		public bool LoadFromResponse( dynamic data ) {
			throw new NotImplementedException();
		}
	}

}
