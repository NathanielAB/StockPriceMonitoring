using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StockPriceMonitoring.Alerts.Internals.Models {
    public class AlertEntityConverter : JsonConverter<AlertEntity> {
        public override AlertEntity ReadJson(JsonReader reader, Type objectType, AlertEntity? existingValue, bool hasExistingValue, JsonSerializer serializer) {
            var jo = JObject.Load(reader);
            var type = jo["type"]?.ToString();

            AlertEntity result = type switch {
                AboveAlertEntity.SchemaReference => new AboveAlertEntity(),
                BelowAlertEntity.SchemaReference => new BelowAlertEntity(),
                _ => throw new JsonSerializationException($"Unknown alert type: {type}")
            };

            serializer.Populate(jo.CreateReader(), result);

            return result;
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, AlertEntity? value, JsonSerializer serializer) => throw new NotImplementedException();
    }
}
