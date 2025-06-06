using Newtonsoft.Json;

namespace StockPriceMonitoring.Alerts.Internals.Models {
    public abstract class AlertEntity {
        [JsonProperty("type")]
        public abstract string Type { get; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("userId")]
        public Guid UserId { get; set; }

        [JsonProperty("stockSymbol")]
        public string StockSymbol { get; set; } = null!;

        [JsonProperty("thresholdPrice")]
        public decimal ThresholdPrice { get; set; }

        [JsonProperty("triggered")]
        public bool Triggered { get; set; }

        public abstract bool IsThresholdReached(decimal currentPrice);
    }

    public sealed class AboveAlertEntity : AlertEntity {
        public const string SchemaReference = "above";

        public override string Type => SchemaReference;

        public override bool IsThresholdReached(decimal currentPrice) {
            return currentPrice >= ThresholdPrice;
        }
    }

    public sealed class BelowAlertEntity : AlertEntity {
        public const string SchemaReference = "below";

        public override string Type => SchemaReference;

        public override bool IsThresholdReached(decimal currentPrice) {
            return currentPrice <= ThresholdPrice;
        }
    }
}