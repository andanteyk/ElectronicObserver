using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	public static class Constants {

		#region 艦船・装備

		/// <summary>
		/// 艦船の速力を表す文字列を取得します。
		/// </summary>
		public static string GetSpeed( int value ) {
			switch ( value ) {
				case 0:
                    return LoadResources.getter("Constants_1");
				case 5:
                    return LoadResources.getter("Constants_2");
				case 10:
                    return LoadResources.getter("Constants_3");
				default:
					return LoadResources.getter("Constants_4");
			}
		}

		/// <summary>
		/// 射程を表す文字列を取得します。
		/// </summary>
		public static string GetRange( int value ) {
			switch ( value ) {
				case 0:
                    return LoadResources.getter("Constants_5");
				case 1:
                    return LoadResources.getter("Constants_6");
				case 2:
					return LoadResources.getter("Constants_7");
				case 3:
					return LoadResources.getter("Constants_8");
				case 4:
					return LoadResources.getter("Constants_9");
				default:
					return LoadResources.getter("Constants_4");
			}
		}

		/// <summary>
		/// 艦船のレアリティを表す文字列を取得します。
		/// </summary>
		public static string GetShipRarity( int value ) {
			switch ( value ) {
				case 0:
                    return LoadResources.getter("Constants_10");
				case 1:
                    return LoadResources.getter("Constants_11");
				case 2:
                    return LoadResources.getter("Constants_12");
				case 3:
                    return LoadResources.getter("Constants_13");
				case 4:
                    return LoadResources.getter("Constants_14");
				case 5:
                    return LoadResources.getter("Constants_15");
				case 6:
                    return LoadResources.getter("Constants_16");
				case 7:
                    return LoadResources.getter("Constants_17");
				case 8:
                    return LoadResources.getter("Constants_18");
				default:
					return LoadResources.getter("Constants_4");
			}
		}

		/// <summary>
		/// 装備のレアリティを表す文字列を取得します。
		/// </summary>
		public static string GetEquipmentRarity( int value ) {
			switch ( value ) {
				case 0:
                    return LoadResources.getter("Constants_19");
				case 1:
                    return LoadResources.getter("Constants_20");
				case 2:
                    return LoadResources.getter("Constants_21");
				case 3:
                    return LoadResources.getter("Constants_22");
				case 4:
                    return LoadResources.getter("Constants_23");
				case 5:
                    return LoadResources.getter("Constants_24");
				default:
					return LoadResources.getter("Constants_4");
			}
		}

		/// <summary>
		/// 装備のレアリティの画像インデックスを取得します。
		/// </summary>
		public static int GetEquipmentRarityID( int value ) {
			switch ( value ) {
				case 0:
					return 1;
				case 1:
					return 4;
				case 2:
					return 5;
				case 3:
					return 6;
				case 4:
					return 7;
				case 5:
					return 8;
				default:
					return 0;
			}
		}


		/// <summary>
		/// 艦船のボイス設定フラグを表す文字列を取得します。
		/// </summary>
		public static string GetVoiceFlag( int value ) {
			switch ( value ) {
				case 0:
                    return LoadResources.getter("Constants_25");
				case 1:
                    return LoadResources.getter("Constants_26");
				case 2:
                    return LoadResources.getter("Constants_27");
				case 3:
                    return LoadResources.getter("Constants_28");
				default:
					return LoadResources.getter("Constants_4");
			}
		}


		/// <summary>
		/// 艦船の損傷度合いを表す文字列を取得します。
		/// </summary>
		/// <param name="hprate">現在HP/最大HPで表される割合。</param>
		/// <param name="isPractice">演習かどうか。</param>
		/// <param name="isLandBase">陸上基地かどうか。</param>
		/// <param name="isEscaped">退避中かどうか。</param>
		/// <returns></returns>
		public static string GetDamageState( double hprate, bool isPractice = false, bool isLandBase = false, bool isEscaped = false ) {

			if ( isEscaped )
                return LoadResources.getter("Constants_29");
			else if ( hprate <= 0.0 )
                return isPractice ? LoadResources.getter("Constants_30") : (!isLandBase ? LoadResources.getter("Constants_31") : LoadResources.getter("Constants_32"));
			else if ( hprate <= 0.25 )
                return !isLandBase ? LoadResources.getter("Constants_33") : LoadResources.getter("Constants_34");
			else if ( hprate <= 0.5 )
                return !isLandBase ? LoadResources.getter("Constants_35") : LoadResources.getter("Constants_36");
			else if ( hprate <= 0.75 )
                return !isLandBase ? LoadResources.getter("Constants_37") : LoadResources.getter("Constants_38");
			else if ( hprate < 1.0 )
                return LoadResources.getter("Constants_39");
			else
                return LoadResources.getter("Constants_40");

		}

		#endregion


		#region 出撃

		/// <summary>
		/// マップ上のセルでのイベントを表す文字列を取得します。
		/// </summary>
		public static string GetMapEventID( int value ) {

			switch ( value ) {

				case 0:
                    return LoadResources.getter("Constants_41");
				case 1:
                    return LoadResources.getter("Constants_42");
				case 2:
                    return LoadResources.getter("Constants_43");
				case 3:
                    return LoadResources.getter("Constants_44");
				case 4:
                    return LoadResources.getter("Constants_45");
				case 5:
                    return LoadResources.getter("Constants_46");
				case 6:
                    return LoadResources.getter("Constants_47");
				case 7:
                    return LoadResources.getter("Constants_48");
				case 8:
                    return LoadResources.getter("Constants_49");
				default:
					return LoadResources.getter("Constants_4");
			}
		}

		/// <summary>
		/// マップ上のセルでのイベント種別を表す文字列を取得します。
		/// </summary>
		public static string GetMapEventKind( int value ) {

			switch ( value ) {
				case 0:
                    return LoadResources.getter("Constants_50");
				case 1:
                    return LoadResources.getter("Constants_51");
				case 2:
                    return LoadResources.getter("Constants_52");
				case 3:
                    return LoadResources.getter("Constants_53");
				case 4:
                    return LoadResources.getter("Constants_54");
				default:
					return LoadResources.getter("Constants_4");
			}
		}

		#endregion


		#region 戦闘

		/// <summary>
		/// 陣形を表す文字列を取得します。
		/// </summary>
		public static string GetFormation( int id ) {
			switch ( id ) {
				case 1:
                    return LoadResources.getter("Constants_55");
				case 2:
                    return LoadResources.getter("Constants_56");
				case 3:
                    return LoadResources.getter("Constants_57");
				case 4:
                    return LoadResources.getter("Constants_58");
				case 5:
                    return LoadResources.getter("Constants_59");
				case 11:
                    return LoadResources.getter("Constants_60");
				case 12:
                    return LoadResources.getter("Constants_61");
				case 13:
                    return LoadResources.getter("Constants_62");
				case 14:
                    return LoadResources.getter("Constants_63");
				default:
					return LoadResources.getter("Constants_4");
			}
		}

		/// <summary>
		/// 陣形を表す文字列(短縮版)を取得します。
		/// </summary>
		public static string GetFormationShort( int id ) {
			switch ( id ) {
				case 1:
                    return LoadResources.getter("Constants_64");
				case 2:
                    return LoadResources.getter("Constants_65");
				case 3:
                    return LoadResources.getter("Constants_66");
				case 4:
                    return LoadResources.getter("Constants_67");
				case 5:
                    return LoadResources.getter("Constants_68");
				case 11:
                    return LoadResources.getter("Constants_69");
				case 12:
                    return LoadResources.getter("Constants_70");
				case 13:
                    return LoadResources.getter("Constants_71");
				case 14:
                    return LoadResources.getter("Constants_72");
				default:
					return LoadResources.getter("Constants_4");
			}
		}

		/// <summary>
		/// 交戦形態を表す文字列を取得します。
		/// </summary>
		public static string GetEngagementForm( int id ) {
			switch ( id ) {
				case 1:
                    return LoadResources.getter("Constants_73");
				case 2:
                    return LoadResources.getter("Constants_74");
				case 3:
                    return LoadResources.getter("Constants_75");
				case 4:
                    return LoadResources.getter("Constants_76");
				default:
					return LoadResources.getter("Constants_4");
			}
		}

		/// <summary>
		/// 索敵結果を表す文字列を取得します。
		/// </summary>
		public static string GetSearchingResult( int id ) {
			switch ( id ) {
				case 1:
                    return LoadResources.getter("Constants_77");
				case 2:
                    return LoadResources.getter("Constants_78");
				case 3:
                    return LoadResources.getter("Constants_79");
				case 4:
                    return LoadResources.getter("Constants_80");
				case 5:
                    return LoadResources.getter("Constants_81");
				case 6:
                    return LoadResources.getter("Constants_82");
				default:
					return LoadResources.getter("Constants_4");
			}
		}

		/// <summary>
		/// 索敵結果を表す文字列(短縮版)を取得します。
		/// </summary>
		public static string GetSearchingResultShort( int id ) {
			switch ( id ) {
				case 1:
                    return LoadResources.getter("Constants_83");
				case 2:
                    return LoadResources.getter("Constants_84");
				case 3:
                    return LoadResources.getter("Constants_85");
				case 4:
                    return LoadResources.getter("Constants_86");
				case 5:
                    return LoadResources.getter("Constants_87");
				case 6:
                    return LoadResources.getter("Constants_88");
				default:
					return LoadResources.getter("Constants_4");
			}
		}

		/// <summary>
		/// 制空戦の結果を表す文字列を取得します。
		/// </summary>
		public static string GetAirSuperiority( int id ) {
			switch ( id ) {
				case 0:
                    return LoadResources.getter("Constants_89");
				case 1:
                    return LoadResources.getter("Constants_90");
				case 2:
                    return LoadResources.getter("Constants_91");
				case 3:
                    return LoadResources.getter("Constants_92");
				case 4:
                    return LoadResources.getter("Constants_93");
				default:
					return LoadResources.getter("Constants_4");
			}
		}



		/// <summary>
		/// 昼戦攻撃種別を表す文字列を取得します。
		/// </summary>
		public static string GetDayAttackKind( int id ) {
			switch ( id ) {
				case 0:
                    return LoadResources.getter("Constants_94");
				case 1:
                    return LoadResources.getter("Constants_95");
				case 2:
                    return LoadResources.getter("Constants_96");
				case 3:
                    return LoadResources.getter("Constants_97");
				case 4:
                    return LoadResources.getter("Constants_98");
				case 5:
                    return LoadResources.getter("Constants_99");
				case 6:
                    return LoadResources.getter("Constants_100");
				case 7:
                    return LoadResources.getter("Constants_101");
				case 8:
                    return LoadResources.getter("Constants_102");
				case 9:
                    return LoadResources.getter("Constants_103");
				case 10:
                    return LoadResources.getter("Constants_104");
				default:
					return LoadResources.getter("Constants_4");
			}
		}


		/// <summary>
		/// 夜戦攻撃種別を表す文字列を取得します。
		/// </summary>
		public static string GetNightAttackKind( int id ) {
			switch ( id ) {
				case 0:
                    return LoadResources.getter("Constants_105");
				case 1:
                    return LoadResources.getter("Constants_106");
				case 2:
                    return LoadResources.getter("Constants_107");
				case 3:
                    return LoadResources.getter("Constants_108");
				case 4:
                    return LoadResources.getter("Constants_109");
				case 5:
                    return LoadResources.getter("Constants_110");
				case 6:
                    return LoadResources.getter("Constants_111");
				case 7:
                    return LoadResources.getter("Constants_112");
				case 8:
                    return LoadResources.getter("Constants_113");
				case 9:
                    return LoadResources.getter("Constants_114");
				case 10:
                    return LoadResources.getter("Constants_115");
				default:
					return LoadResources.getter("Constants_4");
			}
		}


		/// <summary>
		/// 対空カットイン種別を表す文字列を取得します。
		/// </summary>
		public static string GetAACutinKind( int id ) {
			switch ( id ) {
				case 0:
                    return LoadResources.getter("Constants_116");
				case 1:
                    return LoadResources.getter("Constants_117");
				case 2:
                    return LoadResources.getter("Constants_118");
				case 3:
                    return LoadResources.getter("Constants_119");
				case 4:
                    return LoadResources.getter("Constants_120");
				case 5:
					return LoadResources.getter("Constants_121");
				case 6:
					return LoadResources.getter("Constants_122");
				case 7:
                    return LoadResources.getter("Constants_123");
				case 8:
                    return LoadResources.getter("Constants_124");
				case 9:
                    return LoadResources.getter("Constants_125");
				case 10:
                    return LoadResources.getter("Constants_126");
				case 11:
                    return LoadResources.getter("Constants_127");
				case 12:
                    return LoadResources.getter("Constants_128");
				default:
					return LoadResources.getter("Constants_4");
			}
		}


		/// <summary>
		/// 勝利ランクを表すIDを取得します。
		/// </summary>
		public static int GetWinRank( string rank ) {
			switch ( rank.ToUpper() ) {
				case "E":
					return 1;
				case "D":
					return 2;
				case "C":
					return 3;
				case "B":
					return 4;
				case "A":
					return 5;
				case "S":
					return 6;
				case "SS":
					return 7;
				default:
					return 0;
			}
		}

		/// <summary>
		/// 勝利ランクを表す文字列を取得します。
		/// </summary>
		public static string GetWinRank( int rank ) {
			switch ( rank ) {
				case 1:
					return "E";
				case 2:
					return "D";
				case 3:
					return "C";
				case 4:
					return "B";
				case 5:
					return "A";
				case 6:
					return "S";
				case 7:
					return "SS";
				default:
					return LoadResources.getter("Constants_4");
			}
		}

		#endregion


		#region その他

		/// <summary>
		/// 資源の名前を取得します。
		/// </summary>
		/// <param name="materialID">資源のID。</param>
		/// <returns>資源の名前。</returns>
		public static string GetMaterialName( int materialID ) {

			switch ( materialID ) {
				case 1:
					return LoadResources.getter("Constants_129");
				case 2:
                    return LoadResources.getter("Constants_130");
				case 3:
                    return LoadResources.getter("Constants_131");
				case 4:
                    return LoadResources.getter("Constants_132");
				case 5:
                    return LoadResources.getter("Constants_133");
				case 6:
                    return LoadResources.getter("Constants_134");
				case 7:
                    return LoadResources.getter("Constants_135");
				case 8:
                    return LoadResources.getter("Constants_136");
				default:
					return LoadResources.getter("Constants_4");
			}
		}


		/// <summary>
		/// 階級を表す文字列を取得します。
		/// </summary>
		public static string GetAdmiralRank( int id ) {
			switch ( id ) {
				case 1:
                    return LoadResources.getter("Constants_137");
				case 2:
                    return LoadResources.getter("Constants_138");
				case 3:
                    return LoadResources.getter("Constants_139");
				case 4:
                    return LoadResources.getter("Constants_140");
				case 5:
                    return LoadResources.getter("Constants_141");
				case 6:
                    return LoadResources.getter("Constants_142");
				case 7:
                    return LoadResources.getter("Constants_143");
				case 8:
                    return LoadResources.getter("Constants_144");
				case 9:
                    return LoadResources.getter("Constants_145");
				case 10:
                    return LoadResources.getter("Constants_146");
				default:
                    return LoadResources.getter("Constants_147");
			}
		}


		/// <summary>
		/// 任務の発生タイプを表す文字列を取得します。
		/// </summary>
		public static string GetQuestType( int id ) {
			switch ( id ) {
				case 1:		//一回限り
                    return LoadResources.getter("Constants_148");
				case 2:		//デイリー
                    return LoadResources.getter("Constants_149");
				case 3:		//ウィークリー
                    return LoadResources.getter("Constants_150");
				case 4:		//敵空母を3隻撃沈せよ！(日付下一桁0|3|7)
                    return LoadResources.getter("Constants_151");
				case 5:		//敵輸送船団を叩け！(日付下一桁2|8)
                    return LoadResources.getter("Constants_151");
				case 6:		//マンスリー
                    return LoadResources.getter("Constants_152");
				default:
					return "?";
			}

		}


		/// <summary>
		/// 任務のカテゴリを表す文字列を取得します。
		/// </summary>
		public static string GetQuestCategory( int id ) {
			switch ( id ) {
				case 1:
                    return LoadResources.getter("Constants_153");
				case 2:
                    return LoadResources.getter("Constants_154");
				case 3:
                    return LoadResources.getter("Constants_155");
				case 4:
                    return LoadResources.getter("Constants_156");
				case 5:
                    return LoadResources.getter("Constants_157");		//入渠も含むが、文字数の関係
				case 6:
                    return LoadResources.getter("Constants_158");
				case 7:
                    return LoadResources.getter("Constants_159");
				case 8:
                    return LoadResources.getter("Constants_160");
				default:
					return LoadResources.getter("Constants_4");
			}
		}


		/// <summary>
		/// 遠征の結果を表す文字列を取得します。
		/// </summary>
		public static string GetExpeditionResult( int value ) {
			switch ( value ) {
				case 0:
                    return LoadResources.getter("Constants_161");
				case 1:
                    return LoadResources.getter("Constants_162");
				case 2:
                    return LoadResources.getter("Constants_163");
				default:
					return LoadResources.getter("Constants_4");
			}
		}


		public static string GetCombinedFleet( int value ) {
			switch ( value ) {
				case 0:
                    return LoadResources.getter("Constants_164");
				case 1:
                    return LoadResources.getter("Constants_165");
				case 2:
                    return LoadResources.getter("Constants_166");
				default:
					return LoadResources.getter("Constants_4");
			}
		}

		#endregion

	}

}
