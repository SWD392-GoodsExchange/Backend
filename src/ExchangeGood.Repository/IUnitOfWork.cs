using ExchangeGood.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Repository
{
    public interface IUnitOfWork
    {
        public ProductDAO ProductDAO { get; }
        public MemberDAO MemberDAO { get; }
        public CategoryDAO CategoryDAO { get; }
		public ReportDAO ReportDAO { get; }
		public OrderDAO OrderDAO { get; }
        public CommentDAO CommentDAO { get; }
        public BookmarkDAO BookmarkDAO { get; }
        public Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
        public IDbTransaction BeginTransaction();
        public Task<int> SaveChangesWithTransactionAsync();
    }
}