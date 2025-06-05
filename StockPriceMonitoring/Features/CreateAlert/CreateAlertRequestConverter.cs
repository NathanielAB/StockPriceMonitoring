using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StockPriceMonitoring.Alerts.Features.CreateAlert {
    internal sealed class CreateAlertRequestConverter : JsonConverter<CreateAlertRequest> {
        public override CreateAlertRequest ReadJson(JsonReader reader, Type objectType, CreateAlertRequest? existingValue, bool hasExistingValue, JsonSerializer serializer) {
            var jo = JObject.Load(reader);
            var type = jo["type"]?.ToString();

            CreateAlertRequest result = type switch {
                AboveCreateAlertRequest.SchemaReference => new AboveCreateAlertRequest(),
                BelowCreateAlertRequest.SchemaReference => new BelowCreateAlertRequest(),
                _ => throw new JsonSerializationException($"Unknown alert type: {type}")
            };
            
            serializer.Populate(jo.CreateReader(), result);

            return result;
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, CreateAlertRequest? value, JsonSerializer serializer) => throw new NotImplementedException();
    }
}
