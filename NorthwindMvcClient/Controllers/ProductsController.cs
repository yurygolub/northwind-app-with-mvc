using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Products;
using NorthwindMvcClient.ViewModels;

namespace NorthwindMvcClient.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductManagementService managementService;
        private readonly IMapper mapper;

        public ProductsController(IProductManagementService managementService, IMapper mapper)
        {
            this.managementService = managementService ?? throw new ArgumentNullException(nameof(managementService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IActionResult> Index([FromQuery] int offset = 0, [FromQuery] int limit = 10)
        {
            var products = new List<Models.Product>();
            await foreach (var item in this.managementService.GetProductsAsync(offset, limit))
            {
                products.Add(this.mapper.Map<Models.Product>(item));
            }

            return this.View(new ProductsViewModel
            {
                Products = products,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            if (!await this.managementService.DeleteProductAsync(id))
            {
                return this.NotFound();
            }

            return this.NoContent();
        }
    }
}
