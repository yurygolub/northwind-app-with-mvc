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
/// Represents a management service for products.
/// </summary>
public class ProductManagementService : IProductManagementService
{
    private readonly NorthwindContext context;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductManagementService"/> class.
    /// </summary>
    /// <param name="context">NorthwindContext.</param>
    /// <param name="mapper">Mapper for entity mapping.</param>
    public ProductManagementService(NorthwindContext context, IMapper mapper)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<int> CreateProductAsync(Product product)
    {
        _ = product ?? throw new ArgumentNullException(nameof(product));

        await this.context.Products.AddAsync(this.mapper.Map<ProductEntity>(product));
        await this.context.SaveChangesAsync();
        return product.Id;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteProductAsync(int productId)
    {
        ProductEntity productEntity = await this.context.Products.FindAsync(productId);
        if (productEntity is not null)
        {
            this.context.Products.Remove(productEntity);
            IQueryable<OrderDetail> orderDetails = this.context.OrderDetails
                .Where(orderDet => orderDet.Product == productEntity);

            this.context.OrderDetails.RemoveRange(orderDetails);

            await this.context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<Product> GetProductsByNameAsync(IEnumerable<string> names)
    {
        _ = names ?? throw new ArgumentNullException(nameof(names));

        IQueryable<Product> products = from product in this.context.Products
                                       from name in names
                                       where product.ProductName == name
                                       select this.mapper.Map<Product>(product);

        await foreach (Product product in products.AsAsyncEnumerable())
        {
            yield return product;
        }
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<Product> GetProductsAsync(int offset, int limit)
    {
        IQueryable<Product> products = this.context.Products
            .Skip(offset)
            .Take(limit)
            .Select(p => this.mapper.Map<Product>(p));

        await foreach (Product product in products.AsAsyncEnumerable())
        {
            yield return product;
        }
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<Product> GetProductsForCategoryAsync(int categoryId)
    {
        IQueryable<Product> products = from product in this.context.Products
                                       where product.CategoryId == categoryId
                                       select this.mapper.Map<Product>(product);

        await foreach (Product product in products.AsAsyncEnumerable())
        {
            yield return product;
        }
    }

    /// <inheritdoc/>
    public async Task<Product> GetProductAsync(int productId)
    {
        ProductEntity productEntity = await this.context.Products.FindAsync(productId);
        if (productEntity is null)
        {
            return null;
        }

        return this.mapper.Map<Product>(productEntity);
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateProductAsync(int productId, Product product)
    {
        _ = product ?? throw new ArgumentNullException(nameof(product));

        ProductEntity productEntity = await this.context.Products.FindAsync(productId);
        if (productEntity is null)
        {
            return false;
        }

        productEntity.CategoryId = product.CategoryId;
        productEntity.Discontinued = product.Discontinued;
        productEntity.ProductName = product.Name;
        productEntity.QuantityPerUnit = product.QuantityPerUnit;
        productEntity.ReorderLevel = product.ReorderLevel;
        productEntity.SupplierId = product.SupplierId;
        productEntity.UnitPrice = product.UnitPrice;
        productEntity.UnitsInStock = product.UnitsInStock;
        productEntity.UnitsOnOrder = product.UnitsOnOrder;

        await this.context.SaveChangesAsync();
        return true;
    }
}
