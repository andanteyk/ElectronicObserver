using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Configuration = ElectronicObserver.Utility.Configuration;

namespace ElectronicObserver.Observer.Cache {
	enum Direction {
		Discharge_Response,		//无关请求，或需要下载文件 -> 忽略请求
		Return_LocalFile,		//存在无需验证的本地文件 -> 返回本地文件
		Verify_LocalFile,		//验证文件有效性 -> 向服务器发送验证请求
	}

	enum filetype {
		not_file,
		unknown_file,

		game_entry,		//kcs\mainD2.swf
		//kcs\Core.swf

		entry_large,	//kcs\scenes\TitleMain.swf
		//kcs\resources\swf\commonAsset.swf
		//kcs\resources\swf\font.swf
		//kcs\resources\swf\icons.swf

		port_main,		//kcs\PortMain.swf
		//kcs\resources\swf\sound_se.swf

		scenes,			//kcs\scenes\

		resources,		//kcs\resources\bgm_p\
		//kcs\resources\swf\sound_bgm.swf
		//kcs\resources\swf\sound_b_bgm_*.swf
		//kcs\resources\swf\map\
		//kcs\resources\swf\ships\

		image,			//kcs\resources\images
		sound,			//kcs\sound

		world_name,		//kcs\resources\images\world
		title_call,		//kcs\sound\titlecall
	}


	class CacheCore {

		/// <summary>
		/// 对于一个新的客户端请求，根据url，决定下一步要对请求怎样处理
		/// </summary>
		/// <param name="url">请求的url</param>
		/// <param name="result">本地文件地址 or 记录的修改日期</param>
		/// <returns>下一步我们该做什么？忽略请求；返回缓存文件；验证缓存文件</returns>
		public Direction GotNewRequest( string url, out string result ) {
			result = "";
			string filepath = "";

			Uri uri;
			try { uri = new Uri( url ); } catch ( Exception ex ) {
				System.Diagnostics.Debug.WriteLine( ex );
				return Direction.Discharge_Response;
				//url无效，忽略请求（不进行任何操作）
			}

			if ( !uri.IsFilePath() ) {
				return Direction.Discharge_Response;
				//url非文件，忽略请求
			}

			//识别文件类型
			filetype type = _RecognizeFileType( uri );
			if ( type == filetype.unknown_file ||
				type == filetype.not_file ||
				type == filetype.game_entry ) {
				return Direction.Discharge_Response;
				//无效的文件，忽略请求
			}

			//检查Title Call与World Name的特殊地址
			if ( Configuration.Config.CacheSettings.HackTitleEnabled ) {
				if ( type == filetype.title_call ) {
					filepath = uri.AbsolutePath.Replace( '/', '\\' );
					filepath = filepath.Remove( filepath.LastIndexOf( '\\' ) ) + ".mp3";
					filepath = Configuration.Config.CacheSettings.CacheFolder + filepath;
					result = filepath;

					if ( File.Exists( filepath ) )
						return Direction.Return_LocalFile;
				} else if ( type == filetype.world_name ) {
					filepath = Configuration.Config.CacheSettings.CacheFolder + @"\kcs\resources\image\world.png";
					result = filepath;

					if ( File.Exists( filepath ) )
						return Direction.Return_LocalFile;
				}
			}

			//检查一般文件地址
			if ( ( type == filetype.resources && Configuration.Config.CacheSettings.CacheResourceFiles > 0 ) ||
				( type == filetype.entry_large && Configuration.Config.CacheSettings.CacheEntryFiles > 0 ) ||
				( type == filetype.port_main && Configuration.Config.CacheSettings.CachePortFiles > 0 ) ||
				( type == filetype.scenes && Configuration.Config.CacheSettings.CacheSceneFiles > 0 ) ||
				( type == filetype.sound && Configuration.Config.CacheSettings.CacheSoundFiles > 0 ) ||
				( ( type == filetype.title_call ||
				  type == filetype.world_name ||
				  type == filetype.image ) && Configuration.Config.CacheSettings.CacheResourceFiles > 0 ) ) {
				filepath = Configuration.Config.CacheSettings.CacheFolder + uri.AbsolutePath.Replace( '/', '\\' );

				//检查Hack文件地址
				if ( Configuration.Config.CacheSettings.HackEnabled ) {
					var fnext = uri.Segments.Last().Split( '.' );
					string hfilepath = filepath.Replace( uri.Segments.Last(), fnext[0] + ".hack." + fnext.Last() );

					if ( File.Exists( hfilepath ) ) {
						result = hfilepath;
						return Direction.Return_LocalFile;
						//存在hack文件，则返回本地文件
					}

				}

				//检查缓存文件
				if ( File.Exists( filepath ) ) {
					//存在本地缓存文件 -> 检查文件的最后修改时间
					//（验证所有文件 或 只验证非资源文件）
					if ( Configuration.Config.CacheSettings.CheckFiles > 1 || ( Configuration.Config.CacheSettings.CheckFiles > 0 && type != filetype.resources ) ) {
						//只有swf文件需要验证时间
						if ( filepath.EndsWith( ".swf" ) ) {
							//文件存在且需要验证时间
							//-> 请求服务器验证修改时间（记录读取和保存的位置）
							result = filepath;
							_RecordTask( url, filepath );
							return Direction.Verify_LocalFile;
						}
					}
					//文件不需验证
					//->返回本地缓存文件
					result = filepath;
					return Direction.Return_LocalFile;

				} else {
					//缓存文件不存在
					//-> 下载文件 （记录保存地址）
					_RecordTask( url, filepath );
					return Direction.Discharge_Response;
				}
			}

			//文件类型对应的缓存设置没有开启
			//-> 当做文件不存在
			return Direction.Discharge_Response;
		}

		filetype _RecognizeFileType( Uri uri ) {
			if ( !uri.IsFilePath() )
				return filetype.not_file;

			var seg = uri.Segments;

			if ( seg[1] != "kcs/" ) {
				return filetype.not_file;
			} else {

				if ( seg[2] == "resources/" ) {
					if ( seg[3] == "swf/" ) {
						if ( seg[4] == "commonAssets.swf" ||
							seg[4] == "font.swf" ||
							seg[4] == "icons.swf" ) {
							return filetype.entry_large;
						} else if ( seg[4] == ( "sound_se.swf" ) ) {
							return filetype.port_main;
						}
					} else if ( seg[3] == "image/" ) {
						if ( seg[4] == "world/" ) {
							return filetype.world_name;
						}

						return filetype.image;
					}
					return filetype.resources;
				} else if ( seg[2] == "scenes/" ) {
					if ( seg[3] == "TitleMain.swf" ) {
						return filetype.entry_large;
					}

					return filetype.scenes;
				} else if ( seg[2] == "sound/" ) {
					if ( seg[3] == "titlecall/" ) {
						return filetype.title_call;
					}

					return filetype.sound;
				} else {
					if ( seg[2] == "Core.swf" ||
						seg[2] == "mainD2.swf" ) {
						return filetype.game_entry;
						//  kcs/mainD2.swf; kcs/Core.swf;
					} else if ( seg[2] == "PortMain.swf" ) {
						return filetype.port_main;
						//  kcs/PortMain.swf;

					}
				}

				//Debug.WriteLine("CACHR> _RecogniseFileType检查到无法识别的文件");
				//Debug.WriteLine("		"+uri.AbsolutePath);
				return filetype.unknown_file;
			}

		}

		void _RecordTask( string url, string filepath ) {
			TaskRecord.Add( url, filepath );
		}
	}
}
