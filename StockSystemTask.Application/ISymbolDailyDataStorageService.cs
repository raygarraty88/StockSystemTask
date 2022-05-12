using StockSystemTask.Domain.Entities;

namespace StockSystemTask.Application
{
    public interface ISymbolDailyDataStorageService
    {
        Task UpsertAsync(IEnumerable<SymbolDailyData> dailyDataRange);
    }
}