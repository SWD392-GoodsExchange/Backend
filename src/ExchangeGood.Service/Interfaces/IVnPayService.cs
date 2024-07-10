using ExchangeGood.Contract.Payloads.Request;
using ExchangeGood.Contract.Payloads.Response;
using Microsoft.AspNetCore.Http;

namespace ExchangeGood.Service.Interfaces
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel vnPaymentRequestModel);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collection);  
    }
}