using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using StockSystemTask.Application.StockDataProviders;
using StockSystemTask.Domain.Entities;
using StockSystemTask.Domain.Exceptions;
using System.Globalization;

namespace StockSystemTask.Infrustructure
{
    public class AlphaVantageStockDataProvider : IStockDataProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AlphaVantageStockDataProvider> _logger;

        public AlphaVantageStockDataProvider(HttpClient httpClient, ILogger<AlphaVantageStockDataProvider> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<SymbolDailyData>> GetDailyPricesAsync(string symbol, DateTime startDate, DateTime endDate)
        {
            try
            {
                var timeSeries = await RetrieveTimeSeries(symbol);
                var dailyData = ParseTimeSeries(timeSeries, symbol);
                return dailyData.Where(data => data.Date >= startDate.Date && data.Date <= endDate.Date);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving stock data from Alpha Vantage. Message = {ex.Message}");
                throw new StockProviderException(ex.Message);
            }

        }

        private async Task<JToken> RetrieveTimeSeries(string symbol)
        {
            // TODO - retrieve api key from config
            var response = await _httpClient.GetAsync($"query?apikey=A0H821HHPOVYB7M9&function=TIME_SERIES_DAILY&symbol={symbol}");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JObject.Parse(responseString);
            var timeSeries = responseObject["Time Series (Daily)"];

            if (timeSeries is null)
            {
                throw new Exception($"Failed to retrieve time series from AlphaVantage. Http response body - {responseString}");
            }
            return timeSeries;
        }

        private static List<SymbolDailyData> ParseTimeSeries(JToken timeSeries, string symbol)
        {
            var dailyData = new List<SymbolDailyData>();

            foreach (var marketData in timeSeries.Children<JProperty>())
            {
                var date = DateTime.Parse(marketData.Name, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                var open = marketData.Value["1. open"]?.Value<decimal>();
                var high = marketData.Value["2. high"]?.Value<decimal>();
                var low = marketData.Value["3. low"]?.Value<decimal>();
                var close = marketData.Value["4. close"]?.Value<decimal>();
                var volume = marketData.Value["5. volume"]?.Value<decimal>();

                Guard.Against.Null(open);
                Guard.Against.Null(high);
                Guard.Against.Null(low);
                Guard.Against.Null(close);
                Guard.Against.Null(volume);

                dailyData.Add(new SymbolDailyData(
                    symbol: symbol,
                    date: date,
                    open: open.Value,
                    high: high.Value,
                    low: low.Value,
                    close: close.Value,
                    volume: volume.Value));
            }

            return dailyData;
        }
    }
}
