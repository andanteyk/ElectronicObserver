using ElectronicObserver.Data;
using ElectronicObserver.Utility.Mathematics;
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


		private DateTime _prevTime;

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

			_prevTime = DateTime.Now;
			Observer.APIObserver.Instance["api_port/port"].ResponseReceived += TimerSave;
		}

		public bool Load( bool logging = true ) {

			bool succeeded = true;

			ResourceManager.CopyDocumentFromArchive( "Record/" + ShipParameter.FileName, MasterPath + "\\" + ShipParameter.FileName );

			succeeded &= EnemyFleet.Load( MasterPath );
			succeeded &= ShipParameter.Load( MasterPath );
			succeeded &= Construction.Load( MasterPath );
			succeeded &= ShipDrop.Load( MasterPath );
			succeeded &= Development.Load( MasterPath );
			succeeded &= Resource.Load( MasterPath );

			if ( logging ) {
				if ( succeeded )
					Utility.Logger.Add( 2, "レコードをロードしました。" );
				else
					Utility.Logger.Add( 3, "レコードのロードに失敗しました。" );
			}

			return succeeded;
		}


		public bool Save( bool logging = true ) {

			//api_start2がロード済みのときのみ
			if ( KCDatabase.Instance.MasterShips.Count == 0 ) return false;

			bool succeeded = true;


			succeeded &= EnemyFleet.Save( MasterPath );
			succeeded &= ShipParameter.Save( MasterPath );
			succeeded &= Construction.Save( MasterPath );
			succeeded &= ShipDrop.Save( MasterPath );
			succeeded &= Development.Save( MasterPath );
			succeeded &= Resource.Save( MasterPath );

			if ( logging ) {
				if ( succeeded )
					Utility.Logger.Add( 2, "レコードをセーブしました。" );
				else
					Utility.Logger.Add( 2, "レコードのセーブに失敗しました。" );
			}

			return succeeded;
		}


		void TimerSave( string apiname, dynamic data ) {

			bool iscleared;

			switch ( Utility.Configuration.Config.Control.RecordAutoSaving ) {
				case 0:
				default:
					iscleared = false;
					break;
				case 1:
					iscleared = DateTimeHelper.IsCrossedHour( _prevTime );
					break;
				case 2:
					iscleared = DateTimeHelper.IsCrossedDay( _prevTime, 0, 0, 0  );
					break;
			}


			if ( iscleared ) {
				_prevTime = DateTime.Now;

				if ( Save( false ) ) {
					Utility.Logger.Add( 1, "レコードのオートセーブを行いました。" );
				} else {
					Utility.Logger.Add( 3, "レコードのオートセーブに失敗しました。" );
				}
			}
		}

	}
}
