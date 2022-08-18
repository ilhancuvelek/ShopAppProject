using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public bool IsApproved { get; set; }

        public List<ProductCategory> ProductsCategories { get; set; }
    }
}
