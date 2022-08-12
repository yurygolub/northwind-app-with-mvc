using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Products;
using NorthwindMvcClient.Mappers;
using NorthwindMvcClient.ViewModels;

namespace NorthwindMvcClient.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductManagementService managementService;

        public ProductController(IProductManagementService managementService)
        {
            this.managementService = managementService ?? throw new ArgumentNullException(nameof(managementService));
        }

        public async Task<IActionResult> Index([FromQuery] int offset = 0, [FromQuery] int limit = 10)
        {
            var products = new List<Models.Product>();
            await foreach (var item in this.managementService.GetProductsAsync(offset, limit))
            {
                products.Add(ProductMapper.MapProduct(item));
            }

            return this.View(new ProductsViewModel
            {
                Products = products,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            if (!await this.managementService.DestroyProductAsync(id))
            {
                return this.NotFound();
            }

            return this.NoContent();
        }
    }
}
