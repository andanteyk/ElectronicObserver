using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {
	
	/// <summary>
	/// 艦種
	/// </summary>
	public class ShipType : ResponseWrapper, IIdentifiable {

		/// <summary>
		/// 艦種
		/// </summary>
		public int TypeID {
			get { return RawData.api_id; }
		}

		/// <summary>
		/// 並べ替え順
		/// </summary>
		public int SortID {
			get { return RawData.api_sortno; }
		}
		
		/// <summary>
		/// 艦種名
		/// </summary>
		public string Name {
			get { return RawData.api_name; }
		}
		
		/// <summary>
		/// 入渠時間係数
		/// </summary>
		public int RepairTime {
			get { return RawData.api_scnt; }
		}

		//TODO: api_kcnt

		//TODO:外部から書き換えられないように
		/// <summary>
		/// 装備可否フラグ
		/// </summary>
		public Dictionary<int, bool> EquipmentType;



		public int ID {
			get { return TypeID; }
		}


	}

}
