using ExchangeGood.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.DAO
{
    public class CommentDAO
    {
        private readonly GoodsExchangeContext _context;

        public CommentDAO(GoodsExchangeContext context)
        {
            _context = context;
        }

        public void AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
        }

        public async Task<Comment> GetCommentByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }
        public async Task<Comment> GetCommentByContentAsync(string comtCont)
        {
            return await _context.Comments.FirstOrDefaultAsync(c => c.Content == comtCont);
        }

        public async Task<Comment> GetCommentByStudentIdAsync(string FeId)
        {
            return await _context.Comments.FirstOrDefaultAsync(c => c.FeId == FeId);
        }

        public IQueryable<Comment> GetComments()
        {
            var query = _context.Comments
                .AsQueryable()
                .AsNoTracking();

            return query.AsNoTracking();
        }

        public void RemoveComment(Comment comment)
        {
            _context.Comments.Remove(comment);
        }

        public void UpdateComment(Comment comment)
        {
            _context.Comments.Update(comment);
        }
    }
}

