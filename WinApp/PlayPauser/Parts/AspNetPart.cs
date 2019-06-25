using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PlayPauser.Parts.AspNet;
using System.Threading;
using System.Threading.Tasks;

namespace PlayPauser.Parts
{
    public class AspNetPart : IPart
    {
        private readonly EventAggregator eventAggregator;
        private Task serverRunning;
        private CancellationTokenSource cancellationTokenSource;
        private IWebHost webHost;

        public AspNetPart(EventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public void Start(Options options)
        {
            cancellationTokenSource = new CancellationTokenSource();
            webHost = new WebHostBuilder()
                .UseKestrel(o =>
                {
                    o.ListenAnyIP(options.ServerPort);
                })
                .ConfigureServices(serviceCollection =>
                {
                    serviceCollection.AddSingleton(eventAggregator);
                    serviceCollection.AddSingleton(options);
                })
                .UseStartup<AspNetStartup>()
                .Build();
            serverRunning = webHost.StartAsync(cancellationTokenSource.Token);
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
            serverRunning.Wait();
            webHost.Dispose();
            webHost = null;
        }
    }
}
