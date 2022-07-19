using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
#pragma warning disable SA1516 // Elements should be separated by blank line
#pragma warning disable CA1819 // Properties should not return arrays
#pragma warning disable CA2227 // Collection properties should be read only

namespace Northwind.Services.EntityFrameworkCore.Entities
{
    [Index(nameof(LastName), Name = "LastName")]
    [Index(nameof(PostalCode), Name = "PostalCode")]
    public partial class EmployeeEntity
    {
        public EmployeeEntity()
        {
            this.EmployeeTerritories = new HashSet<EmployeeTerritory>();
            this.InverseReportsToNavigation = new HashSet<EmployeeEntity>();
            this.Orders = new HashSet<Order>();
        }

        [Key]
        [Column("EmployeeID")]
        public int EmployeeId { get; set; }
        [Required]
        [StringLength(20)]
        public string LastName { get; set; }
        [Required]
        [StringLength(10)]
        public string FirstName { get; set; }
        [StringLength(30)]
        public string Title { get; set; }
        [StringLength(25)]
        public string TitleOfCourtesy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? BirthDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? HireDate { get; set; }
        [StringLength(60)]
        public string Address { get; set; }
        [StringLength(15)]
        public string City { get; set; }
        [StringLength(15)]
        public string Region { get; set; }
        [StringLength(10)]
        public string PostalCode { get; set; }
        [StringLength(15)]
        public string Country { get; set; }
        [StringLength(24)]
        public string HomePhone { get; set; }
        [StringLength(4)]
        public string Extension { get; set; }
        [Column(TypeName = "image")]
        public byte[] Photo { get; set; }
        [Column(TypeName = "ntext")]
        public string Notes { get; set; }
        public int? ReportsTo { get; set; }
        [StringLength(255)]
        public string PhotoPath { get; set; }

        [ForeignKey(nameof(ReportsTo))]
        [InverseProperty(nameof(EmployeeEntity.InverseReportsToNavigation))]
        public virtual EmployeeEntity ReportsToNavigation { get; set; }
        [InverseProperty(nameof(EmployeeTerritory.Employee))]
        public virtual ICollection<EmployeeTerritory> EmployeeTerritories { get; set; }
        [InverseProperty(nameof(EmployeeEntity.ReportsToNavigation))]
        public virtual ICollection<EmployeeEntity> InverseReportsToNavigation { get; set; }
        [InverseProperty(nameof(Order.Employee))]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
