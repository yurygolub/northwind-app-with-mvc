using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
#pragma warning disable SA1516 // Elements should be separated by blank line

namespace Northwind.Services.EntityFrameworkCore.Entities
{
    [Table("Order Details")]
    [Index(nameof(OrderId), Name = "OrderID")]
    [Index(nameof(OrderId), Name = "OrdersOrder_Details")]
    [Index(nameof(ProductId), Name = "ProductID")]
    [Index(nameof(ProductId), Name = "ProductsOrder_Details")]
    public partial class OrderDetail
    {
        [Key]
        [Column("OrderID")]
        public int OrderId { get; set; }
        [Key]
        [Column("ProductID")]
        public int ProductId { get; set; }
        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }

        [ForeignKey(nameof(OrderId))]
        [InverseProperty("OrderDetails")]
        public virtual Order Order { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty("OrderDetails")]
        public virtual ProductEntity Product { get; set; }
    }
}
