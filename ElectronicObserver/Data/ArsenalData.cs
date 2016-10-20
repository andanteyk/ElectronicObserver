using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	/// <summary>
	/// 工廠ドックの情報を保持します。
	/// </summary>
	public class ArsenalData : ResponseWrapper, IIdentifiable {

		/// <summary>
		/// ドックID
		/// </summary>
		public int ArsenalID {
			get { return (int)RawData.api_id; }
		}

		/// <summary>
		/// 状態
		/// -1=ロック, 0=空き, 2=建造中, 3=完成
		/// </summary>
		public int State { get; internal set; }

		/// <summary>
		/// 艦船ID
		/// </summary>
		public int ShipID {
			get { return (int)RawData.api_created_ship_id; }
		}

		/// <summary>
		/// 完成日時
		/// </summary>
		public DateTime CompletionTime {
			get { return DateTimeHelper.FromAPITime( (long)RawData.api_complete_time ); }
		}


		/// <summary>
		/// 投入燃料
		/// </summary>
		public int Fuel {
			get { return (int)RawData.api_item1; }
		}

		/// <summary>
		/// 投入弾薬
		/// </summary>
		public int Ammo {
			get { return (int)RawData.api_item2; }
		}

		/// <summary>
		/// 投入鋼材
		/// </summary>
		public int Steel {
			get { return (int)RawData.api_item3; }
		}

		/// <summary>
		/// 投入ボーキサイト
		/// </summary>
		public int Bauxite {
			get { return (int)RawData.api_item4; }
		}

		/// <summary>
		/// 投入開発資材
		/// </summary>
		public int DevelopmentMaterial {
			get { return (int)RawData.api_item5; }
		}


		public int ID {
			get { return ArsenalID; }
		}



		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );

			State = (int)RawData.api_state;

		}


		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			sb.Append( "[" + ID.ToString() + "] : " );
			switch ( State ) {
				case -1:
					sb.Append( "<Locked>" ); break;
				case 0:
					sb.Append( "<Empty>" ); break;
				case 2:
					sb.Append( KCDatabase.Instance.MasterShips[ShipID].Name + ", at " + CompletionTime.ToString() ); break;
				case 3:
					sb.Append( KCDatabase.Instance.MasterShips[ShipID].Name + ", Completed!" ); break;
			}

			return sb.ToString();
		}
	}

}
