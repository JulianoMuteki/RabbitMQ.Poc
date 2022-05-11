using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerService.RabbitMQ.Publish
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IMessageBroker _messageBroker;

        public Worker(ILogger<Worker> logger, IMessageBroker manager)
        {
            _messageBroker = manager;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                // publish message  
                _messageBroker.Publish(new
                {
                    field1 = $"Hello-{DateTimeOffset.Now}",
                    field2 = $"rabbit-{DateTimeOffset.Now}"
                }, "demo.exchange.topic.dotnetcore", "topic", "*.queue.durable.dotnetcore.#");


                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}
