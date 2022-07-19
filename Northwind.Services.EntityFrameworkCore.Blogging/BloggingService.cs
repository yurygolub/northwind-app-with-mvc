using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Northwind.Services.Blogging;
using Northwind.Services.EntityFrameworkCore.Blogging.Context;
using Northwind.Services.EntityFrameworkCore.Blogging.Entities;
using Northwind.Services.Products;

namespace Northwind.Services.EntityFrameworkCore.Blogging
{
    public class BloggingService : IBloggingService
    {
        private readonly BloggingContext context;

        public BloggingService(IDesignTimeDbContextFactory<BloggingContext> factory)
        {
            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            this.context = factory.CreateDbContext(null);
        }

        public async IAsyncEnumerable<BlogArticle> GetBlogArticlesAsync(int offset, int limit)
        {
            var blogArticles = this.context.BlogArticles
                    .Skip(offset)
                    .Take(limit)
                    .Select(b => MapBlogArticle(b));

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

            return MapBlogArticle(blogArticleEntity);
        }

        public async Task<int> CreateBlogArticleAsync(BlogArticle blogArticle)
        {
            if (blogArticle is null)
            {
                throw new ArgumentNullException(nameof(blogArticle));
            }

            BlogArticleEntity blogArticleEntity = MapBlogArticle(blogArticle);

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
            if (blogArticle is null)
            {
                throw new ArgumentNullException(nameof(blogArticle));
            }

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
                .Select(b => MapBlogComment(b));

            await foreach (var blogComment in blogComments.AsAsyncEnumerable())
            {
                yield return blogComment;
            }
        }

        public async Task<int> CreateBlogCommentAsync(int articleId, BlogComment blogComment)
        {
            _ = blogComment ?? throw new ArgumentNullException(nameof(blogComment));

            var blogCommentEntity = MapBlogComment(blogComment);
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

        private static BlogArticle MapBlogArticle(BlogArticleEntity blogArticle)
        {
            return new BlogArticle()
            {
                Posted = blogArticle.Posted,
                AuthorId = blogArticle.AuthorId,
                Title = blogArticle.Title,
                Id = blogArticle.Id,
                Text = blogArticle.Text,
            };
        }

        private static BlogArticleEntity MapBlogArticle(BlogArticle blogArticle)
        {
            return new BlogArticleEntity()
            {
                Posted = blogArticle.Posted,
                AuthorId = blogArticle.AuthorId,
                Title = blogArticle.Title,
                Id = blogArticle.Id,
                Text = blogArticle.Text,
            };
        }

        private static BlogComment MapBlogComment(BlogCommentEntity blogComment)
        {
            return new BlogComment()
            {
                Posted = blogComment.Posted,
                AuthorId = blogComment.AuthorId,
                Id = blogComment.Id,
                Text = blogComment.Text,
                BlogArticleId = blogComment.BlogArticleId,
            };
        }

        private static BlogCommentEntity MapBlogComment(BlogComment blogComment)
        {
            return new BlogCommentEntity()
            {
                Posted = blogComment.Posted,
                AuthorId = blogComment.AuthorId,
                Id = blogComment.Id,
                Text = blogComment.Text,
                BlogArticleId = blogComment.BlogArticleId,
            };
        }
    }
}
