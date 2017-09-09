using Codeplex.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	/// <summary>
	/// 個別の装備データを保持します。
	/// </summary>
	[DebuggerDisplay( "[{ID}] : {NameWithLevel}" )]
	public class EquipmentData : ResponseWrapper, IIdentifiable {

		/// <summary>
		/// 艦載機熟練度の文字列表現
		/// </summary>
		public static readonly string[] AircraftLevelString = { 
				"",
				"|",
				"||",
				"|||",
				"/",
				"//",
				"///",
				">>",
			};


		/// <summary>
		/// 装備を一意に識別するID
		/// </summary>
		public int MasterID {
			get { return (int)RawData.api_id; }
		}

		/// <summary>
		/// 装備ID
		/// </summary>
		public int EquipmentID {
			get { return (int)RawData.api_slotitem_id; }
		}


		/// <summary>
		/// 保護ロック
		/// </summary>
		public bool IsLocked {
			get { return (int)RawData.api_locked != 0; }
		}

		/// <summary>
		/// 改修Level
		/// </summary>
		public int Level {
			get { return (int)RawData.api_level; }
		}


		/// <summary>
		/// 艦載機熟練度
		/// </summary>
		public int AircraftLevel {
			get { return RawData.api_alv() ? (int)RawData.api_alv : 0; }
		}



		/// <summary>
		/// 装備のマスターデータへの参照
		/// </summary>
		public EquipmentDataMaster MasterEquipment {
			get { return KCDatabase.Instance.MasterEquipments[EquipmentID]; }
		}

		/// <summary>
		/// 装備名
		/// </summary>
		public string Name {
			get { return MasterEquipment.Name; }
		}

		/// <summary>
		/// 装備名(レベルを含む)
		/// </summary>
		public string NameWithLevel {
			get {
				var sb = new StringBuilder( Name );

				if ( Level > 0 )
					sb.Append( "+" ).Append( Level );
				if ( AircraftLevel > 0 )
					sb.Append( " " ).Append( AircraftLevelString[AircraftLevel] );

				return sb.ToString();
			}
		}


		/// <summary>
		/// 配置転換中かどうか
		/// </summary>
		public bool IsRelocated { get { return KCDatabase.Instance.RelocatedEquipments.Keys.Contains( MasterID ); } }



		public int ID {
			get { return MasterID; }
		}


		public override void LoadFromResponse( string apiname, dynamic data ) {

			switch ( apiname ) {
				case "api_req_kousyou/createitem":		//不足パラメータの追加
				case "api_req_kousyou/getship":
					data.api_locked = 0;
					data.api_level = 0;
					break;

				case "api_get_member/ship3":			//存在しないアイテムを追加…すると処理に不都合があるので、ID:1で我慢　一瞬だし無問題（？）
					{
						int id = data;
						data = new DynamicJson();
						data.api_id = id;
						data.api_slotitem_id = 1;
						data.api_locked = 0;
						data.api_level = 0;
					} break;

				default:
					break;
			}

			base.LoadFromResponse( apiname, (object)data );

		}


		public override string ToString() {
			return NameWithLevel;
		}

	}


}
