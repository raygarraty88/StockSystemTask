namespace StockSystemTask.Application.Queries
{
    public class PricePerformanceItemDto
    {
        public PricePerformanceItemDto(DateTime date, string symbol, decimal performance)
        {
            Date = date;
            Symbol = symbol;
            Performance = performance;
        }

        public DateTime Date { get; }
        public string Symbol { get; }
        public decimal Performance { get; }
    }
}
