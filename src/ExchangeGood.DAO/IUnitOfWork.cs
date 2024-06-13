using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.DAO {
    public interface IUnitOfWork {
        public ProductDAO ProductDAO { get; }
        public MemberDAO MemberDAO { get; }
        public Task SaveChangesAsync(CancellationToken cancellationToken = default);
        public Task<int> SaveChangesWithTransactionAsync();
    }
}
