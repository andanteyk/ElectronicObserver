using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Resource.Record {

	/// <summary>
	/// 建造のレコードです。
	/// </summary>
	[DebuggerDisplay( "{Record.Count} Records" )]
	public class DevelopmentRecord : RecordBase {

		[DebuggerDisplay( "[{EquipmentID}] : {EquipmentName}" )]
		public class DevelopmentElement : RecordElementBase {

			/// <summary>
			/// 開発した装備のID
			/// </summary>
			public int EquipmentID { get; set; }

			/// <summary>
			/// 開発した装備の名前
			/// </summary>
			public string EquipmentName { get; set; }

			/// <summary>
			/// 開発日時
			/// </summary>
			public DateTime Date { get; set; }

			/// <summary>
			/// 投入燃料
			/// </summary>
			public int Fuel { get; set; }

			/// <summary>
			/// 投入弾薬
			/// </summary>
			public int Ammo { get; set; }

			/// <summary>
			/// 投入鋼材
			/// </summary>
			public int Steel { get; set; }

			/// <summary>
			/// 投入ボーキサイト
			/// </summary>
			public int Bauxite { get; set; }

			/// <summary>
			/// 旗艦の艦船ID
			/// </summary>
			public int FlagshipID { get; set; }

			/// <summary>
			/// 旗艦の艦名
			/// </summary>
			public string FlagshipName { get; set; }

			/// <summary>
			/// 旗艦の艦種
			/// </summary>
			public int FlagshipType { get; set; }

			/// <summary>
			/// 司令部Lv.
			/// </summary>
			public int HQLevel { get; set; }



			public DevelopmentElement() {
				EquipmentID = -1;
				Date = DateTime.Now;
			}

			public DevelopmentElement( string line )
				: base( line ) { }

			public DevelopmentElement( int equipmentID, int fuel, int ammo, int steel, int bauxite, int flagshipID, int hqLevel ) {
				EquipmentID = equipmentID;
				Fuel = fuel;
				Ammo = ammo;
				Steel = steel;
				Bauxite = bauxite;
				FlagshipID = flagshipID;
				HQLevel = hqLevel;

				SetSubParameters();
			}


			public override void LoadLine( string line ) {

				string[] elem = line.Split( ",".ToCharArray() );
				if ( elem.Length < 11 ) throw new ArgumentException( "要素数が少なすぎます。" );

				EquipmentID = int.Parse( elem[0] );
				EquipmentName = elem[1];
				Date = DateTimeHelper.CSVStringToTime( elem[2] );
				Fuel = int.Parse( elem[3] );
				Ammo = int.Parse( elem[4] );
				Steel = int.Parse( elem[5] );
				Bauxite = int.Parse( elem[6] );
				FlagshipID = int.Parse( elem[7] );
				FlagshipName = elem[8];
				FlagshipType = int.Parse( elem[9] );
				HQLevel = int.Parse( elem[10] );

			}

			public override string SaveLine() {

				return string.Format( "{" + string.Join( "},{", Enumerable.Range( 0, 11 ) ) + "}",
					EquipmentID,
					EquipmentName,
					DateTimeHelper.TimeToCSVString( Date ),
					Fuel,
					Ammo,
					Steel,
					Bauxite,
					FlagshipID,
					FlagshipName,
					FlagshipType,
					HQLevel );
			}

			/// <summary>
			/// 艦名などのパラメータを現在のIDをもとに設定します。
			/// </summary>
			public void SetSubParameters() {
				var eq = KCDatabase.Instance.MasterEquipments[EquipmentID];
				var flagship = KCDatabase.Instance.MasterShips[FlagshipID];

				EquipmentName = EquipmentID == -1 ? "(失敗)" : eq != null ? eq.Name : "???";
				FlagshipName = flagship != null ? flagship.NameWithClass : "???";
				FlagshipType = flagship != null ? flagship.ShipType : -1;
			}
		}



		public List<DevelopmentElement> Record { get; private set; }
		private DevelopmentElement tempElement;


		public DevelopmentRecord() {
			Record = new List<DevelopmentElement>();
			tempElement = null;

			APIObserver ao = APIObserver.Instance;

			ao.APIList["api_req_kousyou/createitem"].RequestReceived += DevelopmentStart;
			ao.APIList["api_req_kousyou/createitem"].ResponseReceived += DevelopmentEnd;

		}


		public DevelopmentElement this[int i] {
			get { return Record[i]; }
			set { Record[i] = value; }
		}


		private void DevelopmentStart( string apiname, dynamic data ) {

			tempElement = new DevelopmentElement();
			tempElement.Fuel = int.Parse( data["api_item1"] );
			tempElement.Ammo = int.Parse( data["api_item2"] );
			tempElement.Steel = int.Parse( data["api_item3"] );
			tempElement.Bauxite = int.Parse( data["api_item4"] );

		}

		private void DevelopmentEnd( string apiname, dynamic data ) {

			if ( tempElement == null ) return;

			if ( (int)data.api_create_flag == 0 ) {
				tempElement.EquipmentID = -1;
			} else {
				tempElement.EquipmentID = (int)data.api_slot_item.api_slotitem_id;
			}

			ShipData flagship =KCDatabase.Instance.Fleet[1].MembersInstance[0];
			tempElement.FlagshipID = flagship.ShipID;
			tempElement.HQLevel = KCDatabase.Instance.Admiral.Level;

			tempElement.SetSubParameters();

			Record.Add( tempElement );

			tempElement = null;
		}



		protected override void LoadLine( string line ) {
			Record.Add( new DevelopmentElement( line ) );
		}

		protected override string SaveLines() {

			StringBuilder sb = new StringBuilder();

			var list = new List<DevelopmentElement>( Record );
			list.Sort( ( e1, e2 ) => e1.Date.CompareTo( e2.Date ) );

			foreach ( var elem in list ) {
				sb.AppendLine( elem.SaveLine() );
			}

			return sb.ToString();
		}


		/*/
		protected override bool IsAppend { get { return true; } }


		public override bool Load( string path ) {
			return true;
		}

		public override bool Save( string path ) {
			bool ret = base.Save( path );

			Record.Clear();
			return ret;
		}
		//*/


		public override string RecordHeader {
			get { return "装備ID,装備名,開発日時,燃料,弾薬,鋼材,ボーキ,旗艦ID,旗艦名,旗艦艦種,司令部Lv"; }
		}

		public override string FileName {
			get { return "DevelopmentRecord.csv"; }
		}
	}


}
