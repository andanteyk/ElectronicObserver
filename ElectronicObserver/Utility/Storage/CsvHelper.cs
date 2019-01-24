using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility.Storage
{
	public static class CsvHelper
	{
		/// <summary>
		/// 雰囲気で CSV の行をパースします。
		/// </summary>
		public static IEnumerable<string> ParseCsvLine(string line)
		{
			string[] splitted = line.Split(",".ToCharArray());
			for (int i = 0; i < splitted.Length; i++)
			{
				if (splitted[i].Length == 0 || splitted[i][0] != '"')
					yield return splitted[i];
				else
				{
					if (splitted[i].Last() == '"' && splitted[i].Count(c => c == '"') % 2 == 0)
						yield return splitted[i].Substring(1, splitted[i].Length - 2).Replace("\"\"", "\"");
					else
						splitted[i + 1] = splitted[i] + "," + splitted[i + 1];
				}
			}
		}

		/// <summary>
		/// 雰囲気で CSV の文字列セルをエスケープします。
		/// </summary>
		public static string EscapeCsvCell(string cell)
		{
			if (cell == null) return "null";
			var sb = new StringBuilder();
			sb.Append("\"").Append(cell.Replace("\"", "\"\"").Replace("\r\n", "<br>")).Append("\"");
			return sb.ToString();
		}
	}
}
