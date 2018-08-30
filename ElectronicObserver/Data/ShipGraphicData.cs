using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data
{
	/// <summary>
	/// 艦船のグラフィック設定情報を保持します。
	/// </summary>
	public class ShipGraphicData : ResponseWrapper, IIdentifiable
	{
		/// <summary>
		/// 艦船ID
		/// </summary>
		public int ShipID => (int)RawData.api_id;

		/// <summary>
		/// リソースファイル名
		/// </summary>
		public string ResourceName => RawData.api_filename;

		/// <summary>
		/// 画像バージョン
		/// </summary>
		public string GraphicVersion => RawData.api_version[0];

		/// <summary>
		/// ボイスバージョン
		/// </summary>
		public string VoiceVersion => RawData.api_version[1];

		/// <summary>
		/// 母港ボイスバージョン
		/// </summary>
		public string PortVoiceVersion => RawData.api_version[2];


		/// <summary>
		/// 母港での表示座標（通常時）
		/// </summary>
		public Point PortLocation => new Point((int)RawData.api_boko_n[0], (int)RawData.api_boko_n[1]);

		/// <summary>
		/// 母港での表示座標（中破時）
		/// </summary>
		public Point PortLocationDamaged => new Point((int)RawData.api_boko_d[0], (int)RawData.api_boko_d[1]);


		/// <summary>
		/// 改修時の表示座標（通常時）
		/// </summary>
		public Point ModernizationLocation => new Point((int)RawData.api_kaisyu_n[0], (int)RawData.api_kaisyu_n[1]);

		/// <summary>
		/// 改修時の表示座標（中破時）
		/// </summary>
		public Point ModernizationLocationDamaged => new Point((int)RawData.api_kaisyu_d[0], (int)RawData.api_kaisyu_d[1]);


		/// <summary>
		/// 改造時の表示座標（通常時）
		/// </summary>
		public Point RemodelLocation => new Point((int)RawData.api_kaizo_n[0], (int)RawData.api_kaizo_n[1]);

		/// <summary>
		/// 改造時の表示座標（中破時）
		/// </summary>
		public Point RemodelLocationDamaged => new Point((int)RawData.api_kaizo_d[0], (int)RawData.api_kaizo_d[1]);


		/// <summary>
		/// 出撃時の表示座標（通常時）
		/// </summary>
		public Point SortieLocation => new Point((int)RawData.api_map_n[0], (int)RawData.api_map_n[1]);

		/// <summary>
		/// 出撃時の表示座標（中破時）
		/// </summary>
		public Point SortieLocationDamaged => new Point((int)RawData.api_map_d[0], (int)RawData.api_map_d[1]);


		/// <summary>
		/// 味方側での演習開始時の表示座標（通常時）
		/// </summary>
		public Point PracticeFriendLocation => new Point((int)RawData.api_ensyuf_n[0], (int)RawData.api_ensyuf_n[1]);

		/// <summary>
		/// 味方側での演習開始時の表示座標（中破時）
		/// </summary>
		public Point PracticeFriendLocationDamaged => new Point((int)RawData.api_ensyuf_d[0], (int)RawData.api_ensyuf_d[1]);


		/// <summary>
		/// 敵側での演習開始時の表示座標（通常時）
		/// </summary>
		public Point PracticeEnemyLocation => new Point((int)RawData.api_ensyuf_n[0], (int)RawData.api_ensyuf_n[1]);


		/// <summary>
		/// 戦闘時の表示座標（通常時）
		/// </summary>
		public Point BattleFriendLocation => new Point((int)RawData.api_battle_n[0], (int)RawData.api_battle_n[1]);

		/// <summary>
		/// 戦闘時の表示座標（中破時）
		/// </summary>
		public Point BattleFriendLocationDamaged => new Point((int)RawData.api_battle_d[0], (int)RawData.api_battle_d[1]);


		/// <summary>
		/// ケッコンカッコカリ時の表示エリア
		/// ≒　顔座標
		/// </summary>
		public Rectangle FaceArea => new Rectangle(
			(int)RawData.api_weda[0],
			(int)RawData.api_weda[1],
			(int)RawData.api_wedb[0],
			(int)RawData.api_wedb[1]);


		public int ID => ShipID;
	}
}
