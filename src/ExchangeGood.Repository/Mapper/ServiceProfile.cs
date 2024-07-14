using AutoMapper;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Category;
using ExchangeGood.Contract.Payloads.Request.Comment;
using ExchangeGood.Contract.Payloads.Request.Product;
using ExchangeGood.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using ExchangeGood.Contract.Payloads.Request.Bookmark;
using ExchangeGood.Contract.Payloads.Request.Report;
using ExchangeGood.Contract.Payloads.Request.Notification;
using ExchangeGood.Contract.Common;

namespace ExchangeGood.Repository.Mapper {
    public class ServiceProfile : Profile {
        public ServiceProfile() {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Cate.CateName))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => AvatarImage.GetImage(src.FeId)))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Fe.UserName));
			CreateMap<Member, MemberDto>();
			CreateMap<Category, CategoryDto>();
			CreateMap<Report, ReportDto>();
			CreateMap<Notification, NotificationDto>().ReverseMap();
			CreateMap<Image, ImageDto>();
            CreateMap<CreateBookmarkRequest, Bookmark>().ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => Int32.Parse(src.ProductId)));
            CreateMap<DeleteBookmarkRequest, Bookmark>().ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => Int32.Parse(src.ProductId)));
            CreateMap<CreateProductRequest, Product>().ForMember(dest => dest.Price, opt => opt.MapFrom(src => decimal.Parse(src.Price)));
            CreateMap<UpdateProductRequest, Product>().ForMember(dest => dest.Price, opt => opt.MapFrom(src => decimal.Parse(src.Price)));
			CreateMap<CreateCategoryRequest, Category>().ForMember(dest => dest.CateName, opt => opt.MapFrom(src => src.CategoryName));
			CreateMap<UpdateCategoryRequest, Category>().ForMember(dest => dest.CateName, opt => opt.MapFrom(src => src.CategoryName));
            CreateMap<Bookmark, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Product.Cate.CateName))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Product.Description))
                .ForMember(dest => dest.FeId, opt => opt.MapFrom(src => src.Product.FeId))
                .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src.Product.Origin))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Product.Status))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Product.Type))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => AvatarImage.GetImage(src.FeId)))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Product.Title));
            CreateMap<CreateReportRequest, Report>().ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message));
			CreateMap<UpdateReportRequest, Report>().ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Status));
			CreateMap<UpdateNotificationRequest, Notification>().ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content));
		}
	}
}
