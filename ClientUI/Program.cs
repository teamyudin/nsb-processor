using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Shared;
using Utilities.AppSettings;

namespace ClientUI
{   
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "ClientUI";
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var nsbConfig = AppSettingsFactory.GetSettings<NServiceBusConfig>();
            var nsbConnectionString = AzureServiceBusConnectionStringFactory.Build(nsbConfig);

            return Host.CreateDefaultBuilder(args)
                        .UseNServiceBus(context =>
                        {
                            var endpointConfiguration = new EndpointConfiguration(nsbConfig.EndpointName);

                            var transport = new AzureServiceBusTransport(nsbConnectionString);
                            var routing = endpointConfiguration.UseTransport(transport);

                            endpointConfiguration.SendFailedMessagesTo(nsbConfig.EndpointName);
                            endpointConfiguration.AuditProcessedMessagesTo(nsbConfig.AuditEndpointName);

                            endpointConfiguration.EnableInstallers();
                            endpointConfiguration.DefineCriticalErrorAction(CriticalErrorActions.RestartContainer);

                            return endpointConfiguration;

                        })
                       .ConfigureWebHostDefaults(webBuilder =>
                        {
                            webBuilder.UseStartup<Startup>();
                        });
        }
    }
}
