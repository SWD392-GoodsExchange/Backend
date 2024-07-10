using ExchangeGood.Data.Models;

namespace ExchangeGood.Contract.Payloads.Response
{
    public class VnPaymentResponseModel
    {
        public bool Success { get; set; }
        public string ResponseCode { get; set; }
        public int OrderId { get; set; }
    }
}