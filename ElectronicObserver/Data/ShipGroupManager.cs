using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Storage;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	/// <summary>
	/// 艦船グループのデータを管理します。
	/// </summary>
	[DataContract( Name = "ShipGroupManager" )]
	public class ShipGroupManager : DataStorage {

		public const string DefaultFilePath = @"Settings\ShipGroups.xml";


		/// <summary>
		/// 艦船グループリスト
		/// </summary>
		[IgnoreDataMember]
		public IDDictionary<ShipGroupData> ShipGroups { get; private set; }


		[DataMember]
		private IEnumerable<ShipGroupData> ShipGroupsSerializer {
			get { return ShipGroups.Values.OrderBy( g => g.ID ); }
			set { ShipGroups = new IDDictionary<ShipGroupData>( value ); }
		}

		public ShipGroupManager() {
			Initialize();
		}


		public override void Initialize() {
			ShipGroups = new IDDictionary<ShipGroupData>();
		}



		public ShipGroupData this[int index] {
			get {
				return ShipGroups[index];
			}
		}


		public ShipGroupData Add() {

			int key = GetUniqueID();
			var group = new ShipGroupData( key );
			ShipGroups.Add( group );
			return group;

		}

		public int GetUniqueID() {
			return ShipGroups.Count > 0 ? ShipGroups.Keys.Max() + 1 : 1;
		}


		public ShipGroupManager Load() {

			ResourceManager.CopyDocumentFromArchive( DefaultFilePath.Replace( "\\", "/" ), DefaultFilePath );

			return (ShipGroupManager)Load( DefaultFilePath );
		}

		public void Save() {
			Save( DefaultFilePath );
		}

	}

}
