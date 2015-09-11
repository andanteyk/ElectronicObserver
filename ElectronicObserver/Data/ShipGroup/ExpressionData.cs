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
		private static readonly Regex regex_index = new Regex( @"\.?(?<name>\w+)(\[(?<index>\d+?)\])?", RegexOptions.Compiled );

		[IgnoreDataMember]
		public static readonly Dictionary<string, string> DisplayNameTable = new Dictionary<string, string>() {
			{ "MasterID", "艦船固有ID" },
			{ "ShipID", "艦船ID" },
			{ "Level", "レベル" },
			{ "ExpTotal", "経験値" },
			{ "ExpNext", "次のレベルまで" },
			{ "HPCurrent", "現在HP" },
			{ "HPMax", "最大HP" },
			{ "Range", "射程" },		//現在の射程
			{ "SlotMaster[0].EquipmentID", "装備ID[0]" },	//checkme: 要る?
			{ "SlotMaster[1].EquipmentID", "装備ID[1]" },
			{ "SlotMaster[2].EquipmentID", "装備ID[2]" },
			{ "SlotMaster[3].EquipmentID", "装備ID[3]" },
			{ "SlotMaster[4].EquipmentID", "装備ID[4]" },
			{ "ExpansionSlotMaster", "補強装備ID" },
			{ "Aircraft[0]", "搭載機数[0]" },
			{ "Aircraft[1]", "搭載機数[1]" },
			{ "Aircraft[2]", "搭載機数[2]" },
			{ "Aircraft[3]", "搭載機数[3]" },
			{ "Aircraft[4]", "搭載機数[4]" },
			{ "Fuel", "搭載燃料" },
			{ "Ammo", "搭載弾薬" },
			{ "SlotSize", "スロットサイズ" },
			{ "RepairTime", "入渠時間" },
			{ "RepairSteel", "入渠消費鋼材" },
			{ "RepairFuel", "入渠消費燃料" },
			{ "Condition", "コンディション" },
			//強化値シリーズは省略
			{ "FirepowerRemain", "火力改修残り" },
			{ "TorpedoRemain", "雷装改修残り" },
			{ "AARemain", "対空改修残り" },
			{ "ArmorRemain", "装甲改修残り" },
			{ "LuckRemain", "運改修残り" },
			{ "FirepowerTotal", "合計火力" },
			{ "TorpedoTotal", "合計雷装" },
			{ "AATotal", "合計対空" },
			{ "ArmorTotal", "合計装甲" },
			{ "EvasionTotal", "合計回避" },
			{ "ASWTotal", "合計対潜" },
			{ "LOSTotal", "合計索敵" },
			{ "LuckTotal", "合計運" },
			{ "FirepowerBase", "基本火力" },
			{ "TorpedoBase", "基本雷装" },
			{ "AABase", "基本対空" },
			{ "ArmorBase", "基本装甲" },
			{ "EvasionBase", "基本回避" },
			{ "ASWBase", "基本対潜" },
			{ "LOSBase", "基本索敵" },
			{ "LuckBase", "基本運" },
			{ "IsLocked", "ロック" },
			{ "IsLockedByEquipment", "装備ロック" },
			{ "SallyArea", "出撃海域" },
			{ "RepairingDockID", "入渠ドックID" },
			{ "FleetWithIndex", "所属艦隊" },
			{ "IsMarried", "ケッコン" },
			{ "ExpNextRemodel", "次の改装まで" },
			{ "HPRate", "HP割合" },
			{ "MasterShip.AlbumNo", "図鑑番号" },
			{ "MasterShip.NameWithClass", "艦名" },
			{ "MasterShip.NameReading", "艦名読み" },
			{ "MasterShip.ShipType", "艦種" },
			{ "MasterShip.RemodelAfterShipID", "改装後艦船ID" },
			{ "MasterShip.RemodelBeforeShipID", "改装前艦船ID" },
			//マスターのパラメータ系もおそらく意味がないので省略
			{ "MasterShip.Speed", "速力" },
			{ "MasterShip.Rarity", "レアリティ" },
			{ "MasterShip.Fuel", "最大搭載燃料" },
			{ "MasterShip.Ammo", "最大搭載弾薬" },
			{ "MasterShip.AircraftTotal", "合計艦載機数" },		//要る？
			
		};


		[IgnoreDataMember]
		public static readonly Dictionary<ExpressionOperator, string> ExpressionOperatorNameTable = new Dictionary<ExpressionOperator, string>() {
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
		};



		public ExpressionData() {
		}

		public ExpressionData( string left, ExpressionOperator ope, object right )
			: this() {
			LeftOperand = left;
			Operator = ope;
			RightOperand = right;
			Enabled = true;
		}



		public Expression Compile() {

			Expression memberex = null;
			Expression constex = Expression.Constant( RightOperand, RightOperand.GetType() );

			{
				Match match = regex_index.Match( LeftOperand );
				if ( match.Success ) {

					do {

						if ( memberex == null ) {
							memberex = Expression.PropertyOrField( Expression.Parameter( typeof( ShipData ) ), match.Groups["name"].Value );
						} else {
							memberex = Expression.PropertyOrField( memberex, match.Groups["name"].Value );
						}

						int index;
						if ( int.TryParse( match.Groups["index"].Value, out index ) ) {
							memberex = Expression.ArrayAccess( memberex, Expression.Constant( index, typeof( int ) ) );
						}

					} while ( ( match = match.NextMatch() ).Success );

				} else {
					memberex = Expression.PropertyOrField( Expression.Parameter( typeof( ShipData ) ), LeftOperand );
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

				default:
					throw new NotImplementedException();
			}

			return condex;
		}


		public override string ToString() {
			return string.Format( "{0} は {1} {2}", DisplayNameTable.ContainsKey( LeftOperand ) ? DisplayNameTable[LeftOperand] : LeftOperand, RightOperand.ToString(), ExpressionOperatorNameTable[Operator] );
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
