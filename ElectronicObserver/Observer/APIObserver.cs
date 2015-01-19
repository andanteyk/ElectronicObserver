using Codeplex.Data;
using ElectronicObserver.Observer.kcsapi;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ElectronicObserver.Observer {
	

	public sealed class APIObserver {


		#region Singleton

		private static readonly APIObserver instance = new APIObserver();

		public static APIObserver Instance {
			get { return instance; }
		}

		#endregion


		public APIDictionary APIList;

		public string ServerAddress { get; private set; }


		private APIObserver() {

			// 注：重複登録するとあらぬところで落ちるので十分注意すること

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

			APIList.Add( new kcsapi.api_req_quest.clearitemget() );
			APIList.Add( new kcsapi.api_req_nyukyo.start() );
			APIList.Add( new kcsapi.api_req_nyukyo.speedchange() );
			APIList.Add( new kcsapi.api_req_kousyou.createship() );
			APIList.Add( new kcsapi.api_req_kousyou.createship_speedchange() );
			APIList.Add( new kcsapi.api_req_hensei.change() );
			APIList.Add( new kcsapi.api_req_member.updatedeckname() );
			APIList.Add( new kcsapi.api_req_kaisou.remodeling() );


			ServerAddress = null;

			Fiddler.FiddlerApplication.BeforeRequest += FiddlerApplication_BeforeRequest;
			Fiddler.FiddlerApplication.AfterSessionComplete += FiddlerApplication_AfterSessionComplete;
			
		}




		public int Start( int portID ) {

			Fiddler.FiddlerApplication.Startup( portID, Fiddler.FiddlerCoreStartupFlags.ChainToUpstreamGateway | Fiddler.FiddlerCoreStartupFlags.RegisterAsSystemProxy );

			Fiddler.URLMonInterop.SetProxyInProcess( string.Format( "127.0.0.1:{0}",
						Fiddler.FiddlerApplication.oProxy.ListenPort ), "<local>" );

			Utility.Logger.Add( 2, string.Format( "APIObserver: ポート {0} 番で受信を開始しました。", Fiddler.FiddlerApplication.oProxy.ListenPort ) );

			return Fiddler.FiddlerApplication.oProxy.ListenPort;
		}


		public void Stop() {
			
			Fiddler.URLMonInterop.ResetProxyInProcessToDefault();
			Fiddler.FiddlerApplication.Shutdown();

			Utility.Logger.Add( 2, "APIObserver: 受信を停止しました。" );
		}




		private void FiddlerApplication_AfterSessionComplete( Fiddler.Session oSession ) {

			//保存
			{
				Utility.Configuration.ConfigurationData.ConfigConnection c = Utility.Configuration.Config.Connection;

				if ( c.SaveReceivedData ) {

					try {

						if ( c.SaveResponse && oSession.fullUrl.Contains( "/kcsapi/" ) ) {

							SaveResponse( oSession.fullUrl, oSession.GetResponseBodyAsString() );

						} else if ( c.SaveSWF && oSession.fullUrl.IndexOf( "/kcs/" ) != -1 && oSession.oResponse.MIMEType == "application/x-shockwave-flash" ) {

							//string tpath = string.Format( "{0}\\{1}", c.SaveDataPath, oSession.fullUrl.Substring( oSession.fullUrl.LastIndexOf( "/" ) + 1 ) );
							string tpath = string.Format( "{0}\\{1}", c.SaveDataPath, oSession.fullUrl.Substring( oSession.fullUrl.IndexOf( "/kcs/" ) + 5 ).Replace( "/", "\\" ) );
							{
								int index = tpath.IndexOf( "?" );
								if ( index != -1 ) {
									if ( Utility.Configuration.Config.Connection.ApplyVersion ) {
										string over = tpath.Substring( index + 1 );
										if ( over.Contains( "VERSION=" ) ) {
											string version = over.Substring( over.LastIndexOf( '=' ) + 1 );
											tpath = tpath.Insert( tpath.LastIndexOf( '.' ), "_v" + version );
											index += version.Length + 2;
										}
									}
									tpath = tpath.Substring( 0, index );
								}
							} 
							Directory.CreateDirectory( Path.GetDirectoryName( tpath ) );

							using ( var sw = new System.IO.BinaryWriter( System.IO.File.OpenWrite( tpath ) ) ) {
								sw.Write( oSession.ResponseBody );
							}

							Utility.Logger.Add( 1, string.Format( "通信からファイル {0} を保存しました。", tpath ) );

						} else if ( c.SaveOtherFile && oSession.fullUrl.IndexOf( "/kcs/" ) != -1 ) {

							string tpath = string.Format( "{0}\\{1}", c.SaveDataPath, oSession.fullUrl.Substring( oSession.fullUrl.IndexOf( "/kcs/" ) + 5 ).Replace( "/", "\\" ) );
							{
								int index = tpath.IndexOf( "?" );
								if ( index != -1 ) {
									if ( Utility.Configuration.Config.Connection.ApplyVersion ) {
										string over = tpath.Substring( index + 1 );
										if ( over.Contains( "VERSION=" ) ) {
											string version = over.Substring( over.LastIndexOf( '=' ) + 1 );
											tpath = tpath.Insert( tpath.LastIndexOf( '.' ), "_v" + version );
											index += version.Length + 2;
										}
									}
									tpath = tpath.Substring( 0, index );
								}
							}  
							Directory.CreateDirectory( Path.GetDirectoryName( tpath ) );

							using ( var sw = new System.IO.BinaryWriter( System.IO.File.OpenWrite( tpath ) ) ) {
								sw.Write( oSession.ResponseBody );
							}

							Utility.Logger.Add( 1, string.Format( "通信からファイル {0} を保存しました。", tpath ) );

						}


					} catch ( Exception ex ) {

						Utility.ErrorReporter.SendErrorReport( ex, "通信内容の保存に失敗しました。" );
					}

				}

			}


			if ( oSession.fullUrl.Contains( "/kcsapi/" ) && oSession.oResponse.MIMEType == "text/plain" ) {

				LoadResponse( oSession.fullUrl, oSession.GetResponseBodyAsString() );

			}



			if ( ServerAddress == null ) {
				string url = oSession.fullUrl;

				int idxb = url.IndexOf( "/kcsapi/" );
					
				if ( idxb != -1 ) {
					int idxa = url.LastIndexOf( "/", idxb - 1 );

					ServerAddress = url.Substring( idxa + 1, idxb - idxa - 1 );
				}
			}

		}



		private void FiddlerApplication_BeforeRequest( Fiddler.Session oSession ) {

			if ( oSession.fullUrl.Contains( "/kcsapi/" ) ) {
				
				//保存
				{	
					Utility.Configuration.ConfigurationData.ConfigConnection c = Utility.Configuration.Config.Connection;

					if ( c.SaveReceivedData && c.SaveRequest ) {

						SaveRequest( oSession.fullUrl, oSession.GetRequestBodyAsString() );
					}
				}


				LoadRequest( oSession.fullUrl, oSession.GetRequestBodyAsString() );

			}

		}



		public void LoadRequest( string path, string data ) {

			string shortpath = path.Substring( path.LastIndexOf( "/kcsapi/" ) + 8 );

			try {
				
				Utility.Logger.Add( 1, "Request を受信しました : " + shortpath );

			
				var parsedData = new Dictionary<string,string>();
				data = HttpUtility.UrlDecode( data );

				foreach ( string unit in data.Split( "&".ToCharArray() ) ) {
					string[] pair = unit.Split( "=".ToCharArray() );
					parsedData.Add( pair[0], pair[1] );
				}


				APIList.OnRequestReceived( shortpath, parsedData );


			} catch ( Exception ex ) {

				ErrorReporter.SendErrorReport( ex, "Request の受信中にエラーが発生しました。", shortpath, data );
			
			}

		}


		public void LoadResponse( string path, string data ) {

			string shortpath = path.Substring( path.LastIndexOf( "/kcsapi/" ) + 8 );
				
			try {

				Utility.Logger.Add( 1, "Responseを受信しました : " + shortpath );

			
				var json = DynamicJson.Parse( data.Substring( 7 ) );		//remove "svdata="

				if ( (int)json.api_result != 1 ) {

					var ex = new ArgumentException( "エラーコードを含むメッセージを受信しました。" );
					Utility.ErrorReporter.SendErrorReport( ex, "エラーコードを含むメッセージを受信しました。" );
					throw ex;
				}


				if ( shortpath == "api_get_member/ship2" )
					APIList.OnResponseReceived( shortpath, json );
				else if ( json.IsDefined( "api_data" ) )
					APIList.OnResponseReceived( shortpath, json.api_data );
				else
					APIList.OnResponseReceived( shortpath, null );


			} catch ( Exception ex ) {

				ErrorReporter.SendErrorReport( ex, "Responseの受信中にエラーが発生しました。", shortpath, data );
			
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
