using System;
using Northwind.Services.Blogging;
using Northwind.Services.Employees;

namespace NorthwindApiApp.Models
{
    public class BlogArticleShortInfo
    {
        public BlogArticleShortInfo(BlogArticle blogArticle, Employee employee)
        {
            _ = blogArticle ?? throw new ArgumentNullException(nameof(blogArticle));
            _ = employee ?? throw new ArgumentNullException(nameof(employee));

            this.Id = blogArticle.Id;
            this.Title = blogArticle.Title;
            this.Posted = blogArticle.Posted;
            this.AuthorId = blogArticle.AuthorId;
            this.AuthorName = $"{employee.FirstName} {employee.LastName}, {employee.Title}";
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime Posted { get; set; }

        public int AuthorId { get; set; }

        public string AuthorName { get; set; }
    }
}
