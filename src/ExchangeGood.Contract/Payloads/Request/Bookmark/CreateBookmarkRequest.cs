using System.Runtime.InteropServices.JavaScript;

namespace ExchangeGood.Contract.Payloads.Request.Bookmark
{
    public class CreateBookmarkRequest
    {
        public string FeId { get; set; }
        public int ProductId { get; set; }
    }
}