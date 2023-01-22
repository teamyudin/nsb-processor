using System;
using System.Threading.Tasks;
using Shared;
using NServiceBus;
using NServiceBus.Logging;

namespace Processor
{    
    public class ProcessMessageHandler:IHandleMessages<ProcessMessage>
    {
        static readonly ILog log = LogManager.GetLogger<ProcessMessageHandler>();
        static readonly Random random = new Random();

        public Task Handle(ProcessMessage message, IMessageHandlerContext context)
        {
            log.Info($"Received Process Message, MessageId = {message.MessageId}");

            // This is normally where some business logic would occur

            System.Threading.Thread.Sleep(5000);

            #region ThrowTransientException
            // Uncomment to test throwing transient exceptions
            //if (random.Next(0, 5) == 0)
            //{
            //    throw new Exception("Oops");
            //}
            #endregion

            #region ThrowFatalException
            // Uncomment to test throwing fatal exceptions
            //throw new Exception("BOOM");
            #endregion

            var messageRecieved = new MessageProcessed
            {
                MessageId = message.MessageId
            };

            log.Info($"Message Processed, MessageId = {message.MessageId}");

            return context.Publish(messageRecieved);
        }
    }
}
