﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Products;

namespace NorthwindApiApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductManagementService managementService;

    public ProductsController(IProductManagementService managementService)
    {
        this.managementService = managementService ?? throw new ArgumentNullException(nameof(managementService));
    }

    [HttpGet]
    public IAsyncEnumerable<Product> GetProductsAsync([FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        return this.managementService.GetProductsAsync(offset, limit);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductAsync(int id)
    {
        Product product = await this.managementService.GetProductAsync(id);
        if (product is null)
        {
            return this.NotFound();
        }

        return this.Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductAsync([FromBody] Product product)
    {
        await this.managementService.CreateProductAsync(product);
        return this.Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductAsync(int id)
    {
        if (!await this.managementService.DeleteProductAsync(id))
        {
            return this.NotFound();
        }

        return this.NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProductAsync(int id, [FromBody] Product product)
    {
        if (!await this.managementService.UpdateProductAsync(id, product))
        {
            return this.NotFound();
        }

        return this.NoContent();
    }
}
