using Microsoft.AspNetCore.Mvc;
using StockPriceMonitoring.Alerts.Internals;
using System.ComponentModel.DataAnnotations;

namespace StockPriceMonitoring.Alerts.Features.DeleteAlert {
    [ApiController]
    public sealed class DeleteAlertController : Controller {
        [HttpDelete("/alert/{alertId}")]
        public async Task<bool> DeleteAlert([Required] Guid alertId, CancellationToken cancellationToken) {
            return await AlertRepository.DeleteAlertEntitiesToFile(alertId, cancellationToken);
        }
    }
}
