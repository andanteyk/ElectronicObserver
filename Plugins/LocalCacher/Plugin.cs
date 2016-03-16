using ElectronicObserver.Observer.Cache;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalCacher {

	public class Plugin : ObserverPlugin {

		private CacheCore Cache;

		public Plugin() {

			Cache = new CacheCore();
		}

		public override string MenuTitle { get { return "本地缓存"; } }

		public override string Version { get { return "1.0.0"; } }

		public override PluginSettingControl GetSettings() {

			return new Settings();
		}


        public override bool OnBeforeRequest( Fiddler.Session oSession ) {

            if ( Configuration.Config.CacheSettings.CacheEnabled && oSession.fullUrl.Contains( "/kcs/" ) ) {

                // = KanColleCacher =
                string filepath;
                var direction = Cache.GotNewRequest( oSession.fullUrl, out filepath );

                if ( direction == Direction.Return_LocalFile
                    || direction == Direction.NoCache_LocalFile ) {

                    //返回本地文件
                    oSession.utilCreateResponseAndBypassServer();
                    oSession.oResponse.headers["Server"] = "nginx";
                    oSession.oResponse.headers["Date"] = GMTHelper.ToGMTString( DateTime.Now );

                    filepath = filepath.ToLower();
                    if ( filepath.EndsWith( ".swf" ) )
                        oSession.oResponse.headers["Content-Type"] = "application/x-shockwave-flash";
                    else if ( filepath.EndsWith( ".mp3" ) )
                        oSession.oResponse.headers["Content-Type"] = "audio/mpeg";
                    else if ( filepath.EndsWith( ".png" ) )
                        oSession.oResponse.headers["Content-Type"] = "image/png";

                    oSession.oResponse.headers["Last-Modified"] = _GetModifiedTime( filepath );
                    oSession.oResponse.headers["Connection"] = "close";
                    if ( direction == Direction.NoCache_LocalFile ) {
                        oSession.oResponse.headers["Pragma"] = "no-cache";
                        oSession.oResponse.headers["Cache-Control"] = "no-cache";
                    } else {
                        oSession.oResponse.headers["Pragma"] = "public";
                        oSession.oResponse.headers["Cache-Control"] = "max-age=18000, public";
                    }
                    oSession.oResponse.headers["Accept-Ranges"] = "bytes";

                    oSession.ResponseBody = File.ReadAllBytes( filepath );

                    //Debug.WriteLine("CACHR> 【返回本地】" + result);

                } else if ( direction == Direction.Verify_LocalFile ) {

                    //请求服务器验证文件
                    oSession.oRequest.headers["If-Modified-Since"] = _GetModifiedTime( filepath );
                    oSession.bBufferResponse = true;

                    //Debug.WriteLine("CACHR> 【验证文件】" + oSession.PathAndQuery);

                } else if ( Configuration.Config.Log.ShowCacheLog && ( Configuration.Config.Log.ShowMainD2Link || !oSession.fullUrl.Contains( "mainD2.swf" ) ) ) {

                    //下载文件
                    ElectronicObserver.Utility.Logger.Add( 2, string.Format( "重新下载缓存文件: {0}", oSession.fullUrl ) );
                }


                return true;
            }
            return false;
        }

        public override bool OnBeforeResponse( Fiddler.Session oSession ) {

            if ( Configuration.Config.CacheSettings.CacheEnabled && oSession.PathAndQuery.StartsWith( "/kcs/" ) ) {

                if ( oSession.responseCode == 304 ) {

                    string filepath = TaskRecord.GetAndRemove( oSession.fullUrl );
                    //只有TaskRecord中有记录的文件才是验证的文件，才需要修改Header
                    if ( !string.IsNullOrEmpty( filepath ) ) {

                        //服务器返回304，文件没有修改 -> 返回本地文件
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

                    return true;
                } else if ( oSession.responseCode == 200 ) {

                    // 由服务器下载所得
                    if ( oSession.PathAndQuery.StartsWith( "/kcs/sound" ) && oSession.PathAndQuery.IndexOf( "titlecall/" ) < 0 ) {

                        oSession.oResponse.headers["Pragma"] = "no-cache";
                        oSession.oResponse.headers["Cache-Control"] = "no-cache";
                    }
                    return true;
                }

            }

            return false;
        }

		public override bool OnAfterSessionComplete( Fiddler.Session oSession ) {

			if ( Configuration.Config.CacheSettings.CacheEnabled && oSession.responseCode == 200 ) {

				string filepath = TaskRecord.GetAndRemove( oSession.fullUrl );
				if ( !string.IsNullOrEmpty( filepath ) ) {
					if ( File.Exists( filepath ) )
						File.Delete( filepath );

					//保存下载文件并记录Modified-Time
					try {

						if ( Configuration.Config.Log.ShowCacheLog ) {

							ElectronicObserver.Utility.Logger.Add( 2, string.Format( "更新缓存文件： {0}.", filepath ) );
						}

						oSession.SaveResponseBody( filepath );
						_SaveModifiedTime( filepath, oSession.oResponse.headers["Last-Modified"] );
						//Debug.WriteLine("CACHR> 【下载文件】" + oSession.PathAndQuery);
					} catch ( Exception ex ) {
						ElectronicObserver.Utility.ErrorReporter.SendErrorReport( ex, "会话结束时，保存返回文件时发生异常：" + oSession.fullUrl );
					}
				}

				return true;
			}

			return false;
		}


		private string _GetModifiedTime( string filepath ) {
			FileInfo fi;
			DateTime dt = default( DateTime );
			try {
				fi = new FileInfo( filepath );
				dt = fi.LastWriteTime;
				return GMTHelper.ToGMTString( dt );
			} catch ( Exception ex ) {
				ElectronicObserver.Utility.ErrorReporter.SendErrorReport( ex, "在读取文件修改时间时发生异常：" + dt );
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
				ElectronicObserver.Utility.ErrorReporter.SendErrorReport( ex, string.Format( "在保存文件修改时间时发生异常。filepath: {0}, gmTime: {1}", filepath, gmTime ) );
			}
		}

	}
}
