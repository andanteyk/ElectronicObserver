using Codeplex.Data;
using ElectronicObserver.Observer.kcsapi;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Mathematics;
using Nekoxy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace ElectronicObserver.Observer {


	public sealed class APIObserver {


		#region Singleton

		private static readonly APIObserver instance = new APIObserver();

		public static APIObserver Instance {
			get { return instance; }
		}

		#endregion



		public APIDictionary APIList { get; private set; }

		public string ServerAddress { get; private set; }
		public int ProxyPort { get; private set; }

		public delegate void ProxyStartedEventHandler();
		public event ProxyStartedEventHandler ProxyStarted = delegate { };

		private Control UIControl;
		private APIKancolleDB DBSender;

		public event APIReceivedEventHandler RequestReceived = delegate { };
		public event APIReceivedEventHandler ResponseReceived = delegate { };


		private APIObserver() {

			APIList = new APIDictionary();
			APIList.Add( new kcsapi.api_start2() );
			APIList.Add( new kcsapi.api_get_member.basic() );
			APIList.Add( new kcsapi.api_get_member.slot_item() );
			APIList.Add( new kcsapi.api_get_member.useitem() );
			APIList.Add( new kcsapi.api_get_member.kdock() );
			APIList.Add( new kcsapi.api_port.port() );
			APIList.Add( new kcsapi.api_get_member.ship2() );
			APIList.Add( new kcsapi.api_get_member.questlist() );
			APIList.Add( new kcsapi.api_get_member.ndock() );
			APIList.Add( new kcsapi.api_req_kousyou.getship() );
			APIList.Add( new kcsapi.api_req_hokyu.charge() );
			APIList.Add( new kcsapi.api_req_kousyou.destroyship() );
			APIList.Add( new kcsapi.api_req_kousyou.destroyitem2() );
			APIList.Add( new kcsapi.api_req_member.get_practice_enemyinfo() );
			APIList.Add( new kcsapi.api_get_member.picture_book() );
			APIList.Add( new kcsapi.api_req_mission.start() );
			APIList.Add( new kcsapi.api_get_member.ship3() );
			APIList.Add( new kcsapi.api_req_kaisou.powerup() );
			APIList.Add( new kcsapi.api_req_map.start() );
			APIList.Add( new kcsapi.api_req_map.next() );
			APIList.Add( new kcsapi.api_req_kousyou.createitem() );
			APIList.Add( new kcsapi.api_req_sortie.battle() );
			APIList.Add( new kcsapi.api_req_sortie.battleresult() );
			APIList.Add( new kcsapi.api_req_battle_midnight.battle() );
			APIList.Add( new kcsapi.api_req_battle_midnight.sp_midnight() );
			APIList.Add( new kcsapi.api_req_combined_battle.battle() );
			APIList.Add( new kcsapi.api_req_combined_battle.midnight_battle() );
			APIList.Add( new kcsapi.api_req_combined_battle.sp_midnight() );
			APIList.Add( new kcsapi.api_req_combined_battle.airbattle() );
			APIList.Add( new kcsapi.api_req_combined_battle.battleresult() );
			APIList.Add( new kcsapi.api_req_practice.battle() );
			APIList.Add( new kcsapi.api_req_practice.midnight_battle() );
			APIList.Add( new kcsapi.api_req_practice.battle_result() );
			APIList.Add( new kcsapi.api_get_member.deck() );
			APIList.Add( new kcsapi.api_get_member.mapinfo() );
			APIList.Add( new kcsapi.api_req_combined_battle.battle_water() );
			APIList.Add( new kcsapi.api_req_combined_battle.goback_port() );
			APIList.Add( new kcsapi.api_req_kousyou.remodel_slot() );
			APIList.Add( new kcsapi.api_get_member.material() );
			APIList.Add( new kcsapi.api_req_mission.result() );
			APIList.Add( new kcsapi.api_req_ranking.getlist() );
			APIList.Add( new kcsapi.api_req_sortie.airbattle() );
			APIList.Add( new kcsapi.api_get_member.ship_deck() );
			APIList.Add( new kcsapi.api_req_kaisou.marriage() );
			APIList.Add( new kcsapi.api_req_hensei.preset_select() );
			APIList.Add( new kcsapi.api_req_kaisou.slot_exchange_index() );
			APIList.Add( new kcsapi.api_get_member.record() );
			APIList.Add( new kcsapi.api_get_member.payitem() );
			APIList.Add( new kcsapi.api_req_kousyou.remodel_slotlist() );
			APIList.Add( new kcsapi.api_req_sortie.ld_airbattle() );
			APIList.Add( new kcsapi.api_req_combined_battle.ld_airbattle() );
			APIList.Add( new kcsapi.api_get_member.require_info() );
			APIList.Add( new kcsapi.api_get_member.base_air_corps() );
			APIList.Add( new kcsapi.api_req_air_corps.set_plane() );
			APIList.Add( new kcsapi.api_req_air_corps.set_action() );
			APIList.Add( new kcsapi.api_req_air_corps.supply() );
			APIList.Add( new kcsapi.api_req_kaisou.slot_deprive() );
			APIList.Add( new kcsapi.api_req_air_corps.expand_base() );
			APIList.Add( new kcsapi.api_req_combined_battle.ec_battle() );
			APIList.Add( new kcsapi.api_req_combined_battle.ec_midnight_battle() );
			APIList.Add( new kcsapi.api_req_combined_battle.each_battle() );
			APIList.Add( new kcsapi.api_req_combined_battle.each_battle_water() );
			APIList.Add( new kcsapi.api_get_member.sortie_conditions() );

			APIList.Add( new kcsapi.api_req_quest.clearitemget() );
			APIList.Add( new kcsapi.api_req_nyukyo.start() );
			APIList.Add( new kcsapi.api_req_nyukyo.speedchange() );
			APIList.Add( new kcsapi.api_req_kousyou.createship() );
			APIList.Add( new kcsapi.api_req_kousyou.createship_speedchange() );
			APIList.Add( new kcsapi.api_req_hensei.change() );
			APIList.Add( new kcsapi.api_req_member.updatedeckname() );
			APIList.Add( new kcsapi.api_req_kaisou.remodeling() );
			APIList.Add( new kcsapi.api_req_kaisou.open_exslot() );
			APIList.Add( new kcsapi.api_req_map.select_eventmap_rank() );
			APIList.Add( new kcsapi.api_req_hensei.combined() );
			APIList.Add( new kcsapi.api_req_member.updatecomment() );
			APIList.Add( new kcsapi.api_req_air_corps.change_name() );
			APIList.Add( new kcsapi.api_req_quest.stop() );


			ServerAddress = null;

			DBSender = new APIKancolleDB();

			HttpProxy.AfterSessionComplete += HttpProxy_AfterSessionComplete;
		}




		/// <summary>
		/// 通信の受信を開始します。
		/// </summary>
		/// <param name="portID">受信に使用するポート番号。</param>
		/// <param name="UIControl">GUI スレッドで実行するためのオブジェクト。中身は何でもいい</param>
		/// <returns>実際に使用されるポート番号。</returns>
		public int Start( int portID, Control UIControl ) {

			Utility.Configuration.ConfigurationData.ConfigConnection c = Utility.Configuration.Config.Connection;


			this.UIControl = UIControl;


			HttpProxy.Shutdown();
			try {

				if ( c.UseUpstreamProxy )
					HttpProxy.UpstreamProxyConfig = new ProxyConfig( ProxyConfigType.SpecificProxy, c.UpstreamProxyAddress, c.UpstreamProxyPort );
				else if ( c.UseSystemProxy )
					HttpProxy.UpstreamProxyConfig = new ProxyConfig( ProxyConfigType.SystemProxy );
				else
					HttpProxy.UpstreamProxyConfig = new ProxyConfig( ProxyConfigType.DirectAccess );

				HttpProxy.Startup( portID, false, false );
				ProxyPort = portID;


				ProxyStarted();

				Utility.Logger.Add( 2, string.Format( "APIObserver: ポート {0} 番で受信を開始しました。", portID ) );

			} catch ( Exception ex ) {

				Utility.Logger.Add( 3, "APIObserver: 受信開始に失敗しました。" + ex.Message );
				ProxyPort = 0;
			}


			return ProxyPort;
		}


		/// <summary>
		/// 通信の受信を停止します。
		/// </summary>
		public void Stop() {

			HttpProxy.Shutdown();

			Utility.Logger.Add( 2, "APIObserver: 受信を停止しました。" );
		}



		public APIBase this[string key] {
			get {
				if ( APIList.ContainsKey( key ) ) return APIList[key];
				else return null;
			}
		}




		void HttpProxy_AfterSessionComplete( Session session ) {

			Utility.Configuration.ConfigurationData.ConfigConnection c = Utility.Configuration.Config.Connection;

			string baseurl = session.Request.PathAndQuery;

			//debug
			//Utility.Logger.Add( 1, baseurl );


			// request
			if ( baseurl.Contains( "/kcsapi/" ) ) {

				string url = baseurl;
				string body = session.Request.BodyAsString;

				//保存
				if ( c.SaveReceivedData && c.SaveRequest ) {

					Task.Run( (Action)( () => {
						SaveRequest( url, body );
					} ) );
				}


				UIControl.BeginInvoke( (Action)( () => { LoadRequest( url, body ); } ) );
			}



			//response
			//保存

			if ( c.SaveReceivedData ) {

				try {

					if ( !Directory.Exists( c.SaveDataPath ) )
						Directory.CreateDirectory( c.SaveDataPath );


					if ( c.SaveResponse && baseurl.Contains( "/kcsapi/" ) ) {

						// 非同期で書き出し処理するので取っておく
						// stringはイミュータブルなのでOK
						string url = baseurl;
						string body = session.Response.BodyAsString;

						Task.Run( (Action)( () => {
							SaveResponse( url, body );
						} ) );

					} else if ( baseurl.Contains( "/kcs/" ) &&
						( ( c.SaveSWF && session.Response.MimeType == "application/x-shockwave-flash" ) || c.SaveOtherFile ) ) {

						string saveDataPath = c.SaveDataPath; // スレッド間の競合を避けるため取っておく
						string tpath = string.Format( "{0}\\{1}", saveDataPath, baseurl.Substring( baseurl.IndexOf( "/kcs/" ) + 5 ).Replace( "/", "\\" ) );
						{
							int index = tpath.IndexOf( "?" );
							if ( index != -1 ) {
								if ( Utility.Configuration.Config.Connection.ApplyVersion ) {
									string over = tpath.Substring( index + 1 );
									int vindex = over.LastIndexOf( "VERSION=", StringComparison.CurrentCultureIgnoreCase );
									if ( vindex != -1 ) {
										string version = over.Substring( vindex + 8 ).Replace( '.', '_' );
										tpath = tpath.Insert( tpath.LastIndexOf( '.', index ), "_v" + version );
										index += version.Length + 2;
									}

								}

								tpath = tpath.Remove( index );
							}
						}

						// 非同期で書き出し処理するので取っておく
						byte[] responseCopy = new byte[session.Response.Body.Length];
						Array.Copy( session.Response.Body, responseCopy, session.Response.Body.Length );

						Task.Run( (Action)( () => {
							try {
								lock ( this ) {
									// 同時に書き込みが走るとアレなのでロックしておく

									Directory.CreateDirectory( Path.GetDirectoryName( tpath ) );

									//System.Diagnostics.Debug.WriteLine( oSession.fullUrl + " => " + tpath );
									using ( var sw = new System.IO.BinaryWriter( System.IO.File.OpenWrite( tpath ) ) ) {
										sw.Write( responseCopy );
									}
								}

								Utility.Logger.Add( 1, string.Format( "通信からファイル {0} を保存しました。", tpath.Remove( 0, saveDataPath.Length + 1 ) ) );

							} catch ( IOException ex ) {	//ファイルがロックされている; 頻繁に出るのでエラーレポートを残さない

								Utility.Logger.Add( 3, "通信内容の保存に失敗しました。 " + ex.Message );
							}
						} ) );

					}

				} catch ( Exception ex ) {

					Utility.ErrorReporter.SendErrorReport( ex, "通信内容の保存に失敗しました。" );
				}

			}




			if ( baseurl.Contains( "/kcsapi/" ) && session.Response.MimeType == "text/plain" ) {

				// 非同期でGUIスレッドに渡すので取っておく
				// stringはイミュータブルなのでOK
				string url = baseurl;
				string body = session.Response.BodyAsString;
				UIControl.BeginInvoke( (Action)( () => { LoadResponse( url, body ); } ) );

				// kancolle-db.netに送信する
				if ( Utility.Configuration.Config.Connection.SendDataToKancolleDB ) {
					Task.Run( (Action)( () => DBSender.ExecuteSession( session ) ) );
				}

			}


			if ( ServerAddress == null && baseurl.Contains( "/kcsapi/" ) ) {
				ServerAddress = session.Request.Headers.Host;
			}

		}



		public void LoadRequest( string path, string data ) {

			string shortpath = path.Substring( path.LastIndexOf( "/kcsapi/" ) + 8 );

			try {

				Utility.Logger.Add( 1, "Request を受信しました : " + shortpath );

				SystemEvents.UpdateTimerEnabled = false;


				var parsedData = new Dictionary<string, string>();
				data = HttpUtility.UrlDecode( data );

				foreach ( string unit in data.Split( "&".ToCharArray() ) ) {
					string[] pair = unit.Split( "=".ToCharArray() );
					parsedData.Add( pair[0], pair[1] );
				}


				APIList.OnRequestReceived( shortpath, parsedData );
				RequestReceived( shortpath, parsedData );

			} catch ( Exception ex ) {

				ErrorReporter.SendErrorReport( ex, "Request の受信中にエラーが発生しました。", shortpath, data );

			} finally {

				SystemEvents.UpdateTimerEnabled = true;

			}

		}


		public void LoadResponse( string path, string data ) {

			string shortpath = path.Substring( path.LastIndexOf( "/kcsapi/" ) + 8 );

			try {

				Utility.Logger.Add( 1, "Responseを受信しました : " + shortpath );

				SystemEvents.UpdateTimerEnabled = false;


				var json = DynamicJson.Parse( data.Substring( 7 ) );		//remove "svdata="

				int result = (int)json.api_result;
				if ( result != 1 ) {

					throw new InvalidOperationException( "猫を検出しました。(エラーコード: " + result + ")" );
				}


				if ( shortpath == "api_get_member/ship2" ) {
					APIList.OnResponseReceived( shortpath, json );
					ResponseReceived( shortpath, json );

				} else if ( json.IsDefined( "api_data" ) ) {
					APIList.OnResponseReceived( shortpath, json.api_data );
					ResponseReceived( shortpath, json.api_data );

				} else {
					APIList.OnResponseReceived( shortpath, null );
					ResponseReceived( shortpath, null );
				}

			} catch ( Exception ex ) {

				ErrorReporter.SendErrorReport( ex, "Responseの受信中にエラーが発生しました。", shortpath, data );

			} finally {

				SystemEvents.UpdateTimerEnabled = true;

			}

		}


		private void SaveRequest( string url, string body ) {

			try {

				string tpath = string.Format( "{0}\\{1}Q@{2}.json", Utility.Configuration.Config.Connection.SaveDataPath, DateTimeHelper.GetTimeStamp(), url.Substring( url.LastIndexOf( "/kcsapi/" ) + 8 ).Replace( "/", "@" ) );

				using ( var sw = new System.IO.StreamWriter( tpath, false, Encoding.UTF8 ) ) {
					sw.Write( body );
				}


			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "Requestの保存に失敗しました。" );

			}
		}


		private void SaveResponse( string url, string body ) {

			try {

				string tpath = string.Format( "{0}\\{1}S@{2}.json", Utility.Configuration.Config.Connection.SaveDataPath, DateTimeHelper.GetTimeStamp(), url.Substring( url.LastIndexOf( "/kcsapi/" ) + 8 ).Replace( "/", "@" ) );

				using ( var sw = new System.IO.StreamWriter( tpath, false, Encoding.UTF8 ) ) {
					sw.Write( body );
				}

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "Responseの保存に失敗しました。" );

			}



		}

	}



}
