using ElectronicObserver.Utility.Storage;
using CustomShipGroup;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Data;

namespace CustomShipGroup.Model {

	/// <summary>
	/// 艦船グループのデータを管理します。
	/// </summary>
	[DataContract( Name = "CustomShipGroupManager" )]
	public class CustomShipGroupManager : DataStorage {

		public const string DefaultFilePath = @"Settings\CustomShipGroups.xml";


		/// <summary>
		/// 艦船グループリスト
		/// </summary>
		[IgnoreDataMember]
		public IDDictionary<CustomShipGroupData> ShipGroups { get; private set; }

		[DataMember]
		private List<CustomShipGroupData> ShipGroupsSerializer {
			get { return ShipGroups.Values.OrderBy( g => g.ID ).ToList(); }
			set { ShipGroups = new IDDictionary<CustomShipGroupData>( value ); }
		}

		public CustomShipGroupManager() {
			Initialize();
		}


		public override void Initialize() {
			ShipGroups = new IDDictionary<CustomShipGroupData>();
		}



		public CustomShipGroupData this[int index] {
			get {
				return ShipGroups[index];
			}
		}


		public CustomShipGroupData Add() {

			int key = GetUniqueID();
			var group = new CustomShipGroupData( key );
			ShipGroups.Add( group );
			return group;

		}

		public int GetUniqueID() {
			return ShipGroups.Count > 0 ? ShipGroups.Keys.Max() + 1 : 1;
		}


		public CustomShipGroupManager Load() {
			return (CustomShipGroupManager)Load( DefaultFilePath );
		}

		public void Save() {
			Save( DefaultFilePath );
		}

	}

}
