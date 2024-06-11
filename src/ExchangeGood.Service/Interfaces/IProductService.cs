using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.Payloads.Request.Product;
using ExchangeGood.Contract.Payloads.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Service.Interfaces {
    public interface IProductService {
        public Task<BaseResponse> GetAllProducts(ProductParams productParams);
        public Task<BaseResponse> GetProduct(int ProductId);
        public Task<BaseResponse> AddProduct(CreateProductRequest createProductRequest);
        public Task<BaseResponse> UpdateProduct(UpdateProductRequest updateProductRequest);
        public Task<BaseResponse> DeleteProduct(int ProductId);
    }
}
