using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class ProductCategory//çoka çok ilişki
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
