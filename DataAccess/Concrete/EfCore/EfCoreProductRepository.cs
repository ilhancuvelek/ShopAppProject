using DataAccess.Abstract;
using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DataAccess.Concrete.EfCore
{
    public class EfCoreProductRepository : EfCoreGenericRepository<Product, ShopContext>, IProductRepository
    {
        //(sayfalama) kategori seçildikten sonra sayfalama yapılmak istenirse diye kategoriye göre yaptık kategori yoksa normal
        public int GetCountByCategory(string category)
        {
            using (var context = new ShopContext())
            {
                var products = context.Products.Where(i=>i.IsApproved).AsQueryable();
                if (!string.IsNullOrEmpty(category))
                {
                    products = products
                    .Include(p => p.ProductsCategories)
                    .ThenInclude(c => c.Category)
                    .Where(i => i.ProductsCategories.Any(a => a.Category.Url == category));
                }
                return products.Count();

            }
        }
        //anasayfa ürünleri
        public List<Product> GetHomePageProducts()
        {
            using (var context=new ShopContext())
            {
                return context.Products.Where(i => i.IsApproved && i.IsHome).ToList();
            }
        }

        //Kategori inculude
        public Product GetProductDetails(string productNameUrl)
        {
            using (var context=new ShopContext())
            {
                return context.Products.Where(p=>p.Url== productNameUrl).Include(p=>p.ProductsCategories).ThenInclude(c=>c.Category).FirstOrDefault();
            }
        }

        

        //Kategori adına göre ürün filtreleme
        public List<Product> GetProductsByCategory(string name, int page, int pageSize)
        {
            using (var context = new ShopContext())
            {
                var products = context.Products.Where(i => i.IsApproved).AsQueryable();
                if (!string.IsNullOrEmpty(name))
                {
                    products= products
                    .Include(p => p.ProductsCategories)
                    .ThenInclude(c => c.Category)
                    .Where(i => i.ProductsCategories.Any(a => a.Category.Url== name));
                }
                return products.Skip((page-1)*pageSize).Take(pageSize).ToList();
                
            }
        }

        public List<Product> GetSearchResult(string searchString)
        {
            using (var context = new ShopContext())
            {
                var products = context.Products.Where(i => i.IsApproved && (i.Name.ToLower().Contains(searchString.ToLower()) || i.Description.ToLower().Contains(searchString.ToLower()))).AsQueryable();
                return products.ToList();
            }
           
        }
    }
}
