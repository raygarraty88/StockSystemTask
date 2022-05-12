namespace StockSystemTask.Domain.Entities
{
    public class SymbolDailyData
    {
        public SymbolDailyData(string symbol, DateTime date, decimal open, decimal high, decimal low, decimal close, decimal volume)
        {
            Symbol = symbol;
            Date = date;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
        }
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }
    }
}
