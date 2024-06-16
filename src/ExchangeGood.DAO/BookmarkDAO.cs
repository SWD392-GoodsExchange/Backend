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
                .OrderBy(x => x.CreateTime).AsNoTracking();
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