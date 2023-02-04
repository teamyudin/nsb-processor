using NServiceBus.Logging;
using NServiceBus;
using Shared;
using System.Threading.Tasks;

namespace ClientUI.Handlers
{
    public class MessageProcessedHandler :
        IHandleMessages<MessageProcessed>
    {
        static readonly ILog log = LogManager.GetLogger<MessageProcessedHandler>();

        public Task Handle(MessageProcessed message, IMessageHandlerContext context)
        {
            log.Info($"Client has received Message Processed Event, MessageId = {message.MessageId}");
            return Task.CompletedTask;
        }
    }
}
