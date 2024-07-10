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
        Task<PagedList<ProductDto>> GetAllProducts(ProductParams productParams);
        Task<IEnumerable<Product>> GetProductsByFeId(string feId);
        Task<IEnumerable<Product>> GetProductsByCateId(int cateId);
        Task<Product> GetProduct(int ProductId);
        Task<IEnumerable<Product>> GetProductsForExchangeRequest(GetProductsForExchangeRequest request);
        Task<Product> AddProduct(CreateProductRequest createProductRequest);
        Task<Product> UpdateProduct(UpdateProductRequest updateProductRequest);
        Task DeleteProduct(int ProductId);
	}
}
