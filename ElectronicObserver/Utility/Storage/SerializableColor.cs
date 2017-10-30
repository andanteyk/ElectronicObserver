using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility.Storage
{

	/// <summary>
	/// シリアル化可能な <see cref="System.Drawing.Color"/> を扱います。
	/// </summary>
	[DataContract(Name = "SerializableColor")]
	[DebuggerDisplay("{ColorData}")]
	public class SerializableColor
	{

		[IgnoreDataMember]
		public Color ColorData { get; set; }


		public SerializableColor()
		{
			ColorData = Color.Black;
		}

		public SerializableColor(Color color)
		{
			ColorData = color;
		}

		public SerializableColor(string attribute)
		{
			SerializedColor = attribute;
		}

		public SerializableColor(uint colorCode)
		{
			ColorData = UIntToColor(colorCode);
		}


		[DataMember]
		public string SerializedColor
		{
			get { return ColorToString(ColorData); }
			set { ColorData = StringToColor(value); }
		}

		[IgnoreDataMember]
		public uint ColorCode
		{
			get { return ColorToUint(ColorData); }
			set { ColorData = UIntToColor(value); }
		}


		public static implicit operator Color(SerializableColor color)
		{
			if (color == null) return UIntToColor(0);
			return color.ColorData;
		}

		public static implicit operator SerializableColor(Color color)
		{
			return new SerializableColor(color);
		}


		public static Color StringToColor(string value)
		{
			if (value == null || value == string.Empty || !uint.TryParse(value, System.Globalization.NumberStyles.HexNumber, null, out uint i))
				return UIntToColor(0);

			return UIntToColor(i);
		}

		public static Color UIntToColor(uint color)
		{
			return Color.FromArgb(
				(int)((color >> 24) & 0xFF),
				(int)((color >> 16) & 0xFF),
				(int)((color >> 8) & 0xFF),
				(int)((color >> 0) & 0xFF));
		}

		public static string ColorToString(Color color)
		{
			return ColorToUint(color).ToString("X8");
		}

		public static uint ColorToUint(Color color)
		{
			return
				((uint)(color.A) << 24) |
				((uint)(color.R) << 16) |
				((uint)(color.G) << 8) |
				((uint)(color.B) << 0);

		}
	}
}
