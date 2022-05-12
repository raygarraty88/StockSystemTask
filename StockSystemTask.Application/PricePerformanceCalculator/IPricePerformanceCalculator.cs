using StockSystemTask.Application.Queries;
using StockSystemTask.Domain.Entities;

namespace StockSystemTask.Application.PricePerformanceCalculator
{
    public interface IPricePerformanceCalculator
    {
        IEnumerable<PricePerformanceItemDto> GetPerformance(IEnumerable<SymbolDailyData> symbolData);
    }
}