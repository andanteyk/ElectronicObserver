using Codeplex.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElectronicObserver.Data
{

	/// <summary>
	/// 艦種
	/// </summary>
	public class ShipType : ResponseWrapper, IIdentifiable
	{

		/// <summary>
		/// 艦種ID
		/// </summary>
		public int TypeID => (int)RawData.api_id;

		/// <summary>
		/// 並べ替え順
		/// </summary>
		public int SortID => (int)RawData.api_sortno;

		/// <summary>
		/// 艦種名
		/// </summary>
		public string Name => RawData.api_name;

		/// <summary>
		/// 入渠時間係数
		/// </summary>
		public int RepairTime => (int)RawData.api_scnt;


		//TODO: api_kcnt


		/// <summary>
		/// 装備可能なカテゴリ一覧
		/// </summary>
		private int[] _equippableCategories;
		public ReadOnlyCollection<int> EquippableCategories => Array.AsReadOnly(_equippableCategories);


		/// <summary>
		/// 艦種ID
		/// </summary>
		public ShipTypes Type => (ShipTypes)TypeID;


		public int ID => TypeID;
		public override string ToString() => $"[{TypeID}] {Name}";



		public ShipType()
			: base()
		{
			_equippableCategories = new int[0];
		}

		public override void LoadFromResponse(string apiname, dynamic data)
		{

			// api_equip_type の置換処理
			// checkme: 無駄が多い気がするのでもっといい案があったら是非
			data = DynamicJson.Parse(Regex.Replace(data.ToString(), @"""(?<id>\d+?)""", @"""api_id_${id}"""));

			base.LoadFromResponse(apiname, (object)data);


			if (IsAvailable)
			{
				IEnumerable<int> getType()
				{
					foreach (KeyValuePair<string, object> type in RawData.api_equip_type)
					{
						if ((double)type.Value != 0)
							yield return Convert.ToInt32(type.Key.Substring(7));     //skip api_id_
					}
				}

				_equippableCategories = getType().ToArray();
			}
		}

	}

}
