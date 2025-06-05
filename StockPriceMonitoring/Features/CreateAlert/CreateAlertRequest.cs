using Newtonsoft.Json;
using StockPriceMonitoring.Alerts.Internals.Models;

namespace StockPriceMonitoring.Alerts.Features.CreateAlert {
    public abstract class CreateAlertRequest {
        [JsonProperty("type")]
        public abstract string Type { get; }

        [JsonProperty("userId")]
        public Guid UserId { get; set; }

        [JsonProperty("stockSymbol")]
        public string StockSymbol { get; set; } = null!;
        
        [JsonProperty("thresholdPrice")]
        public decimal ThresholdPrice { get; set; }

        public abstract AlertEntity ToEntity();
    }

    public sealed class AboveCreateAlertRequest : CreateAlertRequest {
        public const string SchemaReference = "above";

        public override string Type => SchemaReference;

        public override AlertEntity ToEntity() {
            return new AboveAlertEntity {
                Id = Guid.NewGuid(),
                UserId = UserId,
                StockSymbol = StockSymbol,
                ThresholdPrice = ThresholdPrice
            };
        }
    }

    public sealed class BelowCreateAlertRequest : CreateAlertRequest {
        public const string SchemaReference = "below";

        public override string Type => SchemaReference;

        public override AlertEntity ToEntity() {
            return new BelowAlertEntity {
                Id = Guid.NewGuid(),
                UserId = UserId,
                StockSymbol = StockSymbol,
                ThresholdPrice = ThresholdPrice
            };
        }
    }
}
