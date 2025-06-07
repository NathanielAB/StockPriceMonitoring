using StockPriceMonitoring.Alerts.Internals.Models;
using System;

namespace StockPriceMonitoring.Alerts.Internals {
    internal sealed class StockPriceFetcher : IStockPriceFetcher {
        private readonly static Random random = new();
        
        public async Task<IEnumerable<Stock>> FetchStocksAsync(CancellationToken cancellationToken) {
            var stocks = await StockRepository.GetStocksFromFile(cancellationToken) ?? [];

            foreach (var stock in stocks) {
                stock.Price = (decimal)(random.NextDouble() * 99 + 1);
            }

            return stocks;
        }
    }
}
