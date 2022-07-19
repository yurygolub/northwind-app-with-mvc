using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Services.EntityFrameworkCore.Blogging.Entities
{
    public class BlogArticleEntity
    {
        [Key]
        [Column("blog_article_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50)]
        [Column("title")]
        public string Title { get; set; }

        [Required]
        [Column("text", TypeName = "ntext")]
        public string Text { get; set; }

        [Required]
        [Column("posted", TypeName = "datetime")]
        public DateTime Posted { get; set; }

        [Required]
        [Column("author_id")]
        public int AuthorId { get; set; }
    }
}
