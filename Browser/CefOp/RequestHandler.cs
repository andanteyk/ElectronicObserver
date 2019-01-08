using CefSharp;
using CefSharp.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Browser.CefOp
{
	public class RequestHandler : DefaultRequestHandler
	{
		public delegate void RenderProcessTerminatedEventHandler(string message);
		public event RenderProcessTerminatedEventHandler RenderProcessTerminated;

		bool pixiSettingEnabled;


		public RequestHandler(bool pixiSettingEnabled) : base()
		{
			this.pixiSettingEnabled = pixiSettingEnabled;
		}

		/// <summary>
		/// レスポンスの置換制御を行います。
		/// </summary>
		public override IResponseFilter GetResourceResponseFilter(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
		{
			if (pixiSettingEnabled && request.Url.Contains(@"/kcs2/index.php"))
				return new ResponseFilterPixiSetting();

			return base.GetResourceResponseFilter(browserControl, browser, frame, request, response);
		}

		/// <summary>
		/// 特定の通信をブロックします。
		/// </summary>
		public override CefReturnValue OnBeforeResourceLoad(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
		{
			// ログイン直後に勝手に遷移させられ、ブラウザがホワイトアウトすることがあるためブロックする
			if (request.Url.Contains(@"/rt.gsspat.jp/"))
			{
				return CefReturnValue.Cancel;
			}

			return base.OnBeforeResourceLoad(browserControl, browser, frame, request, callback);
		}

		/// <summary>
		/// 戻る/進む操作をブロックします。
		/// </summary>
		public override bool OnBeforeBrowse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
		{
			if ((request.TransitionType & TransitionType.ForwardBack) != 0)
			{
				return true;
			}
			return base.OnBeforeBrowse(browserControl, browser, frame, request, userGesture, isRedirect);
		}

		/// <summary>
		/// 描画プロセスが何らかの理由で落ちた際の処理を行います。
		/// </summary>
		public override void OnRenderProcessTerminated(IWebBrowser browserControl, IBrowser browser, CefTerminationStatus status)
		{
			// note: out of memory (例外コード: 0xe0000008) でクラッシュした場合、このイベントは呼ばれない

			string ret = "ブラウザの描画プロセスが";
			switch (status)
			{
				case CefTerminationStatus.AbnormalTermination:
					ret += "正常に終了しませんでした。";
					break;
				case CefTerminationStatus.ProcessWasKilled:
					ret += "何者かによって殺害されました。";
					break;
				case CefTerminationStatus.ProcessCrashed:
					ret += "クラッシュしました。";
					break;
				default:
					ret += "謎の死を遂げました。";
					break;
			}
			ret += "再読み込みすると復帰します。";

			RenderProcessTerminated(ret);
		}
	}
}
