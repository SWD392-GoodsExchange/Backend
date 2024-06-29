using ExchangeGood.Data.Models;
using Microsoft.EntityFrameworkCore;
using ExchangeGood.Contract.Enum.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeGood.Contract.Common;

namespace ExchangeGood.DAO {
    public class ProductDAO { // return Entity
        private readonly GoodsExchangeContext _context;

        public ProductDAO(GoodsExchangeContext context) {
            _context = context;
        }

        public void AddProduct(Product product) {
            _context.Products.Add(product);
        }

        public async Task<Product> GetProductByIdAsync(int id, bool includeDetail = false) {

            var query = _context.Products.AsQueryable();
            // for view product so that use .AsNoTracking() otherwise use for business logic
            if(includeDetail) {
                query = query
                    .Include(x => x.Cate)
                    .Include(x => x.Images)
                    .AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(x => x.ProductId == id);   
        }

        public async Task<IEnumerable<Product>> GetProductsForExchange(IEnumerable<int> productIds) {
            return await _context.Products
                .Include(x => x.Cate)
                .Include(x => x.Images)
                .Where(x => productIds.Contains(x.ProductId))
                .ToListAsync();
        }

        public IQueryable<Product> GetProducts(string keyword, string type, string orderBy) {
            var query = _context.Products
                .Include(p => p.Cate)
                .Include(p => p.Images)
                .AsQueryable();

            query = query.Where(p => p.Type.ToLower().Equals(type));
            query = query.Where(p => p.Status.Equals(Status.Selling.Name));

            // Add another logic later
            if(!string.IsNullOrEmpty(keyword)) {
                query = query.Where(x => x.Title.ToLower().Contains(keyword.ToLower().Trim()));
            }

            query = orderBy switch {
                "created" => query.OrderByDescending(u => u.CreatedTime),
                "price" => query.OrderByDescending(u => u.Price),
                _ => query.OrderByDescending(u => u.UpdatedTime)
            };

            return query.AsNoTracking();
        }

        public void RemoveProduct(Product product) {
            _context.Products.Remove(product);
        }

        public void UpdateProduct(Product product) {
            _context.Products.Update(product);
        }
    }
}
