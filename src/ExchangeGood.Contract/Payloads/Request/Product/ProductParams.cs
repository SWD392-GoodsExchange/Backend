using ExchangeGood.Contract.Enum.Product;

namespace ExchangeGood.Contract.Common;

public class ProductParams : PaginationParams
{
    public string Keyword {get; set;}
    public string Type { get; set; } = "trade";
    public string Orderby { get; set; } = "lastActive";
}