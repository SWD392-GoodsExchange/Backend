using AutoMapper;
using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.Payloads.Request.Product;
using ExchangeGood.DAO;
using ExchangeGood.Contract.DTOs;
using AutoMapper.QueryableExtensions;
using ExchangeGood.Repository.Interfaces;
using ExchangeGood.Data.Models;
using ExchangeGood.Repository.Exceptions;
using ExchangeGood.Contract.Enum.Product;
using Azure;
using System.Linq.Expressions;

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

        public async Task<Product> AddProduct(CreateProductRequest productRequest) {
            // vô được tới đây có nghĩa CreateProductRequest đã được Validation
            // get member
            // get category
            var product = _mapper.Map<Product>(productRequest); // map to create new Product
            product.CreatedTime = DateTime.UtcNow;
            product.UpdatedTime = DateTime.UtcNow;
            product.Status = Status.Selling.Name;
            Image image = new Image() {
                PublicId = productRequest.Image.PublicId,
                ImageUrl = productRequest.Image.ImageUrl,
            };
            product.Images.Add(image);

            _uow.ProductDAO.AddProduct(product);

            if(await _uow.SaveChangesAsync()) {
                return product;
            }
            return null;
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
            var query = _uow.ProductDAO.GetProducts(productParams.Keyword, productParams.Type, productParams.Orderby);

            return await PagedList<ProductDto>.CreateAsync(query.ProjectTo<ProductDto>(_mapper.ConfigurationProvider),
            productParams.PageNumber, productParams.PageSize);
        }

        public async Task<Product> GetProduct(int productId, params Expression<Func<Product, object>>[] includeProperties) {
            return await _uow.ProductDAO.GetProductByIdAsync(productId, includeProperties);
        }

        public async Task<IEnumerable<Product>> GetProductsByFeId(string feId, string type = null, bool includeDetail = false) {
            return await _uow.ProductDAO.GetProductsByFeId(feId, type, includeDetail: true);
        }

        public async Task<IEnumerable<Product>> GetProductsByCateId(int cateId)
        {
            return await _uow.ProductDAO.GetProductsByCateId(cateId, includeDetail: true);
        } 

        public async Task<IEnumerable<Product>> GetProductsForExchange(IEnumerable<int> productIds)
        {
            return await _uow.ProductDAO.GetProductsForExchange(productIds);    
        }

        public async Task<Product> UpdateProduct(UpdateProductRequest productRequest)
        {
            Product existedProduct = await _uow.ProductDAO.GetProductByIdAsync(productRequest.ProductId);
            if (existedProduct == null)
            {
                throw new ProductNotFoundException(productRequest.ProductId);
            }

            var currentImage = existedProduct.Images;

            _mapper.Map(productRequest, existedProduct);

            if (productRequest.Image == null)
            {
                existedProduct.Images = currentImage;
            }
            else
            {
                Image image = new Image()
                {
                    PublicId = productRequest.Image.PublicId,
                    ImageUrl = productRequest.Image.ImageUrl,
                };
                existedProduct.Images.Add(image);
            }

            existedProduct.UpdatedTime = DateTime.UtcNow;

            _uow.ProductDAO.UpdateProduct(existedProduct);

            if (await _uow.SaveChangesAsync())
            {
                return existedProduct;
            }
            return null;
        }

    }
}
