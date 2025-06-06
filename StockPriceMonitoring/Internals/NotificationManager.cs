using System.Threading.Channels;

namespace StockPriceMonitoring.Alerts.Internals {
    public class NotificationManager : INotificationManager {
        private readonly Dictionary<Guid, Channel<string>> channels = [];

        public ChannelReader<string> GetUserNotifications(Guid userId) {
            if (!channels.TryGetValue(userId, out Channel<string>? value)) {
                value = Channel.CreateUnbounded<string>();
                channels[userId] = value;
            }

            return value.Reader;
        }

        public void PushUserNotification(Guid userId, string message) {
            if (channels.TryGetValue(userId, out var channel)) {
                channel.Writer.TryWrite(message);
            }
        }
    }
}
