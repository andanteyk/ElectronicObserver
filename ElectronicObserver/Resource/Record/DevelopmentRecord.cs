using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Utility.Mathematics;
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
	/// 建造のレコードです。
	/// </summary>
	[DebuggerDisplay("{Record.Count} Records")]
	public class DevelopmentRecord : RecordBase
	{

		[DebuggerDisplay("[{EquipmentID}] : {EquipmentName}")]
		public sealed class DevelopmentElement : RecordElementBase
		{

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



			public DevelopmentElement()
			{
				EquipmentID = -1;
				Date = DateTime.Now;
			}

			public DevelopmentElement(string line)
				: this()
			{
				LoadLine(line);
			}

			public DevelopmentElement(int equipmentID, int fuel, int ammo, int steel, int bauxite, int flagshipID, int hqLevel)
			{
				EquipmentID = equipmentID;
				Fuel = fuel;
				Ammo = ammo;
				Steel = steel;
				Bauxite = bauxite;
				FlagshipID = flagshipID;
				HQLevel = hqLevel;

				SetSubParameters();
			}


			public override void LoadLine(string line)
			{

				string[] elem = CsvHelper.ParseCsvLine(line).ToArray();
				if (elem.Length < 11) throw new ArgumentException("要素数が少なすぎます。");

				EquipmentID = int.Parse(elem[0]);
				EquipmentName = elem[1];
				Date = DateTimeHelper.CSVStringToTime(elem[2]);
				Fuel = int.Parse(elem[3]);
				Ammo = int.Parse(elem[4]);
				Steel = int.Parse(elem[5]);
				Bauxite = int.Parse(elem[6]);
				FlagshipID = int.Parse(elem[7]);
				FlagshipName = elem[8];
				FlagshipType = int.Parse(elem[9]);
				HQLevel = int.Parse(elem[10]);

			}

			public override string SaveLine()
			{

				return string.Join(",",
					EquipmentID,
					CsvHelper.EscapeCsvCell(EquipmentName),
					DateTimeHelper.TimeToCSVString(Date),
					Fuel,
					Ammo,
					Steel,
					Bauxite,
					FlagshipID,
					CsvHelper.EscapeCsvCell(FlagshipName),
					FlagshipType,
					HQLevel);
			}

			/// <summary>
			/// 艦名などのパラメータを現在のIDをもとに設定します。
			/// </summary>
			public void SetSubParameters()
			{
				var eq = KCDatabase.Instance.MasterEquipments[EquipmentID];
				var flagship = KCDatabase.Instance.MasterShips[FlagshipID];

				EquipmentName = EquipmentID == -1 ? "(失敗)" :
					eq?.Name ?? "???";
				FlagshipName = flagship?.NameWithClass ?? "???";
				FlagshipType = (int?)flagship?.ShipType ?? -1;
			}
		}



		public List<DevelopmentElement> Record { get; private set; }
		private int LastSavedCount;


		public DevelopmentRecord()
		{
			Record = new List<DevelopmentElement>();
		}

		public override void RegisterEvents()
		{
			APIObserver.Instance["api_req_kousyou/createitem"].ResponseReceived += DevelopmentEnd;
		}


		public DevelopmentElement this[int i]
		{
			get { return Record[i]; }
			set { Record[i] = value; }
		}



		private void DevelopmentEnd(string apiname, dynamic data)
		{
			var dev = KCDatabase.Instance.Development;
			var flagshipID = KCDatabase.Instance.Fleet[1].MembersInstance[0].ShipID;
			var hqLevel = KCDatabase.Instance.Admiral.Level;

			foreach (var result in dev.Results)
			{
				var element = new DevelopmentElement
				{
					Fuel = dev.Fuel,
					Ammo = dev.Ammo,
					Steel = dev.Steel,
					Bauxite = dev.Bauxite,

					EquipmentID = result.EquipmentID,
					FlagshipID = flagshipID,
					HQLevel = hqLevel,
				};

				element.SetSubParameters();
				Record.Add(element);
			}
		}



		protected override void LoadLine(string line)
		{
			Record.Add(new DevelopmentElement(line));
		}

		protected override string SaveLinesAll()
		{
			var sb = new StringBuilder();
			foreach (var elem in Record.OrderBy(r => r.Date))
			{
				sb.AppendLine(elem.SaveLine());
			}
			return sb.ToString();
		}

		protected override string SaveLinesPartial()
		{
			var sb = new StringBuilder();
			foreach (var elem in Record.Skip(LastSavedCount).OrderBy(r => r.Date))
			{
				sb.AppendLine(elem.SaveLine());
			}
			return sb.ToString();
		}

		protected override void UpdateLastSavedIndex()
		{
			LastSavedCount = Record.Count;
		}

		public override bool NeedToSave => LastSavedCount < Record.Count;

		public override bool SupportsPartialSave => true;

		protected override void ClearRecord()
		{
			Record.Clear();
			LastSavedCount = 0;
		}


		public override string RecordHeader => "装備ID,装備名,開発日時,燃料,弾薬,鋼材,ボーキ,旗艦ID,旗艦名,旗艦艦種,司令部Lv";

		public override string FileName => "DevelopmentRecord.csv";
	}


}
