using MediatR;
using Microsoft.AspNetCore.Mvc;
using StockSystemTask.Application.Queries;

namespace StockSystemTask.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlphaVantagePricesController : ControllerBase
    {
        private readonly ISender _mediator;

        public AlphaVantagePricesController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<PricePerformanceItemDto>> Get(string symbol)
        {
            return await _mediator.Send(new GetPricePerformanceDataQuery(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow, symbol));
        }            
    }
}