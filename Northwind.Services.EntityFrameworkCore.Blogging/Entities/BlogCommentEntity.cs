using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Services.EntityFrameworkCore.Blogging.Entities
{
    public class BlogCommentEntity
    {
        [Key]
        [Column("blog_comment_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("text", TypeName = "ntext")]
        public string Text { get; set; }

        [Required]
        [Column("posted", TypeName = "datetime")]
        public DateTime Posted { get; set; }

        [Required]
        [Column("blog_article_id")]
        public int BlogArticleId { get; set; }

        [Required]
        [Column("author_id")]
        public int AuthorId { get; set; }
    }
}
