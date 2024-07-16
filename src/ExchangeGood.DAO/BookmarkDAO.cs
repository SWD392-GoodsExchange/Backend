using ExchangeGood.Contract.Payloads.Request.Bookmark;
using ExchangeGood.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeGood.DAO
{
    public class BookmarkDAO
    {
        private readonly GoodsExchangeContext _context;

        public BookmarkDAO(GoodsExchangeContext context)
        {
            _context = context;
        }

        public IQueryable<Bookmark> GetALl(string feId)
        {
            return _context.Bookmarks
                .Where(x => x.FeId == feId)
                .Include(x=>x.Product)
                    .ThenInclude(x => x.Cate)
                 .Include(x=> x.Product)
                    .ThenInclude(x => x.Images)
                .Include(x => x.Product)
                    .ThenInclude(x => x.Fe)
                .OrderBy(x => x.CreateTime).AsNoTracking();
        }
        public async Task<Bookmark> GetBookmark(Bookmark deleteBookmark)
        {
            return await _context.Bookmarks.FirstOrDefaultAsync(x =>
                x.FeId == deleteBookmark.FeId && x.ProductId == deleteBookmark.ProductId);
        }
        public void Add(Bookmark bookmark)
        {
            _context.Bookmarks.Add(bookmark);
        }
        
        public void Delete(Bookmark bookmark)
        {
            _context.Bookmarks.Remove(bookmark);
        }
    }
}