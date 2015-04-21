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
	public class ConstructionRecord : RecordBase {

		[DebuggerDisplay( "[{ShipID}] : {ShipName}" )]
		public class ConstructionElement : RecordElementBase {

			/// <summary>
			/// 建造した艦のID
			/// </summary>
			public int ShipID { get; set; }

			/// <summary>
			/// 建造した艦の名前
			/// </summary>
			public string ShipName {
				get {
					ShipDataMaster ship = KCDatabase.Instance.MasterShips[ShipID];
					return ship != null ? ship.NameWithClass : "???";
				}
			}

			/// <summary>
			/// 建造日時
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
			/// 投入開発資材
			/// </summary>
			public int DevelopmentMaterial { get; set; }

			/// <summary>
			/// 大型艦建造かのフラグ
			/// </summary>
			public bool IsLargeDock { get { return Fuel >= 1000; } }

			/// <summary>
			/// 空きドック数
			/// </summary>
			public int EmptyDockAmount { get; set; }

			/// <summary>
			/// 旗艦の艦船ID
			/// </summary>
			public int FlagshipID { get; set; }

			/// <summary>
			/// 旗艦の艦名
			/// </summary>
			public string FlagshipName {
				get {
					ShipDataMaster ship = KCDatabase.Instance.MasterShips[FlagshipID];
					return ship != null ? ship.NameWithClass : "???";
				}
			}

			/// <summary>
			/// 司令部Lv.
			/// </summary>
			public int HQLevel { get; set; }



			public ConstructionElement() {
				ShipID = -1;
				Date = DateTime.Now;
			}

			public ConstructionElement( string line )
				: base( line ) { }

			public ConstructionElement( int shipID, int fuel, int ammo, int steel, int bauxite, int developmentMaterial, int emptyDock, int flagshipID, int hqLevel ) {
				ShipID = shipID;
				Date = DateTime.Now;
				Fuel = fuel;
				Ammo = ammo;
				Steel = steel;
				Bauxite = bauxite;
				DevelopmentMaterial = developmentMaterial;
				EmptyDockAmount = emptyDock;
				FlagshipID = flagshipID;
				HQLevel = hqLevel;
			}


			public override void LoadLine( string line ) {

				string[] elem = line.Split( ",".ToCharArray() );
				if ( elem.Length < 13 ) throw new ArgumentException( "要素数が少なすぎます。" );

				ShipID = int.Parse( elem[0] );
				//ShipName=elem[1]は読み飛ばす
				Date = DateTimeHelper.CSVStringToTime( elem[2] );
				Fuel = int.Parse( elem[3] );
				Ammo = int.Parse( elem[4] );
				Steel = int.Parse( elem[5] );
				Bauxite = int.Parse( elem[6] );
				DevelopmentMaterial = int.Parse( elem[7] );
				//IsLargeDock=elem[8]は読み飛ばす
				EmptyDockAmount = int.Parse( elem[9] );
				FlagshipID = int.Parse( elem[10] );
				//FlagshipName=elem[11]は読み飛ばす
				HQLevel = int.Parse( elem[12] );

			}

			public override string SaveLine() {
				return string.Format( "{" + string.Join( "},{", Enumerable.Range( 0, 13 ) ) + "}",
					ShipID,
					ShipName,
					DateTimeHelper.TimeToCSVString( Date ),
					Fuel,
					Ammo,
					Steel,
					Bauxite,
					DevelopmentMaterial,
					IsLargeDock ? 1 : 0,
					EmptyDockAmount,
					FlagshipID,
					FlagshipName,
					HQLevel );
			}
		}



		public List<ConstructionElement> Record { get; private set; }
		private int ConstructingDockID;



		public ConstructionRecord()
			: base() {
			Record = new List<ConstructionElement>();
			ConstructingDockID = -1;

			APIObserver ao = APIObserver.Instance;

			ao.APIList["api_req_kousyou/createship"].RequestReceived += ConstructionStart;
			ao.APIList["api_get_member/kdock"].ResponseReceived += ConstructionEnd;
		
		}

		
	
		public ConstructionElement this[int i] {
			get { return Record[i]; }
			set { Record[i] = value; }
		}



		void ConstructionStart( string apiname, dynamic data ) {

			ConstructingDockID = int.Parse( data["api_kdock_id"] );

		}

		void ConstructionEnd( string apiname, dynamic data ) {

			if ( ConstructingDockID == -1 ) return;

			ArsenalData a = KCDatabase.Instance.Arsenals[ConstructingDockID];
			int emptyDock = KCDatabase.Instance.Arsenals.Values.Count( c => c.State == 0 );
			ShipData flagship = KCDatabase.Instance.Ships[KCDatabase.Instance.Fleet[1].Members[0]];

			Record.Add( new ConstructionElement( a.ShipID, a.Fuel, a.Ammo, a.Steel, a.Bauxite, a.DevelopmentMaterial,
				emptyDock, flagship.ShipID, KCDatabase.Instance.Admiral.Level ) );

		}



		protected override void LoadLine( string line ) {
			Record.Add( new ConstructionElement( line ) );
		}

		protected override string SaveLines() {

			StringBuilder sb = new StringBuilder();

			var list = new List<ConstructionElement>( Record );
			list.Sort( ( e1, e2 ) => e1.Date.CompareTo( e2.Date ) );

			foreach ( var elem in list ) {
				sb.AppendLine( elem.SaveLine() );
			}

			return sb.ToString();
		}


		protected override void ClearRecord() {
			Record.Clear();
		}

		protected override bool IsAppend { get { return true; } }


		public override bool Load( string path ) {
			return true;
		}

		public override bool Save( string path ) {
			bool ret = base.Save( path );

			Record.Clear();
			return ret;
		}



		protected override string RecordHeader {
			get { return "艦船ID,艦船名,建造日時,燃料,弾薬,鋼材,ボーキ,開発資材,大型建造,空ドック,旗艦ID,旗艦名,司令部Lv"; }
		}

		public override string FileName {
			get { return "ConstructionRecord.csv"; }
		}
	}

}
