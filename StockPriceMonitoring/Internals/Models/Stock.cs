using Newtonsoft.Json;

namespace StockPriceMonitoring.Alerts.Internals.Models {
    internal sealed class Stock {
        [JsonProperty("symbol")]
        public string Symbol { get; set; } = null!;

        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}
