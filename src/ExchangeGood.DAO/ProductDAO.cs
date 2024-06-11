using ExchangeGood.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.DAO {
    public class ProductDAO { // return Entity
        private readonly GoodsExchangeContext _context;

        public ProductDAO(GoodsExchangeContext context) {
            _context = context;
        }

        public void AddProduct(Product product) {
            _context.Products.Add(product);
        }

        public async Task<Product> GetProductByIdAsync(int id) {
            return await _context.Products.FindAsync(id);
        }

        public IQueryable<Product> GetProducts(string keyword) {
            var query = _context.Products
                .Include(p => p.Cate)
                .AsQueryable();

            // Add another logic later
            query = query.Where(x => x.Title == keyword);
            query = query.Where(x => x.Cate.CateName == keyword);

            // Add thu tu cua list later

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
