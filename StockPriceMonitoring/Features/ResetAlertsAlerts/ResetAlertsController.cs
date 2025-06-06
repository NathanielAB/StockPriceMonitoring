using Microsoft.AspNetCore.Mvc;
using StockPriceMonitoring.Alerts.Internals.Models;

namespace StockPriceMonitoring.Alerts.Features.ResetAlerts {
    public class ResetAlertsController : Controller {
        [HttpPost("/alerts/reset")]
        public async Task ResetAlerts(CancellationToken cancellationToken) {
            var alerts = await AlertRepository.GetAlertEntitiesFromFile(cancellationToken);
            if (alerts is null) {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            var newAlerts = alerts.Select(alert => {
                alert.Triggered = false;
                return alert;
            });

            foreach (var alert in newAlerts) {
                await AlertRepository.UpdateAlertEntitiesToFile(alert.Id, alert, cancellationToken);
            }
        }
    }
}
