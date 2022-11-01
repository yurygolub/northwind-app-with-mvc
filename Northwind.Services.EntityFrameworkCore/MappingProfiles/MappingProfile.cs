using AutoMapper;
using Northwind.Services.Employees;
using Northwind.Services.EntityFrameworkCore.Entities;
using Northwind.Services.Products;

namespace Northwind.Services.EntityFrameworkCore.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Employee, EmployeeEntity>().ReverseMap();
            this.CreateMap<Product, ProductEntity>()
                .ForMember(m => m.ProductId, opt => opt.MapFrom(src => src.Id))
                .ForMember(m => m.ProductName, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();

            this.CreateMap<ProductCategory, CategoryEntity>()
                .ForMember(m => m.CategoryId, opt => opt.MapFrom(src => src.Id))
                .ForMember(m => m.CategoryName, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();
        }
    }
}
