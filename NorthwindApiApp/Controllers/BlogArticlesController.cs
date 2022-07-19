using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Blogging;
using Northwind.Services.Employees;
using Northwind.Services.Products;
using NorthwindApiApp.Models;

#pragma warning disable SA1600

namespace NorthwindApiApp.Controllers
{
    [ApiController]
    [Route("api/articles")]
    public class BlogArticlesController : ControllerBase
    {
        private readonly IBloggingService bloggingService;
        private readonly IEmployeeManagementService employeeService;
        private readonly IProductManagementService productService;

        public BlogArticlesController(IBloggingService bloggingService, IEmployeeManagementService employeeService, IProductManagementService productService)
        {
            this.bloggingService = bloggingService ?? throw new ArgumentNullException(nameof(bloggingService));
            this.employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            this.productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        [HttpGet]
        public async IAsyncEnumerable<BlogArticleShortInfo> GetBlogArticlesAsync([FromQuery] int offset = 0, [FromQuery] int limit = 10)
        {
            await foreach (var blogArticle in this.bloggingService.GetBlogArticlesAsync(offset, limit))
            {
                var author = await this.employeeService.GetEmployeeAsync(blogArticle.AuthorId);
                if (author != null)
                {
                    yield return new BlogArticleShortInfo(blogArticle, author);
                }
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlogArticleAsync(int id)
        {
            var blogArticle = await this.bloggingService.GetBlogArticleAsync(id);
            if (blogArticle is null)
            {
                return this.NotFound();
            }

            var author = await this.employeeService.GetEmployeeAsync(blogArticle.AuthorId);

            if (author is null)
            {
                return this.NotFound();
            }

            return this.Ok(new BlogArticleFullInfo(blogArticle, author));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogArticleAsync([FromBody] BlogArticle blogArticle)
        {
            if (blogArticle is null)
            {
                throw new ArgumentNullException(nameof(blogArticle));
            }

            var author = await this.employeeService.GetEmployeeAsync(blogArticle.AuthorId);
            if (author is null)
            {
                return this.NotFound();
            }

            await this.bloggingService.CreateBlogArticleAsync(blogArticle);
            return this.Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogArticleAsync(int id)
        {
            if (!await this.bloggingService.DeleteBlogArticleAsync(id))
            {
                return this.NotFound();
            }

            return this.NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlogArticleAsync(int id, [FromBody] BlogArticle blogArticle)
        {
            if (!await this.bloggingService.UpdateBlogArticleAsync(id, blogArticle))
            {
                return this.NotFound();
            }

            return this.NoContent();
        }

        [HttpGet("{articleId}/products")]
        public async IAsyncEnumerable<Product> GetAllRelatedProductsAsync(int articleId, [FromQuery] int offset = 0, [FromQuery] int limit = 10)
        {
            var productIds = this.bloggingService.GetAllRelatedProductIdsAsync(articleId, offset, limit);
            await foreach (var productId in productIds)
            {
                yield return await this.productService.GetProductAsync(productId);
            }
        }

        [HttpPost("{articleId}/products/{productId}")]
        public async Task<IActionResult> CreateProductLinkAsync(int articleId, int productId)
        {
            await this.bloggingService.CreateProductLinkAsync(articleId, productId);
            return this.Ok();
        }

        [HttpDelete("{articleId}/products/{productId}")]
        public async Task<IActionResult> RemoveProductLinkAsync(int articleId, int productId)
        {
            if (!await this.bloggingService.RemoveProductLinkAsync(articleId, productId))
            {
                return this.NotFound();
            }

            return this.NoContent();
        }

        [HttpGet("{articleId}/comments")]
        public async IAsyncEnumerable<BlogComment> GetAllBlogCommentsAsync(int articleId, [FromQuery] int offset = 0, [FromQuery] int limit = 10)
        {
            var blogComments = this.bloggingService.GetAllBlogCommentsAsync(articleId, offset, limit);
            await foreach (var blogComment in blogComments)
            {
                yield return blogComment;
            }
        }

        [HttpPost("{articleId}/comments")]
        public async Task<IActionResult> CreateBlogCommentAsync(int articleId, [FromBody] BlogComment blogComment)
        {
            _ = blogComment ?? throw new ArgumentNullException(nameof(blogComment));

            await this.bloggingService.CreateBlogCommentAsync(articleId, blogComment);
            return this.Ok();
        }

        [HttpPut("{articleId}/comments/{id}")]
        public async Task<IActionResult> UpdateBlogCommentAsync(int articleId, int id, [FromBody] BlogComment blogComment)
        {
            _ = blogComment ?? throw new ArgumentNullException(nameof(blogComment));

            if (!await this.bloggingService.UpdateBlogCommentAsync(articleId, id, blogComment))
            {
                return this.NotFound();
            }

            return this.NoContent();
        }

        [HttpDelete("{articleId}/comments/{id}")]
        public async Task<IActionResult> DeleteBlogCommentAsync(int articleId, int id)
        {
            if (!await this.bloggingService.DeleteBlogCommentAsync(articleId, id))
            {
                return this.NotFound();
            }

            return this.NoContent();
        }
    }
}
