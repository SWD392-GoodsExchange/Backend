namespace ExchangeGood.Contract.Common;

public class ProductParams : PaginationParams
{
    public string? Keyword {get; set;}
    public string OrderBy { get; set; } = "lastActive";
}