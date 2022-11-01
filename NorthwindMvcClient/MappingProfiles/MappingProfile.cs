using AutoMapper;
using Northwind.Services.Products;

namespace NorthwindMvcClient.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Product, Models.Product>().ReverseMap();
        }
    }
}
