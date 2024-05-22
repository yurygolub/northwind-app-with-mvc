using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#pragma warning disable SA1516 // Elements should be separated by blank line

namespace Northwind.Services.EntityFrameworkCore.Entities;

[Keyless]
public partial class QuarterlyOrder
{
    [Column("CustomerID")]
    [StringLength(5)]
    public string CustomerId { get; set; }
    [StringLength(40)]
    public string CompanyName { get; set; }
    [StringLength(15)]
    public string City { get; set; }
    [StringLength(15)]
    public string Country { get; set; }
}
