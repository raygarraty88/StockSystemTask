using StockSystemTask.Domain.Entities;

namespace StockSystemTask.Application.StockDataProviders
{
    public interface IStockDataProvider
    {
        Task<IEnumerable<SymbolDailyData>> GetDailyPricesAsync(string symbol, DateTime startDate, DateTime endDate);
    }
}
