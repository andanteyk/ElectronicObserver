using Codeplex.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Resource.SaveData {

	public abstract class SaveData {

		public class InternalBaseData {

			public string TimeStamp {
				get { return DateTime.Now.ToString(); }
			}

		}

		protected InternalBaseData DataInstance;


		public void Load( string path ) {

			try {
				
				path = path + SaveFileName;
				

				using ( StreamReader sr = new StreamReader( path ) ) {

					dynamic tmp = DynamicJson.Parse( sr.ReadToEnd() );

					// -> DataInstance = tmp.Deserialize<DataInstance.GetType()>();
					DataInstance = tmp.GetType().GetMethod( "Deserialize" ).MakeGenericMethod( DataInstance.GetType() ).Invoke( tmp, new Object[] { } );
				}

			} catch ( Exception ) {

				ElectronicObserver.Utility.Logger.Add( 3, "ファイル " + path + " の読み込みに失敗しました。" );
			}
			

		}

		public void Save( string path ) {

			try {

				path = path + SaveFileName;
				

				string data = DynamicJson.Serialize( DataInstance );

				using ( StreamWriter sw = new StreamWriter( path ) ) {
					sw.Write( data );
				}

			} catch ( Exception ) {

				ElectronicObserver.Utility.Logger.Add( 3, "ファイル " + path + " の書き込みに失敗しました。" );
			}

			

		}


		public abstract string SaveFileName { get; }

	}


}
