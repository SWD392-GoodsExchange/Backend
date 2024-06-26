﻿using ExchangeGood.Contract;
using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Product;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Data.Models;
using ExchangeGood.Repository.Exceptions;
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

        public async Task<Product> AddProduct(CreateProductRequest createProductRequest)
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
            
            // return BaseResponse.Success(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, product);
            return product;
        }

        public async Task DeleteProduct(int productId)
        {
            // Call third-party to create picture
            await _productRepository.DeleteProduct(productId);
        }

        public async Task<PagedList<ProductDto>> GetAllProducts(ProductParams productParams) {
            return await _productRepository.GetAllProducts(productParams);
            // return BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, new PaginationResponse<ProductDto>(result, result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));
        }

        public async Task<Product> GetProduct(int productId)
        {
            return await _productRepository.GetProduct(productId); 
        }

        public async Task<IEnumerable<Product>> GetProductsForExchangeRequest(GetProductsForExchangeRequest request)
        {
            var result = await _productRepository.GetProductsForExchange(request.ProductIds);

            if(result.FirstOrDefault(x => x.FeId.Equals(request.FeId)) == null) {
                throw new BadRequestException("Your product has already exchanged before.");
            }

            if(result.Where(x => !x.FeId.Equals(request.FeId)).Count() < 1) {
                throw new BadRequestException("Products of exchanger have already exchange before.");
            }

            return result;
        }

        public async Task<Product> UpdateProduct(UpdateProductRequest updateProductRequest)
        {
            // Call third-party to create picture
            return await _productRepository.UpdateProduct(updateProductRequest);
        }
    }
}

/*    
Note:
- Tách thằng SaveChanges() ra khởi thằng Service -> vì lỡ SaveChanges() fail thì quăng ra lỗi, mà lỗi đó là lỗi
của DB chứ ko phải Service
- Serive chỉ gọi thằng ở dưới ( SaveChanges ở dưới vì throw lỗi DB ) vì service có lỗi thì chỉ cần biết lỗi service
*/
