using Microsoft.AspNetCore.Mvc;
using StockPriceMonitoring.Alerts.Internals.Models;
using System.ComponentModel.DataAnnotations;

namespace StockPriceMonitoring.Alerts.Features.GetAlerts {
    [ApiController]
    public sealed class GetAlertsController : Controller {
        [HttpGet("/alerts")]
        public async Task<IEnumerable<AlertEntity>?> GetAlerts([FromHeader(Name = "x-user-id"), Required] Guid userId, CancellationToken cancellationToken) {
            return await AlertRepository.GetUserAlertEntitiesFromFile(userId, cancellationToken);
        }
    }
}
