using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#pragma warning disable SA1516 // Elements should be separated by blank line

namespace Northwind.Services.EntityFrameworkCore.Entities;

[Index(nameof(CategoryName), Name = "CategoryName")]
public partial class CategoryEntity
{
    public CategoryEntity()
    {
        this.Products = new HashSet<ProductEntity>();
    }

    [Key]
    [Column("CategoryID")]
    public int CategoryId { get; set; }
    [Required]
    [StringLength(15)]
    public string CategoryName { get; set; }
    [Column(TypeName = "ntext")]
    public string Description { get; set; }
    [Column(TypeName = "image")]
    public byte[] Picture { get; set; }

    [InverseProperty(nameof(ProductEntity.Category))]
    public virtual ICollection<ProductEntity> Products { get; set; }
}
