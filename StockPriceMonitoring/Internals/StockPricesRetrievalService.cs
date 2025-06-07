using Newtonsoft.Json;
using StockPriceMonitoring.Alerts.Internals.Models;

namespace StockPriceMonitoring.Alerts.Internals {
    internal class StockPricesRetrievalService : BackgroundService {
        private readonly ILogger<StockPricesRetrievalService> logger;
        private readonly IAlertChecker alertChecker;
        private readonly IStockPriceFetcher stockPriceFetcher;

        public StockPricesRetrievalService(ILogger<StockPricesRetrievalService> logger, IAlertChecker alertChecker, IStockPriceFetcher stockPriceFetcher) {
            this.logger = logger;
            this.alertChecker = alertChecker;
            this.stockPriceFetcher = stockPriceFetcher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {

                var stocks = await stockPriceFetcher.FetchStocksAsync(stoppingToken);

                await StockRepository.UpdateAllStocksFromFile(stocks, stoppingToken);

                logger.LogInformation("Stock prices updated: {Stocks}", JsonConvert.SerializeObject(stocks));

                foreach (var stock in stocks) {
                    await alertChecker.CheckAlertsAsync(stock.Symbol, stock.Price, stoppingToken);
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
