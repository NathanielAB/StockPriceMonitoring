using Microsoft.AspNetCore.Mvc;
using StockPriceMonitoring.Alerts.Internals.Models;

namespace StockPriceMonitoring.Alerts.Features.ResetAlerts {
    public class ResetAlertsController : Controller {
        private readonly AlertRepository alertRepository;

        public ResetAlertsController(AlertRepository alertRepository) {
            this.alertRepository = alertRepository;
        }

        [HttpPost("/alerts/reset")]
        public async Task ResetAlerts(CancellationToken cancellationToken) {
            var alerts = await alertRepository.GetAlertEntitiesFromFile(cancellationToken);
            if (alerts is null) {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            var newAlerts = alerts.Select(alert => {
                alert.Triggered = false;
                return alert;
            });

            foreach (var alert in newAlerts) {
                await alertRepository.UpdateAlertEntitiesToFile(alert.Id, alert, cancellationToken);
            }
        }
    }
}
