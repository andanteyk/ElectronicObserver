using Codeplex.Data;
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
using FiddlerFlags = Fiddler.FiddlerCoreStartupFlags;
using System.Web.Script.Serialization;
using ElectronicObserver.Utility.Modify;

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
		public int ProxyPort { get { return Fiddler.FiddlerApplication.oProxy.ListenPort; } }

		public delegate void ProxyStartedEventHandler();
		public event ProxyStartedEventHandler ProxyStarted = delegate { };

		private Control UIControl;
        private JavaScriptSerializer JavaScriptSerializer = new JavaScriptSerializer();


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

			var flag = FiddlerFlags.ChainToUpstreamGateway; // | FiddlerFlags.OptimizeThreadPool;
			if ( Utility.Configuration.Config.Connection.RegisterAsSystemProxy )
				flag |= FiddlerFlags.RegisterAsSystemProxy;

			Fiddler.FiddlerApplication.Startup( portID, flag );

			/*
			Fiddler.URLMonInterop.SetProxyInProcess( string.Format( "127.0.0.1:{0}",
						Fiddler.FiddlerApplication.oProxy.ListenPort ), "<local>" );
			*/
			ProxyStarted();

			Utility.Logger.Add( 2, string.Format( "APIObserver: 端口 {0} 开始监听。", Fiddler.FiddlerApplication.oProxy.ListenPort ) );


			//checkme: 一応警告をつけてみる
			if ( portID != Fiddler.FiddlerApplication.oProxy.ListenPort ) {
				Utility.Logger.Add( 3, "APIObserver: 实际监听端口号与指定的端口号不一致。" );

				if ( Fiddler.FiddlerApplication.oProxy.ListenPort == 0 ) {
					MessageBox.Show(
						"当前监听端口被占用，请在设置对话框中更换。",
						"启动", MessageBoxButtons.OK, MessageBoxIcon.Error );
				}
			}

			return Fiddler.FiddlerApplication.oProxy.ListenPort;
		}


		/// <summary>
		/// 通信の受信を停止します。
		/// </summary>
		public void Stop() {

			Fiddler.URLMonInterop.ResetProxyInProcessToDefault();
			Fiddler.FiddlerApplication.Shutdown();

			Utility.Logger.Add( 2, "APIObserver: 监听终止。" );

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

						if ( !Directory.Exists( c.SaveDataPath ) )
							Directory.CreateDirectory( c.SaveDataPath );


						if ( c.SaveResponse && oSession.fullUrl.Contains( "/kcsapi/" ) ) {

							// 非同期で書き出し処理するので取っておく
							// stringはイミュータブルなのでOK
							string url = oSession.fullUrl;
							string body = oSession.GetResponseBodyAsString();

							Task.Factory.StartNew( (Action)( () => {
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

							Task.Factory.StartNew( (Action)( () => {
								try {
									lock ( this ) {
										// 同時に書き込みが走るとアレなのでロックしておく

										Directory.CreateDirectory( Path.GetDirectoryName( tpath ) );

										//System.Diagnostics.Debug.WriteLine( oSession.fullUrl + " => " + tpath );
										using ( var sw = new System.IO.BinaryWriter( System.IO.File.OpenWrite( tpath ) ) ) {
											sw.Write( responseCopy );
										}
									}

									Utility.Logger.Add( 1, string.Format( "通信文件 {0} 已保存。", tpath.Remove( 0, saveDataPath.Length + 1 ) ) );

								} catch ( IOException ex ) {	//ファイルがロックされている; 頻繁に出るのでエラーレポートを残さない

									Utility.Logger.Add( 3, "通信内容保存失败。 " + ex.Message );
								}
							} ) );

						}

					} catch ( Exception ex ) {

						Utility.ErrorReporter.SendErrorReport( ex, "通信内容保存失败。" );
					}

				}

			}

			if ( oSession.fullUrl.Contains( "/kcsapi/" ) && oSession.oResponse.MIMEType == "text/plain" ) {

				// 非同期でGUIスレッドに渡すので取っておく
				// stringはイミュータブルなのでOK
				string url = oSession.fullUrl;
				string body = oSession.GetResponseBodyAsString();
				UIControl.BeginInvoke( (Action)( () => { LoadResponse( url, body ); } ) );

			} else if ( ObserverResult( p => {
                try {
                    return p.OnAfterSessionComplete( oSession );
                } catch ( Exception oe ) {
                    Logger.Add( 3, string.Format( "插件 {0}({1}) 执行 OnAfterSessionComplete 时出错！", p.MenuTitle, p.Version ) );
                    ErrorReporter.SendErrorReport( oe, p.MenuTitle );
                    return false;
                }
            } ) ) {

				// do nothing.
			}

			// 保存本地api_start2
			if ( Utility.Configuration.Config.CacheSettings.SaveApiStart2 && oSession.PathAndQuery == "/kcsapi/api_start2" )
			{
				if ( !Directory.Exists( "Record" ) )
					Directory.CreateDirectory( "Record" );
				File.WriteAllText( @"Record\api_start2.json", oSession.GetResponseBodyAsString() );
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

        private bool ObserverResult( Func<Window.Plugins.ObserverPlugin, bool> func ) {

            bool b = false;

            foreach (var p in Configuration.Instance.ObserverPlugins) {
                b |= func( p );
            }

            return b;
        }

        private void FiddlerApplication_BeforeResponse(Fiddler.Session oSession)
        {

            if (ObserverResult(p =>
            {
                try
                {
                    return p.OnBeforeResponse(oSession);
                }
                catch (Exception oe)
                {
                    Logger.Add(3, string.Format("插件 {0}({1}) 执行 OnBeforeResponse 时出错！", p.MenuTitle, p.Version));
                    ErrorReporter.SendErrorReport(oe, p.MenuTitle);
                    return false;
                }
            }))
            {

                // do nothing

            }
            else if (oSession.PathAndQuery.StartsWith("/kcs") && oSession.responseCode >= 400)
            {

                Utility.ErrorReporter.SendErrorReport(new Exception(oSession.fullUrl), "返回错误状态码：" + oSession.responseCode, oSession.fullUrl, oSession.GetResponseBodyAsString());

            }
            else if (oSession.bBufferResponse)
            {

                if (oSession.fullUrl.Contains("/kcsapi/api_start2"))
                {
                    string api_start2full = oSession.GetResponseBodyAsString();

                    // output list
                    string filename = @"Settings\GraphicList.csv";
                    if (Utility.Configuration.Config.Log.OutputGraphicList && !File.Exists(filename))
                    {

                        Task.Factory.StartNew((Action)(() => APIGraphicList.Instance.OutputGraphicList(api_start2full, filename)))
                            .ContinueWith(t => Utility.Logger.Add(2, "输出舰船列表至: " + filename));
                    }

                    var mod = Utility.Modify.ModifyConfiguration.Instance;
                    bool changed = false;
                    string api_start2_json = api_start2full.Substring(7);
                    Dictionary<string, object> api_start2 = JavaScriptSerializer.DeserializeObject(api_start2_json) as Dictionary<string, object>;
                    try
                    {
                        var api_data = api_start2["api_data"] as Dictionary<string, object>;
                        var api_mst_ship = api_data["api_mst_ship"] as object[];
                        var api_mst_shipgraph = api_data["api_mst_shipgraph"] as object[];

                        string shipCache = Path.Combine(Configuration.Config.CacheSettings.CacheFolder, @"kcs\resources\swf\ships");
                        //for debug//shipCache = Path.Combine(Application.StartupPath, "Settings");
                        foreach (var shipgraph_data_obj in api_mst_shipgraph)
                        {
                            var shipgraph_data = shipgraph_data_obj as Dictionary<string, object>;
                            if (shipgraph_data["api_sortno"].ToString() == "0")
                                continue;
                            string shipid = shipgraph_data["api_id"].ToString();
                            string api_filename = shipgraph_data["api_filename"].ToString();
                            var ship_data = api_mst_ship.FirstOrDefault(e => (e as Dictionary<string, object>)["api_id"].ToString() == shipid) as Dictionary<string, object>;

                            string configFile = null;
                            //if (Configuration.Config.CacheSettings.CacheEnabled)
                            {
                                configFile = Path.Combine(shipCache, api_filename + ".config.ini");
                            }
                            if (File.Exists(configFile))//岛风GO格式
                            {
                                IniFile iniFile = new IniFile(configFile);
                                ModifyConfigurationIniNode IniNode = new ModifyConfigurationIniNode();
                                IniNode.api_filename = api_filename;
                                IniNode.api_name = iniFile.ReadString("info", "ship_name", null);
                                IniNode.api_getmes = iniFile.ReadString("info", "getmes", null);
                                //IniNode.api_info = iniFile.ReadString("info", "sinfo", null);
                                IniNode.api_config_parameter = iniFile.ReadSectionValues("graph");

                                bool flag = ModifyIt("api_boko_n", shipgraph_data, IniNode);
                                flag |= ModifyIt("api_boko_d", shipgraph_data, IniNode);
                                flag |= ModifyIt("api_kaisyu_n", shipgraph_data, IniNode);
                                flag |= ModifyIt("api_kaisyu_d", shipgraph_data, IniNode);
                                flag |= ModifyIt("api_kaizo_n", shipgraph_data, IniNode);
                                flag |= ModifyIt("api_kaizo_d", shipgraph_data, IniNode);
                                flag |= ModifyIt("api_map_n", shipgraph_data, IniNode);
                                flag |= ModifyIt("api_map_d", shipgraph_data, IniNode);
                                flag |= ModifyIt("api_ensyuf_n", shipgraph_data, IniNode);
                                flag |= ModifyIt("api_ensyuf_d", shipgraph_data, IniNode);
                                flag |= ModifyIt("api_ensyue_n", shipgraph_data, IniNode);
                                flag |= ModifyIt("api_battle_n", shipgraph_data, IniNode);
                                flag |= ModifyIt("api_battle_d", shipgraph_data, IniNode);
                                flag |= ModifyIt("api_weda", shipgraph_data, IniNode);
                                flag |= ModifyIt("api_wedb", shipgraph_data, IniNode);

                                if (flag)
                                {
                                    changed = true;
                                }

                                // 魔改名称
                                if (!string.IsNullOrEmpty(IniNode.api_name))
                                {
                                    ship_data["api_name"] = IniNode.api_name;
                                    flag = true;
                                    changed = true;
                                }
                                // 魔改获得信息
                                if (!string.IsNullOrEmpty(IniNode.api_getmes))
                                {
                                    ship_data["api_getmes"] = IniNode.api_getmes;
                                    flag = true;
                                    changed = true;
                                }

                                if (flag)
                                {
                                    Utility.Logger.Add(2, string.Format("应用魔改: {0} → {1}", IniNode.api_filename, IniNode.api_name));
                                }
                            }
                            else//ApiModify.json格式
                            {
                                var ModifyNode = Utility.Modify.ModifyConfiguration.Instance.GetModifyNode(api_filename);
                                if (ModifyNode == null)
                                    continue;

                                // 魔改立绘坐标
                                bool flag = ModifyIt("api_boko_n", shipgraph_data, ModifyNode.api_parameter);
                                flag |= ModifyIt("api_boko_d", shipgraph_data, ModifyNode.api_parameter);
                                flag |= ModifyIt("api_kaisyu_n", shipgraph_data, ModifyNode.api_parameter);
                                flag |= ModifyIt("api_kaisyu_d", shipgraph_data, ModifyNode.api_parameter);
                                flag |= ModifyIt("api_kaizo_n", shipgraph_data, ModifyNode.api_parameter);
                                flag |= ModifyIt("api_kaizo_d", shipgraph_data, ModifyNode.api_parameter);
                                flag |= ModifyIt("api_map_n", shipgraph_data, ModifyNode.api_parameter);
                                flag |= ModifyIt("api_map_d", shipgraph_data, ModifyNode.api_parameter);
                                flag |= ModifyIt("api_ensyuf_n", shipgraph_data, ModifyNode.api_parameter);
                                flag |= ModifyIt("api_ensyuf_d", shipgraph_data, ModifyNode.api_parameter);
                                flag |= ModifyIt("api_ensyue_n", shipgraph_data, ModifyNode.api_parameter);
                                flag |= ModifyIt("api_battle_n", shipgraph_data, ModifyNode.api_parameter);
                                flag |= ModifyIt("api_battle_d", shipgraph_data, ModifyNode.api_parameter);
                                flag |= ModifyIt("api_weda", shipgraph_data, ModifyNode.api_parameter);
                                flag |= ModifyIt("api_wedb", shipgraph_data, ModifyNode.api_parameter);

                                if (flag)
                                {
                                    changed = true;
                                }

                                // 魔改名称
                                if (!string.IsNullOrEmpty(ModifyNode.api_name))
                                {
                                    ship_data["api_name"] = ModifyNode.api_name;
                                    flag = true;
                                    changed = true;
                                }
                                if (flag)
                                {
                                    Utility.Logger.Add(2, string.Format("应用魔改: {0} → {1}", ModifyNode.api_filename, ModifyNode.api_name));
                                }
                            }

                        }

                        // 如果有变动
                        if (changed)
                        {
                            StringBuilder builder = new StringBuilder("svdata=");
                            builder.Append(JavaScriptSerializer.Serialize(api_start2));
                            oSession.utilSetResponseBody(builder.ToString());
                        }
                    }
                    catch(Exception e)
                    {
                        Utility.Logger.Add(3, "应用魔改过程中出现错误:" + e.Message + Environment.NewLine + e.StackTrace);
                    }
                }
                else if (oSession.fullUrl.Contains("/gadget/js/kcs_flash.js"))
                {

                    string js = oSession.GetResponseBodyAsString();
                    bool flag = false;

                    var wmode = _wmodeRegex.Match(js);
                    if (wmode.Success)
                    {
                        js = js.Replace(wmode.Value, string.Format(@"""wmode"":""{0}""", Utility.Configuration.Config.FormBrowser.FlashWmode));
                        flag = true;
                    }

                    var quality = _qualityRegex.Match(js);
                    if (quality.Success)
                    {
                        js = js.Replace(quality.Value, string.Format(@"""quality"":""{0}""", Utility.Configuration.Config.FormBrowser.FlashQuality));
                        flag = true;
                    }

                    if (flag)
                    {
                        oSession.utilSetResponseBody(js);

                        Utility.Logger.Add(1, "应用自定义flash模式/质量");
                    }
                }
            }
        }

        private bool ModifyIt(string parameter, Dictionary<string, object> source, Dictionary<string, object> dest)
        {
            try
            {
                if (dest.ContainsKey(parameter))
                {
                    var ModifyData = source[parameter] as object[];
                    var NewModifyData = dest[parameter] as object[];
                    ModifyData[0] = NewModifyData[0];
                    ModifyData[1] = NewModifyData[1];
                    return true;
                }
            }
            catch (Exception e)
            {
                Utility.ErrorReporter.SendErrorReport(e, string.Format("魔改参数错误，{0}.{1}", source["api_filename"], parameter));
            }

            return false;
        }

        private bool ModifyIt(string parameter, Dictionary<string, object> source, ModifyConfigurationIniNode iniNode)
        {
            try
            {
                var ModifyData = source[parameter] as object[];
                string strLeft = parameter.Substring(4) + "_left";
                string strTop = parameter.Substring(4) + "_top";
                int Left, Top;
                bool Modified = false;
                if (int.TryParse(iniNode.Get(strLeft), out Left))
                {
                    ModifyData[0] = Left;
                    Modified = true;
                }
                if (int.TryParse(iniNode.Get(strTop), out Top))
                {
                    ModifyData[1] = Top;
                    Modified = true;
                }
                return Modified;
            }
            catch (Exception e)
            {
                Utility.ErrorReporter.SendErrorReport(e, string.Format("魔改参数错误，{0}.{1}", source["api_filename"], parameter));
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

		// regex
		private Regex _cookieRegionRegex = new Regex(@"ckcy=\d+", RegexOptions.Compiled);
		private Regex _cookieLanguageRegex = new Regex(@"cklg=[^;]+", RegexOptions.Compiled);

		private void FiddlerApplication_BeforeRequest( Fiddler.Session oSession ) {

			Utility.Configuration.ConfigurationData.ConfigConnection c = Utility.Configuration.Config.Connection;


			// 上流プロキシ設定
			if ( c.UseUpstreamProxy ) {
				if ( !IsSessionSSL( oSession ) || ( c.EnableSslUpstreamProxy && c.UpstreamProxyPortSSL == 0) ) {
					oSession["X-OverrideGateway"] = string.Format( "{0}:{1}", c.UpstreamProxyAddress, c.UpstreamProxyPort );
				} else if ( c.EnableSslUpstreamProxy ) { 
					oSession["X-OverrideGateway"] = string.Format( "{0}:{1}", c.UpstreamProxyAddressSSL, c.UpstreamProxyPortSSL );
				}
			}

			// 修改Cookie
			if (Utility.Configuration.Config.FormBrowser.ModifyCookieRegion) {
				string cookie = oSession.oRequest["Cookie"];

				var cookieRegion = _cookieRegionRegex.Match(cookie);
				if (cookieRegion.Success) {
					cookie = cookie.Replace(cookieRegion.Value, "ckcy=1");
				}

				var cookieLanguage = _cookieLanguageRegex.Match(cookie);
				if (cookieLanguage.Success) {
					cookie = cookie.Replace(cookieLanguage.Value, "cklg=welcome");
				}
				oSession.oRequest["Cookie"] = cookie;
			}

			if ( oSession.fullUrl.Contains( "/kcsapi/" ) ) {

				string url = oSession.fullUrl;
				string body = oSession.GetRequestBodyAsString();

				// 魔改api_start2
				{
					if ( oSession.fullUrl.Contains( "/kcsapi/api_start2" )  ) {
						oSession.bBufferResponse = true;
					}
				}

				//保存
				{
					if ( c.SaveReceivedData && c.SaveRequest ) {

						Task.Factory.StartNew( (Action)( () => {
							SaveRequest( url, body );
						} ) );
					}
				}

				UIControl.BeginInvoke( (Action)( () => { LoadRequest( url, body ); } ) );
			} else if ( ObserverResult( p => {
                try {
                    return p.OnBeforeRequest( oSession );
                } catch ( Exception oe ) {
                    Logger.Add( 3, string.Format( "插件 {0}({1}) 执行 OnBeforeRequest 时出错！", p.MenuTitle, p.Version ) );
                    ErrorReporter.SendErrorReport( oe, p.MenuTitle );
                    return false;
                }
            } ) ) {

				// just skip the control flow: ObserverPlugin applied.
			}

			// use cache js
			else if ( Utility.Configuration.Config.CacheSettings.UseCacheJs && oSession.fullUrl.StartsWith( "http://203.104.209.7/" ) ) {

				string filepath = Path.Combine( Utility.Configuration.Config.CacheSettings.CacheFolder, "kcs" ) + oSession.PathAndQuery.Replace( '/', '\\' );
				if ( File.Exists( filepath ) ) {

					//返回本地文件
					oSession.utilCreateResponseAndBypassServer();
					oSession.ResponseBody = File.ReadAllBytes( filepath );
					oSession.oResponse.headers["Server"] = "nginx";
					oSession.oResponse.headers["Cache-Control"] = "max-age=18000, public";
					oSession.oResponse.headers["Date"] = GMTHelper.ToGMTString( DateTime.Now );
					oSession.oResponse.headers["Connection"] = "close";
					oSession.oResponse.headers["Accept-Ranges"] = "bytes";

					filepath = filepath.ToLower();
					if ( filepath.EndsWith( ".js" ) )
						oSession.oResponse.headers["Content-Type"] = "application/x-javascript";
				}

			}

			// flash wmode & quality
			else if ( oSession.fullUrl.Contains( "/gadget/js/kcs_flash.js" ) ) {

				oSession.bBufferResponse = true;
			}

			// block media
			else if ( Utility.Configuration.Config.Connection.BlockMedia ) {
				string file = oSession.PathAndQuery;
				int n = file.IndexOf( '?' );
				if ( n > 0 )
					file = file.Substring( 0, n );

				string ext = file.Substring( file.LastIndexOf( '.' ) + 1 ).ToLower();
				if ( ext == "jpg" || ext == "gif" || ext == "png" ) {
					// 直接返回204
					oSession.utilCreateResponseAndBypassServer();
					oSession.responseCode = 204;
				} else if ( ext == "css" ) {
					string path = Path.Combine( Utility.Configuration.Config.CacheSettings.CacheFolder, "kcs" ) + file.Replace( '/', '\\' );
					if ( File.Exists( path ) ) {
						// 返回缓存
						oSession.utilCreateResponseAndBypassServer();
						oSession.ResponseBody = File.ReadAllBytes( path );
						oSession.oResponse.headers["Server"] = "nginx";
						oSession.oResponse.headers["Cache-Control"] = "max-age=18000, public";
						oSession.oResponse.headers["Date"] = GMTHelper.ToGMTString( DateTime.Now );
						oSession.oResponse.headers["Connection"] = "close";
						oSession.oResponse.headers["Accept-Ranges"] = "bytes";
						oSession.oResponse.headers["Content-Type"] = "text/css";
					}
				}
			}

		}



		public void LoadRequest( string path, string data ) {

			string shortpath = path.Substring( path.LastIndexOf( "/kcsapi/" ) + 8 );

			try {

				Utility.Logger.Add( 1, "开始处理 Request : " + shortpath );

				SystemEvents.UpdateTimerEnabled = false;


				var parsedData = new Dictionary<string, string>();
				data = HttpUtility.UrlDecode( data );

				foreach ( string unit in data.Split( "&".ToCharArray() ) ) {
					string[] pair = unit.Split( "=".ToCharArray() );
					parsedData.Add( pair[0], pair[1] );
				}


				APIList.OnRequestReceived( shortpath, parsedData );


			} catch ( Exception ex ) {

				ErrorReporter.SendErrorReport( ex, "处理 Request 时发生错误。", shortpath, data );

			} finally {

				SystemEvents.UpdateTimerEnabled = true;

			}

		}


		public void LoadResponse( string path, string data ) {

			string shortpath = path.Substring( path.LastIndexOf( "/kcsapi/" ) + 8 );

			try {

				Utility.Logger.Add( 1, "开始处理 Response : " + shortpath );

				SystemEvents.UpdateTimerEnabled = false;


				var json = DynamicJson.Parse( data.Substring( 7 ) );		//remove "svdata="

				int result = (int)json.api_result;
				if ( result != 1 ) {

					throw new InvalidOperationException( "发现服务器猫错误。(错误代码: " + result + ")" );
				}


				if ( shortpath == "api_get_member/ship2" )
					APIList.OnResponseReceived( shortpath, json );
				else if ( json.IsDefined( "api_data" ) )
					APIList.OnResponseReceived( shortpath, json.api_data );
				else
					APIList.OnResponseReceived( shortpath, null );


			} catch ( Exception ex ) {

				ErrorReporter.SendErrorReport( ex, "处理 Response 时发生错误。", shortpath, data );

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
