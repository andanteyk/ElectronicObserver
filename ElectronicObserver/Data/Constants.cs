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
					return "陸上";
				case 5:
					return "低速";
				case 10:
					return "高速";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// 射程を表す文字列を取得します。
		/// </summary>
		public static string GetRange( int value ) {
			switch ( value ) {
				case 0:
					return "無";
				case 1:
					return "短";
				case 2:
					return "中";
				case 3:
					return "長";
				case 4:
					return "超長";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// 艦船のレアリティを表す文字列を取得します。
		/// </summary>
		public static string GetShipRarity( int value ) {
			switch ( value ) {
				case 0:
					return "赤";
				case 1:
					return "藍";
				case 2:
					return "青";
				case 3:
					return "水";
				case 4:
					return "銀";
				case 5:
					return "金";
				case 6:
					return "虹";
				case 7:
					return "輝虹";
				case 8:
					return "桜虹";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// 装備のレアリティを表す文字列を取得します。
		/// </summary>
		public static string GetEquipmentRarity( int value ) {
			switch ( value ) {
				case 0:
					return "コモン";
				case 1:
					return "レア";
				case 2:
					return "ホロ";
				case 3:
					return "Sホロ";
				case 4:
					return "SSホロ";
				case 5:
					return "EXホロ";
				default:
					return "不明";
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
					return "-";
				case 1:
					return "時報";
				case 2:
					return "放置";
				case 3:
					return "時報+放置";
				default:
					return "不明";
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
				return "退避";
			else if ( hprate <= 0.0 )
				return isPractice ? "離脱" : ( !isLandBase ? "撃沈" : "破壊" );
			else if ( hprate <= 0.25 )
				return !isLandBase ? "大破" : "損壊";
			else if ( hprate <= 0.5 )
				return !isLandBase ? "中破" : "損害";
			else if ( hprate <= 0.75 )
				return !isLandBase ? "小破" : "混乱";
			else if ( hprate < 1.0 )
				return "健在";
			else
				return "無傷";

		}

		#endregion


		#region 出撃

		/// <summary>
		/// マップ上のセルでのイベントを表す文字列を取得します。
		/// </summary>
		public static string GetMapEventID( int value ) {

			switch ( value ) {

				case 0:
					return "初期位置";
				case 1:
					return "なし";
				case 2:
					return "資源";
				case 3:
					return "渦潮";
				case 4:
					return "通常戦闘";
				case 5:
					return "ボス戦闘";
				case 6:
					return "気のせいだった";
				case 7:
					return "航空戦";
				case 8:
					return "船団護衛成功";
				case 9:
					return "揚陸地点";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// マップ上のセルでのイベント種別を表す文字列を取得します。
		/// </summary>
		public static string GetMapEventKind( int value ) {

			switch ( value ) {
				case 0:
					return "非戦闘";
				case 1:
					return "昼夜戦";
				case 2:
					return "夜戦";
				case 3:
					return "夜昼戦";
				case 4:
					return "航空戦";
				case 6:
					return "空襲戦";
				default:
					return "不明";
			}
		}


		/// <summary>
		/// 海域難易度を表す文字列を取得します。
		/// </summary>
		public static string GetDifficulty( int value ) {

			switch ( value ) {
				case -1:
					return "なし";
				case 0:
					return "未選択";
				case 1:
					return "丙";
				case 2:
					return "乙";
				case 3:
					return "甲";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// 海域難易度を表す数値を取得します。
		/// </summary>
		public static int GetDifficulty( string value ) {

			switch ( value ) {
				case "未選択":
					return 0;
				case "丙":
					return 1;
				case "乙":
					return 2;
				case "甲":
					return 3;
				default:
					return -1;
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
					return "単縦陣";
				case 2:
					return "複縦陣";
				case 3:
					return "輪形陣";
				case 4:
					return "梯形陣";
				case 5:
					return "単横陣";
				case 11:
					return "第一警戒航行序列";
				case 12:
					return "第二警戒航行序列";
				case 13:
					return "第三警戒航行序列";
				case 14:
					return "第四警戒航行序列";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// 陣形を表す数値を取得します。
		/// </summary>
		public static int GetFormation( string value ) {
			switch ( value ) {
				case "単縦陣":
					return 1;
				case "複縦陣":
					return 2;
				case "輪形陣":
					return 3;
				case "梯形陣":
					return 4;
				case "単横陣":
					return 5;
				case "第一警戒航行序列":
					return 11;
				case "第二警戒航行序列":
					return 12;
				case "第三警戒航行序列":
					return 13;
				case "第四警戒航行序列":
					return 14;
				default:
					return -1;
			}
		}

		/// <summary>
		/// 陣形を表す文字列(短縮版)を取得します。
		/// </summary>
		public static string GetFormationShort( int id ) {
			switch ( id ) {
				case 1:
					return "単縦陣";
				case 2:
					return "複縦陣";
				case 3:
					return "輪形陣";
				case 4:
					return "梯形陣";
				case 5:
					return "単横陣";
				case 11:
					return "第一警戒";
				case 12:
					return "第二警戒";
				case 13:
					return "第三警戒";
				case 14:
					return "第四警戒";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// 交戦形態を表す文字列を取得します。
		/// </summary>
		public static string GetEngagementForm( int id ) {
			switch ( id ) {
				case 1:
					return "同航戦";
				case 2:
					return "反航戦";
				case 3:
					return "T字有利";
				case 4:
					return "T字不利";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// 索敵結果を表す文字列を取得します。
		/// </summary>
		public static string GetSearchingResult( int id ) {
			switch ( id ) {
				case 1:
					return "成功";
				case 2:
					return "成功(未帰還有)";
				case 3:
					return "未帰還";
				case 4:
					return "失敗";
				case 5:
					return "成功(非索敵機)";
				case 6:
					return "失敗(非索敵機)";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// 索敵結果を表す文字列(短縮版)を取得します。
		/// </summary>
		public static string GetSearchingResultShort( int id ) {
			switch ( id ) {
				case 1:
					return "成功";
				case 2:
					return "成功△";
				case 3:
					return "未帰還";
				case 4:
					return "失敗";
				case 5:
					return "成功";
				case 6:
					return "失敗";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// 制空戦の結果を表す文字列を取得します。
		/// </summary>
		public static string GetAirSuperiority( int id ) {
			switch ( id ) {
				case 0:
					return "航空均衡";
				case 1:
					return "制空権確保";
				case 2:
					return "航空優勢";
				case 3:
					return "航空劣勢";
				case 4:
					return "制空権喪失";
				default:
					return "不明";
			}
		}



		/// <summary>
		/// 昼戦攻撃種別を表す文字列を取得します。
		/// </summary>
		public static string GetDayAttackKind( int id ) {
			switch ( id ) {
				case 0:
					return "砲撃";
				case 1:
					return "レーザー攻撃";
				case 2:
					return "連続射撃";
				case 3:
					return "カットイン(主砲/副砲)";
				case 4:
					return "カットイン(主砲/電探)";
				case 5:
					return "カットイン(主砲/徹甲)";
				case 6:
					return "カットイン(主砲/主砲)";
				case 7:
					return "空撃";
				case 8:
					return "爆雷攻撃";
				case 9:
					return "雷撃";
				case 10:
					return "ロケット砲撃";
				default:
					return "不明";
			}
		}


		/// <summary>
		/// 夜戦攻撃種別を表す文字列を取得します。
		/// </summary>
		public static string GetNightAttackKind( int id ) {
			switch ( id ) {
				case 0:
					return "砲撃";
				case 1:
					return "連続射撃";
				case 2:
					return "カットイン(主砲/魚雷)";
				case 3:
					return "カットイン(魚雷x2)";
				case 4:
					return "カットイン(主砲x2/副砲)";
				case 5:
					return "カットイン(主砲x3)";
				case 6:
					return "不明";
				case 7:
					return "空撃";
				case 8:
					return "爆雷攻撃";
				case 9:
					return "雷撃";
				case 10:
					return "ロケット砲撃";
				default:
					return "不明";
			}
		}


		/// <summary>
		/// 対空カットイン種別を表す文字列を取得します。
		/// </summary>
		public static string GetAACutinKind( int id ) {
			switch ( id ) {
				case 0:
					return "なし";
				case 1:
					return "高角砲x2/電探";
				case 2:
					return "高角砲/電探";
				case 3:
					return "高角砲x2";
				case 4:
					return "大口径主砲/三式弾/高射装置/電探";
				case 5:
					return "高角砲+高射装置x2/電探";
				case 6:
					return "大口径主砲/三式弾/高射装置";
				case 7:
					return "高角砲/高射装置/電探";
				case 8:
					return "高角砲+高射装置/電探";
				case 9:
					return "高角砲/高射装置";
				case 10:
					return "高角砲/集中機銃/電探";
				case 11:
					return "高角砲/集中機銃";
				case 12:
					return "集中機銃/機銃/電探";
				case 14:
					return "高角砲/機銃/電探";
				case 15:
					return "高角砲/機銃";
				case 16:
					return "高角砲/機銃/電探";
				case 17:
					return "高角砲/機銃";
				default:
					return "不明";
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
					return "不明";
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
					return "燃料";
				case 2:
					return "弾薬";
				case 3:
					return "鋼材";
				case 4:
					return "ボーキサイト";
				case 5:
					return "高速建造材";
				case 6:
					return "高速修復材";
				case 7:
					return "開発資材";
				case 8:
					return "改修資材";
				default:
					return "不明";
			}
		}


		/// <summary>
		/// 階級を表す文字列を取得します。
		/// </summary>
		public static string GetAdmiralRank( int id ) {
			switch ( id ) {
				case 1:
					return "元帥";
				case 2:
					return "大将";
				case 3:
					return "中将";
				case 4:
					return "少将";
				case 5:
					return "大佐";
				case 6:
					return "中佐";
				case 7:
					return "新米中佐";
				case 8:
					return "少佐";
				case 9:
					return "中堅少佐";
				case 10:
					return "新米少佐";
				default:
					return "提督";
			}
		}


		/// <summary>
		/// 任務の発生タイプを表す文字列を取得します。
		/// </summary>
		public static string GetQuestType( int id ) {
			switch ( id ) {
				case 1:		//一回限り
					return "1";
				case 2:		//デイリー
					return "日";
				case 3:		//ウィークリー
					return "週";
				case 4:		//敵空母を3隻撃沈せよ！(日付下一桁0|3|7)
					return "変";
				case 5:		//敵輸送船団を叩け！(日付下一桁2|8)
					return "変";
				case 6:		//マンスリー
					return "月";
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
					return "編成";
				case 2:
					return "出撃";
				case 3:
					return "演習";
				case 4:
					return "遠征";
				case 5:
					return "補給";		//入渠も含むが、文字数の関係
				case 6:
					return "工廠";
				case 7:
					return "改装";
				case 8:
					return "出撃";
				case 9:
					return "他";
				default:
					return "不明";
			}
		}


		/// <summary>
		/// 遠征の結果を表す文字列を取得します。
		/// </summary>
		public static string GetExpeditionResult( int value ) {
			switch ( value ) {
				case 0:
					return "失敗";
				case 1:
					return "成功";
				case 2:
					return "大成功";
				default:
					return "不明";
			}
		}


		/// <summary>
		/// 連合艦隊の編成名を表す文字列を取得します。
		/// </summary>
		public static string GetCombinedFleet( int value ) {
			switch ( value ) {
				case 0:
					return "通常艦隊";
				case 1:
					return "機動部隊";
				case 2:
					return "水上部隊";
				case 3:
					return "輸送部隊";
				default:
					return "不明";
			}
		}

		#endregion

	}

}
