using System;
using Northwind.Services.Products;

namespace NorthwindMvcClient.Mappers
{
    public static class ProductMapper
    {
        public static Models.Product MapProduct(Product product)
        {
            _ = product ?? throw new ArgumentNullException(nameof(product));

            return new Models.Product
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                Discontinued = product.Discontinued,
                Name = product.Name,
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
