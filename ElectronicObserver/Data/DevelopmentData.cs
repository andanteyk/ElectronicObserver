using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data
{
	public class DevelopmentData : APIWrapper
	{
		// request

		/// <summary> 投入燃料(1回分) </summary>
		public int Fuel { get; private set; }

		/// <summary> 投入弾薬(1回分) </summary>
		public int Ammo { get; private set; }

		/// <summary> 投入鋼材(1回分) </summary>
		public int Steel { get; private set; }

		/// <summary> 投入ボーキサイト(1回分) </summary>
		public int Bauxite { get; private set; }

		/// <summary> 開発の試行回数 </summary>
		public int DevelopmentTrials { get; private set; }


		// response

		/// <summary> 開発に成功したか </summary>
		public bool IsSucceeded { get; private set; }

		/// <summary> 開発後の各種保有資源 </summary>
		public ReadOnlyCollection<int> Materials { get; private set; }


		public class DevelopmentResult
		{
			public readonly int MasterID;
			public readonly int EquipmentID;

			public bool IsSucceeded => MasterID != -1;
			public EquipmentData Equipment => KCDatabase.Instance.Equipments[MasterID];
			public EquipmentDataMaster MasterEquipment => Equipment?.MasterEquipment;

			public DevelopmentResult() : this(null) { }
			public DevelopmentResult(dynamic data)
			{
				MasterID = (int?)data?.api_id ?? -1;
				EquipmentID = (int?)data?.api_slotitem_id ?? -1;
			}

			public override string ToString()
			{
				return IsSucceeded ? $"{MasterEquipment.CategoryTypeInstance.Name}「{MasterEquipment.Name}」" : "失敗";
			}
		}
		/// <summary> 開発結果 </summary>
		public ReadOnlyCollection<DevelopmentResult> Results { get; private set; }


		public override void LoadFromRequest(string apiname, Dictionary<string, string> data)
		{
			base.LoadFromRequest(apiname, data);

			Fuel = int.Parse(data["api_item1"]);
			Ammo = int.Parse(data["api_item2"]);
			Steel = int.Parse(data["api_item3"]);
			Bauxite = int.Parse(data["api_item4"]);

			if (data.ContainsKey("api_multiple_flag"))
				DevelopmentTrials = int.Parse(data["api_multiple_flag"]) != 0 ? 3 : 1;
			else
				DevelopmentTrials = 1;


			IsSucceeded = false;
			Materials = null;
			Results = null;
		}

		public override void LoadFromResponse(string apiname, dynamic data)
		{
			base.LoadFromResponse(apiname, (object)data);

			IsSucceeded = (int)data.api_create_flag != 0;
			Materials = Array.AsReadOnly((int[])data.api_material);


			void AddToDatabase(dynamic equipmentData)
			{
				var eq = new EquipmentData();
				eq.LoadFromResponse(apiname, Codeplex.Data.DynamicJson.Parse(equipmentData.ToString()));
				KCDatabase.Instance.Equipments.Add(eq);
			}


			bool isOldAPI = data.api_shizai_flag();

			if (isOldAPI)
			{
				// 旧 API フォーマット (-2019/09/30 12:00)
				Results = Array.AsReadOnly(new[] {
					IsSucceeded ?
					new DevelopmentResult(data.api_slot_item) :
					new DevelopmentResult()
				});

				if (IsSucceeded)
					AddToDatabase(data.api_slot_item);
			}
			else
			{
				// 新 API フォーマット (2019/09/30 21:00-)
				dynamic[] elems = data.api_get_items;
				var results = new DevelopmentResult[elems.Length];

				for (int i = 0; i < elems.Length; i++)
				{
					results[i] = new DevelopmentResult(elems[i]);

					if (results[i].IsSucceeded)
						AddToDatabase(elems[i]);
				}

				Results = Array.AsReadOnly(results);
			}

			KCDatabase.Instance.Material.LoadFromResponse(apiname, data.api_material);

		}

	}
}
