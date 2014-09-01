using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	/// <summary>
	/// 個別の装備データを保持します。
	/// </summary>
	class EquipmentData : IResponseLoader {

		public int MasterID { get; set; }
		public int EquipmentID { get; set; }


		public bool LoadFromResponse( dynamic data ) {
			throw new NotImplementedException();
		}
	}


}
