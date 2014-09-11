using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	/*
	 * ぶっちゃけconstが*非常に*面倒なので、使わない可能性が…？
	 */



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


	/// <summary>
	/// 装備のレアリティ
	/// </summary>
	public enum EquipmentRarity {
		Common,
		Rare,
		Rainbow,
		RainbowS,
		RainbowSS,
		RainbowEx,
	}


	/// <summary>
	/// 提督の階級
	/// </summary>
	public enum HQRank {
		MarshalAdmiral = 1,
		Admiral,
		ViceAdmiral,
		RearAdmiral,
		Captain,
		Commander,
		LieutenantCommander,
		MiddleCommander,
		NoviceCommander,
	}

}
