namespace Shared
{
    public static class AzureServiceBusConnectionStringFactory
    {
        public static string Build(NServiceBusConfig nsbConfig)
        {
            var result = $"Endpoint={nsbConfig.Endpoint};SharedAccessKeyName={nsbConfig.SharedAccessKeyName};SharedAccessKey={nsbConfig.SharedAccessKey}";
            return result;
        }
    }
}
