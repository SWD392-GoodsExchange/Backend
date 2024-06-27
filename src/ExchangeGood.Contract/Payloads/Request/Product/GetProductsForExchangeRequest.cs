namespace ExchangeGood.Contract;

public class GetProductsForExchangeRequest
{
    public string FeId { get; set; }
    public int[] ProductIds { get; set; }
}