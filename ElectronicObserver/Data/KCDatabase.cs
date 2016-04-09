using ElectronicObserver.Data.Battle;
using ElectronicObserver.Data.Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {


	/// <summary>
	/// 艦これのデータを扱う中核です。
	/// </summary>
	public sealed class KCDatabase {


		#region Singleton

		private static readonly KCDatabase instance = new KCDatabase();

		public static KCDatabase Instance {
			get { return instance; }
		}

		#endregion



		/// <summary>
		/// 艦船のマスターデータ
		/// </summary>
		public IDDictionary<ShipDataMaster> MasterShips { get; private set; }

		/// <summary>
		/// 艦種データ
		/// </summary>
		public IDDictionary<ShipType> ShipTypes { get; private set; }

		/// <summary>
		/// 装備のマスターデータ
		/// </summary>
		public IDDictionary<EquipmentDataMaster> MasterEquipments { get; private set; }

		/// <summary>
		/// 装備種別
		/// </summary>
		public IDDictionary<EquipmentType> EquipmentTypes { get; private set; }


		/// <summary>
		/// 保有艦娘のデータ
		/// </summary>
		public IDDictionary<ShipData> Ships { get; private set; }

		/// <summary>
		/// 保有装備のデータ
		/// </summary>
		public IDDictionary<EquipmentData> Equipments { get; private set; }


		/// <summary>
		/// 提督・司令部データ
		/// </summary>
		public AdmiralData Admiral { get; private set; }


		/// <summary>
		/// アイテムのマスターデータ
		/// </summary>
		public IDDictionary<UseItemMaster> MasterUseItems { get; private set; }

		/// <summary>
		/// アイテムデータ
		/// </summary>
		public IDDictionary<UseItem> UseItems { get; private set; }


		/// <summary>
		/// 工廠ドックデータ
		/// </summary>
		public IDDictionary<ArsenalData> Arsenals { get; private set; }

		/// <summary>
		/// 入渠ドックデータ
		/// </summary>
		public IDDictionary<DockData> Docks { get; private set; }


		/// <summary>
		/// 艦隊データ
		/// </summary>
		public FleetManager Fleet { get; private set; }


		/// <summary>
		/// 資源データ
		/// </summary>
		public MaterialData Material { get; private set; }


		/// <summary>
		/// 任務データ
		/// </summary>
		public QuestManager Quest { get; private set; }

		/// <summary>
		/// 任務進捗データ
		/// </summary>
		public QuestProgressManager QuestProgress { get; private set; }


		/// <summary>
		/// 戦闘データ
		/// </summary>
		public BattleManager Battle { get; private set; }


		/// <summary>
		/// 海域データ
		/// </summary>
		public IDDictionary<MapInfoData> MapInfo { get; private set; }


		/// <summary>
		/// 遠征データ
		/// </summary>
		public IDDictionary<MissionData> Mission { get; private set; }


		/// <summary>
		/// 艦船グループデータ
		/// </summary>
		public ShipGroupManager ShipGroup { get; private set; }

        public Dictionary<int, string> BGM_List { get; private set; }

        private KCDatabase() {

			MasterShips = new IDDictionary<ShipDataMaster>();
			ShipTypes = new IDDictionary<ShipType>();
			MasterEquipments = new IDDictionary<EquipmentDataMaster>();
			EquipmentTypes = new IDDictionary<EquipmentType>();
			Ships = new IDDictionary<ShipData>();
			Equipments = new IDDictionary<EquipmentData>();
			Admiral = new AdmiralData();
			MasterUseItems = new IDDictionary<UseItemMaster>();
			UseItems = new IDDictionary<UseItem>();
			Arsenals = new IDDictionary<ArsenalData>();
			Docks = new IDDictionary<DockData>();
			Fleet = new FleetManager();
			Material = new MaterialData();
			Quest = new QuestManager();
			QuestProgress = new QuestProgressManager();
			Battle = new BattleManager();
			MapInfo = new IDDictionary<MapInfoData>();
			Mission = new IDDictionary<MissionData>();
			ShipGroup = new ShipGroupManager();
            BGM_List = new Dictionary<int, string>();

        }


		public void Load() {

			{
				var temp = (ShipGroupManager)ShipGroup.Load();
				if ( temp != null )
					ShipGroup = temp;
			}
			{
				var temp = QuestProgress.Load();
				if ( temp != null ) {
					if ( QuestProgress != null )
						QuestProgress.RemoveEvents();
					QuestProgress = temp;
				}
			}

		}

		public void Save() {
			ShipGroup.Save();
			QuestProgress.Save();
		}

	}


}
