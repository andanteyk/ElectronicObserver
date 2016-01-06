using ElectronicObserver.Window.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EquipmentsDetail
{
	public class Plugin : DialogPlugin
	{

		public override string MenuTitle
		{
			get { return "装备一览改"; }
		}

		public override string Version
		{
			get { return "1.0.0.1"; }
		}

		public override Form GetToolWindow()
		{
            return new DialogEquipmentList();
		}
	}
}
