
using Microsoft.AspNetCore.Mvc;
using StockPriceMonitoring.Alerts.Internals.Models;
using System.ComponentModel.DataAnnotations;

namespace StockPriceMonitoring.Alerts.Features.CreateAlert {
    [ApiController]
    public sealed class CreateAlertController : Controller {
        [HttpPost("/alerts")]
        public async Task<bool> CreateAlert([FromBody, Required] CreateAlertRequest request, [FromHeader(Name = "x-user-id"), Required] Guid userId, CancellationToken cancellationToken) {
            return await AlertRepository.AddAlertEntitiesToFile(request.ToEntity(userId), cancellationToken);
        }
    }
}
