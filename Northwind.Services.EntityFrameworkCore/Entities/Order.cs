using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
#pragma warning disable SA1516 // Elements should be separated by blank line
#pragma warning disable CA2227 // Collection properties should be read only

namespace Northwind.Services.EntityFrameworkCore.Entities
{
    [Index(nameof(CustomerId), Name = "CustomerID")]
    [Index(nameof(CustomerId), Name = "CustomersOrders")]
    [Index(nameof(EmployeeId), Name = "EmployeeID")]
    [Index(nameof(EmployeeId), Name = "EmployeesOrders")]
    [Index(nameof(OrderDate), Name = "OrderDate")]
    [Index(nameof(ShipPostalCode), Name = "ShipPostalCode")]
    [Index(nameof(ShippedDate), Name = "ShippedDate")]
    [Index(nameof(ShipVia), Name = "ShippersOrders")]
    public partial class Order
    {
        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }

        [Key]
        [Column("OrderID")]
        public int OrderId { get; set; }
        [Column("CustomerID")]
        [StringLength(5)]
        public string CustomerId { get; set; }
        [Column("EmployeeID")]
        public int? EmployeeId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? OrderDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? RequiredDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ShippedDate { get; set; }
        public int? ShipVia { get; set; }
        [Column(TypeName = "money")]
        public decimal? Freight { get; set; }
        [StringLength(40)]
        public string ShipName { get; set; }
        [StringLength(60)]
        public string ShipAddress { get; set; }
        [StringLength(15)]
        public string ShipCity { get; set; }
        [StringLength(15)]
        public string ShipRegion { get; set; }
        [StringLength(10)]
        public string ShipPostalCode { get; set; }
        [StringLength(15)]
        public string ShipCountry { get; set; }

        [ForeignKey(nameof(CustomerId))]
        [InverseProperty("Orders")]
        public virtual Customer Customer { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        [InverseProperty("Orders")]
        public virtual EmployeeEntity Employee { get; set; }
        [ForeignKey(nameof(ShipVia))]
        [InverseProperty(nameof(Shipper.Orders))]
        public virtual Shipper ShipViaNavigation { get; set; }
        [InverseProperty(nameof(OrderDetail.Order))]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
