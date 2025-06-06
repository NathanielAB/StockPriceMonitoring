using Microsoft.AspNetCore.Mvc;
using StockPriceMonitoring.Alerts.Internals.Models;
using System.ComponentModel.DataAnnotations;

namespace StockPriceMonitoring.Alerts.Features.DeleteAlert {
    [ApiController]
    public sealed class DeleteAlertController : Controller {
        [HttpDelete("/alert/{alertId}")]
        public async Task<bool> DeleteAlert([Required] Guid alertId, [FromHeader(Name = "x-user-id"), Required] Guid userId, CancellationToken cancellationToken) {
            var alerts = await AlertRepository.GetUserAlertEntitiesFromFile(userId, cancellationToken);
            if (alerts is null) {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return false;
            }

            if (!alerts.Any(alerts => alerts.Id == alertId)) {
                HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                return false;
            }

            return await AlertRepository.DeleteAlertEntitiesToFile(alertId, cancellationToken);
        }
    }
}
