using System.Collections.Generic;
using NorthwindMvcClient.Models;

namespace NorthwindMvcClient.ViewModels
{
    public class ProductCategoriesViewModel
    {
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
    }
}
