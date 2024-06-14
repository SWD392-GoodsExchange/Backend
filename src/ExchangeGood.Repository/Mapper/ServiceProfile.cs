using AutoMapper;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Category;
using ExchangeGood.Contract.Payloads.Request.Product;
using ExchangeGood.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Repository.Mapper {
    public class ServiceProfile : Profile {
        public ServiceProfile() {
            CreateMap<Product, ProductDto>();
			CreateMap<Member, MemberDto>();
			CreateMap<Category, CategoryDto>();
			CreateMap<CreateProductRequest, Product>().ForMember(dest => dest.Price, opt => opt.MapFrom(src => decimal.Parse(src.Price)));
            CreateMap<UpdateProductRequest, Product>().ForMember(dest => dest.Price, opt => opt.MapFrom(src => decimal.Parse(src.Price)));
			CreateMap<CreateCategoryRequest, Category>().ForMember(dest => dest.CateName, opt => opt.MapFrom(src => src.CategoryName));
			CreateMap<UpdateCategoryRequest, Category>().ForMember(dest => dest.CateName, opt => opt.MapFrom(src => src.CategoryName));
		}
	}
}
