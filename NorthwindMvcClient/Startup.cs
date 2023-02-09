using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;
using Northwind.Services.Employees;
using Northwind.Services.EntityFrameworkCore.Context;
using Northwind.Services.EntityFrameworkCore.Employees;
using Northwind.Services.EntityFrameworkCore.MappingProfiles;
using Northwind.Services.EntityFrameworkCore.Products;
using Northwind.Services.Products;

namespace NorthwindMvcClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services
                .AddTransient<IProductManagementService, ProductManagementService>()
                .AddScoped<IProductCategoryManagementService, ProductCategoryManagementService>()
                .AddTransient<IEmployeeManagementService, EmployeeManagementService>()
                .AddScoped(s => new NorthwindContext(this.Configuration.GetConnectionString("SqlConnection")))
                .AddAutoMapper(typeof(MappingProfile), typeof(MappingProfiles.MappingProfile))
                .AddLogging(builder => builder.AddNLog());
        }
    }
}
