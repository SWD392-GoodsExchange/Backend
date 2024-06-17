using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Product;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Repository.Interfaces;
using ExchangeGood.Service.Interfaces;

namespace ExchangeGood.Service.UseCase {
    public class ProductService : IProductService { // Return BaseResponse
        private readonly IProductRepository _productRepository;
        private readonly IPhotoService _photoService;

        public ProductService(IProductRepository productRepository, IPhotoService photoService)
        {
            _productRepository = productRepository;
            _photoService = photoService;
        }

        public async Task<BaseResponse> AddProduct(CreateProductRequest createProductRequest)
        {
            // Call third-party to create picture
            var result = await _photoService.AddPhotoAsync(createProductRequest.File);
            if(result.Error != null) {
                throw new Exception("Upload images fail");
            }
            ImageDto imageDto = new ImageDto() {
                ImageUrl = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
            };
            createProductRequest.Image = imageDto;
            // create new product
            var product = await _productRepository.AddProduct(createProductRequest);
            if(product != null) {
                return BaseResponse.Success(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, product);
            }

            return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_CREATE_MSG);
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
            var product = await _productRepository.UpdateProduct(updateProductRequest);
            if(product != null) {
                return BaseResponse.Success(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
            }
            return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_UPDATE_MSG);
        }
    }
}

/*    
Note:
- Tách thằng SaveChanges() ra khởi thằng Service -> vì lỡ SaveChanges() fail thì quăng ra lỗi, mà lỗi đó là lỗi
của DB chứ ko phải Service
- Serive chỉ gọi thằng ở dưới ( SaveChanges ở dưới vì throw lỗi DB ) vì service có lỗi thì chỉ cần biết lỗi service
*/
