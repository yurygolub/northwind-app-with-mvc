using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Services.EntityFrameworkCore.Blogging.Entities
{
    public class BlogArticleProductEntity
    {
        [Key]
        [Column("blog_article_product_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("blog_article_id")]
        public int BlogArticleId { get; set; }

        [Required]
        [Column("product_id")]
        public int ProductId { get; set; }
    }
}
