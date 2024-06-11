using System.Runtime.InteropServices;
using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Product;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Repository.Interfaces;
using ExchangeGood.Service.Interfaces;

namespace ExchangeGood.Service.UseCase {
    public class ProductService : IProductService { // Return BaseResponse
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<BaseResponse> AddProduct(CreateProductRequest createProductRequest)
        {
            // get FeId
            // Call third-party to create picture
            await _productRepository.AddProduct(createProductRequest);
            return BaseResponse.Success(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
        }

        public async Task<BaseResponse> DeleteProduct(int productId)
        {
            // Call third-party to create picture
            await _productRepository.DeleteProduct(productId);
            return BaseResponse.Success(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
        }

        public async Task<BaseResponse> GetAllProducts(ProductParams productParams) {
            var result = await _productRepository.GetAllProducts(productParams);
         
            if (result.TotalCount > 0) {
                return BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, new PaginationResponse<ProductDto>(result, result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));
            }

            return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG);
        }

        public async Task<BaseResponse> GetProduct(int productId)
        {
           var result = await _productRepository.GetProduct(productId);
         
            if (result != null) {
                return BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }

            return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG);
        }

        public async Task<BaseResponse> UpdateProduct(UpdateProductRequest updateProductRequest)
        {
            // Call third-party to create picture
            await _productRepository.UpdateProduct(updateProductRequest);
            return BaseResponse.Success(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
        }
    }
}

/*
            Note:
        - Tách thằng SaveChanges() ra khởi thằng Service -> vì lỡ SaveChanges() fail thì quăng ra lỗi, mà lỗi đó là lỗi
        của DB chứ ko phải Service
        - Serive chỉ gọi thằng ở dưới ( SaveChanges ở dưới vì throw lỗi DB ) vì service có lỗi thì chỉ cần biết lỗi service
        */
