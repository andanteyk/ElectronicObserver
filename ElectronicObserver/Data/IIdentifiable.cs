using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	/// <summary>
	/// 固有のIDを取得できることを示します。
	/// </summary>
	public interface IIdentifiable {

		int ID { get; }

	}

}
