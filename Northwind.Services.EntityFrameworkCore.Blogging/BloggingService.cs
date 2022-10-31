using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Northwind.Services.Blogging;
using Northwind.Services.EntityFrameworkCore.Blogging.Context;
using Northwind.Services.EntityFrameworkCore.Blogging.Entities;

namespace Northwind.Services.EntityFrameworkCore.Blogging
{
    public class BloggingService : IBloggingService
    {
        private readonly BloggingContext context;
        private readonly IMapper mapper;

        public BloggingService(IDesignTimeDbContextFactory<BloggingContext> factory, IMapper mapper)
        {
            _ = factory ?? throw new ArgumentNullException(nameof(factory));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            this.context = factory.CreateDbContext(null);
        }

        public async IAsyncEnumerable<BlogArticle> GetBlogArticlesAsync(int offset, int limit)
        {
            var blogArticles = this.context.BlogArticles
                    .Skip(offset)
                    .Take(limit)
                    .Select(b => this.mapper.Map<BlogArticle>(b));

            await foreach (var blogArticle in blogArticles.AsAsyncEnumerable())
            {
                yield return blogArticle;
            }
        }

        public async Task<BlogArticle> GetBlogArticleAsync(int blogArticleId)
        {
            var blogArticleEntity = await this.context.BlogArticles.FindAsync(blogArticleId);
            if (blogArticleEntity is null)
            {
                return null;
            }

            return this.mapper.Map<BlogArticle>(blogArticleEntity);
        }

        public async Task<int> CreateBlogArticleAsync(BlogArticle blogArticle)
        {
            _ = blogArticle ?? throw new ArgumentNullException(nameof(blogArticle));

            BlogArticleEntity blogArticleEntity = this.mapper.Map<BlogArticleEntity>(blogArticle);

            blogArticleEntity.Posted = DateTime.Now;

            await this.context.BlogArticles.AddAsync(blogArticleEntity);
            await this.context.SaveChangesAsync();

            return blogArticleEntity.Id;
        }

        public async Task<bool> DeleteBlogArticleAsync(int blogArticleId)
        {
            var blogArticle = await this.context.BlogArticles.FindAsync(blogArticleId);
            if (blogArticle != null)
            {
                this.context.BlogArticles.Remove(blogArticle);
                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateBlogArticleAsync(int blogArticleId, BlogArticle blogArticle)
        {
            _ = blogArticle ?? throw new ArgumentNullException(nameof(blogArticle));

            var blogArticleEntity = await this.context.BlogArticles.FindAsync(blogArticleId);
            if (blogArticleEntity is null)
            {
                return false;
            }

            blogArticleEntity.Title = blogArticle.Title;
            blogArticleEntity.Text = blogArticle.Text;
            blogArticleEntity.Posted = DateTime.Now;

            await this.context.SaveChangesAsync();
            return true;
        }

        public async IAsyncEnumerable<int> GetAllRelatedProductIdsAsync(int articleId, int offset, int limit)
        {
            var productIds = this.context.BlogArticleProducts
                .Where(b => b.BlogArticleId == articleId)
                .Skip(offset)
                .Take(limit)
                .Select(b => b.ProductId);

            await foreach (var productId in productIds.AsAsyncEnumerable())
            {
                yield return productId;
            }
        }

        public async Task<int> CreateProductLinkAsync(int articleId, int productId)
        {
            var blogArticleProductEntity = new BlogArticleProductEntity()
            {
                BlogArticleId = articleId,
                ProductId = productId,
            };

            await this.context.BlogArticleProducts.AddAsync(blogArticleProductEntity);
            await this.context.SaveChangesAsync();

            return blogArticleProductEntity.Id;
        }

        public async Task<bool> RemoveProductLinkAsync(int articleId, int productId)
        {
            var blogArticleProductEntity = await this.context.BlogArticleProducts
                .FirstAsync(b => (b.BlogArticleId == articleId) && (b.ProductId == productId));

            if (blogArticleProductEntity != null)
            {
                this.context.BlogArticleProducts.Remove(blogArticleProductEntity);
                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async IAsyncEnumerable<BlogComment> GetAllBlogCommentsAsync(int articleId, int offset, int limit)
        {
            var blogComments = this.context.BlogComments
                .Where(b => b.BlogArticleId == articleId)
                .Skip(offset)
                .Take(limit)
                .Select(b => this.mapper.Map<BlogComment>(b));

            await foreach (var blogComment in blogComments.AsAsyncEnumerable())
            {
                yield return blogComment;
            }
        }

        public async Task<int> CreateBlogCommentAsync(int articleId, BlogComment blogComment)
        {
            _ = blogComment ?? throw new ArgumentNullException(nameof(blogComment));

            var blogCommentEntity = this.mapper.Map<BlogCommentEntity>(blogComment);
            blogCommentEntity.BlogArticleId = articleId;
            blogCommentEntity.Posted = DateTime.Now;

            await this.context.BlogComments.AddAsync(blogCommentEntity);
            await this.context.SaveChangesAsync();

            return blogCommentEntity.Id;
        }

        public async Task<bool> UpdateBlogCommentAsync(int articleId, int commentId, BlogComment blogComment)
        {
            _ = blogComment ?? throw new ArgumentNullException(nameof(blogComment));

            var blogCommentEntity = await this.context.BlogComments
                .FirstAsync(c => (c.BlogArticleId == articleId) && (c.Id == commentId));

            if (blogCommentEntity is null)
            {
                return false;
            }

            blogCommentEntity.AuthorId = blogComment.AuthorId;
            blogCommentEntity.Text = blogComment.Text;
            blogCommentEntity.Posted = DateTime.Now;

            await this.context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBlogCommentAsync(int articleId, int commentId)
        {
            var blogCommentEntity = await this.context.BlogComments
                .FirstOrDefaultAsync(c => (c.BlogArticleId == articleId) && (c.Id == commentId));

            if (blogCommentEntity != null)
            {
                this.context.BlogComments.Remove(blogCommentEntity);
                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
