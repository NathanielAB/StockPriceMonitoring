using StockPriceMonitoring.Alerts.Internals.Models;

namespace StockPriceMonitoring.Alerts.Internals {
    public class AlertChecker : IAlertChecker {

        private readonly INotificationManager notificationManager;

        public AlertChecker(INotificationManager notificationManager) {
            this.notificationManager = notificationManager;
        }

        public async Task CheckAlertsAsync(string symbol, decimal currentPrice, CancellationToken cancellationToken = default) {

            var alerts = await AlertRepository.GetSymbolAlertEntitiesFromFile(symbol, cancellationToken);

            var validAlerts = alerts?.Where(alert => alert.IsThresholdReached(currentPrice) && !alert.Triggered);

            if (validAlerts is null || !validAlerts.Any()) {
                return;
            }

            foreach (var alert in validAlerts) {
                notificationManager.PushUserNotification(
                    alert.UserId,
                    $"Threshold reached for your Alert on {alert.StockSymbol}, the price is now at {currentPrice}");

                alert.Triggered = true;
                await AlertRepository.UpdateAlertEntitiesToFile(alert.Id, alert, cancellationToken);
            }
        }
    }
}
