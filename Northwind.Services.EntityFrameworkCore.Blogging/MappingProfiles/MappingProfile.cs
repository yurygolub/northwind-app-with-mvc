using AutoMapper;
using Northwind.Services.Blogging;
using Northwind.Services.EntityFrameworkCore.Blogging.Entities;

namespace Northwind.Services.EntityFrameworkCore.Blogging.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<BlogArticle, BlogArticleEntity>().ReverseMap();
            this.CreateMap<BlogComment, BlogCommentEntity>().ReverseMap();
        }
    }
}
