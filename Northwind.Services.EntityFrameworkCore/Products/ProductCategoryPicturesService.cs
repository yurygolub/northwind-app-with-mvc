﻿using System;
using System.IO;
using System.Threading.Tasks;
using Northwind.Services.EntityFrameworkCore.Context;
using Northwind.Services.Products;

namespace Northwind.Services.EntityFrameworkCore.Products;

/// <summary>
/// Represents a management service for product categories pictures.
/// </summary>
public class ProductCategoryPicturesService : IProductCategoryPicturesService
{
    private readonly NorthwindContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductCategoryPicturesService"/> class.
    /// </summary>
    /// <param name="context">NorthwindContext.</param>
    public ProductCategoryPicturesService(NorthwindContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public async Task<Stream> GetProductCategoryPictureAsync(int categoryId)
    {
        var contextCategory = await this.context.Categories.FindAsync(categoryId);
        if (contextCategory?.Picture is null)
        {
            return null;
        }

        return new MemoryStream(contextCategory.Picture[78..]);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteProductCategoryPictureAsync(int categoryId)
    {
        var contextCategory = await this.context.Categories.FindAsync(categoryId);
        if (contextCategory is null)
        {
            return false;
        }

        contextCategory.Picture = null;

        await this.context.SaveChangesAsync();
        return true;
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateProductCategoryPictureAsync(int categoryId, Stream stream)
    {
        _ = stream ?? throw new ArgumentNullException(nameof(stream));

        var contextCategory = await this.context.Categories.FindAsync(categoryId);
        if (contextCategory is null)
        {
            return false;
        }

        await using MemoryStream memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        memoryStream.ToArray().CopyTo(contextCategory.Picture, 78);

        await this.context.SaveChangesAsync();
        return true;
    }
}
