using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Products;
using NorthwindMvcClient.ViewModels;

namespace NorthwindMvcClient.Controllers;

public class ProductCategoriesController : Controller
{
    private readonly IProductCategoryManagementService managementService;
    private readonly IMapper mapper;

    public ProductCategoriesController(IProductCategoryManagementService managementService, IMapper mapper)
    {
        this.managementService = managementService ?? throw new ArgumentNullException(nameof(managementService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IActionResult> Index([FromQuery] int offset = 0, [FromQuery] int limit = 5)
    {
        var categories = new List<Models.ProductCategory>();

        IAsyncEnumerable<ProductCategory> result = this.managementService.GetCategoriesAsync(offset, limit);
        int count = 0;
        await foreach (ProductCategory item in result)
        {
            Models.ProductCategory category = this.mapper.Map<Models.ProductCategory>(item);
            if (category.Picture?.Length != 0)
            {
                category.Picture = category.Picture[78..];
            }

            categories.Add(category);
            count++;
        }

        return this.View(new ProductCategoriesViewModel
        {
            ProductCategories = categories,
            PageViewModel = new PageViewModel
            {
                Offset = offset,
                Limit = limit,
                Count = count,
            },
        });
    }
}
