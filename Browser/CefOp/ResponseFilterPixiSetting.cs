using CefSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Browser.CefOp
{
	/// <summary>
	/// スクリーンショット撮影に必要なデータを挿入します。
	/// </summary>
	public class ResponseFilterPixiSetting : IResponseFilter
	{
		public bool InitFilter() => true;

		public FilterStatus Filter(Stream dataIn, out long dataInRead, Stream dataOut, out long dataOutWritten)
		{
			if (dataIn == null)
			{
				dataInRead = 0;
				dataOutWritten = 0;
				return FilterStatus.Done;
			}

			using (var reader = new StreamReader(dataIn))
			{
				string raw = reader.ReadToEnd();

				// note: preserveDrawingBuffer = true 設定時に動作が重くなる可能性がある
				// が、 false だとスクリーンショットがハードコピー(Graphics.CopyFromScreen 等)でしか取れなくなる
				// 描画直後に保存処理(toDataUrl)を行うと false でも撮れるらしいが、外部からの操作でそれができるかは不明
				string replaced = raw.Replace(
					@"/pixi.js""></script>",
					@"/pixi.js""></script><script>PIXI.settings.RENDER_OPTIONS.preserveDrawingBuffer=true;</script>");

				var bytes = Encoding.UTF8.GetBytes(replaced);
				dataOut.Write(bytes, 0, bytes.Length);

				dataInRead = dataIn.Length;
				dataOutWritten = Math.Min(bytes.Length, dataOut.Length);
			}

			return FilterStatus.Done;
		}

		public void Dispose() { }
	}
}
