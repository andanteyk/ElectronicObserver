using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	/// <summary>
	/// 個別の装備データを保持します。
	/// </summary>
	[DebuggerDisplay( "[{ID}] : {KCDatabase.Instance.MasterEquipments[EquipmentID].Name}" )]
	public class EquipmentData : ResponseWrapper, IIdentifiable {

		/// <summary>
		/// 装備を一意に識別するID
		/// </summary>
		public int MasterID {
			get { return (int)RawData.api_id; }
		}

		/// <summary>
		/// 装備ID
		/// </summary>
		public int EquipmentID {
			get { return (int)RawData.api_slotitem_id; }
		}



		public int ID {
			get { return MasterID; }
		}

	}


}
