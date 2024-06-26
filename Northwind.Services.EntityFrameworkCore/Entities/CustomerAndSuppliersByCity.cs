﻿using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

#pragma warning disable SA1516 // Elements should be separated by blank line

namespace Northwind.Services.EntityFrameworkCore.Entities;

[Keyless]
public partial class CustomerAndSuppliersByCity
{
    [StringLength(15)]
    public string City { get; set; }
    [Required]
    [StringLength(40)]
    public string CompanyName { get; set; }
    [StringLength(30)]
    public string ContactName { get; set; }
    [Required]
    [StringLength(9)]
    public string Relationship { get; set; }
}
