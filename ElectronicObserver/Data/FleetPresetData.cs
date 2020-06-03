using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data
{
	public class FleetPresetData : APIWrapper, IIdentifiable
	{
		public int PresetID => (int)RawData.api_preset_no;

		public string Name => RawData.api_name;

		private int[] _members;
		public ReadOnlyCollection<int> Members => Array.AsReadOnly(_members);

		public IEnumerable<ShipData> MembersInstance => Members.Select(id => KCDatabase.Instance.Ships[id]);


		public override void LoadFromResponse(string apiname, dynamic data)
		{
			_members = (int[])data.api_ship;
			base.LoadFromResponse(apiname, (object)data);
		}


		public int ID => PresetID;
	}
}
