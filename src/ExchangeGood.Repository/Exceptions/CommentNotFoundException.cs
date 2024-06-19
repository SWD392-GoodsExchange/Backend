using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Repository.Exceptions
{
    public class CommentNotFoundException : NotFoundException
    {
        public CommentNotFoundException(int id) : base($"Comment with id {id} is not found.")
        {
        }
    }
}
