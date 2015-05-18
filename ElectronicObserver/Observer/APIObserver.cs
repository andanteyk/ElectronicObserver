using Codeplex.Data;
using ElectronicObserver.Observer.Cache;
using ElectronicObserver.Observer.kcsapi;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Mathematics;
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

		private CacheCore cache = new CacheCore();

		public APIDictionary APIList;

		public string ServerAddress { get; private set; }
		public int ProxyPort { get { return Fiddler.FiddlerApplication.oProxy.ListenPort; } }

		public delegate void ProxyStartedEventHandler();
		public event ProxyStartedEventHandler ProxyStarted = delegate { };

		private Control UIControl;

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
			APIList.Add( new kcsapi.api_get_member.ship_deck() );
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
			Fiddler.FiddlerApplication.BeforeResponse += FiddlerApplication_BeforeResponse;
			Fiddler.FiddlerApplication.AfterSessionComplete += FiddlerApplication_AfterSessionComplete;

		}


		/// <summary>
		/// 通信の受信を開始します。
		/// </summary>
		/// <param name="portID">受信に使用するポート番号。</param>
		/// <param name="UIControl">GUI スレッドで実行するためのオブジェクト。中身は何でもいい</param>
		/// <returns>実際に使用されるポート番号。</returns>
		public int Start( int portID, Control UIControl ) {
			this.UIControl = UIControl;

			Fiddler.FiddlerApplication.Startup( portID, Fiddler.FiddlerCoreStartupFlags.ChainToUpstreamGateway |
				( Utility.Configuration.Config.Connection.RegisterAsSystemProxy ? Fiddler.FiddlerCoreStartupFlags.RegisterAsSystemProxy : 0 ) );

			/*
			Fiddler.URLMonInterop.SetProxyInProcess( string.Format( "127.0.0.1:{0}",
						Fiddler.FiddlerApplication.oProxy.ListenPort ), "<local>" );
			*/
			ProxyStarted();

			Utility.Logger.Add( 2, string.Format( "APIObserver: ポート {0} 番で受信を開始しました。", Fiddler.FiddlerApplication.oProxy.ListenPort ) );


			//checkme: 一応警告をつけてみる
			if ( portID != Fiddler.FiddlerApplication.oProxy.ListenPort ) {
				Utility.Logger.Add( 3, "APIObserver: 実際に受信を開始したポート番号が指定されたポート番号とは異なります。" );
			}

			return Fiddler.FiddlerApplication.oProxy.ListenPort;
		}


		/// <summary>
		/// 通信の受信を停止します。
		/// </summary>
		public void Stop() {

			Fiddler.URLMonInterop.ResetProxyInProcessToDefault();
			Fiddler.FiddlerApplication.Shutdown();

			Utility.Logger.Add( 2, "APIObserver: 受信を停止しました。" );
		}



		public APIBase this[string key] {
			get {
				if ( APIList.ContainsKey( key ) ) return APIList[key];
				else return null;
			}
		}


		private void FiddlerApplication_AfterSessionComplete( Fiddler.Session oSession ) {

			//保存
			{
				Utility.Configuration.ConfigurationData.ConfigConnection c = Utility.Configuration.Config.Connection;

				if ( c.SaveReceivedData ) {

					try {

						if ( c.SaveResponse && oSession.fullUrl.Contains( "/kcsapi/" ) ) {

							// 非同期で書き出し処理するので取っておく
							// stringはイミュータブルなのでOK
							string url = oSession.fullUrl;
							string body = oSession.GetResponseBodyAsString();

							Task.Run( (Action)( () => {
								SaveResponse( url, body );
							} ) );

						} else if ( oSession.fullUrl.Contains( "/kcs/" ) &&
							( ( c.SaveSWF && oSession.oResponse.MIMEType == "application/x-shockwave-flash" ) || c.SaveOtherFile ) ) {

							string saveDataPath = c.SaveDataPath; // スレッド間の競合を避けるため取っておく
							string tpath = string.Format( "{0}\\{1}", saveDataPath, oSession.fullUrl.Substring( oSession.fullUrl.IndexOf( "/kcs/" ) + 5 ).Replace( "/", "\\" ) );
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
							byte[] responseCopy = new byte[oSession.ResponseBody.Length];
							Array.Copy( oSession.ResponseBody, responseCopy, oSession.ResponseBody.Length );

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

			}


			if ( oSession.fullUrl.Contains( "/kcsapi/" ) && oSession.oResponse.MIMEType == "text/plain" ) {

				// 非同期でGUIスレッドに渡すので取っておく
				// stringはイミュータブルなのでOK
				string url = oSession.fullUrl;
				string body = oSession.GetResponseBodyAsString();
				UIControl.BeginInvoke( (Action)( () => { LoadResponse( url, body ); } ) );

			} else if ( Configuration.Config.CacheSettings.CacheEnabled && oSession.responseCode == 200 ) {
				string filepath = TaskRecord.GetAndRemove( oSession.fullUrl );
				if ( !string.IsNullOrEmpty( filepath ) ) {
					if ( File.Exists( filepath ) )
						File.Delete( filepath );

					//保存下载文件并记录Modified-Time
					try {

						if ( Configuration.Config.Log.ShowCacheLog ) {

							Utility.Logger.Add( 2, string.Format( "更新缓存文件：{0}.", filepath ) );
						}

						oSession.SaveResponseBody( filepath );
						_SaveModifiedTime( filepath, oSession.oResponse.headers["Last-Modified"] );
						//Debug.WriteLine("CACHR> 【下载文件】" + oSession.PathAndQuery);
					} catch ( Exception ex ) {
						Utility.ErrorReporter.SendErrorReport( ex, "会话结束时，保存返回文件时发生异常：" + oSession.fullUrl );
					}
				}
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

		// regex
		private Regex _wmodeRegex = new Regex( @"""wmode""[\s]*?:[\s]*?""[^""]+?""", RegexOptions.Compiled );
		private Regex _qualityRegex = new Regex( @"""quality""[\s]*?:[\s]*?""[^""]+?""", RegexOptions.Compiled );

		private void FiddlerApplication_BeforeResponse( Fiddler.Session oSession ) {
			if ( Configuration.Config.CacheSettings.CacheEnabled && oSession.PathAndQuery.StartsWith( "/kcs/" ) && oSession.responseCode == 304 ) {
				string filepath = TaskRecord.GetAndRemove( oSession.fullUrl );
				//只有TaskRecord中有记录的文件才是验证的文件，才需要修改Header
				if ( !string.IsNullOrEmpty( filepath ) ) {
					//服务器返回304，文件没有修改 -> 返回本地文件
					oSession.bBufferResponse = true;
					oSession.ResponseBody = File.ReadAllBytes( filepath );
					oSession.oResponse.headers.HTTPResponseCode = 200;
					oSession.oResponse.headers.HTTPResponseStatus = "200 OK";
					oSession.oResponse.headers["Last-Modified"] = oSession.oRequest.headers["If-Modified-Since"];
					oSession.oResponse.headers["Accept-Ranges"] = "bytes";
					oSession.oResponse.headers.Remove( "If-Modified-Since" );
					oSession.oRequest.headers.Remove( "If-Modified-Since" );
					if ( filepath.EndsWith( ".swf" ) )
						oSession.oResponse.headers["Content-Type"] = "application/x-shockwave-flash";
				}

			} else if ( oSession.PathAndQuery.StartsWith( "/kcs" ) && oSession.responseCode != 200 && oSession.responseCode != 304 ) {

				Utility.ErrorReporter.SendErrorReport( new Exception( oSession.fullUrl ), "返回错误状态码：" + oSession.responseCode, oSession.fullUrl, oSession.GetResponseBodyAsString() );

			} else if ( oSession.bBufferResponse ) {

				if ( oSession.fullUrl.Contains( "/kcsapi/api_start2" ) ) {
					string api_start2 = oSession.GetResponseBodyAsString();

					var mod = Utility.Modify.ModifyConfiguration.Instance;
					bool changed = false;

					for ( int i = 0; i < mod.Count; i++ ) {

						var node = mod[i];
						string pattern = @"{""api_id"":[\d]+?,""api_sortno"":[\d]+?,""api_filename"":""" + node.api_filename + @""",""api_version"":""[\d]+?"",""api_boko_n"":\[[\d-,]+?\],""api_boko_d"":\[[\d-,]+?\],""api_kaisyu_n"":\[[\d-,]+?\],""api_kaisyu_d"":\[[\d-,]+?\],""api_kaizo_n"":\[[\d-,]+?\],""api_kaizo_d"":\[[\d-,]+?\],""api_map_n"":\[[\d-,]+?\],""api_map_d"":\[[\d-,]+?\],""api_ensyuf_n"":\[[\d-,]+?\],""api_ensyuf_d"":\[[\d-,]+?\],""api_ensyue_n"":\[[\d-,]+?\],""api_battle_n"":\[[\d-,]+?\],""api_battle_d"":\[[\d-,]+?\],""api_weda"":\[[\d-,]+?\],""api_wedb"":\[[\d-,]+?\]}";

						var m = Regex.Match( api_start2, pattern );
						if ( m.Success ) {

							var json = Codeplex.Data.DynamicJson.Parse( m.Value );

							// 魔改立绘坐标
							bool flag = ModifyIt( "api_boko_n", json, node.api_parameter );
							flag |= ModifyIt( "api_boko_d", json, node.api_parameter );
							flag |= ModifyIt( "api_kaisyu_n", json, node.api_parameter );
							flag |= ModifyIt( "api_kaisyu_d", json, node.api_parameter );
							flag |= ModifyIt( "api_kaizo_n", json, node.api_parameter );
							flag |= ModifyIt( "api_kaizo_d", json, node.api_parameter );
							flag |= ModifyIt( "api_map_n", json, node.api_parameter );
							flag |= ModifyIt( "api_map_d", json, node.api_parameter );
							flag |= ModifyIt( "api_ensyuf_n", json, node.api_parameter );
							flag |= ModifyIt( "api_ensyuf_d", json, node.api_parameter );
							flag |= ModifyIt( "api_ensyue_n", json, node.api_parameter );
							flag |= ModifyIt( "api_battle_n", json, node.api_parameter );
							flag |= ModifyIt( "api_battle_d", json, node.api_parameter );
							flag |= ModifyIt( "api_weda", json, node.api_parameter );
							flag |= ModifyIt( "api_wedb", json, node.api_parameter );

							if ( flag ) {
								api_start2 = api_start2.Replace( m.Value, json.ToString() );
								changed = true;
							}

							// 魔改名称
							if ( !string.IsNullOrEmpty( node.api_name ) ) {


								pattern = @"{""api_id"":" + json.api_id + @",""api_sortno"":" + json.api_sortno + @",""api_name"":""";
								m = Regex.Match( api_start2, pattern + @"[^""]+?"",""api_yomi"":""" );
								if ( m.Success ) {

									api_start2 = api_start2.Replace( m.Value, pattern + node.Raw_api_name + @""",""api_yomi"":""" );
									changed = true;
								}
							}

							if ( changed ) {
								Utility.Logger.Add( 2, string.Format( "应用魔改：{0} → {1}", node.api_filename, node.api_name ) );
							}
						}
					}

					// 如果有变动
					if ( changed ) {
						oSession.utilSetResponseBody( api_start2 );
					}

				} else if ( oSession.fullUrl.Contains( "/gadget/js/kcs_flash.js" ) ) {

					string js = oSession.GetResponseBodyAsString();
					bool flag = false;

					var wmode = _wmodeRegex.Match( js );
					if ( wmode.Success ) {
						js = js.Replace( wmode.Value, string.Format( @"""wmode"":""{0}""", Utility.Configuration.Config.FormBrowser.FlashWmode ) );
						flag = true;
					}

					var quality = _qualityRegex.Match( js );
					if ( quality.Success ) {
						js = js.Replace( quality.Value, string.Format( @"""quality"":""{0}""", Utility.Configuration.Config.FormBrowser.FlashQuality ) );
						flag = true;
					}

					if ( flag ) {
						oSession.utilSetResponseBody( js );

						Utility.Logger.Add( 1, "应用自定义flash模式/质量" );
					}
				}
			}
		}

		private bool ModifyIt( string parameter, dynamic source, dynamic dest ) {

			var g = new Utility.Modify.ModifyBinder( parameter );
			object o;

			if ( dest.TryGetMember( g, out o ) ) {

				try {
					if ( o is Codeplex.Data.DynamicJson ) {
						int[] rst = ( (Codeplex.Data.DynamicJson)o ).Deserialize<int[]>();

						if ( rst.Length == 2 ) {

							return source.TrySetMember( new Utility.Modify.ModifySetBinder( parameter ), rst );
						}
					}

				} catch ( Exception e ) {
					Utility.ErrorReporter.SendErrorReport( e, string.Format( "魔改参数错误，{0}.{1}", source.api_filename, parameter ) );
				}
			}

			return false;
		}


		/// <summary>
		/// セッションが SSL 接続を使用しているかどうかを返します。
		/// </summary>
		/// <param name="session">セッション。</param>
		/// <returns>セッションが SSL 接続を使用する場合は true、そうでない場合は false。</returns>
		private bool IsSessionSSL( Fiddler.Session session ) {
			// 「http://www.dmm.com:433/」の場合もあり、これは Session.isHTTPS では判定できない
			return session.isHTTPS || session.fullUrl.StartsWith( "https:" ) || session.fullUrl.Contains( ":443" );
		}

		private void FiddlerApplication_BeforeRequest( Fiddler.Session oSession ) {

			Utility.Configuration.ConfigurationData.ConfigConnection c = Utility.Configuration.Config.Connection;


			// 上流プロキシ設定
			if ( c.UseUpstreamProxy ) {
				if ( c.EnableSslUpstreamProxy || !IsSessionSSL( oSession ) ) {
					oSession["X-OverrideGateway"] = string.Format( "{0}:{1}", c.UpstreamProxyAddress, c.UpstreamProxyPort );
				}
			}


			if ( oSession.fullUrl.Contains( "/kcsapi/" ) ) {

				string url = oSession.fullUrl;
				string body = oSession.GetRequestBodyAsString();

				// 魔改api_start2
				{
					if ( oSession.fullUrl.Contains( "/kcsapi/api_start2" ) && Utility.Modify.ModifyConfiguration.Instance.Count > 0 ) {
						oSession.bBufferResponse = true;
					}
				}

				//保存
				{
					if ( c.SaveReceivedData && c.SaveRequest ) {

						Task.Run( (Action)( () => {
							SaveRequest( url, body );
						} ) );
					}
				}

				UIControl.BeginInvoke( (Action)( () => { LoadRequest( url, body ); } ) );
			} else if ( Configuration.Config.CacheSettings.CacheEnabled && oSession.fullUrl.Contains( "/kcs/" ) ) {

				// = KanColleCacher =
				string filepath;
				var direction = cache.GotNewRequest( oSession.fullUrl, out filepath );

				if ( direction == Direction.Return_LocalFile ) {

					//返回本地文件
					oSession.utilCreateResponseAndBypassServer();
					oSession.ResponseBody = File.ReadAllBytes( filepath );
					oSession.oResponse.headers["Server"] = "Apache";
					oSession.oResponse.headers["Cache-Control"] = "max-age=18000, public";
					oSession.oResponse.headers["Date"] = GMTHelper.ToGMTString( DateTime.Now );
					oSession.oResponse.headers["Connection"] = "close";
					oSession.oResponse.headers["Accept-Ranges"] = "bytes";

					filepath = filepath.ToLower();
					if ( filepath.EndsWith( ".swf" ) )
						oSession.oResponse.headers["Content-Type"] = "application/x-shockwave-flash";
					else if ( filepath.EndsWith( ".mp3" ) )
						oSession.oResponse.headers["Content-Type"] = "audio/mpeg";
					else if ( filepath.EndsWith( ".png" ) )
						oSession.oResponse.headers["Content-Type"] = "image/png";

					//Debug.WriteLine("CACHR> 【返回本地】" + result);

				} else if ( direction == Direction.Verify_LocalFile ) {

					//请求服务器验证文件
					oSession.oRequest.headers["If-Modified-Since"] = _GetModifiedTime( filepath );
					oSession.bBufferResponse = true;

					//Debug.WriteLine("CACHR> 【验证文件】" + oSession.PathAndQuery);

				} else if ( Configuration.Config.Log.ShowCacheLog && ( Configuration.Config.Log.ShowMainD2Link || !oSession.fullUrl.Contains( "mainD2.swf" ) ) ) {

					//下载文件
					Utility.Logger.Add( 2, string.Format( "BeforeRequest: 重新下载文件。{0}", oSession.fullUrl ) );
				}

			}

			// flash wmode & quality
			else if ( oSession.fullUrl.Contains( "/gadget/js/kcs_flash.js" ) ) {

				oSession.bBufferResponse = true;
			}

		}


		private string _GetModifiedTime( string filepath ) {
			FileInfo fi;
			DateTime dt = default( DateTime );
			try {
				fi = new FileInfo( filepath );
				dt = fi.LastWriteTime;
				return GMTHelper.ToGMTString( dt );
			} catch ( Exception ex ) {
				Utility.ErrorReporter.SendErrorReport( ex, "在读取文件修改时间时发生异常：" + dt );
				return "";
			}
		}

		private void _SaveModifiedTime( string filepath, string gmTime ) {
			FileInfo fi;
			try {
				fi = new FileInfo( filepath );
				DateTime dt = GMTHelper.GMT2Local( gmTime );
				if ( dt.Year > 1900 ) {
					fi.LastWriteTime = dt;
				}
			} catch ( Exception ex ) {
				Utility.ErrorReporter.SendErrorReport( ex, string.Format( "在保存文件修改时间时发生异常。filepath: {0}, gmTime: {1}", filepath, gmTime ) );
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

			} finally {

				SystemEvents.UpdateTimerEnabled = true;

			}

		}


		private void SaveRequest( string url, string body ) {

			try {

				if ( !Directory.Exists( Utility.Configuration.Config.Connection.SaveDataPath ) ) {
					Directory.CreateDirectory( Utility.Configuration.Config.Connection.SaveDataPath );
				}

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

				if ( !Directory.Exists( Utility.Configuration.Config.Connection.SaveDataPath ) ) {
					Directory.CreateDirectory( Utility.Configuration.Config.Connection.SaveDataPath );
				}

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
