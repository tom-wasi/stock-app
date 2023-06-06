namespace StocksAppWithConfiguration.Models
{
    public class StockTrade
    {
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public string Price { get; set; } = "";
        public uint Quantity { get; set; } = 0;
    }
}
