﻿using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.DataAccess;
using Northwind.DataAccess.SqlServer;
using Northwind.Services.Blogging;
using Northwind.Services.Employees;
using Northwind.Services.EntityFrameworkCore.Blogging;
using Northwind.Services.EntityFrameworkCore.Blogging.Context;
using Northwind.Services.EntityFrameworkCore.Blogging.MappingProfiles;
using Northwind.Services.EntityFrameworkCore.Context;
using Northwind.Services.Products;
using DataAccess = Northwind.Services.DataAccess;
using EntityFramework = Northwind.Services.EntityFrameworkCore;

namespace NorthwindApiApp;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddSqlServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddTransient<IProductManagementService, DataAccess.Products.ProductManagementService>()
            .AddTransient<IProductCategoryManagementService, DataAccess.Products.ProductCategoryManagementService>()
            .AddTransient<IProductCategoryPicturesService, DataAccess.Products.ProductCategoryPicturesService>()
            .AddTransient<IEmployeeManagementService, DataAccess.Employees.EmployeeManagementService>()
            .AddTransient<IEmployeePicturesService, DataAccess.Employees.EmployeePicturesService>()
            .AddScoped(s => new SqlConnection(configuration.GetConnectionString("SqlConnection")))
            .AddTransient<NorthwindDataAccessFactory, SqlServerDataAccessFactory>()
            .AddAutoMapper(typeof(DataAccess.MappingProfiles.MappingProfile));
    }

    public static IServiceCollection AddEfServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddTransient<IProductManagementService, EntityFramework.Products.ProductManagementService>()
            .AddTransient<IProductCategoryManagementService, EntityFramework.Products.ProductCategoryManagementService>()
            .AddTransient<IProductCategoryPicturesService, EntityFramework.Products.ProductCategoryPicturesService>()
            .AddTransient<IEmployeeManagementService, EntityFramework.Employees.EmployeeManagementService>()
            .AddTransient<IEmployeePicturesService, EntityFramework.Employees.EmployeePicturesService>()
            .AddTransient<IBloggingService, BloggingService>()
            .AddTransient<IDesignTimeDbContextFactory<BloggingContext>, DesignTimeBloggingContextFactory>()
            .AddScoped(s => new NorthwindContext(configuration.GetConnectionString("SqlConnection")))
            .AddAutoMapper(typeof(MappingProfile), typeof(EntityFramework.MappingProfiles.MappingProfile));
    }
}
