using System;

namespace Northwind.Services.Blogging
{
    public class BlogArticle
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime Posted { get; set; }

        public int AuthorId { get; set; }
    }
}
