using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ElectronicObserver.Utility.Storage
{

	/// <summary>
	/// シリアル化可能な Font を扱います。
	/// </summary>
	public class SerializableFont
	{


		[IgnoreDataMember]
		public Font FontData { get; set; }


		public SerializableFont()
		{
			FontData = null;
		}

		public SerializableFont(Font font)
		{
			FontData = font;
		}

		public SerializableFont(string attribute)
		{
			SerializeFontAttribute = attribute;
		}


		[DataMember]
		public string SerializeFontAttribute
		{
			get { return FontToString(FontData); }
			set { FontData = StringToFont(value); }
		}


		public static implicit operator Font(SerializableFont value)
		{
			if (value == null) return null;
			return value.FontData;
		}

		public static implicit operator SerializableFont(Font value)
		{
			return new SerializableFont(value);
		}


		public static Font StringToFont(string value, bool suppressError = false)
		{
			try
			{
				return (Font)TypeDescriptor.GetConverter(typeof(Font)).ConvertFromString(value);

			}
			catch (Exception ex)
			{
				if (!suppressError)
					Utility.ErrorReporter.SendErrorReport(ex, "SerializableFont.StringToFont failed");
				return null;
			}
		}

		public static string FontToString(Font value, bool suppressError = false)
		{
			try
			{
				if (value != null)
				{
					return TypeDescriptor.GetConverter(typeof(Font)).ConvertToString(value);
				}
			}
			catch (Exception ex)
			{
				if (!suppressError)
					Utility.ErrorReporter.SendErrorReport(ex, "SerializableFont.FontToString failed");
			}

			return null;
		}


	}

}
