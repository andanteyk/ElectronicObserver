using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	/// <summary>
	/// 海域情報を保持します。
	/// </summary>
	public class MapInfoData : ResponseWrapper, IIdentifiable {

		/// <summary>
		/// 海域ID
		/// </summary>
		public int MapID {
			get { return (int)RawData.api_id; }
		}

		/// <summary>
		/// 海域カテゴリID
		/// </summary>
		public int MapAreaID {
			get { return (int)RawData.api_maparea_id; }
		}

		/// <summary>
		/// 海域カテゴリ内番号
		/// </summary>
		public int MapInfoID {
			get { return (int)RawData.api_no; }
		}

		/// <summary>
		/// 海域名
		/// </summary>
		public string Name {
			get { return RawData.api_name; }
		}

		/// <summary>
		/// 難易度
		/// </summary>
		public int Difficulty {
			get { return (int)RawData.api_level; }
		}

		/// <summary>
		/// 作戦名
		/// </summary>
		public string OperationName {
			get { return RawData.api_opetext; }
		}

		/// <summary>
		/// 作戦情報
		/// </summary>
		public string Information {
			get { return ( (string)RawData.api_infotext ).Replace( "<br>", "" ); }
		}

		/// <summary>
		/// 必要な撃破回数(EO海域)
		/// 存在しなければ -1
		/// </summary>
		public int RequiredDefeatedCount {
			get { 
				if ( RawData.api_required_defeat_count == null )
					return -1;
				else
					return (int)RawData.api_required_defeat_count;
			}
		}


		public int ID {
			get { return MapID; }
		}
	}

}
