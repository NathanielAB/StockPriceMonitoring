namespace StockPriceMonitoring.Alerts.Internals {
    public interface INotifyUser {
        Task NotifyUserAsync(Guid userId, string message, CancellationToken cancellationToken = default);
    }
}
