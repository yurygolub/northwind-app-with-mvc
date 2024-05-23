using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Northwind.Services.EntityFrameworkCore.Blogging.Context;

public class DesignTimeBloggingContextFactory : IDesignTimeDbContextFactory<BloggingContext>
{
    private readonly ILogger<BloggingContext> logger;

    public DesignTimeBloggingContextFactory()
    {
    }

    public DesignTimeBloggingContextFactory(ILogger<BloggingContext> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public BloggingContext CreateDbContext(string[] args)
    {
        const string connectionStringName = "NORTHWIND_BLOGGING";
        const string connectioStringPrefix = "SQLCONNSTR_";

        IConfigurationRoot configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();
        string connectionString = configuration.GetConnectionString(connectionStringName);

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new EnvironmentVariableException($"{connectioStringPrefix}{connectionStringName} environment variable is not set.");
        }

        this.logger?.LogInformation($"Using {connectioStringPrefix}{connectionStringName} environment variable as a connection string.");

        DbContextOptions<BloggingContext> builderOptions = new DbContextOptionsBuilder<BloggingContext>()
            .UseSqlServer(connectionString).Options;

        return new BloggingContext(builderOptions);
    }
}
