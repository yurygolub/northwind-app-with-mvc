﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable SA1516 // Elements should be separated by blank line

namespace Northwind.Services.EntityFrameworkCore.Entities;

public partial class EmployeeTerritory
{
    [Key]
    [Column("EmployeeID")]
    public int EmployeeId { get; set; }
    [Key]
    [Column("TerritoryID")]
    [StringLength(20)]
    public string TerritoryId { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    [InverseProperty("EmployeeTerritories")]
    public virtual EmployeeEntity Employee { get; set; }
    [ForeignKey(nameof(TerritoryId))]
    [InverseProperty("EmployeeTerritories")]
    public virtual Territory Territory { get; set; }
}
