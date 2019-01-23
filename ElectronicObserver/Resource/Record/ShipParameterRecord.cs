using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.Observer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Resource.Record
{

	[DebuggerDisplay("{Record.Count} Records")]
	public class ShipParameterRecord : RecordBase
	{


		/// <summary>
		/// パラメータの初期値と最大値の予測値を保持します。
		/// </summary>
		[DebuggerDisplay("[{MinimumEstMin}-{MinimumEstMax}]-{Maximum}")]
		public class Parameter
		{

			/// <summary>
			/// 初期値(推測値)
			/// </summary>
			public int Minimum => (MinimumEstMin + MinimumEstMax) / 2;

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
			public bool IsMinimumDefault => MinimumEstMin == MinimumDefault && MinimumEstMax == MaximumDefault;

			/// <summary>
			/// 最大値がデフォルト状態かどうか
			/// </summary>
			public bool IsMaximumDefault => Maximum == MaximumDefault;

			/// <summary>
			/// 有効なデータか
			/// </summary>
			public bool IsAvailable => !IsMinimumDefault && !IsMaximumDefault;

			/// <summary>
			/// 値が特定されているか
			/// </summary>
			public bool IsDetermined => IsAvailable && MinimumEstMin == MinimumEstMax;

			/// <summary>
			/// 最小値の初期値
			/// </summary>
			public static int MinimumDefault => 0;

			/// <summary>
			/// 最大値の初期値
			/// </summary>
			public static int MaximumDefault => 9999;


			public Parameter()
			{
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
			public bool SetEstParameter(int level, int current, int max)
			{

				int clamp(int _value, int _min, int _max) => _value < _min ? _min : (_value > _max ? _max : _value);


				if (max != MaximumDefault)
					Maximum = max;

				if (level == 1)
				{
					MinimumEstMin = MinimumEstMax = current;
				}
				else if (level != 99)
				{
					double emind = (99.0 * (current + 0.0) - max * level) / (99.0 - level);
					double emaxd = (99.0 * (current + 1.0) - max * level) / (99.0 - level);

					int emin = clamp((int)(emind < emaxd ? Math.Ceiling(emind) : Math.Floor(emaxd + 1.0)), 0, max);
					int emax = clamp((int)(emind < emaxd ? Math.Ceiling(emaxd - 1.0) : Math.Floor(emind)), 0, max);

					if (emax < MinimumEstMin || MinimumEstMax < emin)
					{       //明らかに範囲から外れた場合
						MinimumEstMin = emin;
						MinimumEstMax = emax;
						return true;
					}
					else
					{
						MinimumEstMin = Math.Max(MinimumEstMin, emin);
						MinimumEstMax = Math.Min(MinimumEstMax, emax);
					}
				}

				return false;
			}



			// level > 99 のとき、最小値と最大値が反転するため
			public int GetEstParameterMin(int level)
			{
				int min = CalculateParameter(level, MinimumEstMin, Maximum);
				int max = CalculateParameter(level, MinimumEstMax, Maximum);
				return Math.Min(min, max);
			}

			public int GetEstParameterMax(int level)
			{
				int min = CalculateParameter(level, MinimumEstMin, Maximum);
				int max = CalculateParameter(level, MinimumEstMax, Maximum);
				return Math.Max(min, max);
			}

			public int GetParameter(int level)
			{
				return CalculateParameter(level, Minimum, Maximum);
			}

			private int CalculateParameter(int level, int min, int max)
			{
				return min + (int)((max - min) * level / 99.0);
			}
		}




		/// <summary>
		/// 各艦船のパラメータを保持します。
		/// </summary>
		[DebuggerDisplay("[{ShipID}] : {ShipName}")]
		public sealed class ShipParameterElement : RecordElementBase
		{

			/// <summary>
			/// 艦船ID
			/// </summary>
			public int ShipID { get; set; }

			/// <summary>
			/// 艦船名
			/// 可読性向上のために存在します。
			/// </summary>
			public string ShipName => KCDatabase.Instance.MasterShips[ShipID]?.NameWithClass;


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


			/// <summary>
			/// リソースのファイル/フォルダ名
			/// </summary>
			public string ResourceName { get; internal set; }

			/// <summary>
			/// 画像リソースのバージョン
			/// </summary>
			public string ResourceGraphicVersion { get; internal set; }

			/// <summary>
			/// ボイスリソースのバージョン
			/// </summary>
			public string ResourceVoiceVersion { get; internal set; }

			/// <summary>
			/// 母港ボイスリソースのバージョン
			/// </summary>
			public string ResourcePortVoiceVersion { get; internal set; }

			/// <summary>
			/// 衣替え艦：ベースとなる艦船ID (なければ -1)
			/// </summary>
			public int OriginalCostumeShipID { get; internal set; }


			public ShipParameterElement() : base()
			{
				ASW = new Parameter();
				Evasion = new Parameter();
				LOS = new Parameter();

				Aircraft = null;
				DefaultSlot = null;

				MessageGet = null;
				MessageAlbum = null;

				OriginalCostumeShipID = -1;
			}

			public ShipParameterElement(string line) : this()
			{
				LoadLine(line);
			}


			public override void LoadLine(string line)
			{
				string[] elem = line.Split(",".ToCharArray());
				if (elem.Length < 36) throw new ArgumentException("要素数が少なすぎます。");

				ShipID = int.Parse(elem[0]);

				//ShipName=elem[1]は読み飛ばす

				HPMin = int.Parse(elem[2]);
				HPMax = int.Parse(elem[3]);

				FirepowerMin = int.Parse(elem[4]);
				FirepowerMax = int.Parse(elem[5]);

				TorpedoMin = int.Parse(elem[6]);
				TorpedoMax = int.Parse(elem[7]);

				AAMin = int.Parse(elem[8]);
				AAMax = int.Parse(elem[9]);

				ArmorMin = int.Parse(elem[10]);
				ArmorMax = int.Parse(elem[11]);


				ASW.MinimumEstMin = int.Parse(elem[12]);
				ASW.MinimumEstMax = int.Parse(elem[13]);
				ASW.Maximum = int.Parse(elem[14]);

				Evasion.MinimumEstMin = int.Parse(elem[15]);
				Evasion.MinimumEstMax = int.Parse(elem[16]);
				Evasion.Maximum = int.Parse(elem[17]);

				LOS.MinimumEstMin = int.Parse(elem[18]);
				LOS.MinimumEstMax = int.Parse(elem[19]);
				LOS.Maximum = int.Parse(elem[20]);


				LuckMin = int.Parse(elem[21]);
				LuckMax = int.Parse(elem[22]);


				Range = int.Parse(elem[23]);


				if (elem[24].ToLower() == "null")
				{
					DefaultSlot = null;
				}
				else
				{
					DefaultSlot = new int[5];

					for (int i = 0; i < DefaultSlot.Length; i++)
					{
						DefaultSlot[i] = elem[i + 24] == "null" ? -1 : int.Parse(elem[i + 24]);
					}
				}

				if (elem[29].ToLower() == "null")
				{
					Aircraft = null;
				}
				else
				{
					Aircraft = new int[5];

					for (int i = 0; i < Aircraft.Length; i++)
					{
						Aircraft[i] = elem[i + 29] == "null" ? 0 : int.Parse(elem[i + 29]);
					}
				}


				MessageGet = elem[34].ToLower() == "null" ? null : elem[34];
				MessageAlbum = elem[35].ToLower() == "null" ? null : elem[35];


				if (elem.Length >= 41)
				{
					ResourceName = elem[36].ToLower() == "null" ? null : elem[36];
					ResourceGraphicVersion = elem[37].ToLower() == "null" ? null : elem[37];
					ResourceVoiceVersion = elem[38].ToLower() == "null" ? null : elem[38];
					ResourcePortVoiceVersion = elem[39].ToLower() == "null" ? null : elem[39];
					OriginalCostumeShipID = int.Parse(elem[40]);
				}

			}


			public override string SaveLine()
			{
				StringBuilder sb = new StringBuilder();

				sb.Append(string.Join(",",
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
					Range));

				if (DefaultSlot == null)
				{
					sb.Append(",null,null,null,null,null");
				}
				else
				{
					sb.Append(",").Append(string.Join(",", DefaultSlot));
				}

				if (Aircraft == null)
				{
					sb.Append(",null,null,null,null,null");
				}
				else
				{
					sb.Append(",").Append(string.Join(",", Aircraft));

				}

				sb.Append(",").Append(string.Join(",",
					MessageGet ?? "null",
					MessageAlbum ?? "null",
					ResourceName ?? "null",
					ResourceGraphicVersion ?? "null",
					ResourceVoiceVersion ?? "null",
					ResourcePortVoiceVersion ?? "null",
					OriginalCostumeShipID
					));

				return sb.ToString();
			}
		}



		public Dictionary<int, ShipParameterElement> Record { get; private set; }
		private int newShipIDBorder;
		private int remodelingShipID;
		private int questRewardShipID;
		private bool changed;
		public bool ParameterLoadFlag { get; set; }


		public ShipParameterRecord() : base()
		{
			Record = new Dictionary<int, ShipParameterElement>();
			newShipIDBorder = -1;
			remodelingShipID = -1;
			questRewardShipID = -1;
			changed = false;
			ParameterLoadFlag = true;
		}

		public override void RegisterEvents()
		{
			APIObserver ao = APIObserver.Instance;

			ao["api_start2/getData"].ResponseReceived += GameStart;

			ao["api_port/port"].ResponseReceived += ParameterLoaded;

			ao["api_get_member/ship3"].ResponseReceived += EquipmentChanged;

			ao["api_get_member/picture_book"].ResponseReceived += AlbumOpened;

			//戦闘系：最初のフェーズのみ要るから夜戦(≠開幕)は不要
			ao["api_req_sortie/battle"].ResponseReceived += BattleStart;
			ao["api_req_battle_midnight/sp_midnight"].ResponseReceived += BattleStart;
			ao["api_req_sortie/airbattle"].ResponseReceived += BattleStart;
			ao["api_req_sortie/ld_airbattle"].ResponseReceived += BattleStart;
			ao["api_req_sortie/night_to_day"].ResponseReceived += BattleStart;
			ao["api_req_sortie/ld_shooting"].ResponseReceived += BattleStart;
			ao["api_req_combined_battle/battle"].ResponseReceived += BattleStart;
			ao["api_req_combined_battle/sp_midnight"].ResponseReceived += BattleStart;
			ao["api_req_combined_battle/airbattle"].ResponseReceived += BattleStart;
			ao["api_req_combined_battle/battle_water"].ResponseReceived += BattleStart;
			ao["api_req_combined_battle/ld_airbattle"].ResponseReceived += BattleStart;
			ao["api_req_combined_battle/ec_battle"].ResponseReceived += BattleStart;
			ao["api_req_combined_battle/ec_night_to_day"].ResponseReceived += BattleStart;
			ao["api_req_combined_battle/each_battle"].ResponseReceived += BattleStart;
			ao["api_req_combined_battle/each_battle_water"].ResponseReceived += BattleStart;
			ao["api_req_combined_battle/ld_shooting"].ResponseReceived += BattleStart;

			ao["api_req_map/next"].ResponseReceived += SortieNext;

			ao["api_req_map/start"].ResponseReceived += SortieStart;
			ao["api_get_member/slot_item"].ResponseReceived += SortieEnd;

			ao["api_req_kousyou/getship"].ResponseReceived += ConstructionReceived;

			ao["api_req_kaisou/remodeling"].RequestReceived += RemodelingStart;
			ao["api_get_member/slot_item"].ResponseReceived += RemodelingEnd;

			ao["api_req_quest/clearitemget"].ResponseReceived += QuestRewardReceived;
			ao["api_get_member/ship2"].ResponseReceived += QuestRewardReceivedEnd;
		}


		public ShipParameterElement this[int i]
		{
			get
			{
				return Record.ContainsKey(i) ? Record[i] : null;
			}
			set
			{
				if (!Record.ContainsKey(i))
				{
					Record.Add(i, value);
				}
				else
				{
					Record[i] = value;
				}
				changed = true;
			}
		}


		/// <summary>
		/// レコードの要素を更新します。
		/// </summary>
		/// <param name="elem">更新する要素。</param>
		public void Update(ShipParameterElement elem)
		{
			this[elem.ShipID] = elem;
		}


		/// <summary>
		/// パラメータを更新します。
		/// </summary>
		/// <param name="ship">対象の艦船。</param>
		public void UpdateParameter(ShipData ship)
		{
			UpdateParameter(ship.ShipID, ship.Level, ship.ASWBase - ship.ASWModernized, ship.ASWMax, ship.EvasionBase, ship.EvasionMax, ship.LOSBase, ship.LOSMax);
		}


		/// <summary>
		/// パラメータを更新します。
		/// </summary>
		public void UpdateParameter(int shipID, int level, int aswMin, int aswMax, int evasionMin, int evasionMax, int losMin, int losMax)
		{
			ShipParameterElement e = this[shipID];
			if (e == null)
			{
				e = new ShipParameterElement { ShipID = shipID };
			}


			if (e.ASW.SetEstParameter(level, aswMin, aswMax))
				Utility.Logger.Add(1, string.Format("ShipParameter: {0} の対潜値が予測範囲から外れました( [{1} ~ {2}] ~ {3} )。",
					KCDatabase.Instance.MasterShips[e.ShipID].NameWithClass, e.ASW.MinimumEstMin, e.ASW.MinimumEstMax, e.ASW.Maximum));

			if (e.Evasion.SetEstParameter(level, evasionMin, evasionMax))
				Utility.Logger.Add(1, string.Format("ShipParameter: {0} の回避値が予測範囲から外れました( [{1} ~ {2}] ~ {3} )。",
					KCDatabase.Instance.MasterShips[e.ShipID].NameWithClass, e.Evasion.MinimumEstMin, e.Evasion.MinimumEstMax, e.Evasion.Maximum));

			if (e.LOS.SetEstParameter(level, losMin, losMax))
				Utility.Logger.Add(1, string.Format("ShipParameter: {0} の索敵値が予測範囲から外れました( [{1} ~ {2}] ~ {3} )。",
					KCDatabase.Instance.MasterShips[e.ShipID].NameWithClass, e.LOS.MinimumEstMin, e.LOS.MinimumEstMax, e.LOS.Maximum));


			Update(e);
		}


		/// <summary>
		/// パラメータを更新します。（対潜/回避/索敵最大値のみ）
		/// </summary>
		public void UpdateMaxParameter(ShipData ship)
		{
			ShipParameterElement e = this[ship.ShipID];
			if (e == null)
			{
				e = new ShipParameterElement { ShipID = ship.ShipID };
			}


			e.ASW.Maximum = ship.ASWMax;
			e.Evasion.Maximum = ship.EvasionMax;
			e.LOS.Maximum = ship.LOSMax;


			Update(e);
		}


		/// <summary>
		/// 初期装備を更新します。
		/// </summary>
		/// <param name="ship">対象の艦船。入手直後・改装直後のものに限ります。</param>
		public void UpdateDefaultSlot(ShipData ship)
		{
			UpdateDefaultSlot(ship.ShipID, ship.SlotMaster.ToArray());
		}

		/// <summary>
		/// 初期装備を更新します。
		/// </summary>
		/// <param name="shipID">艦船ID。</param>
		/// <param name="slot">装備スロット配列。</param>
		public void UpdateDefaultSlot(int shipID, int[] slot)
		{
			ShipParameterElement e = this[shipID];
			if (e == null)
			{
				e = new ShipParameterElement { ShipID = shipID };
			}

			if (e.DefaultSlot == null || !e.DefaultSlot.SequenceEqual(slot))
				Utility.Logger.Add(2, KCDatabase.Instance.MasterShips[shipID].NameWithClass + " の初期装備が更新されました。");

			e.DefaultSlot = slot;

			Update(e);
		}




		#region API Events

		/// <summary>
		/// ゲーム開始時にパラメータ読み込みフラグを解除、初回読み込みの準備をします。
		/// </summary>
		private void GameStart(string apiname, dynamic data)
		{
			ParameterLoadFlag = true;


			foreach (var elem in data.api_mst_ship)
			{
				var param = this[(int)elem.api_id];
				if (param == null)
				{
					param = new ShipParameterElement { ShipID = (int)elem.api_id };
				}

				if (elem.api_taik())
				{
					param.HPMin = (int)elem.api_taik[0];
					param.HPMax = (int)elem.api_taik[1];
				}
				if (elem.api_houg())
				{
					param.FirepowerMin = (int)elem.api_houg[0];
					param.FirepowerMax = (int)elem.api_houg[1];
				}
				if (elem.api_raig())
				{
					param.TorpedoMin = (int)elem.api_raig[0];
					param.TorpedoMax = (int)elem.api_raig[1];
				}
				if (elem.api_tyku())
				{
					param.AAMin = (int)elem.api_tyku[0];
					param.AAMax = (int)elem.api_tyku[1];
				}
				if (elem.api_souk())
				{
					param.ArmorMin = (int)elem.api_souk[0];
					param.ArmorMax = (int)elem.api_souk[1];
				}
				if (elem.api_tais())
				{
					int[] api_tais = elem.api_tais;     // Length = 1 の場合がある
					param.ASW.SetEstParameter(1, api_tais[0], api_tais.Length >= 2 ? api_tais[1] : Parameter.MaximumDefault);
				}
				if (elem.api_luck())
				{
					param.LuckMin = (int)elem.api_luck[0];
					param.LuckMax = (int)elem.api_luck[1];
				}

				if (elem.api_leng())
				{
					param.Range = (int)elem.api_leng;
				}
				if (elem.api_maxeq())
				{
					param.Aircraft = (int[])elem.api_maxeq;
				}
				if (elem.api_getmes())
				{
					string mes = elem.api_getmes;
					if (!string.IsNullOrWhiteSpace(mes.Replace("<br>", "\r\n")))
						param.MessageGet = mes;
				}

				Update(param);
			}

			foreach (var elem in data.api_mst_shipgraph)
			{
				var param = this[(int)elem.api_id];
				if (param == null)
				{
					param = new ShipParameterElement { ShipID = (int)elem.api_id };
				}

				if (elem.api_filename())
				{
					param.ResourceName = elem.api_filename;
				}
				if (elem.api_version())
				{
					var values = (string[])elem.api_version;
					param.ResourceGraphicVersion = values[0];
					param.ResourceVoiceVersion = values[1];
					param.ResourcePortVoiceVersion = values[2];
				}

				Update(param);
			}

			// validation
			foreach (var record in Record.Values)
			{

				// 無効な装備を持っていた場合、初期装備データを初期化
				if (record.DefaultSlot != null)
				{
					if (record.DefaultSlot.Any(id => id != -1 && !KCDatabase.Instance.MasterEquipments.ContainsKey(id)))
						record.DefaultSlot = null;
				}
			}

		}


		/// <summary>
		/// 保有艦船から各パラメータを読み込みます。
		/// </summary>
		void ParameterLoaded(string apiname, dynamic data)
		{
			// note: 暫定的に毎回ロードしてみる
			//if (!ParameterLoadFlag) return;


			foreach (ShipData ship in KCDatabase.Instance.Ships.Values)
			{
				// 非武装時のみ詳細にステータスを更新
				if (ship.AllSlot.All(id => id <= 0))
					UpdateParameter(ship);
				else
					UpdateMaxParameter(ship);
			}

			//ParameterLoadFlag = false;      //一回限り(基本的に起動直後の1回)

		}


		private void EquipmentChanged(string apiname, dynamic data)
		{
			foreach (var rawship in data.api_ship_data)
			{
				var ship = KCDatabase.Instance.Ships[(int)rawship.api_id];

				// 非武装時のみ詳細にステータスを更新
				if (ship.AllSlot.All(id => id <= 0))
					UpdateParameter(ship);
				else
					UpdateMaxParameter(ship);
			}
		}


		/// <summary>
		/// 艦娘図鑑から回避・対潜の初期値及び説明文を読み込みます。
		/// </summary>
		private void AlbumOpened(string apiname, dynamic data)
		{

			if (data == null || !data.api_list())       //空のページ
				return;

			foreach (dynamic elem in data.api_list)
			{

				if (!elem.api_yomi()) break;        //装備図鑑だった場合終了


				int shipID = (int)elem.api_table_id[0];
				var ship = KCDatabase.Instance.MasterShips[shipID];

				var e = this[shipID];
				if (e == null)
				{
					e = new ShipParameterElement { ShipID = shipID };
				}


				if (e.ASW.SetEstParameter(1, (int)elem.api_tais, Parameter.MaximumDefault))
					Utility.Logger.Add(1, string.Format("ShipParameter: {0} の対潜値が予測範囲から外れました( [{1} ~ {2}] ~ {3} )。",
						ship.NameWithClass, e.ASW.MinimumEstMin, e.ASW.MinimumEstMax, e.ASW.Maximum));

				if (e.Evasion.SetEstParameter(1, (int)elem.api_kaih, Parameter.MaximumDefault))
					Utility.Logger.Add(1, string.Format("ShipParameter: {0} の回避値が予測範囲から外れました( [{1} ~ {2}] ~ {3} )。",
						ship.NameWithClass, e.Evasion.MinimumEstMin, e.Evasion.MinimumEstMax, e.Evasion.Maximum));


				{   //図鑑説明文登録(図鑑に載っていない改装艦に関してはその改装前の艦の説明文を設定する)
					e.MessageAlbum = elem.api_sinfo;
					LinkedList<int> processedIDs = new LinkedList<int>();

					while (ship != null && !processedIDs.Contains(ship.ShipID) && ship.RemodelAfterShipID > 0)
					{
						processedIDs.AddLast(ship.ID);
						var e2 = this[ship.RemodelAfterShipID];
						if (e2 == null)
						{
							e2 = new ShipParameterElement { ShipID = ship.RemodelAfterShipID };
						}

						ship = KCDatabase.Instance.MasterShips[ship.RemodelAfterShipID];
						if (ship != null && ship.IsListedInAlbum)
							break;

						e2.MessageAlbum = e.MessageAlbum;
						Update(e2);

					}
				}

				{
					var costumeIDs = (int[])elem.api_table_id;
					foreach (var id in costumeIDs)
					{
						if (id == shipID)
							continue;

						var e2 = this[id];
						if (e2 == null)
						{
							e2 = new ShipParameterElement { ShipID = id };
						}

						e2.OriginalCostumeShipID = shipID;
						Update(e2);
					}
				}


				Update(e);
			}
		}

		/// <summary>
		/// 戦闘開始時の情報から敵艦の情報を読み込みます。
		/// </summary>
		private void BattleStart(string apiname, dynamic data)
		{

			var battle = KCDatabase.Instance.Battle.FirstBattle;
			var binit = battle.Initial;

			void UpdateParams(int id, int maxhp, int[] status, int[] slot)
			{
				var param = this[id];
				if (param == null)
				{
					param = new ShipParameterElement { ShipID = id };
				}

				param.HPMin = param.HPMax = maxhp;

				if (status != null)
				{
					param.FirepowerMin = param.FirepowerMax = status[0];
					param.TorpedoMin = param.TorpedoMax = status[1];
					param.AAMin = param.AAMax = status[2];
					param.ArmorMin = param.ArmorMax = status[3];
				}

				param.DefaultSlot = slot;

				Update(param);
			}


			for (int i = 0; i < binit.EnemyMembers.Length; i++)
			{
				int id = binit.EnemyMembers[i];
				if (id <= 0)
					continue;

				UpdateParams(id, binit.EnemyMaxHPs[i], binit.EnemyParameters?[i], binit.EnemySlots[i]);
			}

			if (battle.IsEnemyCombined)
			{
				for (int i = 0; i < binit.EnemyMembersEscort.Length; i++)
				{
					int id = binit.EnemyMembersEscort[i];
					if (id <= 0)
						continue;

					UpdateParams(id, binit.EnemyMaxHPsEscort[i], binit.EnemyParametersEscort?[i], binit.EnemySlotsEscort[i]);
				}
			}



			void EstimateUnknownAircraftSlot()
			{
				var db = KCDatabase.Instance;

				// 航空戦が存在するときだけ処理する
				if (!(battle is BattleDay battleDay))
					return;



				IEnumerable<int> members = binit.EnemyMembers;
				if (battleDay.IsEnemyCombined)
					members = members.Concat(binit.EnemyMembersEscort);


				void checkNoAircraft(int id, int[] eqs)
				{
					if (id <= 0 ||
						this[id].Aircraft != null)
						return;

					if (eqs.Select(eqid => db.MasterEquipments[eqid]).All(eq => !eq?.IsAircraft ?? true))
					{
						Utility.Logger.Add(1, $"搭載チェック：ID{id} {db.MasterShips[id].NameWithClass} は航空機を装備していません。搭載数 0 と推定します。");
						this[id].Aircraft = new int[5];
					}
				}

				for (int i = 0; i < binit.EnemyMembers.Length; i++)
					checkNoAircraft(binit.EnemyMembers[i], binit.EnemySlots[i]);

				if (battleDay.IsEnemyCombined)
				{
					for (int i = 0; i < binit.EnemyMembersEscort.Length; i++)
						checkNoAircraft(binit.EnemyMembersEscort[i], binit.EnemySlotsEscort[i]);
				}



				// 搭載不明艦が 1 種類 && 航空機搭載スロットが 1 つだけの場合、搭載数推定を行う

				var unknowns = members.Where(id => this[id] != null && this[id].Aircraft == null);

				if (unknowns.Distinct().Count() == 1)
				{
					int unknownShipID = unknowns.First();

					int[] unknownShipEquipment;

					{
						int unknownShipIndex = Array.IndexOf(binit.EnemyMembers, unknownShipID);
						if (unknownShipIndex != -1)
						{
							unknownShipEquipment = binit.EnemySlots[unknownShipIndex];
						}
						else
						{
							unknownShipIndex = Array.IndexOf(binit.EnemyMembersEscort, unknownShipID);
							unknownShipEquipment = binit.EnemySlotsEscort[unknownShipIndex];
						}
					}


					void estimate(int actualAircraftCount, Func<EquipmentDataMaster, bool> isAircraft)
					{
						if (unknownShipEquipment.Select(id => db.MasterEquipments[id]).Count(isAircraft) != 1)
							return;


						int getAircraftCount(int shipID, int[] equipmentIDs, int[] aircraft)
						{
							int aircount = 0;
							if (shipID <= 0 || aircraft == null)
								return 0;

							for (int i = 0; i < Math.Min(equipmentIDs.Length, aircraft.Length); i++)
							{
								if (isAircraft(db.MasterEquipments[equipmentIDs[i]]))
									aircount += aircraft[i];
							}
							return aircount;
						}

						int knownAircraftCount = 0;
						for (int i = 0; i < binit.EnemyMembers.Length; i++)
							knownAircraftCount += getAircraftCount(binit.EnemyMembers[i], binit.EnemySlots[i], this[binit.EnemyMembers[i]]?.Aircraft);

						if (battleDay.IsEnemyCombined)
						{
							for (int i = 0; i < binit.EnemyMembersEscort.Length; i++)
								knownAircraftCount += getAircraftCount(binit.EnemyMembersEscort[i], binit.EnemySlotsEscort[i], this[binit.EnemyMembersEscort[i]]?.Aircraft);
						}

						int estimatedAircraftCount = (actualAircraftCount - knownAircraftCount) / unknowns.Count();

						{
							var p = this[unknownShipID];
							var aircraft = new int[5];
							aircraft[Array.FindIndex(unknownShipEquipment, id => isAircraft(db.MasterEquipments[id]))] = estimatedAircraftCount;
							p.Aircraft = aircraft;
							Update(p);
							Utility.Logger.Add(1, $"搭載チェック：ID{unknownShipID} {db.MasterShips[unknownShipID].NameWithClass} の搭載数を [{string.Join(", ", aircraft)}] と推定しました。");
						}
					}


					if (battleDay.BaseAirAttack?.IsAvailable ?? false)
					{
						var firstUnit = battleDay.BaseAirAttack.AirAttackUnits.FirstOrDefault();
						if (!firstUnit?.IsStage1Available ?? false)
							return;

						estimate(firstUnit.AircraftTotalStage1Enemy, eq => eq?.IsAircraft ?? false);
					}
					else if (battleDay.AirBattle?.IsStage1Available ?? false)
					{
						estimate(battleDay.AirBattle.AircraftTotalStage1Enemy, eq => eq?.IsCombatAircraft ?? false);
					}
					else if (battleDay is BattleBaseAirRaid battleAirRaid && (battleAirRaid.BaseAirRaid?.IsStage1Available ?? false))
					{
						estimate(battleAirRaid.BaseAirRaid.AircraftTotalStage1Enemy, eq => eq?.IsCombatAircraft ?? false);
					}

				}
			}
			EstimateUnknownAircraftSlot();

		}


		private void SortieNext(string apiname, dynamic data)
		{
			if (KCDatabase.Instance.Battle.Compass.HasAirRaid)
				BattleStart(apiname, data);
		}


		/// <summary>
		/// 出撃開始時の最新艦を記録します。SortieEndで使用されます。
		/// </summary>
		private void SortieStart(string apiname, dynamic data)
		{
			newShipIDBorder = KCDatabase.Instance.Ships.Keys.Max();


			if (KCDatabase.Instance.Battle.Compass.HasAirRaid)
				BattleStart(apiname, data);
		}


		/// <summary>
		/// 艦隊帰投時に新規入手艦を取得、情報を登録します。
		/// </summary>
		private void SortieEnd(string apiname, dynamic data)
		{

			if (newShipIDBorder == -1) return;

			foreach (ShipData s in KCDatabase.Instance.Ships.Values.Where(s => s.MasterID > newShipIDBorder))
			{
				// note: 初期装備がシナジー装備であることがあるので、記録しない　要再確認
				//UpdateParameter(s);
				UpdateDefaultSlot(s);

			}

			newShipIDBorder = -1;
		}


		/// <summary>
		/// 艦船建造時に新規入手艦を取得、情報を登録します。
		/// </summary>
		private void ConstructionReceived(string apiname, dynamic data)
		{

			int shipID = (int)data.api_id;
			ShipData ship = KCDatabase.Instance.Ships.Values.FirstOrDefault(s => s.MasterID == shipID);

			if (ship != null)
			{
				// note: 初期装備がシナジー装備であることがあるので、記録しない　要再確認
				//UpdateParameter(ship);
				UpdateDefaultSlot(ship);
			}

		}


		/// <summary>
		/// 改装艦のIDを記録します。RemodelingEndで使用されます。
		/// </summary>
		void RemodelingStart(string apiname, dynamic data)
		{

			remodelingShipID = int.Parse(data["api_id"]);

		}

		/// <summary>
		/// 改装艦のパラメータと装備を記録します。
		/// </summary>
		void RemodelingEnd(string apiname, dynamic data)
		{

			if (remodelingShipID == -1)
				return;

			ShipData ship = KCDatabase.Instance.Ships[remodelingShipID];

			if (ship != null)
			{
				// note: 初期装備がシナジー装備であることがあるので、記録しない　要再確認
				//UpdateParameter(ship);
				UpdateDefaultSlot(ship);
			}

			remodelingShipID = -1;
		}



		void QuestRewardReceived(string apiname, dynamic data)
		{
			questRewardShipID = -1;

			if (data.api_bounus())
			{
				foreach (var b in data.api_bounus)
				{
					if ((int)b.api_type == 11 && b.api_item() && b.api_item != null)
					{
						if (b.api_item.api_ship_id())
						{
							questRewardShipID = (int)b.api_item.api_ship_id;

						}
					}
				}
			}
		}

		void QuestRewardReceivedEnd(string apiname, dynamic data)
		{
			if (questRewardShipID == -1)
				return;

			var ship = KCDatabase.Instance.Ships.OrderBy(p => p.Key).Last().Value;

			if (ship.ShipID == questRewardShipID)
			{
				// 装備データがまだ送られてきていないので、remodeling と同様 slot_item が来た時点で処理する
				remodelingShipID = ship.MasterID;
			}
		}


		#endregion



		protected override void LoadLine(string line)
		{
			Update(new ShipParameterElement(line));
		}

		protected override string SaveLinesAll()
		{
			var sb = new StringBuilder();
			foreach (var elem in Record.Values.OrderBy(r => r.ShipID))
			{
				sb.AppendLine(elem.SaveLine());
			}
			return sb.ToString();
		}

		protected override string SaveLinesPartial()
		{
			throw new NotSupportedException();
		}

		protected override void UpdateLastSavedIndex()
		{
			changed = false;
		}

		public override bool NeedToSave => changed;

		public override bool SupportsPartialSave => false;


		protected override void ClearRecord()
		{
			Record.Clear();
		}


		public override string RecordHeader => "艦船ID,艦船名,耐久初期,耐久最大,火力初期,火力最大,雷装初期,雷装最大,対空初期,対空最大,装甲初期,装甲最大,対潜初期下限,対潜初期上限,対潜最大,回避初期下限,回避初期上限,回避最大,索敵初期下限,索敵初期上限,索敵最大,運初期,運最大,射程,装備1,装備2,装備3,装備4,装備5,機数1,機数2,機数3,機数4,機数5,ドロップ説明,図鑑説明,リソース名,画像ver,ボイスver,母港ボイスver,元衣装ID";

		public override string FileName => "ShipParameterRecord.csv";
	}

}
