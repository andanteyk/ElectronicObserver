using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {
	

	public interface IResponseLoader {
		bool LoadFromResponse( dynamic data );
	}


}
