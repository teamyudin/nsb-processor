using System;
using System.Threading.Tasks;
using Shared;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Microsoft.Extensions.Configuration;
using Utilities.AppSettings;
using System.Reflection;

namespace Processor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Processor";
            await CreateHostBuilder(args).RunConsoleAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var nsbConfig = AppSettingsFactory.GetSettings<NServiceBusConfig>();
            var nsbConnectionString = AzureServiceBusConnectionStringFactory.Build(nsbConfig);

            return Host.CreateDefaultBuilder(args)
                       .UseNServiceBus(context =>
                       {
                           var endpointConfiguration = new EndpointConfiguration(nsbConfig.EndpointName);
                           endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
                           var transport = new AzureServiceBusTransport(nsbConnectionString);
                           var routing = endpointConfiguration.UseTransport(transport);

                           endpointConfiguration.SendFailedMessagesTo(nsbConfig.ErrorEndpointName);
                           endpointConfiguration.AuditProcessedMessagesTo(nsbConfig.AuditEndpointName);

                           // So that when we test recoverability, we don't have to wait so long
                           // for the failed message to be sent to the error queue
                           var recoverablility = endpointConfiguration.Recoverability();
                           recoverablility.Delayed(
                               delayed =>
                               {
                                   delayed.TimeIncrease(TimeSpan.FromSeconds(2));
                               }
                           );

                           endpointConfiguration.EnableInstallers();
                           endpointConfiguration.DefineCriticalErrorAction(CriticalErrorActions.RestartContainer);

                           return endpointConfiguration;
                       });
        }
    }
}
