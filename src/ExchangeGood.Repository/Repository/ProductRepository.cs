using AutoMapper;
using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.Payloads.Request.Product;
using ExchangeGood.DAO;
using ExchangeGood.Contract.DTOs;
using AutoMapper.QueryableExtensions;
using ExchangeGood.Repository.Interfaces;
using ExchangeGood.Data.Models;
using ExchangeGood.Repository.Exceptions;

namespace ExchangeGood.Repository.Repository
{
    internal class ProductRepository : IProductRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ProductRepository(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<int> AddProduct(CreateProductRequest productRequest) {
            // vô được tới đây có nghĩa CreateProductRequest đã được Validation
            var product = _mapper.Map<Product>(productRequest);
            _uow.ProductDAO.AddProduct(product);

            await _uow.SaveChangesAsync();
            return product.ProductId;
        }

        public async Task DeleteProduct(int productId) {
            Product existedProduct = await _uow.ProductDAO.GetProductByIdAsync(productId);
            if (existedProduct == null) {
                throw new ProductNotFoundException(productId);
            }
            _uow.ProductDAO.RemoveProduct(existedProduct);

            await _uow.SaveChangesAsync();
        }

        public async Task<PagedList<ProductDto>> GetAllProducts(ProductParams productParams) {
            var query = _uow.ProductDAO.GetProducts(productParams.Keyword);

            var result = await PagedList<ProductDto>.CreateAsync(query.ProjectTo<ProductDto>(_mapper.ConfigurationProvider),
            productParams.PageNumber, productParams.PageSize);

            return result;
        }

        public async Task<ProductDto> GetProduct(int productId) {
            var product = await _uow.ProductDAO.GetProductByIdAsync(productId);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<int> UpdateProduct(UpdateProductRequest productRequest)
        {
            Product existedProduct = await _uow.ProductDAO.GetProductByIdAsync(productRequest.ProductId);
            if(existedProduct == null) {
                 throw new ProductNotFoundException(productRequest.ProductId);
            }
            _mapper.Map(productRequest, existedProduct);
            _uow.ProductDAO.UpdateProduct(existedProduct);

            await _uow.SaveChangesAsync();
            return existedProduct.ProductId;
        }
    }
}
