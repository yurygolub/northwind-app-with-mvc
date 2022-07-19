using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Northwind.Services.EntityFrameworkCore.Entities;
using Northwind.Services.Products;

namespace Northwind.Services.EntityFrameworkCore.Products
{
    /// <summary>
    /// Represents a management service for product categories.
    /// </summary>
    public class ProductCategoryManagementService : IProductCategoryManagementService
    {
        private readonly Context.NorthwindContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoryManagementService"/> class.
        /// </summary>
        /// <param name="context">NorthwindContext.</param>
        public ProductCategoryManagementService(Context.NorthwindContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            this.context = context;
        }

        /// <inheritdoc/>
        public async Task<int> CreateCategoryAsync(ProductCategory productCategory)
        {
            if (productCategory is null)
            {
                throw new ArgumentNullException(nameof(productCategory));
            }

            await this.context.Categories.AddAsync(MapProductCategory(productCategory));
            await this.context.SaveChangesAsync();
            return productCategory.Id;
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyCategoryAsync(int categoryId)
        {
            var category = await this.context.Categories.FindAsync(categoryId);
            if (category != null)
            {
                this.context.Categories.Remove(category);
                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<ProductCategory> GetCategoriesByNameAsync(IEnumerable<string> names)
        {
            if (names is null)
            {
                throw new ArgumentNullException(nameof(names));
            }

            var categories = from category in this.context.Categories
                           from name in names
                           where category.CategoryName == name
                           select MapProductCategory(category);

            await foreach (var category in categories.AsAsyncEnumerable())
            {
                yield return category;
            }
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<ProductCategory> GetCategoriesAsync(int offset, int limit)
        {
            var categories = this.context.Categories
                    .Skip(offset)
                    .Take(limit)
                    .Select(c => MapProductCategory(c));

            await foreach (var category in categories.AsAsyncEnumerable())
            {
                yield return category;
            }
        }

        /// <inheritdoc/>
        public async Task<ProductCategory> GetCategoryAsync(int categoryId)
        {
            var contextCategory = await this.context.Categories.FindAsync(categoryId);
            if (contextCategory is null)
            {
                return null;
            }

            return MapProductCategory(contextCategory);
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateCategoryAsync(int categoryId, ProductCategory productCategory)
        {
            if (productCategory is null)
            {
                throw new ArgumentNullException(nameof(productCategory));
            }

            var contextCategory = await this.context.Categories.FindAsync(categoryId);
            if (contextCategory is null)
            {
                return false;
            }

            contextCategory.CategoryName = productCategory.Name;
            contextCategory.Description = productCategory.Description;
            contextCategory.Picture = productCategory.Picture;

            await this.context.SaveChangesAsync();
            return true;
        }

        private static ProductCategory MapProductCategory(CategoryEntity category)
        {
            return new ProductCategory()
            {
                Id = category.CategoryId,
                Name = category.CategoryName,
                Description = category.Description,
                Picture = category.Picture,
            };
        }

        private static CategoryEntity MapProductCategory(ProductCategory productCategory)
        {
            return new CategoryEntity()
            {
                CategoryId = productCategory.Id,
                CategoryName = productCategory.Name,
                Description = productCategory.Description,
                Picture = productCategory.Picture,
            };
        }
    }
}
