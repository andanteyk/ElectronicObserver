using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer {


	public class ResponseReceivedEventArgs : EventArgs {

		public string Name { get; private set; }
		public dynamic ResponseData { get; private set; }
		
		public ResponseReceivedEventArgs( string name, dynamic data ) {
			Name = name;
			ResponseData = data;
		}
	}


}
