using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer {

	[Obsolete( "開発初期の名残です, 今となっては何に使うものやら覚えていません", false )]
	public class ResponseReceivedEventArgs : EventArgs {

		public string Name { get; private set; }
		public dynamic ResponseData { get; private set; }
		
		public ResponseReceivedEventArgs( string name, dynamic data ) {
			Name = name;
			ResponseData = data;
		}
	}


}
