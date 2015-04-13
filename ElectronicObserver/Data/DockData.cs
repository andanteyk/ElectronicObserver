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
		/// -1=ロック, 0=空き, 1=入渠中
		/// </summary>
		public int State { get; internal set; }

		/// <summary>
		/// 入渠中の艦船のID
		/// </summary>
		public int ShipID { get; internal set; }

		/// <summary>
		/// 入渠完了日時
		/// </summary>
		public DateTime CompletionTime { get; internal set; }


		public int ID {
			get { return DockID; }
		}


		public override void LoadFromResponse( string apiname, dynamic data ) {

			switch ( apiname ) {
				case "api_req_nyukyo/speedchange":
					if ( State == 1 && ShipID != 0 ) {
						KCDatabase.Instance.Ships[ShipID].Repair();

						State = 0;
						ShipID = 0;
					}
					break;

				default: {
						base.LoadFromResponse( apiname, (object)data );

						int newstate = (int)RawData.api_state;

						if ( State == 1 && newstate == 0 && ShipID != 0 ) {
							KCDatabase.Instance.Ships[ShipID].Repair();
						}

						State = newstate;
						ShipID = (int)RawData.api_ship_id;
						CompletionTime = DateTimeHelper.FromAPITime( (long)RawData.api_complete_time );
					} break;
			}


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
					sb.Append( KCDatabase.Instance.Ships[ShipID].MasterShip.Name + ", at " + CompletionTime.ToString() ); break;
			}

			return sb.ToString();
		}
	}

}
