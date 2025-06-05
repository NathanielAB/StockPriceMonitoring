using Newtonsoft.Json;
using StockPriceMonitoring.Alerts.Internals;
using StockPriceMonitoring.Alerts.Internals.Models;

namespace StockPriceMonitoring.Alerts {
    public class StockPricesRetrievalService : BackgroundService {
        private readonly static string FilePath = Path.Combine(AppContext.BaseDirectory, "Data", "StockPricesMockData.json");
        private readonly static Random random = new();
        private List<Stock> stocks = [];

        private readonly ILogger<StockPricesRetrievalService> logger;
        private readonly IAlertChecker alertChecker;

        public StockPricesRetrievalService(ILogger<StockPricesRetrievalService> logger, IAlertChecker alertChecker) {
            this.logger = logger;
            this.alertChecker = alertChecker;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            if (!File.Exists(FilePath)) {
                logger.LogError("Data file not found at: {Path}", FilePath);
                return;
            }

            stocks = JsonConvert.DeserializeObject<List<Stock>>(await File.ReadAllTextAsync(FilePath, stoppingToken)) ?? [];

            while (!stoppingToken.IsCancellationRequested) {
                UpdateStockPrices();
                logger.LogDebug("Stock prices updated: {Stocks}", JsonConvert.SerializeObject(stocks));

                foreach(var stock in stocks) {
                    await alertChecker.CheckAlertsAsync(stock.Symbol, stock.Price, stoppingToken);
                }

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
