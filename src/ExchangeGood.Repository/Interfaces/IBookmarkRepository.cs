using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Bookmark;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Data.Models;

namespace ExchangeGood.Repository.Interfaces
{
    public interface IBookmarkRepository
    {
        public Task<List<Bookmark>> GetAllBookmarks(string feId);
        public Task<bool> AddBookmark(CreateBookmarkRequest createBookmarkRequest);
        public Task<bool> DeleteBookmark(DeleteBookmarkRequest deleteBookmarkRequest);
    }
}