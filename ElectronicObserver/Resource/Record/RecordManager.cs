using ElectronicObserver.Data;
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

			EnemyFleet.Load( MasterPath );
			ShipParameter.Load( MasterPath );
			//Construction.Load( MasterPath );
			//ShipDrop.Load( MasterPath );
			//Development.Load( MasterPath );
			//Resource.Load( MasterPath );

			//fixme: 読み込みに成功した時だけこれを表示できるように
			Utility.Logger.Add( 2, "レコードをロードしました。" );
		}

		public void Save() {

			//api_start2がロード済みのときのみ
			if ( KCDatabase.Instance.MasterShips.Count == 0 ) return;
				

			EnemyFleet.Save( MasterPath );
			ShipParameter.Save( MasterPath );
			Construction.Save( MasterPath );
			ShipDrop.Save( MasterPath );
			Development.Save( MasterPath );
			Resource.Save( MasterPath );

			Utility.Logger.Add( 2, "レコードをセーブしました。" );
		}

	}
}
