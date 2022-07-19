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
    /// Represents a management service for products.
    /// </summary>
    public class ProductManagementService : IProductManagementService
    {
        private readonly Context.NorthwindContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductManagementService"/> class.
        /// </summary>
        /// <param name="context">NorthwindContext.</param>
        public ProductManagementService(Context.NorthwindContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            this.context = context;
        }

        /// <inheritdoc/>
        public async Task<int> CreateProductAsync(Product product)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            await this.context.Products.AddAsync(MapProduct(product));
            await this.context.SaveChangesAsync();
            return product.Id;
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyProductAsync(int productId)
        {
            var product = await this.context.Products.FindAsync(productId);
            if (product != null)
            {
                this.context.Products.Remove(product);
                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<Product> GetProductsByNameAsync(IEnumerable<string> names)
        {
            if (names is null)
            {
                throw new ArgumentNullException(nameof(names));
            }

            var products = from product in this.context.Products
                           from name in names
                           where product.ProductName == name
                           select MapProduct(product);

            await foreach (var product in products.AsAsyncEnumerable())
            {
                yield return product;
            }
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<Product> GetProductsAsync(int offset, int limit)
        {
            var products = this.context.Products
                    .Skip(offset)
                    .Take(limit)
                    .Select(p => MapProduct(p));

            await foreach (var product in products.AsAsyncEnumerable())
            {
                yield return product;
            }
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<Product> GetProductsForCategoryAsync(int categoryId)
        {
            var products = from product in this.context.Products
                           where product.CategoryId == categoryId
                           select MapProduct(product);

            await foreach (var product in products.AsAsyncEnumerable())
            {
                yield return product;
            }
        }

        /// <inheritdoc/>
        public async Task<Product> GetProductAsync(int productId)
        {
            var contextProduct = await this.context.Products.FindAsync(productId);
            if (contextProduct is null)
            {
                return null;
            }

            return MapProduct(contextProduct);
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateProductAsync(int productId, Product product)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            var contextProduct = await this.context.Products.FindAsync(productId);
            if (contextProduct is null)
            {
                return false;
            }

            contextProduct.CategoryId = product.CategoryId;
            contextProduct.Discontinued = product.Discontinued;
            contextProduct.ProductName = product.Name;
            contextProduct.QuantityPerUnit = product.QuantityPerUnit;
            contextProduct.ReorderLevel = product.ReorderLevel;
            contextProduct.SupplierId = product.SupplierId;
            contextProduct.UnitPrice = product.UnitPrice;
            contextProduct.UnitsInStock = product.UnitsInStock;
            contextProduct.UnitsOnOrder = product.UnitsOnOrder;

            await this.context.SaveChangesAsync();
            return true;
        }

        private static Product MapProduct(ProductEntity product)
        {
            return new Product()
            {
                Id = product.ProductId,
                CategoryId = product.CategoryId,
                Discontinued = product.Discontinued,
                Name = product.ProductName,
                QuantityPerUnit = product.QuantityPerUnit,
                ReorderLevel = product.ReorderLevel,
                SupplierId = product.SupplierId,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
                UnitsOnOrder = product.UnitsOnOrder,
            };
        }

        private static ProductEntity MapProduct(Product product)
        {
            return new ProductEntity()
            {
                ProductId = product.Id,
                CategoryId = product.CategoryId,
                Discontinued = product.Discontinued,
                ProductName = product.Name,
                QuantityPerUnit = product.QuantityPerUnit,
                ReorderLevel = product.ReorderLevel,
                SupplierId = product.SupplierId,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
                UnitsOnOrder = product.UnitsOnOrder,
            };
        }
    }
}
