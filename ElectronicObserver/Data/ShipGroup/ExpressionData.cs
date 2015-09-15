using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.ShipGroup {

	/// <summary>
	/// 艦船フィルタの式データ
	/// </summary>
	[DataContract( Name = "ExpressionData" )]
	public class ExpressionData : ICloneable {

		public enum ExpressionOperator {
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
		private static readonly Regex regex_index = new Regex( @"\.(?<name>\w+)(\[(?<index>\d+?)\])?", RegexOptions.Compiled );

		[IgnoreDataMember]
		public static readonly Dictionary<string, string> LeftOperandNameTable = new Dictionary<string, string>() {
			{ ".MasterID", "艦船固有ID" },
			{ ".ShipID", "艦船ID" },
			{ ".Level", "レベル" },
			{ ".ExpTotal", "経験値" },
			{ ".ExpNext", "次のレベルまで" },
			{ ".HPCurrent", "現在HP" },
			{ ".HPMax", "最大HP" },
			{ ".Range", "射程" },		//現在の射程
			{ ".AllSlotMaster", "装備" },
			{ ".SlotMaster[0]", "装備ID[0]" },	//checkme: 要る?
			{ ".SlotMaster[1]", "装備ID[1]" },
			{ ".SlotMaster[2]", "装備ID[2]" },
			{ ".SlotMaster[3]", "装備ID[3]" },
			{ ".SlotMaster[4]", "装備ID[4]" },
			{ ".ExpansionSlotMaster", "補強装備ID" },
			{ ".Aircraft[0]", "搭載機数[0]" },
			{ ".Aircraft[1]", "搭載機数[1]" },
			{ ".Aircraft[2]", "搭載機数[2]" },
			{ ".Aircraft[3]", "搭載機数[3]" },
			{ ".Aircraft[4]", "搭載機数[4]" },
			{ ".Fuel", "搭載燃料" },
			{ ".Ammo", "搭載弾薬" },
			{ ".SlotSize", "スロットサイズ" },
			{ ".RepairTime", "入渠時間" },
			{ ".RepairSteel", "入渠消費鋼材" },
			{ ".RepairFuel", "入渠消費燃料" },
			{ ".Condition", "コンディション" },
			//強化値シリーズは省略
			{ ".FirepowerRemain", "火力改修残り" },
			{ ".TorpedoRemain", "雷装改修残り" },
			{ ".AARemain", "対空改修残り" },
			{ ".ArmorRemain", "装甲改修残り" },
			{ ".LuckRemain", "運改修残り" },
			{ ".FirepowerTotal", "合計火力" },
			{ ".TorpedoTotal", "合計雷装" },
			{ ".AATotal", "合計対空" },
			{ ".ArmorTotal", "合計装甲" },
			{ ".EvasionTotal", "合計回避" },
			{ ".ASWTotal", "合計対潜" },
			{ ".LOSTotal", "合計索敵" },
			{ ".LuckTotal", "合計運" },
			{ ".FirepowerBase", "基本火力" },
			{ ".TorpedoBase", "基本雷装" },
			{ ".AABase", "基本対空" },
			{ ".ArmorBase", "基本装甲" },
			{ ".EvasionBase", "基本回避" },
			{ ".ASWBase", "基本対潜" },
			{ ".LOSBase", "基本索敵" },
			{ ".LuckBase", "基本運" },
			{ ".IsLocked", "ロック" },
			{ ".IsLockedByEquipment", "装備ロック" },
			{ ".SallyArea", "出撃海域" },
			{ ".RepairingDockID", "入渠ドックID" },
			{ ".FleetWithIndex", "所属艦隊" },
			{ ".IsMarried", "ケッコン" },
			{ ".ExpNextRemodel", "次の改装まで" },
			{ ".HPRate", "HP割合" },
			{ ".AirBattlePower", "航空威力" },
			{ ".ShellingPower", "砲撃威力" },
			{ ".AircraftPower", "空撃威力" },
			{ ".AntiSubmarinePower", "対潜威力" },
			{ ".TorpedoPower", "雷撃威力" },
			{ ".NightBattlePower", "夜戦威力" },
			{ ".MasterShip.AlbumNo", "図鑑番号" },
			{ ".MasterShip.NameWithClass", "艦名" },
			{ ".MasterShip.NameReading", "艦名読み" },
			{ ".MasterShip.ShipType", "艦種" },
			{ ".MasterShip.RemodelBeforeShipID", "改装前艦船ID" },
			{ ".MasterShip.RemodelAfterShipID", "改装後艦船ID" },
			//マスターのパラメータ系もおそらく意味がないので省略
			{ ".MasterShip.Speed", "速力" },
			{ ".MasterShip.Rarity", "レアリティ" },
			{ ".MasterShip.Fuel", "最大搭載燃料" },
			{ ".MasterShip.Ammo", "最大搭載弾薬" },
			{ ".MasterShip.AircraftTotal", "合計艦載機数" },		//要る？
			
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



		public ExpressionData() {
			Enabled = true;
		}

		public ExpressionData( string left, ExpressionOperator ope, object right )
			: this() {
			LeftOperand = left;
			Operator = ope;
			RightOperand = right;
		}


		public Expression Compile( ParameterExpression paramex ) {

			Expression memberex = null;
			Expression constex = Expression.Constant( RightOperand, RightOperand.GetType() );

			{
				Match match = regex_index.Match( LeftOperand );
				if ( match.Success ) {

					do {

						if ( memberex == null ) {
							memberex = Expression.PropertyOrField( paramex, match.Groups["name"].Value );
						} else {
							memberex = Expression.PropertyOrField( memberex, match.Groups["name"].Value );
						}

						int index;
						if ( int.TryParse( match.Groups["index"].Value, out index ) ) {
							memberex = Expression.ArrayAccess( memberex, Expression.Constant( index, typeof( int ) ) );
						}

					} while ( ( match = match.NextMatch() ).Success );

				} else {
					memberex = Expression.PropertyOrField( paramex, LeftOperand );
				}
			}

			Expression  condex;
			switch ( Operator ) {
				case ExpressionOperator.Equal:
					condex = Expression.Equal( memberex, constex );
					break;
				case ExpressionOperator.NotEqual:
					condex = Expression.NotEqual( memberex, constex );
					break;
				case ExpressionOperator.LessThan:
					condex = Expression.LessThan( memberex, constex );
					break;
				case ExpressionOperator.LessEqual:
					condex = Expression.LessThanOrEqual( memberex, constex );
					break;
				case ExpressionOperator.GreaterThan:
					condex = Expression.GreaterThan( memberex, constex );
					break;
				case ExpressionOperator.GreaterEqual:
					condex = Expression.GreaterThanOrEqual( memberex, constex );
					break;
				case ExpressionOperator.Contains:
					condex = Expression.Call( memberex, typeof( string ).GetMethod( "Contains", new Type[] { typeof( string ) } ), constex );
					break;
				case ExpressionOperator.NotContains:
					condex = Expression.Not( Expression.Call( memberex, typeof( string ).GetMethod( "Contains", new Type[] { typeof( string ) } ), constex ) );
					break;
				case ExpressionOperator.BeginWith:
					condex = Expression.Equal( Expression.Call( memberex, typeof( string ).GetMethod( "IndexOf", new Type[] { typeof( string ) } ), constex ), Expression.Constant( 0, typeof( int ) ) );
					break;
				case ExpressionOperator.NotBeginWith:
					condex = Expression.NotEqual( Expression.Call( memberex, typeof( string ).GetMethod( "IndexOf", new Type[] { typeof( string ) } ), constex ), Expression.Constant( 0, typeof( int ) ) );
					break;
				case ExpressionOperator.EndWith:	// returns memberex.LastIndexOf( constex ) == ( memberex.Length - constex.Length )
					condex = Expression.Equal(
						Expression.Call( memberex, typeof( string ).GetMethod( "LastIndexOf", new Type[] { typeof( string ) } ), constex ),
						Expression.Subtract( Expression.PropertyOrField( memberex, "Length" ), Expression.PropertyOrField( constex, "Length" ) ) );
					break;
				case ExpressionOperator.NotEndWith:	// returns memberex.LastIndexOf( constex ) != ( memberex.Length - constex.Length )
					condex = Expression.NotEqual(
						Expression.Call( memberex, typeof( string ).GetMethod( "LastIndexOf", new Type[] { typeof( string ) } ), constex ),
						Expression.Subtract( Expression.PropertyOrField( memberex, "Length" ), Expression.PropertyOrField( constex, "Length" ) ) );
					break;
				case ExpressionOperator.ArrayContains:	// returns Enumerable.Contains<>( memberex )
					condex = Expression.Call( typeof( Enumerable ), "Contains", new Type[] { memberex.Type.GetElementType() ?? memberex.Type.GetGenericArguments().First() }, memberex, constex );
					break;
				case ExpressionOperator.ArrayNotContains:	// returns !Enumerable.Contains<>( memberex )
					condex = Expression.Not( Expression.Call( typeof( Enumerable ), "Contains", new Type[] { memberex.Type.GetElementType() ?? memberex.Type.GetGenericArguments().First() }, memberex, constex ) );
					break;

				default:
					throw new NotImplementedException();
			}

			return condex;
		}



		public static Type GetLeftOperandType( string left ) {

			if ( ExpressionTypeTable.ContainsKey( left ) ) {
				return ExpressionTypeTable[left];

			} else if ( KCDatabase.Instance.Ships.Count > 0 ) {

				object obj = KCDatabase.Instance.Ships.Values.First();

				Match match = regex_index.Match( left );
				if ( match.Success ) {

					do {

						int index;
						if ( int.TryParse( match.Groups["index"].Value, out index ) ) {
							obj = ( (dynamic)obj.GetType().InvokeMember( match.Groups["name"].Value, System.Reflection.BindingFlags.GetProperty, null, obj, null ) )[index];
						} else {
							object obj2 = obj.GetType().InvokeMember( match.Groups["name"].Value, System.Reflection.BindingFlags.GetProperty, null, obj, null );
							if ( obj2 == null ) {	//プロパティはあるけどnull
								var type = obj.GetType().GetProperty( match.Groups["name"].Value ).GetType();
								ExpressionTypeTable.Add( left, type );
								return type;
							} else {
								obj = obj2;
							}
						}

					} while ( obj != null && ( match = match.NextMatch() ).Success );


					if ( obj != null ) {
						ExpressionTypeTable.Add( left, obj.GetType() );
						return obj.GetType();
					}
				}

			}

			return null;
		}

		public Type GetLeftOperandType() {
			return GetLeftOperandType( LeftOperand );
		}



		public override string ToString() {
			return string.Format( "{0} は {1} {2}", LeftOperandNameTable.ContainsKey( LeftOperand ) ? LeftOperandNameTable[LeftOperand] : LeftOperand, RightOperand.ToString(), OperatorNameTable[Operator] );
		}



		public ExpressionData Clone() {
			var clone = MemberwiseClone();		//fixme: 右辺値に参照型を含む場合死ぬ
			return (ExpressionData)clone;
		}

		object ICloneable.Clone() {
			return Clone();
		}
	}




}
