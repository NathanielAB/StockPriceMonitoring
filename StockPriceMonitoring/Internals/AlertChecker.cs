using StockPriceMonitoring.Alerts.Internals.Models;

namespace StockPriceMonitoring.Alerts.Internals {
    public class AlertChecker : IAlertChecker {

        private readonly INotificationManager notificationManager;
        private readonly AlertRepository alertRepository;

        public AlertChecker(INotificationManager notificationManager, AlertRepository alertRepository) {
            this.notificationManager = notificationManager;
            this.alertRepository = alertRepository;
        }

        public async Task CheckAlertsAsync(string symbol, decimal currentPrice, CancellationToken cancellationToken = default) {

            var alerts = await alertRepository.GetSymbolAlertEntitiesFromFile(symbol, cancellationToken);

            var validAlerts = alerts?.Where(alert => alert.IsThresholdReached(currentPrice) && !alert.Triggered);

            if (validAlerts is null || !validAlerts.Any()) {
                return;
            }

            foreach (var alert in validAlerts) {
                notificationManager.PushUserNotification(
                    alert.UserId,
                    $"Threshold reached for your Alert on {alert.StockSymbol}, the price is now at {currentPrice}");

                alert.Triggered = true;
                await alertRepository.UpdateAlertEntitiesToFile(alert.Id, alert, cancellationToken);
            }
        }
    }
}
