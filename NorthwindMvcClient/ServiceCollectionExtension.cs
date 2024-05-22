using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.DataAccess;
using Northwind.DataAccess.SqlServer;
using Northwind.Services.Employees;
using Northwind.Services.EntityFrameworkCore.Context;
using Northwind.Services.Products;
using DataAccess = Northwind.Services.DataAccess;
using EntityFramework = Northwind.Services.EntityFrameworkCore;

namespace NorthwindMvcClient;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddSqlServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddScoped<IProductManagementService, DataAccess.Products.ProductManagementService>()
            .AddScoped<IProductCategoryManagementService, DataAccess.Products.ProductCategoryManagementService>()
            .AddScoped<IEmployeeManagementService, DataAccess.Employees.EmployeeManagementService>()
            .AddScoped(s => new SqlConnection(configuration.GetConnectionString("SqlConnection")))
            .AddScoped<NorthwindDataAccessFactory, SqlServerDataAccessFactory>()
            .AddAutoMapper(typeof(MappingProfiles.MappingProfile), typeof(DataAccess.MappingProfiles.MappingProfile));
    }

    public static IServiceCollection AddEfServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddScoped<IProductManagementService, EntityFramework.Products.ProductManagementService>()
            .AddScoped<IProductCategoryManagementService, EntityFramework.Products.ProductCategoryManagementService>()
            .AddScoped<IEmployeeManagementService, EntityFramework.Employees.EmployeeManagementService>()
            .AddScoped(s => new NorthwindContext(configuration.GetConnectionString("SqlConnection")))
            .AddAutoMapper(typeof(MappingProfiles.MappingProfile), typeof(EntityFramework.MappingProfiles.MappingProfile));
    }
}
