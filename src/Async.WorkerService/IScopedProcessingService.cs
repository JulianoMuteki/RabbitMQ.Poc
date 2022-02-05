using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Async.WorkerService
{
    internal interface IScopedProcessingService
    {
        Task DoService(CancellationToken stoppingToken, string key);
    }

    internal class ScopedProcessingService : IScopedProcessingService
    {
        private int executionCount = 0;
        private readonly ILogger _logger;

        public ScopedProcessingService(ILogger<ScopedProcessingService> logger)
        {
            _logger = logger;
        }

        public async Task DoService(CancellationToken stoppingToken, string key)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                executionCount++;

                _logger.LogInformation(
                    "Scoped Processing Service is working. Count: {Count} - key: {key}", executionCount, key);

                await Task.Delay(100000, stoppingToken);
            }
        }
    }
}
