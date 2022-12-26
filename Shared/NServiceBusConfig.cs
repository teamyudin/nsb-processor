namespace Shared
{
    public class NServiceBusConfig
    {
        public string SharedAccessKey { get; set; }
        public string SharedAccessKeyName { get; set; }
        public string Endpoint { get; set; }
        public string EndpointName { get; set; }
        public string ErrorEndpointName { get; set; }
        public string AuditEndpointName { get; set; }
    }
}
