using Moq;
using StockPriceMonitoring.Alerts.Internals;
using StockPriceMonitoring.Alerts.Internals.Models;

namespace StockPriceMonitoring.AlertsTests.Internals {
    public class AlertCheckerTests {
        [Fact]
        public async Task CheckAlertsAsync_TriggersAlerts_WhenThresholdIsReached() {
            // Arrange
            var mockNotificationManager = new Mock<INotificationManager>();
            var alertRepository = new AlertRepository(Path.Combine(AppContext.BaseDirectory, "Data", "user_alert_trigger_test.json"));
            var alertChecker = new AlertChecker(mockNotificationManager.Object, alertRepository);

            decimal currentPrice = 50;

            // Act
            await alertChecker.CheckAlertsAsync("AAPL", currentPrice);

            // Assert
            mockNotificationManager.Verify(m =>
                m.PushUserNotification(Guid.Parse("01850077-1578-459c-9d92-fb18257f77e2"), It.Is<string>(msg => msg.Equals("Threshold reached for your Alert on AAPL, the price is now at 50"))),
                Times.Once);
        }

        [Fact]
        public async Task CheckAlertsAsync_DoesNotTriggersAlerts_WhenAlreadyTriggered() {
            // Arrange
            var mockNotificationManager = new Mock<INotificationManager>();
            var alertRepository = new AlertRepository(Path.Combine(AppContext.BaseDirectory, "Data", "user_alert_not_trigger_test.json"));
            var alertChecker = new AlertChecker(mockNotificationManager.Object, alertRepository);

            decimal currentPrice = 50;

            // Act
            await alertChecker.CheckAlertsAsync("AAPL", currentPrice);

            // Assert
            mockNotificationManager.Verify(m =>
                m.PushUserNotification(Guid.Parse("73e557c1-fed3-4bfd-a026-e44f41f79f67"), It.Is<string>(msg => msg.Equals("Threshold reached for your Alert on AAPL, the price is now at 50"))),
                Times.Never);
        }
    }
}