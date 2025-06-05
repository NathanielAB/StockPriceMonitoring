
namespace StockPriceMonitoring.Alerts.Internals {
    public class NotifyUser : INotifyUser {
        public Task NotifyUserAsync(Guid userId, string message, CancellationToken cancellationToken = default) {
            throw new NotImplementedException();
        }
    }
}
