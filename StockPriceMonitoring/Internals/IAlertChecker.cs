namespace StockPriceMonitoring.Alerts.Internals {
    public interface IAlertChecker {
        public Task CheckAlertsAsync(string symbol, decimal currentPrice, CancellationToken cancellationToken = default);
    }
}
