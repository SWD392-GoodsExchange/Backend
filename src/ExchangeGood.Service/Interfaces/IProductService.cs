using ExchangeGood.Contract;
using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Product;
using ExchangeGood.Contract.Payloads.Request.Report;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Service.Interfaces {
    public interface IProductService {
        public Task<PagedList<ProductDto>> GetAllProducts(ProductParams productParams);
        public Task<Product> GetProduct(int ProductId);
        public Task<IEnumerable<Product>> GetProductsForExchangeRequest(GetProductsForExchangeRequest request);
        public Task<Product> AddProduct(CreateProductRequest createProductRequest);
        public Task<Product> UpdateProduct(UpdateProductRequest updateProductRequest);
        public Task DeleteProduct(int ProductId);
	}
}
