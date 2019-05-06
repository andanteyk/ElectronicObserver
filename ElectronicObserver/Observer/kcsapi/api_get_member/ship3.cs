using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_get_member
{

	public class ship3 : APIBase
	{

		public override void OnResponseReceived(dynamic data)
		{

			KCDatabase db = KCDatabase.Instance;

			//api_ship_data
			foreach (var elem in data.api_ship_data)
			{

				int id = (int)elem.api_id;

				ShipData ship = db.Ships[id];
				ship.LoadFromResponse(APIName, elem);

				for (int i = 0; i < ship.Slot.Count; i++)
				{
					if (ship.Slot[i] == -1) continue;
					if (!db.Equipments.ContainsKey(ship.Slot[i]))
					{       //改装時に新装備を入手するが、追加される前にIDを与えられてしまうため
						EquipmentData eq = new EquipmentData();
						eq.LoadFromResponse(APIName, ship.Slot[i]);
						db.Equipments.Add(eq);
					}
				}


				// 装備シナジー検出カッコカリ
				if (ship.MasterShip.ASW.IsDetermined &&
					ship.MasterShip.Evasion.IsDetermined &&
					ship.MasterShip.LOS.IsDetermined)
				{
					int firepower = ship.FirepowerTotal - ship.FirepowerBase;
					int torpedo = ship.TorpedoTotal - ship.TorpedoBase;
					int aa = ship.AATotal - ship.AABase;
					int armor = ship.ArmorTotal - ship.ArmorBase;
					int asw = ship.ASWTotal - (ship.MasterShip.ASW.GetEstParameterMin(ship.Level) + ship.ASWModernized);
					int evasion = ship.EvasionTotal - ship.MasterShip.Evasion.GetEstParameterMin(ship.Level);
					int los = ship.LOSTotal - ship.MasterShip.LOS.GetEstParameterMin(ship.Level);
					int luck = ship.LuckTotal - ship.LuckBase;
					int range = ship.MasterShip.Range;

					foreach (var eq in ship.AllSlotInstanceMaster.Where(eq => eq != null))
					{
						firepower -= eq.Firepower;
						torpedo -= eq.Torpedo;
						aa -= eq.AA;
						armor -= eq.Armor;
						asw -= eq.ASW;
						evasion -= eq.Evasion;
						los -= eq.LOS;
						luck -= eq.Luck;
						range = Math.Max(range, eq.Range);
					}

					range = ship.Range - range;

					if (firepower != 0 ||
						torpedo != 0 ||
						aa != 0 ||
						armor != 0 ||
						asw != 0 ||
						evasion != 0 ||
						los != 0 ||
						luck != 0 ||
						range != 0)
					{
						var sb = new StringBuilder();
						sb.Append("装備シナジーを検出しました：");
							
						var a = new List<string>();
						if (firepower != 0)
							a.Add($"火力{firepower:+#;-#;0}");
						if (torpedo != 0)
							a.Add($"雷装{torpedo:+#;-#;0}");
						if (aa != 0)
							a.Add($"対空{aa:+#;-#;0}");
						if (armor != 0)
							a.Add($"装甲{armor:+#;-#;0}");
						if (asw != 0)
							a.Add($"対潜{asw:+#;-#;0}");
						if (evasion != 0)
							a.Add($"回避{evasion:+#;-#;0}");
						if (los != 0)
							a.Add($"索敵{los:+#;-#;0}");
						if (luck != 0)
							a.Add($"運{luck:+#;-#;0}");
						if (range != 0)
							a.Add($"射程{range:+#;-#;0}");

						sb.Append(string.Join(", ", a));

						sb.AppendFormat(" ; {0} [{1}]", 
							ship.NameWithLevel,
							string.Join(", ", ship.AllSlotInstance.Where(eq => eq != null).Select(eq => eq.NameWithLevel)));

						Utility.Logger.Add(2, sb.ToString());
					}
				}
			}

			//api_deck_data
			db.Fleet.LoadFromResponse(APIName, data.api_deck_data);



			base.OnResponseReceived((object)data);
		}

		public override string APIName => "api_get_member/ship3";
	}


}
