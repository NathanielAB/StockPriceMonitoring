using Newtonsoft.Json;

namespace StockPriceMonitoring.Alerts.Internals.Models {
    internal static class StockRepository {
        private readonly static string FilePath = Path.Combine(AppContext.BaseDirectory, "Data", "StockPricesMockData.json");

        public static async Task<IEnumerable<Stock>?> GetStocksFromFile(CancellationToken cancellationToken) {
            if (!File.Exists(FilePath)) {
                return null;
            }
            var fileContent = await File.ReadAllTextAsync(FilePath, cancellationToken);
            return JsonConvert.DeserializeObject<IEnumerable<Stock>>(fileContent);
        }

        public static async Task<bool> UpdateAllStocksFromFile(IEnumerable<Stock> stocks, CancellationToken cancellationToken) {
            if (!File.Exists(FilePath)) {
                return false;
            }

            await File.WriteAllTextAsync(FilePath, JsonConvert.SerializeObject(stocks), cancellationToken);

            return true;
        }
    }
}
