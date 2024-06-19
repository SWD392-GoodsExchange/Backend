using ExchangeGood.Contract.Payloads.Request.Category;
using ExchangeGood.Contract.Payloads.Request.Comment;
using ExchangeGood.Contract.Payloads.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Service.Interfaces
{
    public interface ICommentService
    {
        public Task<BaseResponse> GetAllComments();
        public Task<BaseResponse> GetCommentByID(int id);
        public Task<BaseResponse> AddComment(CreateCommentRequest createComment);
        public Task<BaseResponse> UpdateComment(UpdateCommentRequest updateComment);
        public Task<BaseResponse> DeleteComment(int id);
    }
}
