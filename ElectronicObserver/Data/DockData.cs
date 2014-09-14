using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	/// <summary>
	/// 入渠ドックの情報を保持します。
	/// </summary>
	public class DockData : ResponseWrapper, IIdentifiable {

		/// <summary>
		/// ドックID
		/// </summary>
		public int DockID {
			get { return (int)RawData.api_id; }
		}

		/// <summary>
		/// 入渠状態
		/// </summary>
		public int State {
			get { return (int)RawData.api_state; }
		}

		/// <summary>
		/// 入渠中の艦船のID
		/// </summary>
		public int ShipID {
			get { return (int)RawData.api_ship_id; }
		}

		/// <summary>
		/// 入渠完了日時
		/// </summary>
		public DateTime CompletionTime {
			get { return DateConverter.FromAPITime( (long)RawData.api_complete_time ); }
		}


		public int ID {
			get { return DockID; }
		}


		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			sb.Append( "[" + ID.ToString() + "] : " );
			switch ( State ) {
				case -1:
					sb.Append( "<Locked>" ); break;
				case 0:
					sb.Append( "<Empty>" ); break;
				case 1:
					sb.Append( KCDatabase.Instance.MasterShips[ShipID].Name + ", at " + CompletionTime.ToString() ); break;
			}

			return sb.ToString();
		}
	}

}
