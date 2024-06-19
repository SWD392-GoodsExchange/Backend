using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Comment;
using ExchangeGood.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Repository.Interfaces
{
    public interface ICommentRepository
    {
        public Task<List<CommentDto>> GetAllComments();
        public Task<CommentDto> GetCommentByID(int id);
        public Task<int> AddComment(CreateCommentRequest createComment);
        public Task<int> UpdateComment(UpdateCommentRequest updateComment);
        public Task DeleteComment(int id);
    }
}
