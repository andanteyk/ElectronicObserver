﻿using CustomShipGroup.Model;
using ElectronicObserver.Data;
using ElectronicObserver.Window;
using ElectronicObserver.Window.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomShipGroup
{
	public class Plugin : DockPlugin
	{
		static CustomShipGroupManager _ShipGroupManager;
		public static CustomShipGroupManager ShipGroupManager
		{
			get
			{
				if ( _ShipGroupManager == null )
				{
					_ShipGroupManager = new CustomShipGroupManager();
					var temp = (CustomShipGroupManager)_ShipGroupManager.Load();
					if ( temp != null )
						_ShipGroupManager = temp;
				}
				return _ShipGroupManager;
			}
		}

		public override string MenuTitle
		{
			get { return "自定义舰船编成(&S)"; }
		}

		public override string Version
		{
			get { return "1.0.0.0"; }
		}
	}

	public class PluginServer : ServerPlugin
	{
		public override string MenuTitle
		{
			get { return "舰船编成服务"; }
		}

		public override bool RunService( FormMain main )
		{
			main.FormClosing += ( sender, e ) => Plugin.ShipGroupManager.Save();

			return true;
		}
	}
}
