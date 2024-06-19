using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Contract.Payloads.Request.Comment
{
    public class CreateCommentRequest
    {
        public int CommentId { get; set; }
        public string FeId { get; set; }
        public int ProductId { get; set; }
        public string Content { get; set; }
    }
}
