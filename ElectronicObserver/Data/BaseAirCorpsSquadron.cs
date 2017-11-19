using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data
{

	/// <summary>
	/// 基地航空隊の航空中隊データを扱います。
	/// </summary>
	public class BaseAirCorpsSquadron : APIWrapper, IIdentifiable
	{

		/// <summary>
		/// 中隊ID
		/// </summary>
		public int SquadronID => (int)RawData.api_squadron_id;


		/// <summary>
		/// 状態
		/// 0=未配属, 1=配属済み, 2=配置転換中
		/// </summary>
		public int State => RawData.api_state() ? (int)RawData.api_state : 0;


		/// <summary>
		/// 装備固有ID
		/// </summary>
		public int EquipmentMasterID => (int)RawData.api_slotid;


		/// <summary>
		/// 装備データ
		/// </summary>
		public EquipmentData EquipmentInstance => KCDatabase.Instance.Equipments[EquipmentMasterID];


		/// <summary>
		/// 装備ID
		/// </summary>
		public int EquipmentID => EquipmentInstance?.EquipmentID ?? -1;

		/// <summary>
		/// マスター装備データ
		/// </summary>
		public EquipmentDataMaster EquipmentInstanceMaster => EquipmentInstance?.MasterEquipment;

		/// <summary>
		/// 現在の稼働機数
		/// </summary>
		public int AircraftCurrent => RawData.api_count() ? (int)RawData.api_count : 0;


		/// <summary>
		/// 最大機数
		/// </summary>
		public int AircraftMax => RawData.api_max_count() ? (int)RawData.api_max_count : 0;


		/// <summary>
		/// コンディション
		/// 1=通常、2=橙疲労、3=赤疲労
		/// </summary>
		public int Condition => RawData.api_cond() ? (int)RawData.api_cond : 1;



		/// <summary>
		/// 配置転換を開始した時刻
		/// </summary>
		public DateTime RelocatedTime
		{
			get
			{
				if (State != 2)
					return DateTime.MinValue;

				var relocated = KCDatabase.Instance.RelocatedEquipments[EquipmentMasterID];
				if (relocated == null)
					return DateTime.MinValue;

				return relocated.RelocatedTime;
			}
		}


		public override void LoadFromResponse(string apiname, dynamic data)
		{

			int prevState = RawData != null ? State : 0;

			base.LoadFromResponse(apiname, (object)data);

		}


		public override string ToString() => $"{EquipmentInstance} {AircraftCurrent}/{AircraftMax}";


		public int ID => SquadronID;
	}

}
