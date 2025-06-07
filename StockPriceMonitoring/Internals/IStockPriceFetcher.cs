using StockPriceMonitoring.Alerts.Internals.Models;

namespace StockPriceMonitoring.Alerts.Internals {
    internal interface IStockPriceFetcher {
        Task<IEnumerable<Stock>> FetchStocksAsync(CancellationToken cancellationToken);
    }
}
