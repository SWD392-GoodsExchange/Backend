namespace ExchangeGood.Contract.Common;

public class ProductParams : PaginationParams
{
    public string Keyword {get; set;}    
    public string UsageInformation { get; set;}
    public string Origin { get; set;}
    public string Title { get; set;}
    public string Price { get; set;}
}