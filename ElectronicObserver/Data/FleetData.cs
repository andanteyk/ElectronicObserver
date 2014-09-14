using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {
	
	/// <summary>
	/// 艦隊の情報を保持します。
	/// </summary>
	[DebuggerDisplay( "[{ID}] : {Name}" )]
	public class FleetData : ResponseWrapper, IIdentifiable {

		/// <summary>
		/// 艦隊ID
		/// </summary>
		public int FleetID {
			get { return (int)RawData.api_id; }
		}

		/// <summary>
		/// 艦隊名
		/// </summary>
		public string Name {
			get { return (string)RawData.api_name; }
		}

		/// <summary>
		/// 遠征状態
		/// 0=未出撃, 1=遠征中, 2=遠征帰投, 3=強制帰投中
		/// </summary>
		public int ExpeditionState {
			get { return (int)RawData.api_mission[0]; }
		}

		/// <summary>
		/// 遠征先ID
		/// </summary>
		public int ExpeditionDestination {
			get { return (int)RawData.api_mission[1]; }
		}

		/// <summary>
		/// 遠征帰投時間
		/// </summary>
		public DateTime ExpeditionTime {
			get { return DateConverter.FromAPITime( (long)RawData.api_mission[2] ); }
		}

		/// <summary>
		/// 艦隊メンバー
		/// </summary>
		public ReadOnlyCollection<int> FleetMember {
			get { return Array.AsReadOnly<int>( (int[])RawData.api_ship ); }
		}


		public int ID {
			get { return FleetID; }
		}
	}

}
