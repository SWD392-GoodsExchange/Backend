namespace ExchangeGood.Contract;

public class GetProductsForExchangeRequest
{
    public string OwnerId { get; set; } // who has a product 
    public string ExchangerId { get; set; } // who want to exchange
    public List<int> ProductIds { get; set; }
}