using System;
using System.Threading.Tasks;
using Shared;
using Microsoft.Extensions.Hosting;
using NServiceBus;

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
            return Host.CreateDefaultBuilder(args)
                       .UseNServiceBus(context =>
                       {
                           var endpointConfiguration = new EndpointConfiguration("Processor");

                           endpointConfiguration.UseTransport(new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Classic), "host=rabbitmq"));

                           endpointConfiguration.SendFailedMessagesTo("error");
                           endpointConfiguration.AuditProcessedMessagesTo("audit");

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
