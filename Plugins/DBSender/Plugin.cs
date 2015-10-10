using Codeplex.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Utility;
using ElectronicObserver.Window;
using ElectronicObserver.Window.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBSender
{
	public class Plugin : ServerPlugin
	{
		private const string PLUGIN_SETTINGS = @"Settings\PluginSettings.json";
		private const string DEFAULT_SETTINGS = @"{""DBSender"":{""send_with_proxy"":true},""Poi"":{""enable"":true}}";

		private APIKancolleDB DBSender;
		private APIPoiSender PoiSender;
		public dynamic Settings;

		public override string MenuTitle
		{
			get { return "数据发送插件"; }
		}

		public override string Version
		{
			get { return "2.0.0.1"; }
		}

		public override PluginSettingControl GetSettings()
		{
			return new Settings( this );
		}

		public void SaveSettings()
		{
			if ( Settings == null )
			{
				Settings = DynamicJson.Parse( DEFAULT_SETTINGS );
			}
			if ( !Directory.Exists( "Settings" ) )
			{
				Directory.CreateDirectory( "Settings" );
			}
			File.WriteAllText( PLUGIN_SETTINGS, Settings.ToString() );
		}

		public override bool RunService( FormMain main )
		{
			try
			{
				if ( File.Exists( PLUGIN_SETTINGS ) )
					Settings = DynamicJson.Parse( File.ReadAllText( PLUGIN_SETTINGS ) );
				else
					Settings = DynamicJson.Parse( DEFAULT_SETTINGS );
			}
			catch ( Exception ex )
			{
				Settings = DynamicJson.Parse( DEFAULT_SETTINGS );
				ErrorReporter.SendErrorReport( ex, string.Format( "读取插件设置时出错，{0}({1}", MenuTitle, Version ) );
			}

			DBSender = new APIKancolleDB();
			PoiSender = new APIPoiSender();

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
					bool sendWithProxy = Settings.DBSender.send_with_proxy;
					Task.Factory.StartNew( (Action)( () => DBSender.ExecuteSession( oSession, sendWithProxy ) ) );
				}

				// poi-statistics 送信
				if ( !Settings.Poi() || Settings.Poi.enable )
				{
					Task.Factory.StartNew( (Action)( () => PoiSender.ExecuteSession( oSession ) ) );
				}
			}
		}
	}
}
