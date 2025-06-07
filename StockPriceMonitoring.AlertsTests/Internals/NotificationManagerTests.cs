using StockPriceMonitoring.Alerts.Internals;

namespace StockPriceMonitoring.AlertsTests.Internals {
    public class NotificationManagerTests {
        [Fact]
        public void PushUserNotification_AddsMessageToQueue() {
            // Arrange
            var notificationManager = new NotificationManager();
            var userId = new Guid();
            var message = "Test Notification";

            // Act
            var stream = notificationManager.GetUserNotifications(userId);
            notificationManager.PushUserNotification(userId, message);
            stream.TryPeek(out var result);

            // Assert
            Assert.Equal(message, result);
        }

        [Fact]
        public void GetUserStream_AlwaysReturnsSameStream_ForSameUser() {
            // Arrange
            var notificationManager = new NotificationManager();
            var userId = new Guid();

            // Act
            var stream1 = notificationManager.GetUserNotifications(userId);
            var stream2 = notificationManager.GetUserNotifications(userId);

            // Assert
            Assert.Same(stream1, stream2);
        }
    }
}
