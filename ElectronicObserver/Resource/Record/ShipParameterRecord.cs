using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Resource.Record {

	[DebuggerDisplay( "{Record.Count} Records" )]
	public class ShipParameterRecord : RecordBase {


		/// <summary>
		/// パラメータの初期値と最大値の予測値を保持します。
		/// </summary>
		[DebuggerDisplay( "[{MinimumEstMin}-{MinimumEstMax}]-{Maximum}" )]
		public class Parameter {

			/// <summary>
			/// 初期値(推測値)
			/// </summary>
			public int Minimum {
				get { return ( MinimumEstMin + MinimumEstMax ) / 2; }
			}

			/// <summary>
			/// 最大値
			/// </summary>
			public int Maximum { get; set; }

			/// <summary>
			/// 初期値の推測下限
			/// </summary>
			public int MinimumEstMin { get; set; }

			/// <summary>
			/// 初期値の推測上限
			/// </summary>
			public int MinimumEstMax { get; set; }


			/// <summary>
			/// 初期値がデフォルト状態かどうか
			/// </summary>
			public bool IsMinimumDefault {
				get { return MinimumEstMin == MinimumDefault && MinimumEstMax == MaximumDefault; }
			}

			/// <summary>
			/// 最大値がデフォルト状態かどうか
			/// </summary>
			public bool IsMaximumDefault {
				get { return MinimumEstMin == MinimumDefault && MinimumEstMax == MaximumDefault; }
			}

			/// <summary>
			/// 有効なデータか
			/// </summary>
			public bool IsAvailable {
				get { return !IsMinimumDefault && !IsMaximumDefault; }
			}


			/// <summary>
			/// 最小値の初期値
			/// </summary>
			public const int MinimumDefault = 0;
			
			/// <summary>
			/// 最大値の初期値
			/// </summary>
			public const int MaximumDefault = 9999;


			public Parameter() {
				MinimumEstMin = MinimumDefault;
				MinimumEstMax = MaximumDefault;
				Maximum = MaximumDefault;
			}


			/// <summary>
			/// パラメータを推測します。
			/// </summary>
			/// <param name="level">艦船のレベル。</param>
			/// <param name="current">現在値。</param>
			/// <param name="max">最大値。</param>
			public void SetEstParameter( int level, int current, int max ) {

				Func<int, int, int, int> clamp = ( int _value, int _min, int _max ) => _value < _min ? _min : ( _value > _max ? _max : _value );


				if ( max != MaximumDefault )
					Maximum = max;

				if ( level == 1 ) {
					MinimumEstMin = MinimumEstMax = current;

				} else if ( level != 99 ) {

					double emind = ( 99.0 * ( current + 0.0 ) - max * level ) / ( 99.0 - level );
					double emaxd = ( 99.0 * ( current + 1.0 ) - max * level ) / ( 99.0 - level );

					int emin = clamp( (int)( emind < emaxd ? Math.Ceiling( emind ) : Math.Floor( emaxd + 1.0 ) ), 0, max );
					int emax = clamp( (int)( emind < emaxd ? Math.Ceiling( emaxd - 1.0 ) : Math.Floor( emind ) ), 0, max );

					if ( emax < MinimumEstMin || MinimumEstMax < emin ) {		//明らかに範囲から外れた場合
						MinimumEstMin = emin;
						MinimumEstMax = emax;
					} else {
						MinimumEstMin = Math.Max( MinimumEstMin, emin );
						MinimumEstMax = Math.Min( MinimumEstMax, emax );
					}

				}

			}


			public int GetEstParameterMin( int level ) {
				return MinimumEstMin + (int)( ( Maximum - MinimumEstMin ) * level / 99.0 );
			}

			public int GetEstParameterMax( int level ) {
				return MinimumEstMax + (int)( ( Maximum - MinimumEstMax ) * level / 99.0 );
			}


			public int GetParameter( int level ) {
				return Minimum + (int)( ( Maximum - Minimum ) * level / 99.0 );
			}

		}




		/// <summary>
		/// 各艦船のパラメータを保持します。
		/// </summary>
		[DebuggerDisplay( "[{ID}] : {FleetName}" )]
		public class ShipParameterElement : RecordElementBase {

			/// <summary>
			/// 艦船ID
			/// </summary>
			public int ShipID { get; set; }

			/// <summary>
			/// 艦船名
			/// 可読性向上のために存在します。
			/// </summary>
			public string ShipName {
				get {
					ShipDataMaster ship = KCDatabase.Instance.MasterShips[ShipID];
					if ( ship != null ) {
						return ship.NameWithClass;
					} else {
						return null;
					}
				}
			}


			/// <summary>
			/// 対潜
			/// </summary>
			public Parameter ASW { get; private set; }

			/// <summary>
			/// 回避
			/// </summary>
			public Parameter Evasion { get; private set; }

			/// <summary>
			/// 索敵
			/// </summary>
			public Parameter LOS { get; private set; }


			/// <summary>
			/// 初期装備
			/// </summary>
			public int[] DefaultSlot { get; internal set; }


			/// <summary>
			/// 図鑑の説明文
			/// </summary>
			public string MessageAlbum { get; internal set; }


			public ShipParameterElement()
				: base() {

				ASW = new Parameter();
				Evasion = new Parameter();
				LOS = new Parameter();

				DefaultSlot = null;
				MessageAlbum = null;
			}

			public ShipParameterElement( string line )
				: this() {

				LoadLine( line );
			}


			public override void LoadLine( string line ) {
				string[] elem = line.Split( ",".ToCharArray() );
				if ( elem.Length < 17 ) throw new ArgumentException( "要素数が少なすぎます。" );

				ShipID = int.Parse( elem[0] );

				//ShipName=elem[1]は読み飛ばす

				ASW.MinimumEstMin = int.Parse( elem[2] );
				ASW.MinimumEstMax = int.Parse( elem[3] );
				ASW.Maximum = int.Parse( elem[4] );

				Evasion.MinimumEstMin = int.Parse( elem[5] );
				Evasion.MinimumEstMax = int.Parse( elem[6] );
				Evasion.Maximum = int.Parse( elem[7] );

				LOS.MinimumEstMin = int.Parse( elem[8] );
				LOS.MinimumEstMax = int.Parse( elem[9] );
				LOS.Maximum = int.Parse( elem[10] );


				if ( elem[11].ToLower() == "null" ) {
					DefaultSlot = null;
				
				} else {
					DefaultSlot = new int[5];

					for ( int i = 0; i < DefaultSlot.Length; i++ ) {
						DefaultSlot[i] = int.Parse( elem[i + 11] );
					}
				}

				MessageAlbum = elem[16].ToLower() == "null" ? null : elem[16];

			}


			public override string SaveLine() {
				StringBuilder sb = new StringBuilder();

				sb.AppendFormat( "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
					ShipID,
					ShipName,
					ASW.MinimumEstMin,
					ASW.MinimumEstMax,
					ASW.Maximum,
					Evasion.MinimumEstMin,
					Evasion.MinimumEstMax,
					Evasion.Maximum,
					LOS.MinimumEstMin,
					LOS.MinimumEstMax,
					LOS.Maximum );

				if ( DefaultSlot == null ) {
					sb.Append( ",null,null,null,null,null" );
				} else {
					foreach ( int i in DefaultSlot ) {
						sb.AppendFormat( ",{0}", i );
					}
				}

				sb.AppendFormat( ",{0}", MessageAlbum );

				return sb.ToString();
			}
		}



		public Dictionary<int, ShipParameterElement> Record { get; private set; }
		private int NewShipIDBorder;
		private int RemodelingShipID;
		public bool ParameterLoadFlag { get; set; }


		public ShipParameterRecord()
			: base() {
			
			Record = new Dictionary<int, ShipParameterElement>();
			NewShipIDBorder = -1;
			RemodelingShipID = -1;
			ParameterLoadFlag = true;

			APIObserver ao = APIObserver.Instance;


			ao.APIList["api_start2"].ResponseReceived += GameStart;

			ao.APIList["api_port/port"].ResponseReceived += ParameterLoaded;

			ao.APIList["api_get_member/picture_book"].ResponseReceived += AlbumOpened;

			//戦闘系：最初のフェーズのみ要るから夜戦(≠開幕)は不要
			ao.APIList["api_req_sortie/battle"].ResponseReceived += BattleStart;
			//ao.APIList["api_req_battle_midnight/battle"].ResponseReceived += BattleStart;
			ao.APIList["api_req_battle_midnight/sp_midnight"].ResponseReceived += BattleStart;
			ao.APIList["api_req_sortie/airbattle"].ResponseReceived += BattleStart;
			ao.APIList["api_req_combined_battle/battle"].ResponseReceived += BattleStart;
			//ao.APIList["api_req_combined_battle/midnight_battle"].ResponseReceived += BattleStart;
			ao.APIList["api_req_combined_battle/sp_midnight"].ResponseReceived += BattleStart;
			ao.APIList["api_req_combined_battle/airbattle"].ResponseReceived += BattleStart;
			ao.APIList["api_req_combined_battle/battle_water"].ResponseReceived += BattleStart;

			ao.APIList["api_req_map/start"].ResponseReceived += SortieStart;
			ao.APIList["api_port/port"].ResponseReceived += SortieEnd;

			ao.APIList["api_req_kousyou/getship"].ResponseReceived += ConstructionReceived;

			ao.APIList["api_req_kaisou/remodeling"].RequestReceived += RemodelingStart;
			ao.APIList["api_get_member/slot_item"].ResponseReceived += RemodelingEnd;

		}

		

		public ShipParameterElement this[int i] {
			get {
				return Record.ContainsKey( i ) ? Record[i] : null;
			}
			set {
				if ( !Record.ContainsKey( i ) ) {
					Record.Add( i, value );
				} else {
					Record[i] = value;
				}
			}
		}


		/// <summary>
		/// レコードの要素を更新します。
		/// </summary>
		/// <param name="elem">更新する要素。</param>
		public void Update( ShipParameterElement elem ) {
			this[elem.ShipID] = elem;
		}


		/// <summary>
		/// パラメータを更新します。
		/// </summary>
		/// <param name="ship">対象の艦船。</param>
		public void UpdateParameter( ShipData ship ) {

			UpdateParameter( ship.ShipID, ship.Level, ship.ASWBase, ship.ASWMax, ship.EvasionBase, ship.EvasionMax, ship.LOSBase, ship.LOSMax );
		}


		/// <summary>
		/// パラメータを更新します。
		/// </summary>
		public void UpdateParameter( int shipID, int level, int aswMin, int aswMax, int evasionMin, int evasionMax, int losMin, int losMax ) {

			ShipParameterElement e = this[shipID];
			if ( e == null ) {
				e = new ShipParameterElement();
				e.ShipID = shipID;
				Utility.Logger.Add( 2, KCDatabase.Instance.MasterShips[shipID].NameWithClass + "のパラメータを記録しました。" );
			}

			e.ASW.SetEstParameter( level, aswMin, aswMax );
			e.Evasion.SetEstParameter( level, evasionMin, evasionMax );
			e.LOS.SetEstParameter( level, losMin, losMax );

			Update( e );
			Utility.Logger.Add( 1, KCDatabase.Instance.MasterShips[shipID].NameWithClass + "のパラメータを更新しました。" );
		}


		/// <summary>
		/// 初期装備を更新します。
		/// </summary>
		/// <param name="ship">対象の艦船。入手直後・改装直後のものに限ります。</param>
		public void UpdateDefaultSlot( ShipData ship ) {

			int[] slot = new int[ship.Slot.Count];

			for ( int i = 0; i < slot.Length; i++ ) {
				if ( ship.Slot[i] == -1 )
					slot[i] = -1;
				else
					slot[i] = KCDatabase.Instance.Equipments[ship.Slot[i]].EquipmentID;
			}

			UpdateDefaultSlot( ship.ShipID, slot );
		}

		/// <summary>
		/// 初期装備を更新します。
		/// </summary>
		/// <param name="shipID">艦船ID。</param>
		/// <param name="slot">装備スロット配列。</param>
		public void UpdateDefaultSlot( int shipID, int[] slot ) {

			ShipParameterElement e = this[shipID];
			if ( e == null ) {
				e = new ShipParameterElement();
				e.ShipID = shipID;
				Utility.Logger.Add( 2, KCDatabase.Instance.MasterShips[shipID].NameWithClass + "の初期装備を記録しました。" );
			}

			e.DefaultSlot = slot;

			Update( e );
			Utility.Logger.Add( 1, KCDatabase.Instance.MasterShips[shipID].NameWithClass + "の初期装備を更新しました。" );
		}




		#region API Events

		/// <summary>
		/// ゲーム開始時にパラメータ読み込みフラグを解除、初回読み込みの準備をします。
		/// </summary>
		private void GameStart( string apiname, dynamic data ) {

			ParameterLoadFlag = true;

		}
		

		/// <summary>
		/// 保有艦船から各パラメータを読み込みます。
		/// </summary>
		void ParameterLoaded( string apiname, dynamic data ) {

			if ( !ParameterLoadFlag ) return;


			foreach ( ShipData ship in KCDatabase.Instance.Ships.Values ) {

				UpdateParameter( ship );

			}

			ParameterLoadFlag = false;		//一回限り(基本的に起動直後の1回)

		}


		/// <summary>
		/// 艦娘図鑑から回避・対潜の初期値及び説明文を読み込みます。
		/// </summary>
		private void AlbumOpened( string apiname, dynamic data ) {

			foreach ( dynamic elem in data.api_list ) {

				if ( !elem.api_yomi() ) break;		//装備図鑑だった場合終了


				int shipID = (int)elem.api_table_id[0];

				ShipParameterElement e = this[shipID];
				if ( e == null ) {
					e = new ShipParameterElement();
					e.ShipID = shipID;
					Utility.Logger.Add( 2, KCDatabase.Instance.MasterShips[shipID].NameWithClass + "のパラメータを記録しました。" );
				}

				e.ASW.SetEstParameter( 1, (int)elem.api_tais, Parameter.MaximumDefault );
				e.Evasion.SetEstParameter( 1, (int)elem.api_kaih, Parameter.MaximumDefault ); 
				

				{	//図鑑説明文登録(図鑑に載っていない改装艦に関してはその改装前の艦の説明文を設定する)
					e.MessageAlbum = elem.api_sinfo;

					ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];
					while ( ship != null && ship.RemodelAfterShipID > 0 ) {
						ShipParameterElement e2 = this[ship.RemodelAfterShipID];
						if ( e2 == null ) {
							e2 = new ShipParameterElement();
							e2.ShipID = ship.RemodelAfterShipID;
						}
						if ( e2.MessageAlbum == null ) {
							e2.MessageAlbum = e.MessageAlbum;
							Update( e2 );
						}

						ship = KCDatabase.Instance.MasterShips[ship.RemodelAfterShipID];
					}
				}
				

				Update( e );
				Utility.Logger.Add( 1, KCDatabase.Instance.MasterShips[shipID].NameWithClass + "のパラメータを更新しました。" );
			}
		}

		/// <summary>
		/// 戦闘開始時の情報から敵艦の装備を読み込みます。
		/// </summary>
		private void BattleStart( string apiname, dynamic data ) {

			int[] efleet = (int[])data.api_ship_ke;


			//[0]はダミー(-1)
			for ( int i = 1; i < efleet.Length; i++ ) {
				if ( efleet[i] == -1 ) continue;

				UpdateDefaultSlot( efleet[i], (int[])( data.api_eSlot[i - 1] ) );
			}

		}


		/// <summary>
		/// 出撃開始時の最新艦を記録します。SortieEndで使用されます。
		/// </summary>
		private void SortieStart( string apiname, dynamic data ) {

			NewShipIDBorder = KCDatabase.Instance.Ships.Max( ( KeyValuePair<int, ShipData> s ) => s.Value.MasterID );

		}


		/// <summary>
		/// 艦隊帰投時に新規入手艦を取得、情報を登録します。
		/// </summary>
		private void SortieEnd( string apiname, dynamic data ) {

			if ( NewShipIDBorder == -1 ) return;

			foreach ( ShipData s in KCDatabase.Instance.Ships.Values.Where( ( ShipData s ) => s.MasterID > NewShipIDBorder ) ) {

				UpdateParameter( s );
				UpdateDefaultSlot( s );

			}

			NewShipIDBorder = -1;
		}


		/// <summary>
		/// 艦船建造時に新規入手艦を取得、情報を登録します。
		/// </summary>
		private void ConstructionReceived( string apiname, dynamic data ) {

			int shipID = (int)data.api_id;
			ShipData ship = KCDatabase.Instance.Ships.Values.FirstOrDefault( ( ShipData s ) => s.MasterID == shipID );

			if ( ship != null ) {
				UpdateParameter( ship );
				UpdateDefaultSlot( ship );
			}

		}


		/// <summary>
		/// 改装艦のIDを記録します。RemodelingEndで使用されます。
		/// </summary>
		void RemodelingStart( string apiname, dynamic data ) {

			RemodelingShipID = int.Parse( data["api_id"] );

		}

		/// <summary>
		/// 改装艦のパラメータと装備を記録します。
		/// </summary>
		void RemodelingEnd( string apiname, dynamic data ) {

			if ( RemodelingShipID == -1 ) return;

			ShipData ship = KCDatabase.Instance.Ships[RemodelingShipID];

			if ( ship != null ) {
				UpdateParameter( ship );
				UpdateDefaultSlot( ship );
			}

			RemodelingShipID = -1;
		}

		

		#endregion



		protected override void LoadLine( string line ) {
			Update( new ShipParameterElement( line ) );
		}

		protected override string SaveLines() {

			StringBuilder sb = new StringBuilder();

			var list = Record.Values.ToList();
			list.Sort( ( e1, e2 ) => e1.ShipID - e2.ShipID );

			foreach ( var elem in list ) {
				sb.AppendLine( elem.SaveLine() );
			}

			return sb.ToString();
		}

		protected override void ClearRecord() {
			Record.Clear();
		}

		protected override string RecordHeader {
			get { return "艦船ID,艦船名,対潜初期下限,対潜初期上限,対潜最大,回避初期下限,回避初期上限,回避最大,索敵初期下限,索敵初期上限,索敵最大,装備1,装備2,装備3,装備4,装備5,図鑑説明"; }
		}

		public override string FileName {
			get { return "ShipParameterRecord.csv"; }
		}
	}
}
