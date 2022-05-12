using Ardalis.GuardClauses;
using StockSystemTask.Application.Queries;
using StockSystemTask.Domain.Entities;

namespace StockSystemTask.Application.PricePerformanceCalculator
{
    public class PricePerformanceCalculator : IPricePerformanceCalculator
    {
        public IEnumerable<PricePerformanceItemDto> GetPerformance(IEnumerable<SymbolDailyData> symbolData)
        {
            Guard.Against.NullOrEmpty(symbolData);

            var basePrice = symbolData.OrderBy(data => data.Date).First();

            return symbolData.Select(dailyData =>
            {
                Guard.Against.NegativeOrZero(dailyData.Close);
                Guard.Against.NegativeOrZero(basePrice.Close);
                return new PricePerformanceItemDto(dailyData.Date, dailyData.Symbol, dailyData.Close / basePrice.Close * 100 - 100);
            });
        }
    }
}
