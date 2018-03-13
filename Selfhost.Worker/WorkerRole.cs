using Microsoft.Owin.Hosting;
using Microsoft.WindowsAzure.ServiceRuntime;
using Owin;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Selfhost.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private IDisposable _webHost;

        public override void Run()
        {
            Trace.TraceInformation("Selfhost.Worker is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            _webHost = StartWebHost();

            bool result = base.OnStart();

            Trace.TraceInformation("Selfhost.Worker has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("Selfhost.Worker is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            _webHost.Dispose();

            base.OnStop();

            Trace.TraceInformation("Selfhost.Worker has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                //Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }

        private IDisposable StartWebHost()
        {
            StartOptions options = new StartOptions();

            foreach (RoleInstanceEndpoint ep in RoleEnvironment.CurrentRoleInstance.InstanceEndpoints.Values)
            {
                options.Urls.Add($"{ep.Protocol}://{ep.IPEndpoint}");
            }

            return WebApp.Start(options, OwinStartup);
        }

        private void OwinStartup(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            config.EnsureInitialized();

            app.UseWebApi(config);
        }
    }
}
