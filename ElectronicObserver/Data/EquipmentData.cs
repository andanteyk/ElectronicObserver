using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	/// <summary>
	/// 個別の装備データを保持します。
	/// </summary>
	public class EquipmentData : IIdentifiable, IResponseLoader {

		/// <summary>
		/// 装備を一意に識別するID
		/// </summary>
		public int MasterID { get; private set; }

		/// <summary>
		/// 装備ID
		/// </summary>
		public int EquipmentID { get; private set; }



		public int ID {
			get { return MasterID; }
		}

		public bool LoadFromResponse( string apiname, dynamic data ) {
			throw new NotImplementedException();
		}
	}


}
