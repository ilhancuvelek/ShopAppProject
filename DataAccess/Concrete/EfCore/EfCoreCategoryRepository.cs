using DataAccess.Abstract;
using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Concrete.EfCore
{
    public class EfCoreCategoryRepository : EfCoreGenericRepository<Category, ShopContext>, ICategoryRepository
    {
        public void DeleteFromCategory(int productId, int categoryId)
        {
            using (var context = new ShopContext())
            {
                var cmd = "delete from ProductCategory where ProductId=@p0 and CategoryId=@p1";
                context.Database.ExecuteSqlRaw(cmd, productId, categoryId);
            }
                
        }

        public Category GetByIdWithProducts(int id)
        {
            using (var context=new ShopContext())
            {
                return context.Categories.Where(c => c.CategoryId == id)
                    .Include(pc => pc.ProductsCategories).
                    ThenInclude(p => p.Product).
                    FirstOrDefault();
            }
        }
    }
}
