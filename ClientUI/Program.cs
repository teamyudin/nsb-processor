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

                            var transport = new AzureServiceBusTransport("Endpoint=sb://nsb-processor.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=dHU0ZHC+WxQ7e7PEdTpi+8dEG6k2meP23V7UxyyPf4E=");
                            var routing = endpointConfiguration.UseTransport(transport);

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
