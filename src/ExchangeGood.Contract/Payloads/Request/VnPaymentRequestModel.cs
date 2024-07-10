namespace ExchangeGood.Contract.Payloads.Request
{
    public class VnPaymentRequestModel
    {
        public string FullName { get; set; }
        public string OrderId { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}