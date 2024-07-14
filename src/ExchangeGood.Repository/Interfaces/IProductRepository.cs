using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Product;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Repository.Interfaces {
    public interface IProductRepository { // Return DTOs
        public Task<PagedList<ProductDto>> GetAllProducts(ProductParams productParams);
        public Task<IEnumerable<Product>> GetProductsByFeId(string feId);
        public Task<Product> GetProduct(int productId, params Expression<Func<Product, object>>[] includeProperties);
        public Task<IEnumerable<Product>> GetProductsByCateId(int cateId);
        public Task<IEnumerable<Product>> GetProductsForExchange(IEnumerable<int> productIds);
        public Task<Product> AddProduct(CreateProductRequest productRequest);
        public Task<Product> UpdateProduct(UpdateProductRequest prodductRequest);
        public Task DeleteProduct(int productId);
    } 
}
