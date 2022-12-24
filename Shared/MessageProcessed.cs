using NServiceBus;

namespace Shared
{
    public class MessageProcessed : IEvent
    {
        public string MessageId { get; set; }
    }
}