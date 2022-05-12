using MediatR;

namespace StockSystemTask.Application.Queries
{
    public class GetPricePerformanceDataQuery : IRequest<IEnumerable<PricePerformanceItemDto>>
    {
        public GetPricePerformanceDataQuery(DateTime startDate, DateTime endDate, string symbol)
        {
            StartDate = startDate;
            EndDate = endDate;
            Symbol = symbol;
        }

        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public string Symbol { get; }
    }
}
