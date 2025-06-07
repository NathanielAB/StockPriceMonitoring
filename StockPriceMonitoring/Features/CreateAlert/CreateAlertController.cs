
using Microsoft.AspNetCore.Mvc;
using StockPriceMonitoring.Alerts.Internals.Models;
using System.ComponentModel.DataAnnotations;

namespace StockPriceMonitoring.Alerts.Features.CreateAlert {
    [ApiController]
    public sealed class CreateAlertController : Controller {
        private readonly AlertRepository alertRepository;

        public CreateAlertController(AlertRepository alertRepository) {
            this.alertRepository = alertRepository;
        }

        [HttpPost("/alerts")]
        public async Task<bool> CreateAlert([FromBody, Required] CreateAlertRequest request, [FromHeader(Name = "x-user-id"), Required] Guid userId, CancellationToken cancellationToken) {
            return await alertRepository.AddAlertEntitiesToFile(request.ToEntity(userId), cancellationToken);
        }
    }
}
