using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {


	/// <summary>
	/// 速力
	/// </summary>
	public enum ShipSpeed {
		Low,
		Fast,
	}


	/// <summary>
	/// 射程
	/// </summary>
	public enum ShipRange {
		None,
		Short,
		Middle,
		Long,
		VeryLong,
	}


	/// <summary>
	/// 艦船のレアリティ
	/// </summary>
	public enum ShipRarity {
		Black,
		DeepBlue,
		Blue,
		LightBlue,
		Silver,
		Gold,
		Rainbow,
		Shining,
		Cherry,
	}


	public enum EquipmentRarity {
		Common,
		Rare,
		Rainbow,
		RainbowS,
		RainbowSS,
		RainbowEx,
	}
}
