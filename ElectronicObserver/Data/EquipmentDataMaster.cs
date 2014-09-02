using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {


	/// <summary>
	/// 装備のマスターデータを保持します。
	/// </summary>
	public class EquipmentDataMaster : IIdentifiable, IResponseLoader {

		public int EquipmentID { get; private set; }
		public int SortID { get; private set; }
		public string Name { get; private set; }

		public int[] EquipmentType;

		public ParameterBase Param { get; private set; }

		public EquipmentRarity Rarity { get; private set; }
		
		public int[] Material;
		
		public string Message { get; private set; }


		public int ID {
			get { return EquipmentID; }
		}

		public bool LoadFromResponse( string apiname, dynamic data ) {
			throw new NotImplementedException();
		}

		
	}

}
