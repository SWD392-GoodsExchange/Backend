using System.Runtime.InteropServices.JavaScript;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Bookmark;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.DAO;
using ExchangeGood.Data.Models;
using ExchangeGood.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExchangeGood.Repository.Repository
{
    public class BookmarkRepository : IBookmarkRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public BookmarkRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _uow = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> GetAllBookmarks(string feId)
        {
            var query = _uow.BookmarkDAO.GetALl(feId);
            var list = await query.ProjectTo<ProductDto>(_mapper.ConfigurationProvider).ToListAsync();
            return list;
        }

        public async Task<bool> AddBookmark(CreateBookmarkRequest createBookmarkRequest)
        {
            var bookmark = _mapper.Map<Bookmark>(createBookmarkRequest);
            bookmark.CreateTime = DateTime.UtcNow;
            _uow.BookmarkDAO.Add(bookmark);
            return await _uow.SaveChangesAsync();
        }

        public async Task<bool> DeleteBookmark(DeleteBookmarkRequest deleteBookmarkRequest)
        {
            Bookmark bookmark = _mapper.Map<Bookmark>(deleteBookmarkRequest);
            Bookmark bookmarkDelete = await _uow.BookmarkDAO.GetBookmark(bookmark);
            _uow.BookmarkDAO.Delete(bookmarkDelete);
            return await _uow.SaveChangesAsync();
        }
    }
}