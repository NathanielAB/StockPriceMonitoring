using Microsoft.AspNetCore.Mvc;
using StockPriceMonitoring.Alerts.Internals;

namespace StockPriceMonitoring.Alerts.Features.GetNotifications {
    [ApiController]
    public sealed class GetNotificationsController : Controller {
        private readonly INotificationManager notificationManager;

        public GetNotificationsController(INotificationManager notificationManager) {
            this.notificationManager = notificationManager;
        }

        [HttpGet("/notifications/{userId}")]
        public async Task GetNotifications(Guid userId, CancellationToken cancellationToken) {
            HttpContext.Response.Headers.Append("Content-Type", "text/event-stream");

            var stream = notificationManager.GetUserNotifications(userId);
            await foreach (var message in stream.ReadAllAsync(cancellationToken)) {
                await HttpContext.Response.WriteAsync($"{message}\n\n", cancellationToken);
                await HttpContext.Response.Body.FlushAsync(cancellationToken);
            }
        }
    }
}
