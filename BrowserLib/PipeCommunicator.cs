using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BrowserLib
{

	/// <summary>
	/// WCFのNetNamedPipeBindingを使ったサーバ・クライアント
	/// </summary>
	public class PipeCommunicator<ClientType> where ClientType : class
	{
		private ServiceHost Server;
		private string ServerUrl;
		private ChannelFactory<ClientType> PipeFactory;
		private NetNamedPipeBinding Binding = new NetNamedPipeBinding();

		/// <summary>
		/// サーバ側オブジェクトへの通信インターフェース
		/// 通信エラーが発生するとnullになることがあるので注意
		/// </summary>
		public ClientType Proxy { get; private set; }

		/// <summary>
		/// エラーハンドラ
		/// </summary>
		public delegate void FaultedDelegate(Exception e);
		public event FaultedDelegate Faulted = delegate { };

		/// <summary>
		/// Closeされたか
		/// </summary>
		public bool Closed { get; private set; }

		/// <summary>
		/// サーバ起動もする
		/// </summary>
		public PipeCommunicator(object instance, Type type, string listenUrl, string serviceAddress)
		{
			Server = new ServiceHost(instance, new Uri[] { new Uri(listenUrl) });
			Binding.ReceiveTimeout = TimeSpan.MaxValue;
			Server.AddServiceEndpoint(type, Binding, serviceAddress);
			Server.Open();
		}

		/// <summary>
		/// サーバに接続
		/// </summary>
		public void Connect(string to)
		{
			if (Proxy == null)
			{
				ServerUrl = to;
				if (PipeFactory == null)
				{
					PipeFactory = new ChannelFactory<ClientType>(Binding, new EndpointAddress(ServerUrl));
				}
				Proxy = PipeFactory.CreateChannel();
				Closed = false;
			}
		}

		/// <summary>
		/// 閉じる
		/// </summary>
		public void Close()
		{
			if (!Closed)
			{
				if (Proxy != null)
				{
					((IClientChannel)Proxy).Abort();
					Proxy = null;
				}
				Server.Close();
				Closed = true;
			}
		}

		/// <summary>
		/// 閉じる(非同期)
		/// </summary>
		public async Task CloseAsync(object instance)
		{
			if (!Closed)
			{
				if (Proxy != null)
				{
					((IClientChannel)Proxy).Abort();
					Proxy = null;
				}
				await Task.Factory.FromAsync(Server.BeginClose(_ => { }, instance), _ => { });
				
				Closed = true;
			}
		}

		/// <summary>
		/// 非同期でactionを実行。例外が発生したらFaultイベントが発生するので、例外が出ることはない
		/// </summary>
		public async void AsyncRemoteRun(Action action)
		{
			try
			{
				// リトライループ
				for (int i = 0; i < 2; ++i)
				{
					try
					{
						if (Proxy == null) return;
						await Task.Run(action);
						return;
					}
					catch (CommunicationException cex)
					{
						((IClientChannel)Proxy)?.Abort();
						Proxy = null;
						if (i >= 1)
						{
							// これ以上リトライしない
							Faulted(cex);
							break;
						}
						Connect(ServerUrl);
					}
				}
			}
			catch (Exception ex)
			{
				Faulted(ex);
			}
		}
	}

}
