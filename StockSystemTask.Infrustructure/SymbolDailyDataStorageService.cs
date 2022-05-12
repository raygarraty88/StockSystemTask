using MongoDB.Driver;
using StockSystemTask.Application;
using StockSystemTask.Domain.Entities;

namespace StockSystemTask.Infrustructure
{
    public class SymbolDailyDataStorageService : ISymbolDailyDataStorageService
    {
        private readonly IMongoCollection<SymbolDailyData> _dailyDataCollection;

        public SymbolDailyDataStorageService(IMongoCollection<SymbolDailyData> dailyDataCollection)
        {
            _dailyDataCollection = dailyDataCollection;
        }

        public async Task UpsertAsync(IEnumerable<SymbolDailyData> dailyDataRange)
        {
            var bulkOps = new List<WriteModel<SymbolDailyData>>();

            foreach (var record in dailyDataRange)
            {
                var upsertOne = new ReplaceOneModel<SymbolDailyData>(
                    Builders<SymbolDailyData>.Filter.Where(x => x.Symbol == record.Symbol && x.Date == record.Date),
                    record)
                { IsUpsert = true };

                bulkOps.Add(upsertOne);
            }

            await _dailyDataCollection.BulkWriteAsync(bulkOps);
        }

    }
}
