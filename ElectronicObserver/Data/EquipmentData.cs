using Codeplex.Data;
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


		/// <summary>
		/// 保護ロック
		/// </summary>
		public bool IsLocked {
			get { return (int)RawData.api_locked != 0; }
		}

		/// <summary>
		/// 改修Lv.
		/// </summary>
		public int Level {
			get { return (int)RawData.api_level; }
		}



		/// <summary>
		/// 装備のマスターデータへの参照
		/// </summary>
		public EquipmentDataMaster MasterEquipment {
			get { return KCDatabase.Instance.MasterEquipments[EquipmentID]; }
		}


		public int ID {
			get { return MasterID; }
		}


		public override void LoadFromResponse( string apiname, dynamic data ) {

			switch ( apiname ) {
				case "api_req_kousyou/createitem":		//不足パラメータの追加
					data.api_locked = 0;
					data.api_level = 0;
					break;

				default:
					break;
			}

			base.LoadFromResponse( apiname, (object)data );
		
		}

	}


}
