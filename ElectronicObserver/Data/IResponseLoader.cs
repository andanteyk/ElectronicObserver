using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {
	

	/// <summary>
	/// Responseを傍受し、データを更新できることを示します。
	/// </summary>
	[Obsolete( "開発初期の名残です", false )]	
	public interface IResponseLoader {
		bool LoadFromResponse( string apiname, dynamic data );
	}


}
