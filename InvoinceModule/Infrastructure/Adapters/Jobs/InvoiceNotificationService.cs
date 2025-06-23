using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCrontab;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using InvoinceModule.Infrastructure.Context;
using InvoinceModule.Application.Ports.Out;
using InvoinceModule.Application.UseCases;
using Microsoft.Extensions.Logging;

namespace InvoinceModule.Infrastructure.Adapters.Jobs
{
    public class InvoiceNotificationBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<InvoiceNotificationBackgroundService> _logger;
        private readonly CrontabSchedule _schedule;
        private DateTime _nextRun;
        public InvoiceNotificationBackgroundService(IServiceProvider serviceProvider, ILogger<InvoiceNotificationBackgroundService> logger)
        {
            this._serviceProvider = serviceProvider;
            this._logger = logger;

             _schedule = CrontabSchedule.Parse("* * * * *");
            this._nextRun = DateTime.Now.AddSeconds(-1);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Background service started.");
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                try
                {
                    if (now > _nextRun)
                    {
                        _logger.LogInformation("Executing invoice notification job at {Time}", now);

                        await ProcessUnsentInvoicesAsync();

                        _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("An error occurred while processing unsent invoices: {Message}", ex.Message);
                }
                

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _logger.LogWarning("Background service cancellation requested.");
        }

        private async Task ProcessUnsentInvoicesAsync()
        {
            using var scope = _serviceProvider.CreateScope();

            var notificationService = scope.ServiceProvider.GetRequiredService<EmailNotificationUseCase>();

            try
            {

                await notificationService.SendPendingInvoiceNotificationsAsync();

                _logger.LogInformation("Successfully processed unsent invoices.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing unsent invoices.");
            }
        }
    }
}
