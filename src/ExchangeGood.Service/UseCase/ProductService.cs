using ExchangeGood.Contract;
using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Product;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Data.Models;
using ExchangeGood.Repository.Exceptions;
using ExchangeGood.Repository.Interfaces;
using ExchangeGood.Service.Interfaces;
using System.Linq.Expressions;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using ExchangeGood.Repository.Repository;

namespace ExchangeGood.Service.UseCase
{
    public class ProductService : IProductService
    { // Return BaseResponse
        private readonly IProductRepository _productRepository;
        private readonly IPhotoService _photoService;
        private readonly SmtpSettings _smtpSetting;
        private readonly IMemberService _memberService;

        public ProductService(IProductRepository productRepository, IPhotoService photoService, IOptions<SmtpSettings> smtpSetting, IMemberService memberService)
        {
            _productRepository = productRepository;
            _photoService = photoService;
            _memberService = memberService;
            _smtpSetting = smtpSetting.Value;
        }

        public async Task<Product> AddProduct(CreateProductRequest createProductRequest)
        {
            if (createProductRequest.Type.Equals(Contract.Enum.Product.Type.Trade.Name) &&
                createProductRequest.Price.Equals("0"))
            {
                throw new BadRequestException("Price must be larger than 0");
            }
            // Call third-party to create picture
            var result = await _photoService.AddPhotoAsync(createProductRequest.File);
            if (result.Error != null)
            {
                throw new Exception("Upload images fail");
            }
            ImageDto imageDto = new ImageDto()
            {
                ImageUrl = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
            };
            createProductRequest.Image = imageDto;
            // create new product
            var product = await _productRepository.AddProduct(createProductRequest);

            await SendProductAddedEmail(createProductRequest.FeId, createProductRequest.Title);

            // return BaseResponse.Success(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, product);
            return product;
        }

        public async Task DeleteProduct(int productId)
        {
            // Call third-party to create picture
            await _productRepository.DeleteProduct(productId);
        }

        public async Task<PagedList<ProductDto>> GetAllProducts(ProductParams productParams)
        {
            return await _productRepository.GetAllProducts(productParams);
            // return BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, new PaginationResponse<ProductDto>(result, result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));
        }

        public async Task<Product> GetProduct(int productId, params Expression<Func<Product, object>>[] includeProperties)
        {
            return await _productRepository.GetProduct(productId, includeProperties);
        }

        public async Task<IEnumerable<Product>> GetProductsByFeId(string feId, string type = null, bool includeDetail = false)
        {
            return await _productRepository.GetProductsByFeId(feId, type);
        }

        public async Task<IEnumerable<Product>> GetProductsByCateId(int cateId)
        {
            return await _productRepository.GetProductsByCateId(cateId);
        }

        public async Task<IEnumerable<Product>> GetProductsForExchangeRequest(GetProductsForExchangeRequest request)
        {
            var result = await _productRepository.GetProductsForExchange(request.ProductIds);

            if (result.Count() < 1)
            {
                throw new BadRequestException("Products of this request not found");
            }

            if (!result.Any(x => x.FeId == request.ExchangerId))
            {
                throw new BadRequestException("Your products were not found.");
            }

            if (result.SingleOrDefault(x => x.FeId == request.OwnerId) == null)
            {
                throw new BadRequestException("Product of exchanger were not found.");
            }

            return result;
        }

        public async Task<Product> UpdateProduct(UpdateProductRequest updateProductRequest)
        {
            // Call third-party to create picture
            if (updateProductRequest.File != null)
            {
                var result = await _photoService.AddPhotoAsync(updateProductRequest.File);
                if (result.Error != null)
                {
                    throw new Exception("Upload images fail");
                }
                ImageDto imageDto = new ImageDto()
                {
                    ImageUrl = result.SecureUrl.AbsoluteUri,
                    PublicId = result.PublicId,
                };
                updateProductRequest.Image = imageDto;
            }

            await SendProductUpdatedEmail(updateProductRequest.FeId, updateProductRequest.Title);

            return await _productRepository.UpdateProduct(updateProductRequest);
        }

        private async Task SendProductAddedEmail(string feId, string productName)
        {
            var member = await _memberService.GetMemberByFeId(feId);
            if (member == null || string.IsNullOrEmpty(member.Email))
            {
                throw new Exception($"Member with FeId {feId} not found or has no email specified.");
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ExchangeGood System", _smtpSetting.Username));
            message.To.Add(new MailboxAddress(member.UserName, member.Email));
            message.Subject = "New Product Added";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $@"
        <p>Dear {member.UserName},</p>
        <p>We are pleased to inform you that your product <strong>{productName}</strong> has been successfully added to ExchangeGood.</p>
        <p>Thank you for using our platform!</p>
        <p>Best regards,</p>
        <p>The ExchangeGood Team</p>
    ";

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_smtpSetting.SmtpServer, _smtpSetting.Port, _smtpSetting.UseSsl);
                    client.Authenticate(_smtpSetting.Username, _smtpSetting.Password);
                    await client.SendAsync(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email: {ex.Message}");
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }

        private async Task SendProductUpdatedEmail(string feId, string productName)
        {
            var member = await _memberService.GetMemberByFeId(feId);
            if (member == null || string.IsNullOrEmpty(member.Email))
            {
                throw new Exception($"Member with FeId {feId} not found or has no email specified.");
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ExchangeGood System", _smtpSetting.Username));
            message.To.Add(new MailboxAddress(member.UserName, member.Email));
            message.Subject = "Product Updated";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $@"
        <p>Dear {member.UserName},</p>
        <p>We are writing to inform you that your product <strong>{productName}</strong> has been successfully updated on ExchangeGood.</p>
        <p>Thank you for using our platform!</p>
        <p>Best regards,</p>
        <p>The ExchangeGood Team</p>
    ";

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_smtpSetting.SmtpServer, _smtpSetting.Port, _smtpSetting.UseSsl);
                    client.Authenticate(_smtpSetting.Username, _smtpSetting.Password);
                    await client.SendAsync(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email: {ex.Message}");
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }

    }
}

/*    
Note:
- Tách thằng SaveChanges() ra khởi thằng Service -> vì lỡ SaveChanges() fail thì quăng ra lỗi, mà lỗi đó là lỗi
của DB chứ ko phải Service
- Serive chỉ gọi thằng ở dưới ( SaveChanges ở dưới vì throw lỗi DB ) vì service có lỗi thì chỉ cần biết lỗi service
*/
