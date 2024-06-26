﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable SA1516 // Elements should be separated by blank line

namespace Northwind.Services.EntityFrameworkCore.Entities;

public partial class Shipper
{
    public Shipper()
    {
        this.Orders = new HashSet<Order>();
    }

    [Key]
    [Column("ShipperID")]
    public int ShipperId { get; set; }
    [Required]
    [StringLength(40)]
    public string CompanyName { get; set; }
    [StringLength(24)]
    public string Phone { get; set; }

    [InverseProperty(nameof(Order.ShipViaNavigation))]
    public virtual ICollection<Order> Orders { get; set; }
}
