﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#pragma warning disable SA1516 // Elements should be separated by blank line

namespace Northwind.Services.EntityFrameworkCore.Entities;

[Keyless]
public partial class SalesTotalsByAmount
{
    [Column(TypeName = "money")]
    public decimal? SaleAmount { get; set; }
    [Column("OrderID")]
    public int OrderId { get; set; }
    [Required]
    [StringLength(40)]
    public string CompanyName { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? ShippedDate { get; set; }
}
