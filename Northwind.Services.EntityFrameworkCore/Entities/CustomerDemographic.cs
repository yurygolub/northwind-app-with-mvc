using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable SA1516 // Elements should be separated by blank line

namespace Northwind.Services.EntityFrameworkCore.Entities;

public partial class CustomerDemographic
{
    public CustomerDemographic()
    {
        this.CustomerCustomerDemos = new HashSet<CustomerCustomerDemo>();
    }

    [Key]
    [Column("CustomerTypeID")]
    [StringLength(10)]
    public string CustomerTypeId { get; set; }
    [Column(TypeName = "ntext")]
    public string CustomerDesc { get; set; }

    [InverseProperty(nameof(CustomerCustomerDemo.CustomerType))]
    public virtual ICollection<CustomerCustomerDemo> CustomerCustomerDemos { get; set; }
}
