using Newtonsoft.Json;

namespace StockPriceMonitoring.Alerts.Internals.Models {
    public class AlertRepository {

        private static readonly JsonSerializerSettings Settings = new() {
            Converters = [
                new AlertEntityConverter()
            ],
        };

        private readonly string FilePath;

        public AlertRepository(string path) {
            FilePath = path;
        }

        public async Task<IEnumerable<AlertEntity>?> GetAlertEntitiesFromFile(CancellationToken cancellationToken) {
            if (!File.Exists(FilePath)) {
                return null;
            }

            var fileContent = await File.ReadAllTextAsync(FilePath, cancellationToken);

            return JsonConvert.DeserializeObject<IEnumerable<AlertEntity>>(fileContent, Settings);
        }

        public async Task<IEnumerable<AlertEntity>?> GetUserAlertEntitiesFromFile(Guid userId, CancellationToken cancellationToken) {
            if (!File.Exists(FilePath)) {
                return null;
            }

            var fileContent = await File.ReadAllTextAsync(FilePath, cancellationToken);

            return JsonConvert.DeserializeObject<IEnumerable<AlertEntity>>(fileContent, Settings)?
                .Where(alert => alert.UserId == userId);
        }

        public async Task<IEnumerable<AlertEntity>?> GetSymbolAlertEntitiesFromFile(string symbol, CancellationToken cancellationToken) {
            if (!File.Exists(FilePath)) {
                return null;
            }

            var fileContent = await File.ReadAllTextAsync(FilePath, cancellationToken);

            return JsonConvert.DeserializeObject<IEnumerable<AlertEntity>>(fileContent, Settings)?
                .Where(alert => alert.StockSymbol == symbol);
        }

        public async Task<bool> AddAlertEntitiesToFile(AlertEntity alertEntity, CancellationToken cancellationToken) {
            var alerts = await GetAlertEntitiesFromFile(cancellationToken);
            if (alerts is null) {
                return false;
            }

            var newAlerts = alerts.Append(alertEntity);
            await File.WriteAllTextAsync(FilePath, JsonConvert.SerializeObject(newAlerts, Formatting.Indented, Settings), cancellationToken);

            return true;
        }

        public async Task<bool> DeleteAlertEntitiesToFile(Guid id, CancellationToken cancellationToken) {
            var alerts = await GetAlertEntitiesFromFile(cancellationToken);
            if (alerts is null) {
                return false;
            }

            var newAlerts = alerts.Where(alert => alert.Id != id);
            await File.WriteAllTextAsync(FilePath, JsonConvert.SerializeObject(newAlerts, Formatting.Indented, Settings), cancellationToken);

            return true;
        }

        public async Task<bool> UpdateAlertEntitiesToFile(Guid id, AlertEntity alertEntity, CancellationToken cancellationToken) {
            var alertEnumerable = await GetAlertEntitiesFromFile(cancellationToken);

            var alerts = alertEnumerable?.ToList();
            if (alerts is null) {
                return false;
            }

            var alertIndex = alerts.FindIndex(alert => alert.Id == id);
            if (alertIndex == -1) {
                return false;
            }

            alerts[alertIndex] = alertEntity;

            await File.WriteAllTextAsync(FilePath, JsonConvert.SerializeObject(alerts, Formatting.Indented, Settings), cancellationToken);

            return true;
        }
    }
}
