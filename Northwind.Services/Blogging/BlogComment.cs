using System;

namespace Northwind.Services.Blogging
{
    public class BlogComment
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime Posted { get; set; }

        public int BlogArticleId { get; set; }

        public int AuthorId { get; set; }
    }
}
