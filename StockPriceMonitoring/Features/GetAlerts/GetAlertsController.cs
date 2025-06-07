using Microsoft.AspNetCore.Mvc;
using StockPriceMonitoring.Alerts.Internals.Models;
using System.ComponentModel.DataAnnotations;

namespace StockPriceMonitoring.Alerts.Features.GetAlerts {
    [ApiController]
    public sealed class GetAlertsController : Controller {
        private readonly AlertRepository alertRepository;

        public GetAlertsController(AlertRepository alertRepository) {
            this.alertRepository = alertRepository;
        }

        [HttpGet("/alerts")]
        public async Task<IEnumerable<AlertEntity>?> GetAlerts([FromHeader(Name = "x-user-id"), Required] Guid userId, CancellationToken cancellationToken) {
            return await alertRepository.GetUserAlertEntitiesFromFile(userId, cancellationToken);
        }
    }
}
