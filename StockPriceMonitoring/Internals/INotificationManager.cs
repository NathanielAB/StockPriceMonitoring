using System.Threading.Channels;

namespace StockPriceMonitoring.Alerts.Internals {
    public interface INotificationManager {
        void PushUserNotification(Guid userId, string message);

        ChannelReader<string> GetUserNotifications(Guid userId);
    }
}
