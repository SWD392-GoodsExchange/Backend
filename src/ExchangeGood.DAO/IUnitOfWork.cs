using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.DAO {
    public interface IUnitOfWork {
        public ProductDAO ProductDAO { get; }
        public MemberDAO MemberDAO { get; }
        public CategoryDAO CategoryDAO { get; }
        public Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
        public Task<int> SaveChangesWithTransactionAsync();
    }
}
