using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using DigisatServer.Infrastructure.Crosscutting.Function;
using DigisatServer.Infrastructure.Crosscutting.Model;

namespace DigisatServer.Distributed.Wcf.Main.Clients
{
	// Token: 0x02000034 RID: 52
	public class ChannelFactoryBasico
	{
		// Token: 0x060000ED RID: 237 RVA: 0x000057DC File Offset: 0x000039DC
		public ChannelFactoryBasico(EndPointWcf endPoint)
		{
			this._endPoint = endPoint;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000057F8 File Offset: 0x000039F8
		public TContract CriarCanal<TContract>()
		{
			object sync = this._sync;
			TContract tcontract;
			lock (sync)
			{
				GerarLog gerarLog = new GerarLog();
				string name = typeof(TContract).Name;
				BasicHttpBinding basicHttpBinding2;
				if (!name.ToLower().Contains("informacao") && !name.ToLower().Contains("pdvoffline"))
				{
					BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
					basicHttpBinding.UseDefaultWebProxy = false;
					basicHttpBinding2 = basicHttpBinding;
					basicHttpBinding.MaxReceivedMessageSize = 2147483647L;
				}
				else
				{
					BasicHttpBinding basicHttpBinding3 = new BasicHttpBinding();
					basicHttpBinding3.UseDefaultWebProxy = false;
					basicHttpBinding3.MaxReceivedMessageSize = 2147483647L;
					basicHttpBinding3.OpenTimeout = TimeSpan.FromSeconds(5.0);
					basicHttpBinding3.ReceiveTimeout = TimeSpan.FromSeconds(5.0);
					basicHttpBinding3.CloseTimeout = TimeSpan.FromSeconds(5.0);
					basicHttpBinding2 = basicHttpBinding3;
					basicHttpBinding3.SendTimeout = TimeSpan.FromSeconds(5.0);
				}
				BasicHttpBinding basicHttpBinding4 = basicHttpBinding2;
				gerarLog.GravarLog(string.Format("Contract: {0} CloseTimeout: {1} OpenTimeout: {2} SendTimeout: {3} ReceiveTimeout: {4}", new object[] { name, basicHttpBinding4.CloseTimeout, basicHttpBinding4.OpenTimeout, basicHttpBinding4.SendTimeout, basicHttpBinding4.ReceiveTimeout }));
				tcontract = new ChannelFactory<TContract>(basicHttpBinding4).CreateChannel(new EndpointAddress(new Uri(string.Format("http://{0}/{1}", this._endPoint, typeof(TContract).Name)), Array.Empty<AddressHeader>()));
			}
			return tcontract;
		}

		// Token: 0x0400004F RID: 79
		private readonly EndPointWcf _endPoint;

		// Token: 0x04000050 RID: 80
		private readonly object _sync = new object();
	}
}
