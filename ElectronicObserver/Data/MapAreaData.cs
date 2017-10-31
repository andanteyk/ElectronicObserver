using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data
{

	public class MapAreaData : APIWrapper, IIdentifiable
	{

		/// <summary>
		/// 海域カテゴリID
		/// </summary>
		public int MapAreaID => (int)RawData.api_id;

		/// <summary>
		/// 海域カテゴリ名
		/// </summary>
		public string Name => RawData.api_name;

		/// <summary>
		/// 海域タイプ　0=通常, 1=イベント
		/// </summary>
		public int MapType => (int)RawData.api_type;



		public int ID => MapAreaID;
		public override string ToString() => $"[{MapAreaID}] {Name}";
	}

}
