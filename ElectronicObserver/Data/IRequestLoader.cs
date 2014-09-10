using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	/// <summary>
	/// Requestを傍受し、データを更新できることを示します。
	/// </summary>
	[Obsolete( "開発初期の名残です", false )]
	public interface IRequestLoader {
		bool LoadFromRequest( string apiname, Dictionary<string, string> data );
	}

}
