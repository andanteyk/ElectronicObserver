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
				get { return Maximum == MaximumDefault; }
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
			public static int MinimumDefault { get { return 0; } }

			/// <summary>
			/// 最大値の初期値
			/// </summary>
			public static int MaximumDefault { get { return 9999; } }


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
			/// <returns>予測パラメータが範囲外の値をとったとき true 。</returns>
			public bool SetEstParameter( int level, int current, int max ) {

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
						return true;

					} else {
						MinimumEstMin = Math.Max( MinimumEstMin, emin );
						MinimumEstMax = Math.Min( MinimumEstMax, emax );
					}

				}

				return false;
			}



			// level > 99 のとき、最小値と最大値が反転するため
			public int GetEstParameterMin( int level ) {
				int min = CalculateParameter( level, MinimumEstMin, Maximum );
				int max = CalculateParameter( level, MinimumEstMax, Maximum );
				return Math.Min( min, max );
			}

			public int GetEstParameterMax( int level ) {
				int min = CalculateParameter( level, MinimumEstMin, Maximum );
				int max = CalculateParameter( level, MinimumEstMax, Maximum );
				return Math.Max( min, max );
			}

			public int GetParameter( int level ) {
				return CalculateParameter( level, Minimum, Maximum );
			}

			private int CalculateParameter( int level, int min, int max ) {
				return min + (int)( ( max - min ) * level / 99.0 );
			}
		}




		/// <summary>
		/// 各艦船のパラメータを保持します。
		/// </summary>
		[DebuggerDisplay( "[{ShipID}] : {ShipName}" )]
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
			/// 耐久初期値
			/// </summary>
			public int HPMin { get; set; }

			/// <summary>
			/// 耐久最大値
			/// </summary>
			public int HPMax { get; set; }


			/// <summary>
			/// 火力初期値
			/// </summary>
			public int FirepowerMin { get; set; }

			/// <summary>
			/// 火力最大値
			/// </summary>
			public int FirepowerMax { get; set; }

			/// <summary>
			/// 雷装初期値
			/// </summary>
			public int TorpedoMin { get; set; }

			/// <summary>
			/// 雷装最大値
			/// </summary>
			public int TorpedoMax { get; set; }

			/// <summary>
			/// 対空初期値
			/// </summary>
			public int AAMin { get; set; }

			/// <summary>
			/// 対空最大値
			/// </summary>
			public int AAMax { get; set; }

			/// <summary>
			/// 装甲初期値
			/// </summary>
			public int ArmorMin { get; set; }

			/// <summary>
			/// 装甲最大値
			/// </summary>
			public int ArmorMax { get; set; }


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
			/// 運初期値
			/// </summary>
			public int LuckMin { get; set; }

			/// <summary>
			/// 運最大値
			/// </summary>
			public int LuckMax { get; set; }

			/// <summary>
			/// 射程
			/// </summary>
			public int Range { get; set; }


			/// <summary>
			/// 初期装備
			/// </summary>
			public int[] DefaultSlot { get; internal set; }

			/// <summary>
			/// 搭載機数
			/// </summary>
			public int[] Aircraft { get; internal set; }


			/// <summary>
			/// ドロップ時の説明文
			/// </summary>
			public string MessageGet { get; internal set; }

			/// <summary>
			/// 図鑑の説明文
			/// </summary>
			public string MessageAlbum { get; internal set; }



			public ShipParameterElement()
				: base() {

				ASW = new Parameter();
				Evasion = new Parameter();
				LOS = new Parameter();

				Aircraft = null;
				DefaultSlot = null;

				MessageGet = null;
				MessageAlbum = null;
			}

			public ShipParameterElement( string line )
				: this() {

				LoadLine( line );
			}


			public override void LoadLine( string line ) {
				string[] elem = line.Split( ",".ToCharArray() );
				if ( elem.Length < 36 ) throw new ArgumentException( "要素数が少なすぎます。" );

				ShipID = int.Parse( elem[0] );

				//ShipName=elem[1]は読み飛ばす

				HPMin = int.Parse( elem[2] );
				HPMax = int.Parse( elem[3] );

				FirepowerMin = int.Parse( elem[4] );
				FirepowerMax = int.Parse( elem[5] );

				TorpedoMin = int.Parse( elem[6] );
				TorpedoMax = int.Parse( elem[7] );

				AAMin = int.Parse( elem[8] );
				AAMax = int.Parse( elem[9] );

				ArmorMin = int.Parse( elem[10] );
				ArmorMax = int.Parse( elem[11] );


				ASW.MinimumEstMin = int.Parse( elem[12] );
				ASW.MinimumEstMax = int.Parse( elem[13] );
				ASW.Maximum = int.Parse( elem[14] );

				Evasion.MinimumEstMin = int.Parse( elem[15] );
				Evasion.MinimumEstMax = int.Parse( elem[16] );
				Evasion.Maximum = int.Parse( elem[17] );

				LOS.MinimumEstMin = int.Parse( elem[18] );
				LOS.MinimumEstMax = int.Parse( elem[19] );
				LOS.Maximum = int.Parse( elem[20] );


				LuckMin = int.Parse( elem[21] );
				LuckMax = int.Parse( elem[22] );


				Range = int.Parse( elem[23] );


				if ( elem[24].ToLower() == "null" ) {
					DefaultSlot = null;

				} else {
					DefaultSlot = new int[5];

					for ( int i = 0; i < DefaultSlot.Length; i++ ) {

						DefaultSlot[i] = elem[i + 24] == "null" ? -1 : int.Parse( elem[i + 24] );
					}
				}

				if ( elem[29].ToLower() == "null" ) {
					Aircraft = null;

				} else {
					Aircraft = new int[5];

					for ( int i = 0; i < Aircraft.Length; i++ ) {
						Aircraft[i] = elem[i + 29] == "null" ? 0 : int.Parse( elem[i + 29] );
					}
				}


				MessageGet = elem[34].ToLower() == "null" ? null : elem[34];
				MessageAlbum = elem[35].ToLower() == "null" ? null : elem[35];

			}


			public override string SaveLine() {
				StringBuilder sb = new StringBuilder();

				sb.AppendFormat( "{" + string.Join( "},{", Enumerable.Range( 0, 24 ) ) + "}",
					ShipID,
					ShipName,
					HPMin,
					HPMax,
					FirepowerMin,
					FirepowerMax,
					TorpedoMin,
					TorpedoMax,
					AAMin,
					AAMax,
					ArmorMin,
					ArmorMax,
					ASW.MinimumEstMin,
					ASW.MinimumEstMax,
					ASW.Maximum,
					Evasion.MinimumEstMin,
					Evasion.MinimumEstMax,
					Evasion.Maximum,
					LOS.MinimumEstMin,
					LOS.MinimumEstMax,
					LOS.Maximum,
					LuckMin,
					LuckMax,
					Range );

				if ( DefaultSlot == null ) {
					sb.Append( ",null,null,null,null,null" );
				} else {
					foreach ( int i in DefaultSlot ) {
						sb.AppendFormat( ",{0}", i );
					}
				}

				if ( Aircraft == null ) {
					sb.Append( ",null,null,null,null,null" );
				} else {
					foreach ( int i in Aircraft ) {
						sb.AppendFormat( ",{0}", i );
					}
				}


				sb.AppendFormat( ",{0},{1}", MessageGet, MessageAlbum );

				return sb.ToString();
			}
		}



		public Dictionary<int, ShipParameterElement> Record { get; private set; }
		private int newShipIDBorder;
		private int remodelingShipID;
		public bool ParameterLoadFlag { get; set; }


		public ShipParameterRecord()
			: base() {

			Record = new Dictionary<int, ShipParameterElement>();
			newShipIDBorder = -1;
			remodelingShipID = -1;
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
			ao.APIList["api_req_sortie/ld_airbattle"].ResponseReceived += BattleStart;
			ao.APIList["api_req_combined_battle/battle"].ResponseReceived += BattleStart;
			//ao.APIList["api_req_combined_battle/midnight_battle"].ResponseReceived += BattleStart;
			ao.APIList["api_req_combined_battle/sp_midnight"].ResponseReceived += BattleStart;
			ao.APIList["api_req_combined_battle/airbattle"].ResponseReceived += BattleStart;
			ao.APIList["api_req_combined_battle/battle_water"].ResponseReceived += BattleStart;
			ao.APIList["api_req_combined_battle/ld_airbattle"].ResponseReceived += BattleStart;

			ao.APIList["api_req_map/start"].ResponseReceived += SortieStart;
			//ao.APIList["api_port/port"].ResponseReceived += SortieEnd;
			ao.APIList["api_get_member/slot_item"].ResponseReceived += SortieEnd;

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
				Utility.Logger.Add( 2, KCDatabase.Instance.MasterShips[shipID].NameWithClass + "的参数已记录。" );
			}


			if ( e.ASW.SetEstParameter( level, aswMin, aswMax ) )
				Utility.Logger.Add( 1, string.Format( "ShipParameter: {0} の対潜値が予測範囲から外れました( [{1} ~ {2}] ~ {3} )。",
					KCDatabase.Instance.MasterShips[e.ShipID].NameWithClass, e.ASW.MinimumEstMin, e.ASW.MinimumEstMax, e.ASW.Maximum ) );

			if ( e.Evasion.SetEstParameter( level, evasionMin, evasionMax ) )
				Utility.Logger.Add( 1, string.Format( "ShipParameter: {0} の回避値が予測範囲から外れました( [{1} ~ {2}] ~ {3} )。",
					KCDatabase.Instance.MasterShips[e.ShipID].NameWithClass, e.Evasion.MinimumEstMin, e.Evasion.MinimumEstMax, e.Evasion.Maximum ) );

			if ( e.LOS.SetEstParameter( level, losMin, losMax ) )
				Utility.Logger.Add( 1, string.Format( "ShipParameter: {0} の索敵値が予測範囲から外れました( [{1} ~ {2}] ~ {3} )。",
					KCDatabase.Instance.MasterShips[e.ShipID].NameWithClass, e.LOS.MinimumEstMin, e.LOS.MinimumEstMax, e.LOS.Maximum ) );


			Update( e );
		}


		/// <summary>
		/// 初期装備を更新します。
		/// </summary>
		/// <param name="ship">対象の艦船。入手直後・改装直後のものに限ります。</param>
		public void UpdateDefaultSlot( ShipData ship ) {

			UpdateDefaultSlot( ship.ShipID, ship.SlotMaster.ToArray() );
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
				Utility.Logger.Add( 2, KCDatabase.Instance.MasterShips[shipID].NameWithClass + "的初始装备信息已记录。" );
			}

			e.DefaultSlot = slot;

			Update( e );
		}




		#region API Events

		/// <summary>
		/// ゲーム開始時にパラメータ読み込みフラグを解除、初回読み込みの準備をします。
		/// </summary>
		private void GameStart( string apiname, dynamic data ) {

			ParameterLoadFlag = true;


			foreach ( var elem in data.api_mst_ship ) {
				var param = this[(int)elem.api_id];
				if ( param == null ) {
					param = new ShipParameterElement();
					param.ShipID = (int)elem.api_id;
					this[param.ShipID] = param;
				}

				if ( elem.api_taik() ) {
					param.HPMin = (int)elem.api_taik[0];
					param.HPMax = (int)elem.api_taik[1];
				}
				if ( elem.api_houg() ) {
					param.FirepowerMin = (int)elem.api_houg[0];
					param.FirepowerMax = (int)elem.api_houg[1];
				}
				if ( elem.api_raig() ) {
					param.TorpedoMin = (int)elem.api_raig[0];
					param.TorpedoMax = (int)elem.api_raig[1];
				}
				if ( elem.api_tyku() ) {
					param.AAMin = (int)elem.api_tyku[0];
					param.AAMax = (int)elem.api_tyku[1];
				}
				if ( elem.api_souk() ) {
					param.ArmorMin = (int)elem.api_souk[0];
					param.ArmorMax = (int)elem.api_souk[1];
				}
				if ( elem.api_luck() ) {
					param.LuckMin = (int)elem.api_luck[0];
					param.LuckMax = (int)elem.api_luck[1];
				}

				if ( elem.api_leng() ) {
					param.Range = (int)elem.api_leng;
				}
				if ( elem.api_maxeq() ) {
					param.Aircraft = (int[])elem.api_maxeq;
				}
				if ( elem.api_getmes() ) {
					param.MessageGet = elem.api_getmes;
				}

				Update( param );
			}


			// validation
			foreach ( var record in Record.Values ) {

				// 無効な装備を持っていた場合、初期装備データを初期化
				if ( record.DefaultSlot != null ) {

					var keys = KCDatabase.Instance.MasterEquipments.Keys;

					foreach ( int eq in record.DefaultSlot.ToArray() ) {
						if ( eq != -1 && !keys.Contains( eq ) ) {
							record.DefaultSlot = null;
							break;
						}
					}
				}
			}

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

			if ( data == null || !data.api_list() )		//空のページ
				return;

			foreach ( dynamic elem in data.api_list ) {

				if ( !elem.api_yomi() ) break;		//装備図鑑だった場合終了


				int shipID = (int)elem.api_table_id[0];
				var ship = KCDatabase.Instance.MasterShips[shipID];

				ShipParameterElement e = this[shipID];
				if ( e == null ) {
					e = new ShipParameterElement();
					e.ShipID = shipID;
					Utility.Logger.Add( 2, ship.NameWithClass + "的参数已记录。" );
				}


				if ( e.ASW.SetEstParameter( 1, (int)elem.api_tais, Parameter.MaximumDefault ) )
					Utility.Logger.Add( 1, string.Format( "ShipParameter: {0} の対潜値が予測範囲から外れました( [{1} ~ {2}] ~ {3} )。",
						ship.NameWithClass, e.ASW.MinimumEstMin, e.ASW.MinimumEstMax, e.ASW.Maximum ) );

				if ( e.Evasion.SetEstParameter( 1, (int)elem.api_kaih, Parameter.MaximumDefault ) )
					Utility.Logger.Add( 1, string.Format( "ShipParameter: {0} の回避値が予測範囲から外れました( [{1} ~ {2}] ~ {3} )。",
						ship.NameWithClass, e.Evasion.MinimumEstMin, e.Evasion.MinimumEstMax, e.Evasion.Maximum ) );


				{	//図鑑説明文登録(図鑑に載っていない改装艦に関してはその改装前の艦の説明文を設定する)
					e.MessageAlbum = elem.api_sinfo;
					LinkedList<int> processedIDs = new LinkedList<int>();

					while ( ship != null && !processedIDs.Contains( ship.ShipID ) && ship.RemodelAfterShipID > 0 ) {

						processedIDs.AddLast( ship.ID );
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
				//Utility.Logger.Add( 1, KCDatabase.Instance.MasterShips[shipID].NameWithClass + "的参数已更新。" );
			}
		}

		/// <summary>
		/// 戦闘開始時の情報から敵艦の情報を読み込みます。
		/// </summary>
		private void BattleStart( string apiname, dynamic data ) {

			int[] efleet = (int[])data.api_ship_ke;
			int[] hpMax = (int[])data.api_maxhps;

			//[0]はダミー(-1)
			for ( int i = 1; i < efleet.Length; i++ ) {
				if ( efleet[i] == -1 ) continue;

				var param = this[efleet[i]];
				if ( param == null ) {
					param = new ShipParameterElement();
					param.ShipID = efleet[i];
					this[param.ShipID] = param;
				}

				int[] baseparam = (int[])data.api_eParam[i - 1];

				param.HPMin = param.HPMax = hpMax[i + 6];
				param.FirepowerMin = param.FirepowerMax = baseparam[0];
				param.TorpedoMin = param.TorpedoMax = baseparam[1];
				param.AAMin = param.AAMax = baseparam[2];
				param.ArmorMin = param.ArmorMax = baseparam[3];

				param.DefaultSlot = (int[])data.api_eSlot[i - 1];

				Update( param );
			}

		}


		/// <summary>
		/// 出撃開始時の最新艦を記録します。SortieEndで使用されます。
		/// </summary>
		private void SortieStart( string apiname, dynamic data ) {

			newShipIDBorder = KCDatabase.Instance.Ships.Keys.Max();

		}


		/// <summary>
		/// 艦隊帰投時に新規入手艦を取得、情報を登録します。
		/// </summary>
		private void SortieEnd( string apiname, dynamic data ) {

			if ( newShipIDBorder == -1 ) return;

			foreach ( ShipData s in KCDatabase.Instance.Ships.Values.Where( s => s.MasterID > newShipIDBorder ) ) {

				UpdateParameter( s );
				UpdateDefaultSlot( s );

			}

			newShipIDBorder = -1;
		}


		/// <summary>
		/// 艦船建造時に新規入手艦を取得、情報を登録します。
		/// </summary>
		private void ConstructionReceived( string apiname, dynamic data ) {

			int shipID = (int)data.api_id;
			ShipData ship = KCDatabase.Instance.Ships.Values.FirstOrDefault( s => s.MasterID == shipID );

			if ( ship != null ) {
				UpdateParameter( ship );
				UpdateDefaultSlot( ship );
			}

		}


		/// <summary>
		/// 改装艦のIDを記録します。RemodelingEndで使用されます。
		/// </summary>
		void RemodelingStart( string apiname, dynamic data ) {

			remodelingShipID = int.Parse( data["api_id"] );

		}

		/// <summary>
		/// 改装艦のパラメータと装備を記録します。
		/// </summary>
		void RemodelingEnd( string apiname, dynamic data ) {

			if ( remodelingShipID == -1 ) return;

			ShipData ship = KCDatabase.Instance.Ships[remodelingShipID];

			if ( ship != null ) {
				UpdateParameter( ship );
				UpdateDefaultSlot( ship );
			}

			remodelingShipID = -1;
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

		public override string RecordHeader {
			get { return "艦船ID,艦船名,耐久初期,耐久最大,火力初期,火力最大,雷装初期,雷装最大,対空初期,対空最大,装甲初期,装甲最大,対潜初期下限,対潜初期上限,対潜最大,回避初期下限,回避初期上限,回避最大,索敵初期下限,索敵初期上限,索敵最大,運初期,運最大,射程,装備1,装備2,装備3,装備4,装備5,機数1,機数2,機数3,機数4,機数5,ドロップ説明,図鑑説明"; }
		}

		public override string FileName {
			get { return "ShipParameterRecord.csv"; }
		}
	}
}
