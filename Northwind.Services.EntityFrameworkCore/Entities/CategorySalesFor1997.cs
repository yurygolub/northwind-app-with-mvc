using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#pragma warning disable SA1516 // Elements should be separated by blank line

namespace Northwind.Services.EntityFrameworkCore.Entities;

[Keyless]
public partial class CategorySalesFor1997
{
    [Required]
    [StringLength(15)]
    public string CategoryName { get; set; }
    [Column(TypeName = "money")]
    public decimal? CategorySales { get; set; }
}
