using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksAppWithConfiguration.Models;
using StocksAppWithConfiguration.Services;
using System.Diagnostics;
using System.Reflection;

namespace StocksAppWithConfiguration.Controllers
{
    public class TradeController : Controller
    {
        private readonly TradingOptions _options;
        private readonly FinnhubService _finnhubService;
        private readonly IConfiguration _configuration;
       

        public TradeController(IOptions<TradingOptions> options, FinnhubService finnhubService, IConfiguration configuration)
        {
            _options = options.Value;
            _finnhubService = finnhubService;
            _configuration = configuration;
        }

        [Route("[action]")]
        [Route("/")]
        [Route("~/[controller]")]
        public IActionResult Index()
        {
            if(_options.DefaultStockSymbol == null)
            {
                _options.DefaultStockSymbol = "MSFT";
            }

            Dictionary<string, object>? profileResponseDictionary = _finnhubService.GetCompanyProfile(_options.DefaultStockSymbol);

            
            Dictionary<string, object>? stockResponseDictionary = _finnhubService.GetStockPriceQuote(_options.DefaultStockSymbol);


            StockTrade stockTrade = new StockTrade() { StockSymbol = _options.DefaultStockSymbol };

            if (profileResponseDictionary != null && stockResponseDictionary != null)
            {
                stockTrade = new StockTrade()
                {
                    StockSymbol = profileResponseDictionary["ticker"].ToString(),
                    StockName = profileResponseDictionary["name"].ToString(),
                    Price = stockResponseDictionary["c"].ToString()
                };
            }
            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            return View(stockTrade);
        }
    }
}
