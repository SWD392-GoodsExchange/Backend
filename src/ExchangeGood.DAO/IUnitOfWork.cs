using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.DAO {
    public interface IUnitOfWork {
        public ProductDAO ProductDAO { get; }
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
