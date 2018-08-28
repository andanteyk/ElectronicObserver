using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Browser.CefOp
{
	public class ScreenShotPacket
	{
		public string ID { get; }
		public string DataUrl;
		private TaskCompletionSource<Bitmap> taskSource;

		public ScreenShotPacket(TaskCompletionSource<Bitmap> source) : this("ss_" + Guid.NewGuid().ToString("N"), source) { }
		public ScreenShotPacket(string id, TaskCompletionSource<Bitmap> source)
		{
			ID = id;
			taskSource = source;
		}

		public void Complete(string dataurl)
		{
			DataUrl = dataurl;
			taskSource.SetResult(ConvertToImage(dataurl));
		}

		public static Bitmap ConvertToImage(string dataurl)
		{
			if (dataurl == null || !dataurl.StartsWith("data:image/png"))
				return null;

			var s = dataurl.Substring(dataurl.IndexOf(',') + 1);
			var bytes = Convert.FromBase64String(s);

			Bitmap bitmap;
			using (var ms = new MemoryStream(bytes))
				bitmap = new Bitmap(ms);

			return bitmap;
		}
	}
}
