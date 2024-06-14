namespace ExchangeGood.Contract.Common;

public class ProductParams : PaginationParams
{
    public string? Keyword {get; set;}
    public string Orderby { get; set; } = "lastActive";
}