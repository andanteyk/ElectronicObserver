using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.Utility.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Resource.Record
{

	/// <summary>
	/// 敵艦隊編成のレコードです。
	/// </summary>
	[DebuggerDisplay("{Record.Count} Records")]
	public class EnemyFleetRecord : RecordBase
	{

		public sealed class EnemyFleetElement : RecordElementBase
		{

			/// <summary>
			/// 艦隊ID
			/// </summary>
			public ulong FleetID { get; private set; }

			/// <summary>
			/// 艦隊名
			/// </summary>
			public string FleetName { get; private set; }

			/// <summary>
			/// 海域カテゴリID
			/// </summary>
			public int MapAreaID { get; private set; }

			/// <summary>
			/// 海域カテゴリ内番号
			/// </summary>
			public int MapInfoID { get; private set; }

			/// <summary>
			/// 海域セルID
			/// </summary>
			public int CellID { get; private set; }

			/// <summary>
			/// 海域難易度(甲乙丙)
			/// </summary>
			public int Difficulty { get; private set; }

			/// <summary>
			/// 陣形
			/// </summary>
			public int Formation { get; private set; }

			/// <summary>
			/// 敵艦船リスト [12]
			/// </summary>
			public int[] FleetMember { get; private set; }

			/// <summary>
			/// 敵艦船レベル [12]
			/// </summary>
			public int[] FleetMemberLevel { get; private set; }


			/// <summary>
			/// 艦娘の獲得経験値
			/// </summary>
			public int ExpShip { get; private set; }


			/// <summary>
			/// 連合艦隊かどうか
			/// </summary>
			public bool IsCombined => Formation >= 10;



			public EnemyFleetElement()
				: base() { }

			public EnemyFleetElement(string line)
				: this()
			{
				LoadLine(line);
			}

			public EnemyFleetElement(string fleetName, int mapAreaID, int mapInfoID, int cellID, int difficulty, int formation, int[] fleetMember, int[] fleetMemberLevel, int expShip)
				: base()
			{
				FleetName = fleetName;
				MapAreaID = mapAreaID;
				MapInfoID = mapInfoID;
				CellID = cellID;
				Difficulty = difficulty;
				Formation = formation;

				int[] To12Array(int[] a) => a.Length < 12 ? a.Concat(Enumerable.Repeat(-1, 12 - a.Length)).ToArray() : a.Take(12).ToArray();

				FleetMember = To12Array(fleetMember);
				FleetMemberLevel = To12Array(fleetMemberLevel);
				ExpShip = expShip;


				FleetID = ComputeHash();
			}


			public override void LoadLine(string line)
			{

				string[] elem = CsvHelper.ParseCsvLine(line).ToArray();
				if (elem.Length < 44)
					throw new ArgumentException("要素数が少なすぎます。");

				ulong id = Convert.ToUInt64(elem[0], 16);
				FleetName = elem[1];
				MapAreaID = int.Parse(elem[2]);
				MapInfoID = int.Parse(elem[3]);
				CellID = int.Parse(elem[4]);
				Difficulty = Constants.GetDifficulty(elem[5]);
				Formation = Constants.GetFormation(elem[6]);
				ExpShip = int.Parse(elem[7]);

				FleetMember = new int[12];
				for (int i = 0; i < FleetMember.Length; i++)
					FleetMember[i] = int.Parse(elem[8 + i]);

				FleetMemberLevel = new int[12];
				for (int i = 0; i < FleetMember.Length; i++)
					FleetMemberLevel[i] = int.Parse(elem[32 + i]);


				FleetID = ComputeHash();

				if (FleetID != id)
					Utility.Logger.Add(1, $"EnemyFleetRecord: 敵編成IDに誤りがあります。 (記録されているID {id:x16} -> 現在のID {FleetID:x16})");
			}

			public override string SaveLine()
			{
				return string.Join(",",
					FleetID.ToString("x16"),
					CsvHelper.EscapeCsvCell(FleetName),
					MapAreaID,
					MapInfoID,
					CellID,
					Constants.GetDifficulty(Difficulty),
					Constants.GetFormation(Formation),
					ExpShip,
					string.Join(",", FleetMember),
					string.Join(",", FleetMember.Select(id => CsvHelper.EscapeCsvCell(KCDatabase.Instance.MasterShips[id]?.NameWithClass ?? "-"))),
					string.Join(",", FleetMemberLevel)
					);
			}


			/// <summary>
			/// 現在のインスタンスのIDとなるハッシュ値を求めます。
			/// </summary>
			/// <returns></returns>
			private ulong ComputeHash()
			{
				string key = string.Join(",", MapAreaID, MapInfoID, CellID, Difficulty, Formation, string.Join(",", FleetMember), string.Join(",", FleetMemberLevel));
				return BitConverter.ToUInt64(Utility.Data.RecordHash.ComputeHash(key), 0);
			}


			/// <summary>
			/// 現在の状態からインスタンスを生成します。
			/// </summary>
			public static EnemyFleetElement CreateFromCurrentState()
			{

				var battle = KCDatabase.Instance.Battle;
				string fleetName = battle.IsBaseAirRaid ? "敵基地空襲" : battle.Result?.EnemyFleetName ?? "";
				int baseExp = battle.Result?.BaseExp ?? 0;
				var initial = battle.FirstBattle.Initial;

				if (battle.IsPractice)
					return null;


				return new EnemyFleetElement(
					fleetName,
					battle.Compass.MapAreaID,
					battle.Compass.MapInfoID,
					battle.IsBaseAirRaid ? -1 : battle.Compass.Destination,
					battle.Compass.MapInfo.EventDifficulty,
					battle.FirstBattle.Searching.FormationEnemy,
					battle.IsEnemyCombined ? initial.EnemyMembers.Take(6).Concat(initial.EnemyMembersEscort).ToArray() : initial.EnemyMembers,
					battle.IsEnemyCombined ? initial.EnemyLevels.Take(6).Concat(initial.EnemyLevelsEscort).ToArray() : initial.EnemyLevels,
					baseExp);

			}

			public override string ToString() => $"[{FleetID:x16}] {MapAreaID}-{MapInfoID}-{CellID} {FleetName}";
		}



		public Dictionary<ulong, EnemyFleetElement> Record { get; private set; }
		private bool _changed;


		public EnemyFleetRecord()
			: base()
		{
			Record = new Dictionary<ulong, EnemyFleetElement>();
			_changed = false;
		}

		public override void RegisterEvents()
		{
			// nop
		}


		public EnemyFleetElement this[ulong i]
		{
			get
			{
				return Record.ContainsKey(i) ? Record[i] : null;
			}
			set
			{
				if (!Record.ContainsKey(i))
				{
					Record.Add(i, value);
				}
				else
				{
					Record[i] = value;
				}
				_changed = true;
			}
		}


		public void Update(EnemyFleetElement elem)
		{
			this[elem.FleetID] = elem;
		}


		protected override void LoadLine(string line)
		{
			Update(new EnemyFleetElement(line));
		}

		protected override string SaveLinesAll()
		{
			var sb = new StringBuilder();

			var rs = Record.Values
				.OrderBy(r => r.MapAreaID)
				.ThenBy(r => r.MapInfoID)
				.ThenBy(r => r.CellID)
				.ThenBy(r => r.Difficulty);

			for (int i = 0; i < 12; i++)
			{
				int ii = i;
				rs = rs.ThenBy(r => r.FleetMember[ii]);
			}

			rs = rs
				.ThenBy(r => r.Formation)
				.ThenBy(r => r.ExpShip);

			foreach (var elem in rs)
				sb.AppendLine(elem.SaveLine());


			return sb.ToString();
		}

		protected override string SaveLinesPartial()
		{
			throw new NotSupportedException();
		}

		protected override void UpdateLastSavedIndex()
		{
			_changed = false;
		}

		public override bool NeedToSave => _changed;

		public override bool SupportsPartialSave => false;

		protected override void ClearRecord()
		{
			Record.Clear();
		}


		public override string RecordHeader => "敵編成ID,敵艦隊名,海域,海域,セル,難易度,陣形,艦娘経験値,ID#01,ID#02,ID#03,ID#04,ID#05,ID#06,ID#07,ID#08,ID#09,ID#10,ID#11,ID#12,艦名#01,艦名#02,艦名#03,艦名#04,艦名#05,艦名#06,艦名#07,艦名#08,艦名#09,艦名#10,艦名#11,艦名#12,Lv#01,Lv#02,Lv#03,Lv#04,Lv#05,Lv#06,Lv#07,Lv#08,Lv#09,Lv#10,Lv#11,Lv#12";

		public override string FileName => "EnemyFleetRecord.csv";
	}


}
