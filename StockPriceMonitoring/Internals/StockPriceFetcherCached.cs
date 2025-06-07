using Microsoft.Extensions.Caching.Memory;
using StockPriceMonitoring.Alerts.Internals.Models;
using System;

namespace StockPriceMonitoring.Alerts.Internals {
    internal sealed class StockPriceFetcherCached : IStockPriceFetcher {
        private readonly IStockPriceFetcher stockPriceFetcher;
        private readonly IMemoryCache memoryCache;

        public StockPriceFetcherCached(IStockPriceFetcher stockPriceFetcher, IMemoryCache memoryCache) {
            this.stockPriceFetcher = stockPriceFetcher;
            this.memoryCache = memoryCache;
        }

        public async Task<IEnumerable<Stock>> FetchStocksAsync(CancellationToken cancellationToken) {
            var stocks = await memoryCache.GetOrCreateAsync("stocks", async entry => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20);
                return await stockPriceFetcher.FetchStocksAsync(cancellationToken);
            });

            return stocks ?? [];
        }
    }
}
