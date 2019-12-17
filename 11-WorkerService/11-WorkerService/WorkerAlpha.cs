using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace _11_WorkerService
{
    public class WorkerAlpha : BackgroundService
    {
        private readonly ILogger<WorkerAlpha> _logger;
        private byte _workCount = 0;

        public WorkerAlpha(ILogger<WorkerAlpha> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker has started!");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker has stopped!");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested && _workCount < 10)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
                _workCount++;
            }
        }

        public override void Dispose()
        {
            _logger.LogInformation("Worker has disposed!");
        }
    }
}