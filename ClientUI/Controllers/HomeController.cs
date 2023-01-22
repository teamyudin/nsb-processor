using System;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

namespace ClientUI.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        static int messagesSent;
        private readonly ILogger<HomeController> _log;
        private readonly IMessageSession _messageSession;

        public HomeController(IMessageSession messageSession, ILogger<HomeController> logger)
        {
            _messageSession = messageSession;
            _log = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PlaceOrder()
        {
            string orderId = Guid.NewGuid().ToString().Substring(0, 8);

            var command = new ProcessMessage { MessageId = orderId };

            var options = new SendOptions();
            options.SetDestination("Processor");

            // Send the command
            await _messageSession.Send(command, options)
                .ConfigureAwait(false);

            _log.LogInformation($"Sending RequestMessage, MessageId = {orderId}");

            dynamic model = new ExpandoObject();
            model.OrderId = orderId;
            model.MessagesSent = Interlocked.Increment(ref messagesSent);

            return View(model);
        }
    }
}
