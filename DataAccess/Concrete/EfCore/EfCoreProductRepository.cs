using DataAccess.Abstract;
using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Concrete.EfCore
{
    public class EfCoreProductRepository : EfCoreGenericRepository<Product, ShopContext>, IProductRepository
    {
        //Kategori inculude
        public Product GetProductDetails(string productNameUrl)
        {
            using (var context=new ShopContext())
            {
                return context.Products.Where(p=>p.Url== productNameUrl).Include(p=>p.ProductsCategories).ThenInclude(c=>c.Category).FirstOrDefault();
            }
        }

        

        //Kategori adına göre ürün filtreleme
        public List<Product> GetProductsByCategory(string name)
        {
            using (var context = new ShopContext())
            {
                var products = context.Products.AsQueryable();
                if (!string.IsNullOrEmpty(name))
                {
                    products= products
                    .Include(p => p.ProductsCategories)
                    .ThenInclude(c => c.Category)
                    .Where(i => i.ProductsCategories.Any(a => a.Category.Url== name));
                }
                return products.ToList();
                
            }
        }
    }
}
