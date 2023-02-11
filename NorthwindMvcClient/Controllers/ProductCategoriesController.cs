using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Products;
using NorthwindMvcClient.ViewModels;

namespace NorthwindMvcClient.Controllers
{
    public class ProductCategoriesController : Controller
    {
        private readonly IProductCategoryManagementService managementService;
        private readonly IMapper mapper;

        public ProductCategoriesController(IProductCategoryManagementService managementService, IMapper mapper)
        {
            this.managementService = managementService ?? throw new ArgumentNullException(nameof(managementService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IActionResult> Index([FromQuery] int offset = 0, [FromQuery] int limit = 10)
        {
            var categories = new List<Models.ProductCategory>();
            await foreach (var item in this.managementService.GetCategoriesAsync(offset, limit))
            {
                var category = this.mapper.Map<Models.ProductCategory>(item);
                if (category.Picture?.Length != 0)
                {
                    category.Picture = category.Picture[78..];
                }

                categories.Add(category);
            }

            return this.View(new ProductCategoriesViewModel
            {
                ProductCategories = categories,
            });
        }
    }
}
