﻿using ElectronicObserver.Window.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overview
{
	public class Plugin : DockPlugin
	{
		public override string MenuTitle
		{
			get { return "日志(&L)"; }
		}

		public override string Version
		{
			get { return "1.0.0"; }
		}
	}
}
