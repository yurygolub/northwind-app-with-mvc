﻿using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

#pragma warning disable SA1516 // Elements should be separated by blank line

namespace Northwind.Services.EntityFrameworkCore.Entities;

[Keyless]
public partial class ProductsByCategory
{
    [Required]
    [StringLength(15)]
    public string CategoryName { get; set; }
    [Required]
    [StringLength(40)]
    public string ProductName { get; set; }
    [StringLength(20)]
    public string QuantityPerUnit { get; set; }
    public short? UnitsInStock { get; set; }
    public bool Discontinued { get; set; }
}
