﻿using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Resource.Record {

	public sealed class RecordManager {

		#region Singleton

		private static readonly RecordManager instance = new RecordManager();

		public static RecordManager Instance {
			get { return instance; }
		}

		#endregion

		public string MasterPath { get; private set; }

		public EnemyFleetRecord EnemyFleet { get; private set; }
		public ShipParameterRecord ShipParameter { get; private set; }
		public ConstructionRecord Construction { get; private set; }
		public ShipDropRecord ShipDrop { get; private set; }
		public DevelopmentRecord Development { get; private set; }
		public ResourceRecord Resource { get; private set; }

		private RecordManager() {

			MasterPath = @"Record";
			EnemyFleet = new EnemyFleetRecord();
			ShipParameter = new ShipParameterRecord();
			Construction = new ConstructionRecord();
			ShipDrop = new ShipDropRecord();
			Development = new DevelopmentRecord();
			Resource = new ResourceRecord();

			if ( !Directory.Exists( MasterPath ) ) {
				Directory.CreateDirectory( MasterPath );
			}
		}


		public void Load() {

			bool succeeded = true;

			ResourceManager.CopyFromArchive( "Record/" + ShipParameter.FileName, MasterPath + "\\" + ShipParameter.FileName );

			succeeded &= EnemyFleet.Load( MasterPath );
			succeeded &= ShipParameter.Load( MasterPath );
			succeeded &= Construction.Load( MasterPath );
			succeeded &= ShipDrop.Load( MasterPath );
			succeeded &= Development.Load( MasterPath );
			succeeded &= Resource.Load( MasterPath );

			if ( succeeded )
				Utility.Logger.Add( 2, "记录已读取。" );
			else
				Utility.Logger.Add( 3, "记录读取失败。" );
		}

		public void Save() {

			//api_start2がロード済みのときのみ
			if ( KCDatabase.Instance.MasterShips.Count == 0 ) return;

			bool succeeded = true;


			succeeded &= EnemyFleet.Save( MasterPath );
			succeeded &= ShipParameter.Save( MasterPath );
			succeeded &= Construction.Save( MasterPath );
			succeeded &= ShipDrop.Save( MasterPath );
			succeeded &= Development.Save( MasterPath );
			succeeded &= Resource.Save( MasterPath );

			if ( succeeded )
				Utility.Logger.Add( 2, "记录已保存。" );
			else
				Utility.Logger.Add( 2, "记录保存失败。" );

		}

	}
}
