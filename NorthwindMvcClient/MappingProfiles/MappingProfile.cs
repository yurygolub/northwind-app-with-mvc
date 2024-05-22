using AutoMapper;
using Northwind.Services.Employees;
using Northwind.Services.Products;

namespace NorthwindMvcClient.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.CreateMap<Product, Models.Product>().ReverseMap();
        this.CreateMap<Employee, Models.Employee>().ReverseMap();
        this.CreateMap<ProductCategory, Models.ProductCategory>().ReverseMap();
    }
}
