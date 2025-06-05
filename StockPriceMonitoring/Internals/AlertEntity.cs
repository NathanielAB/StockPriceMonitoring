using Newtonsoft.Json;

namespace StockPriceMonitoring.Alerts.Internals {
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
    }

    public sealed class AboveAlertEntity : AlertEntity {
        public const string SchemaReference = "above";

        public override string Type => SchemaReference;
    }

    public sealed class BelowAlertEntity : AlertEntity {
        public const string SchemaReference = "below";

        public override string Type => SchemaReference;
    }
}