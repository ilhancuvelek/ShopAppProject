using Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IProductService
    {
        void Create(Product product);
        void Update(Product product);
        void Delete(Product product);
        Product GetById(int id);
        List<Product> GetAll();

        //Kategori inculude
        Product GetProductDetails(string productNameUrl);

        //Kategori adına göre ürün filtreleme
        List<Product> GetProductsByCategory(string name,int page,int pageSize);
        int GetCountByCategory(string category);
    }
}
