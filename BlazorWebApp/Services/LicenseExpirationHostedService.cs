using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlazorWebApp.Services
{
    public class LicenseExpirationHostedService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<LicenseExpirationHostedService> _logger;
        private Timer? _timer;

        public LicenseExpirationHostedService(IServiceProvider serviceProvider, ILogger<LicenseExpirationHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("License Expiration Hosted Service is starting.");

            // Run immediately on start, then every minute for testing
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var licenseService = scope.ServiceProvider.GetRequiredService<LicenseService>();
                try
                {
                    await licenseService.UpdateExpiredLicensesAsync();
                    _logger.LogInformation("Checked and updated expired licenses.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating expired licenses.");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("License Expiration Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}