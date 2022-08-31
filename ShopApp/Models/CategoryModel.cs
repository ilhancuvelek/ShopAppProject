using Entity;
using System.Collections.Generic;

namespace ShopApp.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        //category edit product-category birleştirme
        public List<Product> Products { get; set; }
    }
}
