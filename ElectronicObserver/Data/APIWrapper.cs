using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ElectronicObserver.Data {

	/// <summary>
	/// Responseに加え、Requestも扱うことのできるクラスの基底です。
	/// </summary>
	public abstract class APIWrapper : ResponseWrapper {

		public Dictionary<string, string> RequestData { get; private set; }


		public APIWrapper() 
			: base() {

			RequestData = new Dictionary<string, string>();
		
		}


		public virtual void LoadFromRequest( string apiname, Dictionary<string, string> data ) {

			/*
			// data is string:
			data = HttpUtility.UrlDecode( data );

			RequestData.Clear();
			foreach ( string unit in data.Split( "&".ToCharArray() ) ) {
				string[] pair = unit.Split( "=".ToCharArray() );
				RequestData.Add( pair[0], pair[1] );
			}
			*/

			RequestData = data;
		}

	}

}
