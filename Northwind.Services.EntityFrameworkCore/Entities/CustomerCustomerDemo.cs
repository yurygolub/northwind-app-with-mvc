using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable SA1516 // Elements should be separated by blank line

namespace Northwind.Services.EntityFrameworkCore.Entities;

[Table("CustomerCustomerDemo")]
public partial class CustomerCustomerDemo
{
    [Key]
    [Column("CustomerID")]
    [StringLength(5)]
    public string CustomerId { get; set; }
    [Key]
    [Column("CustomerTypeID")]
    [StringLength(10)]
    public string CustomerTypeId { get; set; }

    [ForeignKey(nameof(CustomerId))]
    [InverseProperty("CustomerCustomerDemos")]
    public virtual Customer Customer { get; set; }
    [ForeignKey(nameof(CustomerTypeId))]
    [InverseProperty(nameof(CustomerDemographic.CustomerCustomerDemos))]
    public virtual CustomerDemographic CustomerType { get; set; }
}
