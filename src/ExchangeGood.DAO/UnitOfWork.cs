using ExchangeGood.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.DAO {
    public class UnitOfWork : IUnitOfWork {
        private readonly GoodsExchangeContext _context;
        private ProductDAO _productDAO;
        private MemberDAO _memberDAO;
        private CategoryDAO _categoryDAO;
        private OrderDAO _orderDAO;
        private BookmarkDAO _bookmarkDAO;
        public UnitOfWork(GoodsExchangeContext context)
        {
            _context = context;
        }

        public CategoryDAO CategoryDAO => _categoryDAO = new CategoryDAO(_context);
        public BookmarkDAO BookmarkDAO => _bookmarkDAO =  new BookmarkDAO(_context);
        public ProductDAO ProductDAO => _productDAO = new ProductDAO(_context);
        public MemberDAO MemberDAO => _memberDAO = new MemberDAO(_context);

        public OrderDAO OrderDAO => _orderDAO = new OrderDAO(_context);

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default) {
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<int> SaveChangesWithTransactionAsync() {
            int result = -1;

            //System.Data.IsolationLevel.Snapshot
            using (var dbContextTransaction = _context.Database.BeginTransaction()) {
                try {
                    result = await _context.SaveChangesAsync();
                    dbContextTransaction.Commit();
                }
                catch (Exception) {
                    //Log Exception Handling message                      
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }
            return result;
        }
    }
}
