using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Utilities.AppSettings
{
    public static class AppSettingsFactory
    {
        public static T GetSettings<T>()
        {
            var consfiguration = GetConfiguration();
            var result = consfiguration.GetSection(typeof(T).Name).Get<T>();
            return result;
        }

        private static IConfigurationRoot GetConfiguration()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                  .AddJsonFile($"appsettings.json", true, true)
                  .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                  .AddUserSecrets(Assembly.GetEntryAssembly())
                  .AddEnvironmentVariables();

            var configuration = builder.Build();

            return configuration;
        }
    }
}