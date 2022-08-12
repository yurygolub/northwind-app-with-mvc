using System.Collections.Generic;
using NorthwindMvcClient.Models;

namespace NorthwindMvcClient.ViewModels
{
    public class ProductsViewModel
    {
        public IEnumerable<Product> Products { get; set; }
    }
}
