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

			bool successed = true;

			successed &= EnemyFleet.Load( MasterPath );
			successed &= ShipParameter.Load( MasterPath );
			successed &= Construction.Load( MasterPath );
			successed &= ShipDrop.Load( MasterPath );
			successed &= Development.Load( MasterPath );
			successed &= Resource.Load( MasterPath );

			if ( successed )
				Utility.Logger.Add( 2, "レコードをロードしました。" );
			else
				Utility.Logger.Add( 3, "レコードのロードに失敗しました。" );
		}

		public void Save() {

			//api_start2がロード済みのときのみ
			if ( KCDatabase.Instance.MasterShips.Count == 0 ) return;

			bool successed = true;


			successed &= EnemyFleet.Save( MasterPath );
			successed &= ShipParameter.Save( MasterPath );
			successed &= Construction.Save( MasterPath );
			successed &= ShipDrop.Save( MasterPath );
			successed &= Development.Save( MasterPath );
			successed &= Resource.Save( MasterPath );

			if ( successed )
				Utility.Logger.Add( 2, "レコードをセーブしました。" );
			else
				Utility.Logger.Add( 2, "レコードのセーブに失敗しました。" );

		}

	}
}
