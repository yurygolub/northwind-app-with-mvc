using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Northwind.Services.EntityFrameworkCore.Context;
using Northwind.Services.EntityFrameworkCore.Entities;
using Northwind.Services.Products;

namespace Northwind.Services.EntityFrameworkCore.Products;

/// <summary>
/// Represents a management service for product categories.
/// </summary>
public class ProductCategoryManagementService : IProductCategoryManagementService
{
    private readonly NorthwindContext context;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductCategoryManagementService"/> class.
    /// </summary>
    /// <param name="context">NorthwindContext.</param>
    /// <param name="mapper">Mapper for entity mapping.</param>
    public ProductCategoryManagementService(NorthwindContext context, IMapper mapper)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<int> CreateCategoryAsync(ProductCategory productCategory)
    {
        _ = productCategory ?? throw new ArgumentNullException(nameof(productCategory));

        await this.context.Categories.AddAsync(this.mapper.Map<CategoryEntity>(productCategory));
        await this.context.SaveChangesAsync();
        return productCategory.Id;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteCategoryAsync(int categoryId)
    {
        CategoryEntity categoryEntity = await this.context.Categories.FindAsync(categoryId);
        if (categoryEntity is not null)
        {
            this.context.Categories.Remove(categoryEntity);

            var products = this.context.Products.Where(product => product.Category == categoryEntity);
            this.context.Products.RemoveRange(products);

            IQueryable<OrderDetail> orderDetails = products.SelectMany(
                p => this.context.OrderDetails.Where(orderDet => orderDet.Product == p));

            this.context.OrderDetails.RemoveRange(orderDetails);

            await this.context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<ProductCategory> GetCategoriesByNameAsync(IEnumerable<string> names)
    {
        _ = names ?? throw new ArgumentNullException(nameof(names));

        IQueryable<ProductCategory> categories = from category in this.context.Categories
                                                 from name in names
                                                 where category.CategoryName == name
                                                 select this.mapper.Map<ProductCategory>(category);

        await foreach (ProductCategory category in categories.AsAsyncEnumerable())
        {
            yield return category;
        }
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<ProductCategory> GetCategoriesAsync(int offset, int limit)
    {
        IQueryable<ProductCategory> categories = this.context.Categories
            .Skip(offset)
            .Take(limit)
            .Select(c => this.mapper.Map<ProductCategory>(c));

        await foreach (ProductCategory category in categories.AsAsyncEnumerable())
        {
            yield return category;
        }
    }

    /// <inheritdoc/>
    public async Task<ProductCategory> GetCategoryAsync(int categoryId)
    {
        CategoryEntity categoryEntity = await this.context.Categories.FindAsync(categoryId);
        if (categoryEntity is null)
        {
            return null;
        }

        return this.mapper.Map<ProductCategory>(categoryEntity);
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateCategoryAsync(int categoryId, ProductCategory productCategory)
    {
        _ = productCategory ?? throw new ArgumentNullException(nameof(productCategory));

        CategoryEntity categoryEntity = await this.context.Categories.FindAsync(categoryId);
        if (categoryEntity is null)
        {
            return false;
        }

        categoryEntity.CategoryName = productCategory.Name;
        categoryEntity.Description = productCategory.Description;
        categoryEntity.Picture = productCategory.Picture;

        await this.context.SaveChangesAsync();
        return true;
    }
}
