﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable SA1516 // Elements should be separated by blank line

namespace Northwind.Services.EntityFrameworkCore.Entities;

public partial class Territory
{
    public Territory()
    {
        this.EmployeeTerritories = new HashSet<EmployeeTerritory>();
    }

    [Key]
    [Column("TerritoryID")]
    [StringLength(20)]
    public string TerritoryId { get; set; }
    [Required]
    [StringLength(50)]
    public string TerritoryDescription { get; set; }
    [Column("RegionID")]
    public int RegionId { get; set; }

    [ForeignKey(nameof(RegionId))]
    [InverseProperty("Territories")]
    public virtual Region Region { get; set; }
    [InverseProperty(nameof(EmployeeTerritory.Territory))]
    public virtual ICollection<EmployeeTerritory> EmployeeTerritories { get; set; }
}
