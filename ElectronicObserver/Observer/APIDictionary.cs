using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer {


	public class APIDictionary : Dictionary<string, APIBase> {

		public APIDictionary()
			: this( new List<APIBase>() ) {
		}

		public APIDictionary( IEnumerable<APIBase> source )
			: base( source.ToDictionary( x => x.APIName ) ) {
		}


		internal void Add( APIBase data ) {
			Add( data.APIName, data );
		}

		internal void Remove( APIBase data ) {
			Remove( data.APIName );
		}


		public void OnRequestReceived( string apiname, Dictionary<string, string> data ) {
			if ( ContainsKey( apiname ) && this[apiname].IsRequestSupported ) {
				this[apiname].OnRequestReceived( data );
			}
		}

		public void OnResponseReceived( string apiname, dynamic data ) {
			if ( ContainsKey( apiname ) && this[apiname].IsResponseSupported ) {
				this[apiname].OnResponseReceived( data );
			}
		}


		public new APIBase this[string key] {
			get { return ContainsKey( key ) ? base[key] : null; }
		}
	}

}
