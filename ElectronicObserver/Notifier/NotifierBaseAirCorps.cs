using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Notifier
{
	/// <summary>
	/// 基地航空隊出撃通知を扱います。
	/// </summary>
	public class NotifierBaseAirCorps : NotifierBase
	{
		private readonly static TimeSpan RelocationSpan = TimeSpan.FromMinutes(12);

		/// <summary>
		/// 未補給時に通知する
		/// </summary>
		public bool NotifiesNotSupplied { get; set; }

		/// <summary>
		/// 疲労時に通知する
		/// </summary>
		public bool NotifiesTired { get; set; }

		/// <summary>
		/// 編成されていないときに通知する
		/// </summary>
		public bool NotifiesNotOrganized { get; set; }


		/// <summary>
		/// 待機のとき通知する
		/// </summary>
		public bool NotifiesStandby { get; set; }

		/// <summary>
		/// 退避の時通知する
		/// </summary>
		public bool NotifiesRetreat { get; set; }

		/// <summary>
		/// 休息の時通知する
		/// </summary>
		public bool NotifiesRest { get; set; }


		/// <summary>
		/// 通常海域で通知する
		/// </summary>
		public bool NotifiesNormalMap { get; set; }

		/// <summary>
		/// イベント海域で通知する
		/// </summary>
		public bool NotifiesEventMap { get; set; }


		/// <summary>
		/// 基地枠の配置転換完了時に通知する
		/// </summary>
		public bool NotifiesSquadronRelocation { get; set; }

		/// <summary>
		/// 装備の配置転換完了時に通知する
		/// </summary>
		public bool NotifiesEquipmentRelocation { get; set; }


		// supress when sortieing

		private bool _isAlreadyNotified = false;
		private bool _isInSortie = false;
		private HashSet<int> _notifiedEquipments = new HashSet<int>();



		public NotifierBaseAirCorps()
			: base()
		{
			Initalize();
		}

		public NotifierBaseAirCorps(Utility.Configuration.ConfigurationData.ConfigNotifierBaseAirCorps config)
			: base(config)
		{
			Initalize();

			NotifiesNotSupplied = config.NotifiesNotSupplied;
			NotifiesTired = config.NotifiesTired;
			NotifiesNotOrganized = config.NotifiesNotOrganized;

			NotifiesStandby = config.NotifiesStandby;
			NotifiesRetreat = config.NotifiesRetreat;
			NotifiesRest = config.NotifiesRest;

			NotifiesNormalMap = config.NotifiesNormalMap;
			NotifiesEventMap = config.NotifiesEventMap;

			NotifiesSquadronRelocation = config.NotifiesSquadronRelocation;
			NotifiesEquipmentRelocation = config.NotifiesEquipmentRelocation;
		}

		private void Initalize()
		{
			DialogData.Title = "基地航空隊報告";

			var o = APIObserver.Instance;

			o["api_port/port"].ResponseReceived += Port;
			o["api_get_member/mapinfo"].ResponseReceived += BeforeSortie;
			o["api_get_member/sortie_conditions"].ResponseReceived += BeforeSortieEventMap;
			o["api_req_map/start"].RequestReceived += Sally;
		}


		private void Port(string apiname, dynamic data)
		{
			_isAlreadyNotified = false;
			_isInSortie = false;
			_notifiedEquipments.Clear();
		}

		private void BeforeSortieEventMap(string apiname, dynamic data)
		{
			if (_isAlreadyNotified)
				return;

			if (!NotifiesEventMap)
				return;

			var db = KCDatabase.Instance;
			CheckBaseAirCorps(db.BaseAirCorps.Values.Where(corps => db.MapArea[corps.MapAreaID].MapType == 1));
		}

		private void BeforeSortie(string apiname, dynamic data)
		{
			if (_isAlreadyNotified)
				return;

			if (!NotifiesNormalMap)
				return;

			var db = KCDatabase.Instance;
			CheckBaseAirCorps(db.BaseAirCorps.Values.Where(corps => db.MapArea[corps.MapAreaID].MapType == 0));
		}


		private bool CheckBaseAirCorps(IEnumerable<BaseAirCorpsData> corpslist)
		{
			var db = KCDatabase.Instance;
			var sb = new StringBuilder();
			var messages = new LinkedList<string>();

			foreach (var corps in corpslist)
			{
				if (NotifiesNotSupplied && corps.Squadrons.Values.Any(sq => sq.State == 1 && sq.AircraftCurrent < sq.AircraftMax))
					messages.AddLast("未補給");
				if (NotifiesTired && corps.Squadrons.Values.Any(sq => sq.State == 1 && sq.Condition > 1))
					messages.AddLast("疲労");
				if (NotifiesNotOrganized)
				{
					if (corps.Squadrons.Values.Any(sq => sq.State == 0))
						messages.AddLast("未編成");
					if (corps.Squadrons.Values.Any(sq => sq.State == 2))
						messages.AddLast("配置転換中");
				}

				if (NotifiesStandby && corps.ActionKind == 0)
					messages.AddLast("待機中");
				if (NotifiesRetreat && corps.ActionKind == 3)
					messages.AddLast("退避中");
				if (NotifiesRest && corps.ActionKind == 4)
					messages.AddLast("休息中");

				if (messages.Any())
				{
					if (sb.Length == 0)
						sb.Append("出撃準備ができていません：");
					sb.Append($"#{corps.MapAreaID} {corps.Name} ({string.Join(", ", messages)})");
				}
				messages.Clear();
			}

			if (sb.Length > 0)
			{
				Notify(sb.ToString());
				_isAlreadyNotified = true;
				return true;
			}
			return false;
		}

		private void Sally(string apiname, dynamic data)
		{
			_isInSortie = true;
		}


		protected override void UpdateTimerTick()
		{
			var db = KCDatabase.Instance;

			if (!db.RelocatedEquipments.Any())
				return;

			if (_isInSortie)
				return;

			if (NotifiesSquadronRelocation)
			{

				StringBuilder sb = null;
				foreach (var corps in db.BaseAirCorps.Values.Where(corps =>
						 (NotifiesNormalMap && db.MapArea[corps.MapAreaID].MapType == 0) ||
						 (NotifiesEventMap && db.MapArea[corps.MapAreaID].MapType == 1)))
				{
					var targetSquadrons = corps.Squadrons.Values.Where(sq => sq.State == 2 && !_notifiedEquipments.Contains(sq.EquipmentID) && (DateTime.Now - sq.RelocatedTime) >= RelocationSpan);

					if (targetSquadrons.Any())
					{
						sb = sb?.Append(", ") ?? new StringBuilder();

						sb.Append(string.Join(", ", targetSquadrons.Select(sq =>
							$"#{corps.MapAreaID} {corps.Name} 第{sq.SquadronID}中隊 ({sq.EquipmentInstance.NameWithLevel})")));

						foreach (var sq in targetSquadrons)
							_notifiedEquipments.Add(sq.EquipmentID);
					}
				}

				if (sb != null)
				{
					Notify(sb.ToString() + "の配置転換が完了しました。母港に入り直すと更新されます。");
				}
			}

			if (NotifiesEquipmentRelocation)
			{
				var targets = db.RelocatedEquipments.Where(kv => !_notifiedEquipments.Contains(kv.Key) &&
					(DateTime.Now - kv.Value.RelocatedTime) >= RelocationSpan);

				if (targets.Any())
				{
					Notify($"{string.Join(", ", targets.Select(kv => kv.Value.EquipmentInstance.NameWithLevel))} の配置転換が完了しました。母港に入り直すと更新されます。");

					foreach (var t in targets)
						_notifiedEquipments.Add(t.Key);
				}
			}


		}

		private void Notify(string message)
		{
			DialogData.Title = "基地航空隊報告";
			DialogData.Message = message;

			base.Notify();
		}

		public override void ApplyToConfiguration(Utility.Configuration.ConfigurationData.ConfigNotifierBase config)
		{
			base.ApplyToConfiguration(config);

			var c = config as Utility.Configuration.ConfigurationData.ConfigNotifierBaseAirCorps;

			if (c != null)
			{
				c.NotifiesNotSupplied = NotifiesNotSupplied;
				c.NotifiesTired = NotifiesTired;
				c.NotifiesNotOrganized = NotifiesNotOrganized;

				c.NotifiesStandby = NotifiesStandby;
				c.NotifiesRetreat = NotifiesRetreat;
				c.NotifiesRest = NotifiesRest;

				c.NotifiesNormalMap = NotifiesNormalMap;
				c.NotifiesEventMap = NotifiesEventMap;

				c.NotifiesSquadronRelocation = NotifiesSquadronRelocation;
				c.NotifiesEquipmentRelocation = NotifiesEquipmentRelocation;
			}
		}
	}
}
