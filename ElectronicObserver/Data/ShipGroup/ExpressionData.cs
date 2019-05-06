using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.ShipGroup
{

	/// <summary>
	/// 艦船フィルタの式データ
	/// </summary>
	[DataContract(Name = "ExpressionData")]
	public class ExpressionData : ICloneable
	{

		public enum ExpressionOperator
		{
			Equal,
			NotEqual,
			LessThan,
			LessEqual,
			GreaterThan,
			GreaterEqual,
			Contains,
			NotContains,
			BeginWith,
			NotBeginWith,
			EndWith,
			NotEndWith,
			ArrayContains,
			ArrayNotContains,
		}


		[DataMember]
		public string LeftOperand { get; set; }

		[DataMember]
		public ExpressionOperator Operator { get; set; }

		[DataMember]
		public object RightOperand { get; set; }


		[DataMember]
		public bool Enabled { get; set; }


		[IgnoreDataMember]
		private static readonly Regex regex_index = new Regex(@"\.(?<name>\w+)(\[(?<index>\d+?)\])?", RegexOptions.Compiled);

		[IgnoreDataMember]
		public static readonly Dictionary<string, string> LeftOperandNameTable = new Dictionary<string, string>() {
			{ ".MasterID", "艦船固有ID" },
			{ ".ShipID", "艦船ID" },
			{ ".MasterShip.NameWithClass", "艦名" },
			{ ".MasterShip.ShipType", "艦種" },
			{ ".Level", "レベル" },
			{ ".ExpTotal", "経験値" },
			{ ".ExpNext", "次のレベルまで" },
			{ ".ExpNextRemodel", "次の改装まで" },
			{ ".HPCurrent", "現在HP" },
			{ ".HPMax", "最大HP" },
			{ ".HPRate", "HP割合" },
			{ ".Condition", "コンディション" },
			{ ".AllSlotMaster", "装備" },
			{ ".SlotMaster[0]", "装備 #1" },	//checkme: 要る?
			{ ".SlotMaster[1]", "装備 #2" },
			{ ".SlotMaster[2]", "装備 #3" },
			{ ".SlotMaster[3]", "装備 #4" },
			{ ".SlotMaster[4]", "装備 #5" },
			{ ".ExpansionSlotMaster", "補強装備" },
			{ ".Aircraft[0]", "搭載 #1" },
			{ ".Aircraft[1]", "搭載 #2" },
			{ ".Aircraft[2]", "搭載 #3" },
			{ ".Aircraft[3]", "搭載 #4" },
			{ ".Aircraft[4]", "搭載 #5" },
			{ ".AircraftTotal", "搭載機数合計" },
			{ ".MasterShip.Aircraft[0]", "最大搭載 #1" },
			{ ".MasterShip.Aircraft[1]", "最大搭載 #2" },
			{ ".MasterShip.Aircraft[2]", "最大搭載 #3" },
			{ ".MasterShip.Aircraft[3]", "最大搭載 #4" },
			{ ".MasterShip.Aircraft[4]", "最大搭載 #5" },
			{ ".MasterShip.AircraftTotal", "最大搭載機数" },		//要る？
			{ ".AircraftRate[0]", "搭載割合 #1" },
			{ ".AircraftRate[1]", "搭載割合 #2" },
			{ ".AircraftRate[2]", "搭載割合 #3" },
			{ ".AircraftRate[3]", "搭載割合 #4" },
			{ ".AircraftRate[4]", "搭載割合 #5" },
			{ ".AircraftTotalRate", "搭載割合合計" },
			{ ".Fuel", "搭載燃料" },
			{ ".Ammo", "搭載弾薬" },
			{ ".FuelMax", "最大搭載燃料" },
			{ ".AmmoMax", "最大搭載弾薬" },
			{ ".FuelRate", "搭載燃料割合" },
			{ ".AmmoRate", "搭載弾薬割合" },
			{ ".SlotSize", "スロット数" },
			{ ".RepairingDockID", "入渠ドック" },
			{ ".RepairTime", "入渠時間" },
			{ ".RepairSteel", "入渠消費鋼材" },
			{ ".RepairFuel", "入渠消費燃料" },
			//強化値シリーズは省略
			{ ".FirepowerBase", "基本火力" },
			{ ".TorpedoBase", "基本雷装" },
			{ ".AABase", "基本対空" },
			{ ".ArmorBase", "基本装甲" },
			{ ".EvasionBase", "基本回避" },
			{ ".ASWBase", "基本対潜" },
			{ ".LOSBase", "基本索敵" },
			{ ".LuckBase", "基本運" },
			{ ".FirepowerTotal", "合計火力" },
			{ ".TorpedoTotal", "合計雷装" },
			{ ".AATotal", "合計対空" },
			{ ".ArmorTotal", "合計装甲" },
			{ ".EvasionTotal", "合計回避" },
			{ ".ASWTotal", "合計対潜" },
			{ ".LOSTotal", "合計索敵" },
			{ ".LuckTotal", "合計運" },
			{ ".BomberTotal", "合計爆装" },
			{ ".FirepowerRemain", "火力改修残り" },
			{ ".TorpedoRemain", "雷装改修残り" },
			{ ".AARemain", "対空改修残り" },
			{ ".ArmorRemain", "装甲改修残り" },
			{ ".LuckRemain", "運改修残り" },
			{ ".Range", "射程" },		//現在の射程
			{ ".Speed", "速力" },
			{ ".MasterShip.Speed", "基礎速力" },
			{ ".MasterShip.Rarity", "レアリティ" },
			{ ".IsLocked", "ロック" },
			{ ".IsLockedByEquipment", "装備ロック" },
			{ ".SallyArea", "出撃海域" },
			{ ".FleetWithIndex", "所属艦隊" },
			{ ".IsMarried", "ケッコンカッコカリ" },
			{ ".AirBattlePower", "航空威力" },
			{ ".ShellingPower", "砲撃威力" },
			{ ".AircraftPower", "空撃威力" },
			{ ".AntiSubmarinePower", "対潜威力" },
			{ ".TorpedoPower", "雷撃威力" },
			{ ".NightBattlePower", "夜戦威力" },
			{ ".MasterShip.AlbumNo", "図鑑番号" },
			{ ".MasterShip.NameReading", "艦名読み" },
			{ ".MasterShip.RemodelBeforeShipID", "改装前艦船ID" },
			{ ".MasterShip.RemodelAfterShipID", "改装後艦船ID" },
			//マスターのパラメータ系もおそらく意味がないので省略		
			{ ".MasterShip.EquippableCategories", "装備可能リスト" },
		};

		private static Dictionary<string, Type> ExpressionTypeTable = new Dictionary<string, Type>();


		[IgnoreDataMember]
		public static readonly Dictionary<ExpressionOperator, string> OperatorNameTable = new Dictionary<ExpressionOperator, string>() {
			{ ExpressionOperator.Equal, "と等しい" },
			{ ExpressionOperator.NotEqual, "と等しくない" },
			{ ExpressionOperator.LessThan, "より小さい" },
			{ ExpressionOperator.LessEqual, "以下" },
			{ ExpressionOperator.GreaterThan, "より大きい" },
			{ ExpressionOperator.GreaterEqual, "以上" },
			{ ExpressionOperator.Contains, "を含む" },
			{ ExpressionOperator.NotContains, "を含まない" },
			{ ExpressionOperator.BeginWith, "から始まる" },
			{ ExpressionOperator.NotBeginWith, "から始まらない" },
			{ ExpressionOperator.EndWith, "で終わる" },
			{ ExpressionOperator.NotEndWith, "で終わらない" },
			{ ExpressionOperator.ArrayContains, "を含む" },
			{ ExpressionOperator.ArrayNotContains, "を含まない" },

		};



		public ExpressionData()
		{
			Enabled = true;
		}

		public ExpressionData(string left, ExpressionOperator ope, object right)
			: this()
		{
			LeftOperand = left;
			Operator = ope;
			RightOperand = right;
		}


		public Expression Compile(ParameterExpression paramex)
		{

			Expression memberex = null;
			Expression constex = Expression.Constant(RightOperand, RightOperand.GetType());

			{
				Match match = regex_index.Match(LeftOperand);
				if (match.Success)
				{

					do
					{

						if (memberex == null)
						{
							memberex = Expression.PropertyOrField(paramex, match.Groups["name"].Value);
						}
						else
						{
							memberex = Expression.PropertyOrField(memberex, match.Groups["name"].Value);
						}

						if (int.TryParse(match.Groups["index"].Value, out int index))
						{
							memberex = Expression.Property(memberex, "Item", Expression.Constant(index, typeof(int)));
						}

					} while ((match = match.NextMatch()).Success);

				}
				else
				{
					memberex = Expression.PropertyOrField(paramex, LeftOperand);
				}
			}

			if (memberex.Type.IsEnum)
				memberex = Expression.Convert(memberex, typeof(int));

			Expression condex;
			switch (Operator)
			{
				case ExpressionOperator.Equal:
					condex = Expression.Equal(memberex, constex);
					break;
				case ExpressionOperator.NotEqual:
					condex = Expression.NotEqual(memberex, constex);
					break;
				case ExpressionOperator.LessThan:
					condex = Expression.LessThan(memberex, constex);
					break;
				case ExpressionOperator.LessEqual:
					condex = Expression.LessThanOrEqual(memberex, constex);
					break;
				case ExpressionOperator.GreaterThan:
					condex = Expression.GreaterThan(memberex, constex);
					break;
				case ExpressionOperator.GreaterEqual:
					condex = Expression.GreaterThanOrEqual(memberex, constex);
					break;
				case ExpressionOperator.Contains:
					condex = Expression.Call(memberex, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), constex);
					break;
				case ExpressionOperator.NotContains:
					condex = Expression.Not(Expression.Call(memberex, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), constex));
					break;
				case ExpressionOperator.BeginWith:
					condex = Expression.Call(memberex, typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }), constex);
					break;
				case ExpressionOperator.NotBeginWith:
					condex = Expression.Not(Expression.Call(memberex, typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }), constex));
					break;
				case ExpressionOperator.EndWith:
					condex = Expression.Call(memberex, typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }), constex);
					break;
				case ExpressionOperator.NotEndWith:
					condex = Expression.Not(Expression.Call(memberex, typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }), constex));
					break;
				case ExpressionOperator.ArrayContains:  // returns Enumerable.Contains<>( memberex )
					condex = Expression.Call(typeof(Enumerable), "Contains", new Type[] { memberex.Type.GetElementType() ?? memberex.Type.GetGenericArguments().First() }, memberex, constex);
					break;
				case ExpressionOperator.ArrayNotContains:   // returns !Enumerable.Contains<>( memberex )
					condex = Expression.Not(Expression.Call(typeof(Enumerable), "Contains", new Type[] { memberex.Type.GetElementType() ?? memberex.Type.GetGenericArguments().First() }, memberex, constex));
					break;

				default:
					throw new NotImplementedException();
			}

			return condex;
		}



		public static Type GetLeftOperandType(string left)
		{

			if (ExpressionTypeTable.ContainsKey(left))
			{
				return ExpressionTypeTable[left];

			}
			else if (KCDatabase.Instance.Ships.Count > 0)
			{

				object obj = KCDatabase.Instance.Ships.Values.First();

				Match match = regex_index.Match(left);
				if (match.Success)
				{

					do
					{

						if (int.TryParse(match.Groups["index"].Value, out int index))
						{
							obj = ((dynamic)obj.GetType().InvokeMember(match.Groups["name"].Value, System.Reflection.BindingFlags.GetProperty, null, obj, null))[index];
						}
						else
						{
							object obj2 = obj.GetType().InvokeMember(match.Groups["name"].Value, System.Reflection.BindingFlags.GetProperty, null, obj, null);
							if (obj2 == null)
							{   //プロパティはあるけどnull
								var type = obj.GetType().GetProperty(match.Groups["name"].Value).GetType();
								ExpressionTypeTable.Add(left, type);
								return type;
							}
							else
							{
								obj = obj2;
							}
						}

					} while (obj != null && (match = match.NextMatch()).Success);


					if (obj != null)
					{
						ExpressionTypeTable.Add(left, obj.GetType());
						return obj.GetType();
					}
				}

			}

			return null;
		}

		public Type GetLeftOperandType()
		{
			return GetLeftOperandType(LeftOperand);
		}



		public override string ToString() => $"{LeftOperandToString()} は {RightOperandToString()} {OperatorToString()}";



		/// <summary>
		/// 左辺値の文字列表現を求めます。
		/// </summary>
		public string LeftOperandToString()
		{
			if (LeftOperandNameTable.ContainsKey(LeftOperand))
				return LeftOperandNameTable[LeftOperand];
			else
				return LeftOperand;
		}

		/// <summary>
		/// 演算子の文字列表現を求めます。
		/// </summary>
		public string OperatorToString()
		{
			return OperatorNameTable[Operator];
		}

		/// <summary>
		/// 右辺値の文字列表現を求めます。
		/// </summary>
		public string RightOperandToString()
		{

			if (LeftOperand == ".MasterID")
			{
				var ship = KCDatabase.Instance.Ships[(int)RightOperand];
				if (ship != null)
					return $"{ship.MasterID} ({ship.NameWithLevel})";
				else
					return $"{(int)RightOperand} (未在籍)";

			}
			else if (LeftOperand == ".ShipID")
			{
				var ship = KCDatabase.Instance.MasterShips[(int)RightOperand];
				if (ship != null)
					return $"{ship.ShipID} ({ship.NameWithClass})";
				else
					return $"{(int)RightOperand} (存在せず)";

			}
			else if (LeftOperand == ".MasterShip.ShipType")
			{
				var shiptype = KCDatabase.Instance.ShipTypes[(int)RightOperand];
				if (shiptype != null)
					return shiptype.Name;
				else
					return $"{(int)RightOperand} (未定義)";

			}
			else if (LeftOperand.Contains("SlotMaster"))
			{
				if ((int)RightOperand == -1)
				{
					return "(なし)";
				}
				else
				{
					var eq = KCDatabase.Instance.MasterEquipments[(int)RightOperand];
					if (eq != null)
						return eq.Name;
					else
						return $"{(int)RightOperand} (未定義)";
				}
			}
			else if (LeftOperand == ".MasterShip.EquippableCategories")
			{
				var cat = KCDatabase.Instance.EquipmentTypes[(int)RightOperand];
				if (cat != null)
					return cat.Name;
				else
					return $"{(int)RightOperand} (未定義)";

			}
			else if (LeftOperand.Contains("Rate") && RightOperand is double)
			{
				return ((double)RightOperand).ToString("P0");

			}
			else if (LeftOperand == ".RepairTime")
			{
				return DateTimeHelper.ToTimeRemainString(DateTimeHelper.FromAPITimeSpan((int)RightOperand));

			}
			else if (LeftOperand == ".Range")
			{
				return Constants.GetRange((int)RightOperand);

			}
			else if (LeftOperand == ".Speed" || LeftOperand == ".MasterShip.Speed")
			{
				return Constants.GetSpeed((int)RightOperand);

			}
			else if (LeftOperand == ".MasterShip.Rarity")
			{
				return Constants.GetShipRarity((int)RightOperand);

			}
			else if (LeftOperand == ".MasterShip.AlbumNo")
			{
				var ship = KCDatabase.Instance.MasterShips.Values.FirstOrDefault(s => s.AlbumNo == (int)RightOperand);
				if (ship != null)
					return $"{(int)RightOperand} ({ship.NameWithClass})";
				else
					return $"{(int)RightOperand} (存在せず)";

			}
			else if (LeftOperand == ".MasterShip.RemodelAfterShipID")
			{

				if (((int)RightOperand) == 0)
					return "最終改装";

				var ship = KCDatabase.Instance.MasterShips[(int)RightOperand];
				if (ship != null)
					return $"{ship.ShipID} ({ship.NameWithClass})";
				else
					return $"{(int)RightOperand} (存在せず)";

			}
			else if (LeftOperand == ".MasterShip.RemodelBeforeShipID")
			{

				if (((int)RightOperand) == 0)
					return "未改装";

				var ship = KCDatabase.Instance.MasterShips[(int)RightOperand];
				if (ship != null)
					return $"{ship.ShipID} ({ship.NameWithClass})";
				else
					return $"{(int)RightOperand} (存在せず)";

			}
			else if (RightOperand is bool)
			{
				return ((bool)RightOperand) ? "○" : "×";

			}
			else
			{
				return RightOperand.ToString();

			}

		}


		public ExpressionData Clone()
		{
			var clone = MemberwiseClone();      //checkme: 右辺値に参照型を含む場合死ぬ
			return (ExpressionData)clone;
		}

		object ICloneable.Clone()
		{
			return Clone();
		}
	}




}
