using MediatR;
using StockSystemTask.Application.PricePerformanceCalculator;
using StockSystemTask.Application.StockDataProviders;
using StockSystemTask.Domain;

namespace StockSystemTask.Application.Queries
{
    public class GetPricePerformanceDataQueryHandler : IRequestHandler<GetPricePerformanceDataQuery, IEnumerable<PricePerformanceItemDto>>
    {
        private readonly IStockDataProvider _stockDataProvider;
        private readonly ISymbolDailyDataStorageService _symbolDailyDataStorageService;
        private readonly IPricePerformanceCalculator _pricePerformanceProvider;

        public GetPricePerformanceDataQueryHandler(
            IStockDataProvider stockDataProvider,
            ISymbolDailyDataStorageService symbolDailyDataStorageService,
            IPricePerformanceCalculator pricePerformanceProvider)
        {
            _stockDataProvider = stockDataProvider;
            _symbolDailyDataStorageService = symbolDailyDataStorageService;
            _pricePerformanceProvider = pricePerformanceProvider;
        }

        public async Task<IEnumerable<PricePerformanceItemDto>> Handle(GetPricePerformanceDataQuery request, CancellationToken cancellationToken)
        {
            var requestedPricesTask = _stockDataProvider.GetDailyPricesAsync(request.Symbol, request.StartDate, request.EndDate);
            var upsertAsyncTask = requestedPricesTask.ContinueWith(async result => await _symbolDailyDataStorageService.UpsertAsync(result.Result));

            var spyPricesTask = _stockDataProvider.GetDailyPricesAsync(Constants.SPYSymbol, request.StartDate, request.EndDate);

            await Task.WhenAll(requestedPricesTask, upsertAsyncTask, spyPricesTask);

            var requestedSymbolPerformance = _pricePerformanceProvider.GetPerformance(requestedPricesTask.Result);
            var spyPerformance = _pricePerformanceProvider.GetPerformance(spyPricesTask.Result);

            return requestedSymbolPerformance.Concat(spyPerformance);
        }
    }
}
