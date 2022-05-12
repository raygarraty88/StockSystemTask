using MongoDB.Driver;
using StockSystemTask.Application;
using StockSystemTask.Application.PricePerformanceCalculator;
using StockSystemTask.Application.StockDataProviders;
using StockSystemTask.Domain.Entities;
using StockSystemTask.Infrustructure;
using MediatR;
using StockSystemTask.Application.Queries;
using StockSystemTask.WebUI.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options => options.Filters.Add<ApiExceptionFilterAttribute>());
builder.Services.AddMediatR(typeof(GetPricePerformanceDataQueryHandler).Assembly);
builder.Services.AddHttpClient<IStockDataProvider, AlphaVantageStockDataProvider>((services, client) =>
{
    // TODO - retrieve base address from config
    client.BaseAddress = new Uri("https://www.alphavantage.co");
});
 
builder.Services.AddSingleton<ISymbolDailyDataStorageService>(sp =>
{
    // TODO - retrieve mongo settings from config
    var mongoClient = new MongoClient("mongodb://localhost:27018");
    var mongoDatabase = mongoClient.GetDatabase("StockSystemTask");
    var symbolDailyDataCollection = mongoDatabase.GetCollection<SymbolDailyData>("SymbolDailyData");
    return new SymbolDailyDataStorageService(symbolDailyDataCollection);
});
builder.Services.AddScoped<IPricePerformanceCalculator, PricePerformanceCalculator>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
}

app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
