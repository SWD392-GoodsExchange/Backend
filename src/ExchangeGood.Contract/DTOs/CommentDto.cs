using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Contract.DTOs
{
    public class CommentDto
    {
        public int CommentId { get; set; }
        public string FeId { get; set; }
        public int ProductId { get; set; }
        public string Content { get; set; }
    }
}
