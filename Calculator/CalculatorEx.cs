using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronicObserver.Utility.Data
{
	public static class CalculatorEx
    {
		/// <summary>
		/// 计算我方空母火力
		/// </summary>
		/// <param name="ship">舰船实体</param>
		public static double CalculateFire( ShipData ship )
		{
			return Math.Floor( ( ship.FirepowerTotal + ship.TorpedoTotal + Math.Floor( ship.BombTotal * 1.3 ) ) * 1.5 + 55 );
		}

		/// <summary>
		/// 计算敌舰空母火力
		/// </summary>
		/// <param name="shipID">敌舰ID</param>
		/// <param name="slot">装备ID</param>
		/// <param name="firepower">火力</param>
		/// <param name="torpedo">雷装</param>
		/// <returns></returns>
		public static double CalculateFireEnemy( int shipID, int[] slot, int firepower, int torpedo )
		{
			int fire = firepower;
			int tor = torpedo;
			int bomb = 0;
			if ( slot != null )
			{
				for ( int i = 0; i < slot.Length; i++ )
				{
					if ( slot[i] != -1 && KCDatabase.Instance.MasterEquipments[slot[i]] != null )
					{
						var eqmaster = KCDatabase.Instance.MasterEquipments[slot[i]];
						fire += eqmaster.Firepower;
						tor += eqmaster.Torpedo;
						bomb += eqmaster.Bomber;
					}
				}
			}
			return Math.Floor( ( fire + tor + Math.Floor( bomb * 1.3 ) ) * 1.5 + 55 );
		}

		/// <summary>
		/// 计算我方加权对空
		/// </summary>
		/// <param name="ship"></param>
		/// <returns></returns>
		public static double CalculateWeightingAA( ShipData ship )
		{
			double aatotal = ship.AABase;
			foreach ( var eq in ship.AllSlotInstance )
			{
				if ( eq == null )
					continue;

				int ratio;
				var eqmaster = eq.MasterEquipment;
				switch ( eqmaster.IconType )
				{
					case 15:	// 对空机枪
						ratio = 6;
						break;

					case 16:	// 高角炮
					case 30:	// 高射装置
						ratio = 4;
						break;

					case 11:	// 电探
						ratio = ( eqmaster.AA > 0 ) ? 3 : 0;
						break;

					default:
						ratio = 0;
						break;
				}
				if ( ratio <= 0 )
					continue;

				aatotal += ratio * ( eqmaster.AA + 0.7 * Math.Sqrt( eq.Level ) );
			}
			return aatotal;
		}

		/// <summary>
		/// 计算敌方加权对空
		/// </summary>
		/// <param name="shipID">敌舰ID</param>
		/// <param name="slot">装备ID</param>
		/// <param name="aa">对空</param>
		public static double CalculateWeightingAAEnemy( int shipID, int[] slot, int aa )
		{
			double aatotal = aa;
			if ( slot != null )
			{
				var slots = slot.Select( id => KCDatabase.Instance.MasterEquipments[id] );
				foreach ( var eqmaster in slots )
				{
					if ( eqmaster == null )
						continue;

					int ratio;
					switch ( eqmaster.IconType )
					{
						case 15:	// 对空机枪
							ratio = 6;
							break;

						case 16:	// 高角炮
						case 30:	// 高射装置
							ratio = 4;
							break;

						case 11:	// 电探
							ratio = ( eqmaster.AA > 0 ) ? 3 : 0;
							break;

						default:
							ratio = 0;
							break;
					}
					if ( ratio <= 0 )
						continue;

					aatotal += ratio * eqmaster.AA;
				}
			}
			return aatotal / 2;
		}

		/// <summary>
		/// 阵形防空补正系数
		/// </summary>
		private static readonly double[] FormationAA = new double[]
		{
			45, 35,	// 单纵
			41,		// 复纵
			55,		// 轮型
			35,		// 梯形
			35,		// 单横
			0, 0, 0, 0, 0,
			35,			// 第一警戒航行序列
			41,			// 第二警戒航行序列
			55,			// 第三警戒航行序列
			35			// 第四警戒航行序列
		};

		/// <summary>
		/// 获取我方舰队防空值
		/// </summary>
		/// <param name="fleet">舰队实体</param>
		/// <param name="formation">阵形id</param>
		public static double GetFleetAAValue( FleetData fleet, int formation )
		{
			if ( formation < 0 || formation > 14 )
				return 0;

			double aatotal = fleet.MembersWithoutEscaped.Sum( ship => ship == null ? 0.0 : GetShipAAValue( ship ) );
			return Math.Floor( aatotal * FormationAA[formation] * 20 / 45 ) / 10;
		}

		/// <summary>
		/// 获取我方舰娘防空值
		/// </summary>
		/// <param name="ship">舰娘实体</param>
		public static double GetShipAAValue( ShipData ship )
		{
			double aavalue = 0;
			foreach ( var eq in ship.AllSlotInstance )
			{
				if ( eq == null )
					continue;

				double ratio;
				var eqmaster = eq.MasterEquipment;
				switch ( eqmaster.IconType )
				{
					case 1:		// 小口径主炮
					case 2:		// 中口径主炮
					case 3:		// 大口径主炮
					case 4:		// 副炮
					case 15:	// 对空机枪
						ratio = 0.2;
						break;

					case 16:	// 高角炮
					case 30:	// 高射装置
						ratio = 0.35;
						break;

					case 11:	// 电探
						ratio = ( eqmaster.AA > 0 ) ? 0.4 : 0;
						break;

					case 12:	// 対空強化弾
						ratio = 0.6;
						break;

					default:
						ratio = 0;
						break;
				}
				if ( ratio <= 0 )
					continue;

				aavalue += ratio * eqmaster.AA;
			}
			return aavalue;
		}

		/// <summary>
		/// 获取敌舰队防空值
		/// </summary>
		/// <param name="fleet">敌舰成员ID</param>
		/// <param name="formation">敌舰阵形</param>
		public static double GetEnemyFleetAAValue( int[] fleet, int formation )
		{
			if ( formation < 0 || formation > 14 )
				return 0;

			double aatotal = 0;
			foreach ( var ship in fleet.Select( id => KCDatabase.Instance.MasterShips[id] ) )
			{
				if ( ship == null || ship.DefaultSlot == null )
					continue;

				foreach ( var eqmaster in ship.DefaultSlot.Select( eid => KCDatabase.Instance.MasterEquipments[eid] ) )
				{
					if ( eqmaster == null )
						continue;

					double ratio;
					switch ( eqmaster.IconType )
					{
						case 1:		// 小口径主炮
						case 2:		// 中口径主炮
						case 3:		// 大口径主炮
						case 4:		// 副炮
						case 15:	// 对空机枪
							ratio = 0.2;
							break;

						case 16:	// 高角炮
						case 30:	// 高射装置
							ratio = 0.35;
							break;

						case 11:	// 电探
							ratio = ( eqmaster.AA > 0 ) ? 0.4 : 0;
							break;

						case 12:	// 対空強化弾
							ratio = 0.6;
							break;

						default:
							ratio = 0;
							break;
					}
					if ( ratio <= 0 )
						continue;

					aatotal += ratio * eqmaster.AA;
				}

			}
			return Math.Floor( aatotal * FormationAA[formation] * 10 / 45 ) / 10;
		}



		/// <summary>
		/// 计算我方舰队制空值
		/// </summary>
		/// <param name="fleet">舰队实体</param>
		public static int GetAirSuperiorityEnhance( FleetData fleet )
		{
			int air = 0;

			foreach ( var ship in fleet.MembersWithoutEscaped )
			{
				if ( ship == null )
					continue;

				air += GetAirSuperiorityEnhance( ship.SlotInstance.ToArray(), ship.Aircraft.ToArray() );
			}

			return air;
		}

		/// <summary>
		/// 熟练度制空补正值
		/// </summary>
		private static readonly Dictionary<int, int[]> ProficiencyArray = new Dictionary<int, int[]>
		{
			{ 6, new [] { 0, 1, 4, 6, 11, 16, 17, 25 } },
			{ 7, new [] { 0, 1, 1, 1, 2, 2, 2, 3 } },
			{ 8, new [] { 0, 1, 1, 1, 2, 2, 2, 3 } },
			{ 11, new [] { 0, 1, 2, 2, 4, 4, 4, 9 } }
		};

		/// <summary>
		/// 获取一些装备的综合制空值
		/// </summary>
		/// <param name="slot">装备实体</param>
		/// <param name="aircraft">对应搭载量</param>
		public static int GetAirSuperiorityEnhance( EquipmentData[] slot, int[] aircraft )
		{

			int air = 0;
			int length = Math.Min( slot.Length, aircraft.Length );

			for ( int s = 0; s < length; s++ )
			{

				if ( aircraft[s] <= 0 )
					continue;

				EquipmentData equip = slot[s];
				if ( equip == null )
					continue;

				EquipmentDataMaster eq = equip.MasterEquipment;
				if ( eq == null )
					continue;

				switch ( eq.EquipmentType[2] )
				{
					case 6:
					case 7:
					case 8:
					case 11:
						air += (int)( eq.AA * Math.Sqrt( aircraft[s] ) );
						if ( equip.AircraftLevel > 0 && equip.AircraftLevel <= 7 )
							air += ProficiencyArray[eq.EquipmentType[2]][equip.AircraftLevel];
						break;
				}
			}

			return air;
		}

    }
}
