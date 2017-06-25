using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Resource.Record {

	/// <summary>
	/// 敵艦隊編成のレコードです。
	/// </summary>
	[DebuggerDisplay( "{Record.Count} Records" )]
	public class EnemyFleetRecord : RecordBase {

		[DebuggerDisplay( "[{ID}] : {FleetName}" )]
		public class EnemyFleetElement : RecordElementBase {

			/// <summary>
			/// 艦隊ID
			/// </summary>
			public uint FleetID { get { return ComputeHash(); } }

			/// <summary>
			/// 艦隊名
			/// </summary>
			public string FleetName { get; set; }

			/// <summary>
			/// 海域カテゴリID
			/// </summary>
			public int MapAreaID { get; set; }

			/// <summary>
			/// 海域カテゴリ内番号
			/// </summary>
			public int MapInfoID { get; set; }

			/// <summary>
			/// 海域セルID
			/// </summary>
			public int CellID { get; set; }

			/// <summary>
			/// 海域難易度(甲乙丙)
			/// </summary>
			public int Difficulty { get; set; }

			/// <summary>
			/// 陣形
			/// </summary>
			public int Formation { get; set; }

			/// <summary>
			/// 敵艦船リスト
			/// </summary>
			public int[] FleetMember { get; set; }

			/// <summary>
			/// 敵艦船名リスト
			/// </summary>
			public string[] FleetMemberName {
				get {
					return FleetMember.Select( id => KCDatabase.Instance.MasterShips[id] != null ? KCDatabase.Instance.MasterShips[id].NameWithClass : "-" ).ToArray();
				}
			}

			/// <summary>
			/// 艦娘の獲得経験値
			/// </summary>
			public int ExpShip { get; set; }



			public EnemyFleetElement()
				: base() { }

			public EnemyFleetElement( string line )
				: base( line ) { }

			public EnemyFleetElement( string fleetName, int mapAreaID, int mapInfoID, int cellID, int difficulty, int formation, int[] fleetMember, int expShip )
				: base() {
				FleetName = fleetName;
				MapAreaID = mapAreaID;
				MapInfoID = mapInfoID;
				CellID = cellID;
				Difficulty = difficulty;
				Formation = formation;
				FleetMember = fleetMember;
				ExpShip = expShip;
			}


			public override void LoadLine( string line ) {

				string[] elem = line.Split( ",".ToCharArray() );
				if ( elem.Length < 20 ) throw new ArgumentException( "要素数が少なすぎます。" );

				FleetName = elem[1];
				MapAreaID = int.Parse( elem[2] );
				MapInfoID = int.Parse( elem[3] );
				CellID = int.Parse( elem[4] );
				Difficulty = Constants.GetDifficulty( elem[5] );
				Formation = Constants.GetFormation( elem[6] );

				FleetMember = new int[6];
				for ( int i = 7; i < 7 + 6; i++ ) {
					FleetMember[i - 7] = int.Parse( elem[i] );
				}

				ExpShip = int.Parse( elem[19] );


				if ( FleetID != uint.Parse( elem[0] ) )
					Utility.Logger.Add( 1, string.Format( "EnemyFleetRecord: 敵編成IDに誤りがあります。 ({0:x8} -> {1:x8})", uint.Parse( elem[0] ), FleetID ) );
			}

			public override string SaveLine() {

				return string.Join( ",", FleetID, FleetName, SaveLinePart(), string.Join( ",", FleetMemberName ), ExpShip );
			}


			/// <summary>
			/// ハッシュ処理に用いる行を求めます。
			/// 処理の都合上、戦闘開始直後に判明するもののみ利用します。
			/// </summary>
			private string SaveLinePart() {

				return string.Join( ",", MapAreaID, MapInfoID, CellID, Constants.GetDifficulty( Difficulty ), Constants.GetFormation( Formation ),
					string.Join( ",", FleetMember ) );

			}


			/// <summary>
			/// 現在のインスタンスのIDとなるハッシュ値を求めます。
			/// </summary>
			/// <returns></returns>
			private uint ComputeHash() {

				byte[] hash = ElectronicObserver.Utility.Data.RecordHash.ComputeHash( SaveLinePart() );
				return (uint)hash[0] << 24 | (uint)hash[1] << 16 | (uint)hash[2] << 8 | (uint)hash[3];

			}


			/// <summary>
			/// 現在の状態からインスタンスを生成します。
			/// </summary>
			public static EnemyFleetElement CreateFromCurrentState() {

				var battle = KCDatabase.Instance.Battle;
				string fleetName = battle.Result != null ? battle.Result.EnemyFleetName : "";
				int baseExp = battle.Result != null ? battle.Result.BaseExp : 0;

				if ( battle.IsPractice )
					return null;


				return new EnemyFleetElement(
					fleetName,
					battle.Compass.MapAreaID,
					battle.Compass.MapInfoID,
					battle.Compass.Destination,
					battle.Compass.MapInfo.EventDifficulty,
					battle.FirstBattle.Searching.FormationEnemy,
					battle.FirstBattle.Initial.EnemyMembers,
					baseExp );

			}


		}



		public Dictionary<uint, EnemyFleetElement> Record { get; private set; }
		private bool _changed;


		public EnemyFleetRecord()
			: base() {
			Record = new Dictionary<uint, EnemyFleetElement>();
			_changed = false;
		}

		public override void RegisterEvents() {
			// nop
		}


		public EnemyFleetElement this[uint i] {
			get {
				return Record.ContainsKey( i ) ? Record[i] : null;
			}
			set {
				if ( !Record.ContainsKey( i ) ) {
					Record.Add( i, value );
				} else {
					Record[i] = value;
				}
				_changed = true;
			}
		}


		public void Update( EnemyFleetElement elem ) {
			this[elem.FleetID] = elem;
		}


		protected override void LoadLine( string line ) {
			Update( new EnemyFleetElement( line ) );
		}

		protected override string SaveLinesAll() {
			var sb = new StringBuilder();
			foreach ( var elem in Record.Values
				.OrderBy( r => r.MapAreaID )
				.ThenBy( r => r.MapInfoID )
				.ThenBy( r => r.CellID )
				.ThenBy( r => r.Difficulty )
				.ThenBy( r => r.FleetMember[0] )
				.ThenBy( r => r.FleetMember[1] )
				.ThenBy( r => r.FleetMember[2] )
				.ThenBy( r => r.FleetMember[3] )
				.ThenBy( r => r.FleetMember[4] )
				.ThenBy( r => r.FleetMember[5] )
				.ThenBy( r => r.Formation )
				) {
				sb.AppendLine( elem.SaveLine() );
			}
			return sb.ToString();
		}

		protected override string SaveLinesPartial() {
			throw new NotSupportedException();
		}

		protected override void UpdateLastSavedIndex() {
			_changed = false;
		}

		public override bool NeedToSave {
			get { return _changed; }
		}

		public override bool SupportsPartialSave {
			get { return false; }
		}

		protected override void ClearRecord() {
			Record.Clear();
		}


		public override string RecordHeader {
			get { return "敵編成ID,敵艦隊名,海域,海域,セル,難易度,陣形,敵1番艦,敵2番艦,敵3番艦,敵4番艦,敵5番艦,敵6番艦,敵1番艦名,敵2番艦名,敵3番艦名,敵4番艦名,敵5番艦名,敵6番艦名,経験値"; }
		}

		public override string FileName {
			get { return "EnemyFleetRecord.csv"; }
		}

	}

}
