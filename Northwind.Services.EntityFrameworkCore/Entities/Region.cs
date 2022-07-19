using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
#pragma warning disable SA1516 // Elements should be separated by blank line
#pragma warning disable CA2227 // Collection properties should be read only

namespace Northwind.Services.EntityFrameworkCore.Entities
{
    [Table("Region")]
    public partial class Region
    {
        public Region()
        {
            this.Territories = new HashSet<Territory>();
        }

        [Key]
        [Column("RegionID")]
        public int RegionId { get; set; }
        [Required]
        [StringLength(50)]
        public string RegionDescription { get; set; }

        [InverseProperty(nameof(Territory.Region))]
        public virtual ICollection<Territory> Territories { get; set; }
    }
}
