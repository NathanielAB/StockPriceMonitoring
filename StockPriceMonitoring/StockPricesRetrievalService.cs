using Newtonsoft.Json;
using StockPriceMonitoring.Alerts.Internals;
using StockPriceMonitoring.Alerts.Internals.Models;

namespace StockPriceMonitoring.Alerts {
    public class StockPricesRetrievalService : BackgroundService {
        
        private readonly static Random random = new();
        private IEnumerable<Stock> stocks = [];

        private readonly ILogger<StockPricesRetrievalService> logger;
        private readonly IAlertChecker alertChecker;

        public StockPricesRetrievalService(ILogger<StockPricesRetrievalService> logger, IAlertChecker alertChecker) {
            this.logger = logger;
            this.alertChecker = alertChecker;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            stocks = await StockRepository.GetStocksFromFile(stoppingToken) ?? [];

            while (!stoppingToken.IsCancellationRequested) {
                UpdateStockPrices();
                logger.LogInformation("Stock prices updated: {Stocks}", JsonConvert.SerializeObject(stocks));

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
