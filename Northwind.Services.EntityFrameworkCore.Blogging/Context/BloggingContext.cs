using Microsoft.EntityFrameworkCore;
using Northwind.Services.EntityFrameworkCore.Blogging.Entities;

namespace Northwind.Services.EntityFrameworkCore.Blogging.Context
{
    public class BloggingContext : DbContext
    {
        public BloggingContext(DbContextOptions<BloggingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BlogArticleEntity> BlogArticles { get; set; }

        public virtual DbSet<BlogArticleProductEntity> BlogArticleProducts { get; set; }

        public virtual DbSet<BlogCommentEntity> BlogComments { get; set; }
    }
}
