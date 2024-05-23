using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Northwind.DataAccess.Products;

namespace Northwind.DataAccess.SqlServer.Products;

/// <summary>
/// Represents a SQL Server-tailored DAO for Northwind products.
/// </summary>
public sealed class ProductSqlServerDataAccessObject : IProductDataAccessObject
{
    private readonly SqlConnection connection;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductSqlServerDataAccessObject"/> class.
    /// </summary>
    /// <param name="connection">A <see cref="SqlConnection"/>.</param>
    public ProductSqlServerDataAccessObject(SqlConnection connection)
    {
        this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    /// <inheritdoc/>
    public async Task<int> InsertProductAsync(ProductTransferObject product)
    {
        _ = product ?? throw new ArgumentNullException(nameof(product));

        await using var command = new SqlCommand("InsertProduct", this.connection)
        {
            CommandType = CommandType.StoredProcedure,
        };

        AddSqlParameters(product, command);

        if (this.connection.State != ConnectionState.Open)
        {
            await this.connection.OpenAsync();
        }

        return await command.ExecuteNonQueryAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteProductAsync(int productId)
    {
        if (productId <= 0)
        {
            throw new ArgumentException("Must be greater than zero.", nameof(productId));
        }

        await using var command = new SqlCommand("DeleteProduct", this.connection)
        {
            CommandType = CommandType.StoredProcedure,
        };

        command.SetParameter("@productID", productId, SqlDbType.Int, isNullable: false);

        if (this.connection.State != ConnectionState.Open)
        {
            await this.connection.OpenAsync();
        }

        int result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    /// <inheritdoc/>
    public async Task<ProductTransferObject> FindProductAsync(int productId)
    {
        if (productId <= 0)
        {
            throw new ArgumentException("Must be greater than zero.", nameof(productId));
        }

        await using var command = new SqlCommand("FindProduct", this.connection)
        {
            CommandType = CommandType.StoredProcedure,
        };

        command.SetParameter("@productID", productId, SqlDbType.Int, isNullable: false);

        if (this.connection.State != ConnectionState.Open)
        {
            await this.connection.OpenAsync();
        }

        await using SqlDataReader reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            throw new ProductNotFoundException(productId);
        }

        return CreateProduct(reader);
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<ProductTransferObject> SelectProductsAsync(int offset, int limit)
    {
        if (offset < 0)
        {
            throw new ArgumentException("Must be greater than zero or equals zero.", nameof(offset));
        }

        if (limit < 1)
        {
            throw new ArgumentException("Must be greater than zero.", nameof(limit));
        }

        await foreach (var product in SelectProductsAsync(offset, limit))
        {
            yield return product;
        }

        async IAsyncEnumerable<ProductTransferObject> SelectProductsAsync(int offset, int limit)
        {
            await using var command = new SqlCommand("SelectProducts", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            command.SetParameter("@offset", offset, SqlDbType.Int, isNullable: false);
            command.SetParameter("@limit", limit, SqlDbType.Int, isNullable: false);

            if (this.connection.State != ConnectionState.Open)
            {
                await this.connection.OpenAsync();
            }

            await using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                yield return CreateProduct(reader);
            }
        }
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<ProductTransferObject> SelectProductsByNameAsync(IEnumerable<string> productNames)
    {
        _ = productNames ?? throw new ArgumentNullException(nameof(productNames));

        if (productNames.Any())
        {
            throw new ArgumentException("Collection is empty.", nameof(productNames));
        }

        foreach (string name in productNames)
        {
            await foreach (ProductTransferObject product in SelectProductsByNameAsync(name))
            {
                yield return product;
            }
        }

        async IAsyncEnumerable<ProductTransferObject> SelectProductsByNameAsync(string productName)
        {
            await using var command = new SqlCommand("SelectProductsByName", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            command.SetParameter("@productName", productName, SqlDbType.NVarChar, 40, false);

            if (this.connection.State != ConnectionState.Open)
            {
                await this.connection.OpenAsync();
            }

            await using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                yield return CreateProduct(reader);
            }
        }
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateProductAsync(int productId, ProductTransferObject product)
    {
        _ = product ?? throw new ArgumentNullException(nameof(product));

        await using var command = new SqlCommand("UpdateProduct", this.connection)
        {
            CommandType = CommandType.StoredProcedure,
        };

        command.SetParameter("@productId", productId, SqlDbType.Int, isNullable: false);
        AddSqlParameters(product, command);

        if (this.connection.State != ConnectionState.Open)
        {
            await this.connection.OpenAsync();
        }

        int result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<ProductTransferObject> SelectProductByCategoryAsync(IEnumerable<int> collectionOfCategoryId)
    {
        _ = collectionOfCategoryId ?? throw new ArgumentNullException(nameof(collectionOfCategoryId));

        foreach (int categoryId in collectionOfCategoryId)
        {
            await foreach (ProductTransferObject product in SelectProductByCategoryAsync(categoryId))
            {
                yield return product;
            }
        }

        async IAsyncEnumerable<ProductTransferObject> SelectProductByCategoryAsync(int categoryId)
        {
            await using var command = new SqlCommand("SelectProductsByCategory", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            command.SetParameter("@categoryId", categoryId, SqlDbType.Int, isNullable: false);

            if (this.connection.State != ConnectionState.Open)
            {
                await this.connection.OpenAsync();
            }

            await using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                yield return CreateProduct(reader);
            }
        }
    }

    private static ProductTransferObject CreateProduct(SqlDataReader reader)
    {
        return new ProductTransferObject
        {
            Id = (int)reader["ProductID"],
            Name = (string)reader["ProductName"],
            SupplierId = GetValueStruct<int>("SupplierID"),
            CategoryId = GetValueStruct<int>("CategoryID"),
            QuantityPerUnit = GetValueClass<string>("QuantityPerUnit"),
            UnitPrice = GetValueStruct<decimal>("UnitPrice"),
            UnitsInStock = GetValueStruct<short>("UnitsInStock"),
            UnitsOnOrder = GetValueStruct<short>("UnitsOnOrder"),
            ReorderLevel = GetValueStruct<short>("ReorderLevel"),
            Discontinued = (bool)reader["Discontinued"],
        };

        T GetValueClass<T>(string text)
            where T : class
            => reader[text] == DBNull.Value ? null : (T)reader[text];

        T? GetValueStruct<T>(string text)
            where T : struct
            => reader[text] == DBNull.Value ? null : (T)reader[text];
    }

    private static void AddSqlParameters(ProductTransferObject product, SqlCommand command)
    {
        command.SetParameter("@productName", product.Name, SqlDbType.NVarChar, 40, false);
        command.SetParameter("@supplierId", product.SupplierId, SqlDbType.Int);
        command.SetParameter("@categoryId", product.CategoryId, SqlDbType.Int);
        command.SetParameter("@quantityPerUnit", product.QuantityPerUnit, SqlDbType.NVarChar, 20);
        command.SetParameter("@unitPrice", product.UnitPrice, SqlDbType.Money);
        command.SetParameter("@unitsInStock", product.UnitsInStock, SqlDbType.SmallInt);
        command.SetParameter("@unitsOnOrder", product.UnitsOnOrder, SqlDbType.SmallInt);
        command.SetParameter("@reorderLevel", product.ReorderLevel, SqlDbType.SmallInt);
        command.SetParameter("@discontinued", product.Discontinued, SqlDbType.Bit, isNullable: false);
    }
}
