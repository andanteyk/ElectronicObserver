using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace ElectronicObserver.Utility.Modify {

	public class ModifyConfiguration {

		private static ModifyConfiguration _instance;
        private JavaScriptSerializer JavaScriptSerializer;

        public ModifyConfigurationNode GetModifyNode(string filename)
        {
            return modifies.FirstOrDefault(e => e.api_filename == filename);
        }

        public static ModifyConfiguration Instance {
			get {
				if ( _instance == null ) {
					_instance = new ModifyConfiguration();
				}
				return _instance;
			}
		}

		private bool loadflag = false;

		private ModifyConfiguration() { JavaScriptSerializer = new JavaScriptSerializer(); }

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

					var array = JavaScriptSerializer.DeserializeObject( File.ReadAllText( ModifyFileName ) );

					if ( array is object[] ) {
						foreach ( var o in array as object[]) {
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

		private void AddNode( object o ) {

            Dictionary<string, object> parameter = o as Dictionary<string, object>;

            var modify = new ModifyConfigurationNode {
                api_filename = parameter["api_filename"].ToString(),
                api_name = parameter.ContainsKey("api_name") ? parameter["api_name"].ToString() : null,
				api_parameter = parameter
            };

			modifies.Add( modify );
		}
	}

	public class ModifyConfigurationNode {

		private static Regex _unicodeRegex = new Regex( @"\\u([0-9a-fA-F]{4})", RegexOptions.Compiled );

		public string api_filename { get; set; }

		protected string _api_name = string.Empty;
		public string api_name {
			get
            {
                return Convert2String(_api_name);
            }

			set {
                _api_name = Convert2uCode(value);
            }
		}

		public string Raw_api_name {
			get { return _api_name; }
		}

		public Dictionary<string, object> api_parameter { get; set; }

        protected static string Convert2String(string str)
        {
            string name = str;
            if (name != null)
            {
                foreach (Match m in _unicodeRegex.Matches(name))
                {
                    name = name.Replace(m.Value, ((char)int.Parse(m.Groups[1].Value, System.Globalization.NumberStyles.HexNumber)).ToString());
                }

                return name;
            }
            return null;
        }

        protected static string Convert2uCode(string str)
        {
            if (str == null)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (char c in str.ToCharArray())
            {
                if (c < 0xff)
                {
                    sb.Append(c);
                }
                else
                {
                    sb.AppendFormat(@"\u{0:x4}", (int)c);
                }
            }
           return sb.ToString();
        }
	}

    public class ModifyConfigurationIniNode : ModifyConfigurationNode
    {
        protected string _api_getmes = string.Empty;
        public string api_getmes
        {
            get
            {
                return Convert2String(_api_getmes);
            }

            set
            {
                _api_getmes = Convert2uCode(value);
            }
        }

        protected string _api_info = string.Empty;
        public string api_info
        {
            get
            {
                return Convert2String(_api_info);
            }

            set
            {
                _api_info = Convert2uCode(value);
            }
        }
        public Dictionary<string, string> api_config_parameter { get; set; }

        public string Get(string key)
        {
            if (api_config_parameter.ContainsKey(key))
                return api_config_parameter[key];
            else
                return null;
        }
    }
}
