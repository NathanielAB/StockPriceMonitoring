using Newtonsoft.Json;

namespace StockPriceMonitoring.StockPrices {
    public class StockPricesRetrievalService : BackgroundService {
        private readonly ILogger<StockPricesRetrievalService> logger;
        private readonly string filePath = Path.Combine(AppContext.BaseDirectory, "Data", "StockPricesMockData.json");
        private readonly static Random random = new();
        private List<Stock> stocks = [];

        public StockPricesRetrievalService(ILogger<StockPricesRetrievalService> logger) {
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            if (!File.Exists(filePath)) {
                logger.LogError("Data file not found at: {Path}", filePath);
                return;
            }

            stocks = JsonConvert.DeserializeObject<List<Stock>>(await File.ReadAllTextAsync(filePath, stoppingToken)) ?? [];

            while (!stoppingToken.IsCancellationRequested) {
                // Mock stock price updates via http
                UpdateStockPrices();

                logger.LogInformation("Stock prices updated: {Stocks}", JsonConvert.SerializeObject(stocks));

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        private void UpdateStockPrices() {
            foreach (var stock in stocks) {
                stock.Price = (decimal)(random.NextDouble() * 99 + 1);
            }
        }
    }
}
