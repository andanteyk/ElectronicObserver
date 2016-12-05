using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {
	
	public class RelocationData : IIdentifiable {

		/// <summary>
		/// 装備ID
		/// </summary>
		public int EquipmentID { get; set; }
		
		/// <summary>
		/// 配置転換を開始した時間
		/// </summary>
		public DateTime RelocatedTime { get; set; }


		/// <summary>
		/// 装備のインスタンス
		/// </summary>
		public EquipmentData EquipmentInstance { get { return KCDatabase.Instance.Equipments[EquipmentID]; } }


		public RelocationData( int equipmentID, DateTime relocatedTime ) {
			EquipmentID = equipmentID;
			RelocatedTime = relocatedTime;
		}

		public int ID {
			get { return EquipmentID; }
		}
	}
}
