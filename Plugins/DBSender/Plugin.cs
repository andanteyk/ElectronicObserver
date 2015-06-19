using ElectronicObserver.Observer;
using ElectronicObserver.Utility;
using ElectronicObserver.Window;
using ElectronicObserver.Window.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBSender
{
	public class Plugin : ServerPlugin
	{
		private APIKancolleDB DBSender;

		public override string MenuTitle
		{
			get { return "数据发送插件"; }
		}

		public override PluginSettingControl GetSettings()
		{
			return new Settings();
		}

		public override bool RunService( FormMain main )
		{

			DBSender = new APIKancolleDB();

			Fiddler.FiddlerApplication.AfterSessionComplete += FiddlerApplication_AfterSessionComplete;

			return true;
		}

		void FiddlerApplication_AfterSessionComplete( Fiddler.Session oSession )
		{
			if ( oSession.fullUrl.Contains( "/kcsapi/" ) && oSession.oResponse.MIMEType == "text/plain" )
			{

				// kancolle-db.netに送信する
				if ( Configuration.Config.Connection.SendDataToKancolleDB )
				{
					Task.Factory.StartNew( (Action)( () => DBSender.ExecuteSession( oSession ) ) );
				}

			}
		}
	}
}
