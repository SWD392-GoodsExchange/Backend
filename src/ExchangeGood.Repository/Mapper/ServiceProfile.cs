using AutoMapper;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Category;
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

namespace ExchangeGood.Repository.Mapper {
    public class ServiceProfile : Profile {
        public ServiceProfile() {
            CreateMap<Product, ProductDto>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Cate.CateName));
			CreateMap<Member, MemberDto>();
			CreateMap<Category, CategoryDto>();
            CreateMap<Image, ImageDto>().ReverseMap();
            CreateMap<CreateBookmarkRequest, Bookmark>();
            CreateMap<CreateProductRequest, Product>().ForMember(dest => dest.Price, opt => opt.MapFrom(src => decimal.Parse(src.Price)));
            CreateMap<UpdateProductRequest, Product>().ForMember(dest => dest.Price, opt => opt.MapFrom(src => decimal.Parse(src.Price)));
			CreateMap<CreateCategoryRequest, Category>().ForMember(dest => dest.CateName, opt => opt.MapFrom(src => src.CategoryName));
			CreateMap<UpdateCategoryRequest, Category>().ForMember(dest => dest.CateName, opt => opt.MapFrom(src => src.CategoryName));
            CreateMap<Bookmark, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Product.Cate.CateName))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.UsageInformation, opt => opt.MapFrom(src => src.Product.UsageInformation))
                .ForMember(dest => dest.FeId, opt => opt.MapFrom(src => src.Product.FeId))
                .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src.Product.Origin))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Product.Status))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Product.Title));
        }
	}
}
