using NServiceBus;

namespace Shared
{
    public class ProcessMessage : ICommand
    {
        public string MessageId { get; set; }
    }
}