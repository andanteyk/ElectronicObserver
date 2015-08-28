using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronicObserver.Utility.Data
{
	public static class CalculatorEx
    {

		public static double CalculateFire( ShipData ship )
		{
			return Math.Floor( ( ship.FirepowerTotal + ship.TorpedoTotal ) * 1.5 + ship.BombTotal * 2 + 50 );
		}


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

		private static readonly Dictionary<int, int[]> ProficiencyArray = new Dictionary<int, int[]>
		{
			{ 6, new [] { 0, 1, 4, 6, 11, 16, 17, 25 } },
			{ 7, new [] { 0, 1, 1, 1, 2, 2, 2, 3 } },
			{ 8, new [] { 0, 1, 1, 1, 2, 2, 2, 3 } },
			{ 11, new [] { 0, 1, 2, 2, 4, 4, 4, 9 } }
		};

		public static int GetAirSuperiorityEnhance( EquipmentData[] slot, int[] aircraft )
		{

			int air = 0;
			int length = Math.Min( slot.Length, aircraft.Length );

			for ( int s = 0; s < length; s++ )
			{

				if ( aircraft[s] < 0 )
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
