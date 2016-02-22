using ElectronicObserver.Data;
using ElectronicObserver.Resource.Record;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility.Data {

	public static class CalculatorEx
	{
		static Dictionary<string, MethodInfo> methodInfos;
		static DateTime dateLast;

		static CalculatorEx()
		{
			methodInfos = new Dictionary<string, MethodInfo>();
			RefreshMethods();
		}

		private static bool RefreshMethods()
		{
			FileInfo file = new FileInfo( "CalculatorEx.cs" );
			if ( file.Exists && file.LastWriteTime > dateLast )
			{

				var provider = new CSharpCodeProvider();
				var parameters = new CompilerParameters()
				{
					CompilerOptions = "/target:library /optimize",
					GenerateExecutable = false,
					GenerateInMemory = true
				};
				if ( File.Exists( "CalculatorEx.lst" ) )
					parameters.ReferencedAssemblies.AddRange( File.ReadAllLines( "CalculatorEx.lst" ) );
				parameters.ReferencedAssemblies.Add( Assembly.GetExecutingAssembly().Location );

				var result = provider.CompileAssemblyFromFile( parameters, file.FullName );
				if ( result.Errors.HasErrors )
				{
					string err = "Errors:\r\n";
					foreach ( CompilerError e in result.Errors )
					{
						err += "\r\n" + e.ToString();
					}
					System.Windows.Forms.MessageBox.Show( err, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
				}
				else
				{
					dateLast = file.LastWriteTime;

					var ass = result.CompiledAssembly;
					var type = ass.GetType( "ElectronicObserver.Utility.Data.CalculatorEx" );

					methodInfos.Clear();
					foreach ( var m in type.GetMethods( BindingFlags.Public | BindingFlags.Static ) )
					{
						string key = m.Name;
						foreach ( var p in m.GetParameters() )
							key += "," + p.ParameterType.FullName;
						methodInfos.Add( key, m );
					}
				}

				return true;
			}

			return false;
		}


		/// <summary>
		/// 计算我方空母火力
		/// </summary>
		/// <param name="ship">舰船实体</param>
		public static double CalculateFire( ShipData ship )
		{
			RefreshMethods();
			MethodInfo m;
			if ( methodInfos.TryGetValue( "CalculateFire,ElectronicObserver.Data.ShipData", out m ) )
				return (double)m.Invoke( null, new object[] { ship } );

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
			RefreshMethods();
			MethodInfo m;
			if ( methodInfos.TryGetValue( "CalculateFireEnemy,System.Int32,System.Int32[],System.Int32,System.Int32", out m ) )
				return (double)m.Invoke( null, new object[] { shipID, slot, firepower, torpedo } );

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
			RefreshMethods();
			MethodInfo m;
			if ( methodInfos.TryGetValue( "CalculateWeightingAA,ElectronicObserver.Data.ShipData", out m ) )
				return (double)m.Invoke( null, new object[] { ship } );

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
			RefreshMethods();
			MethodInfo m;
			if ( methodInfos.TryGetValue( "CalculateWeightingAAEnemy,System.Int32,System.Int32[],System.Int32", out m ) )
				return (double)m.Invoke( null, new object[] { shipID, slot, aa } );

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
			45, 35,		// 单纵
			41,			// 复纵
			55,			// 轮型
			35,			// 梯形
			35,			// 单横
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

			RefreshMethods();
			MethodInfo m;
			if ( methodInfos.TryGetValue( "GetFleetAAValue,ElectronicObserver.Data.FleetData,System.Int32", out m ) )
				return (double)m.Invoke( null, new object[] { fleet, formation } );

			double aatotal = fleet.MembersWithoutEscaped.Sum( ship => ship == null ? 0.0 : GetShipAAValue( ship ) );
			return Math.Floor( aatotal * FormationAA[formation] * 20 / 45 ) / 10;
		}

		/// <summary>
		/// 获取我方舰娘防空值
		/// </summary>
		/// <param name="ship">舰娘实体</param>
		public static double GetShipAAValue( ShipData ship )
		{
			RefreshMethods();
			MethodInfo m;
			if ( methodInfos.TryGetValue( "GetShipAAValue,ElectronicObserver.Data.ShipData", out m ) )
				return (double)m.Invoke( null, new object[] { ship } );

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

			RefreshMethods();
			MethodInfo m;
			if ( methodInfos.TryGetValue( "GetEnemyFleetAAValue,System.Int32[],System.Int32", out m ) )
				return (double)m.Invoke( null, new object[] { fleet, formation } );

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
			return Math.Floor( aatotal * FormationAA[formation] * 20 / 45 ) / 10;
		}



		/// <summary>
		/// 计算我方舰队制空值
		/// </summary>
		/// <param name="fleet">舰队实体</param>
		public static int GetAirSuperiorityEnhance( FleetData fleet )
		{
			RefreshMethods();
			MethodInfo m;
			if ( methodInfos.TryGetValue( "GetAirSuperiorityEnhance,ElectronicObserver.Data.FleetData", out m ) )
				return (int)m.Invoke( null, new object[] { fleet } );


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
			RefreshMethods();
			MethodInfo m;
			if ( methodInfos.TryGetValue( "GetAirSuperiorityEnhance,ElectronicObserver.Data.EquipmentData[],System.Int32[]", out m ) )
				return (int)m.Invoke( null, new object[] { slot, aircraft } );

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

	/// <summary>
	/// 汎用計算クラス
	/// </summary>
	public static class Calculator {

		/// <summary>
		/// レベルに依存するパラメータ値を求めます。
		/// </summary>
		/// <param name="min">初期値。</param>
		/// <param name="max">最大値。</param>
		/// <param name="lv">レベル。</param>
		/// <returns></returns>
		public static int GetParameterFromLevel( int min, int max, int lv ) {
			return min + ( max - min ) * lv / 99;
		}




		/// <summary>
		/// 制空戦力を求めます。
		/// </summary>
		/// <param name="slot">装備スロット。</param>
		/// <param name="aircraft">艦載機搭載量の配列。</param>
		public static int GetAirSuperiority( int[] slot, int[] aircraft ) {

			int air = 0;
			int length = Math.Min( slot.Length, aircraft.Length );

			for ( int s = 0; s < length; s++ ) {

				if ( aircraft[s] < 0 )
					continue;

				EquipmentDataMaster eq = KCDatabase.Instance.MasterEquipments[slot[s]];

				if ( eq == null ) continue;

				switch ( eq.EquipmentType[2] ) {
					case 6:
					case 7:
					case 8:
					case 11:
						air += (int)( eq.AA * Math.Sqrt( aircraft[s] ) );
						break;
				}
			}

			return air;
		}

		/// <summary>
		/// 制空戦力を求めます。
		/// </summary>
		/// <param name="fleet">艦船IDの配列。</param>
		public static int GetAirSuperiority( int[] fleet ) {

			int air = 0;

			for ( int i = 0; i < fleet.Length; i++ ) {
				ShipDataMaster ship = KCDatabase.Instance.MasterShips[fleet[i]];
				if ( ship == null ) continue;

				air += GetAirSuperiority( ship );
			}

			return air;
		}

		/// <summary>
		/// 制空戦力を求めます。
		/// </summary>
		/// <param name="fleet">艦船IDの配列。</param>
		/// <param name="slot">各艦船の装備スロット。</param>
		public static int GetAirSuperiority( int[] fleet, int[][] slot ) {

			int air = 0;
			int length = Math.Min( fleet.Length, slot.GetLength( 0 ) );

			for ( int i = 0; i < length; i++ ) {
				ShipDataMaster ship = KCDatabase.Instance.MasterShips[fleet[i]];
				if ( ship == null ) continue;

				air += GetAirSuperiority( slot[i], ship.Aircraft.ToArray() );

			}

			return air;
		}


		/// <summary>
		/// 各装備カテゴリにおける制空値の熟練度ボーナス
		/// </summary>
		private static readonly Dictionary<int, int[]> AircraftLevelBonus = new Dictionary<int, int[]>() {
			{ 6, new int[] { 0, 0, 2, 5, 9, 14, 14, 22, 22 } },	//艦上戦闘機
			{ 7, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 } },		//艦上爆撃機
			{ 8, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 } },		//艦上攻撃機
			{ 11, new int[] { 0, 1, 1, 1, 1, 3, 3, 6, 6 } },		//水上爆撃機
		};

		/// <summary>
		/// 艦載機熟練度の内部値テーブル(仮)
		/// </summary>
		private static readonly List<int> AircraftExpTable = new List<int>() {
			0, 10, 25, 40, 55, 70, 85, 100, 120
		};


		/// <summary>
		/// 制空戦力を求めます。
		/// </summary>
		/// <param name="ship">対象の艦船。</param>
        public static int GetAirSuperiority(ShipData ship, int FullExp = 0)
        {

			if ( ship == null ) return 0;

            if (Utility.Configuration.Config.FormFleet.AirSuperiorityMethod == 0)
            {
                return GetAirSuperiority(ship.SlotMaster.ToArray(), ship.Aircraft.ToArray());
            }
            int air = 0;
            var eqs = ship.SlotInstance;
            var aircrafts = ship.Aircraft;


            for (int i = 0; i < eqs.Count; i++)
            {
                var eq = eqs[i];
                if (eq != null && aircrafts[i] > 0)
                {

                    int category = eq.MasterEquipment.CategoryType;

                    if (AircraftLevelBonus.ContainsKey(category))
                    {
                        if (CheckCategoryType(category, FullExp) && eq.AircraftLevel == 7)//如果选择了百战历练,对7级舰战应用120熟练度,其余情况不作处理
                            air += (int)(eq.MasterEquipment.AA * Math.Sqrt(aircrafts[i]) + Math.Sqrt(AircraftExpTable[eq.AircraftLevel + 1] / 10.0) + AircraftLevelBonus[category][eq.AircraftLevel]);
                        else
                            air += (int)(eq.MasterEquipment.AA * Math.Sqrt(aircrafts[i]) + Math.Sqrt(AircraftExpTable[eq.AircraftLevel] / 10.0) + AircraftLevelBonus[category][eq.AircraftLevel]);
                    }

                }
            }

            return air;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="category">装备类别</param>
        /// <param name="CategoryTypeList">1+2+4+8=舰战,舰爆,舰攻,水爆</param>
        /// <returns></returns>
        static bool CheckCategoryType(int category, int CategoryTypeList)
        {
           switch(category)
           {
               case 6:
                   return (CategoryTypeList & 1) > 0;
               case 7:
                   return (CategoryTypeList & 2) > 0;
               case 8:
                   return (CategoryTypeList & 4) > 0;
               case 11:
                   return (CategoryTypeList & 8) > 0;
               default:
                   return false;
           }
        }
		/// <summary>
		/// 制空戦力を求めます。
		/// </summary>
		/// <param name="slot">各スロットの装備IDリスト。</param>
		/// <param name="aircraft">艦載機搭載量。</param>
		/// <param name="level">各スロットの艦載機熟練度。</param>
		/// <returns></returns>
		public static int GetAirSuperiority( int[] slot, int[] aircraft, int[] level ) {
			int air = 0;

			for ( int i = 0; i < aircraft.Length; i++ ) {
				var eq = KCDatabase.Instance.MasterEquipments[slot[i]];
				if ( eq == null || aircraft[i] == 0 ) continue;

				int category = eq.CategoryType;
				if ( AircraftLevelBonus.ContainsKey( category ) ) {
					air += (int)( eq.AA * Math.Sqrt( aircraft[i] ) + Math.Sqrt( AircraftExpTable[level[i]] / 10.0 ) + AircraftLevelBonus[category][level[i]] );
				}
			}

			return air;
		}


		/// <summary>
		/// 最大練度の艦載機を搭載している場合の制空戦力を求めます。
		/// </summary>
		/// <param name="fleet">艦船IDリスト。</param>
		/// <param name="slot">各艦の装備IDリスト。</param>
		/// <returns></returns>
		public static int GetAirSuperiorityAtMaxLevel( int[] fleet, int[][] slot ) {
			return fleet.Select( id => KCDatabase.Instance.MasterShips[id] )
				.Select( ( ship, i ) => ship == null ? 0 : GetAirSuperiority( slot[i], ship.Aircraft.ToArray(), new int[] { 8, 8, 8, 8, 8 } ) ).Sum();
		}

		/// <summary>
		/// 制空戦力を求めます。
		/// </summary>
		/// <param name="ship">対象の艦船。</param>
		public static int GetAirSuperiority( ShipDataMaster ship ) {

			if ( ship.DefaultSlot == null ) return 0;
			return GetAirSuperiority( ship.DefaultSlot.ToArray(), ship.Aircraft.ToArray() );

		}

		/// <summary>
		/// 制空戦力を求めます。
		/// </summary>
		/// <param name="fleet">対象の艦隊。</param>
        public static int GetAirSuperiority(FleetData fleet, int FullExp = 0)
        {

            int air = 0;

            foreach (var ship in fleet.MembersWithoutEscaped)
            {
                if (ship == null) continue;

                air += GetAirSuperiority(ship, FullExp);
            }

            return air;
        }


		/// <summary>
		/// 索敵能力を求めます。「2-5式」です。
		/// </summary>
		/// <param name="fleet">対象の艦隊。</param>
		public static int GetSearchingAbility_Old( FleetData fleet ) {

			KCDatabase db = KCDatabase.Instance;

			int los_reconplane = 0;
			int los_radar = 0;
			int los_other = 0;

			foreach ( var ship in fleet.MembersWithoutEscaped ) {

				if ( ship == null )
					continue;

				los_other += ship.LOSBase;

				var slot = ship.SlotInstanceMaster;

				for ( int j = 0; j < slot.Count; j++ ) {

					if ( slot[j] == null ) continue;

					switch ( slot[j].EquipmentType[2] ) {
						case 9:		//艦偵
						case 10:	//水偵
						case 11:	//水爆
							if ( ship.Aircraft[j] > 0 )
								los_reconplane += slot[j].LOS * 2;
							break;

						case 12:	//小型電探
						case 13:	//大型電探
							los_radar += slot[j].LOS;
							break;

						default:
							los_other += slot[j].LOS;
							break;
					}
				}
			}


			return (int)Math.Sqrt( los_other ) + los_radar + los_reconplane;
		}


		/// <summary>
		/// 索敵能力を求めます。「2-5式(秋)」です。
		/// </summary>
		/// <param name="fleet">対象の艦隊。</param>
		public static double GetSearchingAbility_Autumn( FleetData fleet ) {

			double ret = 0.0;

			foreach ( var ship in fleet.MembersWithoutEscaped ) {
				if ( ship == null ) continue;

				ret += Math.Sqrt( ship.LOSBase ) * 1.6841056;

				foreach ( var eq in ship.SlotInstanceMaster ) {
					if ( eq == null ) continue;

					switch ( eq.CategoryType ) {

						case 7:		//艦爆
							ret += eq.LOS * 1.0376255; break;

						case 8:		//艦攻
							ret += eq.LOS * 1.3677954; break;

						case 9:		//艦偵
							ret += eq.LOS * 1.6592780; break;

						case 10:	//水偵
							ret += eq.LOS * 2.0000000; break;

						case 11:	//水爆
							ret += eq.LOS * 1.7787282; break;

						case 12:	//小型電探
							ret += eq.LOS * 1.0045358; break;

						case 13:	//大型電探
							ret += eq.LOS * 0.9906638; break;

						case 29:	//探照灯
							ret += eq.LOS * 0.9067950; break;

					}
				}
			}

			ret -= Math.Ceiling( KCDatabase.Instance.Admiral.Level / 5.0 ) * 5.0 * 0.6142467;

			return Math.Round( ret, 1 );
		}


		/// <summary>
		/// 索敵能力を求めます。「2-5式(秋)簡易式」です。
		/// </summary>
		/// <param name="fleet">対象の艦隊。</param>
		public static double GetSearchingAbility_TinyAutumn( FleetData fleet ) {

			double ret = 0.0;

			foreach ( var ship in fleet.MembersWithoutEscaped ) {
				if ( ship == null ) continue;

				double cur = Math.Sqrt( ship.LOSBase );

				foreach ( var eq in ship.SlotInstanceMaster ) {
					if ( eq == null ) continue;

					switch ( eq.CategoryType ) {

						case 7:		//艦爆
							cur += eq.LOS * 0.6; break;

						case 8:		//艦攻
							cur += eq.LOS * 0.8; break;

						case 9:		//艦偵
							cur += eq.LOS * 1.0; break;

						case 10:	//水偵
							cur += eq.LOS * 1.2; break;

						case 11:	//水爆
							cur += eq.LOS * 1.0; break;

						case 12:	//小型電探
							cur += eq.LOS * 0.6; break;

						case 13:	//大型電探
							cur += eq.LOS * 0.6; break;

						case 29:	//探照灯
							cur += eq.LOS * 0.5; break;

						default:	//その他
							cur += eq.LOS * 0.5; break;
					}
				}

				ret += Math.Floor( cur );
			}

			ret -= Math.Floor( KCDatabase.Instance.Admiral.Level * 0.4 );

			return Math.Round( ret, 1 );
		}


		/// <summary>
		/// 艦隊の触接開始率を求めます。
		/// </summary>
		/// <param name="fleet">対象の艦隊。</param>
		public static double GetContactProbability( FleetData fleet ) {

			double successProb = 0.0;

			foreach ( var ship in fleet.MembersWithoutEscaped ) {
				if ( ship == null ) continue;

				var eqs = ship.SlotInstanceMaster;

				for ( int i = 0; i < ship.Slot.Count; i++ ) {
					if ( eqs[i] == null )
						continue;

					if ( eqs[i].CategoryType == 9 ||	// 艦上偵察機
						eqs[i].CategoryType == 10 ) {	// 水上偵察機

						successProb += 0.04 * eqs[i].LOS * Math.Sqrt( ship.Aircraft[i] );
					}
				}
			}

			return successProb;
		}

		/// <summary>
		/// 機体命中率別の触接選択率を求めます。
		/// </summary>
		/// <param name="fleet">対象の艦隊。</param>
		/// <returns>機体の命中をキー, 触接選択率を値とした Dictionary 。</returns>
		public static Dictionary<int, double> GetContactSelectionProbability( FleetData fleet ) {

			var probs = new Dictionary<int, double>();

			foreach ( var ship in fleet.MembersWithoutEscaped ) {
				if ( ship == null )
					continue;

				foreach ( var eq in ship.SlotInstanceMaster ) {
					if ( eq == null )
						continue;

					switch ( eq.CategoryType ) {
						case 8:		// 艦上攻撃機
						case 9:		// 艦上偵察機
						case 10:	// 水上偵察機

							if ( !probs.ContainsKey( eq.Accuracy ) )
								probs.Add( eq.Accuracy, 1.0 );

							probs[eq.Accuracy] *= 1.0 - ( 0.07 * eq.LOS );
							break;
					}
				}
			}

			foreach ( int key in probs.Keys.ToArray() ) {		//列挙中の変更エラーを防ぐため 
				probs[key] = 1.0 - probs[key];
			}

			return probs;
		}


		/// <summary>
		/// 輸送作戦成功時の輸送量(減少TP)を求めます。
		/// (S勝利時のもの。A勝利時は int( value * 0.7 ) )
		/// </summary>
		/// <param name="fleet">対象の艦隊。</param>
		/// <returns>減少TP。</returns>
		public static int GetTPDamage( FleetData fleet ) {

			int tp = 0;

			foreach ( var ship in fleet.MembersWithoutEscaped.Where( s => s != null && s.HPRate > 0.25 ) ) {

				// 装備ボーナス
				foreach ( var eq in ship.AllSlotInstanceMaster.Where( q => q != null ) ) {

					switch ( eq.CategoryType ) {

						case 24:	// 上陸用舟艇
							tp += 8;
							break;
						case 30:	// 簡易輸送部材
							tp += 5;
							break;
						case 43:	// 戦闘糧食
							tp += 1;
							break;
					}
				}


				// 艦種ボーナス
				switch ( ship.MasterShip.ShipType ) {

					case 2:		// 駆逐艦
						tp += 5;
						break;
					case 3:		// 軽巡洋艦
						tp += 2;
						break;
					case 5:		// 重巡洋艦
						tp += 0;
						break;
					case 6:		// 航空巡洋艦
						tp += 4;
						break;
					case 10:	// 航空戦艦
						tp += 7;
						break;
					case 16:	// 水上機母艦
						tp += 9;
						break;
					case 17:	// 揚陸艦
						tp += 12;
						break;
					case 20:	// 潜水母艦
						tp += 7;
						break;
					case 21:	// 練習巡洋艦
						tp += 6;
						break;
					case 22:	// 補給艦
						tp += 15;
						break;
				}
			}


			return tp;
		}


		/// <summary>
		/// 昼戦における攻撃種別を取得します。
		/// </summary>
		/// <param name="slot">攻撃艦のスロット(マスターID)。</param>
		/// <param name="attackerShipID">攻撃艦の艦船ID。</param>
		/// <param name="defenerShipID">防御艦の艦船ID。なければ-1</param>
		/// <param name="includeSpecialAttack">弾着観測砲撃を含むか。falseなら除外して計算</param>
		public static int GetDayAttackKind( int[] slot, int attackerShipID, int defenerShipID, bool includeSpecialAttack = true ) {

			int reconcnt = 0;
			int mainguncnt = 0;
			int subguncnt = 0;
			int apshellcnt = 0;
			int radarcnt = 0;
			int rocketcnt = 0;

			if ( slot == null ) return -1;

			for ( int i = 0; i < slot.Length; i++ ) {

				EquipmentDataMaster eq = KCDatabase.Instance.MasterEquipments[slot[i]];
				if ( eq == null ) continue;

				int eqtype = eq.CategoryType;

				switch ( eqtype ) {
					case 1:
					case 2:
					case 3:
						mainguncnt++;
						break;
					case 4:
						subguncnt++;
						break;
					case 10:
					case 11:
						reconcnt++;
						break;
					case 12:
					case 13:
						radarcnt++;
						break;
					case 19:
						apshellcnt++;
						break;
					case 37:
						rocketcnt++;
						break;
				}
			}

			if ( reconcnt > 0 && includeSpecialAttack ) {
				if ( mainguncnt == 2 && apshellcnt == 1 )
					return 6;		//カットイン(主砲/主砲)
				else if ( mainguncnt == 1 && subguncnt == 1 && apshellcnt == 1 )
					return 5;		//カットイン(主砲/徹甲弾)
				else if ( mainguncnt == 1 && subguncnt == 1 && radarcnt == 1 )
					return 4;		//カットイン(主砲/電探)
				else if ( mainguncnt >= 1 && subguncnt >= 1 )
					return 3;		//カットイン(主砲/副砲)
				else if ( mainguncnt >= 2 )
					return 2;		//連撃
			}


			ShipDataMaster atkship = KCDatabase.Instance.MasterShips[attackerShipID];
			ShipDataMaster defship = KCDatabase.Instance.MasterShips[defenerShipID];

			if ( atkship != null ) {

				if ( defship != null && defship.IsLandBase && rocketcnt > 0 )
					return 10;		//ロケット砲撃

				else if ( attackerShipID == 352 ) {	//速吸改

					if ( defship != null && ( defship.ShipType == 13 || defship.ShipType == 14 ) ) {
						if ( slot.Select( id => KCDatabase.Instance.MasterEquipments[id] )
							.Count( eq => eq != null && ( ( eq.CategoryType == 8 && eq.ASW > 0 ) || eq.CategoryType == 11 || eq.CategoryType == 25 ) ) > 0 )
							return 7;		//空撃
						else
							return 8;	//爆雷攻撃

					} else if ( slot.Select( id => KCDatabase.Instance.MasterEquipments[id] ).Count( eq => eq != null && eq.CategoryType == 8 ) > 0 )
						return 7;		//空撃
					else
						return 0;		//砲撃

				} else if ( atkship.ShipType == 7 || atkship.ShipType == 11 || atkship.ShipType == 18 )		//軽空母/正規空母/装甲空母
					return 7;		//空撃

				else if ( defship != null && ( defship.ShipType == 13 || defship.ShipType == 14 ) )			//潜水艦/潜水空母
					if ( atkship.ShipType == 6 || atkship.ShipType == 10 ||
						 atkship.ShipType == 16 || atkship.ShipType == 17 )			//航空巡洋艦/航空戦艦/水上機母艦/揚陸艦
						return 7;		//空撃
					else
						return 8;		//爆雷攻撃

				//本来の雷撃は発生しない
				else if ( atkship.ShipType == 13 || atkship.ShipType == 14 )		//潜水艦/潜水空母
					return 9;			//雷撃(特例措置, 本来のコード中には存在しない)

			}

			return 0;
		}


		/// <summary>
		/// 夜戦における攻撃種別を取得します。
		/// </summary>
		/// <param name="slot">攻撃艦のスロット(マスターID)。</param>
		/// <param name="attackerShipID">攻撃艦の艦船ID。</param>
		/// <param name="defenerShipID">防御艦の艦船ID。なければ-1</param>
		public static int GetNightAttackKind( int[] slot, int attackerShipID, int defenerShipID ) {

			int mainguncnt = 0;
			int subguncnt = 0;
			int torpcnt = 0;
			int rocketcnt = 0;

			if ( slot == null ) return -1;

			for ( int i = 0; i < slot.Length; i++ ) {
				EquipmentDataMaster eq = KCDatabase.Instance.MasterEquipments[slot[i]];
				if ( eq == null ) continue;

				int eqtype = eq.EquipmentType[2];

				switch ( eqtype ) {
					case 1:
					case 2:
					case 3:
						mainguncnt++; break;	//主砲

					case 4:
						subguncnt++; break;		//副砲

					case 5:
					case 32:
						torpcnt++; break;		//魚雷

					case 37:					//対地装備
						rocketcnt++; break;
				}

			}


			if ( torpcnt >= 2 )
				return 3;			//カットイン(魚雷/魚雷)
			else if ( mainguncnt >= 3 )
				return 5;			//カットイン(主砲x3)
			else if ( mainguncnt == 2 && subguncnt > 0 )
				return 4;			//カットイン(主砲x2/副砲)
			else if ( ( mainguncnt == 2 && subguncnt == 0 && torpcnt == 1 ) || ( mainguncnt == 1 && torpcnt == 1 ) )
				return 2;			//カットイン(主砲/魚雷)
			else if ( ( mainguncnt == 2 && subguncnt == 0 & torpcnt == 0 ) ||
				( mainguncnt == 1 && subguncnt > 0 ) ||
				( subguncnt >= 2 && torpcnt <= 1 ) ) {
				return 1;			//連撃
			}


			ShipDataMaster atkship = KCDatabase.Instance.MasterShips[attackerShipID];
			ShipDataMaster defship = KCDatabase.Instance.MasterShips[defenerShipID];

			if ( atkship != null ) {

				if ( defship != null && defship.IsLandBase && rocketcnt > 0 )
					return 10;		//ロケット砲撃

				else if ( atkship.ShipType == 7 || atkship.ShipType == 11 || atkship.ShipType == 18 ) {		//軽空母/正規空母/装甲空母

					if ( attackerShipID == 432 || attackerShipID == 353 )		//Graf Zeppelin(改)
						return 0;		//砲撃
					else
						return 7;		//空撃

				} else if ( atkship.ShipType == 13 || atkship.ShipType == 14 )	//潜水艦/潜水空母
					return 9;			//雷撃

				else if ( defship != null && ( defship.ShipType == 13 || defship.ShipType == 14 ) )			//潜水艦/潜水空母
					if ( atkship.ShipType == 6 || atkship.ShipType == 10 ||
						 atkship.ShipType == 16 || atkship.ShipType == 17 )			//航空巡洋艦/航空戦艦/水上機母艦/揚陸艦
						return 7;		//空撃
					else
						return 8;		//爆雷攻撃

				else if ( slot.Length > 0 ) {
					EquipmentDataMaster eq = KCDatabase.Instance.MasterEquipments[slot[0]];
					if ( eq != null && eq.EquipmentType[2] == 5 ) {		//最初のスロット==魚雷		(本来の判定とは微妙に異なるが無問題)
						return 9;		//雷撃
					}
				}

			}

			return 0;		//砲撃

		}


		/// <summary>
		/// 対空カットイン種別を取得します。
		/// </summary>
		public static int GetAACutinKind( int shipID, int[] slot ) {

			int highangle = 0;
			int highangle_director = 0;
			int director = 0;
			int radar = 0;
			int aaradar = 0;
			int maingunl = 0;
			int aashell = 0;
			int aagun = 0;
			int aagun_concentrated = 0;


			foreach ( int eid in slot ) {

				EquipmentDataMaster eq = KCDatabase.Instance.MasterEquipments[eid];
				if ( eq == null ) continue;

				if ( eq.IconType == 16 ) {	//高角砲
					// 10cm連装高角砲+高射装置 or 12.7cm高角砲+高射装置 or 90mm単装高角砲
					if ( eq.EquipmentID == 122 || eq.EquipmentID == 130 || eq.EquipmentID == 135 ) {
						highangle_director++;
					}
					highangle++;

				} else if ( eq.CategoryType == 36 ) {	//高射装置
					director++;

				} else if ( eq.CardType == 8 ) {	//電探
					if ( eq.AA > 0 ) {
						aaradar++;
					}
					radar++;

				} else if ( eq.CategoryType == 3 ) {	//大口径主砲
					maingunl++;

				} else if ( eq.CategoryType == 18 ) {	//対空強化弾
					aashell++;

				} else if ( eq.CategoryType == 21 ) {	//対空機銃
					if ( eq.EquipmentID == 131 ) {		//25mm三連装機銃 集中配備
						aagun_concentrated++;
					}
					aagun++;

				}

			}


			// 固有カットイン
			switch ( shipID ) {

				case 421:	//秋月
				case 330:	//秋月改
				case 422:	//照月
				case 346:	//照月改
				case 423:	//初月
				case 357:	//初月改
					if ( highangle >= 2 && radar >= 1 ) {
						return 1;
					}
					if ( highangle >= 1 && radar >= 1 ) {
						return 2;
					}
					if ( highangle >= 2 ) {
						return 3;
					}
					break;

				case 428:	//摩耶改二
					if ( highangle >= 1 && aagun_concentrated >= 1 ) {
						if ( aaradar >= 1 )
							return 10;

						return 11;
					}
					break;

				case 141:	//五十鈴改二
					if ( highangle >= 1 && aagun >= 1 ) {
						if ( aaradar >= 1 )
							return 14;
						else
							return 15;
					}
					break;

				case 470:	//霞改二乙
					if ( highangle >= 1 && aagun >= 1 ) {
						if ( aaradar >= 1 )
							return 16;
						else
							return 17;
					}
					break;
			}



			if ( maingunl >= 1 && aashell >= 1 && director >= 1 && aaradar >= 1 ) {
				return 4;
			}
			if ( highangle_director >= 2 && aaradar >= 1 ) {
				return 5;
			}
			if ( maingunl >= 1 && aashell >= 1 && director >= 1 ) {
				return 6;
			}
			if ( highangle >= 1 && director >= 1 && aaradar >= 1 ) {
				return 7;
			}
			if ( highangle_director >= 1 && aaradar >= 1 ) {
				return 8;
			}
			if ( highangle >= 1 && director >= 1 ) {
				return 9;
			}

			if ( aagun_concentrated >= 1 && aagun >= 2 && aaradar >= 1 ) {	//注: 機銃2なのは集中機銃がダブるため
				return 12;
			}

			return 0;
		}


		/// <summary>
		/// 装備が艦載機であるかを取得します。
		/// </summary>
		/// <param name="equipmentID">装備ID。</param>
		/// <param name="containsRecon">偵察機(非攻撃機)を含めるか。</param>
		/// <param name="containsASWAircraft">対潜可能機を含めるか。</param>
		public static bool IsAircraft( int equipmentID, bool containsRecon, bool containsASWAircraft = false ) {

			var eq = KCDatabase.Instance.MasterEquipments[equipmentID];

			if ( eq == null ) return false;

			switch ( eq.CategoryType ) {
				case 6:		//艦上戦闘機
				case 7:		//艦上爆撃機
				case 8:		//艦上攻撃機
				case 11:	//水上爆撃機
				case 25:	//オートジャイロ
				case 26:	//対潜哨戒機
					return true;

				case 9:		//艦上偵察機
				case 10:	//水上偵察機
					return containsRecon;

				case 41:	//大型飛行艇
					return containsRecon || containsASWAircraft;
				default:
					return false;
			}

		}



		/// <summary>
		/// 対潜攻撃可能であるかを取得します。
		/// </summary>
		/// <param name="ship">対象の艦船データ。</param>
		public static bool CanAttackSubmarine( ShipData ship ) {

			switch ( ship.MasterShip.ShipType ) {
				case 2:		//駆逐
				case 3:		//軽巡
				case 4:		//雷巡
				case 21:	//練巡
				case 22:	//補給
					return ship.ASWBase > 0;

				case 6:		//航巡
				case 7:		//軽空母
				case 10:	//航戦
				case 16:	//水母
				case 17:	//揚陸
					return ship.SlotInstanceMaster.Count( eq => eq != null && IsAircraft( eq.EquipmentID, false, true ) && eq.ASW > 0 ) > 0;

				default:
					return false;
			}

		}

	}


}
