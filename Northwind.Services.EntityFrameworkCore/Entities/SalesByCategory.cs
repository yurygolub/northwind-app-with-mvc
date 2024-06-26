﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#pragma warning disable SA1516 // Elements should be separated by blank line

namespace Northwind.Services.EntityFrameworkCore.Entities;

[Keyless]
public partial class SalesByCategory
{
    [Column("CategoryID")]
    public int CategoryId { get; set; }
    [Required]
    [StringLength(15)]
    public string CategoryName { get; set; }
    [Required]
    [StringLength(40)]
    public string ProductName { get; set; }
    [Column(TypeName = "money")]
    public decimal? ProductSales { get; set; }
}
