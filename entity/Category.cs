using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public List<ProductCategory> ProductsCategories { get; set; }
    }
}
