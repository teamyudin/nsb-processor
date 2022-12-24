using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Shared;

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
            return Host.CreateDefaultBuilder(args)
                        .UseNServiceBus(context =>
                        {
                            var endpointConfiguration = new EndpointConfiguration("ClientUI");
                            var routing = endpointConfiguration.UseTransport(new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Classic), "host=rabbitmq"));

                            routing.RouteToEndpoint(typeof(ProcessMessage), "Processor");

                            endpointConfiguration.SendFailedMessagesTo("error");
                            endpointConfiguration.AuditProcessedMessagesTo("audit");

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
