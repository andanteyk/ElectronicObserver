using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	/// <summary>
	/// 基地航空隊の航空中隊データを扱います。
	/// </summary>
	[DebuggerDisplay( "{EquipmentInstance} {AircraftCurrent}/{AircraftMax}" )]
	public class BaseAirCorpsSquadron : APIWrapper, IIdentifiable {

		/// <summary>
		/// 中隊ID
		/// </summary>
		public int SquadronID {
			get {
				return (int)RawData.api_squadron_id;
			}
		}

		/// <summary>
		/// 状態
		/// 0=未配属, 1=配属済み, 2=配置転換中
		/// </summary>
		public int State {
			get {
				return RawData.api_state() ? (int)RawData.api_state : 0;
			}
		}

		/// <summary>
		/// 装備固有ID
		/// </summary>
		public int EquipmentMasterID {
			get {
				return (int)RawData.api_slotid;
			}
		}

		/// <summary>
		/// 装備データ
		/// </summary>
		public EquipmentData EquipmentInstance {
			get {
				return KCDatabase.Instance.Equipments[EquipmentMasterID];
			}
		}

		/// <summary>
		/// 装備ID
		/// </summary>
		public int EquipmentID {
			get {
				var eq = EquipmentInstance;
				return eq != null ? eq.EquipmentID : -1;
			}
		}

		/// <summary>
		/// マスター装備データ
		/// </summary>
		public EquipmentDataMaster EquipmentInstanceMaster {
			get {
				var eq = EquipmentInstance;
				return eq != null ? eq.MasterEquipment : null;
			}
		}

		/// <summary>
		/// 現在の稼働機数
		/// </summary>
		public int AircraftCurrent {
			get {
				return RawData.api_count() ? (int)RawData.api_count : 0;
			}
		}

		/// <summary>
		/// 最大機数
		/// </summary>
		public int AircraftMax {
			get {
				return RawData.api_max_count() ? (int)RawData.api_max_count : 0;
			}
		}

		/// <summary>
		/// コンディション
		/// 1=通常、2=橙疲労、3=赤疲労
		/// </summary>
		public int Condition {
			get {
				return RawData.api_cond() ? (int)RawData.api_cond : 1;
			}
		}


		/// <summary>
		/// 配置転換を開始した時刻
		/// </summary>
		public DateTime RelocatedTime { get; private set; }


		public override void LoadFromResponse( string apiname, dynamic data ) {

			int prevState = RawData != null ? State : 0;

			base.LoadFromResponse( apiname, (object)data );

			// 配置転換中になったとき
			if ( prevState == 1 && State == 2 ) {
				RelocatedTime = DateTime.Now;
			}

			if ( State != 2 ) {
				RelocatedTime = DateTime.MinValue;
			}
		}


		public int ID {
			get { return SquadronID; }
		}
	}
}
