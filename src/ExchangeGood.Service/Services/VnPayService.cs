using ExchangeGood.Contract.Payloads.Request;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Service.Extensions;
using ExchangeGood.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ExchangeGood.Service.Services
{
    public class VnPayService : IVnPayService

    {
        
        private readonly VnPaySettings _vnPaySettings;

        public VnPayService(IOptions<VnPaySettings> config)
        {
            _vnPaySettings = config.Value;
        }

        public string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel vnPaymentRequestModel)
        {
            var vnPay = new VnPayLibrary();
            vnPay.AddRequestData("vnp_Version", _vnPaySettings.Version);
            vnPay.AddRequestData("vnp_Command", _vnPaySettings.Command);
            vnPay.AddRequestData("vnp_TmnCode", _vnPaySettings.TmnCode);
            vnPay.AddRequestData("vnp_Amount", (vnPaymentRequestModel.Amount * 100).ToString());
            vnPay.AddRequestData("vnp_CreateDate", vnPaymentRequestModel.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnPay.AddRequestData("vnp_CurrCode", _vnPaySettings.CurrCode);
            vnPay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnPay.AddRequestData("vnp_Locale", _vnPaySettings.Locale);
            vnPay.AddRequestData("vnp_OrderInfo", "Pay for your order");
            vnPay.AddRequestData("vnp_OrderType", "other");
            vnPay.AddRequestData("vnp_TxnRef", vnPaymentRequestModel.OrderId);
            vnPay.AddRequestData("vnp_ReturnUrl", _vnPaySettings.PaymentBackReturnUrl);
            return vnPay.CreateRequestUrl(_vnPaySettings.BaseUrl, _vnPaySettings.HashSecret);
        }

        public VnPaymentResponseModel PaymentExecute(IQueryCollection collection)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in collection)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            var vnp_orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            var vnp_SecureHash = collection.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_TransactionStatus");
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _vnPaySettings.HashSecret);
            if (!checkSignature)
            {
                return new VnPaymentResponseModel
                {
                    Success = false,
                    OrderId = Convert.ToInt32(vnp_orderId) 
                };
            }
            
            return new VnPaymentResponseModel
            {
                Success = true,
                OrderId = Convert.ToInt32(vnp_orderId),
                ResponseCode = vnp_ResponseCode
            };
        }
    }
}