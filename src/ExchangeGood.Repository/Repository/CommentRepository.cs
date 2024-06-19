using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Comment;
using ExchangeGood.DAO;
using ExchangeGood.Data.Models;
using ExchangeGood.Repository.Exceptions;
using ExchangeGood.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Repository.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public CommentRepository(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<int> AddComment(CreateCommentRequest createComment)
        {
            var comment = _mapper.Map<Comment>(createComment);
            _uow.CommentDAO.AddComment(comment);

            await _uow.SaveChangesAsync();
            return comment.CommentId;
        }

        public async Task DeleteComment(int id)
        {
            Comment existedComment = await _uow.CommentDAO.GetCommentByIdAsync(id);
            if (existedComment == null)
            {
                throw new CommentNotFoundException(id);
            }
            _uow.CommentDAO.RemoveComment(existedComment);

            await _uow.SaveChangesAsync();
        }

        public async Task<List<CommentDto>> GetAllComments()
        {
            var query = _uow.CommentDAO.GetComments();

            var result = await query
                .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return result;
        }

        public async Task<CommentDto> GetCommentByID(int id)
        {
            var comment = await _uow.CommentDAO.GetCommentByIdAsync(id);
            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<int> UpdateComment(UpdateCommentRequest updateComment)
        {
            Comment existedComment = await _uow.CommentDAO.GetCommentByIdAsync(updateComment.CommentId);
            if (existedComment == null)
            {
                throw new CommentNotFoundException(updateComment.CommentId);
            }
            _mapper.Map(updateComment, existedComment);
            _uow.CommentDAO.UpdateComment(existedComment);

            await _uow.SaveChangesAsync();
            return existedComment.CommentId;
        }
    }
}
