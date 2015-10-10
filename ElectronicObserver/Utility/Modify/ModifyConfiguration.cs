using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility.Modify {

	public class ModifyConfiguration {

		private static ModifyConfiguration _instance;

		public static ModifyConfiguration Instance {
			get {
				if ( _instance == null ) {
					_instance = new ModifyConfiguration();
				}
				return _instance;
			}
		}

		private bool loadflag = false;

		private ModifyConfiguration() { }

		const string ModifyFileName = @"Settings\ApiModify.json";

		private List<ModifyConfigurationNode> modifies;

		public ModifyConfigurationNode this[int index] {
			get {
				if ( modifies == null || index < 0 || index >= modifies.Count ) {
					return null;
				}

				return modifies[index];
			}
		}

		public int Count {
			get { return modifies == null ? 0 : modifies.Count; }
		}

		public void LoadSettings() {

			if ( loadflag ) {
				return;
			}

			if ( File.Exists( ModifyFileName ) ) {

				modifies = new List<ModifyConfigurationNode>();

				try {

					var array = Codeplex.Data.DynamicJson.Parse( File.ReadAllText( ModifyFileName ) );

					if ( array.IsArray ) {
						foreach ( var o in array ) {
							AddNode( o );
						}
					} else {
						AddNode( array );
					}

				} catch ( Exception ex ) {
					Utility.ErrorReporter.SendErrorReport( ex, "魔改文件格式错误。" );
				}

			}

			loadflag = true;
		}

		private void AddNode( dynamic o ) {

			var modify = new ModifyConfigurationNode {
				api_filename = o.api_filename,
				api_name = o.api_name() ? o.api_name : null,
				api_parameter = o
			};

			modifies.Add( modify );
		}
	}

	public class ModifyConfigurationNode {

		private static Regex _unicodeRegex = new Regex( @"\\u([0-9a-fA-F]{4})", RegexOptions.Compiled );

		public string api_filename { get; set; }

		private string _api_name = string.Empty;
		public string api_name {
			get {
				string name = _api_name;
				if ( _api_name != null ) {
					foreach ( Match m in _unicodeRegex.Matches( _api_name ) ) {
						name = name.Replace( m.Value, ( (char)int.Parse( m.Groups[1].Value, System.Globalization.NumberStyles.HexNumber ) ).ToString() );
					}
					return name;
				}

				return string.Empty;
			}

			set {
				if ( value == null ) {
					_api_name = string.Empty;
					return;
				}

				StringBuilder sb = new StringBuilder();
				foreach ( char c in value.ToCharArray() ) {
					if ( c < 0xff ) {
						sb.Append( c );
					} else {
						sb.AppendFormat( @"\u{0:x4}", (int)c );
					}
				}
				_api_name = sb.ToString();
			}
		}

		public string Raw_api_name {
			get { return _api_name; }
		}

		public dynamic api_parameter { get; set; }
	}

	public class ModifyBinder : System.Dynamic.GetMemberBinder {

		public ModifyBinder( string name ) : base( name, false ) { }

		public override System.Dynamic.DynamicMetaObject FallbackGetMember( System.Dynamic.DynamicMetaObject target, System.Dynamic.DynamicMetaObject errorSuggestion ) {
			throw new NotImplementedException();
		}

	}

	public class ModifySetBinder : System.Dynamic.SetMemberBinder {

		public ModifySetBinder( string name ) : base( name, false ) { }

		public override System.Dynamic.DynamicMetaObject FallbackSetMember( System.Dynamic.DynamicMetaObject target, System.Dynamic.DynamicMetaObject value, System.Dynamic.DynamicMetaObject errorSuggestion ) {
			throw new NotImplementedException();
		}
	}
}
