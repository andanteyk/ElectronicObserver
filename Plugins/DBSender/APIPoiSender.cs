using Codeplex.Data;
using ElectronicObserver.Data;
using Fiddler;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer
{
	public class APIPoiSender
	{

		private readonly string SERVER_HOSTNAME = "poi.0u0.moe";
		private readonly string UAString =
#if TEST
			"ElectronicObserver-MaKai Plugin Test";
#else
            "ElectronicObserver-MaKai Plugin v2.0.0.1";
#endif

		public APIPoiSender()
		{
			// to avoid http 417 error
			ServicePointManager.Expect100Continue = false;
		}

		public void ExecuteSession( Session oSession )
		{
			try
			{
				switch ( oSession.PathAndQuery )
				{

					case "/kcsapi/api_req_kousyou/createitem":
						CreateItem(
							ParseApiData( oSession.GetResponseBodyAsString() ),
							ParseRequest( oSession.GetRequestBodyAsString() ) ); break;


					case "/kcsapi/api_req_kousyou/createship":
						CreateShip( ParseRequest( oSession.GetRequestBodyAsString() ) ); break;
					case "/kcsapi/api_get_member/kdock":
						KDockEvent( ParseApiData( oSession.GetResponseBodyAsString() ) ); break;


					case "/kcsapi/api_get_member/mapinfo":
						this.mapinfo = ParseApiData( oSession.GetResponseBodyAsString() ); break;
					case "/kcsapi/api_req_map/start":
					case "/kcsapi/api_req_map/next":
						StartNextEvent( ParseApiData( oSession.GetResponseBodyAsString() ) ); break;
					case "/kcsapi/api_req_sortie/battle":
					case "/kcsapi/api_req_combined_battle/battle":
					case "/kcsapi/api_req_combined_battle/airbattle":
					case "/kcsapi/api_req_combined_battle/battle_water":
						BattleEvent( ParseApiData( oSession.GetResponseBodyAsString() ) ); break;
					case "/kcsapi/api_req_sortie/battleresult":
					case "/kcsapi/api_req_combined_battle/battleresult":
						BattleResultEvent( ParseApiData( oSession.GetResponseBodyAsString() ) ); break;

				}
			}
			catch ( ParseException ) { }
			catch ( Exception ex )
			{
				Utility.ErrorReporter.SendErrorReport( ex, string.Format( "提交 poi-statistics 时发生错误：{0}", ex.Message ) );
			}
		}

		#region - Tool Method -

		private dynamic ParseApiData( string body )
		{
			if ( string.IsNullOrEmpty( body ) )
				throw new ParseException( "正文不可为空。" );

			if ( body.StartsWith( "svdata=" ) )
			{
				var json = DynamicJson.Parse( body.Substring( 7 ) );        //remove "svdata="

				if ( (int)json.api_result != 1 )
					throw new ParseException( "返回信息中含有错误码。" );

				return json.api_data;
			}

			throw new ParseException( "无效数据。" );
		}

		private NameValueCollection ParseRequest( string request )
		{
			var pars = new NameValueCollection();
			request = System.Web.HttpUtility.UrlDecode( request );

			string[] pairs = request.Split( new char[] { '&' } );
			int tmpIndex;
			foreach ( string pair in pairs )
			{
				if ( string.IsNullOrEmpty( pair ) )
					continue;

				tmpIndex = pair.IndexOf( '=' );
				if ( tmpIndex == -1 )
					continue;

				pars.Add( pair.Substring( 0, tmpIndex ), pair.Substring( tmpIndex + 1 ) );
			}
			return pars;
		}

		private void Report( string data, string apiname )
		{
			HttpWebRequest wrq = (HttpWebRequest)WebRequest.Create(
				string.Format( "http://{0}/api/report/v2/{1}", SERVER_HOSTNAME, apiname ) );

			wrq.UserAgent = UAString;
			wrq.Method = "POST";
			wrq.Timeout = 30000;

#if TEST
			Utility.Logger.Add( 1, string.Format( "data={0}", data ) );
#else
			byte[] bs = Encoding.UTF8.GetBytes( string.Format( "data={0}", data ) );
			using ( System.IO.Stream reqs = wrq.GetRequestStream() )
				reqs.Write( bs, 0, bs.Length );

			wrq.ContentType = "text/plain-text";
			try
			{
				var response = (HttpWebResponse)wrq.GetResponse();  // Async().Result;
				Utility.Logger.Add( 1, string.Format( "已发送至 poi-statistics: {0}-{1}", (int)response.StatusCode, response.StatusCode ) );
			}
			catch ( Exception ex )
			{
                Utility.Logger.Add(1, "送信至 poi-statistics 失败 : " + ex.Message);
			}
#endif
		}

		private void ReportAsync( dynamic data, string apiname )
		{
			if ( data != null )
			{
				string str = data.ToString();
				Task.Factory.StartNew( () => Report( str, apiname ) );
			}
		}

		#endregion

		#region - Logic -

		private dynamic createship;
		private bool waitForDock;

		private dynamic mapinfo;
		private dynamic dropship;
		private bool waitForBattleResult;

		private void CreateItem( dynamic data, NameValueCollection request )
		{
			dynamic item = new DynamicJson();
			item.items = new[]
			{
				int.Parse(request["api_item1"]),
				int.Parse(request["api_item2"]),
				int.Parse(request["api_item3"]),
				int.Parse(request["api_item4"])
			};
			item.secretary = KCDatabase.Instance.Fleet[1].MembersInstance[0].ShipID;
			item.successful = ( data.api_create_flag != 0 );
			item.teitokuLv = KCDatabase.Instance.Admiral.Level;
			item.itemId = item.successful ? data.api_slot_item.api_slotitem_id : int.Parse( data.api_fdata.Split( ',' )[1] );
			ReportAsync( item, "create_item" );
		}

		private void CreateShip( NameValueCollection request )
		{
			createship = new DynamicJson();
			createship.items = new[]
			{
				int.Parse( request["api_item1"] ),
				int.Parse( request["api_item2"] ),
				int.Parse( request["api_item3"] ),
				int.Parse( request["api_item4"] ),
				int.Parse( request["api_item5"] )
			};
			createship.kdockId = int.Parse( request["api_kdock_id"] ) - 1;
			createship.secretary = KCDatabase.Instance.Fleet[1].MembersInstance[0].ShipID;
			createship.teitokuLv = KCDatabase.Instance.Admiral.Level;
			createship.largeFlag = ( int.Parse( request["api_large_flag"] ) != 0 );
			createship.highspeed = int.Parse( request["api_highspeed"] );
			waitForDock = true;
		}

		private void KDockEvent( dynamic data )
		{
			if ( !waitForDock )
				return;

			createship.shipId = data[(int)createship.kdockId].api_created_ship_id;
			waitForDock = false;

			ReportAsync( createship, "create_ship" );
		}

		private void StartNextEvent( dynamic data )
		{
			dropship = new DynamicJson();
			dropship.mapId = data.api_maparea_id * 10 + data.api_mapinfo_no;
			dropship.cellId = data.api_no;
			dropship.isBoss = ( data.api_event_id == 5 );
			waitForBattleResult = true;
		}

		private void BattleEvent( dynamic data )
		{
			dropship.enemyFormation = data.api_formation[1];
		}

		private void BattleResultEvent( dynamic data )
		{
			if ( !waitForBattleResult )
				return;

			dropship.shipId = data.api_get_ship() ? data.api_get_ship.api_ship_id : -1;
			dropship.enemy = data.api_enemy_info.api_deck_name;
			dropship.quest = data.api_quest_name;

			// TODO: dynamic object 不允许动态方法中使用 lambda
			double mapId = dropship.mapId;
			foreach ( var m in mapinfo )
			{
				if ( m.api_id == mapId )
				{
					dropship.mapLv = m.api_eventmap() ? m.api_eventmap.api_selected_rank : 0;
					break;
				}
			}
			// dropship.mapLv = mapinfo.Where( x => x.api_id == dropship.mapId ).First().api_level;
			dropship.rank = data.api_win_rank;
			dropship.teitokuLv = KCDatabase.Instance.Admiral.Level;
			dropship.enemyShips = ( (int[])data.api_ship_id ).Skip( 1 ).ToArray();
			waitForBattleResult = false;

			ReportAsync( dropship, "drop_ship" );
		}

		#endregion
	}

	class ParseException : Exception
	{
		public ParseException( string message ) : base( message ) { }
	}
}
