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


		#region Events

		public delegate void DatabaseUpdatedEventHandler( DatabaseUpdatedEventArgs e );

		//private DatabaseUpdatedEventHandler _fleetUpdated = delegate { };

		public event DatabaseUpdatedEventHandler MasterDataUpdated = delegate { };
		public event DatabaseUpdatedEventHandler ShipsUpdated = delegate { };
		public event DatabaseUpdatedEventHandler EquipmentsUpdated = delegate { };
		public event DatabaseUpdatedEventHandler AdmiralUpdated = delegate { };
		public event DatabaseUpdatedEventHandler UseItemsUpdated = delegate { };
		public event DatabaseUpdatedEventHandler ArsenalsUpdated = delegate { };
		public event DatabaseUpdatedEventHandler DocksUpdated = delegate { };
		public event DatabaseUpdatedEventHandler FleetUpdated = delegate { };
		
		public event DatabaseUpdatedEventHandler MaterialUpdated = delegate { };
		public event DatabaseUpdatedEventHandler QuestUpdated = delegate { };

		internal void OnMasterDataUpdated() { MasterDataUpdated( new DatabaseUpdatedEventArgs() ); }
		internal void OnShipsUpdated() { ShipsUpdated( new DatabaseUpdatedEventArgs() ); }
		internal void OnEquipmentsUpdated() { EquipmentsUpdated( new DatabaseUpdatedEventArgs() ); }
		internal void OnAdmiralUpdated() { AdmiralUpdated( new DatabaseUpdatedEventArgs() ); }
		internal void OnUseItemsUpdated() { UseItemsUpdated( new DatabaseUpdatedEventArgs() ); }
		internal void OnArsenalsUpdated() { ArsenalsUpdated( new DatabaseUpdatedEventArgs() ); }
		internal void OnDocksUpdated() { DocksUpdated( new DatabaseUpdatedEventArgs() ); }
		internal void OnFleetUpdated() { FleetUpdated( new DatabaseUpdatedEventArgs() ); }
		internal void OnMaterialUpdated() { MaterialUpdated( new DatabaseUpdatedEventArgs() ); }
		internal void OnQuestUpdated() { QuestUpdated( new DatabaseUpdatedEventArgs() ); }

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
		//public IDDictionary<FleetData> Fleets { get; private set; }
		public FleetManager Fleet { get; private set; }


		/// <summary>
		/// 資源データ
		/// </summary>
		public MaterialData Material { get; private set; }


		/// <summary>
		/// 任務データ
		/// </summary>
		public QuestManager Quest { get; private set; }


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

		}

	}


}
