using StockPriceMonitoring.Alerts.Internals.Models;

namespace StockPriceMonitoring.Alerts.Internals {
    public class AlertChecker : IAlertChecker {

        private readonly INotifyUser notifyUser;

        public AlertChecker(INotifyUser notifyUser) {
            this.notifyUser = notifyUser;
        }

        public async Task CheckAlertsAsync(string symbol, decimal currentPrice, CancellationToken cancellationToken = default) {

            var alerts = await AlertRepository.GetSymbolAlertEntitiesFromFile(symbol, cancellationToken);

            var validAlerts = alerts?.Where(alert => alert.IsThresholdReached(currentPrice));

            if (validAlerts is null || !validAlerts.Any()) {
                return;
            }

            foreach (var alert in validAlerts) {
                await notifyUser.NotifyUserAsync(
                    alert.UserId,
                    $"Threshold reached for your Alert on {alert.StockSymbol}, the price is now at {currentPrice}",
                    cancellationToken);
            }
        }
    }
}
